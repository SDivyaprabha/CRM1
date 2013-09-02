using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.BusinessObjects
{
    class ProgressBillBO
    {
    }
    class ProgressBillRegister
    {
        public int PBillId { set; get; }
        public string BillNo { set; get; }
        public string CCBillNo { set; get; }
        public int CCId { set; get; }
        public int LandId { set; get; }
        public DateTime BillDate { set; get; }
        public DateTime AsOnDate { set; get; }
        public decimal TotalAmount { set; get; }
        public int IncomeId { set; get; }
        public int AdvanceId { set; get; }
        public int BuyerAccountId { set; get; }
        public string Remarks { set; get; }
        public string SchType { set; get; }
        public int StageId { set; get; }

        //public decimal CurrentAmount { set; get; }
    }
}
