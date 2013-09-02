using System;
using System.Collections.Generic;
using System.Linq;

namespace CRM.BusinessObjects
{
    class ExtraBillBO
    {
         
    }
    public class ExtraBillRegBO
    {
        public int FlatId { set; get; }
        public int BillRegId { set; get; }
        public DateTime BillDate { set; get; }
        public string RefNo { set; get; }
        public int CCId { set; get; }
        public decimal BillAmt { set; get; }
        public decimal NetAmt { set; get; }
        public string Narration { set; get; }       

    }
    public class ExtraBillTransBO
    {
        public int FlatId { set; get; }
        public int BillRegId { set; get; }        
        public int EItemId { set; get; }
        public decimal Qty { set; get; }
        public decimal Rate { set; get; }
        public decimal Amt { set; get; }
        public decimal NetAmt { get; set; }

    }
}

