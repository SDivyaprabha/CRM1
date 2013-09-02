namespace CRM
{
    partial class frmApplyOthers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmApplyOthers));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.grdFlatCheckList = new DevExpress.XtraGrid.GridControl();
            this.grdFlatCheckListView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.btnOk = new DevExpress.XtraBars.BarButtonItem();
            this.btnExit = new DevExpress.XtraBars.BarButtonItem();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.cboCheckList = new DevExpress.XtraEditors.LookUpEdit();
            this.lblCheckList = new DevExpress.XtraEditors.LabelControl();
            this.cboFlatType = new DevExpress.XtraEditors.LookUpEdit();
            this.lblFlatType = new DevExpress.XtraEditors.LabelControl();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFlatCheckList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdFlatCheckListView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboCheckList.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboFlatType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.grdFlatCheckList);
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "Money Twins";
            this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(589, 334);
            this.panelControl1.TabIndex = 0;
            // 
            // grdFlatCheckList
            // 
            this.grdFlatCheckList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdFlatCheckList.Location = new System.Drawing.Point(0, 35);
            this.grdFlatCheckList.LookAndFeel.SkinName = "Blue";
            this.grdFlatCheckList.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdFlatCheckList.MainView = this.grdFlatCheckListView;
            this.grdFlatCheckList.MenuManager = this.barManager1;
            this.grdFlatCheckList.Name = "grdFlatCheckList";
            this.grdFlatCheckList.Size = new System.Drawing.Size(589, 299);
            this.grdFlatCheckList.TabIndex = 0;
            this.grdFlatCheckList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdFlatCheckListView});
            // 
            // grdFlatCheckListView
            // 
            this.grdFlatCheckListView.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdFlatCheckListView.Appearance.ColumnFilterButton.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.grdFlatCheckListView.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdFlatCheckListView.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.Black;
            this.grdFlatCheckListView.Appearance.ColumnFilterButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.grdFlatCheckListView.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.grdFlatCheckListView.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.grdFlatCheckListView.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            this.grdFlatCheckListView.Appearance.ColumnFilterButtonActive.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(190)))), ((int)(((byte)(243)))));
            this.grdFlatCheckListView.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            this.grdFlatCheckListView.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.grdFlatCheckListView.Appearance.ColumnFilterButtonActive.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.grdFlatCheckListView.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.grdFlatCheckListView.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.grdFlatCheckListView.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.grdFlatCheckListView.Appearance.Empty.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            this.grdFlatCheckListView.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.grdFlatCheckListView.Appearance.EvenRow.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.EvenRow.Options.UseForeColor = true;
            this.grdFlatCheckListView.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdFlatCheckListView.Appearance.FilterCloseButton.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.grdFlatCheckListView.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdFlatCheckListView.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.Black;
            this.grdFlatCheckListView.Appearance.FilterCloseButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.grdFlatCheckListView.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.grdFlatCheckListView.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.grdFlatCheckListView.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(109)))), ((int)(((byte)(185)))));
            this.grdFlatCheckListView.Appearance.FilterPanel.ForeColor = System.Drawing.Color.White;
            this.grdFlatCheckListView.Appearance.FilterPanel.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.FilterPanel.Options.UseForeColor = true;
            this.grdFlatCheckListView.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.grdFlatCheckListView.Appearance.FixedLine.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.FocusedCell.BackColor = System.Drawing.Color.White;
            this.grdFlatCheckListView.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.grdFlatCheckListView.Appearance.FocusedCell.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.FocusedCell.Options.UseForeColor = true;
            this.grdFlatCheckListView.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(106)))), ((int)(((byte)(197)))));
            this.grdFlatCheckListView.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.grdFlatCheckListView.Appearance.FocusedRow.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.FocusedRow.Options.UseForeColor = true;
            this.grdFlatCheckListView.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdFlatCheckListView.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.grdFlatCheckListView.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdFlatCheckListView.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.grdFlatCheckListView.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.grdFlatCheckListView.Appearance.FooterPanel.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.grdFlatCheckListView.Appearance.FooterPanel.Options.UseForeColor = true;
            this.grdFlatCheckListView.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.grdFlatCheckListView.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.grdFlatCheckListView.Appearance.GroupButton.ForeColor = System.Drawing.Color.Black;
            this.grdFlatCheckListView.Appearance.GroupButton.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.GroupButton.Options.UseBorderColor = true;
            this.grdFlatCheckListView.Appearance.GroupButton.Options.UseForeColor = true;
            this.grdFlatCheckListView.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.grdFlatCheckListView.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.grdFlatCheckListView.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.grdFlatCheckListView.Appearance.GroupFooter.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.grdFlatCheckListView.Appearance.GroupFooter.Options.UseForeColor = true;
            this.grdFlatCheckListView.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(109)))), ((int)(((byte)(185)))));
            this.grdFlatCheckListView.Appearance.GroupPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdFlatCheckListView.Appearance.GroupPanel.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.GroupPanel.Options.UseForeColor = true;
            this.grdFlatCheckListView.Appearance.GroupRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.grdFlatCheckListView.Appearance.GroupRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.grdFlatCheckListView.Appearance.GroupRow.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.grdFlatCheckListView.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.grdFlatCheckListView.Appearance.GroupRow.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.GroupRow.Options.UseBorderColor = true;
            this.grdFlatCheckListView.Appearance.GroupRow.Options.UseFont = true;
            this.grdFlatCheckListView.Appearance.GroupRow.Options.UseForeColor = true;
            this.grdFlatCheckListView.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdFlatCheckListView.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.grdFlatCheckListView.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdFlatCheckListView.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.grdFlatCheckListView.Appearance.HeaderPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.grdFlatCheckListView.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.grdFlatCheckListView.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.grdFlatCheckListView.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(153)))), ((int)(((byte)(228)))));
            this.grdFlatCheckListView.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(251)))));
            this.grdFlatCheckListView.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.grdFlatCheckListView.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.grdFlatCheckListView.Appearance.HorzLine.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.OddRow.BackColor = System.Drawing.Color.White;
            this.grdFlatCheckListView.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.grdFlatCheckListView.Appearance.OddRow.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.OddRow.Options.UseForeColor = true;
            this.grdFlatCheckListView.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(252)))), ((int)(((byte)(255)))));
            this.grdFlatCheckListView.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(129)))), ((int)(((byte)(185)))));
            this.grdFlatCheckListView.Appearance.Preview.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.Preview.Options.UseForeColor = true;
            this.grdFlatCheckListView.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.grdFlatCheckListView.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.grdFlatCheckListView.Appearance.Row.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.Row.Options.UseForeColor = true;
            this.grdFlatCheckListView.Appearance.RowSeparator.BackColor = System.Drawing.Color.White;
            this.grdFlatCheckListView.Appearance.RowSeparator.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(126)))), ((int)(((byte)(217)))));
            this.grdFlatCheckListView.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.grdFlatCheckListView.Appearance.SelectedRow.Options.UseBackColor = true;
            this.grdFlatCheckListView.Appearance.SelectedRow.Options.UseForeColor = true;
            this.grdFlatCheckListView.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.grdFlatCheckListView.Appearance.VertLine.Options.UseBackColor = true;
            this.grdFlatCheckListView.GridControl = this.grdFlatCheckList;
            this.grdFlatCheckListView.Name = "grdFlatCheckListView";
            this.grdFlatCheckListView.OptionsCustomization.AllowColumnMoving = false;
            this.grdFlatCheckListView.OptionsCustomization.AllowFilter = false;
            this.grdFlatCheckListView.OptionsCustomization.AllowSort = false;
            this.grdFlatCheckListView.OptionsNavigation.EnterMoveNextColumn = true;
            this.grdFlatCheckListView.OptionsPrint.UsePrintStyles = true;
            this.grdFlatCheckListView.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.grdFlatCheckListView.OptionsView.EnableAppearanceEvenRow = true;
            this.grdFlatCheckListView.OptionsView.EnableAppearanceOddRow = true;
            this.grdFlatCheckListView.OptionsView.ShowGroupPanel = false;
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
            this.btnExit,
            this.btnOk});
            this.barManager1.MaxItemId = 2;
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnOk, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnExit, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // btnOk
            // 
            this.btnOk.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnOk.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnOk.Appearance.Options.UseFont = true;
            this.btnOk.Caption = "Ok";
            this.btnOk.Glyph = ((System.Drawing.Image)(resources.GetObject("btnOk.Glyph")));
            this.btnOk.Id = 1;
            this.btnOk.Name = "btnOk";
            this.btnOk.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOk_ItemClick);
            // 
            // btnExit
            // 
            this.btnExit.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnExit.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnExit.Appearance.Options.UseFont = true;
            this.btnExit.Caption = "Cancel";
            this.btnExit.Glyph = ((System.Drawing.Image)(resources.GetObject("btnExit.Glyph")));
            this.btnExit.Id = 0;
            this.btnExit.Name = "btnExit";
            this.btnExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExit_ItemClick);
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
            this.barDockControlTop.Size = new System.Drawing.Size(589, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 334);
            this.barDockControlBottom.Size = new System.Drawing.Size(589, 26);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 334);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(589, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 334);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.cboCheckList);
            this.panelControl2.Controls.Add(this.lblCheckList);
            this.panelControl2.Controls.Add(this.cboFlatType);
            this.panelControl2.Controls.Add(this.lblFlatType);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.LookAndFeel.SkinName = "Money Twins";
            this.panelControl2.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(589, 35);
            this.panelControl2.TabIndex = 1;
            // 
            // cboCheckList
            // 
            this.cboCheckList.Location = new System.Drawing.Point(381, 7);
            this.cboCheckList.MenuManager = this.barManager1;
            this.cboCheckList.Name = "cboCheckList";
            this.cboCheckList.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.cboCheckList.Properties.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCheckList.Properties.Appearance.Options.UseBackColor = true;
            this.cboCheckList.Properties.Appearance.Options.UseFont = true;
            this.cboCheckList.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboCheckList.Properties.LookAndFeel.SkinName = "Money Twins";
            this.cboCheckList.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.cboCheckList.Properties.NullText = "None";
            this.cboCheckList.Size = new System.Drawing.Size(162, 22);
            this.cboCheckList.TabIndex = 7;
            this.cboCheckList.EditValueChanged += new System.EventHandler(this.cboCheckList_EditValueChanged);
            // 
            // lblCheckList
            // 
            this.lblCheckList.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCheckList.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lblCheckList.Location = new System.Drawing.Point(299, 12);
            this.lblCheckList.Name = "lblCheckList";
            this.lblCheckList.Size = new System.Drawing.Size(54, 15);
            this.lblCheckList.TabIndex = 6;
            this.lblCheckList.Text = "Check List";
            // 
            // cboFlatType
            // 
            this.cboFlatType.Location = new System.Drawing.Point(109, 7);
            this.cboFlatType.MenuManager = this.barManager1;
            this.cboFlatType.Name = "cboFlatType";
            this.cboFlatType.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.cboFlatType.Properties.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboFlatType.Properties.Appearance.Options.UseBackColor = true;
            this.cboFlatType.Properties.Appearance.Options.UseFont = true;
            this.cboFlatType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboFlatType.Properties.LookAndFeel.SkinName = "Money Twins";
            this.cboFlatType.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.cboFlatType.Properties.NullText = "None";
            this.cboFlatType.Size = new System.Drawing.Size(162, 22);
            this.cboFlatType.TabIndex = 5;
            this.cboFlatType.EditValueChanged += new System.EventHandler(this.cboFlatType_EditValueChanged);
            // 
            // lblFlatType
            // 
            this.lblFlatType.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFlatType.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lblFlatType.Location = new System.Drawing.Point(32, 12);
            this.lblFlatType.Name = "lblFlatType";
            this.lblFlatType.Size = new System.Drawing.Size(49, 15);
            this.lblFlatType.TabIndex = 0;
            this.lblFlatType.Text = "Flat Type";
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Money Twins";
            // 
            // frmApplyOthers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 360);
            this.ControlBox = false;
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmApplyOthers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Check List Updates";
            this.Load += new System.EventHandler(this.frmApplyOthers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdFlatCheckList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdFlatCheckListView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboCheckList.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboFlatType.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl grdFlatCheckList;
        private DevExpress.XtraGrid.Views.Grid.GridView grdFlatCheckListView;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarButtonItem btnOk;
        private DevExpress.XtraBars.BarButtonItem btnExit;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.LabelControl lblFlatType;
        private DevExpress.XtraEditors.LookUpEdit cboCheckList;
        private DevExpress.XtraEditors.LabelControl lblCheckList;
        private DevExpress.XtraEditors.LookUpEdit cboFlatType;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
    }
}