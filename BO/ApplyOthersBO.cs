using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.BO
{
    class ApplyOthersBO
    {
        #region Methods

        public int i_FlatTypeId { get; set; }
        public int i_CheckListId { get; set; }
        public int i_ExeId { get; set; }

        public string s_FlatTypeName { get; set; }
        public string s_CheckListName { get; set; }
        public string s_Remarks { get; set; }
        public string s_ExeName { get; set; }

        public DateTime DE_ExeCompletion { get; set; }
        public DateTime DE_CompletionDate { get; set; }

        public int i_Status { get; set; }
        public string s_Status { get; set; }

        #endregion
    }
}
