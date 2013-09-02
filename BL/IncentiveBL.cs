using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.BO;
using CRM.DL;
using System.Data;

namespace CRM.BL
{
    class IncentiveBL
    {
        #region Methods

        public static int InsertIncGen(string argMode, IncentiveBO IncGenBO)
        {
            return IncentiveDL.InsertIncGen(argMode, IncGenBO);
        }

        public static void InsertAmount(string argMode, IncentiveBO IncGenBO)
        {
            IncentiveDL.InsertAmount(argMode, IncGenBO);
        }

        public static DataTable SelectIncGen()
        {
            return IncentiveDL.SelectIncGen();
        }

        public static DataTable SelectIncGenTrans(IncentiveBO IncGenBO)
        {
            return IncentiveDL.SelectIncGenTrans(IncGenBO);
        }

        public static void DeleteIncDet(int argId)
        {
            IncentiveDL.DeleteIncDet(argId);
        }

        #endregion
    }
}
