namespace CRM
{
    partial class frmStageEntry
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStageEntry));
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnExit = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.btnOK = new DevExpress.XtraBars.BarButtonItem();
            this.btnCancel = new DevExpress.XtraBars.BarButtonItem();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.dEDue = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cboProj = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.cboStage = new DevExpress.XtraEditors.LookUpEdit();
            this.cboSLevel = new DevExpress.XtraEditors.LookUpEdit();
            this.cboSBlock = new DevExpress.XtraEditors.LookUpEdit();
            this.txtSRemark = new DevExpress.XtraEditors.MemoEdit();
            this.txtRefNo = new DevExpress.XtraEditors.TextEdit();
            this.SCdate = new DevExpress.XtraEditors.DateEdit();
            this.Sdate = new DevExpress.XtraEditors.DateEdit();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dEDue.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dEDue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProj.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSLevel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSBlock.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRefNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SCdate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SCdate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Sdate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Sdate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Blue";
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.AllowQuickCustomization = false;
            this.barManager1.AllowShowToolbarsPopup = false;
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar3});
            this.barManager1.Controller = this.barAndDockingController1;
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnExit,
            this.btnOK,
            this.btnCancel});
            this.barManager1.MaxItemId = 3;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnExit, DevExpress.XtraBars.BarItemPaintStyle.Standard)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // btnExit
            // 
            this.btnExit.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnExit.Caption = "Exit";
            this.btnExit.Glyph = global::CRM.Properties.Resources.exit1;
            this.btnExit.Id = 0;
            this.btnExit.Name = "btnExit";
            this.btnExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExit_ItemClick);
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnOK, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnCancel, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // btnOK
            // 
            this.btnOK.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnOK.Caption = "OK";
            this.btnOK.Glyph = ((System.Drawing.Image)(resources.GetObject("btnOK.Glyph")));
            this.btnOK.Id = 1;
            this.btnOK.Name = "btnOK";
            this.btnOK.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOK_ItemClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Caption = "Cancel";
            this.btnCancel.Glyph = ((System.Drawing.Image)(resources.GetObject("btnCancel.Glyph")));
            this.btnCancel.Id = 2;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCancel_ItemClick);
            // 
            // barAndDockingController1
            // 
            this.barAndDockingController1.AppearancesBar.StatusBarAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.barAndDockingController1.AppearancesBar.StatusBarAppearance.Normal.Options.UseFont = true;
            this.barAndDockingController1.LookAndFeel.SkinName = "Blue";
            this.barAndDockingController1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.barAndDockingController1.PropertiesBar.AllowLinkLighting = false;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(607, 26);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 330);
            this.barDockControlBottom.Size = new System.Drawing.Size(607, 26);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 26);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 304);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(607, 26);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 304);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.dEDue);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.cboProj);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.comboBoxEdit1);
            this.panelControl1.Controls.Add(this.labelControl13);
            this.panelControl1.Controls.Add(this.cboStage);
            this.panelControl1.Controls.Add(this.cboSLevel);
            this.panelControl1.Controls.Add(this.cboSBlock);
            this.panelControl1.Controls.Add(this.txtSRemark);
            this.panelControl1.Controls.Add(this.txtRefNo);
            this.panelControl1.Controls.Add(this.SCdate);
            this.panelControl1.Controls.Add(this.Sdate);
            this.panelControl1.Controls.Add(this.labelControl12);
            this.panelControl1.Controls.Add(this.labelControl11);
            this.panelControl1.Controls.Add(this.labelControl10);
            this.panelControl1.Controls.Add(this.labelControl9);
            this.panelControl1.Controls.Add(this.labelControl8);
            this.panelControl1.Controls.Add(this.labelControl7);
            this.panelControl1.Controls.Add(this.labelControl6);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 26);
            this.panelControl1.LookAndFeel.SkinName = "Money Twins";
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(607, 304);
            this.panelControl1.TabIndex = 38;
            // 
            // dEDue
            // 
            this.dEDue.EditValue = null;
            this.dEDue.EnterMoveNextControl = true;
            this.dEDue.Location = new System.Drawing.Point(430, 184);
            this.dEDue.MenuManager = this.barManager1;
            this.dEDue.Name = "dEDue";
            this.dEDue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dEDue.Properties.LookAndFeel.SkinName = "Money Twins";
            this.dEDue.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.dEDue.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dEDue.Size = new System.Drawing.Size(161, 20);
            this.dEDue.TabIndex = 51;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Location = new System.Drawing.Point(320, 187);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(95, 13);
            this.labelControl2.TabIndex = 52;
            this.labelControl2.Text = "Due On or Before";
            // 
            // cboProj
            // 
            this.cboProj.EnterMoveNextControl = true;
            this.cboProj.Location = new System.Drawing.Point(136, 54);
            this.cboProj.MenuManager = this.barManager1;
            this.cboProj.Name = "cboProj";
            this.cboProj.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            this.cboProj.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboProj.Properties.LookAndFeel.SkinName = "Money Twins";
            this.cboProj.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.cboProj.Properties.NullText = "None";
            this.cboProj.Size = new System.Drawing.Size(161, 20);
            this.cboProj.TabIndex = 3;
            this.cboProj.EditValueChanged += new System.EventHandler(this.cboProj_EditValueChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Location = new System.Drawing.Point(23, 58);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(41, 13);
            this.labelControl1.TabIndex = 50;
            this.labelControl1.Text = "Project";
            // 
            // comboBoxEdit1
            // 
            this.comboBoxEdit1.EditValue = "None";
            this.comboBoxEdit1.EnterMoveNextControl = true;
            this.comboBoxEdit1.Location = new System.Drawing.Point(136, 137);
            this.comboBoxEdit1.MenuManager = this.barManager1;
            this.comboBoxEdit1.Name = "comboBoxEdit1";
            this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit1.Properties.Items.AddRange(new object[] {
            "SchDescription",
            "Stagewise",
            "OtherCost"});
            this.comboBoxEdit1.Properties.LookAndFeel.SkinName = "Money Twins";
            this.comboBoxEdit1.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.comboBoxEdit1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboBoxEdit1.Size = new System.Drawing.Size(161, 20);
            this.comboBoxEdit1.TabIndex = 6;
            this.comboBoxEdit1.SelectedIndexChanged += new System.EventHandler(this.comboBoxEdit1_SelectedIndexChanged);
            // 
            // labelControl13
            // 
            this.labelControl13.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl13.Location = new System.Drawing.Point(23, 141);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(64, 13);
            this.labelControl13.TabIndex = 48;
            this.labelControl13.Text = "Stage Type";
            // 
            // cboStage
            // 
            this.cboStage.EnterMoveNextControl = true;
            this.cboStage.Location = new System.Drawing.Point(430, 137);
            this.cboStage.MenuManager = this.barManager1;
            this.cboStage.Name = "cboStage";
            this.cboStage.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboStage.Properties.LookAndFeel.SkinName = "Money Twins";
            this.cboStage.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.cboStage.Properties.NullText = "None";
            this.cboStage.Size = new System.Drawing.Size(161, 20);
            this.cboStage.TabIndex = 7;
            // 
            // cboSLevel
            // 
            this.cboSLevel.EnterMoveNextControl = true;
            this.cboSLevel.Location = new System.Drawing.Point(430, 94);
            this.cboSLevel.MenuManager = this.barManager1;
            this.cboSLevel.Name = "cboSLevel";
            this.cboSLevel.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboSLevel.Properties.LookAndFeel.SkinName = "Money Twins";
            this.cboSLevel.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.cboSLevel.Properties.NullText = "None";
            this.cboSLevel.Size = new System.Drawing.Size(161, 20);
            this.cboSLevel.TabIndex = 5;
            // 
            // cboSBlock
            // 
            this.cboSBlock.EnterMoveNextControl = true;
            this.cboSBlock.Location = new System.Drawing.Point(136, 94);
            this.cboSBlock.MenuManager = this.barManager1;
            this.cboSBlock.Name = "cboSBlock";
            this.cboSBlock.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboSBlock.Properties.LookAndFeel.SkinName = "Money Twins";
            this.cboSBlock.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.cboSBlock.Properties.NullText = "None";
            this.cboSBlock.Size = new System.Drawing.Size(161, 20);
            this.cboSBlock.TabIndex = 4;
            // 
            // txtSRemark
            // 
            this.txtSRemark.EnterMoveNextControl = true;
            this.txtSRemark.Location = new System.Drawing.Point(136, 224);
            this.txtSRemark.MenuManager = this.barManager1;
            this.txtSRemark.Name = "txtSRemark";
            this.txtSRemark.Properties.LookAndFeel.SkinName = "Blue";
            this.txtSRemark.Properties.MaxLength = 5000;
            this.txtSRemark.Size = new System.Drawing.Size(455, 66);
            this.txtSRemark.TabIndex = 9;
            // 
            // txtRefNo
            // 
            this.txtRefNo.EnterMoveNextControl = true;
            this.txtRefNo.Location = new System.Drawing.Point(430, 16);
            this.txtRefNo.MenuManager = this.barManager1;
            this.txtRefNo.Name = "txtRefNo";
            this.txtRefNo.Properties.LookAndFeel.SkinName = "Blue";
            this.txtRefNo.Size = new System.Drawing.Size(161, 20);
            this.txtRefNo.TabIndex = 2;
            // 
            // SCdate
            // 
            this.SCdate.EditValue = null;
            this.SCdate.EnterMoveNextControl = true;
            this.SCdate.Location = new System.Drawing.Point(136, 183);
            this.SCdate.MenuManager = this.barManager1;
            this.SCdate.Name = "SCdate";
            this.SCdate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SCdate.Properties.LookAndFeel.SkinName = "Money Twins";
            this.SCdate.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.SCdate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.SCdate.Size = new System.Drawing.Size(161, 20);
            this.SCdate.TabIndex = 7;
            // 
            // Sdate
            // 
            this.Sdate.EditValue = null;
            this.Sdate.EnterMoveNextControl = true;
            this.Sdate.Location = new System.Drawing.Point(136, 16);
            this.Sdate.MenuManager = this.barManager1;
            this.Sdate.Name = "Sdate";
            this.Sdate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Sdate.Properties.LookAndFeel.SkinName = "Money Twins";
            this.Sdate.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Sdate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.Sdate.Size = new System.Drawing.Size(161, 20);
            this.Sdate.TabIndex = 1;
            this.Sdate.EditValueChanged += new System.EventHandler(this.Sdate_EditValueChanged_1);
            // 
            // labelControl12
            // 
            this.labelControl12.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl12.Location = new System.Drawing.Point(23, 227);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(51, 13);
            this.labelControl12.TabIndex = 40;
            this.labelControl12.Text = "Remarks";
            // 
            // labelControl11
            // 
            this.labelControl11.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl11.Location = new System.Drawing.Point(23, 187);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(94, 13);
            this.labelControl11.TabIndex = 39;
            this.labelControl11.Text = "Completion Date";
            // 
            // labelControl10
            // 
            this.labelControl10.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl10.Location = new System.Drawing.Point(320, 141);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(68, 13);
            this.labelControl10.TabIndex = 38;
            this.labelControl10.Text = "Stage Name";
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl9.Location = new System.Drawing.Point(320, 98);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(30, 13);
            this.labelControl9.TabIndex = 37;
            this.labelControl9.Text = "Level";
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl8.Location = new System.Drawing.Point(23, 98);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(31, 13);
            this.labelControl8.TabIndex = 36;
            this.labelControl8.Text = "Block";
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl7.Location = new System.Drawing.Point(320, 20);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(36, 13);
            this.labelControl7.TabIndex = 35;
            this.labelControl7.Text = "Ref No";
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl6.Location = new System.Drawing.Point(23, 20);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(27, 13);
            this.labelControl6.TabIndex = 34;
            this.labelControl6.Text = "Date";
            // 
            // frmStageEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 356);
            this.ControlBox = false;
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmStageEntry";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stage Entry";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmStageEntry_FormClosed);
            this.Load += new System.EventHandler(this.frmStageEntry_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dEDue.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dEDue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProj.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSLevel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSBlock.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRefNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SCdate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SCdate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Sdate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Sdate.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem btnExit;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem btnOK;
        private DevExpress.XtraBars.BarButtonItem btnCancel;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraEditors.LookUpEdit cboStage;
        private DevExpress.XtraEditors.LookUpEdit cboSLevel;
        private DevExpress.XtraEditors.LookUpEdit cboSBlock;
        private DevExpress.XtraEditors.MemoEdit txtSRemark;
        private DevExpress.XtraEditors.TextEdit txtRefNo;
        private DevExpress.XtraEditors.DateEdit SCdate;
        private DevExpress.XtraEditors.DateEdit Sdate;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LookUpEdit cboProj;
        private DevExpress.XtraEditors.DateEdit dEDue;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}