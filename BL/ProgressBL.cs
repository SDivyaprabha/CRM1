using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace CRM.BusinessLayer
{
    class ProgressBL
    {
        CRM.DataLayer.ProgressDL m_oProgress;

        public ProgressBL()
        {
            m_oProgress = new DataLayer.ProgressDL();
        }

        public DataTable GetCostCentreList()
        {
            return m_oProgress.GetCostCentreList();
        }
        public DataTable GetProjectCheckList(int argCCID)
        {
            return m_oProgress.GetProjectCheckList(argCCID);
        }
        public DataTable GetFinalCheckList(int argCCID)
        {
            return m_oProgress.GetFinalCheckList(argCCID);
        }
        public DataTable GetFlatType(int argCCId,string argType,int argLandId)
        {
            return m_oProgress.GetFlatType(argCCId,argType,argLandId);
        }

        public DataTable GetPlotProjectCheckList(int arglandId, string argTypeId, string argType)
        {
            return m_oProgress.GetPlotProjectCheckList(arglandId, argTypeId, argType);
        }

        public DataSet GetHandingCheckList(int argCCId, string argFlatTypeId,string argType)
        {
            return m_oProgress.GetHandingCheckList(argCCId, argFlatTypeId,argType);
        }
        public DataSet GetPlotHandingCheckList(int argLandId, string argFlatTypeId, string argType)
        {
            return m_oProgress.GetPlotHandingCheckList(argLandId, argFlatTypeId, argType);
        }
    }
}
