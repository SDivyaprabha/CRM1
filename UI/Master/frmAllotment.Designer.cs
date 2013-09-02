namespace CRM
{
    partial class frmAllotment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAllotment));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.btnCancelChkList = new DevExpress.XtraBars.BarButtonItem();
            this.btnSave = new DevExpress.XtraBars.BarButtonItem();
            this.btnCancel = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.txtBalAmt = new DevExpress.XtraEditors.TextEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.txtRecdAmt = new DevExpress.XtraEditors.TextEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.txtRecvAmt = new DevExpress.XtraEditors.TextEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.txtAmt = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.txtDate = new DevExpress.XtraEditors.DateEdit();
            this.txtBuyerName = new DevExpress.XtraEditors.TextEdit();
            this.txtFlatNo = new DevExpress.XtraEditors.TextEdit();
            this.txtCCName = new DevExpress.XtraEditors.TextEdit();
            this.txtRemarks = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBalAmt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecdAmt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecvAmt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBuyerName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFlatNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCCName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemarks.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
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
            this.btnSave,
            this.btnCancel,
            this.btnCancelChkList});
            this.barManager1.MaxItemId = 3;
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnCancelChkList, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnSave, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnCancel, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // btnCancelChkList
            // 
            this.btnCancelChkList.Caption = "Cancellation CheckList";
            this.btnCancelChkList.Glyph = ((System.Drawing.Image)(resources.GetObject("btnCancelChkList.Glyph")));
            this.btnCancelChkList.Id = 2;
            this.btnCancelChkList.Name = "btnCancelChkList";
            this.btnCancelChkList.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCancelChkList_ItemClick);
            // 
            // btnSave
            // 
            this.btnSave.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnSave.Caption = "OK";
            this.btnSave.Glyph = ((System.Drawing.Image)(resources.GetObject("btnSave.Glyph")));
            this.btnSave.Id = 0;
            this.btnSave.Name = "btnSave";
            this.btnSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSave_ItemClick);
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
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(424, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 302);
            this.barDockControlBottom.Size = new System.Drawing.Size(424, 26);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 302);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(424, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 302);
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Money Twins";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.txtBalAmt);
            this.panelControl1.Controls.Add(this.labelControl9);
            this.panelControl1.Controls.Add(this.txtRecdAmt);
            this.panelControl1.Controls.Add(this.labelControl8);
            this.panelControl1.Controls.Add(this.txtRecvAmt);
            this.panelControl1.Controls.Add(this.labelControl7);
            this.panelControl1.Controls.Add(this.txtAmt);
            this.panelControl1.Controls.Add(this.labelControl6);
            this.panelControl1.Controls.Add(this.txtDate);
            this.panelControl1.Controls.Add(this.txtBuyerName);
            this.panelControl1.Controls.Add(this.txtFlatNo);
            this.panelControl1.Controls.Add(this.txtCCName);
            this.panelControl1.Controls.Add(this.txtRemarks);
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "Money Twins";
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(424, 302);
            this.panelControl1.TabIndex = 14;
            // 
            // txtBalAmt
            // 
            this.txtBalAmt.Location = new System.Drawing.Point(112, 199);
            this.txtBalAmt.Name = "txtBalAmt";
            this.txtBalAmt.Properties.Appearance.Options.UseTextOptions = true;
            this.txtBalAmt.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
            this.txtBalAmt.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.txtBalAmt.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtBalAmt.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtBalAmt.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtBalAmt.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtBalAmt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtBalAmt.Size = new System.Drawing.Size(180, 20);
            this.txtBalAmt.TabIndex = 27;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(11, 201);
            this.labelControl9.LookAndFeel.SkinName = "Money Twins";
            this.labelControl9.LookAndFeel.UseDefaultLookAndFeel = false;
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(77, 13);
            this.labelControl9.TabIndex = 26;
            this.labelControl9.Text = "Balance Amount";
            // 
            // txtRecdAmt
            // 
            this.txtRecdAmt.Location = new System.Drawing.Point(112, 142);
            this.txtRecdAmt.Name = "txtRecdAmt";
            this.txtRecdAmt.Properties.Appearance.Options.UseTextOptions = true;
            this.txtRecdAmt.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
            this.txtRecdAmt.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.txtRecdAmt.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtRecdAmt.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtRecdAmt.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtRecdAmt.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtRecdAmt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtRecdAmt.Size = new System.Drawing.Size(180, 20);
            this.txtRecdAmt.TabIndex = 25;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(11, 144);
            this.labelControl8.LookAndFeel.SkinName = "Money Twins";
            this.labelControl8.LookAndFeel.UseDefaultLookAndFeel = false;
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(84, 13);
            this.labelControl8.TabIndex = 24;
            this.labelControl8.Text = "Received Amount";
            // 
            // txtRecvAmt
            // 
            this.txtRecvAmt.Location = new System.Drawing.Point(112, 114);
            this.txtRecvAmt.Name = "txtRecvAmt";
            this.txtRecvAmt.Properties.Appearance.Options.UseTextOptions = true;
            this.txtRecvAmt.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
            this.txtRecvAmt.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.txtRecvAmt.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtRecvAmt.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtRecvAmt.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtRecvAmt.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtRecvAmt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtRecvAmt.Size = new System.Drawing.Size(180, 20);
            this.txtRecvAmt.TabIndex = 23;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(11, 116);
            this.labelControl7.LookAndFeel.SkinName = "Money Twins";
            this.labelControl7.LookAndFeel.UseDefaultLookAndFeel = false;
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(92, 13);
            this.labelControl7.TabIndex = 22;
            this.labelControl7.Text = "Receivable Amount";
            // 
            // txtAmt
            // 
            this.txtAmt.Location = new System.Drawing.Point(112, 171);
            this.txtAmt.Name = "txtAmt";
            this.txtAmt.Properties.Appearance.Options.UseTextOptions = true;
            this.txtAmt.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
            this.txtAmt.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.txtAmt.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtAmt.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtAmt.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtAmt.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtAmt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtAmt.Size = new System.Drawing.Size(180, 20);
            this.txtAmt.TabIndex = 21;
            this.txtAmt.EditValueChanged += new System.EventHandler(this.txtAmt_EditValueChanged);
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(11, 173);
            this.labelControl6.LookAndFeel.SkinName = "Money Twins";
            this.labelControl6.LookAndFeel.UseDefaultLookAndFeel = false;
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(76, 13);
            this.labelControl6.TabIndex = 20;
            this.labelControl6.Text = "Penalty Amount";
            // 
            // txtDate
            // 
            this.txtDate.EditValue = null;
            this.txtDate.Location = new System.Drawing.Point(112, 86);
            this.txtDate.Name = "txtDate";
            this.txtDate.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
            this.txtDate.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.txtDate.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtDate.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDate.Size = new System.Drawing.Size(180, 20);
            this.txtDate.TabIndex = 19;
            // 
            // txtBuyerName
            // 
            this.txtBuyerName.Location = new System.Drawing.Point(112, 60);
            this.txtBuyerName.Name = "txtBuyerName";
            this.txtBuyerName.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
            this.txtBuyerName.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.txtBuyerName.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtBuyerName.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtBuyerName.Size = new System.Drawing.Size(180, 20);
            this.txtBuyerName.TabIndex = 18;
            // 
            // txtFlatNo
            // 
            this.txtFlatNo.Location = new System.Drawing.Point(112, 34);
            this.txtFlatNo.Name = "txtFlatNo";
            this.txtFlatNo.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
            this.txtFlatNo.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.txtFlatNo.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtFlatNo.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtFlatNo.Size = new System.Drawing.Size(180, 20);
            this.txtFlatNo.TabIndex = 17;
            // 
            // txtCCName
            // 
            this.txtCCName.Location = new System.Drawing.Point(112, 8);
            this.txtCCName.Name = "txtCCName";
            this.txtCCName.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
            this.txtCCName.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.txtCCName.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtCCName.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtCCName.Size = new System.Drawing.Size(180, 20);
            this.txtCCName.TabIndex = 16;
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(111, 229);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
            this.txtRemarks.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.txtRemarks.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtRemarks.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtRemarks.Size = new System.Drawing.Size(300, 62);
            this.txtRemarks.TabIndex = 15;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(10, 231);
            this.labelControl5.LookAndFeel.SkinName = "Money Twins";
            this.labelControl5.LookAndFeel.UseDefaultLookAndFeel = false;
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(41, 13);
            this.labelControl5.TabIndex = 14;
            this.labelControl5.Text = "Remarks";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(11, 88);
            this.labelControl4.LookAndFeel.SkinName = "Money Twins";
            this.labelControl4.LookAndFeel.UseDefaultLookAndFeel = false;
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(58, 13);
            this.labelControl4.TabIndex = 13;
            this.labelControl4.Text = "Cancel Date";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(11, 62);
            this.labelControl3.LookAndFeel.SkinName = "Money Twins";
            this.labelControl3.LookAndFeel.UseDefaultLookAndFeel = false;
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(28, 13);
            this.labelControl3.TabIndex = 12;
            this.labelControl3.Text = "Buyer";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(13, 36);
            this.labelControl2.LookAndFeel.SkinName = "Money Twins";
            this.labelControl2.LookAndFeel.UseDefaultLookAndFeel = false;
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(31, 13);
            this.labelControl2.TabIndex = 11;
            this.labelControl2.Text = "FlatNo";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(13, 10);
            this.labelControl1.LookAndFeel.SkinName = "Money Twins";
            this.labelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(85, 13);
            this.labelControl1.TabIndex = 10;
            this.labelControl1.Text = "CostCentre Name";
            // 
            // barAndDockingController1
            // 
            this.barAndDockingController1.LookAndFeel.SkinName = "Blue";
            this.barAndDockingController1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.barAndDockingController1.PropertiesBar.AllowLinkLighting = false;
            // 
            // frmAllotment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 328);
            this.ControlBox = false;
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmAllotment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Allotment Cancel";
            this.Load += new System.EventHandler(this.frmAllotment_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBalAmt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecdAmt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecvAmt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBuyerName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFlatNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCCName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemarks.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarButtonItem btnSave;
        private DevExpress.XtraBars.BarButtonItem btnCancel;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.DateEdit txtDate;
        private DevExpress.XtraEditors.TextEdit txtBuyerName;
        private DevExpress.XtraEditors.TextEdit txtFlatNo;
        private DevExpress.XtraEditors.TextEdit txtCCName;
        private DevExpress.XtraEditors.MemoEdit txtRemarks;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtAmt;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraBars.BarButtonItem btnCancelChkList;
        private DevExpress.XtraEditors.TextEdit txtBalAmt;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.TextEdit txtRecdAmt;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.TextEdit txtRecvAmt;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
    }
}