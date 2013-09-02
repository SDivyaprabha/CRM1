using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CRM
{
    class FilterBL
    {
        public static DataTable GetFilterProj(FilterBO e_FilterBO)
        {
            return FilterDL.GetFilterProj(e_FilterBO);
        }
    }
}
