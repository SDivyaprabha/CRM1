using System;
using System.Collections.Generic;
using System.Linq;
using CRM.DataLayer;
using System.Data;
using System.Data.SqlClient;

namespace CRM.BusinessLayer
{
    class FlatUnitBL
    {
        public static DataTable GetBlock(int argCCID)
        {
            return FlatUnitDL.GetBlock(argCCID);
        }

        public static DataTable GetLevel(int argCCID)
        {
            return FlatUnitDL.GetLevel(argCCID);
        }

        public static DataTable GetFlatType(int argCCID)
        {
            return FlatUnitDL.GetFlatType(argCCID);
        }

        public static bool GetFlatNoFound(int argCCId,string argFlatNo)
        {
            return FlatUnitDL.GetFlatNoFound(argCCId,argFlatNo);
        }

        public static bool GetFNoFound(int argCCId, string argFlatNo, SqlConnection conn, SqlTransaction tran)
        {
            return FlatUnitDL.GetFNoFound(argCCId,argFlatNo, conn, tran);
        }

        public static string GetUniqueNoFound(int argCCId, int argFlatId, SqlConnection conn, SqlTransaction tran)
        {
            return FlatUnitDL.GetUniqueNoFound(argCCId,argFlatId, conn, tran);
        }

        public static void InsertFlatUnit(int argCCId, int argBlockId, int argLevelId, int argFlatTypeId, string argFlatNo)
        {
            FlatUnitDL.InsertFlatUnit(argCCId, argBlockId, argLevelId, argFlatTypeId, argFlatNo);
        }

        #region SortOrder

        public static DataTable GetSOBlock(int argCCId)
        {
            return FlatUnitDL.GetSOBlock(argCCId);
        }

        public static DataTable GetSOLevel(int argCCId)
        {
            return FlatUnitDL.GetSOLevel(argCCId);
        }

        public static DataTable GetSOFlat(int argCCId,int argBlockId,int argLevelId)
        {
            return FlatUnitDL.GetSOFlat(argCCId,argBlockId,argLevelId);
        }

        public static void UpdateSortOrder(DataTable argdt)
        {
            FlatUnitDL.UpdateSortOrder(argdt);
        }

        #endregion

    }
}
