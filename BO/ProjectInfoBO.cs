using System;
using System.Collections.Generic;
using System.Linq;

namespace CRM.BO
{
    class ProjectInfoBO
    {
        #region Variable

        public int i_CostCentreId { get; set; }
        public int i_TotalFlats { get; set; }
        public int i_TotalBlocks { get; set; }
        public string s_TotalArea { get; set; }
        public int i_NoOfFloors { get; set; }
        public decimal d_FSIIndex { get; set; }
        public decimal d_Rate { get; set; }
        public decimal d_GuideLineValue { get; set; }
        public decimal d_LandRate { get; set; }
        public decimal d_LandArea { get; set; }
        public decimal d_BuildArea { get; set; }
        public decimal d_NetLandArea { get; set; }
        public decimal d_WithHeld { get; set; }

        public bool b_UDS { get; set; }
        public bool b_LCB { get; set; }
        public string s_UDS { get; set; }
        public string s_LCB { get; set; }

        public DateTime dt_StartDate { get; set; }
        public DateTime dt_EndDate { get; set; }

        public string s_AmenityName { get; set; }
        public string s_NBSName { get; set; }
        public string s_Distance { get; set; }

        public int i_ServicesId { get; set; }
        public int i_AmenityId { get; set; }
        public int i_CompetitorId { get; set; }
        public int i_CompProjectId { get; set; }

        #endregion

        public string c_InterestBasedOn { get; set; }

        public decimal d_RegValue { get; set; }

        public decimal d_CancelPenalty { get; set; }
    }
}
