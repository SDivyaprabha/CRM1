using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.BusinessObjects;
using CRM.DataLayer;
using System.Data.SqlClient;

namespace CRM
{
    class LeadBL
    {
        #region Methods
        
        //public static DataTable GetProject(int argProjTypeId)
        //{
        //    return LeadDL.GetProject(argProjTypeId);
        //}

        public static DataTable GetCat()
        {
            return LeadDL.GetCat();
        }

        public static DataTable GetExecutiveList()
        {
            return LeadDL.GetExecutiveList();
        }

        public static DataTable FillProjType()
        {
            return LeadDL.FillProjType();       
        }

        public static DataTable GetCallType()
        {
            return LeadDL.GetCallType();       
        }

        public static DataTable GetBroker(int argCCId)
        {
            return LeadDL.GetBroker(argCCId);
        }

        //NewForm

        public static DataTable GetReligion()
        {
            return LeadDL.GetReligion();
        }

        public static DataTable GetCountry()
        {
            return LeadDL.GetCountry();
        }

        public static DataTable GetApartment()
        {
            return LeadDL.GetApartment();
        }

        public static DataTable GetApartmentSize()
        {
            return LeadDL.GetApartmentsize();
        }

        public static DataTable GetApartmentType()
        {
            return LeadDL.GetApartmentType();
        }

        public static DataTable GetCostPref()
        {
            return LeadDL.GetCostPreference();
        }

        public static DataTable GetFacilityMaster()
        {
            return LeadDL.GetFacilityMaster();
        }

        public static DataTable GetAreaMaster()
        {
            return LeadDL.GetAreaMaster();
        }

        public static DataTable GetProject()
        {
            return LeadDL.GetProject();
        }

        public static DataTable GetNature()
        {
            return LeadDL.GetNature();
        }

        public static DataTable GetSource()
        {
            return LeadDL.GetSource();
        }

        public static DataTable GetSubSource()
        {
            return LeadDL.GetSubSource();
        }

        public static DataTable GetExecutive()
        {
            return LeadDL.GetExecutive();
        }

        public static DataTable GetEmpStatus()
        {
            return LeadDL.GetEmpStatus();
        }

        public static DataTable GetGuestHouse()
        {
            return LeadDL.GetGuestHouse();
        }

        public static DataTable GetStay()
        {
            return LeadDL.GetStay();
        }

        public static DataTable GetPossessMaster()
        {
            return LeadDL.GetPossessMaster();
        }

        public static DataTable GetIncome()
        {
            return LeadDL.GetIncome();
        }

        public static DataTable GetFacility()
        {
            return LeadDL.GetFacility();
        }

        public static DataTable GetArea()
        {
            return LeadDL.GetArea();
        }

        public static DataTable GetPossess()
        {
            return LeadDL.GetPossess();
        }

        internal static DataTable GetBusinessType(int argCCId)
        {
            return LeadDL.GetBusinessType(argCCId);
        }

        internal static DataSet ShowProjectGrid(int argCCId)
        {
            return LeadDL.ShowProjectGrid(argCCId);
        }

        internal static DataSet ShowRegisterProjectGrid(int argCCId, int argLeadId)
        {
            return LeadDL.ShowRegisterProjectGrid(argCCId,argLeadId);
        }

        #endregion

        #region InsertData

        public static void InsertFacility(LeadBO oLeedBO, string s_Mode, SqlConnection conn, SqlTransaction tran)
        {
            LeadDL.InsertFacility(oLeedBO, s_Mode,conn,tran);
        }

        public static void InsertArea(LeadBO oLeedBO, string s_Mode, SqlConnection conn, SqlTransaction tran)
        {
            LeadDL.InsertArea(oLeedBO, s_Mode,conn,tran);
        }

        public static int InsertLeadDetails(LeadBO oLeedBO, string s_Mode, DataTable dtEnqTrans,bool UpdateLead,SqlConnection conn,SqlTransaction tran,DataTable dtFinal,string argFlatNo,bool argChk)
        {
            return LeadDL.InsertLeadDetails(oLeedBO, s_Mode, dtEnqTrans, UpdateLead,conn,tran,dtFinal,argFlatNo,argChk);
        }

        public static int InsertMultipleLeadDetails(LeadBO oLeedBO, string s_Mode, DataTable dtEnqTrans, bool UpdateLead, SqlConnection conn, SqlTransaction tran, DataTable dtFinal, string argFlatNo, bool argChk,DataTable dtProj)
        {
            return LeadDL.InsertMultipleLeadDetails(oLeedBO, s_Mode, dtEnqTrans, UpdateLead, conn, tran, dtFinal, argFlatNo, argChk,dtProj);
        }

        internal static int InsertPlotLeadDetails(LeadBO oLeedBO, string s_Mode, DataTable dtEnqTrans, bool UpdateLead, SqlConnection conn, SqlTransaction tran, DataTable dtFinal, string argFlatNo, bool argChk,DataTable dtLand)
        {
            return LeadDL.InsertPlotLeadDetails(oLeedBO, s_Mode, dtEnqTrans, UpdateLead, conn, tran,dtFinal,argFlatNo,argChk,dtLand);
        }

        internal static int InsertMultiplePlotLeadDetails(LeadBO oLeedBO, string s_Mode, DataTable dtEnqTrans, bool UpdateLead, SqlConnection conn, SqlTransaction tran, DataTable dtFinal, string argFlatNo, bool argChk, DataTable dtLand,DataTable dtProj)
        {
            return LeadDL.InsertMultiplePlotLeadDetails(oLeedBO, s_Mode, dtEnqTrans, UpdateLead, conn, tran, dtFinal, argFlatNo, argChk, dtLand,dtProj);
        }

        public static void InsertProjectInformation(LeadBO oLeedBO, string s_Mode, bool argUpdateLead,SqlConnection conn,SqlTransaction tran)
        {
            LeadDL.InsertProjectInformation(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertMultipleProjectInformation(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran,DataTable dtProj)
        {
            LeadDL.InsertMultipleProjectInformation(oLeedBO, s_Mode, argUpdateLead, conn, tran,dtProj);
        }

        public static void InsertExeInformation(LeadBO oLeedBO, string s_Mode, bool argUpdateLead,SqlConnection conn,SqlTransaction tran)
        {
            LeadDL.InsertExeInformation(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertMultipleExeInformation(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran,DataTable dtProj)
        {
            LeadDL.InsertMultipleExeInformation(oLeedBO, s_Mode, argUpdateLead, conn, tran, dtProj);
        }

        public static void InsertPersonalInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            LeadDL.InsertPersonalInfo(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertCoAppInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            LeadDL.InsertCoAppInfo(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }
        internal static void InsertPOAInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            LeadDL.InsertPOAInfo(oLeedBO, s_Mode, argUpdateLead, conn, tran);
        }
        public static void InsertCommAddInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead,SqlConnection conn,SqlTransaction tran)
        {
            LeadDL.InsertCommAddInfo(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertOffAddInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            LeadDL.InsertOffAddInfo(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertPermAddInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            LeadDL.InsertPermAddInfo(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertNRIAddInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            LeadDL.InsertNRIAddInfo(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertCoAppAddInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            LeadDL.InsertCoAppAddInfo(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }
        internal static void InsertPOAAddInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            LeadDL.InsertPOAAddInfo(oLeedBO, s_Mode, argUpdateLead, conn, tran);
        }
        public static void InsertSourceInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead)
        {
            LeadDL.InsertSourceInfo(oLeedBO, s_Mode,argUpdateLead);
        }

        public static void InsertSubSourceInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead)
        {
            LeadDL.InsertSubSourceInfo(oLeedBO, s_Mode,argUpdateLead);
        }

        public static void InsertChildInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            LeadDL.InsertChildInfo(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertApartmentInfo(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            LeadDL.InsertApartmentInfo(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertRequirement(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            LeadDL.InsertRequirement(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertFinance(LeadBO oLeedBo, string s_Mode, bool argUpdateLead,SqlConnection conn,SqlTransaction tran)
        {
            LeadDL.InsertFinance(oLeedBo, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertPossess(LeadBO oLeedBo, string s_Mode, SqlConnection conn, SqlTransaction tran)
        {
            LeadDL.InsertPossess(oLeedBo, s_Mode,conn,tran);
        }

        public static void InsertBankDet(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            LeadDL.InsertBankDet(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        public static void InsertNRIContDet(LeadBO oLeedBO, string s_Mode, bool argUpdateLead, SqlConnection conn, SqlTransaction tran)
        {
            LeadDL.InsertNRIContDet(oLeedBO, s_Mode,argUpdateLead,conn,tran);
        }

        internal static void Insert_BuyerAlert(int argLeadID, string argProjName, string argType)
        {
            LeadDL.Insert_BuyerAlert(argLeadID,argProjName,argType);
        }

        #endregion

        #region Register Function

        internal static DataTable LeadLogin(int argLeadId)
        {
            return LeadDL.LeadLogin(argLeadId);
        }

        internal static bool LeadFound(int argLeadId)
        {
            return LeadDL.LeadFound(argLeadId);
        }

        internal static DataTable ShowPOAInfo(LeadBO e_leadbo)
        {
            return LeadDL.ShowPOAInfo(e_leadbo);
        }

        internal static DataTable ShowPOAAddInfo(LeadBO e_leadbo)
        {
            return LeadDL.ShowPOAAddInfo(e_leadbo);
        }

        public static DataTable ShowUser()
        {
            return LeadDL.ShowUser();
        }

        public static DataTable ShowFacility(LeadBO e_leadbo)
        {
            return LeadDL.ShowFacility(e_leadbo);
        }

        public static DataTable ShowArea(LeadBO e_leadbo)
        {
            return LeadDL.ShowArea(e_leadbo);
        }

        public static DataTable ShowPossess(LeadBO e_leadbo)
        {
            return LeadDL.ShowPossess(e_leadbo);
        }

        public static DataTable ShowLeadDetails(LeadBO e_leadbo)
        {
            return LeadDL.ShowLeadDetails(e_leadbo);
        }

        public static DataTable ShowLeadName()
        {
            return LeadDL.ShowLeadName();
        }

        public static DataTable ShowApartInfo(LeadBO e_leadbo)
        {
            return LeadDL.ShowApartInfo(e_leadbo);
        }

        public static DataTable ShowChildInfo(LeadBO e_leadbo)
        {
            return LeadDL.ShowChildInfo(e_leadbo);
        }

        public static DataTable ShowCoAppAddInfo(LeadBO e_leadbo)
        {
            return LeadDL.ShowCoAppAddInfo(e_leadbo);
        }

        public static DataTable ShowCoAppInfo(LeadBO e_leadbo)
        {
            return LeadDL.ShowCoAppInfo(e_leadbo);
        }

        public static DataTable ShowCommAddInfo(LeadBO e_leadbo)
        {
            return LeadDL.ShowCommAddInfo(e_leadbo);
        }

        public static DataTable ShowExeInfo(LeadBO e_leadbo)
        {
            return LeadDL.ShowExeInfo(e_leadbo);
        }

        public static DataTable ShowFinance(LeadBO e_leadbo)
        {
            return LeadDL.ShowFinance(e_leadbo);
        }

        public static DataTable ShowRequirement(LeadBO e_leadbo)
        {
            return LeadDL.ShowRequirement(e_leadbo);
        }

        public static DataTable ShowNRIAddInfo(LeadBO e_leadbo)
        {
            return LeadDL.ShowNRIAddInfo(e_leadbo);
        }

        public static DataTable ShowOffAddInfo(LeadBO e_leadbo)
        {
            return LeadDL.ShowOffAddInfo(e_leadbo);
        }

        public static DataTable ShowPermAddInfo(LeadBO e_leadbo)
        {
            return LeadDL.ShowPermAddInfo(e_leadbo);
        }

        public static DataTable ShowPersonalInfo(LeadBO e_leadbo)
        {
            return LeadDL.ShowPersonalInfo(e_leadbo);
        }

        public static DataTable ShowProjectInfo(LeadBO e_leadbo)
        {
            return LeadDL.ShowProjectInfo(e_leadbo);
        }

        public static DataTable ShowSource(LeadBO e_leadbo)
        {
            return LeadDL.ShowSource(e_leadbo);
        }

        public static DataTable ShowLeadType(LeadBO e_leadbo)
        {
            return LeadDL.ShowLeadType(e_leadbo);
        }

        public static DataTable ShowNRIContDet(LeadBO e_leadbo)
        {
            return LeadDL.ShowNRIContDet(e_leadbo);
        }

        public static DataTable ShowBankDet(LeadBO e_leadbo)
        {
            return LeadDL.ShowBankDet(e_leadbo);
        }

        internal static DataTable ShowLeadDate(string argFromDate, string argToDate, bool argOtherExec)
        {
            return LeadDL.ShowLeadDate(argFromDate, argToDate, argOtherExec);
        }

        public static DataTable GetCompanyMailDetails()
        {
            return LeadDL.GetCompanyMailDetails();
        }

        internal static void InsertEmailSent(string argEmail, string argMobile, int argLeadId, string argSub)
        {
            LeadDL.InsertEmailSent(argEmail, argMobile, argLeadId, argSub);
        }

        internal static DataTable MobileNo_Found(string argMobileNo)
        {
            return LeadDL.MobileNo_Found(argMobileNo);
        }

        internal static bool Sold_LeadFound(int argLeadId)
        {
            return LeadDL.Sold_LeadFound(argLeadId);
        }

        #endregion

        public static void DelFacility(int argLeadId, SqlConnection conn, SqlTransaction tran)
        {
            LeadDL.DelFacility(argLeadId,conn,tran);
        }

        public static void DelArea(int argLeadId, SqlConnection conn, SqlTransaction tran)
        {
            LeadDL.DelArea(argLeadId,conn,tran);
        }

        public static void DelPoss(int argLeadId, SqlConnection conn, SqlTransaction tran)
        {
            LeadDL.DelPoss(argLeadId,conn,tran);
        }

        internal static int GetBuyerEntryId(int argLeadId)
        {
            return LeadDL.GetBuyerEntryId(argLeadId);
        }

        internal static void InsertLeadCheckList(string[] argFields)
        {
            LeadDL.InsertLeadCheckList(argFields);
        }

        internal static DataTable GetLeadCheckList()
        {
            return LeadDL.GetLeadCheckList();
        }

        internal static void UpdateLeadCheckList(DataTable argdt)
        {
            LeadDL.UpdateLeadCheckList(argdt);
        }

        internal static DataTable PopulateCustomerFeedback()
        {
            return LeadDL.PopulateCustomerFeedback();
        }
    }
}
