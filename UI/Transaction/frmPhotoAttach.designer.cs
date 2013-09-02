namespace CRM
{
    partial class frmPhotoAttach
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPhotoAttach));
            this.Panel1 = new System.Windows.Forms.Panel();
            this.PicDoc = new System.Windows.Forms.PictureBox();
            this.BarAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.BarButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.BarButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.BarButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.OpenDlg = new System.Windows.Forms.OpenFileDialog();
            this.BarManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.txtDesc = new DevExpress.XtraEditors.MemoEdit();
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicDoc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.PicDoc);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel1.Location = new System.Drawing.Point(0, 0);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(438, 211);
            this.Panel1.TabIndex = 4;
            // 
            // PicDoc
            // 
            this.PicDoc.BackColor = System.Drawing.Color.White;
            this.PicDoc.Location = new System.Drawing.Point(0, 0);
            this.PicDoc.Name = "PicDoc";
            this.PicDoc.Size = new System.Drawing.Size(438, 295);
            this.PicDoc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PicDoc.TabIndex = 4;
            this.PicDoc.TabStop = false;
            // 
            // BarAndDockingController1
            // 
            this.BarAndDockingController1.LookAndFeel.SkinName = "Money Twins";
            this.BarAndDockingController1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.BarAndDockingController1.PropertiesBar.AllowLinkLighting = false;
            // 
            // BarButtonItem3
            // 
            this.BarButtonItem3.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.BarButtonItem3.Caption = "Exit";
            this.BarButtonItem3.Glyph = ((System.Drawing.Image)(resources.GetObject("BarButtonItem3.Glyph")));
            this.BarButtonItem3.Hint = "Exit";
            this.BarButtonItem3.Id = 2;
            this.BarButtonItem3.Name = "BarButtonItem3";
            this.BarButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarButtonItem3_ItemClick);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 295);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 295);
            this.barDockControlBottom.Size = new System.Drawing.Size(438, 26);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(438, 0);
            // 
            // BarButtonItem2
            // 
            this.BarButtonItem2.Caption = "Save";
            this.BarButtonItem2.Glyph = ((System.Drawing.Image)(resources.GetObject("BarButtonItem2.Glyph")));
            this.BarButtonItem2.Id = 1;
            this.BarButtonItem2.Name = "BarButtonItem2";
            // 
            // BarButtonItem1
            // 
            this.BarButtonItem1.Caption = "Add";
            this.BarButtonItem1.Glyph = ((System.Drawing.Image)(resources.GetObject("BarButtonItem1.Glyph")));
            this.BarButtonItem1.Id = 0;
            this.BarButtonItem1.Name = "BarButtonItem1";
            // 
            // OpenDlg
            // 
            this.OpenDlg.FileName = "OpenFileDialog1";
            // 
            // BarManager1
            // 
            this.BarManager1.AllowCustomization = false;
            this.BarManager1.AllowQuickCustomization = false;
            this.BarManager1.AllowShowToolbarsPopup = false;
            this.BarManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2});
            this.BarManager1.Controller = this.BarAndDockingController1;
            this.BarManager1.DockControls.Add(this.barDockControlTop);
            this.BarManager1.DockControls.Add(this.barDockControlBottom);
            this.BarManager1.DockControls.Add(this.barDockControlLeft);
            this.BarManager1.DockControls.Add(this.barDockControlRight);
            this.BarManager1.Form = this;
            this.BarManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.BarButtonItem1,
            this.BarButtonItem2,
            this.BarButtonItem3,
            this.barButtonItem4,
            this.barButtonItem5,
            this.barButtonItem6});
            this.BarManager1.MaxItemId = 7;
            this.BarManager1.StatusBar = this.bar2;
            // 
            // bar2
            // 
            this.bar2.BarName = "Custom 3";
            this.bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem6, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem4, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem5, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Custom 3";
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "Add";
            this.barButtonItem6.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem6.Glyph")));
            this.barButtonItem6.Id = 5;
            this.barButtonItem6.Name = "barButtonItem6";
            this.barButtonItem6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem6_ItemClick);
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barButtonItem4.Caption = "OK";
            this.barButtonItem4.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem4.Glyph")));
            this.barButtonItem4.Id = 3;
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem4_ItemClick);
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barButtonItem5.Caption = "Cancel";
            this.barButtonItem5.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem5.Glyph")));
            this.barButtonItem5.Id = 4;
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem5_ItemClick);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(438, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 295);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.txtDesc);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl1.Location = new System.Drawing.Point(0, 211);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(438, 84);
            this.groupControl1.TabIndex = 5;
            this.groupControl1.Text = "Description";
            // 
            // txtDesc
            // 
            this.txtDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDesc.Location = new System.Drawing.Point(2, 22);
            this.txtDesc.MenuManager = this.BarManager1;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(434, 60);
            this.txtDesc.TabIndex = 0;
            // 
            // frmPhotoAttach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 321);
            this.ControlBox = false;
            this.Controls.Add(this.Panel1);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmPhotoAttach";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Photo";
            this.Load += new System.EventHandler(this.frmPhotoAttach_Load);
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicDoc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.PictureBox PicDoc;
        internal DevExpress.XtraBars.BarAndDockingController BarAndDockingController1;
        internal DevExpress.XtraBars.BarButtonItem BarButtonItem3;
        internal DevExpress.XtraBars.BarDockControl barDockControlLeft;
        internal DevExpress.XtraBars.BarDockControl barDockControlBottom;
        internal DevExpress.XtraBars.BarDockControl barDockControlTop;
        internal DevExpress.XtraBars.BarButtonItem BarButtonItem2;
        internal DevExpress.XtraBars.BarButtonItem BarButtonItem1;
        internal System.Windows.Forms.OpenFileDialog OpenDlg;
        internal DevExpress.XtraBars.BarManager BarManager1;
        internal DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.MemoEdit txtDesc;
    }
}