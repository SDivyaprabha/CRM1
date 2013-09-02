using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CRM.BusinessLayer
{
    class LevelMasterBL
    {
        #region Variables

        DataTable dtLevel;

        #endregion

        #region Objects
        
        CRM.DataLayer.LevelMasterDL oLevelMaster;

        #endregion

        #region Constructor

        public LevelMasterBL()
        {
            oLevelMaster = new CRM.DataLayer.LevelMasterDL();
        }

        #endregion

        #region Properties

        public DataTable DtLevel
        {
            get { return dtLevel; }
            set { dtLevel = value; }
        }

        #endregion

        #region Methods

        public DataTable GetLevel(SqlConnection Con)
        {
            try
            {
                dtLevel = oLevelMaster.GetData();
            }
            catch (Exception ce)
            {
                throw ce;
            }
            return dtLevel;

        }

        public int Update(SqlConnection Con)
        {
            int i = 0;

            try
            {
               i = oLevelMaster.Update(this);
            }
            catch (Exception ce)
            {
                throw ce;
            }
            return i;

        }

        public int Delete(SqlConnection Con,int arglevelId)
        {
            int i = 0;

            try
            {
                i = oLevelMaster.Delete(this, Con, arglevelId);
            }
            catch (Exception ce)
            {
                throw ce;
            }
            return i;

        }

        #endregion
    }
}
