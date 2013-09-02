using System;
using System.Collections.Generic;
using System.Linq;
using CRM.BusinessObjects;
using CRM.DataLayer;
using System.Data;

namespace CRM.BusinessLayer
{ 

    class OPPCampaignBL
    {
         #region Object
        OPPCampaignDL oOppCapDL;
         #endregion
        
        #region Constructor

        public OPPCampaignBL()
        {
            oOppCapDL = new OPPCampaignDL();
        }
        
        #endregion
        
        #region Methods


        public bool OPPFound(int argReqId)
        {
            return oOppCapDL.OPPFound(argReqId);
        }

        public void DeleteOpportunity(int argReqId)
        {
            oOppCapDL.DeleteOpportunity(argReqId);
        }

        public void DeleteCampaign(int argCampId)
        {
            oOppCapDL.DeleteCampaign(argCampId);
        }

        public int InsertOpportunity(OpportunityBO argObject)
        {
            return oOppCapDL.InsertOpportunity(argObject);
        }

        public bool OppNameFound(int argReqId, string argoppName,int argCCId)
        { return oOppCapDL.OppNameFound(argReqId, argoppName,argCCId); }

        public void UpdateOpportunity(OpportunityBO argObject)
        { oOppCapDL.UpdateOpportunity(argObject); }

        public int InsertCampaign(CampaignBO argObject,DataTable argdtT,DataTable argdtQ)
        { return oOppCapDL.InsertCampaign(argObject,argdtT,argdtQ); }

        public void UpdateCampaign(CampaignBO argObject,DataTable argdtT,DataTable argdtQ)
        { oOppCapDL.UpdateCampaign(argObject, argdtT, argdtQ); }

        public DataTable GetCostCentre()
        {
            return oOppCapDL.GetCostCentre();
        }

        public DataTable GetOpportunity()
        {
            return oOppCapDL.GetOpportunity();
        }

        public static DataTable GetCampaignAmount(int argCampId)
        { return OPPCampaignDL.GetCampaignAmount(argCampId); }

        public static decimal GetPrevCampaignAmount(int argOppId, int argCampId)
        { return OPPCampaignDL.GetPrevCampaignAmount(argOppId, argCampId); }

        public static void InsertCampaignAmount(DataTable argDt, int argCampId)
        {
            OPPCampaignDL.InsertCampaignAmount(argDt, argCampId);
        }

        public DataTable GetCampaign(int argCCId)
        {
            return oOppCapDL.GetCampaign(argCCId);
        }
        public DataTable GetCampaignDetails()
        {
            return oOppCapDL.GetCampaignDetails();
        }
        public DataTable GetExecutive()
        {
            return oOppCapDL.GetExecutive();
        }

        public DataTable GetOpCostCentre()
        {
            return oOppCapDL.GetOpCostCentre();
        }

        public DataTable GetOpReq()
        {
            return oOppCapDL.GetOpReq();
        }

        public DataTable GetOppId(int argCCId)
        {
            return oOppCapDL.GetOppId(argCCId);
        }

        public DataTable GetOPPReqDetails(int argReqId)
        {
            return oOppCapDL.GetOPPReqDetails(argReqId);
        }

        public DataTable GetCampaignDetails(int argcampId)
        {
            return oOppCapDL.GetCampaignDetails(argcampId);
        }

        public static DataTable GetCampaignList(int argReqId, int argCCId)
        {
            return OPPCampaignDL.GetCampaignList(argReqId,argCCId);
        }

        internal static DataTable GetCampaignListTrans(int argCampId)
        {
            return OPPCampaignDL.GetCampaignListTrans(argCampId);
        }

        public int GetReqId(string argReqNo)
        {
            return oOppCapDL.GetReqId(argReqNo);
        }

        public DataSet GetCampDetails(int argReqId, string argFromDate, string argToDate)
        {
            return oOppCapDL.GetCampDetails(argReqId, argFromDate, argToDate);
        }

        public DataSet GetOpportunityDetails(string argFromDate,string argToDate)
        {
            return oOppCapDL.GetOpportunityDetails(argFromDate, argToDate);
        }

        public DataSet GetCostOpportunityDetails()
        {
            return oOppCapDL.GetCostOpportunityDetails();
        }

        public DataTable GetCostCampDetails(int argReqId)
        {
            return oOppCapDL.GetCostCampDetails(argReqId);
        }

        public DataSet GetCampDailyDetails(DateTime argDate)
        {
            return oOppCapDL.GetCampDailyDetails(argDate);
        }

        #endregion

    }
  
}
