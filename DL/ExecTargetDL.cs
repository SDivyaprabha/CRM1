using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessObjects;

namespace CRM.DataLayer
{
    class ExecTargetDL
    {
        internal static DataTable GetCostCentre()
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            DataTable dt = null;
            SqlDataAdapter sda;
            try
            {
                sSql = "Select CostCentreId,CostCentreName From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                        " Where ProjectDB in(Select ProjectName from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister " +
                        " Where BusinessType in('B','L')) and CostCentreId not in (Select CostCentreId " +
                        " From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                        " Where UserId=" + BsfGlobal.g_lUserId + ") Order By CostCentreName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable GetExecutive()
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            DataTable dt = null;
            SqlDataAdapter sda;
            try
            {
                sSql = "Select 0 RowId,UserId ExecutiveId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As ExecutiveName,Convert(bit,0,0) Sel " +
                        " From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId " +
                        " Where B.PositionType='M' ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable GetEditExecutive(int argTargetId)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            DataTable dt = null;
            SqlDataAdapter sda;
            try
            {
                sSql = "Select Distinct A.ExecutiveId,Case When B.EmployeeName='' Then B.UserName Else B.EmployeeName End As ExecutiveName From dbo.TargetTrans A " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users B On A.ExecutiveId=B.UserId Where A.TargetId=" + argTargetId + " ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable GetEditTrans(int argTargetId)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            DataTable dt = null;
            SqlDataAdapter sda;
            try
            {
                sSql = "Select A.ExecutiveId,Case When B.EmployeeName='' Then B.UserName Else B.EmployeeName End As ExecutiveName,TMonth,TYear,TValue,TUnits " +
                         " From dbo.TargetTrans A Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users B On A.ExecutiveId=B.UserId " +
                         " Where A.TargetId=" + argTargetId + " ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static void InsertTarget(DataTable argdtA, DataTable argdtU, DataTable argdtI)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                int identity = 0;
                string sSql = "";
                try
                {
                    sSql = "Insert Into dbo.TargetMaster (TargetDate,RefNo,FromDate,ToDate,CostCentreId,PeriodType,IncentiveType,IncentiveFrom) " +
                         " Values('" + ExecTargetBO.d_TargetDate.ToString("dd-MMM-yyyy") + "','" + ExecTargetBO.s_RefNo + "'," +
                         " '" + ExecTargetBO.DE_FromDate.ToString("dd-MMM-yyyy") + "','" + ExecTargetBO.DE_ToDate.ToString("dd-MMM-yyyy") + "',"+
                         " " + ExecTargetBO.i_CostCentreId + ",'" + ExecTargetBO.s_PeriodType + "','" + ExecTargetBO.s_IncenType + "',"+
                         " '" + ExecTargetBO.s_Incentivefrom + "')" +
                         " SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(sSql, conn, tran);
                    identity = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();

                    if (argdtA!=null)
                    {
                        for (int a = 0; a < argdtA.Rows.Count; a++)
                        {
                            sSql = "INSERT INTO dbo.TargetTrans(TargetId,ExecutiveId,TMonth,TYear,TValue,TUnits) Values" +
                                " (" + identity + "," + argdtA.Rows[a]["ExecutiveId"] + ", " + argdtA.Rows[a]["TMonth"] + ", " +
                                " " + argdtA.Rows[a]["TYear"] + ", " + argdtA.Rows[a]["TValue"] + ", " + argdtA.Rows[a]["TUnits"] + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }

                    if (argdtI != null)
                    {
                        for (int a = 0; a < argdtI.Rows.Count; a++)
                        {
                            sSql = "INSERT INTO dbo.IncentiveTrans(TargetId,FromValue,ToValue,IncValue) Values" +
                                " (" + identity + "," + argdtI.Rows[a]["FromValue"] + ", " + argdtI.Rows[a]["ToValue"] + ", " +
                                " " + argdtI.Rows[a]["IncValue"] + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }

                    tran.Commit();
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

        public static void UpdateTarget(int argTargetId,DataTable argdtA, DataTable argdtU, DataTable argdtI)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "Update dbo.TargetMaster Set TargetDate='" + ExecTargetBO.d_TargetDate.ToString("dd-MMM-yyyy") + "',RefNo='" + ExecTargetBO.s_RefNo + "'," +
                        " FromDate='" + ExecTargetBO.DE_FromDate.ToString("dd-MMM-yyyy") + "',ToDate='" + ExecTargetBO.DE_ToDate.ToString("dd-MMM-yyyy") + "'," +
                        " CostCentreId=" + ExecTargetBO.i_CostCentreId + ",PeriodType='" + ExecTargetBO.s_PeriodType + "',IncentiveType='" + ExecTargetBO.s_IncenType + "'," +
                        " IncentiveFrom='" + ExecTargetBO.s_Incentivefrom + "' Where TargetId=" + argTargetId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Delete From dbo.TargetTrans Where TargetId=" + argTargetId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (argdtA != null)
                    {
                        for (int a = 0; a < argdtA.Rows.Count; a++)
                        {
                            sSql = "INSERT INTO dbo.TargetTrans(TargetId,ExecutiveId,TMonth,TYear,TValue,TUnits) Values" +
                                " (" + argTargetId + "," + argdtA.Rows[a]["ExecutiveId"] + ", " + argdtA.Rows[a]["TMonth"] + ", " +
                                " " + argdtA.Rows[a]["TYear"] + ", " + argdtA.Rows[a]["TValue"] + ", " + argdtA.Rows[a]["TUnits"] + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }

                    sSql = "Delete From dbo.IncentiveTrans Where TargetId=" + argTargetId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (argdtI != null)
                    {
                        for (int a = 0; a < argdtI.Rows.Count; a++)
                        {
                            sSql = "INSERT INTO dbo.IncentiveTrans(TargetId,FromValue,ToValue,IncValue) Values" +
                                " (" + argTargetId + "," + argdtI.Rows[a]["FromValue"] + ", " + argdtI.Rows[a]["ToValue"] + ", " +
                                " " + argdtI.Rows[a]["IncValue"] + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }

                    tran.Commit();
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

        internal static DataTable GetTargetReg(string argFromDate, string argToDate)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtTarReg = null;
            SqlDataAdapter da = null;

            try
            {
                ssql = "Select TargetId, TargetDate,RefNo, FromDate, B.CostCentreName,PeriodType From TargetMaster A" +
                    " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B ON A.CostCentreId=B.CostCentreId " +
                    " Where TargetDate Between '" + argFromDate + "' And '" + argToDate + "' Order by TargetDate,RefNo";
                da = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtTarReg = new DataTable();
                da.Fill(dtTarReg);
                dtTarReg.Dispose();
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

        internal static void DeleteReg(int i_TargetId)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            SqlCommand cmd = null;

            try
            {
                sSql = String.Format("Delete dbo.TargetMaster Where TargetId={0}", i_TargetId);
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = String.Format("Delete dbo.TargetTrans Where TargetId={0}", i_TargetId);
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = String.Format("Delete dbo.IncentiveTrans Where TargetId={0}", i_TargetId);
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
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

        internal static DataTable GetEditTarMas(int argTargetId)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            DataTable dtTarRegM = null;
            SqlDataAdapter da = null;

            try
            {
                sSql = String.Format("Select * From dbo.TargetMaster Where TargetId={0}", argTargetId);
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dtTarRegM = new DataTable();
                da.Fill(dtTarRegM);
                dtTarRegM.Dispose();
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

        internal static DataTable GetTargetTrans(int argTargetId)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtTarReg = null;
            SqlDataAdapter da = null;

            try
            {
                ssql = String.Format("Select TargetTransId, TargetId, B.UserId ExecutiveId, B.EmployeeName, TMonth, TYear, TValue,TUnits From TargetTrans A" +
                    " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users B ON A.ExecutiveId=B.UserId Where TargetId={0}", argTargetId);
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

        internal static DataTable GetIncen(int argTargetId)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtTarReg = null;
            SqlDataAdapter da = null;

            try
            {
                ssql = String.Format("Select * from IncentiveTrans Where TargetId={0}", argTargetId);
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

        public static DataTable GetGridTarget(int argEntryId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql = "";

            sSql = "Select TargetDate,RefNo,FromDate,ToDate,Case When PeriodType='M' Then 'Monthly' When PeriodType='Q' Then 'Quarterly' " +
                    " When PeriodType='M' Then 'Half yearly' When PeriodType='M' Then 'Yearly' End as PeriodType,CostCentreName From dbo.TargetMaster A" +
                    " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre b On A.CostCentreId=B.CostCentreId" +
                    " Where TargetId=" + argEntryId + "";
            try
            {
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;

        }

    }
}
