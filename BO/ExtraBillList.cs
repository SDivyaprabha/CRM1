using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.BusinessObjects
{
    class ExtraBillList
    {
    }
    public class ExtraBillListEntry
    {
        public int FlatId { set; get; }
        public int FlatTypeId { set; get; }
        public int ExtraItemTypeId { set; get; }
        public int TransId { set; get; }
        public string ItemCode { set; get; }
        public string TypeName { set; get; }
        public string Description { set; get; }
        public decimal Qty { set; get; }
        public decimal Rate { set; get; }
        public decimal Amount { set; get; }       
    }
}
