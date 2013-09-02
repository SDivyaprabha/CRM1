using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DataLayer;
using CRM.BusinessObjects;

namespace CRM.BusinessLayer
{
    class ServiceOrderBillBL
    {
        #region Methods

        public static DataTable PopulateProject(int Id)
        {
            return ServiceOrderBillDL.PopulateProject(Id);
        }

        public static DataTable Fill_SerOrderRegister(int argATRegId)
        {
            return ServiceOrderBillDL.Fill_SerOrderRegister(argATRegId);
        }

        public static DataTable Fill_SerOrderTrans(int argATRegId)
        {
            return ServiceOrderBillDL.Fill_SerOrderTrans(argATRegId);
        }

        public static DataTable Fill_SerOrderTransQu(int argATRegId)
        {
            return ServiceOrderBillDL.Fill_SerOrderTransQu(argATRegId);
        }


        public static DataTable PopulateQuote(int ProjId)
        {
            return ServiceOrderBillDL.PopulateQuote(ProjId);
        }

        public static DataTable PopulateFlatQuote(int QuoteId)
        {
            return ServiceOrderBillDL.PopulateFlatQuote(QuoteId);
        }

        public static DataTable PopulateServiceQ(int QuoteId)
        {
            return ServiceOrderBillDL.PopulateServiceQ(QuoteId);
        }


        public static bool InsertSerOrderBill(DataTable dtComPList, DataTable argQTrans)
        {
            return ServiceOrderBillDL.InsertSerOrderBill(dtComPList, argQTrans);
        }


        public static bool UpdateSerOrderBill( DataTable dtComPList, DataTable argQTrans)
        {
            return ServiceOrderBillDL.UpdateSerOrderBill(dtComPList, argQTrans);
        }

        public static DataTable Populate_SerOrderBillRegister(DateTime frmDate, DateTime toDate)
        {
            return ServiceOrderBillDL.Populate_SerOrderBillRegister(frmDate, toDate);
        }

        public static DataTable Populate_SerOrderBillRegisterChange(int argRegId)
        {
            return ServiceOrderBillDL.Populate_SerOrderBillRegisterChange(argRegId);
        }

        public static bool DeleteSerOrderBillRegister(int RegId, int argCostId, string argVouNo)
        {
            return ServiceOrderBillDL.DeleteSerOrderBillRegister(RegId, argCostId, argVouNo);
        }

        #endregion
    }
}
