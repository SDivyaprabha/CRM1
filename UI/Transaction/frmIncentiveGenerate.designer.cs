namespace CRM.UI.Transaction
{
    partial class frmIncentiveGenerate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIncentiveGenerate));
            DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule conditionValidationRule3 = new DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule();
            DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule conditionValidationRule1 = new DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.txtNarration = new DevExpress.XtraEditors.TextEdit();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnExit = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.btnOk = new DevExpress.XtraBars.BarButtonItem();
            this.btnCancel = new DevExpress.XtraBars.BarButtonItem();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.baarbtnExit = new DevExpress.XtraBars.BarButtonItem();
            this.grdIG = new DevExpress.XtraGrid.GridControl();
            this.grdIGView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btnGenerate = new DevExpress.XtraEditors.SimpleButton();
            this.txtRefNo = new DevExpress.XtraEditors.TextEdit();
            this.DETo = new DevExpress.XtraEditors.DateEdit();
            this.DEFrom = new DevExpress.XtraEditors.DateEdit();
            this.DEDate = new DevExpress.XtraEditors.DateEdit();
            this.lblTo = new DevExpress.XtraEditors.LabelControl();
            this.lblFrom = new DevExpress.XtraEditors.LabelControl();
            this.lblRefNo = new DevExpress.XtraEditors.LabelControl();
            this.lblDate = new DevExpress.XtraEditors.LabelControl();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNarration.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdIG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdIGView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRefNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DETo.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DETo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DEFrom.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DEFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DEDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DEDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.groupControl2);
            this.panelControl1.Controls.Add(this.grdIG);
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 26);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(519, 376);
            this.panelControl1.TabIndex = 0;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.txtNarration);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl2.Location = new System.Drawing.Point(0, 329);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(519, 47);
            this.groupControl2.TabIndex = 7;
            this.groupControl2.Text = "NARRATION";
            // 
            // txtNarration
            // 
            this.txtNarration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNarration.Location = new System.Drawing.Point(2, 21);
            this.txtNarration.MenuManager = this.barManager1;
            this.txtNarration.Name = "txtNarration";
            this.txtNarration.Properties.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNarration.Properties.Appearance.Options.UseFont = true;
            this.txtNarration.Properties.LookAndFeel.SkinName = "Money Twins";
            this.txtNarration.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtNarration.Size = new System.Drawing.Size(515, 22);
            this.txtNarration.TabIndex = 0;
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
            this.baarbtnExit,
            this.btnOk,
            this.btnCancel,
            this.btnExit});
            this.barManager1.MaxItemId = 4;
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
            this.btnExit.Id = 3;
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnOk, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnCancel, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // btnOk
            // 
            this.btnOk.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnOk.Caption = "Ok";
            this.btnOk.Glyph = ((System.Drawing.Image)(resources.GetObject("btnOk.Glyph")));
            this.btnOk.Id = 1;
            this.btnOk.Name = "btnOk";
            this.btnOk.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOk_ItemClick);
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
            this.barAndDockingController1.LookAndFeel.SkinName = "Money Twins";
            this.barAndDockingController1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.barAndDockingController1.PropertiesBar.AllowLinkLighting = false;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(519, 26);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 402);
            this.barDockControlBottom.Size = new System.Drawing.Size(519, 26);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 26);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 376);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(519, 26);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 376);
            // 
            // baarbtnExit
            // 
            this.baarbtnExit.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.baarbtnExit.Caption = "Exit";
            this.baarbtnExit.Id = 0;
            this.baarbtnExit.Name = "baarbtnExit";
            // 
            // grdIG
            // 
            this.grdIG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdIG.EmbeddedNavigator.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.grdIG.Location = new System.Drawing.Point(0, 94);
            this.grdIG.MainView = this.grdIGView;
            this.grdIG.Name = "grdIG";
            this.grdIG.Size = new System.Drawing.Size(519, 282);
            this.grdIG.TabIndex = 6;
            this.grdIG.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdIGView});
            // 
            // grdIGView
            // 
            this.grdIGView.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdIGView.Appearance.ColumnFilterButton.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.grdIGView.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdIGView.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.Black;
            this.grdIGView.Appearance.ColumnFilterButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.grdIGView.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.grdIGView.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.grdIGView.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.grdIGView.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            this.grdIGView.Appearance.ColumnFilterButtonActive.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(190)))), ((int)(((byte)(243)))));
            this.grdIGView.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            this.grdIGView.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.grdIGView.Appearance.ColumnFilterButtonActive.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.grdIGView.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.grdIGView.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.grdIGView.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.grdIGView.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.grdIGView.Appearance.Empty.Options.UseBackColor = true;
            this.grdIGView.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            this.grdIGView.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.grdIGView.Appearance.EvenRow.Options.UseBackColor = true;
            this.grdIGView.Appearance.EvenRow.Options.UseForeColor = true;
            this.grdIGView.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdIGView.Appearance.FilterCloseButton.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.grdIGView.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdIGView.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.Black;
            this.grdIGView.Appearance.FilterCloseButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.grdIGView.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.grdIGView.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.grdIGView.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.grdIGView.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(109)))), ((int)(((byte)(185)))));
            this.grdIGView.Appearance.FilterPanel.ForeColor = System.Drawing.Color.White;
            this.grdIGView.Appearance.FilterPanel.Options.UseBackColor = true;
            this.grdIGView.Appearance.FilterPanel.Options.UseForeColor = true;
            this.grdIGView.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.grdIGView.Appearance.FixedLine.Options.UseBackColor = true;
            this.grdIGView.Appearance.FocusedCell.BackColor = System.Drawing.Color.White;
            this.grdIGView.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.grdIGView.Appearance.FocusedCell.Options.UseBackColor = true;
            this.grdIGView.Appearance.FocusedCell.Options.UseForeColor = true;
            this.grdIGView.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(106)))), ((int)(((byte)(197)))));
            this.grdIGView.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.grdIGView.Appearance.FocusedRow.Options.UseBackColor = true;
            this.grdIGView.Appearance.FocusedRow.Options.UseForeColor = true;
            this.grdIGView.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdIGView.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.grdIGView.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdIGView.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.grdIGView.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.grdIGView.Appearance.FooterPanel.Options.UseBackColor = true;
            this.grdIGView.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.grdIGView.Appearance.FooterPanel.Options.UseForeColor = true;
            this.grdIGView.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.grdIGView.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.grdIGView.Appearance.GroupButton.ForeColor = System.Drawing.Color.Black;
            this.grdIGView.Appearance.GroupButton.Options.UseBackColor = true;
            this.grdIGView.Appearance.GroupButton.Options.UseBorderColor = true;
            this.grdIGView.Appearance.GroupButton.Options.UseForeColor = true;
            this.grdIGView.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.grdIGView.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.grdIGView.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.grdIGView.Appearance.GroupFooter.Options.UseBackColor = true;
            this.grdIGView.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.grdIGView.Appearance.GroupFooter.Options.UseForeColor = true;
            this.grdIGView.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(109)))), ((int)(((byte)(185)))));
            this.grdIGView.Appearance.GroupPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdIGView.Appearance.GroupPanel.Options.UseBackColor = true;
            this.grdIGView.Appearance.GroupPanel.Options.UseForeColor = true;
            this.grdIGView.Appearance.GroupRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.grdIGView.Appearance.GroupRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.grdIGView.Appearance.GroupRow.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.grdIGView.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.grdIGView.Appearance.GroupRow.Options.UseBackColor = true;
            this.grdIGView.Appearance.GroupRow.Options.UseBorderColor = true;
            this.grdIGView.Appearance.GroupRow.Options.UseFont = true;
            this.grdIGView.Appearance.GroupRow.Options.UseForeColor = true;
            this.grdIGView.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdIGView.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.grdIGView.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdIGView.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.grdIGView.Appearance.HeaderPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.grdIGView.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.grdIGView.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.grdIGView.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.grdIGView.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(153)))), ((int)(((byte)(228)))));
            this.grdIGView.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(251)))));
            this.grdIGView.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.grdIGView.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.grdIGView.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.grdIGView.Appearance.HorzLine.Options.UseBackColor = true;
            this.grdIGView.Appearance.OddRow.BackColor = System.Drawing.Color.White;
            this.grdIGView.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.grdIGView.Appearance.OddRow.Options.UseBackColor = true;
            this.grdIGView.Appearance.OddRow.Options.UseForeColor = true;
            this.grdIGView.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(252)))), ((int)(((byte)(255)))));
            this.grdIGView.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(129)))), ((int)(((byte)(185)))));
            this.grdIGView.Appearance.Preview.Options.UseBackColor = true;
            this.grdIGView.Appearance.Preview.Options.UseForeColor = true;
            this.grdIGView.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.grdIGView.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.grdIGView.Appearance.Row.Options.UseBackColor = true;
            this.grdIGView.Appearance.Row.Options.UseForeColor = true;
            this.grdIGView.Appearance.RowSeparator.BackColor = System.Drawing.Color.White;
            this.grdIGView.Appearance.RowSeparator.Options.UseBackColor = true;
            this.grdIGView.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(126)))), ((int)(((byte)(217)))));
            this.grdIGView.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.grdIGView.Appearance.SelectedRow.Options.UseBackColor = true;
            this.grdIGView.Appearance.SelectedRow.Options.UseForeColor = true;
            this.grdIGView.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.grdIGView.Appearance.VertLine.Options.UseBackColor = true;
            this.grdIGView.ColumnPanelRowHeight = 30;
            this.grdIGView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.grdIGView.GridControl = this.grdIG;
            this.grdIGView.IndicatorWidth = 60;
            this.grdIGView.Name = "grdIGView";
            this.grdIGView.OptionsBehavior.AllowIncrementalSearch = true;
            this.grdIGView.OptionsBehavior.Editable = false;
            this.grdIGView.OptionsBehavior.ReadOnly = true;
            this.grdIGView.OptionsCustomization.AllowColumnMoving = false;
            this.grdIGView.OptionsCustomization.AllowColumnResizing = false;
            this.grdIGView.OptionsCustomization.AllowFilter = false;
            this.grdIGView.OptionsCustomization.AllowRowSizing = true;
            this.grdIGView.OptionsCustomization.AllowSort = false;
            this.grdIGView.OptionsNavigation.AutoFocusNewRow = true;
            this.grdIGView.OptionsNavigation.EnterMoveNextColumn = true;
            this.grdIGView.OptionsSelection.InvertSelection = true;
            this.grdIGView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.grdIGView.OptionsView.ShowFooter = true;
            this.grdIGView.OptionsView.ShowGroupPanel = false;
            this.grdIGView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.grdIGView_CustomDrawRowIndicator);
            this.grdIGView.CustomDrawFooterCell += new DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventHandler(this.grdIGView_CustomDrawFooterCell);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.btnGenerate);
            this.groupControl1.Controls.Add(this.txtRefNo);
            this.groupControl1.Controls.Add(this.DETo);
            this.groupControl1.Controls.Add(this.DEFrom);
            this.groupControl1.Controls.Add(this.DEDate);
            this.groupControl1.Controls.Add(this.lblTo);
            this.groupControl1.Controls.Add(this.lblFrom);
            this.groupControl1.Controls.Add(this.lblRefNo);
            this.groupControl1.Controls.Add(this.lblDate);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(519, 94);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "GENERATE INCENTIVE";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerate.Appearance.Options.UseFont = true;
            this.btnGenerate.Location = new System.Drawing.Point(430, 60);
            this.btnGenerate.LookAndFeel.SkinName = "iMaginary";
            this.btnGenerate.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // txtRefNo
            // 
            this.txtRefNo.Location = new System.Drawing.Point(295, 32);
            this.txtRefNo.Name = "txtRefNo";
            this.txtRefNo.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtRefNo.Properties.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRefNo.Properties.Appearance.Options.UseBackColor = true;
            this.txtRefNo.Properties.Appearance.Options.UseFont = true;
            this.txtRefNo.Properties.LookAndFeel.SkinName = "Money Twins";
            this.txtRefNo.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtRefNo.Properties.ReadOnly = true;
            this.txtRefNo.Size = new System.Drawing.Size(120, 22);
            this.txtRefNo.TabIndex = 1;
            // 
            // DETo
            // 
            this.DETo.EditValue = null;
            this.dxValidationProvider1.SetIconAlignment(this.DETo, System.Windows.Forms.ErrorIconAlignment.MiddleRight);
            this.DETo.Location = new System.Drawing.Point(295, 60);
            this.DETo.Name = "DETo";
            this.DETo.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.DETo.Properties.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DETo.Properties.Appearance.Options.UseBackColor = true;
            this.DETo.Properties.Appearance.Options.UseFont = true;
            this.DETo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DETo.Properties.DisplayFormat.FormatString = "MMM yyyy";
            this.DETo.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.DETo.Properties.EditFormat.FormatString = "MMM yyyy";
            this.DETo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.DETo.Properties.LookAndFeel.SkinName = "Money Twins";
            this.DETo.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.DETo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.DETo.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.DETo.Size = new System.Drawing.Size(120, 22);
            this.DETo.TabIndex = 3;
            conditionValidationRule3.ConditionOperator = DevExpress.XtraEditors.DXErrorProvider.ConditionOperator.IsNotBlank;
            conditionValidationRule3.ErrorText = "Enter To Date";
            conditionValidationRule3.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
            this.dxValidationProvider1.SetValidationRule(this.DETo, conditionValidationRule3);
            // 
            // DEFrom
            // 
            this.DEFrom.EditValue = null;
            this.dxValidationProvider1.SetIconAlignment(this.DEFrom, System.Windows.Forms.ErrorIconAlignment.MiddleRight);
            this.DEFrom.Location = new System.Drawing.Point(64, 61);
            this.DEFrom.Name = "DEFrom";
            this.DEFrom.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.DEFrom.Properties.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DEFrom.Properties.Appearance.Options.UseBackColor = true;
            this.DEFrom.Properties.Appearance.Options.UseFont = true;
            this.DEFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DEFrom.Properties.DisplayFormat.FormatString = "MMM yyyy";
            this.DEFrom.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.DEFrom.Properties.EditFormat.FormatString = "MMM yyyy";
            this.DEFrom.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.DEFrom.Properties.LookAndFeel.SkinName = "Money Twins";
            this.DEFrom.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.DEFrom.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.DEFrom.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.DEFrom.Size = new System.Drawing.Size(120, 22);
            this.DEFrom.TabIndex = 2;
            conditionValidationRule1.ConditionOperator = DevExpress.XtraEditors.DXErrorProvider.ConditionOperator.IsNotBlank;
            conditionValidationRule1.ErrorText = "Enter From Date";
            conditionValidationRule1.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
            this.dxValidationProvider1.SetValidationRule(this.DEFrom, conditionValidationRule1);
            // 
            // DEDate
            // 
            this.DEDate.EditValue = new System.DateTime(2011, 10, 11, 16, 35, 4, 0);
            this.DEDate.Location = new System.Drawing.Point(64, 32);
            this.DEDate.Name = "DEDate";
            this.DEDate.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.DEDate.Properties.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DEDate.Properties.Appearance.Options.UseBackColor = true;
            this.DEDate.Properties.Appearance.Options.UseFont = true;
            this.DEDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DEDate.Properties.LookAndFeel.SkinName = "Money Twins";
            this.DEDate.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.DEDate.Properties.ReadOnly = true;
            this.DEDate.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.DEDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.DEDate.Size = new System.Drawing.Size(120, 22);
            this.DEDate.TabIndex = 0;
            // 
            // lblTo
            // 
            this.lblTo.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTo.Location = new System.Drawing.Point(207, 65);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(13, 15);
            this.lblTo.TabIndex = 3;
            this.lblTo.Text = "To";
            // 
            // lblFrom
            // 
            this.lblFrom.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrom.Location = new System.Drawing.Point(19, 64);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(28, 15);
            this.lblFrom.TabIndex = 2;
            this.lblFrom.Text = "From";
            // 
            // lblRefNo
            // 
            this.lblRefNo.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRefNo.Location = new System.Drawing.Point(207, 35);
            this.lblRefNo.Name = "lblRefNo";
            this.lblRefNo.Size = new System.Drawing.Size(71, 15);
            this.lblRefNo.TabIndex = 1;
            this.lblRefNo.Text = "Reference No";
            // 
            // lblDate
            // 
            this.lblDate.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(19, 36);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(25, 15);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "Date";
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Blue";
            // 
            // dxValidationProvider1
            // 
            this.dxValidationProvider1.ValidationMode = DevExpress.XtraEditors.DXErrorProvider.ValidationMode.Auto;
            // 
            // frmIncentiveGenerate
            // 
            this.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 428);
            this.ControlBox = false;
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "frmIncentiveGenerate";
            this.Text = "Incentive";
            this.Load += new System.EventHandler(this.frmIncentiveGenerate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtNarration.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdIG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdIGView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRefNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DETo.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DETo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DEFrom.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DEFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DEDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DEDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraGrid.GridControl grdIG;
        private DevExpress.XtraGrid.Views.Grid.GridView grdIGView;
        private DevExpress.XtraEditors.LabelControl lblTo;
        private DevExpress.XtraEditors.LabelControl lblFrom;
        private DevExpress.XtraEditors.LabelControl lblRefNo;
        private DevExpress.XtraEditors.LabelControl lblDate;
        private DevExpress.XtraEditors.DateEdit DEDate;
        private DevExpress.XtraEditors.DateEdit DETo;
        private DevExpress.XtraEditors.DateEdit DEFrom;
        private DevExpress.XtraEditors.TextEdit txtRefNo;
        private DevExpress.XtraEditors.SimpleButton btnGenerate;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem baarbtnExit;
        private DevExpress.XtraBars.BarButtonItem btnOk;
        private DevExpress.XtraBars.BarButtonItem btnCancel;
        private DevExpress.XtraBars.BarButtonItem btnExit;
        private DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider dxValidationProvider1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.TextEdit txtNarration;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
    }
}