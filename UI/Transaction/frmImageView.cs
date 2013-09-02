using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace CRM
{
    public partial class frmImageView : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        //byte[] Array1 = null;
        int m_lId = 0;
        int m_lCCId = 0;
        string m_lFrom = "";
        int m_lTempId = 0;
        string m_sExtension = "";

        public Image m_limage = null;
        #endregion

        #region Object
        TemplateBL oTempBL;

        #endregion

        #region Constructor

        public frmImageView()
        {
            InitializeComponent();

            oTempBL = new TemplateBL();
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
        private void frmImageView_Load(object sender, EventArgs e)
        {
            PopulateData();
            //PicDoc.Image = m_limage;
        }

        #endregion

        #region Functions

        public void Execute(int argTempId, int argFlatId, int argCCId, string argfrmWhere, string argExt)
        {
            m_lFrom = argfrmWhere;
            m_lId = argFlatId;
            m_lCCId = argCCId;
            m_lTempId = argTempId;
            m_sExtension = argExt;
            this.ShowDialog();
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

        private void PopulateData()
        {
            System.Drawing.Bitmap iImage = null;
            System.Drawing.Image iImage2 = null;
            if (m_sExtension == ".pdf")
            {
                iImage2 = oTempBL.GetPDF(m_lCCId, m_lId, m_lTempId, m_lFrom);
                if (iImage2 != null)
                {
                    PicDoc.SizeMode = PictureBoxSizeMode.Zoom;
                    PicDoc.Image = iImage2;

                }
            }
            else
            {
                iImage = oTempBL.GetImage(m_lCCId, m_lId, m_lTempId, m_lFrom);
                if (iImage != null)
                {
                    PicDoc.SizeMode = PictureBoxSizeMode.Zoom;
                    PicDoc.Image = iImage;

                }
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

        private void ZommInOut(bool zoom)
        {
            int zoomRatio = 10; // percent
            int widthZoom = PicDoc.Width * zoomRatio / 100;
            int heightZoom = PicDoc.Height * zoomRatio / 100;

            if (zoom)
            {
                widthZoom *= -1;
                heightZoom *= -1;
            }

            PicDoc.Width += widthZoom;
            PicDoc.Height += heightZoom;
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
        #endregion


        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        #endregion

        private void btnZOut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PicDoc.Top = (int)(PicDoc.Top + (PicDoc.Height * 0.025));
            PicDoc.Left = (int)(PicDoc.Left + (PicDoc.Width * 0.025));
            PicDoc.Height = (int)(PicDoc.Height - (PicDoc.Height * 0.05));
            PicDoc.Width = (int)(PicDoc.Width - (PicDoc.Width * 0.05)); 
        }

        private void btnZIn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PicDoc.Top = (int)(PicDoc.Top - (PicDoc.Height * 0.025));
            PicDoc.Left = (int)(PicDoc.Left - (PicDoc.Width * 0.025));
            PicDoc.Height = (int)(PicDoc.Height + (PicDoc.Height * 0.05));
            PicDoc.Width = (int)(PicDoc.Width + (PicDoc.Width * 0.05));

            //if (this.PicDoc.Image == null) return;
            //Size nSize = new Size(PicDoc.Image.Width * 2,
            //PicDoc.Image.Height * 2);
            //Image gdi = new Bitmap(nSize.Width, nSize.Height);

            //Graphics ZoomInGraphics = Graphics.FromImage(gdi);

            //ZoomInGraphics.InterpolationMode =
            //InterpolationMode.NearestNeighbor;
            //ZoomInGraphics.DrawImage(PicDoc.Image, new Rectangle(new
            //Point(0, 0), nSize), new Rectangle(new Point(0, 0),
            //PicDoc.Image.Size), GraphicsUnit.Pixel);
            //ZoomInGraphics.Dispose();

            //PicDoc.Image = gdi;
            //PicDoc.Size = gdi.Size;
        }

       
    }
}
