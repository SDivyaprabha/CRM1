using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CRM.DataLayer;
using CRM.BusinessObjects;

namespace CRM.BusinessLayer
{
    class PickListBL
    {
        #region Variables
        DataTable dtGetData;
        string tableName;
        #endregion

        #region Properties
        
        public DataTable DtGetData
        {
            get { return dtGetData; }
            set {dtGetData=value;}
        }
        public String TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }
        #endregion

        #region Objects
        PickListDL oPickListDL;
        #endregion

        #region Constrctor
        public PickListBL()
        {
            oPickListDL = new PickListDL();
        }
        #endregion 

        #region Methods

        public DataTable GetCountry(SqlConnection Con)
        {
            try
            {
                dtGetData = oPickListDL.GetCountry(Con);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dtGetData;

        }
        public DataTable GetEmployment(SqlConnection Con)
        {
            try
            {
                dtGetData = oPickListDL.GetEmployment(Con);
            }

            catch (Exception e)
            {
                throw e;
            }
            return dtGetData;

        }
        public DataTable GetCostCentre()
        {
            try
            {
                dtGetData = oPickListDL.GetCostCentre();

            }

            catch (Exception e)
            {
                throw e;
            }
            return dtGetData;

        }
        public DataTable GetReligion(SqlConnection Con)
        {
            try
            {
                dtGetData = oPickListDL.GetReligion(Con);

            }

            catch (Exception e)
            {
                throw e;
            }
            return dtGetData;

        }
        public DataTable GetAcheivement(SqlConnection Con)
        {

            try
            {
                dtGetData = oPickListDL.GetAcheivement(Con);

            }

            catch (Exception e)
            {
                throw e;
            }
            return dtGetData;

        }
        public DataTable GetApartmentType(SqlConnection Con)
        {

            try
            {
                dtGetData = oPickListDL.GetApartmentType(Con);

            }

            catch (Exception e)
            {
                throw e;
            }
            return dtGetData;

        }
        public DataTable GetApartment(SqlConnection Con)
        {

            try
            {
                dtGetData = oPickListDL.GetApartment(Con);

            }

            catch (Exception e)
            {
                throw e;
            }
            return dtGetData;

        }


        public DataTable GetStay(SqlConnection Con)
        {

            try
            {
                dtGetData = oPickListDL.GetStay(Con);

            }

            catch (Exception e)
            {
                throw e;
            }
            return dtGetData;

        }
        public DataTable GetGuestHouse(SqlConnection Con)
        {

            try
            {
                dtGetData = oPickListDL.GetGuestHouse(Con);

            }

            catch (Exception e)
            {
                throw e;
            }
            return dtGetData;

        }

        public DataTable GetPosesses(SqlConnection Con)
        {

            try
            {
                dtGetData = oPickListDL.GetPosesses(Con);

            }

            catch (Exception e)
            {
                throw e;
            }
            return dtGetData;

        }
        public DataTable GetFacility(SqlConnection Con)
        {

            try
            {
                dtGetData = oPickListDL.GetFacility(Con);

            }

            catch (Exception e)
            {
                throw e;
            }
            return dtGetData;

        }
        public DataTable GetArea(SqlConnection Con)
        {

            try
            {
                dtGetData = oPickListDL.GetArea(Con);

            }

            catch (Exception e)
            {
                throw e;
            }
            return dtGetData;

        }

        #endregion
    }
}
