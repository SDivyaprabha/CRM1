namespace CRM
{
    partial class frmFeatureTrans
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFeatureTrans));
            this.panelmain = new DevExpress.XtraEditors.PanelControl();
            this.grdTrans = new DevExpress.XtraGrid.GridControl();
            this.gridViewTrans = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnAddNew = new DevExpress.XtraBars.BarButtonItem();
            this.cmdOK = new DevExpress.XtraBars.BarButtonItem();
            this.cmdCancel = new DevExpress.XtraBars.BarButtonItem();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelmain)).BeginInit();
            this.panelmain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTrans)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTrans)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelmain
            // 
            this.panelmain.Controls.Add(this.grdTrans);
            this.panelmain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelmain.Location = new System.Drawing.Point(0, 0);
            this.panelmain.LookAndFeel.SkinName = "Blue";
            this.panelmain.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelmain.Name = "panelmain";
            this.panelmain.Size = new System.Drawing.Size(335, 296);
            this.panelmain.TabIndex = 0;
            // 
            // grdTrans
            // 
            this.grdTrans.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTrans.Location = new System.Drawing.Point(2, 2);
            this.grdTrans.LookAndFeel.SkinName = "Blue";
            this.grdTrans.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdTrans.MainView = this.gridViewTrans;
            this.grdTrans.Name = "grdTrans";
            this.grdTrans.Size = new System.Drawing.Size(331, 292);
            this.grdTrans.TabIndex = 7;
            this.grdTrans.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTrans,
            this.gridView4});
            // 
            // gridViewTrans
            // 
            this.gridViewTrans.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewTrans.GridControl = this.grdTrans;
            this.gridViewTrans.Name = "gridViewTrans";
            this.gridViewTrans.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewTrans.OptionsMenu.EnableColumnMenu = false;
            this.gridViewTrans.OptionsMenu.EnableFooterMenu = false;
            this.gridViewTrans.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewTrans.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridViewTrans.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridViewTrans.OptionsNavigation.AutoFocusNewRow = true;
            this.gridViewTrans.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gridViewTrans.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gridViewTrans.OptionsView.ShowGroupPanel = false;
            this.gridViewTrans.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gridViewTrans_ShowingEditor);
            // 
            // gridView4
            // 
            this.gridView4.GridControl = this.grdTrans;
            this.gridView4.Name = "gridView4";
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
            this.cmdOK,
            this.cmdCancel,
            this.btnAddNew});
            this.barManager1.MaxItemId = 3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnAddNew, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.cmdOK, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.cmdCancel, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.AllowRename = true;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // btnAddNew
            // 
            this.btnAddNew.Caption = "Add";
            this.btnAddNew.Glyph = global::CRM.Properties.Resources.application_add;
            this.btnAddNew.Id = 2;
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAddNew_ItemClick);
            // 
            // cmdOK
            // 
            this.cmdOK.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.cmdOK.Caption = "OK";
            this.cmdOK.Glyph = ((System.Drawing.Image)(resources.GetObject("cmdOK.Glyph")));
            this.cmdOK.Id = 0;
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.cmdOK_ItemClick);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Caption = "Cancel";
            this.cmdCancel.Glyph = ((System.Drawing.Image)(resources.GetObject("cmdCancel.Glyph")));
            this.cmdCancel.Id = 1;
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.cmdCancel_ItemClick);
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
            this.barDockControlTop.Size = new System.Drawing.Size(335, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 296);
            this.barDockControlBottom.Size = new System.Drawing.Size(335, 26);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 296);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(335, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 296);
            // 
            // frmFeatureTrans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 322);
            this.ControlBox = false;
            this.Controls.Add(this.panelmain);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmFeatureTrans";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Feature List";
            this.Load += new System.EventHandler(this.frmFeatureTrans_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelmain)).EndInit();
            this.panelmain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTrans)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTrans)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelmain;
        private DevExpress.XtraGrid.GridControl grdTrans;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewTrans;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView4;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem cmdOK;
        private DevExpress.XtraBars.BarButtonItem cmdCancel;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem btnAddNew;
    }
}