using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.BusinessObjects
{
    class PaymentScheduleBO
    {
    }
    public class StageList
    {
        public int StageId { set; get; }        
        public string StageName { set; get; }        
        public bool Select { set; get; }      
    }

    public class DescList
    {
        public int SchDescId { set; get; }
        public string SchDescName { set; get; }
        public bool Select { set; get; }     

    }

    public class PaymentSchTrans
    {
        public int CCId { set; get; }
        public int TypeId { set; get; }
        public int PaymentSchId { set; get; }
        public int FlatTypeId { set; get; }
        public int FlatId { set; get; }
        public int DescId { set; get; }
        public int StageId { set; get; }
        public int OtherCostId { set; get; }
        public string Description { set; get; }
        public string EntryType { set; get; }
        public string SchDate { set; get; }
        public string PreStageType { set; get; }
        public int DateAfter { set; get; }
        public int Duration { set; get; }
        public string DurationType { set; get; }
        public decimal AmtPercent { set; get; }
        public decimal Amount { set; get; }
        public int PreStageTypeId { set; get; }

    }
    public class DateTrans
    {
        public int DateAfter { set; get; }
        public int Duration { set; get; }
        public int PreStageTypeId { set; get; }
        public DateTime DurDate { set; get; }
        public string DurationType { set; get; }
        public string PreStageType { set; get; }

    }
}
