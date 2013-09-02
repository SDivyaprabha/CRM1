using System;
using System.Collections.Generic;
using System.Linq;

namespace CRM.BusinessObjects
{
    
    class BankDetailsBO
    {
        public static int BankId { set; get; }
        public static string BranchName { set; get; }
        public static string Addr1 { get; set; }
        public static string Addr2 { set; get; }
        public static string City { set; get; }
        public static string State { get; set; }
        public static string PIN { set; get; }
        public static string Country { set; get; }
        public static string Contact { set; get; }
        public static string IFC { set; get; }
        public static string Mobile { set; get; }
        public static string Phone { set; get; }
        public static string FAX { set; get; }
        public static decimal IntRate { set; get; }
        public static decimal LoanPer { set; get; }
        public static int PrDays { set; get; }
        public static string Doc { set; get; }
        public static decimal ProcFee { set; get; }
        public static decimal LegalFee { set; get; }
        public static int Insurance { set; get; }
        public static string Remarks { set; get; }
    }

}
