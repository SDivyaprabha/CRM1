using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CRM.BusinessLayer
{
    class PhotoBL
    {
        CRM.DataLayer.PhotoDL m_oPhoto;

        public PhotoBL()
        {
            m_oPhoto = new DataLayer.PhotoDL();
        }
        public System.Drawing.Bitmap GetPhoto(int argImageId)
        {
            return m_oPhoto.GetPhoto(argImageId);
        }
        public DataTable GetPhotoDates(int argCCId)
        {
            return m_oPhoto.GetPhotoDates(argCCId);
        }
        public DataTable GetPhotos(string argStr, int argCCID)
        {
            return m_oPhoto.GetPhotos(argStr, argCCID);
        }
        public void UpdatePhoto(byte[] FileByteArray, System.IO.FileStream o, int argCCId, string argDesc)
        {
            m_oPhoto.UpdatePhoto(FileByteArray, o, argCCId,argDesc);
        }
        public void DeletePhoto(int argImageId)
        {
            m_oPhoto.DeletePhoto(argImageId);
        }
    }
}
