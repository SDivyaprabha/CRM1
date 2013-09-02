namespace CRM
{
    partial class frmTargetRegister
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
            this.panelTargetReg = new DevExpress.XtraEditors.PanelControl();
            this.grdTargetRegister = new DevExpress.XtraGrid.GridControl();
            this.grdTargetRegView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barTargetReg = new DevExpress.XtraBars.Bar();
            this.barbtnAdd = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnEdit = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnDelete = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.deFrom = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.deTo = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.btnIncentiveGen = new DevExpress.XtraBars.BarButtonItem();
            this.btnIncentiveDet = new DevExpress.XtraBars.BarButtonItem();
            this.btnPrint = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnExit = new DevExpress.XtraBars.BarButtonItem();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelTargetReg)).BeginInit();
            this.panelTargetReg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTargetRegister)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTargetRegView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTargetReg
            // 
            this.panelTargetReg.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.panelTargetReg.Appearance.Options.UseBackColor = true;
            this.panelTargetReg.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelTargetReg.Controls.Add(this.grdTargetRegister);
            this.panelTargetReg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTargetReg.Location = new System.Drawing.Point(0, 26);
            this.panelTargetReg.Name = "panelTargetReg";
            this.panelTargetReg.Size = new System.Drawing.Size(825, 248);
            this.panelTargetReg.TabIndex = 0;
            // 
            // grdTargetRegister
            // 
            this.grdTargetRegister.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTargetRegister.Location = new System.Drawing.Point(0, 0);
            this.grdTargetRegister.MainView = this.grdTargetRegView;
            this.grdTargetRegister.MenuManager = this.barManager1;
            this.grdTargetRegister.Name = "grdTargetRegister";
            this.grdTargetRegister.Size = new System.Drawing.Size(825, 248);
            this.grdTargetRegister.TabIndex = 0;
            this.grdTargetRegister.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdTargetRegView});
            // 
            // grdTargetRegView
            // 
            this.grdTargetRegView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.grdTargetRegView.GridControl = this.grdTargetRegister;
            this.grdTargetRegView.Name = "grdTargetRegView";
            this.grdTargetRegView.OptionsBehavior.Editable = false;
            this.grdTargetRegView.OptionsCustomization.AllowColumnMoving = false;
            this.grdTargetRegView.OptionsCustomization.AllowColumnResizing = false;
            this.grdTargetRegView.OptionsCustomization.AllowSort = false;
            this.grdTargetRegView.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.grdTargetRegView.OptionsSelection.InvertSelection = true;
            this.grdTargetRegView.OptionsView.ShowGroupPanel = false;
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.AllowQuickCustomization = false;
            this.barManager1.AllowShowToolbarsPopup = false;
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.barTargetReg});
            this.barManager1.Controller = this.barAndDockingController1;
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barbtnAdd,
            this.barbtnEdit,
            this.barbtnDelete,
            this.barbtnExit,
            this.btnIncentiveGen,
            this.btnIncentiveDet,
            this.btnPrint,
            this.barStaticItem1,
            this.deFrom,
            this.barStaticItem2,
            this.deTo});
            this.barManager1.MainMenu = this.barTargetReg;
            this.barManager1.MaxItemId = 11;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemDateEdit1,
            this.repositoryItemDateEdit2});
            // 
            // barTargetReg
            // 
            this.barTargetReg.BarAppearance.Normal.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.barTargetReg.BarAppearance.Normal.Options.UseFont = true;
            this.barTargetReg.BarName = "Target Reg";
            this.barTargetReg.DockCol = 0;
            this.barTargetReg.DockRow = 0;
            this.barTargetReg.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barTargetReg.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barbtnAdd, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barbtnEdit, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barbtnDelete, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.deFrom),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.deTo),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnIncentiveGen, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnIncentiveDet, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnPrint, true),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barbtnExit, DevExpress.XtraBars.BarItemPaintStyle.Standard)});
            this.barTargetReg.OptionsBar.AllowQuickCustomization = false;
            this.barTargetReg.OptionsBar.MultiLine = true;
            this.barTargetReg.OptionsBar.UseWholeRow = true;
            this.barTargetReg.Text = "Target Reg";
            // 
            // barbtnAdd
            // 
            this.barbtnAdd.Caption = "Add";
            this.barbtnAdd.Glyph = global::CRM.Properties.Resources.application_add;
            this.barbtnAdd.Id = 0;
            this.barbtnAdd.Name = "barbtnAdd";
            this.barbtnAdd.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barbtnAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnAdd_ItemClick);
            // 
            // barbtnEdit
            // 
            this.barbtnEdit.Caption = "Edit";
            this.barbtnEdit.Glyph = global::CRM.Properties.Resources.application_edit;
            this.barbtnEdit.Id = 1;
            this.barbtnEdit.Name = "barbtnEdit";
            this.barbtnEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnEdit_ItemClick);
            // 
            // barbtnDelete
            // 
            this.barbtnDelete.Caption = "Delete";
            this.barbtnDelete.Glyph = global::CRM.Properties.Resources.Delete_Icon;
            this.barbtnDelete.Id = 2;
            this.barbtnDelete.Name = "barbtnDelete";
            this.barbtnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnDelete_ItemClick);
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "From";
            this.barStaticItem1.Id = 7;
            this.barStaticItem1.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.barStaticItem1.ItemAppearance.Normal.Options.UseFont = true;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // deFrom
            // 
            this.deFrom.Edit = this.repositoryItemDateEdit1;
            this.deFrom.EditValue = new System.DateTime(2012, 7, 14, 12, 11, 9, 0);
            this.deFrom.Id = 8;
            this.deFrom.Name = "deFrom";
            this.deFrom.Width = 90;
            this.deFrom.EditValueChanged += new System.EventHandler(this.deFrom_EditValueChanged);
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
            // barStaticItem2
            // 
            this.barStaticItem2.Caption = "To";
            this.barStaticItem2.Id = 9;
            this.barStaticItem2.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.barStaticItem2.ItemAppearance.Normal.Options.UseFont = true;
            this.barStaticItem2.Name = "barStaticItem2";
            this.barStaticItem2.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // deTo
            // 
            this.deTo.Edit = this.repositoryItemDateEdit2;
            this.deTo.EditValue = new System.DateTime(2012, 7, 14, 13, 14, 50, 81);
            this.deTo.Id = 10;
            this.deTo.Name = "deTo";
            this.deTo.Width = 97;
            this.deTo.EditValueChanged += new System.EventHandler(this.deTo_EditValueChanged);
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
            // btnIncentiveGen
            // 
            this.btnIncentiveGen.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnIncentiveGen.Caption = "Inc Generate";
            this.btnIncentiveGen.Glyph = global::CRM.Properties.Resources.money_dollar;
            this.btnIncentiveGen.Id = 4;
            this.btnIncentiveGen.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnIncentiveGen.ItemAppearance.Normal.Options.UseFont = true;
            this.btnIncentiveGen.Name = "btnIncentiveGen";
            this.btnIncentiveGen.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnIncentiveGen.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnIncentive_ItemClick);
            // 
            // btnIncentiveDet
            // 
            this.btnIncentiveDet.Caption = "Inc Details";
            this.btnIncentiveDet.Glyph = global::CRM.Properties.Resources.arrow_merge;
            this.btnIncentiveDet.Id = 5;
            this.btnIncentiveDet.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnIncentiveDet.ItemAppearance.Normal.Options.UseFont = true;
            this.btnIncentiveDet.Name = "btnIncentiveDet";
            this.btnIncentiveDet.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnIncentiveDet.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnIncentiveDet_ItemClick);
            // 
            // btnPrint
            // 
            this.btnPrint.Caption = "Print";
            this.btnPrint.Glyph = global::CRM.Properties.Resources.printer1;
            this.btnPrint.Id = 6;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPrint_ItemClick);
            // 
            // barbtnExit
            // 
            this.barbtnExit.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barbtnExit.Caption = "Exit";
            this.barbtnExit.Glyph = global::CRM.Properties.Resources.exit1;
            this.barbtnExit.Id = 3;
            this.barbtnExit.Name = "barbtnExit";
            this.barbtnExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnExit_ItemClick);
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
            this.barDockControlTop.Size = new System.Drawing.Size(825, 26);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 274);
            this.barDockControlBottom.Size = new System.Drawing.Size(825, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 26);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 248);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(825, 26);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 248);
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Money Twins";
            // 
            // frmTargetRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 274);
            this.ControlBox = false;
            this.Controls.Add(this.panelTargetReg);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmTargetRegister";
            this.ShowIcon = false;
            this.Text = "Target Register";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmTargetRegister_FormClosed_1);
            this.Load += new System.EventHandler(this.frmTargetRegister_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelTargetReg)).EndInit();
            this.panelTargetReg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTargetRegister)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTargetRegView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelTargetReg;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar barTargetReg;
        private DevExpress.XtraBars.BarButtonItem barbtnAdd;
        private DevExpress.XtraBars.BarButtonItem barbtnEdit;
        private DevExpress.XtraBars.BarButtonItem barbtnDelete;
        private DevExpress.XtraBars.BarButtonItem barbtnExit;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl grdTargetRegister;
        private DevExpress.XtraGrid.Views.Grid.GridView grdTargetRegView;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.XtraBars.BarButtonItem btnIncentiveGen;
        private DevExpress.XtraBars.BarButtonItem btnIncentiveDet;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraBars.BarButtonItem btnPrint;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarEditItem deFrom;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.BarEditItem deTo;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit2;
    }
}