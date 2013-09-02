using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using CRM.BO;
using CRM.DataLayer;

namespace CRM.DL
{
    class FinalizationDL
    {
        #region Methods

        internal static DataTable GetFlatDetails(int argCCId, string argStatus)
        {
            String sSql = "";
            DataTable dt = null;
            SqlDataAdapter sda = null;

            try
            {
                if (argStatus == "S")
                {
                    sSql = String.Format("SELECT FlatTypeId,FlatId,FlatNo FROM dbo.FlatDetails where Status='S' AND CostCentreId={0}", argCCId);
                }
                else if (argStatus == "I")
                {
                    sSql = String.Format("SELECT FlatTypeId,FlatId,FlatNo FROM dbo.FlatDetails where Status='I' AND CostCentreId={0}", argCCId);
                }
                else if (argStatus == "B")
                {
                    sSql = String.Format("SELECT FlatTypeId,FlatId,FlatNo FROM dbo.FlatDetails where Status='B' AND CostCentreId={0}", argCCId);
                }

                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
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

        internal static DataTable EditFinalization(int argiFlatId, int argCCId)
        {
            String sSql = "";
            DataTable dt = null;
            SqlDataAdapter sda = null;

            try
            {
                sSql = String.Format("Select * From dbo.BuyerDetail A Where FlatId={0} AND CostCentreId={1} ", argiFlatId, argCCId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        internal static DataTable GetBrokerDetails(int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            string sSql = "";
            try
            {
                if (BsfGlobal.g_bWPMDB == true)
                {
                    sSql = "SELECT DISTINCT ISNULL(E.SORegisterId,0) SORegisterId,A.BrokerId,A.VendorId,B.VendorName BrokerName FROM BrokerDet A " +
                           " INNER JOIN dbo.BrokerCC F ON A.BrokerId=F.BrokerId " +
                           " INNER Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorMaster B On A.VendorId=B.VendorId " +
                           " LEFT Join [" + BsfGlobal.g_sWPMDBName + "].dbo.SORegister E On A.VendorId=E.ContractorId AND F.CostCentreId=E.CostCentreId " +
                           " LEFT Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorContact C On C.VendorID=B.VendorId " +
                           " LEFT Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CityMaster D On D.CityId=B.CityId" +
                           " ORDER BY VendorName";
                }
                else
                {
                    sSql = "Select 0 SORegisterId,0 BrokerId,0 VendorId,'' BrokerName ";
                }

                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        internal static DataTable GetBankDetails()
        {
            DataTable dt = null;
            SqlDataAdapter sda = null;
            string sSql = "";
            try
            {
                sSql = "select BranchId,Branch BranchName from dbo.BankDetails";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
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

        internal static void UpdateBuyerDet(string argMode, FinalizationBO BOFIN,DataTable dtFinal,string argFlatNo,bool argChkSend)
        {
            SqlConnection conn = new SqlConnection();
            String sSql = "";
            SqlCommand cmd = null;
            string ValidUpto = "",FinalDate="";
            conn=BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                if (argMode == "E")
                {
                    ValidUpto = string.Format("{0:dd/MMM/yyyy}", BOFIN.DE_ValidUpto);
                    FinalDate = string.Format("{0:dd/MMM/yyyy}", BOFIN.DE_Final);
                    if (ValidUpto != "01/Jan/0001")
                    {
                        sSql = "Update dbo.BuyerDetail Set Status='" + BOFIN.s_Status + "'," +
                            " CustomerType='" + BOFIN.s_CustomerType + "', PaymentOption='" + BOFIN.s_PaymentOption + "'," +
                            " BrokerId=" + BOFIN.i_BrokerId + ",BrokerComm=" + BOFIN.d_BrokerComm + ",BrokerAmount=" + BOFIN.d_BrokerAmt + "," +
                            " ValidUpto='" + ValidUpto + "',FinaliseDate='" + FinalDate + "',PostSaleExecId=" + BOFIN.i_PostExecId + ", " +
                            " Advance=" + BOFIN.d_AdvAmt + " Where FlatId=" + BOFIN.i_FlatId + " AND CostCentreId=" + BOFIN.i_CostCentreId + "";
                    }
                    else
                    {
                        ValidUpto = null;
                        sSql = "Update dbo.BuyerDetail Set Status='" + BOFIN.s_Status + "'," +
                            " CustomerType='" + BOFIN.s_CustomerType + "', PaymentOption='" + BOFIN.s_PaymentOption + "'," +
                            " BrokerId=" + BOFIN.i_BrokerId + ",BrokerComm=" + BOFIN.d_BrokerComm + ",BrokerAmount=" + BOFIN.d_BrokerAmt + "," +
                            " ValidUpto='" + ValidUpto + "',FinaliseDate='" + FinalDate + "',PostSaleExecId=" + BOFIN.i_PostExecId + ", " +
                            " Advance=" + BOFIN.d_AdvAmt + " Where FlatId=" + BOFIN.i_FlatId + " AND CostCentreId=" + BOFIN.i_CostCentreId + "";
                    }

                    cmd = new SqlCommand(sSql, conn,tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Update dbo.FollowUp Set ExecutiveId=" + BOFIN.i_PostExecId + "" +
                           " Where FlatId=" + BOFIN.i_FlatId + " AND CostCentreId=" + BOFIN.i_CostCentreId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Update dbo.PaymentScheduleFlat Set SchDate='" + FinalDate + "'" +
                           " Where TemplateId=0 And FlatId=" + BOFIN.i_FlatId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Update dbo.PaymentScheduleFlat Set SchDate='" + FinalDate + "'" +
                            " Where TemplateId=-1 And FlatId=" + BOFIN.i_FlatId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    SqlDataReader sdr, sdr1, sdr2; DataTable dt, dt1; int iStgId = 0, iTemplateId = 0;
                    int iDateAfter = 0, iDuration = 0; string sDurType = ""; DateTime SchDate; int iSortOrder = 0;
                    DateTime StartDate = DateTime.Now; DateTime EndDate = DateTime.Now; int ipre = 0;
                    sSql = "Select TemplateId,PreStageTypeId from dbo.PaymentScheduleFlat Where FlatId=" + BOFIN.i_FlatId + " And PreStageTypeId=-1";
                    cmd = new SqlCommand(sSql, conn, tran);
                    sdr = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(sdr); cmd.Dispose();

                    if (dt.Rows.Count > 0)
                    {
                        iStgId = Convert.ToInt32(dt.Rows[0]["PreStageTypeId"]);
                        iTemplateId = Convert.ToInt32(dt.Rows[0]["TemplateId"]);
                    }
                    dt.Dispose();

                    sSql = "Select SortOrder From dbo.PaymentScheduleFlat Where FlatId=" + BOFIN.i_FlatId + " And TemplateId=" + iTemplateId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    sdr2 = cmd.ExecuteReader();
                    dt1 = new DataTable();
                    dt1.Load(sdr2); cmd.Dispose();
                    dt1.Dispose();

                    if (dt1.Rows.Count > 0)
                    {
                        iSortOrder = Convert.ToInt32(dt1.Rows[0]["SortOrder"]);
                    }

                    sSql = "select StartDate,EndDate From ProjectInfo Where CostCentreId= " + BOFIN.i_CostCentreId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    dt = new DataTable();
                    sdr = cmd.ExecuteReader();
                    dt.Load(sdr);
                    dt.Dispose();

                    if (dt.Rows.Count > 0)
                    {
                        StartDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["StartDate"], CommFun.datatypes.VarTypeDate));
                        EndDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["EndDate"], CommFun.datatypes.VarTypeDate));
                    }

                    sSql = "Update dbo.PaymentScheduleFlat Set SchDate='" + FinalDate + "'" +
                        " Where TemplateId=" + iTemplateId + " And FlatId=" + BOFIN.i_FlatId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Update dbo.PaymentScheduleFlat Set SchDate='" + FinalDate + "'" +
                        " Where TemplateId=0 And FlatId=" + BOFIN.i_FlatId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (iStgId == -1)
                    {
                        if (iStgId == -1)
                            sSql = "Select A.PreStageTypeId,A.CostCentreId,A.TemplateId,A.DateAfter,A.Duration,A.Durationtype from dbo.PaymentScheduleFlat A" +
                            " Left Join dbo.ProgressBillRegister B On A.FlatId=B.FlatId " +
                            " Where A.FlatId=" + BOFIN.i_FlatId + " And A.SortOrder>=" + iSortOrder + "" +
                            " And A.PaymentSchId Not In " +
                            " (Select PaySchId From dbo.ProgressBillRegister Where FlatId=" + BOFIN.i_FlatId + ") Order By A.SortOrder";

                        cmd = new SqlCommand(sSql, conn, tran);
                        sdr1 = cmd.ExecuteReader();
                        dt = new DataTable();
                        dt.Load(sdr1);
                        cmd.Dispose();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            iTemplateId = Convert.ToInt32(dt.Rows[i]["TemplateId"]);
                            ipre = Convert.ToInt32(dt.Rows[i]["PreStageTypeId"]);
                            iDateAfter = Convert.ToInt32(dt.Rows[i]["DateAfter"]);
                            iDuration = Convert.ToInt32(dt.Rows[i]["Duration"]);
                            sDurType = dt.Rows[i]["DurationType"].ToString();

                            if (ipre == -1) { } else if (ipre == -2) { } else if (ipre == -3) { } else if (ipre == 0) { } else { iTemplateId = ipre; }

                            sSql = "Select SchDate From dbo.PaymentScheduleFlat Where CostCentreId=" + dt.Rows[i]["CostCentreId"] + " And FlatId=" + BOFIN.i_FlatId + "" +
                                  " And TemplateId=" + iTemplateId + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            DataTable dtDate = new DataTable();
                            sdr = cmd.ExecuteReader();
                            dtDate.Load(sdr);
                            dtDate.Dispose();

                            if (ipre == -1) { SchDate = Convert.ToDateTime(CommFun.IsNullCheck(FinalDate, CommFun.datatypes.VarTypeDate)); }
                            else if (ipre == -2) { SchDate = StartDate; }
                            else if (ipre == -3) { SchDate = EndDate; }
                            else
                                SchDate = Convert.ToDateTime(CommFun.IsNullCheck(dtDate.Rows[0]["SchDate"], CommFun.datatypes.VarTypeDate));

                            if (sDurType == "D")
                            { if (iDateAfter == 0) SchDate = SchDate.AddDays(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }
                            else if (sDurType == "M")
                            { if (iDateAfter == 0) SchDate = SchDate.AddMonths(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }


                            sSql = "Update dbo.PaymentScheduleFlat Set SchDate='" + string.Format(Convert.ToDateTime(SchDate).ToString("dd-MMM-yyyy")) + "'" +
                                " Where TemplateId=" + dt.Rows[i]["TemplateId"] + " And FlatId=" + BOFIN.i_FlatId + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                        }
                    }
                    UnitDirDL.InsertFlatChk(dtFinal, BOFIN.i_FlatId, "F", argChkSend, argFlatNo, BOFIN.i_CostCentreId, conn, tran);

                    sSql = "Select AdvAmount From dbo.FlatDetails Where FlatId=" + BOFIN.i_FlatId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dtA = new DataTable();
                    dtA.Load(dr);
                    cmd.Dispose();

                    if (dtA.Rows.Count > 0)
                    {
                        decimal dAdvAmt = Convert.ToDecimal(dtA.Rows[0]["AdvAmount"]);
                        if (dAdvAmt != Convert.ToDecimal(BOFIN.d_AdvAmt))
                        {
                            sSql = "Update FlatDetails Set AdvAmount=" + BOFIN.d_AdvAmt + " WHERE FlatId=" + BOFIN.i_FlatId + " ";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();

                            PaymentScheduleDL.InsertFlatScheduleI(BOFIN.i_FlatId, conn, tran);
                           
                            dtA.Dispose();
                            cmd.Dispose();
                        }
                    }
                }

                //PaymentScheduleDL.InsertFlatScheduleI(BOFIN.i_FlatId, conn, tran);
                //FlatDetailsDL.UpdateFlatQualAmt(BOFIN.i_, BOFIN.i_FlatId, conn, tran);

                tran.Commit();

                if (argMode == "E")
                {
                    BsfGlobal.InsertLog(DateTime.Now, "Flat-Finalisation-Modify", "N", "Flat Finalisation", BOFIN.i_FlatId, BOFIN.i_CostCentreId, 0, BsfGlobal.g_sCRMDBName, argFlatNo, BsfGlobal.g_lUserId, BOFIN.d_AdvAmt, 0);
                }
                else
                {
                    BsfGlobal.InsertLog(DateTime.Now, "Flat-Finalisation-Create", "C", "Flat Finalisation", BOFIN.i_FlatId, BOFIN.i_CostCentreId, 0, BsfGlobal.g_sCRMDBName, argFlatNo, BsfGlobal.g_lUserId, BOFIN.d_AdvAmt, 0);
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

        #endregion
    }
}
