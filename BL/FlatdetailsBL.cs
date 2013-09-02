using System;
using System.Collections.Generic;
using System.Data;
using CRM.BusinessObjects;
using CRM.DataLayer;
using System.Data.SqlClient;

namespace CRM.BusinessLayer
{
    class FlatdetailsBL
    {
        
        #region Objects
	    #endregion

        #region Constructor
        #endregion

        #region Variable
        #endregion
        
        #region Properties

        public DataTable DtOtherCost { get; set; }

        public int FlatId { get; set; }

        public int FlatTypeId { set; get; }
        
        public int BlockId { set; get; }

        public decimal AdvAmt { set; get; }

        public decimal LandRate { set; get; }

        public decimal LandAmt { set; get; }

        public int ProjId { get; set; }

        public int LevelId { get; set; }

        public string FlatNo { get; set; }

        public decimal Area { get; set; }

        public decimal Rate { get; set; }

        public decimal BaseAmt { get; set; }

        public decimal USLand { get; set; }

        public decimal OtherCostAmt { get; set; }

        public decimal TotalCarPark { get; set; }

        public decimal ExtraBillAmt { get; set; }

        public decimal VatAmt { get; set; }

        public decimal SerTaxAmt { get; set; }

        public decimal NetAmt { get; set; }

        public bool IncludePaySch { get; set; }

        public string Remarks { get; set; }

        public DataTable DtFlatType { get; set; }

        public DataTable DtLevel { get; set; }

        public DataTable DtExtra { get; set;}

        public DataTable DtFlatArea { get; set; }

        public DataTable DtFlatDet { get; set; }

        public string FlatFace { set; get; }
        

            #endregion

        #region Methods

        public static DataSet GetFlat(int argCCId, int argFlatTypeId,int argBlockId,int argLevelId,string argStatus)
        {
            return FlatDetailsDL.GetFlat(argCCId,argFlatTypeId,argBlockId,argLevelId,argStatus);
        }
        public static DataTable GetFlatType()
        {
            return FlatDetailsDL.GetFlatType();
        }

        internal static bool getWebFound(int argFlatId, int argExtraId)
        {
            return FlatDetailsDL.getWebFound(argFlatId, argExtraId);
        }

        public static DataTable GetProj()
        {
            return FlatDetailsDL.GetProj();
        }
        public static DataTable GetPayInfo(int argFlatId)
        {
            return FlatDetailsDL.GetPayInfo(argFlatId);
        }
        public static DataTable GetClientBuyerInfo(int argFlatId, int argLandId,string argType)
        {
            return FlatDetailsDL.GetClientBuyerInfo(argFlatId, argLandId,argType);
        }
        public static DataTable GetBuyerInfo(int argFlatId,string argType,bool argPayType)
        {
            return FlatDetailsDL.GetBuyerInfo(argFlatId,argType,argPayType);
        }
        public static DataTable GetInterest(int argFlatId,string argType,DateTime argAsOn,int argCreditDays)
        {
            return FlatDetailsDL.GetInterest(argFlatId,argType,argAsOn,argCreditDays);
        }
        public static DataTable GetSOA(int argFlatId, string argType, DateTime argAsOn, int argCreditDays)
        {
            return FlatDetailsDL.GetSOA(argFlatId, argType, argAsOn, argCreditDays);
        }
        public static DataSet GetSOADet(int argFlatId,DateTime argAsOn)
        {
            return FlatDetailsDL.GetSOADet(argFlatId, argAsOn);
        }
        public static DataSet GetPBSOADet(int argFlatId, DateTime argAsOn)
        { 
            return FlatDetailsDL.GetPBSOADet(argFlatId, argAsOn);
        }
        public static DataTable GetReceiptTypeTrans(int argFlatId)
        {
            return FlatDetailsDL.GetReceiptTypeTrans(argFlatId);
        }

        public static DataSet GetFlatReceiptType(int argCCId, int argFlatId, DateTime argDate)
        {
            return FlatDetailsDL.GetFlatReceiptType(argCCId, argFlatId, argDate);
        }

        public static DataSet GetAsOnFlatReceiptType(int argCCId, int argFlatId, DateTime argDate)
        {
            return FlatDetailsDL.GetAsOnFlatReceiptType(argCCId, argFlatId, argDate);
        }

        public static DataTable GetLevel()
        {
            return FlatDetailsDL.GetLevel();
        }

        public static void InsertFlatTypeArea(DataTable dtAreatrans, int argFTId)
        {
            FlatDetailsDL.InsertFlatTypeArea(dtAreatrans, argFTId);
        }

        public static void InsertFlatArea(DataTable dtAreatrans, int argFId)
        {
            FlatDetailsDL.InsertFlatArea(dtAreatrans, argFId);
        }

        public static DataTable GetData()
        {
            return FlatDetailsDL.GetData();                    
        }

        public static DataTable GetFlatDetails(int argCCId,string argMode,int argLeadId,string argType, int argEntryId)
        {
            return FlatDetailsDL.GetFlatDetails(argCCId, argMode, argLeadId, argType, argEntryId);
        }
        public static string GetBlockFlat(int argCCId, int argLeadId,string argsType)
        {
            return FlatDetailsDL.GetBlockFlat(argCCId, argLeadId,argsType);           
        }
        public static DataTable GetFromFlatDetails(int argCCId, int argFlatId)
        {
            return FlatDetailsDL.GetFromFlatDetails(argCCId, argFlatId);
        }

        public static DataTable GetToFlatDetails(int argCCId)
        {
            return FlatDetailsDL.GetToFlatDetails(argCCId);
        }

        public static void UpdateFlatTransfer(int argNewFlatId, int argOldFlatId, int argLeadId)
        {
            FlatDetailsDL.UpdateFlatTransfer(argNewFlatId, argOldFlatId, argLeadId);
        }

        public static DataTable GetFlatSoldDetails(int argCCId)
        {
            return FlatDetailsDL.GetFlatSoldDetails(argCCId);
        }

        public static DataTable GetFlatDetailsD(int argFlatId)
        {
            return FlatDetailsDL.GetFlatDetailsD(argFlatId);                    
        }
        public static DataTable GetPlotDetails(int argPlotId)
        {
            return FlatDetailsDL.GetPlotDetails(argPlotId);        
        }
        public static decimal GetPlotQualiAmt(int argPlotId)
        {
            return FlatDetailsDL.GetPlotQualiAmt(argPlotId);        
        }
        public static DataTable GetFlatDet(int argFlatId,int argCCId)
        {
            return FlatDetailsDL.GetFlatDet(argFlatId,argCCId);
        }

        public static DataTable GetBrokerDetailsD(int argCCId)
        {
            return FlatDetailsDL.GetBrokerDetailsD(argCCId);
        }

        public static DataTable GetBrokerComm(int argSORegId, int argCCId, int argBrokerId)
        {
            return FlatDetailsDL.GetBrokerComm(argSORegId, argCCId, argBrokerId);
        }

        public static DataTable GetBankDetailsD()
        {
            return FlatDetailsDL.GetBankDetailsD();
        }

        public static DataTable getBuyFinalDetailsE(int argEntryId)
        {
            return FlatDetailsDL.getBuyFinalDetailsE(argEntryId);
        }

        public static void UpdateFlatDetails(FlatdetailsBL OFlatDetails, DataTable dtAreatrans, DataTable dtSTaxtrans, DataTable dtOCosttrans, DataTable argExtraItemtrans,DataTable dtFCheck)
        {
            FlatDetailsDL.UpdateFlatDetails(OFlatDetails, dtAreatrans, dtSTaxtrans, dtOCosttrans, argExtraItemtrans,dtFCheck);
        }

        public static void UpdateUnitFlatDetails(FlatDetailBO OFlatDetails)
        {
            FlatDetailsDL.UpdateUnitFlatDetails(OFlatDetails);
        }

        public static void InsertFlatDetails(FlatdetailsBL OFlatDetails, DataTable dtAreatrans, DataTable dtSTaxtrans, DataTable dtOCosttrans, DataTable argExtraItemtrans,DataTable dtCheck)
        {
            FlatDetailsDL.InsertFlatDetails(OFlatDetails, dtAreatrans, dtSTaxtrans, dtOCosttrans, argExtraItemtrans,dtCheck);
        }

        public static void InsertUnitFlatDetails(FlatDetailBO OFlatDetails)
        {
            FlatDetailsDL.InsertUnitFlatDetails(OFlatDetails);
        }

        //public static DataSet GetFlatDetailsEdit(int argFlatId,int argCCId)
        //{
        //    return FlatDetailsDL.GetFlatDetailsEdit(argFlatId,argCCId);
        //}

        public static DataTable GetFlatCheckList(int argCCId,int argFlatId)
        {
            return FlatDetailsDL.GetFlatCheckList(argCCId,argFlatId);
        }

        public static DataTable GetFlatTypeEINull(int argCCId)
        {
            return FlatDetailsDL.GetFlatTypeEINull(argCCId);
        }

        public static DataTable GetFlatEINull(int argCCId)
        {
            return FlatDetailsDL.GetFlatEINull(argCCId);
        }

        public static DataTable GetFlatTypeEI(int argFlatTypeId)
        {
            return FlatDetailsDL.GetFlatTypeEI(argFlatTypeId);
        }

        public static DataTable GetFlatEI(int argFlatId)
        {
            return FlatDetailsDL.GetFlatEI(argFlatId);
        }

        public static void UpdateEI(int argFlatId, int argTransId)
        {
            FlatDetailsDL.UpdateEI(argFlatId, argTransId);
        }

        public static void DeleteFlatDetails(int argFlatId,int argCCId)
        {
            FlatDetailsDL.DeleteFlatDetails(argFlatId,argCCId);
        }

        public static DataTable GetFlatArea(int argFlatId)
        {
            return FlatDetailsDL.GetFlatArea(argFlatId);
        }

        public static DataTable GetFlatTypeArea(int argFlatTypeId)
        {
            return FlatDetailsDL.GetFlatTypeArea(argFlatTypeId);
        }

        public static DataTable GetFlatTypeAreaNull()
        {
            return FlatDetailsDL.GetFlatTypeAreaNull();
        }

        public static DataTable GetFlatAreaNull()
        {
            return FlatDetailsDL.GetFlatAreaNull();
        }

        public static DataTable GetPaySchType(int argFlatId,string argType)
        {
            return FlatDetailsDL.GetPaySchType(argFlatId,argType);
        }
        public static DataTable GetPayFlatDetail(int argFlatId, int argCCId)
        {
            return FlatDetailsDL.GetPayFlatDetail(argFlatId, argCCId);
        }
        #endregion

        internal static bool GetTypewise(int argPayTypeId)
        {
            return FlatDetailsDL.GetTypewise(argPayTypeId);
        }
        internal static bool GetBuyerTypewise(int argFlatId)
        {
            return FlatDetailsDL.GetBuyerTypewise(argFlatId);
        }
        internal static DataTable getFinalDetails(int argiFlatId, int argCCId)
        {
            return FlatDetailsDL.getFinalDetails(argiFlatId, argCCId);
        }
        internal static void UpdateFlatQualAmt(int argPayTypeId, int argFlatId, SqlConnection conn, SqlTransaction tran)
        {
            FlatDetailsDL.UpdateFlatQualAmt(argPayTypeId, argFlatId, conn, tran);
        }
        public static void InsertFlatSortOrder(int argCCId, int argFlatId, int argBlockId, int argLevelId, SqlConnection conn, SqlTransaction tran)
        {
            FlatDetailsDL.InsertFlatSortOrder(argCCId, argFlatId,argBlockId,argLevelId, conn, tran);
        }

        #region Reports

        public static DataTable GetAllotmentPrint(int argFlatId, int argCCId)
        {
            return FlatDetailsDL.GetAllotmentPrint(argFlatId, argCCId);
        }

        public static DataTable GetSubAllotmentPrint(int argFlatId)
        {
            return FlatDetailsDL.GetSubAllotmentPrint(argFlatId);
        }

        public static DataTable GetSubAllotPaymentPrint(int argFlatId, string argType)
        {
            return FlatDetailsDL.GetSubAllotPaymentPrint(argFlatId, argType);
        }

        public static DataTable GetSubAllotPaymentHandingOverPrint(int argFlatId)
        {
            return FlatDetailsDL.GetSubAllotPaymentHandingOverPrint(argFlatId);
        }

        public static DataTable GetSubSOAPrint(int argFlatId, DateTime argAsOn)
        {
            return FlatDetailsDL.GetSubSOAPrint(argFlatId, argAsOn);
        }

        #endregion

        #region FlatDetails

        public static DataTable GetAllotmentRegister(int argCCId)
        {
            return FlatDetailsDL.GetAllotmentRegister(argCCId);
        }

        #endregion


        internal static DataTable PopulateChangedBuyerName(int argiCCId)
        {
            return FlatDetailsDL.PopulateChangedBuyerName(argiCCId);
        }

        internal static DataTable GetPaymentBlocks(int m_iCCId)
        {
            return FlatDetailsDL.GetPaymentBlocks(m_iCCId);
        }

        internal static DataTable GetBrokerCommission(int m_iBrokId, int m_iBranchId)
        {
            return FlatDetailsDL.GetBrokerCommission(m_iBrokId, m_iBranchId);
        }

        internal static DataTable PopulateCustomerFeedback(int FlatId)
        {
            return FlatDetailsDL.PopulateCustomerFeedback(FlatId);
        }
    }
}
