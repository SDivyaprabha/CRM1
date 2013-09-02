using System;
using System.Collections.Generic;
using System.Linq;

namespace CRM
{
    class LeadBO
    {
        #region Variables

        public string AllotmentNo { get; set; }
        public string s_LeadName { get; set; }
        public string s_LeadTypeName { get; set; }
        public string s_LeadCallTypeName { get; set; }

        public string s_ProjectName { get; set; }
        public string s_ProjStatus { get; set; }
        public string s_ProjRemarks { get; set; }

        public int UserId { get; set; }
        public int VIP { get; set; }
        public int NatureId { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        public string s_ExeName { get; set; }
        public string s_ExeStatus { get; set; }
        public string s_ExeRemarks { get; set; }

        public string s_PersonalReligion { get; set; }
        public string s_PersonalNationality { get; set; }
        public string s_PersonalProfession { get; set; }
        public string s_PersonalOrganization { get; set; }
        public string s_PersonalMotherName { get; set; }
        public string s_PersonalFatherName { get; set; }
        public string s_PersonalWifeName { get; set; }
        public string s_PersonalMaritalStatus { get; set; }
        public string s_PersonalNRI { get; set; }

        public string s_CoApplicantName { get; set; }
        public string s_CoAppReligion { get; set; }
        public string s_CoAppNationality { get; set; }
        public string s_CoAppProfession { get; set; }
        public string s_CoAppOrganization { get; set; }
        public string s_CoAppMotherName { get; set; }
        public string s_CoAppFatherName { get; set; }
        public string s_CoAppWifeName { get; set; }
        public string s_CoAppMaritalStatus { get; set; }

        public string s_POAName { get; set; }
        public string s_POAReligion { get; set; }
        public string s_POANationality { get; set; }
        public string s_POAProfession { get; set; }
        public string s_POAOrganization { get; set; }
        public string s_POAMotherName { get; set; }
        public string s_POAFatherName { get; set; }
        public string s_POAWifeName { get; set; }
        public string s_POAMaritalStatus { get; set; }
        public string s_POARelation { get; set; }

        public string s_CommAdd1 { get; set; }
        public string s_CommAdd2 { get; set; }
        public string s_CommCity { get; set; }
        public string s_CommState { get; set; }
        public string s_CommCountry { get; set; }

        public string s_OffAdd1 { get; set; }
        public string s_OffAdd2 { get; set; }
        public string s_OffCity { get; set; }
        public string s_OffState { get; set; }
        public string s_OffCountry { get; set; }

        public string s_NRIAdd1 { get; set; }
        public string s_NRIAdd2 { get; set; }
        public string s_NRICity { get; set; }
        public string s_NRIState { get; set; }
        public string s_NRICountry { get; set; }

        public string s_PermAdd1 { get; set; }
        public string s_PermAdd2 { get; set; }
        public string s_PermCity { get; set; }
        public string s_PermState { get; set; }
        public string s_PermCountry { get; set; }

        public string s_CoAdd1 { get; set; }
        public string s_CoAdd2 { get; set; }
        public string s_CoCity { get; set; }
        public string s_CoState { get; set; }
        public string s_CoCountry { get; set; }

        public string s_POAAdd1 { get; set; }
        public string s_POAAdd2 { get; set; }
        public string s_POACity { get; set; }
        public string s_POAState { get; set; }
        public string s_POACountry { get; set; }

        public string s_ChildName { get; set; }

        public string s_SourceName { get; set; }
        public string s_SubSource { get; set; }
        public string s_SourceProjName { get; set; }

        public string s_ChklstFacility { get; set; }
        public string s_ChklstArea { get; set; }
        public string s_ReqNeed { get; set; }
        public string s_ReqRemarks { get; set; }
        public string s_ReqProjBlock { get; set; }

        public string s_FinEmpStatus { get; set; }
        public string s_FinYearlyIncome { get; set; }
        public string s_FinBankName { get; set; }
        public string s_FinLoanNo { get; set; }

        public string s_FinApartmentName { get; set; }
        public string s_FinApartmentSize { get; set; }
        public string s_FinApartmentType { get; set; }
        public string s_FinCostPref { get; set; }

        public string s_FinStay { get; set; }
        public string s_FinGuestHouse { get; set; }
        public string s_FinChklstPossess { get; set; }
        public string s_FinProjBlock { get; set; }
        public string s_FinBroker { get; set; }
        public string s_FinComission { get; set; }
        public string s_FinContPer { get; set; }

        public string s_Category { get; set; }

        public string s_CoEmail { get; set; }
        public string s_POAEmail { get; set; }
        public string s_CommEmail { get; set; }
        public string s_OffEmail { get; set; }
        public string s_NRIEmail { get; set; }
        public string s_PermEmail { get; set; }

        public string s_FacDescription { get; set; }
        public string s_AreDescription { get; set; }
        public string s_PossDescription { get; set; }

        public string s_NRIContPerson { get; set; }
        public string s_NRIContAdd { get; set; }
        public string s_NRIContCity { get; set; }
        public string s_NRIContState { get; set; }
        public string s_NRIContMail { get; set; }

        public string s_BankName { get; set; }
        public string s_BankBranch { get; set; }
        public string s_BankContPer { get; set; }

        public string s_NRIContNo { get; set; }

        public string s_BankContNo { get; set; }
        public string s_BankLoanNo { get; set; }
        public int i_BankIntRate { get; set; }
        public int i_BankLoanAmt { get; set; }

        public int i_LeadId { get; set; }
        public int i_LeadTypeId { get; set; }
        public int i_LeadCallTypeId { get; set; }
        public int i_ProjCostCentreId { get; set; }
        public int i_ExecutiveId { get; set; }
        public int i_PersonalGender { get; set; }
        public int i_CoAppGenderId { get; set; }
        public int i_POAGenderId { get; set; }
        public int i_PersonalMaritalStatusId { get; set; }
        public int i_CoAppMaritalStatusId { get; set; }
        public int i_POAMaritalStatusId { get; set; }
        public int i_ChildNameId { get; set; }

        public string s_CommPinCode { get; set; }
        public string s_POAPinCode { get; set; }
        public string s_CoPinCode { get; set; }
        public string s_OffPinCode { get; set; }
        public string s_PermPinCode { get; set; }
        public string s_NRIPinCode { get; set; }

        public string s_CommMobile { get; set; }
        public string s_POAMobile { get; set; }
        public string s_CoMobile { get; set; }
        public string s_OffMobile { get; set; }
        public string s_PermMobile { get; set; }
        public string s_NRIMobile { get; set; }

        public string s_CommLandLine { get; set; }
        public string s_POALandLine { get; set; }
        public string s_CoLandLine { get; set; }
        public string s_OffLandLine { get; set; }
        public string s_PermLandLine { get; set; }
        public string s_NRILandLine { get; set; }

        public string s_CommFax { get; set; }
        public string s_CoFax { get; set; }
        public string s_POAFax { get; set; }
        public string s_OffFax { get; set; }
        public string s_PermFax { get; set; }
        public string s_NRIFax { get; set; }

        public int i_ChildGender { get; set; }

        public int i_SourceProjNameId { get; set; }
        public int i_SourceNameId { get; set; }
        public int i_SubSourceId { get; set; }

        public int i_PersonalReligion { get; set; }
        public int i_CoAppReligion { get; set; }
        public int i_POAReligion { get; set; }

        public int i_PersonalNRI { get; set; }

        public string i_CommCountry { get; set; }
        public string i_CoCountry { get; set; }
        public string i_POACountry { get; set; }
        public string i_OffCountry { get; set; }
        public string i_NRICountry { get; set; }
        public string i_PermCountry { get; set; }

        public int i_ChklstFacility { get; set; }
        public int i_ChklstArea { get; set; }
        public int i_ReqNeed { get; set; }
        public int i_ReqProjBlock { get; set; }

        public int i_FinEmpStatusId { get; set; }
        public int i_FinIncomeId { get; set; }

        public int i_FinApartmentNameId { get; set; }
        public int i_FinApartmentTypeId { get; set; }
        public int i_FinApartmentSizeId { get; set; }
        public int i_FinApartmentCostId { get; set; }

        public int i_FinStayId { get; set; }
        public int i_FinGuestHouseId { get; set; }
        public int i_ChklstPossId { get; set; }
        public int i_FinProjBlockId { get; set; }
        public int i_FinBrokerId { get; set; }

        public DateTime DE_LeadDate { get; set; }
        public DateTime DE_NextCallDate { get; set; }

        public DateTime DE_CoAppDOB { get; set; }
        public DateTime DE_CoAppWeddingDate { get; set; }

        public DateTime DE_POADOB { get; set; }
        public DateTime DE_POAWeddingDate { get; set; }

        public DateTime DE_PersonalDOB { get; set; }
        public DateTime DE_PersonalWeddingDate { get; set; }

        public DateTime DE_ChildDOB { get; set; }
        public DateTime DE_ExeFromDate { get; set; }
        public DateTime DE_ExeEndDate { get; set; }

        public int b_FacSel { get; set; }
        public int b_AreaSel { get; set; }
        public int b_PossSel { get; set; }

        public string s_CoAppPanNo { get; set; }
        public string s_POAPanNo { get; set; }
        public string s_OffPanNo { get; set; }
        public string s_PermPanNo { get; set; }
        public string s_NRIPanNo { get; set; }
        public string s_CommPanNo { get; set; }

        public string s_NRIPassportNo { get; set; }
        public string s_CommPassportNo { get; set; }



        public int EntryID { set; get; }
        public int ProjID { set; get; }
        public int FlatID { set; get; }
        public int EnquiryID { set; get; }
        public int Flag { set; get; }
        public int ExecutiveID { set; get; }
        public DateTime TrnDate { set; get; }
        public string CallFF { set; get; }
        public string BuyerName { set; get; }
        public int StatusId { set; get; }
        public int NatureID { set; get; }
        public int CallTypeID { set; get; }
        public DateTime NextCallDate { set; get; }
        public string Remarks { set; get; }
        public int ReminderId { set; get; }
        public string Reminder { set; get; } 
        public int BrokerId { get; set; }
        public decimal CommPer { get; set; }
        public decimal CommAmt { get; set; }
        public string CallType { get; set; }

        #endregion
    }
}
