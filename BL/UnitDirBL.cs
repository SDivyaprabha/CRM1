using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using CRM.DataLayer;

namespace CRM.BusinessLayer
{
    class UnitDirBL
    {
        #region Objects
        #endregion

        #region Properties
        public int CCId { set; get; }

        public string RefNo { get; set; }

        public int BlockId { set; get; }

        public int LevelId { set; get; }

        public string SchType { get; set; }

        public int StageId { set; get; }

        public DateTime StageDate { set; get; }

        public DateTime CompletionDate { set; get; }

        public DateTime DueDate { set; get; }

        public string Remarks { set; get; }

        #endregion

        #region Methods

        public static DataTable GetCC()
        {
            return UnitDirDL.GetCC();
        }

        public static void UpdateDefaultDatas()
        {
            UnitDirDL.UpdateDefaultDatas();
        }

        public static DataTable Concep(string argPDB)
        {
            return UnitDirDL.Concep(argPDB);
        }

        public static DataTable GetFlats(int argCCId, int argBlockId, int argTypeId,int argFlatId)
        {
            return UnitDirDL.GetFlats(argCCId, argBlockId, argTypeId, argFlatId);
        }

        public static string FoundCancelDate(int argId)
        {
            return UnitDirDL.FoundCancelDate(argId);
        }

        public static bool FoundStatus(int argFlatId)
        {
            return UnitDirDL.FoundStatus(argFlatId);
        }

        public static bool FoundCallType(int argCTId, string argDes, string argType)
        {
            return UnitDirDL.FoundCallType(argCTId,argDes, argType);
        }

        public static DataTable GuideLine(int argCCId)
        {
            return UnitDirDL.GuideLine(argCCId);
        }

        public static void UpdateUnitRateChange(DataTable dtFlat)
        {
            UnitDirDL.UpdateUnitRateChange(dtFlat);
        }

        public static void UpdateReceiptUnitRateChange(DataTable dtFlat)
        {
            UnitDirDL.UpdateReceiptUnitRateChange(dtFlat);
        }

        public static DataTable GetUnitRateChange(int argCCId,int argFlatTypeId,string argType,string argFilterType)
        {
            return UnitDirDL.GetUnitRateChange(argCCId,argFlatTypeId,argType,argFilterType);
        }

        public static DataTable UOM()
        {
            return UnitDirDL.UOM();
        }

        public static decimal GetNetAmt(int argFlatId)
        {
            return UnitDirDL.GetNetAmt(argFlatId);
        }

        public static decimal GetQualiAmt(int argFlatId,bool argType)
        {
            return UnitDirDL.GetQualiAmt(argFlatId,argType);
        }

        public static DataTable GetQualifier(int argFlatId)
        {
            return UnitDirDL.GetQualifier(argFlatId);
        }

        public static decimal GetQualifierAmt(int argFlatId,bool argType)
        {
            return UnitDirDL.GetQualifierAmt(argFlatId,argType);
        }

        public static decimal GetGlobalQualifierAmt(int argFlatId, bool argType, SqlConnection conn, SqlTransaction tran)
        {
            return UnitDirDL.GetGlobalQualifierAmt(argFlatId, argType, conn, tran);
        }

        public static int FoundUOM(int argCCId)
        {
            return UnitDirDL.FoundUOM(argCCId);
        }

        public static void UpdateUOM(int argCCId, int argUnitId)
        {
            UnitDirDL.UpdateUOM(argCCId, argUnitId);
        }

        public static DataTable Executive()
        {
            return UnitDirDL.Executive();
        }

        //public static DataTable FlatCheckListNull()
        //{
        //    return UnitDirDL.FlatCheckListNull();
        //}
        //public static DataTable FlatTypeCheckListNull()
        //{
        //    return UnitDirDL.FlatTypeCheckListNull();
        //}

        public static DataTable FlatCheckList(int argFlatId,string argType)
        {
            return UnitDirDL.FlatCheckList(argFlatId,argType);
        }

        public static string FlatRegCheckList(int argFlatId, string argType)
        {
            return UnitDirDL.FlatRegCheckList(argFlatId, argType);
        }

        public static DataTable FlatBuyer(int argFlatId)
        {
            return UnitDirDL.FlatBuyer(argFlatId);
        }

        public static DataTable FlatTypeCheckList(int argFlatTypeId,string argType,int argCCId)
        {
            return UnitDirDL.FlatTypeCheckList(argFlatTypeId,argType,argCCId);
        }
        
        public static DataTable ProjectCheckList(int argCCId)
        {
            return UnitDirDL.ProjectCheckList(argCCId);
        }

        public static DataTable ProjectCheckListFlat(int argCCId, string argType)
        {
            return UnitDirDL.ProjectCheckListFlat(argCCId,argType);
        }

        public static DataTable FinalCheckListFlat(int argFlatId)
        {
            return UnitDirDL.FinalCheckListFlat(argFlatId);
        }

        public static DataTable FinalCheckListPlot(int argPlotId)
        {
            return UnitDirDL.FinalCheckListPlot(argPlotId);
        }

        public static DataTable FinalPlot(int argPlotId)
        {
            return UnitDirDL.FinalPlot(argPlotId);
        }

        public static DataTable CancelCheckListFlat(int argFlatId)
        {
            return UnitDirDL.CancelCheckListFlat(argFlatId);
        }

        public static void InsertProjCheckList(DataTable dt, int argCCId)
        {
            UnitDirDL.InsertProjCheckList(dt, argCCId);
        }

        public static void InsertProjCheckListFlat(DataTable dt, int argCCId, string argType)
        {
            UnitDirDL.InsertProjCheckListFlat(dt, argCCId,argType);
        }

        public static void InsertFlatTypeCheckList(DataTable dt, int argFlatTypeId,string argType)
        {
            UnitDirDL.InsertFlatTypeCheckList(dt, argFlatTypeId,argType);
        }

        public static void InsertFlatCheckList(DataTable dt, int argFlatId, string argType, bool argChkSend, int argCCId, string argFlatNo)
        {
            UnitDirDL.InsertFlatCheckList(dt, argFlatId, argType, argChkSend, argCCId, argFlatNo);
        }

        public static void UpdateBuyerName(int argLeadId, string argLeadName, string argCoAppName)
        {
            UnitDirDL.UpdateBuyerName(argLeadId, argLeadName, argCoAppName);
        }

        public static DataTable PaySchType()
        {
            return UnitDirDL.PaySchType();
        }

        public static DataTable PlotPaySchType()
        {
            return UnitDirDL.PlotPaySchType();
        }

        public static DataTable PaymentSchType()
        {
            return UnitDirDL.PaymentSchType();
        }

        public static DataTable PayAmount(int argFlatTypeId)
        {
            return UnitDirDL.PayAmount(argFlatTypeId);
        }

        public static DataTable PayOC(int argFlatTypeId)
        {
            return UnitDirDL.PayOC(argFlatTypeId);
        }

        public static DataTable PayOCSetup(int argCCId)
        {
            return UnitDirDL.PayOCSetup(argCCId);
        }

        public static void FlatTemplate(DataTable dtTwise, int argCCId, string start, string block, string level, string type)
        {
           UnitDirDL.FlatTemplate(dtTwise, argCCId, start, block,level,type);
        }

        public static void GenerateFlat(DataTable dtT, DataTable dtTemp, int argCCId, string l, string m, string n, string o)
        {
           UnitDirDL.GenerateFlat(dtT, dtTemp, argCCId,l,m,n,o);
        }

        public static void InsertStage(UnitDirBL OUintDirBL)
        {
            UnitDirDL.InsertStage(OUintDirBL);
        }

        public static void UpdateStage(UnitDirBL OUintDirBL, int argStageDetId)
        {
            UnitDirDL.UpdateStage(OUintDirBL, argStageDetId);
        }

        internal static void UpdateRefreshStage()
        {
            UnitDirDL.UpdateRefreshStage();
        }

        public static DataTable GetStage(string argSchType,int argCCId)
        {
            return UnitDirDL.GetStage(argSchType,argCCId);
        }

        public static DataTable GetStageDetails(int argCCId)
        {
            return UnitDirDL.GetStageDetails(argCCId);
        }

        public static DataTable GetChangeGridStageDetails(int argStageDetId)
        {
            return UnitDirDL.GetChangeGridStageDetails(argStageDetId);
        }

        public static DataTable GetEditStgDetails(int argStgId)
        {
            return UnitDirDL.GetEditStgDetails(argStgId);
        }

        public static bool CheckPBStage(int argStageDetId)
        {
            return UnitDirDL.CheckPBStage(argStageDetId);
        }

        public static void DeleteStage(int argStageDetId)
        {
            UnitDirDL.DeleteStage(argStageDetId);
        }

        public static void InsertEItem(int argCCId)
        {
            UnitDirDL.InsertEItem(argCCId);
        }

        public static DataTable GetEItem(int argCCId)
        {
            return UnitDirDL.GetEItem(argCCId);
        }

        public static DataTable GetCar(int argTypeId,int argCCId)
        {
            return UnitDirDL.GetCar(argTypeId, argCCId);
        }

        public static DataTable GetBlockWiseUDS(int argCCId)
        {
            return UnitDirDL.GetBlockWiseUDS(argCCId);
        }

        public static DataSet GetBlockWiseUDSReport(int argCCId)
        {
            return UnitDirDL.GetBlockWiseUDSReport(argCCId);
        }

        public static void InsertUDS(DataTable argdt,int argCCId)
        {
            UnitDirDL.InsertUDS(argdt,argCCId);
        }

        public static void InsertFlatUDS(DataTable argdt, int argCCId)
        {
            UnitDirDL.InsertFlatUDS(argdt, argCCId);
        }

        public static bool GetUDS(int argCCId)
        {
            return UnitDirDL.GetUDS(argCCId); 
        }

        public static void InsertProjectwiseFlatUDS(int argCCId)
        {
            UnitDirDL.InsertProjectwiseFlatUDS(argCCId);
        }

        public static DataTable GetBlockFlats(int argCCId)
        {
            return UnitDirDL.GetBlockFlats(argCCId);
        }

        public static void UpdateBlockFlats(DataTable argdt)
        {
            UnitDirDL.UpdateBlockFlats(argdt);
        }

        public static DataTable GetCarTagName(int argCCId, int argBlockId, int argTypeId)
        {
            return UnitDirDL.GetCarTagName(argCCId, argBlockId, argTypeId);
        }

        public static void InsertFlatTypeCar(DataTable argdt)
        {
            UnitDirDL.InsertFlatTypeCar(argdt);
        }

        public static void UpdateRegistrationFlat(int argFlatId, decimal argAmt)
        {
            UnitDirDL.UpdateRegistrationFlat(argFlatId, argAmt);
        }

        public static void UpdateRegistrationFlatType(int argFlatTypeId, decimal argAmt)
        {
            UnitDirDL.UpdateRegistrationFlatType(argFlatTypeId, argAmt);
        }

        public static decimal GetOtherCostflatType(int argflatTypeID)
        {
            return UnitDirDL.GetOtherCostflatType(argflatTypeID);
        }

        public static decimal GetOtherCostFlat(int argFlatID)
        {
            return UnitDirDL.GetOtherCostFlat(argFlatID);
        }

        public static void InsertFlatCar(DataTable argdt)
        {
            UnitDirDL.InsertFlatCar(argdt);
        }

        public static DataTable GetFlatTypeCarPark(int argCCId, int argFlatTypeId, int argBlockId)
        {
            return UnitDirDL.GetFlatTypeCarPark(argCCId, argFlatTypeId, argBlockId);
        }

        public static DataTable GetFlatCarPark(int argCCId, int argFlatId, int argBlockId)
        {
            return UnitDirDL.GetFlatCarPark(argCCId, argFlatId, argBlockId);
        }

        public static DataTable GetFloorRate(int argCCId, int argFlatTypeId)
        {
            return UnitDirDL.GetFloorRate(argCCId,argFlatTypeId);
        }

        public static DataTable GetFloorChangeRate(int argCCId, int argFlatTypeId)
        {
            return UnitDirDL.GetFloorChangeRate(argCCId, argFlatTypeId);
        }

        public static DataTable GetFloorRate(int argCCId)
        {
            return UnitDirDL.GetFloorRate(argCCId);
        }

        public static void InsertFloorRate(DataTable argdt, int argFlatTypeId)
        {
            UnitDirDL.InsertFloorRate(argdt, argFlatTypeId);
        }

        public static DataTable GetCarCost(int argCCId, int argTypeId)
        {
            return UnitDirDL.GetCarCost(argCCId, argTypeId);
        }

        public static void InsertCar(DataTable argdt, int argCCId)
        {
            UnitDirDL.InsertCar(argdt,argCCId);
        }

        public static DataTable GetApplyOthers(int argCCId)
        {
           return UnitDirDL.GetApplyOthers(argCCId);
        }

        public static void ApplyOthersTransaction(DataTable argDt,int argChkId,string argExpDate)
        {
            UnitDirDL.ApplyOthersTransaction(argDt, argChkId, argExpDate);
        }

        internal static bool BrokerFound(int argBrokerId)
        {
            return UnitDirDL.BrokerFound(argBrokerId);
        }

        public static void InsertFlatCar(int argFlatId, int CCId,SqlConnection conn, SqlTransaction tran)
        {
            UnitDirDL.InsertFlatCar(argFlatId, CCId, conn, tran);
        }

        public static void InsertCarSlots(DataTable argdt, int argCCId, int argBlockId, int argTypeId, int argSlots)
        {
            UnitDirDL.InsertCarSlots(argdt, argCCId, argBlockId, argTypeId, argSlots);
        }

        public static void InsertCarParkSlots(int argCCId, int argBlockId, int argTypeId, int argSlots)
        {
            UnitDirDL.InsertCarParkSlots(argCCId, argBlockId, argTypeId, argSlots);
        }

        public static void UpdateCarSlots(DataTable argdt, int argCCId, int argBlockId, int argTypeId, int argSlots)
        {
            UnitDirDL.UpdateCarSlots(argdt, argCCId, argBlockId, argTypeId, argSlots);
        }

        public static void DeleteCarSlots(int argCCId, int argBlockId, int argTypeId, int argSlots)
        {
            UnitDirDL.DeleteCarSlots(argCCId, argBlockId, argTypeId, argSlots);
        }

        public static DataTable GetCarParkAllot(int argBlockId, int argCCId)
        {
           return UnitDirDL.GetCarParkAllot(argBlockId, argCCId);
        }

        public static DataTable GetCarAllot(int argBlockId, int argCCId, int argFlatId)
        {
            return UnitDirDL.GetCarAllot(argBlockId, argCCId, argFlatId);
        }

        public static DataTable GetCarParkAllotFlat(int argFlatId)
        {
            return UnitDirDL.GetCarParkAllotFlat(argFlatId);
        }

        public static DataTable GetBlock(int argCCId)
        {
            return UnitDirDL.GetBlock(argCCId);
        }

        public static DataTable GetSlots(int argCCId, int argBlockId, int argTypeId)
        {
            return UnitDirDL.GetSlots(argCCId, argBlockId, argTypeId);
        }

        public static void UpdateFlatSeletion(int argCCId, int argBlockId, int argTypeId, int argSlotNo, int argOldFlatId, int argNewFlatId)
        {
            UnitDirDL.UpdateFlatSeletion(argCCId, argBlockId, argTypeId, argSlotNo, argOldFlatId, argNewFlatId);
        }

        public static DataTable GetFacing(int argCCId)
        {
            return UnitDirDL.GetFacing(argCCId);
        }

        public static int InsertFacing(string argDesc, int argCCId)
        {
            return UnitDirDL.InsertFacing(argDesc, argCCId);
        }

        public static void UpdateFacing(string argDesc, int argFacId,int argCCId)
        {
            UnitDirDL.UpdateFacing(argDesc, argFacId,argCCId);
        }

        public static bool FacingFound(int argFacId)
        {
            return UnitDirDL.FacingFound(argFacId);
        }

        public static void DeleteFacing(int argCCId, int argFacId)
        {
            UnitDirDL.DeleteFacing(argCCId,argFacId);
        }

        public static void InsertLandRateChange(bool argGLV,bool argMLV,bool argReg,int argBlockId)
        {
            UnitDirDL.InsertLandRateChange(argGLV, argMLV, argReg,argBlockId);
        }

        public static string FoundDate()
        {
            return UnitDirDL.FoundDate();
        }

        public static string FoundDateLand()
        {
            return UnitDirDL.FoundDateLand();
        }

        public static DataTable GetCancelPenalty(int argCCId,int argFlatId)
        {
            return UnitDirDL.GetCancelPenalty(argCCId,argFlatId);
        }

        public static DataTable GetCancelPenaltyEdit(int argFlatId)
        {
            return UnitDirDL.GetCancelPenaltyEdit(argFlatId);
        }

        public static bool FoundProgressBill(int argFlatId)
        {
            return UnitDirDL.FoundProgressBill(argFlatId);
        }

        public static DataSet GetLoanReport()
        {
            return UnitDirDL.GetLoanReport();
        }

        public static DataSet GetSalesReport(int argProjId)
        {
            return UnitDirDL.GetSalesReport(argProjId);
        }

        public static DataTable GetTimeLineReport(int argProjId)
        {
            return UnitDirDL.GetTimeLineReport(argProjId);
        }

        public static DataTable GetCostCentre()
        {
            return UnitDirDL.GetCostCentre();
        }

        public static DataTable GetArea(string argEAreaId)
        {
            return UnitDirDL.GetArea(argEAreaId);
        }

        public static DataTable GetAreaMaster()
        {
            return UnitDirDL.GetAreaMaster();
        }

        public static void InsertAreaMaster(string argDesc)
        {
            UnitDirDL.InsertAreaMaster(argDesc);
        }

        public static void UpdateAreaMaster(int argAreaId, string argDesc)
        {
            UnitDirDL.UpdateAreaMaster(argAreaId,argDesc);
        }

        public static void DeleteAreaMaster(int argAreaId)
        {
            UnitDirDL.DeleteAreaMaster(argAreaId);
        }

        public static bool FoundArea(int argAreaId)
        {
            return UnitDirDL.FoundArea(argAreaId);
        }


        #endregion

        #region BlockCancelUnits

        public static DataTable GetBlockUnits(int argFlatId)
        {
            return UnitDirDL.GetBlockUnits(argFlatId);
        }

        public static void InsertBlockUnits()
        {
            UnitDirDL.InsertBlockUnits();
        }

        public static DataTable GetBlockLead(int argCCId, string argType)
        {
            return UnitDirDL.GetBlockLead(argCCId, argType);
        }

        public static void UpdateUnBlockUnits()
        {
            UnitDirDL.UpdateUnBlockUnits();
        }

        public static DataTable GetCancelUnits()
        {
            return UnitDirDL.GetCancelUnits();
        }

        #endregion

        #region CarPark

        public static bool FoundCP(int argTypeId)
        {
            return UnitDirDL.FoundCP(argTypeId);
        }

        public static void UpdateUnitChangeCP(int argFlatId, int argCCId,int argNoCP,decimal argAmt)
        {
            UnitDirDL.UpdateUnitChangeCP(argFlatId, argCCId,argNoCP,argAmt);
        }

        #endregion

        #region PaymentInfo

        internal static DataTable GetFiscalYear()
        {
            return UnitDirDL.GetFiscalYear();
        }

        internal static DataTable GetFlatPaymentInfo(int argFlatId, DateTime argFrom, DateTime argTo)
        {
            return UnitDirDL.GetFlatPaymentInfo(argFlatId, argFrom, argTo);
        }

        internal static DataTable GetFlatPaymentwiseInfo(int argFlatId)
        {
            return UnitDirDL.GetFlatPaymentwiseInfo(argFlatId);
        }

        #endregion

        #region OCGroup

        internal static DataTable GetOCGMaster()
        {
            return UnitDirDL.GetOCGMaster();
        }

        internal static void InsertOCG(string argDesc)
        {
            UnitDirDL.InsertOCG(argDesc);
        }

        public static void UpdateOCG(int argGroupId, string argDesc)
        {
            UnitDirDL.UpdateOCG(argGroupId, argDesc);
        }

        public static void DeleteOCG(int argGroupId)
        {
            UnitDirDL.DeleteOCG(argGroupId);
        }

        public static bool FoundOCG(int argGroupId)
        {
            return UnitDirDL.FoundOCG(argGroupId);
        }

        public static bool FoundOCGName(string argGroupName)
        {
            return UnitDirDL.FoundOCGName(argGroupName);
        }

        internal static DataTable GetOCGList()
        {
            return UnitDirDL.GetOCGList();
        }

        internal static void UpdateOCGList(DataTable argdt)
        {
            UnitDirDL.UpdateOCGList(argdt);
        }

        #endregion

        #region Reports

        internal static DataTable GetReceiptDet(int argFlatId)
        {
            return UnitDirDL.GetReceiptDet(argFlatId);
        }

        #endregion

        #region OtherCost

        public static decimal GetFlatTypeOCAmt(int argCCId, int argFTId, int argPayTypeId)
        {
            return UnitDirDL.GetFlatTypeOCAmt(argCCId, argFTId, argPayTypeId);
        }

        public static decimal GetFlatOCAmt(int argCCId, int argFlatId, int argPayTypeId)
        {
            return UnitDirDL.GetFlatOCAmt(argCCId, argFlatId, argPayTypeId);
        }

        #endregion


        internal static bool CheckReceipt(int m_iFlatId)
        {
            return UnitDirDL.CheckReceipt(m_iFlatId);
        }
    }
}
