using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DataLayer;
using CRM.BusinessObjects;

namespace CRM.BusinessLayer
{
    class ReceiptDetailBL
    {

        #region Objects

        #endregion


        #region Methods

        public static DataSet GetFlatReceipt(int argCCId, int argBuyerId, DateTime argDate)
        {
            return ReceiptDetailDL.GetFlatReceipt(argCCId,argBuyerId,argDate);
        }

        public static DataTable GetReceiptPayment( int argSId, string arg_sStage, int argTenant,int argCCId,DateTime argDate,int argFlatId)
        {
            return ReceiptDetailDL.GetReceiptPayment(argSId, arg_sStage, argTenant,argCCId,argDate,argFlatId);
        }
        public static DataTable GetPBQualifierAbs(int argReciptId)
        {
            return ReceiptDetailDL.GetPBQualifierAbs(argReciptId);
        }
        public static DataTable GetPlotReceiptPayment(int argBuyerId, string arg_sStage, int argTenant, int argCCId,int argLandId)
        {
            return ReceiptDetailDL.GetPlotReceiptPayment(argBuyerId, arg_sStage, argTenant, argCCId,argLandId);
        }

        public static void InsertReceiptDetails(ReceiptDetailBO argReceiptItemContactBO, DataTable dtReceipt, string argType, DataTable dtQual, DataTable dtPayInfo, DataTable argdtRecType, DataTable argdtRecQual,DataTable dtQualAbs)
        {
            ReceiptDetailDL.InsertReceiptDetails(argReceiptItemContactBO, dtReceipt, argType, dtQual, dtPayInfo, argdtRecType, argdtRecQual,dtQualAbs);
        }

        public static decimal GetReceiptAmount(int argRecId, int argPaySchId)
        {
            return ReceiptDetailDL.GetReceiptAmount(argRecId, argPaySchId);
        }
        public static void UpdateReceiptDetails(ReceiptDetailBO argReceiptItemContactBO, DataTable dtReceipt, string argType, DataTable dtPay, DataTable argdtRT, DataTable argdtRQ,DataTable dtQualAbs, bool argHiddenUpdate)
        {
            ReceiptDetailDL.UpdateReceiptDetails(argReceiptItemContactBO, dtReceipt, argType, dtPay, argdtRT, argdtRQ,dtQualAbs, argHiddenUpdate);
        }

        public static DataSet GetReceiptDetE(int argReciptId)
        {
            return ReceiptDetailDL.GetReceiptDetE(argReciptId);
        }
        public static DataTable GetReceiptTransE(int argReciptId, string argBillType, int argBuyeId, int argFlatId)
        {
            return ReceiptDetailDL.GetReceiptTransE(argReciptId, argBillType, argBuyeId, argFlatId);
        }
        public static DataTable GetReceiptTypeTrans(int argReciptId, int argFlatId)
        {
            return ReceiptDetailDL.GetReceiptTypeTrans(argReciptId,argFlatId);
        }
        public static DataTable GetQualifierTrans(int argReciptId, string argPaymentOpt)
        {
            return ReceiptDetailDL.GetQualifierTrans(argReciptId, argPaymentOpt);
        }
        public static DataTable GetQualifierAbs(int argReciptId, string argPaymentOpt)
        {
            return ReceiptDetailDL.GetQualifierAbs(argReciptId, argPaymentOpt);
        }
        public static string GetApprove(int argRegId)
        {
            return ReceiptDetailDL.GetApprove(argRegId);
        }
       
        public static DataTable GetPlotReceiptTransE(int argReciptId, string argBillType, int argBuyeId)
        {
            return ReceiptDetailDL.GetPlotReceiptTransE(argReciptId, argBillType, argBuyeId);
        }
        public static DataTable GetPaymentMode()
        {
            return ReceiptDetailDL.GetPaymentMode();
        }

        public static DataTable GetQualifierMaster(string argType)
        {
            return ReceiptDetailDL.GetQualifierMaster(argType);
        }
        public static bool Check_ReceiptDet(int argRepId)
        {
            return ReceiptDetailDL.Check_ReceiptDet(argRepId);
        }
        public static void DeleteReceiptDetails(int argRecpId,string argType)
        {
            ReceiptDetailDL.DeleteReceiptDetails(argRecpId,argType);
        }

        public static DataTable GetPayInfo(int argBuyerId)
        {
            return ReceiptDetailDL.GetPayInfo(argBuyerId);
        }

        public static DataTable GetEditPayInfo(int argBuyerId)
        {
            return ReceiptDetailDL.GetEditPayInfo(argBuyerId);
        }

        public static DataTable GetReceiptRegister()
        {
            return ReceiptDetailDL.GetReceiptRegister();
        }

        public static string GetReceiptPrint(string argFlatNo, int argCCId, int argRecId)
        {
            return ReceiptDetailDL.GetReceiptPrint(argFlatNo, argCCId, argRecId);
        }

        public static DataTable GetChangeGridReceiptRegister(int argRecpId)
        {
            return ReceiptDetailDL.GetChangeGridReceiptRegister(argRecpId);
        }
        public static DataSet GetFlatReceiptNew(int argCCId, int argBuyerId, DateTime argDate, int argFlatId)
        {
            return ReceiptDetailDL.GetFlatReceiptNew(argCCId, argBuyerId, argDate,argFlatId);
        }
        public static DataSet GetPBFlatReceiptNew(int argCCId, int argBuyerId, DateTime argDate, int argFlatId, bool argType)
        {
            return ReceiptDetailDL.GetPBFlatReceiptNew(argCCId, argBuyerId, argDate, argFlatId, argType);
        }
        public static bool GetFAUpdateFound(int argCCId)
        {
            return ReceiptDetailDL.GetFAUpdateFound(argCCId);
        }
        public static DataTable GetFlat(int argCCId, int argBuyerId)
        {
            return ReceiptDetailDL.GetFlat(argCCId, argBuyerId);
        }

        public static bool GetPaymentAdvance(int argFlatId)
        {
            return ReceiptDetailDL.GetPaymentAdvance(argFlatId);
        }

        #region PopulateLookUpEdit

        public static DataTable GetCostCentre()
        {
            return ReceiptDetailDL.GetCostCentre();
        }

        public static DataTable GetBuyer(int argCCId)
        {
            return ReceiptDetailDL.GetBuyer(argCCId);
        }

        public static DataTable GetTenant(int argCCId)
        {
            return ReceiptDetailDL.GetTenant(argCCId);
        }

        public static DataTable GetSTSettings(string argWOType, DateTime argDate)
        {
            return ReceiptDetailDL.GetSTSettings(argWOType, argDate);
        }

        public static DataTable QualifierSelect(int argQId, string argQType, DateTime argDate)
        {
            return ReceiptDetailDL.QualifierSelect(argQId, argQType, argDate);
        }

        public static DataTable GetQual(int argQId, DateTime argDate)
        {
            return ReceiptDetailDL.GetQual(argQId, argDate);
        }

        #endregion

        #region ReceiptAcct

        public static DataTable GetQualifierAccount(string argBus)
        {
            return ReceiptDetailDL.GetQualifierAccount(argBus);
        }

        public static void UpdateQualAccount(DataTable argDt, string argBType)
        {
            ReceiptDetailDL.UpdateQualAccount(argDt, argBType);
        }

        public static DataTable GetAccountQualifier(string argBType)
        {
            return ReceiptDetailDL.GetAccountQualifier(argBType);
        }

        #endregion


        #endregion


        #region Narration

        internal static DataTable PopulateNarrationMaster()
        {
            return ReceiptDetailDL.PopulateNarrationMaster();
        }

        internal static void DeleteNarrationMaster(int argNarrId)
        {
            ReceiptDetailDL.NarrationMasterDelete(argNarrId);
        }

        internal static void InsertNarrationMaster(int argNarrId, string argDescription)
        {
            ReceiptDetailDL.InsertNarrationMaster(argNarrId, argDescription);
        }

        public static DataTable PopulateNarr()
        {
            return ReceiptDetailDL.PopulateNarr();
        }

        #endregion

        internal static DataSet GetEBQualifier(int argReceiptId, int argFlatId, string argPaymentOpt)
        {
            return ReceiptDetailDL.GetEBQualifier(argReceiptId, argFlatId, argPaymentOpt);
        }

        internal static DataTable GetFlatInterest(int argCCId, int argFlatId, int argPaymentSchId, int argReceiptId)
        {
            return ReceiptDetailDL.GetFlatInterest(argCCId, argFlatId, argPaymentSchId, argReceiptId);
        }

        internal static DataTable GetOtherCost()
        {
            return ReceiptDetailDL.GetOtherCost();
        }

        internal static DataTable PopulateF1CollectionReport(int argReportType, DateTime argFrom, DateTime argTo)
        {
            return ReceiptDetailDL.PopulateF1CollectionReport(argReportType, argFrom, argTo);
        }

        internal static DataTable PopulateF2CollectionReport(int argReportType, DateTime argFrom, DateTime argTo, int argOCId)
        {
            return ReceiptDetailDL.PopulateF2CollectionReport(argReportType, argFrom, argTo, argOCId);
        }
    }
}
