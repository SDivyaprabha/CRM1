namespace CRM
{
    partial class frmFollowUp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFollowUp));
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.lblProject = new DevExpress.XtraEditors.LabelControl();
            this.btnCallType = new DevExpress.XtraEditors.SimpleButton();
            this.chkReq = new DevExpress.XtraEditors.CheckEdit();
            this.cboCall = new DevExpress.XtraEditors.LookUpEdit();
            this.cboNature = new DevExpress.XtraEditors.LookUpEdit();
            this.cboFlat = new DevExpress.XtraEditors.LookUpEdit();
            this.cboBuyer = new DevExpress.XtraEditors.LookUpEdit();
            this.cboProject = new DevExpress.XtraEditors.LookUpEdit();
            this.txtRemarks = new DevExpress.XtraEditors.MemoEdit();
            this.dECallDate = new DevExpress.XtraEditors.DateEdit();
            this.dEDate = new DevExpress.XtraEditors.DateEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.grdCall = new DevExpress.XtraGrid.GridControl();
            this.grdViewCall = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkReq.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCall.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboNature.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboFlat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboBuyer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProject.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemarks.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dECallDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dECallDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dEDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dEDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCall)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewCall)).BeginInit();
            this.SuspendLayout();
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Money Twins";
            // 
            // barManager1
            // 
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
            this.btnCancel,
            this.barButtonItem1});
            this.barManager1.MaxItemId = 4;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar1
            // 
            this.bar1.BarAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bar1.BarAppearance.Normal.Options.UseFont = true;
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem1, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnExit)});
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
            this.barAndDockingController1.AppearancesBar.StatusBarAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
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
            this.barDockControlTop.Size = new System.Drawing.Size(835, 26);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 422);
            this.barDockControlBottom.Size = new System.Drawing.Size(835, 26);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 26);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 396);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(835, 26);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 396);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl1.Location = new System.Drawing.Point(0, 26);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(387, 396);
            this.panelControl1.TabIndex = 4;
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.lblProject);
            this.groupControl1.Controls.Add(this.btnCallType);
            this.groupControl1.Controls.Add(this.chkReq);
            this.groupControl1.Controls.Add(this.cboCall);
            this.groupControl1.Controls.Add(this.cboNature);
            this.groupControl1.Controls.Add(this.cboFlat);
            this.groupControl1.Controls.Add(this.cboBuyer);
            this.groupControl1.Controls.Add(this.cboProject);
            this.groupControl1.Controls.Add(this.txtRemarks);
            this.groupControl1.Controls.Add(this.dECallDate);
            this.groupControl1.Controls.Add(this.dEDate);
            this.groupControl1.Controls.Add(this.labelControl9);
            this.groupControl1.Controls.Add(this.labelControl8);
            this.groupControl1.Controls.Add(this.labelControl6);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(2, 2);
            this.groupControl1.LookAndFeel.SkinName = "Blue";
            this.groupControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(384, 392);
            this.groupControl1.TabIndex = 24;
            this.groupControl1.Text = "Post Sale - FollowUp Entry";
            // 
            // lblProject
            // 
            this.lblProject.Location = new System.Drawing.Point(309, 63);
            this.lblProject.Name = "lblProject";
            this.lblProject.Size = new System.Drawing.Size(34, 13);
            this.lblProject.TabIndex = 9;
            this.lblProject.Text = "Project";
            // 
            // btnCallType
            // 
            this.btnCallType.Location = new System.Drawing.Point(309, 161);
            this.btnCallType.LookAndFeel.SkinName = "Money Twins";
            this.btnCallType.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCallType.Name = "btnCallType";
            this.btnCallType.Size = new System.Drawing.Size(35, 23);
            this.btnCallType.TabIndex = 41;
            this.btnCallType.Text = "...";
            this.btnCallType.Click += new System.EventHandler(this.btnCallType_Click);
            // 
            // chkReq
            // 
            this.chkReq.Location = new System.Drawing.Point(81, 202);
            this.chkReq.MenuManager = this.barManager1;
            this.chkReq.Name = "chkReq";
            this.chkReq.Properties.Caption = "FollowUp Required";
            this.chkReq.Size = new System.Drawing.Size(123, 19);
            this.chkReq.TabIndex = 40;
            this.chkReq.CheckedChanged += new System.EventHandler(this.chkReq_CheckedChanged);
            // 
            // cboCall
            // 
            this.cboCall.Location = new System.Drawing.Point(118, 164);
            this.cboCall.MenuManager = this.barManager1;
            this.cboCall.Name = "cboCall";
            this.cboCall.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboCall.Properties.LookAndFeel.SkinName = "Money Twins";
            this.cboCall.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.cboCall.Properties.NullText = "None";
            this.cboCall.Size = new System.Drawing.Size(185, 20);
            this.cboCall.TabIndex = 39;
            // 
            // cboNature
            // 
            this.cboNature.Location = new System.Drawing.Point(118, 138);
            this.cboNature.MenuManager = this.barManager1;
            this.cboNature.Name = "cboNature";
            this.cboNature.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboNature.Properties.LookAndFeel.SkinName = "Money Twins";
            this.cboNature.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.cboNature.Properties.NullText = "None";
            this.cboNature.Size = new System.Drawing.Size(185, 20);
            this.cboNature.TabIndex = 38;
            // 
            // cboFlat
            // 
            this.cboFlat.Location = new System.Drawing.Point(118, 112);
            this.cboFlat.MenuManager = this.barManager1;
            this.cboFlat.Name = "cboFlat";
            this.cboFlat.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboFlat.Properties.LookAndFeel.SkinName = "Money Twins";
            this.cboFlat.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.cboFlat.Properties.NullText = "None";
            this.cboFlat.Size = new System.Drawing.Size(185, 20);
            this.cboFlat.TabIndex = 37;
            this.cboFlat.EditValueChanged += new System.EventHandler(this.cboFlat_EditValueChanged);
            // 
            // cboBuyer
            // 
            this.cboBuyer.Location = new System.Drawing.Point(118, 86);
            this.cboBuyer.MenuManager = this.barManager1;
            this.cboBuyer.Name = "cboBuyer";
            this.cboBuyer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboBuyer.Properties.LookAndFeel.SkinName = "Money Twins";
            this.cboBuyer.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.cboBuyer.Properties.NullText = "None";
            this.cboBuyer.Size = new System.Drawing.Size(185, 20);
            this.cboBuyer.TabIndex = 36;
            this.cboBuyer.EditValueChanged += new System.EventHandler(this.cboBuyer_EditValueChanged);
            // 
            // cboProject
            // 
            this.cboProject.Location = new System.Drawing.Point(118, 60);
            this.cboProject.MenuManager = this.barManager1;
            this.cboProject.Name = "cboProject";
            this.cboProject.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboProject.Properties.LookAndFeel.SkinName = "Money Twins";
            this.cboProject.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.cboProject.Properties.NullText = "None";
            this.cboProject.Size = new System.Drawing.Size(185, 20);
            this.cboProject.TabIndex = 35;
            this.cboProject.EditValueChanged += new System.EventHandler(this.cboProject_EditValueChanged);
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(118, 261);
            this.txtRemarks.MenuManager = this.barManager1;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Properties.LookAndFeel.SkinName = "Money Twins";
            this.txtRemarks.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtRemarks.Size = new System.Drawing.Size(248, 64);
            this.txtRemarks.TabIndex = 34;
            // 
            // dECallDate
            // 
            this.dECallDate.EditValue = null;
            this.dECallDate.Location = new System.Drawing.Point(118, 235);
            this.dECallDate.MenuManager = this.barManager1;
            this.dECallDate.Name = "dECallDate";
            this.dECallDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dECallDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dECallDate.Size = new System.Drawing.Size(127, 20);
            this.dECallDate.TabIndex = 33;
            this.dECallDate.EditValueChanged += new System.EventHandler(this.dECallDate_EditValueChanged);
            // 
            // dEDate
            // 
            this.dEDate.EditValue = null;
            this.dEDate.Location = new System.Drawing.Point(118, 34);
            this.dEDate.MenuManager = this.barManager1;
            this.dEDate.Name = "dEDate";
            this.dEDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dEDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dEDate.Size = new System.Drawing.Size(127, 20);
            this.dEDate.TabIndex = 32;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(22, 264);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(41, 13);
            this.labelControl9.TabIndex = 31;
            this.labelControl9.Text = "Remarks";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(22, 239);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(69, 13);
            this.labelControl8.TabIndex = 30;
            this.labelControl8.Text = "Next Call Date";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(22, 167);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(44, 13);
            this.labelControl6.TabIndex = 29;
            this.labelControl6.Text = "Call Type";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(22, 141);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(33, 13);
            this.labelControl5.TabIndex = 28;
            this.labelControl5.Text = "Nature";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(22, 115);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(35, 13);
            this.labelControl4.TabIndex = 27;
            this.labelControl4.Text = "Unit No";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(22, 89);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(58, 13);
            this.labelControl3.TabIndex = 26;
            this.labelControl3.Text = "Buyer Name";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(23, 63);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(34, 13);
            this.labelControl2.TabIndex = 25;
            this.labelControl2.Text = "Project";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(22, 38);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(23, 13);
            this.labelControl1.TabIndex = 24;
            this.labelControl1.Text = "Date";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.groupControl2);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(387, 26);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(448, 396);
            this.panelControl2.TabIndex = 0;
            // 
            // groupControl2
            // 
            this.groupControl2.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl2.AppearanceCaption.Options.UseFont = true;
            this.groupControl2.Controls.Add(this.grdCall);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(2, 2);
            this.groupControl2.LookAndFeel.SkinName = "Blue";
            this.groupControl2.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(444, 392);
            this.groupControl2.TabIndex = 106;
            this.groupControl2.Text = "Pending Calls";
            // 
            // grdCall
            // 
            this.grdCall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCall.Location = new System.Drawing.Point(2, 22);
            this.grdCall.LookAndFeel.SkinName = "Blue";
            this.grdCall.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdCall.MainView = this.grdViewCall;
            this.grdCall.Name = "grdCall";
            this.grdCall.Size = new System.Drawing.Size(440, 368);
            this.grdCall.TabIndex = 105;
            this.grdCall.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdViewCall});
            // 
            // grdViewCall
            // 
            this.grdViewCall.ColumnPanelRowHeight = 30;
            this.grdViewCall.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.grdViewCall.GridControl = this.grdCall;
            this.grdViewCall.IndicatorWidth = 60;
            this.grdViewCall.Name = "grdViewCall";
            this.grdViewCall.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.grdViewCall.OptionsBehavior.AllowIncrementalSearch = true;
            this.grdViewCall.OptionsBehavior.Editable = false;
            this.grdViewCall.OptionsBehavior.ReadOnly = true;
            this.grdViewCall.OptionsNavigation.AutoFocusNewRow = true;
            this.grdViewCall.OptionsNavigation.EnterMoveNextColumn = true;
            this.grdViewCall.OptionsView.ShowFooter = true;
            this.grdViewCall.OptionsView.ShowGroupedColumns = true;
            this.grdViewCall.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.grdViewCall.OptionsView.ShowGroupPanel = false;
            this.grdViewCall.PaintStyleName = "Skin";
            this.grdViewCall.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.grdViewCall_CustomDrawRowIndicator);
            this.grdViewCall.Click += new System.EventHandler(this.grdViewCall_Click);
            this.grdViewCall.Layout += new System.EventHandler(this.grdViewCall_Layout);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barButtonItem1.Caption = "Fix Layout";
            this.barButtonItem1.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.Glyph")));
            this.barButtonItem1.Id = 3;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // frmFollowUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 448);
            this.ControlBox = false;
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "frmFollowUp";
            this.Text = "Post Sale FollowUp";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmFollowUp_FormClosed);
            this.Load += new System.EventHandler(this.frmFollowUp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkReq.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCall.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboNature.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboFlat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboBuyer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProject.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemarks.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dECallDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dECallDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dEDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dEDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCall)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewCall)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem btnExit;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraBars.BarButtonItem btnOK;
        private DevExpress.XtraBars.BarButtonItem btnCancel;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.GridControl grdCall;
        private DevExpress.XtraGrid.Views.Grid.GridView grdViewCall;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton btnCallType;
        private DevExpress.XtraEditors.CheckEdit chkReq;
        private DevExpress.XtraEditors.LookUpEdit cboCall;
        private DevExpress.XtraEditors.LookUpEdit cboNature;
        private DevExpress.XtraEditors.LookUpEdit cboFlat;
        private DevExpress.XtraEditors.LookUpEdit cboBuyer;
        private DevExpress.XtraEditors.LookUpEdit cboProject;
        private DevExpress.XtraEditors.MemoEdit txtRemarks;
        private DevExpress.XtraEditors.DateEdit dECallDate;
        private DevExpress.XtraEditors.DateEdit dEDate;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl lblProject;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
    }
}