using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using CRM.DataLayer;

namespace CRM
{
    class LeadDL
    {        
        #region Methods
        
        public static DataTable GetCat()
        {
            DataTable dt = null;
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
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable GetExecutiveList()
        {
            DataTable dt = null;
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
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable FillProjType()
        {
            DataTable dtRec = null;
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
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtRec;
        }

        public static DataTable GetCallType()
        {
            DataTable dt = null;
            SqlDataAdapter sda; 
            BsfGlobal.OpenCRMDB();
            String sSql = "SELECT CallTypeID,Description,SortId FROM dbo.CallType ORDER BY SortID ";
            try
            {
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        //NewForm

        public static DataTable GetReligion()
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtReligion = null;
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
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtReligion;
        }

        public static DataTable GetCountry()
        {
            BsfGlobal.OpenWorkFlowDB();
            DataTable dtCountry = null;
            SqlDataAdapter sda;
            string ssql = "";

            try
            {
                ssql = "SELECT CountryId as Id, CountryName as Name FROM dbo.Countrymaster WHERE CountryName  <> '' Order By CountryName";
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_WorkFlowDB);
                dtCountry = new DataTable();
                sda.Fill(dtCountry);
                dtCountry.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dtCountry;
        }

        public static DataTable GetApartment()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtApartment = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT ApartmentId as Id,Description as Name FROM dbo.Apartment Order By ApartmentId", BsfGlobal.g_CRMDB);
                dtApartment = new DataTable();
                sda.Fill(dtApartment);
                dtApartment.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
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
            DataTable dtApartSize = null;
            SqlDataAdapter sda;
            try
            {
                sda = new SqlDataAdapter("SELECT ApartmentSizeId as Id, str(cast(ApartmentSizeFrom as int),10) + ' -' + str(cast(ApartmentSizeTo as int),10) as ApartmentSize FROM dbo.ApartmentSize", BsfGlobal.g_CRMDB);
                dtApartSize = new DataTable();
                sda.Fill(dtApartSize);
                dtApartSize.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
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
            DataTable dtApartType = null;
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
                BsfGlobal.CustomException(e.Message, e.StackTrace);
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
            DataTable dtCostPref = null;
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
                BsfGlobal.CustomException(e.Message, e.StackTrace);
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
                BsfGlobal.CustomException(e.Message, e.StackTrace);
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

            try
            {
                sda = new SqlDataAdapter("SELECT 0 LeadId,AreaId as Id, Description as Name, CONVERT(bit,0,1) Sel FROM dbo.LeadAreaMaster Order By AreaId", BsfGlobal.g_CRMDB);
                dtAreMaster = new DataTable();
                sda.Fill(dtAreMaster);
                dtAreMaster.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
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
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtPosessMaster;
        }

        public static DataTable GetProject()
        {
            DataTable dtProject = null;
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
                sda = new SqlDataAdapter(sSql,BsfGlobal.g_WorkFlowDB);
                dtProject = new DataTable();
                sda.Fill(dtProject);
                dtProject.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dtProject;

        }

        public static DataTable GetNature()
        {
            DataTable dtNature = null;
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
                BsfGlobal.CustomException(e.Message, e.StackTrace);
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
            DataTable dtSource = null;
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
                BsfGlobal.CustomException(e.Message, e.StackTrace);
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
                BsfGlobal.CustomException(e.Message, e.StackTrace);
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
                BsfGlobal.CustomException(e.Message, e.StackTrace);
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
            DataTable dtEmployment = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT EmploymentId as Id,Description as Name FROM dbo.Employment Order By Description", BsfGlobal.g_CRMDB);
                dtEmployment = new DataTable();
                sda.Fill(dtEmployment);
                dtEmployment.Dispose();
            }

            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
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
            DataTable dtGuestHouse = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT GuestHouseId as Id,Description as Name FROM dbo.GuestHouse Order By Description", BsfGlobal.g_CRMDB);
                dtGuestHouse = new DataTable();
                sda.Fill(dtGuestHouse);
                dtGuestHouse.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
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
            DataTable dtStay = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT StayId as Id,Description as Name FROM dbo.Stay Order By Description", BsfGlobal.g_CRMDB);
                dtStay = new DataTable();
                sda.Fill(dtStay);
                dtStay.Dispose();
            }

            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
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
            DataTable GetIncome = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT IncomeId as Id, str(cast(Incomefrom as float),10) + '   -' + str(cast(IncomeTo as float),12) as Income  FROM dbo.Income", BsfGlobal.g_CRMDB);
                GetIncome = new DataTable();
                sda.Fill(GetIncome);
                GetIncome.Dispose();
            }

            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return GetIncome;
        }

        internal static DataTable GetBroker(int argCCId)
        {
            DataTable dtBroker = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                //sSql = "SELECT BrokerId,B.VendorName BrokerName FROM dbo.BrokerDet A" +
                //            " Inner Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorMaster B On B.VendorId=A.VendorId" +
                //            " ORDER BY VendorName";
                if (BsfGlobal.g_bWPMDB == true)
                {
                    sSql = "Select A.SORegisterId,A.ContractorId VendorId,B.VendorName BrokerName,C.CPerson1 ContactPerson From [" + BsfGlobal.g_sWPMDBName + "].dbo.SORegister A " +
                            " Inner Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorMaster B On A.ContractorId=B.VendorId " +
                            " Inner Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorContact C On C.VendorID=B.VendorId " +
                            " Where A.CostCentreId=" + argCCId + " And A.ServiceType='B' And A.Approve='Y'";
                }
                else
                {
                    sSql = "Select 0 SORegisterId,0 VendorId,'' BrokerName,'' ContactPerson";
                }
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dtBroker = new DataTable();
                sda.Fill(dtBroker);
                dtBroker.Dispose();
            }

            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
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
                BsfGlobal.CustomException(e.Message, e.StackTrace);
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
                BsfGlobal.CustomException(e.Message, e.StackTrace);
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
                BsfGlobal.CustomException(e.Message, e.StackTrace);
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
            try
            {
                string sSql = "Select BusinessType,LandId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister " +
                                " Where ProjectName in(Select ProjectDB from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                                " Where CostCentreId=" + argCCId + ") And BusinessType In('B','L')";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        internal static DataSet ShowProjectGrid(int argCCId)
        {
            DataSet ds= new DataSet();
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select A.UserId ExecutiveId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As EmployeeName from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                      "Inner join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId " +
                      "Where A.UserId=" + BsfGlobal.g_lUserId + "";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Executive");
                da.Dispose();

                sSql = "Select CostCentreId,CostCentreName from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                        " Where ProjectDB in(Select ProjectName from " +
                        " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType in('B','L'))" +
                        " and CostCentreId not in (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                        " Where UserId=" + BsfGlobal.g_lUserId + ") Order by CostCentreName";
                BsfGlobal.OpenCRMDB();
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Project");
                da.Dispose();

                //sSql = "Select FlatTypeId,TypeName From FlatType Order By TypeName ";
                //BsfGlobal.OpenCRMDB();
                //da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                //da.Fill(ds, "Type");
                //da.Dispose();

                sSql = "Select CCId,CampaignId,CampaignName,RequestId From CampaignDetails Order By CampaignName";//Where CCId=" + argCCId + "";
                BsfGlobal.OpenCRMDB();
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Campaign");
                da.Dispose();

                sSql = "Select 0 CostCentreId,0 CampaignId,0 ExecutiveId From LeadProjectInfo Where LeadId=0";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                sda.Fill(ds,"Grid");
                sda.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return ds;
        }

        internal static DataSet ShowRegisterProjectGrid(int argCCId,int argLeadId)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select A.UserId ExecutiveId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As EmployeeName From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                      "Inner join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId " +
                      "Where A.UserId=" + BsfGlobal.g_lUserId + "";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Executive");
                da.Dispose();

                sSql = "Select CostCentreId,CostCentreName from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                        " Where ProjectDB in(Select ProjectName from " +
                        " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType in('B','L'))" +
                        " and CostCentreId not in (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                        " Where UserId=" + BsfGlobal.g_lUserId + ") Order by CostCentreName";
                BsfGlobal.OpenCRMDB();
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Project");
                da.Dispose();

                //sSql = "Select FlatTypeId,TypeName From FlatType Order By TypeName ";
                //BsfGlobal.OpenCRMDB();
                //da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                //da.Fill(ds, "Type");
                //da.Dispose();

                sSql = "Select CCId,CampaignId,CampaignName,RequestId From CampaignDetails Order By CampaignName";//Where CCId=" + argCCId + "";
                BsfGlobal.OpenCRMDB();
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Campaign");
                da.Dispose();

                sSql = "Select CostCentreId,CampaignId From dbo.LeadProjectInfo Where LeadId=" + argLeadId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                sda.Fill(ds, "Grid");
                sda.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return ds;
        }

        #endregion

        #region Insert Data's

        public static string GetType(string argqry, SqlConnection conn,SqlTransaction tran)
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

        internal static int InsertLeadDetails(LeadBO oLeedBO, string s_Mode, DataTable dtEnqTrans,bool UpdateLead,SqlConnection conn,SqlTransaction tran,DataTable dtFinal,string argFlatNo,bool argChk)
        {
            SqlCommand cmd; SqlDataReader dr;
            string sSql = "";
            DataTable dtLeadDetails = null;
            string s_NextCallDate = "";
            string s_LeadDate = "";
            string validUpto = "",dateFinal="";
            bool vip=false;
            decimal dAdvAmt = 0; decimal dRate = 0;

            try
            {
                s_LeadDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(oLeedBO.DE_LeadDate));
                s_NextCallDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(oLeedBO.DE_NextCallDate));
                oLeedBO.i_ExecutiveId = oLeedBO.UserId;

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
                        sSql = String.Format("Insert Into dbo.LeadRegister(LeadName, LeadDate, LeadType, CallTypeId,UserId,VIP,TypeId,CostId,Mobile,Email,NatureId,Remarks,MultiProject)" +
                        " Values('{0}', '{1}', '{2}' , {3},{4},'{5}',{6},{7},'{8}','{9}',{10},'{11}',{12} ) SELECT SCOPE_IDENTITY();", oLeedBO.s_LeadName, s_LeadDate, oLeedBO.s_LeadTypeName, oLeedBO.i_LeadCallTypeId, oLeedBO.UserId, vip, oLeedBO.i_FinApartmentTypeId, oLeedBO.i_FinApartmentCostId, oLeedBO.Mobile, oLeedBO.Email, oLeedBO.NatureId, oLeedBO.Remarks,0);
                    }
                    else
                    {
                        sSql = String.Format("Insert Into dbo.LeadRegister(LeadName, LeadDate, LeadType, CallTypeId, NextCallDate,UserId,VIP,TypeId,CostId,Mobile,Email,NatureId,Remarks,MultiProject)" +
                        " Values('{0}','{1}','{2}', {3} ,'{4}',{5},'{6}',{7},{8},'{9}','{10}',{11},'{12}',{13}) SELECT SCOPE_IDENTITY();", oLeedBO.s_LeadName, s_LeadDate, oLeedBO.s_LeadTypeName, oLeedBO.i_LeadCallTypeId, s_NextCallDate, oLeedBO.UserId, vip, oLeedBO.i_FinApartmentTypeId, oLeedBO.i_FinApartmentCostId, oLeedBO.Mobile, oLeedBO.Email, oLeedBO.NatureId, oLeedBO.Remarks,0);
                    }
                    cmd = new SqlCommand(sSql, conn, tran);
                    oLeedBO.EnquiryID = int.Parse(cmd.ExecuteScalar().ToString());
                    oLeedBO.i_LeadId = oLeedBO.EnquiryID;
                    cmd.Dispose();

                    
                }
                else
                {
                    dtLeadDetails = new DataTable();
                    sSql = string.Format("Update dbo.LeadRegister Set LeadName='{0}', LeadDate='{1}', LeadType='{2}', CallTypeId={3}, NextCallDate='{4}',UserId={5},VIP='{6}',TypeId={7},CostId={8},Mobile='{9}',Email='{10}',NatureId={11},Remarks='{12}',MultiProject=0 Where LeadId={13}",
                        oLeedBO.s_LeadName, s_LeadDate, oLeedBO.s_LeadTypeName, oLeedBO.i_LeadCallTypeId, s_NextCallDate, oLeedBO.UserId, vip, oLeedBO.i_FinApartmentTypeId, oLeedBO.i_FinApartmentCostId, oLeedBO.Mobile, oLeedBO.Email, oLeedBO.NatureId, oLeedBO.Remarks, oLeedBO.i_LeadId);

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
                            sSql = "INSERT INTO dbo.CallSheet (ExecutiveId,LeadId,ProjectId,TrnDate,CallFF,CallTypeID,NatureID,Remarks)VALUES" +
                                " (" + oLeedBO.ExecutiveID + "," + oLeedBO.EnquiryID + "," + oLeedBO.ProjID + ",'" + s_LeadDate + "','Fresh'," + oLeedBO.i_LeadCallTypeId + "," + oLeedBO.NatureId + ",'" + oLeedBO.Remarks + "')" +
                                    " SELECT SCOPE_IDENTITY();";
                            cmd = new SqlCommand(sSql, conn, tran);
                            iEntryId = int.Parse(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();

                            sSql = "INSERT INTO dbo.BuyerDetail(AllotmentNo,CostCentreId,ExecutiveId,LeadId,EntryId,FlatId,Status,CustomerType," +
                            " PaymentOption,BranchId,LoanPer,LoanAccNo,BrokerId,BrokerComm,BrokerAmount,ValidUpto,FinaliseDate,PostSaleExecId,Advance) Values('" + oLeedBO.AllotmentNo + "'," + oLeedBO.ProjID + "," +
                            " " + oLeedBO.ExecutiveID + "," + oLeedBO.EnquiryID + "," + iEntryId + "," + oLeedBO.FlatID + "," +
                            " '" + dtEnqTrans.Rows[0]["Status"] + "','" + dtEnqTrans.Rows[0]["CustomerType"] + "','" + dtEnqTrans.Rows[0]["PaymentOption"] + "'," + dtEnqTrans.Rows[0]["BranchId"] + ", " +
                            " " + dtEnqTrans.Rows[0]["LoanPer"] + ",'" + dtEnqTrans.Rows[0]["LoanAccNo"] + "'," + dtEnqTrans.Rows[0]["BrokerId"] + "," + dtEnqTrans.Rows[0]["ComPer"] + "," +
                            " " + dtEnqTrans.Rows[0]["ComAmount"] + ",'" + validUpto + "','" + dateFinal + "'," +
                            " " + dtEnqTrans.Rows[0]["PostSaleExecId"] + "," + dtEnqTrans.Rows[0]["AdvAmt"] + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                        }
                        if (oLeedBO.CallType == "Block")
                            sSql = String.Format("Update dbo.FlatDetails Set Status='B',LeadId={0} WHERE FlatId={1} AND CostCentreId={2}", oLeedBO.EnquiryID, oLeedBO.FlatID, oLeedBO.ProjID);
                        else
                            sSql = "Update dbo.FlatDetails Set Status='S',LeadId=" + oLeedBO.EnquiryID + " WHERE FlatId=" + oLeedBO.FlatID + " AND CostCentreId=" + oLeedBO.ProjID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        if (oLeedBO.CallType == "Block")
                        {
                            sSql = "INSERT INTO dbo.BlockUnits(CostCentreId,FlatId,CustomerType,LeadId,BlockUpto,Date,BlockType) Values(" + oLeedBO.ProjID + "," +
                             " " + oLeedBO.FlatID + ",'" + dtEnqTrans.Rows[0]["CustomerType"] + "'," + oLeedBO.EnquiryID + ",'" + validUpto + "'," +
                             " '" + Convert.ToDateTime(DateTime.Now).ToString("MM-dd-yyyy") + "','B')";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }

                        sSql = "INSERT INTO dbo.LeadFlatInfo (LeadId,Buyer,FlatId,CostCentreId)VALUES(" + oLeedBO.EnquiryID + ",1," + oLeedBO.FlatID + "," + oLeedBO.ProjID + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "SELECT UserName FROM dbo.UserLogin Where UserName='" + oLeedBO.s_LeadName + "'";
                        cmd = new SqlCommand(sSql, conn, tran);
                        SqlDataReader sdr; DataTable dt = new DataTable();
                        sdr = cmd.ExecuteReader();
                        dt.Load(sdr);
                        if (dt.Rows.Count > 0)
                            oLeedBO.s_LeadName = oLeedBO.s_LeadName + '1';

                        sSql = "INSERT INTO dbo.UserLogin(UserName,Password,Email,LeadId) VALUES" +
                            " ('" + oLeedBO.s_LeadName + "','" + oLeedBO.s_LeadName + "','" + oLeedBO.Email + "'," + oLeedBO.EnquiryID + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();


                        SqlDataReader sdr2, sdr1; DataTable dt1; int iStgId = 0, iTemplateId = 0;
                        int iDateAfter = 0, iDuration = 0; string sDurType = ""; DateTime SchDate;
                        sSql = "Select TemplateId,PreStageTypeId from dbo.PaymentScheduleFlat Where FlatId=" + oLeedBO.FlatID + " And PreStageTypeId=-1";
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
                                " Where A.FlatId=" + oLeedBO.FlatID + " And A.TemplateId>=" + iTemplateId + "" +
                                " And A.PaymentSchId Not In " +
                                " (Select PaySchId From dbo.ProgressBillRegister Where FlatId=" + oLeedBO.FlatID + ")";
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
                                SchDate = Convert.ToDateTime(dateFinal);

                                if (sDurType == "D")
                                { if (iDateAfter == 0) SchDate = SchDate.AddDays(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }
                                else if (sDurType == "M")
                                { if (iDateAfter == 0) SchDate = SchDate.AddMonths(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }


                                sSql = "Update dbo.PaymentScheduleFlat Set SchDate='" + Convert.ToDateTime(SchDate).ToString("dd-MMM-yyyy") + "'" +
                                    " Where TemplateId=" + iTemplateId + " And FlatId=" + oLeedBO.FlatID + "";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }

                        }

                        sSql = "Select Rate,AdvAmount From dbo.FlatDetails Where FlatId=" + oLeedBO.FlatID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        dr = cmd.ExecuteReader();
                        DataTable dtA = new DataTable();
                        dtA.Load(dr);
                        if (dtA.Rows.Count > 0)
                        {
                            dAdvAmt = Convert.ToDecimal(dtA.Rows[0]["AdvAmount"]);
                            dRate = Convert.ToDecimal(dtA.Rows[0]["Rate"]);
                            if (dAdvAmt != Convert.ToDecimal(dtEnqTrans.Rows[0]["AdvAmt"]))
                            {
                                sSql = "Update dbo.FlatDetails Set AdvAmount=" + dtEnqTrans.Rows[0]["AdvAmt"] + " WHERE FlatId=" + oLeedBO.FlatID + " ";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                //PaymentScheduleDL.InsertFlatScheduleI(oLeedBO.FlatID, conn, tran);
                            }
                        }

                        InsertChangeRate(oLeedBO.FlatID, dRate, Convert.ToDecimal(dtEnqTrans.Rows[0]["Rate"]), conn, tran);

                        if (dtFinal != null)
                            UnitDirDL.InsertFlatChk(dtFinal, oLeedBO.FlatID, "F", argChk, argFlatNo, oLeedBO.ProjID, conn, tran);

                        if (oLeedBO.CallType == "Finalization" || oLeedBO.CallType == "Finalisation")
                        {
                            PaymentScheduleDL.InsertFinalFlatScheduleI(oLeedBO.FlatID, "S", conn, tran);
                        }
                        else if (oLeedBO.CallType == "Block")
                        {
                            PaymentScheduleDL.InsertFinalFlatScheduleI(oLeedBO.FlatID, "B", conn, tran);
                        }
                        else { PaymentScheduleDL.InsertFlatScheduleI(oLeedBO.FlatID, conn, tran); }

                    }
                    else
                    {
                        if (s_Mode == "A")
                        {
                            sSql = "INSERT INTO dbo.CallSheet (ExecutiveId,LeadId,ProjectId,TrnDate,CallFF,NextCallDate,CallTypeID,NatureID,Remarks)VALUES" +
                                " (" + oLeedBO.ExecutiveID + "," + oLeedBO.EnquiryID + "," + oLeedBO.ProjID + ",'" + s_LeadDate + "','Fresh','" + s_NextCallDate + "'," + oLeedBO.i_LeadCallTypeId + "," + oLeedBO.NatureId + ",'" + oLeedBO.Remarks + "')";
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
            
            return oLeedBO.i_LeadId;

        }

        internal static void Insert_BuyerAlert(int argLeadID, string argProjName, string argType)
        {
            string sMsg = "";
            if (argType == "One")
                sMsg = "Dear Customer, Thank you for your interest shown in our Project - " + argProjName + ". Our Customer Relationship Executive will stay in touch with you.";
            else
                sMsg = "Dear Customer, Thank you for your interest shown in our Project. Our Customer Relationship Executive will stay in touch with you.";
            BsfGlobal.InsertBuyerAlert("Buyer-New-Lead-Wise", sMsg, argLeadID, BsfGlobal.g_sCRMDBName);
        }

        internal static int InsertMultipleLeadDetails(LeadBO oLeedBO, string s_Mode, DataTable dtEnqTrans, bool UpdateLead, SqlConnection conn, SqlTransaction tran, DataTable dtFinal, string argFlatNo, bool argChk, DataTable dtProj)
        {
            SqlCommand cmd; SqlDataReader dr;
            string sSql = "";
            DataTable dtLeadDetails = null;
            string s_NextCallDate = "";
            string s_LeadDate = "";
            bool vip = false;

            try
            {
                s_LeadDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(oLeedBO.DE_LeadDate));
                s_NextCallDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(oLeedBO.DE_NextCallDate));
                oLeedBO.i_ExecutiveId = oLeedBO.UserId;

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
                        sSql = String.Format("Insert Into dbo.LeadRegister(LeadName, LeadDate, LeadType, CallTypeId,UserId,VIP,TypeId,CostId,Mobile,Email,NatureId,Remarks,MultiProject)" +
                        " Values('{0}', '{1}', '{2}' , {3},{4},'{5}',{6},{7},'{8}','{9}',{10},'{11}',12 ) SELECT SCOPE_IDENTITY();", oLeedBO.s_LeadName, s_LeadDate, oLeedBO.s_LeadTypeName, oLeedBO.i_LeadCallTypeId, oLeedBO.UserId, vip, oLeedBO.i_FinApartmentTypeId, oLeedBO.i_FinApartmentCostId, oLeedBO.Mobile, oLeedBO.Email, oLeedBO.NatureId, oLeedBO.Remarks, 1);
                    }
                    else
                    {
                        sSql = String.Format("Insert Into dbo.LeadRegister(LeadName, LeadDate, LeadType, CallTypeId, NextCallDate,UserId,VIP,TypeId,CostId,Mobile,Email,NatureId,Remarks,MultiProject)" +
                        " Values('{0}','{1}','{2}', {3} ,'{4}',{5},'{6}',{7},{8},'{9}','{10}',{11},'{12}',13) SELECT SCOPE_IDENTITY();", oLeedBO.s_LeadName, s_LeadDate, oLeedBO.s_LeadTypeName, oLeedBO.i_LeadCallTypeId, s_NextCallDate, oLeedBO.UserId, vip, oLeedBO.i_FinApartmentTypeId, oLeedBO.i_FinApartmentCostId, oLeedBO.Mobile, oLeedBO.Email, oLeedBO.NatureId, oLeedBO.Remarks, 1);
                    }
                    cmd = new SqlCommand(sSql, conn, tran);
                    oLeedBO.EnquiryID = int.Parse(cmd.ExecuteScalar().ToString());
                    oLeedBO.i_LeadId = oLeedBO.EnquiryID;
                    cmd.Dispose();
                }
                else
                {
                    dtLeadDetails = new DataTable();
                    sSql = string.Format("Update dbo.LeadRegister Set LeadName='{0}', LeadDate='{1}', LeadType='{2}', CallTypeId={3}, NextCallDate='{4}',UserId={5},VIP='{6}',TypeId={7},CostId={8},Mobile='{9}',Email='{10}',NatureId={11},Remarks='{12}',MultiProject=1 Where LeadId={13}",
                        oLeedBO.s_LeadName, s_LeadDate, oLeedBO.s_LeadTypeName, oLeedBO.i_LeadCallTypeId, s_NextCallDate, oLeedBO.UserId, vip, oLeedBO.i_FinApartmentTypeId, oLeedBO.i_FinApartmentCostId, oLeedBO.Mobile, oLeedBO.Email, oLeedBO.NatureId, oLeedBO.Remarks, oLeedBO.i_LeadId);

                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                }


                if (UpdateLead == false)
                {
                    sSql = ""; 
                    if (s_Mode == "A")
                    {
                        if (dtProj.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtProj.Rows.Count; i++)
                            {
                                sSql = "INSERT INTO dbo.CallSheet (ExecutiveId,LeadId,ProjectId,TrnDate,CallFF,NextCallDate,CallTypeID,NatureID,Remarks)VALUES" +
                                    " (" + oLeedBO.ExecutiveID + "," + oLeedBO.EnquiryID + "," + dtProj.Rows[i]["CostCentreId"] + ",'" + s_LeadDate + "','Fresh','" + s_NextCallDate + "'," + oLeedBO.i_LeadCallTypeId + "," + oLeedBO.NatureId + ",'" + oLeedBO.Remarks + "')";
                                cmd = new SqlCommand(sSql, conn, tran);
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
            
            return oLeedBO.i_LeadId;
        }

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

                        PaymentScheduleDL.InsertFlatScheduleI(argFlatId, conn, tran);
                    }
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            
        }

        internal static int InsertPlotLeadDetails(LeadBO oLeedBO, string s_Mode, DataTable dtEnqTrans, bool UpdateLead, SqlConnection conn, SqlTransaction tran, DataTable dtFinal, string argFlatNo, bool argChk, DataTable dtLand)
        {
            SqlCommand cmd; SqlDataReader dr;
            string sSql = "";
            DataTable dtLeadDetails = null;
            string s_NextCallDate = "";
            string s_LeadDate = "";
            string validUpto = "", dateFinal = "";
            bool vip = false;
            decimal dAdvAmt = 0; decimal dRate = 0;
            try
            {
                s_LeadDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(oLeedBO.DE_LeadDate));
                s_NextCallDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(oLeedBO.DE_NextCallDate));
                oLeedBO.i_ExecutiveId = oLeedBO.UserId;

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
                        sSql = String.Format("Insert into dbo.LeadRegister(LeadName, LeadDate, LeadType, CallTypeId,UserId,VIP,TypeId,CostId,Mobile,Email,NatureId,Remarks)" +
                        " Values('{0}','{1}','{2}',{3},{4},'{5}',{6},{7},'{8}','{9}',{10},'{11}') SELECT SCOPE_IDENTITY();", oLeedBO.s_LeadName, s_LeadDate, oLeedBO.s_LeadTypeName, oLeedBO.i_LeadCallTypeId, oLeedBO.UserId, vip, oLeedBO.i_FinApartmentTypeId, oLeedBO.i_FinApartmentCostId, oLeedBO.Mobile, oLeedBO.Email, oLeedBO.NatureId, oLeedBO.Remarks);
                    }
                    else
                    {
                        sSql = String.Format("Insert into dbo.LeadRegister(LeadName, LeadDate, LeadType, CallTypeId, NextCallDate,UserId,VIP,TypeId,CostId,Mobile,Email,NatureId,Remarks)" +
                        " Values('{0}','{1}','{2}',{3},'{4}',{5},'{6}',{7},{8},'{9}','{10}',{11},'{12}') SELECT SCOPE_IDENTITY();", oLeedBO.s_LeadName, s_LeadDate, oLeedBO.s_LeadTypeName, oLeedBO.i_LeadCallTypeId, s_NextCallDate, oLeedBO.UserId, vip, oLeedBO.i_FinApartmentTypeId, oLeedBO.i_FinApartmentCostId, oLeedBO.Mobile, oLeedBO.Email, oLeedBO.NatureId, oLeedBO.Remarks);
                    }
                    cmd = new SqlCommand(sSql, conn, tran);
                    oLeedBO.EnquiryID = int.Parse(cmd.ExecuteScalar().ToString());
                    oLeedBO.i_LeadId = oLeedBO.EnquiryID;
                    cmd.Dispose();
                }
                else
                {
                    dtLeadDetails = new DataTable();
                    sSql = string.Format("Update dbo.LeadRegister Set LeadName='{0}', LeadDate='{1}', LeadType='{2}', CallTypeId={3}, NextCallDate='{4}',UserId={5},VIP='{6}',TypeId={7},CostId={8},Mobile='{9}',Email='{10}',NatureId={11},Remarks='{12}' Where LeadId={13}",
                        oLeedBO.s_LeadName, s_LeadDate, oLeedBO.s_LeadTypeName, oLeedBO.i_LeadCallTypeId, s_NextCallDate, oLeedBO.UserId, vip, oLeedBO.i_FinApartmentTypeId, oLeedBO.i_FinApartmentCostId, oLeedBO.Mobile, oLeedBO.Email, oLeedBO.NatureId, oLeedBO.Remarks, oLeedBO.i_LeadId);

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
                            sSql = "INSERT INTO dbo.CallSheet (ExecutiveId,LeadId,ProjectId,TrnDate,CallFF,CallTypeID,NatureID,Remarks)VALUES" +
                                " (" + oLeedBO.ExecutiveID + "," + oLeedBO.EnquiryID + "," + oLeedBO.ProjID + ",'" + s_LeadDate + "','Fresh'," + oLeedBO.i_LeadCallTypeId + "," + oLeedBO.NatureId + ",'" + oLeedBO.Remarks + "')" +
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

                        SqlDataReader sdr2, sdr1; DataTable dt1; int iStgId = 0, iTemplateId = 0;
                        int iDateAfter = 0, iDuration = 0; string sDurType = ""; DateTime SchDate;
                        sSql = "Select TemplateId,PreStageTypeId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot " +
                            " Where PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + " And PreStageTypeId=-1";
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
                            sSql = "Select A.TemplateId,A.DateAfter,A.Duration,A.Durationtype from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot A" +
                                " Left Join dbo.PlotProgressBillRegister B On A.PlotDetailsId=B.PlotDetailsId " +
                                " Where A.PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + " And A.TemplateId>=" + iTemplateId + "" +
                                " And A.PaymentSchId Not In " +
                                " (Select PaySchId From dbo.PlotProgressBillRegister Where PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + ")";
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
                                SchDate = Convert.ToDateTime(dateFinal);

                                if (sDurType == "D")
                                { if (iDateAfter == 0) SchDate = SchDate.AddDays(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }
                                else if (sDurType == "M")
                                { if (iDateAfter == 0) SchDate = SchDate.AddMonths(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }


                                sSql = "Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot Set SchDate='" + Convert.ToDateTime(SchDate).ToString("dd-MMM-yyyy") + "'" +
                                    " Where TemplateId=" + iTemplateId + " And PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + "";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }

                        }

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
                            sSql = "INSERT INTO dbo.CallSheet (ExecutiveId,LeadId,ProjectId,TrnDate,CallFF,NextCallDate,CallTypeID,NatureID,Remarks)VALUES" +
                                " (" + oLeedBO.ExecutiveID + "," + oLeedBO.EnquiryID + "," + oLeedBO.ProjID + ",'" + s_LeadDate + "','Fresh','" + s_NextCallDate + "'," + oLeedBO.i_LeadCallTypeId + "," + oLeedBO.NatureId + ",'" + oLeedBO.Remarks + "')";
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
            
            return oLeedBO.i_LeadId;
        }

        internal static int InsertMultiplePlotLeadDetails(LeadBO oLeedBO, string s_Mode, DataTable dtEnqTrans, bool UpdateLead, SqlConnection conn, SqlTransaction tran, DataTable dtFinal, string argFlatNo, bool argChk, DataTable dtLand, DataTable dtProj)
        {
            SqlCommand cmd; SqlDataReader dr;
            string sSql = "";
            DataTable dtLeadDetails = null;
            string s_NextCallDate = "";
            string s_LeadDate = "";
            string validUpto = "", dateFinal = "";
            bool vip = false;
            decimal dAdvAmt = 0; decimal dRate = 0;
            try
            {
                s_LeadDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(oLeedBO.DE_LeadDate));
                s_NextCallDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(oLeedBO.DE_NextCallDate));
                oLeedBO.i_ExecutiveId = oLeedBO.UserId;

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
                        sSql = String.Format("Insert into dbo.LeadRegister(LeadName, LeadDate, LeadType, CallTypeId,UserId,VIP,TypeId,CostId,Mobile,Email,NatureId,Remarks,MultiProject)" +
                           " Values('{0}','{1}','{2}',{3},{4},'{5}',{6},{7},'{8}','{9}',{10},'{11}',{12}) SELECT SCOPE_IDENTITY();", oLeedBO.s_LeadName, s_LeadDate, oLeedBO.s_LeadTypeName, oLeedBO.i_LeadCallTypeId, oLeedBO.UserId, vip, oLeedBO.i_FinApartmentTypeId, oLeedBO.i_FinApartmentCostId, oLeedBO.Mobile, oLeedBO.Email, oLeedBO.NatureId, oLeedBO.Remarks,1);
                    }
                    else
                    {
                        sSql = String.Format("Insert into dbo.LeadRegister(LeadName, LeadDate, LeadType, CallTypeId, NextCallDate,UserId,VIP,TypeId,CostId,Mobile,Email,NatureId,Remarks,MultiProject)" +
                        " Values('{0}','{1}','{2}',{3},'{4}',{5},'{6}',{7},{8},'{9}','{10}',{11},'{12}',{13}) SELECT SCOPE_IDENTITY();", oLeedBO.s_LeadName, s_LeadDate, oLeedBO.s_LeadTypeName, oLeedBO.i_LeadCallTypeId, s_NextCallDate, oLeedBO.UserId, vip, oLeedBO.i_FinApartmentTypeId, oLeedBO.i_FinApartmentCostId, oLeedBO.Mobile, oLeedBO.Email, oLeedBO.NatureId, oLeedBO.Remarks,1);
                    }
                    cmd = new SqlCommand(sSql, conn, tran);
                    oLeedBO.EnquiryID = int.Parse(cmd.ExecuteScalar().ToString());
                    oLeedBO.i_LeadId = oLeedBO.EnquiryID;
                    cmd.Dispose();
                }
                else
                {
                    dtLeadDetails = new DataTable();
                    sSql = string.Format("Update dbo.LeadRegister Set LeadName='{0}', LeadDate='{1}', LeadType='{2}', CallTypeId={3}, NextCallDate='{4}',UserId={5},VIP='{6}',TypeId={7},CostId={8},Mobile='{9}',Email='{10}',NatureId={11},Remarks='{12}',MultiProject=1 Where LeadId={13}",
                       oLeedBO.s_LeadName, s_LeadDate, oLeedBO.s_LeadTypeName, oLeedBO.i_LeadCallTypeId, s_NextCallDate, oLeedBO.UserId, vip, oLeedBO.i_FinApartmentTypeId, oLeedBO.i_FinApartmentCostId, oLeedBO.Mobile, oLeedBO.Email, oLeedBO.NatureId, oLeedBO.Remarks, oLeedBO.i_LeadId);

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
                            sSql = "INSERT INTO dbo.CallSheet (ExecutiveId,LeadId,ProjectId,TrnDate,CallFF,CallTypeID,NatureID,Remarks)VALUES" +
                                " (" + oLeedBO.ExecutiveID + "," + oLeedBO.EnquiryID + "," + oLeedBO.ProjID + ",'" + s_LeadDate + "','Fresh'," + oLeedBO.i_LeadCallTypeId + "," + oLeedBO.NatureId + ",'" + oLeedBO.Remarks + "')" +
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
                            if (dtProj.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtProj.Rows.Count; i++)
                                {
                                    sSql = "INSERT INTO dbo.CallSheet (ExecutiveId,LeadId,ProjectId,TrnDate,CallFF,NextCallDate,CallTypeID,NatureID,Remarks)VALUES" +
                                        " (" + oLeedBO.ExecutiveID + "," + oLeedBO.EnquiryID + "," + dtProj.Rows[i]["CostCentreId"] + ",'" + s_LeadDate + "','Fresh','" + s_NextCallDate + "'," + oLeedBO.i_LeadCallTypeId + "," + oLeedBO.NatureId + ",'" + oLeedBO.Remarks + "')";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
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
            
        }

        internal static void InsertProjectInformation(LeadBO oLeedBO, string s_Mode, bool argUpdateLead,SqlConnection conn,SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null; SqlDataReader dr;
            try
            {
                oLeedBO.i_ExecutiveId = oLeedBO.UserId;

                ssql = "Select Isnull(B.PositionType,'') PositionType,A.PowerUser From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                       " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B On A.PositionId=B.PositionId" +
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

                ssql = "Delete From dbo.LeadProjectInfo Where LeadId=" + oLeedBO.i_LeadId + "";
                cmd = new SqlCommand(ssql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                //if (s_Mode == "A" && argUpdateLead==false)
                //{
                ssql = String.Format("Insert Into dbo.LeadProjectInfo(CostCentreId,ExecutiveId,CampaignId,Status,Remarks,LeadId,NextCallDate) Values({0},{1},{2},'{3}','{4}',{5},@NextCallDate)", 
                                    oLeedBO.i_ProjCostCentreId, oLeedBO.i_ExecutiveId, oLeedBO.i_SubSourceId, oLeedBO.s_ProjStatus, oLeedBO.s_ProjRemarks, oLeedBO.i_LeadId);
                cmd = new SqlCommand(ssql, conn, tran);
                SqlParameter dateparameter = new SqlParameter() { ParameterName = "@NextCallDate", DbType = DbType.DateTime };
                if (NewLeadBL.NextCallDate == DateTime.MinValue)
                    dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                else
                    dateparameter.Value = oLeedBO.NextCallDate;
                cmd.Parameters.Add(dateparameter);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                //}
                //else
                //{
                //    ssql = String.Format("Update dbo.LeadProjectInfo set CostCentreId={0},ExecutiveId={1},CampaignId={2}, Status='{3}', Remarks='{4}' where LeadId={5}", oLeedBO.i_ProjCostCentreId, oLeedBO.i_ExecutiveId, oLeedBO.i_SubSourceId, oLeedBO.s_ProjStatus, oLeedBO.s_ProjRemarks, oLeedBO.i_LeadId);
                //    cmd = new SqlCommand(ssql, conn,tran);
                //    cmd.ExecuteNonQuery();
                //    cmd.Dispose();
                //}
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            
        }

        internal static void InsertMultipleProjectInformation(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran,DataTable dtProj)
        {
            string ssql = ""; SqlDataReader dr;
            SqlCommand cmd = null;
            try
            {
                oLeedBO.i_ExecutiveId = oLeedBO.UserId;

                ssql = "Select Isnull(B.PositionType,'') PositionType,A.PowerUser From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                       " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B On A.PositionId=B.PositionId" +
                       " Where A.UserId=" + oLeedBO.UserId + "";
                cmd = new SqlCommand(ssql, conn, tran);
                dr = cmd.ExecuteReader();
                DataTable dtE = new DataTable();
                dtE.Load(dr);
                if (dtE.Rows.Count > 0)
                {
                    if (dtE.Rows[0]["PositionType"].ToString() == "M" || Convert.ToInt32(dtE.Rows[0]["PowerUser"]) == 1)
                    {
                        oLeedBO.i_ExecutiveId = 0;
                    }
                    else
                        oLeedBO.i_ExecutiveId = 0;
                }
                //string sUserType = GetType(ssql, conn, tran);
                //if (sUserType == "M")
                //    oLeedBO.i_ExecutiveId = 0;//oLeedBO.UserId;
                //else
                //    oLeedBO.i_ExecutiveId = 0;

                ssql = "Delete From dbo.LeadProjectInfo Where LeadId=" + oLeedBO.i_LeadId + "";
                cmd = new SqlCommand(ssql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                if (dtProj.Rows.Count > 0)
                {
                    for (int i = 0; i < dtProj.Rows.Count; i++)
                    {
                        //if (s_Mode == "A" && argUpdateLead == false)
                        //{
                        ssql = String.Format("Insert Into dbo.LeadProjectInfo(CostCentreId,ExecutiveId,CampaignId,Status,Remarks,LeadId,NextCallDate) Values({0},{1},{2},'{3}','{4}',{5},@NextCallDate)", 
                                            dtProj.Rows[i]["CostCentreId"], oLeedBO.i_ExecutiveId, dtProj.Rows[i]["CampaignId"], oLeedBO.s_ProjStatus, oLeedBO.s_ProjRemarks, oLeedBO.i_LeadId);
                        cmd = new SqlCommand(ssql, conn, tran);
                        SqlParameter dateparameter = new SqlParameter() { ParameterName = "@NextCallDate", DbType = DbType.DateTime };
                        if (NewLeadBL.NextCallDate == DateTime.MinValue)
                            dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            dateparameter.Value = oLeedBO.NextCallDate;
                        cmd.Parameters.Add(dateparameter);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        //}
                        //else
                        //{
                        //    ssql = String.Format("Update dbo.LeadProjectInfo set CostCentreId={0},ExecutiveId={1},CampaignId={2}, Status='{3}', Remarks='{4}' where LeadId={5}", dtProj.Rows[i]["CostCentreId"], oLeedBO.i_ExecutiveId, dtProj.Rows[i]["CampaignId"], oLeedBO.s_ProjStatus, oLeedBO.s_ProjRemarks, oLeedBO.i_LeadId);
                        //    cmd = new SqlCommand(ssql, conn, tran);
                        //    cmd.ExecuteNonQuery();
                        //    cmd.Dispose();
                        //}
                    }
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            
        }

        internal static void InsertExeInformation(LeadBO oLeedBO, string s_Mode,bool argUpdateLead,SqlConnection conn,SqlTransaction tran)
        {
            string ssql = ""; SqlDataReader dr;
            DataTable dtExeInfo = new DataTable();
            SqlCommand cmd;
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
                    //ssql = String.Format("Update dbo.LeadExecutiveInfo Set ExecutiveId={0}, Status='{1}', Remarks='{2}', Category='{3}' where LeadId={4}", oLeedBO.i_ExecutiveId, oLeedBO.s_ExeStatus, oLeedBO.s_ExeRemarks, oLeedBO.s_Category, oLeedBO.i_LeadId);
                    ssql = String.Format("Update dbo.LeadExecutiveInfo Set Status='{0}', Remarks='{1}', Category='{2}' Where LeadId={3}", oLeedBO.s_ExeStatus, oLeedBO.s_ExeRemarks, oLeedBO.s_Category, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            
        }

        internal static void InsertMultipleExeInformation(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran, DataTable dtProj)
        {
            string ssql = "";
            DataTable dtExeInfo = new DataTable();
            SqlCommand cmd; SqlDataReader dr;
            oLeedBO.i_ExecutiveId = BsfGlobal.g_lUserId;
            try
            {
                ssql = "Select Isnull(B.PositionType,'') PositionType,A.PowerUser from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A "+
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
                        oLeedBO.i_ExecutiveId = 0;
                    }
                    else
                        oLeedBO.i_ExecutiveId = 0;
                }
                //string sUserType = GetType(ssql, conn, tran);
                //if (sUserType == "M")
                //    oLeedBO.i_ExecutiveId = 0;//oLeedBO.UserId;
                //else
                //    oLeedBO.i_ExecutiveId = 0;

                //if (s_Mode == "A" && argUpdateLead == false)
                //{

                ssql = "Delete From dbo.LeadExecutiveInfo Where LeadId=" + oLeedBO.i_LeadId + "";
                cmd = new SqlCommand(ssql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                for (int i = 0; i < dtProj.Rows.Count; i++)
                {
                    ssql = String.Format("Insert Into dbo.LeadExecutiveInfo(ExecutiveId, Status, Remarks, Category, LeadId,CostCentreId) Values({0},'{1}','{2}','{3}',{4},{5})", oLeedBO.i_ExecutiveId, oLeedBO.s_ExeStatus, oLeedBO.s_ExeRemarks, oLeedBO.s_Category, oLeedBO.i_LeadId, dtProj.Rows[i]["CostCentreId"]);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                //}
                //else
                //{
                //    ssql = String.Format("Update dbo.LeadExecutiveInfo Set ExecutiveId={0}, Status='{1}', Remarks='{2}', Category='{3}' where LeadId={4}", oLeedBO.i_ExecutiveId, oLeedBO.s_ExeStatus, oLeedBO.s_ExeRemarks, oLeedBO.s_Category, oLeedBO.i_LeadId);
                //    cmd = new SqlCommand(ssql, conn, tran);
                //    cmd.ExecuteNonQuery();
                //    cmd.Dispose();
                //}
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            
        }

        internal static void InsertPersonalInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
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
            

        }

        internal static void InsertCoAppInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
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
            
        }

        internal static void InsertPOAInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            string s_POADOB = "";
            string s_POAWeddingDate = "";
            try
            {
                s_POAWeddingDate = string.Format("{0:dd/MMM/yyyy}", oLeedBO.DE_POAWeddingDate);
                s_POADOB = string.Format("{0:dd/MMM/yyyy}", oLeedBO.DE_POADOB);

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
            
        }

        internal static void InsertCommAddInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead,SqlConnection conn,SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            try
            {
                if (s_Mode == "A" && argUpdateLead == false)
                {
                    ssql = String.Format("Insert Into dbo.LeadCommAddressInfo(Address1, Address2, City, State, Country, PinCode, LandLine, Mobile, Fax, LeadId, Email, PanNo, PassportNo) Values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', {9}, '{10}', '{11}', '{12}')", oLeedBO.s_CommAdd1, oLeedBO.s_CommAdd2, oLeedBO.s_CommCity, oLeedBO.s_CommState, oLeedBO.i_CommCountry, oLeedBO.s_CommPinCode, oLeedBO.s_CommLandLine, oLeedBO.s_CommMobile, oLeedBO.s_CommFax, oLeedBO.i_LeadId, oLeedBO.s_CommEmail, oLeedBO.s_CommPanNo, oLeedBO.s_CommPassportNo);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update dbo.LeadCommAddressInfo Set Address1='{0}', Address2='{1}', City='{2}', State='{3}', Country='{4}', PinCode='{5}', LandLine='{6}', Mobile='{7}', Fax='{8}', Email='{9}', PanNo='{10}', PassportNo='{11}',WebUpdate={12} Where LeadId={13}", oLeedBO.s_CommAdd1, oLeedBO.s_CommAdd2, oLeedBO.s_CommCity, oLeedBO.s_CommState, oLeedBO.i_CommCountry, oLeedBO.s_CommPinCode, oLeedBO.s_CommLandLine, oLeedBO.s_CommMobile, oLeedBO.s_CommFax, oLeedBO.s_CommEmail, oLeedBO.s_CommPanNo, oLeedBO.s_CommPassportNo, 0, oLeedBO.i_LeadId);
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
            
        }

        internal static void InsertOffAddInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
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
            
        }

        internal static void InsertPermAddInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
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
            
        }

        internal static void InsertNRIAddInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
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
            
        }

        internal static void InsertCoAppAddInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
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
            
        }

        internal static void InsertPOAAddInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
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
            
        }

        internal static void InsertSourceInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand cmd = null; 

            try
            {
                if (s_Mode == "A" && argUpdateLead==false)
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
            
        }

        internal static void InsertChildInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
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
            
        }

        internal static void InsertApartmentInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
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
            
        }

        internal static void InsertSubSourceInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand cmd = null; 

            try
            {
                if (s_Mode == "A" && argUpdateLead==false)
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
            
        }

        internal static void InsertRequirement(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
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
            
        }

        internal static void InsertFinance(LeadBO oLeedBo, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
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
            
        }

        internal static void InsertFacility(LeadBO oLeedBO, string s_Mode, SqlConnection conn, SqlTransaction tran)
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
            
        }

        internal static void InsertArea(LeadBO oLeedBO, string s_Mode, SqlConnection conn, SqlTransaction tran)
        {
            string ssql = "";
            SqlCommand cmd = null;
            try
            {
                if (s_Mode == "A")
                {
                    ssql = String.Format("Insert Into dbo.Area(AreaId, Sel, LeadId, Description) values({0}, {1}, {2}, '{3}')", oLeedBO.i_ChklstArea, oLeedBO.b_AreaSel, oLeedBO.i_LeadId, oLeedBO.s_AreDescription);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update dbo.Area Set Sel={0}, Description='{1}' Where AreaId={2} and LeadId={3}", oLeedBO.b_AreaSel, oLeedBO.s_AreDescription, oLeedBO.i_ChklstArea, oLeedBO.i_LeadId);
                    cmd = new SqlCommand(ssql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            
        }

        internal static void InsertPossess(LeadBO oLeedBO, string s_Mode, SqlConnection conn, SqlTransaction tran)
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
            
        }

        internal static void InsertBankDet(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
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
            
        }

        internal static void InsertNRIContDet(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
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
                sSql = "Select A.UserName,A.Password,B.Email From dbo.UserLogin A Inner Join dbo.LeadRegister B "+
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

        internal static DataTable ShowLeadName()
        {
            DataTable dtLeadName = null;
            SqlDataAdapter sdaLName;
            string sSql = "";
            try
            {
                if (BsfGlobal.FindPermission("Lead-View-Other-Users-Lead") == false)
                {
                    sSql = "Select Distinct R.LeadId, R.LeadName,A.CostCentreId From dbo.LeadRegister R " +
                           " Inner Join dbo.LeadProjectInfo A On A.LeadId=R.LeadId " +
                           " Inner Join dbo.LeadExecutiveInfo E On A.LeadID=E.LeadId " +
                           " Where E.ExecutiveId=" + BsfGlobal.g_lUserId + " Order By LeadName";
                }
                else 
                {
                    sSql = "Select Distinct R.LeadId, R.LeadName,A.CostCentreId From dbo.LeadRegister R " +
                           " Inner Join dbo.LeadProjectInfo A On A.LeadId=R.LeadId Order By LeadName ";
                }
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

        internal static DataTable ShowApartInfo(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadApartmentInfo Where LeadId=" + e_leadbo.i_LeadId + "" + "Order By LeadId";
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

        internal static DataTable ShowChildInfo(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadChildInfo Where LeadId=" + e_leadbo.i_LeadId + "";
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

        internal static DataTable ShowCoAppAddInfo(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadCoAppAddressInfo Where LeadId=" + e_leadbo.i_LeadId + "" + "Order By LeadId";
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

        internal static DataTable ShowCoAppInfo(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadCoApplicantInfo Where LeadId=" + e_leadbo.i_LeadId + "" + "Order By LeadId";
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

        internal static DataTable ShowCommAddInfo(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadCommAddressInfo Where LeadId=" + e_leadbo.i_LeadId + "" + "Order By LeadId";
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

        internal static DataTable ShowPOAInfo(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadPOAInfo Where LeadId=" + e_leadbo.i_LeadId + "" + "Order By LeadId";
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

        internal static DataTable ShowPOAAddInfo(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadPOAAddressInfo Where LeadId=" + e_leadbo.i_LeadId + "" + "Order By LeadId";
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

        internal static DataTable ShowExeInfo(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadExecutiveInfo Where LeadId=" + e_leadbo.i_LeadId + "" + "Order By LeadId";
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

        internal static DataTable ShowFinance(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadFinance Where LeadId=" + e_leadbo.i_LeadId + "" + "Order By LeadId";
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

        internal static DataTable ShowRequirement(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select LeadId, Facility, Area, Remarks  FROM dbo.LeadRequirement"+
                    " Where LeadId=" + e_leadbo.i_LeadId + "" + "Order By LeadId";
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

        internal static DataTable ShowNRIAddInfo(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadNRIAddressInfo Where LeadId=" + e_leadbo.i_LeadId + "" + "Order By LeadId";
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

        internal static DataTable ShowOffAddInfo(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadOffAddressInfo Where LeadId=" + e_leadbo.i_LeadId + "" + "Order By LeadId";
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

        internal static DataTable ShowPermAddInfo(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadPermanentAddressInfo Where LeadId=" + e_leadbo.i_LeadId + "" + "Order By LeadId";
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

        internal static DataTable ShowPersonalInfo(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadPersonalInfo Where LeadId=" + e_leadbo.i_LeadId + "" + "Order By LeadId";
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

        internal static DataTable ShowProjectInfo(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadProjectInfo Where LeadId=" + e_leadbo.i_LeadId + "" ;
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

        internal static DataTable ShowSource(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadSource Where LeadId=" + e_leadbo.i_LeadId + "" + "Order By LeadId";
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

        internal static DataTable ShowSubSource(LeadBO e_leadbo)
        {
            DataTable dtLeadName = null;
            SqlDataAdapter sdaLName;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadSubSource Where LeadId=" + e_leadbo.i_LeadId + "" + "Order By LeadId";
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

        internal static DataTable ShowLeadType(LeadBO e_leadbo)
        {
            DataTable dtLeadType = null;
            SqlDataAdapter sdaLType;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadType Where LeadId=" + e_leadbo.i_LeadId + "" + "Order By LeadId";
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
        
        internal static DataTable ShowLeadDetails(LeadBO e_leadbo)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtLeadName = null;
            SqlDataAdapter sdaLName = null;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadRegister Where LeadId=" + e_leadbo.i_LeadId + "" + "Order By LeadId";
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

        internal static DataTable ShowFacility(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select Distinct A.FacilityId as Id, A.Description as Name, Case when B.Sel=0"+
                    " Then  Convert(bit, 0, 0) else Convert(bit, 1, 1) End as Sel, "+
                    " LeadId From dbo.FacilityMaster A Left Join dbo.Facility B on A.FacilityId=B.FacilityId "+
                    " And LeadId=" + e_leadbo.i_LeadId + " Order By A.FacilityId";
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

        internal static DataTable ShowArea(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select Distinct LeadId,A.AreaId as Id, A.Description as Name, Case when ISNULL(B.Sel,0)=0" +
                    " Then Convert(bit, 0, 0) else Convert(bit, 1, 1) End as Sel " +
                    " From dbo.LeadAreaMaster A Left Join dbo.Area B on A.AreaId=B.AreaId " +
                    " And LeadId=" + e_leadbo.i_LeadId + " Order By A.AreaId";
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

        internal static DataTable ShowPossess(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select Distinct A.PossessId as Id, A.Description as Name, Case when B.Sel=0"+
                    " Then Convert(bit, 0, 0) else Convert(bit, 1, 1) End as Sel,"+ 
                    " LeadId From dbo.PossessMaster A Left Join dbo.Possess B On A.PossessId=B.PossessId "+
                    " And LeadId=" + e_leadbo.i_LeadId + " Order By A.PossessId";
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

        internal static DataTable ShowNRIContDet(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadNRIContactDet Where LeadId=" + e_leadbo.i_LeadId + "" + " Order By LeadId";
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

        internal static DataTable ShowBankDet(LeadBO e_leadbo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select * From dbo.LeadBankDetails Where LeadId=" + e_leadbo.i_LeadId + "" + " Order By LeadId";
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

        internal static DataTable ShowLeadDate(string argFromDate, string argToDate, bool argOtherExec)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql = "";
            //sSql = " Select R.LeadId, R.LeadName,A.CostCentreId From dbo.LeadRegister R " +
            //        " Inner Join dbo.LeadProjectInfo A On A.LeadId=R.LeadId " +
            //        " Where R.LeadDate Between '" + argFromDate + "' And '" + argToDate + "' " +
            //        " Order By R.LeadDate";
            if (BsfGlobal.FindPermission("Lead-View-Other-Users-Lead") == false)
            {
                sSql = " Select Distinct R.LeadId, R.LeadName,R.LeadDate,A.CostCentreId,R.Mobile,R.Email,IsNull(O.CostCentreName,'')CostCentre,IsNull(U.EmployeeName,'') ExecutiveName From dbo.LeadRegister R  " +
                         " Left Join dbo.LeadProjectInfo A On A.LeadId=R.LeadId " +
                         " Inner Join dbo.LeadExecutiveInfo E On A.LeadID=E.LeadId " +
                         " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O On O.CostCentreId=A.CostCentreId " +
                         " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users U On U.UserId=A.ExecutiveId " +
                         " Where R.LeadDate Between '" + argFromDate + "' And '" + argToDate + "'";
                         //" And E.ExecutiveId=" + BsfGlobal.g_lUserId + " Order By R.LeadDate";
                if (argOtherExec == false) { sSql = sSql + " and E.ExecutiveId = " + BsfGlobal.g_lUserId + " Order By R.LeadDate"; }
                else
                {
                    sSql = sSql + " AND E.ExecutiveId In " +
                     " (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users Where UserId=" + BsfGlobal.g_lUserId + " or " +
                     " UserId in (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans Where sUserId=" + BsfGlobal.g_lUserId + ")) Order By R.LeadDate ";
                }
            }
            else
            {
                sSql = " Select Distinct R.LeadId, R.LeadName,R.LeadDate,A.CostCentreId,R.Mobile,R.Email,IsNull(O.CostCentreName,'')CostCentre,IsNull(U.EmployeeName,'') ExecutiveName From dbo.LeadRegister R  " +
                       " Left Join dbo.LeadProjectInfo A On A.LeadId=R.LeadId Inner Join dbo.LeadExecutiveInfo E On A.LeadId=E.LeadId " +
                       " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O On O.CostCentreId=A.CostCentreId " +
                       " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users U On U.UserId=A.ExecutiveId " +
                       " Where R.LeadDate Between '" + argFromDate + "' And '" + argToDate + "' ";
                if (argOtherExec == false) { sSql = sSql + " Order By R.LeadDate"; }
                else
                {
                    sSql = sSql + " AND E.ExecutiveId In " +
                     " (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users Where UserId=" + BsfGlobal.g_lUserId + " or " +
                     " UserId in (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans Where sUserId=" + BsfGlobal.g_lUserId + ")) Order By R.LeadDate ";
                }
            }
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

        internal static void InsertEmailSent(string argEmail,string argMobile,int argLeadId,string argSub)
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
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }           
        }

        internal static DataTable MobileNo_Found(string argMobileNo)
        {
            DataTable dtUser = null;
            SqlDataAdapter sdaLName;
            string sSql = "";
            try
            {
                sSql = "Select LeadId,LeadName From dbo.LeadRegister Where Mobile='" + argMobileNo + "'";
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

        internal static bool Sold_LeadFound(int argLeadId)
        {
            DataTable dt;
            SqlDataAdapter sdaLName;
            string sSql = "";
            bool bAns = false;
            try
            {
                sSql = "Select LeadId From dbo.FlatDetails Where LeadId=" + argLeadId + " And Status='S'";
                sdaLName = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sdaLName.Fill(dt);
                if (dt.Rows.Count > 0)
                    bAns = true;
                else bAns = false;
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
            return bAns;
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
            
        }

        #endregion

        internal static int GetBuyerEntryId(int argLeadId)
        {
            int i_EntryId = 0;
            try
            {
                string sSql = "SELECT EntryId FROM dbo.BuyerDetail Where LeadId=" + argLeadId + "";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();

                if (dt.Rows.Count > 0)
                    i_EntryId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["EntryId"], CommFun.datatypes.vartypenumeric));
                else
                    i_EntryId = 0;
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return i_EntryId;
        }

        internal static void InsertLeadCheckList(string[] argFieldName)
        {
            BsfGlobal.OpenCRMDB();
            try
            {
                string ssql = "Delete LeadCheckList Where Sel=0";
                SqlCommand cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                ssql = "Select COUNT(*) from LeadCheckList Where FieldName IN('Lead Date', 'Type', 'Lead Name')";
                cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                int iCount = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                cmd.Dispose();

                if (iCount == 0)
                {
                    ssql = "Insert Into LeadCheckList(FieldId, FieldName, Sel) " +
                           " Values(1, 'Lead Date', 1),(2, 'Type', 1),(3, 'Lead Name', 1)";
                    cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                for (int i = 0; i <= argFieldName.Length - 1; i++)
                {
                    ssql = "Select COUNT(*) from LeadCheckList Where FieldName='" + CommFun.IsNullCheck(argFieldName[i], CommFun.datatypes.vartypestring).ToString() + "'";
                    cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                    iCount = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                    cmd.Dispose();

                    if (iCount == 0)
                    {
                        int Id = i + 4;
                        ssql = "Insert Into LeadCheckList(FieldId, FieldName) Values(" + Id + ",'" + argFieldName[i] + "')";
                        cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
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
                BsfGlobal.g_CRMDB.Close();
            }
        }

        internal static DataTable GetLeadCheckList()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Select FieldId, FieldName, Sel From dbo.LeadCheckList ORDER BY FieldId";
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static void UpdateLeadCheckList(DataTable argdt)
        {
            BsfGlobal.OpenCRMDB();
            try
            {
                for (int i = 0; i <= argdt.Rows.Count - 1; i++)
                {
                    string ssql = "Update LeadCheckList Set Sel='" + Convert.ToBoolean(CommFun.IsNullCheck(argdt.Rows[i]["Sel"], CommFun.datatypes.varTypeBoolean)) + "' "+
                                  " Where FieldId=" + Convert.ToInt32(CommFun.IsNullCheck(argdt.Rows[i]["FieldId"], CommFun.datatypes.vartypenumeric)) + "";
                    SqlCommand cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
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

        internal static DataTable PopulateCustomerFeedback()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Select FeedbackDate, Case When FeedbackType=1 Then 'Feedback' When FeedbackType=2 Then 'Queries' " +
                              " When FeedbackType=3 Then 'Complaint' Else 'None' End FeedbackType, Remarks FROM dbo.CustomerFeedback";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dr.Close();
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
            return dt;
        }
    }
}
