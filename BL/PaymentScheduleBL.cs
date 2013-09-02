using System;
using System.Collections.Generic;
using System.Data;
using CRM.DataLayer;
using System.Data.SqlClient;

namespace CRM.BusinessLayer
{
    class PaymentScheduleBL
    {
        #region Declaration

        DataTable dtAnalHead = new DataTable();
        DataTable dtpaymentSch = new DataTable();

        #endregion

        #region Properties

        public DataTable DtReceiptType { get; set; }

        public DataTable DtReceipt { get; set; }

        public DataTable DtPayment { get; set; }

        public DataTable DtAanalHead
        {
            get { return dtAnalHead; }
            set { value = dtAnalHead; }

        }

        public DataTable DtPaymentSchedule
        {
            get { return dtpaymentSch; }
            set { value = dtpaymentSch; }

        }

        #endregion

        #region Methods

        public static DataTable GetStagesBlock(int argBlockId, int argCCId)
        {
            return PaymentScheduleDL.GetStagesBlock(argBlockId, argCCId);
        }

        public static DataTable GetStagesLevel(int argLevelId, int argCCId)
        {
            return PaymentScheduleDL.GetStagesLevel(argLevelId, argCCId);
        }

        public static bool GetAdvFound(int argPayTypeId, int argCCId)
        {
            return PaymentScheduleDL.GetAdvFound(argPayTypeId, argCCId);
        }

        public static DataTable GetReceiptTypeOrder(int argPayTypeId, int argCCId)
        {
            return PaymentScheduleDL.GetReceiptTypeOrder(argPayTypeId,argCCId);
        }

        public static DataTable GetReceiptTypes(int argTempId, string argType, int argPayTypeId, int argCCId)
        {
            return PaymentScheduleDL.GetReceiptTypes(argTempId, argType, argPayTypeId, argCCId);
        }

        public static DataTable GetAdvReceipt(int argTempId, string argType, int argPayTypeId, int argCCId)
        {
            return PaymentScheduleDL.GetAdvReceipt(argTempId, argType, argPayTypeId, argCCId);
        }

        public static DataTable GetQualifierMaster(string argType, int argId, string argFlatType, int argFlatId, string argQualType)
        {
            return PaymentScheduleDL.GetQualifierMaster(argType, argId, argFlatType, argFlatId, argQualType);
        }

        public static DataSet GetFlatAdvReceipt(int argTempId, string argType, int argPayTypeId, int argCCId)
        {
            return PaymentScheduleDL.GetFlatAdvReceipt(argTempId, argType, argPayTypeId, argCCId);
        }

        public static void UpdateReceiptTypeM(int argTempId, DataTable argR,DataTable dtTax)
        {
            PaymentScheduleDL.UpdateReceiptTypeM(argTempId, argR,dtTax);
        }
        public static bool CheckNetAmt(int argFlatId,int argPaySchId, DataTable dtAmt)
        {
            return PaymentScheduleDL.CheckNetAmt(argFlatId,argPaySchId, dtAmt);
        }
        public static string GetAdvPer(int argPayTypeId, int argCCId)
        {
            return PaymentScheduleDL.GetAdvPer(argPayTypeId, argCCId);
        }
        public static string GetTAXPer(int argPayTypeId, int argCCId)
        {
            return PaymentScheduleDL.GetTAXPer(argPayTypeId,argCCId);
        }
        public static bool GetRecOrder(int argPayTypeId,int argCCId)
        {
            return PaymentScheduleDL.GetRecOrder(argPayTypeId,argCCId);
        }

        public static void UpdateCCQualifier(int argCCId, DataTable argR, DataTable argQ)
        {
            PaymentScheduleDL.UpdateCCQualifier(argCCId, argR, argQ);
        }

        public static string GetPaySchFlatValidate(int argCCId, int argFlatId)
        {
            return PaymentScheduleDL.GetPaySchFlatValidate(argCCId, argFlatId);
        }

        public static void UpdateReceiptTypeF(int argTempId, DataTable argR, DataTable argQ,decimal argNetAmt,int argFlatId,DataTable dtTax)
        {
            PaymentScheduleDL.UpdateReceiptTypeF(argTempId, argR, argQ,argNetAmt,argFlatId,dtTax);
        }

        public static decimal GetAdvance(int argFlatId,int argPaySchId)
        {
            return PaymentScheduleDL.GetAdvance(argFlatId, argPaySchId);
        }

        public static DataSet GetReceiptTypeFlat(int argTempId, string argType, int argPayTypeId, int argCCId)
        {
            return PaymentScheduleDL.GetReceiptTypeFlat(argTempId, argType, argPayTypeId, argCCId);
        }

        public static DataTable GetReceiptQualFlat(int argTempId, string argType, int argPayTypeId, int argCCId, int argFlatId, bool argTypewise, string argQualType)
        {
            return PaymentScheduleDL.GetReceiptQualFlat(argTempId, argType, argPayTypeId, argCCId, argFlatId, argTypewise, argQualType);
        }
        public static DataTable GetReceiptQualMaster(int argBuyerId, DateTime argDate, string argQualType)
        {
            return PaymentScheduleDL.GetReceiptQualMaster(argBuyerId, argDate, argQualType);
        }
        public static DataSet GetPBReceiptTypeFlat(int argTempId, string argType, int argPayTypeId, int argCCId,int argPBillId)
        {
            return PaymentScheduleDL.GetPBReceiptTypeFlat(argTempId, argType, argPayTypeId, argCCId,argPBillId);
        }

        public static DataTable GetPaymentScheduleFlat(int argCCId, int argFlatId)
        {
            return PaymentScheduleDL.GetPaymentScheduleFlat(argCCId, argFlatId);
        }
        public static DataTable GetPaymentSchedulePlot(int argCCId, int argPlotId)
        {
            return PaymentScheduleDL.GetPaymentSchedulePlot(argCCId, argPlotId);
        }
        public static DataTable GetPayRec(int argSchId, int argFlatId, int argRId)
        {
            return PaymentScheduleDL.GetPayRec(argSchId, argFlatId, argRId);
        }

        public static DataTable GetPayOCRec(int argSchId, int argFlatId, int argRId,int argOCId)
        {
            return PaymentScheduleDL.GetPayOCRec(argSchId, argFlatId, argRId,argOCId);
        }

        public static DataTable GetOCRec(int argFlatId,int argCCId)
        {
            return PaymentScheduleDL.GetOCRec(argFlatId,argCCId);
        }

        public static DataTable GetReceiptType()
        {
          return PaymentScheduleDL.GetReceiptType();
        }

        public static DataTable GetStages(int argCCId,int argPayTypeId)
        {
            return PaymentScheduleDL.GetStages(argCCId,argPayTypeId);
        }

        public static DataTable GetDesc(int argCCId, int argPayTypeId, string argDescType)
        {
            return PaymentScheduleDL.GetDesc(argCCId, argPayTypeId, argDescType);
        }

        internal static DataTable PopulateDescriptionMaster(string argDescType)
        {
            return PaymentScheduleDL.PopulateDescriptionMaster(argDescType);
        }

        public static void InsertPayScheduleDes(DataTable argDt, int argCCId, int argPayTypeId, int argRow, string argDescType)
        {
            PaymentScheduleDL.InsertPayScheduleDes(argDt, argCCId, argPayTypeId, argRow, argDescType);
        }

        public static void InsertPayScheduleStage(DataTable argDt, int argCCId, int argPayTypeId, int argRow)
        {
            PaymentScheduleDL.InsertPayScheduleStage(argDt, argCCId, argPayTypeId, argRow);
        }

        public static DataTable GetOCSetup(int argCCId)
        {
            return PaymentScheduleDL.GetOCSetup(argCCId);
        }

        public static void InsertPaymentSchedule(DataTable argPayTrans, int argCCId,int argTId)
        {
            PaymentScheduleDL.InsertPaymentSchedule(argPayTrans, argCCId,argTId);
        }

        public static void UpdatePaymentSchedule(DataTable argPayTrans, int argTId)
        {
            PaymentScheduleDL.UpdatePaymentSchedule(argPayTrans,argTId);
        }

        public static void InsertPaymentSchFlat(DataTable argPayTrans, int argFTId,int argFId,DataTable argdtR,SqlConnection conn,SqlTransaction tran)
        {
            PaymentScheduleDL.InsertPaymentSchFlat(argPayTrans, argFTId, argFId, argdtR,conn,tran);
        }
        
        public static void InsertFlatSchedule(int argFlatId)
        {
            PaymentScheduleDL.InsertFlatSchedule(argFlatId);
        }

        public static void InsertFinalFlatSchedule(int argFlatId,string argType)
        {
            PaymentScheduleDL.InsertFinalFlatSchedule(argFlatId,argType);
        }

        public static void UpdateFlatSchedule(int argFlatId,DataTable argdt)
        {
            PaymentScheduleDL.UpdateFlatSchedule(argFlatId, argdt);
        }

        public static void UpdateFinalFlatSchedule(int argFlatId,string argsType, DataTable argdt)
        {
            PaymentScheduleDL.UpdateFinalFlatSchedule(argFlatId, argsType, argdt);
        }

        public static void UpdateFlatScheduleQual(int argFlatId, DataTable argdt)
        {
            PaymentScheduleDL.UpdateFlatScheduleQual(argFlatId, argdt);
        }

        public static void UpdateReceiptFlatSchedule(int argFlatId, DataTable argdt)
        {
            PaymentScheduleDL.UpdateReceiptFlatSchedule(argFlatId, argdt);
        }

        public static void UpdateReceiptFlatScheduleQual(int argFlatId, DataTable argdt)
        {
            PaymentScheduleDL.UpdateReceiptFlatScheduleQual(argFlatId, argdt);
        }

        public static DataTable GetCommPaySchFlat(int argCCId, int argFlatId, int argPayTypeId)
        {
            return PaymentScheduleDL.GetCommPaySchFlat(argCCId, argFlatId, argPayTypeId);
        }

        public static DataTable GetReceiptCommPaySchFlat(int argCCId, int argFlatId, int argPayTypeId)
        {
            return PaymentScheduleDL.GetReceiptCommPaySchFlat(argCCId, argFlatId, argPayTypeId);
        }

        public static DataSet GetReceiptQ(int argCCId)
        {
            return PaymentScheduleDL.GetReceiptQ(argCCId);
        }

        public static void InsertNoOfFlats(int argCCId, int argFlatId, SqlConnection conn, SqlTransaction tran)
        {
            PaymentScheduleDL.InsertNoOfFlats(argCCId, argFlatId, conn, tran);
        }

        public static void InsertFlatScheduleI(int argFlatId, SqlConnection conn, SqlTransaction tran)
        {
            PaymentScheduleDL.InsertFlatScheduleI(argFlatId,conn,tran);
        }

        public static DataTable GetPaymentSchFlat(int argCCId, int argTId, SqlConnection conn,SqlTransaction tran)
        {
            return PaymentScheduleDL.GetPaymentSchFlat(argCCId,argTId,conn,tran);
        }

        public static DataTable GetCCReceipt(int argCCId, int argTId,SqlConnection conn,SqlTransaction tran)
        {
            return PaymentScheduleDL.GetCCReceipt(argCCId, argTId,conn,tran);
        }

        public static DataTable PopulatePaySchTemp(int argCCId)
        {
            return PaymentScheduleDL.PopulatePaySchTemp(argCCId);
        }

        public static DataTable Payment(int argCCId,int argTId)
        {
            return PaymentScheduleDL.Payment(argCCId,argTId);
        }

        public static bool CheckTemplateUsed(int argId)
        {
            return PaymentScheduleDL.CheckTemplateUsed(argId);
        }

        public static void UpdateSortOrder(DataTable argDt)
        {
            PaymentScheduleDL.UpdateSortOrder(argDt);
        }

        public static void UpdateReceiptTypeSortOrder(DataTable argDt)
        {
            PaymentScheduleDL.UpdateReceiptTypeSortOrder(argDt);
        }

        public static DataTable GetPayScheduleDate(int argTempId)
        {
            return PaymentScheduleDL.GetPayScheduleDate(argTempId);
        }

        public static DataTable GetFlatPayScheduleDate(int argPaySchId)
        {
            return PaymentScheduleDL.GetFlatPayScheduleDate(argPaySchId);
        }

        public static void UpdatePayScheduleDate(int argTempId, int argTypeId, bool argAfter, string argDurType, int argduration, string argDate,int argCCId,int argPayTypeId)
        {
            PaymentScheduleDL.UpdatePayScheduleDate(argTempId, argTypeId, argAfter, argDurType, argduration, argDate,argCCId,argPayTypeId);
        }

        public static bool CheckUp(int argTempId, int argCheckId)
        {
            return PaymentScheduleDL.CheckUp(argTempId, argCheckId);
        }

        public static void UpdateTemplatePer(int argId, decimal argPer)
        {
            PaymentScheduleDL.UpdateTemplatePer(argId, argPer);
        }

        public static bool CheckDown(int argTempId, int argCheckId)
        {
            return PaymentScheduleDL.CheckDown(argTempId, argCheckId);
        }

        public static DataTable GetPreviousStages(int argTempId, int argCCId, int argPayTypeId)
        {
            return PaymentScheduleDL.GetPreviousStages(argTempId, argCCId, argPayTypeId);
        }

        public static DataTable GetFlatPreviousStages(int argTempId, int argCCId, int argPayTypeId,int argFlatId)
        {
            return PaymentScheduleDL.GetFlatPreviousStages(argTempId, argCCId, argPayTypeId,argFlatId);
        }

        public static void UpdatePayDate(int argCCId,int argTempId,DataTable argPayTrans)
        {
            PaymentScheduleDL.UpdatePayDate(argCCId, argTempId,argPayTrans);
        }

        public static void UpdatePayPercent(int argCCId, int argTempId, decimal argPer)
        {
            PaymentScheduleDL.UpdatePayPercent(argCCId, argTempId, argPer);
        }

        public static DataTable PaySchType(int argCCId)
        {
            return PaymentScheduleDL.PaySchType(argCCId);
        }

        public static void DeletePay(int argTempId)
        {
            PaymentScheduleDL.DeletePay(argTempId);
        }

        public static bool CheckPaymentScheduleUsed(int argId)
        {
            return PaymentScheduleDL.CheckPaymentScheduleUsed(argId);
        }

        public static bool CheckPaymentScheduleDesUsed(int argId)
        {
            return PaymentScheduleDL.CheckPaymentScheduleDesUsed(argId);
        }

        public static void UpdateFlatPaySchDate(int argFlatId, int argTempId, int argTypeId, bool argAfter, string argDurType, int argduration, string argDate, int argCCId, int argPayTypeId)
        {
            PaymentScheduleDL.UpdateFlatPaySchDate(argFlatId, argTempId, argTypeId, argAfter, argDurType, argduration, argDate, argCCId, argPayTypeId);
        }

        public static DataTable GetAllFlats(int argCCId)
        {
            return PaymentScheduleDL.GetAllFlats(argCCId);
        }

        public static bool CheckBillPassed(int argFlatId)
        {
            return PaymentScheduleDL.CheckBillPassed(argFlatId);
        }

        public static decimal CheckReceiptAmt(int argFlatId, int argPaySchId, int argReceiptId, int argOCId,decimal argAmt)
        {
            return PaymentScheduleDL.CheckReceiptAmt(argFlatId, argPaySchId, argReceiptId, argOCId, argAmt);
        }

        public static string GetStatus(int argFlatId)
        {
            return PaymentScheduleDL.GetStatus(argFlatId);
        }

        #endregion
    }
}
