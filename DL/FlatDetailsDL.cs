using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessObjects;
using CRM.BusinessLayer;
using Qualifier;
using Microsoft.VisualBasic;

namespace CRM.DataLayer
{
    class FlatDetailsDL
    {
        #region Methods

        public static DataSet GetFlat(int argCCId, int argFlatTypeId, int argBlockId, int argLevelId, string argStatus)
        {
            DataSet ds = null;
            BsfGlobal.OpenCRMDB();
            try
            {
                string sSql = "SELECT B.SortOrder,L.SortOrder,F.SortOrder,F.BlockId,B.BlockName,F.FlatId,F.FlatTypeId,F.FlatNo,F.Area,F.Rate,F.NetAmt,F.Status, " +
                                " L.LevelName,F.LeadId,IsNull(LR.LeadName,'') BuyerName, Balance=(SUM(ISNULL(PSF.NetAmount,0))-SUM(ISNULL(PSF.PaidAmount,0))) from dbo.FlatDetails F " +
                                " LEFT JOIN dbo.LevelMaster L ON F.LevelId=L.LevelId " +
                                " LEFT JOIN dbo.BlockMaster B ON F.BlockId=B.BlockId " +
                                " LEFT JOIN dbo.LeadRegister LR ON LR.LeadId=F.LeadId " +
                                " LEFT JOIN dbo.PaymentScheduleFlat PSF ON F.FlatId=PSF.FlatId " +
                                " WHERE F.CostCentreId=" + argCCId;
                if (argFlatTypeId > 0) { sSql = sSql + " and F.FlatTypeId= " + argFlatTypeId; }
                if (argBlockId > 0) { sSql = sSql + " and F.BlockId= " + argBlockId; }
                if (argLevelId > 0) { sSql = sSql + " and F.LevelId= " + argLevelId; }
                if (argStatus != "") { sSql = sSql + " and F.Status= '" + argStatus + "'"; }

                sSql = sSql + " GROUP BY B.SortOrder,L.SortOrder,F.SortOrder,F.BlockId,B.BlockName,F.FlatId,F.FlatTypeId,F.FlatNo,F.Area,F.Rate,F.NetAmt,F.Status, L.LevelName,F.LeadId,LR.LeadName ORDER BY B.SortOrder,L.SortOrder,F.SortOrder,dbo.Val(F.FlatNo) ";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                ds = new DataSet();
                sda.Fill(ds, "Flat");

                if (argFlatTypeId <= 0 && argBlockId <= 0 && argLevelId <= 0 && argStatus == "")
                {
                    sSql = "SELECT A.BlockId,A.BlockName, Child=(SELECT COUNT(FlatId) FROM dbo.FlatDetails B " +
                            " WHERE B.BlockId=A.BlockId) FROM dbo.BlockMaster A " +
                            " Where A.CostCentreId=" + argCCId + " and A.BlockId IN(Select BlockId from dbo.FlatDetails) ORDER BY SortOrder";
                }
                else
                {
                    string sCond1 = "", sCond2 = "";
                    if (argFlatTypeId > 0) { sCond1 = "AND B.FlatTypeId = " + argFlatTypeId + ""; sCond2 = "FlatTypeId = " + argFlatTypeId + ""; }
                    if (argBlockId > 0) { sCond1 = sCond1 + "AND B.BlockId = " + argBlockId + ""; if (sCond2 == "") { sCond2 = sCond2 + "BlockId = " + argBlockId + ""; } else sCond2 = sCond2 + "AND BlockId = " + argBlockId + ""; }
                    if (argLevelId > 0) { sCond1 = sCond1 + "AND B.LevelId = " + argLevelId + ""; if (sCond2 == "") { sCond2 = sCond2 + "LevelId = " + argLevelId + ""; } else sCond2 = sCond2 + "AND LevelId = " + argLevelId + ""; }
                    if (argStatus != "") { sCond1 = sCond1 + "AND B.Status = '" + argStatus + "'"; if (sCond2 == "") { sCond2 = sCond2 + "Status = '" + argStatus + "'"; } else sCond2 = sCond2 + "AND Status = '" + argStatus + "'"; }

                    sSql = "SELECT A.BlockId,A.BlockName, Child=(SELECT COUNT(FlatId) FROM dbo.FlatDetails B " +
                            " WHERE B.BlockId=A.BlockId " + sCond1 + ") FROM dbo.BlockMaster A " +
                            " Where A.CostCentreId=" + argCCId + " and A.BlockId IN (Select BlockId From dbo.FlatDetails Where " + sCond2 + ") ORDER BY SortOrder";
                }
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Block");
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
            return ds;
        }

        //public static DataSet GetFlat(int argCCId,int argFlatTypeId,int argBlockId,int argLevelId)
        //{
        //    DataSet ds = null;
        //    SqlDataAdapter sda;
        //    BsfGlobal.OpenCRMDB();
        //    try
        //    {
        //        string sSql = "SELECT F.BlockId,B.BlockName,F.FlatId,F.FlatTypeId,F.FlatNo,F.Area,F.Rate,F.NetAmt,F.Status,L.LevelName,F.LeadId,IsNull(LR.LeadName,'') BuyerName from dbo.FlatDetails F " +
        //                      "INNER JOIN dbo.LevelMaster L ON F.LevelId=L.LevelId " +
        //                      "INNER JOIN dbo.BlockMaster B ON F.BlockId=B.BlockId " +
        //                      "LEFT JOIN dbo.LeadRegister LR ON LR.LeadId=F.LeadId " +
        //                      "WHERE F.CostCentreId=" + argCCId;
        //        if (argFlatTypeId > 0) { sSql = sSql + " and F.FlatTypeId= " + argFlatTypeId; }
        //        sSql = sSql + " ORDER BY B.SortOrder,L.SortOrder,F.SortOrder,dbo.Val(F.FlatNo) ";

        //        sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
        //        ds = new DataSet();
        //        sda.Fill(ds, "Flat");

        //        if (argFlatTypeId <= 0)
        //        {
        //            sSql = "SELECT A.BlockId,A.BlockName, Child=(SELECT COUNT(FlatId) FROM dbo.FlatDetails B WHERE B.BlockId=A.BlockId) FROM dbo.BlockMaster A Where A.CostCentreId=" + argCCId + " and " +
        //                   "A.BlockId in (Select BlockId from dbo.FlatDetails) ORDER BY SortOrder";
        //        }
        //        else
        //        {
        //            sSql = "SELECT A.BlockId,A.BlockName, Child=(SELECT COUNT(FlatId) FROM dbo.FlatDetails B WHERE B.BlockId=A.BlockId and B.FlatTypeId = " + argFlatTypeId + ") FROM dbo.BlockMaster A Where A.CostCentreId=" + argCCId + " and " +
        //                   "A.BlockId in (Select BlockId from dbo.FlatDetails Where FlatTypeId = " + argFlatTypeId + ") ORDER BY SortOrder";
        //        }

        //        sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
        //        sda.Fill(ds, "Block");
        //    }
        //    catch (Exception ex)
        //    {
        //        BsfGlobal.CustomException(ex.Message, ex.StackTrace);
        //    }
        //    finally
        //    {
        //        BsfGlobal.g_CRMDB.Close();
        //    }
        //    return ds;
        //}

        public static DataTable GetPayInfo(int argFlatId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                //sSql = "SELECT X.CostCentreName,X.TypeName,X.BlockName,X.LevelName,X.LeadName,X.ReceiptId,X.ReceiptDate,Y.ReceiptNo,Y.Narration,Y.ChequeDate,Y.ChequeNo,Y.BankName,X.QualifierId,X.NetPer,X.Amount FROM ( " +
                //       " SELECT O.CostCentreName,FT.TypeName,BM.BlockName,LM.LevelName,LR.LeadName,A.ReceiptId,B.ReceiptDate,0 QualifierId,0 NetPer,isnull(SUM(A.Amount),0) Amount FROM ReceiptTrans A    " +
                //       " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId INNER JOIN FlatDetails Z ON Z.FlatId=A.FlatId " +
                //       " INNER JOIN dbo.BlockMaster BM ON BM.BlockId=Z.BlockId INNER JOIN dbo.LevelMaster LM ON LM.LevelId=Z.LevelId " +
                //       " INNER JOIN dbo.FlatType FT ON FT.FlatTypeId=Z.FlatTypeId INNER JOIN dbo.LeadRegister LR ON LR.LeadId=Z.LeadId " +
                //       " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=Z.CostCentreId " +
                //       " WHERE A.Amount<>0 And A.FlatId=" + argFlatId + " GROUP BY O.CostCentreName,FT.TypeName,BM.BlockName,LM.LevelName,LR.LeadName,A.ReceiptId,B.ReceiptDate) X " +
                //       " INNER JOIN ReceiptRegister Y ON X.ReceiptId=Y.ReceiptId " +
                //       " ORDER BY X.ReceiptId";

                string sSql = "Select CostCentreName,TypeName,BlockName,LevelName,LeadName,ReceiptId,ReceiptDate,ReceiptNo,Narration, " +
                                " ChequeDate,ChequeNo,BankName,PaymentMode,QualifierId,NetPer,Amount FROM( " +
                                " SELECT ''CostCentreName,''TypeName,''BlockName,''LevelName,''LeadName,0 ReceiptId,Null ReceiptDate,''ReceiptNo, " +
                                " 'O/B' Narration,Null ChequeDate,''ChequeNo,''BankName,''PaymentMode,0 QualifierId,0 NetPer,abs(OBReceipt) Amount " +
                                " FROM dbo.FlatDetails WHERE FlatId=" + argFlatId + " " +
                                " UNION ALL " +
                                " SELECT X.CostCentreName,X.TypeName,X.BlockName,X.LevelName,X.LeadName,X.ReceiptId,X.ReceiptDate,Y.ReceiptNo,Y.Narration, " +
                                " Y.ChequeDate,Y.ChequeNo,Y.BankName,Y.PaymentMode,X.QualifierId,X.NetPer,X.Amount FROM (  " +
                                " SELECT O.CostCentreName,FT.TypeName,BM.BlockName,LM.LevelName,LR.LeadName,A.ReceiptId, " +
                                " B.ReceiptDate,0 QualifierId,0 NetPer,ISNULL(SUM(A.Amount),0) Amount FROM dbo.ReceiptTrans A  " +
                                " INNER JOIN dbo.ReceiptRegister B ON A.ReceiptId=B.ReceiptId" +
                                " INNER JOIN BuyerDetail C on A.FlatId=C.FlatId" +
                                " INNER JOIN dbo.FlatDetails Z ON Z.FlatId=A.FlatId  " +
                                " INNER JOIN dbo.BlockMaster BM ON BM.BlockId=Z.BlockId" +
                                " INNER JOIN dbo.LevelMaster LM ON LM.LevelId=Z.LevelId  " +
                                " INNER JOIN dbo.FlatType FT ON FT.FlatTypeId=Z.FlatTypeId" +
                                " INNER JOIN dbo.LeadRegister LR ON LR.LeadId=Z.LeadId  " +
                                " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=Z.CostCentreId  " +
                                " WHERE A.Amount<>0 And A.FlatId=" + argFlatId + " AND A.CancelDate IS NULL " +
                                " GROUP BY O.CostCentreName,FT.TypeName,BM.BlockName,LM.LevelName,LR.LeadName,A.ReceiptId,B.ReceiptDate) X  " +
                                " INNER JOIN dbo.ReceiptRegister Y ON X.ReceiptId=Y.ReceiptId )Z ORDER BY Z.ReceiptId";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dr.Close();
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

        public static DataTable GetClientBuyerInfo(int argFlatId, int argLandId, string argType)
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                if (argLandId == 0)
                {
                    if (argType == "Schedule")
                    {
                        sSql = " Select '' PBNo,A.SchDate AsOnDate,A.Description,A.NetAmount BillAmount,A.PaidAmount,(A.NetAmount-A.PaidAmount) Balance, " +
                                " 0 CumulativeAmount From dbo.PaymentScheduleFlat A INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                                " INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId " +
                                " Where A.FlatId=" + argFlatId + " And TemplateId=0 " +
                                " UNION ALL " +
                                " Select '',A.SchDate AsOnDate,A.Description,A.NetAmount BillAmount,A.PaidAmount, " +
                                " (A.Amount-F.Amt-A.Advance) Balance,0 CumulativeAmount From dbo.PaymentScheduleFlat A " +
                                " INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId   " +
                                " INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId    " +
                                " Join (Select FlatId,PaymentSchId,SUM(PaidGrossAmount) Amt from ReceiptShTrans Group by FlatId,PaymentSchId) F on A.FlatId=F.FlatId And A.PaymentSchId=F.PaymentSchId " +
                                " INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId Where A.FlatId=" + argFlatId + " " +
                                " And A.PaidAmount>0 And TemplateId<>0 ";
                        //sSql = " Select '' PBNo,A.SchDate AsOnDate,A.Description,A.NetAmount BillAmount,A.PaidAmount,(A.NetAmount-A.PaidAmount) Balance, " +
                        //   " 0 CumulativeAmount From dbo.PaymentScheduleFlat A " +
                        //   " INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                        //   " INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId " +
                        //   " Where A.FlatId=" + argFlatId + " And TemplateId=0 " +
                        //   //" UNION ALL " +
                        //   //" Select A.PBNo,A.AsOnDate,S.Description,A.NetAmount BillAmount,A.PaidAmount, " +
                        //   //" (A.NetAmount-A.PaidAmount) Balance,0 CumulativeAmount From dbo.ProgressBillRegister A " +
                        //   //" INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId INNER JOIN dbo.ProgressBillMaster M On M.ProgRegId=A.ProgRegId   " +
                        //   //" INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId Left JOIN dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId   " +
                        //   //" INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId Where M.Approve='Y' and A.FlatId=" + argFlatId + " And S.BillPassed=1" +
                        //   " UNION ALL" +
                        //    " Select '',A.SchDate AsOnDate,A.Description,A.NetAmount BillAmount,A.PaidAmount, " +
                        //    " (A.Amount-F.Amt) Balance,0 CumulativeAmount From dbo.PaymentScheduleFlat A " +
                        //    //" INNER JOIN dbo.ReceiptTrans RT On RT.PayschId=A.PaymentSchId " +
                        //    " INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId   " +
                        //    " INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId    " +
                        //    " Join (Select FlatId,PaymentSchId,SUM(PaidGrossAmount) Amt from ReceiptShTrans Group by FlatId,PaymentSchId) F on A.FlatId=F.FlatId and A.PaymentSchId=F.PaymentSchId " +
                        //    " INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId Where A.FlatId=" + argFlatId + " " +
                        //    " And A.PaidAmount>0 And TemplateId<>0 ";
                    }
                    else
                    {
                        sSql = " Select '' PBNo,A.SchDate AsOnDate,A.Description,A.NetAmount BillAmount,A.PaidAmount,(A.NetAmount-A.PaidAmount) Balance, " +
                                " 0 CumulativeAmount From dbo.PaymentScheduleFlat A INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                                " INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId " +
                                " Where A.FlatId=" + argFlatId + " And TemplateId=0 " +
                                " UNION ALL " +
                                " Select A.PBNo,A.AsOnDate,S.Description,S.NetAmount BillAmount,A.PaidAmount, " +
                                " (S.NetAmount-A.PaidAmount) Balance,0 CumulativeAmount From dbo.ProgressBillRegister A " +
                                " INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId INNER JOIN dbo.ProgressBillMaster M On M.ProgRegId=A.ProgRegId   " +
                                " INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId Left JOIN dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId   " +
                                " INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId Where M.Approve='Y' and A.FlatId=" + argFlatId + " And S.BillPassed=1";

                    }

                }
                else
                    sSql = "Select A.PBNo,A.AsOnDate,S.Description,A.NetAmount BillAmount,IsNull (B.Amount,0) PaidAmount, " +
                            " (IsNull (A.NetAmount,0)-IsNull (B.Amount,0)) Balance,0 CumulativeAmount From dbo.PlotProgressBillRegister A    " +
                            " LEFT JOIN dbo.ReceiptRegister B On A.BuyerId=B.LeadId And A.CostCentreId=B.CostCentreId   " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails D On A.PlotDetailsId=D.PlotDetailsId " +
                            " INNER JOIN dbo.PlotProgressBillMaster M On M.ProgRegId=A.ProgRegId   " +
                            " INNER JOIN dbo.BuyerDetail E ON D.PlotDetailsId=E.FlatId " +
                            " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot S On S.PaymentSchId=A.PaySchId   " +
                            " INNER JOIN dbo.LeadRegister L ON L.LeadId=D.BuyerId Where M.Approve='Y' and A.PlotDetailsId=" + argFlatId + "";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds);
                da.Dispose();
                ds.Dispose();
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

        public static DataTable GetBuyerInfo(int argFlatId, string argType, bool argPayType)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                // Code Added By Bala on 29/05/2013 Due to Paid Amount Not updated in Payment Schedule ....
                string sSql = "UPDATE PaymentScheduleFlat SET PaidAmount=TPaidAmount FROM " +
                       "PaymentScheduleFlat A JOIN (SELECT PaySchId,TPaidAmount=SUM(Amount) FROM ReceiptTrans GROUP BY PaySchId ) B " +
                       "ON A.PaymentSchId=B.PaySchId AND A.PaidAmount<>B.TPaidAmount ";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                if (BsfGlobal.g_bFADB == true)
                {
                    sSql = "DELETE FROM [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister" +
                           " WHERE EntryId=0 AND ReceiptId NOT IN (SELECT MIN(ReceiptId) FROM [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister " +
                           " GROUP BY ReceiptNo,ReferenceId,ChequeNo,CostCentreId) ";
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                if (argPayType == true)
                {
                    if (argType == "Schedule")
                    {
                        // Code Added By Bala on 29/05/2013 
                        sSql = "SELECT RefDate=PS.SchDate,RefNo='', PS.[Description],BillAmount=PS.NetAmount,PS.PaidAmount,Balance=Round(PS.NetAmount,0)-Round(PS.PaidAmount,0)," +
                               " 0 CumulativeAmount,PS.SortOrder FROM PaymentScheduleFlat PS" +
                               " LEFT JOIN dbo.AllotmentCancel AC ON PS.FlatId=AC.FlatId AND AC.Approve='Y' " +
                               " WHERE (PS.StageDetId<>0 Or PS.PaidAmount<>0 OR PS.TemplateId=0) AND PS.FlatId=" + argFlatId +
                               " AND PS.SchDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                               " ORDER BY PS.SortOrder";
                    }
                    else if (argType == "Bill")
                    {
                        // Code Added By Bala on 29/05/2013 
                        sSql = "SELECT RefDate=PB.PBDate,RefNo=PBNo, PS.[Description],BillAmount=PB.NetAmount,PS.PaidAmount,Balance=Round(PS.NetAmount,0)-Round(PS.PaidAmount,0), " +
                              " 0 CumulativeAmount,PS.SortOrder FROM PaymentScheduleFlat PS INNER JOIN ProgressBillRegister PB ON PS.PaymentSchId=PB.PaySchId " +
                              " LEFT JOIN dbo.AllotmentCancel AC ON PS.FlatId=AC.FlatId AND AC.Approve='Y' " +
                              " WHERE PS.FlatId=" + argFlatId +
                              " AND PB.PBDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                              " ORDER BY PS.SortOrder";
                    }
                }
                else
                {
                    if (argType == "Schedule")
                    {
                        // Code Added By Bala on 29/05/2013
                        sSql = "SELECT RefDate=PS.SchDate,RefNo='', PS.[Description],BillAmount=PS.NetAmount,PS.PaidAmount,Balance=Round(PS.NetAmount,0)-Round(PS.PaidAmount,0)," +
                               " 0 CumulativeAmount,PS.SortOrder FROM PaymentScheduleFlat PS" +
                               " LEFT JOIN dbo.AllotmentCancel AC ON PS.FlatId=AC.FlatId AND AC.Approve='Y' " +
                               " WHERE (PS.StageDetId<>0 Or PS.PaidAmount<>0 OR PS.TemplateId=0) AND PS.FlatId=" + argFlatId +
                               " AND PS.SchDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                               " ORDER BY PS.SortOrder";
                    }
                    else if (argType == "Bill")
                    {
                        // Code Added By Bala on 29/05/2013
                        sSql = "SELECT RefDate=PB.PBDate,RefNo=PBNo, PS.[Description],BillAmount=PB.NetAmount,PS.PaidAmount,Balance=Round(PS.NetAmount,0)-Round(PS.PaidAmount,0), " +
                               " 0 CumulativeAmount,PS.SortOrder FROM PaymentScheduleFlat PS INNER JOIN ProgressBillRegister PB ON PS.PaymentSchId=PB.PaySchId " +
                               " LEFT JOIN dbo.AllotmentCancel AC ON PS.FlatId=AC.FlatId AND AC.Approve='Y' " +
                               " WHERE PS.FlatId=" + argFlatId +
                               " AND PB.PBDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                               " ORDER BY PS.SortOrder";
                    }
                }

                if (argType.TrimEnd() != "")
                {
                    SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    dt = new DataTable();
                    sda.Fill(dt);
                    sda.Dispose();
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

        public static DataTable GetFlatType()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            try
            {
                sda = new SqlDataAdapter("SELECT FlatTypeId,TypeName,ProjId FROM dbo.FlatType", BsfGlobal.g_CRMDB);
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

        public static DataTable GetInterest(int argFlatId, string argType, DateTime argAsOn, int argCreditDays)
        {
            DataTable dt = null;
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            try
            {
                if (argType == "B")
                {
                    sSql = " Select FlatId,PaymentSchId,SortOrder,[Date],[Description],AsOnDate,Receivable,Received,CreditDays,IntPercent,FinaliseDate,[Type] FROM( " +
                            " Select DISTINCT S.FlatId,S.PaymentSchId,S.SortOrder,A.PBDate [Date],A.AsOnDate,S.[Description]," +
                            " A.NetAmount Receivable,0 Received,D.CreditDays,D.IntPercent,E.FinaliseDate,'P' [Type] From dbo.ProgressBillRegister A" +
                            " INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                             " INNER JOIN dbo.ProgressBillMaster M On M.ProgRegId=A.ProgRegId INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId " +
                             " LEFT JOIN dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId" +
                             " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                             " Where M.Approve='Y' And A.FlatId=" + argFlatId + " And S.BillPassed=1 And A.PBDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' " +
                             " AND A.PBDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                             " UNION ALL " +
                             " Select DISTINCT S.FlatId,S.PaymentSchId,S.SortOrder,RR.ReceiptDate [Date],NULL AsOnDate,RR.Narration [Description]," +
                             " 0 Receivable,RT.Amount Received,D.CreditDays,D.IntPercent,E.FinaliseDate,'R' [Type] " +
                             " From dbo.ProgressBillRegister A INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                             " INNER JOIN dbo.ProgressBillMaster M On M.ProgRegId=A.ProgRegId INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId " +
                             " Left JOIN dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId " +
                             " INNER JOIN dbo.ReceiptTrans RT ON RT.PaySchId=S.PaymentSchId " +
                             " INNER JOIN dbo.ReceiptRegister RR ON RR.ReceiptId=RT.ReceiptId And RR.ReceiptDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' " +
                             " Where M.Approve='Y' And A.FlatId=" + argFlatId + " AND RT.CancelDate IS NULL AND S.BillPassed=1" +
                             " AND A.PBDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' " +
                             " ) X Order By X.SortOrder,X.[Type],X.[Date]";
                }
                else
                {
                    sSql = " Select FlatId,PaymentSchId,SortOrder,[Date],[Description],Receivable,Received,CreditDays,IntPercent,FinaliseDate,[Type] FROM( " +
                             " Select DISTINCT A.FlatId,A.PaymentSchId,A.SortOrder,F.CompletionDate [Date],A.[Description],A.NetAmount Receivable,0 Received,D.CreditDays,D.IntPercent,E.FinaliseDate,'P' [Type] " +
                             " From dbo.PaymentScheduleFlat A INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                             " INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId" +
                             " INNER JOIN dbo.StageDetails F ON F.StageDetId=A.StageDetId" +
                             " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                             " Where A.FlatId=" + argFlatId + " And A.StageDetId>0 And A.SchDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' " +
                             " AND F.CompletionDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                             " UNION ALL" +
                             " Select DISTINCT A.FlatId,A.PaymentSchId,A.SortOrder,RR.ReceiptDate [Date],RR.Narration [Description],0 Receivable,RT.Amount Received,D.CreditDays,D.IntPercent,E.FinaliseDate,'R' [Type] " +
                             " From dbo.PaymentScheduleFlat A INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                             " INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId " +
                             " INNER JOIN dbo.StageDetails F ON F.StageDetId=A.StageDetId" +
                             " INNER JOIN dbo.ReceiptTrans RT ON RT.PaySchId=A.PaymentSchId " +
                             " INNER JOIN dbo.ReceiptRegister RR ON RR.ReceiptId=RT.ReceiptId And RR.ReceiptDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' " +
                             " Where A.FlatId=" + argFlatId + " AND RT.CancelDate IS NULL AND A.StageDetId>0 AND A.SchDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' " +
                             " ) X Order By X.SortOrder,X.[Type],X.[Date]";
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

        public static DataTable GetSOA(int argFlatId, string argType, DateTime argAsOn, int argCreditDays)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            try
            {
                if (argType == "B")
                {
                    sSql = " Select SortOrder,Date,BillNo,Description,Receivable,Received,CreditDays,IntPercent FROM( " +
                            " Select S.SortOrder,A.PBDate Date,A.PBNo BillNo,S.Description,A.NetAmount Receivable,0 Received,D.CreditDays,D.IntPercent " +
                             " From dbo.ProgressBillRegister A INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                             " INNER JOIN dbo.ProgressBillMaster M On M.ProgRegId=A.ProgRegId INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId " +
                             " Left JOIN dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId " +
                             " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                             " Where M.Approve='Y' And A.FlatId=" + argFlatId + " And S.BillPassed=1 And A.PBDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' " +
                             " AND A.PBDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                             " UNION ALL " +
                             " Select S.SortOrder,RR.ReceiptDate Date,RR.ReceiptNo BillNo,RR.Narration Description,0 Receivable,RT.Amount Received,D.CreditDays,D.IntPercent " +
                             " From dbo.ProgressBillRegister A INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                             " INNER JOIN dbo.ProgressBillMaster M On M.ProgRegId=A.ProgRegId INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId " +
                             " Left JOIN dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId " +
                             " INNER JOIN dbo.ReceiptTrans RT ON RT.PaySchId=S.PaymentSchId And RT.Amount<>0 " +
                             " INNER JOIN dbo.ReceiptRegister RR ON RR.ReceiptId=RT.ReceiptId And RR.ReceiptDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' " +
                             " Where M.Approve='Y' AND RT.CancelDate IS NULL And A.FlatId=" + argFlatId + " And S.BillPassed=1 " +
                             " And A.PBDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' " +
                             " ) X Order By X.SortOrder";
                }
                else
                {
                    sSql = "  Select SortOrder,Date,BillNo,Description,Receivable,Received,CreditDays,IntPercent FROM( " +
                             " Select A.SortOrder,A.SchDate Date,'' BillNo,A.Description,A.NetAmount Receivable,0 Received,D.CreditDays,D.IntPercent " +
                             " From dbo.PaymentScheduleFlat A INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                             " INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId " +
                             " INNER JOIN dbo.StageDetails F ON F.StageDetId=A.StageDetId" +
                             " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                             " Where A.FlatId=" + argFlatId + " And A.PaidAmount>0 And A.BillPassed=0 And A.SchDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' " +
                             " AND F.CompletionDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                             " UNION ALL" +
                             " Select A.SortOrder,RR.ReceiptDate Date,RR.ReceiptNo BillNo,RR.Narration Description,0 Receivable,RT.Amount Received,D.CreditDays,D.IntPercent " +
                             " From dbo.PaymentScheduleFlat A INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                             " INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId " +
                             " INNER JOIN dbo.ReceiptTrans RT ON RT.PaySchId=A.PaymentSchId And RT.Amount<>0 " +
                             " INNER JOIN dbo.ReceiptRegister RR ON RR.ReceiptId=RT.ReceiptId And RR.ReceiptDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' " +
                             " Where A.FlatId=" + argFlatId + " And A.PaidAmount>0 AND RT.CancelDate IS NULL And A.BillPassed=0 " +
                             " And A.SchDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' " +
                             " ) X Order By X.SortOrder";
                }
                //sSql = "Select A.AsOnDate Date,A.NetAmount Payable,D.Amount Paid,Datediff(DD,A.AsOnDate,D.ReceiptDate)Days," +
                //        " F.CreditDays,(Datediff(DD,A.AsOnDate,D.ReceiptDate)-F.CreditDays) InterestCallDays, " +
                //        " ((Datediff(DD,A.AsOnDate,D.ReceiptDate)-F.CreditDays)*A.NetAmount*(Datediff(DD,A.AsOnDate,D.ReceiptDate)/365)) IntPayable" +
                //        " From ProgressBillRegister A Inner Join ProgressBillMaster B on A.ProgRegId=B.ProgRegId  " +
                //        " Inner Join dbo.PaymentScheduleFlat C On A.PaySchId=C.PaymentSchId And A.FlatId=C.FlatId  " +
                //        " Inner Join dbo.ReceiptRegister D On A.LeadId=D.LeadId And A.CostCentreId=D.CostCentreId" +
                //        " Inner Join FlatDetails F On F.FlatId=A.FlatId Where B.Approve='Y' And A.FlatId=" + argFlatId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();

                //DataRow dr;
                //dr = dt.NewRow();
                //dr["Date"] = argAsOn.ToString("dd-MMM-yyyy");
                //dr["Description"] = "Interest Payable";
                //dr["Receivable"] = 0;
                //dr["Received"] = 0;
                //dr["CreditDays"] = 0;
                //dr["IntPercent"] = 0;
                ////dr["Balance"] = 0;
                ////dr["Days"] = argCreditDays;
                ////dr["Interest"] = 0;//decimal.Round((dBal * dIntPer / 36500) * argCreditDays, 3); 

                //dt.Rows.Add(dr);
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

        public static DataSet GetSOADet(int argFlatId, DateTime argAsOn)
        {
            BsfGlobal.OpenCRMDB();
            DataSet ds = new DataSet();
            try
            {
                string sSql = "Update dbo.PaymentScheduleFlat Set Amount=NetAmount Where FlatId=" + argFlatId + " And SchType='A' And Amount=0";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "SELECT DISTINCT A.SortOrder,A.PaymentSchId, A.[Description], " +
                        " CASE When PBDate IS NULL THEN CONVERT(varchar(10),SchDate,103) Else CONVERT(varchar(10),PBDate,103) End SchDate, " +
                        " CONVERT(Varchar(10),B.FinaliseDate,103) FinaliseDate,ISNULL(A.Amount,0) Amount, " +
                        " CAST(0 as decimal(18,3)) as Interest,CAST(0 as int) as Age, " +
                        " Tax=(SELECT ISNULL(SUM(A1.Amount),0) Amount FROM dbo.FlatReceiptQualifier A1 " +
                        " INNER JOIN dbo.FlatReceiptType A2 ON A1.SchId=A2.SchId " +
                        " WHERE A.PaymentSchId=A2.PaymentSchId), " +
                        " A.NetAmount,F.CreditDays,F.IntPercent,A.SchType," +
                        " AdvSchType=Isnull((Select X.SchType From dbo.FlatReceiptType X Where X.FlatId=F.FlatId And X.SchType='A' And X.PaymentSchId=A.PaymentSchId),''), " +
                        " ISNULL(AC.CancelDate, '') CancelDate, AC.Approve FROM dbo.PaymentScheduleFlat A " +
                        " INNER JOIN dbo.BuyerDetail B On A.FlatId=B.FlatId " +
                        " INNER JOIN dbo.FlatDetails F ON F.FlatId=B.FlatId " +
                        " LEFT JOIN dbo.ProgressBillRegister PR ON PR.PaySchId=A.PaymentSchId " +
                        " LEFT JOIN dbo.AllotmentCancel AC ON PR.FlatId=AC.FlatId AND AC.Approve='Y' " +
                        " WHERE A.FlatId=" + argFlatId + " " +//And A.SchDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' " +
                        " ORDER BY A.SortOrder ASC";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                ds = new DataSet();
                sda.Fill(ds, "PaymentSch");
                sda.Dispose();

                sSql = "SELECT PaySchId,B.ReceiptDate,isnull(A.Amount,0) PaidGrossAmount,Cast(0  as decimal(18,3)) PaidTaxAmount,cast(0 as decimal(18,3)) as PaidInterest," +
                        " cast(0 as decimal(18,3)) Balance FROM dbo.ReceiptTrans A " +
                        " INNER JOIN dbo.ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                        " WHERE A.FlatId=" + argFlatId + " AND B.ReceiptDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' AND ReceiptType='Advance' " +
                        " AND A.CancelDate IS NULL" +
                        " UNION ALL " +
                        " SELECT PaymentSchId,ReceiptDate,PaidGrossAmount,PaidTaxAmount,PaidInterest,cast(0 as decimal(18,3)) Balance FROM( " +
                        " SELECT PaymentSchId,B.ReceiptDate,isnull(SUM(PaidGrossAmount),0) PaidGrossAmount, " +
                        " isnull(SUM(PaidTaxAmount),0) PaidTaxAmount,cast(0 as decimal(18,3)) PaidInterest " +
                        " FROM dbo.ReceiptShTrans A  INNER JOIN dbo.ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                        " WHERE A.FlatId=" + argFlatId + "  AND B.CancelDate IS NULL " +//AND B.ReceiptDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' "+
                        " AND PaidNetAmount<>0 and ReceiptTypeId<>1 " +
                        " GROUP BY PaymentSchId,B.ReceiptDate)A";
                //" ORDER BY PaySchId,ReceiptDate DESC";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Receivable");
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
            return ds;
        }

        public static DataSet GetPBSOADet(int argFlatId, DateTime argAsOn)
        {
            DataSet dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            string frmdat = string.Format("{0:dd MMM yyyy}", argAsOn);
            string sSql = "";
            try
            {
                sSql = "Update PaymentScheduleFlat Set Amount=NetAmount Where FlatId=" + argFlatId + " And SchType='A' And Amount=0";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();


                sSql = "SELECT A.SortOrder,PaymentSchId, [Description] ,convert(varchar(10),SchDate,103) SchDate,Convert(Varchar(10), " +
                        " B.FinaliseDate,103) FinaliseDate,isnull(Amount,0) Amount,cast(0 as decimal(18,3)) as Interest,cast(0 as int) as Age,Tax=(SELECT  isnull(SUM(A1.Amount),0) Amount " +
                        " FROM FlatReceiptQualifier A1 INNER JOIN dbo.FlatReceiptType A2 ON A1.SchId=A2.SchId WHERE A.PaymentSchId=A2.PaymentSchId),NetAmount,F.CreditDays,F.IntPercent,A.SchType," +
                        " AdvSchType=Isnull((Select X.SchType From FlatReceiptType X Where X.FlatId=F.FlatId And X.SchType='A' And X.PaymentSchId=A.PaymentSchId),'') " +
                        " FROM PaymentScheduleFlat A INNER JOIN BuyerDetail B On A.FlatId=B.FlatId " +
                        " INNER JOIN FlatDetails F ON F.FlatId=B.FlatId " +
                        " WHERE A.FlatId=" + argFlatId + " " +//And A.SchDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' " +
                        " ORDER BY A.SortOrder ASC";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataSet();
                sda.Fill(dt, "PaymentSch");

                sSql = "SELECT PaySchId,B.ReceiptDate,isnull(A.Amount,0) PaidGrossAmount,Cast(0  as decimal(18,3)) PaidTaxAmount,cast(0 as decimal(18,3)) as PaidInterest,cast(0 as decimal(18,3)) Balance FROM ReceiptTrans A  INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                        "WHERE FlatId=" + argFlatId + " AND B.ReceiptDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' AND ReceiptType='Advance' " +
                        "UNION ALL " +
                        "SELECT PaySchId,B.ReceiptDate,isnull(A.Amount,0) PaidGrossAmount,Cast(0  as decimal(18,3)) PaidTaxAmount, " +
                        " cast(0 as decimal(18,3)) as PaidInterest,cast(0 as decimal(18,3)) Balance FROM ReceiptTrans A  " +
                        " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId WHERE FlatId=" + argFlatId + " AND B.ReceiptDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' " +
                        " AND ReceiptType<>'Advance' ";

                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dt, "Receivable");
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

        public static DataTable GetReceiptTypeTrans(int argFlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            sSql = "Select RR.ReceiptDate,A.FlatId,A.PaymentSchId,A.ReceiptTypeId,A.OtherCostId,Case When A.ReceiptTypeId <>0 then B.ReceiptTypeName Else C.OtherCostName End Description,A.GrossAmount, A.TaxAmount,A.NetAmount,A.PaidAmount," +
                  " (A.GrossAmount-A.PaidGrossAmount)BalGross,(A.TaxAmount-A.PaidTaxAmount)BalTax, " +
                  " A.PaidGrossAmount,A.PaidTaxAmount,A.PaidNetAmount from ReceiptShTrans A " +
                  "Left join ReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId " +
                  "Left join OtherCostMaster C on A.OtherCostId=C.OtherCostId " +
                  "Inner Join ReceiptRegister RR On RR.ReceiptId=A.ReceiptId " +
                  "Where A.FlatId= " + argFlatId + " And A.PaidNetAmount<>0";
            sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            dt = new DataTable();
            sda.Fill(dt);
            sda.Dispose();
            BsfGlobal.g_CRMDB.Close();
            return dt;
        }

        public static DataSet GetFlatReceiptType(int argCCId, int argFlatId, DateTime argDate)
        {
            DataSet ds = new DataSet();
            string sSql = "";

            cRateQualR RAQual = new cRateQualR();
            Collection QualVBC = new Collection();

            BsfGlobal.OpenCRMDB();

            sSql = "Update FlatReceiptType Set Amount = NetAmount where Amount=0 and NetAmount>0";
            SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            SqlDataReader dr;

            int iPaySchId = 0;
            int iQualId = 0; string sQualName = "";
            int iFlatId = argFlatId;

            decimal dTNetAmt = 0;
            decimal dTTaxAmt = 0;
            int iReceiptId = 0;
            int iOthId = 0;
            decimal dAdvAmt = 0;

            decimal dRTAmt = 0;

            decimal dQNetAmt = 0;
            decimal dTaxAmt = 0;



            DataRow dRowT;

            DataTable dtR = new DataTable();
            //dtR.Columns.Add("FlatId", typeof(Int32));
            //dtR.Columns.Add("PaymentSchId", typeof(Int32));
            dtR.Columns.Add("ReceiptTypeId", typeof(Int32));
            dtR.Columns.Add("OtherCostId", typeof(Int32));
            dtR.Columns.Add("Description", typeof(string));
            dtR.Columns.Add("Gross", typeof(Decimal));
            dtR.Columns.Add("PaidGross", typeof(Decimal));
            dtR.Columns.Add("Balance", typeof(Decimal));
            dtR.Columns.Add("TaxAmount", typeof(Decimal));
            dtR.Columns.Add("NetAmount", typeof(Decimal));

            DataTable dtQualifier = new DataTable();
            dtQualifier.Columns.Add("FlatId", typeof(int));
            dtQualifier.Columns.Add("PaymentSchId", typeof(int));
            dtQualifier.Columns.Add("QualifierId", typeof(int));
            dtQualifier.Columns.Add("Expression", typeof(string));
            dtQualifier.Columns.Add("ExpPer", typeof(decimal));
            dtQualifier.Columns.Add("NetPer", typeof(decimal));
            dtQualifier.Columns.Add("Add_Less_Flag", typeof(string));
            dtQualifier.Columns.Add("SurCharge", typeof(decimal));
            dtQualifier.Columns.Add("EDCess", typeof(decimal));
            dtQualifier.Columns.Add("HEDPer", typeof(decimal));
            dtQualifier.Columns.Add("ExpValue", typeof(decimal));
            dtQualifier.Columns.Add("ExpPerValue", typeof(decimal));
            dtQualifier.Columns.Add("SurValue", typeof(decimal));
            dtQualifier.Columns.Add("EDValue", typeof(decimal));
            dtQualifier.Columns.Add("Amount", typeof(decimal));
            dtQualifier.Columns.Add("ReceiptTypeId", typeof(int));
            dtQualifier.Columns.Add("OtherCostId", typeof(int));
            dtQualifier.Columns.Add("Description", typeof(string));

            DataTable dtTax = new DataTable();
            dtTax.Columns.Add("ReceiptTypeId", typeof(Int32));
            dtTax.Columns.Add("OtherCostId", typeof(Int32));
            dtTax.Columns.Add("Description", typeof(string));
            dtTax.Columns.Add("Gross", typeof(Decimal));
            dtTax.Columns.Add("PaidGross", typeof(Decimal));
            dtTax.Columns.Add("Balance", typeof(Decimal));
            dtTax.Columns.Add("TaxAmount", typeof(Decimal));
            dtTax.Columns.Add("NetAmount", typeof(Decimal));

            sSql = "Select A.ReceiptTypeId,A.OtherCostId,Case When A.ReceiptTypeId <>0 then B.ReceiptTypeName Else C.OtherCostName End Description,SUM(A.Amount)Amount from FlatReceiptType A " +
                      "Left Join ReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId " +
                      "Left Join OtherCostMaster C on A.OtherCostId=C.OtherCostId " +
                      "Where A.FlatId= " + iFlatId + " Group By A.ReceiptTypeId,A.OtherCostId,Case When A.ReceiptTypeId <>0 then B.ReceiptTypeName Else C.OtherCostName End Order By A.OtherCostId";
            cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);

            dr = cmd.ExecuteReader();
            DataTable dtG = new DataTable();
            dtG.Load(dr);
            dr.Close();
            cmd.Dispose();

            DataRow drG;
            for (int i = 0; i < dtG.Rows.Count; i++)
            {
                drG = dtTax.NewRow();
                drG["ReceiptTypeId"] = Convert.ToInt32(dtG.Rows[i]["ReceiptTypeId"]);
                drG["OtherCostId"] = Convert.ToInt32(dtG.Rows[i]["OtherCostId"]);
                drG["Description"] = dtG.Rows[i]["Description"].ToString();
                drG["Gross"] = Convert.ToDecimal(dtG.Rows[i]["Amount"]);
                drG["PaidGross"] = 0;
                drG["Balance"] = 0;
                drG["TaxAmount"] = 0;
                drG["NetAmount"] = 0;
                dtTax.Rows.Add(drG);
            }

            sSql = "Select A.ReceiptTypeId,A.OtherCostId,Case When A.ReceiptTypeId <>0 then B.ReceiptTypeName Else C.OtherCostName End Description," +
                    " SUM(PaidGrossAmount)PaidGross From dbo.ReceiptShTrans A " +
                    " Left Join ReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId " +
                    " Left Join OtherCostMaster C on A.OtherCostId=C.OtherCostId Where A.FlatId= " + iFlatId + " " +
                    " Group By A.ReceiptTypeId,A.OtherCostId,Case When A.ReceiptTypeId <>0 then B.ReceiptTypeName Else C.OtherCostName End " +
                    " Order By A.OtherCostId";
            cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);

            dr = cmd.ExecuteReader();
            DataTable dtP = new DataTable();
            dtP.Load(dr);
            dr.Close();
            cmd.Dispose();

            DataRow[] drR;

            for (int m = 0; m < dtP.Rows.Count; m++)
            {
                drR = dtTax.Select("ReceiptTypeId=" + Convert.ToInt32(dtP.Rows[m]["ReceiptTypeId"]) + " And OtherCostId=" + Convert.ToInt32(dtP.Rows[m]["OtherCostId"]) + "");
                if (drR.Length > 0)
                {
                    drR[0]["PaidGross"] = Convert.ToDecimal(dtP.Rows[m]["PaidGross"]);
                    drR[0]["Balance"] = Convert.ToDecimal(drR[0]["Gross"]) - Convert.ToDecimal(dtP.Rows[m]["PaidGross"]);
                }
            }
            dtTax.AcceptChanges();

            DataTable dt = new DataTable();
            dt = dtTax;

            for (int j = 0; j < dtTax.Rows.Count; j++)
            {

                dRowT = dtR.NewRow();
                dRTAmt = 0;

                iReceiptId = Convert.ToInt32(dtTax.Rows[j]["ReceiptTypeId"]);
                iOthId = Convert.ToInt32(dtTax.Rows[j]["OtherCostId"]);

                DataTable dtTN;

                dRowT["ReceiptTypeId"] = iReceiptId;
                dRowT["OtherCostId"] = iOthId;
                //Description
                sSql = "Select P.Description From dbo.ReceiptShTrans A  " +
                        " Left Join dbo.PaymentScheduleFlat P On P.PaymentSchId=A.PaymentSchId" +
                        " Left Join ReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId  " +
                        " Left Join OtherCostMaster C on A.OtherCostId=C.OtherCostId " +
                        " Where A.FlatId=" + iFlatId + " And A.ReceiptTypeId= " + iReceiptId + " And A.OtherCostId= " + iOthId + " And P.CostCentreId = " + argCCId;
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                dtTN = new DataTable();
                dtTN.Load(dr);
                dr.Close();
                cmd.Dispose();
                if (dtTN.Rows.Count > 0) { dRowT["Description"] = CommFun.IsNullCheck(dtTN.Rows[0]["Description"], CommFun.datatypes.vartypestring).ToString() + "(" + CommFun.IsNullCheck(dtTax.Rows[j]["Description"], CommFun.datatypes.vartypestring).ToString() + ")"; }
                else
                    dRowT["Description"] = CommFun.IsNullCheck(dtTax.Rows[j]["Description"], CommFun.datatypes.vartypestring).ToString();
                dRowT["Gross"] = Convert.ToDecimal(CommFun.IsNullCheck(dtTax.Rows[j]["Gross"], CommFun.datatypes.vartypenumeric));
                dRowT["PaidGross"] = Convert.ToDecimal(CommFun.IsNullCheck(dtTax.Rows[j]["PaidGross"], CommFun.datatypes.vartypenumeric));


                //sSql = "Select QualifierId from CCReceiptQualifier Where ReceiptTypeId= " + iReceiptId + " and OtherCostId= " + iOthId + " and CostCentreId = " + argCCId;
                sSql = "Select A.QualifierId,B.QualifierName From dbo.CCReceiptQualifier A" +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp B On A.QualifierId=B.QualifierId" +
                        " Where ReceiptTypeId= " + iReceiptId + " and OtherCostId= " + iOthId + " and CostCentreId = " + argCCId;
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                DataTable dtTT = new DataTable();
                dtTT.Load(dr);
                dr.Close();
                cmd.Dispose();

                dQNetAmt = 0;
                decimal dQBaseAmt = Convert.ToDecimal(dtTax.Rows[j]["Balance"]);

                if (dtTT.Rows.Count == 0) { dTNetAmt = dTNetAmt + dQBaseAmt; }

                for (int k = 0; k < dtTT.Rows.Count; k++)
                {
                    iQualId = Convert.ToInt32(dtTT.Rows[k]["QualifierId"]);
                    sQualName = dtTT.Rows[k]["QualifierName"].ToString();

                    RAQual = new cRateQualR();
                    QualVBC = new Collection();

                    DataTable dtQ = new DataTable();
                    dtQ = GetQual(iQualId, argDate);
                    if (dtQ.Rows.Count > 0)
                    {
                        RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                        RAQual.Amount = 0;
                        RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                        RAQual.RateID = iQualId;
                        RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                        RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                        RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                        RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                        RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                        RAQual.HEDValue = 0;
                    }

                    QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);

                    Qualifier.frmQualifier qul = new Qualifier.frmQualifier();

                    dQNetAmt = 0;
                    dTaxAmt = 0;

                    DataRow dr1;

                    decimal dVATAmt = 0;
                    if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, argDate, ref dVATAmt) == true)
                    {
                        dRTAmt = dRTAmt + dTaxAmt;

                        dTNetAmt = dTNetAmt + dQNetAmt;
                        dTTaxAmt = dTTaxAmt + dTaxAmt;

                        foreach (Qualifier.cRateQualR d in QualVBC)
                        {
                            dr1 = dtQualifier.NewRow();

                            dr1["FlatId"] = iFlatId;
                            dr1["PaymentSchId"] = iPaySchId;
                            dr1["QualifierId"] = d.RateID;
                            dr1["Expression"] = d.Expression;
                            dr1["ExpPer"] = d.ExpPer;
                            dr1["NetPer"] = d.NetPer;
                            dr1["Add_Less_Flag"] = d.Add_Less_Flag;
                            dr1["SurCharge"] = d.SurPer;
                            dr1["EDCess"] = d.EDPer;
                            dr1["HEDPer"] = d.HEDPer;
                            dr1["ExpValue"] = d.ExpValue;
                            dr1["ExpPerValue"] = d.ExpPerValue;
                            dr1["SurValue"] = d.SurValue;
                            dr1["EDValue"] = d.EDValue;
                            dr1["Amount"] = d.Amount;
                            dr1["ReceiptTypeId"] = iReceiptId;
                            dr1["OtherCostId"] = iOthId;
                            dr1["Description"] = sQualName + " " + d.NetPer;

                            dtQualifier.Rows.Add(dr1);
                        }

                    }
                }
                if (dQNetAmt == 0) { dQNetAmt = dQBaseAmt; }

                if (iReceiptId != 1)
                {
                    dRowT["Balance"] = dQBaseAmt;
                    dRowT["TaxAmount"] = dRTAmt;

                    dRowT["NetAmount"] = dQNetAmt;
                }
                dtR.Rows.Add(dRowT);

            }

            //drow["NetAmount"] = dTNetAmt - dAdvAmt;
            //    drow["BalanceAmount"] = dTNetAmt - Convert.ToDecimal(drow["PaidAmount"]) - dAdvAmt;

            //}

            ds.Tables.Add(dtR);
            ds.Tables.Add(dtQualifier);
            //ds.Tables.Add(dtTax);

            return ds;
        }

        public static DataSet GetAsOnFlatReceiptType(int argCCId, int argFlatId, DateTime argDate)
        {
            DataSet ds = new DataSet();
            string sSql = "";

            cRateQualR RAQual = new cRateQualR();
            Collection QualVBC = new Collection();

            BsfGlobal.OpenCRMDB();

            sSql = "Update FlatReceiptType Set Amount = NetAmount where Amount=0 and NetAmount>0";
            SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            SqlDataReader dr;

            int iPaySchId = 0;
            int iQualId = 0; string sQualName = "";
            int iFlatId = argFlatId;

            decimal dTNetAmt = 0;
            decimal dTTaxAmt = 0;
            int iReceiptId = 0;
            int iOthId = 0;
            decimal dAdvAmt = 0;

            decimal dRTAmt = 0;

            decimal dQNetAmt = 0;
            decimal dTaxAmt = 0;



            DataRow dRowT;

            DataTable dtR = new DataTable();
            //dtR.Columns.Add("FlatId", typeof(Int32));
            //dtR.Columns.Add("PaymentSchId", typeof(Int32));
            dtR.Columns.Add("ReceiptTypeId", typeof(Int32));
            dtR.Columns.Add("OtherCostId", typeof(Int32));
            dtR.Columns.Add("Description", typeof(string));
            dtR.Columns.Add("Gross", typeof(Decimal));
            dtR.Columns.Add("PaidGross", typeof(Decimal));
            dtR.Columns.Add("Balance", typeof(Decimal));
            dtR.Columns.Add("TaxAmount", typeof(Decimal));
            dtR.Columns.Add("NetAmount", typeof(Decimal));

            DataTable dtQualifier = new DataTable();
            dtQualifier.Columns.Add("FlatId", typeof(int));
            dtQualifier.Columns.Add("PaymentSchId", typeof(int));
            dtQualifier.Columns.Add("QualifierId", typeof(int));
            dtQualifier.Columns.Add("Expression", typeof(string));
            dtQualifier.Columns.Add("ExpPer", typeof(decimal));
            dtQualifier.Columns.Add("NetPer", typeof(decimal));
            dtQualifier.Columns.Add("Add_Less_Flag", typeof(string));
            dtQualifier.Columns.Add("SurCharge", typeof(decimal));
            dtQualifier.Columns.Add("EDCess", typeof(decimal));
            dtQualifier.Columns.Add("HEDPer", typeof(decimal));
            dtQualifier.Columns.Add("ExpValue", typeof(decimal));
            dtQualifier.Columns.Add("ExpPerValue", typeof(decimal));
            dtQualifier.Columns.Add("SurValue", typeof(decimal));
            dtQualifier.Columns.Add("EDValue", typeof(decimal));
            dtQualifier.Columns.Add("Amount", typeof(decimal));
            dtQualifier.Columns.Add("ReceiptTypeId", typeof(int));
            dtQualifier.Columns.Add("OtherCostId", typeof(int));
            dtQualifier.Columns.Add("Description", typeof(string));

            DataTable dtTax = new DataTable();
            dtTax.Columns.Add("ReceiptTypeId", typeof(Int32));
            dtTax.Columns.Add("OtherCostId", typeof(Int32));
            dtTax.Columns.Add("Description", typeof(string));
            dtTax.Columns.Add("Gross", typeof(Decimal));
            dtTax.Columns.Add("PaidGross", typeof(Decimal));
            dtTax.Columns.Add("Balance", typeof(Decimal));
            dtTax.Columns.Add("TaxAmount", typeof(Decimal));
            dtTax.Columns.Add("NetAmount", typeof(Decimal));

            sSql = "Select A.ReceiptTypeId,A.OtherCostId,Case When A.ReceiptTypeId <>0 then B.ReceiptTypeName Else C.OtherCostName End Description,SUM(A.Amount)Amount from FlatReceiptType A " +
                      " Left Join ReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId " +
                      " Left Join OtherCostMaster C on A.OtherCostId=C.OtherCostId " +
                      " Left Join dbo.PaymentScheduleFlat P On P.PaymentSchId=A.PaymentSchId " +
                      " Where A.FlatId= " + iFlatId + " And P.SchDate<='" + Convert.ToDateTime(argDate).ToString("dd-MMM-yyyy") + "'" +
                      " Group By A.ReceiptTypeId,A.OtherCostId,Case When A.ReceiptTypeId <>0 then B.ReceiptTypeName Else C.OtherCostName End Order By A.OtherCostId";
            cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);

            dr = cmd.ExecuteReader();
            DataTable dtG = new DataTable();
            dtG.Load(dr);
            dr.Close();
            cmd.Dispose();

            DataRow drG;
            for (int i = 0; i < dtG.Rows.Count; i++)
            {
                drG = dtTax.NewRow();
                drG["ReceiptTypeId"] = Convert.ToInt32(dtG.Rows[i]["ReceiptTypeId"]);
                drG["OtherCostId"] = Convert.ToInt32(dtG.Rows[i]["OtherCostId"]);
                drG["Description"] = dtG.Rows[i]["Description"].ToString();
                drG["Gross"] = Convert.ToDecimal(dtG.Rows[i]["Amount"]);
                drG["PaidGross"] = 0;
                drG["Balance"] = 0;
                drG["TaxAmount"] = 0;
                drG["NetAmount"] = 0;
                dtTax.Rows.Add(drG);
            }

            sSql = "Select A.ReceiptTypeId,A.OtherCostId,Case When A.ReceiptTypeId <>0 then B.ReceiptTypeName Else C.OtherCostName End Description," +
                    " SUM(PaidGrossAmount)PaidGross From dbo.ReceiptShTrans A " +
                    " Left Join ReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId " +
                    " Left Join OtherCostMaster C on A.OtherCostId=C.OtherCostId " +
                    " Where A.FlatId= " + iFlatId + " " +
                    " Group By A.ReceiptTypeId,A.OtherCostId,Case When A.ReceiptTypeId <>0 then B.ReceiptTypeName Else C.OtherCostName End " +
                    " Order By A.OtherCostId";
            cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);

            dr = cmd.ExecuteReader();
            DataTable dtP = new DataTable();
            dtP.Load(dr);
            dr.Close();
            cmd.Dispose();

            DataRow[] drR;

            for (int m = 0; m < dtP.Rows.Count; m++)
            {
                drR = dtTax.Select("ReceiptTypeId=" + Convert.ToInt32(dtP.Rows[m]["ReceiptTypeId"]) + " And OtherCostId=" + Convert.ToInt32(dtP.Rows[m]["OtherCostId"]) + "");
                if (drR.Length > 0)
                {
                    drR[0]["PaidGross"] = Convert.ToDecimal(dtP.Rows[m]["PaidGross"]);
                    drR[0]["Balance"] = Convert.ToDecimal(drR[0]["Gross"]) - Convert.ToDecimal(dtP.Rows[m]["PaidGross"]);
                }
            }
            dtTax.AcceptChanges();

            DataTable dt = new DataTable();
            dt = dtTax;

            for (int j = 0; j < dtTax.Rows.Count; j++)
            {

                dRowT = dtR.NewRow();
                dRTAmt = 0;

                iReceiptId = Convert.ToInt32(dtTax.Rows[j]["ReceiptTypeId"]);
                iOthId = Convert.ToInt32(dtTax.Rows[j]["OtherCostId"]);

                DataTable dtTN;

                dRowT["ReceiptTypeId"] = iReceiptId;
                dRowT["OtherCostId"] = iOthId;
                //Description
                sSql = "Select P.Description From dbo.ReceiptShTrans A  " +
                        " Left Join dbo.PaymentScheduleFlat P On P.PaymentSchId=A.PaymentSchId" +
                        " Left Join ReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId  " +
                        " Left Join OtherCostMaster C on A.OtherCostId=C.OtherCostId " +
                        " Where A.FlatId=" + iFlatId + " And A.ReceiptTypeId= " + iReceiptId + " And A.OtherCostId= " + iOthId + " And P.CostCentreId = " + argCCId;
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                dtTN = new DataTable();
                dtTN.Load(dr);
                dr.Close();
                cmd.Dispose();
                if (dtTN.Rows.Count > 0) { dRowT["Description"] = CommFun.IsNullCheck(dtTN.Rows[0]["Description"], CommFun.datatypes.vartypestring).ToString() + "(" + CommFun.IsNullCheck(dtTax.Rows[j]["Description"], CommFun.datatypes.vartypestring).ToString() + ")"; }
                else
                    dRowT["Description"] = CommFun.IsNullCheck(dtTax.Rows[j]["Description"], CommFun.datatypes.vartypestring).ToString();
                dRowT["Gross"] = Convert.ToDecimal(CommFun.IsNullCheck(dtTax.Rows[j]["Gross"], CommFun.datatypes.vartypenumeric));
                dRowT["PaidGross"] = Convert.ToDecimal(CommFun.IsNullCheck(dtTax.Rows[j]["PaidGross"], CommFun.datatypes.vartypenumeric));


                //sSql = "Select QualifierId from CCReceiptQualifier Where ReceiptTypeId= " + iReceiptId + " and OtherCostId= " + iOthId + " and CostCentreId = " + argCCId;
                sSql = "Select A.QualifierId,B.QualifierName From dbo.CCReceiptQualifier A" +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp B On A.QualifierId=B.QualifierId" +
                        " Where ReceiptTypeId= " + iReceiptId + " and OtherCostId= " + iOthId + " and CostCentreId = " + argCCId;
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                DataTable dtTT = new DataTable();
                dtTT.Load(dr);
                dr.Close();
                cmd.Dispose();

                dQNetAmt = 0;
                decimal dQBaseAmt = Convert.ToDecimal(dtTax.Rows[j]["Balance"]);

                if (dtTT.Rows.Count == 0) { dTNetAmt = dTNetAmt + dQBaseAmt; }

                for (int k = 0; k < dtTT.Rows.Count; k++)
                {
                    iQualId = Convert.ToInt32(dtTT.Rows[k]["QualifierId"]);
                    sQualName = dtTT.Rows[k]["QualifierName"].ToString();

                    RAQual = new cRateQualR();
                    QualVBC = new Collection();

                    DataTable dtQ = new DataTable();
                    dtQ = GetQual(iQualId, argDate);
                    if (dtQ.Rows.Count > 0)
                    {
                        RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                        RAQual.Amount = 0;
                        RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                        RAQual.RateID = iQualId;
                        RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                        RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                        RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                        RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                        RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                        RAQual.HEDValue = 0;
                    }

                    QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);

                    Qualifier.frmQualifier qul = new Qualifier.frmQualifier();

                    dQNetAmt = 0;
                    dTaxAmt = 0;
                    decimal dVATAmt = 0;

                    DataRow dr1;

                    if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, argDate, ref dVATAmt) == true)
                    {
                        dRTAmt = dRTAmt + dTaxAmt;

                        dTNetAmt = dTNetAmt + dQNetAmt;
                        dTTaxAmt = dTTaxAmt + dTaxAmt;

                        foreach (Qualifier.cRateQualR d in QualVBC)
                        {
                            dr1 = dtQualifier.NewRow();

                            dr1["FlatId"] = iFlatId;
                            dr1["PaymentSchId"] = iPaySchId;
                            dr1["QualifierId"] = d.RateID;
                            dr1["Expression"] = d.Expression;
                            dr1["ExpPer"] = d.ExpPer;
                            dr1["NetPer"] = d.NetPer;
                            dr1["Add_Less_Flag"] = d.Add_Less_Flag;
                            dr1["SurCharge"] = d.SurPer;
                            dr1["EDCess"] = d.EDPer;
                            dr1["HEDPer"] = d.HEDPer;
                            dr1["ExpValue"] = d.ExpValue;
                            dr1["ExpPerValue"] = d.ExpPerValue;
                            dr1["SurValue"] = d.SurValue;
                            dr1["EDValue"] = d.EDValue;
                            dr1["Amount"] = d.Amount;
                            dr1["ReceiptTypeId"] = iReceiptId;
                            dr1["OtherCostId"] = iOthId;
                            dr1["Description"] = sQualName + " " + d.NetPer;

                            dtQualifier.Rows.Add(dr1);
                        }

                    }
                }
                if (dQNetAmt == 0) { dQNetAmt = dQBaseAmt; }

                if (iReceiptId != 1)
                {
                    dRowT["Balance"] = dQBaseAmt;
                    dRowT["TaxAmount"] = dRTAmt;

                    dRowT["NetAmount"] = dQNetAmt;
                }
                dtR.Rows.Add(dRowT);

            }

            ds.Tables.Add(dtR);
            ds.Tables.Add(dtQualifier);

            return ds;
        }

        public static DataTable GetQual(int argQId, DateTime argDate)
        {

            int iPeriodId = 0;

            string sSql = "Select PeriodId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriod Where QualType='B' and " +
                            "((TDate is not null and Fdate <= '" + argDate.ToString("dd MMM yyyy") + "' and TDate >= '" + argDate.ToString("dd MMM yyyy") + "') or " +
                            "(TDate is null  and FDate <= '" + argDate.ToString("dd MMM yyyy") + "'))";
            SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dtT = new DataTable();
            dtT.Load(dr);
            dr.Close();
            cmd.Dispose();

            if (dtT.Rows.Count > 0) { iPeriodId = Convert.ToInt32(dtT.Rows[0]["PeriodId"]); }
            dtT.Dispose();

            if (iPeriodId != 0)
            {
                sSql = "Select B.Expression,B.ExpPer,A.Add_Less_Flag,B.SurCharge,B.EDCess,B.HEDCess,B.Net From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A " +
                       "Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriodTrans B on A.QualifierId=B.QualifierId and B.PeriodId = " + iPeriodId + "" +
                       "Where A.QualifierId = " + argQId;
            }
            else
            {
                sSql = "Select Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,HEDCess,Net From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp " +
                       "Where QualifierId = " + argQId;

            }

            cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            cmd.Dispose();

            return dt;
        }

        public static DataTable GetProj()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenWorkFlowDB();
            try
            {
                sda = new SqlDataAdapter("SELECT CostCentreId ID,CostCentreName Name FROM dbo.OperationalCostCentre ORDER BY CostCentreName", BsfGlobal.g_WorkFlowDB);
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
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dt;
        }

        public static DataTable GetLevel()
        {
            SqlDataAdapter sda;
            DataTable dtLevel = null;
            BsfGlobal.OpenCRMDB();
            try
            {
                sda = new SqlDataAdapter("SELECT LevelId,LevelName FROM dbo.Level_Master ORDER BY LevelId", BsfGlobal.g_CRMDB);
                dtLevel = new DataTable();
                sda.Fill(dtLevel);
                dtLevel.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtLevel;
        }

        public static DataTable GetFlatDetails(int argCCId, string argMode, int argLeadId, string argType, int argEntryId)
        {
            string sSql = "";
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            try
            {
                if (argType == "B")
                {
                    sSql = "SELECT FlatTypeId,FlatId,FlatNo FROM dbo.FlatDetails" +
                            " Where Status In('B','U') And LeadId=" + argLeadId + " And CostCentreId=" + argCCId + "";
                }
                else
                {
                    //if (argMode == "A")
                    //    sSql = String.Format("SELECT FlatTypeId,FlatId,FlatNo FROM dbo.FlatDetails where Status='U' AND CostCentreId={0} Order By BlockT,LevelT,FlatTypeT,SequenceT", argCCId);
                    //else
                    //    sSql = String.Format("SELECT FlatTypeId,FlatId,FlatNo FROM dbo.FlatDetails where CostCentreId={0} Order By BlockT,LevelT,FlatTypeT,SequenceT", argCCId);
                    if (argMode == "A")
                    {
                        sSql = "SELECT F.FlatTypeId,F.FlatId,F.FlatNo FROM dbo.FlatDetails F " +
                                " INNER JOIN dbo.LevelMaster L ON F.LevelId=L.LevelId " +
                                " INNER JOIN dbo.BlockMaster B ON F.BlockId=B.BlockId " +
                                " Where Status='U' AND F.CostCentreId=" + argCCId + " " +
                                " Order By B.SortOrder,L.SortOrder,dbo.Val(F.FlatNo)";
                    }
                    else
                    {
                        sSql = "SELECT F.FlatTypeId,F.FlatId,F.FlatNo,B.SortOrder Order1,L.SortOrder Order2,dbo.Val(F.FlatNo) Order3 FROM dbo.FlatDetails F " +
                                " INNER JOIN dbo.LevelMaster L ON F.LevelId=L.LevelId " +
                                " INNER JOIN dbo.BlockMaster B ON F.BlockId=B.BlockId " +
                                " Where F.Status='U' AND F.CostCentreId=" + argCCId +
                                " UNION ALL " +
                                " SELECT F.FlatTypeId,F.FlatId,F.FlatNo,B.SortOrder,L.SortOrder,dbo.Val(F.FlatNo) FROM dbo.FlatDetails F " +
                                " INNER JOIN dbo.LevelMaster L ON F.LevelId=L.LevelId " +
                                " INNER JOIN dbo.BlockMaster B ON F.BlockId=B.BlockId " +
                                " INNER JOIN dbo.BuyerDetail C ON F.FlatId=C.FlatId" +
                                " Where F.Status='S' AND F.CostCentreId=" + argCCId + " AND C.EntryId=" + argEntryId + "" +
                                " Order By B.SortOrder,L.SortOrder,dbo.Val(F.FlatNo)";
                    }
                }
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

        public static string GetBlockFlat(int argCCId, int argLeadId, string argsType)
        {
            string sSql = "";
            DataTable dt = null; string sBlock = "";
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            try
            {
                if (argsType == "B")
                    sSql = "Select Status From dbo.FlatDetails Where CostCentreId=" + argCCId + " And LeadId=" + argLeadId + "";
                else
                    sSql = "Select Status From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails Where LandRegisterId=" + argCCId + " And BuyerId=" + argLeadId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sBlock = CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.vartypestring).ToString();
                        if (sBlock == "B") { return sBlock; }
                    }
                }
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
            return sBlock;

        }

        public static DataTable GetFromFlatDetails(int argCCId, int argFlatId)
        {
            string sSql = "";
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "SELECT FlatTypeId,FlatId,FlatNo FROM dbo.FlatDetails where Status='S'" +
                    " AND CostCentreId=" + argCCId + " And FlatId=" + argFlatId + " Order By BlockT,LevelT,FlatTypeT,SequenceT";
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

        public static DataTable GetToFlatDetails(int argCCId)
        {
            string sSql = "";
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "SELECT FlatTypeId,FlatId,FlatNo FROM dbo.FlatDetails where Status='U'" +
                    " AND CostCentreId=" + argCCId + " Order By BlockT,LevelT,FlatTypeT,SequenceT";
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

        public static void UpdateFlatTransfer(int argNewFlatId, int argOldFlatId, int argLeadId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "Update dbo.FlatDetails set Status='U',LeadId=0 Where FlatId=" + argOldFlatId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Update dbo.FlatDetails set Status='S',LeadId=" + argLeadId + " Where FlatId=" + argNewFlatId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //sSql = "Update dbo.ProgressBillRegister Set FlatId=" + argNewFlatId + ",LeadId=" + argLeadId + " Where FlatId=" + argOldFlatId + "";
                    sSql = "Update dbo.ProgressBillRegister Set FlatId=" + argNewFlatId + " Where FlatId=" + argOldFlatId + " AND LeadId=" + argLeadId + " ";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Update dbo.LeadFlatInfo Set FlatId=" + argNewFlatId + " Where FlatId=" + argOldFlatId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //sSql = "Update dbo.BuyerDetail Set FlatId=" + argNewFlatId + ",LeadId=" + argLeadId + " Where FlatId=" + argOldFlatId + "";
                    sSql = "Update dbo.BuyerDetail Set FlatId=" + argNewFlatId + " Where FlatId=" + argOldFlatId + " AND LeadId=" + argLeadId + " ";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Update dbo.ExtraBillRegister Set FlatId=" + argNewFlatId + " Where FlatId=" + argOldFlatId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    tran.Commit();
                }
                catch (Exception ex)
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
        }

        public static DataTable GetFlatSoldDetails(int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            String sSql = String.Format("SELECT FlatId,FlatNo FROM dbo.FlatDetails where Status='S' AND CostCentreId={0}", argCCId);
            try
            {
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

        public static DataTable GetData()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            try
            {
                String sSql = "SELECT * FROM dbo.FlatType";
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

        public static DataTable GetFlatDetailsD(int argFlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            String sSql = String.Format("select B.Typename,C.BlockName,D.LevelName,A.Area,A.Rate,A.BaseAmt,A.AdvAmount,A.USLandAmt,A.OtherCostAmt,A.NetAmt from dbo.FlatDetails A  Inner Join dbo.FlatType B on A.FlatTypeId=B.FlatTypeId and A.CostCentreId=B.ProjId and A.FlatId={0} Inner Join dbo.BlockMaster C on A.BlockId=C.BlockId Inner Join dbo.LevelMaster D on A.LevelId=D.LevelId ", argFlatId);
            try
            {
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

        public static DataTable GetPlotDetails(int argPlotId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            String sSql = "select B.PlotTypeName,A.Area,A.Rate,A.BaseAmount,A.AdvanceAmount,A.GuideLine,A.OtherCost,A.NetAmount" +
                          " From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A" +
                          " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotType B On A.PlotTypeId=B.PlotTypeId" +
                          " Where A.PlotDetailsId=" + argPlotId + "";
            try
            {
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

        public static decimal GetPlotQualiAmt(int argPlotId)
        {
            DataTable dt = null;
            SqlDataAdapter sda; decimal dAmt = 0;
            BsfGlobal.OpenCRMDB();
            String sSql = "Select Sum(Case When Add_Less_Flag='-' Then A.Amount*-1 Else A.Amount End) Amount" +
                        " from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptQualifier A Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType B On A.SchId=B.SchId" +
                        " Where B.PlotDetailsId=" + argPlotId + "";
            try
            {
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0) { dAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }
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
            return dAmt;
        }

        public static DataTable GetFlatDet(int argFlatId, int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            string sSql = "SELECT F.FlatID,F.FlatTypeId,F.PayTypeId,B.BlockName,L.LevelName,F.BlockId,F.LevelId,F.Status,R.LeadName BuyerName," +
                " Case When X.EmployeeName='' Then X.UserName Else X.EmployeeName End As ExecName,F.FlatNo,FT.Typename,F.Area,F.Rate,F.BaseAmt,F.OtherCostAmt,F.ExtraBillAmt,F.USLand,F.USLandAmt,F.LandRate," +
                " F.AdvPercent,F.AdvAmount,F.Guidelinevalue,F.TotalCarPark,F.Remarks,F.CostCentreId,F.FacingId,G.Description FacingName, F.NetAmt,F.IntPercent,F.CreditDays,F.LeadId " +
                " FROM dbo.FlatDetails F INNER JOIN dbo.FlatType FT ON F.FlatTypeId=FT.FlatTypeID  " +
                " INNER JOIN dbo.LevelMaster L ON F.LevelId=L.LevelId INNER JOIN dbo.BlockMaster B " +
                " ON F.BlockId=B.BlockId LEFT JOIN dbo.BuyerDetail E ON E.FlatId=F.FlatId " +
                " LEFT JOIN dbo.LeadRegister R ON R.LeadId=F.LeadId " +
                " Left Join dbo.LeadExecutiveInfo LE On LE.LeadId=R.LeadId " +
                " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users X On X.UserId=LE.ExecutiveId " +
                " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position Y " +
                " On X.PositionId=Y.PositionId Left Join Facing G On G.FacingId=F.FacingId " +
                " WHERE F.CostCentreId=" + argCCId + " AND F.FlatId=" + argFlatId + " ORDER BY F.FlatNo";
            try
            {
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

        public static DataTable GetBrokerDetailsD(int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            String sSql = "";
            //sSql = "Select A.BrokerId,C.VendorName BrokerName From dbo.BrokerDet A  " +
            //            " Inner Join dbo.BrokerCC B On A.BrokerId=B.BrokerId" +
            //            " Inner Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorMaster C On C.VendorId=A.VendorId" +
            //            " Where B.CostCentreId=" + argCCId + "";
            if (BsfGlobal.g_bWPMDB == true)
            {
                // Commented on 12/06/2013
                //sSql = "Select A.SORegisterId,A.ContractorId VendorId,B.VendorName BrokerName From [" + BsfGlobal.g_sWPMDBName + "].dbo.SORegister A " +
                //        " Inner Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorMaster B On A.ContractorId=B.VendorId " +
                //        " Where A.CostCentreId=" + argCCId + " And A.ServiceType='B' And A.Approve='Y'";

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
            try
            {
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

        public static DataTable GetBrokerComm(int argSORegId, int argCCId, int argBrokerId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "Select * From [" + BsfGlobal.g_sWPMDBName + "].dbo.SOBrokerTrans Where SORegId=" + argSORegId + "";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();

                if(dt.Rows.Count == 0)
                {
                    sSql = "Select BrokerId, CommPer Percentage, Amount, PerBased, CommType From dbo.BrokerCC Where CostCentreId=" + argCCId + " AND BrokerId=" + argBrokerId + "";
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    dt = new DataTable();
                    sda.Fill(dt);
                    sda.Dispose();
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

        public static DataTable GetBankDetailsD()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            String sSql = "Select BranchId,Branch BranchName From dbo.BankDetails ";
            try
            {
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

        public static DataTable getBuyFinalDetailsE(int argEntryId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            String sSql = "Select AllotmentNo GAllotNo,COAllotNo,CCAllotNo,PlotId,RegDate,FlatId,PaySchId,BrokerId,BranchId,Status,CustomerType,PaymentOption,LoanPer,LoanAccNo,BrokerComm,BrokerAmount," +
                        " ValidUpto,FinaliseDate,PostSaleExecId,Advance From dbo.BuyerDetail Where EntryId=" + argEntryId + "";
            try
            {
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

        public static void UpdateEI(int argFlatId, int argTransId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "UPDATE dbo.FlatExtraItem SET Approve='" + true + "' WHERE FlatId=" + argFlatId + " AND ExtraItemId=" + argTransId + "";
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

        public static void UpdateFlatDetails(FlatdetailsBL OFlatDetails, DataTable dtAreatrans, DataTable dtSTaxtrans, DataTable dtOCosttrans, DataTable argExtraItemtrans, DataTable dtFCheck)
        {
            //int iFTypeId = 0;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = String.Format("Update dbo.FlatDetails set FlatNo='{0}',FlatTypeId={1},BlockId={2},LevelId={3},Area={4},Rate={5} ,BaseAmt={6},AdvAmount={7},OtherCostAmt={8},ExtraBillAmt={9},VatAmt={10},SerTaxAmt={11},NetAmt={12}, USLand={13},LandRate={14},USLandAmt={15},Remarks='{16}',CostCentreId={17} ,FacingId={18} Where FlatId={19} ", OFlatDetails.FlatNo, OFlatDetails.FlatTypeId, OFlatDetails.BlockId, OFlatDetails.LevelId, OFlatDetails.Area, OFlatDetails.Rate, OFlatDetails.BaseAmt, OFlatDetails.AdvAmt, OFlatDetails.OtherCostAmt, OFlatDetails.ExtraBillAmt, OFlatDetails.VatAmt, OFlatDetails.SerTaxAmt, OFlatDetails.NetAmt, OFlatDetails.USLand, OFlatDetails.LandRate, OFlatDetails.LandAmt, OFlatDetails.Remarks, OFlatDetails.ProjId, OFlatDetails.FlatFace, OFlatDetails.FlatId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    sSql = String.Format("Delete from dbo.FlatAreaTrans Where FlatId={0} ", OFlatDetails.FlatId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    if (dtAreatrans.Rows.Count > 0)
                    {
                        for (int a = 0; a < dtAreatrans.Rows.Count; a++)
                        {
                            sSql = String.Format("INSERT INTO dbo.FlatAreaTrans(FlatId,AreaId,AreaSqft) Values({0},{1},{2})", OFlatDetails.FlatId, Convert.ToInt32(dtAreatrans.Rows[a]["AreaId"]), dtAreatrans.Rows[a]["AreaSqft"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    sSql = String.Format("Delete from dbo.FlatSTaxTrans Where FlatId={0} ", OFlatDetails.FlatId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    if (dtSTaxtrans.Rows.Count > 0)
                    {
                        for (int s = 0; s < dtSTaxtrans.Rows.Count; s++)
                        {
                            sSql = String.Format("INSERT INTO dbo.FlatSTaxTrans(FlatId,AccountId,TaxId,TaxDescp,TaxFormula,AddFlag) Values({0},{1},{2},'{3}','{4}','{5}')", OFlatDetails.FlatId, dtSTaxtrans.Rows[s]["AccId"], dtSTaxtrans.Rows[s]["TaxId"], dtSTaxtrans.Rows[s]["TaxDescp"], dtSTaxtrans.Rows[s]["TaxFormula"], dtSTaxtrans.Rows[s]["AddFlag"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    sSql = String.Format("Delete from dbo.FlatOCostTrans Where FlatId={0} ", OFlatDetails.FlatId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    if (dtOCosttrans.Rows.Count > 0)
                    {
                        for (int c = 0; c < dtOCosttrans.Rows.Count; c++)
                        {
                            sSql = String.Format("INSERT INTO dbo.FlatOCostTrans(FlatId,OtherCostId,OtherCostName,Area,Rate,Flag,Amount) Values({0},{1},'{2}',{3},{4},'{5}',{6}) ", OFlatDetails.FlatId, dtOCosttrans.Rows[c]["OtherCostId"], dtOCosttrans.Rows[c]["OtherCostName"], dtOCosttrans.Rows[c]["Area"], dtOCosttrans.Rows[c]["Rate"], dtOCosttrans.Rows[c]["Flag"], dtOCosttrans.Rows[c]["Amount"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    sSql = String.Format("Delete from dbo.FlatExtraItemTrans Where FlatId={0} ", OFlatDetails.FlatId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    if (argExtraItemtrans.Rows.Count > 0)
                    {
                        for (int c = 0; c < argExtraItemtrans.Rows.Count; c++)
                        {
                            sSql = String.Format("INSERT INTO dbo.FlatExtraItemTrans(FlatId,TransId,ExtraItemTypeId,Quantity,Rate,Amount) Values({0},{1},{2},{3},{4},{5}) ", OFlatDetails.FlatId, argExtraItemtrans.Rows[c]["TransId"], argExtraItemtrans.Rows[c]["ExtraItemTypeId"], argExtraItemtrans.Rows[c]["Qty"], argExtraItemtrans.Rows[c]["Rate"], argExtraItemtrans.Rows[c]["Amount"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    sSql = String.Format("Delete from dbo.FlatCheckList Where FlatId={0} ", OFlatDetails.FlatId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    if (dtFCheck.Rows.Count > 0)
                    {
                        for (int c = 0; c < dtFCheck.Rows.Count; c++)
                        {
                            sSql = String.Format("INSERT INTO dbo.FlatCheckList(CheckListId,FlatId,Description,ExpCompletionDate,CompletionDate,ExecutiveId,Status) Values({0},{1},'{2}','{3:dd-MMM-yyyy}','{4:dd-MMM-yyyy}',{5},'{6}') ", dtFCheck.Rows[c]["CheckListId"], OFlatDetails.FlatId, dtFCheck.Rows[c]["Description"], Convert.ToDateTime(dtFCheck.Rows[c]["ExpCompletionDate"]), Convert.ToDateTime(dtFCheck.Rows[c]["CompletionDate"]), dtFCheck.Rows[c]["ExecutiveId"], dtFCheck.Rows[c]["Status"]);
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

        public static void UpdateUnitFlatDetails(FlatDetailBO OFlatDetails)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;

            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = String.Format("Update dbo.FlatDetails set FlatNo='{0}',FlatTypeId={1},PayTypeId={2},BlockId={3},LevelId={4},Area={5},Rate={6} ,BaseAmt={7},AdvAmount={8},OtherCostAmt={9},ExtraBillAmt={10},NetAmt={11}, USLand={12},LandRate={13},USLandAmt={14},Remarks='{15}',CostCentreId={16} ,FacingId={17},CreditDays={18},IntPercent={19},TotalCarPark = {20},AdvPercent={21},Guidelinevalue={22},QualifierAmt={23} Where FlatId={24} ", OFlatDetails.FlatNo, OFlatDetails.FlatTypeId, OFlatDetails.PayTypeId, OFlatDetails.BlockId, OFlatDetails.LevelId, OFlatDetails.Area, OFlatDetails.Rate, OFlatDetails.BaseAmt, OFlatDetails.AdvAmount, OFlatDetails.OtherCostAmt, OFlatDetails.ExtraBillAmt, OFlatDetails.NetAmt, OFlatDetails.USLand, OFlatDetails.LandRate, OFlatDetails.USLandAmt, OFlatDetails.Remarks, OFlatDetails.CostCentreId, OFlatDetails.Facing, OFlatDetails.CreditDays, OFlatDetails.InterestPercent, OFlatDetails.TotalCarPark, OFlatDetails.AdvPercent, OFlatDetails.GuideLineValue, OFlatDetails.QualAmt, OFlatDetails.FlatId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //PaymentScheduleBL.InsertFlatScheduleI(OFlatDetails.FlatId,conn, tran);
                    tran.Commit();
                }
                catch (Exception ex)
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
        }

        public static void InsertFlatDetails(FlatdetailsBL OFlatDetails, DataTable dtAreatrans, DataTable dtSTaxtrans, DataTable dtOCosttrans, DataTable argExtraItemtrans, DataTable dtCheck)
        {
            int iFlatId = 0;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = String.Format("INSERT INTO dbo.FlatDetails(FlatNo, FlatTypeId, BlockId, LevelId," +
                        " Area, Rate, BaseAmt, AdvAmount, OtherCostAmt, ExtraBillAmt, VatAmt, SerTaxAmt, NetAmt" +
                        ", USLand, LandRate, USLandAmt, Remarks, CostCentreId, FacingId) Values('{0}',{1},{2},{3},{4},{5}," +
                        "{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},'{16}',{17},'{18}')SELECT SCOPE_IDENTITY(); ",
                        OFlatDetails.FlatNo, OFlatDetails.FlatTypeId, OFlatDetails.BlockId, OFlatDetails.LevelId,
                        OFlatDetails.Area, OFlatDetails.Rate, OFlatDetails.BaseAmt, OFlatDetails.AdvAmt,
                        OFlatDetails.OtherCostAmt, OFlatDetails.ExtraBillAmt, OFlatDetails.VatAmt,
                        OFlatDetails.SerTaxAmt, OFlatDetails.NetAmt, OFlatDetails.USLand, OFlatDetails.LandRate,
                        OFlatDetails.LandAmt, OFlatDetails.Remarks, OFlatDetails.ProjId, OFlatDetails.FlatFace);

                    cmd = new SqlCommand(sSql, conn, tran);
                    iFlatId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();
                    if (dtAreatrans.Rows.Count > 0)
                    {
                        for (int a = 0; a < dtAreatrans.Rows.Count; a++)
                        {
                            sSql = String.Format("INSERT INTO dbo.FlatAreaTrans(FlatId,AreaId,AreaSqft) Values({0},{1},{2})", iFlatId, Convert.ToInt32(dtAreatrans.Rows[a]["AreaId"]), dtAreatrans.Rows[a]["AreaSqft"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    if (dtSTaxtrans.Rows.Count > 0)
                    {
                        for (int s = 0; s < dtSTaxtrans.Rows.Count; s++)
                        {
                            sSql = String.Format("INSERT INTO dbo.FlatSTaxTrans(FlatId,AccountId,TaxId,TaxDescp,TaxFormula,AddFlag) Values({0},{1},{2},'{3}','{4}','{5}')", iFlatId, dtSTaxtrans.Rows[s]["AccId"], dtSTaxtrans.Rows[s]["TaxId"], dtSTaxtrans.Rows[s]["TaxDescp"], dtSTaxtrans.Rows[s]["TaxFormula"], dtSTaxtrans.Rows[s]["AddFlag"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    if (dtOCosttrans.Rows.Count > 0)
                    {
                        for (int c = 0; c < dtOCosttrans.Rows.Count; c++)
                        {
                            sSql = String.Format("INSERT INTO dbo.FlatOCostTrans(FlatId,OtherCostId,OtherCostName,Area,Rate,Flag,Amount) Values({0},{1},'{2}',{3},{4},'{5}',{6}) ", iFlatId, dtOCosttrans.Rows[c]["OtherCostId"], dtOCosttrans.Rows[c]["OtherCostName"], dtOCosttrans.Rows[c]["Area"], dtOCosttrans.Rows[c]["Rate"], dtOCosttrans.Rows[c]["Flag"], dtOCosttrans.Rows[c]["Amount"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    if (argExtraItemtrans.Rows.Count > 0)
                    {
                        for (int c = 0; c < argExtraItemtrans.Rows.Count; c++)
                        {
                            sSql = String.Format("INSERT INTO dbo.FlatExtraItemTrans(FlatId,TransId,ExtraItemTypeId,Quantity,Rate,Amount) Values({0},{1},{2},{3},{4},{5}) ", iFlatId, argExtraItemtrans.Rows[c]["TransId"], argExtraItemtrans.Rows[c]["ExtraItemTypeId"], argExtraItemtrans.Rows[c]["Qty"], argExtraItemtrans.Rows[c]["Rate"], argExtraItemtrans.Rows[c]["Amount"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    sSql = String.Format("DELETE FROM dbo.FlatChecklist WHERE FlatId={0}", iFlatId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    if (dtCheck.Rows.Count > 0)
                    {
                        for (int c = 0; c < dtCheck.Rows.Count; c++)
                        {
                            sSql = String.Format("INSERT INTO dbo.FlatChecklist(CheckListId,FlatId,Description,ExpCompletionDate,CompletionDate,ExecutiveId,Status) Values({0},{1},'{2}','{3:dd-MMM-yyyy}','{4:dd-MMM-yyyy}',{5},'{6}') ", dtCheck.Rows[c]["CheckListId"], iFlatId, dtCheck.Rows[c]["Description"], Convert.ToDateTime(dtCheck.Rows[c]["ExpCompletionDate"]), Convert.ToDateTime(dtCheck.Rows[c]["CompletionDate"]), dtCheck.Rows[c]["ExecutiveId"], dtCheck.Rows[c]["Status"]);
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

        public static decimal GetNetAmt(string argqry, SqlConnection conn)
        {
            DataTable dt = new DataTable();
            decimal netamt = 0;
            SqlCommand sda;

            try
            {
                sda = new SqlCommand(argqry, BsfGlobal.g_CRMDB);
                netamt = Convert.ToDecimal(sda.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return netamt;
        }

        public static decimal GetOtherCost(string argqry)
        {
            decimal netamt = 0;
            SqlCommand sda;

            try
            {
                sda = new SqlCommand(argqry, BsfGlobal.g_CRMDB);
                netamt = Convert.ToDecimal(sda.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return netamt;
        }

        public static void InsertUnitFlatDetails(FlatDetailBO OFlatDetails)
        {
            int iFlatId = 0;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;

            conn = BsfGlobal.OpenCRMDB();

            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = String.Format("INSERT INTO dbo.FlatDetails(FlatNo,FlatTypeId,PayTypeId,BlockId,LevelId,Area,Rate,BaseAmt,AdvAmount,OtherCostAmt,ExtraBillAmt,NetAmt,USLand,LandRate,USLandAmt,Remarks,CostCentreId,FacingId,CreditDays,IntPercent) Values('{0}',{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},'{15}',{16},'{17}',{18},{19})SELECT SCOPE_IDENTITY(); ", OFlatDetails.FlatNo, OFlatDetails.FlatTypeId, OFlatDetails.PayTypeId, OFlatDetails.BlockId, OFlatDetails.LevelId, OFlatDetails.Area, OFlatDetails.Rate, OFlatDetails.BaseAmt, OFlatDetails.AdvAmount, OFlatDetails.OtherCostAmt, OFlatDetails.ExtraBillAmt, OFlatDetails.NetAmt, OFlatDetails.USLand, OFlatDetails.LandRate, OFlatDetails.USLandAmt, OFlatDetails.Remarks, OFlatDetails.CostCentreId, OFlatDetails.Facing, OFlatDetails.CreditDays, OFlatDetails.InterestPercent);
                    cmd = new SqlCommand(sSql, conn, tran);
                    iFlatId = int.Parse(cmd.ExecuteScalar().ToString());
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

        public static void DeleteFlatDetails(int argFlatId, int argCCId)
        {
            string sSql = "";
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd; SqlDataReader dr;
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            int iBlockId = 0;
            DataTable dt = null;

            try
            {
                sSql = "Select BlockId From dbo.FlatDetails Where FlatId=" + argFlatId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    iBlockId = Convert.ToInt32(dt.Rows[0]["BlockId"]);
                }

                sSql = String.Format("DELETE FROM dbo.FlatDetails WHERE FlatId={0}", argFlatId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = String.Format("DELETE FROM dbo.FlatArea WHERE FlatId={0}", argFlatId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = String.Format("DELETE FROM dbo.FlatOtherCost WHERE FlatId={0}", argFlatId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = String.Format("DELETE FROM dbo.FlatOtherArea WHERE FlatId={0}", argFlatId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = String.Format("DELETE FROM dbo.FlatExtraItem WHERE FlatId={0}", argFlatId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = String.Format("DELETE FROM dbo.FlatChecklist WHERE FlatId={0}", argFlatId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = String.Format("Select * FROM dbo.FlatCarPark WHERE FlatId={0}", argFlatId);
                cmd = new SqlCommand(sSql, conn, tran);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                cmd.Dispose();

                sSql = String.Format("DELETE FROM dbo.FlatCarPark WHERE FlatId={0}", argFlatId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = String.Format("DELETE FROM dbo.PaymentScheduleFlat WHERE FlatId={0}", argFlatId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = String.Format("DELETE FROM dbo.FlatReceiptType WHERE FlatId={0}", argFlatId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "Update dbo.ProjectInfo Set TotalFlats=(Select Count(FlatId) TotalFlat From dbo.FlatDetails" +
                       " Where CostCentreId=" + argCCId + ") Where CostCentreId=" + argCCId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    int TypeId = Convert.ToInt32(dt.Rows[0]["TypeId"]);

                    sSql = "Select SUM(TotalCP)TotalCP,TypeId " +
                            " from FlatCarPark Where FlatId in (Select FlatId from FlatDetails " +
                            " where BlockId=" + iBlockId + " and CostCentreId=" + argCCId + ") Group By TypeId";
                    cmd = new SqlCommand(sSql, conn, tran);
                    dr = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(dr);
                    dr.Close();
                    cmd.Dispose();

                    if (dt != null)
                    {
                        if (dt.Rows.Count == 0)
                        {
                            sSql = "Update CarParkMaster Set AllottedSlots = 0 Where BlockId= " + iBlockId + " And CostCentreId = " + argCCId + "";//  and TypeId=" + TypeId + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                        else if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                sSql = "Update CarParkMaster Set AllottedSlots = " + dt.Rows[i]["TotalCP"] + " Where BlockId= " + iBlockId + " and CostCentreId = " + argCCId + "  and TypeId=" + dt.Rows[i]["TypeId"] + "";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
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

        public static DataTable GetFlatArea(int argFlatId)
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = String.Format("SELECT A.AreaId,A.Description,F.AreaSqft from dbo.FlatArea F INNER JOIN dbo.AreaMaster A ON F.AreaId=A.AreaId where F.FlatId={0}", argFlatId);
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
                ds.Dispose();
            }
            return ds;
        }

        public static DataTable GetFlatTypeArea(int argFlatTypeId)
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = String.Format("SELECT A.AreaId,A.Description,F.AreaSqft,F.FlatTypeId FROM dbo.FlatTypeArea F INNER JOIN dbo.AreaMaster A  ON F.AreaId=A.AreaId WHERE FlatTypeId={0}", argFlatTypeId);
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
                ds.Dispose();
            }
            return ds;
        }

        public static DataTable GetFlatTypeAreaNull()
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "SELECT A.AreaId,A.Description,F.AreaSqft,F.FlatTypeId FROM dbo.FlatTypeArea F INNER JOIN dbo.AreaMaster A " +
                    " ON F.AreaId=A.AreaId WHERE FlatTypeId=null";
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
                ds.Dispose();
            }
            return ds;
        }

        public static DataTable GetFlatAreaNull()
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "SELECT A.AreaId,A.Description,F.AreaSqft,F.FlatId FROM dbo.FlatArea F INNER JOIN dbo.AreaMaster A " +
                    " ON F.AreaId=A.AreaId WHERE FlatId=null";
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
                ds.Dispose();
            }
            return ds;
        }

        public static DataTable GetFlatCheckList(int argCCId, int argFlatId)
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "select ISNULL(A.CheckListId,0) CheckListId,A.CheckListName Description, B.FlatId,B.ExpCompletionDate, " +
                     "Case When A.CheckListId In (Select CheckListId From FlatChecklist) Then B.Status Else CONVERT(bit,0) End Status, " +
                     "B.CompletionDate,B.ExecutiveId Executive,B.Remarks from dbo.CheckListMaster  A " +
                     "Left Join dbo.FlatChecklist B On A.CheckListId=B.CheckListId And B.FlatId=" + argFlatId + " " +
                     "WHERE A.TypeName='F' ";
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
                ds.Dispose();
            }
            return ds;
        }

        public static DataTable GetFlatTypeEINull(int argCCId)
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.TransId,B.ItemCode,B.ExtraItemTypeId,B.ItemDescription,C.ExtraItemTypeName from dbo.FlatTypeExtraItem A Inner Join dbo.ExtraItemMaster B on A.TransId=B.TransId and A.FlatTypeId=NULL Inner Join dbo.ExtraItemTypeMaster C on B.ExtraItemTypeId=C.ExtraItemTypeId WHERE B.CostCentreId=" + argCCId + " AND C.CostCentreId=" + argCCId + "";
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
                ds.Dispose();
            }
            return ds;
        }

        public static DataTable GetFlatEINull(int argCCId)
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.TransId,B.ItemCode,B.ExtraItemTypeId,B.ItemDescription,C.ExtraItemTypeName from dbo.FlatExtraItem A Inner Join dbo.ExtraItemMaster B on A.TransId=B.TransId and A.FlatId=NULL Inner Join dbo.ExtraItemTypeMaster C on B.ExtraItemTypeId=C.ExtraItemTypeId WHERE B.CostCentreId=" + argCCId + " AND C.CostCentreId=" + argCCId + "";
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
                ds.Dispose();
            }
            return ds;
        }

        public static DataTable GetFlatTypeEI(int argFlatTypeId)
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.ExtraItemId,A.ItemCode,A.ItemDescription,B.ExtraItemTypeName, " +
                       "Case When C.flatTypeId is Null then Convert(bit,0,0) Else Convert(bit,1,1) End Sel " +
                       "from dbo.ExtraItemMaster A " +
                       "Inner Join dbo.ExtraItemTypeMaster B on A.ExtraItemTypeId=B.ExtraItemTypeId " +
                       "Left Join dbo.FlatTypeExtraItem C on A.ExtraItemId=C.ExtraItemId and C.FlatTypeId=" + argFlatTypeId;
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
                ds.Dispose();
            }
            return ds;
        }

        public static DataTable GetFlatEI(int argFlatId)
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                //sSql = "Select A.ExtraItemId,B.ItemCode,B.ItemDescription,Rate,Approve from dbo.FlatExtraItem A " +
                //       "Inner Join dbo.ExtraItemMaster B on A.ExtraItemId=B.ExtraItemId " +
                //       "Where A.FlatId= " + argFlatId;
                sSql = "Select A.FlatExtraItemId,A.FlatId,A.ExtraItemId,B.ItemCode,B.ItemDescription,A.Amount,A.NetAmount, "+
                        "Case When A.Approve='Y' Then 'Yes' Else 'No' End Approve From FlatExtraItem A" +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ExtraItemMaster B On A.ExtraItemId=B.ExtraItemId " +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ExtraItemTypeMaster C On C.ExtraItemTypeId=B.ExtraItemTypeId" +
                        " Where FlatId= " + argFlatId + " AND A.Sel=1";
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
                ds.Dispose();
            }
            return ds;
        }

        public static DataTable GetPaySchType(int argFlatId, string argType)
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                if (argType == "B")
                    sSql = " SELECT DISTINCT F.PayTypeId TypeId,P.TypeName from dbo.PaySchType P " +
                           " INNER JOIN dbo.FlatDetails F ON P.TypeId=F.PayTypeId WHERE F.FlatId=" + argFlatId + "";
                else
                    sSql = " SELECT DISTINCT F.PaymentScheduleId TypeId,P.TypeName from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaySchType P  " +
                           " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails F ON P.TypeId=F.PaymentScheduleId" +
                           " WHERE F.PlotDetailsId=" + argFlatId + "";
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
                ds.Dispose();
            }
            return ds;
        }

        public static DataTable GetPayFlatDetail(int argFlatId, int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "SELECT F.FlatId,USLandAmt,BaseAmt,AdvAmount FROM dbo.FlatDetails F " +
                              " WHERE F.FlatId=" + argFlatId + " AND F.CostCentreId=" + argCCId + "";
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
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
            return dt;
        }

        internal static DataTable GetPaymentBlocks(int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "SELECT BlockId, BlockName FROM dbo.BlockMaster WHERE CostCentreId=" + argCCId + " ORDER BY SortOrder";
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
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
            return dt;
        }

        public static void InsertFlatTypeArea(DataTable dtAreatrans, int argFTId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "DELETE FROM dbo.FlatTypeArea WHERE FlatTypeId=" + argFTId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (dtAreatrans.Rows.Count > 0)
                    {
                        for (int a = 0; a < dtAreatrans.Rows.Count; a++)
                        {
                            sSql = "INSERT INTO dbo.FlatTypeArea(FlatTypeId,AreaId,AreaSqft) Values" +
                                " (" + argFTId + "," + dtAreatrans.Rows[a]["AreaId"] + ", " + dtAreatrans.Rows[a]["AreaSqft"] + ")";
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

        public static void InsertFlatArea(DataTable dtAreatrans, int argFId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "DELETE FROM dbo.FlatArea WHERE FlatId=" + argFId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (dtAreatrans.Rows.Count > 0)
                    {
                        for (int a = 0; a < dtAreatrans.Rows.Count; a++)
                        {
                            sSql = "INSERT INTO dbo.FlatArea(FlatId,AreaId,AreaSqft) Values" +
                                " (" + argFId + "," + dtAreatrans.Rows[a]["AreaId"] + ", " + dtAreatrans.Rows[a]["AreaSqft"] + ")";
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

        internal static DataTable getFinalDetails(int argiFlatId, int argCCId)
        {
            String sSql = "";
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = String.Format("select * from dbo.BuyerDetail" +
                " where FlatId={0} AND CostCentreId={1}", argiFlatId, argCCId);
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

        internal static bool getWebFound(int argFlatId, int argExtraId)
        {
            String sSql = "";
            DataTable dt = null;
            SqlDataAdapter sda;
            bool bAns = false;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select FlatId From dbo.FlatExtraItem Where UpdateFrom='W' And FlatId=" + argFlatId + " And ExtraItemId=" + argExtraId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    bAns = true;
                }
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
            return bAns;
        }

        internal static bool GetTypewise(int argPayTypeId)
        {
            String sSql = "";
            DataTable dt = null;
            SqlDataAdapter sda;
            bool bAns = false;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select Typewise From dbo.PaySchType Where TypeId=" + argPayTypeId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    bAns = Convert.ToBoolean(dt.Rows[0]["Typewise"]);
                }
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
            return bAns;
        }

        internal static bool GetBuyerTypewise(int argFlatId)
        {
            String sSql = "";
            DataTable dt = null;
            SqlDataAdapter sda;
            bool bAns = false;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select Typewise From dbo.PaySchType A Inner Join dbo.FlatDetails B On A.TypeId=B.PayTypeId " +
                        " Where FlatId=" + argFlatId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    bAns = Convert.ToBoolean(dt.Rows[0]["Typewise"]);
                }
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
            return bAns;
        }


        internal static void UpdateFlatQualAmt(int argPayTypeId, int argFlatId, SqlConnection conn, SqlTransaction tran)
        {
            String sSql = "";
            DataTable dt = null;
            SqlDataReader dr; SqlCommand cmd;
            bool bAns = false; decimal dQualAmt = 0;
            try
            {
                sSql = "Select Typewise From dbo.PaySchType Where TypeId=" + argPayTypeId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    bAns = Convert.ToBoolean(dt.Rows[0]["Typewise"]);
                }
                dt.Dispose();

                if (bAns == false)
                    sSql = "Select SUM(Amount)Amount From dbo.PaySchTaxFlat Where FlatId=" + argFlatId + "";
                else
                    sSql = "Select  Sum(Case When Add_Less_Flag = '-' Then Amount*-1 Else Amount End) Amount from dbo.FlatReceiptQualifier  " +
                            "Where SchId in (Select SchId from dbo.FlatReceiptType Where FlatId=" + argFlatId + ")";
                cmd = new SqlCommand(sSql, conn, tran);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);

                if (dt.Rows.Count > 0)
                {
                    dQualAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
                }
                dt.Dispose();

                sSql = "Update FlatDetails Set QualifierAmt=" + dQualAmt + " Where FlatId=" + argFlatId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        public static void InsertFlatSortOrder(int argCCId, int argFlatId, int argBlockId, int argLevelId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmd;
            string sSql = "";
            sSql = "Update dbo.FlatDetails Set SortOrder=(Select Max(Isnull(SortOrder,0))SortOrder From dbo.FlatDetails " +
                    " Where CostCentreId=" + argCCId + " And BlockId=" + argBlockId + " And LevelId=" + argLevelId + ")+1 Where FlatId=" + argFlatId + " ";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        #endregion

        #region Reports

        public static DataTable GetAllotmentPrint(int argFlatId, int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Select A.FlatId,LR.LeadName,LA.Address1,LA.Address2,LCC.CityName City,LA.Pincode,LA.Mobile,LCS.StateName State,D.CompanyName," +
                                " B.CostCentreName,BM.BlockName,L.LevelName,A.FlatNo,FT.TypeName,A.Area,A.Rate,A.BaseAmt," +
                                " A.OtherCostAmt,A.NetAmt,A.USLand,A.USLandAmt,D.Address1 CompAddress1,D.Address2 CompAddress2,D.Phone CompPhone," +
                                " D.Mobile CompMobile,D.Email CompEmail,CM.CityName CompCityName,SM.StateName CompStateName,D.Pincode CompPincode," +
                                " C.Address1 CCAddress1,C.Address2 CCAddress2,CCM.CityName CCCityName,CSM.StateName CCStateName,C.Pincode CCPincode,PF.Description," +
                                " PF.SchPercent,PF.Amount Gross,Isnull(Q.Amount,0) Tax,PF.PaidAmount,PF.NetAmount," +
                                " CF=IsNull((Select Amount From dbo.FlatOtherCost A Inner Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId Where A.FlatId=" + argFlatId + " And OCTypeId=8),0)," +
                                " MC=IsNull((Select Amount From dbo.FlatOtherCost A Inner Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId Where A.FlatId=" + argFlatId + " And OCTypeId=3),0), " +
                                " BD.FinaliseDate FinalizeDate, ISNULL(CPTM.TypeName, 'None') CarParkTypeName, ISNULL(FCP.TotalCP,0) TotalCarPark From FlatDetails A " +
                                " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B On A.CostCentreId=B.CostCentreId" +
                                " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CostCentre C On B.FACostCentreId=C.CostCentreId" +
                                " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CompanyMaster D On D.CompanyId=C.CompanyId" +
                                " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CityMaster CM On CM.CityId=D.CityId" +
                                " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.StateMaster SM On SM.StateId=D.StateId" +
                                " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CityMaster CCM On CCM.CityId=C.CityId" +
                                " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.StateMaster CSM On CSM.StateId=C.StateId" +
                                " INNER JOIN LevelMaster L On L.LevelId=A.LevelId " +
                                " INNER JOIN BlockMaster BM On A.BlockId=BM.BlockId " +
                                " INNER JOIN FlatType FT On FT.FlatTypeId=A.FlatTypeId" +
                                " INNER JOIN LeadRegister LR On LR.LeadId=A.LeadId " +
                                " LEFT JOIN LeadCommAddressInfo LA On LA.LeadId=A.LeadId " +
                                " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CityMaster LCC On LCC.CityId=LA.CityId" +
                                " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.StateMaster LCS On LCS.StateId=LA.StateId" +
                                " INNER JOIN PaymentScheduleFlat PF On PF.FlatId=A.FlatId " +
                                " LEFT JOIN dbo.FlatReceiptQualifier Q On Q.SchId=PF.PaymentSchId " +
                                " LEFT JOIN dbo.BuyerDetail BD On A.FlatId=BD.FlatId " +
                                " LEFT JOIN dbo.FlatCarPark FCP On A.FlatId=FCP.FlatId AND FCP.TotalCP<>0 " +
                                " LEFT JOIN dbo.CarParkTypeMaster CPTM On FCP.TypeId=CPTM.TypeId " +
                                " Where A.CostCentreId=" + argCCId + " And A.FlatId=" + argFlatId + 
                                " ORDER BY PF.SortOrder";
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

        public static DataTable GetSubAllotmentPrint(int argFlatId)
        {
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                //sSql = "Select B.OCTypeId,A.OtherCostId,B.OtherCostName,'' DuePeriod,A.FlatId,IsNull(A.Amount,0) Amount,IsNull(C.NetAmount,0) NetAmount From dbo.FlatOtherCost A  " +
                //        " Inner Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId"+
                //        " Inner Join dbo.FlatReceiptType C On A.FlatId=C.FlatId" +
                //        " Where A.FlatId=" + argFlatId + " And B.OCTypeId Not In(3,8,2,10) Order By B.SortOrder";

                sSql = "Select X.SortOrder,X.OCTypeId,X.OtherCostId,X.OtherCostName,DuePeriod,Amount,Tax,NetAmount From( " +
                       " Select A.SortOrder,A.OCTypeId,A.OtherCostId,A.OtherCostName,IsNull(C.Description,'')DuePeriod," +
                       " Sum(IsNull(D.Amount,0))Amount,Sum(IsNull(D.NetAmount-D.Amount,0))Tax,Sum(IsNull(D.NetAmount,0)) NetAmount From dbo.OtherCostMaster A  " +
                       " Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId" +
                       " Inner Join dbo.FlatReceiptType D On B.FlatId=D.FlatId And D.OtherCostId=B.OtherCostId  " +
                       " Left Join dbo.PaymentScheduleFlat C On B.FlatId=C.FlatId And C.PaymentSchId=D.PaymentSchId And C.OtherCostId=D.OtherCostId  " +
                       " Where B.FlatId=" + argFlatId + " AND A.OCTypeId NOT IN(2,3,8)" +
                       " Group By A.SortOrder,A.OCTypeId,A.OtherCostId,A.OtherCostName,C.Description " +
                    //" UNION ALL " +
                    //" Select A.SortOrder,A.OCTypeId,A.OtherCostId,A.OtherCostName,''DuePeriod,0 Amount,0 Tax,B.Amount NetAmount From dbo.OtherCostMaster A  " +
                    //" Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId Where B.FlatId=" + argFlatId + " And A.OtherCostId=2" +
                       " )X " +
                       " Inner Join dbo.OtherCostMaster Y On X.OtherCostId=Y.OtherCostId Order By X.SortOrder";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(dt);
                da.Dispose();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dt.Rows[i]["OCTypeId"]) == 1) { dt.Rows[i]["OtherCostName"] = "Registration Fees (Approx)"; }
                    else if (Convert.ToInt32(dt.Rows[i]["OCTypeId"]) == 2) { dt.Rows[i]["OtherCostName"] = "Other Charges"; }
                    else if (Convert.ToInt32(dt.Rows[i]["OCTypeId"]) == 4) { dt.Rows[i]["DuePeriod"] = "To be paid on 45th day from the due date of 2nd installment"; }
                    else if (Convert.ToInt32(dt.Rows[i]["OCTypeId"]) == 5) { dt.Rows[i]["DuePeriod"] = "To be paid after completion of Brick work"; }
                    else if (Convert.ToInt32(dt.Rows[i]["OCTypeId"]) == 6) { dt.Rows[i]["DuePeriod"] = "To be paid after completion of Plastering"; }
                    else if (Convert.ToInt32(dt.Rows[i]["OCTypeId"]) == 7) { dt.Rows[i]["DuePeriod"] = "To be paid before Registration"; }
                    else if (Convert.ToInt32(dt.Rows[i]["OCTypeId"]) == 1) { dt.Rows[i]["DuePeriod"] = ""; }
                    else if (Convert.ToInt32(dt.Rows[i]["OCTypeId"]) == 8) { dt.Rows[i]["DuePeriod"] = "To be paid before HandingOver"; }
                    else if (Convert.ToInt32(dt.Rows[i]["OCTypeId"]) == 3) { dt.Rows[i]["DuePeriod"] = "To be paid before HandingOver"; }
                }
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

        public static DataTable GetSubAllotPaymentPrint(int argFlatId, string argType)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = new DataTable();
            try
            {
                //sSql = "Select A.FlatId,A.Description,A.SchPercent Percentage,Sum(B.Amount)Amount,SUM(B.NetAmount-B.Amount)Tax,A.PaidAmount,A.NetAmount,(A.NetAmount-A.PaidAmount)Balance " +
                //        " From dbo.PaymentScheduleFlat A Inner Join FlatReceiptType B On A.PaymentSchId=B.PaymentSchId And B.SchType<>'A' " +
                //        " Where A.FlatId=" + argFlatId + " Group By A.Description,A.SchPercent,A.SortOrder " +
                //        " Order By A.SortOrder";
                string sSql = "";
                if (argType == "SSRG")
                {
                    sSql = "Select A.FlatId,'' Installment,A.Description,A.SchPercent Percentage,Sum(ISNULL(A.Amount,0))Amount,SUM(ISNULL(A.NetAmount,0)-ISNULL(A.Amount,0))Tax," +
                            " A.PaidAmount,A.NetAmount, (A.NetAmount-A.PaidAmount) Balance  From dbo.PaymentScheduleFlat A" +
                            " Where A.FlatId=" + argFlatId + " AND A.SchType='A' " +
                            " Group By A.SortOrder,A.FlatId,A.Description,A.SchPercent,A.SortOrder,A.PaidAmount,A.NetAmount" +
                            " UNION ALL " +
                            " Select A.FlatId,'' Installment,A.Description,A.SchPercent Percentage,Sum(ISNULL(B.Amount,0))Amount,SUM(ISNULL(B.NetAmount,0)-ISNULL(B.Amount,0))Tax," +
                            " A.PaidAmount,A.NetAmount, (A.NetAmount-A.PaidAmount) Balance  From dbo.PaymentScheduleFlat A" +
                            " INNER Join dbo.FlatReceiptType B On A.PaymentSchId=B.PaymentSchId AND B.SchType<>'A' AND B.SchType<>'O' " +
                            " Where A.FlatId=" + argFlatId + " " +
                            " Group By A.SortOrder,A.FlatId,A.Description,A.SchPercent,A.SortOrder,A.PaidAmount,A.NetAmount";
                }
                else
                {
                    sSql = "Select A.FlatId,'' Installment,A.Description,A.SchPercent Percentage,Sum(ISNULL(B.Amount,0))Amount,SUM(ISNULL(B.NetAmount,0)-ISNULL(B.Amount,0))Tax," +
                            " A.PaidAmount,A.NetAmount, (A.NetAmount-A.PaidAmount) Balance  From dbo.PaymentScheduleFlat A" +
                            " INNER Join dbo.FlatReceiptType B On A.PaymentSchId=B.PaymentSchId And B.SchType<>'A' And B.SchType<>'O' " +
                            " Where A.FlatId=" + argFlatId + " " +
                            " Group By A.FlatId,A.Description,A.SchPercent,A.SortOrder,A.PaidAmount,A.NetAmount" +
                            " Order By A.SortOrder";
                }
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(dt);
                da.Dispose();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 1)
                        dt.Rows[i]["Installment"] = i + "st Installment";
                    else if (i == 2)
                        dt.Rows[i]["Installment"] = i + "nd Installment";
                    else if (i == 3)
                        dt.Rows[i]["Installment"] = i + "rd Installment";
                    else if (i > 3)
                        dt.Rows[i]["Installment"] = i + "th Installment";
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

        public static DataTable GetSubAllotPaymentHandingOverPrint(int argFlatId)
        {
            BsfGlobal.OpenCRMDB();

            DataTable dt = new DataTable();
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Amount", typeof(decimal));
            try
            {
                string sSql = "Select C.OtherCostName Description,Sum(ISNULL(B.Amount,0))Amount " +
                    //" SUM(ISNULL(B.NetAmount,0)-ISNULL(B.Amount,0))Tax, A.PaidAmount,A.NetAmount, (A.NetAmount-A.PaidAmount) Balance "+
                                "  From dbo.PaymentScheduleFlat A" +
                                " INNER Join dbo.FlatReceiptType B On A.PaymentSchId=B.PaymentSchId  " +
                                " INNER Join dbo.OtherCostMaster C On B.OtherCostId=C.OtherCostId " +
                                " Where A.FlatId=" + argFlatId + " And B.SchType='O' AND C.Area=1 " +
                                " GROUP BY C.SortOrder,C.OtherCostName" +
                                " ORDER BY C.SortOrder";
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable getdt = new DataTable();
                da.Fill(getdt);
                da.Dispose();

                string s_HandingOver = "";
                decimal d_HandingAmt = 0;
                for (int i = 0; i < getdt.Rows.Count; i++)
                {
                    if (s_HandingOver == "")
                        s_HandingOver = CommFun.IsNullCheck(getdt.Rows[i]["Description"], CommFun.datatypes.vartypestring).ToString();
                    else
                        s_HandingOver = s_HandingOver + " + " + CommFun.IsNullCheck(getdt.Rows[i]["Description"], CommFun.datatypes.vartypestring).ToString();

                    d_HandingAmt = d_HandingAmt + Convert.ToDecimal(CommFun.IsNullCheck(getdt.Rows[i]["Amount"], CommFun.datatypes.vartypenumeric));
                }

                DataRow drow = dt.NewRow();
                drow["Description"] = s_HandingOver;
                drow["Amount"] = d_HandingAmt;
                dt.Rows.Add(drow);

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

        public static DataTable GetSubSOAPrint(int argFlatId, DateTime argAsOn)
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = " Select FlatId,SortOrder,Date,Description,Receivable Payable,Received Paid " +
                         " FROM(  " +
                         " Select A.FlatId,A.SortOrder,A.SchDate Date,A.Description,A.NetAmount Receivable,0 Received  " +
                         " From dbo.PaymentScheduleFlat A " +
                         " Where A.FlatId=" + argFlatId + " And A.PaidAmount>0 And A.BillPassed=0 And A.SchDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "'  " +
                         " UNION ALL " +
                         " Select A.FlatId,A.SortOrder,RR.ReceiptDate Date,RR.Narration Description,0 Receivable,RT.Amount Received  " +
                         " From dbo.PaymentScheduleFlat A " +
                         " INNER JOIN dbo.ReceiptTrans RT ON RT.PaySchId=A.PaymentSchId  " +
                         " INNER JOIN dbo.ReceiptRegister RR ON RR.ReceiptId=RT.ReceiptId And RR.ReceiptDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "'  " +
                         " Where A.FlatId=" + argFlatId + " And A.PaidAmount>0 And A.BillPassed=0 And A.SchDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "'  " +
                         " ) X Order By X.SortOrder";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds);
                da.Dispose();
                ds.Dispose();
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

        #region FlatDetails

        public static DataTable GetAllotmentRegister(int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Select DISTINCT A.CancelId, A.CancelDate,D.CostCentreName,E.LeadName BuyerName," +
                                "ISNULL(C.AllotmentNo,'') AllotmentNo,B.FlatNo,B.Area,A.NetAmount,A.PenaltyAmt," +
                                " A.BalanceAmount,ISNULL(SUM(F.TotalCP),0) TotalCarParking,A.Remarks," +
                                "Case When A.Approve='' Then 'N' Else A.Approve End Approve From dbo.AllotmentCancel A " +
                                " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre D On A.CostCentreId=D.CostCentreId " +
                                " Left Join dbo.FlatDetails B On A.FlatId=B.FlatId " +
                                " Left Join dbo.BuyerDetail C On C.FlatId=A.FlatId " +
                                " Left Join dbo.LeadRegister E On E.LeadId=A.BuyerId " +
                                " Left Join dbo.CancelledCarPark F On F.CancelId=A.CancelId " +
                                " Where A.CostCentreId=" + argCCId +
                                " GROUP BY A.CancelId, A.CancelDate,D.CostCentreName,E.LeadName,C.AllotmentNo,B.FlatNo,B.Area,A.NetAmount,A.PenaltyAmt," +
                                " A.BalanceAmount,A.Remarks,A.Approve";
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
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
            return dt;
        }

        #endregion

        internal static DataTable PopulateChangedBuyerName(int argiCCId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Select C.AllotmentNo,A.FlatNo,B.BlockName,D.LeadName BuyerName,F.CompletionDate ChangedDate,E.CoApplicantName from dbo.FlatDetails A " +
                                " LEFT JOIN dbo.BlockMaster B ON A.BlockId=B.BlockId " +
                                " INNER JOIN dbo.BuyerDetail C ON A.LeadId=C.LeadId AND A.FlatId=C.FlatId " +
                                " INNER JOIN dbo.LeadRegister D ON C.LeadId=D.LeadId " +
                                " INNER JOIN dbo.LeadCoApplicantInfo E ON D.LeadId=E.LeadId " +
                                " INNER JOIN dbo.FlatCheckList F ON A.FlatId=F.FlatId AND F.CheckListId=1 " +
                                " Where A.CostCentreId=" + argiCCId + " AND E.CoApplicantName<>''";
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

        internal static DataTable GetBrokerCommission(int m_iBrokId, int m_iBranchId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "Select * From dbo.BrokerCC Where CostCentreId=" + m_iBranchId + "";
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

        internal static DataTable PopulateCustomerFeedback(int argFlatId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Select FeedbackDate, Case When FeedbackType=1 Then 'Feedback' When FeedbackType=2 Then 'Queries' " +
                              " When FeedbackType=3 Then 'Complaint' Else 'None' End FeedbackType, Remarks FROM dbo.CustomerFeedback " +
                              " WHERE FlatId=" + argFlatId + " ";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dr.Close();
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
    }
}
