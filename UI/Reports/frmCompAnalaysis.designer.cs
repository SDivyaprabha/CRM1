namespace CRM
{
    partial class frmCompAnalaysis
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCompAnalaysis));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.radDock1 = new Telerik.WinControls.UI.Docking.RadDock();
            this.dwGeneral = new Telerik.WinControls.UI.Docking.DocumentWindow();
            this.vGridControl1 = new DevExpress.XtraVerticalGrid.VGridControl();
            this.documentContainer1 = new Telerik.WinControls.UI.Docking.DocumentContainer();
            this.documentTabStrip1 = new Telerik.WinControls.UI.Docking.DocumentTabStrip();
            this.dwAmenity = new Telerik.WinControls.UI.Docking.DocumentWindow();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.cboFlatType = new DevExpress.XtraBars.BarEditItem();
            this.cboFT = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.btnFTReport = new DevExpress.XtraBars.BarButtonItem();
            this.standaloneBarDockControl1 = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.bar4 = new DevExpress.XtraBars.Bar();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.cboCostCentre = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.btnPrint = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dwService = new Telerik.WinControls.UI.Docking.DocumentWindow();
            this.gridControl3 = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.dwReport = new Telerik.WinControls.UI.Docking.DocumentWindow();
            this.grdReport = new DevExpress.XtraGrid.GridControl();
            this.advBandViewReport = new DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radDock1)).BeginInit();
            this.radDock1.SuspendLayout();
            this.dwGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vGridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentContainer1)).BeginInit();
            this.documentContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentTabStrip1)).BeginInit();
            this.documentTabStrip1.SuspendLayout();
            this.dwAmenity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboFT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCostCentre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            this.dwService.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.dwReport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advBandViewReport)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.radDock1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 26);
            this.panelControl1.LookAndFeel.SkinName = "Blue";
            this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(785, 346);
            this.panelControl1.TabIndex = 0;
            // 
            // radDock1
            // 
            this.radDock1.ActiveWindow = this.dwGeneral;
            this.radDock1.CausesValidation = false;
            this.radDock1.Controls.Add(this.documentContainer1);
            this.radDock1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radDock1.DocumentManager.DocumentInsertOrder = Telerik.WinControls.UI.Docking.DockWindowInsertOrder.InFront;
            this.radDock1.IsCleanUpTarget = true;
            this.radDock1.Location = new System.Drawing.Point(0, 0);
            this.radDock1.MainDocumentContainer = this.documentContainer1;
            this.radDock1.Name = "radDock1";
            this.radDock1.Padding = new System.Windows.Forms.Padding(5);
            // 
            // 
            // 
            this.radDock1.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.radDock1.RootElement.Padding = new System.Windows.Forms.Padding(5);
            this.radDock1.Size = new System.Drawing.Size(785, 346);
            this.radDock1.SplitterWidth = 4;
            this.radDock1.TabIndex = 0;
            this.radDock1.TabStop = false;
            this.radDock1.Text = "radDock1";
            this.radDock1.ActiveWindowChanged += new Telerik.WinControls.UI.Docking.DockWindowEventHandler(this.radDock1_ActiveWindowChanged);
            // 
            // dwGeneral
            // 
            this.dwGeneral.CloseAction = Telerik.WinControls.UI.Docking.DockWindowCloseAction.Hide;
            this.dwGeneral.Controls.Add(this.vGridControl1);
            this.dwGeneral.DocumentButtons = Telerik.WinControls.UI.Docking.DocumentStripButtons.None;
            this.dwGeneral.Location = new System.Drawing.Point(6, 29);
            this.dwGeneral.Name = "dwGeneral";
            this.dwGeneral.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.TabbedDocument;
            this.dwGeneral.Size = new System.Drawing.Size(763, 301);
            this.dwGeneral.Text = "General";
            this.dwGeneral.ToolCaptionButtons = Telerik.WinControls.UI.Docking.ToolStripCaptionButtons.None;
            // 
            // vGridControl1
            // 
            this.vGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vGridControl1.Location = new System.Drawing.Point(0, 0);
            this.vGridControl1.LookAndFeel.SkinName = "Money Twins";
            this.vGridControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.vGridControl1.Name = "vGridControl1";
            this.vGridControl1.OptionsBehavior.AutoFocusNewRecord = true;
            this.vGridControl1.OptionsBehavior.CopyToClipboardWithRowHeaders = false;
            this.vGridControl1.OptionsBehavior.Editable = false;
            this.vGridControl1.OptionsBehavior.RecordsMouseWheel = true;
            this.vGridControl1.OptionsBehavior.ResizeHeaderPanel = false;
            this.vGridControl1.OptionsBehavior.ResizeRowHeaders = false;
            this.vGridControl1.OptionsBehavior.ResizeRowValues = false;
            this.vGridControl1.OptionsBehavior.SmartExpand = false;
            this.vGridControl1.OptionsBehavior.UseEnterAsTab = true;
            this.vGridControl1.OptionsView.AutoScaleBands = true;
            this.vGridControl1.OptionsView.FixRowHeaderPanelWidth = true;
            this.vGridControl1.Size = new System.Drawing.Size(763, 301);
            this.vGridControl1.TabIndex = 0;
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
            this.documentContainer1.Size = new System.Drawing.Size(775, 336);
            this.documentContainer1.SizeInfo.SizeMode = Telerik.WinControls.UI.Docking.SplitPanelSizeMode.Fill;
            this.documentContainer1.SplitterWidth = 4;
            this.documentContainer1.TabIndex = 0;
            this.documentContainer1.TabStop = false;
            // 
            // documentTabStrip1
            // 
            this.documentTabStrip1.CausesValidation = false;
            this.documentTabStrip1.Controls.Add(this.dwGeneral);
            this.documentTabStrip1.Controls.Add(this.dwAmenity);
            this.documentTabStrip1.Controls.Add(this.dwService);
            this.documentTabStrip1.Controls.Add(this.dwReport);
            this.documentTabStrip1.Location = new System.Drawing.Point(0, 0);
            this.documentTabStrip1.Name = "documentTabStrip1";
            // 
            // 
            // 
            this.documentTabStrip1.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.documentTabStrip1.SelectedIndex = 0;
            this.documentTabStrip1.Size = new System.Drawing.Size(775, 336);
            this.documentTabStrip1.TabIndex = 0;
            this.documentTabStrip1.TabStop = false;
            // 
            // dwAmenity
            // 
            this.dwAmenity.CloseAction = Telerik.WinControls.UI.Docking.DockWindowCloseAction.Hide;
            this.dwAmenity.Controls.Add(this.gridControl2);
            this.dwAmenity.DocumentButtons = Telerik.WinControls.UI.Docking.DocumentStripButtons.None;
            this.dwAmenity.Location = new System.Drawing.Point(6, 29);
            this.dwAmenity.Name = "dwAmenity";
            this.dwAmenity.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.TabbedDocument;
            this.dwAmenity.Size = new System.Drawing.Size(542, 285);
            this.dwAmenity.Text = "Common Amenties";
            this.dwAmenity.ToolCaptionButtons = Telerik.WinControls.UI.Docking.ToolStripCaptionButtons.None;
            // 
            // gridControl2
            // 
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.Location = new System.Drawing.Point(0, 0);
            this.gridControl2.LookAndFeel.SkinName = "Blue";
            this.gridControl2.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gridControl2.MainView = this.gridView1;
            this.gridControl2.MenuManager = this.barManager1;
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(542, 285);
            this.gridControl2.TabIndex = 0;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.ColumnPanelRowHeight = 30;
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.GridControl = this.gridControl2;
            this.gridView1.IndicatorWidth = 60;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowIncrementalSearch = true;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsNavigation.EnterMoveNextColumn = true;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView1_CustomDrawRowIndicator);
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.AllowQuickCustomization = false;
            this.barManager1.AllowShowToolbarsPopup = false;
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar4});
            this.barManager1.Controller = this.barAndDockingController1;
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockControls.Add(this.standaloneBarDockControl1);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barEditItem1,
            this.barStaticItem1,
            this.barButtonItem2,
            this.btnPrint,
            this.barStaticItem2,
            this.cboFlatType,
            this.btnFTReport});
            this.barManager1.MaxItemId = 9;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cboCostCentre,
            this.cboFT});
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 2";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.cboFlatType),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnFTReport, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.AllowRename = true;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.MultiLine = true;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.StandaloneBarDockControl = this.standaloneBarDockControl1;
            this.bar1.Text = "Custom 2";
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.barStaticItem2.Caption = "Flat Type";
            this.barStaticItem2.Id = 6;
            this.barStaticItem2.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.barStaticItem2.ItemAppearance.Normal.Options.UseFont = true;
            this.barStaticItem2.Name = "barStaticItem2";
            this.barStaticItem2.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // cboFlatType
            // 
            this.cboFlatType.Edit = this.cboFT;
            this.cboFlatType.Id = 7;
            this.cboFlatType.Name = "cboFlatType";
            this.cboFlatType.Width = 170;
            // 
            // cboFT
            // 
            this.cboFT.AutoHeight = false;
            this.cboFT.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboFT.LookAndFeel.SkinName = "Money Twins";
            this.cboFT.LookAndFeel.UseDefaultLookAndFeel = false;
            this.cboFT.Name = "cboFT";
            this.cboFT.NullText = "None";
            this.cboFT.EditValueChanged += new System.EventHandler(this.cboFT_EditValueChanged);
            // 
            // btnFTReport
            // 
            this.btnFTReport.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnFTReport.Caption = "Report";
            this.btnFTReport.Glyph = global::CRM.Properties.Resources.report;
            this.btnFTReport.Id = 8;
            this.btnFTReport.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnFTReport.ItemAppearance.Normal.Options.UseFont = true;
            this.btnFTReport.Name = "btnFTReport";
            this.btnFTReport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnFTReport_ItemClick);
            // 
            // standaloneBarDockControl1
            // 
            this.standaloneBarDockControl1.CausesValidation = false;
            this.standaloneBarDockControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.standaloneBarDockControl1.Location = new System.Drawing.Point(0, 0);
            this.standaloneBarDockControl1.Name = "standaloneBarDockControl1";
            this.standaloneBarDockControl1.Size = new System.Drawing.Size(763, 26);
            this.standaloneBarDockControl1.Text = "standaloneBarDockControl1";
            // 
            // bar4
            // 
            this.bar4.BarAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bar4.BarAppearance.Normal.Options.UseFont = true;
            this.bar4.BarName = "Custom 4";
            this.bar4.DockCol = 0;
            this.bar4.DockRow = 0;
            this.bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar4.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barEditItem1),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem2, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnPrint, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1)});
            this.bar4.OptionsBar.AllowQuickCustomization = false;
            this.bar4.OptionsBar.DrawDragBorder = false;
            this.bar4.OptionsBar.MultiLine = true;
            this.bar4.OptionsBar.UseWholeRow = true;
            this.bar4.Text = "Custom 4";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.barStaticItem1.Caption = "Project";
            this.barStaticItem1.Id = 2;
            this.barStaticItem1.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.barStaticItem1.ItemAppearance.Normal.Options.UseFont = true;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barEditItem1
            // 
            this.barEditItem1.Caption = "Cost Centre";
            this.barEditItem1.Edit = this.cboCostCentre;
            this.barEditItem1.Id = 1;
            this.barEditItem1.Name = "barEditItem1";
            this.barEditItem1.Width = 177;
            this.barEditItem1.EditValueChanged += new System.EventHandler(this.barEditItem1_EditValueChanged);
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
            // barButtonItem2
            // 
            this.barButtonItem2.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barButtonItem2.Caption = "Competitors";
            this.barButtonItem2.Glyph = global::CRM.Properties.Resources.application_add;
            this.barButtonItem2.Id = 3;
            this.barButtonItem2.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.barButtonItem2.ItemAppearance.Normal.Options.UseFont = true;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // btnPrint
            // 
            this.btnPrint.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnPrint.Caption = "Print";
            this.btnPrint.Glyph = global::CRM.Properties.Resources.printer1;
            this.btnPrint.Id = 4;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3_ItemClick);
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
            this.barAndDockingController1.LookAndFeel.SkinName = "Blue";
            this.barAndDockingController1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.barAndDockingController1.PropertiesBar.AllowLinkLighting = false;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(785, 26);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 372);
            this.barDockControlBottom.Size = new System.Drawing.Size(785, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 26);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 346);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(785, 26);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 346);
            // 
            // dwService
            // 
            this.dwService.CloseAction = Telerik.WinControls.UI.Docking.DockWindowCloseAction.Hide;
            this.dwService.Controls.Add(this.gridControl3);
            this.dwService.DocumentButtons = Telerik.WinControls.UI.Docking.DocumentStripButtons.None;
            this.dwService.Location = new System.Drawing.Point(6, 29);
            this.dwService.Name = "dwService";
            this.dwService.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.TabbedDocument;
            this.dwService.Size = new System.Drawing.Size(763, 301);
            this.dwService.Text = "Near by Services";
            this.dwService.ToolCaptionButtons = Telerik.WinControls.UI.Docking.ToolStripCaptionButtons.None;
            // 
            // gridControl3
            // 
            this.gridControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl3.Location = new System.Drawing.Point(0, 0);
            this.gridControl3.LookAndFeel.SkinName = "Blue";
            this.gridControl3.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gridControl3.MainView = this.gridView2;
            this.gridControl3.MenuManager = this.barManager1;
            this.gridControl3.Name = "gridControl3";
            this.gridControl3.Size = new System.Drawing.Size(763, 301);
            this.gridControl3.TabIndex = 1;
            this.gridControl3.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.ColumnPanelRowHeight = 30;
            this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView2.GridControl = this.gridControl3;
            this.gridView2.IndicatorWidth = 60;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.AllowIncrementalSearch = true;
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsNavigation.EnterMoveNextColumn = true;
            this.gridView2.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gridView2.OptionsView.ShowAutoFilterRow = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView2_CustomDrawRowIndicator);
            // 
            // dwReport
            // 
            this.dwReport.CloseAction = Telerik.WinControls.UI.Docking.DockWindowCloseAction.Hide;
            this.dwReport.Controls.Add(this.grdReport);
            this.dwReport.Controls.Add(this.standaloneBarDockControl1);
            this.dwReport.DocumentButtons = Telerik.WinControls.UI.Docking.DocumentStripButtons.None;
            this.dwReport.Location = new System.Drawing.Point(6, 29);
            this.dwReport.Name = "dwReport";
            this.dwReport.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.TabbedDocument;
            this.dwReport.Size = new System.Drawing.Size(763, 301);
            this.dwReport.Text = "Report";
            // 
            // grdReport
            // 
            this.grdReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdReport.Location = new System.Drawing.Point(0, 26);
            this.grdReport.LookAndFeel.SkinName = "Blue";
            this.grdReport.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdReport.MainView = this.advBandViewReport;
            this.grdReport.MenuManager = this.barManager1;
            this.grdReport.Name = "grdReport";
            this.grdReport.Size = new System.Drawing.Size(763, 275);
            this.grdReport.TabIndex = 13;
            this.grdReport.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.advBandViewReport});
            // 
            // advBandViewReport
            // 
            this.advBandViewReport.BandPanelRowHeight = 30;
            this.advBandViewReport.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1});
            this.advBandViewReport.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.advBandViewReport.GridControl = this.grdReport;
            this.advBandViewReport.IndicatorWidth = 60;
            this.advBandViewReport.Name = "advBandViewReport";
            this.advBandViewReport.OptionsBehavior.AllowIncrementalSearch = true;
            this.advBandViewReport.OptionsBehavior.Editable = false;
            this.advBandViewReport.OptionsNavigation.EnterMoveNextColumn = true;
            this.advBandViewReport.OptionsPrint.AutoWidth = false;
            this.advBandViewReport.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.advBandViewReport.OptionsView.ShowAutoFilterRow = true;
            this.advBandViewReport.OptionsView.ShowGroupPanel = false;
            this.advBandViewReport.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.advBandViewReport_CustomDrawRowIndicator);
            // 
            // gridBand1
            // 
            this.gridBand1.Caption = "gridBand1";
            this.gridBand1.Name = "gridBand1";
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Blue";
            // 
            // frmCompAnalaysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 372);
            this.ControlBox = false;
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "frmCompAnalaysis";
            this.Text = "Competitor Analysis";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCompAnalaysis_FormClosed);
            this.Load += new System.EventHandler(this.frmCompAnalaysis_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radDock1)).EndInit();
            this.radDock1.ResumeLayout(false);
            this.dwGeneral.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vGridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentContainer1)).EndInit();
            this.documentContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.documentTabStrip1)).EndInit();
            this.documentTabStrip1.ResumeLayout(false);
            this.dwAmenity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboFT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCostCentre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            this.dwService.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.dwReport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advBandViewReport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboCostCentre;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private Telerik.WinControls.UI.Docking.RadDock radDock1;
        private Telerik.WinControls.UI.Docking.DocumentContainer documentContainer1;
        private Telerik.WinControls.UI.Docking.DocumentWindow dwService;
        private Telerik.WinControls.UI.Docking.DocumentTabStrip documentTabStrip1;
        private Telerik.WinControls.UI.Docking.DocumentWindow dwGeneral;
        private Telerik.WinControls.UI.Docking.DocumentWindow dwAmenity;
        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.GridControl gridControl3;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem btnPrint;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private Telerik.WinControls.UI.Docking.DocumentWindow dwReport;
        private DevExpress.XtraGrid.GridControl grdReport;
        private DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView advBandViewReport;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.BarEditItem cboFlatType;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboFT;
        private DevExpress.XtraBars.StandaloneBarDockControl standaloneBarDockControl1;
        private DevExpress.XtraBars.BarButtonItem btnFTReport;
        private DevExpress.XtraBars.Bar bar4;
        private DevExpress.XtraVerticalGrid.VGridControl vGridControl1;
    }
}