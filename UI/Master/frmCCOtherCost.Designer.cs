namespace CRM
{
    partial class frmCCOtherCost
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCCOtherCost));
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.btnAdd = new DevExpress.XtraBars.BarButtonItem();
            this.btnAddOCArea = new DevExpress.XtraBars.BarButtonItem();
            this.btnOIMaster = new DevExpress.XtraBars.BarButtonItem();
            this.btnOK = new DevExpress.XtraBars.BarButtonItem();
            this.btnCancel = new DevExpress.XtraBars.BarButtonItem();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnServiceOC = new DevExpress.XtraBars.BarButtonItem();
            this.btnOCType = new DevExpress.XtraBars.BarButtonItem();
            this.btnOX = new DevExpress.XtraBars.BarButtonItem();
            this.btnUp = new DevExpress.XtraBars.BarButtonItem();
            this.btnDown = new DevExpress.XtraBars.BarButtonItem();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.grdOC = new DevExpress.XtraGrid.GridControl();
            this.grdViewOC = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdOC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewOC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            this.SuspendLayout();
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
            this.btnAdd,
            this.btnOK,
            this.btnCancel,
            this.btnAddOCArea,
            this.btnServiceOC,
            this.btnOIMaster,
            this.btnOCType,
            this.btnUp,
            this.btnDown,
            this.btnOX});
            this.barManager1.MaxItemId = 13;
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnAdd, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnAddOCArea, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnOIMaster, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnOK, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnCancel, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.MultiLine = true;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // btnAdd
            // 
            this.btnAdd.Caption = "OC Master";
            this.btnAdd.Glyph = ((System.Drawing.Image)(resources.GetObject("btnAdd.Glyph")));
            this.btnAdd.Id = 0;
            this.btnAdd.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ItemAppearance.Normal.Options.UseFont = true;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAdd_ItemClick);
            // 
            // btnAddOCArea
            // 
            this.btnAddOCArea.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
            this.btnAddOCArea.Caption = "OA Master";
            this.btnAddOCArea.Glyph = ((System.Drawing.Image)(resources.GetObject("btnAddOCArea.Glyph")));
            this.btnAddOCArea.Id = 3;
            this.btnAddOCArea.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnAddOCArea.ItemAppearance.Normal.Options.UseFont = true;
            this.btnAddOCArea.Name = "btnAddOCArea";
            this.btnAddOCArea.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAddOCArea_ItemClick);
            // 
            // btnOIMaster
            // 
            this.btnOIMaster.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
            this.btnOIMaster.Caption = "OI Master";
            this.btnOIMaster.Glyph = ((System.Drawing.Image)(resources.GetObject("btnOIMaster.Glyph")));
            this.btnOIMaster.Id = 7;
            this.btnOIMaster.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnOIMaster.ItemAppearance.Normal.Options.UseFont = true;
            this.btnOIMaster.Name = "btnOIMaster";
            this.btnOIMaster.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOIMaster_ItemClick);
            // 
            // btnOK
            // 
            this.btnOK.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnOK.Caption = "OK";
            this.btnOK.Glyph = global::CRM.Properties.Resources.Ok_icon;
            this.btnOK.Id = 1;
            this.btnOK.Name = "btnOK";
            this.btnOK.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOK_ItemClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnCancel.Caption = "Cancel";
            this.btnCancel.Glyph = global::CRM.Properties.Resources.Button_Close_icon;
            this.btnCancel.Id = 2;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCancel_ItemClick);
            // 
            // bar1
            // 
            this.bar1.BarAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bar1.BarAppearance.Normal.Options.UseFont = true;
            this.bar1.BarName = "Custom 3";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnServiceOC, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnOCType, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnOX, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnUp, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnDown, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.MultiLine = true;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Custom 3";
            // 
            // btnServiceOC
            // 
            this.btnServiceOC.Caption = "Service";
            this.btnServiceOC.Glyph = ((System.Drawing.Image)(resources.GetObject("btnServiceOC.Glyph")));
            this.btnServiceOC.Id = 6;
            this.btnServiceOC.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnServiceOC.ItemAppearance.Normal.Options.UseFont = true;
            this.btnServiceOC.Name = "btnServiceOC";
            this.btnServiceOC.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnServiceOC_ItemClick);
            // 
            // btnOCType
            // 
            this.btnOCType.Caption = "OC Type";
            this.btnOCType.Glyph = ((System.Drawing.Image)(resources.GetObject("btnOCType.Glyph")));
            this.btnOCType.Id = 8;
            this.btnOCType.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnOCType.ItemAppearance.Normal.Options.UseFont = true;
            this.btnOCType.Name = "btnOCType";
            this.btnOCType.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOCType_ItemClick);
            // 
            // btnOX
            // 
            this.btnOX.Caption = "Exclude OtherCost";
            this.btnOX.Glyph = ((System.Drawing.Image)(resources.GetObject("btnOX.Glyph")));
            this.btnOX.Id = 12;
            this.btnOX.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnOX.ItemAppearance.Normal.Options.UseFont = true;
            this.btnOX.Name = "btnOX";
            toolTipTitleItem1.Text = "Exclude OtherCost";
            superToolTip1.Items.Add(toolTipTitleItem1);
            this.btnOX.SuperTip = superToolTip1;
            this.btnOX.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOX_ItemClick);
            // 
            // btnUp
            // 
            this.btnUp.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnUp.Caption = "Up";
            this.btnUp.Glyph = ((System.Drawing.Image)(resources.GetObject("btnUp.Glyph")));
            this.btnUp.Id = 9;
            this.btnUp.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnUp.ItemAppearance.Normal.Options.UseFont = true;
            this.btnUp.Name = "btnUp";
            this.btnUp.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUp_ItemClick);
            // 
            // btnDown
            // 
            this.btnDown.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnDown.Caption = "Down";
            this.btnDown.Glyph = ((System.Drawing.Image)(resources.GetObject("btnDown.Glyph")));
            this.btnDown.Id = 10;
            this.btnDown.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnDown.ItemAppearance.Normal.Options.UseFont = true;
            this.btnDown.Name = "btnDown";
            this.btnDown.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDown_ItemClick);
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
            this.barDockControlTop.Size = new System.Drawing.Size(439, 26);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 307);
            this.barDockControlBottom.Size = new System.Drawing.Size(439, 26);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 26);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 281);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(439, 26);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 281);
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Blue";
            // 
            // grdOC
            // 
            this.grdOC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdOC.Location = new System.Drawing.Point(0, 26);
            this.grdOC.LookAndFeel.SkinName = "Blue";
            this.grdOC.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdOC.MainView = this.grdViewOC;
            this.grdOC.Name = "grdOC";
            this.grdOC.Size = new System.Drawing.Size(439, 281);
            this.grdOC.TabIndex = 11;
            this.grdOC.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdViewOC,
            this.gridView4});
            // 
            // grdViewOC
            // 
            this.grdViewOC.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdViewOC.Appearance.FooterPanel.Options.UseFont = true;
            this.grdViewOC.Appearance.GroupFooter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdViewOC.Appearance.GroupFooter.Options.UseFont = true;
            this.grdViewOC.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdViewOC.Appearance.HeaderPanel.Options.UseFont = true;
            this.grdViewOC.ColumnPanelRowHeight = 30;
            this.grdViewOC.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.grdViewOC.GridControl = this.grdOC;
            this.grdViewOC.IndicatorWidth = 50;
            this.grdViewOC.Name = "grdViewOC";
            this.grdViewOC.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.grdViewOC.OptionsBehavior.AllowIncrementalSearch = true;
            this.grdViewOC.OptionsCustomization.AllowColumnMoving = false;
            this.grdViewOC.OptionsCustomization.AllowFilter = false;
            this.grdViewOC.OptionsCustomization.AllowSort = false;
            this.grdViewOC.OptionsMenu.EnableColumnMenu = false;
            this.grdViewOC.OptionsMenu.EnableFooterMenu = false;
            this.grdViewOC.OptionsMenu.EnableGroupPanelMenu = false;
            this.grdViewOC.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.grdViewOC.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.grdViewOC.OptionsNavigation.AutoFocusNewRow = true;
            this.grdViewOC.OptionsNavigation.EnterMoveNextColumn = true;
            this.grdViewOC.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.grdViewOC.OptionsView.ShowGroupPanel = false;
            this.grdViewOC.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.grdViewOC_CustomDrawRowIndicator);
            this.grdViewOC.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.grdViewOC_ShowingEditor);
            // 
            // gridView4
            // 
            this.gridView4.GridControl = this.grdOC;
            this.gridView4.Name = "gridView4";
            // 
            // frmCCOtherCost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 333);
            this.ControlBox = false;
            this.Controls.Add(this.grdOC);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmCCOtherCost";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProjectWise Other Cost";
            this.Load += new System.EventHandler(this.frmCCOtherCost_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdOC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewOC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
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
        private DevExpress.XtraGrid.GridControl grdOC;
        private DevExpress.XtraGrid.Views.Grid.GridView grdViewOC;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView4;
        private DevExpress.XtraBars.BarButtonItem btnAdd;
        private DevExpress.XtraBars.BarButtonItem btnOK;
        private DevExpress.XtraBars.BarButtonItem btnCancel;
        private DevExpress.XtraBars.BarButtonItem btnAddOCArea;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem btnServiceOC;
        private DevExpress.XtraBars.BarButtonItem btnOIMaster;
        private DevExpress.XtraBars.BarButtonItem btnOCType;
        private DevExpress.XtraBars.BarButtonItem btnUp;
        private DevExpress.XtraBars.BarButtonItem btnDown;
        private DevExpress.XtraBars.BarButtonItem btnOX;
    }
}