using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DataLayer;
using CRM.BusinessObjects;


namespace CRM.BusinessLayer
{
    class ServiceOrderBL
    {
        #region Methods

        public static DataTable GetServices()
        {
            return ServiceOrderDL.GetServices();
        }

        public static DataTable PopulateCostCentre(int Id)
        {
            return ServiceOrderDL.PopulateCostCentre(Id);
        }

        public static DataTable Populate_SerQuoteRegister(int RegId)
        {
            return ServiceOrderDL.Populate_SerQuoteRegister(RegId);
        }

        public static DataTable Populate_SerQuoteTrans(int RegId)
        {
            return ServiceOrderDL.Populate_SerQuoteTrans(RegId);
        }

        public static DataTable Populate_SerQuoteListTrans(int RegId)
        {
            return ServiceOrderDL.Populate_SerQuoteListTrans(RegId);
        }

        public static bool InsertSerOrderDetails(DataTable dtComPList,DataTable argQTrans)
        {
            return ServiceOrderDL.InsertSerOrderDetails(dtComPList, argQTrans);
        }
        public static bool UpdateSerOrderDetails(DataTable dtComPList, DataTable argQTrans)
        {
            return ServiceOrderDL.UpdateSerOrderDetails(dtComPList, argQTrans);
        }

        public static DataTable Fill_SerQuoteRegister(DateTime frmDate, DateTime toDate)
        {
            return ServiceOrderDL.Fill_SerQuoteRegister(frmDate, toDate);
        }

        public static DataTable Fill_SerQuoteRegisterChange(int argRegId)
        {
            return ServiceOrderDL.Fill_SerQuoteRegisterChange(argRegId);
        }

        public static bool DeleteSerQuoteRegister(int RegId, int argCostId, string argVouNo)
        {
            return ServiceOrderDL.DeleteSerQuoteRegister(RegId, argCostId, argVouNo);
        }

        public static DataTable PopulateCostCentreLead(int ProId)
        {
            return ServiceOrderDL.PopulateCostCentreLead(ProId);
        }

        public static DataTable PopulateProjectLead(int FlatId)
        {
            return ServiceOrderDL.PopulateProjectLead(FlatId);
        }

        #endregion
    }
}
