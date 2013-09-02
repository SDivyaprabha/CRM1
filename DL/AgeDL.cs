using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;


namespace CRM.DataLayer
{
    class AgeDL
    {
        public static void Delete_Age(int argId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    SqlCommand Command = new SqlCommand("Trans_Age", conn, tran) { CommandType = CommandType.StoredProcedure };
                    Command.Parameters.Clear();
                    Command.Parameters.AddWithValue("@Type", "D");
                    Command.Parameters.AddWithValue("@AgeId", argId);
                    Command.Parameters.AddWithValue("@AgeDesc", "");
                    Command.Parameters.AddWithValue("@FromDays", 0);
                    Command.Parameters.AddWithValue("@ToDays", 0);
                    Command.ExecuteNonQuery();
                    tran.Commit();
                }
                catch (Exception ce)
                {
                    tran.Rollback();
                    System.Windows.Forms.MessageBox.Show(ce.Message, "CRM", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static void Update_Age(DataTable argAge)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt = argAge.GetChanges(DataRowState.Added);
                    if (dt!=null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            SqlCommand Command = new SqlCommand("Trans_Age", conn, tran) { CommandType = CommandType.StoredProcedure };
                            Command.Parameters.Clear();
                            Command.Parameters.AddWithValue("@Type", "I");
                            Command.Parameters.AddWithValue("@AgeDesc",CommFun.IsNullCheck(dt.Rows[i]["AgeDesc"], CommFun.datatypes.vartypestring));
                            Command.Parameters.AddWithValue("@AgeId", 0);
                            Command.Parameters.AddWithValue("@FromDays",CommFun.IsNullCheck( dt.Rows[i]["FromDays"], CommFun.datatypes.vartypenumeric));
                            Command.Parameters.AddWithValue("@ToDays",CommFun.IsNullCheck( dt.Rows[i]["ToDays"], CommFun.datatypes.vartypenumeric));
                            Command.ExecuteNonQuery();
                        }
                        dt.Dispose();
                    }
                    dt = new DataTable();
                    dt = argAge.GetChanges(DataRowState.Modified);
                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            SqlCommand Command = new SqlCommand("Trans_Age", conn, tran) { CommandType = CommandType.StoredProcedure };
                            Command.Parameters.Clear();
                            Command.Parameters.AddWithValue("@Type", "U");
                            Command.Parameters.AddWithValue("@AgeId", dt.Rows[i]["AgeId"]);
                            Command.Parameters.AddWithValue("@AgeDesc", dt.Rows[i]["AgeDesc"]);
                            Command.Parameters.AddWithValue("@FromDays", dt.Rows[i]["FromDays"]);
                            Command.Parameters.AddWithValue("@ToDays", dt.Rows[i]["ToDays"]);
                            Command.ExecuteNonQuery();
                        }
                        dt.Dispose();
                    }

                    tran.Commit();
                }
                catch (Exception ce)
                {
                    tran.Rollback();
                    System.Windows.Forms.MessageBox.Show(ce.Message, "CRM", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static DataTable Get_AgeDet()
        {
          
            SqlDataAdapter sda;
            DataTable dt = null;
            SqlConnection con=new SqlConnection();
            con = BsfGlobal.OpenCRMDB();
            string sSql = "";
            try
            {

                sSql = "Select AgeId,AgeDesc,FromDays,ToDays FROM dbo.AgeSetup";
                sda = new SqlDataAdapter(sSql, con);
                //sda = new SqlDataAdapter( "SELECT AgeId,AgeDesc,FromDays,ToDays FROM dbo.AgeSetup ");
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "CRM", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static void Delete_DemandAge(int argId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    SqlCommand Command = new SqlCommand("Trans_DemandAge", conn, tran) { CommandType = CommandType.StoredProcedure };
                    Command.Parameters.Clear();
                    Command.Parameters.AddWithValue("@Type", "D");
                    Command.Parameters.AddWithValue("@AgeId", argId);
                    Command.Parameters.AddWithValue("@AgeDesc", "");
                    Command.Parameters.AddWithValue("@FromDays", 0);
                    Command.Parameters.AddWithValue("@ToDays", 0);
                    Command.Parameters.AddWithValue("@CostCentreId", 0);
                    Command.Parameters.AddWithValue("@ReportName", "");
                    Command.ExecuteNonQuery();
                    tran.Commit();
                }
                catch (Exception ce)
                {
                    tran.Rollback();
                    System.Windows.Forms.MessageBox.Show(ce.Message, "CRM", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static void Update_DemandAge(DataTable argAge,int argCCId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt = argAge.GetChanges(DataRowState.Added);
                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            SqlCommand Command = new SqlCommand("Trans_DemandAge", conn, tran) { CommandType = CommandType.StoredProcedure };
                            Command.Parameters.Clear();
                            Command.Parameters.AddWithValue("@Type", "I");
                            Command.Parameters.AddWithValue("@AgeDesc", CommFun.IsNullCheck(dt.Rows[i]["AgeDesc"], CommFun.datatypes.vartypestring));
                            Command.Parameters.AddWithValue("@AgeId", 0);
                            Command.Parameters.AddWithValue("@FromDays", CommFun.IsNullCheck(dt.Rows[i]["FromDays"], CommFun.datatypes.vartypenumeric));
                            Command.Parameters.AddWithValue("@ToDays", CommFun.IsNullCheck(dt.Rows[i]["ToDays"], CommFun.datatypes.vartypenumeric));
                            Command.Parameters.AddWithValue("@CostCentreId", argCCId);
                            Command.Parameters.AddWithValue("@ReportName", CommFun.IsNullCheck(dt.Rows[i]["ReportName"], CommFun.datatypes.vartypestring));
                            Command.ExecuteNonQuery();
                        }
                        dt.Dispose();
                    }
                    dt = new DataTable();
                    dt = argAge.GetChanges(DataRowState.Modified);
                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            SqlCommand Command = new SqlCommand("Trans_DemandAge", conn, tran) { CommandType = CommandType.StoredProcedure };
                            Command.Parameters.Clear();
                            Command.Parameters.AddWithValue("@Type", "U");
                            Command.Parameters.AddWithValue("@AgeId", dt.Rows[i]["AgeId"]);
                            Command.Parameters.AddWithValue("@AgeDesc", dt.Rows[i]["AgeDesc"]);
                            Command.Parameters.AddWithValue("@FromDays", dt.Rows[i]["FromDays"]);
                            Command.Parameters.AddWithValue("@ToDays", dt.Rows[i]["ToDays"]);
                            Command.Parameters.AddWithValue("@CostCentreId", argCCId);
                            Command.Parameters.AddWithValue("@ReportName", CommFun.IsNullCheck(dt.Rows[i]["ReportName"], CommFun.datatypes.vartypestring));
                            Command.ExecuteNonQuery();
                        }
                        dt.Dispose();
                    }

                    tran.Commit();
                }
                catch (Exception ce)
                {
                    tran.Rollback();
                    System.Windows.Forms.MessageBox.Show(ce.Message, "CRM", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static DataTable Get_DemandAgeDet(int argCCId)
        {

            SqlDataAdapter sda;
            DataTable dt = null;
            SqlConnection con = new SqlConnection();
            con = BsfGlobal.OpenCRMDB();
            string sSql = "";
            try
            {

                sSql = "Select AgeId,AgeDesc,FromDays,ToDays,CostCentreId,ReportName FROM dbo.DemandLetterSetup Where CostCentreId=" + argCCId + "";
                sda = new SqlDataAdapter(sSql, con);
                //sda = new SqlDataAdapter( "SELECT AgeId,AgeDesc,FromDays,ToDays FROM dbo.AgeSetup ");
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "CRM", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        }
    }

