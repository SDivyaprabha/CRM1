using System;
using System.Collections.Generic;
using System.Linq;

namespace CRM.BusinessObjects
{
    class UnitDirBO
    {
    }
    public class FlatDetailBO
    {
        public int FlatId { set; get; }
        public string FlatNo { set; get; }
        public string FlatTypeName { set; get; }
        public string BlockName { set; get; }
        public string LevelName { set; get; }
        public int FlatTypeId { set; get; }
        public int PayTypeId { set; get; }
        public int LeadId { set; get; }
        public int BlockId { set; get; }
        public int LevelId { set; get; }
        public decimal Area { set; get; }
        public decimal Rate { set; get; }
        public decimal BaseAmt { set; get; }
        public decimal AdvPercent { set; get; }
        public decimal AdvAmount { set; get; }
        public decimal USLand { set; get; }
        public decimal LandRate { set; get; }
        public decimal USLandAmt { set; get; }
        public decimal OtherCostAmt { set; get; }
        public decimal ExtraBillAmt { set; get; }
        public int TotalCarPark { set; get; }
        public decimal GuideLineValue { set; get; }
        public decimal NetAmt { set; get; }
        public decimal QualAmt { set; get; }
        public decimal TotalAmt { set; get; }
        public string Remarks { set; get; }
        public int CostCentreId { set; get; }
        public string Status { set; get; }
        public int Facing { set; get; }
        public string FacingName { set; get; }
        public string BuyerName { set; get; }
        public string ExecutiveName { set; get; }
        public string TypeName { set; get; }
        public string Investor { set; get; }
        public decimal InterestPercent { set; get; }
        public int CreditDays { set; get; }
    }

    public class ExtraItemBO
    {
        public int TransId { set; get; }
        public int ItemCode { set; get; }
        public string ExtraItemTypeName { set; get; }
        public int ExtraItemTypeId { set; get; }
        public string ItemDescription { set; get; }
        //public decimal ExtraRate { set; get; }
    }

    public class CheckBO
    {
        public int CheckListId { set; get; }
        public int FlatTypeId { set; get; }
        public int FlatId { set; get; }
        public string Description { set; get; }
        public int Executive { set; get; }
        public string ExpCompletionDate { set; get; }
        public string Status { set; get; }
        public string CompletionDate { set; get; }
        public string Remarks { set; get; }
    }

    public class CarBO
    {
        public int CostCentreId { set; get; }
        public int BlockId { set; get; }
        public string BlockName { set; get; }
        public int TypeId { set; get; }
        public static int TId { set; get; }
        public int NoOfSlots { set; get; }
        public int AllotSlots { set; get; }
        public static string Slots1 { set; get; }
        public static decimal Cost1 { set; get; }
        public static string Slots2 { set; get; }
        public static decimal Cost2 { set; get; }
    }

    public class FlatCarBO
    {
        public static int CostCentreId { set; get; }
        public static int FlatTypeId { set; get; }
        public static int FlatId { set; get; }
        public static int Open { set; get; }
        public static int Closed { set; get; }
        public static int Terrace { set; get; }
        public static int TotCP { set; get; }
    }
    
    public class LevelwiseBO
    {
        public int CostCentreId { set; get; }
        public int LevelId { set; get; }
        public string LevelName { set; get; }
        public int FlatTypeId { set; get; }
        public decimal Rate { set; get; }
    }

    public class BankBO
    {
        public static int BankId { set; get; }
        public static string BankName { set; get; }
        public static decimal IntRate { set; get; }
        public static decimal LoanAmt { set; get; }
        public static string PDays { set; get; }
    }

    public class BankInfoBO
    {
        public static int BranchId { set; get; }
        public static int LeadId { set; get; }
        public static int FlatId { set; get; }
        public static decimal LoanPer { set; get; }
        public static DateTime LoanAppDate { set; get; }
        public static decimal LoanAccNo { set; get; }
    }

    public class LandRateChangeBO
    {
        public static int CCId { set; get; }
        public static string Date { set; get; }
        public static decimal OldGuideValue { set; get; }
        public static decimal NewGuideValue { set; get; }
        public static decimal OldMarketValue { set; get; }
        public static decimal NewMarketValue { set; get; }
        public static decimal OldRegistration { set; get; }
        public static decimal NewRegistration { set; get; }
    }

    public class Blockunits
    {
        public static int CCId { set; get; }
        public static int FlatId { set; get; }
        public static int LeadId { set; get; }
        public static string CustomerType { set; get; }
        public static string Remarks { set; get; }
        public static string BlockUpto { set; get; }
        public static string Date { set; get; }
        public static decimal PenaltyAmt { set; get; }
    }
}
