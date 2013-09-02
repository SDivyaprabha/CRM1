using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessLayer;

namespace CRM.DataLayer
{
    class ExecutiveDetailDL
    {
        #region Methods
        public DataTable GetDesignation(SqlConnection Con)
        {
            DataTable Dt=null;
            SqlDataAdapter Sda;
            try 
            {
                Sda = new SqlDataAdapter("SELECT DesignationId as Id,Description as Name FROM dbo.Designation Order By Description", Con);
                Dt=new DataTable();
                Sda.Fill(Dt);
                Dt.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return Dt;

        }
        public void UpdateData(ExecutiveDetailBL OExecUpdate, SqlConnection Con)
        {
            SqlCommand cmd= new SqlCommand();
        }

        public void Update(ExecutiveDetailBL oExecutiveBL, SqlConnection Con)
        {
            SqlCommand cmd;
            try
            {

                cmd = new SqlCommand("UpdateExec", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Flag", oExecutiveBL.Flag);
                if(oExecutiveBL.ExecId==0)
                    cmd.Parameters.AddWithValue("@ExecId", DBNull.Value);
                else
                cmd.Parameters.AddWithValue("@ExecId", oExecutiveBL.ExecId);
                cmd.Parameters.AddWithValue("@ExecName", oExecutiveBL.ExecName);
                cmd.Parameters.AddWithValue("@DesigId", oExecutiveBL.DesigId);
                cmd.Parameters.AddWithValue("@PhoneRes", oExecutiveBL.PhoneRes);
                cmd.Parameters.AddWithValue("@Mobile", oExecutiveBL.Mobile);
                cmd.Parameters.AddWithValue("@Email", oExecutiveBL.Email);
                cmd.Parameters.AddWithValue("@Address", oExecutiveBL.Address);
                cmd.Parameters.AddWithValue("@EduQual", oExecutiveBL.EduQual);
                cmd.Parameters.AddWithValue("@DOB ", oExecutiveBL.DOB);
                cmd.Parameters.AddWithValue("@Fathername ", oExecutiveBL.Fathername);
                cmd.Parameters.AddWithValue("@DOJ", oExecutiveBL.DOJ);
                cmd.Parameters.AddWithValue("@Remarks", oExecutiveBL.Remarks);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }

        }
        public void FillExecutive(SqlConnection Con,int argExecId)
        {
            SqlDataAdapter sd;
            DataTable dt;
            try
            {
                sd = new SqlDataAdapter("FillExec", Con);
                sd.SelectCommand.CommandType = CommandType.StoredProcedure;
                sd.SelectCommand.Parameters.Clear();
                sd.SelectCommand.Parameters.AddWithValue("@ExecId", argExecId);
                dt = new DataTable();
                sd.Fill(dt);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        #endregion

    }
}
