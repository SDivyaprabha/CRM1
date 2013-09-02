using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.BusinessObjects;
using CRM.DataLayer;
using System.Data.SqlClient;

namespace CRM
{
    class NewLeadBL
    {
        public static int  CostCentreId { get; set; }
        public static int CampaignId { get; set; }

        #region LeadEntry

        internal static int LeadId { get; set; }
        internal static string LeadName { get; set; }
        internal static DateTime LeadDate { get; set; }
        internal static string LeadType { get; set; }
        internal static int CallTypeId { get; set; }
        internal static int NatureId { get; set; }
        internal static DateTime NextCallDate { get; set; }
        internal static int UnitType { get; set; }
        internal static int VIP { get; set; }
        internal static int CostPreference { get; set; }
        internal static string Mobile { get; set; }
        internal static string Email { get; set; }
        internal static int UserId { get; set; }
        internal static string Remarks { get; set; }
        internal static bool MultiProject { get; set; }
        //internal static int Campaign { get; set; }
        //internal static string ProjectName { get; set; }
        //internal static string Remarks { get; set; }
        //internal static string AttendBy { get; set; }
      
        #endregion

        #region Finalization

        internal static string AllotmentNo { get; set; }
        internal static string CallType { get; set; }
        internal static string FlatNo { get; set; }
        internal static string PlotNo { get; set; }
        internal static string ProjectName { get; set; }
        internal static int ProjectId { get; set; }
        internal static int FlatId { get; set; }
        internal static string BuyerName { get; set; }
        internal static int BrokerId { get; set; }
        internal static decimal CommPer { get; set; }
        internal static decimal CommAmt { get; set; }
        internal static int ReminderId { get; set; }
        internal static bool bChkSend { get; set; }
        internal static string BusinessType { get; set; }

        #endregion

        #region Personal Entry
        // internal static int Pe_LeadId { get; set; }
        internal static string Pe_Gender { get; set; }
        internal static DateTime Pe_DOB { get; set; }
        internal static int Pe_Religion { get; set; }
        internal static int Pe_Nationality { get; set; }
        internal static int Pe_Profession { get; set; }
        internal static string Pe_Organization { get; set; }
        internal static string Pe_FatherName { get; set; }
        internal static string Pe_MotherName { get; set; }
        internal static int Pe_MarritalStatus { get; set; }
        internal static string Pe_WifeName { get; set; }
        internal static DateTime Pe_WeddingDate { get; set; }
        internal static int Pe_NRI { get; set; }

        internal static string Pe_ContactPerson { get; set; }
        internal static string Pe_ContactAdd { get; set; }
        internal static int Pe_ContactCity { get; set; }
        internal static int Pe_ContactState { get; set; }
        internal static string Pe_ContactMobileNo { get; set; }
        internal static string Pe_ContactMailId { get; set; }
        #endregion

        #region Power Of Attorney Address Entry
      //  internal static int PoAA_LeadId { get; set; }
        internal static string PoAA_Address1 { get; set; }
        internal static string PoAA_Address2 { get; set; }
        internal static int PoAA_City { get; set; }
        internal static int PoAA_State { get; set; }
        internal static int PoAA_Country { get; set; }
        internal static string PoAA_PinCode { get; set; }
        internal static string PoAA_LandLine { get; set; }
        internal static string PoAA_Mobile { get; set; }
        internal static string PoAA_Email { get; set; }
        internal static string PoAA_Fax { get; set; }
        internal static string PoAA_PanNo { get; set; }
       
        #endregion

        #region Co-Applicant Entry
       // internal static int CoApp_LeadId { get; set; }
        internal static string CoApp_CoApplicantName { get; set; }
        internal static string CoApp_Gender { get; set; }
        internal static DateTime CoApp_DOB { get; set; }
        internal static int CoApp_Religion { get; set; }
        internal static int CoApp_Nationality { get; set; }
        internal static string CoApp_FatherName { get; set; }
        internal static string CoApp_MotherName { get; set; }
        internal static string CoApp_WifeName { get; set; }
        internal static DateTime CoApp_WeddingDate { get; set; }
        internal static int CoApp_Profession { get; set; }
        internal static string CoApp_Organization { get; set; }
        internal static int  CoApp_MarritalStatus { get; set; }

        #endregion

        #region Child Entry
       // internal static int C_LeadId { get; set; }
        internal static string C_ChildName { get; set; }
        internal static string C_Sex { get; set; }
        internal static DateTime C_DOB { get; set; }
        #endregion

        #region Financial Entry
       // internal static int F_LeadId { get; set; }
        internal static int  F_Employment { get; set; }
        internal static int F_Income { get; set; }
        internal static string F_BankName { get; set; }
        internal static string F_LoanNO { get; set; }
        internal static int F_Stay { get; set; }
        internal static int F_GuestHouse { get; set; }
        internal static int F_Possess { get; set; }
        internal static int F_Broker { get; set; }
        internal static int F_Block { get; set; }
        internal static decimal F_Comission { get; set; }
        internal static string F_ContactPerson { get; set; }
        // internal static int F_Block { get; set; }
        //internal static int F_Apartmenttype { get; set; }
        //internal static int F_CostPreference { get; set; }
        internal static int F_Appatment { get; set; }
        internal static int F_AppatmentSize { get; set; }
        #endregion

        #region Bank Details
      //  internal static int B_LeadId { get; set; }
        internal static string B_ContactPerson { get; set; }
        internal static string B_ContactMobileNo { get; set; }
        internal static string B_BankName { get; set; }
        internal static string B_LoanNo { get; set; }
        internal static string B_Branch { get; set; }
        internal static decimal B_InterestRate { get; set; }
        internal static decimal B_LoanAmount{ get; set; }
        internal static string B_Need { get; set; }
        
        #endregion

        #region Permanent Entry
       // internal static int P_LeadId { get; set; }
        internal static string P_Address1 { get; set; }
        internal static string P_Address2 { get; set; }
        internal static string P_Locality { get; set; }
        internal static int P_City { get; set; }
        internal static int P_State { get; set; }
        internal static int P_Country { get; set; }
        internal static string P_PinCode { get; set; }
        internal static string P_LandLine { get; set; }
        internal static string P_Mobile { get; set; }
        internal static string P_Email { get; set; }
        internal static string P_Fax { get; set; }
        internal static string P_PanNo { get; set; }
        #endregion

        #region Co-Applicant Address Entry
      //  internal static int CoA_LeadId { get; set; }
        internal static string CoA_Address1 { get; set; }
        internal static string CoA_Address2 { get; set; }
        internal static int CoA_City { get; set; }
        internal static int CoA_State { get; set; }
        internal static int CoA_Country { get; set; }
        internal static string CoA_PinCode { get; set; }
        internal static string CoA_LandLine { get; set; }
        internal static string CoA_Mobile { get; set; }
        internal static string CoA_Email { get; set; }
        internal static string CoA_Fax { get; set; }
        internal static string CoA_PanNo { get; set; }
        #endregion

        #region PowerOfAttorney Entry
       // internal static int PoA_LeadId { get; set; }
        internal static string PoA_ApplicantName { get; set; }
        internal static string PoA_Gender { get; set; }
        internal static DateTime PoA_DOB { get; set; }
        internal static int PoA_Religion { get; set; }
        internal static int PoA_Nationality { get; set; }
        internal static string PoA_FatherName { get; set; }
        internal static string PoA_MotherName { get; set; }
        internal static string PoA_WifeName { get; set; }
        internal static DateTime PoA_WeddingDate { get; set; }
        internal static int PoA_Profession { get; set; }
        internal static string PoA_Organization { get; set; }
        internal static int PoA_MarritalStatus { get; set; }
        internal static string PoA_Relation { get; set; }
        #endregion

        #region Office Entry
     //   internal static int Office_LeadId { get; set; }
        internal static string Office_Address1 { get; set; }
        internal static string Office_Address2 { get; set; }
        internal static int Office_City { get; set; }
        internal static int Office_State { get; set; }
        internal static int Office_Country { get; set; }
        internal static string Office_PinCode { get; set; }
        internal static string Office_LandLine { get; set; }
        internal static string Office_Mobile { get; set; }
        internal static string Office_Email { get; set; }
        internal static string Office_Fax { get; set; }
        internal static string Office_PanNo { get; set; }
        #endregion

        #region NRI Entry
       // internal static int NRI_LeadId { get; set; }
        internal static string NRI_PersonName { get; set; }
        internal static string NRI_Address { get; set; }
        internal static int NRI_City { get; set; }
        internal static int NRI_State { get; set; }
        internal static string NRI_Mobile { get; set; }
        internal static string NRI_Email { get; set; }
        internal static string NRI_PinCode { get; set; }
        internal static string NRI_LandLine { get; set; }
        internal static int NRI_Country { get; set; }
        internal static string NRI_Fax { get; set; }
        internal static string NRI_Address2 { get; set; }
        internal static string NRI_PanNo { get; set; }
        internal static string NRI_PassportNo { get; set; }
        
        #endregion

        #region Communication Entry
      //  internal static int Com_LeadId { get; set; }
        internal static string Com_AddressType { get; set; }
        internal static string Com_Address1 { get; set; }
        internal static string Com_Address2 { get; set; }
        internal static string Com_Locality { get; set; }
        internal static int Com_City { get; set; }
        internal static int Com_State { get; set; }
        internal static int Com_Country { get; set; }
        internal static string Com_PinCode { get; set; }
        internal static string Com_LandLine { get; set; }
        internal static string Com_Mobile { get; set; }
        internal static string Com_Email { get; set; }
        internal static string Com_Fax { get; set; }
        internal static string Com_PanNo { get; set; }
        internal static string Com_PassportNo { get; set; }
        #endregion
    
        #region Requirement Entry
      //  internal static int Req_LeadId { get; set; }
        internal static int Req_Facility { get; set; }
        internal static int Req_Area { get; set; }
        internal static string Req_Remarks { get; set; }
        //internal static string Req_ProBlock { get; set; }
        
       
        #endregion

        #region Executive Entry
       // internal static int Exe_LeadId { get; set; }
        internal static int Exe_ExecutiveId { get; set; }
        internal static DateTime Exe_FromDate { get; set; }
        internal static DateTime Exe_EndDate { get; set; }
        internal static string Exe_Status { get; set; }
        internal static string Exe_Remarks { get; set; }
        internal static string Exe_Category { get; set; }
        internal static int Exe_CostCenterId { get; set; }
        
        #endregion

        #region Methods


        //public static DataTable GetProject(int argProjTypeId)
        //{
        //    return NewLeadDL.GetProject(argProjTypeId);
        //}

        public static DataTable GetCat()
        {
            return NewLeadDL.GetCat();
        }

        public static DataTable GetExecutiveList()
        {
            return NewLeadDL.GetExecutiveList();
        }

        public static DataTable FillProjType()
        {
            return NewLeadDL.FillProjType();       
        }

        public static DataTable GetCallType()
        {
            return NewLeadDL.GetCallType();       
        }

        public static DataTable GetBroker(int argCCId)
        {
            return NewLeadDL.GetBroker(argCCId);
        }

        //NewForm

        public static DataTable GetReligion()
        {
            return NewLeadDL.GetReligion();
        }

        public static DataTable GetNationality()
        {
            return NewLeadDL.GetNationality();
        }

        public static DataTable GetProfession()
        {
            return NewLeadDL.GetProfession();
        }

        public static DataTable GetCountry()
        {
            return NewLeadDL.GetCountry();
        }

        public static DataTable GetApartment()
        {
            return NewLeadDL.GetApartment();
        }

        public static DataTable GetApartmentSize()
        {
            return NewLeadDL.GetApartmentsize();
        }

        public static DataTable GetApartmentType()
        {
            return NewLeadDL.GetApartmentType();
        }

        public static DataTable GetCostPref()
        {
            return NewLeadDL.GetCostPreference();
        }

        public static DataTable GetFacilityMaster()
        {
            return NewLeadDL.GetFacilityMaster();
        }

        public static DataTable GetAreaMaster()
        {
            return NewLeadDL.GetAreaMaster();
        }

        public static DataTable GetProject()
        {
            return NewLeadDL.GetProject();
        }

        public static DataTable GetNature()
        {
            return NewLeadDL.GetNature();
        }

        public static DataTable GetSource()
        {
            return NewLeadDL.GetSource();
        }

        public static DataTable GetSubSource()
        {
            return NewLeadDL.GetSubSource();
        }

        public static DataTable GetExecutive()
        {
            return NewLeadDL.GetExecutive();
        }

        public static DataTable GetEmpStatus()
        {
            return NewLeadDL.GetEmpStatus();
        }

        public static DataTable GetGuestHouse()
        {
            return NewLeadDL.GetGuestHouse();
        }

        public static DataTable GetStay()
        {
            return NewLeadDL.GetStay();
        }

        public static DataTable GetPossessMaster()
        {
            return NewLeadDL.GetPossessMaster();
        }

        public static DataTable GetIncome()
        {
            return NewLeadDL.GetIncome();
        }

        public static DataTable GetFacility()
        {
            return NewLeadDL.GetFacility();
        }

        public static DataTable GetArea()
        {
            return NewLeadDL.GetArea();
        }

        public static DataTable GetPossess()
        {
            return NewLeadDL.GetPossess();
        }

        internal static DataTable GetBusinessType(int argCCId)
        {
            return NewLeadDL.GetBusinessType(argCCId);
        }

        #endregion

        #region InsertData

        public static void InsertFacility(NewLeadBO oLeedBO, string s_Mode, SqlConnection conn, SqlTransaction tran)
        {
            NewLeadDL.InsertFacility(oLeedBO, s_Mode,conn,tran);
        }

        //public static void InsertArea(NewLeadBO oLeedBO, string s_Mode, SqlConnection conn, SqlTransaction tran)
        //{
        //    NewLeadDL.InsertArea(oLeedBO, s_Mode,conn,tran);
        //}


        //public static int InsertLeadDetails(NewLeadBO oLeedBO, string s_Mode, DataTable dtEnqTrans,bool UpdateLead,SqlConnection conn,SqlTransaction tran,DataTable dtFinal,string argFlatNo,bool argChk)
        //{
        //    return NewLeadDL.InsertLeadDetails(oLeedBO, s_Mode, dtEnqTrans, UpdateLead,conn,tran,dtFinal,argFlatNo,argChk);
        //}

        internal static int InsertPlotLeadDetails(NewLeadBO oLeedBO, string s_Mode, DataTable dtEnqTrans, bool UpdateLead, SqlConnection conn, SqlTransaction tran, DataTable dtFinal, string argFlatNo, bool argChk,DataTable dtLand)
        {
            return NewLeadDL.InsertPlotLeadDetails(oLeedBO, s_Mode, dtEnqTrans, UpdateLead, conn, tran,dtFinal,argFlatNo,argChk,dtLand);
        }

        public static void InsertProjectInformation(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead,SqlConnection conn,SqlTransaction tran)
        {
            NewLeadDL.InsertProjectInformation(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertExeInformation(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead,SqlConnection conn,SqlTransaction tran)
        {
            NewLeadDL.InsertExeInformation(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertPersonalInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            NewLeadDL.InsertPersonalInfo(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertCoAppInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            NewLeadDL.InsertCoAppInfo(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        internal static void InsertPOAInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            NewLeadDL.InsertPOAInfo(oLeedBO, s_Mode, argUpdateLead, conn, tran);
        }

        public static void InsertCommAddInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead,SqlConnection conn,SqlTransaction tran)
        {
            NewLeadDL.InsertCommAddInfo(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertOffAddInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            NewLeadDL.InsertOffAddInfo(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertPermAddInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            NewLeadDL.InsertPermAddInfo(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertNRIAddInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            NewLeadDL.InsertNRIAddInfo(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertCoAppAddInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            NewLeadDL.InsertCoAppAddInfo(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }
        internal static void InsertPOAAddInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            NewLeadDL.InsertPOAAddInfo(oLeedBO, s_Mode, argUpdateLead, conn, tran);
        }
        public static void InsertSourceInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead)
        {
            NewLeadDL.InsertSourceInfo(oLeedBO, s_Mode,argUpdateLead);
        }

        public static void InsertSubSourceInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead)
        {
            NewLeadDL.InsertSubSourceInfo(oLeedBO, s_Mode,argUpdateLead);
        }

        public static void InsertChildInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            NewLeadDL.InsertChildInfo(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertApartmentInfo(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            NewLeadDL.InsertApartmentInfo(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertRequirement(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            NewLeadDL.InsertRequirement(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertFinance(NewLeadBO oLeedBo, string s_Mode, bool argUpdateLead,SqlConnection conn,SqlTransaction tran)
        {
            NewLeadDL.InsertFinance(oLeedBo, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertPossess(NewLeadBO oLeedBo, string s_Mode, SqlConnection conn, SqlTransaction tran)
        {
            NewLeadDL.InsertPossess(oLeedBo, s_Mode,conn,tran);
        }

        public static void InsertBankDet(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            NewLeadDL.InsertBankDet(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertNRIContDet(NewLeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            NewLeadDL.InsertNRIContDet(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        #endregion

        #region Register Function

        internal static DataTable LeadLogin(int argLeadId)
        {
            return NewLeadDL.LeadLogin(argLeadId);
        }

        internal static bool LeadFound(int argLeadId)
        {
            return NewLeadDL.LeadFound(argLeadId);
        }

        internal static DataTable ShowPOAInfo(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowPOAInfo(e_NewLeadBO);
        }

        internal static DataTable ShowPOAAddInfo(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowPOAAddInfo(e_NewLeadBO);
        }

        public static DataTable ShowUser()
        {
            return NewLeadDL.ShowUser();
        }

        public static DataTable ShowFacility(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowFacility(e_NewLeadBO);
        }

        public static DataTable ShowArea(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowArea(e_NewLeadBO);
        }

        public static DataTable ShowPossess(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowPossess(e_NewLeadBO);
        }

        public static DataTable ShowLeadDetails(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowLeadDetails(e_NewLeadBO);
        }

        public static DataTable FillLeadGrid(string argFromDate, string argToDate)
        {
            return NewLeadDL.FillLeadGrid(argFromDate, argToDate);
        }

        internal static DataTable ShowLeadDate(string argFromDate, string argToDate, bool argOtherExec)
        {
            return NewLeadDL.ShowLeadDate(argFromDate, argToDate, argOtherExec);
        }

        public static DataTable ShowApartInfo(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowApartInfo(e_NewLeadBO);
        }

        public static DataTable ShowChildInfo(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowChildInfo(e_NewLeadBO);
        }

        public static DataTable ShowCoAppAddInfo(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowCoAppAddInfo(e_NewLeadBO);
        }

        public static DataTable ShowCoAppInfo(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowCoAppInfo(e_NewLeadBO);
        }

        public static DataTable ShowCommAddInfo(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowCommAddInfo(e_NewLeadBO);
        }

        public static DataTable ShowExeInfo(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowExeInfo(e_NewLeadBO);
        }

        public static DataTable ShowFinance(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowFinance(e_NewLeadBO);
        }

        public static DataTable ShowRequirement(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowRequirement(e_NewLeadBO);
        }

        public static DataTable ShowNRIAddInfo(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowNRIAddInfo(e_NewLeadBO);
        }

        public static DataTable ShowOffAddInfo(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowOffAddInfo(e_NewLeadBO);
        }

        public static DataTable ShowPermAddInfo(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowPermAddInfo(e_NewLeadBO);
        }

        public static DataTable ShowPersonalInfo(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowPersonalInfo(e_NewLeadBO);
        }

        public static DataTable ShowProjectInfo(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowProjectInfo(e_NewLeadBO);
        }

        public static DataTable ShowSource(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowSource(e_NewLeadBO);
        }

        public static DataTable ShowLeadType(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowLeadType(e_NewLeadBO);
        }

        public static DataTable ShowNRIContDet(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowNRIContDet(e_NewLeadBO);
        }

        public static DataTable ShowBankDet(NewLeadBO e_NewLeadBO)
        {
            return NewLeadDL.ShowBankDet(e_NewLeadBO);
        }

        internal static DataTable ShowLeadDate(string argFromDate, string argToDate)
        {
            return NewLeadDL.ShowLeadDate(argFromDate, argToDate);
        }

        public static DataTable GetCompanyMailDetails()
        {
            return NewLeadDL.GetCompanyMailDetails();
        }

        public static DataTable GetLeadFlatTypeDetails(int argProjId, string argsType, int argLandId, int argLeadId)
        {
            return NewLeadDL.GetLeadFlatTypeDetails(argProjId, argsType, argLandId, argLeadId);
        }

        #endregion

        #region PickList

        public static DataTable GetPLMaster(string argType)
        {
            return NewLeadDL.GetPLMaster(argType);
        }

        public static DataTable GetPLData(int argLeadId, string argType)
        {
            return NewLeadDL.GetPLData(argLeadId,argType);
        }

        public static void InsertPLMaster(string argDesc, string argType)
        {
            NewLeadDL.InsertPLMaster(argDesc,argType);
        }

        public static void UpdatePLMaster(int argId, string argDesc, string argType)
        {
            NewLeadDL.UpdatePLMaster(argId,argDesc,argType);
        }

        public static bool CheckPLId(int argId, string argType)
        {
            return NewLeadDL.CheckPLId(argId, argType);
        }

        public static bool CheckDesc(string argDesc, string argType)
        {
            return NewLeadDL.CheckDesc(argDesc,argType);
        }

        public static void DeletePLMaster(int argId, string argType)
        {
            NewLeadDL.DeletePLMaster(argId,argType);
        }

        #endregion

        #region Religion Master

        public static DataTable GetReligionMaster()
        {
            return NewLeadDL.GetReligionMaster();
        }

        public static void InsertReligionMaster(string argDesc)
        {
            NewLeadDL.InsertReligionMaster(argDesc);
        }

        public static void UpdateReligionMaster(int argRelId, string argDesc)
        {
            NewLeadDL.UpdateReligionMaster(argRelId, argDesc);
        }

        public static void DeleteReligionMaster(int argRelId)
        {
            NewLeadDL.DeleteReligionMaster(argRelId);
        }

        public static bool FoundReligion(int argRelId)
        {
            return NewLeadDL.FoundReligion(argRelId);
        }

        public static bool CheckRelDesc(string argDesc)
        {
            return NewLeadDL.CheckRelDesc(argDesc);
        }

        #endregion

        #region LeadDetails

        internal static DataTable ShowLeadName()
        {
            return NewLeadDL.ShowLeadName();
        }

        public static void DelFacility(int argLeadId, SqlConnection conn, SqlTransaction tran)
        {
            NewLeadDL.DelFacility(argLeadId,conn,tran);
        }

        public static void DelArea(int argLeadId, SqlConnection conn, SqlTransaction tran)
        {
            NewLeadDL.DelArea(argLeadId,conn,tran);
        }

        public static void DelPoss(int argLeadId, SqlConnection conn, SqlTransaction tran)
        {
            NewLeadDL.DelPoss(argLeadId,conn,tran);
        }

        internal static DataTable ShowProjectGrid(int argLeadId)
        {
            return NewLeadDL.ShowProjectGrid(argLeadId);
        }

        internal static DataTable ShowCampaignGrid()
        {
            return NewLeadDL.ShowCampaignGrid();
        }

        internal static bool Update_LeadDet(List<NewLeadBO> ProjectSel,DataTable dtA,DataTable dtP,DataTable dtF,DataTable dtBD,DataTable dtChk)
        {
            return NewLeadDL.Update_LeadDet(ProjectSel,dtA,dtP,dtF,dtBD,dtChk);
        }

        public static bool Check_Lead_Name(string arg_sLeadName, int arg_iLeadId)
        {
            return NewLeadDL.Check_Lead_Name(arg_sLeadName, arg_iLeadId);
        }

        internal static DataSet Get_LeadDet(int arg_iLeadId)
        {
            return NewLeadDL.Get_LeadDet(arg_iLeadId);
        }

        internal static bool Delete_LeadDetils(int arg_iLeadId)
        {
            return NewLeadDL.Delete_LeadDetils(arg_iLeadId);
        }

        internal static bool FoundLeadDetils(int arg_iLeadId)
        {
            return NewLeadDL.FoundLeadDetils(arg_iLeadId);
        }

        public static void InsertEmailSent(string argEmail, string argMobile, int argLeadId, string argSub)
        {
             NewLeadDL.InsertEmailSent(argEmail,argMobile,argLeadId,argSub);
        }

        public static DataTable GetEditRegisterBuyerDet(int argLeadId)
        {
            return NewLeadDL.GetEditRegisterBuyerDet(argLeadId);
        }

        public static DataTable GetGridLeadReg(int argLeadId)
        {
            return NewLeadDL.GetGridLeadReg(argLeadId);
        }

        public static DataTable GetLoanStatus()
        {
            return NewLeadDL.GetLoanStatus();
        }

        #endregion

        #region Email

        public static DataTable GetOpCostCentre()
        {
            return NewLeadDL.GetOpCostCentre();
        }

        public static DataTable GetEmailExecutive()
        {
            return NewLeadDL.GetEmailExecutive();
        }

        public static DataTable GetEmailBuyers(string argBuyer,int argExecId,int argCCId,string argDateType,DateTime argFrom,DateTime argTo)
        {
            return NewLeadDL.GetEmailBuyers(argBuyer,argExecId,argCCId,argDateType,argFrom,argTo);
        }

        #endregion

        internal static DataTable GetCityMaster()
        {
            return NewLeadDL.GetCityMaster();
        }

        internal static DataTable GetStateMaster()
        {
            return NewLeadDL.GetStateMaster();
        }
    }
}
