using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DataLayer;

namespace CRM.BusinessLayer
{
    class AllotBL
    {
        public AllotBL()
        {
            
        }

        public static DataSet GetAllot(int argCCId, DateTime argFrom, DateTime argTo)
        {
            return AllotDL.GetAllot(argCCId, argFrom, argTo);
        }

        public static DataTable GetAllotProject()
        {
            return AllotDL.GetAllotProject();
        }

        public static DataTable GetAllotExecutive(int argCCId)
        {
            return AllotDL.GetAllotExecutive(argCCId);
        }

        public static void Update_Allot(int argExeId, int argLeadId, int argCCId)
        {
            AllotDL.Update_Allot(argExeId, argLeadId, argCCId);
        }

        public static void Update_MulAllot(int argExeId, int argLeadId, int argCCId)
        {
            AllotDL.Update_MulAllot(argExeId, argLeadId, argCCId);
        }

        public static void UpdateMultipleAllot(int argExeId, int argCCId,string argType)
        {
            AllotDL.UpdateMultipleAllot(argExeId, argCCId,argType);
        }

        public static int GetExecutiveId(int argCCId)
        {
            return AllotDL.GetExecutiveId(argCCId);
        }

        public static DataTable GetExecutive(int argCCId)
        {
            return AllotDL.GetExecutive(argCCId);
        }

        public static DataTable GetCostCentre()
        {
            return AllotDL.GetCostCentre();
        }

        public static DataTable GetReserveFlats(int argCCID)
        {
            return AllotDL.GetReserveFlats(argCCID);
        }
        public static DataTable GetCCId(int argLeadID)
        {
            return AllotDL.GetCCId(argLeadID);
        }
        public static DataTable GetCCwiseExecutiveId(int argCCId)
        {
            return AllotDL.GetCCwiseExecutiveId(argCCId);
        }
        public static DataTable GetMultiCCwiseExecutiveId(string argCCId)
        {
            return AllotDL.GetMultiCCwiseExecutiveId(argCCId);
        }
        public static void InsertReserveFlats(int argCCId, string argFlatIds)
        {
            AllotDL.InsertReserveFlats(argCCId, argFlatIds);
        }
        public static DataTable GetPaySchFlat(int argFlatId)
        {
            return AllotDL.GetPaySchFlat(argFlatId);
        }
        public static void InsertAllotment(int argCCId, int argFlatId, int argBuyerId, decimal argNetAmt, decimal argPaidAmt, decimal argPenaltyAmt, decimal argBalAmt, DateTime argDate,
            string argCancelType, string argRemarks, int argBlockId, string argFlatNo, DataTable dtChk, bool argChkSend,string argCCName)
        {
            AllotDL.InsertAllotment(argCCId, argFlatId, argBuyerId,argNetAmt,argPaidAmt, argPenaltyAmt,argBalAmt, argDate, argCancelType, argRemarks, argBlockId, argFlatNo, dtChk, argChkSend, argCCName);
        }

        internal static DataSet GetBulkAllot()
        {
            return AllotDL.GetBulkAllot();
        }

        internal static void UpdateBulkAllocation(int i_FromExeId, int i_ToExeId)
        {
            AllotDL.UpdateBulkAllocation(i_FromExeId, i_ToExeId);
        }
    }
}
