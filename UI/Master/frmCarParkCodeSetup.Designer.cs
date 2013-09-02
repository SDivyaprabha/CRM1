namespace CRM
{
    partial class frmCarParkCodeSetup
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
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.btnOk = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtPrefix = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtSuffix = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtStartNo = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtWidth = new DevExpress.XtraEditors.TextEdit();
            this.RGType = new DevExpress.XtraEditors.RadioGroup();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrefix.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSuffix.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWidth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RGType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Blue";
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.AllowMoveBarOnToolbar = false;
            this.barManager1.AllowQuickCustomization = false;
            this.barManager1.AllowShowToolbarsPopup = false;
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar3});
            this.barManager1.Controller = this.barAndDockingController1;
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnOk,
            this.barButtonItem2});
            this.barManager1.MaxItemId = 2;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar3
            // 
            this.bar3.BarAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bar3.BarAppearance.Normal.Options.UseFont = true;
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnOk, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem2, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // btnOk
            // 
            this.btnOk.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnOk.Caption = "Ok";
            this.btnOk.Glyph = global::CRM.Properties.Resources.Ok_icon;
            this.btnOk.Id = 0;
            this.btnOk.Name = "btnOk";
            this.btnOk.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOk_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "Cancel";
            this.barButtonItem2.Glyph = global::CRM.Properties.Resources.cancel;
            this.barButtonItem2.Id = 1;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
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
            this.barDockControlTop.Size = new System.Drawing.Size(397, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 109);
            this.barDockControlBottom.Size = new System.Drawing.Size(397, 26);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 109);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(397, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 109);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(13, 51);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(34, 13);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "Prefix  ";
            // 
            // txtPrefix
            // 
            this.txtPrefix.Enabled = false;
            this.txtPrefix.EnterMoveNextControl = true;
            this.txtPrefix.Location = new System.Drawing.Point(59, 48);
            this.txtPrefix.MenuManager = this.barManager1;
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Properties.LookAndFeel.SkinName = "Money Twins";
            this.txtPrefix.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtPrefix.Properties.ValidateOnEnterKey = true;
            this.txtPrefix.Size = new System.Drawing.Size(136, 20);
            this.txtPrefix.TabIndex = 5;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(215, 51);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(34, 13);
            this.labelControl2.TabIndex = 6;
            this.labelControl2.Text = "Suffix  ";
            // 
            // txtSuffix
            // 
            this.txtSuffix.Enabled = false;
            this.txtSuffix.EnterMoveNextControl = true;
            this.txtSuffix.Location = new System.Drawing.Point(249, 48);
            this.txtSuffix.MenuManager = this.barManager1;
            this.txtSuffix.Name = "txtSuffix";
            this.txtSuffix.Properties.LookAndFeel.SkinName = "Money Twins";
            this.txtSuffix.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtSuffix.Properties.ValidateOnEnterKey = true;
            this.txtSuffix.Size = new System.Drawing.Size(136, 20);
            this.txtSuffix.TabIndex = 7;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(13, 79);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(46, 13);
            this.labelControl3.TabIndex = 8;
            this.labelControl3.Text = "Start No  ";
            // 
            // txtStartNo
            // 
            this.txtStartNo.EditValue = "0";
            this.txtStartNo.Enabled = false;
            this.txtStartNo.EnterMoveNextControl = true;
            this.txtStartNo.Location = new System.Drawing.Point(59, 76);
            this.txtStartNo.MenuManager = this.barManager1;
            this.txtStartNo.Name = "txtStartNo";
            this.txtStartNo.Properties.LookAndFeel.SkinName = "Money Twins";
            this.txtStartNo.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtStartNo.Properties.Mask.EditMask = "n0";
            this.txtStartNo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtStartNo.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtStartNo.Properties.ValidateOnEnterKey = true;
            this.txtStartNo.Size = new System.Drawing.Size(136, 20);
            this.txtStartNo.TabIndex = 9;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(215, 79);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(34, 13);
            this.labelControl4.TabIndex = 10;
            this.labelControl4.Text = "Width  ";
            // 
            // txtWidth
            // 
            this.txtWidth.EditValue = "eee";
            this.txtWidth.Enabled = false;
            this.txtWidth.EnterMoveNextControl = true;
            this.txtWidth.Location = new System.Drawing.Point(249, 76);
            this.txtWidth.MenuManager = this.barManager1;
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Properties.LookAndFeel.SkinName = "Money Twins";
            this.txtWidth.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtWidth.Properties.Mask.EditMask = "n0";
            this.txtWidth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtWidth.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtWidth.Properties.ValidateOnEnterKey = true;
            this.txtWidth.Size = new System.Drawing.Size(136, 20);
            this.txtWidth.TabIndex = 11;
            // 
            // RGType
            // 
            this.RGType.EditValue = 0;
            this.RGType.EnterMoveNextControl = true;
            this.RGType.Location = new System.Drawing.Point(79, 12);
            this.RGType.MenuManager = this.barManager1;
            this.RGType.Name = "RGType";
            this.RGType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Manual"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Automatic")});
            this.RGType.Properties.LookAndFeel.SkinName = "Glass Oceans";
            this.RGType.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.RGType.Size = new System.Drawing.Size(254, 23);
            this.RGType.TabIndex = 12;
            this.RGType.SelectedIndexChanged += new System.EventHandler(this.RGType_SelectedIndexChanged);
            // 
            // frmCarParkCodeSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 135);
            this.ControlBox = false;
            this.Controls.Add(this.RGType);
            this.Controls.Add(this.txtWidth);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.txtStartNo);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.txtSuffix);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtPrefix);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "frmCarParkCodeSetup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Blockwise - Car Park Code Setup";
            this.Load += new System.EventHandler(this.frmCarParkCodeSetup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrefix.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSuffix.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWidth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RGType.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem btnOk;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraEditors.TextEdit txtStartNo;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtSuffix;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtPrefix;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtWidth;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.RadioGroup RGType;
    }
}