using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessObjects;

namespace CRM.DataLayer
{
    class ServiceOrderBillDL
    {
        public static DataTable PopulateProject(int Id)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();

                sSql = "SELECT CostCentreId,CostCentreName From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                    " Where ProjectDB in(Select ProjectName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister" +
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

        public static DataTable Fill_SerOrderRegister(int argATRegId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT * FROM SerOrderBillReg WHERE RegBillId =" + argATRegId + "";
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

        public static DataTable Fill_SerOrderTrans(int argATRegId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT B.ServiceName as Description,B.ServiceId,A.Amount FROM SerOrderBillTrans A inner join [" + BsfGlobal.g_sVendorDBName + "].dbo.ServiceMaster B on A.ServiceID=B.ServiceId WHERE A.BillRegId=" + argATRegId + "";
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

        public static DataTable Fill_SerOrderTransQu(int argATRegId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT * FROM ServiceOrderBillRateQ  WHERE BillRegId=" + argATRegId + "";
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


        public static DataTable PopulateQuote(int ProjId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = String.Format("SELECT RegisterId,RefNo FROM ServiceQuoteReg WHERE CostCentreId={0} and Approve='Y' ORDER BY RefNo", ProjId);
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

        public static DataTable PopulateFlatQuote(int QuoteId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = String.Format("SELECT A.FlatID,B.FlatNo,A.LeadId,D.LeadName,A.GrossAmt,A.NetAmt FROM ServiceQuoteReg A inner join FlatDetails B on A.FlatID=B.FlatId inner join BuyerDetail C on C.LeadId=A.LeadId inner join LeadRegister D on D.LeadId=C.LeadId where A.RegisterId={0}", QuoteId);
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

        public static DataTable PopulateServiceQ(int QuoteId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = String.Format("SELECT B.ServiceName as Description,B.ServiceId,A.Amount From serQuoteTrans A inner join [" + BsfGlobal.g_sVendorDBName + "].dbo.ServiceMaster B on A.ServiceID=B.ServiceId Where A.RegisterID={0}", QuoteId);
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


        public static bool InsertSerOrderBill(DataTable dtComPList, DataTable argQTrans)
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
                    //sSql = String.Format("INSERT INTO FlatType(ProjId,Typename,Area,USLandArea,LandRate,LandAmount,Rate,BaseAmt,AdvAmount,OtherCostAmt,TotalCarPark,NetAmt,PayTypeId,Remarks) Values({0},'{1}',{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},'{13}')  SELECT SCOPE_IDENTITY(); ", OUnitDirBO.ProjId, OUnitDirBO.TypeName, OUnitDirBO.Area, OUnitDirBO.USLandArea, OUnitDirBO.LandRate, OUnitDirBO.LandAmount, OUnitDirBO.Rate, OUnitDirBO.BaseAmt, OUnitDirBO.AdvAmount, OUnitDirBO.OtherCostAmt, OUnitDirBO.TotalCarpark, OUnitDirBO.NetAmt, OUnitDirBO.PayTypeId, OUnitDirBO.Remarks);
                    sSql = String.Format("INSERT INTO SerOrderBillReg(Billdate,BillRefNo,CostcentreID,FlatID,GrossAmt,NetAmt,Narration,QuoteRegId,QualifierAmt) Values('{0}','{1}',{2},{3},'{4}','{5}','{6}',{7},'{8}') SELECT SCOPE_IDENTITY()", ServiceOrderBillBO.Billdate, ServiceOrderBillBO.BillRefNo, ServiceOrderBillBO.CostcentreID, ServiceOrderBillBO.FlatID, ServiceOrderBillBO.GrossAmt, ServiceOrderBillBO.NetAmt, ServiceOrderBillBO.Narration, ServiceOrderBillBO.QuoteRegId,ServiceOrderBillBO.QualifierAmt);
                    cmd = new SqlCommand(sSql, conn, tran);
                    iFRegId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();
                    if (dtComPList.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtComPList.Rows.Count; k++)
                        {
                            sSql = String.Format("INSERT INTO SerOrderBillTrans(BillRegId,ServiceID,Amount) values({0},{1},'{2}')", iFRegId, (dtComPList.Rows[k]["ServiceID"]), dtComPList.Rows[k]["Amount"]);
                            //CommFun.CRMExecute(sSql);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }

                    //Bill Qualifier
                    for (int u = 0; u < argQTrans.Rows.Count; u++)
                    {
                        sSql = "Insert Into ServiceOrderBillRateQ (BillRegId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,Amount,ExpValue,ExpPerValue,SurValue,EdValue) " +
                            "Values (" + iFRegId + "," + argQTrans.Rows[u]["QualifierId"].ToString() + ",'" + argQTrans.Rows[u]["Expression"].ToString() + "'," + argQTrans.Rows[u]["ExpPer"].ToString() + ",'" + argQTrans.Rows[u]["Add_Less_Flag"].ToString() + "'," + argQTrans.Rows[u]["SurCharge"].ToString() + "," + argQTrans.Rows[u]["EDCess"].ToString() + "," + argQTrans.Rows[u]["Amount"].ToString() + "," + argQTrans.Rows[u]["ExpValue"].ToString() + "," + argQTrans.Rows[u]["ExpPerValue"].ToString() + "," + argQTrans.Rows[u]["SurValue"].ToString() + "," + argQTrans.Rows[u]["EdValue"].ToString() + ")";
                        cmd = new SqlCommand(sSql, conn,tran);
                        cmd.ExecuteNonQuery();
                    }
                    tran.Commit();

                    bUpdate = true;
                    BsfGlobal.InsertLog(DateTime.Now, "CRM-ServiceBill-Add", "N", "CRM-ServiceBill", iFRegId, ServiceOrderBillBO.CostcentreID, 0, BsfGlobal.g_sCRMDBName, ServiceOrderBillBO.BillRefNo, BsfGlobal.g_lUserId);

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


        public static bool UpdateSerOrderBill(DataTable dtComPList, DataTable argQTrans)
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
                    sSql = String.Format("UPDATE SerOrderBillReg SET Billdate='{0}',BillRefNo='{1}',CostcentreID={2},FlatID={3},GrossAmt='{4}',NetAmt='{5}',Narration='{6}', QuoteRegId={7}, QualifierAmt='{8}' WHERE RegBillId={9}", ServiceOrderBillBO.Billdate, ServiceOrderBillBO.BillRefNo, ServiceOrderBillBO.CostcentreID, ServiceOrderBillBO.FlatID, ServiceOrderBillBO.GrossAmt, ServiceOrderBillBO.NetAmt, ServiceOrderBillBO.Narration, ServiceOrderBillBO.QuoteRegId, ServiceOrderBillBO.QualifierAmt, ServiceOrderBillBO.RegBillId);
                    
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                 
                    sSql = String.Format("DELETE FROM SerOrderBillTrans WHERE BillRegId={0}", ServiceOrderBillBO.RegBillId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = String.Format("DELETE FROM ServiceOrderBillRateQ WHERE BillRegId={0}", ServiceOrderBillBO.RegBillId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (dtComPList.Rows.Count > 0)
                    {
                        for (int a = 0; a < dtComPList.Rows.Count; a++)
                        {
                            sSql = String.Format("INSERT INTO SerOrderBillTrans(BillRegId,ServiceID,Amount) values({0},{1},'{2}')", ServiceOrderBillBO.RegBillId, (dtComPList.Rows[a]["ServiceID"]), dtComPList.Rows[a]["Amount"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    //Bill Qualifier
                    for (int u = 0; u < argQTrans.Rows.Count; u++)
                    {
                        sSql = "Insert Into ServiceOrderBillRateQ (BillRegId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,Amount,ExpValue,ExpPerValue,SurValue,EdValue) " +
                            "Values (" + ServiceOrderBillBO.RegBillId + "," + argQTrans.Rows[u]["QualifierId"].ToString() + ",'" + argQTrans.Rows[u]["Expression"].ToString() + "'," + argQTrans.Rows[u]["ExpPer"].ToString() + ",'" + argQTrans.Rows[u]["Add_Less_Flag"].ToString() + "'," + argQTrans.Rows[u]["SurCharge"].ToString() + "," + argQTrans.Rows[u]["EDCess"].ToString() + "," + argQTrans.Rows[u]["Amount"].ToString() + "," + argQTrans.Rows[u]["ExpValue"].ToString() + "," + argQTrans.Rows[u]["ExpPerValue"].ToString() + "," + argQTrans.Rows[u]["SurValue"].ToString() + "," + argQTrans.Rows[u]["EdValue"].ToString() + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                    }

                    tran.Commit();
                    bUpdate = true;
                    BsfGlobal.InsertLog(DateTime.Now, "CRM-ServiceBill-Edit", "E", "CRM-ServiceBill", ServiceOrderBillBO.RegBillId, ServiceOrderBillBO.CostcentreID, 0, BsfGlobal.g_sCRMDBName, ServiceOrderBillBO.BillRefNo, BsfGlobal.g_lUserId);


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

        public static DataTable Populate_SerOrderBillRegister(DateTime frmDate, DateTime toDate)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();

                string frmdat = string.Format("{0:dd MMM yyyy}", frmDate);
                string tdat = string.Format("{0:dd MMM yyyy}", toDate.AddDays(0));

                sSql = "SELECT A.RegBillId,A.Billdate,A.BillRefNo,A.CostcentreID,B.CostCentreName,S.RefNo QuoteNo,C.FlatNo,E.LeadName BuyerName,A.NetAmt,A.Approve FROM SerOrderBillReg A " +
                         "Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B on A.CostCentreId=B.CostCentreId " +
                         "Inner Join FlatDetails C on A.FlatId=C.FlatId " +
                         "Left Join ServiceQuoteReg S on A.QuoteRegId=S.RegisterId  " +
                         "Left Join LeadRegister E on S.LeadId=E.LeadId " +
                         "WHERE A.Billdate between '" + frmdat + "'  And '" + tdat + "' Order by A.Billdate,A.BillRefNo";

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

        public static DataTable Populate_SerOrderBillRegisterChange(int argRegId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
           
                sSql = "SELECT A.RegBillId,A.Billdate,A.BillRefNo,A.CostcentreID,B.CostCentreName,S.RefNo QuoteNo,C.FlatNo,E.LeadName BuyerName,A.NetAmt,A.Approve FROM SerOrderBillReg A " +
                         "Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B on A.CostCentreId=B.CostCentreId " +
                         "Inner Join FlatDetails C on A.FlatId=C.FlatId " +
                         "Left Join ServiceQuoteReg S on A.QuoteRegId=S.RegisterId  " +
                         "Left Join LeadRegister E on S.LeadId=E.LeadId " +
                         "WHERE A.RegBillId=" + argRegId + "";

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

        public static bool DeleteSerOrderBillRegister(int RegId, int argCostId, string argVouNo)
        {
            string sSql = "";
            bool bSuccess = false;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();

            try
            {
                sSql = String.Format("DELETE FROM SerOrderBillReg WHERE RegBillId={0}", RegId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = String.Format("DELETE FROM SerOrderBillTrans WHERE BillRegId={0}", RegId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = String.Format("DELETE FROM ServiceOrderBillRateQ WHERE BillRegId={0}", RegId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                BsfGlobal.InsertLog(DateTime.Now, "CRM-ServiceBill-Delete", "D", "CRM-ServiceBill", RegId, argCostId, 0, BsfGlobal.g_sCRMDBName, argVouNo, BsfGlobal.g_lUserId);

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


    }
}
