namespace CRM
{
    partial class frmCashEntry
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.btnOK = new DevExpress.XtraBars.BarButtonItem();
            this.btnCancel = new DevExpress.XtraBars.BarButtonItem();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnExit = new DevExpress.XtraBars.BarButtonItem();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtAmt = new DevExpress.XtraEditors.TextEdit();
            this.cboBuyer = new DevExpress.XtraEditors.LookUpEdit();
            this.cboProject = new DevExpress.XtraEditors.LookUpEdit();
            this.dEDate = new DevExpress.XtraEditors.DateEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.gridReceipt = new DevExpress.XtraGrid.GridControl();
            this.gridViewReceipt = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboBuyer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProject.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dEDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dEDate.Properties)).BeginInit();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridReceipt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewReceipt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.radioGroup1);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.txtAmt);
            this.panelControl1.Controls.Add(this.cboBuyer);
            this.panelControl1.Controls.Add(this.cboProject);
            this.panelControl1.Controls.Add(this.dEDate);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 26);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(829, 77);
            this.panelControl1.TabIndex = 0;
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(323, 6);
            this.radioGroup1.MenuManager = this.barManager1;
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
            this.radioGroup1.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.radioGroup1.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.radioGroup1.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Progress Bill"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Payment Schedule")});
            this.radioGroup1.Properties.LookAndFeel.SkinName = "Money Twins";
            this.radioGroup1.Size = new System.Drawing.Size(242, 26);
            this.radioGroup1.TabIndex = 21;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar3,
            this.bar1});
            this.barManager1.Controller = this.barAndDockingController1;
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnOK,
            this.btnCancel,
            this.btnExit});
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
            this.btnOK.Glyph = global::CRM.Properties.Resources.Ok_icon;
            this.btnOK.Id = 0;
            this.btnOK.Name = "btnOK";
            this.btnOK.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOK_ItemClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnCancel.Caption = "Cancel";
            this.btnCancel.Glyph = global::CRM.Properties.Resources.Button_Close_icon;
            this.btnCancel.Id = 1;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCancel_ItemClick);
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 3";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnExit)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Custom 3";
            // 
            // btnExit
            // 
            this.btnExit.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnExit.Caption = "Exit";
            this.btnExit.Glyph = global::CRM.Properties.Resources.exit1;
            this.btnExit.Id = 2;
            this.btnExit.Name = "btnExit";
            this.btnExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExit_ItemClick);
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
            this.barDockControlTop.Size = new System.Drawing.Size(829, 26);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 316);
            this.barDockControlBottom.Size = new System.Drawing.Size(829, 26);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 26);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 290);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(829, 26);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 290);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(600, 46);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(24, 13);
            this.labelControl4.TabIndex = 17;
            this.labelControl4.Text = "Cash";
            // 
            // txtAmt
            // 
            this.txtAmt.EnterMoveNextControl = true;
            this.txtAmt.Location = new System.Drawing.Point(648, 44);
            this.txtAmt.MenuManager = this.barManager1;
            this.txtAmt.Name = "txtAmt";
            this.txtAmt.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
            this.txtAmt.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.txtAmt.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtAmt.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtAmt.Properties.DisplayFormat.FormatString = "n2";
            this.txtAmt.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtAmt.Properties.EditFormat.FormatString = "n2";
            this.txtAmt.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtAmt.Properties.Mask.EditMask = "n2";
            this.txtAmt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtAmt.Size = new System.Drawing.Size(145, 20);
            this.txtAmt.TabIndex = 3;
            this.txtAmt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAmt_KeyPress);
            this.txtAmt.Validating += new System.ComponentModel.CancelEventHandler(this.textEdit1_Validating);
            // 
            // cboBuyer
            // 
            this.cboBuyer.EnterMoveNextControl = true;
            this.cboBuyer.Location = new System.Drawing.Point(407, 43);
            this.cboBuyer.MenuManager = this.barManager1;
            this.cboBuyer.Name = "cboBuyer";
            this.cboBuyer.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
            this.cboBuyer.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.cboBuyer.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.cboBuyer.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.cboBuyer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboBuyer.Properties.NullText = "-- Select Buyer Name --";
            this.cboBuyer.Size = new System.Drawing.Size(158, 20);
            this.cboBuyer.TabIndex = 2;
            this.cboBuyer.EditValueChanged += new System.EventHandler(this.cboBuyer_EditValueChanged);
            // 
            // cboProject
            // 
            this.cboProject.EnterMoveNextControl = true;
            this.cboProject.Location = new System.Drawing.Point(132, 43);
            this.cboProject.MenuManager = this.barManager1;
            this.cboProject.Name = "cboProject";
            this.cboProject.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
            this.cboProject.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.cboProject.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.cboProject.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.cboProject.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboProject.Properties.NullText = "-- Select CostCentre --";
            this.cboProject.Size = new System.Drawing.Size(158, 20);
            this.cboProject.TabIndex = 1;
            this.cboProject.EditValueChanged += new System.EventHandler(this.cboProject_EditValueChanged);
            // 
            // dEDate
            // 
            this.dEDate.EditValue = new System.DateTime(2012, 9, 13, 14, 38, 8, 854);
            this.dEDate.Enabled = false;
            this.dEDate.EnterMoveNextControl = true;
            this.dEDate.Location = new System.Drawing.Point(132, 9);
            this.dEDate.MenuManager = this.barManager1;
            this.dEDate.Name = "dEDate";
            this.dEDate.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
            this.dEDate.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.dEDate.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.dEDate.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.dEDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dEDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dEDate.Size = new System.Drawing.Size(129, 20);
            this.dEDate.TabIndex = 0;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(323, 47);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(58, 13);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "Buyer Name";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(25, 47);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(85, 13);
            this.labelControl2.TabIndex = 6;
            this.labelControl2.Text = "CostCentre Name";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(25, 13);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(62, 13);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "Receipt Date";
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.gridReceipt);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(821, 184);
            this.xtraTabPage1.Text = "Payment Schedule";
            // 
            // gridReceipt
            // 
            this.gridReceipt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridReceipt.Location = new System.Drawing.Point(0, 0);
            this.gridReceipt.LookAndFeel.SkinName = "Blue";
            this.gridReceipt.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gridReceipt.MainView = this.gridViewReceipt;
            this.gridReceipt.MenuManager = this.barManager1;
            this.gridReceipt.Name = "gridReceipt";
            this.gridReceipt.Size = new System.Drawing.Size(821, 184);
            this.gridReceipt.TabIndex = 1;
            this.gridReceipt.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewReceipt});
            // 
            // gridViewReceipt
            // 
            this.gridViewReceipt.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewReceipt.GridControl = this.gridReceipt;
            this.gridViewReceipt.Name = "gridViewReceipt";
            this.gridViewReceipt.OptionsBehavior.Editable = false;
            this.gridViewReceipt.OptionsBehavior.ReadOnly = true;
            this.gridViewReceipt.OptionsSelection.InvertSelection = true;
            this.gridViewReceipt.OptionsView.ShowFooter = true;
            this.gridViewReceipt.OptionsView.ShowGroupPanel = false;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(3, 3);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(823, 207);
            this.xtraTabControl1.TabIndex = 11;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1});
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Money Twins";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.xtraTabControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 103);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(829, 213);
            this.panelControl2.TabIndex = 16;
            // 
            // frmCashEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 342);
            this.ControlBox = false;
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmCashEntry";
            this.Text = "Cash Receipt Entry";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCashEntry_FormClosed);
            this.Load += new System.EventHandler(this.frmCashEntry_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboBuyer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProject.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dEDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dEDate.Properties)).EndInit();
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridReceipt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewReceipt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarButtonItem btnOK;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem btnCancel;
        private DevExpress.XtraEditors.LookUpEdit cboBuyer;
        private DevExpress.XtraEditors.LookUpEdit cboProject;
        private DevExpress.XtraEditors.DateEdit dEDate;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraGrid.GridControl gridReceipt;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewReceipt;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtAmt;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem btnExit;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
    }
}