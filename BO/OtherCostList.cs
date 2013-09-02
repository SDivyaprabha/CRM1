using System;
using System.Collections.Generic;
using System.Linq;

namespace CRM.BusinessObjects
{
    class OtherCostList
    {
    }
    public class OtherCostFlatEntry
    {
        //public int CostCentreId { set; get; }
        //public int FlatTypeId { set; get; }
        //public int FlatId { set; get; }
        public int OtherCostId { set; get; }
        public string OtherCostName { set; get; }
        //public decimal Area { set; get; }
        //public decimal Rate { set; get; }
        //public string ADD_LESS_FLAG { set; get; }
        public string Flag { set; get; }
        public decimal Amount { set; get; }
        //public bool Select { set; get; }
        //public string FormName { set; get; }

    }

    public class FlatTypeQualifier
    {
        public int FlatTypeId {get;set; }
        public int OtherCostId { get; set; }
        public int QualiId { get; set; }
        public string Expression { get; set; }
        public decimal QualiAmt { get; set; }
        public string Flag { get; set; }
    }

    public class OtherArea
    {
        public int OtherCostId { get; set; }
        public int FlatTypeId { get; set; }
        public int FlatId { get; set; }
        public decimal Area { get; set; }
        public decimal Rate { get; set; }
        public int Unit { get; set; }
        public decimal Amount { get; set; }
    }

    public class OtherInfra
    {
        public int OtherCostId { get; set; }
        public int FlatTypeId { get; set; }
        public int FlatId { get; set; }
        public string AmountType { get; set; }
        public decimal Percent { get; set; }
        public decimal Amount { get; set; }
    }

}
