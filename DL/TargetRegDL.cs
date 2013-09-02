using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using CRM.BO;

namespace CRM.DL
{
    class TargetRegDL
    {
        #region Methods

        internal static DataTable getTargetReg(string argFromDate,string argToDate)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtTarReg = null;
            SqlDataAdapter da = null;

            try
            {
                ssql = "Select TargetId, TargetDate,RefNo, FromDate, B.CostCentreName,TargetType, NoofPeriods,PeriodType from TargetMaster A" +
                    " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B ON A.CostCentre=B.CostCentreId " +
                    " Where TargetDate Between '" + argFromDate + "' And '" + argToDate + "' Order by TargetDate,RefNo";
                da = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtTarReg = new DataTable();
                da.Fill(dtTarReg);
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtTarReg;
        }

        internal static DataTable getTargetTrans(TargetEntryBO TarentBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtTarReg = null;
            SqlDataAdapter da = null;

            try
            {
                ssql = String.Format("Select TargetTransId, TargetId, B.UserId, B.EmployeeName, TMonth, TYear, TValue from TargetTrans A" +
                    " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users B ON A.ExecutiveId=B.UserId Where TargetId={0}", TarentBO.i_TargetId);
                da = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtTarReg = new DataTable();
                da.Fill(dtTarReg);
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtTarReg;
        }

        internal static DataTable getTargetUnitTrans(TargetEntryBO TarentBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtTarReg = null;
            SqlDataAdapter da = null;

            try
            {
                ssql = String.Format("Select TargetTransId, TargetId, B.UserId, B.EmployeeName, TMonth, TYear, TValue from TargetAmtTrans A" +
                    " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users B ON A.ExecutiveId=B.UserId Where TargetId={0}", TarentBO.i_TargetId);
                da = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtTarReg = new DataTable();
                da.Fill(dtTarReg);
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtTarReg;
        }

        internal static DataTable getIncen(TargetEntryBO TarentBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtTarReg = null;
            SqlDataAdapter da = null;

            try
            {
                ssql = String.Format("Select * from IncentiveTrans Where TargetId={0}", TarentBO.i_TargetId);
                da = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtTarReg = new DataTable();
                da.Fill(dtTarReg);
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtTarReg;
        }

        public static DataTable FillRegExec(int argTargetId)
        {
            DataTable dtRec = null;
            try
            {
                
                SqlDataAdapter sda;
                BsfGlobal.OpenCRMDB();
                string sql = string.Empty;

                //sql = "Select UserId ExecId,EmployeeName ExecName From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId Where B.PositionType='M'";
                sql = "Select Distinct UserId ExecId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As ExecName From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId " +
                        " Inner Join TargetTrans C On C.ExecutiveId=A.UserId" +
                        " Where B.PositionType='M' And TargetId=" + argTargetId + "";
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dtRec = new DataTable();
                sda.Fill(dtRec);            
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtRec;
        }

        internal static DataTable getEditTarMas(TargetEntryBO TargetBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtTarRegM = null;
            SqlDataAdapter da = null;

            try
            {
                ssql = String.Format("Select * from TargetMaster Where TargetId={0}", TargetBO.i_TargetId);
                da = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtTarRegM = new DataTable();
                da.Fill(dtTarRegM);
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtTarRegM;
        }

        internal static DataTable getEditTarTrans(TargetEntryBO TargetBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtTarRegT = null;
            SqlDataAdapter da = null;

            try
            {
                ssql = String.Format("Select * from TargetTrans Where TargetId={0} AND TargetTransId={1}", TargetBO.i_TargetId, TargetBO.i_TargetTransId);
                da = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtTarRegT = new DataTable();
                da.Fill(dtTarRegT);
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtTarRegT;
        }

        internal static DataTable getEditIncentiveTran(TargetEntryBO TargetBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtTarRegI = null;
            SqlDataAdapter da = null;

            try
            {
                ssql = String.Format("Select * from IncentiveTrans Where TargetId={0} AND IncentiveId={1}", TargetBO.i_TargetId, TargetBO.i_IncentiveId);
                da = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtTarRegI = new DataTable();
                da.Fill(dtTarRegI);
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtTarRegI;
        }

        internal static void DeleteReg(int i_TargetId)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand cmd = null;

            try
            {
                ssql = String.Format("Delete TargetMaster Where TargetId={0}", i_TargetId);
                cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
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

        #endregion
    }
}
