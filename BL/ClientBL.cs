using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DataLayer;

namespace CRM.BusinessLayer
{
    class ClientBL
    {

        public static DataSet GetProsAndBuyer(int argLCCId, int argBCCId, string argCCId)
        {
            return ClientDL.GetProsAndBuyer(argLCCId, argBCCId, argCCId);
        }

        public static DataTable GetProsDetails(int argEnqId)
        {
            return ClientDL.GetProsDetails(argEnqId);
        }

        public static DataSet GetContactDetails(int argEnqId)
        {
            return ClientDL.GetContactDetails(argEnqId);
        }

        public static DataTable GetFlatDetails(int argEnqId, int argFlatId)
        {
            return ClientDL.GetFlatDetails(argEnqId, argFlatId);
        }

        public static DataTable GetFollowupDetails(int argEnqId)
        {
            return ClientDL.GetFollowupDetails(argEnqId);
        }
        public static DataTable GetPostFollowupDetails(int argEnqId)
        {
            return ClientDL.GetPostFollowupDetails(argEnqId);
        }
    }
}
