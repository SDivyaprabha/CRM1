using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CRM.DL;
using CRM.BO;

namespace CRM.BL
{
    class TargetRegBL
    {
        #region Methods

        public static DataTable getTargetReg(string argFromDate, string argToDate)
        {
            return TargetRegDL.getTargetReg(argFromDate, argToDate);
        }

        public static DataTable getTargetTrans(TargetEntryBO TargetBO)
        {
            return TargetRegDL.getTargetTrans(TargetBO);
        }

        public static DataTable FillRegExec(int argTargetId)
        {
            return TargetRegDL.FillRegExec(argTargetId);
        }

        public static DataTable getIncen(TargetEntryBO TargetBO)
        {
            return TargetRegDL.getIncen(TargetBO);
        }

        public static DataTable getEditTarMas(TargetEntryBO TargetBO)
        {
            return TargetRegDL.getEditTarMas(TargetBO);
        }

        public static DataTable getEditTarTrans(TargetEntryBO TargetBO)
        {
            return TargetRegDL.getEditTarTrans(TargetBO);
        }

        public static DataTable getEditIncentiveTran(TargetEntryBO TargetBO)
        {
            return TargetRegDL.getEditIncentiveTran(TargetBO);
        }

        public static void DeleteReg(int i_TargetId)
        {
            TargetRegDL.DeleteReg(i_TargetId);
        }

        #endregion
    }
}
