using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.BusinessObjects
{
    class ServiceOrderBO
    {

        public static int RegisterId { set; get; }
        public static string SDate { set; get; }
        public static string RefNo { set; get; }
        public static int CostcentreID { set; get; }
        public static int FlatID { set; get; }
        public static int BuyerId { set; get; }
        public static decimal GrossAmt { set; get; }
        public static decimal QualifierAmt { set; get; }
        public static decimal NetAmt { set; get; }
        public static string Remarks { set; get; }
     

        public static int TransID { set; get; }
        public static int RegisterID { set; get; }
        public static int ServiceID { set; get; }
        public static decimal Amount { set; get; }

    }
    public class ServiceList
    {
        public int ServiceId { set; get; }
        public string ServiceName { set; get; }
        public bool choose { set; get; }
    }

}
