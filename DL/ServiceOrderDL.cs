using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessObjects;

namespace CRM.DataLayer
{
    class ServiceOrderDL
    {


        public static DataTable GetServices()
        {
            DataTable dtService = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenVendorAnalDB();
                sSql = String.Format("SELECT ServiceId,ServiceName FROM [{0}].dbo.ServiceMaster order by ServiceName", BsfGlobal.g_sVendorDBName);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_VendorDB);
                dtService = new DataTable();
                sda.Fill(dtService);
       
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_VendorDB.Close();
            }
            return dtService;
        }

        public static DataTable PopulateCostCentre(int Id)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT CostCentreId,CostCentreName from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                    " Where ProjectDB in(Select ProjectName from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister" +
                    " Where BusinessType In('B','L')) and CostCentreId not in (Select CostCentreId " +
                    " From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where " +
                    " UserId=" + Id + ") Order By CostCentreName";

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


        public static DataTable Populate_SerQuoteRegister(int RegId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                //sSql = "Select * from ServiceQuoteReg Where RegisterId = " + RegId;
                sSql = "SELECT A.RegisterId,A.SDate,A.RefNo,A.CostcentreID,A.FlatID,A.LeadId,A.GrossAmt,A.QualifierAmt,A.NetAmt,A.Remarks,B.LeadName,A.Approve FROM ServiceQuoteReg A inner join LeadRegister B on A.LeadId=B.LeadId WHERE A.RegisterId=" + RegId + "";
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

        public static DataTable Populate_SerQuoteTrans(int RegId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = String.Format("SELECT B.ServiceName as Description,B.ServiceId,A.Amount FROM serQuoteTrans A inner join [" + BsfGlobal.g_sVendorDBName + "].dbo.ServiceMaster B on A.ServiceID=B.ServiceId WHERE A.RegisterID={0}", RegId);
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

        public static DataTable Populate_SerQuoteListTrans(int RegId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = String.Format("SELECT * FROM ServiceQuoteRateQ WHERE BillRegId={0}", RegId);
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

        public static bool InsertSerOrderDetails(DataTable dtComPList, DataTable argQTrans)
        {
            int iFRegId = 0;
            bool bUpdate;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = String.Format("INSERT INTO ServiceQuoteReg(SDate,RefNo,CostcentreID,FlatID,GrossAmt,NetAmt,Remarks,LeadId,QualifierAmt) Values('{0}','{1}',{2},{3},'{4}','{5}','{6}',{7},'{8}') SELECT SCOPE_IDENTITY()", ServiceOrderBO.SDate, ServiceOrderBO.RefNo, ServiceOrderBO.CostcentreID, ServiceOrderBO.FlatID, ServiceOrderBO.GrossAmt, ServiceOrderBO.NetAmt, ServiceOrderBO.Remarks, ServiceOrderBO.BuyerId, ServiceOrderBO.QualifierAmt);
                    cmd = new SqlCommand(sSql, conn, tran);
                    iFRegId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();
                    if (dtComPList.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtComPList.Rows.Count; k++)
                        {
                            sSql = String.Format("INSERT INTO serQuoteTrans(RegisterID,ServiceID,Amount) values({0},{1},'{2}')", iFRegId, (dtComPList.Rows[k]["ServiceID"]), dtComPList.Rows[k]["Amount"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    //Bill Qualifier
                    for (int u = 0; u < argQTrans.Rows.Count; u++)
                    {
                        sSql = "Insert Into ServiceQuoteRateQ (BillRegId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,Amount,ExpValue,ExpPerValue,SurValue,EdValue) " +
                            "Values (" + iFRegId + "," + argQTrans.Rows[u]["QualifierId"].ToString() + ",'" + argQTrans.Rows[u]["Expression"].ToString() + "'," + argQTrans.Rows[u]["ExpPer"].ToString() + ",'" + argQTrans.Rows[u]["Add_Less_Flag"].ToString() + "'," + argQTrans.Rows[u]["SurCharge"].ToString() + "," + argQTrans.Rows[u]["EDCess"].ToString() + "," + argQTrans.Rows[u]["Amount"].ToString() + "," + argQTrans.Rows[u]["ExpValue"].ToString() + "," + argQTrans.Rows[u]["ExpPerValue"].ToString() + "," + argQTrans.Rows[u]["SurValue"].ToString() + "," + argQTrans.Rows[u]["EdValue"].ToString() + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                    }
                    tran.Commit();
                    bUpdate = true;
                    BsfGlobal.InsertLog(DateTime.Now, "Service-Quote-Add", "N", "ServiceQuote", iFRegId, ServiceOrderBO.CostcentreID, 0, BsfGlobal.g_sCRMDBName, ServiceOrderBO.RefNo, BsfGlobal.g_lUserId);

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


        public static bool UpdateSerOrderDetails(DataTable dtComPList, DataTable argQTrans)
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
                    sSql = String.Format("UPDATE ServiceQuoteReg SET SDate='{0}',RefNo='{1}',CostcentreID={2},FlatID={3},GrossAmt='{4}',NetAmt='{5}',Remarks='{6}', LeadId={7}, QualifierAmt='{8}' WHERE RegisterID={9}", ServiceOrderBO.SDate, ServiceOrderBO.RefNo, ServiceOrderBO.CostcentreID, ServiceOrderBO.FlatID, ServiceOrderBO.GrossAmt, ServiceOrderBO.NetAmt, ServiceOrderBO.Remarks, ServiceOrderBO.BuyerId, ServiceOrderBO.QualifierAmt, ServiceOrderBO.RegisterId);                
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                
                    sSql = String.Format("DELETE FROM serQuoteTrans WHERE RegisterID={0}", ServiceOrderBO.RegisterId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = String.Format("DELETE FROM ServiceQuoteRateQ WHERE BillRegId={0}", ServiceOrderBO.RegisterId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (dtComPList.Rows.Count > 0)
                    {
                        for (int a = 0; a < dtComPList.Rows.Count; a++)
                        {
                            sSql = String.Format("INSERT INTO serQuoteTrans(RegisterID,ServiceID,Amount) values({0},{1},'{2}')", ServiceOrderBO.RegisterId, (dtComPList.Rows[a]["ServiceID"]), dtComPList.Rows[a]["Amount"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    //Bill Qualifier
                    for (int u = 0; u < argQTrans.Rows.Count; u++)
                    {
                        sSql = "Insert Into ServiceQuoteRateQ (BillRegId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,Amount,ExpValue,ExpPerValue,SurValue,EdValue) " +
                            "Values (" + ServiceOrderBO.RegisterId + "," + argQTrans.Rows[u]["QualifierId"].ToString() + ",'" + argQTrans.Rows[u]["Expression"].ToString() + "'," + argQTrans.Rows[u]["ExpPer"].ToString() + ",'" + argQTrans.Rows[u]["Add_Less_Flag"].ToString() + "'," + argQTrans.Rows[u]["SurCharge"].ToString() + "," + argQTrans.Rows[u]["EDCess"].ToString() + "," + argQTrans.Rows[u]["Amount"].ToString() + "," + argQTrans.Rows[u]["ExpValue"].ToString() + "," + argQTrans.Rows[u]["ExpPerValue"].ToString() + "," + argQTrans.Rows[u]["SurValue"].ToString() + "," + argQTrans.Rows[u]["EdValue"].ToString() + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                    }
                    tran.Commit();
                    bUpdate = true;
                    BsfGlobal.InsertLog(DateTime.Now, "Service-Quote-Edit", "E", "ServiceQuote", ServiceOrderBO.RegisterId, ServiceOrderBO.CostcentreID, 0, BsfGlobal.g_sCRMDBName, ServiceOrderBO.RefNo, BsfGlobal.g_lUserId);

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

        public static DataTable Fill_SerQuoteRegister(DateTime frmDate, DateTime toDate)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();

                string frmdat = string.Format("{0:dd MMM yyyy}", frmDate);
                string tdat = string.Format("{0:dd MMM yyyy}", toDate.AddDays(0));

                sSql = "SELECT A.RegisterId,A.SDate,A.RefNo,A.CostcentreID,B.CostCentreName,C.FlatNo,E.LeadName BuyerName, A.NetAmt,A.Approve from ServiceQuoteReg A " +
                               "Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B on A.CostCentreId=B.CostCentreId " +
                               "Inner Join FlatDetails C on A.FlatId=C.FlatId " +
                               "Left Join LeadRegister E on A.LeadId=E.LeadId " +
                                "Where A.SDate between '" + frmdat + "'  And '" + tdat + "' Order by A.SDate,A.RefNo";
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

        public static DataTable Fill_SerQuoteRegisterChange(int argRegId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
             
                sSql = "SELECT A.RegisterId,A.SDate,A.RefNo,A.CostcentreID,B.CostCentreName,C.FlatNo,E.LeadName BuyerName, A.NetAmt,A.Approve from ServiceQuoteReg A " +
                               "Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B on A.CostCentreId=B.CostCentreId " +
                               "Inner Join FlatDetails C on A.FlatId=C.FlatId " +
                               "Left Join LeadRegister E on A.LeadId=E.LeadId " +
                                "Where A.RegisterId =" + argRegId + "";
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

        public static bool DeleteSerQuoteRegister(int RegId, int argCostId, string argVouNo)
        {
            string sSql = "";
            bool bSuccess = false;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();

            try
            {
                sSql = String.Format("DELETE FROM ServiceQuoteReg WHERE RegisterId={0}", RegId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = String.Format("DELETE FROM serQuoteTrans WHERE RegisterId={0}", RegId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = String.Format("DELETE FROM ServiceQuoteRateQ WHERE BillRegId={0}", RegId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                BsfGlobal.InsertLog(DateTime.Now, "Service-Quote-Delete", "D", "ServiceQuote", RegId, argCostId, 0, BsfGlobal.g_sCRMDBName, argVouNo, BsfGlobal.g_lUserId);

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


        public static DataTable PopulateCostCentreLead(int ProId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = String.Format("SELECT FlatId,FlatNo FROM FlatDetails WHERE CostCentreId={0} and Status='S' ORDER BY FlatNo", ProId);

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

        public static DataTable PopulateProjectLead(int FlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = String.Format("SELECT A.LeadId,B.LeadName FROM BuyerDetail A inner join LeadRegister B on A.LeadId=B.LeadId WHERE A.FlatId={0}", FlatId);

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
