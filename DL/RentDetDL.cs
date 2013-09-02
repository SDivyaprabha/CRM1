using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessObjects;

namespace CRM.DataLayer
{
    class RentDetDL
    {

        public static DataTable GetTenant()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT TenantId,TenantName FROM TenantRegister WHERE TenantId <> 0 and Approve='Y' order by TenantName";
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

        public static DataTable Fill_AgreementNo()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT RentId,AgreementNo FROM RentDetail WHERE RentId <> 0 and Approve='Y' order by AgreementNo";
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

        public static DataTable FillRentReg(int RegTransId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                //sSql = "Select * from RentDetail Where RentId = " + RegId + "";
                sSql = "SELECT B.RefNo,B.RegDate,B.StartDate,B.EndDate,A.TenantId,A.FlatId,A.CostCentreId,A.AgreementNo,A.RentPeriod,A.Status,B.Rent,B.NetRent,B.RentDuration,B.Advance,B.IntRate,B.IntDuration,B.Terms,B.RenewType,B.GracePriod,B.RentType,B.Approve FROM RentDetail A inner join RentAgreementTrans B on A.RentId=B.RentId WHERE B.RentTransId= " + RegTransId + "";
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

        public static int FindRentTransId(int argRentId)
        {
            int iRegTransId = 0;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            SqlDataReader sdr;
            DataTable dtTran;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {

                    dtTran = new DataTable();
                    sSql = "SELECT Max(RentTransId) MaxNo FROM RentAgreementTrans WHERE RentId=" + argRentId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    sdr = cmd.ExecuteReader();
                    dtTran.Load(sdr);
                    cmd.Dispose();
                    if (dtTran.Rows.Count > 0)
                    {
                        for (int a = 0; a < dtTran.Rows.Count; a++)
                        {
                            iRegTransId = Convert.ToInt32(CommFun.IsNullCheck(dtTran.Rows[a]["MaxNo"], CommFun.datatypes.vartypenumeric));

                        }
                    }

                    dtTran.Dispose();


                    tran.Commit();
                }
                catch (Exception ex)
                {
                    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                }
                finally
                {
                    BsfGlobal.g_CRMDB.Close();
                    //conn.Close();
                    //conn.Dispose();
                }
            }
            return iRegTransId;
        }

        public static DataTable FillRentTrans(int argRegTransId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = String.Format("SELECT Date,Rent as Amount FROM RentSchTrans WHERE RentTransId={0}", argRegTransId);
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

        public static DataTable FillPopStEndDate(int argRegTransId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                //sSql = String.Format("SELECT StartDate,EndDate FROM RentDetail where FlatId={0}", FlatId);
                sSql = "SELECT StartDate,EndDate FROM RentAgreementTrans WHERE RentTransId=" + argRegTransId + "";
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

        public static DataTable PopCostTenant(int TenId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = String.Format("SELECT A.CostCentreId,A.CostCentreName FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre A inner join TenantRegister B on A.CostCentreId=B.CostCentreId WHERE B.TenantId={0}", TenId);
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


        public static DataTable PopFlatTenant(int TenId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = String.Format("SELECT A.FlatId ,A.FlatNo FROM FlatDetails A inner join TenantRegister B on A.FlatId=B.FlatId WHERE B.TenantId={0}", TenId);
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

        public static bool InsertRentDetails(DataTable dt)
        {
            int iFTypeId = 0;
            int iTransId = 0;
            bool bUpdate;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {

                    sSql = String.Format("INSERT INTO RentDetail(RefNo,TenantId,CostCentreId,FlatId,RegDate,StartDate,EndDate,Rent,RentDuration,Advance,IntRate,IntDuration,Terms,RenewType,GracePriod,RentType,NetRent,AgreementNo) Values('{0}','{1}',{2},{3},'{4}','{5}','{6}', {7},'{8}',{9},{10},'{11}','{12}','{13}',{14},'{15}','{16}','{17}') SELECT SCOPE_IDENTITY()", RentDetBO.RefNo, RentDetBO.TenantId, RentDetBO.CostCentreId, RentDetBO.FlatId, RentDetBO.RegDate, RentDetBO.StartDate, RentDetBO.EndDate, RentDetBO.Rent, RentDetBO.RentDuration, RentDetBO.Advance, RentDetBO.IntRate, RentDetBO.IntDuration, RentDetBO.Terms, RentDetBO.RenewType, RentDetBO.GracePriod, RentDetBO.RentType, RentDetBO.NetRent, RentDetBO.AgreementNo);
                    cmd = new SqlCommand(sSql, conn, tran);
                    iFTypeId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();

                    sSql = String.Format("INSERT INTO RentAgreementTrans(RentId,RefNo,RegDate,StartDate,EndDate,Rent,NetRent,RentDuration,Advance,IntRate,IntDuration,Terms,RenewType,GracePriod,RentType) Values ({0},'{1}','{2}','{3}','{4}','{5}','{6}', '{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}') SELECT SCOPE_IDENTITY()", iFTypeId,RentDetBO.RefNo, RentDetBO.RegDate, RentDetBO.StartDate, RentDetBO.EndDate, RentDetBO.Rent, RentDetBO.NetRent, RentDetBO.RentDuration, RentDetBO.Advance, RentDetBO.IntRate, RentDetBO.IntDuration, RentDetBO.Terms, RentDetBO.RenewType, RentDetBO.GracePriod, RentDetBO.RentType);
                    cmd = new SqlCommand(sSql, conn, tran);
                    iTransId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();

                    if (dt.Rows.Count > 0)
                    {
                            for (int k = 0; k < dt.Rows.Count; k++)
                            {
                                sSql = String.Format("INSERT INTO Rentschtrans(RentTransId,Date,Rent) values({0},'{1:dd-MMM-yyyy}',{2})", iTransId, Convert.ToDateTime(dt.Rows[k]["Date"]), dt.Rows[k]["Amount"]);
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }                                                
                    }    
                              
                    tran.Commit();
                    bUpdate = true;
                    BsfGlobal.InsertLog(DateTime.Now, "Rent-Add", "N", "Rent", iFTypeId, RentDetBO.CostCentreId, 0, BsfGlobal.g_sCRMDBName, RentDetBO.RefNo, BsfGlobal.g_lUserId);

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

        public static bool UpdateRentDetails(int argTransId, DataTable dtGrid)
        {
            
            bool bUpdate;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = String.Format("UPDATE RentDetail SET RefNo='{0}', TenantId='{1}', CostCentreId={2},FlatId={3}, RegDate='{4}', StartDate='{5}',EndDate='{6}',Rent='{7}',RentDuration='{8}',Advance='{9}' ,IntRate={10},IntDuration='{11}',Terms='{12}',RenewType='{13}',GracePriod={14},RentType='{15}',NetRent='{16}',AgreementNo='{17}' WHERE RentId={18}", RentDetBO.RefNo, RentDetBO.TenantId, RentDetBO.CostCentreId, RentDetBO.FlatId, RentDetBO.RegDate, RentDetBO.StartDate, RentDetBO.EndDate, RentDetBO.Rent, RentDetBO.RentDuration, RentDetBO.Advance, RentDetBO.IntRate, RentDetBO.IntDuration, RentDetBO.Terms, RentDetBO.RenewType, RentDetBO.GracePriod, RentDetBO.RentType, RentDetBO.NetRent, RentDetBO.AgreementNo, RentDetBO.RentId); 
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = String.Format("UPDATE RentAgreementTrans SET RefNo='{0}',RegDate='{1}',StartDate='{2}',EndDate='{3}',Rent='{4}',NetRent='{5}',RentDuration='{6}',Advance='{7}',IntRate='{8}',IntDuration='{9}',Terms='{10}',RenewType='{11}',GracePriod='{12}',RentType='{13}' WHERE RentTransId={14} ", RentDetBO.RefNo, RentDetBO.RegDate, RentDetBO.StartDate, RentDetBO.EndDate, RentDetBO.Rent, RentDetBO.NetRent, RentDetBO.RentDuration, RentDetBO.Advance, RentDetBO.IntRate, RentDetBO.IntDuration, RentDetBO.Terms, RentDetBO.RenewType, RentDetBO.GracePriod, RentDetBO.RentType, argTransId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = String.Format("DELETE FROM RentSchTrans WHERE RentTransId={0}", argTransId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    if (dtGrid.Rows.Count > 0)
                    {
                        for (int a = 0; a < dtGrid.Rows.Count; a++)
                        {
                            sSql = String.Format("INSERT INTO Rentschtrans(RentTransId,Date,Rent) values({0},'{1:dd-MMM-yyyy}',{2})", argTransId, Convert.ToDateTime(dtGrid.Rows[a]["Date"]), dtGrid.Rows[a]["Amount"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }    
                               
                    tran.Commit();
                    bUpdate = true;

                    BsfGlobal.InsertLog(DateTime.Now, "Rent-Edit", "E", "Rent", RentDetBO.RentId, RentDetBO.CostCentreId, 0, BsfGlobal.g_sCRMDBName, RentDetBO.RefNo, BsfGlobal.g_lUserId);


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

        public static bool UpdateNewRentDetails(int argTransId, bool argbAdd, DataTable dt)
        {
            int iTransId = 0;
            bool bUpdate;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    if (argbAdd == true)
                    {
                        sSql = String.Format("UPDATE RentDetail SET RefNo='{0}', TenantId='{1}', CostCentreId={2},FlatId={3}, RegDate='{4}', StartDate='{5}',EndDate='{6}',Rent='{7}',RentDuration='{8}',Advance='{9}' ,IntRate={10},IntDuration='{11}',Terms='{12}',RenewType='{13}',GracePriod={14}, RentType='{15}',AgreementNo='{16}' WHERE RentId={17}", RentDetBO.RefNo, RentDetBO.TenantId, RentDetBO.CostCentreId, RentDetBO.FlatId, RentDetBO.RegDate, RentDetBO.StartDate, RentDetBO.EndDate, RentDetBO.Rent, RentDetBO.RentDuration, RentDetBO.Advance, RentDetBO.IntRate, RentDetBO.IntDuration, RentDetBO.Terms, RentDetBO.RenewType, RentDetBO.GracePriod, RentDetBO.RentType, RentDetBO.AgreementNo, RentDetBO.RentId);
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = String.Format("INSERT INTO RentAgreementTrans(RentId,RefNo,RegDate,StartDate,EndDate,Rent,NetRent,RentDuration,Advance,IntRate,IntDuration,Terms,RenewType,GracePriod,RentType) Values ({0},'{1}','{2}','{3}','{4}','{5}','{6}', '{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}') SELECT SCOPE_IDENTITY()", RentDetBO.RentId, RentDetBO.RefNo, RentDetBO.RegDate, RentDetBO.StartDate, RentDetBO.EndDate, RentDetBO.Rent, RentDetBO.NetRent, RentDetBO.RentDuration, RentDetBO.Advance, RentDetBO.IntRate, RentDetBO.IntDuration, RentDetBO.Terms, RentDetBO.RenewType, RentDetBO.GracePriod, RentDetBO.RentType);
                        cmd = new SqlCommand(sSql, conn, tran);
                        iTransId = int.Parse(cmd.ExecuteScalar().ToString());
                        cmd.Dispose();

                        //sSql = String.Format("DELETE FROM RentSchTrans WHERE RentId={0}", RentDetBO.RentId);
                        //cmd = new SqlCommand(sSql, conn, tran);
                        //cmd.ExecuteNonQuery();
                        //cmd.Dispose();
                        if (dt.Rows.Count > 0)
                        {
                            for (int a = 0; a < dt.Rows.Count; a++)
                            {
                                sSql = String.Format("INSERT INTO Rentschtrans(RentTransId,Date,Rent) values({0},'{1:dd-MMM-yyyy}',{2})", iTransId, Convert.ToDateTime(dt.Rows[a]["Date"]), dt.Rows[a]["Amount"]);
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }
                    }
                    else
                    {
                        sSql = String.Format("UPDATE RentDetail SET RefNo='{0}', TenantId='{1}', CostCentreId={2},FlatId={3}, RegDate='{4}', StartDate='{5}',EndDate='{6}',Rent='{7}',RentDuration='{8}',Advance='{9}' ,IntRate={10},IntDuration='{11}',Terms='{12}',RenewType='{13}',GracePriod={14}, RentType='{15}',AgreementNo='{16}' WHERE RentId={17}", RentDetBO.RefNo, RentDetBO.TenantId, RentDetBO.CostCentreId, RentDetBO.FlatId, RentDetBO.RegDate, RentDetBO.StartDate, RentDetBO.EndDate, RentDetBO.Rent, RentDetBO.RentDuration, RentDetBO.Advance, RentDetBO.IntRate, RentDetBO.IntDuration, RentDetBO.Terms, RentDetBO.RenewType, RentDetBO.GracePriod, RentDetBO.RentType, RentDetBO.AgreementNo, RentDetBO.RentId);
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = String.Format("UPDATE RentAgreementTrans SET RefNo='{0}',RegDate='{1}',StartDate='{2}',EndDate='{3}',Rent='{4}',NetRent='{5}',RentDuration='{6}',Advance='{7}',IntRate='{8}',IntDuration='{9}',Terms='{10}',RenewType='{11}',GracePriod='{12}',RentType='{13}' WHERE RentTransId={14} ", RentDetBO.RefNo, RentDetBO.RegDate, RentDetBO.StartDate, RentDetBO.EndDate, RentDetBO.Rent, RentDetBO.NetRent, RentDetBO.RentDuration, RentDetBO.Advance, RentDetBO.IntRate, RentDetBO.IntDuration, RentDetBO.Terms, RentDetBO.RenewType, RentDetBO.GracePriod, RentDetBO.RentType, argTransId);
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = String.Format("DELETE FROM RentSchTrans WHERE RentTransId={0}", argTransId);
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        if (dt.Rows.Count > 0)
                        {
                            for (int a = 0; a < dt.Rows.Count; a++)
                            {
                                sSql = String.Format("INSERT INTO Rentschtrans(RentTransId,Date,Rent) values({0},'{1:dd-MMM-yyyy}',{2})", argTransId, Convert.ToDateTime(dt.Rows[a]["Date"]), dt.Rows[a]["Amount"]);
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }   
                     
                    }

                    tran.Commit();
                    bUpdate = true;
                    if (argbAdd == true)
                    {
                        BsfGlobal.InsertLog(DateTime.Now, "Rent-Add", "N", "Rent", RentDetBO.RentId, RentDetBO.CostCentreId, 0, BsfGlobal.g_sCRMDBName, RentDetBO.RefNo, BsfGlobal.g_lUserId);
                    }
                    else
                    {
                        BsfGlobal.InsertLog(DateTime.Now, "Rent-Edit", "E", "Rent", RentDetBO.RentId, RentDetBO.CostCentreId, 0, BsfGlobal.g_sCRMDBName, RentDetBO.RefNo, BsfGlobal.g_lUserId);
                    }

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

        public static DataTable PopulateRentRegister(DateTime frmDate, DateTime toDate)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();

                string frmdat = string.Format("{0:dd MMM yyyy}", frmDate);
                string tdat = string.Format("{0:dd MMM yyyy}", toDate.AddDays(0));

                sSql = "SELECT C.RentId,C.RegDate as Date,C.RefNo,C.CostCentreId,C1.CostCentreName,F.FlatNo,E.TenantName, C.StartDate,C.EndDate, " +
                         "Case (C.RentType) When 'R' then 'Rent' else 'Lease' END AS RentType,C.Advance as AdvanceAmount, C.NetRent as Rent, C.Approve FROM RentDetail C " +
                         "INNER JOIN FlatDetails F ON C.FlatId=F.FlatId " +
                         "INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C1  ON C.CostCentreId=C1.CostCentreId " +
                         "INNER JOIN TenantRegister E ON C.TenantId=E.TenantId " +
                         "Where C.RegDate between '" + frmdat + "'  And '" + tdat + "' ORDER BY C.RegDate,C.RefNo";
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

        public static DataTable PopulateRentRegMaster(int argRentId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();

              
                sSql = "SELECT C.RentId,C.RegDate as Date,C.RefNo,C.CostCentreId,C1.CostCentreName,F.FlatNo,E.TenantName, C.StartDate,C.EndDate, " +
                         "Case (C.RentType) When 'R' then 'Rent' else 'Lease' END AS RentType,C.Advance as AdvanceAmount, C.NetRent as Rent, C.Approve FROM RentDetail C " +
                         "INNER JOIN FlatDetails F ON C.FlatId=F.FlatId " +
                         "INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C1  ON C.CostCentreId=C1.CostCentreId " +
                         "INNER JOIN TenantRegister E ON C.TenantId=E.TenantId " +
                         "Where C.RentId=" + argRentId + "";
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

        public static DataTable PopulateRentRegisterChange(int argRenTranstId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();

                sSql = "SELECT RentTransId,RentId,RefNo,RegDate,StartDate,EndDate,Rent,NetRent,Case (RenewType) When 'N' then 'New' When 'R' then 'Renewal' else 'Cancel' END AS RenewType,Approve from RentAgreementTrans WHERE RentTransId=" + argRenTranstId + "";
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

        public static bool DeleteRentRegister(int RegId, int argCostId, string argVouNo)
        {
            string sSql = "";
            bool bSuccess = false;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();

             try
             {
                 sSql = String.Format("DELETE FROM RentDetail WHERE RentId={0}", RegId);
                 cmd = new SqlCommand(sSql, conn, tran);
                 cmd.ExecuteNonQuery();
                 cmd.Dispose();

                 sSql = String.Format("DELETE FROM RentSchTrans WHERE RentId={0}", RegId);
                 cmd = new SqlCommand(sSql, conn, tran);
                 cmd.ExecuteNonQuery();
                 cmd.Dispose();

                 BsfGlobal.InsertLog(DateTime.Now, "Rent-Delete", "D", "Rent", RegId, argCostId, 0, BsfGlobal.g_sCRMDBName, argVouNo, BsfGlobal.g_lUserId);

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

        public static bool DeleteRentAgreementRegister(int RegId, int argCostId, string argVouNo, int RegTransId, string argType)
        {
            int iRegTransId = 0;
            string sRefNo = "";
            string sRegDate, sStDate, sEndDate;
            string sRentDuration = "";
            string sIntDuration = "";
            string sTerms = "";
            string sRenewType = "";
            decimal dGPriod = 0;
            string sRentType = "";
            // DateTime RegDate,StDate, EndDate;
            decimal dRent = 0;
            decimal dNetRent = 0;
            decimal dAdvance = 0;
            decimal dIntRate = 0;

            string sSql = "";
            bool bSuccess = false;
            DataTable dtTran, dtUpTran;
            SqlDataReader sdr,sdrT;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();

            try
            {
                sSql = String.Format("DELETE FROM RentAgreementTrans WHERE RentTransId={0}", RegTransId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                if (argType == "Renewal" || argType == "Cancel")
                {
                    dtTran = new DataTable();
                    sSql = "SELECT Max(RentTransId) MaxNo FROM RentAgreementTrans WHERE RentId=" + RegId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    sdr = cmd.ExecuteReader();
                    dtTran.Load(sdr);
                    cmd.Dispose();
                    if (dtTran.Rows.Count > 0)
                    {
                        for (int a = 0; a < dtTran.Rows.Count; a++)
                        {
                            iRegTransId = Convert.ToInt32(CommFun.IsNullCheck(dtTran.Rows[a]["MaxNo"], CommFun.datatypes.vartypenumeric));
                        }
                    }
                    dtTran.Dispose();

                    dtUpTran = new DataTable();
                    sSql = "SELECT * FROM RentAgreementTrans WHERE RentTransId=" + iRegTransId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    sdrT = cmd.ExecuteReader();
                    dtUpTran.Load(sdrT);
                    cmd.Dispose();
                    if (dtUpTran.Rows.Count > 0)
                    {
                        for (int b = 0; b < dtUpTran.Rows.Count; b++)
                        {
                            
                            sRefNo = dtUpTran.Rows[b]["RefNo"].ToString();
                            sRegDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtUpTran.Rows[b]["RegDate"].ToString(), CommFun.datatypes.VarTypeDate));
                            sStDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtUpTran.Rows[b]["StartDate"].ToString(), CommFun.datatypes.VarTypeDate));
                            sEndDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtUpTran.Rows[b]["EndDate"].ToString(), CommFun.datatypes.VarTypeDate));
                            //RegDate = Convert.ToDateTime(dtUpTran.Rows[b]["RegDate"].ToString());
                            //StDate = Convert.ToDateTime(dtUpTran.Rows[b]["StartDate"].ToString());
                            //EndDate = Convert.ToDateTime(dtUpTran.Rows[b]["EndDate"].ToString());
                            dRent = Convert.ToDecimal(dtUpTran.Rows[b]["Rent"].ToString());
                            dNetRent = Convert.ToDecimal(dtUpTran.Rows[b]["NetRent"].ToString());
                            sRentDuration = dtUpTran.Rows[b]["RentDuration"].ToString();
                            dAdvance = Convert.ToDecimal(dtUpTran.Rows[b]["Advance"].ToString());
                            dIntRate = Convert.ToDecimal(dtUpTran.Rows[b]["IntRate"].ToString());
                            sIntDuration = dtUpTran.Rows[b]["IntDuration"].ToString();
                            sTerms = dtUpTran.Rows[b]["Terms"].ToString();
                            sRenewType = dtUpTran.Rows[b]["RenewType"].ToString();
                            dGPriod = Convert.ToDecimal(dtUpTran.Rows[b]["GracePriod"].ToString());
                            sRentType = dtUpTran.Rows[b]["RentType"].ToString();


                            sSql = "UPDATE RentDetail SET RefNo='" + sRefNo + "',RegDate='" + sRegDate + "',StartDate='" + sStDate + "',EndDate='" + sEndDate + "'," +
                                " Rent='" + dRent + "',NetRent='" + dNetRent + "',RentDuration='" + sRentDuration + "',Advance='" + dAdvance + "',IntRate='" + dIntRate + "',IntDuration='" + sIntDuration + "'," +
                                " Terms='" + sTerms + "',RenewType='" + sRenewType + "',GracePriod='" + dGPriod + "',RentType='" + sRentType + "' WHERE RentId=" + RegId + " ";

                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    dtUpTran.Dispose();
                }
                else
                {
                    sSql = String.Format("DELETE FROM RentDetail WHERE RentId={0}", RegId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                sSql = String.Format("DELETE FROM RentSchTrans WHERE RentTransId={0}", RegTransId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                BsfGlobal.InsertLog(DateTime.Now, "Rent-Delete", "D", "Rent", RegId, argCostId, 0, BsfGlobal.g_sCRMDBName, argVouNo, BsfGlobal.g_lUserId);

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

        public static bool CheckTransId(int RegTransId, int RegId)
        {
            string sSql = "";
            SqlDataReader sdr;
            DataTable dt;
            SqlCommand cmd;
            bool bStatus = false;
            DataView dv;
            try
            {
                BsfGlobal.g_CRMDB.Close();
                BsfGlobal.g_CRMDB.Open();

                sSql = "SELECT Max(RentTransId) MaxNo FROM RentAgreementTrans WHERE RentId=" + RegId + " ";

                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                sdr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(sdr);
                dv = new DataView(dt);
                dv.RowFilter = "MaxNo='" + RegTransId + "' ";
                if (dv.ToTable().Rows.Count > 0) bStatus = true; else bStatus = false;
            }
            catch (Exception ce)
            {
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return bStatus;


        }

        public static DataTable CheckRentDet(string RefNo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT * FROM RentDetail WHERE AgreementNo='" + RefNo + "'";
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

        public static DataTable Fill_AgreementDetail(int argRentId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT * FROM RentDetail WHERE RentId=" + argRentId + " ";
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

        public static DataTable FillAgreementTransDetails(int argRentId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT RentTransId,RentId,RefNo,RegDate,StartDate,EndDate,Rent,NetRent,Case (RenewType) When 'N' then 'New' When 'R' then 'Renewal' else 'Cancel' END AS RenewType,Approve from RentAgreementTrans WHERE RentId=" + argRentId + " ORDER BY RegDate,RefNo ";
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

    }
}
