using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessObjects;

namespace CRM.DataLayer
{
    class PMSReceiptDL
    {
        public static DataTable GetCostCentre()
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select CostCentreId,CostCentreName From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                         " Where ProjectDB In(Select ProjectName From " +
                         " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType In('B','L'))" +
                         " And CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                         " Where UserId=" + BsfGlobal.g_lUserId + ") Order by CostCentreName";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds);
                da.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return ds;
        }

        public static DataTable GetFlat(int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql = "";

            sSql = "SELECT F.FlatId,F.FlatNo FROM dbo.FlatDetails F WHERE F.CostCentreId=" + argCCId + " ORDER BY FlatNo";
            try
            {
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;

        }

        public static DataTable GetSchedule(int argFlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql = "";
            sSql = "SELECT M.MaintenanceId,F.TransId,F.FromDate,F.ToDate,F.Amount,F.NetAmount,(F.NetAmount-F.PaidAmount) Balance," +
                    " cast(0 as decimal(18,3)) CurrentAmount,(F.NetAmount-F.PaidAmount) HBalance,0 HAmount FROM dbo.MaintenanceSchTrans F  " +
                    " INNER JOIN dbo.MaintenanceDet M ON M.MaintenanceId=F.MaintenanceId WHERE M.FlatId=" + argFlatId + " And (F.PaidAmount-F.NetAmount)<0";
            try
            {
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;

        }

        public static void InsertReceipt(PMSReceiptDetailBO OPMSDetails,DataTable argdt)
        {
            int iRepId = 0;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();

            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "INSERT INTO dbo.PMSReceiptRegister(ReceiptDate,ReceiptNo,ChequeNo,ChequeDate,BankName,PaymentMode,CostCentreId,FlatId,PaidAmount,Narration)VALUES " +
                             " ('" + PMSReceiptDetailBO.ReceiptDate + "','" + PMSReceiptDetailBO.ReceiptNo + "','" + PMSReceiptDetailBO.ChequeNo + "'," +
                              " '" + PMSReceiptDetailBO.ChequeDate + "','" + PMSReceiptDetailBO.BankName + "','" + PMSReceiptDetailBO.PaymentMode + "'," +
                              " " + PMSReceiptDetailBO.CostCentreId + "," + PMSReceiptDetailBO.FlatId + "," + PMSReceiptDetailBO.Amount + ",'" + PMSReceiptDetailBO.Narration + "')SELECT SCOPE_IDENTITY()";
                    cmd = new SqlCommand(sSql, conn, tran);
                    iRepId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();

                    for (int i = 0; i < argdt.Rows.Count; i++)
                    {
                        sSql = "INSERT INTO dbo.PMSReceiptTrans(ReceiptId,MaintenanceId,MainTransId,NetAmount,PaidAmount)VALUES " +
                                " (" + iRepId + "," + argdt.Rows[i]["MaintenanceId"] + "," + argdt.Rows[i]["TransId"] + "," +
                                 " " + argdt.Rows[i]["NetAmount"] + "," + argdt.Rows[i]["CurrentAmount"] + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "UPDATE dbo.MaintenanceSchTrans SET PaidAmount=" + argdt.Rows[i]["CurrentAmount"] + " WHERE TransId=" + argdt.Rows[i]["TransId"] + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static void UpdateReceipt(PMSReceiptDetailBO OPMSDetails, DataTable argdt)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();

            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "DELETE FROM dbo.PMSReceiptTrans WHERE ReceiptId=" + PMSReceiptDetailBO.ReceiptId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "UPDATE dbo.PMSReceiptRegister SET ReceiptDate='" + PMSReceiptDetailBO.ReceiptDate + "',ReceiptNo='" + PMSReceiptDetailBO.ReceiptNo + "'," +
                        " ChequeNo='" + PMSReceiptDetailBO.ChequeNo + "',ChequeDate='" + PMSReceiptDetailBO.ChequeDate + "',BankName='" + PMSReceiptDetailBO.BankName + "'," +
                        " PaymentMode='" + PMSReceiptDetailBO.PaymentMode + "',CostCentreId=" + PMSReceiptDetailBO.CostCentreId + ",FlatId=" + PMSReceiptDetailBO.FlatId + "," +
                        " PaidAmount=" + PMSReceiptDetailBO.Amount + ",Narration='" + PMSReceiptDetailBO.Narration + "' WHERE ReceiptId=" + PMSReceiptDetailBO.ReceiptId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    for (int i = 0; i < argdt.Rows.Count; i++)
                    {
                        sSql = "INSERT INTO dbo.PMSReceiptTrans(ReceiptId,MaintenanceId,MainTransId,NetAmount,PaidAmount)VALUES " +
                                " ('" + PMSReceiptDetailBO.ReceiptId + "'," + argdt.Rows[i]["MaintenanceId"] + "," + argdt.Rows[i]["TransId"] + "," +
                                 " " + argdt.Rows[i]["NetAmount"] + "," + argdt.Rows[i]["CurrentAmount"] + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "UPDATE dbo.MaintenanceSchTrans SET PaidAmount=" + argdt.Rows[i]["CurrentAmount"] + " WHERE TransId=" + argdt.Rows[i]["TransId"] + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static DataTable GetReceiptRegister()
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.CostCentreId,A.ReceiptId,A.ReceiptDate,A.ReceiptNo,B.CostCentreName,F.FlatNo, " +
                        " A.PaidAmount From dbo.PMSReceiptRegister A " +
                        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B on A.CostCentreId=B.CostCentreId " +
                        " Inner Join dbo.FlatDetails F On F.FlatId=A.FlatId Order by A.ReceiptDate,A.ReceiptNo";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds);
                da.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return ds;
        }

        public static DataSet GetReceiptDetE(int argReceiptId)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.*,B.CostCentreName From dbo.PMSReceiptRegister A " +
                    " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B On A.CostCentreId=B.CostCentreId  " +
                    " Inner Join dbo.PMSReceiptTrans C On C.ReceiptId=A.ReceiptId " +
                    " Where A.ReceiptId=" + argReceiptId;

                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Register");
                sda.Dispose();

                sSql = "SELECT M.MaintenanceId,F.TransId,F.FromDate,F.ToDate,F.Amount,F.NetAmount,(F.NetAmount-F.PaidAmount) Balance,"+
                    " F.PaidAmount CurrentAmount,(F.NetAmount-F.PaidAmount) HBalance,F.PaidAmount HAmount FROM dbo.MaintenanceSchTrans F  " +
                  " INNER JOIN dbo.MaintenanceDet M ON M.MaintenanceId=F.MaintenanceId " +
                  " INNER JOIN dbo.PMSReceiptTrans T ON T.MainTransId=F.TransId WHERE T.ReceiptId=" + argReceiptId + " ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "RegisterTrans");
                sda.Dispose();

                BsfGlobal.g_CRMDB.Close();
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public static void DeleteReceiptDetails(int argRecpId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    string sSql = "";
                    //bool bAns = false;

                    sSql = "Update dbo.MaintenanceSchTrans Set PaidAmount=MaintenanceSchTrans.PaidAmount-RT.PaidAmount From dbo.PMSReceiptTrans RT " +
                            " Where RT.MaintenanceId=MaintenanceSchTrans.MaintenanceId And RT.MainTransId=MaintenanceSchTrans.TransId And RT.ReceiptId=" + argRecpId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "DELETE FROM dbo.PMSReceiptRegister Where ReceiptId=" + argRecpId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "DELETE FROM dbo.PMSReceiptTrans Where ReceiptId=" + argRecpId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Update dbo.MaintenanceSchTrans Set PaidAmount=0 Where PaidAmount<=0";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    tran.Commit();
                    //bAns = true;

                    //if (bAns == true)
                    //{
                    //    BsfGlobal.InsertLog(DateTime.Now, "Buyer-Receipt-Delete", "D", "Delete Receipt Register", argRecpId, ReceiptDetailBO.CostCentreId, 0, BsfGlobal.g_sCRMDBName, ReceiptDetailBO.ReceiptNo, BsfGlobal.g_lUserId, ReceiptDetailBO.Amount, 0);
                    //}
                }
                catch (Exception ex)
                {
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
