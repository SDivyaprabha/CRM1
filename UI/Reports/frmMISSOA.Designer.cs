namespace CRM
{
    partial class frmMISSOA
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMISSOA));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.dEAsOnDate = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.btnZoom = new DevExpress.XtraBars.BarButtonItem();
            this.btnPrint = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.btnExit = new DevExpress.XtraBars.BarButtonItem();
            this.repositoryItemRadioGroup1 = new DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.grdInt = new DevExpress.XtraGrid.GridControl();
            this.grdViewInt = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemLookUpEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grdQual = new DevExpress.XtraGrid.GridControl();
            this.grdViewQual = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grdAsOn = new DevExpress.XtraGrid.GridControl();
            this.grdViewAsOn = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.printingSystem1 = new DevExpress.XtraPrinting.PrintingSystem(this.components);
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRadioGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdInt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewInt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdQual)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewQual)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdAsOn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewAsOn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printingSystem1)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.Controller = this.barAndDockingController1;
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnPrint,
            this.btnExit,
            this.barStaticItem1,
            this.dEAsOnDate,
            this.barButtonItem1,
            this.btnZoom});
            this.barManager1.MaxItemId = 7;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemRadioGroup1,
            this.repositoryItemDateEdit1});
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.dEAsOnDate),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnZoom, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnPrint, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.barStaticItem1.Caption = "As On Date";
            this.barStaticItem1.Id = 3;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // dEAsOnDate
            // 
            this.dEAsOnDate.Edit = this.repositoryItemDateEdit1;
            this.dEAsOnDate.EditValue = new System.DateTime(2012, 8, 14, 13, 28, 18, 290);
            this.dEAsOnDate.Id = 4;
            this.dEAsOnDate.Name = "dEAsOnDate";
            this.dEAsOnDate.Width = 95;
            this.dEAsOnDate.EditValueChanged += new System.EventHandler(this.dEAsOnDate_EditValueChanged);
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            this.repositoryItemDateEdit1.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            // 
            // btnZoom
            // 
            this.btnZoom.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnZoom.Caption = "Zoom";
            this.btnZoom.Glyph = ((System.Drawing.Image)(resources.GetObject("btnZoom.Glyph")));
            this.btnZoom.Id = 6;
            this.btnZoom.Name = "btnZoom";
            this.btnZoom.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnZoom_ItemClick);
            // 
            // btnPrint
            // 
            this.btnPrint.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnPrint.Caption = "Print";
            this.btnPrint.Glyph = global::CRM.Properties.Resources.printer1;
            this.btnPrint.Id = 0;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPrint_ItemClick);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Exit";
            this.barButtonItem1.Glyph = global::CRM.Properties.Resources.exit1;
            this.barButtonItem1.Id = 5;
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
            this.barDockControlTop.Size = new System.Drawing.Size(956, 26);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 375);
            this.barDockControlBottom.Size = new System.Drawing.Size(956, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 26);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 349);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(956, 26);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 349);
            // 
            // btnExit
            // 
            this.btnExit.Caption = "Exit";
            this.btnExit.Glyph = global::CRM.Properties.Resources.exit1;
            this.btnExit.Id = 1;
            this.btnExit.Name = "btnExit";
            // 
            // repositoryItemRadioGroup1
            // 
            this.repositoryItemRadioGroup1.Name = "repositoryItemRadioGroup1";
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Blue";
            // 
            // grdInt
            // 
            this.grdInt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdInt.Location = new System.Drawing.Point(0, 0);
            this.grdInt.LookAndFeel.SkinName = "Blue";
            this.grdInt.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdInt.MainView = this.grdViewInt;
            this.grdInt.Name = "grdInt";
            this.grdInt.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1,
            this.repositoryItemLookUpEdit2});
            this.grdInt.Size = new System.Drawing.Size(956, 149);
            this.grdInt.TabIndex = 109;
            this.grdInt.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdViewInt});
            // 
            // grdViewInt
            // 
            this.grdViewInt.ColumnPanelRowHeight = 30;
            this.grdViewInt.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.grdViewInt.GridControl = this.grdInt;
            this.grdViewInt.Name = "grdViewInt";
            this.grdViewInt.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.grdViewInt.OptionsBehavior.AllowIncrementalSearch = true;
            this.grdViewInt.OptionsBehavior.Editable = false;
            this.grdViewInt.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click;
            this.grdViewInt.OptionsBehavior.ReadOnly = true;
            this.grdViewInt.OptionsCustomization.AllowSort = false;
            this.grdViewInt.OptionsNavigation.AutoFocusNewRow = true;
            this.grdViewInt.OptionsNavigation.EnterMoveNextColumn = true;
            this.grdViewInt.OptionsPrint.AutoWidth = false;
            this.grdViewInt.OptionsView.AutoCalcPreviewLineCount = true;
            this.grdViewInt.OptionsView.ShowFooter = true;
            this.grdViewInt.OptionsView.ShowGroupPanel = false;
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            // 
            // repositoryItemLookUpEdit2
            // 
            this.repositoryItemLookUpEdit2.AutoHeight = false;
            this.repositoryItemLookUpEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit2.Name = "repositoryItemLookUpEdit2";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.radioGroup1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 26);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(956, 29);
            this.panelControl1.TabIndex = 114;
            this.panelControl1.Visible = false;
            // 
            // radioGroup1
            // 
            this.radioGroup1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radioGroup1.Location = new System.Drawing.Point(2, 2);
            this.radioGroup1.MenuManager = this.barManager1;
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Payment Schedule"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Progress Bill")});
            this.radioGroup1.Properties.LookAndFeel.SkinName = "Glass Oceans";
            this.radioGroup1.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.radioGroup1.Size = new System.Drawing.Size(952, 27);
            this.radioGroup1.TabIndex = 0;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // panelControl2
            // 
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Controls.Add(this.splitterControl1);
            this.panelControl2.Controls.Add(this.grdInt);
            this.panelControl2.Controls.Add(this.panelControl3);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 55);
            this.panelControl2.LookAndFeel.SkinName = "Blue";
            this.panelControl2.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(956, 320);
            this.panelControl2.TabIndex = 0;
            // 
            // panelControl3
            // 
            this.panelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl3.Controls.Add(this.tableLayoutPanel1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl3.Location = new System.Drawing.Point(0, 149);
            this.panelControl3.LookAndFeel.SkinName = "Blue";
            this.panelControl3.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(956, 171);
            this.panelControl3.TabIndex = 113;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.grdQual, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.grdAsOn, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(956, 171);
            this.tableLayoutPanel1.TabIndex = 112;
            // 
            // grdQual
            // 
            this.grdQual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdQual.Location = new System.Drawing.Point(3, 3);
            this.grdQual.LookAndFeel.SkinName = "Blue";
            this.grdQual.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdQual.MainView = this.grdViewQual;
            this.grdQual.Name = "grdQual";
            this.grdQual.Size = new System.Drawing.Size(472, 165);
            this.grdQual.TabIndex = 111;
            this.grdQual.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdViewQual,
            this.gridView4});
            // 
            // grdViewQual
            // 
            this.grdViewQual.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdViewQual.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Navy;
            this.grdViewQual.Appearance.ViewCaption.Options.UseFont = true;
            this.grdViewQual.Appearance.ViewCaption.Options.UseForeColor = true;
            this.grdViewQual.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.grdViewQual.GridControl = this.grdQual;
            this.grdViewQual.Name = "grdViewQual";
            this.grdViewQual.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.grdViewQual.OptionsBehavior.AllowIncrementalSearch = true;
            this.grdViewQual.OptionsBehavior.Editable = false;
            this.grdViewQual.OptionsBehavior.ReadOnly = true;
            this.grdViewQual.OptionsCustomization.AllowColumnMoving = false;
            this.grdViewQual.OptionsCustomization.AllowSort = false;
            this.grdViewQual.OptionsMenu.EnableColumnMenu = false;
            this.grdViewQual.OptionsMenu.EnableFooterMenu = false;
            this.grdViewQual.OptionsMenu.EnableGroupPanelMenu = false;
            this.grdViewQual.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.grdViewQual.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.grdViewQual.OptionsNavigation.AutoFocusNewRow = true;
            this.grdViewQual.OptionsNavigation.EnterMoveNextColumn = true;
            this.grdViewQual.OptionsView.ShowFooter = true;
            this.grdViewQual.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.grdViewQual.OptionsView.ShowGroupPanel = false;
            this.grdViewQual.OptionsView.ShowViewCaption = true;
            this.grdViewQual.ViewCaption = "Total Receivable";
            // 
            // gridView4
            // 
            this.gridView4.GridControl = this.grdQual;
            this.gridView4.Name = "gridView4";
            // 
            // grdAsOn
            // 
            this.grdAsOn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdAsOn.Location = new System.Drawing.Point(481, 3);
            this.grdAsOn.LookAndFeel.SkinName = "Blue";
            this.grdAsOn.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdAsOn.MainView = this.grdViewAsOn;
            this.grdAsOn.Name = "grdAsOn";
            this.grdAsOn.Size = new System.Drawing.Size(472, 165);
            this.grdAsOn.TabIndex = 111;
            this.grdAsOn.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdViewAsOn,
            this.gridView2});
            // 
            // grdViewAsOn
            // 
            this.grdViewAsOn.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdViewAsOn.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Navy;
            this.grdViewAsOn.Appearance.ViewCaption.Options.UseFont = true;
            this.grdViewAsOn.Appearance.ViewCaption.Options.UseForeColor = true;
            this.grdViewAsOn.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.grdViewAsOn.GridControl = this.grdAsOn;
            this.grdViewAsOn.Name = "grdViewAsOn";
            this.grdViewAsOn.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.grdViewAsOn.OptionsBehavior.AllowIncrementalSearch = true;
            this.grdViewAsOn.OptionsBehavior.Editable = false;
            this.grdViewAsOn.OptionsBehavior.ReadOnly = true;
            this.grdViewAsOn.OptionsCustomization.AllowColumnMoving = false;
            this.grdViewAsOn.OptionsCustomization.AllowSort = false;
            this.grdViewAsOn.OptionsMenu.EnableColumnMenu = false;
            this.grdViewAsOn.OptionsMenu.EnableFooterMenu = false;
            this.grdViewAsOn.OptionsMenu.EnableGroupPanelMenu = false;
            this.grdViewAsOn.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.grdViewAsOn.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.grdViewAsOn.OptionsNavigation.AutoFocusNewRow = true;
            this.grdViewAsOn.OptionsNavigation.EnterMoveNextColumn = true;
            this.grdViewAsOn.OptionsView.ShowFooter = true;
            this.grdViewAsOn.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.grdViewAsOn.OptionsView.ShowGroupPanel = false;
            this.grdViewAsOn.OptionsView.ShowViewCaption = true;
            this.grdViewAsOn.ViewCaption = "Receivable As On";
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.grdAsOn;
            this.gridView2.Name = "gridView2";
            // 
            // splitterControl1
            // 
            this.splitterControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterControl1.Location = new System.Drawing.Point(0, 143);
            this.splitterControl1.LookAndFeel.SkinName = "Money Twins";
            this.splitterControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(956, 6);
            this.splitterControl1.TabIndex = 114;
            this.splitterControl1.TabStop = false;
            // 
            // frmMISSOA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 375);
            this.ControlBox = false;
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "frmMISSOA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tax Calculation";
            this.Load += new System.EventHandler(this.frmSOA_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRadioGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdInt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewInt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdQual)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewQual)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdAsOn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewAsOn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printingSystem1)).EndInit();
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
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraGrid.GridControl grdInt;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit2;
        private DevExpress.XtraBars.BarButtonItem btnPrint;
        private DevExpress.XtraBars.BarButtonItem btnExit;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup repositoryItemRadioGroup1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarEditItem dEAsOnDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraPrinting.PrintingSystem printingSystem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraGrid.GridControl grdAsOn;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraGrid.GridControl grdQual;
        private DevExpress.XtraGrid.Views.Grid.GridView grdViewQual;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView4;
        private DevExpress.XtraGrid.Views.Grid.GridView grdViewAsOn;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraBars.BarButtonItem btnZoom;
        private DevExpress.XtraGrid.Views.Grid.GridView grdViewInt;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
    }
}