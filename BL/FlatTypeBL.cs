using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using CRM.BusinessObjects;
using CRM.DataLayer;

namespace CRM.BusinessLayer
{
    class FlatTypeBL
    {
        #region Objects
  	    #endregion

        #region Constructor
 
        #endregion

        #region Variable
        #endregion
        
        #region Properties

        public DataTable DtExtraItem { get; set; }

        public DataTable DtFlatArea { get; set; }

        public DataTable DTOtherCost { get; set; }

        public DataTable DTCarPrk { get; set; }

        public int TypeId { get; set; }

        public int ProjId { get; set; }

        public int Flag { get; set; }

        public decimal UDSLandFType { get; set; }

        public string Typename { get; set; }

        public decimal Area { get; set; }

        public decimal Rate { get; set; }

        public decimal BaseAmt { get; set; }

        public decimal OtherCostAmt { get; set; }

        public decimal TotalCarPark { get; set; }

        public int NoOfCarPark { set; get;}

        public decimal AdvAmt { set; get; }

        public decimal LandRate { set; get; }

        public decimal LandAmt { set; get; }

        public decimal ExtraBillAmt { get; set; }

        public decimal VatAmt { get; set; }

        public decimal SerTaxAmt { get; set; }

        public decimal NetAmt { get; set; }

        public bool IncludePaySch { get; set; }

        public string Reminder { get; set; }

        public string Remarks { get; set; }

        public decimal STaxPer { get; set; }

        public decimal VatPer { get; set; }


            #endregion

        #region Methods

        //public void Update()
        //{
        //    FlatTypeDL.Update(this );
        //};

        public static DataTable GetProject()
        {
              return FlatTypeDL.GetProject();
        }

        public static DataTable GetType(int projId, string typeName)
        {
             return FlatTypeDL.GetType(projId, typeName);
        }

        public static DataTable GetPayReport(int argCCId, int argPayTypeId)
        {
            return FlatTypeDL.GetPayReport(argCCId,argPayTypeId);
        }

        public static DataTable GetLevel(int argCCId)
        {
            return FlatTypeDL.GetLevel(argCCId);
        }

        public static DataTable GetFlat(int argCCId, int argFlatTypeId, int argBlockId, int argLevelId)
        {
            return FlatTypeDL.GetFlat(argCCId, argFlatTypeId, argBlockId, argLevelId);
        }

        public static bool GetFloorValue(int argCCId, int argFlatTypeId)
        {
            return FlatTypeDL.GetFloorValue(argCCId, argFlatTypeId);
        }

        public static void UpdateFloorValue(int argFlatTypeId)
        {
            FlatTypeDL.UpdateFloorValue(argFlatTypeId);
        }

        public static DataTable GetFTLevel(int argCCId)
        {
            return FlatTypeDL.GetFTLevel(argCCId);
        }

        public static DataTable GetFTLevelwise(string arg_BId, string arg_LId,int argCCId)
        {
            return FlatTypeDL.GetFTLevelwise(arg_BId,arg_LId,argCCId);
        }

        public static DataTable GetBlock(int argCCId)
        {
            return FlatTypeDL.GetBlock(argCCId);
        }       

        public static DataTable GetFlatBlock(int argCCId)
        {
            return FlatTypeDL.GetFlatBlock(argCCId);
        }

        public static DataTable GetBlockCheck(int argCCId)
        {
            return FlatTypeDL.GetBlockCheck(argCCId);
        }

        public static DataTable GetFlatCheck()
        {
            return FlatTypeDL.GetFlatCheck();
        } 

        public static DataTable GetFTBlock(int argCCId)
        {
            return FlatTypeDL.GetFTBlock(argCCId);
        }

        public static DataTable GetFTBlockwise(int argCCId,string arg_BId)
        {
            return FlatTypeDL.GetFTBlockwise(argCCId,arg_BId);
        }

        public static DataTable GetFlatType(int argFlatTypeId)
        {
            return FlatTypeDL.GetFlatType(argFlatTypeId);
        }

        public static DataTable GetFlat_Type(int argCCId)
        {
            return FlatTypeDL.GetFlat_Type(argCCId);
        }

        public static DataTable GetFlatTypeDetails(int argProjId)
        {
            return FlatTypeDL.GetFlatTypeDetails(argProjId);
        }
        public static DataTable GetLeadFlatTypeDetails(int argProjId, string argsType, int argLandId)
        {
            return FlatTypeDL.GetLeadFlatTypeDetails(argProjId, argsType, argLandId);
        }
        public static DataTable GetFlatTypeRateDetails(int argProjId)
        {
            return FlatTypeDL.GetFlatTypeRateDetails(argProjId);
        }
        public static DataTable GetFlatTypeRateHistory(int argProjId, string argDate, string argType)
        {
            return FlatTypeDL.GetFlatTypeRateHistory(argProjId,argDate,argType);
        }
        public static DataTable GetCarDetails()
        {
            return FlatTypeDL.GetCarDetails();
        }
        public static void InsertChangeRate(DataTable argdt,string argDate,DataTable argdtF)
        {
            FlatTypeDL.InsertChangeRate(argdt,argDate,argdtF);
        }
        public static DataTable GetFTType(int argProjId)
        {
            return FlatTypeDL.GetFTType(argProjId);
        }

        public static DataTable GetFTTypewise(string argTId)
        {
            return FlatTypeDL.GetFTTypewise(argTId);
        }

        public static bool GetAreaFound(int argFlatTypeId)
        {
            return FlatTypeDL.GetAreaFound(argFlatTypeId);
        }

        public static bool GetFlatAreaFound(int argFlatId)
        {
            return FlatTypeDL.GetFlatAreaFound(argFlatId);
        }

        public static DataTable GetAddi(int argCCId)
        {
            return FlatTypeDL.GetAddi(argCCId);
        }       

        public static DataTable GetAddiItem()
        {
            return FlatTypeDL.GetAddiItem();
        }

        public static void UpdateFlatTypeDetails(FlatTypeBL OFlatType, DataTable dtAreatrans, DataTable dtSTaxtrans, DataTable dtOCosttrans, ArrayList argExtraItemtrans,DataTable dtCheck)
        {
            FlatTypeDL.UpdateFlatTypeDetails(OFlatType,dtAreatrans,dtSTaxtrans,dtOCosttrans,argExtraItemtrans,dtCheck);
        }

        public static void UpdateUnitFTDetails(int argCCId,FlatTypeBO OUnitDirBO)
        {
            FlatTypeDL.UpdateUnitFTDetails(argCCId,OUnitDirBO);
        }

        public static void InsertFlatTypeDetails(int blockid, FlatTypeBL OFlatType, DataTable dtAreatrans, DataTable dtSTaxtrans, DataTable dtOCosttrans, ArrayList argExtraItemtrans, DataTable dtCheck)
        {
            FlatTypeDL.InsertFlatTypeDetails(blockid,OFlatType, dtAreatrans, dtSTaxtrans, dtOCosttrans, argExtraItemtrans,dtCheck);
        }

        public static int InsertUnitFTDetails(int argCCId, string argTypeName)
        {
            return FlatTypeDL.InsertUnitFTDetails(argCCId, argTypeName);
        }

        public static DataSet GetFlatTypeDetailsEdit(int argFlatTypeId, int argCCId, int argFlatId)
        {
            return FlatTypeDL.GetFlatTypeDetailsEdit(argFlatTypeId, argCCId, argFlatId);
        }

        public static DataTable PopulateFlatType(DataTable dtT, int argCCId)
        {
            return FlatTypeDL.PopulateFlatType(dtT,argCCId);
        }

        public static DataTable GetCostSheet(int argCCId,bool argTypewise)
        {
            return FlatTypeDL.GetCostSheet(argCCId,argTypewise);
        }

        public static DataTable GetAECostSheet(int argCCId, bool argTypewise, DateTime argDate)
        {
            return FlatTypeDL.GetAECostSheet(argCCId, argTypewise, argDate);
        }

        public static DataTable GetBWAECostSheet(int argCCId, bool argTypewise, DateTime argFromDate, DateTime argTo)
        {
            return FlatTypeDL.GetBWAECostSheet(argCCId, argTypewise, argFromDate, argTo);
        }

        public static DataTable BuyerGetCostSheet(int argCCId, bool argTypewise, int argFlatId)
        {
            return FlatTypeDL.BuyerGetCostSheet(argCCId, argTypewise, argFlatId);
        }
        
        public static DataTable GetBuyerTermSheetUnit(int argCCId, bool argTypewise, int argFlatId)
        {
            return FlatTypeDL.GetBuyerTermSheetUnit(argCCId, argTypewise, argFlatId);
        }

        public static DataTable GetBuyerTermSheetOC(int argCCId, bool argTypewise, int argFlatId)
        {
            return FlatTypeDL.GetBuyerTermSheetOC(argCCId, argTypewise, argFlatId);
        }

        public static DataTable GetBuyerTermSheetPayment(int argCCId, bool argTypewise, int argFlatId)
        {
            return FlatTypeDL.GetBuyerTermSheetPayment(argCCId, argTypewise, argFlatId);
        }

        public static DataTable GetOpCostCentre()
        {
            return FlatTypeDL.GetOpCostCentre();
        }

        #endregion

    }
}
