using System;
using System.Collections.Generic;
using System.Data;
using CRM.DataLayer;
using CRM.BusinessObjects;

namespace CRM.BusinessLayer
{
    class ProgBillBL
    {

        #region Variables
        public static int RegId { set; get; }
        public static int CCId { set; get; }
        public static int AnalHeadId { set; get; }
        public static string AcUpdate { set; get; }
        public static DateTime BillDate { set; get; }
        public static DateTime BVDate { set; get; }
        public static int AccId { set; get; }
        public static string BillNo { set; get; }
        public static string Remarks { set; get; }
        public static int CompanyId { set; get; }
        public static string CompanyDBName { set; get; }
        public static int BuyerTypeId { set; get; }
        public static int BuyerSLTypeId { set; get; }
        public static int IncomeId { set; get; }
        public static int AdvanceId { set; get; }
        public static int BuyerAccountId { set; get; }
        public static int IncomeSLTypeId { set; get; }
        public static int FACCId { set; get; }
        public static int BuyerSLId { set; get; }

        #endregion
   
        #region Methods

        public static DataTable GetOpCostCentre()
        {
            return ProgBillDL.GetOpCostCentre();
        }
        public static DataTable GetCostCentre()
        {
            return ProgBillDL.GetCostCentre();
        }

        public static DataTable GetBlock(int argCCId)
        {
            return ProgBillDL.GetBlock(argCCId);
        }

        public static DataTable GetLevel(int argCCId)
        {
            return ProgBillDL.GetLevel(argCCId);
        }

        public static DataTable GetPBMaster(int argCCId, string argBType)
        {
            return ProgBillDL.GetPBMaster(argCCId, argBType);
        }
        public static DataTable GetAcct(int argTypeId)
        {
            return ProgBillDL.GetAcct(argTypeId);
        }

        public static void UpdatePBAccountSetup(int argIncomeId, int argBuyerId, int argAdvanceId,string argBType)
        {
            ProgBillDL.UpdatePBAccountSetup(argIncomeId, argBuyerId, argAdvanceId,argBType);
        }

        public static DataTable GetPBAccountSetup(string argBType)
        {
            return ProgBillDL.GetPBAccountSetup(argBType);
        }
        
        public static DataTable GetSoldFlat()
        {
            return ProgBillDL.GetSoldFlat();
        }

        public static DataTable GetBillNo()
        {
            return ProgBillDL.GetBillNo();
        }
        public static DataTable GetPBRegister(int argCCId, int argProgRegId, string argBType)
        {
            return ProgBillDL.GetPBRegister(argCCId, argProgRegId, argBType);
        }
        public static DataTable GetPaySchSoldFlat(int argCCId,int argSId, string arg_sStage)
        {
            return ProgBillDL.GetPaySchSoldFlat(argCCId, argSId, arg_sStage);
        }

        public static DataSet GetPBReceipt(int argCCId,int argBlockId,int argLevelId, int argSId, string arg_sStage,DateTime argDate)
        {
            return ProgBillDL.GetPBReceipt(argCCId,argBlockId,argLevelId, argSId, arg_sStage,argDate);
        }
        public static DataSet GetPBReceiptPlot(int argLandId, int argSId, string arg_sStage,DateTime argDate)
        {
            return ProgBillDL.GetPBReceiptPlot(argLandId, argSId, arg_sStage,argDate);
        }
        public static DataSet GetAllPBReceipt(int argCCId, int argBlockId, int argLevelId, DateTime argDate)
        {
            return ProgBillDL.GetAllPBReceipt(argCCId, argBlockId, argLevelId, argDate);
        }
        public static DataSet GetAllPBReceiptPlot(int argLandId, DateTime argDate, string argStageDesc)
        {
            return ProgBillDL.GetAllPBReceiptPlot(argLandId, argDate, argStageDesc);
        }
        public static DataSet GetPBPlot(int argLandId)
        {
            return ProgBillDL.GetPBPlot(argLandId);
        }
        public static DataTable GetPaySchStage(int argCCId, int argBlockId)
        {
            return ProgBillDL.GetPaySchStage(argCCId,argBlockId);
        }

        public static DataTable GetStage(int argCCId,string argType,string argBusType)
        {
            return ProgBillDL.GetStage(argCCId, argType,argBusType);
        }
        public static DataTable GetPlot(int argLandId)
        {
            return ProgBillDL.GetPlot(argLandId);
        }
        public static DataTable GetTax()
        {
            return ProgBillDL.GetTax();
        }

        public static int StageComplete(int argBlockId, int argLevelId, int argStageId)
        {
            return ProgBillDL.StageComplete(argStageId, argBlockId, argLevelId);
        }

        public static bool InsertProgressBillRegister(ProgressBillRegister argPBReg, DataTable argM,DataTable argRec,DataTable dtQual,DataTable dtQualAbs)
        {
            return ProgBillDL.InsertProgressBillRegister(argPBReg, argM, argRec, dtQual,dtQualAbs);
        }
        public static bool InsertPlotProgressBillRegister(ProgressBillRegister argPBReg, DataTable argM, DataTable argRec, DataTable dtQual, DataTable dtQualAbs)
        {
            return ProgBillDL.InsertPlotProgressBillRegister(argPBReg, argM, argRec, dtQual, dtQualAbs);
        }
        public static void UpdateProgressBillRegister(ProgressBillRegister argPBReg, DataTable argdt, DataTable argdtQ,DataTable dtQualAbs, bool argHiddenUpdate)
        {
            ProgBillDL.UpdateProgressBillRegister(argPBReg,argdt,argdtQ,dtQualAbs, argHiddenUpdate);
        }
        public static void UpdatePlotProgressBillRegister(ProgressBillRegister argPBReg, DataTable argdt, DataTable argdtQ, DataTable dtQualAbs, bool argHiddenUpdate)
        {
            ProgBillDL.UpdatePlotProgressBillRegister(argPBReg, argdt, argdtQ, dtQualAbs, argHiddenUpdate);
        }
        public static DataSet GetPBillDetailsEdit(int argPBillId,int argCCId)
        {
            return ProgBillDL.GetPBillDetailsEdit(argPBillId,argCCId);
        }
        public static DataSet GetPBillDetailsMultipleEdit(int argProgRegId, int argCCId)
        {
            return ProgBillDL.GetPBillDetailsMultipleEdit(argProgRegId, argCCId);
        }
        public static DataSet GetPlotPBillDetailsEdit(int argPBillId,int argLandId)
        {
            return ProgBillDL.GetPlotPBillDetailsEdit(argPBillId,argLandId);
        }
        public static DataSet GetPlotPBillDetailsMultipleEdit(int argProgRegId, int argLandId)
        {
            return ProgBillDL.GetPlotPBillDetailsMultipleEdit(argProgRegId, argLandId);
        }
        public static DataTable GetQual(int argTempId)
        {
            return ProgBillDL.GetQual(argTempId); 
        }

        public static void DeletePBill(int argId)
        {
            ProgBillDL.DeletePBill(argId);
        }

        public static void DeletePlotPBill(int argId)
        {
            ProgBillDL.DeletePlotPBill(argId);
        }

        public static void DeletePBillMaster(int argId)
        {
            ProgBillDL.DeletePBillMaster(argId);
        }

        public static void DeletePlotPBillMaster(int argId)
        {
            ProgBillDL.DeletePlotPBillMaster(argId);
        }

        public static bool GetApprFound(int argRegId, int argCCId)
        {
            return ProgBillDL.GetApprFound(argRegId, argCCId);
        }


        internal static bool GetReceiptRaised(int argRegId, int argCCId)
        {
            return ProgBillDL.GetReceiptRaised(argRegId, argCCId);
        }

        public static void UpdateQualifiers()
        {
            ProgBillDL.UpdateQualifiers();
        }

        public static DataTable GetQualifierAccount(string argBus)
        {
            return ProgBillDL.GetQualifierAccount(argBus);
        }

        public static DataTable GetQualAcct()
        {
            return ProgBillDL.GetQualAcct();
        }

        public static DataTable GetAccountQualifier(string argBType)
        {
            return ProgBillDL.GetAccountQualifier(argBType);
        }

        public static void UpdateQualAccount(DataTable argDt,string argBType)
        {
            ProgBillDL.UpdateQualAccount(argDt,argBType);
        }

        public static bool GetFAUpdateFound(int argCCId)
        {
            return ProgBillDL.GetFAUpdateFound(argCCId);
        }

        #endregion

        #region DemandLetter

        public static DataTable GetAgeDesc(int argCCId)
        {
            return ProgBillDL.GetAgeDesc(argCCId);
        }

        public static DataSet GetDemandAge(int argFromDays,int argToDays,int argCCId)
        {
            return ProgBillDL.GetDemandAge(argFromDays,argToDays,argCCId);
        }

        public static DataSet GetDemandAgeStatus(int argFromDays, int argToDays, int argCCId)
        {
            return ProgBillDL.GetDemandAgeStatus(argFromDays, argToDays, argCCId);
        }

        public static DataTable GetSentDate(int argPBillId)
        {
            return ProgBillDL.GetSentDate(argPBillId);
        }

        public static void InsertDLStatus(DataTable argdt, int argAgeId)
        {
            ProgBillDL.InsertDLStatus(argdt, argAgeId);
        }

        public static void InsertDLDate(string argsLeadId, string argBillId, DataTable argdt)
        {
            ProgBillDL.InsertDLDate(argsLeadId, argBillId,argdt);
        }

        public static decimal GetSchPer(int argFlatId)
        {
            return ProgBillDL.GetSchPer(argFlatId);
        }

        public static DataTable GetDNPrint(int argFlatId, int argPBillId, int argProgRegId)
        {
            return ProgBillDL.GetDNPrint(argFlatId, argPBillId, argProgRegId);
        }

        public static DataTable GetDNPaymentSchPrint(int argFlatId, int argPBillId, int argProgRegId)
        {
            return ProgBillDL.GetDNPaymentSchPrint(argFlatId, argPBillId, argProgRegId);
        }

        #endregion

        internal static DataTable GetDemandLetterFirstPrint(int argiCCId, string argsBillId, string argsLeadId)
        {
            return ProgBillDL.GetDemandLetterFirstPrint(argiCCId, argsBillId, argsLeadId);
        }

        internal static DataTable GetProgressBillAlert(int argiCCId, int argPBillId, int argFlatId, int argLeadId)
        {
            return ProgBillDL.GetProgressBillAlert(argiCCId, argPBillId, argFlatId, argLeadId);
        }
    }
}
