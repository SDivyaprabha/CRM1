using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CRM
{
    class FilterDL
    {
        internal static DataTable GetFilterProj(FilterBO e_FilterBO)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtLeadName;
            SqlDataAdapter sdaLName;
            string sSql = "";

            try
            {
                sSql = "Select LeadName from LeadRegister where LeadId=" + e_FilterBO.i_LeadId + "" + "Order By LeadId";
                sdaLName = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dtLeadName = new DataTable();
                sdaLName.Fill(dtLeadName);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtLeadName;
        }
    }
}
