using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraPrinting;
using DevExpress.XtraBars;

namespace CRM
{
    public partial class frmProgress : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        CRM.BusinessLayer.ProgressBL m_oProgress;
        DataSet m_ds = new DataSet();
        string m_sType = "";
        int iCCId;
        bool m_bAns;
        string m_sBussinessType = "";
        int m_iLandId;

        #endregion

        #region Functions

        public frmProgress()
        {
            InitializeComponent();
            m_oProgress = new BusinessLayer.ProgressBL();
        }

        private void PopulateCostCentre()
        {
            DataTable dt = new DataTable();
            dt = m_oProgress.GetCostCentreList();
            DataRow dr = dt.NewRow();
            dr["CostCentreId"] = 0;
            dr["CostCentreName"] = "None";
            dt.Rows.InsertAt(dr, 0);
            cboCostCentre.DataSource = dt;
            cboCostCentre.ValueMember = "CostCentreId";
            cboCostCentre.DisplayMember = "CostCentreName";
            cboCostCentre.ForceInitialize();
            cboCostCentre.PopulateColumns();
            cboCostCentre.Columns["CostCentreId"].Visible = false;
            cboCostCentre.ShowFooter = false;
            cboCostCentre.ShowHeader = false;
            barcboProject.EditValue = 0;
        }

        private void PopulateGrid()
        {
            dwAbstract.Hide();
            dwDetails.Hide();

            if (barcboProgress.EditValue.ToString() == "None") { return; }
            iCCId = Convert.ToInt32(barcboProject.EditValue);
            if (iCCId == 0) { return; }

            if (barcboProgress.EditValue.ToString() == "HandingOver" || barcboProgress.EditValue.ToString() == "Works" || barcboProgress.EditValue.ToString() == "Finalization" || barcboProgress.EditValue.ToString() == "Cancellation")
            {

                if (barcboFlatType.EditValue == null) { return; }
                if (barcboFlatType.EditValue.ToString() == "") { return; }
                if (barcboProgress.EditValue.ToString() == "Works")
                    m_sType = "W";
                else
                    m_sType = "H";
                if (barcboProgress.EditValue.ToString() == "Finalization") { m_sType = "F"; }
                if (barcboProgress.EditValue.ToString() == "Cancellation") { m_sType = "C"; }

                if (m_sBussinessType == "B")
                {
                    PopulateHandingCheckList(iCCId);
                    barcboBlock.Visibility = BarItemVisibility.Always;
                    barcboLevel.Visibility = BarItemVisibility.Always;
                }
                else
                {
                    PopulatePlotHandingCheckList();
                    barcboBlock.Visibility = BarItemVisibility.Never;
                    barcboLevel.Visibility = BarItemVisibility.Never;
                }

                dwDetails.Show();
                dwAbstract.Show();

                panelControl2.Visible = false;
            }

            else if (barcboProgress.EditValue.ToString() == "Project")
            {
                if (m_sBussinessType == "B")
                {
                    barcboFlatType.Visibility = BarItemVisibility.Never;
                    PopulateProjectCheckList(iCCId);
                    dwAbstract.Show();

                    PopulateProgress(iCCId, "P");
                    panelControl2.Visible = true;
                }
                else
                {
                    barcboFlatType.Visibility = BarItemVisibility.Always;
                    if (barcboFlatType.EditValue == null) { return; }
                    if (barcboFlatType.EditValue.ToString() == "") { return; }
                    if (barcboProgress.EditValue.ToString() == "Works")
                        m_sType = "W";
                    else
                        m_sType = "H";
                    if (barcboProgress.EditValue.ToString() == "Finalization") { m_sType = "F"; }
                    if (barcboProgress.EditValue.ToString() == "Cancellation") { m_sType = "C"; }

                    PopulatePlotHandingCheckList();

                    dwDetails.Show();
                    dwAbstract.Show();

                    panelControl2.Visible = false;
                    //PopulatePlotProjectCheckList(m_iLandId);
                    //dwAbstract.Show();

                    //PopulateProgress(iCCId, "P");
                    //panelControl2.Visible = true;
                }

            }

        }

        private void PopulateFlatType()
        {
            barcboFlatType.EditValue = null;
            cboFlatType.DataSource = null;
            iCCId = Convert.ToInt32(barcboProject.EditValue);
            if (iCCId == 0) { return; }
            DataTable dt = new DataTable();
            dt = m_oProgress.GetFlatType(iCCId,m_sBussinessType,m_iLandId);
            cboFlatType.DataSource = dt;
            cboFlatType.ValueMember = "FlatTypeId";
            cboFlatType.DisplayMember = "TypeName";
        }

        private void PopulateProjectCheckList(int argCCID)
        {
            DataTable dt = new DataTable();
            dt = m_oProgress.GetProjectCheckList(argCCID);
            grdAbs.DataSource = dt;
            grdAbsView.PopulateColumns();
            grdAbsView.Columns["CheckListId"].Visible = false;
            grdAbsView.Columns["CheckListName"].Width = 300;
            grdAbsView.Columns["Status"].Width = 50;
        }

        private void PopulatePlotProjectCheckList()
        {
            string sFlatTypeId = barcboFlatType.EditValue.ToString();
            if (sFlatTypeId == "") { return; }
            DataTable dt = new DataTable();
            dt = m_oProgress.GetPlotProjectCheckList(m_iLandId, sFlatTypeId, m_sType);
            grdAbs.DataSource = dt;
            grdAbsView.PopulateColumns();
            grdAbsView.Columns["CheckListId"].Visible = false;
            grdAbsView.Columns["CheckListName"].Width = 300;
            grdAbsView.Columns["Status"].Width = 50;
        }

        private void PopulateFinalCheckList(int argCCID)
        {
            DataTable dt = new DataTable();
            dt = m_oProgress.GetFinalCheckList(argCCID);
            grdAbs.DataSource = dt;
            grdAbsView.PopulateColumns();
            grdAbsView.Columns["CheckListId"].Visible = false;
            grdAbsView.Columns["CheckListName"].Width = 300;
            grdAbsView.Columns["Status"].Width = 50;
        }

        private void PopulateHandingCheckList(int argCCID)
        {
            m_ds = new DataSet();
            string sFlatTypeId = barcboFlatType.EditValue.ToString();
            m_ds = m_oProgress.GetHandingCheckList(argCCID, sFlatTypeId, m_sType);

            DataTable dtB = new DataTable();
            DataTable dtL = new DataTable();
            DataTable dtF = new DataTable();
            DataTable dtFList = new DataTable();
            DataTable dtBTotal = new DataTable();
            DataTable dtBComp = new DataTable();

            dtB = m_ds.Tables["Block"];
            dtL = m_ds.Tables["Level"];
            dtF = m_ds.Tables["Flats"];
            dtFList = m_ds.Tables["FlatCheckList"];
            dtBTotal = m_ds.Tables["BlockTotal"];
            dtBComp = m_ds.Tables["BlockComp"];

            DataTable dt = new DataTable();
            dt = m_ds.Tables["CheckList"];

            DataView dv;
            int iBlockId = 0;
            string sBlockName = "";
            int iCheckListId = 0;
            int iTotal = 0; int iTTotal = 0;
            int iComp = 0; int iTComp = 0;
            string sStr = ""; string sTotalStr = "";
            DataRow[] drT; 

            for (int j = 0; j < dtB.Rows.Count; j++)
            {
                iTTotal = 0;
                iTComp = 0;
                iBlockId = Convert.ToInt32(dtB.Rows[j]["BlockId"].ToString());
                sBlockName = dtB.Rows[j]["BlockName"].ToString();
                dt.Columns.Add(sBlockName, typeof(string));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    iCheckListId = Convert.ToInt32(dt.Rows[i]["CheckListId"].ToString());
                    iComp = 0;
                    sStr = "";
                    dv = new DataView(dtBTotal);
                    dv.RowFilter = String.Format("BlockId = {0} and CheckListId = {1}", iBlockId, iCheckListId);
                    if (dv.ToTable().Rows.Count > 0)
                        iTotal = Convert.ToInt32(dv.ToTable().Rows[0]["CFlat"].ToString());
                    else
                        iTotal = 0;
                    dv.Dispose();
                    if (iTotal > 0)
                    {
                        dv = new DataView(dtBComp);
                        dv.RowFilter = String.Format("BlockId = {0} and CheckListId = {1}", iBlockId, iCheckListId);
                        if (dv.ToTable().Rows.Count > 0) { iComp = Convert.ToInt32(dv.ToTable().Rows[0]["CFlat"].ToString()); }
                        dv.Dispose();
                    }

                    if (iTotal == iComp) { sStr = "C"; }
                    else { sStr = String.Format("{0}/{1}", iComp.ToString().Trim(), iTotal.ToString().Trim()); }

                    drT = dt.Select(String.Format("CheckListId = {0} ", iCheckListId));
                    if (drT.Length > 0)
                    {
                        iTComp = iTComp + iComp; iTTotal = iTTotal + iTotal;
                        sTotalStr = String.Format("{0}/{1}", iTComp.ToString().Trim(), iTTotal.ToString().Trim());
                        drT[0][sBlockName] = sStr;
                    }
                    
                }
            }
            
            int iCom = 0, iTot = 0; 
            if (dt.Rows.Count > 0)
            {
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    int iTCom = 0, iTTot = 0;
                    for (int j = 3; j < dt.Columns.Count; j++)
                    {
                        string s = dt.Rows[k][j].ToString();
                        string[] sT = s.Split('/');
                        if (s == "C") { iCom = 0; iTot = 0; }
                        else
                        {
                            iCom = Convert.ToInt32(CommFun.IsNullCheck(sT[0], CommFun.datatypes.vartypenumeric));
                            iTot = Convert.ToInt32(CommFun.IsNullCheck(sT[1], CommFun.datatypes.vartypenumeric));
                        }
                        iTCom = iTCom + iCom; iTTot = iTTot + iTot;
                    }
                    dt.Rows[k]["Total"] = String.Format("{0}/{1}", iTCom.ToString().Trim(), iTTot.ToString().Trim());
                }
            }

            grdAbs.DataSource = dt;
            grdAbsView.PopulateColumns();
            grdAbsView.Columns["CheckListId"].Visible = false;
            grdAbsView.Columns["CheckListName"].Width = 200;
            grdAbsView.Columns["Total"].VisibleIndex = dt.Columns.Count-1;
            grdAbsView.Columns["CheckListName"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            for (int i = 2; i < grdAbsView.Columns.Count; i++)
            {
                grdAbsView.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }

            CboBlock.DataSource = dtB;
            CboBlock.ValueMember = "BlockId";
            CboBlock.DisplayMember = "BlockName";
            CboBlock.ForceInitialize();
            CboBlock.PopulateColumns();
            CboBlock.Columns["BlockId"].Visible = false;
            CboBlock.ShowFooter = false;
            CboBlock.ShowHeader = false;
            barcboBlock.EditValue = null;

            cboLevel.DataSource = dtL;
            cboLevel.ValueMember = "LevelId";
            cboLevel.DisplayMember = "LevelName";
            cboLevel.ForceInitialize();
            cboLevel.PopulateColumns();
            cboLevel.Columns["LevelId"].Visible = false;
            cboLevel.Columns["SortOrder"].Visible = false;
            cboLevel.ShowFooter = false;
            cboLevel.ShowHeader = false;
            barcboLevel.EditValue = null;
            grdDetails.DataSource = null;
        }

        private void PopulatePlotHandingCheckList()
        {
            barcboBlock.Visibility = BarItemVisibility.Never;
            barcboLevel.Visibility = BarItemVisibility.Never;

            m_ds = new DataSet();
            string sFlatTypeId = barcboFlatType.EditValue.ToString();
            m_ds = m_oProgress.GetPlotHandingCheckList(m_iLandId, sFlatTypeId, m_sType);

            DataTable dtF = new DataTable();
            DataTable dtFList = new DataTable();
            DataTable dtBTotal = new DataTable();
            DataTable dtBComp = new DataTable();

            dtF = m_ds.Tables["Flats"];
            dtFList = m_ds.Tables["FlatCheckList"];
            dtBTotal = m_ds.Tables["BlockTotal"];
            dtBComp = m_ds.Tables["BlockComp"];

            DataTable dt = new DataTable();
            dt = m_ds.Tables["CheckList"];

            DataView dv;
            int iCheckListId = 0;
            int iTotal = 0;
            int iComp = 0;
            string sStr = "";
            DataRow[] drT;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iCheckListId = Convert.ToInt32(dt.Rows[i]["CheckListId"].ToString());
                iComp = 0;
                sStr = "";
                dv = new DataView(dtBTotal);
                dv.RowFilter = "CheckListId = " + iCheckListId;
                if (dv.ToTable().Rows.Count > 0)
                    iTotal = Convert.ToInt32(dv.ToTable().Rows[0]["CFlat"].ToString());
                else
                    iTotal = 0;
                dv.Dispose();
                if (iTotal > 0)
                {
                    dv = new DataView(dtBComp);
                    dv.RowFilter = "CheckListId = " + iCheckListId;
                    if (dv.ToTable().Rows.Count > 0) { iComp = Convert.ToInt32(dv.ToTable().Rows[0]["CFlat"].ToString()); }
                    dv.Dispose();
                }

                if (iTotal == iComp) { sStr = "C"; }
                else { sStr = String.Format("{0}/{1}", iComp.ToString().Trim(), iTotal.ToString().Trim()); }

                drT = dt.Select(String.Format("CheckListId = {0} ", iCheckListId));
                    
            }

            grdAbs.DataSource = dt;
            grdAbsView.PopulateColumns();
            grdAbsView.Columns["CheckListId"].Visible = false;
            grdAbsView.Columns["CheckListName"].Width = 200;
            grdAbsView.Columns["CheckListName"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            for (int i = 2; i < grdAbsView.Columns.Count; i++)
            {
                grdAbsView.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }
        }

        private void PopulateBlockDetails()
        {
            int iBlockId=0;
            int iLevelId;

            if (barcboBlock.EditValue == null)  return;
            iBlockId = Convert.ToInt32(barcboBlock.EditValue.ToString());
            if (barcboLevel.EditValue == null) 
                iLevelId=0;
            else
               iLevelId = Convert.ToInt32(barcboLevel.EditValue);


            if (barcboFlatType.EditValue == null) { return; }
            m_ds = new DataSet();
            string sFlatTypeId = barcboFlatType.EditValue.ToString();
            m_ds = m_oProgress.GetHandingCheckList(iCCId, sFlatTypeId, m_sType);

            DataTable dtF = new DataTable();
            DataTable dtFList = new DataTable();


            dtF = m_ds.Tables["Flats"];
            dtFList = m_ds.Tables["FlatCheckList"];


            DataTable dt = new DataTable();
            dt = m_ds.Tables["CheckList"];

            DataView dv = new DataView(dtF);

            int iFlatId = 0;
            string sFlatName = "";

            int iCheckListId = 0;
            DataRow[] drT;

            DataTable dtFT = new DataTable();
            if (iBlockId != 0)
            {
                if (iLevelId != 0)
                    dv.RowFilter = String.Format("BlockId= {0} And LevelId={1}", iBlockId, iLevelId);
                else
                    dv.RowFilter = "BlockId= " + iBlockId;
            }
            dtFT = dv.ToTable();
            dv.Dispose();

            bool bAns = false;


            for (int j = 0; j < dtFT.Rows.Count; j++)
            {
                iFlatId = Convert.ToInt32(dtFT.Rows[j]["FlatId"].ToString());
                sFlatName = dtFT.Rows[j]["FlatNo"].ToString();
                dt.Columns.Add(sFlatName, typeof(bool));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    iCheckListId = Convert.ToInt32(dt.Rows[i]["CheckListId"].ToString());
                    dv = new DataView(dtFList);
                    dv.RowFilter = String.Format("FlatId = {0} and CheckListId = {1}", iFlatId, iCheckListId);
                    if (dv.ToTable().Rows.Count > 0)
                        bAns = true;
                    else
                        bAns = false;
                    dv.Dispose();

                    drT = dt.Select(String.Format("CheckListId = {0} ", iCheckListId));
                    if (drT.Length > 0)
                    {
                        drT[0][sFlatName] = bAns;
                    }
                }
            }

            grdDetails.DataSource = dt;
            grdDetailsView.PopulateColumns();
            grdDetailsView.Columns["CheckListId"].Visible = false;
            grdDetailsView.Columns["Total"].Visible = false;
            grdDetailsView.Columns["CheckListName"].Width = 200;
            grdDetailsView.Columns["CheckListName"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
        }

        private void PopulatePlotDetails()
        {
            m_ds = new DataSet();
            if (barcboFlatType.EditValue == null) { return; }
            if (barcboFlatType.EditValue.ToString() == "") { return; }
            string sFlatTypeId =CommFun.IsNullCheck(barcboFlatType.EditValue.ToString(), CommFun.datatypes.vartypestring).ToString();
            if (sFlatTypeId == "") { return; }
            m_ds = m_oProgress.GetPlotHandingCheckList(m_iLandId, sFlatTypeId, m_sType);

            DataTable dtF = new DataTable();
            DataTable dtFList = new DataTable();

            dtF = m_ds.Tables["Flats"];
            dtFList = m_ds.Tables["FlatCheckList"];

            DataTable dt = new DataTable();
            dt = m_ds.Tables["CheckList"];

            DataView dv = new DataView(dtF);

            int iFlatId = 0;
            string sFlatName = "";

            int iCheckListId = 0;
            DataRow[] drT;

            DataTable dtFT = new DataTable();
            dtFT = dv.ToTable();
            dv.Dispose();

            bool bAns = false;

            for (int j = 0; j < dtFT.Rows.Count; j++)
            {
                iFlatId = Convert.ToInt32(dtFT.Rows[j]["PlotDetailsId"].ToString());
                sFlatName = dtFT.Rows[j]["PlotNo"].ToString();
                dt.Columns.Add(sFlatName, typeof(bool));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    iCheckListId = Convert.ToInt32(dt.Rows[i]["CheckListId"].ToString());
                    dv = new DataView(dtFList);
                    dv.RowFilter = String.Format("PlotDetailsId = {0} and CheckListId = {1}", iFlatId, iCheckListId);
                    if (dv.ToTable().Rows.Count > 0)
                        bAns = true;
                    else
                        bAns = false;
                    dv.Dispose();

                    drT = dt.Select(String.Format("CheckListId = {0} ", iCheckListId));
                    if (drT.Length > 0)
                    {
                        drT[0][sFlatName] = bAns;
                    }
                }
            }

            grdDetails.DataSource = dt;
            grdDetailsView.PopulateColumns();
            grdDetailsView.Columns["CheckListId"].Visible = false;
            grdDetailsView.Columns["CheckListName"].Width = 200;
            grdDetailsView.Columns["CheckListName"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
        }

        private void PopulateProgress(int argFlatId, string argType)
        {
            frmProgressChart frmT = new frmProgressChart();
            panelControl2.Controls.Clear();
            frmT.Dock = DockStyle.Fill;
            frmT.TopLevel = false;
            frmT.FormBorderStyle = FormBorderStyle.None;
            panelControl2.Controls.Add(frmT);
            frmT.Execute(argFlatId, false, argType);
        }

        #endregion

        #region Form Events

        private void frmProgress_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            barcboFlatType.Caption = CommFun.m_sFuncName + " Type";
            m_bAns = false;
            barcboFlatType.Visibility = BarItemVisibility.Never;
            dwAbstract.Hide();
            dwDetails.Hide();

            PopulateCostCentre();
            barcboProgress.EditValue = "None";
            m_bAns = true;
        }

        #endregion

        #region Button Events

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dtLand = new DataTable();
            //if (BsfGlobal.FindPermission("Progress CheckList-View") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Progress CheckList-View");
            //    return;
            //}
            if (m_bAns == false) { return; }
            dtLand = LeadBL.GetBusinessType(Convert.ToInt32(barcboProject.EditValue));
            if (dtLand.Rows.Count > 0)
            {
                m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
            }
            PopulateFlatType();
            PopulateGrid();
        }

        private void barEditItem2_EditValueChanged(object sender, EventArgs e)
        {
            barcboFlatType.Visibility = BarItemVisibility.Never;
            if (m_bAns == false) { return; }
            if (m_sBussinessType == "B")
            {
                if (barcboProgress.EditValue.ToString() == "HandingOver" || barcboProgress.EditValue.ToString() == "Works" || barcboProgress.EditValue.ToString() == "Finalization" || barcboProgress.EditValue.ToString() == "Cancellation")
                {
                    barcboFlatType.Visibility = BarItemVisibility.Always;
                }
            }
            else
            {
                if (barcboProgress.EditValue.ToString() == "HandingOver" || barcboProgress.EditValue.ToString() == "Works" || barcboProgress.EditValue.ToString() == "Finalization" || barcboProgress.EditValue.ToString() == "Cancellation" || barcboProgress.EditValue.ToString() == "Project")
                {
                    barcboFlatType.Visibility = BarItemVisibility.Always;
                }
            }
            PopulateFlatType();
            PopulateGrid();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem()) { PaperKind = System.Drawing.Printing.PaperKind.A2, Landscape = true, Component = grdAbs };
            Link.CreateMarginalHeaderArea += Link1_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void barEditItem3_EditValueChanged(object sender, EventArgs e)
        {
            PopulateGrid();
            if (m_sBussinessType == "B") 
            { 
                PopulateBlockDetails(); 
            } 
            else 
            { 
                PopulatePlotDetails(); 
                PopulateBlockDetails(); 
            }
            dwAbstract.Select();
        }

        private void barEditItem4_EditValueChanged(object sender, EventArgs e)
        {
            ////PopulateGrid();
            PopulateBlockDetails();
        }

        private void cboLevel_EditValueChanged(object sender, EventArgs e)
        {
            PopulateBlockDetails();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            //gridView2.ShowPrintPreview();
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem()) { PaperKind = System.Drawing.Printing.PaperKind.A2, Landscape = true, Component = grdDetails };
            Link.CreateMarginalHeaderArea += Link2_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        void Link_CreateMarginalFooterArea(object sender, CreateAreaEventArgs e)
        {
            PageInfoBrick pib = new PageInfoBrick() { PageInfo = PageInfo.Number, Rect = new RectangleF(0, 0, 300, 20), Alignment = BrickAlignment.Far, BorderWidth = 0, HorzAlignment = DevExpress.Utils.HorzAlignment.Far, Font = new Font("Arial", 8, FontStyle.Italic), Format = "Page : {0}" };
            e.Graph.DrawBrick(pib);
        }

        void Link1_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            TextBrick brick = default(TextBrick);

            sHeader = "Progress Abstract";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new BrickStringFormat(StringAlignment.Near);

            if (Convert.ToInt32(barcboProject.EditValue) > 0)
            {
                sHeader = "(" + cboCostCentre.GetDisplayText(Convert.ToInt32(barcboProject.EditValue));
            }
            if (barcboProgress.EditValue.ToString() != "")
            {
                if (barcboProgress.EditValue.ToString() == "Project")
                {
                    sHeader = String.Format("{0}-{1})", sHeader, cboType.GetDisplayText(barcboProgress.EditValue));
                    TextBrick brick1 = default(TextBrick);

                    brick1 = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 800, 60), BorderSide.None);
                    brick1.Font = new Font("Arial", 9, FontStyle.Bold);
                    brick1.StringFormat = new BrickStringFormat(StringAlignment.Near);
                }
                else
                {
                    sHeader = String.Format("{0}-{1}", sHeader, cboType.GetDisplayText(barcboProgress.EditValue));

                    if (barcboFlatType.EditValue.ToString() != "")
                    {
                        sHeader = String.Format("{0}-{1})", sHeader, cboFlatType.GetDisplayText(barcboFlatType.EditValue));
                        TextBrick brick1 = default(TextBrick);

                        brick1 = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 800, 100), BorderSide.None);
                        brick1.Font = new Font("Arial", 9, FontStyle.Bold);
                        brick1.StringFormat = new BrickStringFormat(StringAlignment.Near);
                    }
                }
            }
        }

        void Link2_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            TextBrick brick = default(TextBrick);

            sHeader = "Progress Details";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new BrickStringFormat(StringAlignment.Near);

            if (Convert.ToInt32(barcboProject.EditValue) > 0)
            {
                sHeader = "(" + cboCostCentre.GetDisplayText(Convert.ToInt32(barcboProject.EditValue));
            }
            if (barcboProgress.EditValue.ToString() != "")
            {
                sHeader = String.Format("{0}-{1}", sHeader, cboType.GetDisplayText(barcboProgress.EditValue));
            }
            if (barcboFlatType.EditValue.ToString() != "")
            {
                sHeader = String.Format("{0}-{1}", sHeader, cboFlatType.GetDisplayText(barcboFlatType.EditValue));
            }

            if (Convert.ToInt32(barcboBlock.EditValue) > 0)
            {
                sHeader = String.Format("{0}-{1})", sHeader, CboBlock.GetDisplayText(Convert.ToInt32(barcboBlock.EditValue)));
                TextBrick brick1 = default(TextBrick);

                brick1 = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 800, 100), BorderSide.None);
                brick1.Font = new Font("Arial", 9, FontStyle.Bold);
                brick1.StringFormat = new BrickStringFormat(StringAlignment.Near);
            }
        }

        #endregion

        private void grdAbsView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdDetailsView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
