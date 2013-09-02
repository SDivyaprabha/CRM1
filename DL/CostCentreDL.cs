using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessLayer;
using CRM.BusinessObjects;


namespace CRM.DataLayer
{
    class CostCentreDL
    {
        #region Methods

        public DataTable GetData()
        {
            DataTable dt=null;
            SqlDataAdapter sda;
            try
            {
                sda = new SqlDataAdapter("LevelProc",  BsfGlobal.OpenCRMDB() );
                dt = new DataTable();
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }
        
        public DataTable GetCompany()
        {
            DataTable dt=null;
            SqlDataAdapter sda;
            string sSql;
            try
            {
                sSql = "SELECT CompanyId,CompanyName from dbo.CompanyMaster";
                sda = new SqlDataAdapter(sSql,  BsfGlobal.OpenWorkFlowDB());
                dt = new DataTable();
                sda.Fill(dt);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public DataTable GetFaCC(string CompanyName)
        {
            DataTable dt=null;
            SqlDataAdapter sda;
            string sSql;

            try
            {
                sSql = "select CostCentreId,CostCentreName from dbo.CostCentre";
                sda = new SqlDataAdapter(sSql,  BsfGlobal.OpenWorkFlowDB());
                dt = new DataTable();
                sda.Fill(dt);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }       

        public DataTable GetExecutive()
        {
            DataTable dt=null;
            SqlDataAdapter sda;
            try
            {
                sda = new SqlDataAdapter("Select UserId ExecId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As ExecName,0 Sel From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId Where B.PositionType='M' order by EmployeeName", BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public DataTable GetLevel(int CCId)
        {
            DataTable dt=null;
            SqlDataAdapter sda;
            string str;
            //CCId = 0;
            try
            {
                if (CCId == 0)
                {
                    str = "SELECT LevelId,Levelname,0 SEL from dbo.LevelMaster";
                }
                else
                {
                    str = "SELECT A.LevelId,A.Levelname,A.Sel FROM (" +
                            " SELECT LevelId,Levelname,0 SEL from dbo.Level_Master WHERE LevelId " +
                            " NOT IN(SELECT LevelId FROM dbo.CostCentreLevels WHERE CostCentreId=" + CCId + ")" +
                            " UNION ALL" +
                            " SELECT L.LevelId,Levelname,1 as SEL FROM dbo.CostCentreLevels C" +
                            " INNER JOIN dbo.LevelMaster L ON L.LevelId=C.LevelId" +
                            " WHERE CostCentreId=" + CCId + ") A Group By A.LevelId,A.Levelname,A.Sel ORDER BY A.LevelId";
                }
                sda = new SqlDataAdapter(str,  BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public DataTable GetProj()
        {
            DataTable dt=null;
            SqlDataAdapter sda;
            try
            {
                sda = new SqlDataAdapter("SELECT Id as ProjId,Name as ProjName From [" +  BsfGlobal.g_sRateAnalDBName + "].dbo.Project_List ORDER BY ProjName",  BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }
     
        public int InsertCompetitor(CompetitorBO argObject,DataTable argdt)
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
                    sSql = "Insert into dbo.CompetitiveProjects (ProjectName,CompetitorId,TotalFlats, " +
                            "TotalArea,UnitId,Rate,TotalBlocks,LandArea,FSIIndex,BuildArea,GuideLineValue,TotalFloors,LocationId, " +
                            " ContactNo,Website,Email,Address,LocationRate,CarParkCharges,RegisterCharges,DocumentCharges,ClubCharges, " +
                            " InfraRate,CMWSSBCharges,OtherCharges,MaintenanceRate,CorpusFundCharges,PipedGasCharges,CancellationCharges) " +
                            "Values('" + argObject.ProjectName + "'," + argObject.CompetitorId + "," +
                            "" + argObject.Com_TotalFlats + ", " + argObject.Com_TotalArea + "," + argObject.Com_UnitId + "," +
                            "" + argObject.Com_Rate + "," + argObject.Com_TotalBlocks + "," + argObject.Com_LandArea + "," +
                            " " + argObject.Com_FSIIndex + "," + argObject.Com_BuildArea + "," + argObject.Com_GLV + "," +
                            " " + argObject.Com_NoofFloors + "," + argObject.Com_LocationId + ",'" + argObject.Com_Contact + "'," +
                            " '" + argObject.Com_Website + "','" + argObject.Com_Email + "','" + argObject.Com_Address + "', " +
                            " " + argObject.Com_LoctionRate + "," + argObject.Com_CarparkCharges + "," + argObject.Com_RegisterCharges + ", " +
                            " " + argObject.Com_DocumentCharges + "," + argObject.Com_ClubCharges + "," + argObject.Com_InfraRate + ", " +
                            " " + argObject.Com_CMWSSBCharges + "," + argObject.Com_OtherCharges + "," + argObject.Com_MaintenanceRate + ", " +
                            " " + argObject.Com_CorpusFundCharges + "," + argObject.Com_PipedGasCharges + "," + argObject.Com_CancellationCharges + " )" +
                            " SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(sSql, conn,tran);
                    identity = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();

                    sSql = "DELETE FROM dbo.CompetitiveTrans WHERE ProjectId=" + identity;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (argdt.Rows.Count > 0)
                    {
                        for (int a = 0; a < argdt.Rows.Count; a++)
                        {
                            sSql = "INSERT INTO dbo.CompetitiveTrans(ProjectId,FlatTypeId,MinArea,MaxArea) Values" +
                                " (" + identity + "," + argdt.Rows[a]["FlatTypeId"] + "," +
                                " " + argdt.Rows[a]["MinArea"] + ", " + argdt.Rows[a]["MaxArea"] + ")";
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
                return identity;
            }
        }

        public bool CheckProjectFound(string argProjName, int argCompId)
        {
            bool bAns = false;
            try
            {
                argProjName = CommFun.Insert_SingleQuot(argProjName);

                string sSql = "Select CompetitorId From dbo.CompetitorDetails Where ProjectName='" + argProjName + "' and CompetitorId <> " + argCompId;
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0) { bAns = true; }
                sda.Dispose();
                dt.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return bAns;
        }

        public void UpdateCompetitor(CompetitorBO argObject,int argProjectId,DataTable argdt)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "Update dbo.CompetitiveProjects set ProjectName= '" + argObject.ProjectName + "',CompetitorId=" + argObject.CompetitorId + ",TotalFlats=" + argObject.Com_TotalFlats + "," +
                            "TotalArea= " + argObject.Com_TotalArea + ",UnitId=" + argObject.Com_UnitId + ",Rate=" + argObject.Com_Rate +
                            ", TotalBlocks=" + argObject.Com_TotalBlocks + ", FSIIndex=" + argObject.Com_FSIIndex + ", GuideLineValue=" +
                            argObject.Com_GLV + ", TotalFloors=" + argObject.Com_NoofFloors + ",LandArea = " + argObject.Com_LandArea + "," +
                            " BuildArea = " + argObject.Com_BuildArea + ",LocationId=" + argObject.Com_LocationId + ",  " +
                            " ContactNo='" + argObject.Com_Contact + "',Website='" + argObject.Com_Website + "'," +
                            " Email='" + argObject.Com_Email + "',Address='" + argObject.Com_Address + "', " +
                            " LocationRate=" + argObject.Com_LoctionRate + ",CarParkCharges=" + argObject.Com_CarparkCharges + ", " +
                            " RegisterCharges=" + argObject.Com_RegisterCharges + ",DocumentCharges=" + argObject.Com_DocumentCharges + ", " +
                            " ClubCharges=" + argObject.Com_ClubCharges + ",InfraRate=" + argObject.Com_InfraRate + ", " +
                            " CMWSSBCharges=" + argObject.Com_CMWSSBCharges + ",OtherCharges=" + argObject.Com_OtherCharges + ", " +
                            " MaintenanceRate=" + argObject.Com_MaintenanceRate + ",CorpusFundCharges=" + argObject.Com_CorpusFundCharges + ", " +
                            " PipedGasCharges=" + argObject.Com_PipedGasCharges + ",CancellationCharges=" + argObject.Com_CancellationCharges + " " +
                            " Where ProjectId= " + argProjectId + "";
                    cmd = new SqlCommand(sSql,conn,tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "DELETE FROM dbo.CompetitiveTrans WHERE ProjectId=" + argProjectId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (argdt != null)
                    {
                        if (argdt.Rows.Count > 0)
                        {
                            for (int a = 0; a < argdt.Rows.Count; a++)
                            {
                                sSql = "INSERT INTO dbo.CompetitiveTrans(ProjectId,FlatTypeId,MinArea,MaxArea) Values" +
                                    " (" + argProjectId + "," + argdt.Rows[a]["FlatTypeId"] + "," +
                                    " " + argdt.Rows[a]["MinArea"] + ", " + argdt.Rows[a]["MaxArea"] + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
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

        public DataTable GetCompetitor()
        {
            DataTable dt=null;
            SqlDataAdapter sda;
            string sSql;

            try
            {
                sSql = "SELECT A.CompetitorId,CompetitorName,A.ProjectId,ProjectName from dbo.CompetitiveProjects A "+
                    " INNER JOIN CompetitorMaster B ON A.CompetitorId=B.CompetitorId Order By ProjectName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public bool CompFound(int argCompId)
        {
            bool bans = false;
            try
            {
                DataTable dt=null;
                string sSql = "Select ProjectId from dbo.CCCompetitorTrans Where ProjectId = " + argCompId;
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                if (dt.Rows.Count > 0) { bans = true; }
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

            return bans;
        }

        public void DeleteCompetitor(int argProjectId)
        {
            try
            {
                SqlCommand cmd;
                string sSql = "Delete from dbo.CompetitiveProjects Where ProjectId = " + argProjectId;
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

        public DataTable GetCompDetails(int argProjectId)
        {
            DataTable dt=null;
            try
            {
                string sSql = "Select * From dbo.CompetitiveProjects Where ProjectId=" + argProjectId + "";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
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
            return dt;
        }

        public DataTable GetFlatType(int argCCId)
        {
            DataTable dt=new DataTable();
            try
            {
                string sSql = "SELECT FlatTypeId,ProjId,Typename,Area,Rate,BaseAmt,OtherCostAmt OtherCost,"+
                    " NetAmt,Remarks FROM dbo.FlatType WHERE ProjId=" + argCCId + " ORDER BY Typename "; 
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
      
                da.Fill(dt);
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
            return dt;
        }

        public static bool StageListFound(UnitDirBL OUintDirBL,int argId)
        {
            
            bool bans = false;
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt=null;
                string sSql = "";
                BsfGlobal.OpenCRMDB();
                if (OUintDirBL.LevelId > 0)
                {

                    sSql = "Select LevelId From dbo.StageDetails Where LevelId=0 And CostCentreId=" + OUintDirBL.CCId + " And " +
                                 " BlockId=" + OUintDirBL.BlockId + " And SchType='" + OUintDirBL.SchType + "' And StageId=" + OUintDirBL.StageId + "";
                    if (argId != 0) { sSql = sSql + " and StageDetId <> " + argId; }
                    
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    dt = new DataTable();
                    da.Fill(dt);
                    da.Dispose();
                    if (dt.Rows.Count > 0) { bans = true; }
                    dt.Dispose();

                    if (bans == false)
                    {
                        sSql = "Select LevelId From dbo.StageDetails Where" +
                                " CostCentreId=" + OUintDirBL.CCId + " and BlockId=" + OUintDirBL.BlockId + " And LevelId=" + OUintDirBL.LevelId + "" +
                                " And SchType='" + OUintDirBL.SchType + "' And StageId=" + OUintDirBL.StageId + "";
                        if (argId != 0) { sSql = sSql + " and StageDetId <> " + argId; }
                        da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                        dt = new DataTable();
                        da.Fill(dt);
                        da.Dispose();
                        if (dt.Rows.Count > 0) { bans = true; }
                        dt.Dispose();
                    }
                }
                else
                {
                    sSql = "Select LevelId From dbo.StageDetails Where" +
                           " CostCentreId=" + OUintDirBL.CCId + " and BlockId=" + OUintDirBL.BlockId + " " +
                           " And SchType='" + OUintDirBL.SchType + "' And StageId=" + OUintDirBL.StageId + "";
                    if (argId != 0) { sSql = sSql + " and StageDetId <> " + argId; }
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    dt = new DataTable();
                    da.Fill(dt);
                    da.Dispose();
                    if (dt.Rows.Count > 0) { bans = true; }
                    dt.Dispose();
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

            return bans;
        }

        public static void CompititorTemplAttach(int argCompId, byte[] argImageData, System.IO.FileStream fileMode)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            SqlCommand cmd = null;
            try
            {
                if (argImageData != null)
                {
                    sSql = "Update CompetitorDetails Set TempDoc= @Doc  Where CompetitorId = " + argCompId;

                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    cmd.Parameters.Add("@Doc", SqlDbType.Binary, Convert.ToInt32(fileMode.Length)).Value = argImageData;
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

        public static byte[] GetDocTemp(int argCompId)
        {
            SqlCommand Cmd = new SqlCommand();
            SqlDataReader OleDbReader1 = null;
            byte[] data = null;
            try
            {
                Cmd.CommandText = "SELECT TempDoc From CompetitorDetails Where CompetitorId=" + argCompId + " and TempDoc is not null";
                Cmd.Connection = BsfGlobal.OpenCRMDB();
                OleDbReader1 = Cmd.ExecuteReader();
                OleDbReader1.Read();
                if (OleDbReader1.HasRows == false)
                    return data;

                long Len1 = OleDbReader1.GetBytes(0, 0, null, 0, 0);
                byte[] Array1 = new byte[Convert.ToInt32(Len1) + 1];
                OleDbReader1.GetBytes(0, 0, Array1, 0, Convert.ToInt32(Len1));
                data = Array1;

                Cmd.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }

            return data;
        } 

        #endregion

    }
}
