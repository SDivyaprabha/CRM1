using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace CRM.DataLayer
{
    class CommonDL
    {
        internal static DataTable Get_CostCentre()
        {
            DataTable dtData=null;
            SqlCommand command;
            SqlDataReader sdr; string sSql = "";
            try
            {
                sSql = "Select CostCentreId,CostCentreName From dbo.OperationalCostCentre" +
                        " Where ProjectDB In(Select ProjectName From " +
                        " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType IN('B'))" +
                        " and CostCentreId Not In (Select CostCentreId From dbo.UserCostCentreTrans " +
                        " Where UserId=" + BsfGlobal.g_lUserId + ") Order By CostCentreName";
                command = new SqlCommand(sSql, BsfGlobal.OpenWorkFlowDB()) { CommandType = CommandType.Text };
                dtData = new DataTable();
                sdr = command.ExecuteReader(CommandBehavior.CloseConnection);
                dtData.Load(sdr);
                dtData.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dtData;

        }

        internal static int Get_Buyer_SL(int arg_iBuyerId)
        {
            SqlDataAdapter sda;
            DataTable dt;
            int BuyerSLId=0;
            string sSql = string.Empty;
            try
            {
                sSql = "SELECT SubLedgerId FROM ["+ BsfGlobal.g_sFaDBName +"].dbo.SubLedgerMaster WHERE SubLedgerTypeId=3"+
                        " AND RefId="+arg_iBuyerId;
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                sda.SelectCommand.CommandType = CommandType.Text;
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count != 0)
                {
                    BuyerSLId = Convert.ToInt32(dt.Rows[0]["SubLedgerId"]);
                }
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
            return BuyerSLId ;
        }

        internal static DataTable Get_Income()
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dtData = null;
            try
            {
                sda = new SqlDataAdapter(String.Format("SELECT AccountId TypeId, AccountName TypeName FROM [{0}].dbo.AccountMaster WHERE LastLevel='Y' AND TypeId=3", BsfGlobal.g_sFaDBName), BsfGlobal.OpenCRMDB());
                sda.SelectCommand.CommandType = CommandType.Text;
                dtData = new DataTable();
                sda.Fill(dtData);
                dtData.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtData;
        }

        internal static int Get_SubLedgerType(string arg_sType)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dtData = null;
            int iSLTypeId = 0;

            try
            {

                sda = new SqlDataAdapter(String.Format("SELECT SubLedgerTypeId FROM [{0}].dbo.SubLedgerType WHERE SubLedgerTypeName='" + arg_sType + "'", BsfGlobal.g_sFaDBName), BsfGlobal.OpenCRMDB());
                sda.SelectCommand.CommandType = CommandType.Text;
                dtData = new DataTable();
                sda.Fill(dtData);
                if (dtData.Rows.Count != 0)
                {
                    iSLTypeId = Convert.ToInt32(dtData.Rows[0]["SubLedgerTypeId"].ToString());
                }
                dtData.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return iSLTypeId;
        }

        internal static string Get_CompanyDB(int arg_iCompanyId, DateTime arg_dPVDate)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dtData = null;
            string sCompDBName = string.Empty;

            try
            {
                sda = new SqlDataAdapter(String.Format("SELECT DBName FROM [{0}].dbo.FiscalYear WHERE FYearId = " +
                           "(SELECT FYearId FROM [{1}].dbo.FiscalYearTrans WHERE CompanyId='{2}' AND '{3}'  " +
                           "BETWEEN FromDate AND ToDate)", BsfGlobal.g_sFaDBName, BsfGlobal.g_sFaDBName, arg_iCompanyId, arg_dPVDate.ToString("dd/MMM/yyyy")), BsfGlobal.OpenCRMDB());
                sda.SelectCommand.CommandType = CommandType.Text;
                dtData = new DataTable();
                sda.Fill(dtData);
                if (dtData.Rows.Count != 0)
                {
                    sCompDBName = dtData.Rows[0]["DBName"].ToString();
                }
                dtData.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return sCompDBName;
        }

        internal static int Get_BuyerType()
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dtData = null;
            int iBuyerTypeId = 0;

            try
            {
                sda = new SqlDataAdapter(String.Format("SELECT AccountId FROM [{0}].dbo.AccountMaster WHERE IsBuyer=1", BsfGlobal.g_sFaDBName), BsfGlobal.OpenCRMDB());
                sda.SelectCommand.CommandType = CommandType.Text;
                dtData = new DataTable();
                sda.Fill(dtData);
                if (dtData.Rows.Count != 0)
                {
                    iBuyerTypeId = Convert.ToInt32(dtData.Rows[0]["AccountId"].ToString());
                }
                dtData.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return iBuyerTypeId;
        }

        internal static DataTable Get_AllLead()
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dtData = null;

            try
            {
                sda = new SqlDataAdapter("SELECT A.LeadId,B.ExecutiveId,A.LeadName FROM LeadRegister A "+
                    " Inner Join dbo.LeadExecutiveInfo B On A.LeadId=B.LeadId Order By A.LeadName", BsfGlobal.g_CRMDB);
                sda.SelectCommand.CommandType = CommandType.Text;
                dtData = new DataTable();
                sda.Fill(dtData);
                dtData.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtData;
        }

        internal static DataTable Get_AllFlat_Unsold()
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dtData = null;

            try
            {
                sda = new SqlDataAdapter("SELECT FlatId,FlatNo FROM FlatDetails WHERE Status='U'", BsfGlobal.g_CRMDB);
                sda.SelectCommand.CommandType = CommandType.Text;
                dtData = new DataTable();
                sda.Fill(dtData);
                dtData.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtData;
        }

        internal static DataTable Get_AllBlock()
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dtData = null;

            try
            {
                sda = new SqlDataAdapter("SELECT BlockId,BlockName,CostCentreId FROM BlockMaster", BsfGlobal.g_CRMDB);
                sda.SelectCommand.CommandType = CommandType.Text;
                dtData = new DataTable();
                sda.Fill(dtData);
                dtData.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtData;
        }

        internal static DataTable Get_AllPaySchType()
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dtData = null;

            try
            {
                sda = new SqlDataAdapter("SELECT TypeId,TypeName FROM PaySchType", BsfGlobal.g_CRMDB);
                sda.SelectCommand.CommandType = CommandType.Text;
                dtData = new DataTable();
                sda.Fill(dtData);
                dtData.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtData;
        }
    }
}
