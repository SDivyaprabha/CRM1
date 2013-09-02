using System;
using System.Collections.Generic;
using System.Linq;

namespace CRM.BO
{
    class IncentiveBO
    {
        #region Methods

        public DateTime DE_Date { get; set; }
        public DateTime DE_From { get; set; }
        public DateTime DE_To { get; set; }
        public string s_RefNo { get; set; }
        public decimal d_Amount { get; set; }
        public string s_Narration { get; set; }
        public string s_ExeName { get; set; }

        public int i_IncGenId { get; set; }
        public int i_ExeId { get; set; }

        public int i_IncTransId { get; set; }
        public decimal d_TotalAmount { get; set; }

        #endregion
    }

    class IncBO
    {
        public static int FromYear { get; set; }
        public static int ToYear { get; set; }
        public static int FromMonth { get; set; }
        public static int ToMonth { get; set; }
    }
}
