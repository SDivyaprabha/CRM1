using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessObjects;

namespace CRM.DataLayer
{
    class ComplaintDetDL
    {

        public static DataTable PopulateNature()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT ComplaintId,NatureComplaint FROM Nature_Complaint ORDER BY NatureComplaint";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable PopulateProject(int Id)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();

                sSql = "SELECT CostCentreId,CostCentreName From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                    " Where ProjectDB in(Select ProjectName from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister " +
                    " Where BusinessType In('B','L')) and CostCentreId not in (Select CostCentreId " +
                    " From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where " +
                    " UserId=" + Id + ") Order By CostCentreName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable PopulateExecutive()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT UserId ExecId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As ExecName,0 Sel FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId WHERE B.PositionType='M' ORDER BY EmployeeName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable PopulateEmployee()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT UserId ExecId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As ExecName,0 Sel FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId WHERE B.PositionType='M' ORDER BY EmployeeName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable PopulateNatureComp()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT ComplaintId,NatureComplaint FROM Nature_Complaint ORDER BY NatureComplaint";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable Fill_ComplaintRegister(int argATRegId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT * FROM Complaint_Entry WHERE ComplaintId = " + argATRegId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;

        }

        public static DataTable Populate_ComplaintRegister(DateTime frmDate, DateTime toDate)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();

                string frmdat = string.Format("{0:dd MMM yyyy}", frmDate);
                string tdat = string.Format("{0:dd MMM yyyy}", toDate.AddDays(0));

                sSql = "SELECT C.ComplaintId,C.TransDate,C.ComplaintNo,C.CostCentreId,C1.CostCentreName,F.FlatNo,N.NatureComplaint,E.EmployeeName AttendedBy," +
                        "C.AttDate DateAttented,C.Approve FROM Complaint_Entry C " +
                        "Left JOIN FlatDetails F ON C.FlatId=F.FlatId " +
                        "Left Join Nature_Complaint N on C.NatureId=N.ComplaintId " +
                        "Left JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C1  ON C.CostCentreId=C1.CostCentreId " +
                        "Left JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users E ON C.ExecutiveId=E.UserId " +
                         "Where C.TransDate between '" + frmdat + "'  And '" + tdat + "' ORDER BY C.TransDate,C.ComplaintNo";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;

        }

        public static DataTable Populate_ComplaintRegisterChange(int argEntryId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
          
                sSql = "SELECT C.ComplaintId,C.TransDate,C.ComplaintNo,C.CostCentreId,C1.CostCentreName,F.FlatNo,N.NatureComplaint,E.EmployeeName AttendedBy," +
                        "C.AttDate DateAttented,C.Approve FROM Complaint_Entry C " +
                        "Left JOIN FlatDetails F ON C.FlatId=F.FlatId " +
                        "Left Join Nature_Complaint N on C.NatureId=N.ComplaintId " +
                        "Left JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C1  ON C.CostCentreId=C1.CostCentreId " +
                        "Left JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users E ON C.ExecutiveId=E.UserId " +
                         "Where C.ComplaintId=" + argEntryId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;

        }

        public static bool InsertCompDetails(string argAlert)
        {
            int iIRegId = 0;
            bool bUpdate;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = String.Format("INSERT INTO dbo.Complaint_Entry(CostCentreId,FlatId,LeadId,TenantId,TransDate,NatureId,ExecutiveId,AttDate,Remarks,complaintNo,ChargeType,ExeAttnId,AttnRemarks) Values({0},{1},{2},{3},'{4}', {5},{6},'{7}','{8}','{9}','{10}',{11},'{12}') SELECT SCOPE_IDENTITY()", ComplaintDetBO.CostCentreId, ComplaintDetBO.FlatId, ComplaintDetBO.BuyerId, ComplaintDetBO.TenantId, ComplaintDetBO.TransDate, ComplaintDetBO.NatureId, ComplaintDetBO.ExecutiveId, ComplaintDetBO.AttDate, ComplaintDetBO.Remarks, ComplaintDetBO.complaintNo, ComplaintDetBO.ChargeTYpe, ComplaintDetBO.ExeAttnId, ComplaintDetBO.AttnRemarks);             
                    cmd = new SqlCommand(sSql, conn, tran);
                    iIRegId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();

                    tran.Commit();
                    bUpdate = true;
                    BsfGlobal.InsertLog(DateTime.Now, "Complaint-Entry-Add", "N", "ComplaintEntry", iIRegId, ComplaintDetBO.CostCentreId, 0, BsfGlobal.g_sCRMDBName, ComplaintDetBO.complaintNo, BsfGlobal.g_lUserId);
                    BsfGlobal.InsertAlert("CRM-Complaint-Alert", argAlert, ComplaintDetBO.CostCentreId, BsfGlobal.g_sCRMDBName);

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    bUpdate = false;
                    System.Windows.Forms.MessageBox.Show(ex.Message, "PMS", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return bUpdate;
        }

        public static bool UpdateCompDetails()
        {
            //const int iFTypeId = 0;
            bool bUpdate;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = String.Format("UPDATE dbo.Complaint_Entry SET CostCentreId={0},FlatId={1}, LeadId={2},TransDate='{3}', NatureId={4},ExecutiveId={5}, AttDate='{6}',Remarks='{7}',complaintNo='{8}', TenantId={9},ExeAttnId={10},AttnRemarks='{11}',ChargeTYpe='{12}' WHERE ComplaintId={13}", ComplaintDetBO.CostCentreId, ComplaintDetBO.FlatId, ComplaintDetBO.BuyerId, ComplaintDetBO.TransDate, ComplaintDetBO.NatureId, ComplaintDetBO.ExecutiveId, ComplaintDetBO.AttDate, ComplaintDetBO.Remarks, ComplaintDetBO.complaintNo, ComplaintDetBO.TenantId, ComplaintDetBO.ExeAttnId, ComplaintDetBO.AttnRemarks, ComplaintDetBO.ChargeTYpe, ComplaintDetBO.ComplaintId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    tran.Commit();
                    bUpdate = true;
                    BsfGlobal.InsertLog(DateTime.Now, "Complaint-Entry-Modify", "E", "ComplaintEntry", ComplaintDetBO.ComplaintId, ComplaintDetBO.CostCentreId, 0, BsfGlobal.g_sCRMDBName, ComplaintDetBO.complaintNo, BsfGlobal.g_lUserId);


                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    bUpdate = false;
                    System.Windows.Forms.MessageBox.Show(ex.Message, "PMS", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return bUpdate;
        }

        public static bool DeleteCompRegister(int RegId, int argCostId, string argVouNo)
        {
            string sSql = "";
            bool bSuccess = false;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();

             try
             {
                 sSql = String.Format("DELETE FROM Complaint_Entry WHERE ComplaintId={0}", RegId);
                 cmd = new SqlCommand(sSql, conn, tran);
                 cmd.ExecuteNonQuery();
                 cmd.Dispose();

                 BsfGlobal.InsertLog(DateTime.Now, "Complaint-Entry-Delete", "D", "ComplaintEntry", RegId, argCostId, 0, BsfGlobal.g_sCRMDBName, argVouNo, BsfGlobal.g_lUserId);

                 tran.Commit();
                 bSuccess = true;
                 tran.Dispose();

             }
             catch (Exception ce)
             {
                 tran.Rollback();
                 System.Windows.Forms.MessageBox.Show(ce.Message, "PMS", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                 BsfGlobal.CustomException(ce.Message, ce.StackTrace);
             }
             finally
             {
                 conn.Close();
                 conn.Dispose();
             }
             return bSuccess;
        }

        public static void DeleteCompMaster(int RegId)
        {
            string sSql = "";

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            try
            {
                sSql = String.Format("DELETE FROM Nature_Complaint WHERE ComplaintId={0}", RegId);
                cmd = new SqlCommand(sSql, conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
        }

        public static DataTable CompliantCheck(int FlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = String.Format("SELECT C.ComplaintId,C.CostCentreId,C.FlatId,C.LeadId,C.TenantId,C.NatureId, C.ExecutiveId,C.TransDate,C.AttDate DateAttented,E.EmployeeName AttendedBy,F.FlatNo,C1.CostCentreName, C.Remarks FROM Complaint_Entry C INNER JOIN FlatDetails F ON C.FlatId=F.FlatId INNER JOIN BuyerDetail B ON C.LeadId=B.LeadId INNER JOIN [{0}].dbo.OperationalCostCentre C1  ON C.CostCentreId=C1.CostCentreId INNER JOIN [{0}].dbo.Users E ON C.ExecutiveId=E.UserId where  F.FlatId={1} and C.chargetype='Y' ORDER BY DateAttented", BsfGlobal.g_sWorkFlowDBName, FlatId);                      
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;

        }


        public static void InsertCompliantMaater(string argName)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "INSERT INTO Nature_Complaint (NatureComplaint) VALUES ('" + argName + "')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static void UpdateCompliantMaater(string argName, int argId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "UPDATE Nature_Complaint SET NatureComplaint='" + argName + "'  WHERE ComplaintId=" + argId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }



    }
}
