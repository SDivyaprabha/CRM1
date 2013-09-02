using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CRM.DataLayer;
using CRM.BusinessObjects;

namespace CRM.BusinessLayer
{
    class PMSReceiptBL
    {
        public static DataTable GetCostCentre()
        {
            return PMSReceiptDL.GetCostCentre();
        }

        public static DataTable GetFlat(int argCCId)
        {
            return PMSReceiptDL.GetFlat(argCCId);
        }

        public static DataTable GetSchedule(int argFlatId)
        {
            return PMSReceiptDL.GetSchedule(argFlatId);
        }

        public static void InsertReceipt(PMSReceiptDetailBO OPMSDetails, DataTable argdt)
        {
            PMSReceiptDL.InsertReceipt(OPMSDetails, argdt);
        }

        public static void UpdateReceipt(PMSReceiptDetailBO OPMSDetails, DataTable argdt)
        {
            PMSReceiptDL.UpdateReceipt(OPMSDetails, argdt);
        }

        public static DataTable GetReceiptRegister()
        {
            return PMSReceiptDL.GetReceiptRegister();
        }

        public static DataSet GetReceiptDetE(int argReceiptId)
        {
            return PMSReceiptDL.GetReceiptDetE(argReceiptId);
        }

        public static void DeleteReceiptDetails(int argRecpId)
        {
            PMSReceiptDL.DeleteReceiptDetails(argRecpId);
        }

    }
}
