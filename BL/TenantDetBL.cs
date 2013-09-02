using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DataLayer;
using CRM.BusinessObjects;

namespace CRM.BusinessLayer
{
    class TenantDetBL
    {
        #region Objects
        readonly TenantDetDL oTenantDetDL;
  
        #endregion

        #region Constructor
        public TenantDetBL()
        {
            //
            // TODO: Add constructor logic here
            //
            oTenantDetDL = new TenantDetDL();
        }
        #endregion

        #region Methods
        public static DataTable GetTenant()
        {
            return TenantDetDL.GetTenant();
        }
        public int InsertTenantContact()
        {
            return oTenantDetDL.InsertTenantContact();
        }

        public static void UpdateTenant()
        {
             TenantDetDL.UpdateTenant();
        }
        public static bool InsertTenantDetails()
        {
            return TenantDetDL.InsertTenantDetails();
        }
        public static bool UpdateTenantDetails()
        {
            return TenantDetDL.UpdateTenantDetails();
        }


        public static DataTable PopulateCostcentre(int Id)
        {
            return TenantDetDL.PopulateCostcentre(Id);
        }

        public static DataTable PopulateCity()
        {
            return TenantDetDL.PopulateCity();
        }

        public static DataTable Fill_TenantDet(int argATRegId)
        {
            return TenantDetDL.Fill_TenantDet(argATRegId);
        }

        public static DataTable PopulateBlock(int ProId)
        {
            return TenantDetDL.PopulateBlock(ProId);
        }

        public static DataTable PopulateFlat(int ProId, int FlatId)
        {
            return TenantDetDL.PopulateFlat(ProId, FlatId);
        }


        public static DataTable PopulateFlatSt(int ProId, int FlatId)
        {
            return TenantDetDL.PopulateFlatSt(ProId, FlatId);
        }

        public static DataTable Fill_CityDet(int cityId)
        {
            return TenantDetDL.Fill_CityDet(cityId);
        }

        public static DataTable Fill_Tenantregister(DateTime frmDate, DateTime toDate)
        {
            return TenantDetDL.Fill_Tenantregister(frmDate, toDate);
        }

        public static DataTable Fill_TenantDetChange(int TenantId)
        {
            return TenantDetDL.Fill_TenantDetChange(TenantId);
        }

        public static DataTable CheckTenant(int TenantId)
        {
            return TenantDetDL.CheckTenant(TenantId);
        }

        public static bool DeleteTenantRegister(int RegId, int argCostId, string argRefNo)
        {
            return TenantDetDL.DeleteTenantRegister(RegId, argCostId, argRefNo);
        }

        #endregion
    }
}
