using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace CRM.DataLayer
{
    class BrokerDL
    {
        public static DataTable getCostCentre(int argBrokerId)
        {
            SqlDataAdapter da;
            DataTable dt = null;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.CostCentreId,A.CostCentreName, Case When ISNULL(D.CommType,'')='A' Then 'Amount' "+
                        " When ISNULL(D.CommType,'')='P' Then 'Percentage' Else 'None' End CommType, Case When ISNULL(D.PerBased,'')='N' Then 'NetAmount' "+
                        " When ISNULL(D.PerBased,'')='B' Then 'BaseAmount' Else 'None' End PerBased, " +
                        " ISNULL(D.Percentage, 0) CommPer, ISNULL(D.Amount,0)Amount, CONVERT(bit,0,0) Sel, " +
                        " Case When D.Percentage>0 Then 'WPM' Else 'CRM' End BrokerType " +
                        " From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre A " +
                        " Left Join [" + BsfGlobal.g_sWPMDBName + "].dbo.SORegister C ON A.CostCentreId=C.CostCentreId AND C.ContractorId=" + argBrokerId +
                        " Left Join [" + BsfGlobal.g_sWPMDBName + "].dbo.SOBrokerTrans D ON C.SORegisterId=D.SORegId AND D.CommType='P' " +
                        " Where A.ProjectDB in(Select ProjectName from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType IN('B','L')) " +
                        " AND A.CostCentreId NOT IN(Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ")" +
                        " ORDER BY CostCentreName";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
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

        public static DataTable getBrokerCostCentre(int argBrokerId, int argVendorId)
        {   
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = " Select CostCentreId,CostCentreName,PerBased,CommType,CommPer,Amount,Sel,BrokerType from( "+
                              " Select A.CostCentreId, A.CostCentreName, "+
                              " Case When ISNULL(D.PerBased,'')='N' Then 'NetAmount' When ISNULL(D.PerBased,'')='B' Then 'BaseAmount' Else 'None' End PerBased, " +
                              " Case When ISNULL(D.CommType,'')='P' Then 'Percentage' When ISNULL(D.CommType,'')='A' Then 'Amount' Else 'None' End CommType, " +
                              " ISNULL(B.CommPer,0) CommPer,ISNULL(B.Amount,0) Amount, " +
                              " Case When B.CostCentreId IS NOT NULL THEN CONVERT(bit,1,1) ELSE CONVERT(bit,0,0) END Sel, " +
                              " 'WPM' BrokerType  From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre A  " +
                              " Inner Join dbo.BrokerCC B on A.CostCentreId=B.CostCentreId  " +
                              " Inner Join dbo.BrokerDet E on B.BrokerId=E.BrokerId  " +
                              " Inner Join [" + BsfGlobal.g_sWPMDBName + "].dbo.SORegister C ON A.CostCentreId=C.CostCentreId " +
                              " Inner Join [" + BsfGlobal.g_sWPMDBName + "].dbo.SOBrokerTrans D ON C.SORegisterId=D.SORegId   " +
                              " Where A.ProjectDB IN(Select ProjectName from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType IN('B','L'))  " +
                              " AND A.CostCentreId NOT IN(Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=1) " +
                              " AND B.BrokerId=" + argBrokerId + " AND E.VendorId=" + argVendorId + " AND C.ContractorId=" + argVendorId + " AND D.CommType='P' "+
                              " AND D.Percentage<>0 " +
                              " UNION ALL   " +
                              " Select A.CostCentreId, A.CostCentreName, "+
                              " Case When ISNULL(B.PerBased,'')='N' Then 'NetAmount' When ISNULL(B.PerBased,'')='B' Then 'BaseAmount' Else 'None' End PerBased, " +
                              " Case When ISNULL(B.CommType,'')='P' Then 'Percentage' When ISNULL(B.CommType,'')='A' Then 'Amount' Else 'None' End CommType, " +
                              " ISNULL(B.CommPer,0) CommPer,ISNULL(B.Amount,0) Amount, " +
                              " Case When B.CostCentreId IS NOT NULL THEN CONVERT(bit,1,1) ELSE CONVERT(bit,0,0) END Sel, " +
                              " 'CRM' BrokerType From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre A  " +
                              " LEFT Join dbo.BrokerCC B on A.CostCentreId=B.CostCentreId AND B.BrokerId=" + argBrokerId + " " +
                              " Where A.ProjectDB IN(Select ProjectName from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType IN('B','L'))  " +
                              " AND A.CostCentreId NOT IN(Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                              " AND A.CostCentreId NOT IN(Select CostCentreId from [" + BsfGlobal.g_sWPMDBName + "].dbo.SORegister A " +
                              " INNER JOIN [" + BsfGlobal.g_sWPMDBName + "].dbo.SOBrokerTrans B ON A.SORegisterId=B.SORegId " +
                              " Where A.ContractorId=" + argVendorId + " AND B.CommType='P' AND B.Percentage<>0) " +
                              " ) X ORDER BY CostCentreName";
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
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

        public static DataTable getVendor(int argBrokerId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "";
                if (argBrokerId != 0)
                    sSql = "Select A.VendorId,VendorName BrokerName From [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorMaster A" +
                            " Where Service=1 And ServiceTypeId=1";
                else
                    sSql = "Select A.VendorId,A.VendorName BrokerName From [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorMaster A " +
                            " Where Service=1 And ServiceTypeId=1 And A.VendorId Not In(Select VendorId From BrokerDet)";
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
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

        public static DataTable getVendorDetails(int argBrokerId)
        {
            SqlDataAdapter da;
            DataTable dt = null;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.*,B.VendorName FROM dbo.BrokerDet A " +
                        " Inner Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorMaster B On A.VendorId=B.VendorId" +
                        " Where BrokerId=" + argBrokerId;
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
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

        public static void UpdateBroker(int argBrokerId, int argVendorId, DataTable argdt)
        {
            SqlConnection conn; SqlTransaction tran; conn = BsfGlobal.OpenCRMDB();
            tran = conn.BeginTransaction();
            SqlCommand cmd;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                if (argBrokerId == 0)
                {
                    sSql = String.Format("INSERT INTO dbo.BrokerDet(VendorId) VALUES({0}) SELECT SCOPE_IDENTITY()", argVendorId);
                    cmd = new SqlCommand(sSql, conn,tran);
                    argBrokerId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();
                    //CommFun.InsertLog(DateTime.Now, "Broker Details-Add", "N", "Add Broker Details", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                    BsfGlobal.InsertLog(DateTime.Now, "Broker Details-Add", "N", "Add Broker Details", argBrokerId, 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                }
                else
                {
                    sSql = String.Format("UPDATE dbo.BrokerDet SET VendorId={0}" +
                        " WHERE BrokerId={1}", argVendorId, argBrokerId);
                    cmd = new SqlCommand(sSql, conn,tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    //CommFun.InsertLog(DateTime.Now, "Broker Details-Edit", "E", "Edit Broker Details", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                    BsfGlobal.InsertLog(DateTime.Now, "Broker Details-Edit", "E", "Edit Broker Details", argBrokerId, 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                }

                sSql = String.Format("DELETE FROM dbo.BrokerCC WHERE BrokerId={0}", argBrokerId);
                cmd = new SqlCommand(sSql, conn,tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                DataView dview = new DataView(argdt) { RowFilter = "Sel='" + true + "'" };
                DataTable dtFilter = new DataTable();
                dtFilter = dview.ToTable();
                if (dtFilter != null)
                {
                    for (int i = 0; i < dtFilter.Rows.Count; i++)
                    {
                        string sCommType = CommFun.IsNullCheck(dtFilter.Rows[i]["CommType"], CommFun.datatypes.vartypestring).ToString();
                        if (sCommType == "Percentage")
                            sCommType = "P";
                        else if (sCommType == "Amount")
                            sCommType = "A";
                        else
                            sCommType = "N";

                        string sPerBased = CommFun.IsNullCheck(dtFilter.Rows[i]["PerBased"], CommFun.datatypes.vartypestring).ToString();
                        if (sPerBased == "NetAmount")
                            sPerBased = "N";
                        else if (sPerBased == "BaseAmount")
                            sPerBased = "B";
                        else
                            sPerBased = "N";

                        sSql = "INSERT INTO dbo.BrokerCC(BrokerId,CostCentreId,CommPer,Amount,CommType,PerBased) VALUES" +
                                " (" + argBrokerId + "," + dtFilter.Rows[i]["CostCentreId"] + "," + dtFilter.Rows[i]["CommPer"] + "" +
                                "," + dtFilter.Rows[i]["Amount"] + ",'" + sCommType + "','" + sPerBased + "')";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
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
                BsfGlobal.g_CRMDB.Close();
            }
        }

    }
}
