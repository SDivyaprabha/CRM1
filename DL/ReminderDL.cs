using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CRM.DataLayer
{
    class ReminderDL
    {
        #region Methods
        public DataTable GetData()
        {
            DataTable dt = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("getReminder",  BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);

            }

            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return dt;
        }
        public int Update(CRM.BusinessLayer.ReminderBL OReminder)
        {
            int j = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("UpdateReminder",  BsfGlobal.OpenCRMDB());
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < OReminder.dtReminder.Rows.Count; i++)
                {
                    if (OReminder.dtReminder.Rows[i]["ReminderID"].ToString() == string.Empty)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Flag", 1);
                        cmd.Parameters.AddWithValue("@ReminderName", OReminder.dtReminder.Rows[i]["ReminderName"].ToString());
                        cmd.Parameters.AddWithValue("@SortOrder", i+1);
                        cmd.Parameters.AddWithValue("@ReminderID", DBNull.Value);

                        j = cmd.ExecuteNonQuery();

                    }
                    else
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Flag", 2);
                        cmd.Parameters.AddWithValue("@ReminderID", OReminder.dtReminder.Rows[i]["ReminderID"].ToString());
                        cmd.Parameters.AddWithValue("@ReminderName", OReminder.dtReminder.Rows[i]["ReminderName"].ToString());
                        cmd.Parameters.AddWithValue("@SortOrder", i + 1);
                        j = cmd.ExecuteNonQuery();
                    }

                }


            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return j;
        }

        public int Delete(int argLevelId)
        {
            int j = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("Delete FROM Reminder WHERE ReminderId='" + argLevelId + "'",  BsfGlobal.OpenCRMDB());
                j = cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return j;
        }

        #endregion

    }
}
