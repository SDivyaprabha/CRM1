using System;
using System.Collections.Generic;
using System.Data;
using CRM.DataLayer;
using CRM.BusinessObjects;

namespace CRM.BusinessLayer
{
    public class CallSheetEntryBL
    {       
      
        #region Object
        CallsheetentryDL callsheetDL;
        #endregion

        #region Constructor

        public CallSheetEntryBL()
        {
            callsheetDL = new CallsheetentryDL();
        }
        #endregion

        #region Methods   

        public DataSet GetData()
        {
            return callsheetDL.GetData();
        }

        public static DataTable GetRegisterCall()
        {
            return CallsheetentryDL.GetRegisterCall();
        }
        public static DataTable GetNextCallReport()
        {
            return CallsheetentryDL.GetNextCallReport();
        }
        public static DataTable GetNextCallReportPost()
        {
            return CallsheetentryDL.GetNextCallReportPost();
        }
        public static DataTable GetFromRegisterCall(string argFromDate, string argToDate,bool argOtherExec)
        {
            return CallsheetentryDL.GetFromRegisterCall(argFromDate, argToDate,argOtherExec);
        }
        public static DataTable GetFromEditRegisterCall(int argRegId)
        {
            return CallsheetentryDL.GetFromEditRegisterCall(argRegId);
        }
        public static DataTable GetGridCallSheet(int argEntryId)
        {
            return CallsheetentryDL.GetGridCallSheet(argEntryId);
        }
        public static DataTable GetOtherReport(string argFromDate, string argToDate, bool argSel)
        {
            return CallsheetentryDL.GetOtherReport(argFromDate, argToDate,argSel);
        }
        public static bool GetOtherExecCall()
        {
            return CallsheetentryDL.GetOtherExecCall();
        }
        public static DataTable GetOtherReportPost(string argFromDate, string argToDate, bool argSel)
        {
            return CallsheetentryDL.GetOtherReportPost(argFromDate, argToDate,argSel);
        }
        public static DataTable GetProject()
        {
            return CallsheetentryDL.GetProject();            
        }
        public static DataTable GetCallSheetProject()
        {
            return CallsheetentryDL.GetCallSheetProject();  
        }

        //public void Update(SqlConnection Con)
        //{
        //    callsheetDL.Update(this );

        //}
        public static DataTable FillLead(string m_sType, bool m_bOtherExec)
        {
            return CallsheetentryDL.FillLead(m_sType, m_bOtherExec);
        }
        public int InsertCallSheet(CallSheetEntryMasterBO argCallSheet, string argmode, DataTable dtEnqTrans, DataTable dtFinal, string argFlatNo, bool argChkSend)
        {
            return callsheetDL.InsertCallSheet(argCallSheet, argmode, dtEnqTrans, dtFinal, argFlatNo, argChkSend);
        }
        public int InsertPlotCallSheet(CallSheetEntryMasterBO argCallSheet, string argmode, DataTable dtEnqTrans, DataTable dtFinal, string argFlatNo, bool argChkSend,DataTable dtLand,int argLandId)
        {
            return callsheetDL.InsertPlotCallSheet(argCallSheet, argmode, dtEnqTrans, dtFinal, argFlatNo, argChkSend,dtLand,argLandId);
        }
        public static DataTable GetCall()
        {
            return CallsheetentryDL.GetCall();            
        }
        public static DataTable GetEditRegisterCall(int argEntryId)
        {
            return CallsheetentryDL.GetEditRegisterCall(argEntryId);  
        }
        public static DataTable GetEditRegisterBuyerDet(int argEntryId)
        {
            return CallsheetentryDL.GetEditRegisterBuyerDet(argEntryId);  
        }
        public static DataTable GetComments(int argLeadId)
        {
            return CallsheetentryDL.GetComments(argLeadId);   
        }
        public static DataTable GetCampaign()
        {
            return CallsheetentryDL.GetCampaign();  
        }
        public static bool GetExecCall(int argLeadId)
        {
            return CallsheetentryDL.GetExecCall(argLeadId);           
        }

        public static int InsertBuyerDetail(DataTable dtEnqTrans)
        {
            return CallsheetentryDL.InsertBuyerDetail(dtEnqTrans); 
        }

        public static bool CallSheetFound(int argLeadId, int argEntryId)
        {
            return CallsheetentryDL.CallSheetFound(argLeadId,argEntryId); 
        }

        public static int GetCompanyId(int argCCId)
        {
            return CallsheetentryDL.GetCompanyId(argCCId); 
        }

        #endregion

        #region PostSale CallType

        public static DataTable GetCallMaster()
        {
            return CallsheetentryDL.GetCallMaster();
        }
        public static void InsertCallTypeMaster(string argDesc)
        {
            CallsheetentryDL.InsertCallTypeMaster(argDesc);
        }
        public static void UpdateCallTypeMaster(string argDesc, int argTypeId)
        {
            CallsheetentryDL.UpdateCallTypeMaster(argDesc, argTypeId);
        }
        public static bool CallTypeMasterFound(int argTypeId)
        {
            return CallsheetentryDL.CallTypeMasterFound(argTypeId);
        }
        public static bool CallTypeSysFound(int argTypeId)
        {
            return CallsheetentryDL.CallTypeSysFound(argTypeId);
        }
        public static void DeleteCallTypeMaster(int argTypeId)
        {
            CallsheetentryDL.DeleteCallTypeMaster(argTypeId);
        }
        public static DataTable PostGetCall()
        {
            return CallsheetentryDL.PostGetCall();
        }
        public static DataTable PostGetCallEdit(string argFromDate, string argToDate,bool argSel)
        {
            return CallsheetentryDL.PostGetCallEdit(argFromDate, argToDate,argSel);
        }
        public static DataTable GetBuyer(int argCCId,string argsType)
        {
            return CallsheetentryDL.GetBuyer(argCCId, argsType);
        }
        public static DataTable GetFlat(int argCCId,string argsType)
        {
            return CallsheetentryDL.GetFlat(argCCId, argsType);
        }
        public static int GetBuyerId(int argCCId, string argsType, int argFlatId)
        {
            return CallsheetentryDL.GetBuyerId(argCCId, argsType, argFlatId);
        }
        public static int GetFlatId(int argCCId, string argsType, int argLeadId)
        {
            return CallsheetentryDL.GetFlatId(argCCId, argsType, argLeadId);
        }
        public static DataTable GetNature()
        {
            return CallsheetentryDL.GetNature();
        }
        public static void InsertFollowUp(string argsType)
        {
            CallsheetentryDL.InsertFollowUp(argsType);
        }
        public static void UpdateFollowUp(int argEntryId,string argsType)
        {
            CallsheetentryDL.UpdateFollowUp(argEntryId,argsType);
        }
        public static DataTable PostRegEdit(int argEntryId)
        {
            return CallsheetentryDL.PostRegEdit(argEntryId); 
        }
        public static DataTable PostRegChangeGrid(int argEntryId)
        {
            return CallsheetentryDL.PostRegChangeGrid(argEntryId); 
        }
        public static void DeleteFollowUp(int argEntryId)
        {
            CallsheetentryDL.DeleteFollowUp(argEntryId); 
        }
        #endregion

        #region Plot
        public static DataTable GetPlotType(int argCCId)
        {
            return CallsheetentryDL.GetPlotType(argCCId);
        }

        public static DataTable GetPlotDetails(int argCCId, string argMode, int argLeadId, string argType)
        {
            return CallsheetentryDL.GetPlotDetails(argCCId,argMode,argLeadId,argType);
        }
        #endregion

        #region Reminder

        public static DataTable GetReminder()
        {
            return CallsheetentryDL.GetReminder();
        }

        public static void InsertReminderMaster(string argDesc)
        {
            CallsheetentryDL.InsertReminderMaster(argDesc);
        }

        public static void UpdateReminderMaster(int argAreaId, string argDesc)
        {
            CallsheetentryDL.UpdateReminderMaster(argAreaId, argDesc);
        }

        public static void DeleteReminderMaster(int argAreaId)
        {
            CallsheetentryDL.DeleteReminderMaster(argAreaId);
        }

        public static bool FoundReminder(int argAreaId)
        {
            return CallsheetentryDL.FoundReminder(argAreaId);
        }

        #endregion


        internal static bool GetFinalisedFlat(int argEntryId, int argLeadId)
        {
            return CallsheetentryDL.GetFinalisedFlat(argEntryId, argLeadId);
        }
    }
}
