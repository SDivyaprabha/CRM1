namespace CRM
{
    partial class frmBankComparision
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBankComparision));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.cboQNo = new DevExpress.XtraEditors.LookUpEdit();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.bntPrint = new DevExpress.XtraBars.BarButtonItem();
            this.btnCancel = new DevExpress.XtraBars.BarButtonItem();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.txtCCName = new DevExpress.XtraEditors.TextEdit();
            this.txtQType = new DevExpress.XtraEditors.TextEdit();
            this.dtpQDate = new DevExpress.XtraEditors.DateEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.grdBankComparision = new DevExpress.XtraGrid.GridControl();
            this.cardBankCompView = new DevExpress.XtraGrid.Views.Card.CardView();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboQNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCCName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpQDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpQDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBankComparision)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardBankCompView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.groupControl1.Appearance.Options.UseBackColor = true;
            this.groupControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.groupControl1.Controls.Add(this.cboQNo);
            this.groupControl1.Controls.Add(this.txtCCName);
            this.groupControl1.Controls.Add(this.txtQType);
            this.groupControl1.Controls.Add(this.dtpQDate);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.LookAndFeel.SkinName = "Blue";
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(709, 72);
            this.groupControl1.TabIndex = 5;
            this.groupControl1.Visible = false;
            // 
            // cboQNo
            // 
            this.cboQNo.Location = new System.Drawing.Point(145, 13);
            this.cboQNo.MenuManager = this.barManager1;
            this.cboQNo.Name = "cboQNo";
            this.cboQNo.Properties.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboQNo.Properties.Appearance.Options.UseFont = true;
            this.cboQNo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboQNo.Properties.LookAndFeel.SkinName = "Money Twins";
            this.cboQNo.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.cboQNo.Properties.NullText = "";
            this.cboQNo.Size = new System.Drawing.Size(148, 22);
            this.cboQNo.TabIndex = 9;
            this.cboQNo.EditValueChanged += new System.EventHandler(this.cboQNo_EditValueChanged);
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.AllowQuickCustomization = false;
            this.barManager1.AllowShowToolbarsPopup = false;
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.Controller = this.barAndDockingController1;
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnCancel,
            this.bntPrint});
            this.barManager1.MaxItemId = 4;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.bntPrint, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnCancel, DevExpress.XtraBars.BarItemPaintStyle.Standard)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // bntPrint
            // 
            this.bntPrint.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.bntPrint.Caption = "Print";
            this.bntPrint.Glyph = ((System.Drawing.Image)(resources.GetObject("bntPrint.Glyph")));
            this.bntPrint.Id = 2;
            this.bntPrint.Name = "bntPrint";
            this.bntPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bntPrint_ItemClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnCancel.Caption = "Cancel";
            this.btnCancel.Glyph = global::CRM.Properties.Resources.exit1;
            this.btnCancel.Id = 1;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCancel_ItemClick);
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
            this.barDockControlTop.Size = new System.Drawing.Size(709, 26);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 434);
            this.barDockControlBottom.Size = new System.Drawing.Size(709, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 26);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 408);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(709, 26);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 408);
            // 
            // txtCCName
            // 
            this.txtCCName.Enabled = false;
            this.txtCCName.Location = new System.Drawing.Point(500, 41);
            this.txtCCName.Name = "txtCCName";
            this.txtCCName.Properties.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCCName.Properties.Appearance.Options.UseFont = true;
            this.txtCCName.Properties.LookAndFeel.SkinName = "Money Twins";
            this.txtCCName.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtCCName.Size = new System.Drawing.Size(159, 22);
            this.txtCCName.TabIndex = 8;
            // 
            // txtQType
            // 
            this.txtQType.Enabled = false;
            this.txtQType.Location = new System.Drawing.Point(145, 41);
            this.txtQType.Name = "txtQType";
            this.txtQType.Properties.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQType.Properties.Appearance.Options.UseFont = true;
            this.txtQType.Properties.LookAndFeel.SkinName = "Money Twins";
            this.txtQType.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtQType.Size = new System.Drawing.Size(148, 22);
            this.txtQType.TabIndex = 7;
            // 
            // dtpQDate
            // 
            this.dtpQDate.EditValue = new System.DateTime(2011, 6, 15, 11, 4, 19, 434);
            this.dtpQDate.Enabled = false;
            this.dtpQDate.Location = new System.Drawing.Point(500, 12);
            this.dtpQDate.Name = "dtpQDate";
            this.dtpQDate.Properties.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpQDate.Properties.Appearance.Options.UseFont = true;
            this.dtpQDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpQDate.Properties.LookAndFeel.SkinName = "Money Twins";
            this.dtpQDate.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.dtpQDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtpQDate.Size = new System.Drawing.Size(159, 22);
            this.dtpQDate.TabIndex = 0;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Location = new System.Drawing.Point(32, 46);
            this.labelControl4.LookAndFeel.SkinName = "Blue";
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(84, 15);
            this.labelControl4.TabIndex = 3;
            this.labelControl4.Text = "Quotation Type";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Location = new System.Drawing.Point(32, 19);
            this.labelControl3.LookAndFeel.SkinName = "Blue";
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(74, 15);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "Quotation No";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Location = new System.Drawing.Point(330, 43);
            this.labelControl2.LookAndFeel.SkinName = "Blue";
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(137, 15);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "CostCentre (Operational)";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(330, 16);
            this.labelControl1.LookAndFeel.SkinName = "Blue";
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(84, 15);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Quotation Date";
            // 
            // grdBankComparision
            // 
            this.grdBankComparision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdBankComparision.Location = new System.Drawing.Point(0, 72);
            this.grdBankComparision.LookAndFeel.SkinName = "Blue";
            this.grdBankComparision.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdBankComparision.MainView = this.cardBankCompView;
            this.grdBankComparision.Name = "grdBankComparision";
            this.grdBankComparision.Size = new System.Drawing.Size(709, 336);
            this.grdBankComparision.TabIndex = 0;
            this.grdBankComparision.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.cardBankCompView});
            // 
            // cardBankCompView
            // 
            this.cardBankCompView.Appearance.CardCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.cardBankCompView.Appearance.CardCaption.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.cardBankCompView.Appearance.CardCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.cardBankCompView.Appearance.CardCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardBankCompView.Appearance.CardCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.cardBankCompView.Appearance.CardCaption.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.cardBankCompView.Appearance.CardCaption.Options.UseBackColor = true;
            this.cardBankCompView.Appearance.CardCaption.Options.UseBorderColor = true;
            this.cardBankCompView.Appearance.CardCaption.Options.UseFont = true;
            this.cardBankCompView.Appearance.CardCaption.Options.UseForeColor = true;
            this.cardBankCompView.Appearance.EmptySpace.BackColor = System.Drawing.Color.White;
            this.cardBankCompView.Appearance.EmptySpace.Options.UseBackColor = true;
            this.cardBankCompView.Appearance.FieldCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            this.cardBankCompView.Appearance.FieldCaption.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardBankCompView.Appearance.FieldCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.cardBankCompView.Appearance.FieldCaption.Options.UseBackColor = true;
            this.cardBankCompView.Appearance.FieldCaption.Options.UseFont = true;
            this.cardBankCompView.Appearance.FieldCaption.Options.UseForeColor = true;
            this.cardBankCompView.Appearance.FieldValue.BackColor = System.Drawing.Color.White;
            this.cardBankCompView.Appearance.FieldValue.ForeColor = System.Drawing.Color.Black;
            this.cardBankCompView.Appearance.FieldValue.Options.UseBackColor = true;
            this.cardBankCompView.Appearance.FieldValue.Options.UseForeColor = true;
            this.cardBankCompView.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.cardBankCompView.Appearance.FilterCloseButton.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.cardBankCompView.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.cardBankCompView.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.Black;
            this.cardBankCompView.Appearance.FilterCloseButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.cardBankCompView.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.cardBankCompView.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.cardBankCompView.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.cardBankCompView.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(109)))), ((int)(((byte)(185)))));
            this.cardBankCompView.Appearance.FilterPanel.ForeColor = System.Drawing.Color.White;
            this.cardBankCompView.Appearance.FilterPanel.Options.UseBackColor = true;
            this.cardBankCompView.Appearance.FilterPanel.Options.UseForeColor = true;
            this.cardBankCompView.Appearance.FocusedCardCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(106)))), ((int)(((byte)(197)))));
            this.cardBankCompView.Appearance.FocusedCardCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(106)))), ((int)(((byte)(197)))));
            this.cardBankCompView.Appearance.FocusedCardCaption.ForeColor = System.Drawing.Color.White;
            this.cardBankCompView.Appearance.FocusedCardCaption.Options.UseBackColor = true;
            this.cardBankCompView.Appearance.FocusedCardCaption.Options.UseBorderColor = true;
            this.cardBankCompView.Appearance.FocusedCardCaption.Options.UseForeColor = true;
            this.cardBankCompView.Appearance.HideSelectionCardCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(153)))), ((int)(((byte)(228)))));
            this.cardBankCompView.Appearance.HideSelectionCardCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(153)))), ((int)(((byte)(228)))));
            this.cardBankCompView.Appearance.HideSelectionCardCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardBankCompView.Appearance.HideSelectionCardCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.cardBankCompView.Appearance.HideSelectionCardCaption.Options.UseBackColor = true;
            this.cardBankCompView.Appearance.HideSelectionCardCaption.Options.UseBorderColor = true;
            this.cardBankCompView.Appearance.HideSelectionCardCaption.Options.UseFont = true;
            this.cardBankCompView.Appearance.HideSelectionCardCaption.Options.UseForeColor = true;
            this.cardBankCompView.Appearance.SelectedCardCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(126)))), ((int)(((byte)(217)))));
            this.cardBankCompView.Appearance.SelectedCardCaption.ForeColor = System.Drawing.Color.White;
            this.cardBankCompView.Appearance.SelectedCardCaption.Options.UseBackColor = true;
            this.cardBankCompView.Appearance.SelectedCardCaption.Options.UseForeColor = true;
            this.cardBankCompView.Appearance.SeparatorLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.cardBankCompView.Appearance.SeparatorLine.Options.UseBackColor = true;
            this.cardBankCompView.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.cardBankCompView.FocusedCardTopFieldIndex = 0;
            this.cardBankCompView.GridControl = this.grdBankComparision;
            this.cardBankCompView.Name = "cardBankCompView";
            this.cardBankCompView.OptionsBehavior.AutoFocusNewCard = true;
            this.cardBankCompView.OptionsBehavior.Editable = false;
            this.cardBankCompView.OptionsBehavior.UseTabKey = true;
            this.cardBankCompView.OptionsView.ShowLines = false;
            this.cardBankCompView.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
            this.cardBankCompView.CustomDrawCardCaption += new DevExpress.XtraGrid.Views.Card.CardCaptionCustomDrawEventHandler(this.cardBankCompView_CustomDrawCardCaption);
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.grdBankComparision);
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 26);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(709, 408);
            this.panelControl1.TabIndex = 16;
            // 
            // frmBankComparision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 434);
            this.ControlBox = false;
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "frmBankComparision";
            this.Text = "Bank Comparision";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmVendorComparision_FormClosed);
            this.Load += new System.EventHandler(this.frmBankComparision_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboQNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCCName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpQDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpQDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBankComparision)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardBankCompView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.TextEdit txtQType;
        private DevExpress.XtraEditors.DateEdit dtpQDate;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.TextEdit txtCCName;
        private DevExpress.XtraBars.BarButtonItem btnCancel;
        private DevExpress.XtraEditors.LookUpEdit cboQNo;
        private DevExpress.XtraGrid.GridControl grdBankComparision;
        private DevExpress.XtraBars.BarButtonItem bntPrint;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.Views.Card.CardView cardBankCompView;
    }
}