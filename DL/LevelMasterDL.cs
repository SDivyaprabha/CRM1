using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CRM.DataLayer
{
    class LevelMasterDL
    {
        #region Methods
        public DataTable GetData() 
        {
            DataTable dt = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("LevelProc", BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
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
        public int Update(CRM.BusinessLayer.LevelMasterBL OLevelUpdate)
        {
            int j = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("UpdateLevelProc",  BsfGlobal.OpenCRMDB());
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < OLevelUpdate.DtLevel.Rows.Count; i++)
                {
                    if (OLevelUpdate.DtLevel.Rows[i]["LevelID"].ToString() == string.Empty)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Flag", 1);
                        cmd.Parameters.AddWithValue("@LevelName",OLevelUpdate.DtLevel.Rows[i]["LevelName"].ToString());
                        cmd.Parameters.AddWithValue("@SortOrder",i+1);
                        cmd.Parameters.AddWithValue("@LevelID", DBNull.Value);
                        
                        j = cmd.ExecuteNonQuery();

                    }
                    else
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Flag", 2);
                        cmd.Parameters.AddWithValue("@LevelID", OLevelUpdate.DtLevel.Rows[i]["LevelID"].ToString());
                        cmd.Parameters.AddWithValue("@LevelName", OLevelUpdate.DtLevel.Rows[i]["LevelName"].ToString());
                        cmd.Parameters.AddWithValue("@SortOrder", i+1);
                        j = cmd.ExecuteNonQuery();
                    }
                
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
            return j;
        }

        public int Delete(CRM.BusinessLayer.LevelMasterBL OLevelDelete, SqlConnection Con,int argLevelId)
        {
            int j = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("Delete FROM dbo.Level_Master WHERE LevelId='" + argLevelId + "'", Con);
                j = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return j;
        }

        #endregion
    }
}
