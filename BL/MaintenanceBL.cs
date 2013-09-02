using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CRM.DataLayer;

namespace CRM.BusinessLayer
{
    class MaintenanceBL
    {
        public static DataTable GetCostCentre()
        {
            return MaintenanceDL.GetCostCentre();
        }

        public static DataTable GetFlat(int argCCId)
        {
            return MaintenanceDL.GetFlat(argCCId);
        }

        public static bool InsertMainDetails(DataTable dt, DataTable argQTrans)
        {
            return MaintenanceDL.InsertMainDetails(dt,argQTrans);
        }

        public static bool UpdateMainDetails(int argTransId, DataTable dtGrid, DataTable argQTrans)
        {
            return MaintenanceDL.UpdateMainDetails(argTransId, dtGrid,argQTrans);
        }

        public static DataTable PopulateMainRegister(DateTime frmDate, DateTime toDate)
        {
            return MaintenanceDL.PopulateMainRegister(frmDate, toDate);
        }

        public static DataTable FillRegister(int RegTransId)
        {
            return MaintenanceDL.FillRegister(RegTransId);
        }

        public static DataTable FillTrans(int argRegTransId)
        {
            return MaintenanceDL.FillTrans(argRegTransId);
        }

        public static DataTable Fill_TransQual(int argRegId)
        {
            return MaintenanceDL.Fill_TransQual(argRegId);
        }

        public static bool DeleteRegister(int argRegId, int argCCId, string argVouNo)
        {
            return MaintenanceDL.DeleteRegister(argRegId,argCCId, argVouNo);
        }

        public static DataTable PopulateRegMaster(int argRegId)
        {
            return MaintenanceDL.PopulateRegMaster(argRegId);
        }

        public static bool FoundDate(int argMainId, int argFlatId, DataTable argdt)
        {
            return MaintenanceDL.FoundDate(argMainId,argFlatId, argdt);
        }

        public static DataTable GetReport(int argFlatId,int argMaintanceId)
        {
            return MaintenanceDL.GetReport(argFlatId, argMaintanceId);
        }

    }
}
