using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DataLayer;

namespace CRM.BusinessLayer
{
    class ProjReceivableBL
    {

        #region Projectwise Receivable

        internal static DataTable Get_Project_Receivable(DateTime arg_dAsOn)
        {
            return ProjReceivableDL.Get_Project_Receivable(arg_dAsOn);
        }

        internal static DataTable Get_Block_Receivable(int arg_iProjectId, DateTime arg_dtAsOn)
        {
            return ProjReceivableDL.Get_Block_Receivable(arg_iProjectId, arg_dtAsOn);
        }

        internal static DataTable Get_Flat_Receivable(int arg_CCId, int arg_iBlockId, DateTime arg_dtAsOn)
        {
            return ProjReceivableDL.Get_Flat_Receivable(arg_CCId,arg_iBlockId, arg_dtAsOn);
        }

        internal static DataTable Get_Flat_Receivable_WithInterest(int argCCId, int arg_iBlockId, DateTime arg_dtAsOn)
        {
            return ProjReceivableDL.Get_Flat_Receivable_WithInterest(argCCId, arg_iBlockId, arg_dtAsOn);
        }

        internal static DataTable Get_Flat_ReceivableReport(int argCCId, DateTime arg_dtAsOn)
        {
            return ProjReceivableDL.Get_Flat_ReceivableReport(argCCId,arg_dtAsOn);
        }

        #endregion

        #region Receivable Statement


        internal static DataTable Get_CC_RecStmt(DateTime argStart,DateTime argEnd)
        {
            return ProjReceivableDL.Get_CC_RecStmt(argStart,argEnd);
        }

        internal static DataTable Get_Block_RecStmt(int argCCId, DateTime argStart, DateTime argEnd)
        {
            return ProjReceivableDL.Get_Block_RecStmt(argCCId, argStart, argEnd);
        }

        internal static DataTable Get_Flat_RecStmt(int argCCId, int argBlockId, DateTime argStart, DateTime argEnd)
        {
            return ProjReceivableDL.Get_Flat_RecStmt(argCCId, argBlockId, argStart, argEnd);
        }

        internal static DataTable Get_CC_ActStmt(DateTime argStart, DateTime argEnd)
        {
            return ProjReceivableDL.Get_CC_ActStmt(argStart, argEnd);
        }

        internal static DataTable Get_Block_ActStmt(int argCCId, DateTime argStart, DateTime argEnd)
        {
            return ProjReceivableDL.Get_Block_ActStmt(argCCId, argStart, argEnd);
        }

        internal static DataTable Get_Flat_ActStmt(int argBlockId, DateTime argStart, DateTime argEnd)
        {
            return ProjReceivableDL.Get_Flat_ActStmt(argBlockId, argStart, argEnd);
        }

        internal static DataTable Get_Flat_RecStmtReport(int argCCId, DateTime argStart, DateTime argEnd)
        {
            return ProjReceivableDL.Get_Flat_RecStmtReport(argCCId, argStart, argEnd);
        }

        internal static DataTable GetFiscalYear()
        {
            return ProjReceivableDL.GetFiscalYear();
        }

        #endregion

        #region Fiscal Master

        internal static DataTable GetFiscalMaster()
        {
            return ProjReceivableDL.GetFiscalMaster();
        }

        internal static void InsertFiscalYear(DataTable argdt)
        {
            ProjReceivableDL.InsertFiscalYear(argdt);
        }

        #endregion

    }
}
