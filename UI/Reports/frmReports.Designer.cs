namespace CRM
{
    partial class frmReports
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReports));
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.btnSales = new DevExpress.XtraBars.BarButtonItem();
            this.btnRecvStmt = new DevExpress.XtraBars.BarButtonItem();
            this.btnPWRecv = new DevExpress.XtraBars.BarButtonItem();
            this.btnSWRecv = new DevExpress.XtraBars.BarButtonItem();
            this.btnAge = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem4 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem3 = new DevExpress.XtraBars.BarSubItem();
            this.btnComp = new DevExpress.XtraBars.BarButtonItem();
            this.btnExecu = new DevExpress.XtraBars.BarButtonItem();
            this.btnCamp = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.btnAvail = new DevExpress.XtraBars.BarButtonItem();
            this.btnBank = new DevExpress.XtraBars.BarButtonItem();
            this.btnRent = new DevExpress.XtraBars.BarButtonItem();
            this.btnBroker = new DevExpress.XtraBars.BarButtonItem();
            this.btnCheck = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem5 = new DevExpress.XtraBars.BarSubItem();
            this.btnCustInf = new DevExpress.XtraBars.BarButtonItem();
            this.btnTypeReport = new DevExpress.XtraBars.BarButtonItem();
            this.btnClose = new DevExpress.XtraBars.BarButtonItem();
            this.standaloneBarDockControl1 = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.btnExit = new DevExpress.XtraBars.BarButtonItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.SuspendLayout();
            // 
            // dockManager1
            // 
            this.dockManager1.Controller = this.barAndDockingController1;
            this.dockManager1.Form = this;
            this.dockManager1.MenuManager = this.barManager1;
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // barAndDockingController1
            // 
            this.barAndDockingController1.LookAndFeel.SkinName = "Blue";
            this.barAndDockingController1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.barAndDockingController1.PaintStyleName = "Skin";
            this.barAndDockingController1.PropertiesBar.AllowLinkLighting = false;
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
            this.barManager1.DockControls.Add(this.standaloneBarDockControl1);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnExit,
            this.barSubItem1,
            this.btnSales,
            this.btnRecvStmt,
            this.barSubItem2,
            this.btnPWRecv,
            this.btnSWRecv,
            this.btnAvail,
            this.btnBank,
            this.btnBroker,
            this.btnRent,
            this.btnClose,
            this.btnCheck,
            this.barSubItem3,
            this.btnComp,
            this.btnExecu,
            this.btnCamp,
            this.btnAge,
            this.barSubItem4,
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem3,
            this.barSubItem5,
            this.btnCustInf,
            this.btnTypeReport});
            this.barManager1.MaxItemId = 35;
            // 
            // bar3
            // 
            this.bar3.BarName = "Custom 4";
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSubItem1, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem4),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSubItem3, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSubItem2, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSubItem5, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnClose, DevExpress.XtraBars.BarItemPaintStyle.CaptionInMenu)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.StandaloneBarDockControl = this.standaloneBarDockControl1;
            this.bar3.Text = "Custom 4";
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "Receivables (E)";
            this.barSubItem1.Glyph = ((System.Drawing.Image)(resources.GetObject("barSubItem1.Glyph")));
            this.barSubItem1.Id = 1;
            this.barSubItem1.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.barSubItem1.ItemAppearance.Normal.Options.UseFont = true;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnSales, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnRecvStmt, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnPWRecv, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnSWRecv, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnAge, true)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // btnSales
            // 
            this.btnSales.Caption = "Project wise Sales";
            this.btnSales.Glyph = ((System.Drawing.Image)(resources.GetObject("btnSales.Glyph")));
            this.btnSales.Id = 2;
            this.btnSales.Name = "btnSales";
            this.btnSales.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSales_ItemClick);
            // 
            // btnRecvStmt
            // 
            this.btnRecvStmt.Caption = "Receivable Statement (E)";
            this.btnRecvStmt.Glyph = ((System.Drawing.Image)(resources.GetObject("btnRecvStmt.Glyph")));
            this.btnRecvStmt.Id = 3;
            this.btnRecvStmt.Name = "btnRecvStmt";
            this.btnRecvStmt.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRecvStmt_ItemClick);
            // 
            // btnPWRecv
            // 
            this.btnPWRecv.Caption = "Project wise Receivable (E)";
            this.btnPWRecv.Glyph = ((System.Drawing.Image)(resources.GetObject("btnPWRecv.Glyph")));
            this.btnPWRecv.Id = 6;
            this.btnPWRecv.Name = "btnPWRecv";
            this.btnPWRecv.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPWRecv_ItemClick);
            // 
            // btnSWRecv
            // 
            this.btnSWRecv.Caption = "Stage wise Receivable (E)";
            this.btnSWRecv.Glyph = ((System.Drawing.Image)(resources.GetObject("btnSWRecv.Glyph")));
            this.btnSWRecv.Id = 7;
            this.btnSWRecv.Name = "btnSWRecv";
            this.btnSWRecv.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSWRecv_ItemClick);
            // 
            // btnAge
            // 
            this.btnAge.Caption = "Ageing Report";
            this.btnAge.Glyph = ((System.Drawing.Image)(resources.GetObject("btnAge.Glyph")));
            this.btnAge.Id = 22;
            this.btnAge.Name = "btnAge";
            this.btnAge.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAge_ItemClick);
            // 
            // barSubItem4
            // 
            this.barSubItem4.Caption = "Receivables (I)";
            this.barSubItem4.Glyph = ((System.Drawing.Image)(resources.GetObject("barSubItem4.Glyph")));
            this.barSubItem4.Id = 27;
            this.barSubItem4.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.barSubItem4.ItemAppearance.Normal.Options.UseFont = true;
            this.barSubItem4.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem3, true)});
            this.barSubItem4.Name = "barSubItem4";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Receivable Statement (I)";
            this.barButtonItem1.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.Glyph")));
            this.barButtonItem1.Id = 28;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "Project wise Receivable (I)";
            this.barButtonItem2.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem2.Glyph")));
            this.barButtonItem2.Id = 29;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "Stage wise Receivable (I)";
            this.barButtonItem3.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem3.Glyph")));
            this.barButtonItem3.Id = 30;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3_ItemClick);
            // 
            // barSubItem3
            // 
            this.barSubItem3.Caption = "Analysis";
            this.barSubItem3.Glyph = ((System.Drawing.Image)(resources.GetObject("barSubItem3.Glyph")));
            this.barSubItem3.Id = 17;
            this.barSubItem3.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.barSubItem3.ItemAppearance.Normal.Options.UseFont = true;
            this.barSubItem3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnComp),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnExecu, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnCamp, true)});
            this.barSubItem3.Name = "barSubItem3";
            // 
            // btnComp
            // 
            this.btnComp.Caption = "Competitor Analysis";
            this.btnComp.Glyph = ((System.Drawing.Image)(resources.GetObject("btnComp.Glyph")));
            this.btnComp.Id = 18;
            this.btnComp.Name = "btnComp";
            this.btnComp.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnComp_ItemClick);
            // 
            // btnExecu
            // 
            this.btnExecu.Caption = "Executive Analysis";
            this.btnExecu.Glyph = ((System.Drawing.Image)(resources.GetObject("btnExecu.Glyph")));
            this.btnExecu.Id = 19;
            this.btnExecu.Name = "btnExecu";
            this.btnExecu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExecu_ItemClick);
            // 
            // btnCamp
            // 
            this.btnCamp.Caption = "Campaign Analysis";
            this.btnCamp.Glyph = ((System.Drawing.Image)(resources.GetObject("btnCamp.Glyph")));
            this.btnCamp.Id = 20;
            this.btnCamp.Name = "btnCamp";
            this.btnCamp.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCamp_ItemClick);
            // 
            // barSubItem2
            // 
            this.barSubItem2.Caption = "Others";
            this.barSubItem2.Glyph = ((System.Drawing.Image)(resources.GetObject("barSubItem2.Glyph")));
            this.barSubItem2.Id = 5;
            this.barSubItem2.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.barSubItem2.ItemAppearance.Normal.Options.UseFont = true;
            this.barSubItem2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnAvail),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnBank, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnRent, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnBroker, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnCheck, true)});
            this.barSubItem2.Name = "barSubItem2";
            // 
            // btnAvail
            // 
            this.btnAvail.Caption = "Availability Chart";
            this.btnAvail.Glyph = ((System.Drawing.Image)(resources.GetObject("btnAvail.Glyph")));
            this.btnAvail.Id = 8;
            this.btnAvail.Name = "btnAvail";
            this.btnAvail.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAvail_ItemClick);
            // 
            // btnBank
            // 
            this.btnBank.Caption = "Bank Comparision";
            this.btnBank.Glyph = ((System.Drawing.Image)(resources.GetObject("btnBank.Glyph")));
            this.btnBank.Id = 9;
            this.btnBank.Name = "btnBank";
            this.btnBank.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnBank_ItemClick);
            // 
            // btnRent
            // 
            this.btnRent.Caption = "Rent Receivable";
            this.btnRent.Glyph = ((System.Drawing.Image)(resources.GetObject("btnRent.Glyph")));
            this.btnRent.Id = 13;
            this.btnRent.Name = "btnRent";
            this.btnRent.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRent_ItemClick);
            // 
            // btnBroker
            // 
            this.btnBroker.Caption = "Payable to Broker";
            this.btnBroker.Glyph = ((System.Drawing.Image)(resources.GetObject("btnBroker.Glyph")));
            this.btnBroker.Id = 12;
            this.btnBroker.Name = "btnBroker";
            this.btnBroker.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnBroker_ItemClick);
            // 
            // btnCheck
            // 
            this.btnCheck.Caption = "Check List Progress";
            this.btnCheck.Glyph = ((System.Drawing.Image)(resources.GetObject("btnCheck.Glyph")));
            this.btnCheck.Id = 15;
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCheck_ItemClick);
            // 
            // barSubItem5
            // 
            this.barSubItem5.Caption = "Project Reports";
            this.barSubItem5.Glyph = ((System.Drawing.Image)(resources.GetObject("barSubItem5.Glyph")));
            this.barSubItem5.Id = 31;
            this.barSubItem5.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.barSubItem5.ItemAppearance.Normal.Options.UseFont = true;
            this.barSubItem5.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnCustInf, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnTypeReport, true)});
            this.barSubItem5.Name = "barSubItem5";
            // 
            // btnCustInf
            // 
            this.btnCustInf.Caption = "Customer Information Report";
            this.btnCustInf.Glyph = ((System.Drawing.Image)(resources.GetObject("btnCustInf.Glyph")));
            this.btnCustInf.Id = 33;
            this.btnCustInf.Name = "btnCustInf";
            this.btnCustInf.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCustInf_ItemClick);
            // 
            // btnTypeReport
            // 
            this.btnTypeReport.Caption = "Type wise Sales Report";
            this.btnTypeReport.Glyph = ((System.Drawing.Image)(resources.GetObject("btnTypeReport.Glyph")));
            this.btnTypeReport.Id = 34;
            this.btnTypeReport.Name = "btnTypeReport";
            this.btnTypeReport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnTypeReport_ItemClick);
            // 
            // btnClose
            // 
            this.btnClose.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnClose.Caption = "Exit";
            this.btnClose.Glyph = ((System.Drawing.Image)(resources.GetObject("btnClose.Glyph")));
            this.btnClose.Id = 14;
            this.btnClose.Name = "btnClose";
            this.btnClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClose_ItemClick);
            // 
            // standaloneBarDockControl1
            // 
            this.standaloneBarDockControl1.AutoSize = true;
            this.standaloneBarDockControl1.CausesValidation = false;
            this.standaloneBarDockControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.standaloneBarDockControl1.Location = new System.Drawing.Point(0, 0);
            this.standaloneBarDockControl1.Name = "standaloneBarDockControl1";
            this.standaloneBarDockControl1.Size = new System.Drawing.Size(770, 26);
            this.standaloneBarDockControl1.Text = "standaloneBarDockControl1";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(770, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 431);
            this.barDockControlBottom.Size = new System.Drawing.Size(770, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 431);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(770, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 431);
            // 
            // btnExit
            // 
            this.btnExit.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnExit.Caption = "Exit";
            this.btnExit.Glyph = ((System.Drawing.Image)(resources.GetObject("btnExit.Glyph")));
            this.btnExit.Id = 0;
            this.btnExit.Name = "btnExit";
            this.btnExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExit_ItemClick);
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.AliceBlue;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "Blue";
            this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(770, 431);
            this.panelControl1.TabIndex = 6;
            // 
            // panelControl2
            // 
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(2, 2);
            this.panelControl2.LookAndFeel.SkinName = "Blue";
            this.panelControl2.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(766, 427);
            this.panelControl2.TabIndex = 0;
            // 
            // frmReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 431);
            this.ControlBox = false;
            this.Controls.Add(this.standaloneBarDockControl1);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmReports";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reports";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmReports_FormClosed);
            this.Load += new System.EventHandler(this.frmReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarButtonItem btnExit;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem btnSales;
        private DevExpress.XtraBars.BarButtonItem btnRecvStmt;
        private DevExpress.XtraBars.BarButtonItem btnPWRecv;
        private DevExpress.XtraBars.BarButtonItem btnSWRecv;
        private DevExpress.XtraBars.BarSubItem barSubItem2;
        private DevExpress.XtraBars.BarButtonItem btnAvail;
        private DevExpress.XtraBars.BarButtonItem btnBank;
        private DevExpress.XtraBars.BarButtonItem btnRent;
        private DevExpress.XtraBars.BarButtonItem btnBroker;
        private DevExpress.XtraBars.BarButtonItem btnClose;
        private DevExpress.XtraBars.StandaloneBarDockControl standaloneBarDockControl1;
        private DevExpress.XtraBars.BarSubItem barSubItem3;
        private DevExpress.XtraBars.BarButtonItem btnComp;
        private DevExpress.XtraBars.BarButtonItem btnExecu;
        private DevExpress.XtraBars.BarButtonItem btnCamp;
        private DevExpress.XtraBars.BarButtonItem btnCheck;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraBars.BarButtonItem btnAge;
        private DevExpress.XtraBars.BarSubItem barSubItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarSubItem barSubItem5;
        private DevExpress.XtraBars.BarButtonItem btnCustInf;
        private DevExpress.XtraBars.BarButtonItem btnTypeReport;
    }
}