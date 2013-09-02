using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace CRM.DataLayer
{
    class CompetitorDL
    {
        public DataTable GetCompetitorDetails(int argCCID,string argCCName)
        {
            DataTable dt = new DataTable();
            try
            {
                string sSql = "Select '" + argCCName + "' ProjectName,' ' CompetitorName,TotalFlats,TotalArea,Rate,NoOfFloors," +
                        " LandArea,FSIIndex,BuildArea,GuideLineValue, 0 LocationRate, 0 OpenCarParkCharges, 0 RegistrationCharges, " +
                        " 0 DocumentationCharges, 0 ClubFees, 0 InfraRate, 0 CMWSSBCharges, 0 OtherCharges, 0 MaintenanceRate, " +
                        " 0 CorpusFundCharges, 0 PipedGasCharges, 0 CancellationCharges from dbo.ProjectInfo " +
                        " Where CostCentreId= " + argCCID + " " +
                        " Union All " +
                        " Select ProjectName,CompetitorName,TotalFlats,CAST(TotalArea as varchar(50)) TotalArea,Rate,TotalFloors NoOfFloors,LandArea,FSIIndex," +
                        " BuildArea,GuideLineValue, LocationRate, CarParkCharges OpenCarParkCharges, RegisterCharges RegistrationCharges, " +
                        " DocumentCharges DocumentationCharges, ClubCharges ClubFees, InfraRate, CMWSSBCharges, OtherCharges, " +
                        " MaintenanceRate, CorpusFundCharges, PipedGasCharges, CancellationCharges from dbo.CompetitiveProjects A " +
                        " Inner Join dbo.CompetitorMaster B On A.CompetitorId=B.CompetitorId" +
                        " Where ProjectId in (Select ProjectId from dbo.CCCompetitorTrans Where CostCentreId= " + argCCID + ")";
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

        public DataSet GetAmenities(int argCCID)
        {
            DataSet ds = new DataSet();
            try
            {
            string sSql = "Select AmenityId,AmenityName from dbo.AmenityMaster " +
                          "Where AmenityId in (Select AmenityId from dbo.CCAmentityTrans Where CostCentreId=" + argCCID + ") " +
                          "Or AmenityId in (Select AmenityId from dbo.CompetitorAmenityTrans Where " +
                          "ProjectId in (Select ProjectId from dbo.CCCompetitorTrans Where CostCentreId=" + argCCID + "))";
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds,"Amenity");
            da.Dispose();


            sSql = "Select CompetitorId,ProjectName from dbo.CompetitiveProjects " +
                   "Where ProjectId in (Select ProjectId from dbo.CCCompetitorTrans Where CostCentreId=" + argCCID + ")";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "Competitor");
            da.Dispose();

            sSql = "Select AmenityId from dbo.CCAmentityTrans Where CostCentreId=" + argCCID + "";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "CCAmenity");
            da.Dispose();

            //sSql = "Select CompetitorId,AmenityId From dbo.CompetitorAmenityTrans " +
            //       "Where CompetitorId in (Select CompetitorId from dbo.CCCompetitorTrans Where CostCentreId=" + argCCID + ")";
            sSql = "Select B.CompetitorId,AmenityId From dbo.CompetitorAmenityTrans A" +
                    " Inner Join CompetitiveProjects B On A.ProjectId=B.ProjectId Where A.ProjectId " +
                    " In (Select ProjectId from dbo.CCCompetitorTrans Where CostCentreId=" + argCCID + ")";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "CompAmenity");
            da.Dispose();

            BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetService(int argCCID)
        {
            DataSet ds = new DataSet();
            try
            {
            string sSql = "Select ServiceId,ServiceName from dbo.NearByServicemaster " +
                          "Where ServiceId in (Select ServiceId from dbo.CCServiceTrans Where CostCentreId=" + argCCID + ") " +
                          "Or ServiceId in (Select ServiceId from dbo.CompetitorServiceTrans Where " +
                          "ProjectId in (Select ProjectId from dbo.CCCompetitorTrans Where CostCentreId=" + argCCID + "))";
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "Service");
            da.Dispose();


            sSql = "Select CompetitorId,ProjectName from dbo.CompetitiveProjects " +
                   "Where ProjectId in (Select ProjectId from dbo.CCCompetitorTrans Where CostCentreId=" + argCCID + ")";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "Competitor");
            da.Dispose();

            sSql = "Select ServiceId,ServiceDistance from dbo.CCServiceTrans Where CostCentreId=" + argCCID + "";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "CCService");
            da.Dispose();

            //sSql = "Select CompetitorId,ServiceId,ServiceDistance From dbo.CompetitorServiceTrans " +
            //       "Where CompetitorId in (Select CompetitorId from dbo.CCCompetitorTrans Where CostCentreId=" + argCCID + ")";
            sSql = "Select B.CompetitorId,ServiceId,ServiceDistance From dbo.CompetitorServiceTrans A" +
                    " Inner Join CompetitiveProjects B On A.ProjectId=B.ProjectId Where A.ProjectId  " +
                    " In (Select ProjectId from dbo.CCCompetitorTrans Where CostCentreId=" + argCCID + ")";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "CompService");
            da.Dispose();

            BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public static DataTable GetTemplate(int argProjId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sql = "Select TemplateId,TemplateName,Case When TempDoc is Null Then Convert(bit,0,0) Else Convert(bit,1,1) End FileFound from CompTemplate " +
                      "Where ProjectId = " + argProjId + " Order by TemplateName";
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static bool TemplateFound(string argName, int argProjId)
        {
            bool bans = false;
            try
            {
                DataTable dt;
                string sSql = "Select TemplateId From dbo.CompTemplate Where " +
                              " TemplateName = '" + argName + "' And ProjectId=" + argProjId + "";
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

        public static int InsertTempName(string argTempName, int argProjId)
        {
            int identity = 0;
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sSql = "Insert Into CompTemplate(TemplateName,ProjectId) Values('" + argTempName + "'," + argProjId + ") SELECT SCOPE_IDENTITY();";
                SqlCommand Command = new SqlCommand(sSql, conn, tran);
                identity = int.Parse(Command.ExecuteScalar().ToString());
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
            return identity;
        }

        public static void UpdateTemplate(int argTempId, string argTempName)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            SqlCommand cmd = null;

            try
            {
                sSql = "Update CompTemplate Set TemplateName= '" + argTempName + "' Where TemplateId = " + argTempId;
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        public static byte[] GetDocTemp(int argTempId)
        {
            SqlCommand Cmd = new SqlCommand();
            SqlDataReader OleDbReader1 = null;
            byte[] data = null;

            try
            {
                Cmd.CommandText = "SELECT TempDoc From CompTemplate Where TemplateId=" + argTempId + " and TempDoc is not null";
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

        public static void DeleteTempate(int argId)
        {
            try
            {
                BsfGlobal.OpenCRMDB();
                string sSql = "Delete from CompTemplate Where TemplateId = " + argId;
                SqlCommand Command = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                Command.ExecuteNonQuery();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        public static void TemplateAttach(int argTemId, byte[] argImageData, System.IO.FileStream fileMode,string argExt)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            SqlCommand cmd = null;
            try
            {
                if (argImageData != null)
                {
                    sSql = "Update CompTemplate Set TempDoc= @Doc,Extension='" + argExt + "' Where TemplateId = " + argTemId;
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

        public static void RemoveTempate(int argId)
        {
            try
            {
                BsfGlobal.OpenCRMDB();
                string sSql = "Update CompTemplate Set TempDoc = null Where TemplateId = " + argId;
                SqlCommand Command = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                Command.ExecuteNonQuery();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }

        }

        public static DataSet GetReport(int argCCId,int argFlatTypeId)
        {
            BsfGlobal.OpenCRMDB();

            DataSet ds = new DataSet();
            try
            {
            string sSql = "Select A.CompetitorId,B.CompetitorName,ProjectName From dbo.CompetitiveProjects A " +
                            " INNER JOIN dbo.CompetitorMaster B ON A.CompetitorId=B.CompetitorId " +
                            " Where A.ProjectId In (Select ProjectId From dbo.CCCompetitorTrans Where CostCentreId=" + argCCId + ")";
            SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "Comp");
            da.Dispose();

            sSql = "Select CompetitorId,MinArea Area,Rate,(MinArea*Rate)BasicPrice,LocationRate,(MinArea*LocationRate)LocationCharges,CarparkCharges," +
                    " RegisterCharges,DocumentCharges,ClubCharges,InfraRate,(MinArea*InfraRate)InfraCharges,CMWSSBCharges,OtherCharges," +
                    " MaintenanceRate,(MinArea*MaintenanceRate)MaintenanceCharges,CorpusFundCharges,PipedGasCharges,CancellationCharges  " +
                    " From dbo.CompetitiveProjects A Inner Join CompetitiveTrans B On A.ProjectId=B.ProjectId " +
                    " Where A.ProjectId In (Select ProjectId from dbo.CCCompetitorTrans Where CostCentreId=" + argCCId + ") And B.FlatTypeId=" + argFlatTypeId + " ";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "Details");
            da.Dispose();

            BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return ds;
        }

        #region CompetitorMaster

        public static DataTable GetCompMaster()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sql = "Select CompetitorId,CompetitorName From dbo.CompetitorMaster Order By CompetitorName";
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static void InsertCompetitorMaster(string argDesc)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            SqlCommand cmd = null;

            try
            {
                sSql = "Insert Into dbo.CompetitorMaster(CompetitorName)Values('" + argDesc + "')";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        public static void UpdateCompetitorMaster(int argCompId, string argCompName)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            SqlCommand cmd = null;

            try
            {
                sSql = "Update dbo.CompetitorMaster Set CompetitorName= '" + argCompName + "' Where CompetitorId = " + argCompId;
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        public static void DeleteCompetitorMaster(int argCompId)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            SqlCommand cmd = null;

            try
            {
                sSql = "Delete From dbo.CompetitorMaster Where CompetitorId = " + argCompId;
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        public static bool CheckCompMasterUsed(int argCompId)
        {
            bool bAns = false;

            DataTable dt;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
            sSql = "Select CompetitorId From dbo.CompetitiveProjects Where CompetitorId= " + argCompId;
            sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            dt = new DataTable();
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

        #endregion

        #region FlatTypeMaster

        public static DataTable GetFlatTypeMaster()
        {
            DataTable dt=null;
            SqlDataAdapter sda;
            string sql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sql = "Select FlatTypeId,FlatTypeName From dbo.FlatTypeMaster Order By FlatTypeName";
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable GetFlatTypeReport(int argCCId)
        {
            DataTable dt=null;
            SqlDataAdapter sda;
            string sql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sql = "Select FlatTypeId,FlatTypeName From dbo.FlatTypeMaster Order By FlatTypeName";
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static void InsertFlatTypeMaster(string argDesc)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            SqlCommand cmd = null;

            try
            {
                sSql = "Insert Into dbo.FlatTypeMaster(FlatTypeName)Values('" + argDesc + "')";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        public static void UpdateFlatTypeMaster(int argCompId, string argCompName)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            SqlCommand cmd = null;

            try
            {
                sSql = "Update dbo.FlatTypeMaster Set FlatTypeName= '" + argCompName + "' Where FlatTypeId = " + argCompId;
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        public static void DeleteFlatTypeMaster(int argCompId)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            SqlCommand cmd = null;

            try
            {
                sSql = "Delete From dbo.FlatTypeMaster Where FlatTypeId = " + argCompId;
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        public static bool CheckFlatTypeMasterUsed(int argCompId)
        {
            bool bAns = false;

            DataTable dt;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
            sSql = "Select FlatTypeId From dbo.CompetitiveTrans Where FlatTypeId= " + argCompId;
            sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            dt = new DataTable();
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

        public static DataTable GetFlatType(string argFlatTypeId)
        {
            DataTable dt=null;
            SqlDataAdapter sda;
            string sSql = "";
            string newS = ""; string stt = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                stt = argFlatTypeId.TrimEnd(',');

                for (int i = 0; i < stt.Length; i++)
                {
                    newS += stt[i].ToString();
                }
                if (newS == "")
                {
                    sSql = "SELECT FlatTypeId,FlatTypeName,0.000 MinArea,0.000 MaxArea,Convert(bit,0,0) Sel FROM dbo.FlatTypeMaster";
                }
                else
                {
                    sSql = "SELECT FlatTypeId,FlatTypeName,0.000 MinArea,0.000 MaxArea,Convert(bit,0,0) Sel FROM dbo.FlatTypeMaster WHERE FlatTypeId NOT IN (" + newS + ")";
                }
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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
            return dt;
        }

        public static DataTable GetFlatTypeComp(int argProjectId)
        {
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "SELECT ProjectId,A.FlatTypeId,A.FlatTypeName,F.MinArea,F.MaxArea FROM dbo.CompetitiveTrans F " +
                    " INNER JOIN dbo.FlatTypeMaster A ON F.FlatTypeId=A.FlatTypeId WHERE ProjectId=" + argProjectId + "";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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
                dt.Dispose();
            }
            return dt;
        }

        public static void InsertFlatTypeComp(DataTable dtFlatTypetrans, int argProjectId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "DELETE FROM dbo.CompetitiveTrans WHERE ProjectId=" + argProjectId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (dtFlatTypetrans.Rows.Count > 0)
                    {
                        for (int a = 0; a < dtFlatTypetrans.Rows.Count; a++)
                        {
                            sSql = "INSERT INTO dbo.CompetitiveTrans(ProjectId,FlatTypeId,MinArea,MaxArea) Values" +
                                " (" + argProjectId + "," + dtFlatTypetrans.Rows[a]["FlatTypeId"] + "," +
                                " " + dtFlatTypetrans.Rows[a]["MinArea"] + ", " + dtFlatTypetrans.Rows[a]["MaxArea"] + ")";
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
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static DataTable Location()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = "Select CityId,CityName From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CityMaster ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static bool CheckFlatTypeUsed(int argProjectId)
        {
            bool bAns = false;

            DataTable dt;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
            sSql = "Select FlatTypeId From dbo.CompetitiveTrans Where ProjectId= " + argProjectId;
            sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            dt = new DataTable();
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

        #endregion

    }
}
