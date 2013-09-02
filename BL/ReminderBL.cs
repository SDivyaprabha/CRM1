using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CRM.DataLayer;
using CRM.BusinessObjects;


namespace CRM.BusinessLayer
{
    class ReminderBL
    {
        #region Variables

        #endregion

        #region Objects
        
        ReminderDL oReminder;


        #endregion

        #region Constructor

        public ReminderBL()
        {
            oReminder = new ReminderDL();

        }

        #endregion

        #region Properties

        public DataTable dtReminder {get; set;}

        #endregion

        #region Methods

        public DataTable GetData()
        {
            try
            {
                dtReminder = oReminder.GetData();
            }
            catch (Exception ce)
            {
                throw ce;
            }
            return dtReminder;

        }

        public int Update()
        {
            int i = 0;

            try
            {
               i = oReminder.Update(this);
            }
            catch (Exception ce)
            {
                throw ce;
            }
            return i;

        }

        public int Delete(int arglevelId)
        {
            int i = 0;

            try
            {
                i = oReminder.Delete(arglevelId);
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
