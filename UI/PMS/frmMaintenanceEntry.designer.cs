namespace CRM
{
    partial class frmMaintenanceEntry
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMaintenanceEntry));
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.btnSave = new DevExpress.XtraBars.BarButtonItem();
            this.btnCancel = new DevExpress.XtraBars.BarButtonItem();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.btnExit = new DevExpress.XtraBars.BarButtonItem();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.deEndDate = new DevExpress.XtraEditors.DateEdit();
            this.deStartDate = new DevExpress.XtraEditors.DateEdit();
            this.deRegDate = new DevExpress.XtraEditors.DateEdit();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.GRDShedule = new DevExpress.XtraGrid.GridControl();
            this.GViewshedule = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.txtRemarks = new DevExpress.XtraEditors.MemoEdit();
            this.label16 = new System.Windows.Forms.Label();
            this.txtGrace = new DevExpress.XtraEditors.TextEdit();
            this.label15 = new System.Windows.Forms.Label();
            this.comboIntRate = new DevExpress.XtraEditors.ComboBoxEdit();
            this.comboPerRate = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label13 = new System.Windows.Forms.Label();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.label9 = new System.Windows.Forms.Label();
            this.txtIntRate = new DevExpress.XtraEditors.TextEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.txtRefNo = new DevExpress.XtraEditors.TextEdit();
            this.label12 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboFlatNo = new DevExpress.XtraEditors.LookUpEdit();
            this.cboProj = new DevExpress.XtraEditors.LookUpEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deEndDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStartDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStartDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deRegDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deRegDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GRDShedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GViewshedule)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemarks.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGrace.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboIntRate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboPerRate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIntRate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRefNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboFlatNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProj.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.AllowQuickCustomization = false;
            this.barManager1.AllowShowToolbarsPopup = false;
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2,
            this.bar1});
            this.barManager1.Controller = this.barAndDockingController1;
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnExit,
            this.btnSave,
            this.btnCancel,
            this.barButtonItem1});
            this.barManager1.MaxItemId = 5;
            this.barManager1.StatusBar = this.bar2;
            // 
            // bar2
            // 
            this.bar2.BarName = "Custom 3";
            this.bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar2.FloatSize = new System.Drawing.Size(500, 30);
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnSave, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnCancel, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Custom 3";
            // 
            // btnSave
            // 
            this.btnSave.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnSave.Caption = "OK";
            this.btnSave.Glyph = ((System.Drawing.Image)(resources.GetObject("btnSave.Glyph")));
            this.btnSave.Id = 1;
            this.btnSave.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ItemAppearance.Normal.Options.UseFont = true;
            this.btnSave.Name = "btnSave";
            toolTipTitleItem2.Text = "Save";
            superToolTip2.Items.Add(toolTipTitleItem2);
            this.btnSave.SuperTip = superToolTip2;
            this.btnSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSave_ItemClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnCancel.Caption = "Cancel";
            this.btnCancel.Glyph = ((System.Drawing.Image)(resources.GetObject("btnCancel.Glyph")));
            this.btnCancel.Id = 3;
            this.btnCancel.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ItemAppearance.Normal.Options.UseFont = true;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCancel_ItemClick);
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 2";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.AllowRename = true;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Custom 2";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.Glyph")));
            this.barButtonItem1.Id = 4;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // barAndDockingController1
            // 
            this.barAndDockingController1.LookAndFeel.SkinName = "Money Twins";
            this.barAndDockingController1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.barAndDockingController1.PropertiesBar.AllowLinkLighting = false;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(917, 26);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 480);
            this.barDockControlBottom.Size = new System.Drawing.Size(917, 26);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 26);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 454);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(917, 26);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 454);
            // 
            // btnExit
            // 
            this.btnExit.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnExit.Caption = "Exit";
            this.btnExit.Glyph = global::CRM.Properties.Resources.exit1;
            this.btnExit.Id = 0;
            this.btnExit.Name = "btnExit";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Money Twins";
            // 
            // radPanel1
            // 
            this.radPanel1.CausesValidation = false;
            this.radPanel1.Controls.Add(this.deEndDate);
            this.radPanel1.Controls.Add(this.deStartDate);
            this.radPanel1.Controls.Add(this.deRegDate);
            this.radPanel1.Controls.Add(this.xtraTabControl1);
            this.radPanel1.Controls.Add(this.label16);
            this.radPanel1.Controls.Add(this.txtGrace);
            this.radPanel1.Controls.Add(this.label15);
            this.radPanel1.Controls.Add(this.comboIntRate);
            this.radPanel1.Controls.Add(this.comboPerRate);
            this.radPanel1.Controls.Add(this.label13);
            this.radPanel1.Controls.Add(this.labelControl1);
            this.radPanel1.Controls.Add(this.label9);
            this.radPanel1.Controls.Add(this.txtIntRate);
            this.radPanel1.Controls.Add(this.label7);
            this.radPanel1.Controls.Add(this.txtRefNo);
            this.radPanel1.Controls.Add(this.label12);
            this.radPanel1.Controls.Add(this.label4);
            this.radPanel1.Controls.Add(this.cboFlatNo);
            this.radPanel1.Controls.Add(this.cboProj);
            this.radPanel1.Controls.Add(this.label8);
            this.radPanel1.Controls.Add(this.label3);
            this.radPanel1.Controls.Add(this.label2);
            this.radPanel1.Controls.Add(this.label1);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(0, 26);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(917, 454);
            this.radPanel1.TabIndex = 1;
            // 
            // deEndDate
            // 
            this.deEndDate.EditValue = null;
            this.deEndDate.Enabled = false;
            this.deEndDate.Location = new System.Drawing.Point(149, 200);
            this.deEndDate.MenuManager = this.barManager1;
            this.deEndDate.Name = "deEndDate";
            this.deEndDate.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.deEndDate.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.deEndDate.Properties.Appearance.Options.UseBackColor = true;
            this.deEndDate.Properties.Appearance.Options.UseForeColor = true;
            this.deEndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deEndDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deEndDate.Size = new System.Drawing.Size(207, 20);
            this.deEndDate.TabIndex = 95;
            // 
            // deStartDate
            // 
            this.deStartDate.EditValue = null;
            this.deStartDate.Location = new System.Drawing.Point(150, 123);
            this.deStartDate.MenuManager = this.barManager1;
            this.deStartDate.Name = "deStartDate";
            this.deStartDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deStartDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deStartDate.Size = new System.Drawing.Size(206, 20);
            this.deStartDate.TabIndex = 94;
            this.deStartDate.Validating += new System.ComponentModel.CancelEventHandler(this.deStartDate_Validating);
            // 
            // deRegDate
            // 
            this.deRegDate.EditValue = null;
            this.deRegDate.Location = new System.Drawing.Point(151, 20);
            this.deRegDate.MenuManager = this.barManager1;
            this.deRegDate.Name = "deRegDate";
            this.deRegDate.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.deRegDate.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.deRegDate.Properties.Appearance.Options.UseBackColor = true;
            this.deRegDate.Properties.Appearance.Options.UseForeColor = true;
            this.deRegDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deRegDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deRegDate.Size = new System.Drawing.Size(205, 20);
            this.deRegDate.TabIndex = 2;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.AppearancePage.HeaderActive.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.xtraTabControl1.AppearancePage.HeaderActive.Options.UseFont = true;
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 271);
            this.xtraTabControl1.LookAndFeel.SkinName = "Blue";
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(917, 183);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.GRDShedule);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(915, 160);
            this.xtraTabPage1.Text = "Schedule";
            // 
            // GRDShedule
            // 
            this.GRDShedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GRDShedule.Location = new System.Drawing.Point(0, 0);
            this.GRDShedule.LookAndFeel.SkinName = "Money Twins";
            this.GRDShedule.LookAndFeel.UseDefaultLookAndFeel = false;
            this.GRDShedule.MainView = this.GViewshedule;
            this.GRDShedule.MenuManager = this.barManager1;
            this.GRDShedule.Name = "GRDShedule";
            this.GRDShedule.Size = new System.Drawing.Size(915, 160);
            this.GRDShedule.TabIndex = 0;
            this.GRDShedule.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GViewshedule});
            // 
            // GViewshedule
            // 
            this.GViewshedule.GridControl = this.GRDShedule;
            this.GViewshedule.IndicatorWidth = 60;
            this.GViewshedule.Name = "GViewshedule";
            this.GViewshedule.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.GViewshedule.OptionsBehavior.FocusLeaveOnTab = true;
            this.GViewshedule.OptionsNavigation.AutoFocusNewRow = true;
            this.GViewshedule.OptionsPrint.AutoWidth = false;
            this.GViewshedule.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.GViewshedule.OptionsView.AnimationType = DevExpress.XtraGrid.Views.Base.GridAnimationType.AnimateFocusedItem;
            this.GViewshedule.OptionsView.ShowFooter = true;
            this.GViewshedule.OptionsView.ShowGroupedColumns = true;
            this.GViewshedule.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.GViewshedule.OptionsView.ShowGroupPanel = false;
            this.GViewshedule.PaintStyleName = "Skin";
            this.GViewshedule.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.GViewshedule_CustomDrawRowIndicator);
            this.GViewshedule.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.GViewshedule_CellValueChanged);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.txtRemarks);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(915, 160);
            this.xtraTabPage2.Text = "Terms";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemarks.EnterMoveNextControl = true;
            this.txtRemarks.Location = new System.Drawing.Point(0, 0);
            this.txtRemarks.MenuManager = this.barManager1;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Properties.LookAndFeel.SkinName = "Money Twins";
            this.txtRemarks.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtRemarks.Size = new System.Drawing.Size(912, 157);
            this.txtRemarks.TabIndex = 21;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(605, 239);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(37, 13);
            this.label16.TabIndex = 84;
            this.label16.Text = "(Days)";
            // 
            // txtGrace
            // 
            this.txtGrace.EditValue = 0;
            this.txtGrace.EnterMoveNextControl = true;
            this.txtGrace.Location = new System.Drawing.Point(519, 235);
            this.txtGrace.MenuManager = this.barManager1;
            this.txtGrace.Name = "txtGrace";
            this.txtGrace.Size = new System.Drawing.Size(80, 20);
            this.txtGrace.TabIndex = 21;
            this.txtGrace.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGrace_KeyPress);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(443, 238);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(70, 15);
            this.label15.TabIndex = 83;
            this.label15.Text = "Grace Priod";
            // 
            // comboIntRate
            // 
            this.comboIntRate.EditValue = "--Select--";
            this.comboIntRate.EnterMoveNextControl = true;
            this.comboIntRate.Location = new System.Drawing.Point(303, 235);
            this.comboIntRate.MenuManager = this.barManager1;
            this.comboIntRate.Name = "comboIntRate";
            this.comboIntRate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboIntRate.Properties.Items.AddRange(new object[] {
            "--Select--",
            "Day",
            "Month",
            "Year"});
            this.comboIntRate.Properties.LookAndFeel.SkinName = "Money Twins";
            this.comboIntRate.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.comboIntRate.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboIntRate.Size = new System.Drawing.Size(80, 20);
            this.comboIntRate.TabIndex = 20;
            this.comboIntRate.SelectedIndexChanged += new System.EventHandler(this.comboIntRate_SelectedIndexChanged);
            // 
            // comboPerRate
            // 
            this.comboPerRate.EditValue = "--Select--";
            this.comboPerRate.EnterMoveNextControl = true;
            this.comboPerRate.Location = new System.Drawing.Point(149, 160);
            this.comboPerRate.MenuManager = this.barManager1;
            this.comboPerRate.Name = "comboPerRate";
            this.comboPerRate.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.comboPerRate.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.comboPerRate.Properties.Appearance.Options.UseBackColor = true;
            this.comboPerRate.Properties.Appearance.Options.UseForeColor = true;
            this.comboPerRate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboPerRate.Properties.Items.AddRange(new object[] {
            "--Select--",
            "Monthly",
            "Quaterly",
            "Half yearly",
            "Yearly"});
            this.comboPerRate.Properties.LookAndFeel.SkinName = "Money Twins";
            this.comboPerRate.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.comboPerRate.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboPerRate.Size = new System.Drawing.Size(207, 20);
            this.comboPerRate.TabIndex = 13;
            this.comboPerRate.SelectedIndexChanged += new System.EventHandler(this.comboPerRate_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(271, 238);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(26, 15);
            this.label13.TabIndex = 82;
            this.label13.Text = "Per";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Times New Roman", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(40, 162);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(37, 16);
            this.labelControl1.TabIndex = 74;
            this.labelControl1.Text = "Period";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Symbol", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.label9.Location = new System.Drawing.Point(245, 237);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 16);
            this.label9.TabIndex = 81;
            this.label9.Text = "%";
            // 
            // txtIntRate
            // 
            this.txtIntRate.EditValue = "0.000";
            this.txtIntRate.EnterMoveNextControl = true;
            this.txtIntRate.Location = new System.Drawing.Point(149, 235);
            this.txtIntRate.MenuManager = this.barManager1;
            this.txtIntRate.Name = "txtIntRate";
            this.txtIntRate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtIntRate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtIntRate.Properties.DisplayFormat.FormatString = "#.00";
            this.txtIntRate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtIntRate.Properties.LookAndFeel.SkinName = "Money Twins";
            this.txtIntRate.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtIntRate.Size = new System.Drawing.Size(90, 20);
            this.txtIntRate.TabIndex = 19;
            this.txtIntRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textEdit3_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(37, 237);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 15);
            this.label7.TabIndex = 79;
            this.label7.Text = "Interest Rate";
            // 
            // txtRefNo
            // 
            this.txtRefNo.EnterMoveNextControl = true;
            this.txtRefNo.Location = new System.Drawing.Point(518, 20);
            this.txtRefNo.MenuManager = this.barManager1;
            this.txtRefNo.Name = "txtRefNo";
            this.txtRefNo.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtRefNo.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtRefNo.Properties.Appearance.Options.UseBackColor = true;
            this.txtRefNo.Properties.Appearance.Options.UseForeColor = true;
            this.txtRefNo.Size = new System.Drawing.Size(175, 20);
            this.txtRefNo.TabIndex = 3;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(438, 23);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(46, 15);
            this.label12.TabIndex = 70;
            this.label12.Text = "Ref. No";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(37, 203);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 15);
            this.label4.TabIndex = 61;
            this.label4.Text = "End Date";
            // 
            // cboFlatNo
            // 
            this.cboFlatNo.EditValue = "";
            this.cboFlatNo.EnterMoveNextControl = true;
            this.cboFlatNo.Location = new System.Drawing.Point(150, 88);
            this.cboFlatNo.Name = "cboFlatNo";
            this.cboFlatNo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboFlatNo.Properties.Appearance.Options.UseFont = true;
            this.cboFlatNo.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            this.cboFlatNo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboFlatNo.Properties.LookAndFeel.SkinName = "Money Twins";
            this.cboFlatNo.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.cboFlatNo.Properties.NullText = "-- Select Flat --";
            this.cboFlatNo.Properties.ShowFooter = false;
            this.cboFlatNo.Properties.ShowHeader = false;
            this.cboFlatNo.Size = new System.Drawing.Size(206, 22);
            this.cboFlatNo.TabIndex = 10;
            this.cboFlatNo.EditValueChanged += new System.EventHandler(this.cboFlatNo_EditValueChanged);
            // 
            // cboProj
            // 
            this.cboProj.EditValue = "";
            this.cboProj.EnterMoveNextControl = true;
            this.cboProj.Location = new System.Drawing.Point(150, 55);
            this.cboProj.Name = "cboProj";
            this.cboProj.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboProj.Properties.Appearance.Options.UseFont = true;
            this.cboProj.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            this.cboProj.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboProj.Properties.LookAndFeel.SkinName = "Money Twins";
            this.cboProj.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.cboProj.Properties.NullText = "-- Select Project --";
            this.cboProj.Properties.ShowFooter = false;
            this.cboProj.Properties.ShowHeader = false;
            this.cboProj.Size = new System.Drawing.Size(206, 22);
            this.cboProj.TabIndex = 9;
            this.cboProj.EditValueChanged += new System.EventHandler(this.cboProj_EditValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(37, 126);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 15);
            this.label8.TabIndex = 50;
            this.label8.Text = "Start Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(38, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 15);
            this.label3.TabIndex = 47;
            this.label3.Text = "Reg. Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(39, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 15);
            this.label2.TabIndex = 46;
            this.label2.Text = "Flat No";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(39, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 15);
            this.label1.TabIndex = 45;
            this.label1.Text = "Project";
            // 
            // frmMaintenanceEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.ClientSize = new System.Drawing.Size(917, 506);
            this.ControlBox = false;
            this.Controls.Add(this.radPanel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmMaintenanceEntry";
            this.Text = "Maintenance Entry";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmRentEntry_FormClosed);
            this.Load += new System.EventHandler(this.frmRentEntry_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deEndDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStartDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStartDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deRegDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deRegDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GRDShedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GViewshedule)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtRemarks.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGrace.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboIntRate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboPerRate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIntRate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRefNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboFlatNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProj.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraBars.BarButtonItem btnExit;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem btnSave;
        private DevExpress.XtraBars.BarButtonItem btnCancel;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.TextEdit txtIntRate;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtRefNo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.MemoEdit txtRemarks;
        private DevExpress.XtraEditors.LookUpEdit cboFlatNo;
        private DevExpress.XtraEditors.LookUpEdit cboProj;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraGrid.GridControl GRDShedule;
        private DevExpress.XtraGrid.Views.Grid.GridView GViewshedule;
        private DevExpress.XtraEditors.ComboBoxEdit comboPerRate;
        private DevExpress.XtraEditors.ComboBoxEdit comboIntRate;
        private DevExpress.XtraEditors.TextEdit txtGrace;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.DateEdit deEndDate;
        private DevExpress.XtraEditors.DateEdit deStartDate;
        private DevExpress.XtraEditors.DateEdit deRegDate;
    }
}