using System;
using System.Collections.Generic;
using System.Linq;

namespace CRM.BO
{
    class StageDetBO
    {
        #region Methods

        public string s_BlockName { get; set; }
        public string s_StageName { get; set; }
        public string s_StageBlockName { get; set; }
        public string s_StageLevelName { get; set; }
        public string s_LevelName { get; set; }
        public string s_LevelBlockName { get; set; }

        public int i_CostCentreId { get; set; }
        public int i_SortOrderBlock { get; set; }
        public int i_SortOrderLevel { get; set; }
        public int i_SorOrderStage { get; set; }

        public int i_SelLevel { get; set; }
        public int i_SelStageBlock { get; set; }
        public int i_SelStageLevel { get; set; }

        public int i_BlockId { get; set; }
        public int i_LevelId { get; set; }
        public int i_LevelBlockId { get; set; }
        public int i_StageId { get; set; }
        public int i_StageBlockId { get; set; }
        public int i_StageLevelId { get; set; }

        #endregion
    }
}
