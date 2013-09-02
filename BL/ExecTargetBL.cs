using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CRM.DataLayer;
using System.Data.SqlClient;
using CRM.BusinessObjects;

namespace CRM.BusinessLayer
{
    class ExecTargetBL
    {
        internal static DataTable GetCostCentre()
        {
            return ExecTargetDL.GetCostCentre();
        }

        internal static DataTable GetExecutive()
        {
            return ExecTargetDL.GetExecutive();
        }

        internal static DataTable GetEditExecutive(int argTargetId)
        {
            return ExecTargetDL.GetEditExecutive(argTargetId);
        }

        internal static DataTable GetEditTrans(int argTargetId)
        {
            return ExecTargetDL.GetEditTrans(argTargetId);
        }

        public static void InsertTarget(DataTable argdtA, DataTable argdtU, DataTable argdtI)
        {
            ExecTargetDL.InsertTarget(argdtA, argdtU, argdtI);
        }

        public static void UpdateTarget(int argTargetId, DataTable argdtA, DataTable argdtU, DataTable argdtI)
        {
            ExecTargetDL.UpdateTarget(argTargetId, argdtA, argdtU, argdtI);
        }

        internal static DataTable GetTargetReg(string argFromDate, string argToDate)
        {
            return ExecTargetDL.GetTargetReg(argFromDate, argToDate);
        }

        internal static void DeleteReg(int i_TargetId)
        {
            ExecTargetDL.DeleteReg(i_TargetId);
        }

        internal static DataTable GetEditTarMas(int argTargetId)
        {
            return ExecTargetDL.GetEditTarMas(argTargetId);
        }

        internal static DataTable GetTargetTrans(int argTargetId)
        {
            return ExecTargetDL.GetTargetTrans(argTargetId);
        }

        internal static DataTable GetIncen(int argTargetId)
        {
            return ExecTargetDL.GetIncen(argTargetId);
        }

        public static DataTable GetGridTarget(int argEntryId)
        {
            return ExecTargetDL.GetGridTarget(argEntryId);
        }
    }
}
