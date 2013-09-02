using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.BusinessObjects
{
    class ReceiptDetailBO
    {
        public static int ReceiptId { set; get; }
        public static string ReceiptDate { set; get; }
        public static string ReceiptNo { set; get; }
        public static string ChequeNo { set; get; }
        public static string PaymentMode { set; get; }

        public static string ChequeDate { set; get; }
        public static string BankName { set; get; }

        public static int CostCentreId { set; get; }
        public static int BuyerId { set; get; }
        public static int TenantId { set; get; }
        public static string PaymentAgainst { set; get; }
        public static string BillType { set; get; }

        public static decimal Amount { set; get; }
        public static decimal ExcessAmount { set; get; }
        public static decimal Interest { get; set; }
        public static string Status { set; get; }
        public static string Narration { set; get; }


        public static int ReceiptTransId { set; get; }
        //public static string ReceiptType { set; get; }
        public static int FlatId { set; get; }
        public static int BillRegId { set; get; }
        public static int BuyerAcctId { set; get; }

    }

    class PMSReceiptDetailBO
    {
        public static int ReceiptId { set; get; }
        public static string ReceiptDate { set; get; }
        public static string ReceiptNo { set; get; }
        public static string ChequeNo { set; get; }
        public static string PaymentMode { set; get; }
        public static string ChequeDate { set; get; }
        public static string BankName { set; get; }
        public static int CostCentreId { set; get; }
        public static decimal Amount { set; get; }
        public static string Narration { set; get; }
        public static int ReceiptTransId { set; get; }
        public static int FlatId { set; get; }

    }
}
