namespace CRM.UI.Transaction
{
    partial class frmIncentiveDetails
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
            this.grdIncDet = new DevExpress.XtraGrid.GridControl();
            this.grdIncDetView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnEdit = new DevExpress.XtraBars.BarButtonItem();
            this.btnDelete = new DevExpress.XtraBars.BarButtonItem();
            this.btnPrint = new DevExpress.XtraBars.BarButtonItem();
            this.btnExit = new DevExpress.XtraBars.BarButtonItem();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdIncDet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdIncDetView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.grdIncDet);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 26);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(519, 343);
            this.panelControl1.TabIndex = 0;
            // 
            // grdIncDet
            // 
            this.grdIncDet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdIncDet.Location = new System.Drawing.Point(0, 0);
            this.grdIncDet.MainView = this.grdIncDetView;
            this.grdIncDet.MenuManager = this.barManager1;
            this.grdIncDet.Name = "grdIncDet";
            this.grdIncDet.Size = new System.Drawing.Size(519, 343);
            this.grdIncDet.TabIndex = 0;
            this.grdIncDet.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdIncDetView});
            // 
            // grdIncDetView
            // 
            this.grdIncDetView.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdIncDetView.Appearance.ColumnFilterButton.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.grdIncDetView.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdIncDetView.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.Black;
            this.grdIncDetView.Appearance.ColumnFilterButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.grdIncDetView.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.grdIncDetView.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.grdIncDetView.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            this.grdIncDetView.Appearance.ColumnFilterButtonActive.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(190)))), ((int)(((byte)(243)))));
            this.grdIncDetView.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            this.grdIncDetView.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.grdIncDetView.Appearance.ColumnFilterButtonActive.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.grdIncDetView.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.grdIncDetView.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.grdIncDetView.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.grdIncDetView.Appearance.Empty.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            this.grdIncDetView.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.grdIncDetView.Appearance.EvenRow.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.EvenRow.Options.UseForeColor = true;
            this.grdIncDetView.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdIncDetView.Appearance.FilterCloseButton.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.grdIncDetView.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdIncDetView.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.Black;
            this.grdIncDetView.Appearance.FilterCloseButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.grdIncDetView.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.grdIncDetView.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.grdIncDetView.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(109)))), ((int)(((byte)(185)))));
            this.grdIncDetView.Appearance.FilterPanel.ForeColor = System.Drawing.Color.White;
            this.grdIncDetView.Appearance.FilterPanel.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.FilterPanel.Options.UseForeColor = true;
            this.grdIncDetView.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.grdIncDetView.Appearance.FixedLine.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.FocusedCell.BackColor = System.Drawing.Color.White;
            this.grdIncDetView.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.grdIncDetView.Appearance.FocusedCell.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.FocusedCell.Options.UseForeColor = true;
            this.grdIncDetView.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(106)))), ((int)(((byte)(197)))));
            this.grdIncDetView.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.grdIncDetView.Appearance.FocusedRow.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.FocusedRow.Options.UseForeColor = true;
            this.grdIncDetView.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdIncDetView.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.grdIncDetView.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdIncDetView.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.grdIncDetView.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.grdIncDetView.Appearance.FooterPanel.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.grdIncDetView.Appearance.FooterPanel.Options.UseForeColor = true;
            this.grdIncDetView.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.grdIncDetView.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.grdIncDetView.Appearance.GroupButton.ForeColor = System.Drawing.Color.Black;
            this.grdIncDetView.Appearance.GroupButton.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.GroupButton.Options.UseBorderColor = true;
            this.grdIncDetView.Appearance.GroupButton.Options.UseForeColor = true;
            this.grdIncDetView.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.grdIncDetView.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.grdIncDetView.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.grdIncDetView.Appearance.GroupFooter.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.grdIncDetView.Appearance.GroupFooter.Options.UseForeColor = true;
            this.grdIncDetView.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(109)))), ((int)(((byte)(185)))));
            this.grdIncDetView.Appearance.GroupPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdIncDetView.Appearance.GroupPanel.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.GroupPanel.Options.UseForeColor = true;
            this.grdIncDetView.Appearance.GroupRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.grdIncDetView.Appearance.GroupRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.grdIncDetView.Appearance.GroupRow.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.grdIncDetView.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.grdIncDetView.Appearance.GroupRow.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.GroupRow.Options.UseBorderColor = true;
            this.grdIncDetView.Appearance.GroupRow.Options.UseFont = true;
            this.grdIncDetView.Appearance.GroupRow.Options.UseForeColor = true;
            this.grdIncDetView.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdIncDetView.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.grdIncDetView.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.grdIncDetView.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.grdIncDetView.Appearance.HeaderPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.grdIncDetView.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.grdIncDetView.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.grdIncDetView.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(153)))), ((int)(((byte)(228)))));
            this.grdIncDetView.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(251)))));
            this.grdIncDetView.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.grdIncDetView.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.grdIncDetView.Appearance.HorzLine.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.OddRow.BackColor = System.Drawing.Color.White;
            this.grdIncDetView.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.grdIncDetView.Appearance.OddRow.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.OddRow.Options.UseForeColor = true;
            this.grdIncDetView.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(252)))), ((int)(((byte)(255)))));
            this.grdIncDetView.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(129)))), ((int)(((byte)(185)))));
            this.grdIncDetView.Appearance.Preview.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.Preview.Options.UseForeColor = true;
            this.grdIncDetView.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.grdIncDetView.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.grdIncDetView.Appearance.Row.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.Row.Options.UseForeColor = true;
            this.grdIncDetView.Appearance.RowSeparator.BackColor = System.Drawing.Color.White;
            this.grdIncDetView.Appearance.RowSeparator.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(126)))), ((int)(((byte)(217)))));
            this.grdIncDetView.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.grdIncDetView.Appearance.SelectedRow.Options.UseBackColor = true;
            this.grdIncDetView.Appearance.SelectedRow.Options.UseForeColor = true;
            this.grdIncDetView.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.grdIncDetView.Appearance.VertLine.Options.UseBackColor = true;
            this.grdIncDetView.ColumnPanelRowHeight = 30;
            this.grdIncDetView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.grdIncDetView.GridControl = this.grdIncDet;
            this.grdIncDetView.IndicatorWidth = 60;
            this.grdIncDetView.Name = "grdIncDetView";
            this.grdIncDetView.OptionsBehavior.AllowIncrementalSearch = true;
            this.grdIncDetView.OptionsBehavior.Editable = false;
            this.grdIncDetView.OptionsCustomization.AllowColumnMoving = false;
            this.grdIncDetView.OptionsCustomization.AllowColumnResizing = false;
            this.grdIncDetView.OptionsCustomization.AllowFilter = false;
            this.grdIncDetView.OptionsCustomization.AllowSort = false;
            this.grdIncDetView.OptionsNavigation.AutoFocusNewRow = true;
            this.grdIncDetView.OptionsNavigation.EnterMoveNextColumn = true;
            this.grdIncDetView.OptionsView.ShowGroupPanel = false;
            this.grdIncDetView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.grdIncDetView_CustomDrawRowIndicator);
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
            this.btnEdit,
            this.btnDelete,
            this.btnExit,
            this.btnPrint});
            this.barManager1.MaxItemId = 4;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnEdit, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnDelete, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnPrint),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnExit, DevExpress.XtraBars.BarItemPaintStyle.Standard)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // btnEdit
            // 
            this.btnEdit.Caption = "Edit";
            this.btnEdit.Glyph = global::CRM.Properties.Resources.application_edit;
            this.btnEdit.Id = 0;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnEdit_ItemClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Caption = "Delete";
            this.btnDelete.Glyph = global::CRM.Properties.Resources.application_delete;
            this.btnDelete.Id = 1;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDelete_ItemClick);
            // 
            // btnPrint
            // 
            this.btnPrint.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnPrint.Caption = "Print";
            this.btnPrint.Glyph = global::CRM.Properties.Resources.printer1;
            this.btnPrint.Id = 3;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPrint_ItemClick);
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
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 369);
            this.barDockControlBottom.Size = new System.Drawing.Size(519, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 26);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 343);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(519, 26);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 343);
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Blue";
            // 
            // frmIncentiveDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 369);
            this.ControlBox = false;
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmIncentiveDetails";
            this.Text = "Incentive Details";
            this.Load += new System.EventHandler(this.frmIncentiveDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdIncDet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdIncDetView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem btnEdit;
        private DevExpress.XtraBars.BarButtonItem btnDelete;
        private DevExpress.XtraBars.BarButtonItem btnExit;
        private DevExpress.XtraGrid.GridControl grdIncDet;
        private DevExpress.XtraGrid.Views.Grid.GridView grdIncDetView;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.XtraBars.BarButtonItem btnPrint;
    }
}