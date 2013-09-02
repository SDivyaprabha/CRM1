using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DataLayer;

namespace CRM.BusinessLayer
{
    class BrokerBL
    {
        public static DataTable getCostCentre(int argBrokerId)
        {
            return BrokerDL.getCostCentre(argBrokerId);
        }

        public static DataTable getBrokerCostCentre(int argBrokerId, int argVendorId)
        {
            return BrokerDL.getBrokerCostCentre(argBrokerId, argVendorId);
        }

        public static DataTable getVendor(int argBrokerId)
        {
            return BrokerDL.getVendor(argBrokerId);
        }

        public static DataTable getVendorDetails(int argBrokerId)
        {
            return BrokerDL.getVendorDetails(argBrokerId);
        }

        public static void UpdateBroker(int argBrokerId, int argVendorId, DataTable argdt)
        {
            BrokerDL.UpdateBroker(argBrokerId, argVendorId, argdt);
        }
    }
}
