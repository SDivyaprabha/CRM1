using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CRM.BO;
using CRM.DL;

namespace CRM.BL
{
    class TargetEntryBL
    {
        #region Methods

        public static int InsertTargetMaster(string s_Mode, TargetEntryBO TarEntBO)
        {
            return TargetEntryDL.InsertTargetMaster(s_Mode, TarEntBO);
        }

        public static void InsertTargetTrans(string s_Mode, TargetEntryBO TarEntBO)
        {
            TargetEntryDL.InsertTargetTrans(s_Mode, TarEntBO);
        }

        public static void InsertIncentiveTrans(string s_Mode, DataTable dt, TargetEntryBO TargetBO)
        {
            TargetEntryDL.InsertIncentiveTrans(s_Mode, dt,TargetBO);
        }

        public static DataTable GetCostCentre()
        {
            return TargetEntryDL.GetCostCentre();
        }

        internal static DataTable GetExecutive(string argId)
        {
            return TargetEntryDL.GetExecutive(argId);
        }

        internal static bool NoofPerFound(int argExecId, int argNoofper, int argCCId)
        {
            return TargetEntryDL.NoofPerFound(argExecId, argNoofper, argCCId);
        }

        public static DataSet GetProjectReport()
        {
            return TargetEntryDL.GetProjectReport();
        }
        public static DataSet GetExecReport(int argCCId,DateTime argFrom)
        {
            return TargetEntryDL.GetExecReport(argCCId,argFrom);
        }
        public static DataSet GetExecDESReport(DateTime argDate)
        {
            return TargetEntryDL.GetExecDESReport(argDate);
        }
        public static DataSet GetPerfAnalysis(DateTime argAsOnDate, DateTime argFromDate, DateTime argToDate,string argType)
        {
            return TargetEntryDL.GetPerfAnalysis(argAsOnDate, argFromDate, argToDate, argType);
        }
        public static DataSet GetProjectAnalysis(int argExecId, DateTime argAsOnDate, DateTime argFromDate, DateTime argToDate, string argType)
        {
            return TargetEntryDL.GetProjectAnalysis(argExecId,argAsOnDate, argFromDate, argToDate, argType);
        }
        public static DataTable GetSoldUnits(int argExecId, int argCCId, DateTime argAsOnDate, DateTime argFromDate, DateTime argToDate, string argType)
        {
            return TargetEntryDL.GetSoldUnits(argExecId, argCCId, argAsOnDate, argFromDate, argToDate, argType);
        }

        internal static bool PowerUserFound()
        {
            return TargetEntryDL.PowerUserFound();
        }
        #endregion
    }
}
