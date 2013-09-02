using System;
using System.Collections.Generic;
using System.Linq;

namespace CRM.BusinessObjects
{
  
    public class RentDetBO
    {
        public static int RentId { set; get; }
        public static string RefNo { set; get; }
        public static string AgreementNo { set; get; }
        public static int TenantId { set; get; }
        public static int CostCentreId { set; get; }
        public static int FlatId { set; get; }

        public static string RegDate { set; get; }
        public static string StartDate { set; get; }
        public static string EndDate { set; get; }

        public static decimal Rent { set; get; }
        public static decimal NetRent { set; get; }
        public static string RentDuration { set; get; }
        public static string RenewType { set; get; }
        public static decimal Advance { set; get; }
        public static decimal RentPeriod { set; get; }
        public static string IntDuration { set; get; }
        public static string RentType { set; get; }
        public static decimal IntRate { set; get; }
        public static string Terms { set; get; }
        public static int GracePriod { set; get; }
     
    }
}
