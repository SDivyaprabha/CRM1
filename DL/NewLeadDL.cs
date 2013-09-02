using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using CRM.DataLayer;
using CRM.BusinessLayer;

namespace CRM
{
    class NewLeadDL
    {
        #region Methods

        public static DataTable GetCat()
        {
            DataTable dt;
            SqlDataAdapter sda;
            string sSql = "";
            sSql = " SELECT LeadTypeId,LeadTypeName FROM dbo.LeadType ORDER BY LeadTypeName";

            try
            {
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable GetExecutiveList()
        {
            DataTable dt;
            SqlDataAdapter sda;
            String sSql = String.Format("Select UserId ExecId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As ExecName From [{0}].dbo.Users A Inner Join [{0}].dbo.Position B on A.PositionId=B.PositionId Where B.PositionType='M'", BsfGlobal.g_sWorkFlowDBName);
            try
            {
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable FillProjType()
        {
            DataTable dtRec;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            string sql = string.Empty;

            try
            {
                sql = String.Format("SELECT ProjectTypeId,ProjectTypeName FROM {0}.dbo.ProjectType ORDER BY ProjectTypeName", BsfGlobal.g_sRateAnalDBName);
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dtRec = new DataTable();
                sda.Fill(dtRec);
                dtRec.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtRec;
        }

        public static DataTable GetCallType()
        {
            DataTable dt;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            String sSql = "SELECT CallTypeId,Description,SortId FROM dbo.CallType ORDER BY SortId ";
            try
            {
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        //NewForm

        public static DataTable GetReligion()
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtReligion;
            SqlDataAdapter sda;

            try
            {
                ssql = "SELECT ReligionId as Id,ReligionName as Name FROM dbo.ReligionMaster WHERE ReligionName <> ''";
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtReligion = new DataTable();
                sda.Fill(dtReligion);
                dtReligion.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtReligion;
        }

        public static DataTable GetNationality()
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtReligion;
            SqlDataAdapter sda;

            try
            {
                ssql = "SELECT NationalityId as Id,Description as Name FROM dbo.NationalityMaster WHERE Description <> ''";
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtReligion = new DataTable();
                sda.Fill(dtReligion);
                dtReligion.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtReligion;
        }

        public static DataTable GetProfession()
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtReligion;
            SqlDataAdapter sda;

            try
            {
                ssql = "SELECT ProfessionId as Id,Description as Name FROM dbo.ProfessionMaster WHERE Description <> ''";
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtReligion = new DataTable();
                sda.Fill(dtReligion);
                dtReligion.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtReligion;
        }

        public static DataTable GetCityMaster()
        {
            BsfGlobal.OpenWorkFlowDB();
            DataTable dt = null;
            try
            {
                string ssql = "SELECT CityId, CityName, StateId, CountryId FROM dbo.CityMaster WHERE CityName<>'' ORDER BY CityName";
                SqlCommand cmd = new SqlCommand(ssql, BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dt;
        }

        public static DataTable GetStateMaster()
        {
            BsfGlobal.OpenWorkFlowDB();
            DataTable dt = null;
            try
            {
                string ssql = "SELECT StateId, StateName, CountryId FROM dbo.StateMaster WHERE StateName<>'' ORDER BY StateName";
                SqlCommand cmd = new SqlCommand(ssql, BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dt;
        }

        public static DataTable GetCountry()
        {
            BsfGlobal.OpenWorkFlowDB();
            DataTable dt = null;
            try
            {
                string ssql = "SELECT CountryId as Id, CountryName as Name FROM dbo.Countrymaster WHERE CountryName<>'' ORDER BY CountryName";
                SqlCommand cmd = new SqlCommand(ssql, BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dt;
        }

        public static DataTable GetApartment()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtApartment;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT ApartmentId as Id,Description FROM dbo.Apartment Order By ApartmentId", BsfGlobal.g_CRMDB);
                dtApartment = new DataTable();
                sda.Fill(dtApartment);
                dtApartment.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtApartment;
        }

        public static DataTable GetApartmentsize()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtApartSize;
            SqlDataAdapter sda;
            try
            {
                sda = new SqlDataAdapter("SELECT ApartmentSizeId as Id, str(cast(ApartmentSizeFrom as int),10) + ' -' + str(cast(ApartmentSizeTo as int),10) as Description FROM dbo.ApartmentSize", BsfGlobal.g_CRMDB);
                dtApartSize = new DataTable();
                sda.Fill(dtApartSize);
                dtApartSize.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtApartSize;
        }

        public static DataTable GetApartmentType()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtApartType;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT ApartmentTypeId as Id,Description as Name FROM dbo.ApartmentType Order By Description", BsfGlobal.g_CRMDB);
                dtApartType = new DataTable();
                sda.Fill(dtApartType);
                dtApartType.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtApartType;
        }

        public static DataTable GetCostPreference()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtCostPref;
            SqlDataAdapter sda;

            try
            {
                string Ssql = "SELECT CostPreferenceId as Id, str(cast(CostPreferenceFrom  as decimal(18,3))) + '   -' + str(cast(CostPreferenceTo as decimal(18,3))) as CostPreference FROM dbo.CostPreference";
                //string Ssql = "SELECT CostPreferenceId as Id, (cast(CostPreferenceFrom  as varchar(50)) + '   -' + " +
                //              " cast(CostPreferenceTo as varchar(50))) as CostPreference FROM dbo.CostPreference";
                sda = new SqlDataAdapter(Ssql, BsfGlobal.g_CRMDB);
                dtCostPref = new DataTable();
                sda.Fill(dtCostPref);
                dtCostPref.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtCostPref;
        }

        public static DataTable GetFacilityMaster()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtGetFacility = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("Select 0 LeadId,FacilityId as Id, Description as Name, CONVERT(bit,0,1) Sel from dbo.FacilityMaster Order By FacilityId", BsfGlobal.g_CRMDB);
                dtGetFacility = new DataTable();
                sda.Fill(dtGetFacility);
                dtGetFacility.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtGetFacility;
        }

        public static DataTable GetAreaMaster()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtAreMaster = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                //sSql = "SELECT AreaId as Id, Description as Name FROM dbo.LeadAreaMaster Order By AreaId";
                sda = new SqlDataAdapter("SELECT 0 LeadId,AreaId as Id, Description as Name, CONVERT(bit,0,1) Sel FROM dbo.LeadAreaMaster Order By AreaId", BsfGlobal.g_CRMDB);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dtAreMaster = new DataTable();
                sda.Fill(dtAreMaster);
                dtAreMaster.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtAreMaster;
        }

        internal static DataTable GetPossessMaster()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtPosessMaster = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT 0 LeadId,PossessId as Id, Description as Name, CONVERT(bit,0,1) Sel FROM dbo.PossessMaster Order By PossessId", BsfGlobal.g_CRMDB);
                dtPosessMaster = new DataTable();
                sda.Fill(dtPosessMaster);
                dtPosessMaster.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtPosessMaster;
        }

        public static DataTable GetProject()
        {
            DataTable dtProject;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                //string sSql = "Select CostCentreId,CostCentreName from dbo.OperationalCostCentre Order by CostCentreName";
                sSql = "Select CostCentreId,CostCentreName from dbo.OperationalCostCentre" +
                        " Where ProjectDB in(Select ProjectName from " +
                        " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType in('B','L'))" +
                        " and CostCentreId not in (Select CostCentreId From dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") Order by CostCentreName";
                BsfGlobal.OpenWorkFlowDB();
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                dtProject = new DataTable();
                sda.Fill(dtProject);
                dtProject.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dtProject;

        }

        public static DataTable GetNature()
        {
            DataTable dtNature;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                sSql = "Select NatureId,Description From NatureMaster Order By SortID";
                BsfGlobal.OpenCRMDB();
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dtNature = new DataTable();
                sda.Fill(dtNature);
                dtNature.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtNature;

        }

        public static DataTable GetSource()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtSource;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT SourceID Id,Sourcename Description FROM dbo.EnquirySource ORDER BY SourceName", BsfGlobal.g_CRMDB);
                dtSource = new DataTable();
                sda.Fill(dtSource);
                dtSource.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtSource;

        }

        public static DataTable GetSubSource()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtsubSource = new DataTable();
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * from dbo.EnquirySubSource";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dtsubSource);
                dtsubSource.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }

            return dtsubSource;

        }

        public static DataTable GetExecutive()
        {
            BsfGlobal.OpenWorkFlowDB();
            DataTable dt = new DataTable();
            SqlDataAdapter sda;
            string ssql = "";

            try
            {
                ssql = "Select UserId ExecId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As ExecName From dbo.Users A Inner Join dbo.Position B on A.PositionId=B.PositionId Where B.PositionType='M'";
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dt;
        }

        internal static DataTable GetEmpStatus()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtEmployment;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT EmploymentId as Id,Description FROM dbo.Employment Order By Description", BsfGlobal.g_CRMDB);
                dtEmployment = new DataTable();
                sda.Fill(dtEmployment);
                dtEmployment.Dispose();
            }

            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtEmployment;
        }

        internal static DataTable GetGuestHouse()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtGuestHouse;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT GuestHouseId as Id,Description FROM dbo.GuestHouse Order By Description", BsfGlobal.g_CRMDB);
                dtGuestHouse = new DataTable();
                sda.Fill(dtGuestHouse);
                dtGuestHouse.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtGuestHouse;
        }

        internal static DataTable GetStay()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtStay;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT StayId as Id,Description FROM dbo.Stay Order By Description", BsfGlobal.g_CRMDB);
                dtStay = new DataTable();
                sda.Fill(dtStay);
                dtStay.Dispose();
            }

            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtStay;
        }

        internal static DataTable GetIncome()
        {
            BsfGlobal.OpenCRMDB();
            DataTable GetIncome;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT IncomeId as Id, str(cast(Incomefrom as float),10) + '   -' + str(cast(IncomeTo as float),12) as Description  FROM dbo.Income", BsfGlobal.g_CRMDB);
                GetIncome = new DataTable();
                sda.Fill(GetIncome);
                GetIncome.Dispose();
            }

            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return GetIncome;
        }

        internal static DataTable GetBroker(int argCCId)
        {
            DataTable dtBroker;
            SqlDataAdapter sda;
            try
            {
                string sSql = "";
                if (BsfGlobal.g_bWPMDB == true && argCCId != 0)
                {
                    sSql = "Select A.ContractorId BrokerId,B.VendorName BrokerName From [" + BsfGlobal.g_sWPMDBName + "].dbo.SORegister A " +
                            " Inner Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorMaster B On A.ContractorId=B.VendorId " +
                            " Where A.CostCentreId=" + argCCId + " And A.ServiceType='B' And A.Approve='Y'";
                }
                else
                {
                    sSql = "Select 0 BrokerId,'' BrokerName ";
                }
                ////sSql = "SELECT 0 BrokerId,'' BName BrokerName FROM dbo.BrokerDet ORDER BY BName";
                //sSql = "SELECT 0 BrokerId,'' BrokerName FROM dbo.BrokerDet";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dtBroker = new DataTable();
                sda.Fill(dtBroker);
                dtBroker.Dispose();
            }

            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtBroker;
        }

        public static DataTable GetFacility()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtGetFacility = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("Select FacilityId as Id, Description as Name, Sel, LeadId from dbo.Facility Order By FacilityId", BsfGlobal.g_CRMDB);
                dtGetFacility = new DataTable();
                sda.Fill(dtGetFacility);
                dtGetFacility.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtGetFacility;
        }

        public static DataTable GetArea()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtAreMaster = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT AreaId as Id, Description as Name, Sel, LeadId FROM dbo.Area Order By AreaId", BsfGlobal.g_CRMDB);
                dtAreMaster = new DataTable();
                sda.Fill(dtAreMaster);
                dtAreMaster.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtAreMaster;
        }

        internal static DataTable GetPossess()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtPosessMaster = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT PossessId as Id, Description as Name, Sel, LeadId FROM dbo.Possess Order By PossessId", BsfGlobal.g_CRMDB);
                dtPosessMaster = new DataTable();
                sda.Fill(dtPosessMaster);
                dtPosessMaster.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtPosessMaster;
        }

        internal static DataTable GetBusinessType(int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            SqlDataAdapter sda;
            try
            {
                string sSql = "Select BusinessType,LandId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister " +
                                " Where ProjectName in(Select ProjectDB from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                                " Where CostCentreId=" + argCCId + ")";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowProjectGrid(int argLeadId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            string sSql = "";
            try
            {
                if (argLeadId == 0)
                    sSql = "Select CostCentreId,CostCentreName, CAST( 0 As Int) CampaignId,0 ExecutiveId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                            " Where ProjectDB in(Select ProjectName from " +
                            " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType in('B','L'))" +
                            " and CostCentreId not in (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                            " Where UserId=" + BsfGlobal.g_lUserId + ") Order by CostCentreName";
                else
                    sSql = "Select CostCentreId,CostCentreName,CampaignId=Isnull((Select Top 1 CampaignId From LeadProjectInfo Where CostCentreId=A.CostCentreId And LeadId=" + argLeadId + "),0), " +
                            " ExecutiveId=IsNull((Select Top 1 ExecutiveId From LeadProjectInfo Where CostCentreId=A.CostCentreId And LeadId=" + argLeadId + "),0) from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre A " +
                            " Where ProjectDB in(Select ProjectName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType in('B','L')) and CostCentreId " +
                            " Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans  Where UserId=" + BsfGlobal.g_lUserId + ") Order by CostCentreName";
                BsfGlobal.OpenCRMDB();
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(dt);
               // da.Fill(dt, "Project");
                da.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;

        }

        internal static DataTable ShowCampaignGrid()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            string sSql = "";
            try
            {
                //sSql = "Select CCId,CampaignId,CampaignName,RequestId From CampaignDetails Order By CampaignName";
                sSql = "Select B.CostCentreId CCId,A.CampaignId,A.CampaignName,A.RequestId From dbo.CampaignDetails A " +
                        " Inner Join dbo.CampaignTrans B On A.CampaignId=B.CampaignId Order By CampaignName";
                BsfGlobal.OpenCRMDB();
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(dt);
                da.Dispose();
                
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;

        }

        #endregion

        #region Insert Data's

        public static string GetType(string argqry, SqlConnection conn, SqlTransaction tran)
        {
            DataTable dt = new DataTable();
            string sType = "";
            SqlCommand sda;
            SqlDataReader sdr;
            try
            {
                sda = new SqlCommand(argqry, conn, tran);
                sdr = sda.ExecuteReader();
                dt.Load(sdr);
                if (dt.Rows.Count > 0)
                { sType = dt.Rows[0]["PositionType"].ToString(); }
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }

            return sType;
        }

        //internal static int InsertLeadDetails(NewLeadBO oLeedBO, string s_Mode, DataTable dtEnqTrans, bool UpdateLead, SqlConnection conn, SqlTransaction tran, DataTable dtFinal, string argFlatNo, bool argChk)
        //{
        //    SqlCommand cmd;
        //    string sSql = "";
        //    DataTable dtLeadDetails = null;
        //    string s_NextCallDate = "";
        //    string s_LeadDate = "";
        //    string validUpto = "", dateFinal = "";
        //    bool vip = false;
        //    decimal dAdvAmt = 0; decimal dRate = 0;

        //    s_LeadDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(oLeedBO.DE_LeadDate));
        //    s_NextCallDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(oLeedBO.DE_NextCallDate));
        //    oLeedBO.i_ExecutiveId = oLeedBO.UserId;

        //    sSql = "Select B.PositionType From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B On A.PositionId=B.PositionId" +
        //           " Where A.UserId=" + oLeedBO.UserId + "";
        //    string sUserType = GetType(sSql, conn, tran);
        //    if (sUserType == "M")
        //        oLeedBO.i_ExecutiveId = oLeedBO.UserId;
        //    else
        //        oLeedBO.i_ExecutiveId = 0;


        //    if (s_NextCallDate == "01/Jan/0001")
        //    {
        //        s_NextCallDate = "";
        //    }
        //    if (oLeedBO.VIP == 1) vip = true;
        //    else vip = false;
        //    if (s_Mode == "A" && UpdateLead == false)
        //    {
        //        if (oLeedBO.CallType == "Finalization" || oLeedBO.CallType == "Finalisation" || oLeedBO.CallType == "Block")
        //        {
        //            sSql = String.Format("Insert into dbo.LeadRegister(LeadName, LeadDate, LeadType, CallTypeId,CampaignId,UserId,VIP,CostCentreId,ExecutiveId,TypeId,CostId,Mobile,Email)" +
        //            " Values('{0}', '{1}', '{2}' , {3},{4},{5},'{6}',{7},{8},{9},{10},'{11}','{12}' ) SELECT SCOPE_IDENTITY();", oLeedBO.s_LeadName, s_LeadDate, oLeedBO.s_LeadTypeName, oLeedBO.i_LeadCallTypeId, oLeedBO.i_SubSourceId, oLeedBO.UserId, vip, oLeedBO.ProjID, oLeedBO.i_ExecutiveId, oLeedBO.i_FinApartmentTypeId, oLeedBO.i_FinApartmentCostId, oLeedBO.Mobile, oLeedBO.Email);
        //        }
        //        else
        //        {
        //            sSql = String.Format("Insert into dbo.LeadRegister(LeadName, LeadDate, LeadType, CallTypeId, NextCallDate,CampaignId,UserId,VIP,CostCentreId,ExecutiveId,TypeId,CostId,Mobile,Email)" +
        //            " Values('{0}', '{1}', '{2}' , {3} , '{4}',{5},{6},'{7}',{8},{9},{10},{11},'{12}','{13}') SELECT SCOPE_IDENTITY();", oLeedBO.s_LeadName, s_LeadDate, oLeedBO.s_LeadTypeName, oLeedBO.i_LeadCallTypeId, s_NextCallDate, oLeedBO.i_SubSourceId, oLeedBO.UserId, vip, oLeedBO.ProjID, oLeedBO.i_ExecutiveId, oLeedBO.i_FinApartmentTypeId, oLeedBO.i_FinApartmentCostId, oLeedBO.Mobile, oLeedBO.Email);
        //        }
        //        cmd = new SqlCommand(sSql, conn, tran);
        //        oLeedBO.EnquiryID = int.Parse(cmd.ExecuteScalar().ToString());
        //        oLeedBO.i_LeadId = oLeedBO.EnquiryID;
        //        cmd.Dispose();
        //    }
        //    else
        //    {
        //        dtLeadDetails = new DataTable();
        //        sSql = string.Format("Update dbo.LeadRegister Set LeadName='{0}', LeadDate='{1}', LeadType='{2}', CallTypeId={3}, NextCallDate='{4}',CampaignId={5},UserId={6},VIP='{7}',TypeId={8},CostId={9},Mobile='{10}',Email='{11}',CostCentreId={12},ExecutiveId={13} where LeadId={14}",
        //            oLeedBO.s_LeadName, s_LeadDate, oLeedBO.s_LeadTypeName, oLeedBO.i_LeadCallTypeId, s_NextCallDate, oLeedBO.i_SubSourceId, oLeedBO.UserId, vip, oLeedBO.i_FinApartmentTypeId, oLeedBO.i_FinApartmentCostId, oLeedBO.Mobile, oLeedBO.Email, oLeedBO.ProjID, oLeedBO.i_ExecutiveId, oLeedBO.i_LeadId);

        //        cmd = new SqlCommand(sSql, conn, tran);
        //        cmd.ExecuteNonQuery();
        //        cmd.Dispose();

        //        //sSql = string.Format("Update CallSheet Set ExecutiveId='{0}', ProjId='{1}', TrnDate='{2}', NextCallDate='{3}' where LeadId={4}",
        //        //    oLeedBO.s_LeadName, s_LeadDate, oLeedBO.s_LeadTypeName, oLeedBO.i_LeadCallTypeId, s_NextCallDate, oLeedBO.i_SubSourceId, oLeedBO.UserId, vip, oLeedBO.i_LeadId);

        //        //cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB, tran);
        //        //cmd.ExecuteNonQuery();
        //        //cmd.Dispose();
        //    }


        //    if (UpdateLead == false)
        //    {
        //        sSql = ""; int iEntryId = 0;
        //        if (oLeedBO.CallType == "Finalization" || oLeedBO.CallType == "Finalisation" || oLeedBO.CallType == "Block")
        //        {
        //            if (dtEnqTrans.Rows[0]["ValidUpto"].ToString() != "")
        //            {
        //                validUpto = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(dtEnqTrans.Rows[0]["ValidUpto"].ToString()));
        //            }
        //            if (dtEnqTrans.Rows[0]["FinaliseDate"].ToString() != "")
        //                dateFinal = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(dtEnqTrans.Rows[0]["FinaliseDate"].ToString()));

        //            if (dtEnqTrans.Rows.Count > 0)
        //            {
        //                sSql = "INSERT INTO dbo.CallSheet (ExecutiveId,LeadId,ProjectId,TrnDate,CallFF,CallTypeID)VALUES" +
        //                    " (" + oLeedBO.ExecutiveID + "," + oLeedBO.EnquiryID + "," + oLeedBO.ProjID + ",'" + s_LeadDate + "','Fresh'," + oLeedBO.i_LeadCallTypeId + ")" +
        //                        " SELECT SCOPE_IDENTITY();";
        //                cmd = new SqlCommand(sSql, conn, tran);
        //                iEntryId = int.Parse(cmd.ExecuteScalar().ToString());
        //                cmd.Dispose();

        //                sSql = "INSERT INTO dbo.BuyerDetail(CostCentreId,ExecutiveId,LeadId,EntryId,FlatId,Status,CustomerType," +
        //                " PaymentOption,BranchId,LoanPer,LoanAccNo,BrokerId,BrokerComm,BrokerAmount,ValidUpto,FinaliseDate,PostSaleExecId,Advance) Values(" + oLeedBO.ProjID + "," +
        //                " " + oLeedBO.ExecutiveID + "," + oLeedBO.EnquiryID + "," + iEntryId + "," + oLeedBO.FlatID + "," +
        //                " '" + dtEnqTrans.Rows[0]["Status"] + "','" + dtEnqTrans.Rows[0]["CustomerType"] + "','" + dtEnqTrans.Rows[0]["PaymentOption"] + "'," + dtEnqTrans.Rows[0]["BranchId"] + ", " +
        //                " " + dtEnqTrans.Rows[0]["LoanPer"] + ",'" + dtEnqTrans.Rows[0]["LoanAccNo"] + "'," + dtEnqTrans.Rows[0]["BrokerId"] + "," + dtEnqTrans.Rows[0]["ComPer"] + "," +
        //                " " + dtEnqTrans.Rows[0]["ComAmount"] + ",'" + validUpto + "','" + dateFinal + "'," +
        //                " " + dtEnqTrans.Rows[0]["PostSaleExecId"] + "," + dtEnqTrans.Rows[0]["AdvAmt"] + ")";
        //                cmd = new SqlCommand(sSql, conn, tran);
        //                cmd.ExecuteNonQuery();
        //                cmd.Dispose();
        //            }
        //            if (oLeedBO.CallType == "Block")
        //                sSql = String.Format("Update dbo.FlatDetails Set Status='B',LeadId={0} WHERE FlatId={1} AND CostCentreId={2}", oLeedBO.EnquiryID, oLeedBO.FlatID, oLeedBO.ProjID);
        //            else
        //                sSql = "Update dbo.FlatDetails Set Status='S',LeadId=" + oLeedBO.EnquiryID + " WHERE FlatId=" + oLeedBO.FlatID + " AND CostCentreId=" + oLeedBO.ProjID + "";
        //            cmd = new SqlCommand(sSql, conn, tran);
        //            cmd.ExecuteNonQuery();
        //            cmd.Dispose();

        //            if (oLeedBO.CallType == "Block")
        //            {
        //                sSql = "INSERT INTO dbo.BlockUnits(CostCentreId,FlatId,CustomerType,LeadId,BlockUpto,Date,BlockType) Values(" + oLeedBO.ProjID + "," +
        //                 " " + oLeedBO.FlatID + ",'" + dtEnqTrans.Rows[0]["CustomerType"] + "'," + oLeedBO.EnquiryID + ",'" + validUpto + "'," +
        //                 " '" + Convert.ToDateTime(DateTime.Now).ToString("MM-dd-yyyy") + "','B')";
        //                cmd = new SqlCommand(sSql, conn, tran);
        //                cmd.ExecuteNonQuery();
        //                cmd.Dispose();
        //            }

        //            sSql = "INSERT INTO dbo.LeadFlatInfo (LeadId,Buyer,FlatId,CostCentreId)VALUES(" + oLeedBO.EnquiryID + ",1," + oLeedBO.FlatID + "," + oLeedBO.ProjID + ")";
        //            cmd = new SqlCommand(sSql, conn, tran);
        //            cmd.ExecuteNonQuery();
        //            cmd.Dispose();

        //            sSql = "SELECT UserName FROM dbo.UserLogin Where UserName='" + oLeedBO.s_LeadName + "'";
        //            cmd = new SqlCommand(sSql, conn, tran);
        //            SqlDataReader sdr; DataTable dt = new DataTable();
        //            sdr = cmd.ExecuteReader();
        //            dt.Load(sdr);
        //            if (dt.Rows.Count > 0)
        //                oLeedBO.s_LeadName = oLeedBO.s_LeadName + '1';

        //            sSql = "INSERT INTO dbo.UserLogin(UserName,Password,Email,LeadId) VALUES" +
        //                " ('" + oLeedBO.s_LeadName + "','" + oLeedBO.s_LeadName + "','" + oLeedBO.Email + "'," + oLeedBO.EnquiryID + ")";
        //            cmd = new SqlCommand(sSql, conn, tran);
        //            cmd.ExecuteNonQuery();
        //            cmd.Dispose();


        //            SqlDataReader sdr2, sdr1; DataTable dt1; int iStgId = 0, iTemplateId = 0;
        //            int iDateAfter = 0, iDuration = 0; string sDurType = ""; DateTime SchDate;
        //            sSql = "Select TemplateId,PreStageTypeId from dbo.PaymentScheduleFlat Where FlatId=" + oLeedBO.FlatID + " And PreStageTypeId=-1";
        //            cmd = new SqlCommand(sSql, conn, tran);
        //            sdr2 = cmd.ExecuteReader();
        //            dt1 = new DataTable();
        //            dt1.Load(sdr2); cmd.Dispose();


        //            if (dt1.Rows.Count > 0)
        //            {
        //                iStgId = Convert.ToInt32(dt1.Rows[0]["PreStageTypeId"]);
        //                iTemplateId = Convert.ToInt32(dt1.Rows[0]["TemplateId"]);
        //            }
        //            dt1.Dispose();

        //            if (iStgId == -1)
        //            {
        //                sSql = "Select A.TemplateId,A.DateAfter,A.Duration,A.Durationtype from dbo.PaymentScheduleFlat A" +
        //                    " Left Join dbo.ProgressBillRegister B On A.FlatId=B.FlatId " +
        //                    " Where A.FlatId=" + oLeedBO.FlatID + " And A.TemplateId>=" + iTemplateId + "" +
        //                    " And A.PaymentSchId Not In " +
        //                    " (Select PaySchId From dbo.ProgressBillRegister Where FlatId=" + oLeedBO.FlatID + ")";
        //                cmd = new SqlCommand(sSql, conn, tran);
        //                sdr1 = cmd.ExecuteReader();
        //                dt1 = new DataTable();
        //                dt1.Load(sdr1);
        //                cmd.Dispose();

        //                for (int i = 0; i < dt1.Rows.Count; i++)
        //                {
        //                    iTemplateId = Convert.ToInt32(dt1.Rows[i]["TemplateId"]);
        //                    iDateAfter = Convert.ToInt32(dt1.Rows[i]["DateAfter"]);
        //                    iDuration = Convert.ToInt32(dt1.Rows[i]["Duration"]);
        //                    sDurType = dt1.Rows[i]["DurationType"].ToString();
        //                    SchDate = Convert.ToDateTime(dateFinal);

        //                    if (sDurType == "D")
        //                    { if (iDateAfter == 0) SchDate = SchDate.AddDays(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }
        //                    else if (sDurType == "M")
        //                    { if (iDateAfter == 0) SchDate = SchDate.AddMonths(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }


        //                    sSql = "Update dbo.PaymentScheduleFlat Set SchDate='" + Convert.ToDateTime(SchDate).ToString("dd-MMM-yyyy") + "'" +
        //                        " Where TemplateId=" + iTemplateId + " And FlatId=" + oLeedBO.FlatID + "";
        //                    cmd = new SqlCommand(sSql, conn, tran);
        //                    cmd.ExecuteNonQuery();
        //                    cmd.Dispose();
        //                }

        //            }

        //            sSql = "Select Rate,AdvAmount From dbo.FlatDetails Where FlatId=" + oLeedBO.FlatID + "";
        //            cmd = new SqlCommand(sSql, conn, tran);
        //            SqlDataReader dr = cmd.ExecuteReader();
        //            DataTable dtA = new DataTable();
        //            dtA.Load(dr);
        //            if (dtA.Rows.Count > 0)
        //            {
        //                dAdvAmt = Convert.ToDecimal(dtA.Rows[0]["AdvAmount"]);
        //                dRate = Convert.ToDecimal(dtA.Rows[0]["Rate"]);
        //                if (dAdvAmt != Convert.ToDecimal(dtEnqTrans.Rows[0]["AdvAmt"]))
        //                {
        //                    sSql = "Update dbo.FlatDetails Set AdvAmount=" + dtEnqTrans.Rows[0]["AdvAmt"] + " WHERE FlatId=" + oLeedBO.FlatID + " ";
        //                    cmd = new SqlCommand(sSql, conn, tran);
        //                    cmd.ExecuteNonQuery();
        //                    cmd.Dispose();

        //                    PaymentScheduleDL.InsertFlatScheduleI(oLeedBO.FlatID, conn, tran);
        //                }
        //            }

        //            InsertChangeRate(oLeedBO.FlatID, dRate, Convert.ToDecimal(dtEnqTrans.Rows[0]["Rate"]), conn, tran);

        //            if (dtFinal != null)
        //                UnitDirDL.InsertFlatChk(dtFinal, oLeedBO.FlatID, "F", argChk, argFlatNo, oLeedBO.ProjID, conn, tran);
        //        }
        //        else
        //        {
        //            if (s_Mode == "A")
        //            {
        //                sSql = "INSERT INTO dbo.CallSheet (ExecutiveId,LeadId,ProjectId,TrnDate,CallFF,NextCallDate,CallTypeID)VALUES" +
        //                    " (" + oLeedBO.ExecutiveID + "," + oLeedBO.EnquiryID + "," + oLeedBO.ProjID + ",'" + s_LeadDate + "','Fresh','" + s_NextCallDate + "'," + oLeedBO.i_LeadCallTypeId + ")";
        //                cmd = new SqlCommand(sSql, conn, tran);
        //                cmd.ExecuteNonQuery();
        //                cmd.Dispose();
        //            }
        //        }
        //    }

        //   // return oLeedBO.i_LeadId;
        //}

        public static void InsertChangeRate(int argFlatId, decimal argOldRate, decimal argNewRate, SqlConnection conn, SqlTransaction tran)
        {
            string sSql = "";
            decimal dFBaseAmt = 0; decimal dFNetAmt = 0;
            try
            {
                if (argOldRate != argNewRate)
                {
                    sSql = "Select Area,OtherCostAmt From FlatDetails Where FlatId=" + argFlatId + "";
                    SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    cmd.Dispose();

                    if (dt.Rows.Count > 0)
                    {
                        dFBaseAmt = Convert.ToDecimal(dt.Rows[0]["Area"]) * argNewRate;
                        dFNetAmt = dFBaseAmt + Convert.ToDecimal(dt.Rows[0]["OtherCostAmt"]);

                        sSql = "Update FlatDetails Set Rate=" + argNewRate + ", " +
                            " BaseAmt=" + dFBaseAmt + ",NetAmt=" + dFNetAmt + "" +
                            " Where FlatId=" + argFlatId + " ";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        //PaymentScheduleDL.InsertFlatScheduleI(argFlatId, conn, tran);
                    }
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
        }

        internal static int InsertPlotLeadDetails(NewLeadBO oLeedBO, string s_Mode, DataTable dtEnqTrans, bool UpdateLead, SqlConnection conn, SqlTransaction tran, DataTable dtFinal, string argFlatNo, bool argChk, DataTable dtLand)
        {
            SqlCommand cmd; SqlDataReader dr;
            string sSql = "";
            DataTable dtLeadDetails = null;
            string s_NextCallDate = "";
            string s_LeadDate = "";
            string validUpto = "", dateFinal = "";
            bool vip = false;
            decimal dAdvAmt = 0; decimal dRate = 0;

            s_LeadDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(oLeedBO.DE_LeadDate));
            s_NextCallDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(oLeedBO.DE_NextCallDate));
            oLeedBO.i_ExecutiveId = oLeedBO.UserId;
            try
            {
                sSql = "Select Isnull(B.PositionType,'') PositionType,A.PowerUser From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                       " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B On A.PositionId=B.PositionId" +
                       " Where A.UserId=" + oLeedBO.UserId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                dr = cmd.ExecuteReader();
                DataTable dtE = new DataTable();
                dtE.Load(dr);
                if (dtE.Rows.Count > 0)
                {
                    if (dtE.Rows[0]["PositionType"].ToString() == "M" || Convert.ToInt32(dtE.Rows[0]["PowerUser"]) == 1)
                    {
                        oLeedBO.i_ExecutiveId = oLeedBO.UserId;
                    }
                    else
                        oLeedBO.i_ExecutiveId = 0;
                }
                //string sUserType = GetType(sSql, conn, tran);
                //if (sUserType == "M")
                //    oLeedBO.i_ExecutiveId = oLeedBO.UserId;
                //else
                //    oLeedBO.i_ExecutiveId = 0;


                if (s_NextCallDate == "01/Jan/0001")
                {
                    s_NextCallDate = "";
                }
                if (oLeedBO.VIP == 1) vip = true;
                else vip = false;
                if (s_Mode == "A" && UpdateLead == false)
                {
                    if (oLeedBO.CallType == "Finalization" || oLeedBO.CallType == "Finalisation" || oLeedBO.CallType == "Block")
                    {
                        sSql = String.Format("Insert into dbo.LeadRegister(LeadName, LeadDate, LeadType, CallTypeId,CampaignId,UserId,VIP,CostCentreId,ExecutiveId,TypeId,CostId,Mobile,Email)" +
                        " Values('{0}', '{1}', '{2}' , {3},{4},{5},'{6}',{7},{8},{9},{10},'{11}','{12}' ) SELECT SCOPE_IDENTITY();", oLeedBO.s_LeadName, s_LeadDate, oLeedBO.s_LeadTypeName, oLeedBO.i_LeadCallTypeId, oLeedBO.i_SubSourceId, oLeedBO.UserId, vip, oLeedBO.ProjID, oLeedBO.i_ExecutiveId, oLeedBO.i_FinApartmentTypeId, oLeedBO.i_FinApartmentCostId, oLeedBO.Mobile, oLeedBO.Email);
                    }
                    else
                    {
                        sSql = String.Format("Insert into dbo.LeadRegister(LeadName, LeadDate, LeadType, CallTypeId, NextCallDate,CampaignId,UserId,VIP,CostCentreId,ExecutiveId,TypeId,CostId,Mobile,Email)" +
                        " Values('{0}', '{1}', '{2}' , {3} , '{4}',{5},{6},'{7}',{8},{9},{10},{11},'{12}','{13}') SELECT SCOPE_IDENTITY();", oLeedBO.s_LeadName, s_LeadDate, oLeedBO.s_LeadTypeName, oLeedBO.i_LeadCallTypeId, s_NextCallDate, oLeedBO.i_SubSourceId, oLeedBO.UserId, vip, oLeedBO.ProjID, oLeedBO.i_ExecutiveId, oLeedBO.i_FinApartmentTypeId, oLeedBO.i_FinApartmentCostId, oLeedBO.Mobile, oLeedBO.Email);
                    }
                    cmd = new SqlCommand(sSql, conn, tran);
                    oLeedBO.EnquiryID = int.Parse(cmd.ExecuteScalar().ToString());
                    oLeedBO.i_LeadId = oLeedBO.EnquiryID;
                    cmd.Dispose();
                }
                else
                {
                    dtLeadDetails = new DataTable();
                    sSql = string.Format("Update dbo.LeadRegister Set LeadName='{0}', LeadDate='{1}', LeadType='{2}', CallTypeId={3}, NextCallDate='{4}',CampaignId={5},UserId={6},VIP='{7}',TypeId={8},CostId={9},Mobile='{10}',Email='{11}',CostCentreId={12},ExecutiveId={13} where LeadId={14}",
                        oLeedBO.s_LeadName, s_LeadDate, oLeedBO.s_LeadTypeName, oLeedBO.i_LeadCallTypeId, s_NextCallDate, oLeedBO.i_SubSourceId, oLeedBO.UserId, vip, oLeedBO.i_FinApartmentTypeId, oLeedBO.i_FinApartmentCostId, oLeedBO.Mobile, oLeedBO.Email, oLeedBO.ProjID, oLeedBO.i_ExecutiveId, oLeedBO.i_LeadId);

                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //sSql = string.Format("Update CallSheet Set ExecutiveId='{0}', ProjId='{1}', TrnDate='{2}', NextCallDate='{3}' where LeadId={4}",
                    //    oLeedBO.s_LeadName, s_LeadDate, oLeedBO.s_LeadTypeName, oLeedBO.i_LeadCallTypeId, s_NextCallDate, oLeedBO.i_SubSourceId, oLeedBO.UserId, vip, oLeedBO.i_LeadId);

                    //cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB, tran);
                    //cmd.ExecuteNonQuery();
                    //cmd.Dispose();
                }


                if (UpdateLead == false)
                {
                    sSql = ""; int iEntryId = 0;
                    if (oLeedBO.CallType == "Finalization" || oLeedBO.CallType == "Finalisation" || oLeedBO.CallType == "Block")
                    {
                        if (dtEnqTrans.Rows[0]["ValidUpto"].ToString() != "")
                        {
                            validUpto = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(dtEnqTrans.Rows[0]["ValidUpto"].ToString()));
                        }
                        if (dtEnqTrans.Rows[0]["FinaliseDate"].ToString() != "")
                            dateFinal = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(dtEnqTrans.Rows[0]["FinaliseDate"].ToString()));

                        if (dtEnqTrans.Rows.Count > 0)
                        {
                            sSql = "INSERT INTO dbo.CallSheet (ExecutiveId,LeadId,ProjectId,TrnDate,CallFF,CallTypeID)VALUES" +
                                " (" + oLeedBO.ExecutiveID + "," + oLeedBO.EnquiryID + "," + oLeedBO.ProjID + ",'" + s_LeadDate + "','Fresh'," + oLeedBO.i_LeadCallTypeId + ")" +
                                    " SELECT SCOPE_IDENTITY();";
                            cmd = new SqlCommand(sSql, conn, tran);
                            iEntryId = int.Parse(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();

                            sSql = "INSERT INTO dbo.BuyerDetail(CostCentreId,ExecutiveId,LeadId,EntryId,FlatId,Status,CustomerType," +
                            " PaymentOption,BranchId,LoanPer,LoanAccNo,BrokerId,BrokerComm,BrokerAmount,ValidUpto,FinaliseDate,PostSaleExecId,Advance,PlotId,RegDate) Values(" + oLeedBO.ProjID + "," +
                            " " + oLeedBO.ExecutiveID + "," + oLeedBO.EnquiryID + "," + iEntryId + ",0," +
                            " '" + dtEnqTrans.Rows[0]["Status"] + "','" + dtEnqTrans.Rows[0]["CustomerType"] + "','" + dtEnqTrans.Rows[0]["PaymentOption"] + "'," + dtEnqTrans.Rows[0]["BranchId"] + ", " +
                            " " + dtEnqTrans.Rows[0]["LoanPer"] + ",'" + dtEnqTrans.Rows[0]["LoanAccNo"] + "'," + dtEnqTrans.Rows[0]["BrokerId"] + "," + dtEnqTrans.Rows[0]["ComPer"] + "," +
                            " " + dtEnqTrans.Rows[0]["ComAmount"] + ",'" + validUpto + "','" + dateFinal + "'," +
                            " " + dtEnqTrans.Rows[0]["PostSaleExecId"] + "," + dtEnqTrans.Rows[0]["AdvAmt"] + "," +
                            " " + dtEnqTrans.Rows[0]["PlotId"] + ",'" + string.Format(Convert.ToDateTime(dtEnqTrans.Rows[0]["RegDate"]).ToString("dd-MMM-yyyy")) + "')";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                        if (oLeedBO.CallType == "Block")
                            sSql = "Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails Set Status='B',BuyerId=" + oLeedBO.EnquiryID + " WHERE PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + " ";
                        else
                            sSql = "Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails Set Status='S',BuyerId=" + oLeedBO.EnquiryID + " WHERE PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + " ";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        if (oLeedBO.CallType == "Block")
                        {
                            sSql = "INSERT INTO dbo.BlockUnits(CostCentreId,PlotId,CustomerType,LeadId,BlockUpto,Date,BlockType) Values(" + oLeedBO.ProjID + "," +
                                    " " + dtEnqTrans.Rows[0]["PlotId"] + ",'" + dtEnqTrans.Rows[0]["CustomerType"] + "'," + oLeedBO.EnquiryID + ",'" + validUpto + "'," +
                                    " '" + String.Format(Convert.ToDateTime(DateTime.Now).ToString("MM-dd-yyyy")) + "','B')";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }

                        sSql = "INSERT INTO dbo.LeadFlatInfo (LeadId,Buyer,PlotId,CostCentreId)VALUES(" + oLeedBO.EnquiryID + ",1," + dtEnqTrans.Rows[0]["PlotId"] + "," + oLeedBO.ProjID + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        oLeedBO.s_LeadName = oLeedBO.s_LeadName + dtEnqTrans.Rows[0]["PlotId"];

                        sSql = "INSERT INTO dbo.UserLogin(UserName,Password,Email,LeadId) VALUES" +
                            " ('" + oLeedBO.s_LeadName + "','" + oLeedBO.s_LeadName + "','" + oLeedBO.Email + "'," + oLeedBO.EnquiryID + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Select Rate,AdvanceAmount From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails Where PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        dr = cmd.ExecuteReader();
                        DataTable dtA = new DataTable();
                        dtA.Load(dr);
                        if (dtA.Rows.Count > 0)
                        {
                            dAdvAmt = Convert.ToDecimal(dtA.Rows[0]["AdvanceAmount"]);
                            dRate = Convert.ToDecimal(dtA.Rows[0]["Rate"]);
                            if (dAdvAmt != Convert.ToDecimal(dtEnqTrans.Rows[0]["AdvAmt"]))
                            {
                                sSql = "Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails Set AdvanceAmount=" + dtEnqTrans.Rows[0]["AdvAmt"] + " WHERE PlotId=" + dtEnqTrans.Rows[0]["PlotId"] + " ";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }

                        InsertPlotChangeRate(Convert.ToInt32(dtEnqTrans.Rows[0]["PlotId"]), dRate, Convert.ToDecimal(dtEnqTrans.Rows[0]["Rate"]), conn, tran);

                        if (dtFinal != null || dtLand != null)
                            UnitDirDL.InsertPlotChk(dtFinal, Convert.ToInt32(dtEnqTrans.Rows[0]["PlotId"]), "F", argChk, argFlatNo, oLeedBO.ProjID, conn, tran, dtLand);
                    }
                    else
                    {
                        if (s_Mode == "A")
                        {
                            sSql = "INSERT INTO dbo.CallSheet (ExecutiveId,LeadId,ProjectId,TrnDate,CallFF,NextCallDate,CallTypeID)VALUES" +
                                " (" + oLeedBO.ExecutiveID + "," + oLeedBO.EnquiryID + "," + oLeedBO.ProjID + ",'" + s_LeadDate + "','Fresh','" + s_NextCallDate + "'," + oLeedBO.i_LeadCallTypeId + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
            return oLeedBO.i_LeadId;
        }

        public static void InsertPlotChangeRate(int argPlotId, decimal argOldRate, decimal argNewRate, SqlConnection conn, SqlTransaction tran)
        {
            string sSql = "";
            decimal dFBaseAmt = 0; decimal dFNetAmt = 0;
            try
            {
                if (argOldRate != argNewRate)
                {
                    sSql = "Select Area,OtherCost From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails Where PlotDetailsId=" + argPlotId + "";
                    SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    cmd.Dispose();

                    if (dt.Rows.Count > 0)
                    {
                        dFBaseAmt = Convert.ToDecimal(dt.Rows[0]["Area"]) * argNewRate;
                        dFNetAmt = dFBaseAmt + Convert.ToDecimal(dt.Rows[0]["OtherCost"]);

                        sSql = "Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails Set Rate=" + argNewRate + ", " +
                            " BaseAmount=" + dFBaseAmt + ",NetAmount=" + dFNetAmt + "" +
                            " Where PlotDetailsId=" + argPlotId + " ";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        //PaymentScheduleDL.InsertFlatScheduleI(argPlotId, conn, tran);
                    }
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void InsertProjectInformation(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            try
            {
                if (s_Mode == "A" && argUpdateLead == false)
                {
                    ssql = String.Format("Insert Into dbo.LeadProjectInfo(CostCentreId, Status, Remarks, LeadId, NextCallDate) Values({0} , '{1}', '{2}', {3}, @NextCallDate)", 
                        oLeedBO.i_ProjCostCentreId, oLeedBO.s_ProjStatus, oLeedBO.s_ProjRemarks, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    SqlParameter dateparameter = new SqlParameter() { ParameterName = "@NextCallDate", DbType = DbType.DateTime };
                    if (NewLeadBL.NextCallDate == DateTime.MinValue)
                        dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        dateparameter.Value = oLeedBO.NextCallDate;
                    cmd.Parameters.Add(dateparameter);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update dbo.LeadProjectInfo set CostCentreId={0}, Status='{1}', Remarks='{2}', NextCallDate=@NextCallDate where LeadId={3}", 
                                        oLeedBO.i_ProjCostCentreId, oLeedBO.s_ProjStatus, oLeedBO.s_ProjRemarks, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    SqlParameter dateparameter = new SqlParameter() { ParameterName = "@NextCallDate", DbType = DbType.DateTime };
                    if (NewLeadBL.NextCallDate == DateTime.MinValue)
                        dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        dateparameter.Value = oLeedBO.NextCallDate;
                    cmd.Parameters.Add(dateparameter);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void InsertExeInformation(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            DataTable dtExeInfo = new DataTable();
            SqlCommand cmd; SqlDataReader dr;
            oLeedBO.i_ExecutiveId = BsfGlobal.g_lUserId;
            try
            {
                ssql = "Select Isnull(B.PositionType,'') PositionType,A.PowerUser from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId" +
                        " Where A.UserId=" + oLeedBO.UserId + "";
                cmd = new SqlCommand(ssql, conn, tran);
                dr = cmd.ExecuteReader();
                DataTable dtE = new DataTable();
                dtE.Load(dr);
                if (dtE.Rows.Count > 0)
                {
                    if (dtE.Rows[0]["PositionType"].ToString() == "M" || Convert.ToInt32(dtE.Rows[0]["PowerUser"]) == 1)
                    {
                        oLeedBO.i_ExecutiveId = oLeedBO.UserId;
                    }
                    else
                        oLeedBO.i_ExecutiveId = 0;
                }
                //string sUserType = GetType(ssql, conn, tran);
                //if (sUserType == "M")
                //    oLeedBO.i_ExecutiveId = oLeedBO.UserId;
                //else
                //    oLeedBO.i_ExecutiveId = 0;

                if (s_Mode == "A" && argUpdateLead == false)
                {
                    ssql = String.Format("Insert Into dbo.LeadExecutiveInfo(ExecutiveId, Status, Remarks, Category, LeadId,CostCentreId) Values({0},'{1}','{2}','{3}',{4},{5})", oLeedBO.i_ExecutiveId, oLeedBO.s_ExeStatus, oLeedBO.s_ExeRemarks, oLeedBO.s_Category, oLeedBO.i_LeadId, oLeedBO.ProjID);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update dbo.LeadExecutiveInfo Set ExecutiveId={0}, Status='{1}', Remarks='{2}', Category='{3}' where LeadId={4}", oLeedBO.i_ExecutiveId, oLeedBO.s_ExeStatus, oLeedBO.s_ExeRemarks, oLeedBO.s_Category, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void InsertPersonalInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            string s_PersonalDOB = "";
            string s_PersonalWeddingDate = "";
            try
            {
                s_PersonalWeddingDate = string.Format("{0:dd/MMM/yyyy}", oLeedBO.DE_PersonalWeddingDate);
                s_PersonalDOB = string.Format("{0:dd/MMM/yyyy}", oLeedBO.DE_PersonalDOB);

                if (s_PersonalDOB == "01/Jan/0001")
                {
                    s_PersonalDOB = null;
                }

                if (s_PersonalWeddingDate == "01/Jan/0001")
                {
                    s_PersonalWeddingDate = null;
                }

                if (s_Mode == "A" && argUpdateLead == false)
                {
                    if (s_PersonalWeddingDate != "01/Jan/0001")
                    {
                        ssql = String.Format("Insert Into dbo.LeadPersonalInfo(Gender, DOB, Religion, Nationality, Profession, Organization, FatherName, MotherName, MarritalStatus, WifeName, WeddingDate, NRI, LeadId) Values({0}, '{1}', {2}, '{3}', '{4}', '{5}', '{6}', '{7}', {8}, '{9}', '{10}', {11}, {12})", oLeedBO.i_PersonalGender, s_PersonalDOB, oLeedBO.i_PersonalReligion, oLeedBO.s_PersonalNationality, oLeedBO.s_PersonalProfession, oLeedBO.s_PersonalOrganization, oLeedBO.s_PersonalFatherName, oLeedBO.s_PersonalMotherName, oLeedBO.i_PersonalMaritalStatusId, oLeedBO.s_PersonalWifeName, s_PersonalWeddingDate, oLeedBO.i_PersonalNRI, oLeedBO.i_LeadId);
                        cmd = new SqlCommand(ssql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    else
                    {
                        if (s_PersonalDOB != "01/Jan/0001")
                        {
                            s_PersonalWeddingDate = null;
                            ssql = String.Format("Insert Into dbo.LeadPersonalInfo(Gender, DOB, Religion, Nationality, Profession, Organization, FatherName, MotherName, MarritalStatus, WeddingDate, NRI, LeadId) Values({0}, '{1}', {2}, '{3}', '{4}', '{5}', '{6}', '{7}', {8}, '{9}', {10}, {11})", oLeedBO.i_PersonalGender, s_PersonalDOB, oLeedBO.i_PersonalReligion, oLeedBO.s_PersonalNationality, oLeedBO.s_PersonalProfession, oLeedBO.s_PersonalOrganization, oLeedBO.s_PersonalFatherName, oLeedBO.s_PersonalMotherName, oLeedBO.i_PersonalMaritalStatusId, s_PersonalWeddingDate, oLeedBO.i_PersonalNRI, oLeedBO.i_LeadId);
                            cmd = new SqlCommand(ssql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                        else
                        {
                            s_PersonalDOB = null;
                            ssql = String.Format("Insert Into dbo.LeadPersonalInfo(Gender, DOB, Religion, Nationality, Profession, Organization, FatherName, MotherName, MarritalStatus, NRI, LeadId) Values({0}, '{1}', {2}, '{3}', '{4}', '{5}', '{6}', '{7}', {8}, {9}, {10})", oLeedBO.i_PersonalGender, s_PersonalDOB, oLeedBO.i_PersonalReligion, oLeedBO.s_PersonalNationality, oLeedBO.s_PersonalProfession, oLeedBO.s_PersonalOrganization, oLeedBO.s_PersonalFatherName, oLeedBO.s_PersonalMotherName, oLeedBO.i_PersonalMaritalStatusId, oLeedBO.i_PersonalNRI, oLeedBO.i_LeadId);
                            cmd = new SqlCommand(ssql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                }
                else
                {
                    if (s_PersonalWeddingDate != "01/Jan/0001")
                    {
                        ssql = String.Format("Update dbo.LeadPersonalInfo Set Gender={0}, DOB='{1}', Religion={2}, Nationality='{3}', Profession='{4}', Organization='{5}', FatherName='{6}', MotherName='{7}', MarritalStatus={8}, WifeName='{9}', WeddingDate='{10}', NRI={11} where LeadId={12}", oLeedBO.i_PersonalGender, s_PersonalDOB, oLeedBO.i_PersonalReligion, oLeedBO.s_PersonalNationality, oLeedBO.s_PersonalProfession, oLeedBO.s_PersonalOrganization, oLeedBO.s_PersonalFatherName, oLeedBO.s_PersonalMotherName, oLeedBO.i_PersonalMaritalStatusId, oLeedBO.s_PersonalWifeName, s_PersonalWeddingDate, oLeedBO.i_PersonalNRI, oLeedBO.i_LeadId);
                        cmd = new SqlCommand(ssql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    else
                    {
                        if (s_PersonalDOB != "01/Jan/0001")
                        {
                            s_PersonalWeddingDate = null;
                            ssql = String.Format("Update dbo.LeadPersonalInfo Set Gender={0}, DOB='{1}', Religion={2}, Nationality='{3}', Profession='{4}', Organization='{5}', FatherName='{6}', MotherName='{7}', MarritalStatus={8}, NRI={9}, WeddingDate='{10}' where LeadId={11}", oLeedBO.i_PersonalGender, s_PersonalDOB, oLeedBO.i_PersonalReligion, oLeedBO.s_PersonalNationality, oLeedBO.s_PersonalProfession, oLeedBO.s_PersonalOrganization, oLeedBO.s_PersonalFatherName, oLeedBO.s_PersonalMotherName, oLeedBO.i_PersonalMaritalStatusId, oLeedBO.i_PersonalNRI, s_PersonalWeddingDate, oLeedBO.i_LeadId);
                            cmd = new SqlCommand(ssql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                        else
                        {
                            s_PersonalDOB = null;
                            ssql = String.Format("Update dbo.LeadPersonalInfo Set Gender={0}, DOB='{1}', Religion={2}, Nationality='{3}', Profession='{4}', Organization='{5}', FatherName='{6}', MotherName='{7}', MarritalStatus={8}, NRI={9} where LeadId={10}", oLeedBO.i_PersonalGender, s_PersonalDOB, oLeedBO.i_PersonalReligion, oLeedBO.s_PersonalNationality, oLeedBO.s_PersonalProfession, oLeedBO.s_PersonalOrganization, oLeedBO.s_PersonalFatherName, oLeedBO.s_PersonalMotherName, oLeedBO.i_PersonalMaritalStatusId, oLeedBO.i_PersonalNRI, oLeedBO.i_LeadId);
                            cmd = new SqlCommand(ssql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void InsertCoAppInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            string s_CoAppDOB = "";
            string s_CoAppWeddingDate = "";
            try
            {
                s_CoAppWeddingDate = string.Format("{0:dd/MMM/yyyy}", oLeedBO.DE_CoAppWeddingDate);
                s_CoAppDOB = string.Format("{0:dd/MMM/yyyy}", oLeedBO.DE_CoAppDOB);

                if (s_CoAppDOB == "01/Jan/0001")
                {
                    s_CoAppDOB = null;
                }

                if (s_CoAppWeddingDate == "01/Jan/0001")
                {
                    s_CoAppWeddingDate = null;
                }

                if (s_Mode == "A" && argUpdateLead == false)
                {
                    if (s_CoAppWeddingDate != "01/Jan/0001")
                    {
                        ssql = String.Format("Insert Into dbo.LeadCoApplicantInfo(CoApplicantName, DOB, Religion, Nationality, Profession, Organization, FatherName, MotherName, MarritalStatus, WifeName, WeddingDate, LeadId, Gender)"
                            + " Values('{0}', '{1}', {2}, '{3}', '{4}', '{5}', '{6}', '{7}', {8}, '{9}', '{10}', {11}, {12})",
                            oLeedBO.s_CoApplicantName, s_CoAppDOB, oLeedBO.i_CoAppReligion, oLeedBO.s_CoAppNationality,
                            oLeedBO.s_CoAppProfession, oLeedBO.s_CoAppOrganization, oLeedBO.s_CoAppFatherName,
                            oLeedBO.s_CoAppMotherName, oLeedBO.i_CoAppMaritalStatusId, oLeedBO.s_CoAppWifeName,
                            s_CoAppWeddingDate, oLeedBO.i_LeadId, oLeedBO.i_CoAppGenderId);
                        cmd = new SqlCommand(ssql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    else
                    {
                        if (s_CoAppDOB != "01/Jan/0001")
                        {
                            s_CoAppWeddingDate = null;
                            ssql = String.Format("Insert Into dbo.LeadCoApplicantInfo(CoApplicantName, DOB, Religion, Nationality, Profession, Organization, FatherName, MotherName, MarritalStatus, LeadId, WeddingDate, Gender)"
                                + " Values('{0}', '{1}', {2}, '{3}', '{4}', '{5}', '{6}', '{7}', {8}, {9}, '{10}', {11})",
                                oLeedBO.s_CoApplicantName, s_CoAppDOB, oLeedBO.i_CoAppReligion, oLeedBO.s_CoAppNationality,
                                oLeedBO.s_CoAppProfession, oLeedBO.s_CoAppOrganization, oLeedBO.s_CoAppFatherName,
                                oLeedBO.s_CoAppMotherName, oLeedBO.i_CoAppMaritalStatusId, oLeedBO.i_LeadId,
                                s_CoAppWeddingDate, oLeedBO.i_CoAppGenderId);
                            cmd = new SqlCommand(ssql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                        else
                        {
                            s_CoAppDOB = null;
                            ssql = String.Format("Insert Into dbo.LeadCoApplicantInfo(CoApplicantName, DOB, Religion, Nationality, Profession, Organization, FatherName, MotherName, MarritalStatus, LeadId, Gender)"
                                + " Values('{0}', '{1}', {2}, '{3}', '{4}', '{5}', '{6}', '{7}', {8}, {9}, {10})",
                                oLeedBO.s_CoApplicantName, s_CoAppDOB, oLeedBO.i_CoAppReligion, oLeedBO.s_CoAppNationality,
                                oLeedBO.s_CoAppProfession, oLeedBO.s_CoAppOrganization, oLeedBO.s_CoAppFatherName,
                                oLeedBO.s_CoAppMotherName, oLeedBO.i_CoAppMaritalStatusId, oLeedBO.i_LeadId, oLeedBO.i_CoAppGenderId);
                            cmd = new SqlCommand(ssql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                }
                else
                {
                    if (oLeedBO.s_CoAppMaritalStatus == null)
                    {
                        if (s_CoAppWeddingDate != "01/Jan/0001")
                        {
                            ssql = String.Format("Update dbo.LeadCoApplicantInfo Set CoApplicantName='{0}', DOB='{1}', Religion={2}, Nationality='{3}', Profession='{4}', Organization='{5}', FatherName='{6}', MotherName='{7}', MarritalStatus={8}, WifeName='{9}', WeddingDate='{10}', Gender={11} where LeadId={12}"
                                , oLeedBO.s_CoApplicantName, s_CoAppDOB, oLeedBO.i_CoAppReligion, oLeedBO.s_CoAppNationality,
                                oLeedBO.s_CoAppProfession, oLeedBO.s_CoAppOrganization, oLeedBO.s_CoAppFatherName,
                                oLeedBO.s_CoAppMotherName, oLeedBO.i_CoAppMaritalStatusId, oLeedBO.s_CoAppWifeName,
                                s_CoAppWeddingDate, oLeedBO.i_CoAppGenderId, oLeedBO.i_LeadId);
                            cmd = new SqlCommand(ssql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                        else
                        {
                            if (s_CoAppDOB != "01/Jan/0001")
                            {
                                s_CoAppWeddingDate = null;
                                ssql = String.Format("Update dbo.LeadCoApplicantInfo Set CoApplicantName='{0}', DOB='{1}', Religion={2}, Nationality='{3}', Profession='{4}', Organization='{5}', FatherName='{6}', MotherName='{7}', MarritalStatus={8}, WeddingDate='{9}', Gender={10} where LeadId={11}"
                                    , oLeedBO.s_CoApplicantName, s_CoAppDOB, oLeedBO.i_CoAppReligion, oLeedBO.s_CoAppNationality,
                                    oLeedBO.s_CoAppProfession, oLeedBO.s_CoAppOrganization, oLeedBO.s_CoAppFatherName,
                                    oLeedBO.s_CoAppMotherName, oLeedBO.i_CoAppMaritalStatusId, s_CoAppWeddingDate, oLeedBO.i_CoAppGenderId, oLeedBO.i_LeadId);
                                cmd = new SqlCommand(ssql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                            else
                            {
                                s_CoAppDOB = null;
                                ssql = String.Format("Update dbo.LeadCoApplicantInfo Set CoApplicantName='{0}', DOB='{1}', Religion={2}, Nationality='{3}', Profession='{4}', Organization='{5}', FatherName='{6}', MotherName='{7}', MarritalStatus={8}, Gender={9} where LeadId={10}"
                                    , oLeedBO.s_CoApplicantName, s_CoAppDOB, oLeedBO.i_CoAppReligion, oLeedBO.s_CoAppNationality,
                                    oLeedBO.s_CoAppProfession, oLeedBO.s_CoAppOrganization, oLeedBO.s_CoAppFatherName,
                                    oLeedBO.s_CoAppMotherName, oLeedBO.i_CoAppMaritalStatusId, oLeedBO.i_CoAppGenderId, oLeedBO.i_LeadId);
                                cmd = new SqlCommand(ssql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void InsertPOAInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            string s_POADOB = "";
            string s_POAWeddingDate = "";

            s_POAWeddingDate = string.Format("{0:dd/MMM/yyyy}", oLeedBO.DE_POAWeddingDate);
            s_POADOB = string.Format("{0:dd/MMM/yyyy}", oLeedBO.DE_POADOB);
            try
            {
                if (s_POADOB == "01/Jan/0001")
                {
                    s_POADOB = null;
                }

                if (s_POAWeddingDate == "01/Jan/0001")
                {
                    s_POAWeddingDate = null;
                }

                if (s_Mode == "A" && argUpdateLead == false)
                {
                    if (s_POAWeddingDate != "01/Jan/0001")
                    {
                        ssql = String.Format("Insert Into dbo.LeadPOAInfo(ApplicantName, DOB, Religion, Nationality, Profession, Organization, FatherName, MotherName, MarritalStatus, WifeName, WeddingDate, LeadId, Gender,Relation)"
                            + " Values('{0}', '{1}', {2}, '{3}', '{4}', '{5}', '{6}', '{7}', {8}, '{9}', '{10}', {11}, {12},'{13}')",
                            oLeedBO.s_POAName, s_POADOB, oLeedBO.i_POAReligion, oLeedBO.s_POANationality,
                            oLeedBO.s_POAProfession, oLeedBO.s_POAOrganization, oLeedBO.s_POAFatherName,
                            oLeedBO.s_POAMotherName, oLeedBO.i_POAMaritalStatusId, oLeedBO.s_POAWifeName,
                            s_POAWeddingDate, oLeedBO.i_LeadId, oLeedBO.i_POAGenderId, oLeedBO.s_POARelation);
                        cmd = new SqlCommand(ssql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    else
                    {
                        if (s_POADOB != "01/Jan/0001")
                        {
                            s_POAWeddingDate = null;
                            ssql = String.Format("Insert Into dbo.LeadPOAInfo(ApplicantName, DOB, Religion, Nationality, Profession, Organization, FatherName, MotherName, MarritalStatus, LeadId, WeddingDate, Gender,Relation)"
                                + " Values('{0}', '{1}', {2}, '{3}', '{4}', '{5}', '{6}', '{7}', {8}, {9}, '{10}', {11},'{12}')",
                                oLeedBO.s_POAName, s_POADOB, oLeedBO.i_POAReligion, oLeedBO.s_POANationality,
                                oLeedBO.s_POAProfession, oLeedBO.s_POAOrganization, oLeedBO.s_POAFatherName,
                                oLeedBO.s_POAMotherName, oLeedBO.i_POAMaritalStatusId, oLeedBO.i_LeadId,
                                s_POAWeddingDate, oLeedBO.i_POAGenderId, oLeedBO.s_POARelation);
                            cmd = new SqlCommand(ssql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                        else
                        {
                            s_POADOB = null;
                            ssql = String.Format("Insert Into dbo.LeadPOAInfo(ApplicantName, DOB, Religion, Nationality, Profession, Organization, FatherName, MotherName, MarritalStatus, LeadId, Gender,Relation)"
                                + " Values('{0}', '{1}', {2}, '{3}', '{4}', '{5}', '{6}', '{7}', {8}, {9}, {10},'{11}')",
                                oLeedBO.s_POAName, s_POADOB, oLeedBO.i_POAReligion, oLeedBO.s_POANationality,
                                oLeedBO.s_POAProfession, oLeedBO.s_POAOrganization, oLeedBO.s_POAFatherName,
                                oLeedBO.s_POAMotherName, oLeedBO.i_POAMaritalStatusId, oLeedBO.i_LeadId, oLeedBO.i_POAGenderId, oLeedBO.s_POARelation);
                            cmd = new SqlCommand(ssql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                }
                else
                {
                    if (oLeedBO.s_CoAppMaritalStatus == null)
                    {
                        if (s_POAWeddingDate != "01/Jan/0001")
                        {
                            ssql = String.Format("Update dbo.LeadPOAInfo Set ApplicantName='{0}', DOB='{1}', Religion={2}, Nationality='{3}', Profession='{4}', Organization='{5}', FatherName='{6}', MotherName='{7}', MarritalStatus={8}, WifeName='{9}', WeddingDate='{10}', Gender={11},Relation='{12}' where LeadId={13}"
                                , oLeedBO.s_POAName, s_POADOB, oLeedBO.i_POAReligion, oLeedBO.s_POANationality,
                                oLeedBO.s_POAProfession, oLeedBO.s_POAOrganization, oLeedBO.s_POAFatherName,
                                oLeedBO.s_POAMotherName, oLeedBO.i_POAMaritalStatusId, oLeedBO.s_POAWifeName,
                                s_POAWeddingDate, oLeedBO.i_POAGenderId, oLeedBO.s_POARelation, oLeedBO.i_LeadId);
                            cmd = new SqlCommand(ssql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                        else
                        {
                            if (s_POADOB != "01/Jan/0001")
                            {
                                s_POAWeddingDate = null;
                                ssql = String.Format("Update dbo.LeadPOAInfo Set ApplicantName='{0}', DOB='{1}', Religion={2}, Nationality='{3}', Profession='{4}', Organization='{5}', FatherName='{6}', MotherName='{7}', MarritalStatus={8}, WeddingDate='{9}', Gender={10},Relation='{11}' where LeadId={12}"
                                    , oLeedBO.s_POAName, s_POADOB, oLeedBO.i_POAReligion, oLeedBO.s_POANationality,
                                    oLeedBO.s_POAProfession, oLeedBO.s_POAOrganization, oLeedBO.s_POAFatherName,
                                    oLeedBO.s_POAMotherName, oLeedBO.i_POAMaritalStatusId, s_POAWeddingDate, oLeedBO.i_POAGenderId, oLeedBO.s_POARelation, oLeedBO.i_LeadId);
                                cmd = new SqlCommand(ssql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                            else
                            {
                                s_POADOB = null;
                                ssql = String.Format("Update dbo.LeadPOAInfo Set ApplicantName='{0}', DOB='{1}', Religion={2}, Nationality='{3}', Profession='{4}', Organization='{5}', FatherName='{6}', MotherName='{7}', MarritalStatus={8}, Gender={9},Relation='{10}' where LeadId={11}"
                                    , oLeedBO.s_POAName, s_POADOB, oLeedBO.i_POAReligion, oLeedBO.s_POANationality,
                                    oLeedBO.s_POAProfession, oLeedBO.s_POAOrganization, oLeedBO.s_POAFatherName,
                                    oLeedBO.s_POAMotherName, oLeedBO.i_POAMaritalStatusId, oLeedBO.i_POAGenderId, oLeedBO.s_POARelation, oLeedBO.i_LeadId);
                                cmd = new SqlCommand(ssql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void InsertCommAddInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            try
            {
                if (s_Mode == "A" && argUpdateLead == false)
                {
                    ssql = String.Format("Insert Into dbo.LeadCommAddressInfo(Address1, Address2, City, State, Country, PinCode, LandLine, Mobile, Fax, LeadId, Email, PanNo, PassportNo, AddressType)"+
                        " Values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', {9}, '{10}', '{11}', '{12}', '{13}')", 
                        oLeedBO.s_CommAdd1, oLeedBO.s_CommAdd2, oLeedBO.s_CommCity, oLeedBO.s_CommState, oLeedBO.i_CommCountry, oLeedBO.s_CommPinCode, 
                        oLeedBO.s_CommLandLine, oLeedBO.s_CommMobile, oLeedBO.s_CommFax, oLeedBO.i_LeadId, oLeedBO.s_CommEmail, oLeedBO.s_CommPanNo,
                        oLeedBO.s_CommPassportNo, oLeedBO.c_AddressType);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update dbo.LeadCommAddressInfo Set Address1='{0}', Address2='{1}', City='{2}', State='{3}', Country='{4}'," +
                                        " PinCode='{5}', LandLine='{6}', Mobile='{7}', Fax='{8}', Email='{9}', PanNo='{10}', PassportNo='{11}', AddressType='{12}'" +
                                        " Where LeadId={13}", oLeedBO.s_CommAdd1, oLeedBO.s_CommAdd2, oLeedBO.s_CommCity, oLeedBO.s_CommState, oLeedBO.i_CommCountry,
                                        oLeedBO.s_CommPinCode, oLeedBO.s_CommLandLine, oLeedBO.s_CommMobile, oLeedBO.s_CommFax, oLeedBO.s_CommEmail,
                                        oLeedBO.s_CommPanNo, oLeedBO.s_CommPassportNo, oLeedBO.c_AddressType, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    ssql = String.Format("Update dbo.LeadRegister Set Mobile='{0}', Email='{1}' Where LeadId={2}", oLeedBO.s_CommMobile, oLeedBO.s_CommEmail, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void InsertOffAddInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            try
            {
                if (s_Mode == "A" && argUpdateLead == false)
                {
                    ssql = String.Format("Insert Into dbo.LeadOffAddressInfo(Address1, Address2, City, State, Country, PinCode, LandLine, Mobile, Fax, LeadId, Email, PanNo) Values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', {9}, '{10}', '{11}')", oLeedBO.s_OffAdd1, oLeedBO.s_OffAdd2, oLeedBO.s_OffCity, oLeedBO.s_OffState, oLeedBO.i_OffCountry, oLeedBO.s_OffPinCode, oLeedBO.s_OffLandLine, oLeedBO.s_OffMobile, oLeedBO.s_OffFax, oLeedBO.i_LeadId, oLeedBO.s_OffEmail, oLeedBO.s_OffPanNo);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update dbo.LeadOffAddressInfo Set Address1='{0}', Address2='{1}', City='{2}', State='{3}', Country='{4}', PinCode='{5}', LandLine='{6}', Mobile='{7}', Fax='{8}', Email='{9}', PanNo='{10}' Where LeadId={11}", oLeedBO.s_OffAdd1, oLeedBO.s_OffAdd2, oLeedBO.s_OffCity, oLeedBO.s_OffState, oLeedBO.i_OffCountry, oLeedBO.s_OffPinCode, oLeedBO.s_OffLandLine, oLeedBO.s_OffMobile, oLeedBO.s_OffFax, oLeedBO.s_OffEmail, oLeedBO.s_OffPanNo, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void InsertPermAddInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            try
            {
                if (s_Mode == "A" && argUpdateLead == false)
                {
                    ssql = String.Format("Insert Into dbo.LeadPermanentAddressInfo(Address1, Address2, City, State, Country, PinCode, LandLine, Mobile, Fax, LeadId, Email, PanNo) Values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', {9}, '{10}', '{11}')", oLeedBO.s_PermAdd1, oLeedBO.s_PermAdd2, oLeedBO.s_PermCity, oLeedBO.s_PermState, oLeedBO.i_PermCountry, oLeedBO.s_PermPinCode, oLeedBO.s_PermLandLine, oLeedBO.s_PermMobile, oLeedBO.s_PermFax, oLeedBO.i_LeadId, oLeedBO.s_PermEmail, oLeedBO.s_PermPanNo);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update dbo.LeadPermanentAddressInfo Set Address1='{0}', Address2='{1}', City='{2}', State='{3}', Country='{4}', PinCode='{5}', LandLine='{6}', Mobile='{7}', Fax='{8}', Email='{9}', PanNo='{10}' Where LeadId={11}", oLeedBO.s_PermAdd1, oLeedBO.s_PermAdd2, oLeedBO.s_PermCity, oLeedBO.s_PermState, oLeedBO.i_PermCountry, oLeedBO.s_PermPinCode, oLeedBO.s_PermLandLine, oLeedBO.s_PermMobile, oLeedBO.s_PermFax, oLeedBO.s_PermEmail, oLeedBO.s_PermPanNo, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void InsertNRIAddInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            try
            {
                if (s_Mode == "A" && argUpdateLead == false)
                {
                    ssql = String.Format("Insert Into dbo.LeadNRIAddressInfo(Address1, Address2, City, State, Country, PinCode, LandLine, Mobile, Fax, LeadId, Email, PanNo, PassportNo) Values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', {9}, '{10}', '{11}', '{12}')", oLeedBO.s_NRIAdd1, oLeedBO.s_NRIAdd2, oLeedBO.s_NRICity, oLeedBO.s_NRIState, oLeedBO.i_NRICountry, oLeedBO.s_NRIPinCode, oLeedBO.s_NRILandLine, oLeedBO.s_NRIMobile, oLeedBO.s_NRIFax, oLeedBO.i_LeadId, oLeedBO.s_NRIEmail, oLeedBO.s_NRIPanNo, oLeedBO.s_NRIPassportNo);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update dbo.LeadNRIAddressInfo Set Address1='{0}', Address2='{1}', City='{2}', State='{3}', Country='{4}', PinCode='{5}', LandLine='{6}', Mobile='{7}', Fax='{8}', Email='{9}', PanNo='{10}', PassportNo='{11}' Where LeadId={12}", oLeedBO.s_NRIAdd1, oLeedBO.s_NRIAdd2, oLeedBO.s_NRICity, oLeedBO.s_NRIState, oLeedBO.i_NRICountry, oLeedBO.s_NRIPinCode, oLeedBO.s_NRILandLine, oLeedBO.s_NRIMobile, oLeedBO.s_NRIFax, oLeedBO.s_NRIEmail, oLeedBO.s_NRIPanNo, oLeedBO.s_NRIPassportNo, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void InsertCoAppAddInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            try
            {
                if (s_Mode == "A" && argUpdateLead == false)
                {
                    ssql = String.Format("Insert Into dbo.LeadCoAppAddressInfo(CoAddress1, CoAddress2, CoCity, CoState, CoCountry, CoPinCode, CoLandLine, CoMobile, CoFax, LeadId, CoEmail, CoPanNo) Values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', {9}, '{10}', '{11}')", oLeedBO.s_CoAdd1, oLeedBO.s_CoAdd2, oLeedBO.s_CoCity, oLeedBO.s_CoState, oLeedBO.i_CoCountry, oLeedBO.s_CoPinCode, oLeedBO.s_CoLandLine, oLeedBO.s_CoMobile, oLeedBO.s_CoFax, oLeedBO.i_LeadId, oLeedBO.s_CoEmail, oLeedBO.s_CoAppPanNo);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update dbo.LeadCoAppAddressInfo Set CoAddress1='{0}', CoAddress2='{1}', CoCity='{2}', CoState='{3}', CoCountry='{4}', CoPinCode='{5}', CoLandLine='{6}', CoMobile='{7}', CoFax='{8}', CoEmail='{9}', CoPanNo='{10}' Where LeadId={11}", oLeedBO.s_CoAdd1, oLeedBO.s_CoAdd2, oLeedBO.s_CoCity, oLeedBO.s_CoState, oLeedBO.i_CoCountry, oLeedBO.s_CoPinCode, oLeedBO.s_CoLandLine, oLeedBO.s_CoMobile, oLeedBO.s_CoFax, oLeedBO.s_CoEmail, oLeedBO.s_CoAppPanNo, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void InsertPOAAddInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            try
            {
                if (s_Mode == "A" && argUpdateLead == false)
                {
                    ssql = String.Format("Insert Into dbo.LeadPOAAddressInfo(Address1, Address2, City, State, Country, PinCode, LandLine, Mobile, Fax, LeadId, Email, PanNo) Values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', {9}, '{10}', '{11}')", oLeedBO.s_POAAdd1, oLeedBO.s_POAAdd2, oLeedBO.s_POACity, oLeedBO.s_POAState, oLeedBO.i_POACountry, oLeedBO.s_POAPinCode, oLeedBO.s_POALandLine, oLeedBO.s_POAMobile, oLeedBO.s_POAFax, oLeedBO.i_LeadId, oLeedBO.s_POAEmail, oLeedBO.s_POAPanNo);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update dbo.LeadPOAAddressInfo Set Address1='{0}', Address2='{1}', City='{2}', State='{3}', Country='{4}', PinCode='{5}', LandLine='{6}', Mobile='{7}', Fax='{8}', Email='{9}', PanNo='{10}' Where LeadId={11}", oLeedBO.s_POAAdd1, oLeedBO.s_POAAdd2, oLeedBO.s_POACity, oLeedBO.s_POAState, oLeedBO.i_POACountry, oLeedBO.s_POAPinCode, oLeedBO.s_POALandLine, oLeedBO.s_POAMobile, oLeedBO.s_POAFax, oLeedBO.s_POAEmail, oLeedBO.s_POAPanNo, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void InsertSourceInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand cmd = null;

            try
            {
                if (s_Mode == "A" && argUpdateLead == false)
                {
                    ssql = String.Format("Insert Into dbo.LeadSource(CostCentreId, SourceId, LeadId) Values({0}, {1}, {2})", oLeedBO.i_SourceProjNameId, oLeedBO.i_SourceNameId, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update dbo.LeadSource Set CostCentreId={0}, SourceId={1} Where LeadId={2}", oLeedBO.i_SourceProjNameId, oLeedBO.i_SourceNameId, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

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

        internal static void InsertChildInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            string s_ChildDOB = "";
            try
            {
                s_ChildDOB = string.Format("{0:dd/MMM/yyyy}", oLeedBO.DE_ChildDOB);

                if (s_ChildDOB == "01/Jan/0001")
                {
                    s_ChildDOB = null;
                }

                if (s_Mode == "A" && argUpdateLead == false)
                {
                    if (s_ChildDOB != "01/Jan/0001")
                    {
                        ssql = String.Format("Insert Into dbo.LeadChildInfo(ChildName, Sex, DOB, LeadId) Values('{0}', {1}, '{2}', {3})", oLeedBO.s_ChildName, oLeedBO.i_ChildGender, s_ChildDOB, oLeedBO.i_LeadId);
                        cmd = new SqlCommand(ssql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    else
                    {
                        s_ChildDOB = null;
                        ssql = String.Format("Insert Into dbo.LeadChildInfo(ChildName, Sex, DOB, LeadId) Values('{0}', {1}, '{2}', {3})", oLeedBO.s_ChildName, oLeedBO.i_ChildGender, s_ChildDOB, oLeedBO.i_LeadId);
                        cmd = new SqlCommand(ssql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
                else
                {
                    if (s_ChildDOB != "01/Jan/0001")
                    {
                        ssql = String.Format("Update dbo.LeadChildInfo Set ChildName='{0}', Sex={1}, DOB='{2}' Where LeadId={3}", oLeedBO.s_ChildName, oLeedBO.i_ChildGender, s_ChildDOB, oLeedBO.i_LeadId);
                        cmd = new SqlCommand(ssql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    else
                    {
                        s_ChildDOB = null;
                        ssql = String.Format("Update dbo.LeadChildInfo Set ChildName='{0}', Sex={1}, DOB='{2}' Where LeadId={3}", oLeedBO.s_ChildName, oLeedBO.i_ChildGender, s_ChildDOB, oLeedBO.i_LeadId);
                        cmd = new SqlCommand(ssql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void InsertApartmentInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            try
            {
                if (s_Mode == "A" && argUpdateLead == false)
                {
                    ssql = String.Format("Insert Into dbo.LeadApartmentInfo(ApartmentId, Size, TypeId, CostId, LeadId) Values({0}, {1}, {2}, {3}, {4})",
                        oLeedBO.i_FinApartmentNameId, oLeedBO.i_FinApartmentSizeId, oLeedBO.i_FinApartmentTypeId,
                        oLeedBO.i_FinApartmentCostId, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update dbo.LeadApartmentInfo Set ApartmentId={0}, Size={1}, TypeId={2}, CostId={3} Where LeadId={4}", oLeedBO.i_FinApartmentNameId, oLeedBO.i_FinApartmentSizeId, oLeedBO.i_FinApartmentTypeId, oLeedBO.i_FinApartmentCostId, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void InsertSubSourceInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand cmd = null;

            try
            {
                if (s_Mode == "A" && argUpdateLead == false)
                {
                    ssql = String.Format("Insert Into dbo.LeadSubSource(SourceId, SubSourceId, LeadId) Values({0}, {1}, {2})", oLeedBO.i_SourceNameId, oLeedBO.i_SubSourceId, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update dbo.LeadSubSource Set SourceId={0}, SubSourceId={1} Where LeadId={2}", oLeedBO.i_SourceNameId, oLeedBO.i_SubSourceId, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
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

        internal static void InsertRequirement(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            try
            {
                if (s_Mode == "A" && argUpdateLead == false)
                {
                    ssql = String.Format("Insert Into dbo.LeadRequirement(Facility, Area, Remarks, LeadId) Values({0}, {1}, '{2}', {3})", oLeedBO.i_LeadId, oLeedBO.i_LeadId, oLeedBO.s_ReqRemarks, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update dbo.LeadRequirement Set Facility={0}, Area={1}, Remarks='{2}' Where LeadId={3}", oLeedBO.i_LeadId, oLeedBO.i_LeadId, oLeedBO.s_ReqRemarks, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void InsertFinance(NewLeadBO oLeedBo, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            try
            {
                if (s_Mode == "A" && argUpdateLead == false)
                {
                    ssql = String.Format("Insert Into dbo.LeadFinance(Employment, Income, BankName, LoanNo, Stay, GuestHouse, Possess, Broker, Block, Comission,ContactPerson, LeadId) Values({0}, {1}, '{2}', '{3}', {4}, {5}, {6}, {7}, {8}, '{9}','{10}', {11})", oLeedBo.i_FinEmpStatusId, oLeedBo.i_FinIncomeId, oLeedBo.s_FinBankName, oLeedBo.s_FinLoanNo, oLeedBo.i_FinStayId, oLeedBo.i_FinGuestHouseId, oLeedBo.i_LeadId, oLeedBo.i_FinBrokerId, oLeedBo.i_FinProjBlockId, oLeedBo.s_FinComission, oLeedBo.s_FinContPer, oLeedBo.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update dbo.LeadFinance Set Employment={0}, Income={1}, BankName='{2}', LoanNo='{3}', Stay={4}, GuestHouse={5}, Possess={6}, Broker={7}, Block={8}, Comission='{9}',ContactPerson='{10}' where LeadId={11}", oLeedBo.i_FinEmpStatusId, oLeedBo.i_FinIncomeId, oLeedBo.s_FinBankName, oLeedBo.s_FinLoanNo, oLeedBo.i_FinStayId, oLeedBo.i_FinGuestHouseId, oLeedBo.i_LeadId, oLeedBo.i_FinBrokerId, oLeedBo.i_FinProjBlockId, oLeedBo.s_FinComission, oLeedBo.s_FinContPer, oLeedBo.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void InsertFacility(NewLeadBO oLeedBO, string s_Mode, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            try
            {
                if (s_Mode == "A")
                {
                    ssql = String.Format("Insert Into dbo.Facility(FacilityId, Sel, LeadId, Description) values({0}, {1}, {2}, '{3}')", oLeedBO.i_ChklstFacility, oLeedBO.b_FacSel, oLeedBO.i_LeadId, oLeedBO.s_FacDescription);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update dbo.Facility Set Sel={0}, Description='{1}' Where FacilityId={2} and LeadId={3}", oLeedBO.b_FacSel, oLeedBO.s_FacDescription, oLeedBO.i_ChklstFacility, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        //internal static void InsertArea(NewLeadBO oLeedBO, string s_Mode, SqlConnection conn, SqlTransaction tran)
        //{
        //    string ssql = "";
        //    SqlCommand cmd = null;
        //    try
        //    {
        //        if (s_Mode == "A")
        //        {
        //            ssql = String.Format("Insert Into dbo.Area(AreaId, Sel, LeadId, Description) values({0}, {1}, {2}, '{3}')", oLeedBO.i_ChklstArea, oLeedBO.b_AreaSel, oLeedBO.i_LeadId, oLeedBO.s_AreDescription);
        //            cmd = new SqlCommand(ssql, conn, tran);
        //            cmd.ExecuteNonQuery();
        //            cmd.Dispose();
        //        }
        //        else
        //        {
        //            ssql = String.Format("Update dbo.Area Set Sel={0}, Description='{1}' Where AreaId={2} and LeadId={3}", oLeedBO.b_AreaSel, oLeedBO.s_AreDescription, oLeedBO.i_ChklstArea, oLeedBO.i_LeadId);
        //            cmd = new SqlCommand(ssql, conn, tran);
        //            cmd.ExecuteNonQuery();
        //            cmd.Dispose();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        BsfGlobal.CustomException(e.Message, e.StackTrace);
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}

        internal static void InsertPossess(NewLeadBO oLeedBO, string s_Mode, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            try
            {
                if (s_Mode == "A")
                {
                    ssql = String.Format("Insert Into dbo.Possess(PossessId, Sel, LeadId, Description) values({0}, {1}, {2}, '{3}')", oLeedBO.i_ChklstPossId, oLeedBO.b_PossSel, oLeedBO.i_LeadId, oLeedBO.s_PossDescription);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    if (s_Mode == "A in E Mode")
                    {
                        ssql = String.Format("Insert Into dbo.Possess(PossessId, Sel, LeadId, Description) values({0}, {1}, {2}, '{3}')", oLeedBO.i_ChklstPossId, oLeedBO.b_PossSel, oLeedBO.i_LeadId, oLeedBO.s_PossDescription);
                        cmd = new SqlCommand(ssql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    else
                    {
                        ssql = String.Format("Update dbo.Possess Set Sel={0}, Description='{1}' Where PossessId={2} and LeadId={3}", oLeedBO.b_PossSel, oLeedBO.s_PossDescription, oLeedBO.i_ChklstPossId, oLeedBO.i_LeadId);
                        cmd = new SqlCommand(ssql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void InsertBankDet(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            try
            {
                if (s_Mode == "A" && argUpdateLead == false)
                {
                    ssql = String.Format("Insert Into dbo.LeadBankDetails(ContactPerson, ContactMobileNo, BankName, LoanNo, Branch, InterestRate, LoanAmount,Need, LeadId) values('{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6},'{7}',{8})", oLeedBO.s_BankContPer, oLeedBO.s_BankContNo, oLeedBO.s_BankName, oLeedBO.s_BankLoanNo, oLeedBO.s_BankBranch, oLeedBO.i_BankIntRate, oLeedBO.i_BankLoanAmt, oLeedBO.s_ReqNeed, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update dbo.LeadBankDetails Set ContactPerson='{0}', ContactMobileNo='{1}', BankName='{2}', LoanNo='{3}', Branch='{4}', InterestRate={5}, LoanAmount={6},Need='{7}' Where LeadId={8}", oLeedBO.s_BankContPer, oLeedBO.s_BankContNo, oLeedBO.s_BankName, oLeedBO.s_BankLoanNo, oLeedBO.s_BankBranch, oLeedBO.i_BankIntRate, oLeedBO.i_BankLoanAmt, oLeedBO.s_ReqNeed, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void InsertNRIContDet(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd;
            try
            {
                if (s_Mode == "A" && argUpdateLead == false)
                {
                    ssql = String.Format("Insert Into dbo.LeadNRIContactDet(PersonName, Address, City, State, MobileNo, Email, LeadId) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6})", oLeedBO.s_NRIContPerson, oLeedBO.s_NRIContAdd, oLeedBO.s_NRIContCity, oLeedBO.s_NRIContState, oLeedBO.s_NRIContNo, oLeedBO.s_NRIContMail, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update dbo.LeadNRIContactDet Set PersonName='{0}', Address='{1}', City='{2}', State='{3}', MobileNo='{4}', Email='{5}' Where LeadId={6}", oLeedBO.s_NRIContPerson, oLeedBO.s_NRIContAdd, oLeedBO.s_NRIContCity, oLeedBO.s_NRIContState, oLeedBO.s_NRIContNo, oLeedBO.s_NRIContMail, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Register Function

        internal static DataTable LeadLogin(int argLeadId)
        {
            DataTable dtUser = null;
            SqlDataAdapter sdaLName;
            string sSql = "";
            try
            {
                sSql = "Select A.UserName,A.Password,B.Email From dbo.UserLogin A Inner Join dbo.LeadRegister B " +
                    " On A.LeadId=B.LeadId Where B.LeadId='" + argLeadId + "'";
                sdaLName = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dtUser = new DataTable();
                sdaLName.Fill(dtUser);
                dtUser.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtUser;
        }

        internal static bool LeadFound(int argLeadId)
        {
            DataTable dtUser;
            SqlDataAdapter sdaLName;
            string sSql = "";
            bool bAns = false;
            try
            {
                sSql = "Select LeadId From dbo.UserLogin Where LeadId=" + argLeadId + "";
                sdaLName = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dtUser = new DataTable();
                sdaLName.Fill(dtUser);
                if (dtUser.Rows.Count > 0)
                    bAns = false;
                else bAns = true;
                dtUser.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return bAns;
        }

        internal static DataTable ShowUser()
        {
            DataTable dtUser = null;
            SqlDataAdapter sdaLName;
            string sSql = "";

            try
            {
                sSql = "Select UserId,CASE WHEN EmployeeName='' THEN UserName ELSE EmployeeName END EmployeeName " +
                    " FROM dbo.Users ORDER BY EmployeeName,UserName";
                sdaLName = new SqlDataAdapter(sSql, BsfGlobal.OpenWorkFlowDB());
                dtUser = new DataTable();
                sdaLName.Fill(dtUser);
                dtUser.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dtUser;
        }

        internal static DataTable FillLeadGrid(string argFromDate, string argToDate)
        {
            DataTable dtLeadName = null;
            SqlDataAdapter sdaLName;
            string sSql = "";

            try
            {
                sSql = "Select Distinct R.LeadId,R.LeadDate,R.LeadName,B.CampaignName,R.LeadType,R.Mobile,R.Email,R.Remarks From dbo.LeadRegister R " +
                       " Left Join dbo.LeadProjectInfo A On A.LeadId=R.LeadId " +
                       " LEFT Join dbo.CampaignDetails B On A.CampaignId=B.CampaignId " +
                       " Where R.LeadDate Between '" + argFromDate + "' And '" + argToDate + "' Order By R.LeadDate ";
                sdaLName = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dtLeadName = new DataTable();
                sdaLName.Fill(dtLeadName);
                dtLeadName.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtLeadName;

        }

        internal static DataTable ShowLeadDate(string argFromDate, string argToDate, bool argOtherExec)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;

            try
            {
                String sSql = "";

                if (BsfGlobal.FindPermission("Lead-View-Other-Users-Lead") == false)
                {
                    sSql = " Select DISTINCT R.LeadId, R.LeadName,B.CampaignName,R.LeadDate,A.CostCentreId,R.Mobile,R.Email,IsNull(O.CostCentreName,'')CostCentre," +
                           " Case When U.EmployeeName='' Then U.UserName Else IsNull(U.EmployeeName,'') End ExecutiveName,R.CallTypeId," +
                           " CONVERT(VARCHAR, R.Remarks) Remarks,ISNULL(LC.Address1,'') Address1,ISNULL(LC.Address2,'') Address2,ISNULL(LC.Locality,'') Locality," +
                           " ISNULL(CM.CityName,'') City,ISNULL(LC.PinCode,'') PinCode, "+
                           " Case When C.Gender='F' Then 'Female' When C.Gender='M' Then 'Male' Else 'None' End Gender From dbo.LeadRegister R  " +
                           " LEFT JOIN dbo.LeadCommAddressInfo LC ON R.LeadId=LC.LeadId " +
                           " LEFT Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CityMaster CM on LC.CityId=CM.CityId " +
                           " LEFT Join dbo.LeadProjectInfo A On A.LeadId=R.LeadId " +
                           " LEFT Join dbo.CampaignDetails B On A.CampaignId=B.CampaignId " +
                           " LEFT Join dbo.LeadExecutiveInfo E On A.LeadID=E.LeadId " +
                           " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O On O.CostCentreId=A.CostCentreId " +
                           " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users U On U.UserId=A.ExecutiveId " +
                           " LEFT Join dbo.LeadPersonalInfo C On C.LeadId=R.LeadId " +
                           " Where R.LeadDate Between '" + argFromDate + "' And '" + argToDate + "'";

                    if (argOtherExec == false)
                    {
                        sSql = sSql + " and E.ExecutiveId = " + BsfGlobal.g_lUserId + " Order By R.LeadDate";
                    }
                    else
                    {
                        sSql = sSql + " AND E.ExecutiveId In " +
                         " (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users Where UserId=" + BsfGlobal.g_lUserId + " or " +
                         " UserId in (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans Where sUserId=" + BsfGlobal.g_lUserId + ")) Order By R.LeadDate ";
                    }
                }
                else
                {
                    sSql = " Select DISTINCT R.LeadId, R.LeadName,B.CampaignName,R.LeadDate,A.CostCentreId,R.Mobile,R.Email,IsNull(O.CostCentreName,'')CostCentre," +
                           " Case When U.EmployeeName='' Then U.UserName Else IsNull(U.EmployeeName,'') End ExecutiveName,R.CallTypeId, " +
                           " CONVERT(VARCHAR, R.Remarks) Remarks,ISNULL(LC.Address1,'') Address1,ISNULL(LC.Address2,'') Address2,ISNULL(LC.Locality,'') Locality, " +
                           " ISNULL(CM.CityName,'') City,ISNULL(LC.PinCode,'') PinCode, "+
                           " Case When C.Gender='F' Then 'Female' When C.Gender='M' Then 'Male' Else 'None' End Gender From dbo.LeadRegister R  " +
                           " LEFT JOIN dbo.LeadCommAddressInfo LC ON R.LeadId=LC.LeadId " +
                           " LEFT Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CityMaster CM on LC.CityId=CM.CityId " +
                           " LEFT Join dbo.LeadProjectInfo A On A.LeadId=R.LeadId" +
                           " LEFT Join dbo.CampaignDetails B On A.CampaignId=B.CampaignId " +
                           " LEFT Join dbo.LeadExecutiveInfo E On A.LeadId=E.LeadId " +
                           " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O On O.CostCentreId=A.CostCentreId " +
                           " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users U On U.UserId=A.ExecutiveId " +
                           " LEFT Join dbo.LeadPersonalInfo C On C.LeadId=R.LeadId " +
                           " Where R.LeadDate Between '" + argFromDate + "' And '" + argToDate + "' ";
                    if (argOtherExec == false)
                    {
                        sSql = sSql + " Order By R.LeadDate";
                    }
                    else
                    {
                        sSql = sSql + " AND E.ExecutiveId In " +
                         " (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users Where UserId=" + BsfGlobal.g_lUserId + " or " +
                         " UserId in (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans Where sUserId=" + BsfGlobal.g_lUserId + ")) Order By R.LeadDate ";
                    }
                }

                if (sSql != "")
                {
                    SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    SqlDataReader dreader = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(dreader);
                    dreader.Close();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowApartInfo(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadApartmentInfo Where LeadId=" + e_NewLeadBO.i_LeadId + "" + "Order By LeadId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowChildInfo(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadChildInfo Where LeadId=" + e_NewLeadBO.i_LeadId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowCoAppAddInfo(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadCoAppAddressInfo Where LeadId=" + e_NewLeadBO.i_LeadId + "" + "Order By LeadId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowCoAppInfo(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadCoApplicantInfo Where LeadId=" + e_NewLeadBO.i_LeadId + "" + "Order By LeadId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowCommAddInfo(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadCommAddressInfo Where LeadId=" + e_NewLeadBO.i_LeadId + "" + "Order By LeadId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowPOAInfo(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadPOAInfo Where LeadId=" + e_NewLeadBO.i_LeadId + "" + "Order By LeadId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowPOAAddInfo(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadPOAAddressInfo Where LeadId=" + e_NewLeadBO.i_LeadId + "" + "Order By LeadId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowExeInfo(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadExecutiveInfo Where LeadId=" + e_NewLeadBO.i_LeadId + "" + "Order By LeadId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowFinance(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadFinance Where LeadId=" + e_NewLeadBO.i_LeadId + "" + "Order By LeadId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowRequirement(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select LeadId, Facility, Area, Remarks  FROM dbo.LeadRequirement" +
                    " Where LeadId=" + e_NewLeadBO.i_LeadId + "" + "Order By LeadId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowNRIAddInfo(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadNRIAddressInfo Where LeadId=" + e_NewLeadBO.i_LeadId + "" + "Order By LeadId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowOffAddInfo(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadOffAddressInfo Where LeadId=" + e_NewLeadBO.i_LeadId + "" + "Order By LeadId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowPermAddInfo(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadPermanentAddressInfo Where LeadId=" + e_NewLeadBO.i_LeadId + "" + "Order By LeadId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowPersonalInfo(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadPersonalInfo Where LeadId=" + e_NewLeadBO.i_LeadId + "" + "Order By LeadId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowProjectInfo(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadProjectInfo Where LeadId=" + e_NewLeadBO.i_LeadId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowSource(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadSource Where LeadId=" + e_NewLeadBO.i_LeadId + "" + "Order By LeadId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowSubSource(NewLeadBO e_NewLeadBO)
        {
            DataTable dtLeadName = null;
            SqlDataAdapter sdaLName;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadSubSource Where LeadId=" + e_NewLeadBO.i_LeadId + "" + "Order By LeadId";
                sdaLName = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dtLeadName = new DataTable();
                sdaLName.Fill(dtLeadName);
                dtLeadName.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtLeadName;
        }

        internal static DataTable ShowLeadType(NewLeadBO e_NewLeadBO)
        {
            DataTable dtLeadType = null;
            SqlDataAdapter sdaLType = null;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadType Where LeadId=" + e_NewLeadBO.i_LeadId + "" + "Order By LeadId";
                sdaLType = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dtLeadType = new DataTable();
                sdaLType.Fill(dtLeadType);
                dtLeadType.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtLeadType;
        }

        internal static DataTable ShowLeadDetails(NewLeadBO e_NewLeadBO)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtLeadName = null;
            SqlDataAdapter sdaLName;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadRegister Where LeadId=" + e_NewLeadBO.i_LeadId + "" + "Order By LeadId";
                sdaLName = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dtLeadName = new DataTable();
                sdaLName.Fill(dtLeadName);
                dtLeadName.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtLeadName;
        }

        internal static DataTable ShowFacility(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select Distinct A.FacilityId as Id, A.Description as Name, Case when B.Sel=0" +
                    " Then  Convert(bit, 0, 0) else Convert(bit, 1, 1) End as Sel, " +
                    " LeadId From dbo.FacilityMaster A Left Join dbo.Facility B on A.FacilityId=B.FacilityId " +
                    " And LeadId=" + e_NewLeadBO.i_LeadId + " Order By A.FacilityId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowArea(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select Distinct LeadId,A.AreaId as Id, A.Description as Name, Case when B.Sel=0" +
                    " Then Convert(bit, 0, 0) else Convert(bit, 1, 1) End as Sel " +
                    " From dbo.LeadAreaMaster A Left Join dbo.Area B on A.AreaId=B.AreaId " +
                    " And LeadId=" + e_NewLeadBO.i_LeadId + " Order By A.AreaId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowPossess(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select Distinct A.PossessId as Id, A.Description as Name, Case when B.Sel=0" +
                    " Then Convert(bit, 0, 0) else Convert(bit, 1, 1) End as Sel," +
                    " LeadId From dbo.PossessMaster A Left Join dbo.Possess B On A.PossessId=B.PossessId " +
                    " And LeadId=" + e_NewLeadBO.i_LeadId + " Order By A.PossessId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowNRIContDet(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadNRIContactDet Where LeadId=" + e_NewLeadBO.i_LeadId + "" + " Order By LeadId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowBankDet(NewLeadBO e_NewLeadBO)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadBankDetails Where LeadId=" + e_NewLeadBO.i_LeadId + "" + " Order By LeadId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable ShowLeadDate(string argFromDate, string argToDate)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql = "";

            sSql = " Select Distinct R.LeadId, R.LeadName,R.LeadDate,0 CostCentreId From dbo.LeadRegister R  " +
                     " Left Join dbo.LeadProjectInfo A On A.LeadId=R.LeadId " +
                     " Where R.LeadDate Between '" + argFromDate + "' And '" + argToDate + "'  Order By R.LeadDate";
            try
            {
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable GetCompanyMailDetails()
        {
            DataTable dt = null;
            try
            {
                string querey = "Select * from CompanyMailSetting";

                BsfGlobal.OpenWorkFlowDB();
                SqlDataAdapter da = new SqlDataAdapter(querey, BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dt;
        }

        public static DataTable GetLeadFlatTypeDetails(int argProjId, string argsType, int argLandId, int argLeadId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            string sql = "";
            try
            {
                if (argProjId == 0) { sql = "SELECT 0 FlatTypeId,'' TypeName "; }
                else
                {
                    if (argLeadId != 0)
                    {
                        if (argsType == "B")
                            sql = "SELECT FlatTypeId,TypeName FROM dbo.FlatType WHERE ProjId In(Select CostCentreId From dbo.LeadProjectInfo Where LeadId=" + argLeadId + ")";
                        else
                        {
                            sql = "Select PlotTypeId FlatTypeId,PlotTypeName TypeName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotType " +
                                  " Where LandRegisterId=" + argLandId + "";
                        }
                    }
                    else
                    {
                        if (argsType == "B")
                            sql = "SELECT FlatTypeId,TypeName FROM dbo.FlatType WHERE ProjId=" + argProjId;
                        else
                            sql = "Select PlotTypeId FlatTypeId,PlotTypeName TypeName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotType " +
                                " Where LandRegisterId=" + argLandId + "";
                    }
                }
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        #endregion

        #region Delete Functions

        internal static void DelFacility(int argLeadId, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            try
            {
                ssql = String.Format("Delete From dbo.Facility Where LeadId={0}", argLeadId);
                cmd = new SqlCommand(ssql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void DelArea(int argLeadId, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            try
            {
                ssql = String.Format("Delete From dbo.Area Where LeadId={0}", argLeadId);
                cmd = new SqlCommand(ssql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        internal static void DelPoss(int argLeadId, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            try
            {
                ssql = String.Format("Delete From dbo.Possess Where LeadId={0}", argLeadId);
                cmd = new SqlCommand(ssql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Lead Insert Data
        public static void insertentry(NewLeadBL insertleaddata)
        {
            //BsfGlobal.OpenCRMDB();
            //SqlCommand com=null;
            //string L_LeadDate=string.Format("{0:dd/mm/yyyy}",NewLeadBL.LeadDate);
            //string s=string.Format("insert into dbo.MCRM values(

        }
        #endregion

        #region PickList

        public static DataTable GetPLMaster(string argType)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                if (argType == "Area")
                    sSql = "SELECT AreaId Id, Description FROM dbo.LeadAreaMaster Order By Description";
                else if (argType == "Cost")
                    sSql = "SELECT CostPreferenceId Id, str(cast(CostPreferenceFrom  as decimal(18,3))) + '   -' + str(cast(CostPreferenceTo as decimal(18,3))) Description FROM dbo.CostPreference";
                else if (argType == "EmpType")
                    sSql = "SELECT EmploymentId Id, Description FROM dbo.Employment Order By Description";
                else if (argType == "Income")
                    sSql = "SELECT IncomeId as Id, str(cast(IncomeFrom as float),10) + '   -' + str(cast(IncomeTo as float),12) as Description FROM dbo.Income ";
                else if (argType == "Apt")
                    sSql = "SELECT ApartmentId Id, Description FROM dbo.Apartment Order By Description";
                else if (argType == "AptSize")
                    sSql = "SELECT ApartmentSizeId as Id, str(cast(ApartmentSizeFrom as int),10) + ' -' + str(cast(ApartmentSizeTo as int),10) as Description FROM dbo.ApartmentSize";
                else if (argType == "Stay")
                    sSql = "SELECT StayId Id, Description FROM dbo.Stay Order By Description";
                else if (argType == "Guest")
                    sSql = "SELECT GuestHouseId Id, Description FROM dbo.GuestHouse Order By Description";
                else if (argType == "Possess")
                    sSql = "SELECT PossessId Id, Description FROM dbo.PossessMaster Order By Description";
                else if (argType == "Facility")
                    sSql = "SELECT FacilityId Id, Description FROM dbo.FacilityMaster Order By Description";
                else if (argType == "Nation")
                    sSql = "SELECT NationalityId Id, Description FROM dbo.NationalityMaster Order By Description";
                else if (argType == "Profession")
                    sSql = "SELECT ProfessionId Id, Description FROM dbo.ProfessionMaster Order By Description";
                else if (argType == "Country")
                    sSql = "SELECT CountryId Id, CountryName Description FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CountryMaster Order By Description";

                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable GetPLData(int argLeadId,string argType)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtMaster = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                if (argType == "Area")
                {
                    if (argLeadId == 0)
                        sSql = "SELECT AreaId, Description,CONVERT(bit,0,1) Sel FROM dbo.LeadAreaMaster Order By Description";
                    else
                        sSql = "Select AreaId, Description, " +
                                " Sel=Isnull((Select Distinct Sel From Area Where AreaId=A.AreaId And LeadId=" + argLeadId + "),0) From LeadAreaMaster A ";
                }
                else if (argType == "Possess")
                {
                    if (argLeadId == 0)
                        sSql = "SELECT PossessId, Description,CONVERT(bit,0,1) Sel FROM dbo.PossessMaster Order By Description";
                    else
                        sSql = "Select PossessId, Description, " +
                                " Sel=Isnull((Select Distinct Sel From dbo.Possess Where PossessId=A.PossessId And LeadId=" + argLeadId + "),0) From dbo.PossessMaster A ";
                }
                else if (argType == "Facility")
                {
                    if (argLeadId == 0)
                        sSql = "SELECT FacilityId, Description,CONVERT(bit,0,1) Sel FROM dbo.FacilityMaster Order By Description";
                    else
                        sSql = "Select FacilityId, Description, " +
                                " Sel=Isnull((Select Distinct Sel From dbo.Facility Where FacilityId=A.FacilityId And LeadId=" + argLeadId + "),0) From dbo.FacilityMaster A ";
                }
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dtMaster = new DataTable();
                sda.Fill(dtMaster);
                dtMaster.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtMaster;
        }

        public static void InsertPLMaster(string argDesc,string argType)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand cmd;
            string[] a = argDesc.Split('-');
            try
            {
                if (argType == "Area")
                    ssql = "Insert Into dbo.LeadAreaMaster(Description) Values('" + argDesc + "')";
                else if (argType == "Cost")
                    ssql = "INSERT INTO dbo.CostPreference (CostPreferenceFrom, CostPreferenceTo) VALUES(" + a[0] + ", " + a[1] + ")";
                else if (argType == "EmpType")
                    ssql = "INSERT INTO dbo.Employment(Description) Values('" + argDesc + "')";
                else if (argType == "Income")
                    ssql = "INSERT INTO dbo.Income(IncomeFrom, IncomeTo) VALUES(" + a[0] + ", " + a[1] + ")";
                else if (argType == "Apt")
                    ssql = "INSERT INTO dbo.Apartment(Description) Values('" + argDesc + "')";
                else if (argType == "AptSize")
                    ssql = "INSERT INTO ApartmentSize (ApartmentSizeFrom, ApartmentSizeTo) VALUES(" + a[0] + ", " + a[1] + ")";
                else if (argType == "Stay")
                    ssql = "INSERT INTO dbo.Stay(Description) Values('" + argDesc + "')";
                else if (argType == "Guest")
                    ssql = "INSERT INTO dbo.GuestHouse(Description) Values('" + argDesc + "')";
                else if (argType == "Possess")
                    ssql = "INSERT INTO dbo.PossessMaster(Description) Values('" + argDesc + "')";
                else if (argType == "Facility")
                    ssql = "INSERT INTO dbo.FacilityMaster(Description) Values('" + argDesc + "')";
                else if (argType == "Nation")
                    ssql = "INSERT INTO dbo.NationalityMaster(Description) Values('" + argDesc + "')";
                else if (argType == "Profession")
                    ssql = "INSERT INTO dbo.ProfessionMaster(Description) Values('" + argDesc + "')";
                else if (argType == "Country")
                    ssql = "INSERT INTO [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CountryMaster(CountryName) Values('" + argDesc + "')";

                cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ce)
            {
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
        }

        public static void UpdatePLMaster(int argId, string argDesc, string argType)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand cmd;
            string[] a = argDesc.Split('-');
            try
            {
                if (argType == "Area")
                    ssql = "Update dbo.LeadAreaMaster Set Description='" + argDesc + "' Where AreaId=" + argId + "";
                else if (argType == "Cost")
                    ssql = "Update dbo.CostPreference Set CostPreferenceFrom=" + a[0] + ", CostPreferenceTo=" + a[1] + "  Where CostPreferenceId=" + argId + "";
                else if (argType == "EmpType")
                    ssql = "Update dbo.Employment Set Description='" + argDesc + "' Where EmploymentId=" + argId + "";
                else if (argType == "Income")
                    ssql = "Update dbo.Income Set IncomeFrom=" + a[0] + ", IncomeTo=" + a[1] + " Where IncomeId=" + argId + "";
                else if (argType == "Apt")
                    ssql = "Update dbo.Apartment Set Description='" + argDesc + "' Where ApartmentId=" + argId + "";
                else if (argType == "AptSize")
                    ssql = "Update dbo.ApartmentSize Set ApartmentSizeFrom=" + a[0] + ", ApartmentSizeTo=" + a[1] + " Where ApartmentSizeId=" + argId + "";
                else if (argType == "Stay")
                    ssql = "Update dbo.Stay Set Description='" + argDesc + "' Where StayId=" + argId + "";
                else if (argType == "Guest")
                    ssql = "Update dbo.GuestHouse Set Description='" + argDesc + "' Where GuestHouseId=" + argId + "";
                else if (argType == "Possess")
                    ssql = "Update dbo.PossessMaster Set Description='" + argDesc + "' Where PossessId=" + argId + "";
                else if (argType == "Facility")
                    ssql = "Update dbo.FacilityMaster Set Description='" + argDesc + "' Where FacilityId=" + argId + "";
                else if (argType == "Nation")
                    ssql = "Update dbo.NationalityMaster Set Description='" + argDesc + "' Where NationalityId=" + argId + "";
                else if (argType == "Profession")
                    ssql = "Update dbo.ProfessionMaster Set Description='" + argDesc + "' Where ProfessionId=" + argId + "";
                else if (argType == "Country")
                    ssql = "Update [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CountryMaster Set CountryName='" + argDesc + "' Where CountryId=" + argId + "";

                cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ce)
            {
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
        }

        public static bool CheckPLId(int argId, string argType)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt;
            bool bExist = false;string sSql = "";
            try
            {
                dt = new DataTable();
                if (argType == "Area")
                    sSql = "SELECT AreaId FROM dbo.Area WHERE AreaId=" + argId + "";
                else if (argType == "Cost")
                    sSql = "SELECT CostId FROM dbo.LeadRegister WHERE CostId=" + argId + "";
                else if (argType == "EmpType")
                    sSql = "SELECT Employment FROM dbo.LeadFinance WHERE Employment=" + argId + "";
                else if (argType == "Income")
                    sSql = "SELECT Income FROM dbo.LeadFinance WHERE Income=" + argId + "";
                else if (argType == "Apt")
                    sSql = "SELECT ApartmentId FROM dbo.LeadApartmentInfo WHERE ApartmentId=" + argId + "";
                else if (argType == "AptSize")
                    sSql = "SELECT Size FROM dbo.LeadApartmentInfo WHERE Size=" + argId + "";
                else if (argType == "Stay")
                    sSql = "SELECT Stay FROM dbo.LeadFinance WHERE Stay=" + argId + "";
                else if (argType == "Guest")
                    sSql = "SELECT GuestHouse FROM dbo.LeadFinance WHERE GuestHouse=" + argId + "";
                else if (argType == "Possess")
                    sSql = "SELECT PossessId FROM dbo.Possess WHERE PossessId=" + argId + "";
                else if (argType == "Facility")
                    sSql = "SELECT FacilityId FROM dbo.Facility WHERE FacilityId=" + argId + "";
                else if (argType == "Nation")
                    sSql = "SELECT NationalityId FROM dbo.LeadPersonalInfo WHERE NationalityId=" + argId + "" +
                        " UNION ALL " +
                        " SELECT NationalityId FROM dbo.LeadPOAInfo WHERE NationalityId=" + argId + "" +
                        " UNION ALL " +
                        " SELECT NationalityId FROM dbo.LeadCoApplicantInfo WHERE NationalityId=" + argId + "";
                else if (argType == "Profession")
                    sSql = "SELECT ProfessionId FROM dbo.LeadPersonalInfo WHERE ProfessionId=" + argId + "" +
                        " UNION ALL " +
                        " SELECT ProfessionId FROM dbo.LeadPOAInfo WHERE ProfessionId=" + argId + "" +
                        " UNION ALL " +
                        " SELECT ProfessionId FROM dbo.LeadCoApplicantInfo WHERE ProfessionId=" + argId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                sda.Fill(dt);
                if (dt.Rows.Count == 0)
                    bExist = false;
                else
                    bExist = true;

            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "Lead", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information); ;
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }

            return bExist;

        }

        public static bool CheckDesc(string argDesc, string argType)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt;
            bool bExist = false; string sSql = "";
            string[] a = argDesc.Split('-');
            try
            {
                dt = new DataTable();
                if (argType == "Area")
                    sSql = "SELECT Description FROM dbo.LeadAreaMaster WHERE Description='" + argDesc + "'";
                else if (argType == "Cost")
                    sSql = "SELECT CostPreferenceFrom,CostPreferenceTo FROM dbo.CostPreference WHERE CostPreferenceFrom=" + a[0] + " And CostPreferenceTo=" + a[1] + "";
                else if (argType == "EmpType")
                    sSql = "SELECT Description FROM dbo.Employment WHERE Description='" + argDesc + "'";
                else if (argType == "Income")
                    sSql = "SELECT IncomeFrom,IncomeTo FROM dbo.Income WHERE IncomeFrom=" + a[0] + " And IncomeTo=" + a[1] + "";
                else if (argType == "Apt")
                    sSql = "SELECT Description FROM dbo.Apartment WHERE Description='" + argDesc + "'";
                else if (argType == "AptSize")
                    sSql = "SELECT ApartmentSizeFrom,ApartmentSizeTo FROM dbo.ApartmentSize WHERE ApartmentSizeFrom=" + a[0] + " And ApartmentSizeTo=" + a[1] + "";
                else if (argType == "Stay")
                    sSql = "SELECT Description FROM dbo.Stay WHERE Description='" + argDesc + "'";
                else if (argType == "Guest")
                    sSql = "SELECT Description FROM dbo.GuestHouse WHERE Description='" + argDesc + "'";
                else if (argType == "Possess")
                    sSql = "SELECT Description FROM dbo.PossessMaster WHERE Description='" + argDesc + "'";
                else if (argType == "Facility")
                    sSql = "SELECT Description FROM dbo.FacilityMaster WHERE Description='" + argDesc + "'";
                else if (argType == "Nation")
                    sSql = "SELECT Description FROM dbo.NationalityMaster WHERE Description='" + argDesc + "'";
                else if (argType == "Profession")
                    sSql = "SELECT Description FROM dbo.ProfessionMaster WHERE Description='" + argDesc + "'";
                else if (argType == "Country")
                    sSql = "SELECT CountryName FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CountryMaster WHERE CountryName='" + argDesc + "'";

                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                sda.Fill(dt);
                if (dt.Rows.Count == 0)
                    bExist = false;
                else
                    bExist = true;
                dt.Dispose();
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "Lead", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }

            return bExist;

        }

        public static void DeletePLMaster(int argId, string argType)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand cmd;
            try
            {
                if(argType=="Area")
                ssql = "Delete From dbo.LeadAreaMaster Where AreaId=" + argId + "";
                else if(argType=="Cost")
                    ssql = "Delete From dbo.CostPreference Where CostPreferenceId=" + argId + "";
                else if (argType == "EmpType")
                    ssql = "Delete From Employment Where EmploymentId=" + argId + "";
                else if (argType == "Income")
                    ssql = "Delete From dbo.Income Where IncomeId=" + argId + "";
                else if (argType == "Apt")
                    ssql = "Delete From dbo.Apartment Where ApartmentId=" + argId + "";
                else if (argType == "AptSize")
                    ssql = "Delete From dbo.ApartmentSize Where ApartmentSizeId=" + argId + "";
                else if (argType == "Stay")
                    ssql = "Delete From dbo.Stay Where StayId=" + argId + "";
                else if (argType == "Guest")
                    ssql = "Delete From dbo.GuestHouse Where GuestHouseId=" + argId + "";
                else if (argType == "Possess")
                    ssql = "Delete From dbo.PossessMaster Where PossessId=" + argId + "";
                else if (argType == "Facility")
                    ssql = "Delete From dbo.FacilityMaster Where FacilityId=" + argId + "";
                else if (argType == "Nation")
                    ssql = "Delete From dbo.NationalityMaster Where NationalityId=" + argId + "";
                else if (argType == "Profession")
                    ssql = "Delete From dbo.ProfessionMaster Where ProfessionId=" + argId + "";

                cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ce)
            {
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
        }

        #endregion

        #region Religion Master

        public static DataTable GetReligionMaster()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "SELECT ReligionId,ReligionName Description FROM dbo.ReligionMaster ORDER BY ReligionName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static void UpdateReligionMaster(int argRelId, string argDesc)
        {
            DataTable dt;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "UPDATE dbo.ReligionMaster SET ReligionName='" + argDesc + "' WHERE ReligionId=" + argRelId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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
                BsfGlobal.g_CRMDB.Close();
            }
        }

        public static void InsertReligionMaster(string argDesc)
        {
            DataTable dt;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "INSERT INTO dbo.ReligionMaster (ReligionName) VALUES ('" + argDesc + "')";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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
                BsfGlobal.g_CRMDB.Close();
            }
        }

        public static void DeleteReligionMaster(int argRelId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "DELETE FROM dbo.ReligionMaster WHERE ReligionId=" + argRelId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

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

        public static bool FoundReligion(int argRelId)
        {
            bool bAns = false;
            DataTable dt = new DataTable();
            try
            {
                string sSql = " Select LeadId From LeadPOAInfo Where Religion= " + argRelId + " " +
                        " Union All " +
                        " Select LeadId From LeadCoApplicantInfo Where Religion = " + argRelId + "" +
                         " Union All " +
                        " Select LeadId From LeadPersonalInfo Where Religion = " + argRelId + "";
                BsfGlobal.OpenCRMDB();
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    bAns = true;
                }
                dr.Close();
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
            return bAns;
        }

        public static bool CheckRelDesc(string argDesc)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt;
            bool bExist = false; string sSql = "";
            try
            {
                dt = new DataTable();
                sSql = "SELECT ReligionName FROM dbo.ReligionMaster WHERE ReligionName='" + argDesc + "'";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                sda.Fill(dt);
                if (dt.Rows.Count == 0)
                    bExist = false;
                else
                    bExist = true;
                dt.Dispose();
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "Lead", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }

            return bExist;

        }


        #endregion

        #region LeadDetails

        internal static DataTable ShowLeadName()
        {
            DataTable dtLeadName = null;
            SqlDataAdapter sdaLName;
            string sSql = "";

            try
            {
                //sSql = "Select R.LeadId, R.LeadName,A.CostCentreId From dbo.LeadRegister R " +
                //    "  Inner Join dbo.LeadProjectInfo A On A.LeadId=R.LeadId " +
                //    "  Order By LeadName";
                sSql = "Select Distinct R.LeadId, R.LeadName,0 CostCentreId From dbo.LeadRegister R " +
                       " Left Join dbo.LeadProjectInfo A On A.LeadId=R.LeadId Order By LeadName";
                sdaLName = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dtLeadName = new DataTable();
                sdaLName.Fill(dtLeadName);
                dtLeadName.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtLeadName;
        }

        internal static bool Update_LeadDet(List<NewLeadBO> ProjectSel, DataTable dtA, DataTable dtP, DataTable dtF, DataTable dtBD, DataTable dtChk)
        {
            bool bUpdate = false;
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();

            string sSql = "Select Isnull(B.PositionType,'') PositionType,A.PowerUser From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                          " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B On A.PositionId=B.PositionId" +
                          " Where A.UserId=" + BsfGlobal.g_lUserId + "";
            SqlCommand cmd = new SqlCommand(sSql, conn, tran);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dtE = new DataTable();
            dtE.Load(dr);
            dr.Close();
            cmd.Dispose();

            if (dtE.Rows.Count > 0)
            {
                if (dtE.Rows[0]["PositionType"].ToString() == "M" || Convert.ToInt32(dtE.Rows[0]["PowerUser"]) == 1)
                {
                    NewLeadBL.Exe_ExecutiveId = BsfGlobal.g_lUserId;
                    if (NewLeadBL.Exe_ExecutiveId == 1) { NewLeadBL.Exe_ExecutiveId = 0; }
                }
                else
                {
                    NewLeadBL.Exe_ExecutiveId = 0;
                }
            }

            //string sUserType = GetType(sSql, conn, tran);
            //if (sUserType == "M")
            //    NewLeadBL.Exe_ExecutiveId = BsfGlobal.g_lUserId;
            //else
            //    NewLeadBL.Exe_ExecutiveId = 0;

            string sCallType = NewLeadBL.CallType;
            BsfGlobal.VoucherType oVType = new BsfGlobal.VoucherType();
            BsfGlobal.VoucherType oVCCType = new BsfGlobal.VoucherType();
            BsfGlobal.VoucherType oVCompanyType = new BsfGlobal.VoucherType();
            try
            {
                int iLeadId = 0;
                if (NewLeadBL.LeadId == 0)
                {
                    //VoucherNo
                    if (NewLeadBL.CallType == "Finalization")
                    {
                        if (dtBD.Rows.Count > 0)
                        {
                            oVType = BsfGlobal.GetVoucherNo(85, Convert.ToDateTime(NewLeadBL.LeadDate), 0, 0, conn, tran, "I");
                            if (oVType.GenType == true)
                            {
                                dtBD.Rows[0]["GAllotNo"] = oVType.VoucherNo;
                            }

                            oVCCType = BsfGlobal.GetVoucherNo(85, Convert.ToDateTime(NewLeadBL.LeadDate), 0, NewLeadBL.ProjectId, conn, tran, "I");
                            if (oVCCType.GenType == true)
                            {
                                dtBD.Rows[0]["CCAllotNo"] = oVCCType.VoucherNo;
                            }

                            int iCompanyId = CallsheetentryDL.GetCompanyId(NewLeadBL.ProjectId);
                            oVCompanyType = BsfGlobal.GetVoucherNo(85, Convert.ToDateTime(NewLeadBL.LeadDate), iCompanyId, 0, conn, tran, "I");
                            if (oVCompanyType.GenType == true)
                            {
                                dtBD.Rows[0]["COAllotNo"] = oVCompanyType.VoucherNo;
                            }
                        }
                    }

                    //Lead Info
                    if (NewLeadBL.CallType == "Finalization" || NewLeadBL.CallType == "Block" || NewLeadBL.CallType == "Drop")
                    {
                        sSql = "INSERT INTO LeadRegister(LeadName,LeadDate,LeadType,CallTypeId,NatureId,TypeId,UserId,VIP,CostId,Mobile,Email,Remarks,MultiProject) " +
                              " VALUES('" + NewLeadBL.LeadName + "','" + NewLeadBL.LeadDate.ToString("dd-MMM-yyyy") + "'," +
                              "'" + NewLeadBL.LeadType + "'," + NewLeadBL.CallTypeId + "," + NewLeadBL.NatureId + "," +
                              "" + NewLeadBL.UnitType + "," + NewLeadBL.UserId + "," + NewLeadBL.VIP + "," + NewLeadBL.CostPreference + ",'" + NewLeadBL.Mobile + "'," +
                              "'" + NewLeadBL.Email + "','" + NewLeadBL.Remarks + "','" + NewLeadBL.MultiProject + "') SELECT SCOPE_IDENTITY()";
                    }
                    else
                    {
                        sSql = "INSERT INTO LeadRegister(LeadName,LeadDate,LeadType,CallTypeId,NatureId,NextCallDate,TypeId,UserId,VIP,CostId,Mobile,Email,Remarks,MultiProject) " +
                              " VALUES('" + NewLeadBL.LeadName + "','" + NewLeadBL.LeadDate.ToString("dd-MMM-yyyy") + "'," +
                              "'" + NewLeadBL.LeadType + "'," + NewLeadBL.CallTypeId + "," + NewLeadBL.NatureId + ",@NextCallDate," +
                              "" + NewLeadBL.UnitType + "," + NewLeadBL.UserId + "," + NewLeadBL.VIP + "," + NewLeadBL.CostPreference + ",'" + NewLeadBL.Mobile + "'," +
                              "'" + NewLeadBL.Email + "','" + NewLeadBL.Remarks + "','" + NewLeadBL.MultiProject + "') SELECT SCOPE_IDENTITY()";
                    }
                    cmd = new SqlCommand(sSql, conn, tran);

                    SqlParameter parameterDate = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@NextCallDate" };
                    if (NewLeadBL.NextCallDate == DateTime.MinValue)
                        parameterDate.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        parameterDate.Value = NewLeadBL.NextCallDate;
                    cmd.Parameters.Add(parameterDate);

                    iLeadId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();


                    //Executive And Project Info
                    foreach (NewLeadBO obj in ProjectSel)
                    {
                        sSql = "INSERT INTO LeadProjectInfo(CostCentreId,ExecutiveId,CampaignId,LeadId,NextCallDate)  " +
                             " Values(" + obj.ProjectId + "," + NewLeadBL.Exe_ExecutiveId + "," + obj.CampaignId + "," + iLeadId + ",@NextCallDate)";
                        cmd = new SqlCommand(sSql, conn, tran);
                        SqlParameter dateparameter = new SqlParameter() { ParameterName = "@NextCallDate", DbType = DbType.DateTime };
                        if (NewLeadBL.NextCallDate == DateTime.MinValue)
                            dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            dateparameter.Value = NewLeadBL.NextCallDate;
                        cmd.Parameters.Add(dateparameter);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "INSERT INTO dbo.LeadExecutiveInfo(LeadId,ExecutiveId,CostCentreId)" +
                           "Values(" + iLeadId + "," + NewLeadBL.Exe_ExecutiveId + "," + obj.ProjectId + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    for (int i = 0; i < dtA.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dtA.Rows[i]["Sel"]) == true)
                        {
                            sSql = "Insert Into dbo.Area(AreaId, Sel, LeadId, Description) Values (" + dtA.Rows[i]["AreaId"] + ",1," + iLeadId + ",'" + dtA.Rows[i]["Description"] + "')";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }

                    //Personal Info
                    sSql = "INSERT INTO LeadPersonalInfo(LeadId,Gender,DOB,Religion,NationalityId,ProfessionId,Organization," +
                         "FatherName,MotherName,MarritalStatus,WifeName,WeddingDate,NRI) VALUES( " + iLeadId + ",'" + NewLeadBL.Pe_Gender + "',@DOB," +
                          "" + NewLeadBL.Pe_Religion + "," + NewLeadBL.Pe_Nationality + "," + NewLeadBL.Pe_Profession + ",'" + NewLeadBL.Pe_Organization + "'," +
                          "'" + NewLeadBL.Pe_FatherName + "','" + NewLeadBL.Pe_MotherName + "','" + NewLeadBL.Pe_MarritalStatus + "'," +
                          "'" + NewLeadBL.Pe_WifeName + "',@WeddingDate," +
                          "" + NewLeadBL.Pe_NRI + ")";
                    cmd = new SqlCommand(sSql, conn, tran);
                    SqlParameter parameterDOB = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@DOB" };
                    if (NewLeadBL.Pe_DOB == DateTime.MinValue)
                        parameterDOB.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        parameterDOB.Value = NewLeadBL.Pe_DOB;
                    cmd.Parameters.Add(parameterDOB);
                    parameterDOB = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@WeddingDate" };
                    if (NewLeadBL.Pe_WeddingDate == DateTime.MinValue)
                        parameterDOB.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        parameterDOB.Value = NewLeadBL.Pe_WeddingDate;
                    cmd.Parameters.Add(parameterDOB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //NRI Info
                    sSql = "INSERT INTO LeadNRIContactDet(LeadId,PersonName,Address,City,State,MobileNo,Email)Values(" + iLeadId + "," +
                            "'" + NewLeadBL.Pe_ContactPerson + "','" + NewLeadBL.Pe_ContactAdd + "','" + NewLeadBL.Pe_ContactCity + "','" + NewLeadBL.Pe_ContactState + "','" + NewLeadBL.Pe_ContactMobileNo + "','" + NewLeadBL.Pe_ContactMailId + "')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //POA Info
                    sSql = "INSERT INTO LeadPOAInfo(LeadId,ApplicantName,Gender,DOB,Religion,NationalityId,FatherName,MotherName," +
                          "WifeName,WeddingDate,ProfessionId,Organization,MarritalStatus,Relation)Values(" + iLeadId + ",'" + NewLeadBL.PoA_ApplicantName + "'," +
                          "'" + NewLeadBL.PoA_Gender + "',@DOB," + NewLeadBL.PoA_Religion + "," + NewLeadBL.PoA_Nationality + "," +
                          "'" + NewLeadBL.PoA_FatherName + "','" + NewLeadBL.PoA_MotherName + "','" + NewLeadBL.PoA_WifeName + "',@WeddingDate," +
                          "" + NewLeadBL.PoA_Profession + ",'" + NewLeadBL.PoA_Organization + "'," + NewLeadBL.PoA_MarritalStatus + ",'" + NewLeadBL.PoA_Relation + "')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    parameterDOB = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@DOB" };
                    if (NewLeadBL.PoA_DOB == DateTime.MinValue)
                        parameterDOB.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        parameterDOB.Value = NewLeadBL.PoA_DOB;
                    cmd.Parameters.Add(parameterDOB);
                    parameterDOB = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@WeddingDate" };
                    if (NewLeadBL.PoA_WeddingDate == DateTime.MinValue)
                        parameterDOB.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        parameterDOB.Value = NewLeadBL.PoA_WeddingDate;
                    cmd.Parameters.Add(parameterDOB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Co-Applicant Info
                    sSql = "INSERT INTO LeadCoApplicantInfo(LeadId,CoApplicantName,Gender,DOB,Religion,NationalityId,FatherName," +
                      "MotherName,WifeName,WeddingDate,ProfessionId,Organization,MarritalStatus) Values(" + iLeadId + ",'" + NewLeadBL.CoApp_CoApplicantName + "','" + NewLeadBL.CoApp_Gender + "',@DOB," +
                      "" + NewLeadBL.CoApp_Religion + "," + NewLeadBL.CoApp_Nationality + ",'" + NewLeadBL.CoApp_FatherName + "','" + NewLeadBL.CoApp_MotherName + "'," +
                      "'" + NewLeadBL.CoApp_WifeName + "',@WeddingDate," + NewLeadBL.CoApp_Profession + "," +
                      "'" + NewLeadBL.CoApp_Organization + "'," + NewLeadBL.CoApp_MarritalStatus + ")";
                    cmd = new SqlCommand(sSql, conn, tran);
                    parameterDOB = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@DOB" };
                    if (NewLeadBL.CoApp_DOB == DateTime.MinValue)
                        parameterDOB.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        parameterDOB.Value = NewLeadBL.CoApp_DOB;
                    cmd.Parameters.Add(parameterDOB);
                    parameterDOB = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@WeddingDate" };
                    if (NewLeadBL.CoApp_WeddingDate == DateTime.MinValue)
                        parameterDOB.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        parameterDOB.Value = NewLeadBL.CoApp_WeddingDate;
                    cmd.Parameters.Add(parameterDOB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Child info
                    sSql = "INSERT INTO LeadChildInfo(LeadId,ChildName,Sex,DOB)" +
                           "Values(" + iLeadId + ",'" + NewLeadBL.C_ChildName + "','" + NewLeadBL.C_Sex + "',@DOB)";
                    cmd = new SqlCommand(sSql, conn, tran);
                    parameterDOB = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@DOB" };
                    if (NewLeadBL.C_DOB == DateTime.MinValue)
                        parameterDOB.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        parameterDOB.Value = NewLeadBL.C_DOB;
                    cmd.Parameters.Add(parameterDOB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Finance Info
                    sSql = "INSERT INTO LeadFinance(LeadId,Employment,Income,Apartment,ApartmentSize," +
                          "Stay,GuestHouse,Broker,Comission,ContactPerson)Values(" + iLeadId + "," + NewLeadBL.F_Employment + "," +
                          "" + NewLeadBL.F_Income + "," + NewLeadBL.F_Appatment + "," + NewLeadBL.F_AppatmentSize + "," + NewLeadBL.F_Stay + "," + NewLeadBL.F_GuestHouse + "," +
                          "" + NewLeadBL.F_Broker + "," + NewLeadBL.F_Comission + ",'" + NewLeadBL.F_ContactPerson + "')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    for (int i = 0; i < dtP.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dtP.Rows[i]["Sel"]) == true)
                        {
                            sSql = "Insert Into dbo.Possess(PossessId, Sel, LeadId, Description) Values (" + dtP.Rows[i]["PossessId"] + ",1," + iLeadId + ",'" + dtP.Rows[i]["Description"] + "')";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }

                    //Bank Details
                    sSql = "INSERT INTO LeadBankDetails(LeadId,ContactPerson,ContactMobileNo,BankName,LoanNo,Branch,InterestRate,LoanAmount,Need)" +
                           "Values(" + iLeadId + ",'" + NewLeadBL.B_ContactPerson + "','" + NewLeadBL.B_ContactMobileNo + "','" + NewLeadBL.B_BankName + "'," +
                           "'" + NewLeadBL.B_LoanNo + "','" + NewLeadBL.B_Branch + "'," + NewLeadBL.B_InterestRate + "," + NewLeadBL.B_LoanAmount + ",'" + NewLeadBL.B_Need + "')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //POA Address Info
                    sSql = "INSERT INTO LeadPOAAddressInfo(LeadId,Address1,Address2,City,State,Country,PinCode," +
                           "LandLine,Mobile,Email,Fax,PanNo) VALUES( " + iLeadId + ",'" + NewLeadBL.PoAA_Address1 + "','" + NewLeadBL.PoAA_Address2 + "'," +
                          "'" + NewLeadBL.PoAA_City + "','" + NewLeadBL.PoAA_State + "'," + NewLeadBL.PoAA_Country + ",'" + NewLeadBL.PoAA_PinCode + "'," +
                          "'" + NewLeadBL.PoAA_LandLine + "','" + NewLeadBL.PoAA_Mobile + "','" + NewLeadBL.PoAA_Email + "'," +
                          "'" + NewLeadBL.PoAA_Fax + "','" + NewLeadBL.PoAA_PanNo + "')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Permanent Address Info
                    sSql = "INSERT INTO LeadPermanentAddressInfo(LeadId,Address1,Address2,Locality,City,State," +
                            "Country,PinCode,LandLine,Mobile,Email,Fax,PanNo)Values(" + iLeadId + ",'" + NewLeadBL.P_Address1 + "','" + NewLeadBL.P_Address2 + "','" + NewLeadBL.P_Locality + "'," +
                            "'" + NewLeadBL.P_City + "','" + NewLeadBL.P_State + "'," + NewLeadBL.P_Country + ",'" + NewLeadBL.P_PinCode + "','" + NewLeadBL.P_LandLine + "','" + NewLeadBL.P_Mobile + "'," +
                            "'" + NewLeadBL.P_Email + "','" + NewLeadBL.P_Fax + "','" + NewLeadBL.P_PanNo + "')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Co-Applicant Address Info
                    sSql = "INSERT INTO LeadCoAppAddressInfo(LeadId,CoAddress1,CoAddress2,CoCity,CoState,CoCountry," +
                            "CoPinCode,CoLandLine,CoMobile,CoEmail,CoFax,CoPanNo)Values(" + iLeadId + ",'" + NewLeadBL.CoA_Address1 + "'," +
                            "'" + NewLeadBL.CoA_Address2 + "','" + NewLeadBL.CoA_City + "','" + NewLeadBL.CoA_State + "'," + NewLeadBL.CoA_Country + "," +
                            "'" + NewLeadBL.CoA_PinCode + "','" + NewLeadBL.CoA_LandLine + "','" + NewLeadBL.CoA_Mobile + "','" + NewLeadBL.CoA_Email + "','" + NewLeadBL.CoA_Fax + "'," +
                            "'" + NewLeadBL.CoA_PanNo + "')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Office Address Info
                    sSql = "INSERT INTO LeadOffAddressInfo(LeadId,Address1,Address2,City,State,Country," +
                           "PinCode,LandLine,Mobile,Email,Fax,PanNo)Values(" + iLeadId + ",'" + NewLeadBL.Office_Address1 + "','" + NewLeadBL.Office_Address2 + "','" + NewLeadBL.Office_City + "'," +
                           "'" + NewLeadBL.Office_State + "'," + NewLeadBL.Office_Country + ",'" + NewLeadBL.Office_PinCode + "','" + NewLeadBL.Office_LandLine + "'," +
                           "'" + NewLeadBL.Office_Mobile + "','" + NewLeadBL.Office_Email + "','" + NewLeadBL.Office_Fax + "','" + NewLeadBL.Office_PanNo + "')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //NRI Address Info
                    sSql = "INSERT INTO LeadNRIAddressInfo(LeadId,Address1,Address2,City,State,Country," +
                          "PinCode,LandLine,Mobile,Email,Fax,PanNo,PassportNo)Values(" + iLeadId + ",'" + NewLeadBL.NRI_Address + "','" + NewLeadBL.NRI_Address2 + "','" + NewLeadBL.NRI_City + "'," +
                          "'" + NewLeadBL.NRI_State + "'," + NewLeadBL.NRI_Country + ",'" + NewLeadBL.NRI_PinCode + "','" + NewLeadBL.NRI_LandLine + "'," +
                          "'" + NewLeadBL.NRI_Mobile + "','" + NewLeadBL.NRI_Email + "','" + NewLeadBL.NRI_Fax + "','" + NewLeadBL.NRI_PanNo + "','" + NewLeadBL.NRI_PassportNo + "')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Communication Address Info
                    sSql = "INSERT INTO LeadCommAddressInfo(LeadId,Address1,Address2,Locality,City,State,Country,PinCode,LandLine,Mobile,Email,Fax,PanNo,PassportNo,AddressType)" +
                            "Values(" + iLeadId + ",'" + NewLeadBL.Com_Address1 + "','" + NewLeadBL.Com_Address2 + "','" + NewLeadBL.Com_Locality + "','" + NewLeadBL.Com_City + "','" + NewLeadBL.Com_State + "'," +
                            "" + NewLeadBL.Com_Country + ",'" + NewLeadBL.Com_PinCode + "','" + NewLeadBL.Com_LandLine + "','" + NewLeadBL.Com_Mobile + "'," +
                            "'" + NewLeadBL.Com_Email + "','" + NewLeadBL.Com_Fax + "','" + NewLeadBL.Com_PanNo + "','" + NewLeadBL.Com_PassportNo + "','" + NewLeadBL.Com_AddressType + "')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Requirement
                    sSql = "INSERT INTO LeadRequirement(LeadId,Remarks)Values(" + iLeadId + ",'" + NewLeadBL.Req_Remarks + "')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    for (int i = 0; i < dtF.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dtF.Rows[i]["Sel"]) == true)
                        {
                            sSql = "Insert Into dbo.Facility(FacilityId, Sel, LeadId, Description) Values (" + dtF.Rows[i]["FacilityId"] + ",1," + NewLeadBL.LeadId + ",'" + dtF.Rows[i]["Description"] + "')";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }

                    int iEntryId = 0;
                    foreach (NewLeadBO obj in ProjectSel)
                    {
                        if (NewLeadBL.CallType == "Finalization" || NewLeadBL.CallType == "Block" || NewLeadBL.CallType == "Drop")
                        {
                            sSql = "INSERT INTO dbo.CallSheet (ExecutiveId,LeadId,ProjectId,TrnDate,CallFF,CallTypeID,NatureID,Remarks)VALUES" +
                                " (" + NewLeadBL.Exe_ExecutiveId + "," + iLeadId + "," + obj.ProjectId + ",'" + NewLeadBL.LeadDate.ToString("dd-MMM-yyyy") + "'," +
                                " 'Fresh'," + NewLeadBL.CallTypeId + "," + NewLeadBL.NatureId + "," +
                                " '" + NewLeadBL.Remarks + "')SELECT SCOPE_IDENTITY()";
                            cmd = new SqlCommand(sSql, conn, tran);
                            iEntryId = int.Parse(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();
                        }
                        else
                        {
                            sSql = "INSERT INTO dbo.CallSheet (ExecutiveId,LeadId,ProjectId,TrnDate,CallFF,NextCallDate,CallTypeID,NatureID,Remarks)VALUES" +
                                   " (" + NewLeadBL.Exe_ExecutiveId + "," + iLeadId + "," + obj.ProjectId + ",'" + NewLeadBL.LeadDate.ToString("dd-MMM-yyyy") + "'," +
                                   " 'Fresh',@NextCallDate," + NewLeadBL.CallTypeId + "," + NewLeadBL.NatureId + "," +
                                   " '" + NewLeadBL.Remarks + "')SELECT SCOPE_IDENTITY()";
                            cmd = new SqlCommand(sSql, conn, tran);
                            SqlParameter parameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@NextCallDate" };
                            if (NewLeadBL.NextCallDate == DateTime.MinValue)
                                parameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                            else
                                parameter.Value = NewLeadBL.NextCallDate;
                            cmd.Parameters.Add(parameter);
                            iEntryId = int.Parse(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();
                        }
                    }

                    //Finalization Details
                    if (dtBD != null)
                    {
                        if (dtBD.Rows.Count > 0)
                        {
                            sSql = "Select BusinessType From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre A" +
                                   " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B ON A.ProjectDB=B.ProjectName" +
                                   " Where CostCentreId=" + NewLeadBL.ProjectId + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            dr = cmd.ExecuteReader();
                            DataTable dtBType = new DataTable();
                            dtBType.Load(dr);
                            dr.Close();
                            cmd.Dispose();

                            string sBusinessType = "";
                            if (dtBType.Rows.Count > 0)
                            {
                                sBusinessType = CommFun.IsNullCheck(dtBType.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                            }

                            if (NewLeadBL.CallType == "Finalization" || NewLeadBL.CallType == "Finalisation" || NewLeadBL.CallType == "Block")
                            {
                                if (sBusinessType == "L")
                                {
                                    sSql = "INSERT INTO dbo.BuyerDetail(AllotmentNo,CCAllotNo,COAllotNo,CostCentreId,ExecutiveId,LeadId,EntryId,PlotId,Status,CustomerType," +
                                            " PaymentOption,BranchId,LoanPer,LoanAccNo,BrokerId,BrokerComm,BrokerAmount,ValidUpto,FinaliseDate,PostSaleExecId,Advance,PaySchId) " +
                                            " Values('" + dtBD.Rows[0]["GAllotNo"] + "','" + dtBD.Rows[0]["CCAllotNo"] + "','" + dtBD.Rows[0]["COAllotNo"] + "'," + NewLeadBL.ProjectId + "," +
                                            " " + NewLeadBL.Exe_ExecutiveId + "," + iLeadId + "," + iEntryId + "," + dtBD.Rows[0]["PlotId"] + "," +
                                            " '" + dtBD.Rows[0]["Status"] + "','" + dtBD.Rows[0]["CustomerType"] + "','" + dtBD.Rows[0]["PaymentOption"] + "'," + dtBD.Rows[0]["BranchId"] + ", " +
                                            " " + dtBD.Rows[0]["LoanPer"] + ",'" + dtBD.Rows[0]["LoanAccNo"] + "'," + dtBD.Rows[0]["BrokerId"] + "," + dtBD.Rows[0]["ComPer"] + "," +
                                            " " + dtBD.Rows[0]["ComAmount"] + ",@ValidUpto,@FinaliseDate," +
                                            " " + dtBD.Rows[0]["PostSaleExecId"] + "," + dtBD.Rows[0]["AdvAmt"] + "," + dtBD.Rows[0]["PaySchId"] + ")";
                                }
                                else
                                {
                                    sSql = "INSERT INTO dbo.BuyerDetail(AllotmentNo,CCAllotNo,COAllotNo,CostCentreId,ExecutiveId,LeadId,EntryId,FlatId,Status,CustomerType," +
                                            " PaymentOption,BranchId,LoanPer,LoanAccNo,BrokerId,BrokerComm,BrokerAmount,ValidUpto,FinaliseDate,PostSaleExecId,Advance,PaySchId) " +
                                            " Values('" + dtBD.Rows[0]["GAllotNo"] + "','" + dtBD.Rows[0]["CCAllotNo"] + "','" + dtBD.Rows[0]["COAllotNo"] + "'," + NewLeadBL.ProjectId + "," +
                                            " " + NewLeadBL.Exe_ExecutiveId + "," + iLeadId + "," + iEntryId + "," + NewLeadBL.FlatId + "," +
                                            " '" + dtBD.Rows[0]["Status"] + "','" + dtBD.Rows[0]["CustomerType"] + "','" + dtBD.Rows[0]["PaymentOption"] + "'," + dtBD.Rows[0]["BranchId"] + ", " +
                                            " " + dtBD.Rows[0]["LoanPer"] + ",'" + dtBD.Rows[0]["LoanAccNo"] + "'," + dtBD.Rows[0]["BrokerId"] + "," + dtBD.Rows[0]["ComPer"] + "," +
                                            " " + dtBD.Rows[0]["ComAmount"] + ",@ValidUpto,@FinaliseDate," +
                                            " " + dtBD.Rows[0]["PostSaleExecId"] + "," + dtBD.Rows[0]["AdvAmt"] + "," + dtBD.Rows[0]["PaySchId"] + ")";
                                }
                                cmd = new SqlCommand(sSql, conn, tran);
                                parameterDOB = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@FinaliseDate" };
                                if (Convert.ToDateTime(CommFun.IsNullCheck(dtBD.Rows[0]["FinaliseDate"], CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                                    parameterDOB.Value = System.Data.SqlTypes.SqlDateTime.Null;
                                else
                                    parameterDOB.Value = Convert.ToDateTime(CommFun.IsNullCheck(dtBD.Rows[0]["FinaliseDate"], CommFun.datatypes.VarTypeDate));
                                cmd.Parameters.Add(parameterDOB);
                                parameterDOB = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@ValidUpto" };
                                if (Convert.ToDateTime(CommFun.IsNullCheck(dtBD.Rows[0]["ValidUpto"], CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                                    parameterDOB.Value = System.Data.SqlTypes.SqlDateTime.Null;
                                else
                                    parameterDOB.Value = Convert.ToDateTime(CommFun.IsNullCheck(dtBD.Rows[0]["ValidUpto"], CommFun.datatypes.VarTypeDate));
                                cmd.Parameters.Add(parameterDOB);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                if (sBusinessType == "L")
                                {
                                    if (NewLeadBL.CallType == "Block")
                                    {
                                        sSql = String.Format("Update ["+BsfGlobal.g_sRateAnalDBName+"].dbo.LandPlotDetails Set Status='B', BuyerId={0},PaymentScheduleId={1} "+
                                                             " WHERE PlotDetailsId={2}", iLeadId,
                                                             Convert.ToInt32(CommFun.IsNullCheck(dtBD.Rows[0]["PaySchId"], CommFun.datatypes.vartypenumeric)), 
                                                             Convert.ToInt32(CommFun.IsNullCheck(dtBD.Rows[0]["PlotId"], CommFun.datatypes.vartypenumeric)));
                                    }
                                    else
                                    {
                                        sSql = "Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails Set Status='S', BuyerId=" + iLeadId + ", " +
                                               " PaymentScheduleId=" + Convert.ToInt32(CommFun.IsNullCheck(dtBD.Rows[0]["PaySchId"], CommFun.datatypes.vartypenumeric)) +
                                               " WHERE PlotDetailsId=" + Convert.ToInt32(CommFun.IsNullCheck(dtBD.Rows[0]["PlotId"], CommFun.datatypes.vartypenumeric));
                                    }
                                }
                                else
                                {
                                    if (NewLeadBL.CallType == "Block")
                                    {
                                        sSql = String.Format("Update dbo.FlatDetails Set Status='B',LeadId={0}, PayTypeId={1} WHERE FlatId={2} AND CostCentreId={3}", iLeadId,
                                                              Convert.ToInt32(CommFun.IsNullCheck(dtBD.Rows[0]["PaySchId"], CommFun.datatypes.vartypenumeric)), NewLeadBL.FlatId,
                                                              NewLeadBL.ProjectId);
                                    }
                                    else
                                    {
                                        sSql = "Update dbo.FlatDetails Set Status='S',LeadId=" + iLeadId + ", " +
                                               " PayTypeId=" + Convert.ToInt32(CommFun.IsNullCheck(dtBD.Rows[0]["PaySchId"], CommFun.datatypes.vartypenumeric)) +
                                               " WHERE FlatId=" + NewLeadBL.FlatId + " AND CostCentreId=" + NewLeadBL.ProjectId + "";
                                    }
                                }
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }

                            if (NewLeadBL.CallType == "Block")
                            {
                                if (sBusinessType == "L")
                                {
                                    sSql = "INSERT INTO dbo.BlockUnits(CostCentreId,PlotId,CustomerType,LeadId,BlockUpto,Date,BlockType) " +
                                           " Values(" + NewLeadBL.ProjectId + "," + dtBD.Rows[0]["PlotId"] + ",'" + dtBD.Rows[0]["CustomerType"] + "'," + iLeadId + ",@ValidUpto," +
                                           " '" + Convert.ToDateTime(DateTime.Now).ToString("MM-dd-yyyy") + "','B')";
                                }
                                else
                                {
                                    sSql = "INSERT INTO dbo.BlockUnits(CostCentreId,FlatId,CustomerType,LeadId,BlockUpto,Date,BlockType) " +
                                           " Values(" + NewLeadBL.ProjectId + "," + NewLeadBL.FlatId + ",'" + dtBD.Rows[0]["CustomerType"] + "'," + iLeadId + ",@ValidUpto," +
                                           " '" + Convert.ToDateTime(DateTime.Now).ToString("MM-dd-yyyy") + "','B')";
                                }
                                cmd = new SqlCommand(sSql, conn, tran);
                                parameterDOB = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@ValidUpto" };
                                if (Convert.ToDateTime(CommFun.IsNullCheck(dtBD.Rows[0]["ValidUpto"], CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                                    parameterDOB.Value = System.Data.SqlTypes.SqlDateTime.Null;
                                else
                                    parameterDOB.Value = Convert.ToDateTime(CommFun.IsNullCheck(dtBD.Rows[0]["ValidUpto"], CommFun.datatypes.VarTypeDate));
                                cmd.Parameters.Add(parameterDOB);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }

                            sSql = "INSERT INTO dbo.LeadFlatInfo(LeadId,Buyer,FlatId,CostCentreId)VALUES(" + iLeadId + ",1," + NewLeadBL.FlatId + "," + NewLeadBL.ProjectId + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            sSql = "SELECT UserName FROM dbo.UserLogin Where UserName='" + NewLeadBL.BuyerName + "'";
                            cmd = new SqlCommand(sSql, conn, tran);
                            DataTable dt = new DataTable();
                            SqlDataReader sdr = cmd.ExecuteReader();
                            dt.Load(sdr);
                            dr.Close();
                            cmd.Dispose();

                            if (dt.Rows.Count > 0) { NewLeadBL.LeadName = NewLeadBL.LeadName + '1'; }

                            sSql = "INSERT INTO dbo.UserLogin(UserName,Password,Email,LeadId) VALUES" +
                                " ('" + NewLeadBL.LeadName + "','" + NewLeadBL.LeadName + "','" + NewLeadBL.Email + "'," + iLeadId + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            DateTime DFinDate = Convert.ToDateTime(CommFun.IsNullCheck(dtBD.Rows[0]["FinaliseDate"], CommFun.datatypes.VarTypeDate));
                            if (DFinDate != DateTime.MinValue)
                            {
                                FillSchDate(NewLeadBL.FlatId, DFinDate, conn, tran);
                            }

                            sSql = "Select Rate,AdvAmount From dbo.FlatDetails Where FlatId=" + NewLeadBL.FlatId + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            dr = cmd.ExecuteReader();
                            DataTable dtAdv = new DataTable();
                            dtAdv.Load(dr);
                            dr.Close();
                            cmd.Dispose();

                            decimal dAdvAmt = 0;
                            decimal dRate = 0;
                            if (dtAdv.Rows.Count > 0)
                            {
                                dAdvAmt = Convert.ToDecimal(dtAdv.Rows[0]["AdvAmount"]);
                                dRate = Convert.ToDecimal(dtAdv.Rows[0]["Rate"]);
                                if (dAdvAmt != Convert.ToDecimal(dtBD.Rows[0]["AdvAmt"]))
                                {
                                    sSql = "Update dbo.FlatDetails Set AdvAmount=" + dtBD.Rows[0]["AdvAmt"] + " WHERE FlatId=" + NewLeadBL.FlatId + " ";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();

                                    //PaymentScheduleDL.InsertFlatScheduleI(oLeedBO.FlatID, conn, tran);
                                }
                            }

                            InsertChangeRate(NewLeadBL.FlatId, dRate, Convert.ToDecimal(dtBD.Rows[0]["Rate"]), conn, tran);

                            if (dtChk != null) { UnitDirDL.InsertFlatChk(dtChk, NewLeadBL.FlatId, "F", NewLeadBL.bChkSend, NewLeadBL.FlatNo, NewLeadBL.ProjectId, conn, tran); }

                            if (NewLeadBL.CallType == "Finalization" || NewLeadBL.CallType == "Finalisation")
                                PaymentScheduleDL.InsertFinalFlatScheduleI(NewLeadBL.FlatId, "S", conn, tran);
                            else if (NewLeadBL.CallType == "Block")
                                PaymentScheduleDL.InsertFinalFlatScheduleI(NewLeadBL.FlatId, "B", conn, tran);
                            else
                                PaymentScheduleDL.InsertFlatScheduleI(NewLeadBL.FlatId, conn, tran);
                        }
                    }
                }

                else
                {
                    bUpdate = true;
                    //Lead Info
                    if (NewLeadBL.CallType == "Finalization" || NewLeadBL.CallType == "Drop" || NewLeadBL.CallType == "Block")
                    {
                        sSql = "UPDATE LeadRegister Set LeadName='" + NewLeadBL.LeadName + "',LeadDate='" + NewLeadBL.LeadDate.ToString("dd-MMM-yyyy") + "'," +
                                "LeadType='" + NewLeadBL.LeadType + "',CallTypeId=" + NewLeadBL.CallTypeId + ",NatureId=" + NewLeadBL.NatureId + "," +
                                "TypeId=" + NewLeadBL.UnitType + "," +
                                "UserId=" + NewLeadBL.UserId + ",VIP=" + NewLeadBL.VIP + ",CostId=" + NewLeadBL.CostPreference + "," +
                                "Mobile='" + NewLeadBL.Mobile + "',Email='" + NewLeadBL.Email + "',Remarks='" + NewLeadBL.Remarks + "'," +
                                " MultiProject='" + NewLeadBL.MultiProject + "' WHERE LeadId=" + NewLeadBL.LeadId + "";
                    }
                    else
                    {
                        sSql = "UPDATE LeadRegister Set LeadName='" + NewLeadBL.LeadName + "',LeadDate='" + NewLeadBL.LeadDate.ToString("dd-MMM-yyyy") + "'," +
                                "LeadType='" + NewLeadBL.LeadType + "',CallTypeId=" + NewLeadBL.CallTypeId + ",NatureId=" + NewLeadBL.NatureId + "," +
                                "NextCallDate=@NextCallDate,TypeId=" + NewLeadBL.UnitType + "," +
                                "UserId=" + NewLeadBL.UserId + ",VIP=" + NewLeadBL.VIP + ",CostId=" + NewLeadBL.CostPreference + "," +
                                "Mobile='" + NewLeadBL.Mobile + "',Email='" + NewLeadBL.Email + "',Remarks='" + NewLeadBL.Remarks + "'," +
                                " MultiProject='" + NewLeadBL.MultiProject + "' WHERE LeadId=" + NewLeadBL.LeadId + "";
                    }
                    cmd = new SqlCommand(sSql, conn, tran);
                    SqlParameter parameterDate = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@NextCallDate" };
                    if (NewLeadBL.NextCallDate == DateTime.MinValue)
                        parameterDate.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        parameterDate.Value = NewLeadBL.NextCallDate;
                    cmd.Parameters.Add(parameterDate);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Executive And Project Info
                    sSql = "DELETE FROM dbo.LeadProjectInfo WHERE LeadId=" + NewLeadBL.LeadId + " ";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "DELETE FROM dbo.LeadExecutiveInfo WHERE LeadId=" + NewLeadBL.LeadId + " ";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    foreach (NewLeadBO obj in ProjectSel)
                    {
                        int iUserId = Convert.ToInt32(ProjectSel[0].ExecutiveID);
                        if (iUserId == 1) { iUserId = 0; }

                        sSql = "INSERT INTO dbo.LeadProjectInfo (LeadId,CostCentreId,CampaignId,ExecutiveId,NextCallDate)  " +
                               "Values(" + NewLeadBL.LeadId + "," + obj.ProjectId + "," + obj.CampaignId + "," + iUserId + ",@NextCallDate)";
                        cmd = new SqlCommand(sSql, conn, tran);
                        SqlParameter dateparameter = new SqlParameter() { ParameterName = "@NextCallDate", DbType = DbType.DateTime };
                        if (NewLeadBL.NextCallDate == DateTime.MinValue)
                            dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            dateparameter.Value = NewLeadBL.NextCallDate;
                        cmd.Parameters.Add(dateparameter);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "INSERT INTO dbo.LeadExecutiveInfo (LeadId,CostCentreId,ExecutiveId)  " +
                               "Values(" + NewLeadBL.LeadId + "," + obj.ProjectId + "," + iUserId + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    sSql = "Delete From dbo.Area Where LeadId=" + NewLeadBL.LeadId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    for (int i = 0; i < dtA.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dtA.Rows[i]["Sel"]) == true)
                        {
                            sSql = "Insert Into dbo.Area(AreaId, Sel, LeadId, Description) Values (" + dtA.Rows[i]["AreaId"] + ",1," + NewLeadBL.LeadId + ",'" + dtA.Rows[i]["Description"] + "')";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }

                    //Personal Info
                    sSql = "UPDATE LeadPersonalInfo Set Gender='" + NewLeadBL.Pe_Gender + "',DOB=@DOB," +
                          "Religion=" + NewLeadBL.Pe_Religion + ",NationalityId=" + NewLeadBL.Pe_Nationality + ",ProfessionId=" + NewLeadBL.Pe_Profession +
                          ",Organization='" + NewLeadBL.Pe_Organization + "'," +
                          "FatherName='" + NewLeadBL.Pe_FatherName + "',MotherName='" + NewLeadBL.Pe_MotherName + "',MarritalStatus='" + NewLeadBL.Pe_MarritalStatus + "'," +
                          "WifeName='" + NewLeadBL.Pe_WifeName + "',WeddingDate=@WeddingDate," +
                          "NRI=" + NewLeadBL.Pe_NRI + " Where LeadId=" + NewLeadBL.LeadId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    parameterDate = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@DOB" };
                    if (NewLeadBL.Pe_DOB == DateTime.MinValue)
                        parameterDate.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        parameterDate.Value = NewLeadBL.Pe_DOB;
                    cmd.Parameters.Add(parameterDate);
                    parameterDate = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@WeddingDate" };
                    if (NewLeadBL.Pe_WeddingDate == DateTime.MinValue)
                        parameterDate.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        parameterDate.Value = NewLeadBL.Pe_WeddingDate;
                    cmd.Parameters.Add(parameterDate);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //NRI Info
                    sSql = "UPDATE LeadNRIContactDet Set PersonName='" + NewLeadBL.Pe_ContactPerson + "',Address='" + NewLeadBL.Pe_ContactAdd + "',CityId=" + NewLeadBL.Pe_ContactCity + "," +
                            "StateId=" + NewLeadBL.Pe_ContactState + ",MobileNo='" + NewLeadBL.Pe_ContactMobileNo + "',Email='" + NewLeadBL.Pe_ContactMailId + "' WHERE LeadId=" + NewLeadBL.LeadId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //POA Info
                    sSql = "UPDATE LeadPOAInfo Set  ApplicantName='" + NewLeadBL.PoA_ApplicantName + "'," +
                            "Gender='" + NewLeadBL.PoA_Gender + "',DOB=@DOB,Religion=" + NewLeadBL.PoA_Religion + ",NationalityId=" + NewLeadBL.PoA_Nationality + "," +
                            "FatherName='" + NewLeadBL.PoA_FatherName + "',MotherName='" + NewLeadBL.PoA_MotherName + "',WifeName='" + NewLeadBL.PoA_WifeName +
                            "',WeddingDate=@WeddingDate," +
                            "ProfessionId=" + NewLeadBL.PoA_Profession + ",Organization='" + NewLeadBL.PoA_Organization + "'," +
                            "MarritalStatus=" + NewLeadBL.PoA_MarritalStatus + ",Relation='" + NewLeadBL.PoA_Relation + "' WHERE LeadId=" + NewLeadBL.LeadId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    parameterDate = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@DOB" };
                    if (NewLeadBL.PoA_DOB == DateTime.MinValue)
                        parameterDate.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        parameterDate.Value = NewLeadBL.PoA_DOB;
                    cmd.Parameters.Add(parameterDate);
                    parameterDate = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@WeddingDate" };
                    if (NewLeadBL.PoA_WeddingDate == DateTime.MinValue)
                        parameterDate.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        parameterDate.Value = NewLeadBL.PoA_WeddingDate;
                    cmd.Parameters.Add(parameterDate);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Co-Applicant Info
                    sSql = "UPDATE LeadCoApplicantInfo Set CoApplicantName='" + NewLeadBL.CoApp_CoApplicantName + "',Gender='" + NewLeadBL.CoApp_Gender + "',DOB=@DOB," +
                       "Religion=" + NewLeadBL.CoApp_Religion + ",NationalityId=" + NewLeadBL.CoApp_Nationality + ",FatherName='" + NewLeadBL.CoApp_FatherName + "',MotherName='" + NewLeadBL.CoApp_MotherName + "'," +
                       "WifeName='" + NewLeadBL.CoApp_WifeName + "',WeddingDate=@WeddingDate,ProfessionId=" + NewLeadBL.CoApp_Profession + "," +
                       "Organization='" + NewLeadBL.CoApp_Organization + "',MarritalStatus=" + NewLeadBL.CoApp_MarritalStatus + "  WHERE LeadId=" + NewLeadBL.LeadId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    parameterDate = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@DOB" };
                    if (NewLeadBL.CoApp_DOB == DateTime.MinValue)
                        parameterDate.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        parameterDate.Value = NewLeadBL.CoApp_DOB;
                    cmd.Parameters.Add(parameterDate);
                    parameterDate = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@WeddingDate" };
                    if (NewLeadBL.CoApp_WeddingDate == DateTime.MinValue)
                        parameterDate.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        parameterDate.Value = NewLeadBL.CoApp_WeddingDate;
                    cmd.Parameters.Add(parameterDate);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Child Info
                    sSql = "UPDATE LeadChildInfo Set ChildName='" + NewLeadBL.C_ChildName + "',Sex='" + NewLeadBL.C_Sex + "'," +
                          "DOB=@DOB WHERE LeadId=" + NewLeadBL.LeadId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    parameterDate = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@DOB" };
                    if (NewLeadBL.C_DOB == DateTime.MinValue)
                        parameterDate.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        parameterDate.Value = NewLeadBL.C_DOB;
                    cmd.Parameters.Add(parameterDate);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Finance Info
                    sSql = "UPDATE dbo.LeadFinance Set Employment=" + NewLeadBL.F_Employment + "," +
                           "Income=" + NewLeadBL.F_Income + ",Apartment=" + NewLeadBL.F_Appatment + ",ApartmentSize=" + NewLeadBL.F_AppatmentSize + ",Stay=" + NewLeadBL.F_Stay + ",GuestHouse=" + NewLeadBL.F_GuestHouse + "," +
                           "Broker=" + NewLeadBL.F_Broker + "," +
                           "Comission=" + NewLeadBL.F_Comission + ",ContactPerson='" + NewLeadBL.F_ContactPerson + "' WHERE LeadId=" + NewLeadBL.LeadId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Delete From dbo.Possess Where LeadId=" + NewLeadBL.LeadId + " ";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    for (int i = 0; i < dtP.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dtP.Rows[i]["Sel"]) == true)
                        {
                            sSql = "Insert Into dbo.Possess(PossessId, Sel, LeadId, Description) Values (" + dtP.Rows[i]["PossessId"] + ",1," + NewLeadBL.LeadId + ",'" + dtP.Rows[i]["Description"] + "')";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }

                    //Bank Details
                    sSql = "UPDATE LeadBankDetails Set ContactPerson='" + NewLeadBL.B_ContactPerson + "',ContactMobileNo='" + NewLeadBL.B_ContactMobileNo + "',BankName='" + NewLeadBL.B_BankName + "'," +
                            "LoanNo='" + NewLeadBL.B_LoanNo + "',Branch='" + NewLeadBL.B_Branch + "',InterestRate=" + NewLeadBL.B_InterestRate + "," +
                            "LoanAmount=" + NewLeadBL.B_LoanAmount + ",Need='" + NewLeadBL.B_Need + "' WHERE LeadId=" + NewLeadBL.LeadId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //POA Address Info
                    sSql = "UPDATE LeadPOAAddressInfo Set Address1= '" + NewLeadBL.PoAA_Address1 + "',Address2='" + NewLeadBL.PoAA_Address2 + "'," +
                          "CityId=" + NewLeadBL.PoAA_City + ",StateId=" + NewLeadBL.PoAA_State + ",Country=" + NewLeadBL.PoAA_Country + ",PinCode='" + NewLeadBL.PoAA_PinCode + "'," +
                          "LandLine='" + NewLeadBL.PoAA_LandLine + "',Mobile='" + NewLeadBL.PoAA_Mobile + "',Email='" + NewLeadBL.PoAA_Email + "'," +
                          "Fax='" + NewLeadBL.PoAA_Fax + "',PanNo='" + NewLeadBL.PoAA_PanNo + "' WHERE LeadId=" + NewLeadBL.LeadId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Permanent Address Info
                    sSql = "UPDATE LeadPermanentAddressInfo Set Address1='" + NewLeadBL.P_Address1 + "',Address2='" + NewLeadBL.P_Address2 + "',Locality='" + NewLeadBL.P_Locality + "'," +
                           "CityId=" + NewLeadBL.P_City + ",StateId=" + NewLeadBL.P_State + ",Country=" + NewLeadBL.P_Country + ",PinCode='" + NewLeadBL.P_PinCode + "'," +
                           "LandLine='" + NewLeadBL.P_LandLine + "',Mobile='" + NewLeadBL.P_Mobile + "'," +
                           "Email='" + NewLeadBL.P_Email + "',Fax='" + NewLeadBL.P_Fax + "',PanNo='" + NewLeadBL.P_PanNo + "' WHERE LeadId=" + NewLeadBL.LeadId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Co-Applicant Address Info
                    sSql = "UPDATE LeadCoAppAddressInfo Set CoAddress1='" + NewLeadBL.CoA_Address1 + "'," +
                            "CoAddress2='" + NewLeadBL.CoA_Address2 + "',CityId=" + NewLeadBL.CoA_City + ",StateId=" + NewLeadBL.CoA_State + ",CoCountry=" + NewLeadBL.CoA_Country + "," +
                            "CoPinCode='" + NewLeadBL.CoA_PinCode + "',CoLandLine='" + NewLeadBL.CoA_LandLine + "',CoMobile='" + NewLeadBL.CoA_Mobile + "',CoEmail='" + NewLeadBL.CoA_Email + "',CoFax='" + NewLeadBL.CoA_Fax + "'," +
                            "CoPanNo='" + NewLeadBL.CoA_PanNo + "' WHERE LeadId=" + NewLeadBL.LeadId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Office Address  Info
                    sSql = "UPDATE LeadOffAddressInfo Set Address1='" + NewLeadBL.Office_Address1 + "',Address2='" + NewLeadBL.Office_Address2 + "',CityId=" + NewLeadBL.Office_City + "," +
                           "StateId=" + NewLeadBL.Office_State + ",Country=" + NewLeadBL.Office_Country + ",PinCode='" + NewLeadBL.Office_PinCode + "',LandLine='" + NewLeadBL.Office_LandLine + "'," +
                           "Mobile='" + NewLeadBL.Office_Mobile + "',Email='" + NewLeadBL.Office_Email + "',Fax='" + NewLeadBL.Office_Fax + "',PanNo='" + NewLeadBL.Office_PanNo + "' WHERE LeadId=" + NewLeadBL.LeadId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //NRI Address  Info
                    sSql = "UPDATE LeadNRIAddressInfo Set Address1='" + NewLeadBL.NRI_Address + "',Address2='" + NewLeadBL.NRI_Address2 + "',CityId=" + NewLeadBL.NRI_City + "," +
                           "StateId=" + NewLeadBL.NRI_State + ",Country=" + NewLeadBL.NRI_Country + ",PinCode='" + NewLeadBL.NRI_PinCode + "',LandLine='" + NewLeadBL.NRI_LandLine + "'," +
                           "Mobile='" + NewLeadBL.NRI_Mobile + "',Email='" + NewLeadBL.NRI_Email + "',Fax='" + NewLeadBL.NRI_Fax + "',PanNo='" + NewLeadBL.NRI_PanNo + "'," +
                           " PassportNo='" + NewLeadBL.NRI_PassportNo + "' WHERE LeadId=" + NewLeadBL.LeadId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Communication Address Info
                    sSql = "UPDATE LeadCommAddressInfo Set Address1='" + NewLeadBL.Com_Address1 + "',Address2='" + NewLeadBL.Com_Address2 + "',Locality='" + NewLeadBL.Com_Locality + "',CityId=" + NewLeadBL.Com_City + ",StateId=" + NewLeadBL.Com_State + "," +
                            "Country=" + NewLeadBL.Com_Country + ",PinCode='" + NewLeadBL.Com_PinCode + "',LandLine='" + NewLeadBL.Com_LandLine + "',Mobile='" + NewLeadBL.Com_Mobile + "'," +
                            "Email='" + NewLeadBL.Com_Email + "',Fax='" + NewLeadBL.Com_Fax + "',PanNo='" + NewLeadBL.Com_PanNo + "',PassportNo='" + NewLeadBL.Com_PassportNo +
                            "', AddressType='" + NewLeadBL.Com_AddressType + "' WHERE LeadId=" + NewLeadBL.LeadId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Requirement
                    sSql = "UPDATE LeadRequirement Set " +
                            "Remarks='" + NewLeadBL.Req_Remarks + "' WHERE LeadId=" + NewLeadBL.LeadId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "DELETE FROM dbo.Facility WHERE LeadId=" + NewLeadBL.LeadId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    for (int i = 0; i < dtF.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dtF.Rows[i]["Sel"]) == true)
                        {
                            sSql = "Insert Into dbo.Facility(FacilityId, Sel, LeadId, Description) Values (" + dtF.Rows[i]["FacilityId"] + ",1," + NewLeadBL.LeadId + ",'" + dtF.Rows[i]["Description"] + "')";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }

                }

                tran.Commit();

                if (NewLeadBL.CallType == "Finalization" || NewLeadBL.CallType == "Finalisation" || NewLeadBL.CallType == "Block")
                {
                    if (dtBD.Rows.Count > 0)
                    {
                        if (NewLeadBL.BusinessType == "B")
                        {
                            if (NewLeadBL.LeadId != 0)
                            {
                                BsfGlobal.InsertLog(DateTime.Now, "Flat-Finalisation-Modify", "N", "Flat Finalisation", NewLeadBL.FlatId, NewLeadBL.ProjectId, 0, BsfGlobal.g_sCRMDBName, NewLeadBL.FlatNo, BsfGlobal.g_lUserId, Convert.ToDecimal(CommFun.IsNullCheck(dtBD.Rows[0]["AdvAmt"], CommFun.datatypes.vartypenumeric)), 0);
                            }
                            else
                            {
                                BsfGlobal.InsertLog(DateTime.Now, "Flat-Finalisation-Create", "C", "Flat Finalisation", NewLeadBL.FlatId, NewLeadBL.ProjectId, 0, BsfGlobal.g_sCRMDBName, NewLeadBL.FlatNo, BsfGlobal.g_lUserId, Convert.ToDecimal(CommFun.IsNullCheck(dtBD.Rows[0]["AdvAmt"], CommFun.datatypes.vartypenumeric)), 0);
                            }
                        }
                        else
                        {
                            if (NewLeadBL.LeadId != 0)
                            {
                                BsfGlobal.InsertLog(DateTime.Now, "Flat-Finalisation-Modify", "N", "Flat Finalisation", Convert.ToInt32(dtBD.Rows[0]["PlotId"]), NewLeadBL.ProjectId, 0, BsfGlobal.g_sCRMDBName, NewLeadBL.PlotNo, BsfGlobal.g_lUserId, Convert.ToDecimal(CommFun.IsNullCheck(dtBD.Rows[0]["AdvAmt"], CommFun.datatypes.vartypenumeric)), 0);
                            }
                            else
                            {
                                BsfGlobal.InsertLog(DateTime.Now, "Flat-Finalisation-Create", "C", "Flat Finalisation", Convert.ToInt32(dtBD.Rows[0]["PlotId"]), NewLeadBL.ProjectId, 0, BsfGlobal.g_sCRMDBName, NewLeadBL.PlotNo, BsfGlobal.g_lUserId, Convert.ToDecimal(CommFun.IsNullCheck(dtBD.Rows[0]["AdvAmt"], CommFun.datatypes.vartypenumeric)), 0);
                            }
                        }
                    }
                }

                if (NewLeadBL.LeadId == 0)
                {
                    string sStatus = "";
                    if (NewLeadBL.MultiProject == false) { sStatus = "One"; } else { sStatus = "Multi"; }
                    LeadBL.Insert_BuyerAlert(iLeadId, NewLeadBL.ProjectName, sStatus);
                }

                if (BsfGlobal.g_bFADB == true) { BsfGlobal.RefreshSubLedger(3); }
            }
            catch (Exception e)
            {
                tran.Rollback();
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }
            return bUpdate;
        }

        public static void FillSchDate(int argFlatId, DateTime dateFinal, SqlConnection conn, SqlTransaction tran)
        {
            string sSql = "";
            SqlCommand cmd;
            SqlDataReader sdr2, sdr1; DataTable dt1; int iStgId = 0, iTemplateId = 0;
            int iDateAfter = 0, iDuration = 0; string sDurType = ""; DateTime SchDate;

            sSql = "Select TemplateId,PreStageTypeId from dbo.PaymentScheduleFlat Where FlatId=" + argFlatId + " And PreStageTypeId=-1";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr2 = cmd.ExecuteReader();
            dt1 = new DataTable();
            dt1.Load(sdr2); cmd.Dispose();


            if (dt1.Rows.Count > 0)
            {
                iStgId = Convert.ToInt32(dt1.Rows[0]["PreStageTypeId"]);
                iTemplateId = Convert.ToInt32(dt1.Rows[0]["TemplateId"]);
            }
            dt1.Dispose();

            if (iStgId == -1)
            {
                sSql = "Select A.TemplateId,A.DateAfter,A.Duration,A.Durationtype from dbo.PaymentScheduleFlat A" +
                    " Left Join dbo.ProgressBillRegister B On A.FlatId=B.FlatId " +
                    " Where A.FlatId=" + argFlatId + " And A.TemplateId>=" + iTemplateId + "" +
                    " And A.PaymentSchId Not In " +
                    " (Select PaySchId From dbo.ProgressBillRegister Where FlatId=" + argFlatId + ")";
                cmd = new SqlCommand(sSql, conn, tran);
                sdr1 = cmd.ExecuteReader();
                dt1 = new DataTable();
                dt1.Load(sdr1);
                cmd.Dispose();

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    iTemplateId = Convert.ToInt32(dt1.Rows[i]["TemplateId"]);
                    iDateAfter = Convert.ToInt32(dt1.Rows[i]["DateAfter"]);
                    iDuration = Convert.ToInt32(dt1.Rows[i]["Duration"]);
                    sDurType = dt1.Rows[i]["DurationType"].ToString();
                    SchDate = Convert.ToDateTime(dateFinal.ToString("dd-MMM-yyyy"));

                    if (sDurType == "D")
                    { if (iDateAfter == 0) SchDate = SchDate.AddDays(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }
                    else if (sDurType == "M")
                    { if (iDateAfter == 0) SchDate = SchDate.AddMonths(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }


                    sSql = "Update dbo.PaymentScheduleFlat Set SchDate='" + Convert.ToDateTime(SchDate).ToString("dd-MMM-yyyy") + "'" +
                        " Where TemplateId=" + iTemplateId + " And FlatId=" + argFlatId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

            }
        }

        public static bool Check_Lead_Name(string arg_sLeadName, int arg_iLeadId)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt;
            bool bExist = false;
            try
            {
                dt = new DataTable();
                string sSql = String.Format("SELECT LeadId FROM dbo.LeadRegister WHERE LeadName='{0}' AND LeadId<> {1}", arg_sLeadName, arg_iLeadId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                sda.Fill(dt);
                if (dt.Rows.Count == 0)
                    bExist = false;
                else
                    bExist = true;

            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "Lead", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information); ;
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }

            return bExist;

        }

        internal static DataSet Get_LeadDet(int arg_iLeadId)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda = null;
            DataSet ds = null;

            try
            {
                ds = new DataSet();
                string sSql = String.Format("SELECT * FROM dbo.LeadRegister WHERE   LeadId= {0}", arg_iLeadId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "LeadInfo");
                sda.Dispose();

                sSql = String.Format("SELECT * FROM dbo.LeadExecutiveInfo WHERE   LeadId= {0}", arg_iLeadId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Executive");
                sda.Dispose();

                sSql = string.Format("SELECT * From dbo.LeadProjectInfo Where LeadId={0}", arg_iLeadId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "LeadProjectInfo");
                sda.Dispose();

                sSql = string.Format("SELECT * From dbo.Area Where LeadId={0}", arg_iLeadId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "LeadArea");
                sda.Dispose();

                sSql = String.Format("SELECT * FROM dbo.LeadPersonalInfo WHERE   LeadId= {0}", arg_iLeadId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Personal");
                sda.Dispose();

                sSql = String.Format("SELECT * FROM dbo.LeadPOAAddressInfo WHERE   LeadId= {0}", arg_iLeadId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "PowerOfAtterneyAddress");
                sda.Dispose();

                sSql = String.Format("SELECT * FROM dbo.LeadCoApplicantInfo WHERE   LeadId= {0}", arg_iLeadId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Co-Applicant");
                sda.Dispose();

                sSql = String.Format("SELECT * FROM dbo.LeadChildInfo WHERE   LeadId= {0}", arg_iLeadId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Child");
                sda.Dispose();


                sSql = String.Format("SELECT * FROM dbo.LeadFinance WHERE   LeadId= {0}", arg_iLeadId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Financial");
                sda.Dispose();


                sSql = String.Format("SELECT * FROM dbo.LeadBankDetails WHERE   LeadId= {0}", arg_iLeadId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Bank Details");
                sda.Dispose();

                sSql = String.Format("SELECT * FROM dbo.LeadPermanentAddressInfo WHERE   LeadId= {0}", arg_iLeadId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Permanent");
                sda.Dispose();

                sSql = String.Format("SELECT * FROM dbo.LeadCoAppAddressInfo WHERE   LeadId= {0}", arg_iLeadId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Co Applicant Address");
                sda.Dispose();

                sSql = String.Format("SELECT * FROM dbo.LeadPOAInfo WHERE   LeadId= {0}", arg_iLeadId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "PowerOfAtterney");
                sda.Dispose();

                sSql = String.Format("SELECT * FROM dbo.LeadOffAddressInfo WHERE   LeadId= {0}", arg_iLeadId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Office");
                sda.Dispose();

                sSql = String.Format("SELECT * FROM dbo.LeadNRIAddressInfo WHERE LeadId= {0}", arg_iLeadId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "NRI");
                sda.Dispose();

                sSql = String.Format("SELECT * FROM dbo.LeadNRIContactDet WHERE   LeadId= {0}", arg_iLeadId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "NRIDet");
                sda.Dispose();

                sSql = String.Format("SELECT * FROM dbo.LeadCommAddressInfo WHERE   LeadId= {0}", arg_iLeadId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Communication");
                sda.Dispose();

                sSql = String.Format("SELECT * FROM dbo.LeadRequirement WHERE   LeadId= {0}", arg_iLeadId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Requirement");
                sda.Dispose();

            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "Lead", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information); ;
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }

            return ds;
        }

        internal static bool Delete_LeadDetils(int arg_iLeadId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            string sql;

            bool bSuccess = false;
            SqlCommand cmd;
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    sql = "DELETE FROM LeadRegister WHERE LeadId=" + arg_iLeadId + "";
                    cmd = new SqlCommand(sql, conn, tran);
                    cmd.ExecuteNonQuery();

                    sql = "DELETE FROM dbo.LeadPersonalInfo WHERE LeadId=" + arg_iLeadId + "";
                    cmd = new SqlCommand(sql, conn, tran);
                    cmd.ExecuteNonQuery();

                    sql = "DELETE FROM dbo.LeadPOAAddressInfo WHERE LeadId=" + arg_iLeadId + "";
                    cmd = new SqlCommand(sql, conn, tran);
                    cmd.ExecuteNonQuery();

                    sql = "DELETE FROM dbo.LeadCoApplicantInfo WHERE LeadId=" + arg_iLeadId + "";
                    cmd = new SqlCommand(sql, conn, tran);
                    cmd.ExecuteNonQuery();

                    sql = "DELETE FROM dbo.LeadChildInfo WHERE LeadId=" + arg_iLeadId + "";
                    cmd = new SqlCommand(sql, conn, tran);
                    cmd.ExecuteNonQuery();

                    sql = "DELETE FROM dbo.LeadFinance WHERE LEadId=" + arg_iLeadId + "";
                    cmd = new SqlCommand(sql, conn, tran);
                    cmd.ExecuteNonQuery();

                    sql = "DELETE FROM dbo.LeadBankDetails WHERE LEadId=" + arg_iLeadId + "";
                    cmd = new SqlCommand(sql, conn, tran);
                    cmd.ExecuteNonQuery();

                    sql = "DELETE FROM dbo.LeadPermanentAddressInfo WHERE LEadId=" + arg_iLeadId + "";
                    cmd = new SqlCommand(sql, conn, tran);
                    cmd.ExecuteNonQuery();

                    sql = "DELETE FROM dbo.LeadCoAppAddressInfo WHERE LEadId=" + arg_iLeadId + "";
                    cmd = new SqlCommand(sql, conn, tran);
                    cmd.ExecuteNonQuery();

                    sql = "DELETE FROM dbo.LeadPOAInfo WHERE LEadId=" + arg_iLeadId + "";
                    cmd = new SqlCommand(sql, conn, tran);
                    cmd.ExecuteNonQuery();


                    sql = "DELETE FROM dbo.LeadOffAddressInfo  WHERE LEadId=" + arg_iLeadId + "";
                    cmd = new SqlCommand(sql, conn, tran);
                    cmd.ExecuteNonQuery();

                    sql = "DELETE FROM dbo.LeadNRIContactDet WHERE LEadId=" + arg_iLeadId + "";
                    cmd = new SqlCommand(sql, conn, tran);
                    cmd.ExecuteNonQuery();

                    sql = "DELETE FROM dbo.LeadCommAddressInfo  WHERE LEadId=" + arg_iLeadId + "";
                    cmd = new SqlCommand(sql, conn, tran);
                    cmd.ExecuteNonQuery();

                    sql = "DELETE FROM dbo.LeadRequirement WHERE LEadId=" + arg_iLeadId + "";
                    cmd = new SqlCommand(sql, conn, tran);
                    cmd.ExecuteNonQuery();

                    sql = "DELETE FROM dbo.LeadExecutiveInfo WHERE LEadId=" + arg_iLeadId + "";
                    cmd = new SqlCommand(sql, conn, tran);
                    cmd.ExecuteNonQuery();
                    bSuccess = true;
                    tran.Commit();
                }

                catch (Exception e)
                {
                    tran.Rollback();
                    BsfGlobal.CustomException(e.Message, e.StackTrace);
                }
                finally
                {

                    conn.Close();
                }
                return bSuccess;


            }
        }

        internal static bool FoundLeadDetils(int arg_iLeadId)
        {
            BsfGlobal.OpenCRMDB();
            bool bAns = false;
            try
            {
                String sSql = "Select * From dbo.CallSheet Where LeadId=" + arg_iLeadId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                int i_Count = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                cmd.Dispose();
                if (i_Count > 0) 
                    bAns = true;
                else
                    bAns = false;

                if (bAns == false)
                {
                    sSql = "Select * From dbo.BuyerDetail Where LeadId=" + arg_iLeadId + "";
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    i_Count = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                    cmd.Dispose();
                    if (i_Count > 0)
                        bAns = true;
                    else
                        bAns = false;
                }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return bAns;
        }

        public static void InsertEmailSent(string argEmail, string argMobile, int argLeadId, string argSub)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand cmd;
            try
            {
                ssql = "Insert Into PendingEmail (TransDate,UserId,LeadId,EmailId,PhoneNo,MessageInfo,EmailSent,SentDate) Values " +
                    " ('" + Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy") + "'," + BsfGlobal.g_lUserId + "," + argLeadId + ",'" + argEmail + "','" + argMobile + "'," +
                    " '" + argSub + "',1,'" + Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy") + "') ";
                cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ce)
            {
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
        }

        public static DataTable GetEditRegisterBuyerDet(int argLeadId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "Select A.AllotmentNo GAllotNo,COAllotNo,CCAllotNo, C.CostCentreName ProjName,A.CostCentreId,A.AccountId, " +
                              " A.ExecutiveId,A.LeadId,A.EntryId CallSheetEntryId,A.FlatId,A.PaySchId,A.Status,A.CustomerType,A.PaymentOption," +
                              " A.BranchId,A.LoanPer,A.LoanAppDate,A.LoanAccNo,A.BrokerId,A.BrokerComm ComPer,A.BrokerAmount ComAmount,A.ValidUpto," +
                              " A.FinaliseDate,A.PostSaleExecId,A.Advance AdvAmt,A.Approve,A.AdvRefId,A.PlotId,A.RegDate," +
                              " Case When A.FlatId=0 Then ISNULL(D.Rate,0) Else ISNULL(B.Rate,0) End Rate, " +
                              " Case When A.FlatId=0 Then 0 Else ISNULL(B.FlatTypeId,0) End FlatTypeId, "+
                              " Case When A.FlatId=0 Then ISNULL(D.PlotTypeId,0) Else 0 End PlotTypeId From dbo.BuyerDetail A " +
                              " LEFT Join dbo.FlatDetails B On A.FlatId=B.FlatId And A.LeadId=B.LeadId " +
                              " LEFT Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails D On A.PlotId=D.PlotDetailsId AND A.LeadId=D.BuyerId " +
                              " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C ON A.CostCentreId=C.CostCentreId " +
                              " Where A.LeadId=" + argLeadId + "";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        public static DataTable GetGridLeadReg(int argLeadId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql = "";

            sSql = " Select Distinct R.LeadId, R.LeadName,R.LeadDate,A.CostCentreId,R.Mobile,R.Email,IsNull(O.CostCentreName,'')CostCentre,IsNull(U.EmployeeName,'') " +
                 " ExecutiveName From dbo.LeadRegister R Left Join dbo.LeadProjectInfo A On A.LeadId=R.LeadId " +
                 " Inner Join dbo.LeadExecutiveInfo E On A.LeadId=E.LeadId  " +
                 " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O On O.CostCentreId=A.CostCentreId  " +
                 " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users U On U.UserId=A.ExecutiveId  " +
                 " Where R.LeadId=" + argLeadId + "";
            try
            {
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

        public static DataTable GetLoanStatus()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql = "";

            sSql = "SELECT C.LeadName,B.FinaliseDate,Case When D.EmployeeName='' Then D.UserName Else D.EmployeeName End as ExecutiveName,ISNULL(G.VendorName,'') BrokerName, " +
                    " A.FlatNo,A.NetAmt+A.QualifierAmt FlatCost,(A.NetAmt+A.QualifierAmt)-B.LoanPer OwnFund,B.LoanPer BankLoan,F.BankName,'VMFS' HomeLoan " +
                    " FROM dbo.FlatDetails A INNER JOIN dbo.BuyerDetail B ON A.FlatId=B.FlatId " +
                    " INNER JOIN dbo.LeadRegister C ON C.LeadId=A.LeadId INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users D ON D.UserId=B.ExecutiveId " +
                    " INNER JOIN dbo.BankDetails E ON E.BranchId=B.BranchId INNER JOIN dbo.BankMaster F ON F.BankId=E.BankId " +
                    " LEFT JOIN [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorMaster G On B.BrokerId=G.VendorId " +
                    " Where B.PaymentOption='L' And A.Status='S'";
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

        #endregion

        #region Email

        public static DataTable GetOpCostCentre()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select X.CostCentreId,X.CostCentreName From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre X" +
                        " Where ProjectDB In(Select ProjectName From " +
                        " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType in('B','L'))" +
                        " and CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                        " Where UserId=" + BsfGlobal.g_lUserId + ") Order By X.CostCentreName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable GetEmailExecutive()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.UserId ExecutiveId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As ExecutiveName " +
                       " From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                       "Inner join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId " +
                       "Where B.PositionType='M' ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable GetEmailBuyers(string argBuyer, int argExecId, int argCCId, string argDateType, DateTime argFrom, DateTime argTo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = ""; string sCond = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                //Filter Buyer type
                if (argBuyer == "All") { }
                else if (argBuyer == "ProspectiveBuyer") { sCond = "Where A.LeadId Not In (Select LeadId From dbo.BuyerDetail)"; }
                else if (argBuyer == "Buyer") { sCond = "Where A.LeadId In (Select LeadId From dbo.BuyerDetail)"; }

                //Filter Executive
                if (sCond == "") 
                {
                    if (argExecId == 0) { } else if (argExecId != 0) { sCond = sCond + "Where B.ExecutiveId=" + argExecId + ""; } 
                }
                else
                {
                    if (argExecId == 0) { } else if (argExecId != 0) { sCond = sCond + "And B.ExecutiveId=" + argExecId + ""; }
                }

                //Filter CostCentre
                if (sCond == "") 
                {
                    if (argCCId == 0) { } else if (argCCId != 0) { sCond = sCond + "Where P.CostCentreId=" + argCCId + ""; } 
                }
                else
                {
                    if (argCCId == 0) { } else if (argCCId != 0) { sCond = sCond + "And P.CostCentreId=" + argCCId + ""; }
                }

                //Filter DateType
                if (sCond == "")
                {
                    if (argDateType == "All") { } else { sCond = sCond + "Where A.LeadDate Between '" + argFrom.ToString("dd-MMM-yyyy") + "' And '" + argTo.ToString("dd-MMM-yyyy") + "'"; }
                }
                else
                {
                    if (argDateType == "All") { } else { sCond = sCond + " And A.LeadDate Between '" + argFrom.ToString("dd-MMM-yyyy") + "' And '" + argTo.ToString("dd-MMM-yyyy") + "'"; }
                }

                //if (sCond == "") { } else { sCond = "Where" + sCond; }

                sSql = "  Select Distinct A.LeadId,B.ExecutiveId,P.CostCentreId,A.LeadName,A.Email,A.Mobile, Cast(0 as bit) [To],Cast(0 as bit) CC," +
                        " Cast(0 as bit) BCC From dbo.LeadRegister A Inner Join LeadExecutiveInfo B On A.LeadId=B.LeadId" +
                        " Inner Join dbo.LeadProjectInfo P On P.LeadId=B.LeadId " + sCond + "" +
                        " Order By LeadName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                dt.Dispose();
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        #endregion
    }
}