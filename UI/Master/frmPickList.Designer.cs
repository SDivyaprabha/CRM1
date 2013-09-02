namespace CRM
{
    partial class frmPickList
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.DGVTrans = new DevExpress.XtraGrid.GridControl();
            this.dgvTransView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemLookUpEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barbtnAdd = new DevExpress.XtraBars.BarButtonItem();
            this.btnDelete = new DevExpress.XtraBars.BarButtonItem();
            this.btnExit = new DevExpress.XtraBars.BarButtonItem();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVTrans)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.DGVTrans);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(368, 262);
            this.panel1.TabIndex = 4;
            // 
            // DGVTrans
            // 
            this.DGVTrans.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGVTrans.Location = new System.Drawing.Point(0, 0);
            this.DGVTrans.LookAndFeel.SkinName = "Blue";
            this.DGVTrans.LookAndFeel.UseDefaultLookAndFeel = false;
            this.DGVTrans.MainView = this.dgvTransView;
            this.DGVTrans.Name = "DGVTrans";
            this.DGVTrans.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1,
            this.repositoryItemLookUpEdit2});
            this.DGVTrans.Size = new System.Drawing.Size(368, 262);
            this.DGVTrans.TabIndex = 35;
            this.DGVTrans.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvTransView});
            // 
            // dgvTransView
            // 
            this.dgvTransView.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.dgvTransView.Appearance.ColumnFilterButton.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.dgvTransView.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.dgvTransView.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.Black;
            this.dgvTransView.Appearance.ColumnFilterButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.dgvTransView.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.dgvTransView.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.dgvTransView.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.dgvTransView.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            this.dgvTransView.Appearance.ColumnFilterButtonActive.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(190)))), ((int)(((byte)(243)))));
            this.dgvTransView.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            this.dgvTransView.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.dgvTransView.Appearance.ColumnFilterButtonActive.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.dgvTransView.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.dgvTransView.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.dgvTransView.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.dgvTransView.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.dgvTransView.Appearance.Empty.Options.UseBackColor = true;
            this.dgvTransView.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            this.dgvTransView.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.dgvTransView.Appearance.EvenRow.Options.UseBackColor = true;
            this.dgvTransView.Appearance.EvenRow.Options.UseForeColor = true;
            this.dgvTransView.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.dgvTransView.Appearance.FilterCloseButton.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.dgvTransView.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.dgvTransView.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.Black;
            this.dgvTransView.Appearance.FilterCloseButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.dgvTransView.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.dgvTransView.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.dgvTransView.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.dgvTransView.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(109)))), ((int)(((byte)(185)))));
            this.dgvTransView.Appearance.FilterPanel.ForeColor = System.Drawing.Color.White;
            this.dgvTransView.Appearance.FilterPanel.Options.UseBackColor = true;
            this.dgvTransView.Appearance.FilterPanel.Options.UseForeColor = true;
            this.dgvTransView.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.dgvTransView.Appearance.FixedLine.Options.UseBackColor = true;
            this.dgvTransView.Appearance.FocusedCell.BackColor = System.Drawing.Color.White;
            this.dgvTransView.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.dgvTransView.Appearance.FocusedCell.Options.UseBackColor = true;
            this.dgvTransView.Appearance.FocusedCell.Options.UseForeColor = true;
            this.dgvTransView.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(106)))), ((int)(((byte)(197)))));
            this.dgvTransView.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.dgvTransView.Appearance.FocusedRow.Options.UseBackColor = true;
            this.dgvTransView.Appearance.FocusedRow.Options.UseForeColor = true;
            this.dgvTransView.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.dgvTransView.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.dgvTransView.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.dgvTransView.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.dgvTransView.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.dgvTransView.Appearance.FooterPanel.Options.UseBackColor = true;
            this.dgvTransView.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.dgvTransView.Appearance.FooterPanel.Options.UseForeColor = true;
            this.dgvTransView.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.dgvTransView.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.dgvTransView.Appearance.GroupButton.ForeColor = System.Drawing.Color.Black;
            this.dgvTransView.Appearance.GroupButton.Options.UseBackColor = true;
            this.dgvTransView.Appearance.GroupButton.Options.UseBorderColor = true;
            this.dgvTransView.Appearance.GroupButton.Options.UseForeColor = true;
            this.dgvTransView.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.dgvTransView.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.dgvTransView.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.dgvTransView.Appearance.GroupFooter.Options.UseBackColor = true;
            this.dgvTransView.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.dgvTransView.Appearance.GroupFooter.Options.UseForeColor = true;
            this.dgvTransView.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(109)))), ((int)(((byte)(185)))));
            this.dgvTransView.Appearance.GroupPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.dgvTransView.Appearance.GroupPanel.Options.UseBackColor = true;
            this.dgvTransView.Appearance.GroupPanel.Options.UseForeColor = true;
            this.dgvTransView.Appearance.GroupRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.dgvTransView.Appearance.GroupRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.dgvTransView.Appearance.GroupRow.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.dgvTransView.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.dgvTransView.Appearance.GroupRow.Options.UseBackColor = true;
            this.dgvTransView.Appearance.GroupRow.Options.UseBorderColor = true;
            this.dgvTransView.Appearance.GroupRow.Options.UseFont = true;
            this.dgvTransView.Appearance.GroupRow.Options.UseForeColor = true;
            this.dgvTransView.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.dgvTransView.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.dgvTransView.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.dgvTransView.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.dgvTransView.Appearance.HeaderPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.dgvTransView.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.dgvTransView.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.dgvTransView.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.dgvTransView.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(153)))), ((int)(((byte)(228)))));
            this.dgvTransView.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(251)))));
            this.dgvTransView.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.dgvTransView.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.dgvTransView.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.dgvTransView.Appearance.HorzLine.Options.UseBackColor = true;
            this.dgvTransView.Appearance.OddRow.BackColor = System.Drawing.Color.White;
            this.dgvTransView.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.dgvTransView.Appearance.OddRow.Options.UseBackColor = true;
            this.dgvTransView.Appearance.OddRow.Options.UseForeColor = true;
            this.dgvTransView.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(252)))), ((int)(((byte)(255)))));
            this.dgvTransView.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(129)))), ((int)(((byte)(185)))));
            this.dgvTransView.Appearance.Preview.Options.UseBackColor = true;
            this.dgvTransView.Appearance.Preview.Options.UseForeColor = true;
            this.dgvTransView.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.dgvTransView.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.dgvTransView.Appearance.Row.Options.UseBackColor = true;
            this.dgvTransView.Appearance.Row.Options.UseForeColor = true;
            this.dgvTransView.Appearance.RowSeparator.BackColor = System.Drawing.Color.White;
            this.dgvTransView.Appearance.RowSeparator.Options.UseBackColor = true;
            this.dgvTransView.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(126)))), ((int)(((byte)(217)))));
            this.dgvTransView.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.dgvTransView.Appearance.SelectedRow.Options.UseBackColor = true;
            this.dgvTransView.Appearance.SelectedRow.Options.UseForeColor = true;
            this.dgvTransView.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.dgvTransView.Appearance.VertLine.Options.UseBackColor = true;
            this.dgvTransView.GridControl = this.DGVTrans;
            this.dgvTransView.Name = "dgvTransView";
            this.dgvTransView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvTransView.OptionsNavigation.AutoFocusNewRow = true;
            this.dgvTransView.OptionsNavigation.EnterMoveNextColumn = true;
            this.dgvTransView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.dgvTransView.OptionsView.AnimationType = DevExpress.XtraGrid.Views.Base.GridAnimationType.AnimateFocusedItem;
            this.dgvTransView.OptionsView.EnableAppearanceEvenRow = true;
            this.dgvTransView.OptionsView.EnableAppearanceOddRow = true;
            this.dgvTransView.OptionsView.ShowFooter = true;
            this.dgvTransView.OptionsView.ShowGroupedColumns = true;
            this.dgvTransView.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.dgvTransView.OptionsView.ShowGroupPanel = false;
            this.dgvTransView.PaintStyleName = "Skin";
            this.dgvTransView.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.dgvTransView_CellValueChanged);
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            // 
            // repositoryItemLookUpEdit2
            // 
            this.repositoryItemLookUpEdit2.AutoHeight = false;
            this.repositoryItemLookUpEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit2.Name = "repositoryItemLookUpEdit2";
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
            this.btnDelete,
            this.btnExit,
            this.barbtnAdd});
            this.barManager1.MaxItemId = 3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 1";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barbtnAdd, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnDelete, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(((DevExpress.XtraBars.BarLinkUserDefines)((DevExpress.XtraBars.BarLinkUserDefines.Caption | DevExpress.XtraBars.BarLinkUserDefines.PaintStyle))), this.btnExit, "Exit", false, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.Standard)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.AllowRename = true;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Custom 1";
            // 
            // barbtnAdd
            // 
            this.barbtnAdd.Caption = "Add";
            this.barbtnAdd.Glyph = global::CRM.Properties.Resources.application_add;
            this.barbtnAdd.Id = 2;
            this.barbtnAdd.Name = "barbtnAdd";
            this.barbtnAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnAdd_ItemClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
            this.btnDelete.Caption = "Delete";
            this.btnDelete.Glyph = global::CRM.Properties.Resources.application_delete;
            this.btnDelete.Id = 0;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDelete_ItemClick);
            // 
            // btnExit
            // 
            this.btnExit.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnExit.Caption = "Exit";
            this.btnExit.Glyph = global::CRM.Properties.Resources.exit1;
            this.btnExit.Id = 1;
            this.btnExit.Name = "btnExit";
            this.btnExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExit_ItemClick);
            // 
            // barAndDockingController1
            // 
            this.barAndDockingController1.LookAndFeel.SkinName = "Money Twins";
            this.barAndDockingController1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.barAndDockingController1.PaintStyleName = "Skin";
            this.barAndDockingController1.PropertiesBar.AllowLinkLighting = false;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(368, 26);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 288);
            this.barDockControlBottom.Size = new System.Drawing.Size(368, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 26);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 262);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(368, 26);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 262);
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Blue";
            // 
            // frmPickList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 288);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmPickList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PickList";
            this.Load += new System.EventHandler(this.frmPickList_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGVTrans)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.GridControl DGVTrans;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvTransView;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit2;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem btnDelete;
        private DevExpress.XtraBars.BarButtonItem btnExit;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.XtraBars.BarButtonItem barbtnAdd;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
    }
}