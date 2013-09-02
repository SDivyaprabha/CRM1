using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DL;
using CRM.BO;

namespace CRM.BL
{
    class FinalizationBL
    {
        #region Methods

        public static DataTable GetBrokerDetails(int argCCId)
        {
            return FinalizationDL.GetBrokerDetails(argCCId);
        }

        public static DataTable GetBankDetails()
        {
            return FinalizationDL.GetBankDetails();
        }

        public static DataTable GetFlatDetails(int argCCId, string argStatus)
        {
            return FinalizationDL.GetFlatDetails(argCCId, argStatus);
        }

        public static DataTable EditFinalization(int argiFlatId, int argCCId)
        {
            return FinalizationDL.EditFinalization(argiFlatId, argCCId);
        }

        public static void UpdateBuyerDet(string argMode, FinalizationBO BOFIN,DataTable dtFinal,string argFlatNo,bool argChkSend)
        {
            FinalizationDL.UpdateBuyerDet(argMode, BOFIN,dtFinal,argFlatNo,argChkSend);
        }

        #endregion
    }
}
