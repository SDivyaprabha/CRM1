using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CRM.DataLayer;


namespace CRM.BusinessLayer
{
    class OtherCostBL
    {
        #region Variables

        #endregion

        #region Objects
        //OtherCostDL oOtherCost;
        #endregion

        #region Constructor

        #endregion

        #region Properties
        public int TypeId { get; set; }

        public DataTable DtOC { get; set; }

        public DataTable DtCarPark { get; set; }
        #endregion

        #region Methods
        public int Update()
        {
            try
            {
                OtherCostDL.Update(this);
            }
            catch (Exception e)
            {
                throw e;
            }
            return 0;
            

        }

        public int UpdateCarPark()
        {
            try
            {
                OtherCostDL.UpdateCarPark(this);
            }
            catch (Exception e)
            {
                throw e;
            }
            return 0;


        }

        public DataTable GetOtherCost(SqlConnection Con)
        {
            try
            {
                DtOC = OtherCostDL.GetData(this,Con);
            }
            catch (Exception ce)
            {
                throw ce;
            }
            return DtOC;

        }

        public DataTable GetCarPark(SqlConnection Con)
        {
            try
            {
                DtOC = OtherCostDL.GetCarpark(this, Con);
            }
            catch (Exception ce)
            {
                throw ce;
            }
            return DtCarPark;

        }

        public DataTable GetCarParktemplate(SqlConnection Con)
        {
            try
            {
                DtOC = OtherCostDL.GetCarTemplate(Con);
            }
            catch (Exception ce)
            {
                throw ce;
            }
            return DtOC;

        }

        public static DataTable GetOCList(int argCCId, int argPayTypeId)
        {
           return OtherCostDL.GetOCList(argCCId,argPayTypeId);
        }

        public static DataTable GetQualifierMaster(string argType,int argId,string argFlatType)
        {
            return OtherCostDL.GetQualifierMaster(argType, argId, argFlatType);
        }

        public static DataTable GetOCListOption(int argCCId, int argPayTypeId)
        {
            return OtherCostDL.GetOCListOption(argCCId, argPayTypeId);
        }

        public static void InsertOCOption(DataTable argDt, int ccid, int argPayTypeId, int argRow)
        {
            OtherCostDL.InsertOCOption(argDt,ccid, argPayTypeId,argRow);
        }

        public static bool GetAdvance(int argCCId, int argPayTypeId)
        {
            return OtherCostDL.GetAdvance(argCCId, argPayTypeId);
        }
        public static bool GetTypewise(int argPayTypeId)
        {
            return OtherCostDL.GetTypewise(argPayTypeId);
        }
        public static bool GetQualifier(int argCCId, int argPayTypeId)
        {
            return OtherCostDL.GetQualifier(argCCId, argPayTypeId);
        }
        public static DataTable GetFTOCNull()
        {
            return OtherCostDL.GetFTOCNull();
        }

        public static DataTable GetFDOCNull()
        {
            return OtherCostDL.GetFDOCNull();
        }

        public static DataTable GetFTOC(int argFlatTypeId)
        {
            return OtherCostDL.GetFTOC(argFlatTypeId);
        }

        public static DataTable GetFDOC(int argFlatId)
        {
            return OtherCostDL.GetFDOC(argFlatId);
        }

        public static DataTable GetGlobalOC(int argFlatTypeId)
        {
            return OtherCostDL.GetGlobalOC(argFlatTypeId);
        }

        public static DataTable GetFTReg(decimal argReg, int iTotCP, int argFTId, int argCCId)
        {
            return OtherCostDL.GetFTReg(argReg, iTotCP, argFTId, argCCId);
        }

        public static DataTable GetFDReg(decimal argReg, int iTotCP, int argFId, int argCCId)
        {
            return OtherCostDL.GetFDReg(argReg, iTotCP, argFId, argCCId);
        }

        public static void InsertOCSetup(DataTable argDtOCSetup, int ccid, int argPayTypeId, bool argAdvance, bool argQualifier,int argTypewise)
        {
            OtherCostDL.InsertOCSetup(argDtOCSetup, ccid, argPayTypeId, argAdvance, argQualifier, argTypewise);
        }

        public static void DeleteOCSetup(int ccid)
        {
            OtherCostDL.DeleteOCSetup(ccid);
        }

        public static bool GetOCAdv(int argCCId)
        {
            return OtherCostDL.GetOCAdv(argCCId);
        }
        public static DataTable GetFTQuali(int argFlatTypeId)
        {
            return OtherCostDL.GetFTQuali(argFlatTypeId);
        }

        public static DataTable GetFDQuali(int argFlatId)
        {
            return OtherCostDL.GetFDQuali(argFlatId);
        }

        public static void InsertFlatTypeOC(DataTable dtOC, int argFTId,DataTable dtOA,DataTable dtOI,DataTable dtTax)
        {
            OtherCostDL.InsertFlatTypeOC(dtOC, argFTId,dtOA,dtOI,dtTax);
        }

        public static void InsertFlatOC(DataTable dtOC, int argFId, DataTable dtOA, DataTable dtOI, DataTable dtTax)
        {
            OtherCostDL.InsertFlatOC(dtOC, argFId,dtOA,dtOI,dtTax);
        }

        public static void InsertGlobalOC(DataTable dtFlat, DataTable dtOC, int argCCId, DataTable dtOA, DataTable dtOI, DataTable dtTax, decimal argAmt)
        {
            OtherCostDL.InsertGlobalOC(dtFlat,dtOC,argCCId, dtOA,dtOI, dtTax,argAmt);
        }

        public static void UpdateUnitOC(int argFlatId, DataTable dtOC, int argCCId, DataTable dtOA, DataTable dtOI, DataTable dtTax, decimal argAmt)
        {
            OtherCostDL.UpdateUnitOC(argFlatId, dtOC, argCCId, dtOA, dtOI, dtTax,argAmt);
        }

        public static DataTable GetFlatTypeOtherArea(int argFlatTypeId, int argOCId)
        {
            return OtherCostDL.GetFlatTypeOtherArea(argFlatTypeId, argOCId);
        }
        public static DataTable GetFlatOtherArea(int argFlatId, int argOCId)
        {
            return OtherCostDL.GetFlatOtherArea(argFlatId, argOCId);
        }
        public static DataTable GetFTOtherArea(int argFlatTypeId)
        {
            return OtherCostDL.GetFTOtherArea(argFlatTypeId);
        }
        public static DataTable GetFOtherArea(int argFlatId)
        {
            return OtherCostDL.GetFOtherArea(argFlatId);
        }

        public static DataTable GetGlobalOtherArea(int argFlatTypeId)
        {
            return OtherCostDL.GetGlobalOtherArea(argFlatTypeId);
        }

        public static bool GetOCStatus(int argFlatId)
        {
            return OtherCostDL.GetOCStatus(argFlatId);
        }

        #endregion

        #region Other Infra

        public static DataTable GetOtherInfraAmount(int argFlatTypeId, int argFlatId)
        {
            return OtherCostDL.GetOtherInfraAmount(argFlatTypeId,argFlatId);
        }

        public static DataTable GetFlatTypeOtherInfra(int argFlatTypeId, int argOCId)
        {
            return OtherCostDL.GetFlatTypeOtherInfra(argFlatTypeId, argOCId);
        }

        public static DataTable GetFlatOtherInfra(int argFlatId, int argOCId)
        {
            return OtherCostDL.GetFlatOtherInfra(argFlatId, argOCId);
        }

        public static DataTable GetFTOtherInfra(int argFlatTypeId)
        {
            return OtherCostDL.GetFTOtherInfra(argFlatTypeId);
        }

        public static DataTable GetFOtherInfra(int argFlatId)
        {
            return OtherCostDL.GetFOtherInfra(argFlatId);
        }

        public static DataTable GetGlobalOtherInfra(int argFlatTypeId)
        {
            return OtherCostDL.GetGlobalOtherInfra(argFlatTypeId);
        }

        #endregion

    }
    
}
