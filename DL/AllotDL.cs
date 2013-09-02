using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace CRM.DataLayer
{
    class AllotDL
    {
        public static DataSet GetAllot(int argCCId,DateTime argFrom,DateTime argTo)
        {
            BsfGlobal.OpenCRMDB();
            string iCCId = "";
            if (argCCId > 0) { iCCId = "And P.CostCentreId=" + argCCId + ""; }
            DataSet ds = new DataSet();
            try
            {
                string sSql = "Select CostCentreId,CostCentreName from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                              " Where ProjectDB in(Select ProjectName from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType in('B','L'))" +
                              " AND CostCentreId NOT IN(Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                              " Where UserId=" + BsfGlobal.g_lUserId + ") Order by CostCentreName";
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Project");
                da.Dispose();

                if (BsfGlobal.g_bHRMDB == true)
                {
                    sSql = "Select A.UserId ExecutiveId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As EmployeeName " +
                           "From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                           "Inner join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId " +
                           "Where B.PositionType='M' AND A.EmployeeId NOT IN(Select EmployeeId from [" + BsfGlobal.g_sHRMDBName + "].dbo.EmployeeMaster Where ActiveEmployment=0)";
                }
                else
                {
                    sSql = "Select A.UserId ExecutiveId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As EmployeeName " +
                          "From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                          "Inner join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId " +
                          "Where B.PositionType='M'";
                }
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Executive");
                da.Dispose();

                sSql = "Select Distinct A.LeadId,B.LeadName,A.CostCentreId,A.ExecutiveId,B.Mobile,B.LeadDate  from dbo.LeadExecutiveInfo  A " +
                       " INNER JOIN dbo.LeadProjectInfo P On A.LeadId=P.LeadId AND A.CostCentreId=P.CostCentreId" +
                       " INNER JOIN dbo.LeadRegister B on A.LeadId=B.LeadId " +
                       " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C on P.CostCentreId=C.CostCentreId" +
                       " Where B.CallTypeId NOT IN(2,3,4) AND B.LeadName<>'' AND A.ExecutiveId IN(-1, 0) AND B.MultiProject=0" +
                       " ORDER BY B.LeadName";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "SingleLead");
                da.Dispose();

                sSql = "Select Distinct A.LeadId,B.LeadName,A.CostCentreId,A.ExecutiveId,B.Mobile,B.LeadDate  from dbo.LeadExecutiveInfo  A " +
                       " INNER JOIN dbo.LeadProjectInfo P On A.LeadId=P.LeadId AND A.CostCentreId=P.CostCentreId" +
                       " INNER JOIN dbo.LeadRegister B on A.LeadId=B.LeadId " +
                       " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C on P.CostCentreId=C.CostCentreId" +
                       " Where B.CallTypeId NOT IN(2,3,4) AND B.LeadName<>'' AND A.ExecutiveId IN(-1, 0) AND B.MultiProject=1" +
                       " ORDER BY B.LeadName";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "MultiLead");
                da.Dispose();

                sSql = "Select DISTINCT A.LeadId,B.LeadName,A.CostCentreId,A.ExecutiveId,B.MultiProject,B.Mobile,B.LeadDate From dbo.LeadExecutiveInfo A " +
                       " INNER JOIN dbo.LeadProjectInfo P On A.LeadId=P.LeadId AND A.CostCentreId=P.CostCentreId" +
                       " INNER JOIN dbo.LeadRegister B on A.LeadId=B.LeadId " +
                       " Where B.CallTypeId NOT IN(2,3,4) AND B.LeadName<>'' AND A.ExecutiveId NOT IN(-1, 0) AND P.NextCallDate IS NOT NULL" +
                       " ORDER BY B.LeadName";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "AllotLead");
                da.Dispose();

                sSql = "Select ExecutiveId,Executive,SUM(NoOfLeads) NoOfLeads from( " +
                       "Select CostCentreId,G.ExecutiveId,Case When U.EmployeeName='' Then U.UserName Else U.EmployeeName End As Executive,Sum(G.NoOfLeads) NoOfLeads From (" +
                      " Select 0 CostCentreId,A.UserId ExecutiveId,0 NoOfLeads From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                      " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B ON A.PositionId=B.PositionId " +
                      " Where B.PositionType='M' " +
                      " Union All " +
                      " Select A.CostCentreId,A.ExecutiveId,Count(DISTINCT A.LeadId) NoOfLeads from dbo.LeadExecutiveInfo A " +
                      " INNER JOIN dbo.LeadProjectInfo P On A.LeadId=P.LeadId AND A.CostCentreId=P.CostCentreId" +
                      " INNER JOIN dbo.LeadRegister B ON A.LeadId=B.LeadId " +
                      " Where B.CallTypeId NOT IN(2,3,4) AND B.LeadName<>'' AND A.ExecutiveId NOT IN(-1, 0)" +
                      " And P.NextCallDate BETWEEN '" + argFrom.ToString("dd-MMM-yyyy") + "' AND '" + argTo.ToString("dd-MMM-yyyy") + "' " +
                      " GROUP BY A.CostCentreId,A.ExecutiveId) G " +
                      " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users U ON G.ExecutiveId=U.UserId " +
                      " GROUP BY CostCentreId,G.ExecutiveId,U.EmployeeName,U.UserName " +
                      ") A GROUP BY ExecutiveId,Executive";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Abstract");
                da.Dispose();

                sSql = "Select CostCentreId,ExecutiveId,Executive,SUM(NoOfLeads) NoOfLeads from( " +
                       "Select CostCentreId,G.ExecutiveId,Case When U.EmployeeName='' Then U.UserName Else U.EmployeeName End As Executive,Sum(G.NoOfLeads) NoOfLeads From (" +
                      " Select 0 CostCentreId,A.UserId ExecutiveId,0 NoOfLeads From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                      " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B ON A.PositionId=B.PositionId " +
                      " Where B.PositionType='M' " +
                      " Union All " +
                      " Select A.CostCentreId,A.ExecutiveId,Count(DISTINCT A.LeadId) NoOfLeads from dbo.LeadExecutiveInfo A " +
                      " INNER JOIN dbo.LeadProjectInfo P On A.LeadId=P.LeadId AND A.CostCentreId=P.CostCentreId" +
                      " INNER JOIN dbo.LeadRegister B ON A.LeadId=B.LeadId " +
                      " Where B.CallTypeId NOT IN(2,3,4) AND B.LeadName<>'' AND A.ExecutiveId NOT IN(-1, 0)" +
                      " And P.NextCallDate BETWEEN '" + argFrom.ToString("dd-MMM-yyyy") + "' AND '" + argTo.ToString("dd-MMM-yyyy") + "' " +
                      " GROUP BY A.CostCentreId,A.ExecutiveId) G " +
                      " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users U ON G.ExecutiveId=U.UserId " +
                      " GROUP BY CostCentreId,G.ExecutiveId,U.EmployeeName,U.UserName " +
                      ") A GROUP BY CostCentreId,ExecutiveId,Executive";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "AllotedExecutive");
                da.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return ds;
        }

        public static void Update_Allot(int argExeId, int argLeadId, int argCCId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sSql = "Update dbo.LeadExecutiveInfo Set ExecutiveId = " + argExeId + " " +
                       "Where LeadId = " + argLeadId + " ";
                SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "Update dbo.LeadProjectInfo Set ExecutiveId = " + argExeId + " " +
                       "Where LeadId = " + argLeadId + " And CostCentreId = " + argCCId;
                cmd = new SqlCommand(sSql, conn,tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                //sSql = "Update dbo.CallSheet Set ExecutiveId = " + argExeId + " " +
                //              "Where LeadId = " + argLeadId + " and ProjectId = " + argCCId;
                //BsfGlobal.OpenCRMDB();
                //cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                //cmd.ExecuteNonQuery();
                //cmd.Dispose();

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }
        }

        public static void Update_MulAllot(int argExeId, int argLeadId, int argCCId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sSql = ""; SqlCommand cmd;
                sSql = "Update dbo.LeadExecutiveInfo Set ExecutiveId = " + argExeId + " " +
                       "Where LeadId = " + argLeadId + " And CostCentreId = " + argCCId;
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                //sSql = "Update dbo.LeadRegister Set ExecutiveId = " + argExeId + " " +
                //              "Where LeadId = " + argLeadId + " and CostCentreId = " + argCCId;
                sSql = "Update dbo.LeadProjectInfo Set ExecutiveId = " + argExeId + " " +
                       "Where LeadId = " + argLeadId + " And CostCentreId = " + argCCId;
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                //sSql = "Update dbo.CallSheet Set ExecutiveId = " + argExeId + " " +
                //              "Where LeadId = " + argLeadId + " and ProjectId = " + argCCId;
                //BsfGlobal.OpenCRMDB();
                //cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                //cmd.ExecuteNonQuery();
                //cmd.Dispose();
                tran.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        public static void UpdateMultipleAllot(int argExeId, int argCCId,string argType)
        {
            try
            {
                SqlConnection conn = new SqlConnection();
                conn = BsfGlobal.OpenCRMDB();
                SqlTransaction tran;
                tran = conn.BeginTransaction();

                string sSql = ""; SqlCommand cmd; int iLeadId = 0;
                if (argType == "Single")
                {
                    sSql = "Select A.LeadId From dbo.LeadExecutiveInfo A Inner Join dbo.LeadProjectInfo P On P.LeadId=A.LeadId  " +
                            " Inner Join dbo.LeadRegister B On A.LeadId=B.LeadId  " +
                            " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C  On P.CostCentreId=C.CostCentreId Where B.CallTypeId  " +
                            " Not In (2,3,4) And P.ExecutiveId=0 And P.CostCentreId =  " + argCCId + " And B.MultiProject=0";
                    //sSql = "Select A.LeadId From dbo.LeadExecutiveInfo  A " +
                    //        " Inner Join dbo.LeadProjectInfo P On P.LeadId=A.LeadId " +
                    //        " Inner Join dbo.LeadRegister B On A.LeadId=B.LeadId " +
                    //        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C " +
                    //        " On P.CostCentreId=C.CostCentreId Where B.CallTypeId " +
                    //        " Not In (2,3,4) And A.ExecutiveId=0 And P.CostCentreId = " + argCCId;
                }
                else if (argType == "Multiple")
                {
                    sSql = "Select A.LeadId From dbo.LeadExecutiveInfo A Inner Join dbo.LeadProjectInfo P On P.LeadId=A.LeadId  " +
                            " Inner Join dbo.LeadRegister B On A.LeadId=B.LeadId And A.CostCentreId=P.CostCentreId " +
                            " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On P.CostCentreId=C.CostCentreId Where B.CallTypeId  " +
                            " Not In (2,3,4) And P.ExecutiveId=0 And P.CostCentreId = " + argCCId + " And B.MultiProject=1";
                }
                cmd = new SqlCommand(sSql, conn, tran);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        iLeadId = Convert.ToInt32(dt.Rows[i]["LeadId"]);

                        if (argType == "Single")
                        {
                            sSql = "Update dbo.LeadExecutiveInfo Set ExecutiveId = " + argExeId + " " +
                                  "Where LeadId = " + iLeadId + " ";
                        }
                        else if (argType == "Multiple")
                        {
                            sSql = "Update dbo.LeadExecutiveInfo Set ExecutiveId = " + argExeId + " " +
                                   "Where LeadId = " + iLeadId + " And CostCentreId = " + argCCId;
                        }
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        if (argType == "Single")
                        {
                            sSql = "Update dbo.LeadProjectInfo Set ExecutiveId = " + argExeId + " " +
                                  "Where LeadId = " + iLeadId + " ";
                        }
                        else if (argType == "Multiple")
                        {
                            sSql = "Update dbo.LeadProjectInfo Set ExecutiveId = " + argExeId + " " +
                                   "Where LeadId = " + iLeadId + " And CostCentreId = " + argCCId;
                        }
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
                dt.Dispose();

                tran.Commit();

                conn.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        public static DataTable GetReserveFlats(int argCCID)
        {
            DataTable dt = new DataTable();
            try
            {
                string sSql = "Select A.FlatId,B.BlockName,A.FlatNo,Case When isnull(C.FlatId,0) <> 0 then Convert(bit,1,1) " +
                              "else Convert(bit,0,0) End Sel from dbo.FlatDetails A " +
                              "Inner Join dbo.BlockMaster B on A.BlockId=B.BlockId " +
                              "Left Join dbo.ReserveFlats C on A.FlatId=C.FlatId and C.CostCentreId= " + argCCID + " " +
                              "Where A.CostCentreId=" + argCCID + " and (A.Status='U' or A.Status='R')";
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

        public static DataTable GetCCId(int argLeadID)
        {
            DataTable dt = new DataTable();
            try
            {
                string sSql = "Select CostCentreId From dbo.LeadProjectInfo Where LeadId=" + argLeadID + " And ExecutiveId=0";
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

        public static DataTable GetCCwiseExecutiveId(int argCCId)
        {
            DataTable dt = new DataTable();
            string sSql = "";
            try
            {
                sSql = "Select A.UserId ExecutiveId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As EmployeeName From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId "+
                        " Where A.UserId NOT IN(Select UserId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where CostCentreId=" + argCCId + ") "+
                        " AND B.PositionType='M'";
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

        public static DataTable GetMultiCCwiseExecutiveId(string argCCId)
        {
            DataTable dt = new DataTable();
            string sSql = "";
            try
            {
                sSql = "Select A.UserId ExecutiveId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As EmployeeName From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId Where " +
                        " A.UserId Not In (Select UserId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where CostCentreId In(" + argCCId.TrimEnd(',') + ")) AND " +
                        " B.PositionType='M'";
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

        public static int GetExecutiveId(int argCCId)
        {
            int iExeId = 0; string sSql = "";
            BsfGlobal.OpenCRMDB();

            try
            {
                //sSql = "Select G.ExecutiveId,Sum(G.CLead) CLead from (" +
                //        "Select A.ExecutiveId,Count(B.LeadId) CLead from dbo.LeadExecutiveInfo A " +
                //        "Inner Join dbo.LeadRegister B on A.LeadId=B.LeadId " +
                //        "Inner Join dbo.LeadProjectInfo C On C.LeadId=B.LeadId " +
                //        "Where B.CallTypeId not in (2,3,4) And A.ExecutiveId<>0 And " +
                //        "A.ExecutiveId Not In (Select UserId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans "+
                //        "Where CostCentreId=" + argCCId + ") Group by A.ExecutiveId) G " +
                //        "RIGHT JOIN ["+BsfGlobal.g_sWorkFlowDBName +"]..Users A ON A.UserId=G.ExecutiveId " +
                //        "INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId " +
                //        "Where B.PositionType='M' " +
                //        "GROUP BY G.ExecutiveId ORDER BY Sum(G.CLead),G.ExecutiveId";
                sSql = "SELECT G.ExecutiveId,Sum(G.CLead) CLead FROM ( SELECT A1.UserId ExecutiveId,Count(B.LeadId) CLead FROM " +
                        " [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A1 INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position A2 on A1.PositionId=A2.PositionId " +
                        " LEFT JOIN dbo.LeadExecutiveInfo A ON A.ExecutiveId=A1.UserId " +
                        " LEFT JOIN dbo.LeadRegister B on A.LeadId=B.LeadId AND CallTypeId not in (2,3,4) " +
                        " LEFT JOIN dbo.LeadProjectInfo C On C.LeadId=B.LeadId " +
                        " Where A2.PositionType='M' AND  A1.UserId " +
                        " Not In (Select UserId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where CostCentreId=" + argCCId + ") Group by A1.UserId " +
                        " ) G " +
                        " GROUP BY G.ExecutiveId ORDER BY Sum(G.CLead),G.ExecutiveId";
                
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader sdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(sdr);

                if (dt.Rows.Count > 0)
                {
                    iExeId = Convert.ToInt32(dt.Rows[0]["ExecutiveId"].ToString());
                }
                sdr.Dispose();
               BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return iExeId;
        }

        public static DataTable GetExecutive(int argCCId)
        {
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            DataTable dt = new DataTable();
            try
            {
                sSql = "SELECT G.ExecutiveId,Sum(G.CLead) CLead FROM (SELECT A1.UserId ExecutiveId,Count(B.LeadId) CLead FROM " +
                        " [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A1 INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position A2 on A1.PositionId=A2.PositionId " +
                        " LEFT JOIN dbo.LeadExecutiveInfo A ON A.ExecutiveId=A1.UserId " +
                        " LEFT JOIN dbo.LeadRegister B on A.LeadId=B.LeadId AND CallTypeId not in (2,3,4) " +
                        " LEFT JOIN dbo.LeadProjectInfo C On C.LeadId=B.LeadId " +
                        " Where A2.PositionType='M' AND  A1.UserId " +
                        " Not In (Select UserId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where CostCentreId=" + argCCId + ")" +
                        " AND A1.UserId NOT IN(Select sUserId from " + BsfGlobal.g_sWorkFlowDBName + ".dbo.UserSuperiorTrans) Group by A1.UserId " +
                        " ) G " +
                        " GROUP BY G.ExecutiveId ORDER BY Sum(G.CLead),G.ExecutiveId";

                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dt);
                dt.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable GetCostCentre()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenWorkFlowDB();
            try
            {
                sSql = "Select CostCentreId,CostCentreName from dbo.OperationalCostCentre" +
                        " Where ProjectDB in(Select ProjectName from " +
                        " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType in('B', 'L'))" +
                        " and CostCentreId not in (Select CostCentreId From dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") Order by CostCentreName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dt;
        }

        public static void InsertReserveFlats(int argCCId, string argFlatIds)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sSql = "Delete from dbo.ReserveFlats Where CostCentreId = " + argCCId;
                SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "Update dbo.FlatDetails Set Status='U' Where CostCentreId = " + argCCId + " and Status='R'"; 
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();


                if (argFlatIds != "")
                {
                    sSql = "Insert into dbo.ReserveFlats(CostCentreId,FlatId) " +
                           "Select " + argCCId + ",FlatId from dbo.FlatDetails Where FlatId in (" + argFlatIds + ")";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Update dbo.FlatDetails Set Status='R' Where FlatId in (" + argFlatIds + ")";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                }
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        public static DataTable GetPaySchFlat(int argFlatId)
        {
            DataTable dt = new DataTable();
            try
            {
                string sSql = "SELECT * FROM dbo.PaymentScheduleFlat WHERE FlatId=" + argFlatId + "";
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

        public static void InsertAllotment(int argCCId, int argFlatId, int argBuyerId, decimal argNetAmt, decimal argPaidAmt, decimal argPenaltyAmt, decimal argBalAmt, DateTime argDate,
            string argCancelType, string argRemarks, int argBlockId, string argFlatNo, DataTable dtChk, bool argChkSend,string argCCName)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sSql = "INSERT INTO dbo.AllotmentCancel(CostCentreId,FlatId,BuyerId,NetAmount,PaidAmount,PenaltyAmt,BalanceAmount,CancelDate,AllotType,Remarks) VALUES " +
                                " (" + argCCId + "," + argFlatId + "," + argBuyerId + "," + argNetAmt + "," + argPaidAmt + "," + argPenaltyAmt + "," + argBalAmt + ", " +
                                " '" + string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(CommFun.IsNullCheck(argDate, CommFun.datatypes.VarTypeDate))) + "', " +
                                " '" + argCancelType + "','" + argRemarks + "') SELECT SCOPE_IDENTITY();";
                SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                int iCancelId = int.Parse(cmd.ExecuteScalar().ToString());
                cmd.Dispose();

                sSql = string.Format("Select * from dbo.FlatCarPark Where CostCentreId={0} AND FlatId={1}", argCCId, argFlatId);
                cmd = new SqlCommand(sSql, conn, tran);
                DataTable dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    sSql = string.Format("Insert into dbo.CancelledCarPark(CancelId, CostCentreId, FlatId, TypeId, TotalCP) Values({0}, {1}, {2}, {3}, {4})",
                                        iCancelId, argCCId, argFlatId, Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["TypeId"], CommFun.datatypes.vartypenumeric)),
                                        Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["TotalCP"], CommFun.datatypes.vartypenumeric)));
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                CRM.BusinessLayer.UnitDirBL.InsertFlatCheckList(dtChk, argFlatId, "C", argChkSend, argCCId, argFlatNo);
                string sStr = "Flat No-" + argFlatNo + " in " + argCCName + " is Cancelled";
                BsfGlobal.InsertAlert("CRM-Flat-Cancel", sStr, argCCId, BsfGlobal.g_sCRMDBName);

                tran.Commit();

                BsfGlobal.InsertLog(DateTime.Now, "Flat-Cancellation-Create", "N", "Flat-Cancellation", iCancelId, argCCId, 0, 
                                    BsfGlobal.g_sCRMDBName, argFlatNo, BsfGlobal.g_lUserId);
            }
            catch(Exception ex)
            {
                tran.Rollback();
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }
        }

        public static DataTable GetAllotProject()
        {
            DataTable dt = new DataTable();
            try
            {
                //string sSql = "Select Distinct C.CostCentreName,A.CostCentreId From dbo.LeadExecutiveInfo  A " +
                //                " Inner Join dbo.LeadRegister B On A.LeadId=B.LeadId Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C " +
                //                " On A.CostCentreId=C.CostCentreId Where B.CallTypeId Not In (2,3,4) And A.ExecutiveId=0 ";
                string sSql = "Select Distinct C.CostCentreName,P.CostCentreId From dbo.LeadExecutiveInfo  A  " +
                            " Inner Join dbo.LeadRegister B On A.LeadId=B.LeadId " +
                            " Inner Join dbo.LeadprojectInfo P On P.LeadId=A.LeadId " +
                            " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C  " +
                            " On P.CostCentreId=C.CostCentreId Where B.CallTypeId Not In (2,3,4) And A.ExecutiveId=0";
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

        public static DataTable GetAllotExecutive(int argCCId)
        {
            DataTable dt = new DataTable();
            string sSql = "";
            try
            {
                sSql = "Select UserId ExecutiveId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End ExecutiveName From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId " +
                        " Where B.PositionType='M' And UserId " +
                        " Not In(Select UserId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                        " Where CostCentreId=" + argCCId + ") ";
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


        internal static DataSet GetBulkAllot()
        {
            BsfGlobal.OpenCRMDB();
            DataSet ds = new DataSet();
            try
            {
                string sSql = "";
                if (BsfGlobal.g_bHRMDB == true)
                {
                    sSql = "Select A.UserId ExecutiveId, Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As EmployeeName From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                            " Inner join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId " +
                            " Where B.PositionType='M' AND A.EmployeeId NOT IN(Select EmployeeId from [" + BsfGlobal.g_sHRMDBName + "].dbo.EmployeeMaster Where ActiveEmployment=0)";
                }
                else
                {
                    sSql = "Select A.UserId ExecutiveId, Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As EmployeeName From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                            " Inner join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId " +
                            " Where B.PositionType='M' ";
                }
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Executive");
                da.Dispose();

                if (BsfGlobal.g_bHRMDB == true)
                {
                    sSql = "Select A.UserId ExecutiveId, Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End EmployeeName From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                           " Inner join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId " +
                           " Inner join [" + BsfGlobal.g_sHRMDBName + "].dbo.EmployeeMaster C on A.EmployeeId=C.EmployeeId " +
                           " Where B.PositionType='M' AND C.ActiveEmployment=0";
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    da.Fill(ds, "BulkExecutive");
                    da.Dispose();
                }                
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return ds;
        }

        internal static void UpdateBulkAllocation(int argFromExeId, int argToExeId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sSql = "Update dbo.LeadExecutiveInfo Set ExecutiveId=" + argToExeId + " Where ExecutiveId=" + argFromExeId + " ";
                SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "Update dbo.LeadProjectInfo Set ExecutiveId=" + argToExeId + " Where ExecutiveId = " + argFromExeId + " ";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "Update dbo.CallSheet Set ExecutiveId=" + argToExeId + " Where ExecutiveId=" + argFromExeId + " ";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }
        }
    }
}
