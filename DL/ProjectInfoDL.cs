using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using CRM.BO;

namespace CRM.DL
{
    class ProjectInfoDL
    {
        #region Project Infor

        internal static void InsertProjInfo(ProjectInfoBO ProjInfoBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand scmd = null;

            try
            {
                ssql = String.Format("Insert into ProjectInfo(CostCentreId, TotalFlats, TotalBlocks, TotalArea, FSIIndex, Rate, NoOfFloors, GuideLineValue) Values({0} , {1}, {2}, '{3}', {4}, {5}, {6}, {7})", ProjInfoBO.i_CostCentreId, ProjInfoBO.i_TotalFlats, ProjInfoBO.i_TotalBlocks, ProjInfoBO.s_TotalArea, ProjInfoBO.d_FSIIndex, ProjInfoBO.d_Rate, ProjInfoBO.i_NoOfFloors, ProjInfoBO.d_GuideLineValue);
                scmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                scmd.ExecuteNonQuery();
                scmd.Dispose();
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

        internal static bool CheckAmenityUsed(int argAId)
        {
            bool bAns =false;
            try
            {
                BsfGlobal.OpenCRMDB();
                string sSql = "Select AmenityId from CCAmentityTrans Where AmenityId = " + argAId + " " +
                              "Union All " +
                              "Select AmenityId from CompetitorAmenityTrans Where AmenityId = " + argAId;
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0) { bAns=true;}
                dt.Dispose();
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
            return bAns;
        }

        internal static string UnitUsed(int argCCId)
        {
            string sUnit = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                string sSql = "Select Unit_Name From dbo.ProjectInfo A Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.UOM B On A.UnitId=B.Unit_ID" +
                              " Where CostCentreId=" + argCCId + "";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    sUnit = dt.Rows[0]["Unit_Name"].ToString();
                }
                dt.Dispose();
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
            return sUnit;
        }

        internal static void UpdateProjInfo(ProjectInfoBO argObj)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand cmd;
            int i = 0;
            try
            {
                ssql = "Update ProjectInfo Set TotalFlats= " + argObj.i_TotalFlats + ",TotalBlocks = " + argObj.i_TotalBlocks + ", " +
                       "TotalArea = '" + argObj.s_TotalArea + "',FSIIndex = " + argObj.d_FSIIndex + ",Rate = " + argObj.d_Rate + "," +
                       "NoOfFloors = " + argObj.i_NoOfFloors + ",GuideLineValue = " + argObj.d_GuideLineValue + "," +
                       "LandCost=" + argObj.d_LandRate + ", LandArea=" + argObj.d_LandArea + ",BuildArea=" + argObj.d_BuildArea + ", " +
                       " NetLandArea=" + argObj.d_NetLandArea + ",WithHeld=" + argObj.d_WithHeld + ", " +
                       " LCBasedon='" + argObj.b_LCB + "',ProjectwiseUDS='" + argObj.b_UDS + "', " +
                       " StartDate=@StartDate," +
                       " EndDate=@EndDate," +
                       "Registration = " + argObj.d_RegValue + ", InterestBasedOn='" + argObj.c_InterestBasedOn + "' " +
                       "Where CostCentreId = " + argObj.i_CostCentreId;
                cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);

                SqlParameter parameterDate = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@StartDate" };
                if (Convert.ToDateTime(CommFun.IsNullCheck(argObj.dt_StartDate, CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                    parameterDate.Value = System.Data.SqlTypes.SqlDateTime.Null;
                else
                    parameterDate.Value = argObj.dt_StartDate;
                cmd.Parameters.Add(parameterDate);

                parameterDate = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@EndDate" };
                if (Convert.ToDateTime(CommFun.IsNullCheck(argObj.dt_EndDate, CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                    parameterDate.Value = System.Data.SqlTypes.SqlDateTime.Null;
                else
                    parameterDate.Value = argObj.dt_EndDate;
                cmd.Parameters.Add(parameterDate);

                i = cmd.ExecuteNonQuery();
                cmd.Dispose();

                if (i == 0)
                {
                    ssql = "Insert into ProjectInfo(CostCentreId,TotalFlats,TotalBlocks,TotalArea,FSIIndex,Rate,NoOfFloors,GuideLineValue,LandCost,LandArea," +
                        " LCBasedon,ProjectwiseUDS,BuildArea,NetLandArea,WithHeld,StartDate,EndDate,Registration,CancelPenalty,InterestBasedOn) " +
                           "Values( " + argObj.i_CostCentreId + "," + argObj.i_TotalFlats + "," + argObj.i_TotalBlocks + ",'" + argObj.s_TotalArea + "'," +
                           "" + argObj.d_FSIIndex + "," + argObj.d_Rate + "," + argObj.i_NoOfFloors + "," + argObj.d_GuideLineValue + "," +
                           "" + argObj.d_LandRate + "," + argObj.d_LandArea + ",'" + argObj.b_LCB + "','" + argObj.b_UDS + "'," + argObj.d_BuildArea + ", " +
                           " " + argObj.d_NetLandArea + "," + argObj.d_WithHeld + ",@StartDate,@EndDate, " +
                           "" + argObj.d_RegValue + "," + argObj.d_CancelPenalty + ", '" + argObj.c_InterestBasedOn + "')";
                    cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);

                    parameterDate = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@StartDate" };
                    if (Convert.ToDateTime(CommFun.IsNullCheck(argObj.dt_StartDate, CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                        parameterDate.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        parameterDate.Value = argObj.dt_StartDate;
                    cmd.Parameters.Add(parameterDate);

                    parameterDate = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@EndDate" };
                    if (Convert.ToDateTime(CommFun.IsNullCheck(argObj.dt_EndDate, CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                        parameterDate.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        parameterDate.Value = argObj.dt_EndDate;
                    cmd.Parameters.Add(parameterDate);

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
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
        }

        internal static DataTable PopulateProjInfo(ProjectInfoBO ProjInfoBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtProjInfo = null;
            SqlDataAdapter sda;

            try
            {
                ssql = String.Format("Select * from ProjectInfo Where CostCentreId={0}", ProjInfoBO.i_CostCentreId);
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtProjInfo = new DataTable();
                sda.Fill(dtProjInfo);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtProjInfo;
        }

        internal static DataTable GetPenalty(int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtProjInfo = null;
            SqlDataAdapter sda;

            try
            {
                ssql = String.Format("Select BlockingType,BlockingPenalty,BookingType,BookingPenalty,CancelType,CancelPenalty" +
                    " from ProjectInfo Where CostCentreId={0}", argCCId);
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtProjInfo = new DataTable();
                sda.Fill(dtProjInfo);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtProjInfo;
        }

        internal static void UpdatePenalty(int argCCId, string argBlkType, decimal argBlkAmt, string argBType, decimal argBAmt, string argFType, decimal argFAmt)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand cmd;
            try
            {
                ssql = "Update ProjectInfo Set BlockingType='" + argBlkType + "',BlockingPenalty=" + argBlkAmt + "," +
                    " BookingType='" + argBType + "',BookingPenalty=" + argBAmt + "," +
                    " CancelType='" + argFType + "',CancelPenalty=" + argFAmt + " Where CostCentreId=" + argCCId + "";
                cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
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

        #endregion

        #region Services

        internal static DataTable SelectServices(int argCCID, string argType)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtSer = null;
            SqlDataAdapter sda;

            try
            {
                if (argType == "P")
                {
                    ssql = String.Format("Select Case When B.ServiceId IS NULL then Convert(bit, 0, 0) else CONVERT(bit, 1, 1) End" +
                          " as Sel, A.ServiceId, A.ServiceName, B.ServiceDistance from NearByServicemaster A" +
                          " Left Join CCServiceTrans B ON A.ServiceId=B.ServiceId AND B.CostCentreId={0}", argCCID);
                }
                else
                {
                    ssql = String.Format("Select Case When B.ServiceId IS NULL then Convert(bit, 0, 0) else CONVERT(bit, 1, 1) End" +
                          " as Sel, A.ServiceId, A.ServiceName, B.ServiceDistance from NearByServicemaster A" +
                          " Left Join CompetitorServiceTrans B ON A.ServiceId=B.ServiceId AND B.ProjectId={0}", argCCID);
                }
                
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtSer = new DataTable();
                sda.Fill(dtSer);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtSer;
        }

        internal static DataTable SelectServicesTrans(ProjectInfoBO NBSBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtSerTrans = null;
            SqlDataAdapter sda;

            try
            {
                ssql = String.Format("Select * from CCServiceTrans Where CostCentreId={0}", NBSBO.i_CostCentreId);
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtSerTrans = new DataTable();
                sda.Fill(dtSerTrans);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtSerTrans;
        }

        internal static DataTable SelectServicesMaster()
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtSer = null;
            SqlDataAdapter sda;

            try
            {
                ssql = String.Format("Select ServiceId, ServiceName from NearByServicemaster");
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtSer = new DataTable();
                sda.Fill(dtSer);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtSer;
        }

        internal static int InsertNBSMaster(string argName)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand scmd = null;
            int identity = 0;
            try
            {
                ssql = String.Format("Insert into NearByServicemaster(ServiceName) Values('{0}')SELECT scope_identity()", argName);
                scmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                identity = int.Parse(scmd.ExecuteScalar().ToString());
                scmd.Dispose();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return identity;
        }

        internal static void InsertNBS(DataTable argdt, int argCCID, string argType)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand scmd = null;

            try
            {
                if (argType == "P") { ssql = "Delete from CCServiceTrans Where CostCentreId = " + argCCID; }
                else { ssql = "Delete from CompetitorServiceTrans Where ProjectId = " + argCCID; }
                scmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                scmd.ExecuteNonQuery();
                scmd.Dispose();


                for (int i = 0; i < argdt.Rows.Count; i++)
                {
                    if (argType == "P")
                    {
                        ssql = "Insert into CCServiceTrans(CostCentreId,ServiceId,ServiceDistance) Values (" + argCCID + "," + argdt.Rows[i]["ServiceId"].ToString() + ",'" + argdt.Rows[i]["ServiceDistance"].ToString() + "')";
                    }
                    else
                    {
                        ssql = "Insert into CompetitorServiceTrans(ProjectId,ServiceId,ServiceDistance) Values (" + argCCID + "," + argdt.Rows[i]["ServiceId"].ToString() + ",'" + argdt.Rows[i]["ServiceDistance"].ToString() + "')";
                    }
                    scmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                    scmd.ExecuteNonQuery();
                    scmd.Dispose();
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
        }

        internal static void DeleteNBSMaster(ProjectInfoBO NBSBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand scmd = null;

            try
            {
                ssql = String.Format("Delete NearByServicemaster Where ServiceName='{0}'", NBSBO.s_NBSName);
                scmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                scmd.ExecuteNonQuery();
                scmd.Dispose();
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

        internal static void DeleteNBS(ProjectInfoBO NBSBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand scmd = null;

            try
            {
                ssql = String.Format("Delete CCServiceTrans Where CostCentreId={0}", NBSBO.i_CostCentreId);
                scmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                scmd.ExecuteNonQuery();
                scmd.Dispose();
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

        #endregion

        #region Amenities

        internal static DataTable SelectAmenities(int argCCID,string argType)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtAme = null;
            SqlDataAdapter sda;

            try
            {
                if (argType == "P")
                {
                    ssql = String.Format("Select A.AmenityId, A.AmenityName, Case When B.AmenityId IS NULL then" +
                                    " Convert(bit, 0, 0) else CONVERT(bit, 1, 1) End as Sel from AmenityMaster A" +
                                    " Left Join CCAmentityTrans B ON A.AmenityId=B.AmenityId AND B.CostCentreId={0}", argCCID);
                }
                else
                {
                    ssql = String.Format("Select A.AmenityId, A.AmenityName, Case When B.AmenityId IS NULL then" +
                                    " Convert(bit, 0, 0) else CONVERT(bit, 1, 1) End as Sel from AmenityMaster A" +
                                    " Left Join CompetitorAmenityTrans B ON A.AmenityId=B.AmenityId AND B.ProjectId={0}", argCCID);
                }
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtAme = new DataTable();
                sda.Fill(dtAme);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtAme;
        }

        internal static DataTable SelectAmenitiesTrans(ProjectInfoBO AmenityBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtAmeTrans = null;
            SqlDataAdapter sda;

            try
            {
                ssql = String.Format("Select * from CCAmentityTrans Where CostCentreId={0}", AmenityBO.i_CostCentreId);
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtAmeTrans = new DataTable();
                sda.Fill(dtAmeTrans);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtAmeTrans;
        }

        internal static DataTable SelectAmenitiesMaster()
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtAmeMas = null;
            SqlDataAdapter sda;

            try
            {
                ssql = String.Format("Select AmenityId, AmenityName from AmenityMaster Order by AmenityName");
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtAmeMas = new DataTable();
                sda.Fill(dtAmeMas);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtAmeMas;
        }

        internal static int InsertAmenitiesMaster(string argName)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand cmd = null;
            int identity=0;
            try
            {
                ssql = "Insert into AmenityMaster(AmenityName) Values('" + argName + "') SELECT scope_identity();";
                cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                identity = int.Parse(cmd.ExecuteScalar().ToString());
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
            return identity;
        }

        internal static void UpdateAmenitiesMaster(int argAID,string argName)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand cmd = null;
            try
            {
                ssql = "Update AmenityMaster Set AmenityName = '" + argName + "' Where AmenityId= " + argAID;
                cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
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

        internal static void UpdateServicesMaster(int argSID, string argName)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand cmd = null;
            try
            {
                ssql = "Update NearByServicemaster Set ServiceName = '" + argName + "' Where ServiceId= " + argSID;
                cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
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

        internal static void InsertAmenities(DataTable argdt, int argCCID,string argType)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand scmd = null;

            try
            {

                if (argType == "P"){ssql = "Delete from CCAmentityTrans Where CostCentreId = " + argCCID; }
                else { ssql = "Delete from CompetitorAmenityTrans Where ProjectId = " + argCCID; }
                scmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                scmd.ExecuteNonQuery();
                scmd.Dispose();


                for (int i = 0; i < argdt.Rows.Count; i++)
                {
                    if (argType == "P")
                    {
                        ssql = "Insert into CCAmentityTrans(CostCentreId,AmenityId) Values (" + argCCID + "," + argdt.Rows[i]["AmenityId"].ToString() + ")";
                    }
                    else
                    {
                        ssql = "Insert into CompetitorAmenityTrans(ProjectId,AmenityId) Values (" + argCCID + "," + argdt.Rows[i]["AmenityId"].ToString() + ")";
                    }
                    scmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                    scmd.ExecuteNonQuery();
                    scmd.Dispose();
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
        }

        internal static void DeleteAmenitiesMaster(int argAId)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand scmd = null;

            try
            {
                ssql = String.Format("Delete AmenityMaster Where AmenityId={0}", argAId);
                scmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                scmd.ExecuteNonQuery();
                scmd.Dispose();
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

        internal static void DeleteAmenities(ProjectInfoBO AmenityBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand scmd = null;

            try
            {
                ssql = String.Format("Delete CCAmentityTrans Where CostCentreId={0}", AmenityBO.i_CostCentreId);
                scmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                scmd.ExecuteNonQuery();
                scmd.Dispose();
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

        #endregion

        #region Competitor

        internal static DataTable SelectCompetitor(ProjectInfoBO CompBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtComp = null;
            SqlDataAdapter sda;

            try
            {
                ssql = " Select A.ProjectId,A.CompetitorId,C.CompetitorName,A.ProjectName,Case When B.ProjectId IS NULL  " +
                     " then Convert(bit, 0, 0) else CONVERT(bit, 1, 1) End as Sel from dbo.CompetitiveProjects A  " +
                     " LEFT JOIN dbo.CompetitorMaster C ON A.CompetitorId=C.CompetitorId " +
                     " Left Join dbo.CCCompetitorTrans B ON A.ProjectId=B.ProjectId AND B.CostCentreId=" + CompBO.i_CostCentreId + "";
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtComp = new DataTable();
                sda.Fill(dtComp);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtComp;
        }

        internal static DataTable SelectCompetitorTrans(ProjectInfoBO CompBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtCompTrans = null;
            SqlDataAdapter sda;

            try
            {
                ssql = String.Format("Select * from CCCompetitorTrans Where CostCentreId={0}", CompBO.i_CostCentreId);
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtCompTrans = new DataTable();
                sda.Fill(dtCompTrans);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtCompTrans;
        }

        internal static void InsertCompetitor(ProjectInfoBO CompBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = ""; 
            SqlCommand scmd = null;

            try
            {
                ssql = String.Format("Insert Into dbo.CCCompetitorTrans(CostCentreId, ProjectId) Values({0}, {1})", CompBO.i_CostCentreId, CompBO.i_CompProjectId);
                scmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                scmd.ExecuteNonQuery();
                scmd.Dispose();
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

        internal static void DeleteCompetitor(ProjectInfoBO CompBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand scmd = null;

            try
            {
                ssql = String.Format("Delete CCCompetitorTrans Where CostCentreId={0}", CompBO.i_CostCentreId);
                scmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                scmd.ExecuteNonQuery();
                scmd.Dispose();
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

        #endregion

        #region CarPark

        public static void UpdateCarParkSlot(int argBlockId, int argCCID, SqlConnection argConn, SqlTransaction argTrans)
        {
            string sSql = "";
            try
            {
                sSql = "Select SUM(TotalCP)TotalCP,TypeId " +
                    " From FlatCarPark Where FlatId In (Select FlatId From FlatDetails " +
                    " Where BlockId=" + argBlockId + " And CostCentreId=" + argCCID + ") Group By TypeId";
                SqlCommand cmd = new SqlCommand(sSql, argConn, argTrans);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();

                if (dt != null)
                {
                    //if (dt.Rows.Count == 0)
                    //{
                    //    sSql = "Update CarParkMaster Set AllottedSlots = 0 Where BlockId= " + argBlockId + " and CostCentreId = " + argCCID + "  and TypeId=" +  + "";
                    //    cmd = new SqlCommand(sSql, argConn, argTrans);
                    //    cmd.ExecuteNonQuery();
                    //    cmd.Dispose();

                    //}
                    //else if (dt.Rows.Count > 0)
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sSql = "Update CarParkMaster Set AllottedSlots = " + dt.Rows[i]["TotalCP"] + " Where BlockId= " + argBlockId + " and CostCentreId = " + argCCID + "  and TypeId=" + dt.Rows[i]["TypeId"] + "";
                            cmd = new SqlCommand(sSql, argConn, argTrans);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }

                }

                //sSql = "Select SUM(OpenCP) OpenCP, SUM(ClosedCP) ClosedCP,SUM(TerraceCP) TerraceCP " +
                //              "from FlatCarPark Where FlatId in (Select FlatId from FlatDetails where BlockId=" + argBlockId + " and CostCentreId=" + argCCID + ")";
                //cmd = new SqlCommand(sSql, argConn, argTrans);
                //dr = cmd.ExecuteReader();
                //dt = new DataTable();
                //dt.Load(dr);
                //dr.Close();
                //cmd.Dispose();

                //int iOpen = 0;
                //int iClosed = 0;
                //int iTerrace = 0;

                //if (dt.Rows.Count > 0)
                //{
                //    iOpen = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["OpenCP"].ToString(), CommFun.datatypes.vartypenumeric));
                //    iClosed = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["ClosedCP"].ToString(), CommFun.datatypes.vartypenumeric));
                //    iTerrace = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["TerraceCP"].ToString(), CommFun.datatypes.vartypenumeric));
                //}
                //dt.Dispose();

                //sSql = "Update CarParkMaster Set AllottedSlots = " + iOpen + " Where BlockId= " + argBlockId + " and CostCentreId = " + argCCID + "  and TypeId=1";
                //cmd = new SqlCommand(sSql, argConn, argTrans);
                //cmd.ExecuteNonQuery();
                //cmd.Dispose();

                //sSql = "Update CarParkMaster Set AllottedSlots = " + iClosed + " Where BlockId= " + argBlockId + " and CostCentreId = " + argCCID + "  and TypeId=2";
                //cmd = new SqlCommand(sSql, argConn, argTrans);
                //cmd.ExecuteNonQuery();
                //cmd.Dispose();

                //sSql = "Update CarParkMaster Set AllottedSlots = " + iTerrace + " Where BlockId= " + argBlockId + " and CostCentreId = " + argCCID + "  and TypeId=3";
                //cmd = new SqlCommand(sSql, argConn, argTrans);
                //cmd.ExecuteNonQuery();
                //cmd.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            
        }

        public static void UpdateCarParkSlotAllot(int argBlockId, int argCCID)
        {
            SqlConnection conn = new SqlConnection();
            SqlTransaction trans;
            conn = BsfGlobal.OpenCRMDB();
            trans = conn.BeginTransaction();
            try
            {
                UpdateCarParkSlot(argBlockId, argCCID, conn, trans);
                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region OtherCost

        internal static DataTable SelectOtherCost(string argNew,int argbArea,int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            DataTable dtOC = null;
            SqlDataAdapter sda;

            try
            {
                if (argNew == "")
                {
                    //sSql = "SELECT OtherCostId,OtherCostName,'+' Flag,0.00 Amount,Convert(bit,0,0) Sel,SysDefault,Area FROM OtherCostMaster Where Area='" + argbArea + "'";
                    sSql = "SELECT A.OtherCostId,OtherCostName,'+' Flag,0.00 Amount,Convert(bit,0,0) Sel,SysDefault,Area " +
                            " FROM dbo.OtherCostMaster A Inner Join dbo.CCOtherCost B On A.OtherCostId=B.OtherCostId " +
                            " Where Area=" + argbArea + " And B.CostCentreId=" + argCCId + "";
                }
                else
                {
                    //sSql = "SELECT OtherCostId,OtherCostName,'+' Flag,0.00 Amount,Convert(bit,0,0) Sel,SysDefault,Area FROM OtherCostMaster WHERE OtherCostId NOT IN (" + argNew + ") And Area='" + argbArea + "'";
                    sSql = "SELECT A.OtherCostId,OtherCostName,'+' Flag,0.00 Amount,Convert(bit,0,0) Sel,SysDefault,Area " +
                            " FROM dbo.OtherCostMaster A Inner Join dbo.CCOtherCost B On A.OtherCostId=B.OtherCostId " +
                            " WHERE A.OtherCostId NOT IN (" + argNew + ") And Area=" + argbArea + " And B.CostCentreId=" + argCCId + "";
                }
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dtOC = new DataTable();
                sda.Fill(dtOC);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtOC;
        }

        internal static DataTable GetOtherCost(int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            DataTable dtOC = null;
            SqlDataAdapter sda;

            try
            {
                //sSql = "Select A.OtherCostId,A.OtherCostName, Case When B.OtherCostId IS NULL then " +
                //        " Convert(bit, 0, 0) else CONVERT(bit, 1, 1) End as Sel from dbo.OtherCostMaster A " +
                //        " Left Join dbo.CCOtherCost B ON A.OtherCostId=B.OtherCostId AND B.CostCentreId=" + argCCId + "";
                sSql = "Select SysDefault,A.OtherCostId,A.OtherCostName, " +
                        " Case When B.OtherCostId IS NOT NULL OR SysDefault=1 then  Convert(bit, 1, 1) " +
                        " else CONVERT(bit, 0, 0) End as Sel from dbo.OtherCostMaster A  " +
                        " Left Join dbo.CCOtherCost B ON A.OtherCostId=B.OtherCostId " +
                        " AND B.CostCentreId=" + argCCId + " Order By A.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dtOC = new DataTable();
                sda.Fill(dtOC);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtOC;
        }
        
        public static void UpdateOC(int argCCID,DataTable argdt)
        {
            SqlConnection conn = new SqlConnection();
            SqlTransaction trans; 
            string sSql = ""; SqlCommand scmd;
            conn = BsfGlobal.OpenCRMDB();
            trans = conn.BeginTransaction();
            try
            {
                sSql = "Delete From dbo.CCOtherCost Where CostCentreId=" + argCCID + "";
                scmd = new SqlCommand(sSql, conn,trans);
                scmd.ExecuteNonQuery();
                scmd.Dispose();

                if (argdt != null)
                {
                    for (int i = 0; i < argdt.Rows.Count; i++)
                    {
                        sSql = "Insert Into dbo.CCOtherCost (OtherCostId,CostCentreId) Values (" + argdt.Rows[i]["OtherCostId"] + "," +
                        " " + argCCID + ") ";
                        scmd = new SqlCommand(sSql, conn,trans);
                        scmd.ExecuteNonQuery();
                        scmd.Dispose();
                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static DataTable GetServiceOtherCost()
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            DataTable dtOC = null;
            SqlDataAdapter sda;

            try
            {
                sSql = "Select OtherCostId,OtherCostName,Case When Service=1 " +
                        " then  Convert(bit, 1, 1)  else CONVERT(bit, 0, 0) End as Service From dbo.OtherCostMaster Order By OtherCostName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dtOC = new DataTable();
                sda.Fill(dtOC);
                dtOC.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtOC;
        }

        internal static DataTable GetExcludeOtherCost(int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            DataTable dtOC = null;
            SqlDataAdapter sda;

            try
            {
                //sSql = "Select A.OtherCostId,A.OtherCostName,Case when B.OtherCostId is null then Convert(bit,0,0) " +
                //        " else Convert(bit,1,1) end Sel From dbo.OtherCostMaster A " +
                //        " Inner Join dbo.CCOtherCost CO On CO.OtherCostId=A.OtherCostId " +
                //        " Left Join dbo.OXGross B On CO.CostCentreId=B.CostCentreId " +
                //        " Where CO.CostCentreId=" + argCCId + "";
                sSql = "Select A.OtherCostId,A.OtherCostName,Case when B.OtherCostId is null then Convert(bit,0,0)  else Convert(bit,1,1) end Sel " +
                        " From dbo.OtherCostMaster A  Inner Join dbo.CCOtherCost CO On CO.OtherCostId=A.OtherCostId  " +
                        " Left Join dbo.OXGross B On CO.CostCentreId=B.CostCentreId And B.OtherCostId=CO.OtherCostId  Where CO.CostCentreId=" + argCCId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dtOC = new DataTable();
                sda.Fill(dtOC);
                dtOC.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtOC;
        }

        internal static void UpdateServiceOC(DataTable argdt, string argType, int argCCId)
        {
            SqlConnection conn = new SqlConnection();
            SqlTransaction trans;
            string sSql = ""; SqlCommand scmd;
            conn = BsfGlobal.OpenCRMDB();
            trans = conn.BeginTransaction();
            try
            {
                if (argType == "Service")
                {
                    if (argdt != null)
                    {
                        for (int i = 0; i < argdt.Rows.Count; i++)
                        {
                            sSql = "Update dbo.OtherCostMaster Set Service='" + argdt.Rows[i]["Service"] + "' Where OtherCostId=" + argdt.Rows[i]["OtherCostId"] + "";
                            scmd = new SqlCommand(sSql, conn, trans);
                            scmd.ExecuteNonQuery();
                            scmd.Dispose();
                        }
                    }
                }
                else 
                {
                    if (argdt != null)
                    {
                        sSql = "Delete From dbo.OXGross Where CostCentreId=" + argCCId + "";
                        scmd = new SqlCommand(sSql, conn, trans);
                        scmd.ExecuteNonQuery();
                        scmd.Dispose();

                        for (int i = 0; i < argdt.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(argdt.Rows[i]["Sel"]) == true)
                            {
                                sSql = "Insert Into dbo.OXGross(CostCentreId,OtherCostId)Values(" + argCCId + "," + argdt.Rows[i]["OtherCostId"] + ")";
                                scmd = new SqlCommand(sSql, conn, trans);
                                scmd.ExecuteNonQuery();
                                scmd.Dispose();
                            }
                        }
                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static DataTable GetOtherCostType()
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            DataTable dtOC = null;
            SqlDataAdapter sda;

            try
            {
                sSql = "Select A.OtherCostId,A.OtherCostName,A.OCTypeId From OtherCostMaster A Left Join OCType B On A.OCTypeId=B.OCTypeId " +
                        " Order By A.OtherCostName ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dtOC = new DataTable();
                sda.Fill(dtOC);
                dtOC.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtOC;
        }

        internal static DataTable GetOCTypeMaster()
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            DataTable dtOC = null;
            SqlDataAdapter sda;

            try
            {
                sSql = "Select OCTypeId,OCTypeName From dbo.OCType Order By OCTypeId ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dtOC = new DataTable();
                sda.Fill(dtOC);
                dtOC.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtOC;
        }

        internal static void UpdateOCType(DataTable argdt)
        {
            SqlConnection conn = new SqlConnection();
            SqlTransaction trans;
            string sSql = ""; SqlCommand scmd;
            conn = BsfGlobal.OpenCRMDB();
            trans = conn.BeginTransaction();
            try
            {
                if (argdt != null)
                {
                    for (int i = 0; i < argdt.Rows.Count; i++)
                    {
                        sSql = "Update dbo.OtherCostMaster Set OCTypeId=" + argdt.Rows[i]["OCTypeId"] + " Where OtherCostId=" + argdt.Rows[i]["OtherCostId"] + "";
                        scmd = new SqlCommand(sSql, conn, trans);
                        scmd.ExecuteNonQuery();
                        scmd.Dispose();
                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        public static void UpdateSortOrder(DataTable dt)
        {
            SqlCommand cmd;
            string sSql = "";
            try
            {
                dt.AcceptChanges();
                BsfGlobal.OpenCRMDB();
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    int iOCId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["OtherCostId"].ToString(), CommFun.datatypes.vartypenumeric));
                    int iOrder = i + 1;
                    sSql = "Update dbo.OtherCostMaster Set SortOrder=" + iOrder + " Where OtherCostId=" + iOCId + " ";
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
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
        }

        #endregion


        internal static bool CheckUsedInFlatsPayment(int argCCId, int argOCId)
        {
            BsfGlobal.OpenCRMDB();
            bool bCheck = false;
            try
            {
                string sSql = "Select COUNT(*) from dbo.FlatOtherCost Where OtherCostId=" + argOCId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                int i_Count = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                cmd.Dispose();

                if (i_Count > 0)
                    bCheck = true;
                else
                {
                    sSql = "Select COUNT(*) from dbo.FlatOtherArea Where OtherCostId=" + argOCId + "";
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    i_Count = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                    cmd.Dispose();

                    if (i_Count > 0)
                        bCheck = true;
                    else
                    {
                        sSql = "Select COUNT(*) from dbo.FlatOtherInfra Where OtherCostId=" + argOCId + "";
                        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                        i_Count = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                        cmd.Dispose();

                        if (i_Count > 0)
                            bCheck = true;
                        else
                            bCheck = false;
                    }
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
            return bCheck;
        }
    }
}
