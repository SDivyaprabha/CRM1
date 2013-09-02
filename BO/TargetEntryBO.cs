using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.BO
{
    class TargetEntryBO
    {
        #region Variables

        public string s_RefNo { get; set; }
        public string s_PeriodType { get; set; }
        public string s_TargetType { get; set; }
        public string s_Incentivefrom { get; set; }
        public string s_IncenType { get; set; }

        public int i_TargetId { get; set; }
        public int i_TargetDate { get; set; }
        public DateTime DE_FromDate { get; set; }
        public int i_CostCentreId { get; set; }
        public DateTime DE_TargetDate { get; set; }
        public decimal d_Incentive { get; set; }

        public int i_TargetTransId { get; set; }
        public int i_TargetTransUnitId { get; set; }
        public int i_ExeId { get; set; }
        public int i_TMonth { get; set; }
        public int i_TYear { get; set; }
        public int i_NoOfPeriod { get; set; }
        public decimal d_TValue { get; set; }
        public decimal d_UnitValue { get; set; }

        public int i_IncentiveId { get; set; }
        public decimal d_FromValue { get; set; }
        public decimal d_ToValue { get; set; }
        public decimal d_IncValue { get; set; }

        #endregion
    }
}
