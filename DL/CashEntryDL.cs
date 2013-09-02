using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace CRM.DataLayer
{
    class CashEntryDL
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

        public static DataTable GetBuyer(int argCCId)
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select Distinct A.LeadId,B.LeadName From BuyerDetail A Inner Join LeadRegister B on A.LeadId=B.LeadId " +
                    " Where A.CostCentreId=" + argCCId + "";
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

        public static DataTable GetPayInfo(int argBuyerId,string argType)
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                if (argType == "PS")
                {
                    sSql = "Select 0 PBillId,0 ProgRegId,A.PaymentSchId PaySchId,B.FlatId,SchDate,B.FlatNo UnitNo,B.Area,B.Rate,[Description],B.NetAmt,B.QualifierAmt,A.Amount,A.NetAmount,A.SurplusAmount,(A.NetAmount-A.SurplusAmount)Balance,Cast(0 as decimal(18,3)) CurrentAmount From dbo.PaymentScheduleFlat A " +
                            " Inner Join dbo.FlatDetails B On A.FlatId=B.FlatId " +
                            " Where BillPassed=0 And PaidAmount=0 And LeadId=" + argBuyerId + " And A.TemplateId<>0 Order By A.SchDate";
                }
                else
                {
                    sSql = "Select PR.PBillId,PR.ProgRegId,A.PaymentSchId PaySchId,B.FlatId,SchDate,B.FlatNo UnitNo,B.Area,B.Rate,[Description],B.NetAmt,B.QualifierAmt,A.Amount,A.NetAmount,A.SurplusAmount,(A.NetAmount-A.SurplusAmount -PR.PaidAmount)Balance,Cast(0 as decimal(18,3)) CurrentAmount From dbo.PaymentScheduleFlat A " +
                            " Inner Join dbo.ProgressBillRegister PR On PR.PaySchId=A.PaymentSchId And PR.BillAmount=PR.NetAmount " +
                            " Inner Join dbo.FlatDetails B On A.FlatId=B.FlatId " +
                            " Where B.LeadId=" + argBuyerId + " And PR.PaidAmount<>PR.NetAmount And A.TemplateId<>0 Order By A.SchDate";
                }
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

        public static void InsertCashDetails(DataTable dtPay, int argCCId, int argBuyerId, decimal argAmt, DateTime argDate,string argType)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            string sSql = ""; int iPaySchId = 0; int iFlatId = 0;
            decimal dNetAmt=0,dArea=0,dRate=0,dBaseAmt=0,dQualAmt=0;
            int iEntryId = 0; bool bAns = false;
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    if (dtPay != null)
                    {
                        sSql = "Insert Into dbo.CashReceiptRegister(CashDate,LeadId,CostCentreId,Amount)Values(" +
                                "'" + argDate.ToString("dd/MMM/yyyy") + "'," + argBuyerId + "," + argCCId + "," + argAmt + ")SELECT SCOPE_IDENTITY()";
                        cmd = new SqlCommand(sSql, conn, tran);
                        iEntryId = int.Parse(cmd.ExecuteScalar().ToString());
                        cmd.Dispose();

                        for (int i = 0; i < dtPay.Rows.Count; i++)
                        {
                            if (Convert.ToDecimal(dtPay.Rows[i]["CurrentAmount"]) > 0)
                            {
                                iPaySchId = Convert.ToInt32(dtPay.Rows[i]["PaySchId"]);
                                iFlatId = Convert.ToInt32(dtPay.Rows[i]["FlatId"]);
                                dArea= Convert.ToDecimal(dtPay.Rows[i]["Area"]);
                                dRate= Convert.ToDecimal(dtPay.Rows[i]["Rate"]);
                                dBaseAmt = dArea * dRate;
                                dNetAmt= Convert.ToDecimal(dtPay.Rows[i]["NetAmt"]);
                                dQualAmt = Convert.ToDecimal(dtPay.Rows[i]["QualifierAmt"]);

                                sSql = "Insert Into dbo.CashReceiptTrans(CashReceiptId,FlatId,PaySchId,PBillId,PaySchAmount,PaySchNetAmount,CashAmount,FlatRate,FlatAmount)Values(" + iEntryId + ", " +
                                " " + dtPay.Rows[i]["FlatId"] + "," + dtPay.Rows[i]["PaySchId"] + "," + dtPay.Rows[i]["PBillId"] + "," + dtPay.Rows[i]["Amount"] + "," + dtPay.Rows[i]["NetAmount"] + "," + dtPay.Rows[i]["CurrentAmount"] + "," + dRate + "," +
                                " " + dNetAmt + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                sSql = "Update PaymentScheduleFlat Set Amount=" + dtPay.Rows[i]["Amount"] + "-" + dtPay.Rows[i]["CurrentAmount"] + ",NetAmount=" + dtPay.Rows[i]["NetAmount"] + "-" + dtPay.Rows[i]["CurrentAmount"] + " Where PaymentSchId=" + iPaySchId + "";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                if (argType == "PB")
                                {
                                    sSql = "Update dbo.ProgressBillRegister Set BillAmount=" + dtPay.Rows[i]["Amount"] + "-" + dtPay.Rows[i]["CurrentAmount"] + ",NetAmount=" + dtPay.Rows[i]["NetAmount"] + "-" + dtPay.Rows[i]["CurrentAmount"] + " Where PaySchId=" + iPaySchId + " And PBillId=" + dtPay.Rows[i]["PBillId"] + "";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();

                                    sSql = "Update dbo.ProgressBillMaster Set NetAmount=NetAmount-" + dtPay.Rows[i]["CurrentAmount"] + " Where ProgRegId=" + dtPay.Rows[i]["ProgRegId"] + "";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }

                                decimal NetAmount = dNetAmt - argAmt;
                                decimal BaseAmount = dBaseAmt-argAmt;
                                decimal Rate = BaseAmount / dArea;

                                sSql = "Update dbo.FlatDetails Set Rate=" + Rate + ",BaseAmt=" + BaseAmount + ",NetAmt=" + NetAmount + " Where FlatId=" + iFlatId + "";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                sSql = "Select * From dbo.PaymentScheduleFlat Where FlatId=" + iFlatId + " And TemplateId<>0";
                                cmd = new SqlCommand(sSql, conn, tran);
                                SqlDataReader dr = cmd.ExecuteReader();
                                DataTable dt = new DataTable();
                                dt.Load(dr);
                                cmd.Dispose();

                                if (dt.Rows.Count > 0)
                                {
                                    for (int x = 0; x < dt.Rows.Count; x++)
                                    {
                                        decimal dSchPer = Convert.ToDecimal(dt.Rows[x]["Amount"]) / (NetAmount + dQualAmt) * 100;
                                        //decimal dSchPer = Convert.ToDecimal(dt.Rows[x]["Amount"]) / (NetAmount + dQualAmt) * 100;
                                        sSql = "Update dbo.PaymentScheduleFlat Set SchPercent=" + dSchPer + " Where PaymentSchId=" + dt.Rows[x]["PaymentSchId"] + "";
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                }

                            }

                            sSql = "Select PaymentSchId,FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate," +
                            " DateAfter, Duration,DurationType,SchPercent,Amount,PreStageTypeId,SortOrder,BillPassed,PaidAmount From dbo.PaymentScheduleFlat " +
                            " Where FlatId=" + iFlatId + " And TemplateId<>0 Order By SortOrder";
                            cmd = new SqlCommand(sSql, conn, tran);
                            SqlDataReader dr1 = cmd.ExecuteReader();
                            DataTable dtF = new DataTable();
                            dtF.Load(dr1);
                            cmd.Dispose();

                            if (dtF.Rows.Count > 0)
                            {
                                PaymentScheduleDL.UpdateCashBuyerScheduleQual(iFlatId, dtF, conn, tran);
                            }

                        }
                    }

                    bAns = true;

                    if (bAns == true)
                    {
                        BsfGlobal.InsertLog(DateTime.Now, "Buyer-Receipt-Cash-Add", "N", "", iEntryId, argCCId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId, argAmt, 0);
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

        public static void UpdateCashDetails(int argCashRepId, DataTable dtPay, int argCCId, int argBuyerId, decimal argAmt, DateTime argDate,string argType)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            string sSql = ""; int iPaySchId = 0; int iFlatId = 0;
            decimal dNetAmt = 0, dArea = 0, dRate = 0, dBaseAmt = 0, dQualAmt = 0;
            int iEntryId = 0; bool bAns = false;
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    if (dtPay != null)
                    {
                        sSql = "Delete From dbo.CashReceiptRegister Where CashReceiptId=" + argCashRepId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Delete From dbo.CashReceiptTrans Where CashReceiptId=" + argCashRepId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Select FlatId From dbo.FlatDetails Where CostCentreId=" + argCCId + " And LeadId=" + argBuyerId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        DataTable dtFlat = new DataTable();
                        dtFlat.Load(sdr);
                        cmd.Dispose();

                        for (int n = 0; n < dtFlat.Rows.Count; n++)
                        {
                            DataTable dt = new DataTable();
                            dt = dtPay;
                            DataView dv = new DataView(dt);
                            dv.RowFilter = "FlatId=" + dtFlat.Rows[n]["FlatId"] + "";

                            dt = dv.ToTable();

                            sSql = "Insert Into dbo.CashReceiptRegister(CashDate,LeadId,CostCentreId,Amount)Values(" +
                                    "'" + argDate.ToString("dd/MMM/yyyy") + "'," + argBuyerId + "," + argCCId + "," + argAmt + ")SELECT SCOPE_IDENTITY()";
                            cmd = new SqlCommand(sSql, conn, tran);
                            iEntryId = int.Parse(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                //if (Convert.ToDecimal(dt.Rows[i]["CurrentAmount"]) > 0)
                                //{
                                iPaySchId = Convert.ToInt32(dt.Rows[i]["PaySchId"]);
                                iFlatId = Convert.ToInt32(dt.Rows[i]["FlatId"]);
                                dArea = Convert.ToDecimal(dt.Rows[i]["Area"]);
                                dRate = Convert.ToDecimal(dt.Rows[0]["Rate"]);
                                dBaseAmt = dArea * dRate;
                                dNetAmt = Convert.ToDecimal(dt.Rows[0]["NetAmt"]);
                                dQualAmt = Convert.ToDecimal(dt.Rows[i]["QualifierAmt"]);


                                sSql = "Insert Into dbo.CashReceiptTrans(CashReceiptId,FlatId,PaySchId,PBillId,PaySchAmount,PaySchNetAmount,CashAmount,FlatRate,FlatAmount)Values(" + iEntryId + ", " +
                                " " + iFlatId + "," + dt.Rows[i]["PaySchId"] + "," + dt.Rows[i]["PBillId"] + "," + dt.Rows[i]["Amount"] + "," + dt.Rows[i]["NetAmount"] + "," + dt.Rows[i]["CurrentAmount"] + "," + dRate + "," +
                                " " + dNetAmt + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                sSql = "Update PaymentScheduleFlat Set Amount=" + dt.Rows[i]["Amount"] + "-" + dt.Rows[i]["CurrentAmount"] + ",NetAmount=" + dt.Rows[i]["NetAmount"] + "-" + dt.Rows[i]["CurrentAmount"] + " Where PaymentSchId=" + iPaySchId + "";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                if (argType == "PB")
                                {
                                    sSql = "Update dbo.ProgressBillRegister Set BillAmount=" + dt.Rows[i]["Amount"] + "-" + dt.Rows[i]["CurrentAmount"] + ",NetAmount=" + dt.Rows[i]["NetAmount"] + "-" + dt.Rows[i]["CurrentAmount"] + " Where PaySchId=" + iPaySchId + " And PBillId=" + dt.Rows[i]["PBillId"] + "";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();

                                    sSql = "Update dbo.ProgressBillMaster Set NetAmount=NetAmount-" + dt.Rows[i]["CurrentAmount"] + " Where ProgRegId=" + dt.Rows[i]["ProgRegId"] + "";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                                //}
                            }

                            decimal NetAmount = dNetAmt - argAmt;
                            decimal BaseAmount = dBaseAmt - argAmt;
                            decimal Rate = BaseAmount / dArea;

                            sSql = "Update dbo.FlatDetails Set Rate=" + Rate + ",BaseAmt=" + BaseAmount + ",NetAmt=" + NetAmount + " Where FlatId=" + iFlatId + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            sSql = "Select * From dbo.PaymentScheduleFlat Where FlatId=" + iFlatId + " And TemplateId<>0";
                            cmd = new SqlCommand(sSql, conn, tran);
                            SqlDataReader dr = cmd.ExecuteReader();
                            DataTable dtPS = new DataTable();
                            dtPS.Load(dr);
                            cmd.Dispose();

                            if (dtPS.Rows.Count > 0)
                            {
                                for (int x = 0; x < dtPS.Rows.Count; x++)
                                {
                                    decimal dSchPer = decimal.Round(Convert.ToDecimal(dtPS.Rows[x]["Amount"]) / (NetAmount + dQualAmt) * 100, 6);
                                    sSql = "Update dbo.PaymentScheduleFlat Set SchPercent=" + dSchPer + " Where PaymentSchId=" + dtPS.Rows[x]["PaymentSchId"] + "";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                            }

                        }

                        sSql = "Select PaymentSchId,FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate," +
                        " DateAfter, Duration,DurationType,SchPercent,Amount,PreStageTypeId,SortOrder,BillPassed,PaidAmount From dbo.PaymentScheduleFlat " +
                        " Where FlatId=" + iFlatId + " And TemplateId<>0 Order By SortOrder";
                        cmd = new SqlCommand(sSql, conn, tran);
                        SqlDataReader dr1 = cmd.ExecuteReader();
                        DataTable dtF = new DataTable();
                        dtF.Load(dr1);
                        cmd.Dispose();

                        if (dtF.Rows.Count > 0)
                        {
                            PaymentScheduleDL.UpdateCashBuyerScheduleQual(iFlatId, dtF, conn, tran);
                        }

                    }
                    //    }
                    //}
                    tran.Commit();

                    bAns = true;
                    if (bAns == true)
                    {
                        BsfGlobal.InsertLog(DateTime.Now, "Buyer-Receipt-Cash-Edit", "E", "", iEntryId, argCCId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId, argAmt, 0);
                    }
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

        public static void InsertDetails(DataTable dtPay, int argCCId, int argBuyerId, decimal argAmt, DateTime argDate)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            string sSql = ""; int iPaySchId = 0; int iFlatId = 0;
            decimal dNetAmt = 0, dArea = 0, dRate = 0, dBaseAmt = 0, dQualAmt = 0;
            int iEntryId = 0;
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    if (dtPay != null)
                    {
                        sSql = "Insert Into dbo.CashReceiptRegister(CashDate,LeadId,CostCentreId,Amount)Values(" +
                                "'" + argDate.ToString("dd/MMM/yyyy") + "'," + argBuyerId + "," + argCCId + "," + argAmt + ")SELECT SCOPE_IDENTITY()";
                        cmd = new SqlCommand(sSql, conn, tran);
                        iEntryId = int.Parse(cmd.ExecuteScalar().ToString());
                        cmd.Dispose();

                        for (int i = 0; i < dtPay.Rows.Count; i++)
                        {
                            sSql = "Insert Into dbo.CashReceiptTrans(CashReceiptId,FlatId,PaySchId,PaySchAmount,PaySchNetAmount,CashAmount,FlatRate,FlatAmount)Values(" + iEntryId + ", " +
                                " " + dtPay.Rows[i]["FlatId"] + "," + dtPay.Rows[i]["PaySchId"] + "," + dtPay.Rows[i]["Amount"] + "," + dtPay.Rows[i]["NetAmount"] + "," + dtPay.Rows[i]["CurrentAmount"] + "," + dtPay.Rows[i]["Rate"] + "," +
                                " " + dtPay.Rows[i]["NetAmt"] + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }

                        for (int i = 0; i < dtPay.Rows.Count; i++)
                        {
                            if (Convert.ToDecimal(dtPay.Rows[i]["CurrentAmount"]) > 0)
                            {
                                iPaySchId = Convert.ToInt32(dtPay.Rows[i]["PaySchId"]);
                                iFlatId = Convert.ToInt32(dtPay.Rows[i]["FlatId"]);
                                dArea = Convert.ToDecimal(dtPay.Rows[i]["Area"]);
                                dRate = Convert.ToDecimal(dtPay.Rows[i]["Rate"]);
                                dBaseAmt = dArea * dRate;
                                dNetAmt = Convert.ToDecimal(dtPay.Rows[i]["NetAmt"]);
                                dQualAmt = Convert.ToDecimal(dtPay.Rows[i]["QualifierAmt"]);

                                //sSql = "Insert Into dbo.CashReceiptTrans(CashReceiptId,FlatId,PaySchId,PaySchAmount,PaySchNetAmount,CashAmount,FlatRate,FlatAmount)Values(" + iEntryId + ", " +
                                //" " + dtPay.Rows[i]["FlatId"] + "," + dtPay.Rows[i]["PaySchId"] + "," + dtPay.Rows[i]["Amount"] + "," + dtPay.Rows[i]["NetAmount"] + "," + dtPay.Rows[i]["CurrentAmount"] + "," + dRate + "," +
                                //" " + dNetAmt + ")";
                                //cmd = new SqlCommand(sSql, conn, tran);
                                //cmd.ExecuteNonQuery();
                                //cmd.Dispose();

                                //sSql = "Update PaymentScheduleFlat Set Amount=" + dtPay.Rows[i]["Amount"] + "-" + dtPay.Rows[i]["CurrentAmount"] + ",NetAmount=" + dtPay.Rows[i]["NetAmount"] + "-" + dtPay.Rows[i]["CurrentAmount"] + " Where PaymentSchId=" + iPaySchId + "";
                                //cmd = new SqlCommand(sSql, conn, tran);
                                //cmd.ExecuteNonQuery();
                                //cmd.Dispose();

                                decimal NetAmount = dNetAmt - argAmt;
                                decimal BaseAmount = dBaseAmt - argAmt;
                                decimal Rate = dRate - decimal.Round(dRate * (argAmt / dNetAmt), 3);

                                sSql = "Update dbo.FlatDetails Set Rate=" + Rate + ",BaseAmt=" + BaseAmount + ",NetAmt=" + NetAmount + " Where FlatId=" + iFlatId + "";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                sSql = "Select * From dbo.PaymentScheduleFlat Where FlatId=" + iFlatId + " And TemplateId<>0";
                                cmd = new SqlCommand(sSql, conn, tran);
                                SqlDataReader dr = cmd.ExecuteReader();
                                DataTable dt = new DataTable();
                                dt.Load(dr);
                                cmd.Dispose();

                                if (dt.Rows.Count > 0)
                                {
                                    for (int x = 0; x < dt.Rows.Count; x++)
                                    {
                                        decimal dAmount = decimal.Round((NetAmount + dQualAmt) * 100 / Convert.ToDecimal(dt.Rows[x]["SchPercent"]), 3);
                                        sSql = "Update dbo.PaymentScheduleFlat Set Amount=" + dAmount + " Where PaymentSchId=" + dt.Rows[x]["PaymentSchId"] + "";
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                }

                            }

                            sSql = "Select PaymentSchId,FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate," +
                            " DateAfter, Duration,DurationType,SchPercent,Amount,PreStageTypeId,SortOrder,BillPassed,PaidAmount From dbo.PaymentScheduleFlat " +
                            " Where FlatId=" + iFlatId + " And TemplateId<>0 Order By SortOrder";
                            cmd = new SqlCommand(sSql, conn, tran);
                            SqlDataReader dr1 = cmd.ExecuteReader();
                            DataTable dtF = new DataTable();
                            dtF.Load(dr1);
                            cmd.Dispose();

                            if (dtF.Rows.Count > 0)
                            {
                                PaymentScheduleDL.UpdateCashBuyerScheduleQual(iFlatId, dtF, conn, tran);
                            }

                        }
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

        public static void UpdateDetails(int argCashRepId, DataTable dtPay, int argCCId, int argBuyerId, decimal argAmt, DateTime argDate)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            string sSql = ""; int iPaySchId = 0; int iFlatId = 0;
            decimal dNetAmt = 0, dArea = 0, dRate = 0, dBaseAmt = 0, dQualAmt = 0;
            int iEntryId = 0;
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    if (dtPay != null)
                    {
                        sSql = "Delete From dbo.CashReceiptRegister Where CashReceiptId=" + argCashRepId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Delete From dbo.CashReceiptTrans Where CashReceiptId=" + argCashRepId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Select FlatId From dbo.FlatDetails Where CostCentreId=" + argCCId + " And LeadId=" + argBuyerId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        DataTable dtFlat = new DataTable();
                        dtFlat.Load(sdr);
                        cmd.Dispose();

                        for (int n = 0; n < dtFlat.Rows.Count; n++)
                        {
                            DataTable dt = new DataTable();
                            dt = dtPay;
                            DataView dv = new DataView(dt);
                            dv.RowFilter = "FlatId=" + dtFlat.Rows[n]["FlatId"] + "";

                            dt = dv.ToTable();

                            sSql = "Insert Into dbo.CashReceiptRegister(CashDate,LeadId,CostCentreId,Amount)Values(" +
                                    "'" + argDate.ToString("dd/MMM/yyyy") + "'," + argBuyerId + "," + argCCId + "," + argAmt + ")SELECT SCOPE_IDENTITY()";
                            cmd = new SqlCommand(sSql, conn, tran);
                            iEntryId = int.Parse(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();


                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                iPaySchId = Convert.ToInt32(dt.Rows[i]["PaySchId"]);
                                iFlatId = Convert.ToInt32(dt.Rows[i]["FlatId"]);
                                dArea = Convert.ToDecimal(dt.Rows[i]["Area"]);
                                dRate = Convert.ToDecimal(dt.Rows[0]["Rate"]);
                                dBaseAmt = dArea * dRate;
                                dNetAmt = Convert.ToDecimal(dt.Rows[0]["NetAmt"]);
                                dQualAmt = Convert.ToDecimal(dt.Rows[i]["QualifierAmt"]);


                                sSql = "Insert Into dbo.CashReceiptTrans(CashReceiptId,FlatId,PaySchId,PaySchAmount,PaySchNetAmount,CashAmount,FlatRate,FlatAmount)Values(" + iEntryId + ", " +
                                " " + iFlatId + "," + dt.Rows[i]["PaySchId"] + "," + dt.Rows[i]["Amount"] + "," + dt.Rows[i]["NetAmount"] + "," + dt.Rows[i]["CurrentAmount"] + "," + dRate + "," +
                                " " + dNetAmt + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }

                            decimal NetAmount = dNetAmt - argAmt;
                            decimal BaseAmount = dBaseAmt - argAmt;
                            decimal Rate = dRate- decimal.Round(dRate * (argAmt / dNetAmt), 3);

                            sSql = "Update dbo.FlatDetails Set Rate=" + Rate + ",BaseAmt=" + BaseAmount + ",NetAmt=" + NetAmount + " Where FlatId=" + iFlatId + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            sSql = "Select * From dbo.PaymentScheduleFlat Where FlatId=" + iFlatId + " And TemplateId<>0";
                            cmd = new SqlCommand(sSql, conn, tran);
                            SqlDataReader dr = cmd.ExecuteReader();
                            DataTable dtPS = new DataTable();
                            dtPS.Load(dr);
                            cmd.Dispose();

                            if (dtPS.Rows.Count > 0)
                            {
                                for (int x = 0; x < dtPS.Rows.Count; x++)
                                {
                                    decimal dAmount = decimal.Round((NetAmount + dQualAmt) * 100 / Convert.ToDecimal(dtPS.Rows[x]["SchPercent"]), 3);
                                    sSql = "Update dbo.PaymentScheduleFlat Set Amount=" + dAmount + " Where PaymentSchId=" + dtPS.Rows[x]["PaymentSchId"] + "";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                            }

                        }

                        sSql = "Select PaymentSchId,FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate," +
                        " DateAfter, Duration,DurationType,SchPercent,Amount,PreStageTypeId,SortOrder,BillPassed,PaidAmount From dbo.PaymentScheduleFlat " +
                        " Where FlatId=" + iFlatId + " And TemplateId<>0 Order By SortOrder";
                        cmd = new SqlCommand(sSql, conn, tran);
                        SqlDataReader dr1 = cmd.ExecuteReader();
                        DataTable dtF = new DataTable();
                        dtF.Load(dr1);
                        cmd.Dispose();

                        if (dtF.Rows.Count > 0)
                        {
                            PaymentScheduleDL.UpdateCashBuyerScheduleQual(iFlatId, dtF, conn, tran);
                        }

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


        #region Register

        public static DataTable GetPayInfoRegister(DateTime argFrom,DateTime argTo)
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                //sSql = "Select C.CashReceiptId,A.PaymentSchId PaySchId,B.FlatId,CashDate,D.CostCentreName,B.FlatNo UnitNo,L.LeadName,[Description], "+
                //        " CR.Amount From dbo.PaymentScheduleFlat A   "+
                //        " Inner Join dbo.FlatDetails B On A.FlatId=B.FlatId Inner Join dbo.CashReceiptTrans C On C.PaySchId=A.PaymentSchId  "+
                //        " Inner Join dbo.CashReceiptRegister CR On CR.CashReceiptId=C.CashReceiptId "+
                //        " Inner Join dbo.LeadRegister L On L.LeadId=B.LeadId"+
                //        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre D On D.CostCentreId=B.CostCentreId  Where BillPassed=0 And PaidAmount=0 And " +
                //        " CashDate Between '" + argFrom.ToString("dd/MMM/yyyy") + "' And '" + argTo.ToString("dd/MMM/yyyy") + "' And A.TemplateId<>0 Order By A.SchDate";
                sSql = "Select Distinct C.PBillId,C.CashReceiptId,B.FlatId,CashDate,D.CostCentreName,B.FlatNo UnitNo,L.LeadName, " +
                       " CR.Amount,Case When C.PBillId=0 Then 'PS' else 'PB' End Type From dbo.PaymentScheduleFlat A   " +
                       " Inner Join dbo.FlatDetails B On A.FlatId=B.FlatId Inner Join dbo.CashReceiptTrans C On C.PaySchId=A.PaymentSchId  " +
                       " Inner Join dbo.CashReceiptRegister CR On CR.CashReceiptId=C.CashReceiptId " +
                       " Inner Join dbo.LeadRegister L On L.LeadId=B.LeadId" +
                       " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre D On D.CostCentreId=B.CostCentreId  Where "+//BillPassed=0 And PaidAmount=0 And " +
                       " CashDate Between '" + argFrom.ToString("dd/MMM/yyyy") + "' And '" + argTo.ToString("dd/MMM/yyyy") + "' And A.TemplateId<>0 ";
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

        public static DataTable GetPayInfoRegEntry(int argCashRecpId,int argLeadId,string argType)
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                if (argType == "PS")
                {
                    sSql = "Select 0 PBillId,0 ProgRegId,CashReceiptId,PaySchId,LeadId,FlatId,CashDate,CashAmount,SchDate,CostCentreId, " +
                            " CostCentreName,UnitNo,Area,Rate,[Description],NetAmt,QualifierAmt,Amount,NetAmount,SurplusAmount,(NetAmount-SurplusAmount)Balance, " +
                            " CurrentAmount From( " +
                            " Select C.CashReceiptId,A.PaymentSchId PaySchId,L.LeadId, B.FlatId,CR.CashDate,CR.Amount CashAmount,SchDate,D.CostCentreId, " +
                            " D.CostCentreName,B.FlatNo UnitNo,B.Area,C.FlatRate Rate,[Description],C.FlatAmount NetAmt,B.QualifierAmt,C.PaySchAmount Amount,"+
                            " C.PaySchNetAmount NetAmount,A.SurplusAmount, " +
                            " CashAmount CurrentAmount From dbo.PaymentScheduleFlat A Inner Join dbo.FlatDetails B On A.FlatId=B.FlatId   " +
                            " Inner Join dbo.CashReceiptTrans C On C.PaySchId=A.PaymentSchId " +
                            " Inner Join dbo.CashReceiptRegister CR On CR.CashReceiptId=C.CashReceiptId  " +
                            " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre D On D.CostCentreId=B.CostCentreId " +
                            " Inner Join dbo.LeadRegister L On L.LeadId=B.LeadId Where "+//BillPassed=0 And PaidAmount=0 And "+
                            " A.TemplateId<>0 And C.CashReceiptId=" + argCashRecpId + " " +
                            " Union All " +
                            " Select 0 CashReceiptId,A.PaymentSchId PaySchId,L.LeadId, B.FlatId,'',0 CashAmount,SchDate,D.CostCentreId, " +
                            " D.CostCentreName,B.FlatNo UnitNo,B.Area,0 Rate,[Description],0 NetAmt,B.QualifierAmt, A.Amount,A.NetAmount, " +
                            " A.SurplusAmount,0 CurrentAmount From dbo.PaymentScheduleFlat A Inner Join dbo.FlatDetails B On A.FlatId=B.FlatId   " +
                            " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre D On D.CostCentreId=B.CostCentreId " +
                            " Inner Join dbo.LeadRegister L On L.LeadId=B.LeadId Where "+//A.BillPassed=0 And A.PaidAmount=0 And "+
                            " A.TemplateId<>0 And B.LeadId=" + argLeadId + " " +
                            " And A.PaymentSchId Not In (Select PaySchId From dbo.CashReceiptTrans Where CashReceiptId=" + argCashRecpId + ") " +
                            " )A ";
                }
                else 
                {
                    sSql = "Select PBillId,ProgRegId,CashReceiptId,PaySchId,LeadId,FlatId,CashDate,CashAmount,SchDate,CostCentreId, " +
                            " CostCentreName,UnitNo,Area,Rate,[Description],NetAmt,QualifierAmt,Amount,NetAmount,SurplusAmount,(NetAmount-SurplusAmount)Balance, " +
                            " CurrentAmount From( " +
                            " Select C.PBillId,PR.ProgRegId,C.CashReceiptId,A.PaymentSchId PaySchId,L.LeadId, B.FlatId,CR.CashDate,CR.Amount CashAmount,SchDate,D.CostCentreId, " +
                            " D.CostCentreName,B.FlatNo UnitNo,B.Area,C.FlatRate Rate,[Description],C.FlatAmount NetAmt,B.QualifierAmt,C.PaySchAmount Amount,"+
                            " C.PaySchNetAmount NetAmount,A.SurplusAmount, " +
                            " CashAmount CurrentAmount From dbo.PaymentScheduleFlat A Inner Join dbo.FlatDetails B On A.FlatId=B.FlatId   " +
                            " Inner Join dbo.CashReceiptTrans C On C.PaySchId=A.PaymentSchId " +
                            " Inner Join dbo.CashReceiptRegister CR On CR.CashReceiptId=C.CashReceiptId  " +
                            " Inner Join dbo.ProgressBillRegister PR On PR.PaySchId=A.PaymentSchId "+
                            " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre D On D.CostCentreId=B.CostCentreId " +
                            " Inner Join dbo.LeadRegister L On L.LeadId=B.LeadId Where "+//BillPassed=0 And PaidAmount=0 And "+
                            " A.TemplateId<>0 And C.CashReceiptId=" + argCashRecpId + " " +
                            //" Union All " +
                            //" Select PR.PBillId,0 CashReceiptId,A.PaymentSchId PaySchId,L.LeadId, B.FlatId,'',0 CashAmount,SchDate,D.CostCentreId, " +
                            //" D.CostCentreName,B.FlatNo UnitNo,B.Area,0 Rate,[Description],0 NetAmt,B.QualifierAmt, A.Amount,A.NetAmount, " +
                            //" 0 CurrentAmount From dbo.PaymentScheduleFlat A Inner Join dbo.FlatDetails B On A.FlatId=B.FlatId   " +
                            //" Inner Join dbo.ProgressBillRegister PR On PR.PaySchId=A.PaymentSchId "+
                            //" Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre D On D.CostCentreId=B.CostCentreId " +
                            //" Inner Join dbo.LeadRegister L On L.LeadId=B.LeadId Where "+//A.BillPassed=0 And A.PaidAmount=0 And "+
                            //" A.TemplateId<>0 And B.LeadId=" + argLeadId + " " +
                            //" And A.PaymentSchId Not In (Select PaySchId From dbo.CashReceiptTrans Where CashReceiptId=" + argCashRecpId + ") " +
                            " )A ";
                }
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

        public static DataTable GetChangeGridCashReceiptRegister(int argCashRecpId)
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select Amount From dbo.CashReceiptRegister Where CashReceiptId=" + argCashRecpId + "";
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

        #endregion

    }
}
