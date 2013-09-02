using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using CRM.DataLayer;
using DevExpress.XtraPrinting;
using System.Drawing;

namespace CRM
{
    public partial class frmBankComparision : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        public int m_iVCompId;
            public string m_sMode = "";    
            string qType = "";

      #endregion

        #region Constructor

        public frmBankComparision()
        {
            InitializeComponent();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (!DesignMode && IsHandleCreated)
                BeginInvoke((MethodInvoker)delegate { base.OnSizeChanged(e); });
            else
                base.OnSizeChanged(e);
        }

        #endregion      

        #region Form Event
        
        private void frmBankComparision_Load(object sender, EventArgs e)
        {
            //if (BsfGlobal.FindPermission("Bank Comparision-View") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Bank Comparision-View");
            //    return;
            //}
            PopulateColumn();
        }

        private void frmVendorComparision_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (BsfGlobal.g_bWorkFlow == true)
            //{
            //    if (BsfGlobal.g_bWorkFlowDialog == false)
            //    {
            //        if (BsfGlobal.g_bWorkFlow == true && BsfGlobal.g_bWorkFlowDialog == false)
            //        {
            //            try
            //            {
            //                Parent.Controls.Owner.Hide();
            //            }
            //            catch
            //            {
            //            }
            //        }
            //    }
            //}
        }
        
        #endregion        

        #region Button Event
       
        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
            FormClosed += new FormClosedEventHandler(frmBankComparision_FormClosed);
        }

        void frmBankComparision_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (BsfGlobal.g_bWorkFlow == true && BsfGlobal.g_bWorkFlowDialog == false)
            //{
            //    try
            //    {
            //        Parent.Controls.Owner.Hide();
            //    }
            //    catch
            //    {
            //    }
            //}
        }
        #endregion

        #region Functions

        public DataTable GenericListToDataTable(object list)
        {
            DataTable dt = null;
            Type listType = list.GetType();
            if (listType.IsGenericType)
            {
                //determine the underlying type the List<> contains
                Type elementType = listType.GetGenericArguments()[0];

                //create empty table -- give it a name in case
                //it needs to be serialized
                dt = new DataTable(elementType.Name + "List");

                //define the table -- add a column for each public
                //property or field
                MemberInfo[] miArray = elementType.GetMembers(
                    BindingFlags.Public | BindingFlags.Instance);
                foreach (MemberInfo mi in miArray)
                {
                    if (mi.MemberType == MemberTypes.Property)
                    {
                        PropertyInfo pi = mi as PropertyInfo;
                        dt.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else if (mi.MemberType == MemberTypes.Field)
                    {
                        FieldInfo fi = mi as FieldInfo;
                        dt.Columns.Add(fi.Name, fi.FieldType);
                    }
                }

                //populate the table
                IList il = list as IList;
                foreach (object record in il)
                {
                    int i = 0;
                    object[] fieldValues = new object[dt.Columns.Count];
                    foreach (DataColumn c in dt.Columns)
                    {
                        MemberInfo mi = elementType.GetMember(c.ColumnName)[0];
                        if (mi.MemberType == MemberTypes.Property)
                        {
                            PropertyInfo pi = mi as PropertyInfo;
                            fieldValues[i] = pi.GetValue(record, null);
                        }
                        else if (mi.MemberType == MemberTypes.Field)
                        {
                            FieldInfo fi = mi as FieldInfo;
                            fieldValues[i] = fi.GetValue(record);
                        }
                        i++;
                    }
                    dt.Rows.Add(fieldValues);
                }
            }
            return dt;
        }

        public void Execute(int argVCompId,string argMode)
        {
            m_iVCompId = argVCompId;
            m_sMode = argMode;
            Show();
        }

        private void PopulateColumn()
        {
            DataTable dtVname = new DataTable();
            dtVname = BankDL.getBankname();
            grdBankComparision.DataSource = dtVname;
            cardBankCompView.PopulateColumns();
            cardBankCompView.Columns["BankName"].Visible = false;
        }

        #endregion     

        #region Drop Down Event

        private void cboQNo_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboQNo.EditValue) != 0)
            {
                DevExpress.XtraEditors.LookUpEdit editor = (DevExpress.XtraEditors.LookUpEdit)sender;
                DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;

                if (dr != null)
                {
                    txtCCName.Text = dr["CostCentreName"].ToString();
                    dtpQDate.EditValue = Convert.ToDateTime(dr["QuotationDate"].ToString());
                    qType = Convert.ToString(dr["QType"].ToString());
                    if (dr["QType"].ToString() == "M")
                    {
                        txtQType.Text = "Material";
                    }
                    if (dr["QType"].ToString() == "I")
                    {
                        txtQType.Text = "IOW";
                    }
                    if (dr["QType"].ToString() == "A")
                    {
                        txtQType.Text = "Asset";
                    }
                    if (dr["QType"].ToString() == "L")
                    {
                        txtQType.Text = "Labour";
                    }
                    if (dr["QType"].ToString() == "S")
                    {
                        txtQType.Text = "Activity";
                    }
                }
                if (m_sMode != "E")
                {
                    PopulateColumn();
                }
            }
        }
        #endregion
       
        #region Grid Event
        
        private void bntPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //grdBankComparision.ShowPrintPreview();
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdBankComparision;
            Link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);
            Link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
            Link.CreateDocument();
            Link.ShowPreview();
        }

        void Link_CreateMarginalFooterArea(object sender, CreateAreaEventArgs e)
        {
            PageInfoBrick pib = new PageInfoBrick();
            pib.PageInfo = PageInfo.Number;
            pib.Rect = new RectangleF(0, 0, 300, 20);
            pib.Alignment = BrickAlignment.Far;
            pib.BorderWidth = 0;
            pib.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            pib.Font = new Font("Arial", 8, FontStyle.Italic);
            pib.Format = "Page : {0}";
            e.Graph.DrawBrick(pib);
        }

        void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Bank Comparision";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        private void cardBankCompView_CustomDrawCardCaption(object sender, DevExpress.XtraGrid.Views.Card.CardCaptionCustomDrawEventArgs e)
        {
            DevExpress.XtraGrid.Views.Card.CardView view = sender as DevExpress.XtraGrid.Views.Card.CardView;
            (e.CardInfo as DevExpress.XtraGrid.Views.Card.ViewInfo.CardInfo).CaptionInfo.CardCaption = view.GetRowCellDisplayText(e.RowHandle, view.Columns["BankName"]);
            view.CardCaptionFormat = "Bank Name: {"+(e.CardInfo as DevExpress.XtraGrid.Views.Card.ViewInfo.CardInfo).CaptionInfo.CardCaption+"}";
        }

        #endregion
    }
}


