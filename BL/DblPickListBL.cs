using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CRM.BusinessLayer
{
    class DblPickListBL
    {
        #region Variables
        DataTable dtGetData;
        string tableName;
        #endregion

        #region Properties

        public DataTable DtGetData
        {
            get { return dtGetData; }
            set { dtGetData = value; }
        }
        public String TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }
        #endregion

        #region Objects
        CRM.DataLayer.DblPickListDL ODblPickListDL;
        #endregion

        #region Constrctor

        public DblPickListBL()
        {
            ODblPickListDL = new CRM.DataLayer.DblPickListDL();
        }
        
        #endregion

        #region Methods

        public int Update(SqlConnection Con, string Tablename)
        {
            int i = 0;
            try
            {
                i = ODblPickListDL.Update(this, Con, Tablename);

            }
            catch (Exception e)
            {
                throw e;
            }
            return i;
        }

        public DataTable GetIncome(SqlConnection Con)
        {
            try
            {
                dtGetData = ODblPickListDL.GetIncome(Con);

            }

            catch (Exception e)
            {
                throw e;
            }
            return dtGetData;

        }
        public DataTable GetApartmentSize(SqlConnection Con)
        {
            try
            {
                dtGetData = ODblPickListDL.GetApartmentSize(Con);

            }

            catch (Exception e)
            {
                throw e;
            }
            return dtGetData;

        }
        public DataTable GetCostPreference(SqlConnection Con)
        {
            try
            {
                dtGetData = ODblPickListDL.GetCostPreference(Con);

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
