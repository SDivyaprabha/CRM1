using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;


namespace CRM.DataLayer
{
    class ControlPanelDL
    {

        public static DataTable GetUserDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                string sSql = "Select A.UserId,B.LeadName CutomerName,A.UserName,B.Email,EditAddress,A.Live from dbo.UserLogin A " +
                              "Inner Join dbo.LeadRegister B on A.LeadId=B.LeadId Order by B.LeadName";

                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(dt);
                da.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static void UpdatePanel(bool argAddr,bool argLive,int argUserId)
        {
            try
            {
                SqlCommand cmd;
                string sSql = "Update dbo.UserLogin Set EditAddress='" + argAddr + "',Live='" + argLive + "' Where UserId=" + argUserId + "";
                BsfGlobal.OpenCRMDB();
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


    }
}
