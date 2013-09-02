namespace CRM
{
    partial class FrmLeadRegister
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLeadRegister));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.radDock1 = new Telerik.WinControls.UI.Docking.RadDock();
            this.dwLeadReg = new Telerik.WinControls.UI.Docking.DocumentWindow();
            this.GrdLeadRegister = new DevExpress.XtraGrid.GridControl();
            this.grdLeadRegView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.documentContainer1 = new Telerik.WinControls.UI.Docking.DocumentContainer();
            this.documentTabStrip1 = new Telerik.WinControls.UI.Docking.DocumentTabStrip();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnEdit = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnDelete = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnfilter = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnclearbtn = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
            this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barstaticfrom = new DevExpress.XtraBars.BarStaticItem();
            this.dEFrm = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.barstaticto = new DevExpress.XtraBars.BarStaticItem();
            this.dETo = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.barbtnbulkmail = new DevExpress.XtraBars.BarButtonItem();
            this.ChkExec = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radDock1)).BeginInit();
            this.radDock1.SuspendLayout();
            this.dwLeadReg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrdLeadRegister)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdLeadRegView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentContainer1)).BeginInit();
            this.documentContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentTabStrip1)).BeginInit();
            this.documentTabStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.radDock1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 53);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1053, 312);
            this.panelControl1.TabIndex = 0;
            // 
            // radDock1
            // 
            this.radDock1.ActiveWindow = this.dwLeadReg;
            this.radDock1.Controls.Add(this.documentContainer1);
            this.radDock1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radDock1.DocumentManager.DocumentInsertOrder = Telerik.WinControls.UI.Docking.DockWindowInsertOrder.InFront;
            this.radDock1.IsCleanUpTarget = true;
            this.radDock1.Location = new System.Drawing.Point(2, 2);
            this.radDock1.MainDocumentContainer = this.documentContainer1;
            this.radDock1.Name = "radDock1";
            this.radDock1.Padding = new System.Windows.Forms.Padding(5);
            // 
            // 
            // 
            this.radDock1.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.radDock1.RootElement.Padding = new System.Windows.Forms.Padding(5);
            this.radDock1.Size = new System.Drawing.Size(1049, 308);
            this.radDock1.SplitterWidth = 4;
            this.radDock1.TabIndex = 0;
            this.radDock1.TabStop = false;
            this.radDock1.Text = "radDock1";
            // 
            // dwLeadReg
            // 
            this.dwLeadReg.Controls.Add(this.GrdLeadRegister);
            this.dwLeadReg.DocumentButtons = Telerik.WinControls.UI.Docking.DocumentStripButtons.None;
            this.dwLeadReg.Location = new System.Drawing.Point(6, 29);
            this.dwLeadReg.Name = "dwLeadReg";
            this.dwLeadReg.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.TabbedDocument;
            this.dwLeadReg.Size = new System.Drawing.Size(1027, 263);
            this.dwLeadReg.Text = "Lead Register";
            // 
            // GrdLeadRegister
            // 
            this.GrdLeadRegister.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GrdLeadRegister.Location = new System.Drawing.Point(0, 0);
            this.GrdLeadRegister.LookAndFeel.SkinName = "Blue";
            this.GrdLeadRegister.LookAndFeel.UseDefaultLookAndFeel = false;
            this.GrdLeadRegister.MainView = this.grdLeadRegView;
            this.GrdLeadRegister.Name = "GrdLeadRegister";
            this.GrdLeadRegister.Size = new System.Drawing.Size(1027, 263);
            this.GrdLeadRegister.TabIndex = 0;
            this.GrdLeadRegister.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdLeadRegView});
            // 
            // grdLeadRegView
            // 
            this.grdLeadRegView.ColumnPanelRowHeight = 30;
            this.grdLeadRegView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.grdLeadRegView.GridControl = this.GrdLeadRegister;
            this.grdLeadRegView.IndicatorWidth = 60;
            this.grdLeadRegView.Name = "grdLeadRegView";
            this.grdLeadRegView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.grdLeadRegView.OptionsBehavior.AllowIncrementalSearch = true;
            this.grdLeadRegView.OptionsBehavior.Editable = false;
            this.grdLeadRegView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click;
            this.grdLeadRegView.OptionsNavigation.AutoFocusNewRow = true;
            this.grdLeadRegView.OptionsNavigation.EnterMoveNextColumn = true;
            this.grdLeadRegView.OptionsView.ShowAutoFilterRow = true;
            this.grdLeadRegView.OptionsView.ShowFooter = true;
            this.grdLeadRegView.OptionsView.ShowGroupedColumns = true;
            this.grdLeadRegView.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.grdLeadRegView.OptionsView.ShowGroupPanel = false;
            this.grdLeadRegView.PaintStyleName = "Skin";
            this.grdLeadRegView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.grdLeadRegView_CustomDrawRowIndicator);
            this.grdLeadRegView.DoubleClick += new System.EventHandler(this.grdLeadRegView_DoubleClick);
            this.grdLeadRegView.Layout += new System.EventHandler(this.grdLeadRegView_Layout);
            // 
            // documentContainer1
            // 
            this.documentContainer1.Controls.Add(this.documentTabStrip1);
            this.documentContainer1.Location = new System.Drawing.Point(5, 5);
            this.documentContainer1.Name = "documentContainer1";
            this.documentContainer1.Padding = new System.Windows.Forms.Padding(5);
            // 
            // 
            // 
            this.documentContainer1.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.documentContainer1.RootElement.Padding = new System.Windows.Forms.Padding(5);
            this.documentContainer1.Size = new System.Drawing.Size(1039, 298);
            this.documentContainer1.SizeInfo.SizeMode = Telerik.WinControls.UI.Docking.SplitPanelSizeMode.Fill;
            this.documentContainer1.SplitterWidth = 4;
            this.documentContainer1.TabIndex = 0;
            this.documentContainer1.TabStop = false;
            // 
            // documentTabStrip1
            // 
            this.documentTabStrip1.Controls.Add(this.dwLeadReg);
            this.documentTabStrip1.Location = new System.Drawing.Point(0, 0);
            this.documentTabStrip1.Name = "documentTabStrip1";
            // 
            // 
            // 
            this.documentTabStrip1.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.documentTabStrip1.SelectedIndex = 0;
            this.documentTabStrip1.Size = new System.Drawing.Size(1039, 298);
            this.documentTabStrip1.TabIndex = 0;
            this.documentTabStrip1.TabStop = false;
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar2});
            this.barManager1.Controller = this.barAndDockingController1;
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem2,
            this.barButtonItem3,
            this.barbtnDelete,
            this.barButtonItem1,
            this.barButtonItem4,
            this.barbtnfilter,
            this.barbtnclearbtn,
            this.barbtnbulkmail,
            this.barstaticfrom,
            this.dEFrm,
            this.barstaticto,
            this.dETo,
            this.ChkExec,
            this.btnRefresh,
            this.btnEdit,
            this.barSubItem1,
            this.barButtonItem5,
            this.barButtonItem6,
            this.barButtonItem7});
            this.barManager1.MaxItemId = 24;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemDateEdit1,
            this.repositoryItemDateEdit2,
            this.repositoryItemCheckEdit1});
            // 
            // bar1
            // 
            this.bar1.BarAppearance.Disabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.bar1.BarAppearance.Disabled.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.bar1.BarAppearance.Disabled.Options.UseBackColor = true;
            this.bar1.BarAppearance.Disabled.Options.UseFont = true;
            this.bar1.BarAppearance.Hovered.BackColor = System.Drawing.Color.White;
            this.bar1.BarAppearance.Hovered.BackColor2 = System.Drawing.Color.White;
            this.bar1.BarAppearance.Hovered.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.bar1.BarAppearance.Hovered.Options.UseBackColor = true;
            this.bar1.BarAppearance.Hovered.Options.UseFont = true;
            this.bar1.BarAppearance.Normal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.bar1.BarAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.bar1.BarAppearance.Normal.Options.UseBackColor = true;
            this.bar1.BarAppearance.Normal.Options.UseFont = true;
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnEdit, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barbtnDelete, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barbtnfilter, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barbtnclearbtn, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem7, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnRefresh, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSubItem1, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem3, DevExpress.XtraBars.BarItemPaintStyle.Standard)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.MultiLine = true;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // btnEdit
            // 
            this.btnEdit.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
            this.btnEdit.Caption = "Edit";
            this.btnEdit.Glyph = global::CRM.Properties.Resources.application_edit;
            this.btnEdit.Id = 18;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnEdit_ItemClick);
            // 
            // barbtnDelete
            // 
            this.barbtnDelete.Caption = "Delete";
            this.barbtnDelete.Glyph = ((System.Drawing.Image)(resources.GetObject("barbtnDelete.Glyph")));
            this.barbtnDelete.Id = 4;
            this.barbtnDelete.Name = "barbtnDelete";
            this.barbtnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnDelete_ItemClick);
            // 
            // barbtnfilter
            // 
            this.barbtnfilter.Caption = "Filter";
            this.barbtnfilter.Glyph = ((System.Drawing.Image)(resources.GetObject("barbtnfilter.Glyph")));
            this.barbtnfilter.Id = 7;
            this.barbtnfilter.Name = "barbtnfilter";
            this.barbtnfilter.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnfilter_ItemClick);
            // 
            // barbtnclearbtn
            // 
            this.barbtnclearbtn.Caption = "Clear Filter";
            this.barbtnclearbtn.Glyph = ((System.Drawing.Image)(resources.GetObject("barbtnclearbtn.Glyph")));
            this.barbtnclearbtn.Id = 8;
            this.barbtnclearbtn.Name = "barbtnclearbtn";
            this.barbtnclearbtn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnclearbtn_ItemClick);
            // 
            // barButtonItem7
            // 
            this.barButtonItem7.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barButtonItem7.Caption = "Customer Feedback";
            this.barButtonItem7.Glyph = global::CRM.Properties.Resources.group_go;
            this.barButtonItem7.Id = 23;
            this.barButtonItem7.Name = "barButtonItem7";
            this.barButtonItem7.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem7_ItemClick);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnRefresh.Caption = "Fix LayOut";
            this.btnRefresh.Glyph = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Glyph")));
            this.btnRefresh.Id = 17;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRefresh_ItemClick);
            // 
            // barSubItem1
            // 
            this.barSubItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barSubItem1.Caption = "Print";
            this.barSubItem1.Glyph = global::CRM.Properties.Resources.printer1;
            this.barSubItem1.Id = 19;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem5),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem6)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "Lead Register";
            this.barButtonItem5.Id = 20;
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem5_ItemClick);
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "Loan Status";
            this.barButtonItem6.Id = 21;
            this.barButtonItem6.Name = "barButtonItem6";
            this.barButtonItem6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem6_ItemClick);
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barButtonItem3.Caption = "Exit";
            this.barButtonItem3.Glyph = global::CRM.Properties.Resources.exit1;
            this.barButtonItem3.Id = 2;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3_ItemClick);
            // 
            // bar2
            // 
            this.bar2.BarAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bar2.BarAppearance.Normal.Options.UseFont = true;
            this.bar2.BarName = "Custom 3";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 1;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barstaticfrom),
            new DevExpress.XtraBars.LinkPersistInfo(this.dEFrm),
            new DevExpress.XtraBars.LinkPersistInfo(this.barstaticto),
            new DevExpress.XtraBars.LinkPersistInfo(this.dETo),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barbtnbulkmail, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ChkExec, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Custom 3";
            // 
            // barstaticfrom
            // 
            this.barstaticfrom.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.barstaticfrom.Caption = "From";
            this.barstaticfrom.Id = 10;
            this.barstaticfrom.ItemAppearance.Hovered.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.barstaticfrom.ItemAppearance.Hovered.ForeColor = System.Drawing.Color.Red;
            this.barstaticfrom.ItemAppearance.Hovered.Options.UseFont = true;
            this.barstaticfrom.ItemAppearance.Hovered.Options.UseForeColor = true;
            this.barstaticfrom.Name = "barstaticfrom";
            this.barstaticfrom.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // dEFrm
            // 
            this.dEFrm.Caption = "From Date";
            this.dEFrm.Edit = this.repositoryItemDateEdit1;
            this.dEFrm.EditValue = new System.DateTime(2012, 10, 8, 10, 31, 23, 0);
            this.dEFrm.Id = 11;
            this.dEFrm.Name = "dEFrm";
            this.dEFrm.Width = 89;
            this.dEFrm.EditValueChanged += new System.EventHandler(this.dEFrm_EditValueChanged);
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            this.repositoryItemDateEdit1.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            // 
            // barstaticto
            // 
            this.barstaticto.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.barstaticto.Caption = "To";
            this.barstaticto.Id = 12;
            this.barstaticto.Name = "barstaticto";
            this.barstaticto.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // dETo
            // 
            this.dETo.Caption = "To Date";
            this.dETo.Edit = this.repositoryItemDateEdit2;
            this.dETo.EditValue = new System.DateTime(2012, 10, 8, 15, 58, 19, 0);
            this.dETo.Id = 13;
            this.dETo.Name = "dETo";
            this.dETo.Width = 86;
            this.dETo.EditValueChanged += new System.EventHandler(this.dETo_EditValueChanged);
            // 
            // repositoryItemDateEdit2
            // 
            this.repositoryItemDateEdit2.AutoHeight = false;
            this.repositoryItemDateEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit2.Name = "repositoryItemDateEdit2";
            this.repositoryItemDateEdit2.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            // 
            // barbtnbulkmail
            // 
            this.barbtnbulkmail.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barbtnbulkmail.Caption = "Bulk Mail";
            this.barbtnbulkmail.Glyph = ((System.Drawing.Image)(resources.GetObject("barbtnbulkmail.Glyph")));
            this.barbtnbulkmail.Id = 9;
            this.barbtnbulkmail.Name = "barbtnbulkmail";
            this.barbtnbulkmail.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnbulkmail_ItemClick);
            // 
            // ChkExec
            // 
            this.ChkExec.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.ChkExec.Caption = "Other Executive";
            this.ChkExec.Edit = this.repositoryItemCheckEdit1;
            this.ChkExec.Glyph = global::CRM.Properties.Resources.group;
            this.ChkExec.Id = 15;
            this.ChkExec.Name = "ChkExec";
            this.ChkExec.Width = 54;
            this.ChkExec.EditValueChanged += new System.EventHandler(this.ChkExec_EditValueChanged);
            this.ChkExec.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ChkExec_ItemClick);
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.LookAndFeel.SkinName = "Metropolis";
            this.repositoryItemCheckEdit1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.repositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
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
            this.barDockControlTop.Size = new System.Drawing.Size(1053, 53);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 365);
            this.barDockControlBottom.Size = new System.Drawing.Size(1053, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 53);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 312);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1053, 53);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 312);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barButtonItem2.Caption = "Exit";
            this.barButtonItem2.Id = 1;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Filter";
            this.barButtonItem1.Id = 5;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "Filter";
            this.barButtonItem4.Id = 6;
            this.barButtonItem4.Name = "barButtonItem4";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Blue";
            // 
            // FrmLeadRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1053, 365);
            this.ControlBox = false;
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "FrmLeadRegister";
            this.Text = "Lead Register";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmLeadRegister_FormClosed);
            this.Load += new System.EventHandler(this.FrmLeadRegister_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radDock1)).EndInit();
            this.radDock1.ResumeLayout(false);
            this.dwLeadReg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrdLeadRegister)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdLeadRegView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentContainer1)).EndInit();
            this.documentContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.documentTabStrip1)).EndInit();
            this.documentTabStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private Telerik.WinControls.UI.Docking.RadDock radDock1;
        private Telerik.WinControls.UI.Docking.DocumentContainer documentContainer1;
        private Telerik.WinControls.UI.Docking.DocumentWindow dwLeadReg;
        private DevExpress.XtraGrid.GridControl GrdLeadRegister;
        private DevExpress.XtraGrid.Views.Grid.GridView grdLeadRegView;
        private Telerik.WinControls.UI.Docking.DocumentTabStrip documentTabStrip1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarButtonItem barbtnDelete;
        private DevExpress.XtraBars.BarButtonItem barbtnfilter;
        private DevExpress.XtraBars.BarButtonItem barbtnclearbtn;
        private DevExpress.XtraBars.BarButtonItem barbtnbulkmail;
        private DevExpress.XtraBars.BarStaticItem barstaticfrom;
        private DevExpress.XtraBars.BarEditItem dEFrm;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraBars.BarStaticItem barstaticto;
        private DevExpress.XtraBars.BarEditItem dETo;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraBars.BarEditItem ChkExec;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraBars.BarButtonItem btnRefresh;
        private DevExpress.XtraBars.BarButtonItem btnEdit;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem7;
    }
}