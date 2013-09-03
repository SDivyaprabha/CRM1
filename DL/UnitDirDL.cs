using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessLayer;
using CRM.BusinessObjects;
using Qualifier;
using Microsoft.VisualBasic;
using System.Windows.Forms;

namespace CRM.DataLayer
{
    class UnitDirDL
    {
        #region Methods

        public static DataTable Concep(string argPDB)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenRateAnalDB();

            try
            {
                sSql = String.Format("Select ConceptionId From dbo.ConceptionRegister Where ProjectName='{0}'", argPDB);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_RateAnalDB);
                dt = new DataTable();
                dt.Rows.Clear();
                dt.Columns.Clear();
                sda.Fill(dt);
                BsfGlobal.g_RateAnalDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable GetCC()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Select CostCentreId,CostCentreName,ProjectDB From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                        " Where ProjectDB IN(Select ProjectName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType='B')" +
                        " And CostCentreId NOT IN(Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ")" +
                        " Order By CostCentreName";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        public static void UpdateDefaultDatas()
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "Update dbo.ReceiptShTrans Set PaidGrossAmount=PaidNetAmount Where PaidNetAmount<>0 And ReceiptTypeId=1";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Update dbo.ReceiptTrans Set NetAmount=Isnull((Select IsNull(NetAmount,0) From dbo.PaymentScheduleFlat Where PaymentSchId=ReceiptTrans.PaySchId),0) Where NetAmount=0";
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

        public static DataTable GuideLine(int argCCID)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = String.Format("Select LandArea,FSIIndex,BuildArea,GuideLineValue,LandCost,StartDate,EndDate,Registration,NetLandArea,WithHeld "+
                    " From dbo.ProjectInfo Where CostCentreId={0}", argCCID);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static bool FoundCallType(int argId,string argDes,string argType)
        {
            DataTable dt;
            SqlDataAdapter sda;
            String sSql="";
            BsfGlobal.OpenCRMDB();
            bool bAns = false;

            try
            {
                if(argType=="C")
                    sSql = "Select Description From dbo.CallType Where Description='" + argDes + "' ";
                else if(argType=="S")
                    sSql = "Select Description From dbo.StatusMaster Where Description='" + argDes + "' ";
                else if (argType == "N")
                    sSql = "Select Description From dbo.NatureMaster Where Description='" + argDes + "' ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    bAns = true;
                }

                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return bAns;
        }

        public static string FoundCancelDate(int argId)
        {
            DataTable dt;
            SqlDataAdapter sda;
            String sSql = "";
            BsfGlobal.OpenCRMDB();
            string date="";

            try
            {
                sSql = "Select FinaliseDate From dbo.BuyerDetail Where FlatId='" + argId + "' ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    date = Convert.ToDateTime(dt.Rows[0]["FinaliseDate"]).ToString("dd-MMM-yyyy");
                }
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return date;
        }

        public static DataTable UOM()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = "Select Unit_ID,Unit_Name from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.UOM ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static int FoundUOM(int argCCId)
        {
            DataTable dt;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            int iUnitId = 0;

            try
            {
                sSql = "Select UnitId From dbo.ProjectInfo Where CostCentreId=" + argCCId + " ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    iUnitId = Convert.ToInt32(dt.Rows[0]["UnitId"]);
                }
                
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return iUnitId;
        }

        public static bool FoundStatus(int argFlatId)
        {
            DataTable dt;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            bool bAns=false;

            try
            {
                sSql = "Select Status From dbo.FlatDetails Where FlatId=" + argFlatId + " ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    string sStatus =CommFun.IsNullCheck( dt.Rows[0]["Status"], CommFun.datatypes.vartypestring).ToString();
                    if (sStatus == "S") bAns = true;
                }

                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return bAns;
        }

        public static decimal GetNetAmt(int argFlatId)
        {
            DataTable dt;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            decimal iNetAmt = 0;

            try
            {
                sSql = "Select SUM(NetAmount) As NetAmount From dbo.PaymentScheduleFlat Where FlatId=" + argFlatId + " ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    iNetAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["NetAmount"], CommFun.datatypes.vartypenumeric));
                }

                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return iNetAmt;
        }

        public static DataTable GetQualifier(int argFlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = "Select C.QualifierName,A.Add_Less_flag Sign,Sum(A.Amount)Amount From dbo.FlatReceiptQualifier A  " +
                       "Inner Join dbo.FlatReceiptType B On A.SchId=B.SchId  " +
                       "Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On A.QualifierId=C.QualifierId  " +
                       "Where FlatId=" + argFlatId + " AND QualType='B' Group By C.QualifierName,A.Add_Less_flag ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);

                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static decimal GetQualifierAmt(int argFlatId,bool argType)
        {
            DataTable dt;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            decimal dQualAmt = 0; decimal dTotal = 0;
            try
            {
                if(argType==true)
                sSql = "Select Sum(A.Amount)Amount From dbo.FlatReceiptQualifier A  " +
                       "Inner Join dbo.FlatReceiptType B On A.SchId=B.SchId  " +
                       "Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On A.QualifierId=C.QualifierId  " +
                       "Where FlatId=" + argFlatId + " AND QualType='B' Group By C.QualifierName,A.Add_Less_flag ";
                else
                    sSql = "Select SUM(Amount)Amount From dbo.PaySchTaxFlat Where FlatId=" + argFlatId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0) 
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dTotal = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["Amount"], CommFun.datatypes.vartypenumeric));
                        dQualAmt = dQualAmt + dTotal;
                    }
                }

                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dQualAmt;
        }

        public static decimal GetGlobalQualifierAmt(int argFlatId, bool argType,SqlConnection conn,SqlTransaction tran)
        {
            DataTable dt;
            SqlDataReader sdr;
            String sSql;
            decimal dQualAmt = 0; decimal dTotal = 0;
            try
            {
                if (argType == true)
                    sSql = "Select Sum(A.Amount)Amount From dbo.FlatReceiptQualifier A  " +
                           "Inner Join dbo.FlatReceiptType B On A.SchId=B.SchId  " +
                           "Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On A.QualifierId=C.QualifierId  " +
                           "Where FlatId=" + argFlatId + " AND QualType='B' Group By C.QualifierName,A.Add_Less_flag ";
                else
                    sSql = "Select SUM(Amount)Amount From dbo.PaySchTaxFlat Where FlatId=" + argFlatId + "";
                SqlCommand cmd = new SqlCommand(sSql, conn,tran);
                sdr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(sdr);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dTotal = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["Amount"], CommFun.datatypes.vartypenumeric));
                        dQualAmt = dQualAmt + dTotal;
                    }
                }

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dQualAmt;
        }

        public static decimal GetQualiAmt(int argFlatId,bool argType)
        {
            DataTable dt;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            decimal iQualAmt = 0;

            try
            {
                if (argType == false)
                    sSql = "Select SUM(Amount)Amount From dbo.PaySchTaxFlat Where FlatId=" + argFlatId + "";
                else
                    sSql = "Select  Sum(Case When Add_Less_Flag = '-' Then Amount*-1 Else Amount End) Amount from dbo.FlatReceiptQualifier  " +
                           "Where SchId in (Select SchId from dbo.FlatReceiptType Where FlatId=" + argFlatId + ")";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    iQualAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
                }

                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return iQualAmt;
        }

        public static void UpdateUOM(int argCCId,int argUnitId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "Update dbo.ProjectInfo Set UnitId=" + argUnitId + " Where CostCentreId=" + argCCId + "";
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

        public static void UpdateUnitRateChange(DataTable dtFlat)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB(); int iFlatId = 0; int iCCId = 0;
            decimal dFBaseAmt = 0, dFNetAmt = 0, dNewRate = 0; bool bTypewise = false;
            int iPayTypeId = 0;
            SqlTransaction tran = conn.BeginTransaction();
            string sSql = "";

                try
                {
                    if (dtFlat.Rows.Count>0)
                    {
                        for (int i = 0; i < dtFlat.Rows.Count; i++)
                        {
                            if (Convert.ToDecimal(dtFlat.Rows[i]["OldRate"]) != Convert.ToDecimal(dtFlat.Rows[i]["NewRate"]))
                            {
                                iFlatId = Convert.ToInt32(dtFlat.Rows[i]["FlatId"]);
                                dNewRate = Convert.ToDecimal(dtFlat.Rows[i]["NewRate"]);

                                //sSql = "Select Area,OtherCostAmt,CostCentreId From dbo.FlatDetails Where FlatId=" + iFlatId + "";
                                sSql = "Select A.Area,A.CostCentreId,B.Typewise,A.PayTypeId,A.AdvPercent,A.AdvAmount From dbo.FlatDetails A " +
                                         " Inner Join dbo.PaySchType B On A.PayTypeId=B.TypeId Where A.FlatId=" + iFlatId + "";
                                cmd = new SqlCommand(sSql, conn, tran);
                                SqlDataReader dr = cmd.ExecuteReader();
                                DataTable dt = new DataTable();
                                dt.Load(dr);
                                dr.Close();
                                cmd.Dispose();

                                if (dt.Rows.Count > 0)
                                {
                                    iCCId = Convert.ToInt32(dt.Rows[0]["CostCentreId"]);
                                    dFBaseAmt = Convert.ToDecimal(dt.Rows[0]["Area"]) * dNewRate;
                                    iPayTypeId = Convert.ToInt32(dt.Rows[0]["PayTypeId"]);

                                    decimal dOtherAmt = 0;
                                    
                                    sSql = "Select ISNULL(Sum(Case When Flag='-' then Amount*(-1) else Amount End),0) Amount from dbo.FlatOtherCost " +
                                            " Where FlatId =" + iFlatId + " AND OtherCostId NOT IN(Select OtherCostId from dbo.OXGross " +
                                            " Where CostCentreId=" + iCCId + ")";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                                    cmd.Dispose();

                                    dFNetAmt = dFBaseAmt + dOtherAmt;

                                    decimal dAdvAmt = Convert.ToDecimal(dt.Rows[0]["AdvAmount"]);
                                    decimal dAdvPer = decimal.Round((dAdvAmt / dFNetAmt) * 100, 2);

                                    sSql = "Update dbo.FlatDetails Set Rate=" + dNewRate + ", " +
                                        " BaseAmt=" + dFBaseAmt + ",NetAmt=" + dFNetAmt + ",AdvAmount=" + dAdvAmt + ",AdvPercent=" + dAdvPer + "" +
                                        " Where FlatId=" + iFlatId + " ";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();

                                    PaymentScheduleDL.InsertFlatScheduleI(iFlatId, conn, tran);
                                    
                                }
                            }
                        }

                    }
                    tran.Commit();

                    BsfGlobal.InsertLog(DateTime.Now, "Flat-Global-Rate-Change", "A","Flat Master", 0, iCCId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                }
                catch (SqlException ex)
                {
                    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                    tran.Rollback();
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

        public static void UpdateReceiptUnitRateChange(DataTable dtFlat)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB(); int iFlatId = 0; int iCCId = 0;
            decimal dFBaseAmt = 0, dFNetAmt = 0, dNewRate = 0; bool bTypewise = false;
            int iPayTypeId = 0;
            SqlTransaction tran = conn.BeginTransaction();
            string sSql = "";

            try
            {
                if (dtFlat.Rows.Count > 0)
                {
                    for (int i = 0; i < dtFlat.Rows.Count; i++)
                    {
                        if (Convert.ToDecimal(dtFlat.Rows[i]["OldRate"]) != Convert.ToDecimal(dtFlat.Rows[i]["NewRate"]))
                        {
                            iFlatId = Convert.ToInt32(dtFlat.Rows[i]["FlatId"]);
                            dNewRate = Convert.ToDecimal(dtFlat.Rows[i]["NewRate"]);

                            sSql = "Select A.Area,A.CostCentreId,B.Typewise,A.PayTypeId,A.AdvPercent,A.AdvAmount From FlatDetails A " +
                                " Inner Join PaySchType B On A.PayTypeId=B.TypeId Where A.FlatId=" + iFlatId + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            SqlDataReader dr = cmd.ExecuteReader();
                            DataTable dt = new DataTable();
                            dt.Load(dr);
                            dr.Close();
                            cmd.Dispose();

                            if (dt.Rows.Count > 0)
                            {
                                iCCId = Convert.ToInt32(dt.Rows[0]["CostCentreId"]);
                                dFBaseAmt = Convert.ToDecimal(dt.Rows[0]["Area"]) * dNewRate;
                                iPayTypeId = Convert.ToInt32(dt.Rows[0]["PayTypeId"]);
                                
                                sSql = "Select ISNULL(Sum(Case When Flag='-' then Amount*(-1) else Amount End),0) Amount from dbo.FlatOtherCost " +
                                        " Where FlatId =" + iFlatId + " and OtherCostId not in (Select OtherCostId from dbo.OXGross " +
                                        " Where CostCentreId=" + iCCId + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                decimal dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                                cmd.Dispose();

                                dFNetAmt = dFBaseAmt + dOtherAmt;
                                bTypewise = Convert.ToBoolean(dt.Rows[0]["Typewise"]);

                                decimal dAdvAmt = Convert.ToDecimal(dt.Rows[0]["AdvAmount"]);
                                decimal dAdvPer = decimal.Round((dAdvAmt / dFNetAmt) * 100, 2);

                                sSql = "Update FlatDetails Set Rate=" + dNewRate + ", " +
                                    " BaseAmt=" + dFBaseAmt + ",NetAmt=" + dFNetAmt + ",AdvAmount=" + dAdvAmt + ",AdvPercent=" + dAdvPer + "" +
                                    " Where FlatId=" + iFlatId + " ";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                sSql = "Select PaymentSchId,FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate," +
                                        " DateAfter, Duration,DurationType,SchPercent,Amount,NetAmount,PreStageTypeId,SortOrder,BillPassed,PaidAmount,StageDetId From dbo.PaymentScheduleFlat " +
                                        " Where FlatId=" + iFlatId + " And TemplateId<>0 Order By SortOrder";
                                cmd = new SqlCommand(sSql, conn, tran);
                                dr = cmd.ExecuteReader();
                                DataTable dtP = new DataTable();
                                dtP.Load(dr);
                                cmd.Dispose();

                                //PaymentScheduleDL.InsertFlatScheduleI(iFlatId, conn, tran);
                                if (bTypewise==true)
                                PaymentScheduleDL.UpdateReceiptBuyerSchedule(iFlatId, dtP, conn, tran);
                                else PaymentScheduleDL.UpdateReceiptBuyerScheduleQual(iFlatId, dtP, conn, tran);
                            }
                        }
                    }

                }
                tran.Commit();

                BsfGlobal.InsertLog(DateTime.Now, "Flat-Rate-Change-After-Receipt", "A", "Flat Master", 0, iCCId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public static DataTable GetUnitRateChange(int argCCId, int argFlatTypeId, string argType, string argFilterType)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql; string sCond = "";
            BsfGlobal.OpenCRMDB();

            try
            {
                if (argType == "N") { sCond = "Not In"; } else { sCond = "In"; }

                sSql = "Select FlatId,B.BlockName,FlatNo,Rate OldRate,Rate NewRate,Convert(bit,0,0) Sel,F.Status From dbo.FlatDetails F" +
                        " INNER JOIN dbo.LevelMaster L ON F.LevelId=L.LevelId " +
                        " INNER JOIN dbo.BlockMaster B ON F.BlockId=B.BlockId " +
                        " Where F.CostCentreId=" + argCCId + " ";
                if (argFilterType == "All" || argFilterType == "Selected Flats" || argFilterType == "Sold" || argFilterType == "UnSold" || argFilterType == "With Receipt")
                {
                    sSql = sSql + " And (FlatId " + sCond + " (Select Distinct FlatId From dbo.PaymentScheduleFlat " +
                            " Where BillPassed=1 Or PaidAmount>0) OR FlatId " + sCond + " (Select Distinct FlatId From ReceiptTrans Where Amount>0)) " +
                            " ORDER BY B.SortOrder,L.SortOrder,dbo.Val(F.FlatNo)";
                }
                else 
                {
                    sSql = sSql + " And FlatTypeId=" + argFlatTypeId + " And (FlatId " + sCond + " (Select Distinct FlatId From dbo.PaymentScheduleFlat " +
                            " Where BillPassed=1 Or PaidAmount>0) OR FlatId " + sCond + " (Select Distinct FlatId From ReceiptTrans Where Amount>0)) " +
                            " ORDER BY B.SortOrder,L.SortOrder,dbo.Val(F.FlatNo)";
                }
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable Executive()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenWorkFlowDB();

            try
            {
                sSql = String.Format("Select UserId ExecId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As ExecName From [{0}].dbo.Users A Inner Join [{0}].dbo.Position B on A.PositionId=B.PositionId Where B.PositionType='M'", BsfGlobal.g_sWorkFlowDBName);
                sda = new SqlDataAdapter(sSql,BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_WorkFlowDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable GetFlats(int argCCId, int argBlockId,int argTypeId,int argFlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                //sSql = "Select FlatId,FlatNo From dbo.FlatDetails Where CostCentreId=" + argCCId + " And BlockId=" + argBlockId + " " +
                //        "and FlatId not in (Select G.FlatId From (Select FlatId,SUM(TotalCP) Total,0 CP from dbo.FlatCarPark " +
                //        "Where TypeId= " + argTypeId + " and CostCentreId = " + argCCId + "  Group by FlatId,TypeId,CostCentreId " +
                //        "Union All Select FlatId,0 Total,Count(SlotNo) CP  from dbo.Slots " +
                //        "Where TypeId=" + argTypeId + "  and CostCentreId = " + argCCId + " and BlockId = " + argBlockId + " " +
                //        "Group by FlatId,TypeId,CostCentreId,BlockId) G Where G.Total-G.CP<=0 and G.FlatId <> " + argFlatId + ")";
                //if (argFlatId == 0)
                //    sSql = "Select FlatId,FlatNo From dbo.FlatDetails Where CostCentreId=" + argCCId + " And BlockId=" + argBlockId + " And FlatId " +
                //            " Not In (Select G.FlatId From (" +
                //            " Select FlatId,SUM(TotalCP) Total,CP=(Select Count(SlotNo) CP from dbo.Slots Where TypeId=A.TypeId  " +
                //            " And CostCentreId=A.CostCentreId and BlockId =" + argBlockId + " And FlatId=A.FlatId) From dbo.FlatCarPark A " +
                //            " Where TypeId= " + argTypeId + " And FlatId<>0  and CostCentreId =" + argCCId + " Group By FlatId,TypeId,CostCentreId " +
                //            " ) G Where G.Total-G.CP<=0 And G.FlatId <> 0)";
                //else
                //    sSql = "Select FlatId,FlatNo From dbo.FlatDetails Where CostCentreId=" + argCCId + " And BlockId=" + argBlockId + " And FlatId " +
                //            " Not In (Select G.FlatId From (" +
                //            " Select FlatId,SUM(TotalCP) Total,CP=(Select Count(SlotNo) CP  " +
                //            " From dbo.Slots Where TypeId=" + argTypeId + " And CostCentreId =" + argCCId + " And BlockId = " + argBlockId + " And FlatId=" + argFlatId + " " +
                //            " Group By FlatId,TypeId,CostCentreId,BlockId) From dbo.FlatCarPark Where TypeId= " + argTypeId + " And FlatId=" + argFlatId + " " +
                //            " And CostCentreId =" + argCCId + " Group By FlatId,TypeId,CostCentreId " +
                //            " ) G Where G.Total<>G.CP And G.FlatId <> 0) Or FlatId In(Select FlatId From dbo.Slots " +
                //            " Where CostCentreId =" + argCCId + " And BlockId = " + argBlockId + " And FlatId=" + argFlatId + " And TypeId=" + argTypeId + ")";
                if (argFlatId == 0)
                    sSql = "Select FlatId,FlatNo From dbo.FlatDetails A" +
                            " INNER JOIN dbo.BlockMaster B ON B.BlockId=A.BlockId " +
                            " INNER JOIN dbo.LevelMaster L ON L.LevelId=A.LevelId " +
                            " Where A.CostCentreId=" + argCCId + " And FlatId " +
                            " Not In (Select G.FlatId From (" +
                            " Select FlatId,SUM(TotalCP) Total,CP=(Select Count(SlotNo) CP From dbo.Slots Where TypeId=A.TypeId  " +
                            " And CostCentreId=A.CostCentreId And FlatId=A.FlatId) From dbo.FlatCarPark A " +
                            " Where TypeId= " + argTypeId + " And FlatId<>0  And CostCentreId =" + argCCId + " Group By FlatId,TypeId,CostCentreId " +
                            " ) G Where G.Total-G.CP<=0 And G.FlatId <> 0) ORDER BY B.SortOrder,L.SortOrder,A.SortOrder,dbo.Val(A.FlatNo) ";
                else
                    sSql = "Select FlatId,FlatNo From dbo.FlatDetails A " +
                            " INNER JOIN dbo.BlockMaster B ON B.BlockId=A.BlockId " +
                            " INNER JOIN dbo.LevelMaster L ON L.LevelId=A.LevelId " +
                            " Where A.CostCentreId=" + argCCId + " And FlatId " +
                            " Not In (Select G.FlatId From (" +
                            " Select FlatId,SUM(TotalCP) Total,CP=(Select Count(SlotNo) CP  " +
                            " From dbo.Slots Where TypeId=" + argTypeId + " And CostCentreId =" + argCCId + " And FlatId=" + argFlatId + " " +
                            " Group By FlatId,TypeId,CostCentreId,BlockId) From dbo.FlatCarPark Where TypeId= " + argTypeId + " And FlatId=" + argFlatId + " " +
                            " And CostCentreId =" + argCCId + " Group By FlatId,TypeId,CostCentreId " +
                            " ) G Where G.Total<>G.CP And G.FlatId <> 0) Or FlatId In(Select FlatId From dbo.Slots " +
                            " Where CostCentreId =" + argCCId + " And FlatId=" + argFlatId + " And TypeId=" + argTypeId + ")";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable FlatCheckList(int argFlatId,string argType)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = "Select A.CheckListId,B.CheckListName,A.ExpCompletionDate,C.Status,C.CompletionDate,C.ExecutiveId,C.Remarks from dbo.FlatTypeChecklist A " +
                       "Inner Join dbo.CheckListMaster B on A.CheckListId = B.CheckListId And A.Status=1" +
                       "Left Join dbo.FlatChecklist C on A.CheckListId = C.CheckListId and C.FlatId = " + argFlatId + " " +
                       "Where B.TypeName = '" + argType + "' and A.FlatTypeId in (Select FlatTypeId from dbo.FlatDetails Where FlatId = " + argFlatId + ") Order by B.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static string FlatRegCheckList(int argFlatId, string argType)
        {
            BsfGlobal.OpenCRMDB();
            string s_Status = "";
            try
            {
                //sSql = "Select A.CheckListId,B.CheckListName,A.ExpCompletionDate,C.Status,C.CompletionDate,C.ExecutiveId,C.Remarks from dbo.FlatTypeChecklist A " +
                //       "Inner Join dbo.CheckListMaster B on A.CheckListId = B.CheckListId And A.Status=1" +
                //       "Left Join dbo.FlatChecklist C on A.CheckListId = C.CheckListId and C.FlatId = " + argFlatId + " " +
                //       "Where B.TypeName = '" + argType + "' and A.FlatTypeId in (Select FlatTypeId from dbo.FlatDetails Where FlatId = " + argFlatId + ") Order by B.SortOrder";

                String sSql = "Select COUNT(*) From FlatChecklist A " +
                              " Inner Join dbo.CheckListMaster B on A.CheckListId=B.CheckListId " +
                              " Where A.FlatId=" + argFlatId + " AND B.CheckListId=1 AND B.TypeName='" + argType + "'";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                int i_Count = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                cmd.ExecuteNonQuery();

                if (i_Count == 0)
                {
                    s_Status = "ND";
                }
                else
                {
                    sSql = "Select Case When A.CompletionDate IS NULL THEN 0 Else 1 End CompDate, A.Status From FlatChecklist A " +
                            " Inner Join dbo.CheckListMaster B on A.CheckListId=B.CheckListId " +
                            " Where A.FlatId=" + argFlatId + " AND B.CheckListId=1 AND B.TypeName='" + argType + "'";
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    DataTable dt = new DataTable();
                    SqlDataReader dreader = cmd.ExecuteReader();
                    dt.Load(dreader);
                    dreader.Close();
                    cmd.ExecuteNonQuery();

                    if (dt.Rows.Count > 0)
                    {
                        int i_CompDate = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["CompDate"], CommFun.datatypes.vartypenumeric));
                        int i_Status = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["Status"], CommFun.datatypes.vartypenumeric));

                        if (i_Status == 1 && i_CompDate == 1)
                            s_Status = "D";
                        else if (i_Status == 0 && i_CompDate == 0)
                            s_Status = "CND";
                        else if (i_Status == 1 && i_CompDate == 0)
                            s_Status = "CND";
                        else if (i_Status == 0 && i_CompDate == 1)
                            s_Status = "CND";
                    }
                    else
                    {
                        s_Status = "ND";
                    }
                }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return s_Status;
        }

        public static DataTable FlatBuyer(int argFlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = "Select A.LeadId,B.LeadName,C.CoApplicantName From FlatDetails A " +
                        " Inner Join dbo.LeadRegister B On A.LeadId=B.LeadId " +
                        " Inner Join dbo.LeadCoApplicantInfo C On C.LeadId=B.LeadId " +
                        " Where A.Status='S' And A.FlatId=" + argFlatId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable FlatTypeCheckList(int argFlatTypeId,string argType,int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.CheckListId, Status, " +
                       "A.CheckListName,B.ExpCompletionDate from dbo.CheckListMaster A  " +
                       "Left Join dbo.FlatTypeChecklist B On A.CheckListId=B.CheckListId and B.FlatTypeID = " + argFlatTypeId + " Where A.TypeName='" + argType + "' and " +
                       "A.CheckListId in (Select CheckListId from dbo.CCCheckListTrans Where CostCentreId= " + argCCId + " And Status=1) Order by A.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable ProjectCheckList(int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = "Select A.CheckListId,A.CheckListName,Status,B.CompletionDate,B.Remarks" +
                    " from dbo.CheckListMaster A Left Join dbo.CCCheckListTrans B" +
                    " on A.CheckListId= B.CheckListId AND B.CostCentreId=" + argCCId + " WHERE A.TypeName='P' Order by A.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable ProjectCheckListFlat(int argCCId,string argType)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = "Select A.CheckListId,A.CheckListName,Status " +
                    " from dbo.CheckListMaster A Left Join dbo.CCCheckListTrans B" +
                    " on A.CheckListId= B.CheckListId AND B.CostCentreId=" + argCCId + " WHERE A.TypeName= '" + argType + "' Order by A.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable FinalCheckListFlat(int argFlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = " Select A.CheckListId,B.CheckListName,A.ExpCompletionDate,C.Status,C.CompletionDate,C.ExecutiveId,C.Remarks from dbo.FlatTypeChecklist A " +
                       " Inner Join dbo.CheckListMaster B on A.CheckListId = B.CheckListId And A.Status=1 " +
                       " Left Join dbo.FlatChecklist C on A.CheckListId = C.CheckListId and C.FlatId = " + argFlatId + " " +
                       " Where B.TypeName = 'F' " +
                       " and A.FlatTypeId in (Select FlatTypeId from dbo.FlatDetails Where FlatId = " + argFlatId + ") " +
                       " Order by B.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable FinalCheckListPlot(int argPlotId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = " Select A.CheckListId,A.CheckListName,B.ExpCompletionDate,B.CompletionDate,B.ExecutiveId,B.Remarks " +
                        " from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.CheckListMaster A " +
                        " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotDetailsCheckList B on A.CheckListId = B.CheckListId " +
                        " and B.PlotDetailsId = " + argPlotId + " Where A.Type = 'F' Order by A.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable FinalPlot(int argPlotId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = " Select CheckListId From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotDetailsCheckList "+
                        " Where PlotDetailsId = " + argPlotId + " ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable CancelCheckListFlat(int argFlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = " Select A.CheckListId,B.CheckListName,A.ExpCompletionDate,C.Status,C.CompletionDate,C.ExecutiveId,C.Remarks from dbo.FlatTypeChecklist A " +
                       " Inner Join dbo.CheckListMaster B on A.CheckListId = B.CheckListId And A.Status=1 " +
                       " Left Join dbo.FlatChecklist C on A.CheckListId = C.CheckListId and C.FlatId = " + argFlatId + " " +
                       " Where B.TypeName = 'C' " +
                       " and A.FlatTypeId in (Select FlatTypeId from dbo.FlatDetails Where FlatId = " + argFlatId + ") " +
                       " Order by B.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static void InsertProjCheckList(DataTable dt,int argCCId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "DELETE FROM dbo.CCCheckListTrans WHERE CostCentreId=" + argCCId + " and " +
                           "CheckListId in (Select CheckListId from dbo.CheckListMaster Where TypeName = 'P')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string sDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[i]["CompletionDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MMM-yyyy");
                            if (Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean)) == true)
                            {
                                sSql = "INSERT INTO dbo.CCCheckListTrans (CostCentreId,CheckListId,CompletionDate,Status,Remarks) VALUES" +
                                    " (" + argCCId + "," + dt.Rows[i]["CheckListId"] + "," +
                                    " '" + sDate + "'," +
                                    " '" + Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean)) + "'," +
                                    "'" + dt.Rows[i]["Remarks"] + "')";
                            }
                            else
                            {
                                sSql = "INSERT INTO dbo.CCCheckListTrans (CostCentreId,CheckListId,CompletionDate,Status,Remarks) VALUES" +
                                " (" + argCCId + "," + dt.Rows[i]["CheckListId"] + "," +
                                " NULL ," +
                                " '" + Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean)) + "'," +
                                "'" + dt.Rows[i]["Remarks"] + "')";
                            }
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
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

        public static void InsertProjCheckListFlat(DataTable dt, int argCCId,string argType)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "DELETE FROM dbo.CCCheckListTrans WHERE CostCentreId=" + argCCId + " and " +
                           "CheckListId in (Select CheckListId from dbo.CheckListMaster Where TypeName = '" + argType +"')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sSql = "INSERT INTO dbo.CCCheckListTrans (CostCentreId,CheckListId,Status) VALUES" +
                                " (" + argCCId + "," + dt.Rows[i]["CheckListId"] + ",'"+Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean))+"')";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
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
        
        public static void InsertFlatTypeCheckList(DataTable dt, int argFlatTypeId,string argType)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "DELETE FROM dbo.FlatTypeChecklist WHERE FlatTypeId=" + argFlatTypeId + " and " +
                           "CheckListID in (Select CheckListId from dbo.CheckListMaster Where TypeName = '" + argType + "')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DateTime deDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[i]["ExpCompletionDate"], CommFun.datatypes.VarTypeDate));
                            if (Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean)) == true)
                            {
                                if (deDate == DateTime.MinValue)
                                {
                                    sSql = "INSERT INTO dbo.FlatTypeChecklist (FlatTypeId,CheckListId,ExpCompletionDate,Status) VALUES" +
                                           " (" + argFlatTypeId + "," + dt.Rows[i]["CheckListId"] + ",NULL," +
                                           " '" + Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean)) + "')";
                                }
                                else
                                {
                                    sSql = "INSERT INTO dbo.FlatTypeChecklist (FlatTypeId,CheckListId,ExpCompletionDate,Status) VALUES" +
                                           " (" + argFlatTypeId + "," + dt.Rows[i]["CheckListId"] + ",'" + deDate.ToString("dd-MMM-yyyy") + "'," +
                                           " '" + Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean)) + "')";
                                }
                            }
                            else
                            {
                                sSql = "INSERT INTO dbo.FlatTypeChecklist (FlatTypeId,CheckListId,ExpCompletionDate,Status) VALUES" +
                                       " (" + argFlatTypeId + "," + dt.Rows[i]["CheckListId"] + ",NULL," +
                                       " '" + Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean)) + "')";
                            }
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
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

        public static void InsertFlatCheckList(DataTable dt, int argFlatId, string argType, bool argChkSend, int argCCId, string argFlatNo)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    string sSql = "DELETE FROM dbo.FlatChecklist WHERE FlatId=" + argFlatId + " and " +
                                    "CheckListID IN(Select CheckListId from dbo.CheckListMaster Where TypeName = '" + argType + "')";
                    SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string sCDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[i]["CompletionDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MMM-yyyy");
                            if (sCDate == "01-Jan-0001") sCDate = "NULL"; else sCDate = "'" + sCDate + "'";
                            string sEDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[i]["ExpCompletionDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MMM-yyyy");
                            if (sEDate == "01-Jan-0001") sEDate = "NULL"; else sEDate = "'" + sEDate + "'";
                            if (Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean)) == true)
                            {
                                sSql = "INSERT INTO dbo.FlatChecklist (FlatId,CheckListId,CompletionDate,ExpCompletionDate,ExecutiveId,Status,Remarks)" +
                                        " VALUES(" + argFlatId + "," + dt.Rows[i]["CheckListId"] + "," + sCDate + "," + sEDate + "," +
                                        "" + CommFun.IsNullCheck(dt.Rows[i]["ExecutiveId"], CommFun.datatypes.vartypenumeric) + "," +
                                        " '" + Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean)) + "'," +
                                        "'" + dt.Rows[i]["Remarks"] + "')";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                            else
                            {
                                sSql = "INSERT INTO dbo.FlatChecklist (FlatId,CheckListId,ExpCompletionDate,CompletionDate,ExecutiveId,Status,Remarks)" +
                                        " VALUES(" + argFlatId + "," + dt.Rows[i]["CheckListId"] + ",NULL,NULL," +
                                        "" + CommFun.IsNullCheck(dt.Rows[i]["ExecutiveId"], CommFun.datatypes.vartypenumeric) + "," +
                                        " '" + Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean)) +
                                        "','" + dt.Rows[i]["Remarks"] + "')";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }
                    }

                    if (argChkSend == true)
                    {
                        if (argType == "H")
                            CommFun.CheckListAlertUpdate("CRM-CheckList-HandingOver", "CRM-CheckList-HandingOver -" + argFlatNo, argCCId, argFlatId);
                        else if (argType == "W")
                            CommFun.CheckListAlertUpdate("CRM-CheckList-Works", "CRM-CheckList-Works -" + argFlatNo, argCCId, argFlatId);
                        else if (argType == "F")
                            CommFun.CheckListAlertUpdate("CRM-CheckList-Finalisation", "CRM-CheckList-Finalisation -" + argFlatNo, argCCId, argFlatId);
                    }
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

        public static void InsertFlatChk(DataTable dt, int argFlatId, string argType, bool argChkSend, string argFlatNo,int argCCId, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                if (dt != null)
                {
                    string sSql = ""; SqlCommand cmd;

                    sSql = "DELETE FROM dbo.FlatChecklist WHERE FlatId=" + argFlatId + " and " +
                            "CheckListID in (Select CheckListId from dbo.CheckListMaster Where TypeName = '" + argType + "')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean)) == true)
                        {
                            sSql = "INSERT INTO dbo.FlatChecklist (FlatId,CheckListId,ExpCompletionDate,CompletionDate,ExecutiveId,Status,Remarks) VALUES" +
                                " (" + argFlatId + "," + dt.Rows[i]["CheckListId"] + "," +
                                "'" + Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[i]["ExpCompletionDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MMM-yyyy") + "'," +
                                "'" + Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[i]["CompletionDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MMM-yyyy") + "'," +
                                "" + CommFun.IsNullCheck(dt.Rows[i]["ExecutiveId"], CommFun.datatypes.vartypenumeric) + "," +
                                " '" + Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean)) + "','" + dt.Rows[i]["Remarks"] + "')";
                        }
                        else
                        {
                            sSql = "INSERT INTO dbo.FlatChecklist (FlatId,CheckListId,ExpCompletionDate,CompletionDate,ExecutiveId,Status,Remarks) VALUES" +
                                    " (" + argFlatId + "," + dt.Rows[i]["CheckListId"] + "," +
                                    "NULL," +
                                    "NULL," +
                                    "" + CommFun.IsNullCheck(dt.Rows[i]["ExecutiveId"], CommFun.datatypes.vartypenumeric) + "," +
                                    " '" + Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean)) + "','" + dt.Rows[i]["Remarks"] + "')";
                        }
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    if (argChkSend == true)
                    {
                        if (argType == "H")
                            CommFun.CheckListAlertUpdate("CRM-CheckList-HandingOver", "CRM-CheckList-HandingOver -" + argFlatNo, argCCId, argFlatId);
                        else if (argType == "W")
                            CommFun.CheckListAlertUpdate("CRM-CheckList-Works", "CRM-CheckList-Works -" + argFlatNo, argCCId, argFlatId);
                        else if (argType == "F")
                            CommFun.CheckListAlertUpdate("CRM-CheckList-Finalisation", "CRM-CheckList-Finalisation -" + argFlatNo, argCCId, argFlatId);
                    }
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
        }

        public static void InsertPlotChk(DataTable dt, int argPlotId, string argType, bool argChkSend, string argFlatNo, int argCCId, SqlConnection conn, SqlTransaction tran,DataTable dtLand)
        {
            string sSql = ""; SqlCommand cmd;
            try
            {
                if (dtLand.Rows.Count > 0)
                {
                    sSql = "DELETE FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotDetailsCheckList WHERE PlotDetailsId=" + argPlotId + " and " +
                            "CheckListId in (Select CheckListId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.CheckListMaster Where Type = '" + argType + "')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                if (dt != null)
                {

                    sSql = "DELETE FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotDetailsCheckList WHERE PlotDetailsId=" + argPlotId + " and " +
                            "CheckListId in (Select CheckListId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.CheckListMaster Where Type = '" + argType + "')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sSql = "INSERT INTO [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotDetailsCheckList (PlotDetailsId,CheckListId,ExpCompletionDate,CompletionDate,ExecutiveId,Remarks) VALUES" +
                            " (" + argPlotId + "," + dt.Rows[i]["CheckListId"] + "," +
                            "'" + Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[i]["ExpCompletionDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MMM-yyyy") + "'," +
                            "'" + Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[i]["CompletionDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MMM-yyyy") + "'," +
                            "" + CommFun.IsNullCheck(dt.Rows[i]["ExecutiveId"], CommFun.datatypes.vartypenumeric) + "," +
                            " '" + dt.Rows[i]["Remarks"] + "')";

                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }



                    if (argChkSend == true)
                    {
                        if (argType == "H")
                            CommFun.CheckListAlertUpdate("CRM-CheckList-HandingOver", "CRM-CheckList-HandingOver -" + argFlatNo, argCCId, argPlotId);
                        else if (argType == "W")
                            CommFun.CheckListAlertUpdate("CRM-CheckList-Works", "CRM-CheckList-Works -" + argFlatNo, argCCId, argPlotId);
                        else if (argType == "F")
                            CommFun.CheckListAlertUpdate("CRM-CheckList-Finalisation", "CRM-CheckList-Finalisation -" + argFlatNo, argCCId, argPlotId);
                    }
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
        }

        public static void UpdateBuyerName(int argLeadId,string argLeadName,string argCoAppName)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "Update dbo.LeadRegister Set LeadName='" + argLeadName + "' Where LeadId=" + argLeadId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Update dbo.LeadCoApplicantInfo Set CoApplicantName='" + argCoAppName + "' Where LeadId=" + argLeadId + "";
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

        public static DataTable PaySchType()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "select TypeId,TypeName,EMI,RoundValue,NoOfMonths from dbo.PaySchType ORDER BY TypeName";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        public static DataTable PlotPaySchType()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = "Select TypeId,TypeName, 0 EMI From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaySchType ORDER BY TypeName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable PaymentSchType()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = "select TypeId,TypeName,Typewise,RoundValue,EMI,NoOfMonths from dbo.PaySchType ORDER BY TypeName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable PayAmount(int argFlatTypeId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = String.Format("select AdvAmount,NetAmt from dbo.FlatType where FlatTypeId={0}", argFlatTypeId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable PayOC(int argFlatTypeId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = String.Format("select FlatTypeId,OtherCostId,Amount from dbo.FlatTypeOtherCost Where FlatTypeId={0}", argFlatTypeId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable PayOCSetup(int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = String.Format("Select OtherCostId From dbo.OtherCostSetupTrans Where CostCentreId={0}", argCCId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static void FlatTemplate(DataTable dtTwise,int argCCId,string start,string block,string level,string type)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    if (dtTwise.Rows.Count > 0)
                    {
                    for (int i = 0; i < dtTwise.Rows.Count; i++)
                    {
                        sSql = "INSERT INTO dbo.FlatTemplate(ProjectId,BlockId,LevelId,NoOfFlats,FlatTypeId,Start,BlockTitle,LevelTitle,TypeTitle) " +
                                " VALUES(" + argCCId + "," + Convert.ToInt32(dtTwise.Rows[i]["BlockId"].ToString()) + "," + Convert.ToInt32(dtTwise.Rows[i]["LevelId"].ToString()) + ", " +
                                " " + Convert.ToInt32(dtTwise.Rows[i]["TotalFlat"].ToString()) + "," + Convert.ToInt32(dtTwise.Rows[i]["FlatTypeId"].ToString()) + ", " +
                                " '" + start + "','" + block + "','" + level + "','" + type + "')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
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

        public static void GenerateFlat(DataTable dtT, DataTable dtTemp, int argCCId, string l, string m, string n, string o)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    if (dtT.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtT.Rows.Count; i++)
                        {
                            sSql = "INSERT INTO dbo.FlatDetails(FlatNo,FlatTypeId,CostCentreId,BlockId,LevelID,Area,Rate,BaseAmt,OtherCostAmt,TotalCarPark," +
                                    " NetAmt,Remarks,Status) Values(" +
                                    " '" + (l + m + n + o) + "'," + dtTemp.Rows[0]["FlatTypeId"] + "," + argCCId + ", " +
                                    " " + dtT.Rows[i]["BlockId"] + "," + dtT.Rows[i]["LevelId"] + "," + dtTemp.Rows[0]["Area"] + "," + dtTemp.Rows[0]["Rate"] + "," +
                                    " " + dtTemp.Rows[0]["BaseAmt"] + "," + dtTemp.Rows[0]["OtherCostAmt"] + "," + dtTemp.Rows[0]["TotalCarPark"] + "," +
                                    " " + dtTemp.Rows[0]["NetAmt"] + ",'" + dtTemp.Rows[0]["Remarks"] + "','U') SELECT SCOPE_IDENTITY();";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
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

        public static void InsertStage(UnitDirBL OUintDirBL)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            int iStageDetId = 0;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "INSERT INTO dbo.StageDetails(CostCentreId,RefNo,BlockId,LevelId,SchType,StageId,StageDate," +
                        " CompletionDate,DueDate,Remarks)VALUES(" + OUintDirBL.CCId + ",'" + OUintDirBL.RefNo + "'," + OUintDirBL.BlockId + "," +
                        " " + OUintDirBL.LevelId + ",'" + OUintDirBL.SchType + "'," + OUintDirBL.StageId + ",'" + Convert.ToDateTime(OUintDirBL.StageDate).ToString("dd-MMM-yyyy") + "'," +
                        " '" + Convert.ToDateTime(OUintDirBL.CompletionDate).ToString("dd-MMM-yyyy") + "'," +
                        " '" + Convert.ToDateTime(OUintDirBL.DueDate).ToString("dd-MMM-yyyy") + "','" + OUintDirBL.Remarks + "')SELECT SCOPE_IDENTITY()";
                    cmd = new SqlCommand(sSql, conn, tran);
                    iStageDetId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();

                    if (OUintDirBL.SchType == "D")
                    {
                        sSql = "Update PaymentScheduleFlat Set StageDetId=" + iStageDetId + " Where CostCentreId=" + OUintDirBL.CCId + " And SchType='D' " +
                                " And SchDescId=" + OUintDirBL.StageId + " And FlatId In " +
                                " (Select FlatId From FlatDetails Where BlockId=" + OUintDirBL.BlockId + " ";
                    }
                    else if (OUintDirBL.SchType == "S")
                    {
                        sSql = "Update PaymentScheduleFlat Set StageDetId=" + iStageDetId + " Where CostCentreId=" + OUintDirBL.CCId + " And SchType='S' " +
                                " And StageId=" + OUintDirBL.StageId + " And FlatId In " +
                                " (Select FlatId From FlatDetails Where BlockId=" + OUintDirBL.BlockId + " ";
                    }
                    else if (OUintDirBL.SchType == "O")
                    {
                        sSql = "Update PaymentScheduleFlat Set StageDetId=" + iStageDetId + " Where CostCentreId=" + OUintDirBL.CCId + " And SchType='O' " +
                                " And OtherCostId=" + OUintDirBL.StageId + " And FlatId In " +
                                " (Select FlatId From FlatDetails Where BlockId=" + OUintDirBL.BlockId + " ";
                    }
                    if (OUintDirBL.LevelId == 0) { sSql = sSql + ")"; } else { sSql = sSql + "And LevelId=" + OUintDirBL.LevelId + ")"; }
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "UPDATE PaymentScheduleFlat SET StageDetId=B.StageDetId FROM " +
                                " PaymentScheduleFlat A JOIN (SELECT TOP 1 StageDetId,FlatId FROM PaymentScheduleFlat WHERE Advance<>0 AND StageDetId=" + iStageDetId +
                                ") B ON B.FlatId=A.FlatId WHERE A.SchType='A'";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

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

        public static void UpdateStage(UnitDirBL OUintDirBL, int argStageDetId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "UPDATE dbo.StageDetails SET CostCentreId=" + OUintDirBL.CCId + ",RefNo='" + OUintDirBL.RefNo + "',BlockId=" + OUintDirBL.BlockId + ",LevelId=" + OUintDirBL.LevelId + "," +
                        " SchType='" + OUintDirBL.SchType + "',StageId=" + OUintDirBL.StageId + ",StageDate='" + Convert.ToDateTime(OUintDirBL.StageDate).ToString("dd-MMM-yyyy") + "'," +
                        " CompletionDate='" + Convert.ToDateTime(OUintDirBL.CompletionDate).ToString("dd-MMM-yyyy") + "'," +
                        " DueDate='" + Convert.ToDateTime(OUintDirBL.DueDate).ToString("dd-MMM-yyyy") + "', " +
                        " Remarks='" + OUintDirBL.Remarks + "' WHERE StageDetId=" + argStageDetId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (OUintDirBL.SchType == "D")
                    {
                        sSql = "Update PaymentScheduleFlat Set StageDetId=" + argStageDetId + " Where CostCentreId=" + OUintDirBL.CCId + " And SchType='D' " +
                                " And SchDescId=" + OUintDirBL.StageId + " And FlatId In " +
                                " (Select FlatId From FlatDetails Where BlockId=" + OUintDirBL.BlockId + " ";
                    }
                    else if (OUintDirBL.SchType == "S")
                    {
                        sSql = "Update PaymentScheduleFlat Set StageDetId=" + argStageDetId + " Where CostCentreId=" + OUintDirBL.CCId + " And SchType='S' " +
                                " And StageId=" + OUintDirBL.StageId + " And FlatId In " +
                                " (Select FlatId From FlatDetails Where BlockId=" + OUintDirBL.BlockId + " ";
                    }
                    else if (OUintDirBL.SchType == "O")
                    {
                        sSql = "Update PaymentScheduleFlat Set StageDetId=" + argStageDetId + " Where CostCentreId=" + OUintDirBL.CCId + " And SchType='O' " +
                                " And OtherCostId=" + OUintDirBL.StageId + " And FlatId In " +
                                " (Select FlatId From FlatDetails Where BlockId=" + OUintDirBL.BlockId + " ";
                    }
                    if (OUintDirBL.LevelId == 0) { sSql = sSql + ")"; } else { sSql = sSql + "And LevelId=" + OUintDirBL.LevelId + ")"; }
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "UPDATE PaymentScheduleFlat SET StageDetId=B.StageDetId FROM " +
                                " PaymentScheduleFlat A JOIN (SELECT TOP 1 StageDetId,FlatId FROM PaymentScheduleFlat WHERE Advance<>0 AND StageDetId=" + argStageDetId +
                                ") B ON B.FlatId=A.FlatId WHERE A.SchType='A'";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

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

        internal static void UpdateRefreshStage()
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "Select * From dbo.StageDetails";
                    cmd = new SqlCommand(sSql, conn, tran);
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    cmd.Dispose();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["SchType"].ToString() == "D")
                        {
                            sSql = "Update PaymentScheduleFlat Set StageDetId=" + dt.Rows[i]["StageDetId"] + 
                                    " Where CostCentreId=" + dt.Rows[i]["CostCentreId"] + " And SchType='D' " +
                                    " And SchDescId=" + dt.Rows[i]["StageId"] + " And FlatId In " +
                                    " (Select FlatId From FlatDetails Where BlockId=" + dt.Rows[i]["BlockId"] + " ";
                        }
                        else if (dt.Rows[i]["SchType"].ToString() == "S")
                        {
                            sSql = "Update PaymentScheduleFlat Set StageDetId=" + dt.Rows[i]["StageDetId"] + 
                                    " Where CostCentreId=" + dt.Rows[i]["CostCentreId"] + " And SchType='S' " +
                                    " And StageId=" + dt.Rows[i]["StageId"] + " And FlatId In " +
                                    " (Select FlatId From FlatDetails Where BlockId=" + dt.Rows[i]["BlockId"] + " ";
                        }
                        else if (dt.Rows[i]["SchType"].ToString() == "O")
                        {
                            sSql = "Update PaymentScheduleFlat Set StageDetId=" + dt.Rows[i]["StageDetId"] + 
                                    " Where CostCentreId=" + dt.Rows[i]["CostCentreId"] + " And SchType='O' " +
                                    " And OtherCostId=" + dt.Rows[i]["StageId"] + " And FlatId In " +
                                    " (Select FlatId From FlatDetails Where BlockId=" + dt.Rows[i]["BlockId"] + " ";
                        }
                        if (Convert.ToInt32(dt.Rows[i]["LevelId"]) == 0) { sSql = sSql + ")"; } else { sSql = sSql + "And LevelId=" + dt.Rows[i]["LevelId"] + ")"; }
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "UPDATE PaymentScheduleFlat SET StageDetId=B.StageDetId FROM "+
                                " PaymentScheduleFlat A JOIN (SELECT StageDetId,FlatId FROM PaymentScheduleFlat WHERE Advance<>0 "+
                                ") B ON B.FlatId=A.FlatId WHERE A.SchType='A' AND A.StageDetId=0";
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

        public static DataTable GetStage(string argSchType, int argCCID)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                if (argSchType != "")
                {
                    String sSql = "";
                    if (argSchType == "SchDescription")
                    {
                        sSql = "SELECT DISTINCT P.SchDescId Id,P.Description Name FROM dbo.PaymentSchedule P WHERE P.SchType='D' AND CostCentreId= " + argCCID;
                    }
                    else if (argSchType == "Stagewise")
                    {
                        sSql = "SELECT DISTINCT P.StageId Id,P.Description Name FROM dbo.PaymentSchedule P WHERE P.SchType='S' AND CostCentreId= " + argCCID;
                    }
                    else if (argSchType == "OtherCost")
                    {
                        sSql = "SELECT DISTINCT P.OtherCostId Id,P.Description Name FROM dbo.PaymentSchedule P WHERE P.SchType='O' AND CostCentreId= " + argCCID;
                    }

                    if (sSql != "")
                    {
                        SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                        dt = new DataTable();
                        SqlDataReader dreader = cmd.ExecuteReader();
                        dt.Load(dreader);
                        dreader.Close();
                        cmd.Dispose();
                    }
                }
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

        public static DataTable GetStageDetails(int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = "Select G.StageDetId,G.StageDate,G.RefNo,Case When G.SchType='S' Then 'Stagewise' When G.SchType='D' Then 'SchDescription' Else " +
                       "'OtherCost' End SchType,G.StageName,B.BlockName,Isnull(L.LevelName,'All Levels') LevelName,G.CompletionDate,G.DueDate from " +
                       "(SELECT SD.StageDetId,SD.StageDate,SD.RefNo,S.StageName,SD.BlockId,SD.LevelId,SD.SchType,SD.CompletionDate,SD.DueDate FROM  dbo.StageDetails SD " +
                       "INNER JOIN dbo.Stages S ON S.StageId=SD.StageId AND SD.SchType='S' WHERE SD.CostCentreId= " + argCCId + " " +
                       "UNION ALL " +
                       "SELECT SD.StageDetId,SD.StageDate,SD.RefNo,S.SchDescName StageName,SD.BlockId,SD.LevelId,SD.SchType,SD.CompletionDate,SD.DueDate FROM  dbo.StageDetails SD " +
                       "INNER JOIN dbo.SchDescription S ON S.SchDescId=SD.StageId AND SD.SchType='D' WHERE SD.CostCentreId=" + argCCId + " " +
                       "UNION ALL " +
                       "SELECT SD.StageDetId,SD.StageDate,SD.RefNo,S.OtherCostName StageName,SD.BlockId,SD.LevelId,SD.SchType,SD.CompletionDate,SD.DueDate FROM  dbo.StageDetails SD " +
                       "INNER JOIN dbo.OtherCostMaster S ON S.OtherCostId=SD.StageId AND SD.SchType='O' WHERE SD.CostCentreId=" + argCCId + ") G " +
                       "Left Join dbo.BlockMaster B on G.BlockId=B.BlockId " +
                       "Left Join dbo.LevelMaster L on G.LevelId=L.LevelId Order by G.StageDate,G.RefNo";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable GetChangeGridStageDetails(int argStageDetId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = "Select G.StageDetId,G.StageDate,G.RefNo,Case When G.SchType='S' Then 'Stagewise' When G.SchType='D' Then 'SchDescription' Else " +
                       "'OtherCost' End SchType,G.StageName,B.BlockName,Isnull(L.LevelName,'All Levels') LevelName,G.CompletionDate,G.DueDate from " +
                       "(SELECT SD.StageDetId,SD.StageDate,SD.RefNo,S.StageName,SD.BlockId,SD.LevelId,SD.SchType,SD.CompletionDate,SD.DueDate FROM  dbo.StageDetails SD " +
                       "INNER JOIN dbo.Stages S ON S.StageId=SD.StageId AND SD.SchType='S' WHERE SD.StageDetId= " + argStageDetId + " " +
                       "UNION ALL " +
                       "SELECT SD.StageDetId,SD.StageDate,SD.RefNo,S.SchDescName StageName,SD.BlockId,SD.LevelId,SD.SchType,SD.CompletionDate,SD.DueDate FROM  dbo.StageDetails SD " +
                       "INNER JOIN dbo.SchDescription S ON S.SchDescId=SD.StageId AND SD.SchType='D' WHERE SD.StageDetId=" + argStageDetId + " " +
                       "UNION ALL " +
                       "SELECT SD.StageDetId,SD.StageDate,SD.RefNo,S.OtherCostName StageName,SD.BlockId,SD.LevelId,SD.SchType,SD.CompletionDate,SD.DueDate FROM  dbo.StageDetails SD " +
                       "INNER JOIN dbo.OtherCostMaster S ON S.OtherCostId=SD.StageId AND SD.SchType='O' WHERE SD.StageDetId=" + argStageDetId + ") G " +
                       "Left Join dbo.BlockMaster B on G.BlockId=B.BlockId " +
                       "Left Join dbo.LevelMaster L on G.LevelId=L.LevelId Order by G.StageDate,G.RefNo";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable GetEditStgDetails(int argStgId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = "SELECT * FROM StageDetails WHERE StageDetId=" + argStgId + "";

                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static bool CheckPBStage(int argStageDetId)
        {
            bool b_PB = false;
            BsfGlobal.OpenCRMDB();
            try
            {
                string sSql = "Select COUNT(*) FROM dbo.PaymentScheduleFlat A" +
                              " INNER JOIN dbo.ProgressBillRegister B ON A.FlatId=B.FlatId" +
                              " INNER JOIN dbo.StageDetails C ON A.StageDetId=C.StageDetId" +
                              " WHERE A.StageDetId=" + argStageDetId + " AND A.BillPassed=1";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                int i_Count = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                cmd.Dispose();

                if (i_Count > 0) { b_PB = true; }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return b_PB;
        }

        public static void DeleteStage(int argStageDetId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    string sSql = "DELETE FROM dbo.StageDetails WHERE StageDetId=" + argStageDetId + "";
                    SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Update PaymentScheduleFlat Set StageDetId=0 WHERE StageDetId=" + argStageDetId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

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

        public static void InsertEItem(int argCCId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "INSERT INTO ExtraItemTypeMaster (ExtraItemTypeId,ExtraItemTypeName," +
                        " CostCentreId)SELECT ExtraItemTypeId,ExtraItemTypeName," + argCCId + " FROM" +
                        " ExtraItemTypeMaster WHERE ExtraItemTypeId" +
                        " NOT IN (SELECT ExtraItemTypeId FROM ExtraItemTypeMaster WHERE CostCentreId=" + argCCId + ")";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "INSERT INTO ExtraItemMaster (ExtraItemId,ItemCode,ExtraItemTypeId," +
                        " ItemDescription,ExtraRate,CostCentreId) SELECT ExtraItemId,ItemCode,ExtraItemTypeId," +
                        " ItemDescription,RevisedRate-DefaultRate," + argCCId + " FROM " +
                        " ExtraItemMaster WHERE ExtraItemId NOT IN " +
                        " (SELECT ExtraItemId FROM ExtraItemMaster WHERE CostCentreId=" + argCCId + ")";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

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

        public static DataTable GetEItem(int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = "SELECT T.TransId,ExtraItemId,ItemCode,ExtraItemTypeName," +
                " ItemDescription,ExtraRate FROM ExtraItemMaster I" +
                " INNER JOIN ExtraItemTypeMaster T ON I.ExtraItemTypeId=T.ExtraItemTypeId" +
                " WHERE T.CostCentreId=" + argCCId + " AND I.CostCentreId=" + argCCId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable GetCar(int argTypeId,int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "SELECT DISTINCT C.TypeId FROM CarParkMaster M " +
                              " INNER JOIN CarParkCost C ON M.TypeId=C.TypeId " +
                              " WHERE M.TypeId=" + argTypeId + "";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();

                int iType = 0;
                if (dt.Rows.Count > 0) { iType = Convert.ToInt32(dt.Rows[0]["TypeId"]); }

                if (iType != 0)
                {
                    sSql = "SELECT B.BlockId, B.BlockName, ISNULL(NoOfSlots,0) NoOfSlots, ISNULL(AllottedSlots,0) AllottedSlots FROM dbo.BlockMaster B " +
                           " LEFT JOIN dbo.CarParkMaster C ON C.BlockId=B.BlockId AND C.TypeId=" + argTypeId +
                           " WHERE B.CostCentreId=" + argCCId + 
                           " ORDER BY B.SortOrder";
                }
                else
                {
                    sSql = "SELECT B.BlockId, B.BlockName, 0 NoOfSlots, 0 AllottedSlots FROM dbo.BlockMaster B " +
                           " WHERE B.CostCentreId=" + argCCId +
                           " ORDER BY B.SortOrder";
                }
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        public static DataTable GetBlockWiseUDS(int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "SELECT A.CostCentreId,A.BlockId,A.BlockName,Isnull(B.LandArea,0)LandArea,"+
                        " Isnull(B.WithHeld,0)WithHeld,Isnull(B.NetLandArea,0)NetLandArea,Isnull(B.FSIIndex,0)FSIIndex," +
                        " Isnull(B.BuildArea,0)BuildArea,Convert(bit,0,0) Sel From BlockMaster A" +
                        " Left Join BlockwiseUDS B On A.CostCentreId=B.CostCentreId And A.BlockId=B.BlockId" +
                        " Where A.CostCentreId=" + argCCId + " Order By A.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataSet GetBlockWiseUDSReport(int argCCId)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "SELECT A.CostCentreId,A.BlockId,A.BlockName,IsNull(B.NetLandArea,0)NetLandArea,IsNull(B.BuildArea,0)BuildArea " +
                        " From BlockMaster A Left Join BlockwiseUDS B On A.CostCentreId=B.CostCentreId And A.BlockId=B.BlockId" +
                        " Where A.CostCentreId=" + argCCId + " Order By A.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds,"NetLandArea");
                sda.Dispose();

                sSql = "Select BlockId,SUM(USLand)FlatUDS,SUM(Area)FlatBuildArea From FlatDetails" +
                        " Where CostCentreId=" + argCCId + " Group By BlockId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "FlatUDS");
                sda.Dispose();

                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public static void InsertUDS(DataTable argdt,int argCCId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "Delete From BlockWiseUDS WHERE CostCentreId=" + argCCId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (argdt != null)
                    {
                        for (int i = 0; i < argdt.Rows.Count; i++)
                        {
                            sSql = "INSERT INTO BlockWiseUDS(CostCentreId,BlockId,LandArea,WithHeld,NetLandArea,FSIIndex,BuildArea)Values" +
                                "(" + argdt.Rows[i]["CostCentreId"] + "," + argdt.Rows[i]["BlockId"] + "," + argdt.Rows[i]["LandArea"] + "," +
                                " " + argdt.Rows[i]["WithHeld"] + "," + argdt.Rows[i]["NetLandArea"] + "," +
                                " " + argdt.Rows[i]["FSIIndex"] + "," + argdt.Rows[i]["BuildArea"] + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
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

        public static void InsertFlatUDS(DataTable argdt, int argCCId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            SqlDataReader dr;
            DataTable dt;
            conn = BsfGlobal.OpenCRMDB();
            int iFlatId = 0; int iFlatTypeId=0; decimal dArea = 0; decimal dFUSLandArea = 0; decimal dGuideLine = 0;
            decimal dFLandAmount = 0; decimal dFLandRate = 0; decimal dReg = 0;
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "Delete From BlockWiseUDS WHERE CostCentreId=" + argCCId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (argdt != null)
                    {
                        for (int i = 0; i < argdt.Rows.Count; i++)
                        {
                            sSql = "INSERT INTO BlockWiseUDS(CostCentreId,BlockId,LandArea,WithHeld,NetLandArea,FSIIndex,BuildArea)Values" +
                                "(" + argdt.Rows[i]["CostCentreId"] + "," + argdt.Rows[i]["BlockId"] + "," + argdt.Rows[i]["LandArea"] + "," +
                                " " + argdt.Rows[i]["WithHeld"] + "," + argdt.Rows[i]["NetLandArea"] + "," +
                                " " + argdt.Rows[i]["FSIIndex"] + "," + argdt.Rows[i]["BuildArea"] + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }

                        for (int i = 0; i < argdt.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(argdt.Rows[i]["Sel"]) == true)
                            {
                                sSql = "Select A.BlockId,A.FlatId,A.FlatTypeId,Area,A.BaseAmt,A.Guidelinevalue,B.Registration From FlatDetails A" +
                                        " Inner Join ProjectInfo B On A.CostCentreId=B.CostCentreId" +
                                        " Where A.CostCentreId=" + argCCId + " And FlatId Not In (Select B.FlatId From CheckListMaster A " +
                                        " Inner Join FlatChecklist B On A.CheckListId=B.CheckListId " +
                                        " Where TypeName='F' And CheckListName='Registration') And BlockId=" + argdt.Rows[i]["BlockId"] + "";
                                cmd = new SqlCommand(sSql, conn, tran);
                                dr = cmd.ExecuteReader();
                                dt = new DataTable();
                                dt.Load(dr);
                                cmd.Dispose();

                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    iFlatId = Convert.ToInt32(dt.Rows[j]["FlatId"]);
                                    iFlatTypeId = Convert.ToInt32(dt.Rows[j]["FlatTypeId"]);
                                    dArea = Convert.ToDecimal(dt.Rows[j]["Area"]);
                                    dFUSLandArea = decimal.Round((dArea / Convert.ToDecimal(CommFun.IsNullCheck(argdt.Rows[i]["BuildArea"], CommFun.datatypes.vartypenumeric))) * Convert.ToDecimal(argdt.Rows[i]["NetLandArea"]));
                                    dGuideLine = Convert.ToDecimal(dt.Rows[j]["Guidelinevalue"]);
                                    dFLandAmount = dFUSLandArea * dGuideLine;
                                    dFLandRate = dFUSLandArea * dGuideLine;
                                    dReg = Convert.ToDecimal(dt.Rows[j]["Registration"]);

                                    decimal dRegValue = dFUSLandArea * dReg * dGuideLine / 100;
                                    decimal dBAmt = Convert.ToDecimal(dt.Rows[j]["BaseAmt"]);
                                    UnitDirBL.UpdateRegistrationFlat(iFlatId, dRegValue);
                                    decimal dOAmt = UnitDirBL.GetOtherCostFlat(iFlatId);
                                    decimal dNetAmt = decimal.Round(dBAmt + dOAmt, 0);

                                    sSql = "Update FlatDetails Set Guidelinevalue=" + dGuideLine + ",USLand=" + dFUSLandArea + "," +
                                        " USLandAmt=" + dFLandAmount + ",LandRate=" + dFLandRate + "," +
                                        " OtherCostAmt=" + dOAmt + ",NetAmt=" + dNetAmt + " Where FlatId=" + iFlatId + "";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();

                                    PaymentScheduleDL.InsertFlatScheduleI(iFlatId, conn, tran);
                                }
                                dt.Dispose();
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

        public static void InsertProjectwiseFlatUDS(int argCCId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            SqlDataReader dr;
            DataTable dt;
            conn = BsfGlobal.OpenCRMDB();
            int iFlatId = 0; int iFlatTypeId = 0; decimal dArea = 0; decimal dFUSLandArea = 0; decimal dGuideLine = 0;
            decimal dFLandAmount = 0; decimal dFLandRate = 0; decimal dReg = 0;
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    //sSql = "Delete From BlockWiseUDS WHERE CostCentreId=" + argCCId + "";
                    //cmd = new SqlCommand(sSql, conn, tran);
                    //cmd.ExecuteNonQuery();
                    //cmd.Dispose();

                    //if (argdt != null)
                    //{
                    //    for (int i = 0; i < argdt.Rows.Count; i++)
                    //    {
                            //sSql = "INSERT INTO BlockWiseUDS(CostCentreId,BlockId,LandArea,WithHeld,NetLandArea,FSIIndex,BuildArea)Values" +
                            //    "(" + argdt.Rows[i]["CostCentreId"] + "," + argdt.Rows[i]["BlockId"] + "," + argdt.Rows[i]["LandArea"] + "," +
                            //    " " + argdt.Rows[i]["WithHeld"] + "," + argdt.Rows[i]["NetLandArea"] + "," +
                            //    " " + argdt.Rows[i]["FSIIndex"] + "," + argdt.Rows[i]["BuildArea"] + ")";
                            //cmd = new SqlCommand(sSql, conn, tran);
                            //cmd.ExecuteNonQuery();
                            //cmd.Dispose();
                        //}

                        //for (int i = 0; i < argdt.Rows.Count; i++)
                        //{
                        //    if (Convert.ToBoolean(argdt.Rows[i]["Sel"]) == true)
                        //    {
                    sSql = "Select A.BlockId,A.FlatId,A.FlatTypeId,Area,A.BaseAmt,A.Guidelinevalue,B.Registration,B.NetLandArea,B.BuildArea From FlatDetails A" +
                            " Inner Join ProjectInfo B On A.CostCentreId=B.CostCentreId" +
                            " Where A.CostCentreId=" + argCCId + " And FlatId Not In (Select B.FlatId From CheckListMaster A " +
                            " Inner Join FlatChecklist B On A.CheckListId=B.CheckListId " +
                            " Where TypeName='F' And CheckListName='Registration') And A.CostCentreId=" + argCCId + " ";
                                cmd = new SqlCommand(sSql, conn, tran);
                                dr = cmd.ExecuteReader();
                                dt = new DataTable();
                                dt.Load(dr);
                                cmd.Dispose();

                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    iFlatId = Convert.ToInt32(dt.Rows[j]["FlatId"]);
                                    iFlatTypeId = Convert.ToInt32(dt.Rows[j]["FlatTypeId"]);
                                    dArea = Convert.ToDecimal(dt.Rows[j]["Area"]);
                                    dFUSLandArea = decimal.Round((dArea / Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[j]["BuildArea"], CommFun.datatypes.vartypenumeric))) *
                                        Convert.ToDecimal(dt.Rows[j]["NetLandArea"]));
                                    dGuideLine = Convert.ToDecimal(dt.Rows[j]["Guidelinevalue"]);
                                    dFLandAmount = dFUSLandArea * dGuideLine;
                                    dFLandRate = dFUSLandArea * dGuideLine;
                                    dReg = Convert.ToDecimal(dt.Rows[j]["Registration"]);

                                    decimal dRegValue = dFUSLandArea * dReg * dGuideLine / 100;
                                    decimal dBAmt = Convert.ToDecimal(dt.Rows[j]["BaseAmt"]);
                                    UnitDirBL.UpdateRegistrationFlat(iFlatId, dRegValue);
                                    decimal dOAmt = UnitDirBL.GetOtherCostFlat(iFlatId);
                                    decimal dNetAmt = decimal.Round(dBAmt + dOAmt, 0);

                                    sSql = "Update FlatDetails Set Guidelinevalue=" + dGuideLine + ",USLand=" + dFUSLandArea + "," +
                                        " USLandAmt=" + dFLandAmount + ",LandRate=" + dFLandRate + "," +
                                        " OtherCostAmt=" + dOAmt + ",NetAmt=" + dNetAmt + " Where FlatId=" + iFlatId + "";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();

                                    PaymentScheduleDL.InsertFlatScheduleI(iFlatId, conn, tran);
                                }
                                dt.Dispose();
                        //    }
                        //}

                    //}

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

        public static DataTable GetBlockFlats(int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.LeadId,A.FlatId,A.FlatNo,Case When B.CustomerType='I' Then 'Investor' Else 'Buyer' End CustomerType,C.LeadName,B.BlockUpTo," +
                        " cast(Case When D.BlockingType='L' Then D.BlockingPenalty Else (D.BlockingPenalty*A.NetAmt/100) End as decimal(18,3)) PenaltyAmount ," +
                        " B.Remarks,Convert(bit,0,0) Sel from FlatDetails A" +
                        " Left Join BlockUnits B On A.CostCentreId=B.CostCentreId And A.FlatId=B.FlatId" +
                        " Left Join LeadRegister C On C.LeadId=A.LeadId" +
                        " Inner Join ProjectInfo D On D.CostCentreId=A.CostCentreId" +
                        " Where A.CostCentreId=" + argCCId + " And A.Status='B' And B.BlockType='B' And B.BlockUpTo<'" + string.Format(Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy")) + "'";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static bool GetUDS(int argCCId)
        {
            DataTable dt;
            SqlDataAdapter sda;
            String sSql;
            bool bAns=false;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select ProjectwiseUDS From ProjectInfo Where CostCentreId=" + argCCId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                { bAns = Convert.ToBoolean(dt.Rows[0]["ProjectwiseUDS"]); }
                dt.Dispose();
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return bAns;
        }

        public static void UpdateBlockFlats(DataTable argdt)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    if (argdt.Rows.Count > 0)
                    {
                        for (int i = 0; i < argdt.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(argdt.Rows[i]["Sel"]) == true)
                            {
                                sSql = "UPDATE BlockUnits SET LeadId=" + argdt.Rows[i]["LeadId"] + "," +
                                " BlockUpto='" + String.Format("{0:dd-MMM-yyyy}", argdt.Rows[i]["BlockUpto"]) + "'," +
                                " Date='" + String.Format(Convert.ToDateTime(DateTime.Now).ToString("MM-dd-yyyy")) + "'," +
                                " PenaltyAmount=" + argdt.Rows[i]["PenaltyAmount"] + ",BlockType='U',Remarks='" + argdt.Rows[i]["Remarks"] + "'" +
                                " WHERE FlatId=" + argdt.Rows[i]["FlatId"] + " And BlockType='B' ";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                sSql = String.Format("UPDATE FlatDetails set Status='U',LeadId=0 WHERE FlatId={0}", argdt.Rows[i]["FlatId"]);
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
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
    
        public static DataTable GetCarTagName(int argCCId, int argBlockId, int argTypeId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
                //sSql = "SELECT * From Slots Where CostCentreId=" + argCCId + " And BlockId=" + argBlockId + "" +
                //    " And TypeId=" + argTypeId + "";
                sSql = "SELECT A.CostCentreId,A.BlockId,A.TypeId,A.SlotNo,A.TagName,A.FlatId,B.FlatNo From Slots A"+
                        " Left Join FlatDetails B on A.FlatId=B.FlatId"+
                        " Where A.CostCentreId=" + argCCId + " And A.BlockId=" + argBlockId + " And A.TypeId=" + argTypeId + " Order By SlotNo";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable GetFloorRate(int argCCId,int argFlatTypeId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.LevelId,A.LevelName,Isnull(Cast(B.Rate as decimal(18,3)),0) Rate From LevelMaster A " +
                       "Left Join FloorRate B on A.LevelId=B.LevelId and B.FlatTypeId= " + argFlatTypeId + " " +
                       "Where A.CostCentreId= " + argCCId + " Order By A.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable GetFloorChangeRate(int argCCId, int argFlatTypeId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.CostCentreId,B.FlatTypeId,A.LevelId,A.LevelName,Isnull(B.Rate,0) OldRate,Isnull(B.Rate,0) NewRate From LevelMaster A " +
                       "Inner Join FloorRate B on A.LevelId=B.LevelId and B.FlatTypeId= " + argFlatTypeId + " " +
                       "Where A.CostCentreId= " + argCCId + " Order By A.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable GetFloorRate(int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.CostCentreId,B.FlatTypeId,A.LevelId,A.LevelName,Isnull(B.Rate,0) OldRate,Isnull(B.Rate,0) NewRate " +
                        " From LevelMaster A Inner Join FloorRate B on A.LevelId=B.LevelId " +
                        " Where B.FlatTypeId In(Select FlatTypeId From FlatType Where" +
                        " ProjId= " + argCCId + " And FloorwiseRate='Y') Order By B.FlatTypeId";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable GetCarCost(int argCCId, int argTypeId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();


            try
            {
                sSql = "SELECT Cost,AddCost FROM CarParkCost WHERE CostCentreId=" + argCCId + " AND TypeId=" + argTypeId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static void InsertCar(DataTable argdt,int argCCId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "DELETE FROM CarParkMaster WHERE CostCentreId=" + argCCId + " AND TypeId=" + CarBO.TId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "DELETE FROM CarParkCost WHERE CostCentreId=" + argCCId + " AND TypeId=" + CarBO.TId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (argdt != null)
                    {
                        for (int i = 0; i < argdt.Rows.Count; i++)
                        {
                            sSql = "INSERT INTO CarParkMaster(CostCentreId,BlockId,TypeId,NoOfSlots,AllottedSlots)VALUES(" + argdt.Rows[i]["CostCentreId"] + "," +
                            " " + argdt.Rows[i]["BlockId"] + "," + argdt.Rows[i]["TypeId"] + "," + argdt.Rows[i]["NoOfSlots"] + "," + argdt.Rows[i]["AllotSlots"] + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    sSql = "INSERT INTO CarParkCost (CostCentreId,TypeId,Cost,AddCost)VALUES(" + argCCId + "," +
                    " " + CarBO.TId + "," + CarBO.Cost1 + "," + CarBO.Cost2 + ")";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

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

        public static void InsertCarSlots(DataTable argdt, int argCCId,int argBlockId,int argTypeId,int argSlots)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "DELETE FROM dbo.CarParkMaster WHERE CostCentreId=" + argCCId + " And BlockId=" + argBlockId + " AND TypeId=" + argTypeId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "INSERT INTO dbo.CarParkMaster(CostCentreId,BlockId,TypeId,NoOfSlots)VALUES(" + argCCId + "," +
                            " " + argBlockId + "," + argTypeId + "," + argSlots + ")";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "DELETE FROM dbo.Slots WHERE CostCentreId=" + argCCId + " And BlockId=" + argBlockId + " AND TypeId=" + argTypeId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (argdt != null)
                    {
                        for (int i = 0; i < argdt.Rows.Count; i++)
                        {
                            sSql = "INSERT INTO dbo.Slots(CostCentreId,BlockId,TypeId,SlotNo,FlatId,TagName)VALUES(" + argdt.Rows[i]["CostCentreId"] + "," +
                            " " + argdt.Rows[i]["BlockId"] + "," + argdt.Rows[i]["TypeId"] + "," + argdt.Rows[i]["SlotNo"] + "," + argdt.Rows[i]["FlatId"] + ",'" + argdt.Rows[i]["TagName"] + "')";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
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

        public static void InsertCarParkSlots(int argCCId, int argBlockId, int argTypeId, int argSlots)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "INSERT INTO Slots(CostCentreId,BlockId,TypeId,SlotNo)VALUES(" + argCCId + "," +
                    " " + argBlockId + "," + argTypeId + "," + argSlots + ")";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

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

        public static void UpdateCarSlots(DataTable argdt, int argCCId, int argBlockId, int argTypeId, int argSlots)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "Update CarParkMaster Set NoOfSlots=" + argSlots + " Where CostCentreId=" + argCCId + " " +
                            " And BlockId=" + argBlockId + " And TypeId=" + argTypeId + " ";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (argdt != null)
                    {
                        for (int i = 0; i < argdt.Rows.Count; i++)
                        {
                            sSql = "Update Slots Set TagName='" + argdt.Rows[i]["TagName"] + "' Where CostCentreId=" + argdt.Rows[i]["CostCentreId"] + " " +
                            " And BlockId=" + argdt.Rows[i]["BlockId"] + " And TypeId=" + argdt.Rows[i]["TypeId"] + " And SlotNo=" + argdt.Rows[i]["SlotNo"] + "  ";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
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

        public static void DeleteCarSlots(int argCCId, int argBlockId, int argTypeId, int argSlots)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "Delete From Slots Where CostCentreId=" + argCCId + " " +
                            " And BlockId=" + argBlockId + " And TypeId=" + argTypeId + " And SlotNo=" + argSlots + " ";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

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

        public static void InsertFlatTypeCar(DataTable argdt)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    if (argdt != null)
                    {
                        if (argdt.Rows.Count > 0)
                        {
                            sSql = "DELETE FROM FlatTypeCarPark WHERE CostCentreId=" + argdt.Rows[0]["CCId"] + " AND FlatTypeId=" + argdt.Rows[0]["FlatTypeId"] + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            if (argdt != null)
                            {
                                for (int i = 0; i < argdt.Rows.Count; i++)
                                {
                                    sSql = "INSERT INTO FlatTypeCarPark (CostCentreId,FlatTypeId,TypeId,TotalCP)VALUES" +
                                            " (" + argdt.Rows[i]["CCId"] + "," + argdt.Rows[i]["FlatTypeId"] + "," +
                                            " " + argdt.Rows[i]["TypeId"] + "," + argdt.Rows[i]["TotalCP"] + ")";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                            }

                            SqlDataReader sdr;
                            DataTable dt; decimal dAmt = 0; decimal dTotAmt = 0;

                            sSql = "Select B.CostCentreId,B.FlatTypeId,A.TypeId,B.TotalCP,Cost,AddCost From CarParkTypeMaster A " +
                                    " Left Join FlatTypeCarPark B On A.TypeId=B.TypeId AND FlatTypeId=" + argdt.Rows[0]["FlatTypeId"] + " " +
                                    " Inner Join CarParkCost C On C.TypeId=A.TypeId And C.CostCentreId=B.CostCentreId" +
                                    " Where A.TypeId In (Select Distinct TypeId From CarParkMaster Where CostCentreId=" + argdt.Rows[0]["CCId"] + "" +
                                    " And NoOfSlots<>0) ";
                            cmd = new SqlCommand(sSql, conn, tran);
                            sdr = cmd.ExecuteReader();
                            dt = new DataTable();
                            dt.Load(sdr);
                            sdr.Close();
                            cmd.Dispose();

                            if (dt != null)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    if (Convert.ToInt32(dt.Rows[i]["TotalCP"]) >= 1)
                                    {
                                        dAmt = dAmt + Convert.ToDecimal(dt.Rows[i]["Cost"]);
                                        if (Convert.ToInt32(dt.Rows[i]["TotalCP"]) >= 1)
                                        {
                                            if (Convert.ToDecimal(dt.Rows[i]["AddCost"]) > 0)
                                            {
                                                dAmt = dAmt + (Convert.ToDecimal(dt.Rows[i]["AddCost"].ToString()) * (Convert.ToInt32(dt.Rows[i]["TotalCP"]) - 1));
                                            }
                                            else
                                            {
                                                dAmt = dAmt + (Convert.ToDecimal(dt.Rows[i]["Cost"].ToString()) * (Convert.ToInt32(dt.Rows[i]["TotalCP"]) - 1));
                                            }
                                        }
                                    }
                                } 
                                dTotAmt = dTotAmt + dAmt;
                            }

                            sSql = "Update FlatTypeOtherCost set Amount= " + dTotAmt + " " +
                                   "Where FlatTypeId = " + argdt.Rows[0]["FlatTypeId"] + " and OtherCostId = (Select OtherCostId from OtherCostMaster Where OtherCostname ='CarParking')";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            tran.Commit();
                        }
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

        public static void UpdateRegistrationFlatType(int argFlatTypeId, decimal argAmt)
        {
            string sSql = "Update FlatTypeOtherCost set Amount= " + argAmt + " " +
                           "Where FlatTypeId = " + argFlatTypeId + " and OtherCostId = (Select OtherCostId from OtherCostMaster Where OtherCostname ='Registration')";
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = new SqlCommand(sSql,BsfGlobal.g_CRMDB);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            BsfGlobal.g_CRMDB.Close();
        }

        public static void UpdateRegistrationFlat(int argFlatId, decimal argAmt)
        {
            string sSql = "Update FlatOtherCost set Amount= " + argAmt + " " +
                           "Where FlatId = " + argFlatId + " and OtherCostId = (Select OtherCostId from OtherCostMaster Where OtherCostname ='Registration')";
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            BsfGlobal.g_CRMDB.Close();
        }

        public static decimal GetOtherCostflatType(int argflatTypeID)
        {
            decimal dAmt = 0;

            string sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount From FlatTypeOtherCost " +
                          "Where FlatTypeId= " + argflatTypeID;
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            sda.Dispose();
            if (dt.Rows.Count > 0)
            {
                dAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
            }
            dt.Dispose();
            BsfGlobal.g_CRMDB.Close();
            return dAmt;
        }

        public static decimal GetOtherCostFlat(int argFlatID)
        {
            decimal dAmt = 0;

            string sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount From FlatOtherCost " +
                          "Where FlatId= " + argFlatID;
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            sda.Dispose();
            if (dt.Rows.Count > 0)
            {
                dAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
            }
            dt.Dispose();
            BsfGlobal.g_CRMDB.Close();
            return dAmt;
        }

        public static void InsertFlatCar(DataTable argdt)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    if (argdt != null)
                    {
                        if (argdt.Rows.Count > 0)
                        {
                            sSql = "DELETE FROM FlatCarPark WHERE FlatId=" + argdt.Rows[0]["FlatId"] + " And CostCentreId=" + argdt.Rows[0]["CCId"] + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            if (argdt != null)
                            {
                                for (int i = 0; i < argdt.Rows.Count; i++)
                                {
                                    sSql = "INSERT INTO FlatCarPark (CostCentreId,FlatId,TypeId,TotalCP)VALUES" +
                                            " (" + argdt.Rows[i]["CCId"] + "," + argdt.Rows[i]["FlatId"] + "," +
                                            " " + argdt.Rows[i]["TypeId"] + "," + argdt.Rows[i]["TotalCP"] + ")";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                            }

                            SqlDataReader sdr;
                            DataTable dt; decimal dAmt = 0; decimal dTotAmt = 0;

                            sSql = "Select B.CostCentreId,B.FlatId,A.TypeId,B.TotalCP,Cost,AddCost From CarParkTypeMaster A " +
                                    " Left Join FlatCarPark B On A.TypeId=B.TypeId AND FlatId=" + argdt.Rows[0]["FlatId"] + "" +
                                    " Inner Join CarParkCost C On C.TypeId=A.TypeId And C.CostCentreId=B.CostCentreId" +
                                    " Where A.TypeId In (Select Distinct TypeId From CarParkMaster Where CostCentreId=" + argdt.Rows[0]["CCId"] + "" +
                                    " And NoOfSlots<>0) ";
                            dt = new DataTable(); cmd = new SqlCommand(sSql, conn, tran);
                            sdr = cmd.ExecuteReader();
                            dt.Load(sdr); cmd.Dispose(); dt.Dispose();

                            if (dt != null)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    if (Convert.ToInt32(dt.Rows[i]["TotalCP"]) >= 1)
                                    {
                                        dAmt = dAmt + Convert.ToDecimal(dt.Rows[i]["Cost"]);
                                        if (Convert.ToInt32(dt.Rows[i]["TotalCP"]) > 1)
                                        {
                                            if (Convert.ToDecimal(dt.Rows[i]["AddCost"]) > 0)
                                            {
                                                dAmt = dAmt + (Convert.ToDecimal(dt.Rows[i]["AddCost"].ToString()) * (Convert.ToInt32(dt.Rows[i]["TotalCP"]) - 1));
                                            }
                                            else
                                            {
                                                dAmt = dAmt + (Convert.ToDecimal(dt.Rows[i]["Cost"].ToString()) * (Convert.ToInt32(dt.Rows[i]["TotalCP"]) - 1));
                                            }
                                        }
                                    }

                                } dTotAmt = dTotAmt + dAmt;
                            }

                            #region coding
                            //decimal dOpenAmt = 0, dClosedAmt = 0, dTerraceAmt = 0, dTotalAmt = 0;
                            //SqlDataReader sdr;
                            //DataTable dt;

                            //if (FlatCarBO.Open >= 1)
                            //{
                            //    sSql = "Select Cost,AddCost from CarParkCost WHERE TypeId=1 and CostCentreId=" + FlatCarBO.CostCentreId;
                            //    dt = new DataTable();

                            //    cmd = new SqlCommand(sSql, conn, tran);
                            //    dt = new DataTable();
                            //    sdr = cmd.ExecuteReader();
                            //    dt.Load(sdr);
                            //    sdr.Close();
                            //    cmd.Dispose();

                            //    if (dt.Rows.Count > 0)
                            //    {
                            //        dOpenAmt = Convert.ToDecimal(dt.Rows[0]["Cost"].ToString());

                            //        if (FlatCarBO.Open > 1)
                            //        {
                            //            if (Convert.ToDecimal(dt.Rows[0]["AddCost"].ToString()) > 0)
                            //            {

                            //                dOpenAmt = dOpenAmt + (Convert.ToDecimal(dt.Rows[0]["AddCost"].ToString()) * (FlatCarBO.Open - 1));
                            //            }
                            //            else
                            //            {
                            //                dOpenAmt = dOpenAmt + (Convert.ToDecimal(dt.Rows[0]["Cost"].ToString()) * (FlatCarBO.Open - 1));
                            //            }
                            //        }

                            //    }
                            //    dt.Dispose();
                            //}


                            //if (FlatCarBO.Closed >= 1)
                            //{
                            //    sSql = "Select Cost,AddCost from CarParkCost WHERE TypeId=2 and CostCentreId=" + FlatCarBO.CostCentreId;
                            //    cmd = new SqlCommand(sSql, conn, tran);
                            //    dt = new DataTable();
                            //    sdr = cmd.ExecuteReader();
                            //    dt.Load(sdr);
                            //    sdr.Close();
                            //    cmd.Dispose();

                            //    if (dt.Rows.Count > 0)
                            //    {
                            //        dClosedAmt = Convert.ToDecimal(dt.Rows[0]["Cost"].ToString());

                            //        if (FlatCarBO.Closed > 1)
                            //        {
                            //            if (Convert.ToDecimal(dt.Rows[0]["AddCost"].ToString()) > 0)
                            //            {

                            //                dClosedAmt = dClosedAmt + (Convert.ToDecimal(dt.Rows[0]["AddCost"].ToString()) * (FlatCarBO.Closed - 1));
                            //            }
                            //            else
                            //            {
                            //                dClosedAmt = dClosedAmt + (Convert.ToDecimal(dt.Rows[0]["Cost"].ToString()) * (FlatCarBO.Closed - 1));
                            //            }
                            //        }

                            //    }
                            //    dt.Dispose();

                            //}

                            //if (FlatCarBO.Terrace >= 1)
                            //{
                            //    sSql = "Select Cost,AddCost from CarParkCost WHERE TypeId=3 and CostCentreId=" + FlatCarBO.CostCentreId;
                            //    cmd = new SqlCommand(sSql, conn, tran);
                            //    dt = new DataTable();
                            //    sdr = cmd.ExecuteReader();
                            //    dt.Load(sdr);
                            //    sdr.Close();
                            //    cmd.Dispose();

                            //    if (dt.Rows.Count > 0)
                            //    {
                            //        dTerraceAmt = Convert.ToDecimal(dt.Rows[0]["Cost"].ToString());

                            //        if (FlatCarBO.Terrace > 1)
                            //        {
                            //            if (Convert.ToDecimal(dt.Rows[0]["AddCost"].ToString()) > 0)
                            //            {

                            //                dTerraceAmt = dTerraceAmt + (Convert.ToDecimal(dt.Rows[0]["AddCost"].ToString()) * (FlatCarBO.Terrace - 1));
                            //            }
                            //            else
                            //            {
                            //                dTerraceAmt = dTerraceAmt + (Convert.ToDecimal(dt.Rows[0]["Cost"].ToString()) * (FlatCarBO.Terrace - 1));
                            //            }
                            //        }

                            //    }
                            //    dt.Dispose();
                            //}

                            //dTotalAmt = dOpenAmt + dClosedAmt + dTerraceAmt;
                            #endregion

                            sSql = "Update FlatOtherCost set Amount= " + dTotAmt + " " +
                                   "Where FlatId = " + argdt.Rows[0]["FlatId"] + " and OtherCostId = (Select OtherCostId from OtherCostMaster Where OtherCostname ='CarParking')";
                            cmd = new SqlCommand(sSql, conn, tran);
                            int icmd = cmd.ExecuteNonQuery();
                            cmd.Dispose();


                            if (dTotAmt > 0)
                            {
                                if (icmd == 0)
                                {
                                    int iOtherId = 0;

                                    sSql = "Select OtherCostId from OtherCostMaster Where OtherCostname ='CarParking'";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    dt = new DataTable();
                                    sdr = cmd.ExecuteReader();
                                    dt.Load(sdr);
                                    cmd.Dispose();
                                    if (dt.Rows.Count > 0) { iOtherId = Convert.ToInt32(dt.Rows[0]["OtherCostId"].ToString()); }
                                    dt.Dispose();

                                    sSql = "Insert into FlatOtherCost(FlatId,OtherCostId,Area,Rate,Flag,Amount) " +
                                          "Values(" + argdt.Rows[0]["FlatId"] + "," + iOtherId + ",0,0,'+'," + dTotAmt + ")";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }

                            }

                            tran.Commit();
                        }
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

        public static DataTable GetFlatTypeCarPark(int argCCId, int argFlatTypeId, int argBlockId)
        {
            SqlDataAdapter sda;
            string sSql = "";
            DataTable dtAO = new DataTable();

            try
            {
                BsfGlobal.OpenCRMDB();

                //sSql = " SELECT * FROM FlatTypeCarPark WHERE CostCentreId=" + argCCId + " AND FlatTypeId=" + argFlatTypeId + "";

                sSql = "Select B.CostCentreId,B.FlatTypeId,A.TypeId,A.TypeName,B.TotalCP From CarParkTypeMaster A" +
                        " Left Join FlatTypeCarPark B On A.TypeId=B.TypeId AND FlatTypeId=" + argFlatTypeId + " Where A.TypeId In" +
                        " (Select Distinct TypeId From CarParkMaster Where CostCentreId=" + argCCId + " And NoOfSlots<>0)";

                // Parama Commented on 13/06/2013
                //sSql = " SELECT A.CostCentreId,FlatTypeId=ISNULL(B.FlatTypeId," + argFlatTypeId + "),A.TypeId,C.TypeName,A.NoOfSlots," +
                //       " AllottedSlots=ISNULL((A.AllottedSlots-B.TotalCP),0),TotalCP=ISNULL(B.TotalCP,0) FROM CarParkMaster A" +
                //       " LEFT JOIN FlatTypeCarPark B On A.TypeId=B.TypeId And A.CostCentreId=B.CostCentreId " +
                //       " INNER JOIN CarParkTypeMaster C On A.TypeId=C.TypeId" +
                //       " WHERE A.TypeId In(Select Distinct TypeId From CarParkMaster Where CostCentreId=" + argCCId + " And NoOfSlots<>0) AND A.CostCentreId=" + argCCId +
                //       " GROUP BY A.CostCentreId,B.FlatTypeId,A.TypeId,C.TypeName,A.NoOfSlots,A.AllottedSlots,B.TotalCP";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dtAO = new DataTable();
                sda.Fill(dtAO);
                sda.Dispose();
            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtAO;
        }

        public static DataTable GetFlatCarPark(int argCCId, int argFlatId, int argBlockId)
        {
            DataTable dtAO = new DataTable();
            try
            {
                BsfGlobal.OpenCRMDB();

                //sSql = " SELECT * FROM FlatCarPark WHERE CostCentreId=" + argCCId + " AND FlatId=" + argFlatId + "";

                // Parama Commented on  12/06/2013
                //sSql = "Select B.CostCentreId,B.FlatId,A.TypeId,A.TypeName,B.TotalCP From CarParkTypeMaster A " +
                //        " Left Join FlatCarPark B On A.TypeId=B.TypeId AND FlatId=" + argFlatId +
                //        " LEFT JOIN CarParkMaster C ON A.TypeId=C.TypeId AND BlockId=" + argBlockId +
                //        " Where A.TypeId IN(Select Distinct TypeId From CarParkMaster Where CostCentreId=" + argCCId + " And NoOfSlots<>0)";

                string sSql = " SELECT A.CostCentreId,B.FlatId,A.TypeId,C.TypeName,A.NoOfSlots,ISNULL((A.AllottedSlots-B.TotalCP),0) AllottedSlots,ISNULL(B.TotalCP,0) TotalCP FROM CarParkMaster A" +
                              " LEFT JOIN FlatCarPark B On A.TypeId=B.TypeId And A.CostCentreId=B.CostCentreId " +
                              " INNER JOIN CarParkTypeMaster C On C.TypeId=B.TypeId" +
                              " WHERE A.TypeId In(Select Distinct TypeId From CarParkMaster Where CostCentreId=" + argCCId + " And NoOfSlots<>0) And" +
                              " A.BlockId=" + argBlockId + " AND A.CostCentreId=" + argCCId + " AND B.FlatId=" + argFlatId +
                              " GROUP BY A.CostCentreId,B.FlatId,A.TypeId,C.TypeName,A.NoOfSlots,A.AllottedSlots,B.TotalCP" +
                              " UNION ALL " +
                              " SELECT A.CostCentreId," + argFlatId + " FlatId,A.TypeId,C.TypeName,A.NoOfSlots,(A.AllottedSlots-0)AllottedSlots,0 TotalCP FROM CarParkMaster A" +
                              " INNER JOIN CarParkTypeMaster C On C.TypeId=A.TypeId" +
                              " WHERE A.TypeId NOT IN(Select Distinct TypeId From FlatCarPark Where CostCentreId=" + argCCId + " And FlatId=" + argFlatId + ") And" +
                              " A.BlockId=" + argBlockId + " AND A.CostCentreId=" + argCCId +
                              " GROUP BY A.CostCentreId,A.TypeId,C.TypeName,A.NoOfSlots,A.AllottedSlots";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dtAO);
                sda.Dispose();
            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtAO;
        }

        public static void InsertFloorRate(DataTable argdt, int argFlatTypeId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "DELETE FROM FloorRate WHERE FlatTypeId=" + argFlatTypeId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                   
                    for (int i = 0; i < argdt.Rows.Count; i++)
                    {
                        sSql = "INSERT INTO FloorRate(FlatTypeId,LevelId,Rate) VALUES(" + argFlatTypeId + "," + argdt.Rows[i]["LevelId"] + "," +
                               "" + argdt.Rows[i]["Rate"] + ")";
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

        public static DataTable GetApplyOthers(int argCCId)
        {
            SqlDataAdapter sda;
            string sSql = "";
            DataTable dtAO = new DataTable();

            try
            {
                BsfGlobal.OpenCRMDB();

                sSql = " Select A.FlatId, A.FlatNo,B.BlockName,CONVERT(bit,0,1) Status,0 Executive,'' CompletionDate,'' Remarks from FlatDetails A " +
                      " Inner Join BlockMaster B On A.BlockId=B.BlockId Where CostCentreId="+ argCCId +" ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dtAO);
                sda.Dispose();
            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtAO;
        }

        public static void ApplyOthersTransaction(DataTable argDt, int argChkId,string argExpDate)
        {
            SqlCommand cmd;
            string sSql = "";
            int FlatId = 0;
            DataTable dtAO=new DataTable();
            string ExpDate = "";
            string CompDate = "";
            BsfGlobal.OpenCRMDB();
            SqlTransaction transaction = null;
            transaction = BsfGlobal.g_CRMDB.BeginTransaction();
            try
            {
                
                DataView dv=new DataView(argDt);
                dv.RowFilter="Status=True";
                dtAO = dv.ToTable();
                for (int i = 0; i < dtAO.Rows.Count; i++)
                {
                    FlatId = Convert.ToInt32(dtAO.Rows[i]["FlatId"]);
                    sSql = "Delete From FlatCheckList Where FlatId="+ FlatId +" ";
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB, transaction);
                    cmd.ExecuteNonQuery();
                    if (argExpDate == "") ExpDate = ""; else ExpDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(argExpDate));
                    if (dtAO.Rows[i]["CompletionDate"].ToString() == "") CompDate = ""; else CompDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(dtAO.Rows[i]["CompletionDate"]));
                    sSql = "Insert Into FlatCheckList (CheckListId,FlatId,ExpCompletionDate,CompletionDate,ExecutiveId,Status,Remarks) " +
                          "Values (" + argChkId + "," + FlatId + ",'" + ExpDate + "','" + CompDate + "','" + dtAO.Rows[i]["Executive"].ToString() + "',1,'" + dtAO.Rows[i]["Remarks"].ToString() + "') ";
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB,transaction);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    
                }
                transaction.Commit();
            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }

        }

        internal static bool BrokerFound(int argBrokerId)
        {
            bool bans = false;
            try
            {
                DataTable dt;
                string sSql = "Select BrokerId from BuyerDetail Where BrokerId = " + argBrokerId;
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                if (dt.Rows.Count > 0) { bans = true; }
                dt.Dispose();
            }
            catch (Exception ce)
            {

                throw ce;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }

            return bans;
        }

        public static DataTable GetCarAllot(int argBlockId, int argCCId,int argFlatId)
        {
            DataTable dt = new DataTable();
            string sSql = " SELECT A.TypeId,C.TypeName,A.NoOfSlots,A.AllottedSlots,B.TotalCP FROM CarParkMaster A" +
                            " Inner Join FlatCarPark B On A.TypeId=B.TypeId And A.CostCentreId=B.CostCentreId "+
                            " Inner Join CarParkTypeMaster C On C.TypeId=B.TypeId"+
                            " WHERE A.TypeId In(Select Distinct TypeId From CarParkMaster Where CostCentreId=" + argCCId + " And NoOfSlots<>0) And" +
                            " BlockId=" + argBlockId + " AND A.CostCentreId=" + argCCId + " And FlatId=" + argFlatId + "";
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            dr.Close();
            cmd.Dispose();
            BsfGlobal.g_CRMDB.Close();
            return dt;
        }

        public static DataTable GetCarParkAllot(int argBlockId,int argCCId)
        {
            DataTable dt = new DataTable();
            //string sSql = "SELECT TypeId,NoOfSlots,AllottedSlots FROM CarParkMaster WHERE BlockId=" + argBlockId + " AND CostCentreId=" + argCCId;
            string sSql = "SELECT TypeId,NoOfSlots,AllottedSlots FROM CarParkMaster WHERE TypeId In " +
                          " (Select Distinct TypeId From CarParkMaster Where CostCentreId=" + argCCId + " And NoOfSlots<>0) And" +
                          " BlockId=" + argBlockId + " AND CostCentreId=" + argCCId + "";
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            dr.Close();
            cmd.Dispose();
            BsfGlobal.g_CRMDB.Close();
            return dt;
        }

        public static DataTable GetCarParkAllotFlat(int argFlatId)
        {
            DataTable dt = new DataTable();
            //string sSql = "Select OpenCP,ClosedCP,TerraceCP from FlatCarPark Where FlatId= " + argFlatId;
            string sSql = " Select TotalCP from FlatCarPark Where FlatId=" + argFlatId;
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            dr.Close();
            cmd.Dispose();
            BsfGlobal.g_CRMDB.Close();
            return dt;
        }

        public static void InsertFlatCar(int argFlatId,int CCId,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmd;

            string sSql = "";
            //decimal dOpenAmt = 0, dClosedAmt = 0, dTerraceAmt = 0, dTotalAmt = 0;
            //SqlDataReader sdr;
            //DataTable dt;

            SqlDataReader sdr;
            DataTable dt; decimal dAmt = 0; decimal dTotAmt = 0;

            sSql = "Select B.CostCentreId,B.FlatId,A.TypeId,B.TotalCP,Cost,AddCost From dbo.CarParkTypeMaster A " +
                    " Left Join dbo.FlatCarPark B On A.TypeId=B.TypeId AND FlatId=" + argFlatId + "" +
                    " Inner Join dbo.CarParkCost C On C.TypeId=A.TypeId And C.CostCentreId=B.CostCentreId" +
                    " Where A.TypeId In (Select Distinct TypeId From dbo.CarParkMaster Where CostCentreId=" + CCId + "" +
                    " And NoOfSlots<>0) ";
            dt = new DataTable(); 
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt.Load(sdr); cmd.Dispose(); dt.Dispose();

            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dt.Rows[i]["TotalCP"]) >= 1)
                    {
                        dAmt = Convert.ToDecimal(dt.Rows[i]["Cost"]);
                        if (Convert.ToInt32(dt.Rows[i]["TotalCP"]) > 1)
                        {
                            if (Convert.ToDecimal(dt.Rows[i]["AddCost"]) > 0)
                            {
                                dAmt = dAmt + (Convert.ToDecimal(dt.Rows[i]["AddCost"].ToString()) * (Convert.ToInt32(dt.Rows[i]["TotalCP"]) - 1));
                            }
                            else
                            {
                                dAmt = dAmt + (Convert.ToDecimal(dt.Rows[i]["Cost"].ToString()) * (Convert.ToInt32(dt.Rows[i]["TotalCP"]) - 1));
                            }
                        }
                    }
                    if (Convert.ToInt32(dt.Rows[i]["TotalCP"]) >= 1)
                    {
                        dTotAmt = dTotAmt + dAmt;
                    }
                }
            }

            #region coding
            //if (iOpen >= 1)
            //{
            //    sSql = "Select Cost,AddCost from CarParkCost WHERE TypeId=1 and CostCentreId=" + CCId;
            //    dt = new DataTable();

            //    cmd = new SqlCommand(sSql, conn, tran);
            //    dt = new DataTable();
            //    sdr = cmd.ExecuteReader();
            //    dt.Load(sdr);
            //    sdr.Close();
            //    cmd.Dispose();

            //    if (dt.Rows.Count > 0)
            //    {
            //        dOpenAmt = Convert.ToDecimal(dt.Rows[0]["Cost"].ToString());

            //        if (iOpen > 1)
            //        {
            //            if (Convert.ToDecimal(dt.Rows[0]["AddCost"].ToString()) > 0)
            //            {

            //                dOpenAmt = dOpenAmt + (Convert.ToDecimal(dt.Rows[0]["AddCost"].ToString()) * (iOpen - 1));
            //            }
            //            else
            //            {
            //                dOpenAmt = dOpenAmt + (Convert.ToDecimal(dt.Rows[0]["Cost"].ToString()) * (iOpen - 1));
            //            }
            //        }

            //    }
            //    dt.Dispose();
            //}


            //if (iClose >= 1)
            //{
            //    sSql = "Select Cost,AddCost from CarParkCost WHERE TypeId=2 and CostCentreId=" + CCId;
            //    cmd = new SqlCommand(sSql, conn, tran);
            //    dt = new DataTable();
            //    sdr = cmd.ExecuteReader();
            //    dt.Load(sdr);
            //    sdr.Close();
            //    cmd.Dispose();


            //    if (dt.Rows.Count > 0)
            //    {
            //        dClosedAmt = Convert.ToDecimal(dt.Rows[0]["Cost"].ToString());

            //        if (iClose > 1)
            //        {
            //            if (Convert.ToDecimal(dt.Rows[0]["AddCost"].ToString()) > 0)
            //            {

            //                dClosedAmt = dClosedAmt + (Convert.ToDecimal(dt.Rows[0]["AddCost"].ToString()) * (iClose - 1));
            //            }
            //            else
            //            {
            //                dClosedAmt = dClosedAmt + (Convert.ToDecimal(dt.Rows[0]["Cost"].ToString()) * (iClose - 1));
            //            }
            //        }

            //    }
            //    dt.Dispose();
            //}

            //if (ITerrace >= 1)
            //{
            //    sSql = "Select Cost,AddCost from CarParkCost WHERE TypeId=3 and CostCentreId=" + CCId;
            //    cmd = new SqlCommand(sSql, conn, tran);
            //    dt = new DataTable();
            //    sdr = cmd.ExecuteReader();
            //    dt.Load(sdr);
            //    sdr.Close();
            //    cmd.Dispose();

            //    if (dt.Rows.Count > 0)
            //    {
            //        dTerraceAmt = Convert.ToDecimal(dt.Rows[0]["Cost"].ToString());

            //        if (ITerrace > 1)
            //        {
            //            if (Convert.ToDecimal(dt.Rows[0]["AddCost"].ToString()) > 0)
            //            {

            //                dTerraceAmt = dTerraceAmt + (Convert.ToDecimal(dt.Rows[0]["AddCost"].ToString()) * (ITerrace - 1));
            //            }
            //            else
            //            {
            //                dTerraceAmt = dTerraceAmt + (Convert.ToDecimal(dt.Rows[0]["Cost"].ToString()) * (ITerrace - 1));
            //            }
            //        }

            //    }
            //    dt.Dispose();
            //}

            //dTotalAmt = dOpenAmt + dClosedAmt + dTerraceAmt;
            #endregion

            sSql = "Update FlatOtherCost set Amount= " + dTotAmt + " " +
                    "Where FlatId = " + argFlatId + " and OtherCostId = (Select OtherCostId from OtherCostMaster Where OtherCostname ='CarParking')";
            cmd = new SqlCommand(sSql, conn, tran);
            int icmd= cmd.ExecuteNonQuery();
            cmd.Dispose();

            if (dTotAmt > 0)
            {
                if (icmd == 0)
                {
                    int iOtherId = 0;

                    sSql = "Select OtherCostId from OtherCostMaster Where OtherCostname ='CarParking'";
                    cmd = new SqlCommand(sSql, conn, tran);
                    dt = new DataTable();
                    sdr = cmd.ExecuteReader();
                    dt.Load(sdr);
                    cmd.Dispose();
                    if (dt.Rows.Count > 0) { iOtherId = Convert.ToInt32(dt.Rows[0]["OtherCostId"].ToString()); }
                    dt.Dispose();

                    sSql = "Insert into FlatOtherCost(FlatId,OtherCostId,Area,Rate,Flag,Amount) " +
                          "Values(" + argFlatId + "," + iOtherId + ",0,0,'+'," + dTotAmt + ")";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                }

            }

            sSql = "Select SUM(Amount)Amount from FlatOtherCost Where FlatId=" + argFlatId + "";
            cmd = new SqlCommand(sSql, conn, tran);
            decimal dAmount = Convert.ToDecimal(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
            cmd.Dispose();

            int iCCId = 0, iPayTypeId = 0; decimal dBaseAmt = 0;
            sSql = "Select BaseAmt,PayTypeId,CostCentreId FROM dbo.FlatDetails Where FlatId=" + argFlatId + "";
            cmd = new SqlCommand(sSql, conn, tran);
            DataTable dtOC = new DataTable();
            sdr = cmd.ExecuteReader();
            dtOC.Load(sdr);
            if (dtOC.Rows.Count > 0)
            {
                iCCId = Convert.ToInt32(CommFun.IsNullCheck(dtOC.Rows[0]["CostCentreId"], CommFun.datatypes.vartypenumeric));
                iPayTypeId = Convert.ToInt32(CommFun.IsNullCheck(dtOC.Rows[0]["PayTypeId"], CommFun.datatypes.vartypenumeric));
                dBaseAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtOC.Rows[0]["BaseAmt"], CommFun.datatypes.vartypenumeric));
            }
            //decimal dBaseAmt = Convert.ToDecimal(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
            cmd.Dispose();

            //sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
            //        " Where FlatId=" + argFlatId + " and OtherCostId in " +
            //        " (Select OtherCostId FROM dbo.OtherCostSetupTrans Where PayTypeId=" + iPayTypeId + " and CostCentreId=" + iCCId + ")";
            sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                    " Where FlatId =" + argFlatId + " and OtherCostId not in (Select OtherCostId from dbo.OXGross " +
                    " Where CostCentreId=" + iCCId + ")";
            cmd = new SqlCommand(sSql, conn, tran);
            decimal dOCost = Convert.ToDecimal(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
            cmd.Dispose();

            decimal dNetAmt = dOCost + dBaseAmt;

            sSql = "Update dbo.FlatDetails Set OtherCostAmt=" + dAmount + ",NetAmt=" + dNetAmt + " Where FlatId=" + argFlatId + "";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        public static DataTable GetBlock(int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = " Select BlockId,BlockName From dbo.BlockMaster Where CostCentreId=" + argCCId + " Order By SortOrder";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        public static DataTable GetSlots(int argCCId,int argBlockId,int argTypeId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Select A.FlatId, ISNULL(A.TagName, '') TagName, ISNULL(C.BlockName, '') BlockName, ISNULL(B.FlatNo, '') FlatNo, " +
                              " ISNULL(D.NoOfSlots, 0) CPPermitNo, ISNULL(E.TotalCP, 0) CarPark, ISNULL(A.SlotNo, '') SlotNo from dbo.Slots A " +
                              " Left Join dbo.FlatDetails B on A.FlatId=B.FlatId" +
                              " Left Join dbo.BlockMaster C on A.BlockId=C.BlockId" +
                              " Left Join dbo.CarParkMaster D on A.BlockId=D.BlockId AND A.TypeId=D.TypeId" +
                              " Left Join dbo.FlatCarPark E on A.FlatId=E.FlatId AND A.TypeId=E.TypeId" +
                              " Where A.CostCentreId=" + argCCId + " And A.BlockId=" + argBlockId + " And A.TypeId=" + argTypeId +
                              " Order By SlotNo";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        public static void UpdateFlatSeletion(int argCCId, int argBlockId, int argTypeId, int argSlotNo, int argOldFlatId, int argNewFlatId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    string sSql = "Update dbo.Slots Set FlatId=" + argNewFlatId + " WHERE CostCentreId=" + argCCId + " " +
                                  " And BlockId=" + argBlockId + " And TypeId=" + argTypeId + " And SlotNo=" + argSlotNo + "";
                    SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (argNewFlatId == 0)
                        sSql = "Update dbo.FlatCarPark Set TotalCP=0 WHERE CostCentreId=" + argCCId + " AND TypeId=" + argTypeId + " AND FlatId=" + argOldFlatId + "";
                    else
                        sSql = "Update dbo.FlatCarPark Set TotalCP=1 WHERE CostCentreId=" + argCCId + " AND TypeId=" + argTypeId + " AND FlatId=" + argNewFlatId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    int iCount = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteNonQuery(), CommFun.datatypes.vartypenumeric));
                    cmd.Dispose();

                    if (iCount == 0 && argNewFlatId != 0)
                    {
                        sSql = "Insert into dbo.FlatCarPark(CostCentreId, FlatId, TypeId, TotalCP) Values(" + argCCId + ", " + argNewFlatId + ", " + argTypeId + ", 1)";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    if (argNewFlatId == 0)
                        sSql = "Update dbo.FlatDetails Set TotalCarPark=TotalCarPark-1 WHERE CostCentreId=" + argCCId + " AND FlatId=" + argOldFlatId + "";
                    else
                        sSql = "Update dbo.FlatDetails Set TotalCarPark=TotalCarPark+1 WHERE CostCentreId=" + argCCId + " AND FlatId=" + argNewFlatId + "";

                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

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

        public static void AllotmentCancel(int argFlatId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd; SqlDataReader dr; DataTable dt;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "SELECT * FROM PaymentScheduleFlat WHERE FlatId=" + argFlatId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    dr=cmd.ExecuteReader();
                    dt=new DataTable();
                    dt.Load(dr);
                    cmd.Dispose();
                    if(dt.Rows.Count>0)
                    {}
                    else
                    {}

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

        public static DataTable GetFacing(int argCCId)
        {
            DataTable dt = new DataTable();
            string sSql = " Select FacingId,Description From dbo.Facing Where CostCentreId=" + argCCId + " Order By Description";
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            dr.Close();
            cmd.Dispose();
            BsfGlobal.g_CRMDB.Close();
            return dt;
        }

        public static int InsertFacing(string argDesc,int argCCId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB(); int iId = 0;
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "INSERT INTO Facing (Description,CostCentreId) Values ('" + argDesc + "'," + argCCId + ")SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(sSql, conn, tran);
                    iId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();

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
            return iId;
        }

        public static void UpdateFacing(string argDesc, int argFacId,int argCCId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "Update Facing Set Description='" + argDesc + "' Where FacingId=" + argFacId + " And CostCentreId=" + argCCId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

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

        public static bool FacingFound(int argFacId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();bool bAns=false;SqlDataReader dr;
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = " Select FacingId From FlatDetails Where FacingId= " + argFacId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    dr=cmd.ExecuteReader();
                    DataTable dt=new DataTable();
                    dt.Load(dr);
                    cmd.Dispose();
                    if(dt.Rows.Count>0)
                    {
                        bAns=true;
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
            return bAns;
        }

        public static void DeleteFacing(int argCCId, int argFacId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "Delete Facing Where FacingId=" + argFacId + " And CostCentreId=" + argCCId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

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

        public static void InsertLandRateChange(bool argGLV, bool argMLV, bool argReg,int argBlockId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            decimal dFTUSLandArea = 0; decimal dFTLandRate = 0; decimal dFTLandAmount = 0; decimal dRegValue = 0;
            int iFlatTypeId = 0; decimal dOAmt = 0; decimal dFTNetAmount = 0; decimal dFTBaseAmt = 0;

            decimal dFUSLandArea = 0; decimal dFLandRate = 0; decimal dFLandAmount = 0; decimal dFRegValue = 0;
            int iFlatId = 0; decimal dFOAmt = 0; decimal dFNetAmount = 0; decimal dFBaseAmt = 0;

            SqlDataReader dr; DataTable dt;
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    if (LandRateChangeBO.OldGuideValue != LandRateChangeBO.NewGuideValue || LandRateChangeBO.OldMarketValue != LandRateChangeBO.NewMarketValue || LandRateChangeBO.OldRegistration != LandRateChangeBO.NewRegistration
                        || argGLV==true || argMLV==true || argReg==true)
                    {
                        sSql = "Insert Into LandRateChange (CostCentreId,Date,OldGuideLine,NewGuideLine,OldMarketValue,NewMarketValue,OldRegValue,NewRegValue) Values" +
                            "(" + LandRateChangeBO.CCId + ",'" + LandRateChangeBO.Date + "'," + LandRateChangeBO.OldGuideValue + "," +
                        " " + LandRateChangeBO.NewGuideValue + "," + LandRateChangeBO.OldMarketValue + "," + LandRateChangeBO.NewMarketValue + "," +
                        " " + LandRateChangeBO.OldRegistration + "," + LandRateChangeBO.NewRegistration + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Update ProjectInfo Set GuideLineValue=" + LandRateChangeBO.NewGuideValue + "," +
                                " LandCost=" + LandRateChangeBO.NewMarketValue + ",Registration=" + LandRateChangeBO.NewRegistration + "" +
                                " Where CostCentreId=" + LandRateChangeBO.CCId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Select FlatTypeId,BaseAmt,USLandArea From FlatType Where ProjId=" + LandRateChangeBO.CCId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        dr = cmd.ExecuteReader();
                        dt = new DataTable();
                        dt.Load(dr);
                        cmd.Dispose();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            iFlatTypeId = Convert.ToInt32(dt.Rows[i]["FlatTypeId"]);
                            dFTBaseAmt = Convert.ToDecimal(dt.Rows[i]["BaseAmt"]);
                            dFTUSLandArea = Convert.ToDecimal(dt.Rows[i]["USLandArea"]);

                            dFTLandRate = dFTUSLandArea * LandRateChangeBO.NewMarketValue;
                            dFTLandAmount = dFTUSLandArea * LandRateChangeBO.NewGuideValue;
                            dRegValue = dFTUSLandArea * LandRateChangeBO.NewRegistration * LandRateChangeBO.NewGuideValue / 100;
                            UnitDirBL.UpdateRegistrationFlatType(iFlatTypeId, dRegValue);
                            dOAmt = UnitDirBL.GetOtherCostflatType(iFlatTypeId);
                            dFTNetAmount = dFTBaseAmt + dOAmt;

                            sSql = "Update FlatType Set Guidelinevalue=" + LandRateChangeBO.NewGuideValue + ",LandRate=" + dFTLandRate + ",LandAmount=" + dFTLandAmount + "," +
                                " OtherCostAmt=" + dOAmt + ",NetAmt=" + dFTNetAmount + " Where FlatTypeId=" + iFlatTypeId + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                        dt.Dispose();
                    }
                        //MarketLandValue
                    if (LandRateChangeBO.OldMarketValue != LandRateChangeBO.NewMarketValue || argMLV==true)
                    {
                        sSql = "Select FlatId,FlatTypeId,BaseAmt,USLand From FlatDetails Where CostCentreId=" + LandRateChangeBO.CCId + " And BlockId=" + argBlockId + " And Status='U'";
                        cmd = new SqlCommand(sSql, conn, tran);
                        dr = cmd.ExecuteReader();
                        dt = new DataTable();
                        dt.Load(dr);
                        cmd.Dispose();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            iFlatId = Convert.ToInt32(dt.Rows[i]["FlatId"]);
                            iFlatTypeId = Convert.ToInt32(dt.Rows[i]["FlatTypeId"]);
                            dFUSLandArea = Convert.ToDecimal(dt.Rows[i]["USLand"]);

                            dFLandRate = dFUSLandArea * LandRateChangeBO.NewMarketValue;

                            sSql = "Update FlatDetails Set LandRate=" + dFLandRate + "" +
                                " Where FlatId=" + iFlatId + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            PaymentScheduleDL.InsertFlatScheduleI(iFlatId, conn, tran);
                        }
                    }

                        //GuideLineValue
                    if (LandRateChangeBO.OldGuideValue != LandRateChangeBO.NewGuideValue || LandRateChangeBO.OldRegistration != LandRateChangeBO.NewRegistration
                        || argGLV==true || argReg==true)
                    {
                        sSql = "Select FlatId,FlatTypeId,BaseAmt,USLand From FlatDetails" +
                                " Where CostCentreId=" + LandRateChangeBO.CCId + " And BlockId=" + argBlockId + " And FlatId " +
                                " Not In (Select B.FlatId From CheckListMaster A" +
                                " Inner Join FlatChecklist B On A.CheckListId=B.CheckListId" +
                                " Where TypeName='F' And CheckListName='Registration')";
                        cmd = new SqlCommand(sSql, conn, tran);
                        dr = cmd.ExecuteReader();
                        dt = new DataTable();
                        dt.Load(dr);
                        cmd.Dispose();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            iFlatId = Convert.ToInt32(dt.Rows[i]["FlatId"]);
                            iFlatTypeId = Convert.ToInt32(dt.Rows[i]["FlatTypeId"]);
                            dFBaseAmt = Convert.ToDecimal(dt.Rows[i]["BaseAmt"]);
                            dFUSLandArea = Convert.ToDecimal(dt.Rows[i]["USLand"]);

                            dFLandAmount = dFUSLandArea * LandRateChangeBO.NewGuideValue;
                            dFRegValue = dFUSLandArea * LandRateChangeBO.NewRegistration * LandRateChangeBO.NewGuideValue / 100;
                            UnitDirBL.UpdateRegistrationFlat(iFlatId, dFRegValue);
                            dFOAmt = UnitDirBL.GetOtherCostFlat(iFlatId);
                            dFNetAmount = dFBaseAmt + dFOAmt;

                            sSql = "Update FlatDetails Set Guidelinevalue=" + LandRateChangeBO.NewGuideValue + ",USLandAmt=" + dFLandAmount + "," +
                                " OtherCostAmt=" + dFOAmt + ",NetAmt=" + dFNetAmount + " Where FlatId=" + iFlatId + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            PaymentScheduleDL.InsertFlatScheduleI(iFlatId, conn, tran);
                            dt.Dispose();
                        }
                    }
                        

                        tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

        }

        public static string FoundDate()
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB(); string bDate = "";
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "select MAX(Date) DateValue From LandRateChange";
                    cmd = new SqlCommand(sSql, conn, tran);
                    bDate = cmd.ExecuteScalar().ToString();
                    cmd.Dispose();

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
            return bDate;
        }

        public static string FoundDateLand()
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB(); string bDate = "";
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "select MAX(Date) DateValue From ChangeRate";
                    cmd = new SqlCommand(sSql, conn, tran);
                    bDate = cmd.ExecuteScalar().ToString();
                    cmd.Dispose();

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
            return bDate;
        }

        public static DataTable GetCancelPenalty(int argCCId,int argFlatId)
        {
            DataTable dt = new DataTable();
            string sSql = "";
            try
            {
                //string sSql = "Select B.FlatId,A.CancelType,A.CancelPenalty,A.BookingType,A.BookingPenalty,B.NetAmt From ProjectInfo A" +
                //" Inner Join FlatDetails B On A.CostCentreId=B.CostCentreId" +
                //" Where A.CostCentreId=" + argCCId + " And B.FlatId=" + argFlatId + "";
                sSql = "Select B.FlatId,A.CancelType,A.CancelPenalty,A.BookingType,A.BookingPenalty,B.NetAmt+B.QualifierAmt NetAmt," +
                        " SUM(Isnull(C.Amount,0)) ReceivedAmount From ProjectInfo A " +
                        " Inner Join FlatDetails B On A.CostCentreId=B.CostCentreId " +
                        " Left Join ReceiptRegister C On C.LeadId=B.LeadId " +
                        " Where A.CostCentreId=" + argCCId + " And B.FlatId=" + argFlatId + " "+
                        " Group by B.FlatId,A.CancelType,A.CancelPenalty,A.BookingType,A.BookingPenalty,B.NetAmt+B.QualifierAmt";
                BsfGlobal.OpenCRMDB();
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                dr.Close();
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
            return dt;
        }

        public static DataTable GetCancelPenaltyEdit(int argFlatId)
        {
            DataTable dt = new DataTable();
            string sSql = "";
            try
            {
                sSql = "Select A.CancelId,D.CostCentreName,B.FlatNo,C.LeadName,A.CancelDate,A.NetAmount ReceivableAmount,A.PaidAmount ReceivedAmount, " +
                        " A.PenaltyAmt,BalanceAmount,A.Remarks From dbo.AllotmentCancel A Inner Join dbo.FlatDetails B On A.FlatId=B.FlatId " +
                        " Inner Join dbo.LeadRegister C On C.LeadId=A.BuyerId " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre D On D.CostCentreId=B.CostCentreId " +
                        " Where A.CancelId=" + argFlatId + " ";
                BsfGlobal.OpenCRMDB();
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                dr.Close();
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
            return dt;
        }

        public static bool FoundProgressBill(int argFlatId)
        {
            bool bAns = false;
            DataTable dt = new DataTable();
            try
            {
                string sSql = "Select FlatId From ProgressBillRegister Where FlatId=" + argFlatId + "";
                BsfGlobal.OpenCRMDB();
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    bAns = true;
                }
                dr.Close();
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
            return bAns;
        }

        public static DataSet GetLoanReport()
        {
            DataSet ds = new DataSet();
            string sSql = "";
            try
            {
                sSql = "Select CostCentreId,CostCentreName from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                        " Where ProjectDB in(Select ProjectName from " +
                        " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType in('B'))" +
                        " and CostCentreId not in (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                        " Where UserId=" + BsfGlobal.g_lUserId + ") Order by CostCentreName";

                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "CostCentre");
                da.Dispose();

                sSql = "Select CostCentreId,Count(PaymentOption)as Loan From BuyerDetail " +
                    " Where PaymentOption='L' And Status='S' Group By CostCentreId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Loan");
                da.Dispose();

                sSql = "Select CostCentreId,Count(PaymentOption)as Own From BuyerDetail " +
                    " Where PaymentOption='O' And Status='S' Group By CostCentreId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Own");
                da.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }

            return ds;
        }

        public static DataSet GetSalesReport(int argProjId)
        {
            DataSet ds = new DataSet();
            string sSql = "";
            try
            {
            sSql = "SELECT A.Date,B.TypeName,IsNull(C.LevelName,'All Levels')LevelName,A.NewRate,A.FlatTypeId,A.LevelId,C.CostCentreId FROM" +
                    " ChangeRate A Inner Join FlatType B On A.FlatTypeId=B.FlatTypeId" +
                    " Inner Join LevelMaster C On C.LevelId=A.LevelId And C.CostCentreId=" + argProjId + "" +
                    " WHERE ProjId=" + argProjId + " And A.CostCentreId=" + argProjId + " Order By B.TypeName,C.LevelName";
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "Sales");
            da.Dispose();

            sSql = "Select A.CostCentreId,B.LevelId,B.FlatTypeId,Count(A.CostCentreId)Total From BuyerDetail A " +
                    " Inner Join FlatDetails B On A.CostCentreId=B.CostCentreId" +
                    " And A.FlatId=B.FlatId Where A.CostCentreId=" + argProjId + " " +
                    " Group By A.CostCentreId,B.LevelId,B.FlatTypeId";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "Lead");
            da.Dispose();

            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }

            return ds;
        }

        public static DataTable GetTimeLineReport(int argProjId)
        {
            DataTable dt = new DataTable();
            string sSql = "";
            try
            {
                sSql = "Select A.CostCentreId,FinaliseDate Date,B.FlatTypeId,C.TypeName,B.LevelId,D.LevelName,B.Rate,Count(FinaliseDate)NoOfFinalization From BuyerDetail A " +
                        " Inner Join FlatDetails B On A.FlatId=B.FlatId" +
                        " Inner Join FlatType C On C.FlatTypeId=B.FlatTypeId " +
                        " Inner Join LevelMaster D On D.LevelId=B.LevelId And D.CostCentreId=B.CostCentreId" +
                        " Where A.CostCentreId=" + argProjId + " And A.Status='S' Group By A.CostCentreId,FinaliseDate,B.Rate,B.FlatTypeId,C.TypeName,B.LevelId,D.LevelName";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(dt);
                da.Dispose();
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
                        " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType in('B'))" +
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

        public static DataTable GetArea(string argEAreaId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            string newS = ""; string stt = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                stt = argEAreaId.TrimEnd(',');

                for (int i = 0; i < stt.Length; i++)
                {
                    newS += stt[i].ToString();
                }
                if (newS == "")
                {
                    sSql = "SELECT AreaId,Description,0.00 AreaSqft,Convert(bit,0,0) Sel FROM AreaMaster";
                }
                else
                {
                    sSql = "SELECT AreaId,Description,0.00 AreaSqft,Convert(bit,0,0) Sel FROM AreaMaster WHERE AreaId NOT IN (" + newS + ")";
                }
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

        public static DataTable GetAreaMaster()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "SELECT AreaId,Description FROM AreaMaster ORDER BY Description";
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

        public static void UpdateAreaMaster(int argAreaId,string argDesc)
        {
            DataTable dt;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "UPDATE AreaMaster SET Description='" + argDesc + "' WHERE AreaId=" + argAreaId + "";
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
        }

        public static void InsertAreaMaster(string argDesc)
        {
            DataTable dt;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "INSERT INTO AreaMaster (Description) VALUES ('" + argDesc + "')";
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
        }

        public static void DeleteAreaMaster(int argAreaId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "DELETE FROM AreaMaster WHERE AreaId=" + argAreaId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

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

        public static bool FoundArea(int argAreaId)
        {
            bool bAns = false;
            DataTable dt = new DataTable();
            try
            {
                string sSql = " Select AreaId From FlatTypeArea Where AreaId= " + argAreaId + " " +
                        " Union All " +
                        " Select AreaId From FlatArea Where AreaId = " + argAreaId + "";
                BsfGlobal.OpenCRMDB();
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    bAns = true;
                }
                dr.Close();
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
            return bAns;
        }

        public static void UpdateUnitChangeCP(int argFlatId, int argCCId, int argNoCP, decimal argAmt)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            int iFlatId = 0; decimal dOCAmt = 0;
            decimal dBaseAmt = 0, dNetAmt = 0, dQualAmt = 0;
            bool bPayTypewise = false;
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    iFlatId = argFlatId;

                    sSql = "Select A.*,B.Typewise From dbo.FlatDetails A Inner Join PaySchType B On A.PayTypeId=B.TypeId " +
                        " Where FlatId=" + iFlatId + " And A.CostCentreId=" + argCCId + " ";
                    cmd = new SqlCommand(sSql, conn, tran);
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    cmd.Dispose();

                    if (dt.Rows.Count > 0)
                    {
                        dOCAmt = argAmt;
                        dBaseAmt = Convert.ToDecimal(dt.Rows[0]["BaseAmt"]);
                        dNetAmt = dBaseAmt + argAmt;
                        dQualAmt = Convert.ToDecimal(dt.Rows[0]["QualifierAmt"]);
                        bPayTypewise = Convert.ToBoolean(dt.Rows[0]["Typewise"]);

                        sSql = "Update dbo.FlatDetails Set TotalCarPark=" + argNoCP + ",OtherCostAmt=" + dOCAmt + ",NetAmt=" + dNetAmt + ",QualifierAmt=" + dQualAmt + " " +
                            " Where FlatId=" + iFlatId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Select PaymentSchId,FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate," +
                                " DateAfter, Duration,DurationType,SchPercent,Amount,NetAmount,PreStageTypeId,SortOrder,BillPassed,PaidAmount,StageDetId From dbo.PaymentScheduleFlat " +
                                " Where FlatId=" + iFlatId + " And TemplateId<>0 Order By SortOrder";
                        cmd = new SqlCommand(sSql, conn, tran);
                        dr = cmd.ExecuteReader();
                        DataTable dtP = new DataTable();
                        dtP.Load(dr);
                        cmd.Dispose();

                        if (bPayTypewise == true)
                            PaymentScheduleDL.UpdateReceiptBuyerSchedule(iFlatId, dtP, conn, tran);
                        else
                            PaymentScheduleDL.UpdateReceiptBuyerScheduleQual(iFlatId, dtP, conn, tran);
                        //PaymentScheduleBL.InsertFlatScheduleI(iFlatId, conn, tran);

                        dQualAmt = UnitDirBL.GetGlobalQualifierAmt(iFlatId, bPayTypewise, conn, tran);

                        sSql = "Update dbo.FlatDetails Set QualifierAmt=" + dQualAmt + " " +
                            " Where FlatId=" + iFlatId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    BsfGlobal.CustomException(e.Message, e.StackTrace);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        #endregion

        #region BlockCancelUnits

        public static DataTable GetBlockUnits(int argFlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "SELECT A.*,B.BlockingPenalty,B.BlockingType,C.NetAmt FROM BlockUnits A " +
                        " INNER JOIN ProjectInfo B On A.CostCentreId=B.CostCentreId " +
                        " INNER JOIN FlatDetails C On C.FlatId=A.FlatId " +
                        " WHERE A.FlatId=" + argFlatId + " And BlockType='B'";
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

        public static void InsertBlockUnits()
        {
            SqlConnection conn;
            SqlTransaction tran;
            conn = BsfGlobal.OpenCRMDB();
            tran = conn.BeginTransaction();
            DataTable dt; SqlDataReader dr;
            SqlCommand cmd;
            string sSql = "";
            
            try
            {
                sSql = "SELECT * FROM BlockUnits WHERE FlatId=" + Blockunits.FlatId + " " +
                    " AND CostCentreId=" + Blockunits.CCId + " And BlockType='B'";
                cmd = new SqlCommand(sSql,conn,tran);
                dr=cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    sSql = "UPDATE BlockUnits SET CustomerType='" + Blockunits.CustomerType + "',LeadId=" + Blockunits.LeadId + "," +
                    " BlockUpto='" + Blockunits.BlockUpto + "',Remarks='" + Blockunits.Remarks + "'," +
                    " BlockType='B',Date='" + Blockunits.Date + "'" +
                    " WHERE FlatId=" + Blockunits.FlatId + " And BlockType='B'";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    sSql = "INSERT INTO BlockUnits(CostCentreId,FlatId,CustomerType,LeadId,BlockUpto,Date,BlockType,Remarks) Values(" + Blockunits.CCId + "," +
                            " " + Blockunits.FlatId + ",'" + Blockunits.CustomerType + "'," + Blockunits.LeadId + ",'" + Blockunits.BlockUpto + "'," +
                            " '" + Blockunits.Date + "','B','" + Blockunits.Remarks + "')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "UPDATE FlatDetails SET Status='B',LeadId=" + Blockunits.LeadId + " WHERE FlatId=" + Blockunits.FlatId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    PaymentScheduleDL.InsertFinalFlatScheduleI(Blockunits.FlatId, "B", conn, tran);
                }
                tran.Commit();
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
        }

        public static DataTable GetBlockLead(int argCCId,string argType)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "SELECT R.LeadId,R.LeadName FROM LeadRegister R Inner Join LeadExecutiveInfo B On R.LeadId=B.LeadId" +
                      " Inner Join dbo.LeadProjectInfo P On P.LeadId=B.LeadId "+
                      " WHERE P.CostCentreId=" + argCCId + " And R.LeadType='" + argType + "' ORDER BY LeadName";
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

        public static void UpdateUnBlockUnits()
        {
            SqlConnection conn;
            SqlTransaction tran;
            conn = BsfGlobal.OpenCRMDB();
            tran = conn.BeginTransaction();
            SqlCommand cmd;
            string sSql = "";

            try
            {
                sSql = "UPDATE BlockUnits SET CustomerType='" + Blockunits.CustomerType + "',LeadId=" + Blockunits.LeadId + "," +
                        " BlockUpto='" + Blockunits.BlockUpto + "', Date='" + Blockunits.Date + "'," +
                        " PenaltyAmount=" + Blockunits.PenaltyAmt + ",BlockType='U',Remarks='" + Blockunits.Remarks + "'" +
                        " WHERE FlatId=" + Blockunits.FlatId + " And BlockType='B' ";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "UPDATE FlatDetails Set Status='U',LeadId=0 WHERE FlatId=" + Blockunits.FlatId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                tran.Commit();
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
        }

        public static DataTable GetCancelUnits()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "SELECT F.FlatNo,F.Area, E.LeadName, A.CancelDate,A.Remarks FROM dbo.AllotmentCancel A" +
                       " INNER JOIN dbo.FlatDetails F ON A.FlatId=F.FlatId LEFT JOIN dbo.LeadRegister E ON E.LeadId=A.BuyerId";
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


        #endregion

        #region CarPark

        public static bool FoundCP(int argTypeId)
        {
            DataTable dt;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            bool bAns = false;

            try
            {
                sSql = " Select TypeId From dbo.CarParkMaster Where TypeId= " + argTypeId + " " +
                   " Union All " +
                   " Select TypeId From dbo.CarParkCost Where TypeId = " + argTypeId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    bAns = true;
                }
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return bAns;
        }

        #endregion

        #region PaymentInfo

        internal static DataTable GetFiscalYear()
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;

            try
            {
                dt = new DataTable();
                if (BsfGlobal.g_bFADB == true)
                { sSql = "Select FYearId,FName,StartDate,EndDate From [" + BsfGlobal.g_sFaDBName + "].dbo.FiscalYear"; }
                else { sSql = "Select FYearId,FName,StartDate,EndDate From dbo.FiscalYear"; }
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ce)
            {
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable GetFlatPaymentInfo(int argFlatId, DateTime argFrom, DateTime argTo)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                //sSql = "SELECT X.RowId,X.ReceiptId,X.ReceiptDate,Y.ReceiptNo,Y.ChequeDate,Y.ChequeNo,Y.BankName,X.QualifierId,X.QualifierName,X.NetPer,X.Amount FROM (" +
                //        " SELECT 1 RowId,A.ReceiptId,B.ReceiptDate,0 QualifierId,'PaidGrossAmount' QualifierName,0 NetPer,isnull(SUM(PaidGrossAmount),0) Amount FROM ReceiptShTrans A   " +
                //        " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                //        " WHERE PaidNetAmount<>0 AND A.ReceiptTypeId<>1 And A.FlatId=" + argFlatId + " " +
                //        " And B.ReceiptDate Between '" + argFrom.ToString("dd-MMM-yyyy") + "' And '" + argTo.ToString("dd-MMM-yyyy") + "' " +
                //        " GROUP BY A.ReceiptId,B.ReceiptDate" +
                //        " UNION ALL " +
                //        " Select 2 RowId,A.ReceiptId,C.ReceiptDate,B.QualifierId,D.QualifierName,B.NetPer,Sum(B.Amount)Amount From ReceiptShTrans A  " +
                //        " Inner Join ReceiptQualifier B On A.ReceiptId=B.ReceiptId And A.PaymentSchId=B.PaymentSchId And A.OtherCostId=B.OtherCostId " +
                //        " And A.ReceiptTypeId=B.ReceiptTypeId  Inner Join ReceiptRegister C On C.ReceiptId=A.ReceiptId  " +
                //        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp D On D.QualifierId=B.QualifierId And QualType='B' " +
                //        " Where A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1 And A.FlatId=" + argFlatId + "" +
                //        " And C.ReceiptDate Between '" + argFrom.ToString("dd-MMM-yyyy") + "' And '" + argTo.ToString("dd-MMM-yyyy") + "' " +
                //        " GROUP BY A.ReceiptId,C.ReceiptDate,B.QualifierId,D.QualifierName,B.NetPer" +
                //        " UNION ALL  " +
                //        " SELECT 3 RowId,A.ReceiptId,B.ReceiptDate,0 QualifierId,'PaidNetAmount' QualifierName,0 NetPer,isnull(SUM(A.Amount),0) Amount FROM ReceiptTrans A   " +
                //        " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId   " +
                //        " WHERE A.Amount<>0 And A.FlatId=" + argFlatId + " " +
                //        " AND B.ReceiptDate Between '" + argFrom.ToString("dd-MMM-yyyy") + "' And '" + argTo.ToString("dd-MMM-yyyy") + "' " +
                //        " GROUP BY A.ReceiptId,B.ReceiptDate) X INNER JOIN ReceiptRegister Y ON X.ReceiptId=Y.ReceiptId " +
                //        " ORDER BY X.ReceiptId,X.RowId";
                sSql = "SELECT RowId,ReceiptId,ReceiptDate,ReceiptNo,Narration,ChequeDate,ChequeNo,BankName,PaymentMode,QualifierId,QualifierName,NetPer,Amount " +
                        " FROM( " +
                        " SELECT Top 1 0 RowId,0 ReceiptId,Null ReceiptDate,''ReceiptNo,'O/B' Narration,Null ChequeDate,''ChequeNo,''BankName,''PaymentMode,0 QualifierId,'' QualifierName,0 NetPer,Abs(A.OBReceipt) Amount " +
                        " FROM dbo.FlatDetails A LEFT JOIN dbo.ReceiptTrans B ON A.FlatId=B.FlatId LEFT JOIN dbo.ReceiptRegister C ON C.ReceiptId=B.ReceiptId " +
                        " WHERE A.FlatId=" + argFlatId + " " +
                        " UNION ALL " +
                        " SELECT X.RowId,X.ReceiptId,X.ReceiptDate,Y.ReceiptNo,Y.Narration,Y.ChequeDate,Y.ChequeNo,Y.BankName,Y.PaymentMode,X.QualifierId,X.QualifierName,X.NetPer,X.Amount FROM ( " +
                        " SELECT 1 RowId,A.ReceiptId,B.ReceiptDate,0 QualifierId,'PaidGrossAmount' QualifierName,0 NetPer,isnull(SUM(PaidGrossAmount),0) Amount " +
                        " FROM dbo.ReceiptShTrans A INNER JOIN dbo.ReceiptRegister B ON A.ReceiptId=B.ReceiptId  " +
                        " WHERE PaidNetAmount<>0 AND A.ReceiptTypeId<>1 And A.FlatId=" + argFlatId + "  And B.ReceiptDate Between '" + argFrom.ToString("dd-MMM-yyyy") + "' And '" + argTo.ToString("dd-MMM-yyyy") + "'  " +
                        " GROUP BY A.ReceiptId,B.ReceiptDate " +
                        " UNION ALL  " +
                        " Select 2 RowId,A.ReceiptId,C.ReceiptDate,B.QualifierId,D.QualifierName,B.NetPer,Sum(B.Amount)Amount From dbo.ReceiptShTrans A   " +
                        " Inner Join dbo.ReceiptQualifier B On A.ReceiptId=B.ReceiptId And A.PaymentSchId=B.PaymentSchId And A.OtherCostId=B.OtherCostId  And A.ReceiptTypeId=B.ReceiptTypeId  " +
                        " Inner Join dbo.ReceiptRegister C On C.ReceiptId=A.ReceiptId   Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp D On D.QualifierId=B.QualifierId And QualType='B'  " +
                        " Where A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1 And A.FlatId=" + argFlatId + " And C.ReceiptDate Between '" + argFrom.ToString("dd-MMM-yyyy") + "' And '" + argTo.ToString("dd-MMM-yyyy") + "'  " +
                        " GROUP BY A.ReceiptId,C.ReceiptDate,B.QualifierId,D.QualifierName,B.NetPer " +
                        " UNION ALL   " +
                        " SELECT 3 RowId,A.ReceiptId,B.ReceiptDate,0 QualifierId,'PaidNetAmount' QualifierName,0 NetPer,isnull(SUM(A.Amount),0) Amount FROM dbo.ReceiptTrans A    " +
                        " INNER JOIN dbo.ReceiptRegister B ON A.ReceiptId=B.ReceiptId WHERE A.Amount<>0 And A.FlatId=" + argFlatId + "  AND B.ReceiptDate " +
                        " Between '" + argFrom.ToString("dd-MMM-yyyy") + "' And '" + argTo.ToString("dd-MMM-yyyy") + "' GROUP BY A.ReceiptId,B.ReceiptDate " +
                        " ) X INNER JOIN dbo.ReceiptRegister Y ON X.ReceiptId=Y.ReceiptId )Z " +
                        " ORDER BY Z.ReceiptId,Z.RowId";
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

        internal static DataTable GetFlatPaymentwiseInfo(int argFlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                //sSql = "SELECT X.CostCentreName,X.TypeName,X.BlockName,X.LevelName,X.LeadName,X.ReceiptId,X.ReceiptDate,Y.ReceiptNo,Y.ChequeDate,Y.ChequeNo,Y.BankName,X.QualifierId,X.NetPer,X.Amount FROM ( " +
                //        " SELECT O.CostCentreName,FT.TypeName,BM.BlockName,LM.LevelName,LR.LeadName,A.ReceiptId,B.ReceiptDate,0 QualifierId,0 NetPer,isnull(SUM(A.Amount),0) Amount FROM ReceiptTrans A    " +
                //        " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId  INNER JOIN FlatDetails Z ON Z.FlatId=A.FlatId " +
                //        " INNER JOIN dbo.BlockMaster BM ON BM.BlockId=Z.BlockId INNER JOIN dbo.LevelMaster LM ON LM.LevelId=Z.LevelId " +
                //        " INNER JOIN dbo.FlatType FT ON FT.FlatTypeId=Z.FlatTypeId INNER JOIN dbo.LeadRegister LR ON LR.LeadId=Z.LeadId " +
                //        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=Z.CostCentreId " +
                //        " WHERE A.Amount<>0 And A.FlatId=" + argFlatId + " GROUP BY O.CostCentreName,FT.TypeName,BM.BlockName,LM.LevelName,LR.LeadName,A.ReceiptId,B.ReceiptDate) X " +
                //        " INNER JOIN ReceiptRegister Y ON X.ReceiptId=Y.ReceiptId " +
                //        " ORDER BY X.ReceiptId";
                sSql = "Select Null ReceiptDate,''ReceiptNo,'O/B' Narration,Null ChequeDate,''ChequeNo,''PaymentMode,'' PayType,'' [Transaction],Abs(OBReceipt)Amount FROM dbo.FlatDetails " +
                        " WHERE FlatId= " + argFlatId + " " +
                        " UNION ALL " +
                        " Select B.ReceiptDate,B.ReceiptNo,B.Narration,B.ChequeDate,B.ChequeNo,B.PaymentMode,A.ReceiptType PayType,B.BankName [Transaction], " +
                        " A.Amount from dbo.ReceiptTrans A" +
                        " Inner Join dbo.ReceiptRegister B on A.ReceiptId=B.ReceiptId" +
                        " Where A.Amount<>0 And A.Cancel=0 And A.FlatId= " + argFlatId;
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


        #endregion

        #region OCGroup

        internal static DataTable GetOCGMaster()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "SELECT GroupId,GroupName FROM dbo.OtherCostGroup ORDER BY GroupName";
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

        internal static void InsertOCG(string argDesc)
        {
            DataTable dt;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "INSERT INTO dbo.OtherCostGroup (GroupName) VALUES ('" + argDesc + "')";
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
        }

        public static void UpdateOCG(int argGroupId, string argDesc)
        {
            DataTable dt;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "UPDATE dbo.OtherCostGroup SET GroupName='" + argDesc + "' WHERE GroupId=" + argGroupId + "";
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
        }

        public static void DeleteOCG(int argGroupId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "DELETE FROM dbo.OtherCostGroup WHERE GroupId=" + argGroupId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

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

        public static bool FoundOCG(int argGroupId)
        {
            bool bAns = false;
            DataTable dt = new DataTable();
            try
            {
                string sSql = " Select GroupId From dbo.OtherCostMaster Where GroupId= " + argGroupId + " ";
                BsfGlobal.OpenCRMDB();
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    bAns = true;
                }
                dr.Close();
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
            return bAns;
        }

        public static bool FoundOCGName(string argGroupName)
        {
            bool bAns = false;
            DataTable dt = new DataTable();
            try
            {
                string sSql = " Select GroupId From dbo.OtherCostGroup Where GroupName='" + argGroupName + "'";
                BsfGlobal.OpenCRMDB();
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    bAns = true;
                }
                dr.Close();
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
            return bAns;
        }

        internal static DataTable GetOCGList()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.OtherCostId,A.OtherCostName,A.GroupId From dbo.OtherCostMaster A Left Join dbo.OtherCostGroup B On A.GroupId=B.GroupId Order By A.OtherCostName";
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

        internal static void UpdateOCGList(DataTable argdt)
        {
            BsfGlobal.OpenCRMDB();
            try
            {
                for (int i = 0; i < argdt.Rows.Count; i++)
                {
                    string sSql = "UPDATE dbo.OtherCostMaster SET GroupId=" + argdt.Rows[i]["GroupId"] + " WHERE OtherCostId=" + argdt.Rows[i]["OtherCostId"] + "";
                    SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
        }

        #endregion

        #region Reports

        internal static DataTable GetReceiptDet(int argFlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select ReceiptDate,ReceiptNo From ReceiptRegister A Inner Join ReceiptTrans B On A.ReceiptId=B.ReceiptId " +
                        " Where FlatId=" + argFlatId + " And BillType='A'";
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

        #endregion

        #region OtherCost

        public static decimal GetFlatTypeOCAmt(int argCCId, int argFTId, int argPayTypeId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB(); decimal dOCAmt=0; SqlDataReader dr;
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    //sSql = " SELECT SUM(CASE WHEN Flag='-' THEN Amount*(-1) ELSE Amount END) Amount FROM dbo.FlatTypeOtherCost " +
                    //        " WHERE FlatTypeId =" + argFTId + " AND OtherCostId IN " +
                    //        " (SELECT OtherCostId FROM dbo.OtherCostSetupTrans WHERE PayTypeId=" + argPayTypeId + " AND CostCentreId=" + argCCId + ")";

                    sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatTypeOtherCost " +
                            " Where FlatTypeId =" + argFTId + " and OtherCostId not in (Select OtherCostId from dbo.OXGross " +
                            " Where CostCentreId=" + argCCId + ")";
                    cmd = new SqlCommand(sSql, conn, tran);
                    dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    cmd.Dispose();
                    if (dt.Rows.Count > 0)
                    {
                        dOCAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
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
            return dOCAmt;
        }

        public static decimal GetFlatOCAmt(int argCCId, int argFlatId, int argPayTypeId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB(); decimal dOCAmt = 0; SqlDataReader dr;
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    string sSql = "Select SUM(CASE WHEN Flag='-' THEN Amount*(-1) ELSE Amount END) Amount from dbo.FlatOtherCost " +
                                    " Where FlatId =" + argFlatId + " AND OtherCostId NOT IN(Select OtherCostId from dbo.OXGross Where CostCentreId=" + argCCId + ")";
                    cmd = new SqlCommand(sSql, conn, tran);
                    dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    dr.Close();
                    cmd.Dispose();

                    if (dt.Rows.Count > 0)
                    {
                        dOCAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
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
            return dOCAmt;
        }

        #endregion


        internal static bool CheckReceipt(int m_iFlatId)
        {
            BsfGlobal.OpenCRMDB();
            bool bReceipt = false;
            try
            {
                string sSql = String.Format("Select COUNT(*) from ReceiptTrans Where FlatId={0}", m_iFlatId);
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                int iCount = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                cmd.Dispose();

                if (iCount > 0) { bReceipt = true; }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return bReceipt;
        }
    }

}
