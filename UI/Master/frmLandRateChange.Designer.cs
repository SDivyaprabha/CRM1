namespace CRM
{
    partial class frmLandRateChange
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLandRateChange));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.btnHistory = new DevExpress.XtraBars.BarButtonItem();
            this.btnOK = new DevExpress.XtraBars.BarButtonItem();
            this.btnCancel = new DevExpress.XtraBars.BarButtonItem();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.radGroupBox3 = new Telerik.WinControls.UI.RadGroupBox();
            this.chkGLV = new DevExpress.XtraEditors.CheckEdit();
            this.chkMLV = new DevExpress.XtraEditors.CheckEdit();
            this.chkReg = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.radGroupBox2 = new Telerik.WinControls.UI.RadGroupBox();
            this.txtNewReg = new DevExpress.XtraEditors.TextEdit();
            this.txtNewGuide = new DevExpress.XtraEditors.TextEdit();
            this.txtNewRate = new DevExpress.XtraEditors.TextEdit();
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.txtOldGuide = new DevExpress.XtraEditors.TextEdit();
            this.txtOldReg = new DevExpress.XtraEditors.TextEdit();
            this.txtOldRate = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dEDate = new DevExpress.XtraEditors.DateEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.cboBlock = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox3)).BeginInit();
            this.radGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkGLV.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMLV.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkReg.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).BeginInit();
            this.radGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewReg.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewGuide.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewRate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtOldGuide.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOldReg.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOldRate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dEDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dEDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboBlock.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.AllowShowToolbarsPopup = false;
            this.barManager1.AutoSaveInRegistry = true;
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar3});
            this.barManager1.Controller = this.barAndDockingController1;
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnOK,
            this.btnCancel,
            this.btnHistory});
            this.barManager1.MaxItemId = 3;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnHistory, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnOK, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnCancel, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // btnHistory
            // 
            this.btnHistory.Caption = "LandRate Change History";
            this.btnHistory.Glyph = global::CRM.Properties.Resources.world;
            this.btnHistory.Id = 2;
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnHistory_ItemClick);
            // 
            // btnOK
            // 
            this.btnOK.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnOK.Caption = "OK";
            this.btnOK.Glyph = ((System.Drawing.Image)(resources.GetObject("btnOK.Glyph")));
            this.btnOK.Id = 0;
            this.btnOK.Name = "btnOK";
            this.btnOK.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOK_ItemClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnCancel.Caption = "Cancel";
            this.btnCancel.Glyph = ((System.Drawing.Image)(resources.GetObject("btnCancel.Glyph")));
            this.btnCancel.Id = 1;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCancel_ItemClick);
            // 
            // barAndDockingController1
            // 
            this.barAndDockingController1.PropertiesBar.AllowLinkLighting = false;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(463, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 249);
            this.barDockControlBottom.Size = new System.Drawing.Size(463, 26);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 249);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(463, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 249);
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Money Twins";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.radGroupBox3);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.radGroupBox2);
            this.panelControl1.Controls.Add(this.radGroupBox1);
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 80);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(463, 169);
            this.panelControl1.TabIndex = 4;
            // 
            // radGroupBox3
            // 
            this.radGroupBox3.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox3.Controls.Add(this.chkGLV);
            this.radGroupBox3.Controls.Add(this.chkMLV);
            this.radGroupBox3.Controls.Add(this.chkReg);
            this.radGroupBox3.FooterImageIndex = -1;
            this.radGroupBox3.FooterImageKey = "";
            this.radGroupBox3.HeaderImageIndex = -1;
            this.radGroupBox3.HeaderImageKey = "";
            this.radGroupBox3.HeaderMargin = new System.Windows.Forms.Padding(0);
            this.radGroupBox3.HeaderText = "All";
            this.radGroupBox3.Location = new System.Drawing.Point(406, 22);
            this.radGroupBox3.Name = "radGroupBox3";
            this.radGroupBox3.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            // 
            // 
            // 
            this.radGroupBox3.RootElement.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            this.radGroupBox3.Size = new System.Drawing.Size(45, 135);
            this.radGroupBox3.TabIndex = 18;
            this.radGroupBox3.Text = "All";
            // 
            // chkGLV
            // 
            this.chkGLV.Location = new System.Drawing.Point(12, 24);
            this.chkGLV.MenuManager = this.barManager1;
            this.chkGLV.Name = "chkGLV";
            this.chkGLV.Properties.Caption = "";
            this.chkGLV.Size = new System.Drawing.Size(21, 19);
            this.chkGLV.TabIndex = 19;
            // 
            // chkMLV
            // 
            this.chkMLV.Location = new System.Drawing.Point(12, 64);
            this.chkMLV.MenuManager = this.barManager1;
            this.chkMLV.Name = "chkMLV";
            this.chkMLV.Properties.Caption = "";
            this.chkMLV.Size = new System.Drawing.Size(20, 19);
            this.chkMLV.TabIndex = 20;
            // 
            // chkReg
            // 
            this.chkReg.Location = new System.Drawing.Point(13, 108);
            this.chkReg.MenuManager = this.barManager1;
            this.chkReg.Name = "chkReg";
            this.chkReg.Properties.Caption = "";
            this.chkReg.Size = new System.Drawing.Size(20, 19);
            this.chkReg.TabIndex = 21;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(20, 132);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(80, 13);
            this.labelControl2.TabIndex = 13;
            this.labelControl2.Text = "Registration (%)";
            // 
            // radGroupBox2
            // 
            this.radGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox2.Controls.Add(this.txtNewReg);
            this.radGroupBox2.Controls.Add(this.txtNewGuide);
            this.radGroupBox2.Controls.Add(this.txtNewRate);
            this.radGroupBox2.FooterImageIndex = -1;
            this.radGroupBox2.FooterImageKey = "";
            this.radGroupBox2.HeaderImageIndex = -1;
            this.radGroupBox2.HeaderImageKey = "";
            this.radGroupBox2.HeaderMargin = new System.Windows.Forms.Padding(0);
            this.radGroupBox2.HeaderText = "New Rate";
            this.radGroupBox2.Location = new System.Drawing.Point(267, 22);
            this.radGroupBox2.Name = "radGroupBox2";
            this.radGroupBox2.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            // 
            // 
            // 
            this.radGroupBox2.RootElement.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            this.radGroupBox2.Size = new System.Drawing.Size(128, 135);
            this.radGroupBox2.TabIndex = 12;
            this.radGroupBox2.Text = "New Rate";
            // 
            // txtNewReg
            // 
            this.txtNewReg.Location = new System.Drawing.Point(13, 107);
            this.txtNewReg.MenuManager = this.barManager1;
            this.txtNewReg.Name = "txtNewReg";
            this.txtNewReg.Size = new System.Drawing.Size(100, 20);
            this.txtNewReg.TabIndex = 5;
            // 
            // txtNewGuide
            // 
            this.txtNewGuide.Location = new System.Drawing.Point(13, 23);
            this.txtNewGuide.MenuManager = this.barManager1;
            this.txtNewGuide.Name = "txtNewGuide";
            this.txtNewGuide.Size = new System.Drawing.Size(100, 20);
            this.txtNewGuide.TabIndex = 1;
            // 
            // txtNewRate
            // 
            this.txtNewRate.Location = new System.Drawing.Point(13, 67);
            this.txtNewRate.MenuManager = this.barManager1;
            this.txtNewRate.Name = "txtNewRate";
            this.txtNewRate.Size = new System.Drawing.Size(100, 20);
            this.txtNewRate.TabIndex = 3;
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.txtOldGuide);
            this.radGroupBox1.Controls.Add(this.txtOldReg);
            this.radGroupBox1.Controls.Add(this.txtOldRate);
            this.radGroupBox1.FooterImageIndex = -1;
            this.radGroupBox1.FooterImageKey = "";
            this.radGroupBox1.HeaderImageIndex = -1;
            this.radGroupBox1.HeaderImageKey = "";
            this.radGroupBox1.HeaderMargin = new System.Windows.Forms.Padding(0);
            this.radGroupBox1.HeaderText = "Old Rate";
            this.radGroupBox1.Location = new System.Drawing.Point(111, 22);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            // 
            // 
            // 
            this.radGroupBox1.RootElement.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            this.radGroupBox1.Size = new System.Drawing.Size(129, 135);
            this.radGroupBox1.TabIndex = 11;
            this.radGroupBox1.Text = "Old Rate";
            // 
            // txtOldGuide
            // 
            this.txtOldGuide.Enabled = false;
            this.txtOldGuide.Location = new System.Drawing.Point(13, 23);
            this.txtOldGuide.MenuManager = this.barManager1;
            this.txtOldGuide.Name = "txtOldGuide";
            this.txtOldGuide.Size = new System.Drawing.Size(100, 20);
            this.txtOldGuide.TabIndex = 0;
            // 
            // txtOldReg
            // 
            this.txtOldReg.Enabled = false;
            this.txtOldReg.Location = new System.Drawing.Point(13, 107);
            this.txtOldReg.MenuManager = this.barManager1;
            this.txtOldReg.Name = "txtOldReg";
            this.txtOldReg.Size = new System.Drawing.Size(100, 20);
            this.txtOldReg.TabIndex = 4;
            // 
            // txtOldRate
            // 
            this.txtOldRate.Enabled = false;
            this.txtOldRate.Location = new System.Drawing.Point(13, 67);
            this.txtOldRate.MenuManager = this.barManager1;
            this.txtOldRate.Name = "txtOldRate";
            this.txtOldRate.Size = new System.Drawing.Size(100, 20);
            this.txtOldRate.TabIndex = 2;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(20, 92);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(85, 13);
            this.labelControl5.TabIndex = 4;
            this.labelControl5.Text = "Market LandValue";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(20, 48);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(75, 13);
            this.labelControl4.TabIndex = 3;
            this.labelControl4.Text = "GuideLine Value";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(44, 23);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(23, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Date";
            // 
            // dEDate
            // 
            this.dEDate.EditValue = null;
            this.dEDate.Location = new System.Drawing.Point(111, 16);
            this.dEDate.MenuManager = this.barManager1;
            this.dEDate.Name = "dEDate";
            this.dEDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dEDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dEDate.Size = new System.Drawing.Size(156, 20);
            this.dEDate.TabIndex = 5;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.cboBlock);
            this.panelControl2.Controls.Add(this.labelControl3);
            this.panelControl2.Controls.Add(this.dEDate);
            this.panelControl2.Controls.Add(this.labelControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(463, 80);
            this.panelControl2.TabIndex = 13;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(44, 51);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(24, 13);
            this.labelControl3.TabIndex = 6;
            this.labelControl3.Text = "Block";
            // 
            // cboBlock
            // 
            this.cboBlock.Location = new System.Drawing.Point(111, 48);
            this.cboBlock.MenuManager = this.barManager1;
            this.cboBlock.Name = "cboBlock";
            this.cboBlock.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboBlock.Properties.NullText = "None";
            this.cboBlock.Size = new System.Drawing.Size(156, 20);
            this.cboBlock.TabIndex = 7;
            this.cboBlock.EditValueChanged += new System.EventHandler(this.cboBlock_EditValueChanged);
            // 
            // frmLandRateChange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 275);
            this.ControlBox = false;
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmLandRateChange";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Land Rate Change";
            this.Load += new System.EventHandler(this.frmLandRateChange_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox3)).EndInit();
            this.radGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkGLV.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMLV.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkReg.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).EndInit();
            this.radGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtNewReg.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewGuide.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewRate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtOldGuide.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOldReg.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOldRate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dEDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dEDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboBlock.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraBars.BarButtonItem btnOK;
        private DevExpress.XtraBars.BarButtonItem btnCancel;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox2;
        private DevExpress.XtraEditors.TextEdit txtNewGuide;
        private DevExpress.XtraEditors.TextEdit txtNewRate;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private DevExpress.XtraEditors.TextEdit txtOldGuide;
        private DevExpress.XtraEditors.TextEdit txtOldRate;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.DateEdit dEDate;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraBars.BarButtonItem btnHistory;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtNewReg;
        private DevExpress.XtraEditors.TextEdit txtOldReg;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox3;
        private DevExpress.XtraEditors.CheckEdit chkGLV;
        private DevExpress.XtraEditors.CheckEdit chkMLV;
        private DevExpress.XtraEditors.CheckEdit chkReg;
        private DevExpress.XtraEditors.LookUpEdit cboBlock;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}