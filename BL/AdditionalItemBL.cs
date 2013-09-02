using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DataLayer;

namespace CRM.BusinessLayer
{
    class AdditionalItemBL
    {
        #region Objects
        
        #endregion

        #region Constructor
        
        #endregion

        #region Methods

        public static DataTable GetExtraItemList(string argItemTranId, string argType, int argCCId, int argFlatTypeId)
        {
            return AdditionalItemDL.GetExtraItemList(argItemTranId, argType, argCCId, argFlatTypeId);
        }

        #endregion

    }
}
