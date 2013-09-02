using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace CRM.DataLayer
{
    class FeatureDL
    {
        #region Methods

        public DataTable GetFMaster()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sql = "";
            BsfGlobal.OpenCRMDB();

            try
            {
                sql = "Select * from dbo.FeatureListMaster";
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                dt.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public int InsertFDesc(string argDescription)
        {
            int iTempId = 0;
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sSql = "Insert into dbo.FeatureListMaster(FeatureDesc) Values('" + argDescription + "') SELECT SCOPE_IDENTITY();";
                SqlCommand Command = new SqlCommand(sSql, conn, tran);
                iTempId = int.Parse(Command.ExecuteScalar().ToString());
                tran.Commit();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return iTempId;
        }

        public void UpdateFDesc(int argFId, string argDescription)
        {
            BsfGlobal.OpenCRMDB();            
            string sSql="";
            SqlCommand cmd = null;
            
            try
            {
                sSql = "Update dbo.FeatureListMaster Set FeatureDesc= '" + argDescription + "' Where FeatureId = " + argFId;
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);               
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }

        }

        public void DeleteFeature(int argId)
        {
            SqlCommand cmd = null;
            BsfGlobal.OpenCRMDB();
            try
            {
                string sSql = "Delete from dbo.FeatureListMaster Where FeatureId = " + argId;
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }            
        }


        public bool CheckUsed(int argId)
        {
            bool Fchek = false;
            SqlDataAdapter sda = null;
            DataTable dt = null;
            BsfGlobal.OpenCRMDB();            
            try
            {
                string sSql = "Delete Count(FeatureId) from dbo.FTypeFeatureTrans Where FeatureId = " + argId;
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0){Fchek = true;}
                else{Fchek = false;}
                sda.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return Fchek;
        }

        public DataTable GetFlatTypeFeatureList(int argFlatTypeID)
        {
            DataTable dt=null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = "Select A.FeatureId,FeatureDesc, Case When B.FeatureId IS NULL THEN " +
                       "CONVERT(bit, 0, 0) else CONVERT(bit, 1, 1) End as  Sel " +
                       "from dbo.FeatureListMaster A  " +
                       "Left Join dbo.FTypeFeatureTrans B On A.FeatureId=B.FeatureId and B.FlatTypeID = " + argFlatTypeID;
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public void InsertFeatureTrans(int argId, DataTable argTransId)
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();           
                    
            string sSql = "";
            try
            {

                sSql = "Delete from dbo.FTypeFeatureTrans Where FlatTypeId =" + argId;
                SqlCommand Command = new SqlCommand(sSql, conn, tran);
                Command.ExecuteNonQuery();
                   
                for (int j = 0; j < argTransId.Rows.Count; j++)
                {
                    sSql = "Insert into dbo.FTypeFeatureTrans(FlatTypeId,FeatureId) Values(" + argId + "," + Convert.ToInt32(argTransId.Rows[j]["FeatureId"].ToString()) + ")";
                    Command = new SqlCommand(sSql, conn, tran);
                    Command.ExecuteNonQuery();
                    Command.Dispose();  
                }                     
                tran.Commit();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        #endregion
    }
}
