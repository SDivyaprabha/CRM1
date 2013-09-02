using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CRM.DataLayer
{
    class PhotoDL
    {
        public System.Drawing.Bitmap GetPhoto(int argImageId)
        {
            SqlCommand Cmd = new SqlCommand();
            SqlDataReader OleDbReader1 = null;
            System.Drawing.Bitmap BImg = null;
            try
            {
                Cmd.CommandText = "SELECT PhotoImage From dbo.ProjectPhoto Where ImageId=" + argImageId + " and PhotoImage is not Null";
                Cmd.Connection = BsfGlobal.OpenCRMDB();
                OleDbReader1 = Cmd.ExecuteReader();
                OleDbReader1.Read();
                if (OleDbReader1.HasRows == false)
                    return BImg;

                long Len1 = OleDbReader1.GetBytes(0, 0, null, 0, 0);
                byte[] Array1 = new byte[Convert.ToInt32(Len1) + 1];
                OleDbReader1.GetBytes(0, 0, Array1, 0, Convert.ToInt32(Len1));

                System.IO.MemoryStream MemoryStream1 = new System.IO.MemoryStream(Array1);
                BImg = new System.Drawing.Bitmap(MemoryStream1);

                //PicDoc.SizeMode = PictureBoxSizeMode.StretchImage;
                //PicDoc.Image = BImg;

                Cmd.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }

            return BImg;
        }
        public DataTable GetPhotoDates(int argCCId)
        {
            DataTable dt = new DataTable();
            try
            {
                string ssql = "Select Distinct Convert(Varchar(10),ImageDate,105)  ImageDate from dbo.ProjectPhoto " +
                               "Where CostCentreId = " + argCCId + " Order by ImageDate";
                BsfGlobal.OpenCRMDB();
                SqlCommand cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                dr.Close();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }
        public DataTable GetPhotos(string argStr,int argCCID)
        {
            DataTable dt = new DataTable();
            string sSql ="";
            try
            {
                sSql = "Select ImageId,PhotoImage,ImageDate,Description from dbo.ProjectPhoto " +
                       "Where CostCentreId = " + argCCID;
                BsfGlobal.OpenCRMDB();
                if (argStr != "All")
                {
                    sSql = sSql + " And CONVERT(CHAR(10),ImageDate,120) = '" + Convert.ToDateTime(argStr).ToString("yyyy-MM-dd") + "'";
                }
                sSql = sSql + " Order by ImageDate,ImageId";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                dr.Close();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }
        public void UpdatePhoto(byte[] FileByteArray, System.IO.FileStream o, int argCCId,string argDesc)
        {
            SqlCommand cmd = default(SqlCommand);
            BsfGlobal.OpenCRMDB();
            try
            {
                string sSql = "";
                if (FileByteArray != null)
                {
                    sSql = "Insert into dbo.ProjectPhoto(CostCentreId,PhotoImage,Description) " +
                           "Values(" + argCCId + ",@Logo,'" + argDesc + "')";
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    cmd.Parameters.Add("@Logo", SqlDbType.Binary, Convert.ToInt32(o.Length)).Value = FileByteArray;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
        }

        public void DeletePhoto(int argImageId)
        {
            string sSql =  "Delete from dbo.ProjectPhoto Where ImageId = " + argImageId;
            BsfGlobal.OpenCRMDB();
            try
            {
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
        }
    }
}
