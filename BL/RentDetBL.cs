using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DataLayer;
using CRM.BusinessObjects;

namespace CRM.BusinessLayer
{
    class RentDetBL
    {
         #region Objects
        readonly RentDetDL oRentDetDL;
  
        #endregion

        #region Constructor
        public RentDetBL()
        {
            //
            // TODO: Add constructor logic here
            //
            oRentDetDL = new RentDetDL();
        }
        #endregion

        #region Methods

        public static DataTable GetTenant()
        {
            return RentDetDL.GetTenant();
        }

        public static DataTable Fill_AgreementNo()
        {
            return RentDetDL.Fill_AgreementNo();
        }

        public static DataTable FillRentReg(int RegTransId)
        {
            return RentDetDL.FillRentReg(RegTransId);
        }

        public static int FindRentTransId(int argRentId)
        {
            return RentDetDL.FindRentTransId(argRentId);
        }

        public static DataTable FillRentTrans(int argRegTransId)
        {
            return RentDetDL.FillRentTrans(argRegTransId);
        }

        public static DataTable FillPopStEndDate(int argRegTransId)
        {
            return RentDetDL.FillPopStEndDate(argRegTransId);
        }

        public static DataTable PopCostTenant(int TenId)
        {
            return RentDetDL.PopCostTenant(TenId);
        }

        public static DataTable PopFlatTenant(int TenId)
        {
            return RentDetDL.PopFlatTenant(TenId);
        }

        public static bool InsertRentDetails(DataTable dt)
        {
            return RentDetDL.InsertRentDetails(dt);
        }
        public static bool UpdateRentDetails(int argTransId, DataTable dtGrid)
        {
            return RentDetDL.UpdateRentDetails(argTransId, dtGrid);
        }
        public static bool UpdateNewRentDetails(int argTransId, bool argbAdd , DataTable dt)
        {
            return RentDetDL.UpdateNewRentDetails(argTransId, argbAdd ,dt);
        }


        public static DataTable PopulateRentRegister(DateTime frmDate, DateTime toDate)
        {
            return RentDetDL.PopulateRentRegister(frmDate, toDate);
        }

        public static DataTable PopulateRentRegMaster(int argRentId)
        {
            return RentDetDL.PopulateRentRegMaster(argRentId);
        }

        public static DataTable PopulateRentRegisterChange(int argRenTranstId)
        {
            return RentDetDL.PopulateRentRegisterChange(argRenTranstId);
        }

        public static bool DeleteRentRegister(int RegId, int argCostId, string argVouNo)
        {
           return RentDetDL.DeleteRentRegister(RegId, argCostId, argVouNo);
        }

        public static bool DeleteRentAgreementRegister(int RegId, int argCostId, string argVouNo, int RegTransId, string argType)
        {
            return RentDetDL.DeleteRentAgreementRegister(RegId, argCostId, argVouNo, RegTransId, argType);
        }

        public static bool CheckTransId(int RegTransId, int RegId)
        {
            return RentDetDL.CheckTransId(RegTransId, RegId);
        }

        public static DataTable CheckRentDet(string RefNo)
        {
            return RentDetDL.CheckRentDet(RefNo);
        }

        public static DataTable Fill_AgreementDetail(int argRentId)
        {
            return RentDetDL.Fill_AgreementDetail(argRentId);
        }

        public static DataTable FillAgreementTransDetails(int argRentId)
        {
            return RentDetDL.FillAgreementTransDetails(argRentId);
        }
        #endregion
    }
}
