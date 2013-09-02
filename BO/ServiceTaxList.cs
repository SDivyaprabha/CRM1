using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.BusinessObjects
{
    class ServiceTaxList
    {
    }
    public class ServiceTaxListBO
    {
        public int TaxId { set; get; }
        public int AccId { set; get; }
        public int FlatTypeId { set; get; }
        public int FlatId { set; get; }
        public string TaxDescp { set; get; }
        public string TaxFormula { set; get; }
        public string AddFlag { set; get; }
        public bool Select { set; get; }
        public string FormName { set; get; }
    }
}
