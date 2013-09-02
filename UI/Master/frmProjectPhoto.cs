using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CRM
{
    public partial class frmProjectPhoto : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        CRM.BusinessLayer.PhotoBL m_oPhoto;
        int m_iCostCentreId = 0;

        #endregion

        #region Constructor

        public frmProjectPhoto()
        {
            InitializeComponent();
            m_oPhoto = new BusinessLayer.PhotoBL();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (!DesignMode && IsHandleCreated)
                BeginInvoke((MethodInvoker)delegate { base.OnSizeChanged(e); });
            else
                base.OnSizeChanged(e);
        }

        #endregion

        #region Functions

        public void Execute(int argCCId)
        {
            m_iCostCentreId = argCCId;
            this.ShowDialog();
        }

        private void PopulateDates()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt = m_oPhoto.GetPhotoDates(m_iCostCentreId);
            dr = dt.NewRow();
            dr["ImageDate"] = "All";
            dt.Rows.InsertAt(dr, 0);
            repositoryItemComboBox1.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                repositoryItemComboBox1.Items.Add(dt.Rows[i]["ImageDate"].ToString());
            }
            barEditItem1.EditValue = "All";
        }

        private void PopulateImage()
        {
            DataTable dt = new DataTable();
            //string sstr = string.Format(Convert.ToDateTime(barEditItem1.EditValue).ToString("MM-dd-yyyy hh:mm:ss tt"));
            string sstr = barEditItem1.EditValue.ToString();
            dt = m_oPhoto.GetPhotos(sstr, m_iCostCentreId);
            GridControl1.DataSource = dt;
            DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit pic = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            LayoutView1.Columns[0].Visible = false;
            LayoutView1.Columns[1].ColumnEdit = pic;
            LayoutView1.Columns[1].Caption = " ";
            pic.BestFitWidth = 0;
        }

        #endregion

        #region Form Event

        private void frmProjectPhoto_Load(object sender, EventArgs e)
        {
            PopulateDates();
            PopulateImage();
        }

        #endregion

        #region Button Event

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Project Status-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Project Status-Add");
                return;
            }
            frmPhotoAttach frm = new frmPhotoAttach();
            if (frm.Execute(m_iCostCentreId, 0) == true)
            {
                PopulateDates();
                PopulateImage();
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (LayoutView1.FocusedRowHandle < 0) { return;}
            if (BsfGlobal.FindPermission("Project Status-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Project Status-Delete");
                return;
            }

            DialogResult reply = MessageBox.Show("Do you want Delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (reply == DialogResult.Yes)
            {
                int iImageId = Convert.ToInt32(LayoutView1.GetRowCellValue(LayoutView1.FocusedRowHandle,"ImageId"));
                m_oPhoto.DeletePhoto(iImageId);
                LayoutView1.DeleteRow(LayoutView1.FocusedRowHandle);
            }
        }

        #endregion

        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        {
            PopulateImage();
        }

    }
}
