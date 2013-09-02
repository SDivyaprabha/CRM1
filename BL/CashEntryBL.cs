using System;
using System.Collections.Generic;
using System.Linq;
using CRM.DataLayer;
using System.Data;

namespace CRM.BusinessLayer
{
    class CashEntryBL
    {
        public static DataTable GetCostCentre()
        {
            return CashEntryDL.GetCostCentre();
        }

        public static DataTable GetBuyer(int argCCId)
        {
            return CashEntryDL.GetBuyer(argCCId);
        }

        public static DataTable GetPayInfo(int argBuyerId,string argType)
        {
            return CashEntryDL.GetPayInfo(argBuyerId,argType);
        }

        public static void InsertCashDetails(DataTable dtPayInfo, int argCCId, int argBuyerId,decimal argAmt,DateTime argDate,string argType)
        {
            CashEntryDL.InsertCashDetails(dtPayInfo, argCCId, argBuyerId,argAmt,argDate,argType);
        }

        public static void UpdateCashDetails(int argCashRepId,DataTable dtPayInfo, int argCCId, int argBuyerId, decimal argAmt, DateTime argDate,string argType)
        {
            CashEntryDL.UpdateCashDetails(argCashRepId,dtPayInfo, argCCId, argBuyerId, argAmt, argDate,argType);
        }

        #region Register

        public static DataTable GetPayInfoRegister(DateTime argFrom, DateTime argTo)
        {
            return CashEntryDL.GetPayInfoRegister(argFrom, argTo);
        }

        public static DataTable GetPayInfoRegEntry(int argCashRecpId, int argLeadId,string argType)
        {
            return CashEntryDL.GetPayInfoRegEntry(argCashRecpId, argLeadId,argType);
        }

        public static DataTable GetChangeGridCashReceiptRegister(int argCashRecpId)
        {
            return CashEntryDL.GetChangeGridCashReceiptRegister(argCashRecpId);
        }

        #endregion

    }
}
