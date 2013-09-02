using System;
using System.Collections.Generic;
using System.Linq;

namespace CRM.BusinessObjects
{
    public class FlatTypeBO
        {
            public int FlatTypeId { set; get; }
            public int BlockId { set; get; }
            public int ProjId { set; get; }
            public string TypeName { set; get; }
            public decimal Area { set; get; }
            public decimal Rate { set; get; }
            public decimal BaseAmt { set; get; }
            public decimal GuideLineValue { set; get; }
            public decimal AdvPercent { set; get; }
            public decimal AdvAmount { set; get; }
            public decimal USLandArea { set; get; }
            public decimal LandRate { set; get; }
            public decimal LandAmount { set; get; }
            public decimal OtherCostAmt { set; get; }
            public int TotalCarpark { set; get; }
            public decimal NetAmt { set; get; }
            public int PayTypeId { set; get; }
            public string Remarks { set; get; }
            public decimal InterestPercent { set; get; }
            public int CreditDays { set; get; }
            public string LevelRate { set; get; }
            public int Facing { set; get; }
        }
}
