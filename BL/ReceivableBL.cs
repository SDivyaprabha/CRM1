using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DataLayer;

namespace CRM.BusinessLayer
{
    class ReceivableBL
    {
        public static DataSet Get_Receivable(DateTime arg_dAsOn, int arg_iBlockId, DateTime arg_dSchDate, bool arg_bSch, int arg_iCCId,string argBuyer)
        {
            return ReceivableDL.Get_Receivable(arg_dAsOn, arg_iBlockId, arg_dSchDate, arg_bSch, arg_iCCId,argBuyer);
        }

        public static DataSet Get_SchReceivable(DateTime arg_dAsOn, int arg_iBlockId, DateTime arg_dSchDate, bool arg_bSch, int arg_iCCId,int argPaySchId)
        {
            return ReceivableDL.Get_SchReceivable(arg_dAsOn, arg_iBlockId, arg_dSchDate, arg_bSch, arg_iCCId,argPaySchId);
        }

        public static DataTable PaymentSchType()
        {
            return ReceivableDL.PaymentSchType();
        }

    }
}
