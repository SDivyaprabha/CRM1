using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.BusinessObjects
{
    class ServiceOrderBillBO
    {
           public static int RegBillId { set; get; }
        public static string Billdate { set; get; }
        public static string BillRefNo { set; get; }
        public static int CostcentreID { set; get; }
        public static int QuoteRegId { set; get; }
        public static int FlatID { set; get; }
        public static int BuyerId { set; get; }
        public static decimal GrossAmt { set; get; }
        public static decimal QualifierAmt { set; get; }
        
        public static decimal NetAmt { set; get; }
        public static string Narration { set; get; }

        public static int BillTransID { set; get; }
        public static int BillRegId { set; get; }
        public static int serviceID { set; get; }
        public static decimal Amount { set; get; }


    }
    public class ServiceOdrList
    {
        public int ServiceId { set; get; }
        public string ServiceName { set; get; }
        public bool choose { set; get; }
    }
}
