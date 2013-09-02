using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DL;
using CRM.BO;

namespace CRM.BL
{
    class StageDetBL
    {
        #region Methods

        public static DataTable PopulateBlock(int argCCId)
        {
            return StageDetDL.PopulateBlock(argCCId);
        }

        public static DataTable PopulateStages(int argCCId)
        {
            return StageDetDL.PopulateStages(argCCId);
        }
        internal static void UpdateBlockSort(DataTable dt,string argType,int argCCId)
        {
            StageDetDL.UpdateBlockSort(dt,argType,argCCId);
        }

        public static DataTable PopulateLevel(int argCCId)
        {
            return StageDetDL.PopulateLevel(argCCId);
        }

        public static DataTable PopulateStageBlock(int i_BlockId, int argCCId)
        {
            return StageDetDL.PopulateStageBlock(i_BlockId, argCCId);
        }

        public static DataTable PopulateStageLevel(int i_LevelId, int argCCId)
        {
            return StageDetDL.PopulateStageLevel(i_LevelId, argCCId);
        }

        public static DataTable PopulateLevelBlock(int i_BlockId, int argCCId)
        {
            return StageDetDL.PopulateLevelBlock(i_BlockId, argCCId);
        }

        internal static DataTable PopulateBlockLevel(int i_BlockId, int argCCId)
        {
            return StageDetDL.PopulateBlockLevel(i_BlockId, argCCId);
        }

        public static void DeleteStageBlock(int i_SBID, int i_BlockId)
        {
            StageDetDL.DeleteStageBlock(i_SBID,i_BlockId);
        }

        public static void DeleteStageLevel(int i_SLID,int i_LevelId)
        {
            StageDetDL.DeleteStageLevel(i_SLID, i_LevelId);
        }

        public static void DeleteLevelBlock(StageDetBO BOstageDet)
        {
            StageDetDL.DeleteLevelBlock(BOstageDet);
        }

        public static void DeletBlock(int i_BID,int argCCId)
        {
            StageDetDL.DeletBlock(i_BID,argCCId);
        }

        public static void DeletStage(int i_SID)
        {
            StageDetDL.DeletStage(i_SID);
        }

        public static void DeletLevel(int i_LID,int argCCId)
        {
            StageDetDL.DeletLevel(i_LID,argCCId);
        }

        public static int InsertBlock(StageDetBO BOstageDet)
        {
            return StageDetDL.InsertBlock(BOstageDet);
        }
        internal static bool CheckBlockFound(StageDetBO BOstageDet)
        {
            return StageDetDL.CheckBlockFound(BOstageDet);
        }
        public static void UpdateBlock(StageDetBO BOstageDet, int argBlockId)
        {
            StageDetDL.UpdateBlock(BOstageDet, argBlockId);
        }

        public static int InsertStage(StageDetBO BOstageDet)
        {
            return StageDetDL.InsertStage(BOstageDet);
        }

        internal static void UpdateStage(StageDetBO BOstageDet, int argStageId)
        {
            StageDetDL.UpdateStage(BOstageDet, argStageId);
        }

        public static void InsertStageBlock(int argBlockId, string argStr)
        {
            StageDetDL.InsertStageBlock(argBlockId,argStr);
        }

        public static void InsertStageLevel(int argBlockId, string argStr)
        {
            StageDetDL.InsertStageLevel(argBlockId, argStr);
        }

        public static int InsertLevel(StageDetBO BOstageDet)
        {
            return StageDetDL.InsertLevel(BOstageDet);
        }
        public static void UpdateLevel(StageDetBO BOstageDet, int argLevelId)
        {
            StageDetDL.UpdateLevel(BOstageDet, argLevelId);
        }
        public static void InsertLevelBlock(StageDetBO BOstageDet)
        {
            StageDetDL.InsertLevelBlock(BOstageDet);
        }

        #endregion

        public static DataTable EditCarParkCodeSetup(int argCCId, int argBlockId)
        {
            return StageDetDL.EditCarParkCodeSetup(argCCId, argBlockId);
        }

        public static void InsertCarParkCodeSetUp(int argCCId, int argBlockId, string argType, string argPrefix, string argSuffix, int argStartNo, int argWidth)
        {
            StageDetDL.InsertCarParkCodeSetUp(argCCId, argBlockId, argType, argPrefix, argSuffix, argStartNo, argWidth);
        }
    }
}
