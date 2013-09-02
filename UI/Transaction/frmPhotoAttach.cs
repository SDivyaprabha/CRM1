using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace CRM
{
    public partial class frmPhotoAttach : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        //byte[] Array1 = null;
        System.IO.FileStream o = null;
        StreamReader r = default(StreamReader);
        string PicPath = null;
        byte[] FileByteArray = null;
        int m_iCostCentreId = 0;
        int m_iImageId = 0;
        bool m_bOk = false;
        CRM.BusinessLayer.PhotoBL m_oPhoto;

        #endregion

        #region Constructor

        public frmPhotoAttach()
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

        public bool Execute(int argCCId, int argImageId)
        {
            m_iCostCentreId = argCCId;
            m_iImageId = argImageId;
            this.ShowDialog();
            return m_bOk;
        }

        private void PopulateData()
        {
            System.Drawing.Bitmap iImage = null;
            iImage = m_oPhoto.GetPhoto(m_iImageId);

            if (iImage != null)
            {
                PicDoc.SizeMode = PictureBoxSizeMode.Zoom;
                PicDoc.Image = iImage;
                barButtonItem6.Enabled = false;
            }
            else
            {
                barButtonItem6.Enabled = true;
            }
        }

        private void AutosizeImage(string ImagePath, PictureBox picBox, PictureBoxSizeMode pSizeMode = PictureBoxSizeMode.CenterImage)
        {
	         try 
            {
		        picBox.Image = null;
		        picBox.SizeMode = pSizeMode;
		        if (System.IO.File.Exists(ImagePath))
                {
			        Bitmap imgOrg = default(Bitmap);
			        Bitmap imgShow = default(Bitmap);
			        Graphics g = default(Graphics);
			        double divideBy = 0;
			        double divideByH = 0;
			        double divideByW = 0;
			        imgOrg = (Bitmap)Bitmap.FromFile(ImagePath);

			        divideByW = imgOrg.Width / picBox.Width;
			        divideByH = imgOrg.Height / picBox.Height;
			        if (divideByW > 1 | divideByH > 1) 
                    {
				        if (divideByW > divideByH)
                        {
					        divideBy = divideByW;
				        } 
                        else 
                        {
					        divideBy = divideByH;
				        }
				        imgShow = new Bitmap(Convert.ToInt32(Convert.ToDouble(imgOrg.Width) / divideBy), Convert.ToInt32(Convert.ToDouble(imgOrg.Height) / divideBy));
				        imgShow.SetResolution(imgOrg.HorizontalResolution, imgOrg.VerticalResolution);
				        g = Graphics.FromImage(imgShow);
				        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				        g.DrawImage(imgOrg, new Rectangle(0, 0, Convert.ToInt32(Convert.ToDouble(imgOrg.Width) / divideBy), Convert.ToInt32(Convert.ToDouble(imgOrg.Height) / divideBy)), 0, 0, imgOrg.Width, imgOrg.Height, GraphicsUnit.Pixel);
				        g.Dispose();
			       } 
                  else 
                  {
				    imgShow = new Bitmap(imgOrg.Width, imgOrg.Height);
				    imgShow.SetResolution(imgOrg.HorizontalResolution, imgOrg.VerticalResolution);
				    g = Graphics.FromImage(imgShow);
				    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				    g.DrawImage(imgOrg, new Rectangle(0, 0, imgOrg.Width, imgOrg.Height), 0, 0, imgOrg.Width, imgOrg.Height, GraphicsUnit.Pixel);
				    g.Dispose();
			      }
			      imgOrg.Dispose();
  			      picBox.Image = imgShow;
		        } 
                else 
                {
			        picBox.Image = null;
		        }
	        } 
            catch 
            {
		    }
        }

        static Image ScaleByPercent(Image imgPhoto, int Percent)
        {
            float nPercent = ((float)Percent / 100);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;

            int destX = 0;
            int destY = 0;
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight,
                                     PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                                    imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        static Image FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Red);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        private byte[] convertPicBoxImageToByte(System.Windows.Forms.PictureBox pbImage)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            pbImage.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        #endregion

        #region Form Event

        private void frmPhotoAttach_Load(object sender, EventArgs e)
        {
            barButtonItem4.Enabled = false;

            PicDoc.Image = null;
            FileByteArray = null;
            PopulateData();
        }

        #endregion

        #region Button Event

        private void BarButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult reply = MessageBox.Show("Do you want Clear the Image?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (reply == DialogResult.Yes)
            {
                PicDoc.Image = null;
                FileByteArray = null;
                barButtonItem4.Enabled = true;
                barButtonItem6.Enabled = true;
            }
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenDlg.Filter = "Image Files (*.JPG)|*.Jpg|Bitmap Files (*.Bmp)|*.Bmp| Gif Files (*.Gif)|*.gif";
            OpenDlg.ShowDialog();
            OpenDlg.FilterIndex = 1;
            PicPath = OpenDlg.FileName.ToString();

            if (PicPath == "OpenFileDialog1" | string.IsNullOrEmpty(PicPath))
                return;
            PicDoc.Load(PicPath);

          //  AutosizeImage(PicPath, PicDoc);
            
            //PicDoc.Image = FixedSize(PicDoc.Image, 100, 100);


            int sourceWidth = PicDoc.Image.Width;
            int sourceHeight = PicDoc.Image.Height;

            //if (sourceWidth > 300 || sourceHeight > 360)
            //{
            //    MessageBox.Show("Picture Size is Abnormal, Size not Exceed 300x360");
            //    PicDoc.Image = null;
            //    FileByteArray = null;
            //    return;
            //}

            PicDoc.SizeMode = PictureBoxSizeMode.CenterImage;


           // FileByteArray = convertPicBoxImageToByte(PicDoc);


            o = new FileStream(PicPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            r = new StreamReader(o);
            Array.Resize<byte>(ref FileByteArray, Convert.ToInt32(o.Length));
            o.Read(FileByteArray, 0, Convert.ToInt32(o.Length));

            barButtonItem4.Enabled = true;
            barButtonItem6.Enabled = false;
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_bOk = false;
            this.Close();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Drawing.Image iImage = PicDoc.Image;
            string sDesc=CommFun.IsNullCheck(txtDesc.EditValue, CommFun.datatypes.vartypestring).ToString();
            m_oPhoto.UpdatePhoto(FileByteArray, o, m_iCostCentreId, sDesc);
            m_bOk = true;
            this.Close();
        }

        #endregion

   }

}
