using System;
using System.Collections.Generic;
using System.Linq;

namespace CRM.BusinessObjects
{
    class BuyerBO
    {
    }
    class BuyerDetailsBO
    {
        public string GAllotNo { set; get; }
        public string CCAllotNo { set; get; }
        public string COAllotNo { set; get; }
        public int LeadId { set; get; }
        public string ProjName { set; get; }
        public int PaySchId { get; set; }
        public int CallSheetEntryId { set; get; }
        public int ExecutiveId { set; get; }
        public int CostCentreId { set; get; }
        public int FlatId { set; get; }
        public int PlotId { set; get; }
        public int FlatTypeId { set; get; }
        public int PlotTypeId { set; get; }
        public int BranchId { set; get; }
        public decimal LoanPer { set; get; }
        public int LoanAccNo { set; get; }
        public string Status { set; get; }
        public string CustomerType { set; get; }
        public string PaymentOption { set; get; }
        public int BrokerId { set; get; }
        public decimal ComPer { set; get; }
        public decimal ComAmount { set; get; }
        public DateTime ValidUpto { set; get; }
        public DateTime FinaliseDate { set; get; }
        public DateTime RegDate { set; get; }
        public int NewLeedId { set; get; }
        public string CallType { set; get; }
        public int PostSaleExecId { set; get; }
        public decimal AdvAmt { set; get; }
        public decimal Rate { set; get; }
    }

    public class CheckListBO
    {
        public int CheckListId { set; get; }
        public string CheckListName { set; get; }
        public string TypeName { set; get; }
        public int SortOrder { set; get; }
    }
    public class CheckListTransBO
    {
        public int FlatId { set; get; }
        public int CheckListId { set; get; }
        public string CheckListName { set; get; }
        public int SubmitReq { set; get; }
        public DateTime SubmitDate { set; get; }
        public DateTime ReceiveDate { set; get; }

    }
    
}
