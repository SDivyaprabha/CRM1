using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessObjects;

namespace CRM.DataLayer
{
    class MaintenanceDL
    {

        public static DataTable GetCostCentre()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenWorkFlowDB();
            try
            {
                sSql = "Select CostCentreId,CostCentreName from dbo.OperationalCostCentre" +
                        " Where ProjectDB in(Select ProjectName from " +
                        " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType in('B','L'))" +
                        " and CostCentreId not in (Select CostCentreId From dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") Order by CostCentreName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dt;
        }

        public static DataTable GetFlat(int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select FlatId,FlatNo From dbo.FlatDetails Where CostCentreId=" + argCCId + " And Status='S'";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static bool InsertMainDetails(DataTable dt, DataTable argQTrans)
        {
            int iMainId = 0, iMainTransId=0;
            bool bUpdate;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {

                    sSql = "INSERT INTO dbo.MaintenanceDet(RefNo,CostCentreId,FlatId,RegDate,StartDate,EndDate,Duration,IntRate,IntDuration," +
                        " Terms,GracePeriod,Approve) Values( '" + MaintenanceBO.RefNo + "'," + MaintenanceBO.CostCentreId + "," + MaintenanceBO.FlatId + ",'" + MaintenanceBO.RegDate + "','" + MaintenanceBO.StartDate + "'," +
                        " '" + MaintenanceBO.EndDate + "','" + MaintenanceBO.Duration + "'," + MaintenanceBO.IntRate + ",'" + MaintenanceBO.IntDuration + "', " +
                        " '" + MaintenanceBO.Terms + "', " + MaintenanceBO.GracePeriod + ", '" + MaintenanceBO.Approve + "') SELECT SCOPE_IDENTITY()";
                    cmd = new SqlCommand(sSql, conn, tran);
                    iMainId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();

                    if (dt.Rows.Count > 0)
                    {
                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            int iRowId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[k]["RowId"], CommFun.datatypes.vartypenumeric));
                            sSql = String.Format("INSERT INTO dbo.MaintenanceSchTrans(MaintenanceId,FromDate,ToDate,Amount,NetAmount) Values({0},'{1:dd-MMM-yyyy}','{2:dd-MMM-yyyy}',{3},{4})SELECT SCOPE_IDENTITY()", iMainId, Convert.ToDateTime(dt.Rows[k]["FromDate"]), Convert.ToDateTime(dt.Rows[k]["ToDate"]), dt.Rows[k]["Amount"], dt.Rows[k]["NetAmount"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            iMainTransId = int.Parse(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();

                            DataView dv = new DataView(argQTrans);
                            dv.RowFilter = "RowId=" + iRowId + "";
                            DataTable dtQ = new DataTable();
                            dtQ = dv.ToTable();

                            for (int i = 0; i < dtQ.Rows.Count; i++)
                            {
                                sSql = "Insert Into dbo.MaintenanceQualifier (MainTransId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,Amount,ExpValue,ExpPerValue,SurValue,EDValue,NetPer,HEDPer,TaxablePer,TaxableValue) " +
                                "Values (" + iMainTransId + "," + dtQ.Rows[i]["QualifierId"].ToString() + ",'" + dtQ.Rows[i]["Expression"].ToString() + "'," + dtQ.Rows[i]["ExpPer"].ToString() + "," +
                                " '" + dtQ.Rows[i]["Add_Less_Flag"].ToString() + "'," + dtQ.Rows[i]["SurCharge"].ToString() + "," +
                                " " + dtQ.Rows[i]["EDCess"].ToString() + "," + dtQ.Rows[i]["Amount"].ToString() + "," + dtQ.Rows[i]["ExpValue"].ToString() + "," +
                                " " + dtQ.Rows[i]["ExpPerValue"].ToString() + "," + dtQ.Rows[i]["SurValue"].ToString() + "," + dtQ.Rows[i]["EDValue"].ToString() + "," +
                                " " + dtQ.Rows[i]["NetPer"].ToString() + "," + dtQ.Rows[i]["HEDPer"].ToString() + "," + dtQ.Rows[i]["TaxablePer"].ToString() + "," +
                                " " + dtQ.Rows[i]["TaxableValue"].ToString() + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }
                    }

                    ////Qualifier
                    //for (int u = 0; u < argQTrans.Rows.Count; u++)
                    //{
                    //    sSql = "Insert Into dbo.MaintenanceQualifier (MainTransId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,Amount,ExpValue,ExpPerValue,SurValue,EDValue,NetPer,HEDPer,TaxablePer,TaxableValue) " +
                    //        "Values (" + iMainId + "," + argQTrans.Rows[u]["QualifierId"].ToString() + ",'" + argQTrans.Rows[u]["Expression"].ToString() + "'," + argQTrans.Rows[u]["ExpPer"].ToString() + "," +
                    //        " '" + argQTrans.Rows[u]["Add_Less_Flag"].ToString() + "'," + argQTrans.Rows[u]["SurCharge"].ToString() + "," +
                    //        " " + argQTrans.Rows[u]["EDCess"].ToString() + "," + argQTrans.Rows[u]["Amount"].ToString() + "," + argQTrans.Rows[u]["ExpValue"].ToString() + "," +
                    //        " " + argQTrans.Rows[u]["ExpPerValue"].ToString() + "," + argQTrans.Rows[u]["SurValue"].ToString() + "," + argQTrans.Rows[u]["EDValue"].ToString() + "," +
                    //        " " + argQTrans.Rows[u]["NetPer"].ToString() + "," + argQTrans.Rows[u]["HEDPer"].ToString() + "," + argQTrans.Rows[u]["TaxablePer"].ToString() + "," +
                    //        " " + argQTrans.Rows[u]["TaxableValue"].ToString() + ")";
                    //    cmd = new SqlCommand(sSql, conn, tran);
                    //    cmd.ExecuteNonQuery();
                    //}

                    tran.Commit();
                    bUpdate = true;
                    BsfGlobal.InsertLog(DateTime.Now, "CRM-Maintenance-Bill-Add", "N", "CRM-Maintenance-Bill", iMainId, MaintenanceBO.CostCentreId, 0,
                        BsfGlobal.g_sCRMDBName, MaintenanceBO.RefNo, BsfGlobal.g_lUserId);
                    //BsfGlobal.InsertLog(DateTime.Now, "Rent-Add", "N", "Rent", iFTypeId, RentDetBO.CostCentreId, 0, BsfGlobal.g_sCRMDBName, RentDetBO.RefNo, BsfGlobal.g_lUserId);

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

        public static bool UpdateMainDetails(int argTransId, DataTable dtGrid, DataTable argQTrans)
        {
            bool bUpdate;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB(); int iMainTransId = 0;
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "UPDATE dbo.MaintenanceDet SET RefNo='" + MaintenanceBO.RefNo + "', CostCentreId=" + MaintenanceBO.CostCentreId + ",FlatId=" + MaintenanceBO.FlatId + ", " +
                        " RegDate='" + MaintenanceBO.RegDate + "',StartDate='" + MaintenanceBO.StartDate + "',EndDate='" + MaintenanceBO.EndDate + "'," +
                        " Duration='" + MaintenanceBO.Duration + "',IntRate=" + MaintenanceBO.IntRate + ",IntDuration='" + MaintenanceBO.IntDuration + "'," +
                        " Terms='" + MaintenanceBO.Terms + "',GracePeriod=" + MaintenanceBO.GracePeriod + ",Approve='" + MaintenanceBO.Approve + "' WHERE MaintenanceId=" + argTransId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "DELETE FROM dbo.MaintenanceQualifier WHERE MainTransId In(Select TransId From MaintenanceSchTrans Where MaintenanceId=" + argTransId + ")";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = String.Format("DELETE FROM dbo.MaintenanceSchTrans WHERE MaintenanceId={0}", argTransId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (dtGrid.Rows.Count > 0)
                    {
                        for (int a = 0; a < dtGrid.Rows.Count; a++)
                        {
                            int iRowId = Convert.ToInt32(CommFun.IsNullCheck(dtGrid.Rows[a]["RowId"], CommFun.datatypes.vartypenumeric));
                            sSql = String.Format("INSERT INTO dbo.MaintenanceSchTrans(MaintenanceId,FromDate,ToDate,Amount,NetAmount) Values({0},'{1:dd-MMM-yyyy}','{2:dd-MMM-yyyy}',{3},{4})SELECT SCOPE_IDENTITY()", argTransId, Convert.ToDateTime(dtGrid.Rows[a]["FromDate"]), Convert.ToDateTime(dtGrid.Rows[a]["ToDate"]), dtGrid.Rows[a]["Amount"], dtGrid.Rows[a]["NetAmount"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            iMainTransId = int.Parse(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();

                            DataView dv = new DataView(argQTrans);
                            dv.RowFilter = "RowId=" + iRowId + "";
                            DataTable dtQ = new DataTable();
                            dtQ = dv.ToTable();

                            for (int i = 0; i < dtQ.Rows.Count; i++)
                            {
                                sSql = "Insert Into dbo.MaintenanceQualifier (MainTransId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,Amount,ExpValue,ExpPerValue,SurValue,EDValue,NetPer,HEDPer,TaxablePer,TaxableValue) " +
                                "Values (" + iMainTransId + "," + dtQ.Rows[i]["QualifierId"].ToString() + ",'" + dtQ.Rows[i]["Expression"].ToString() + "'," + dtQ.Rows[i]["ExpPer"].ToString() + "," +
                                " '" + dtQ.Rows[i]["Add_Less_Flag"].ToString() + "'," + dtQ.Rows[i]["SurCharge"].ToString() + "," +
                                " " + dtQ.Rows[i]["EDCess"].ToString() + "," + dtQ.Rows[i]["Amount"].ToString() + "," + dtQ.Rows[i]["ExpValue"].ToString() + "," +
                                " " + dtQ.Rows[i]["ExpPerValue"].ToString() + "," + dtQ.Rows[i]["SurValue"].ToString() + "," + dtQ.Rows[i]["EDValue"].ToString() + "," +
                                " " + dtQ.Rows[i]["NetPer"].ToString() + "," + dtQ.Rows[i]["HEDPer"].ToString() + "," + dtQ.Rows[i]["TaxablePer"].ToString() + "," +
                                " " + dtQ.Rows[i]["TaxableValue"].ToString() + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }

                        }
                    }

                    ////Qualifier
                    //for (int u = 0; u < argQTrans.Rows.Count; u++)
                    //{
                    //    sSql = "Insert Into dbo.MaintenanceQualifier (MainTransId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,Amount,ExpValue,ExpPerValue,SurValue,EDValue,NetPer,HEDPer,TaxablePer,TaxableValue) " +
                    //        "Values (" + argTransId + "," + argQTrans.Rows[u]["QualifierId"].ToString() + ",'" + argQTrans.Rows[u]["Expression"].ToString() + "'," + argQTrans.Rows[u]["ExpPer"].ToString() + "," +
                    //        " '" + argQTrans.Rows[u]["Add_Less_Flag"].ToString() + "'," + argQTrans.Rows[u]["SurCharge"].ToString() + "," +
                    //        " " + argQTrans.Rows[u]["EDCess"].ToString() + "," + argQTrans.Rows[u]["Amount"].ToString() + "," + argQTrans.Rows[u]["ExpValue"].ToString() + "," +
                    //        " " + argQTrans.Rows[u]["ExpPerValue"].ToString() + "," + argQTrans.Rows[u]["SurValue"].ToString() + "," + argQTrans.Rows[u]["EDValue"].ToString() + "," +
                    //        " " + argQTrans.Rows[u]["NetPer"].ToString() + "," + argQTrans.Rows[u]["HEDPer"].ToString() + "," + argQTrans.Rows[u]["TaxablePer"].ToString() + "," +
                    //        " " + argQTrans.Rows[u]["TaxableValue"].ToString() + ")";
                    //    cmd = new SqlCommand(sSql, conn, tran);
                    //    cmd.ExecuteNonQuery();

                    //}

                    tran.Commit();
                    bUpdate = true;

                    BsfGlobal.InsertLog(DateTime.Now, "CRM-Maintenance-Bill-Modify", "E", "CRM-Maintenance-Bill", argTransId, MaintenanceBO.CostCentreId, 0,
                        BsfGlobal.g_sCRMDBName, MaintenanceBO.RefNo, BsfGlobal.g_lUserId);

                    //BsfGlobal.InsertLog(DateTime.Now, "Rent-Edit", "E", "Rent", RentDetBO.RentId, RentDetBO.CostCentreId, 0, BsfGlobal.g_sCRMDBName, RentDetBO.RefNo, BsfGlobal.g_lUserId);


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

        public static DataTable PopulateMainRegister(DateTime frmDate, DateTime toDate)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();

                string frmdat = string.Format("{0:dd MMM yyyy}", frmDate);
                string tdat = string.Format("{0:dd MMM yyyy}", toDate.AddDays(0));

                sSql = "SELECT C.MaintenanceId,C.FlatId,C.RegDate as Date,C.RefNo,C.CostCentreId,C1.CostCentreName,F.FlatNo,C.StartDate,C.EndDate , C.Approve, " +
                         "NetAmount=(Select SUM(NetAmount)From dbo.MaintenanceSchTrans M Where M.MaintenanceId=C.MaintenanceId) FROM dbo.MaintenanceDet C " +
                         "INNER JOIN dbo.FlatDetails F ON C.FlatId=F.FlatId " +
                         "INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C1  ON C.CostCentreId=C1.CostCentreId " +
                         "Where C.RegDate between '" + frmdat + "'  And '" + tdat + "' ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
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

        public static DataTable FillRegister(int argRegTransId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT * From dbo.MaintenanceDet Where MaintenanceId=" + argRegTransId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
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

        public static DataTable FillTrans(int argRegTransId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = String.Format("SELECT TransId RowId,TransId,FromDate,ToDate,Amount,NetAmount FROM dbo.MaintenanceSchTrans WHERE MaintenanceId={0}", argRegTransId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
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

        public static DataTable Fill_TransQual(int argRegId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT A.MainTransId RowId,A.* FROM dbo.MaintenanceQualifier A INNER JOIN MaintenanceSchTrans B ON A.MainTransId=B.TransId WHERE B.MaintenanceId=" + argRegId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
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

        public static bool DeleteRegister(int argRegId,int argCCId, string argVouNo)
        {
            string sSql = "";
            bool bSuccess = false;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();

            try
            {
                sSql = String.Format("DELETE FROM dbo.MaintenanceDet WHERE MaintenanceId={0}", argRegId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "DELETE FROM dbo.MaintenanceQualifier WHERE MainTransId In(Select TransId From MaintenanceSchTrans Where MaintenanceId=" + argRegId + ")";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = String.Format("DELETE FROM dbo.MaintenanceSchTrans WHERE MaintenanceId={0}", argRegId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                //BsfGlobal.InsertLog(DateTime.Now, "Rent-Delete", "D", "Rent", RegId, argCostId, 0, BsfGlobal.g_sCRMDBName, argVouNo, BsfGlobal.g_lUserId);

                tran.Commit();
                bSuccess = true;

                BsfGlobal.InsertLog(DateTime.Now, "CRM-Maintenance-Bill-Delete", "D", "CRM-Maintenance-Bill", argRegId, MaintenanceBO.CostCentreId, 0,
                    BsfGlobal.g_sCRMDBName, MaintenanceBO.RefNo, BsfGlobal.g_lUserId);

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

        public static DataTable PopulateRegMaster(int argRegId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT C.MaintenanceId,C.RegDate as Date,C.RefNo,C.CostCentreId,C1.CostCentreName,F.FlatNo,C.StartDate,C.EndDate, " +
                         "NetAmount=(Select SUM(NetAmount)From dbo.MaintenanceSchTrans Where MaintenanceId=" + argRegId + "), C.Approve FROM dbo.MaintenanceDet C " +
                         "INNER JOIN FlatDetails F ON C.FlatId=F.FlatId " +
                         "INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C1 ON C.CostCentreId=C1.CostCentreId " +
                         "Where C.MaintenanceId=" + argRegId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
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

        public static bool FoundDate(int argMainId,int argFlatId,DataTable argdt)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            bool b_Ans = false;
            try
            {
                BsfGlobal.OpenCRMDB();
                for (int i = 0; i < argdt.Rows.Count; i++)
                {
                    if (argMainId == 0)
                        sSql = "SELECT * From dbo.MaintenanceDet Where FlatId=" + argFlatId + " And (('" + Convert.ToDateTime(argdt.Rows[i]["FromDate"]).ToString("dd/MMM/yyyy") + "' " +
                            " Between StartDate And EndDate) " +
                            " Or ('" + Convert.ToDateTime(argdt.Rows[i]["ToDate"]).ToString("dd/MMM/yyyy") + "' Between StartDate And EndDate)) ";
                    else
                        sSql = "SELECT * From dbo.MaintenanceDet Where  FlatId=" + argFlatId + " And (('" + Convert.ToDateTime(argdt.Rows[i]["FromDate"]).ToString("dd/MMM/yyyy") + "' " +
                            " Between StartDate And EndDate) " +
                            " Or ('" + Convert.ToDateTime(argdt.Rows[i]["ToDate"]).ToString("dd/MMM/yyyy") + "' Between StartDate And EndDate " +
                            " )) And MaintenanceId Not In (" + argMainId + ")";
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0) { b_Ans = true; }
                }
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return b_Ans;
        }

        public static DataTable GetReport(int argFlatId, int argMaintanceId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "SELECT RefNo,Arrear=(SELECT SUM(F.NetAmount-F.PaidAmount) Arrear " +
                    "FROM dbo.MaintenanceSchTrans F  INNER JOIN dbo.MaintenanceDet M ON M.MaintenanceId=F.MaintenanceId WHERE M.FlatId=" + argFlatId + " AND M.MaintenanceId<" + argMaintanceId + "), " +
                    "BillAmount=(SELECT SUM(F.NetAmount) BillAmount FROM dbo.MaintenanceSchTrans F  INNER JOIN dbo.MaintenanceDet M ON M.MaintenanceId=F.MaintenanceId WHERE M.FlatId=" + argFlatId + " AND M.MaintenanceId=" + argMaintanceId + ") , " +
                    "FromDate=(SELECT min(FromDate) FROM MaintenanceSchTrans F  INNER JOIN dbo.MaintenanceDet M ON M.MaintenanceId=F.MaintenanceId WHERE M.FlatId=" + argFlatId + " AND M.MaintenanceId<" + argMaintanceId + "), " +
                    "ToDate=(SELECT max(ToDate) FROM MaintenanceSchTrans F  INNER JOIN dbo.MaintenanceDet M ON M.MaintenanceId=F.MaintenanceId WHERE M.FlatId=" + argFlatId + " AND M.MaintenanceId=" + argMaintanceId + ") from Maintenancedet M " +
                    "WHERE M.FlatId=" + argFlatId + " AND M.MaintenanceId=" + argMaintanceId;

                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

    }
}
