using System;
using System.Collections.Generic;
using System.Linq;

namespace CRM.BusinessObjects
{
    class OPPCampaignBO
    {
    }
    public class OpportunityBO
    {
        public int ReqId { set; get; }
        public string ReqNo { set; get; }
        public string ReqDate { set; get; }
        public int ExecutiveId { set; get; }
        public int CCId { set; get; }
        public string OppName { set; get; }
        public decimal Amount { set; get; }
        public string CCName { set; get; }
    }

    public class CampaignBO
    {
        public int CCId { set; get; }
        public int CampId { set; get; }
        public int CReqId { set; get; }
        public string CReqNo { set; get; }
        public string CampaignDate { set; get; }        
        public string OppName { set; get; }
        public string CampName { set; get; }
        public decimal TotAmount { set; get; }
        public decimal NetAmount { set; get; }
        public string DurType { set; get; }
        public int DurPeriod { set; get; }
    }

    public class CompetitorBO
    {
        public int CompetitorId { set; get; }
        public string ProjectName { set; get; }
        public int Com_TotalFlats { set; get; }
        public int Com_TotalBlocks { set; get; }
        public int Com_NoofFloors { set; get; }
        public decimal Com_TotalArea { set; get; }
        public int Com_UnitId { set; get; }
        public decimal Com_Rate { set; get; }
        public decimal Com_FSIIndex { set; get; }
        public decimal Com_GLV { set; get; }
        public decimal Com_LandArea { set; get; }
        public decimal Com_BuildArea { set; get; }
        public int Com_LocationId { set; get; }
        public string Com_Contact { set; get; }
        public string Com_Website { set; get; }
        public string Com_Email { set; get; }
        public string Com_Address { set; get; }
        public decimal Com_LoctionRate { set; get; }
        public decimal Com_CarparkCharges { set; get; }
        public decimal Com_RegisterCharges { set; get; }
        public decimal Com_DocumentCharges { set; get; }
        public decimal Com_ClubCharges { set; get; }
        public decimal Com_InfraRate { set; get; }
        public decimal Com_CMWSSBCharges { set; get; }
        public decimal Com_OtherCharges { set; get; }
        public decimal Com_MaintenanceRate { set; get; }
        public decimal Com_CorpusFundCharges { set; get; }
        public decimal Com_PipedGasCharges { set; get; }
        public decimal Com_CancellationCharges { set; get; }
    }
}

