using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MMS.DataLayer;

namespace MMS.BusinessLayer
{
    class CommonBL
    {
        internal static DataTable Get_Vendor()
        {
            return CommonDL.Get_Vendor();
        }

        internal static DataTable Get_CostCentre()
        {
            return CommonDL.Get_CostCentre();
        }

        internal static DataTable Get_PurchaseType()
        {
            return CommonDL.Get_PurchaseType();
        }

        internal static int Get_VendorType()
        {
            return CommonDL.Get_VendorType();
        }

        internal static string Get_CompanyDB(int arg_iCompanyId, DateTime arg_dPVDate)
        {
            return CommonDL.Get_CompanyDB(arg_iCompanyId, arg_dPVDate);
        }

        internal static int Get_SubLedgerType(string arg_sType)
        {
            return CommonDL.Get_SubLedgerType(arg_sType);
        }

        internal static int Get_ResourceSubLedger(int arg_iResId, ref int arg_iResTypeId)
        {
            return CommonDL.Get_ResourceSubLedger(arg_iResId, ref arg_iResTypeId);
        }

    }
}
