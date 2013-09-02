using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using CRM.DataLayer;
using CRM.BusinessObjects;


namespace CRM.BusinessLayer
{
    class ExtraItemTypeBL
    {

          #region Objects
     //   readonly ExtraItemTypeBL oExtraItemDL;
  
        #endregion

        //#region Constructor
        //public ExtraItemTypeBL()
        //{
        //    //
        //    // TODO: Add constructor logic here
        //    //
        //    oExtraItemDL = new ExtraItemTypeDL();
        //}
        //#endregion
        #region Methods

        public static DataTable GetExtraItem()
        {
            return ExtraItemTypeDL.GetExtraItem();
        }

        public static void InsertExtraItemDetails(ExtraItemTypeBO argExtraItemContactBO)
        {
            ExtraItemTypeDL.InsertExtraItemDetails(argExtraItemContactBO);
        }

        public static void UpdateExtraItemDetails(ExtraItemTypeBO argExtraItemContactBO)
        {
            ExtraItemTypeDL.UpdateExtraItemDetails(argExtraItemContactBO);
        }

        public static DataTable GetExtraItemProject(int argCCID)
        {
            return ExtraItemTypeDL.GetExtraItemProject(argCCID);
        }

        public static DataTable GetExtraItemFlatType(int argCCID,int argFlatTypeId)
        {
            return ExtraItemTypeDL.GetExtraItemFlatType(argCCID, argFlatTypeId);
        }

        public static DataTable GetExtraItemFlat(int argCCID, int argFlatId)
        {
            return ExtraItemTypeDL.GetExtraItemFlat(argCCID, argFlatId);
        }

        public static DataTable GetExtraItemFlatQualifier(int argFlatId, string argType)
        {
            return ExtraItemTypeDL.GetExtraItemFlatQualifier(argFlatId, argType);
        }

        public static void InsertExtraItemProjects(DataTable dt, int argCCID)
        {
            ExtraItemTypeDL.InsertExtraItemProjects(dt, argCCID);
        }

        public static void InsertExtraItemFlatType(DataTable dt, int argFlatTypeId, DataTable argdtQualifier)
        {
            ExtraItemTypeDL.InsertExtraItemFlatType(dt, argFlatTypeId, argdtQualifier);
        }

        public static void InsertExtraItemFlat(DataTable dt, int argFlatId, DataTable argdtQualifier, int argCCId)
        {
            ExtraItemTypeDL.InsertExtraItemFlat(dt, argFlatId, argdtQualifier, argCCId);
        }

        public static DataTable GetUnit()
        {
            return ExtraItemTypeDL.GetUnit();
        }

        #endregion

        internal static DataTable GetQuotation(int argiCCId, int argiFlatId, int argiFlatTypeId)
        {
            return ExtraItemTypeDL.GetQuotation(argiCCId, argiFlatId, argiFlatTypeId);
        }
    }
}
