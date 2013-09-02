using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace CRM.BusinessObjects
{
    class CallSheetEntryBO
    {
    }
   public class CallSheetEntryMasterBO
   {
        public string AllotmentNo { set; get; }
        public int EntryID { set; get; }
        public int ProjID { set; get; }
        public int FlatID { set; get; }
        public DateTime RegDate { set; get; }
        public int PlotID { set; get; }
        public int LeadID { set; get; }
        public int Flag { set; get; }
        public int ExecutiveID { set; get; }
        public DateTime TrnDate { set; get; }
        public string CallFF { set; get; }
        public string BuyerName { set; get; }
        public int StatusId { set; get; }
        public int NatureID { set; get; }
        public int CallTypeID { set; get; }
        public DateTime NextCallDate { set; get; }
        public string Remarks { set; get; }
        public int ReminderId { set; get; }
        public string Reminder { set; get; }
        public DataSet Getdata { set; get; }
        public int BrokerId { get; set; }
        public decimal CommPer { get; set; }
        public decimal CommAmt { get; set; }
        public string CallType { get; set; }
        public string Email { get; set; }
    }

   public class FollowUpBO
   {
       public static int ExecutiveId { set; get; }
       public static int LeadId { set; get; }
       public static int CostCentreId { set; get; }
       public static string BuyerDate { set; get; }
       public static int FlatId { set; get; }
       public static int NatureId { set; get; }
       public static int CallTypeId { set; get; }
       public static bool FollowUpReq { set; get; }
       public static string NextCallDate { set; get; }
       public static string Remarks { set; get; }
   }
}
