using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DataLayer;

namespace CRM.BusinessLayer
{
    class AvailabilityBL
    {
        CRM.DataLayer.AvailabilityDL m_oAvail;
        public AvailabilityBL()
        {
            m_oAvail = new DataLayer.AvailabilityDL();
        }
        public DataTable GetFlatDetails(int argCCId, string argType, int argLandId,string argBlockId)
        {
            return m_oAvail.GetFlatDetails(argCCId,argType,argLandId,argBlockId);
        }
        public DataTable GetCostCentreList()
        {
            return m_oAvail.GetCostCentreList();
        }
        public DataTable GetBlockList(int argCCId)
        {
            return m_oAvail.GetBlockList(argCCId);
        }
        public static DataTable GetFlatVDetails(int argCCId, int argFlatId, string argType, int argLandId)
        {
            return  AvailabilityDL.GetFlatVDetails(argCCId, argFlatId,argType,argLandId);
        }
        public static DataTable GetFlatTrans(int argFlatId,string argType)
        {
            return AvailabilityDL.GetFlatTrans(argFlatId,argType);
        }
        public static DataTable GetPlotTrans(int argFlatId, string argType)
        {
            return AvailabilityDL.GetPlotTrans(argFlatId, argType);
        }
        public static DataTable GetProjectTrans(int argCCId, string argType)
        {
            return AvailabilityDL.GetProjectTrans(argCCId, argType);
        }
    }
}
