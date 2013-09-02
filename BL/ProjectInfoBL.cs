using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DL;
using CRM.BO;
using System.Data.SqlClient;

namespace CRM.BL
{
    class ProjectInfoBL
    {
        #region Proj Info

        public static void InsertProjInfo(ProjectInfoBO ProjInfoBO)
        {
            ProjectInfoDL.InsertProjInfo(ProjInfoBO);
        }

        public static void UpdateProjInfo(ProjectInfoBO ProjInfoBO)
        {
            ProjectInfoDL.UpdateProjInfo(ProjInfoBO);
        }

        public static DataTable PopulateProjInfo(ProjectInfoBO ProjInfoBO)
        {
            return ProjectInfoDL.PopulateProjInfo(ProjInfoBO);
        }

        internal static string UnitUsed(int argCCId)
        {
            return ProjectInfoDL.UnitUsed(argCCId);
        }

        internal static DataTable GetPenalty(int argCCId)
        {
            return ProjectInfoDL.GetPenalty(argCCId);
        }
        internal static void UpdatePenalty(int argCCId, string argBlkType, decimal argBlkAmt, string argBType, decimal argBAmt, string argFType, decimal argFAmt)
        {
            ProjectInfoDL.UpdatePenalty(argCCId, argBlkType, argBlkAmt, argBType, argBAmt, argFType, argFAmt);
        }
        #endregion

        #region NBS Master

        public static DataTable SelectServices(int argCCID, string argType)
        {
            return ProjectInfoDL.SelectServices(argCCID, argType);
        }

        public static DataTable SelectServicesMaster()
        {
            return ProjectInfoDL.SelectServicesMaster();
        }

        public static DataTable SelectServicesTrans(ProjectInfoBO NBSBO)
        {
            return ProjectInfoDL.SelectServicesTrans(NBSBO);
        }

        public static void InsertNBSMaster(string argName)
        {
            ProjectInfoDL.InsertNBSMaster(argName);
        }

        public static void InsertNBS(DataTable argdt, int argCCID, string argType)
        {
            ProjectInfoDL.InsertNBS(argdt,argCCID,argType);
        }

        public static void DeleteNBSMaster(ProjectInfoBO NBSBO)
        {
            ProjectInfoDL.DeleteNBSMaster(NBSBO);
        }

        public static void DeleteNBS(ProjectInfoBO NBSBO)
        {
            ProjectInfoDL.DeleteNBS(NBSBO);
        }

        #endregion

        #region Amenities Master

        public static DataTable SelectAmenities(int argCCID, string argType)
        {
            return ProjectInfoDL.SelectAmenities(argCCID,argType);
        }

        public static DataTable SelectAmenitiesMaster()
        {
            return ProjectInfoDL.SelectAmenitiesMaster();
        }

        public static DataTable SelectAmenitiesTrans(ProjectInfoBO AmenityBO)
        {
            return ProjectInfoDL.SelectAmenitiesTrans(AmenityBO);
        }

        public static void InsertAmenitiesMaster(string argName)
        {
            ProjectInfoDL.InsertAmenitiesMaster(argName);
        }

        public static void InsertAmenities(DataTable argdt, int argCCID, string argType)
        {
            ProjectInfoDL.InsertAmenities(argdt,argCCID,argType);
        }

        public static void DeleteAmenitiesMaster(int argAId)
        {
            ProjectInfoDL.DeleteAmenitiesMaster(argAId);
        }

        public static void DeleteAmenities(ProjectInfoBO AmenityBO)
        {
            ProjectInfoDL.DeleteAmenities(AmenityBO);
        }

        #endregion

        #region Competitor

        public static DataTable SelectCompetitor(ProjectInfoBO CompBO)
        {
            return ProjectInfoDL.SelectCompetitor(CompBO);
        }

        public static DataTable SelectCompetitorTrans(ProjectInfoBO CompBO)
        {
            return ProjectInfoDL.SelectCompetitorTrans(CompBO);
        }

        public static void InsertCompetitor(ProjectInfoBO CompBO)
        {
            ProjectInfoDL.InsertCompetitor(CompBO);
        }

        public static void DeleteCompetitor(ProjectInfoBO CompBO)
        {
            ProjectInfoDL.DeleteCompetitor(CompBO);
        }

        #endregion

        #region CarPark

        public static void UpdateCarParkSlot(int argBlockId, int argCCID, SqlConnection argConn, SqlTransaction argTrans)
        {
            ProjectInfoDL.UpdateCarParkSlot(argBlockId, argCCID, argConn, argTrans);
        }

        public static void UpdateCarParkSlotAllot(int argBlockId, int argCCID)
        {
            ProjectInfoDL.UpdateCarParkSlotAllot(argBlockId, argCCID);
        }

        #endregion

        #region OtherCost

        internal static DataTable SelectOtherCost(string argNew,int argbArea,int argCCId)
        {
            return ProjectInfoDL.SelectOtherCost(argNew,argbArea,argCCId);
        }

        internal static DataTable GetOtherCost(int argCCId)
        {
            return ProjectInfoDL.GetOtherCost(argCCId);
        }

        internal static DataTable GetOtherCostType()
        {
            return ProjectInfoDL.GetOtherCostType();
        }

        internal static DataTable GetOCTypeMaster()
        {
            return ProjectInfoDL.GetOCTypeMaster();
        }

        public static void UpdateOC(int argCCID, DataTable argdt)
        {
            ProjectInfoDL.UpdateOC(argCCID, argdt);
        }

        internal static DataTable GetServiceOtherCost()
        {
            return ProjectInfoDL.GetServiceOtherCost();
        }

        internal static DataTable GetExcludeOtherCost(int argCCId)
        {
            return ProjectInfoDL.GetExcludeOtherCost(argCCId);
        }

        internal static void UpdateServiceOC(DataTable argdt,string argType,int argCCId)
        {
            ProjectInfoDL.UpdateServiceOC(argdt,argType,argCCId);
        }

        internal static void UpdateOCType(DataTable argdt)
        {
            ProjectInfoDL.UpdateOCType(argdt);
        }

        public static void UpdateSortOrder(DataTable dt)
        {
            ProjectInfoDL.UpdateSortOrder(dt);
        }

        #endregion


        internal static bool CheckUsedInFlatsPayment(int argCCId, int argOCId)
        {
            return ProjectInfoDL.CheckUsedInFlatsPayment(argCCId, argOCId);
        }
    }
}
