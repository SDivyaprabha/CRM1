using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CRM.DataLayer;

namespace CRM.BusinessLayer
{
    class MISBL
    {

        #region Projectwise Sales

        internal static DataTable Get_Project_Sales(DateTime argAsOnDate)
        {
            return MISDL.Get_Project_Sales(argAsOnDate);
        }

        internal static DataTable Get_Block_Sales(int arg_iProjectId, DateTime argAsOnDate)
        {
            return MISDL.Get_Block_Sales(arg_iProjectId, argAsOnDate);
        }

        internal static DataTable Get_Level_Sales(int arg_iProjectId, int arg_iBlockId, DateTime argAsOnDate)
        {
            return MISDL.Get_Level_Sales(arg_iProjectId, arg_iBlockId, argAsOnDate);
        }

        internal static DataTable Get_Flat_Sales(int arg_iProjectId, int arg_iBlockId, int m_iLevelId, DateTime argAsOnDate)
        {
            return MISDL.Get_Flat_Sales(arg_iProjectId, arg_iBlockId, m_iLevelId, argAsOnDate);
        }

        internal static DataTable Get_BWProject_Sales(string argFromDate, string argToDate)
        {
            return MISDL.Get_BWProject_Sales(argFromDate, argToDate);
        }

        internal static DataTable Get_BWBlock_Sales(int arg_iProjectId, string argFromDate, string argToDate)
        {
            return MISDL.Get_BWBlock_Sales(arg_iProjectId, argFromDate, argToDate);
        }

        internal static DataTable Get_BWLevel_Sales(int arg_iProjectId, int arg_iBlockId, string argFromDate, string argToDate)
        {
            return MISDL.Get_BWLevel_Sales(arg_iProjectId, arg_iBlockId, argFromDate, argToDate);
        }

        internal static DataTable Get_BWFlat_Sales(int arg_iProjectId, int arg_iBlockId, int arg_iLevelId, string argFromDate, string argToDate)
        {
            return MISDL.Get_BWFlat_Sales(arg_iProjectId, arg_iBlockId, arg_iLevelId, argFromDate, argToDate);
        }

        internal static DataTable GetCustomerPrint()
        {
            return MISDL.GetCustomerPrint();
        }

        internal static DataTable GetTypewiseSalesPrint()
        {
            return MISDL.GetTypewiseSalesPrint();
        }

        #endregion

        #region Projectwise Receivable

        internal static DataTable Get_Project_Receivable(DateTime arg_dAsOn)
        {
            return MISDL.Get_Project_Receivable(arg_dAsOn);
        }

        internal static DataTable Get_Block_Receivable(int arg_iProjectId, DateTime arg_dtAsOn)
        {
            return MISDL.Get_Block_Receivable(arg_iProjectId, arg_dtAsOn);
        }

        internal static DataTable Get_Flat_Receivable(int arg_CCId, int arg_iBlockId, DateTime arg_dtAsOn)
        {
            return MISDL.Get_Flat_Receivable(arg_CCId, arg_iBlockId, arg_dtAsOn);
        }

        internal static DataTable Get_Flat_Receivable_WithInterest(int argCCId, int arg_iBlockId, DateTime arg_dtAsOn)
        {
            return MISDL.Get_Flat_Receivable_WithInterest(argCCId, arg_iBlockId, arg_dtAsOn);
        }

        internal static void UpdateFlatProjectReceivableDiscount(int argCCId, int argFlatId, decimal argNetAmount, decimal argWriteOff)
        {
            MISDL.UpdateFlatProjectReceivableDiscount(argCCId, argFlatId, argNetAmount, argWriteOff);
        }

        internal static DataTable Get_Flat_ReceivableReport(int argCCId, DateTime arg_dtAsOn)
        {
            return MISDL.Get_Flat_ReceivableReport(argCCId, arg_dtAsOn);
        }

        #endregion

        #region Receivable Statement Tax

        internal static DataTable Get_CC_RecStmt_Tax(DateTime argStart, DateTime argEnd)
        {
            return MISDL.Get_CC_RecStmt_Tax(argStart, argEnd);
        }

        internal static DataTable Get_Block_RecStmt_Tax(int argCCId, DateTime argStart, DateTime argEnd)
        {
            return MISDL.Get_Block_RecStmt_Tax(argCCId, argStart, argEnd);
        }

        internal static DataTable Get_Flat_RecStmt_Tax(int argCCId, int argBlockId, DateTime argStart, DateTime argEnd)
        {
            return MISDL.Get_Flat_RecStmt_Tax(argCCId, argBlockId, argStart, argEnd);
        }

        internal static DataTable Get_CC_ActStmt(DateTime argStart, DateTime argEnd)
        {
            return MISDL.Get_CC_ActStmt(argStart, argEnd);
        }

        internal static DataTable Get_Block_ActStmt(int argCCId, DateTime argStart, DateTime argEnd)
        {
            return MISDL.Get_Block_ActStmt(argCCId, argStart, argEnd);
        }

        internal static DataTable Get_Flat_ActStmt(int argBlockId, DateTime argStart, DateTime argEnd)
        {
            return MISDL.Get_Flat_ActStmt(argBlockId, argStart, argEnd);
        }

        internal static DataTable Get_Flat_RecStmtReport(int argCCId, DateTime argStart, DateTime argEnd)
        {
            return MISDL.Get_Flat_RecStmtReport(argCCId, argStart, argEnd);
        }

        internal static DataTable GetFiscalYear()
        {
            return MISDL.GetFiscalYear();
        }

        internal static DataTable GetFlatPaymentInfo(int argCCId,int argBlockId,int argFlatId, DateTime argFrom, DateTime argTo, string argType)
        {
            return MISDL.GetFlatPaymentInfo(argCCId, argBlockId,argFlatId, argFrom, argTo, argType);
        }

        #endregion

        #region Stagewise Receivable

        public static DataTable GetProject()
        {
            return MISDL.GetProject();
        }

        public static DataTable GetBlock(int argCCId)
        {
            return MISDL.GetBlock(argCCId);
        }

        public static DataTable GetPayment(int argCCId)
        {
            return MISDL.GetPayment(argCCId);
        }

        public static DataSet GetProjectStageRec(int argCCId, int argPayTypeId, DateTime argDate, int argFromActual, string argBusinessType)
        {
            return MISDL.GetProjectStageRec(argCCId, argPayTypeId, argDate, argFromActual, argBusinessType);
        }

        public static DataSet GetBlockStageRec(int argCCId, int argPayTypeId, DateTime argDate, int argFromActual, string argBusinessType)
        {
            return MISDL.GetBlockStageRec(argCCId, argPayTypeId, argDate, argFromActual, argBusinessType);
        }

        public static DataSet GetBuyerStageRec(int argCCId, int argBlockId, int argPayTypeId, DateTime argDate, int argFromActual, string argBusinessType)
        {
            return MISDL.GetBuyerStageRec(argCCId, argBlockId, argPayTypeId, argDate, argFromActual, argBusinessType);
        }

        public static DataSet GetBuyerStageRecReport(int argCCId, int argPayTypeId, DateTime argDate, int argFromActual, string argBusinessType)
        {
            return MISDL.GetBuyerStageRecReport(argCCId, argPayTypeId, argDate, argFromActual, argBusinessType);
        }

        public static DataSet GetCCSOADet(int argCCId, DateTime argAsOn)
        {
            return MISDL.GetCCSOADet(argCCId, argAsOn);
        }

        public static DataSet GetBlockSOADet(int argBlockId, DateTime argAsOn)
        {
            return MISDL.GetBlockSOADet(argBlockId, argAsOn);
        }

        public static DataSet GetSOADet(int argFlatId, DateTime argAsOn)
        {
            return MISDL.GetSOADet(argFlatId, argAsOn);
        }

        public static DataTable GetStages(int argBlockId)
        {
            return MISDL.GetStages(argBlockId);
        }

        public static DataSet GetCCReceiptType(int argCCId, DateTime argDate)
        {
            return MISDL.GetCCReceiptType(argCCId, argDate);
        }

        public static DataSet GetAsOnCCReceiptType(int argCCId, DateTime argDate)
        {
            return MISDL.GetAsOnCCReceiptType(argCCId, argDate);
        }

        public static DataSet GetBlockReceiptType(int argCCId, int argBlockId, DateTime argDate)
        {
            return MISDL.GetBlockReceiptType(argCCId, argBlockId, argDate);
        }

        public static DataSet GetAsOnBlockReceiptType(int argCCId, int argBlockId, DateTime argDate)
        {
            return MISDL.GetAsOnBlockReceiptType(argCCId, argBlockId, argDate);
        }

        public static DataSet GetFlatReceiptType(int argCCId, int argFlatId, DateTime argDate)
        {
            return FlatDetailsDL.GetFlatReceiptType(argCCId, argFlatId, argDate);
        }

        public static DataSet GetAsOnFlatReceiptType(int argCCId, int argFlatId, DateTime argDate)
        {
            return FlatDetailsDL.GetAsOnFlatReceiptType(argCCId, argFlatId, argDate);
        }

        public static DataTable GetQual(int argQId, DateTime argDate)
        {
            return MISDL.GetQual(argQId, argDate);
        }

        #endregion

        #region Sales Report

        public static DataTable GetBlockwiseSalesReport()
        {
            return MISDL.GetBlockwiseSalesReport();
        }

        public static DataTable GetCustomerSalesReport()
        {
            return MISDL.GetCustomerSalesReport();
        }

        public static DataTable GetLevelwiseSalesReport()
        {
            return MISDL.GetLevelwiseSalesReport();
        }

        #endregion
    }
}
