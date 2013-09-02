using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.BusinessObjects
{
    class ExecTargetBO
    {
        public static string s_RefNo { get; set; }
        public static DateTime d_TargetDate { get; set; }
        public static int i_CostCentreId { get; set; }
        public static DateTime DE_FromDate { get; set; }
        public static string s_PeriodType { get; set; }
        public static DateTime DE_ToDate { get; set; }
        public static string s_IncenType { get; set; }
        public static string s_Incentivefrom { get; set; }

        public static string s_TargetType { get; set; }
       

        public static int i_TargetId { get; set; }
        
        
        
        public static DateTime DE_TargetDate { get; set; }
        public static decimal d_Incentive { get; set; }

        public static int i_TargetTransId { get; set; }
        public static int i_TargetTransUnitId { get; set; }
        public static int i_ExeId { get; set; }
        public static int i_TMonth { get; set; }
        public static int i_TYear { get; set; }
        public static int i_NoOfPeriod { get; set; }
        public static decimal d_TValue { get; set; }
        public static decimal d_UnitValue { get; set; }

        public static int i_IncentiveId { get; set; }
        public static decimal d_FromValue { get; set; }
        public static decimal d_ToValue { get; set; }
        public static decimal d_IncValue { get; set; }
    }
}
