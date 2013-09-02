using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CRM.DL;

namespace CRM.BL
{
    class ApplyOthersBL
    {
        #region Methods

        public static DataTable GetFlatType(int argCostCentreId)
        {
            return ApplyOthersDL.GetFlatType(argCostCentreId);
        }

        public static DataTable GetCheckList(int argFlatTypeId)
        {
            return ApplyOthersDL.GetCheckList(argFlatTypeId);
        }

        public static DataTable GetFlatTypeCheckList(int argFlatTypeId,int argCheckListId)
        {
            return ApplyOthersDL.GetFlatTypeCheckList(argFlatTypeId,argCheckListId);
        }

        public static void InsertFlatCheckList(DataTable dt, int argCheckListId, int argFlatTypeId)
        {
            ApplyOthersDL.InsertFlatCheckList(dt, argCheckListId, argFlatTypeId);
        }

        #endregion
    }
}
