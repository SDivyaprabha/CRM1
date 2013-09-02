using System;
using System.Collections.Generic;
using System.Linq;
using CRM.DataLayer;
using System.Data;

namespace CRM.BusinessLayer
{
    class AgeBL
    {
        public static void Delete_Age(int argId)
        {
            AgeDL.Delete_Age(argId);
        }
        public static void Update_Age(DataTable argAge)
        {
            AgeDL.Update_Age(argAge);
        }
        public static DataTable Get_AgeDet()
        {
            return AgeDL.Get_AgeDet();
        }

        public static void Delete_DemandAge(int argId)
        {
            AgeDL.Delete_DemandAge(argId);
        }
        public static void Update_DemandAge(DataTable argAge,int argCCId)
        {
            AgeDL.Update_DemandAge(argAge,argCCId);
        }
        public static DataTable Get_DemandAgeDet(int argCCId)
        {
            return AgeDL.Get_DemandAgeDet(argCCId);
        }
    }
}
