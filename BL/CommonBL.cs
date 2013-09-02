using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CRM.DataLayer;

namespace CRM.BusinessLayer
{
    class CommonBL
    {
        internal static int Get_Buyer_SL(int arg_iBuyerId)
        {
            return CommonDL.Get_Buyer_SL(arg_iBuyerId);
        }

        internal static DataTable Get_CostCentre()
        {
            return CommonDL.Get_CostCentre();
        }

        internal static DataTable Get_Income()
        {
            return CommonDL.Get_Income();
        }

        internal static int Get_BuyerType()
        {
            return CommonDL.Get_BuyerType();
        }

        internal static string Get_CompanyDB(int arg_iCompanyId, DateTime arg_dPVDate)
        {
            return CommonDL.Get_CompanyDB(arg_iCompanyId, arg_dPVDate);
        }

        internal static int Get_SubLedgerType(string arg_sType)
        {
            return CommonDL.Get_SubLedgerType(arg_sType);
        }

        internal static DataTable Get_AllLead()
        {
            return CommonDL.Get_AllLead();
        }

        internal static DataTable Get_AllFlat_Unsold()
        {
            return CommonDL.Get_AllFlat_Unsold();
        }

        internal static DataTable Get_AllBlock()
        {
            return CommonDL.Get_AllBlock();
        }

        internal static DataTable Get_AllPaySchType()
        {
            return CommonDL.Get_AllPaySchType();
        }
    }
}
