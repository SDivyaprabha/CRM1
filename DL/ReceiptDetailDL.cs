using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessObjects;
using Microsoft.VisualBasic;
using Qualifier;

namespace CRM.DataLayer
{
    class ReceiptDetailDL
    {
        public static DataSet GetReceiptDetE(int argReciptId)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.*,B.CostCentreName,C.FlatId,Isnull(D.Amount,0) ExcessAmount,A.Interest From dbo.ReceiptRegister A " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B On A.CostCentreId=B.CostCentreId  " +
                        " Left Join dbo.ReceiptTrans C On C.ReceiptId=A.ReceiptId " +
                        " Left Join dbo.ExtraPayment D On D.ReceiptId=A.ReceiptId " +
                        " Where A.ReceiptId=" + argReciptId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Register");
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public static DataTable GetReceiptTypeTrans(int argReciptId, int argFlatId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Select  A.FlatId,A.PaymentSchId,A.ReceiptTypeId,A.OtherCostId,Case When E.SchType='R' Or E.SchType='A' Then B.ReceiptTypeName " +
                                " When E.SchType='Q' Then D.QualifierName Else C.OtherCostName End Description,A.GrossAmount, A.TaxAmount, A.NetAmount,"+
                                " A.PaidAmount, A.PaidGrossAmount,A.PaidTaxAmount,A.PaidNetAmount,A.PaidNetAmount HPaidNetAmount,isnull(QA.AccountId,0) AccountId," +
                                " E.SchType,Case When E.SchType='R' Or E.SchType='A' Then 1 When E.SchType='Q' Then 3 Else 2 End SOrder From ReceiptShTrans A " +
                                " Left Join ReceiptType B On A.ReceiptTypeId=B.ReceiptTypeId " +
                                " Left Join OtherCostMaster C On A.OtherCostId=C.OtherCostId " +
                                " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp D On D.QualifierId=A.ReceiptTypeId " +
                                " Left Join FlatReceiptType E On E.PaymentSchId=A.PaymentSchId And E.ReceiptTypeId=A.ReceiptTypeId And E.OtherCostId=A.OtherCostId " +
                                " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp Q On Q.QualifierId=A.ReceiptTypeId " +
                                " Left Join dbo.QualifierAccount QA on QA.QualifierId=Q.QualifierId " +                                
                                " Where A.ReceiptId= " + argReciptId + " AND A.FlatId=" + argFlatId + " And E.NetAmount<>0 ";
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

        public static DataTable GetQualifierTrans(int argReciptId, string argPaymentOpt)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "";
                if (argPaymentOpt == "E")
                {
                    sSql = "Select * From dbo.ReceiptExtraBillQualifier Where ReceiptId= " + argReciptId;
                }
                else
                {
                    sSql = "Select A.FlatId,A.PaymentSchId,C.QualTypeId,A.QualifierId,A.Expression,A.ExpPer,A.NetPer, " +
                            " A.Add_Less_Flag,A.SurCharge,A.EDCess,A.HEDPer,A.ExpValue,A.ExpPerValue,A.SurValue,A.EDValue," +
                            " A.Amount,A.ReceiptTypeId,A.OtherCostId,A.TaxablePer,A.TaxableValue,IsNull(B.Service,'')Service From dbo.ReceiptQualifier A " +
                            " Left Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId " +
                            " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On C.QualifierId=A.QualifierId " +
                            " Where A.ReceiptId= " + argReciptId;
                }
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

        public static DataTable GetQualifierAbs(int argReciptId, string argPaymentOpt)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "";
                if (argPaymentOpt == "E")
                {
                    sSql = "Select * From dbo.ReceiptExtraBillQualifierAbs Where ReceiptId= " + argReciptId;
                }
                else
                {
                    sSql = "Select B.FlatId,B.ReceiptTypeId,B.OtherCostId,A.PaymentSchId,A.QualifierId, " +
                                  " A.ReceiptId,A.ReceiptTransId,A.PBillId,A.AccountId,A.Add_Less_Flag,A.Amount From dbo.ReceiptQualifierAbs A  " +
                                  " Inner Join dbo.ReceiptShTrans B On A.ReceiptId=B.ReceiptId And A.PaymentSchId=B.PaymentSchId And A.Amount=B.PaidTaxAmount " +
                                  " Where A.ReceiptId= " + argReciptId;
                }
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

        internal static DataSet GetEBQualifier(int argReceiptId, int argFlatId, string argPaymentOpt)
        {
            BsfGlobal.OpenCRMDB();
            DataSet ds = null;
            try
            {
                string sSql = "";
                if (argReceiptId == 0)
                {
                    sSql = "Select * from dbo.ExtraBillRateQ Where FlatId=" + argFlatId + "";
                    SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    ds = new DataSet();
                    sda.Fill(ds, "Qualifier");
                    sda.Dispose();

                    sSql = "Select * from dbo.ExtraBillRateQAbs " +
                            " Where BillRegId IN(Select BillRegId from dbo.ExtraBillRegister Where FlatId=" + argFlatId + ")";
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    sda.Fill(ds, "QualifierAbs");
                    sda.Dispose();
                }
                else
                {
                    sSql = "Select * from dbo.ReceiptExtraBillQualifier Where ReceiptId=" + argReceiptId + "";
                    SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    ds = new DataSet();
                    sda.Fill(ds, "Qualifier");
                    sda.Dispose();

                    sSql = "Select * from dbo.ReceiptExtraBillQualifierAbs Where ReceiptId=" + argReceiptId + " ";
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    sda.Fill(ds, "QualifierAbs");
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
            return ds;
        }

        public static DataTable GetPBQualifierAbs(int argReciptId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            sSql = "Select B.FlatId,B.PaySchId PaymentSchId,A.QualifierId,A.AccountId,A.Add_Less_Flag,A.Amount From dbo.PBQualifierAbs A  " +
                        " Inner Join dbo.ProgressBillRegister B on A.PBillId=B.PBillId Where A.PBillId= " + argReciptId;
            sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            dt = new DataTable();
            sda.Fill(dt);
            sda.Dispose();
            BsfGlobal.g_CRMDB.Close();
            return dt;
        }

        public static DataTable GetReceiptTransE(int argReciptId, string argBillType, int argBuyerId,int argFlatId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                SqlDataAdapter sda = null;
                string sSql = "";
                if (argBillType == "B")
                {
                    sSql = "Select * From dbo.ReceiptTrans Where ReceiptId=" + argReciptId + "";
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    dt = new DataTable();
                    sda.Fill(dt);
                    sda.Dispose();

                    if (dt.Rows.Count > 0)
                    {
                        sSql = " Select ReceiptId,[SchDate/BillDate],CostCentreId,FlatId,FlatNo,ReceiptType,BillRegId,PaymentSchId,Typewise,SchType,BillNo,Description," +
                                " Gross,NetAmount,PaidAmount,BalanceAmount,Amount,HBalance,HAmount,Advance,RAmt,SortOrder from (" +
                                " Select RR.ReceiptId,A.PBDate [SchDate/BillDate],A.CostCentreId,A.FlatId,B.FlatNo,'ProgressBill' ReceiptType, " +
                                " A.PBillId BillRegId,A.PaySchId PaymentSchId,PT.Typewise,PF.SchType, A.PBNo BillNo,PF.Description, " +
                                " A.BillAmount Gross,RT.NetAmount,A.PaidAmount,(RT.NetAmount)-A.PaidAmount BalanceAmount, " +
                                " RT.Amount,(RT.NetAmount)-A.PaidAmount HBalance,RT.Amount HAmount,IsNull(PF.Advance,0)Advance, " +
                                " RAmt=(Select IsNull(Sum(Amount),0) From ReceiptTrans Where ReceiptId<>" + argReciptId + //" AND ReceiptId<" + argReciptId + 
                                " And FlatId=" + argFlatId + " And PaySchId=PF.PaymentSchId), " +
                                " PF.SortOrder From dbo.ProgressBillRegister A " +
                                " Inner Join ProgressBillMaster M On A.ProgRegId=M.ProgRegId  " +
                                " Inner Join FlatDetails B on A.FlatId=B.FlatId " +
                                " Left Join dbo.PaymentScheduleFlat PF On PF.PaymentSchId=A.PaySchId" +
                                " Inner Join dbo.ReceiptRegister RR On RR.FlatId=B.FlatId " +
                                " Inner Join dbo.ReceiptTrans RT On RT.BillRegId=A.PBillId And RR.ReceiptId=RT.ReceiptId " +
                                " Inner Join dbo.PaySchType PT On PT.TypeId=B.PayTypeId " +
                                " Where A.LeadId=" + argBuyerId + " And RR.ReceiptId=" + argReciptId + "" +
                                " Union All " +
                                " Select D.ReceiptId,A.SchDate [SchDate/BillDate],B.CostCentreId,B.FlatId,B.FlatNo,'ScheduleBill' ReceiptType,0 BillRegId,A.PaymentSchId, " +
                                " C.Typewise,A.SchType,'' BillNo,A.Description,A.Amount Gross,D.NetAmount,A.PaidAmount, " +
                                " (D.NetAmount)-A.PaidAmount BalanceAmount,D.Amount,(D.NetAmount)-A.PaidAmount HBalance, D.Amount HAmount,IsNull(A.Advance,0)Advance," +
                                " RAmt=(Select IsNull(Sum(Amount),0) From dbo.ReceiptTrans Where ReceiptId<>" + argReciptId + //" AND ReceiptId<" + argReciptId + 
                                " And FlatId=" + argFlatId + " And PaySchId=A.PaymentSchId), " +
                                " A.SortOrder From PaymentScheduleFlat A  " +
                                " Inner Join dbo.FlatDetails B On A.FlatId=B.FlatId " +
                                " Inner Join dbo.PaySchType C On C.TypeId=B.PayTypeId   " +
                                " Inner Join dbo.ReceiptTrans D On D.PaySchId=A.PaymentSchId And D.BillRegId=0" +
                                " Where LeadId=" + argBuyerId + " And D.ReceiptId=" + argReciptId + " And TemplateId<>0) X " +
                                " ORDER BY X.SortOrder";
                    }
                    else
                    {
                        sSql = "Select ReceiptId,[SchDate/BillDate],CostCentreId,FlatId,FlatNo,ReceiptType,BillRegId,PaymentSchId,Typewise,SchType,BillNo,Description," +
                                " Gross,NetAmount,PaidAmount,BalanceAmount,Amount,HBalance,HAmount,Advance,RAmt,SortOrder from (" +
                                " Select RR.ReceiptId,A.PBDate [SchDate/BillDate],A.CostCentreId,A.FlatId,B.FlatNo,'ProgressBill' ReceiptType,A.PBillId BillRegId,A.PaySchId PaymentSchId,PT.Typewise,PF.SchType,  " +
                                " A.PBNo BillNo,PF.Description,A.BillAmount Gross,PF.NetAmount,A.PaidAmount,(PF.NetAmount)-A.PaidAmount BalanceAmount,  " +
                                " 0 Amount,(PF.NetAmount)-A.PaidAmount HBalance,0 HAmount,IsNull(PF.Advance,0)Advance,RAmt=(Select IsNull(Sum(Amount),0) " +
                                " From dbo.ReceiptTrans Where ReceiptId<>" + argReciptId + " And FlatId=" + argFlatId + " And PaySchId=PF.PaymentSchId), 0 SortOrder From dbo.ProgressBillRegister A Inner Join ProgressBillMaster M On A.ProgRegId=M.ProgRegId  " +
                                " Inner Join dbo.FlatDetails B on A.FlatId=B.FlatId Left Join dbo.PaymentScheduleFlat PF On PF.PaymentSchId=A.PaySchId" +
                                " Inner Join dbo.ReceiptRegister RR On RR.FlatId=B.FlatId " +
                                " Inner Join dbo.PaySchType PT On PT.TypeId=B.PayTypeId Where A.LeadId=" + argBuyerId + " And RR.ReceiptId=" + argReciptId + " And (PF.NetAmount)-A.PaidAmount >0 " +
                                " Union All " +
                                " Select RR.ReceiptId,A.SchDate [SchDate/BillDate],B.CostCentreId,B.FlatId,B.FlatNo,'ScheduleBill' ReceiptType,0 BillRegId,A.PaymentSchId, " +
                                " C.Typewise,A.SchType,'' BillNo,A.Description,A.Amount Gross,A.NetAmount,A.PaidAmount,(A.NetAmount)-A.PaidAmount BalanceAmount," +
                                " 0 Amount,(A.NetAmount)-A.PaidAmount HBalance,0 HAmount,IsNull(A.Advance,0)Advance, " +
                                " RAmt=(Select IsNull(Sum(Amount),0) From dbo.ReceiptTrans Where ReceiptId<>" + argReciptId + " And FlatId=" + argFlatId + " And PaySchId=A.PaymentSchId), " +
                                " A.SortOrder From dbo.PaymentScheduleFlat A Inner Join dbo.FlatDetails B On A.FlatId=B.FlatId " +
                                " Inner Join dbo.PaySchType C On C.TypeId=B.PayTypeId Inner Join dbo.ReceiptRegister RR On RR.FlatId=B.FlatId " +
                                " Where RR.LeadId=" + argBuyerId + " And RR.ReceiptId=" + argReciptId + " And TemplateId<>0 And A.BillPassed=0) X " +
                                " ORDER BY X.SortOrder";
                    }
                }
                if (argBillType == "R")
                {
                    sSql = "Select E.ReceiptId,B.TransId,B.RentTransId RentId,B.Date  BillDate,'RentBill' ReceiptType,A.CostCentreId," +
                             " A.FlatId,D.CostCentreName,C.FlatNo,B.Rent NetAmount,B.PaidAmount,B.Rent-PaidAmount " +
                             " BalanceAmount,E.Amount  from RentDetail A " +
                             " Inner Join RentAgreementTrans D1 on A.RentId=D1.RentId " +
                             " Inner join (select  max(RentTransId) as RentTransId from RentAgreementTrans) as E1 on D1.RentTransId = E1.RentTransId  " +
                             " Inner Join RentSchTrans B on D1.RentTransId=B.RentTransId " +
                             " Inner Join FlatDetails C on A.FlatId=C.FlatId " +
                             " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre D on A.CostCentreId = D.CostCentreId " +
                             " Inner Join ReceiptTrans E on B.TransId = E.BillRegId and E.ReceiptId=" + argReciptId + "";
                }
                if (argBillType == "O")
                {
                    sSql = "Select A.ReceiptId,A.FlatId,A.CostCentreId,B.FlatNo,C.CostCentreName,A.ReceiptType,A.Amount from ReceiptTrans A " +
                           "Inner Join FlatDetails B on A.FlatId=B.FlatId " +
                           "Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C on A.CostCentreId=C.CostCentreId " +
                           "Where A.ReceiptId=" + argReciptId;
                }
                if (argBillType == "A")
                {
                    sSql = "Select D.ReceiptId,A.PaymentSchId,B.CostCentreId,A.TemplateId,B.PayTypeId,C.Typewise,A.SchType,B.FlatId,A.SchDate," +
                            " B.FlatNo,D.ReceiptType,A.Description,A.NetAmount,A.PaidAmount,(A.NetAmount)-A.PaidAmount BalanceAmount, " +
                            " D.Amount,(A.NetAmount)-A.PaidAmount HBalance,D.Amount HAmount,IsNull(A.Advance,0)Advance,0 RAmt From PaymentScheduleFlat A" +
                            " Inner Join FlatDetails B On A.FlatId=B.FlatId  " +
                            " Inner Join PaySchType C On C.TypeId=B.PayTypeId  " +
                            " LEFT Join ReceiptTrans D On D.PaySchId=A.PaymentSchId" +
                            " INNER JOIN ReceiptRegister E ON A.FlatId=E.FlatId " +
                            " Where B.LeadId=" + argBuyerId + " And E.ReceiptId=" + argReciptId + " And SchType='A' Order By A.SortOrder";
                }
                if (argBillType == "S")
                {
                    sSql = "Select D.ReceiptId,A.PaymentSchId,B.CostCentreId,A.TemplateId,B.PayTypeId,C.Typewise,A.SchType,B.FlatId,A.SchDate," +
                            " B.FlatNo,D.ReceiptType,A.Description,A.Amount Gross,D.NetAmount,A.PaidAmount,(D.NetAmount)-A.PaidAmount BalanceAmount, " +
                            " D.Amount,(D.NetAmount)-A.PaidAmount HBalance,D.Amount HAmount,IsNull(A.Advance,0)Advance,RAmt=(Select IsNull(Sum(Amount),0) From ReceiptTrans " +
                            " Where ReceiptId<>" + argReciptId + " And FlatId=" + argFlatId + " And PaySchId=A.PaymentSchId) From PaymentScheduleFlat A  Inner Join FlatDetails B On A.FlatId=B.FlatId  " +
                            " Inner Join PaySchType C On C.TypeId=B.PayTypeId  " +
                            " Inner Join ReceiptTrans D On D.PaySchId=A.PaymentSchId" +
                            " Where LeadId=" + argBuyerId + " And D.ReceiptId=" + argReciptId + " And TemplateId<>0  Order By A.SortOrder";
                }
                if (argBillType == "E")
                {
                    sSql = " Select RT.ReceiptId,RT.BillRegId,RR.ReceiptDate BillDate,RR.ReceiptNo BillNo,RT.ReceiptType,RT.CostCentreId,RT.FlatId, "+
                            " RR.LeadId,C.LeadName BuyerName,RT.GrossAmount Gross,RT.NetAmount,RT.PaidAmount, " +
                            " RT.NetAmount-RT.PaidAmount BalanceAmount,RT.Amount,RT.Amount BreakUpAmount from dbo.ReceiptTrans RT " +
                            " Inner Join dbo.ReceiptRegister RR on RT.ReceiptId=RR.ReceiptId " +
                            " Inner Join dbo.LeadRegister C on RR.LeadId=C.LeadId " +
                            " Inner Join dbo.ExtraBillRegister D on RT.BillRegId=D.BillRegId " +
                            " Where RR.LeadId=" + argBuyerId + " And RT.FlatId=" + argFlatId + " And RT.ReceiptId=" + argReciptId + " ";
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

        public static DataTable GetPlotReceiptTransE(int argReciptId, string argBillType, int argBuyeId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                if (argBillType == "PB" || argBillType == "B")
                {
                    sSql = "Select D.ReceiptId,A.CostCentreId,C.CostCentreName,A.PlotDetailsId FlatId,B.PlotNo FlatNo, D.ReceiptType,A.PBDate BillDate ," +
                            " A.PBillId BillRegId,A.PBNo BillNo,A.NetAmount,A.PaidAmount,A.NetAmount-A.PaidAmount BalanceAmount,D.Amount,A.PaySchId PaymentSchId " +
                            " from dbo.PlotProgressBillRegister A " +
                            " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails B on A.PlotDetailsId=B.PlotDetailsId " +
                            " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C on A.CostCentreId= C.CostCentreId " +
                            " Inner Join ReceiptTrans D on A.PBillId=D.BillRegId and D.ReceiptId=" + argReciptId +
                            " Where A.BuyerId=" + argBuyeId + " and A.NetAmount-A.PaidAmount>0 " +
                            " Union All " +
                            " Select D.ReceiptId,A.LandRegId CostCentreId,C.CostCentreName,A.PlotDetailsId FlatId,B.PlotNo FlatNo,D.ReceiptType,A.SchDate BillDate,0 BillRegId," +
                            " '' BillNo,A.NetAmount,D.Amount PaidAmount,A.NetAmount-D.Amount BalanceAmount,D.Amount,A.PaymentSchId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot A " +
                            " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails B on A.PlotDetailsId=B.PlotDetailsId " +
                            " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C ON A.LandRegId=C.CostCentreId " +
                            " Inner Join ReceiptTrans D ON A.PaymentSchId=D.PaySchId AND D.ReceiptId=" + argReciptId + " " +
                            " Where B.BuyerId=" + argBuyeId + " and A.NetAmount>0 ";
                }
                if (argBillType == "PR" || argBillType == "R")
                {
                    sSql = "Select E.ReceiptId,B.TransId,B.RentId,B.Date  BillDate,'RentBill' ReceiptType,A.CostCentreId,A.FlatId,D.CostCentreName,C.FlatNo,B.Rent NetAmount,B.PaidAmount,B.Rent-PaidAmount BalanceAmount,E.Amount  from RentDetail A " +
                       "Inner Join RentSchTrans B on A.RentId=B.RentId " +
                       "Inner Join FlatDetails C on A.FlatId=C.FlatId " +
                       "Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre D on A.CostCentreId= D.CostCentreId " +
                       "Inner Join ReceiptTrans E on B.TransId = E.BillRegId and E.ReceiptId=" + argReciptId + " " +
                       "where B.Rent-PaidAmount>0 ";
                }
                if (argBillType == "PA" || argBillType == "PO" || argBillType == "A" || argBillType == "O")
                {
                    sSql = "Select A.ReceiptId,A.FlatId,A.CostCentreId,B.PlotNo FlatNo,C.CostCentreName,A.ReceiptType,A.Amount from ReceiptTrans A " +
                            " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails B on A.FlatId=B.PlotDetailsId " +
                            " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C " +
                            " on A.CostCentreId=C.CostCentreId Where A.ReceiptId=" + argReciptId;

                }
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable GetReceiptPayment( int argBuyerId, string arg_sStage, int argTenant,int argCCId,DateTime argDate,int argFlatId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                if (arg_sStage == "A")
                {
                    if (argTenant > 0)
                    {
                        sSql = String.Format("Select A.FlatId,B.FlatNo,'Advance' ReceiptType,B.CostCentreId,Cast(0 as Decimal(18,3)) Amount from RentDetail A " +
                            "Inner Join FlatDetails B on A.FlatId=B.FlatId " +
                             "Where A.TenantId={0} And A.FlatId=" + argFlatId + "", argTenant);
                    }
                    else
                    {
                        sSql = "Select A.PaymentSchId,B.CostCentreId,A.TemplateId,B.PayTypeId,C.Typewise,A.SchType,B.FlatId,A.SchDate,B.FlatNo," +
                                " 'Advance' ReceiptType,A.Description,A.NetAmount,A.PaidAmount,(A.NetAmount-A.SurplusAmount)-A.PaidAmount BalanceAmount, " +
                                " Cast(0 as Decimal(18,3)) Amount,A.NetAmount-A.PaidAmount HBalance,0 HAmount,A.Advance,0 RAmt From PaymentScheduleFlat A Inner Join FlatDetails B On A.FlatId=B.FlatId  " +
                                " Inner Join PaySchType C On C.TypeId=B.PayTypeId Where LeadId=" + argBuyerId + " And B.FlatId=" + argFlatId + " And SchType='A' And (A.NetAmount-A.SurplusAmount)-A.PaidAmount>0  " +
                                " Order By A.SortOrder";
                    }
                }

                if (arg_sStage == "B")
                {
                    sSql = "Select A.PBDate BillDate,A.CostCentreId,A.FlatId,B.FlatNo,'ProgressBill' ReceiptType,A.PBillId BillRegId,A.PaySchId,  " +
                           " A.PBNo BillNo,A.NetAmount,A.SurplusAmount,PaidAmount,(A.NetAmount-A.SurplusAmount)-PaidAmount BalanceAmount, " +
                           " Cast(0 as Decimal(18,3)) Amount  from ProgressBillRegister A Inner Join ProgressBillMaster M On A.ProgRegId=M.ProgRegId " +
                           " Inner Join FlatDetails B on A.FlatId=B.FlatId Where A.LeadId=" + argBuyerId + " And A.FlatId=" + argFlatId +
                           " And (A.NetAmount-A.SurplusAmount)-PaidAmount>0 " +
                           " And M.Approve='Y' " +
                           " Union All " +
                           " Select A.BillDate,A.CostCentreId,A.FlatID FlatId,B.FlatNo,'ServiceBill' ReceiptType,A.RegBillId BillRegId , " +
                           " 0 PaySchId,A.BillRefNo BillNo,A.NetAmt NetAmount,0 SurplusAmount,PaidAmount,A.NetAmt-PaidAmount BalanceAmount, " +
                           " Cast(0 as Decimal(18,3)) Amount from SerOrderBillReg A Inner Join FlatDetails B on A.FlatId=B.FlatId " +
                           " Where B.LeadId=" + argBuyerId + " And A.FlatID=" + argFlatId + " And A.NetAmt-PaidAmount>0  ";
                }

                if (arg_sStage == "O")
                {
                    if (argTenant > 0)
                    {
                        sSql = String.Format("Select A.FlatId,B.FlatNo,'Others' ReceiptType,B.CostCentreId,Cast(0 as Decimal(18,3)) Amount from RentDetail A " +
                                              "Inner Join FlatDetails B on A.FlatId=B.FlatId " +
                                              "Where A.TenantId={0} And A.FlatId=" + argFlatId + "", argTenant);
                    }
                    else
                    {
                        sSql = String.Format("Select A.FlatId,A.FlatNo,'Others' ReceiptType,A.CostCentreId,Cast(0 as Decimal(18,3)) Amount from FlatDetails A " +
                                             "Where A.LeadId={0} And A.FlatId=" + argFlatId + "", argBuyerId);
                    }
                }

                if (arg_sStage == "R")
                {
                    sSql = "Select B.Date BillDate,B.TransId,B.RentTransId RentId,A.CostCentreId,A.FlatId,C.FlatNo,'RentBill' ReceiptType,B.Rent NetAmount,B.PaidAmount,B.Rent-PaidAmount BalanceAmount,Cast(0 as Decimal(18,3)) Amount from RentDetail A " +
                            "Inner Join RentAgreementTrans D on A.RentId=D.RentId " +
                            "Inner join (select  max(RentTransId) as RentTransId from RentAgreementTrans) as E on D.RentTransId = E.RentTransId " +
                            "Inner Join RentSchTrans B on D.RentTransId=B.RentTransId  " +
                            "Inner Join FlatDetails C on A.FlatId=C.FlatId " +
                            "Where A.TenantId=" + argTenant + " And A.FlatId=" + argFlatId + " and B.Rent-PaidAmount>0 Order by B.Date";
                }

                if (arg_sStage == "S")
                {
                    sSql = "Select A.PaymentSchId,B.CostCentreId,A.TemplateId,B.PayTypeId,C.Typewise,A.SchType,B.FlatId,A.SchDate,B.FlatNo, " +
                            " 'ScheduleBill' ReceiptType,A.Description,A.Amount Gross,A.NetAmount,A.SurplusAmount,A.PaidAmount, " +
                            " (A.NetAmount-A.SurplusAmount)-A.PaidAmount BalanceAmount, Cast(0 as Decimal(18,3)) Amount " +
                            " From PaymentScheduleFlat A  Inner Join FlatDetails B On A.FlatId=B.FlatId  " +
                            " Inner Join PaySchType C On C.TypeId=B.PayTypeId  Where LeadId=" + argBuyerId + " And B.FlatId=" + argFlatId + " And TemplateId<>0 " +
                            " And (A.NetAmount-A.SurplusAmount)-A.PaidAmount>0 And A.BillPassed=0 " +
                            " And A.SchDate<='" + string.Format(Convert.ToDateTime(argDate).ToString("dd-MMM-yyyy")) + "' "+
                            " Order By A.SortOrder";
                }

                if (arg_sStage == "E")
                {
                    sSql = " Select A.BillRegId,A.BillDate,A.BillNo,'ExtraBill' ReceiptType,A.CostCentreId,A.FlatId,B.LeadId,C.LeadName BuyerName, " +
                            " A.BillAmount Gross, A.NetAmount,A.PaidAmount,A.NetAmount-A.PaidAmount BalanceAmount,Cast(0 as Decimal(18,3)) Amount FROM dbo.ExtraBillRegister A " +
                            " Inner Join dbo.FlatDetails B on A.FlatId=B.FlatId " +
                            " Inner Join dbo.LeadRegister C on B.LeadId=C.LeadId " +
                            " Where B.LeadId=" + argBuyerId + " And A.FlatId=" + argFlatId + " And (A.NetAmount-A.PaidAmount)>0 ";
                }

                if (sSql != "")
                {
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    dt = new DataTable();
                    sda.Fill(dt);
                    sda.Dispose();
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
            return dt;
        }

        public static DataTable GetPlotReceiptPayment(int argBuyerId, string arg_sStage, int argTenant, int argCCId,int argLandId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "";
                if (arg_sStage == "A")
                {
                    if (argTenant > 0)
                    {
                        sSql = String.Format("Select A.FlatId,B.FlatNo,'PlotAdvance' ReceiptType,B.CostCentreId,Cast(0 as Decimal(18,3)) Amount from RentDetail A " +
                                             "Inner Join FlatDetails B on A.FlatId=B.FlatId " +
                                             "Where A.TenantId={0}", argTenant);
                    }
                    else
                    {
                        sSql = "Select X.FlatId,X.FlatNo,X.ReceiptType,X.CostCentreId,SUM(X.NetAmount)NetAmount," +
                                " SUM(X.Amount) PaidAmount, SUM(X.NetAmount-X.Amount) BalanceAmount,SUM(X.Amount) Amount From( " +
                                " Select A.PlotDetailsId FlatId,A.PlotNo FlatNo,'PlotAdvance' ReceiptType," + argCCId + " CostCentreId, " +
                                " A.AdvanceAmount NetAmount, 0 Amount from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A " +
                                " Where A.BuyerId=" + argBuyerId + " And A.PlotDetailsId=" + argLandId + "  " +
                                " Union All " +
                                " Select C.PlotDetailsId FlatId,C.PlotNo FlatNo,'PlotAdvance' ReceiptType,A.CostCentreId, " +
                                " 0 NetAmount, Sum(B.Amount) Amount from ReceiptRegister A " +
                                " Inner Join ReceiptTrans B On B.ReceiptId=A.ReceiptId " +
                                " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails C On C.BuyerId=A.LeadId And C.PlotDetailsId=B.FlatId  " +
                                " Where C.BuyerId=" + argBuyerId + " And A.CostCentreId=" + argCCId + " And PaymentAgainst='PA' AND C.PlotDetailsId=" + argLandId + "" +
                                " GROUP BY C.PlotDetailsId,C.PlotNo,ReceiptType,A.CostCentreId) X " +
                                " GROUP BY X.FlatId,X.FlatNo,X.ReceiptType,X.CostCentreId HAVING SUM(X.NetAmount)<>SUM(X.Amount)";
                    }
                }
                if (arg_sStage == "O")
                {
                    if (argTenant > 0)
                    {

                        sSql = String.Format("Select A.FlatId,B.FlatNo,'PlotOthers' ReceiptType,B.CostCentreId,Cast(0 as Decimal(18,3)) Amount from RentDetail A " +
                                             "Inner Join FlatDetails B on A.FlatId=B.FlatId " +
                                             "Where A.TenantId={0}", argTenant);
                    }
                    else
                    {
                        sSql = "Select A.PlotDetailsId FlatId,A.PlotNo FlatNo,'PlotOthers' ReceiptType," + argCCId + " CostCentreId, " +
                               " Cast(0 as Decimal(18,3)) Amount From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A " +
                               " Where A.BuyerId=" + argBuyerId + " AND A.PlotDetailsId=" + argLandId +" ";
                    }
                }

                if (arg_sStage == "B")
                {
                    sSql = "Select A.PBDate BillDate,A.PBNo BillNo,A.CostCentreId,A.PaySchId PaymentSchId,A.PlotDetailsId FlatId,B.PlotNo FlatNo,'PlotProgressBill' ReceiptType,A.PBillId BillRegId," +
                            " A.NetAmount,PaidAmount,A.NetAmount-PaidAmount BalanceAmount,Cast(0 as Decimal(18,3)) Amount from dbo.PlotProgressBillRegister A " +
                            " Inner Join dbo.PlotProgressBillMaster M On A.ProgRegId=M.ProgRegId " +
                            " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails B on A.PlotDetailsId=B.PlotDetailsId " +
                            " Where A.BuyerId=" + argBuyerId + " And M.Approve='Y' And (A.NetAmount-PaidAmount)>0 AND A.PlotDetailsId=" + argLandId + " " +
                            " Union All " +
                            " Select ISNULL(A.SchDate, '') BillDate, '' BillNo,A.LandRegId CostCentreId,A.PaymentSchId, " +
                            " A.PlotDetailsId FlatId,B.PlotNo FlatNo,'PlotScheduleBill' ReceiptType, " +
                            " 0 BillRegId,A.NetAmount,ISNULL(C.Amount,0) PaidAmount,A.NetAmount-ISNULL(C.Amount,0) BalanceAmount, " +
                            " Cast(0 as Decimal(18,3)) Amount  from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot A  " +
                            " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails B on A.PlotDetailsId=B.PlotDetailsId  " +
                            " LEFT Join dbo.ReceiptTrans C ON A.PaymentSchId=C.PaySchId AND A.PlotDetailsId=C.FlatId " +
                            " Where B.BuyerId=" + argBuyerId + " AND A.SchType<>'A' AND A.BillPassed=0 " +
                            " AND A.NetAmount<>ISNULL(C.Amount,0) AND A.PlotDetailsId=" + argLandId + "";

                            //" Select A.BillDate,A.CostCentreId,A.FlatID FlatId,B.PlotNo FlatNo,'ScheduleBill' ReceiptType,A.RegBillId BillRegId," +
                            //" A.BillRefNo BillNo,A.NetAmt NetAmount,PaidAmount,A.NetAmt-PaidAmount BalanceAmount,Cast(0 as Decimal(18,3)) Amount " +
                            //" from SerOrderBillReg A Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails B on A.FlatId=B.PlotDetailsId "+
                            //" Where B.BuyerId=" + argBuyerId + " AND (A.NetAmt-PaidAmount)>0 ";
                }

                if (arg_sStage == "R")
                {
                    sSql = "Select B.Date BillDate,B.TransId,B.RentId,A.CostCentreId,A.FlatId,C.FlatNo,'PlotRentBill' ReceiptType,B.Rent NetAmount,"+
                           " B.PaidAmount,B.Rent-PaidAmount BalanceAmount,Cast(0 as Decimal(18,3)) Amount from RentDetail A " +
                           "Inner Join RentSchTrans B on A.RentId=B.RentId " +
                           "Inner Join FlatDetails C on A.FlatId=C.FlatId " +
                           "Where A.TenantId=" + argTenant + " and B.Rent-PaidAmount>0 Order by B.Date";
                }

                if (sSql != "")
                {
                    SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    dt = new DataTable();
                    sda.Fill(dt);
                    sda.Dispose();
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
            return dt;
        }

        public static DataTable GetPaymentMode()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = new DataTable();
            try
            {
                string sSql = "Select Distinct PaymentMode from ReceiptRegister Where PaymentMode <>''";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        public static bool GetPaymentAdvance(int argFlatId)
        {
            DataTable dt = new DataTable(); string sSql = "";
            BsfGlobal.OpenCRMDB(); bool b_Ans = false;
            try
            {
                sSql = "Select * From dbo.PaymentScheduleFlat Where PaidAmount<>NetAmount " +
                        " And FlatId=" + argFlatId + " And TemplateId=0";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    b_Ans = true;
                }
                else b_Ans = false;
                dt.Dispose();

                BsfGlobal.g_CRMDB.Close();
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return b_Ans;
        }

        public static DataSet GetFlatReceiptNew(int argCCId, int argBuyerId, DateTime argDate,int argFlatId)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter sda;
            string sSql = "";

            cRateQualR RAQual = new cRateQualR();
            Collection QualVBC = new Collection();

            BsfGlobal.OpenCRMDB();
           
            sSql = "Update FlatReceiptType Set Amount = NetAmount Where Amount=0 And NetAmount>0";
            SqlCommand cmd = new SqlCommand(sSql,BsfGlobal.g_CRMDB);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Select A.FlatId,A.PaymentSchId,B.CostCentreId,A.TemplateId,B.PayTypeId,C.Typewise,A.SchType,A.SchDate,B.FlatNo, " +
                   " 'ScheduleBill' ReceiptType,A.Description,A.Amount Gross,A.NetAmount,A.SurplusAmount,A.PaidAmount, " +
                   " (A.NetAmount)-A.PaidAmount BalanceAmount, Cast(0 as Decimal(18,3)) Amount,A.NetAmount-A.PaidAmount HBalance,0 HAmount,IsNull(A.Advance,0)Advance,0 RAmt " +
                   " From dbo.PaymentScheduleFlat A  Inner Join dbo.FlatDetails B On A.FlatId=B.FlatId  " +
                   " Inner Join dbo.PaySchType C On C.TypeId=B.PayTypeId  Where LeadId=" + argBuyerId + " And A.FlatId=" + argFlatId + " And TemplateId<>0 " +
                   " And (A.NetAmount)-A.PaidAmount>0 And A.BillPassed=0 " +
                   " Order By A.FlatId,A.SortOrder";
            sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            sda.Fill(ds,"PaymentSchedule");
            sda.Dispose();

            SqlDataReader dr;

            int iPaySchId=0;
            int iQualId=0;
            int iFlatId = 0;
            bool bTypewise = false;
            string sSchType = ""; bool bSchAdv = false;
            bool bService = false;

            decimal dTNetAmt=0;
            decimal dTTaxAmt=0;
            int iReceiptId = 0;
            int iOthId = 0;
            decimal dAdvAmt = 0; decimal dAdv = 0; decimal dSchAdvance = 0;

            decimal dRTAmt = 0;

            decimal dQNetAmt = 0;
            decimal dTaxAmt = 0;

            decimal dNetAmt = 0;

            DataRow dRowT;

            DataTable dtR = new DataTable();
            dtR.Columns.Add("FlatId", typeof(Int32));
            dtR.Columns.Add("PaymentSchId", typeof(Int32));
            dtR.Columns.Add("ReceiptTypeId", typeof(Int32));
            dtR.Columns.Add("OtherCostId", typeof(Int32));
            dtR.Columns.Add("Description", typeof(string));
            dtR.Columns.Add("AccountId", typeof(Int32));
            dtR.Columns.Add("SchType", typeof(string));
            dtR.Columns.Add("GrossAmount", typeof(Decimal));
            dtR.Columns.Add("TaxAmount", typeof(Decimal));
            dtR.Columns.Add("NetAmount", typeof(Decimal));
            dtR.Columns.Add("PaidAmount", typeof(Decimal));
            dtR.Columns.Add("PaidGrossAmount", typeof(Decimal));
            dtR.Columns.Add("PaidTaxAmount", typeof(Decimal));
            dtR.Columns.Add("PaidNetAmount", typeof(Decimal));
            dtR.Columns.Add("HPaidNetAmount", typeof(Decimal));
            dtR.Columns.Add("SOrder", typeof(Int32));

            DataTable dtQualifier = new DataTable();
            dtQualifier.Columns.Add("FlatId", typeof(int));
            dtQualifier.Columns.Add("PaymentSchId", typeof(int));
            dtQualifier.Columns.Add("QualifierId", typeof(int));
            dtQualifier.Columns.Add("QualTypeId", typeof(int));
            dtQualifier.Columns.Add("Service", typeof(bool));
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
            dtQualifier.Columns.Add("TaxablePer", typeof(decimal));
            dtQualifier.Columns.Add("TaxableValue", typeof(decimal));

            DataTable dtTQ = new DataTable("QualifierAbs");
            dtTQ.Columns.Add("FlatId", typeof(int));
            dtTQ.Columns.Add("PaymentSchId", typeof(int));
            dtTQ.Columns.Add("QualifierId", typeof(int));
            dtTQ.Columns.Add("SchType", typeof(string));
            dtTQ.Columns.Add("ReceiptTypeId", typeof(int));
            dtTQ.Columns.Add("OtherCostId", typeof(int));
            dtTQ.Columns.Add("AccountId", typeof(int));
            dtTQ.Columns.Add("Add_Less_Flag", typeof(string));
            dtTQ.Columns.Add("Amount", typeof(decimal));

            DataTable dt = new DataTable();
            dt = ds.Tables[0];

            foreach (DataRow drow in dt.Rows)
            {
                iPaySchId = Convert.ToInt32(drow["PaymentSchId"]);
                iFlatId = Convert.ToInt32(drow["FlatId"]);
                bTypewise = Convert.ToBoolean(drow["Typewise"]);
                sSchType = drow["SchType"].ToString(); bSchAdv = false; dNetAmt = 0;
                dTNetAmt = 0;
                dTTaxAmt = 0;
                dAdvAmt = 0;
                dAdv = 0; dSchAdvance = Convert.ToDecimal(drow["Advance"]);

                if (bTypewise == true)
                {
                    sSql = "Select SortOrder,ReceiptTypeId,OtherCostId,Description,Amount,SchType,AccountId From( " +
                          " Select SortOrder=(Case When A.SchType='A' Then 1 When A.SchType='R' Then 2 When A.SchType='O' Then 3 Else 4 End),A.ReceiptTypeId,A.OtherCostId,Case When A.ReceiptTypeId <>0 then B.ReceiptTypeName Else C.OtherCostName End Description, A.Amount,A.SchType,isnull(QA.AccountId,0) AccountId from FlatReceiptType A " +
                           "Left Join dbo.ReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId " +
                           "Left Join dbo.OtherCostMaster C on A.OtherCostId=C.OtherCostId " +
                           " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp Q On Q.QualifierId=A.ReceiptTypeId " +
                           " Left Join dbo.QualifierAccount QA on QA.QualifierId=Q.QualifierId " +
                           "Where A.PaymentSchId = " + iPaySchId + " and A.FlatId= " + iFlatId + " ) A Order By SortOrder,ReceiptTypeId,OtherCostId";
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);

                    dr = cmd.ExecuteReader();
                    DataTable dtT = new DataTable();
                    dtT.Load(dr);
                    dr.Close();
                    cmd.Dispose();

                    for (int j = 0; j < dtT.Rows.Count; j++)
                    {

                        dRowT = dtR.NewRow();
                        dRTAmt = 0;

                        iReceiptId = Convert.ToInt32(dtT.Rows[j]["ReceiptTypeId"]);
                        iOthId = Convert.ToInt32(dtT.Rows[j]["OtherCostId"]);

                        DataTable dtTN;

                        if (iReceiptId == 1)
                        {
                            bSchAdv = true;

                            sSql = "Select Sum(Amount) Amount From ReceiptTrans Where ReceiptType='Advance' And FlatId= " + iFlatId;
                            cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                            dr = cmd.ExecuteReader();
                            dtTN = new DataTable();
                            dtTN.Load(dr);
                            dr.Close();
                            cmd.Dispose();
                            if (dtTN.Rows.Count > 0)
                            {
                                dRowT["PaidNetAmount"] = CommFun.IsNullCheck(dtTN.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric);
                                //dAdv = Convert.ToDecimal(dRowT["PaidNetAmount"]);
                            }
                            dtTN.Dispose();


                            sSql = "Select SUM(PaidNetAmount) PAmt from ReceiptShTrans Where ReceiptTypeId=1  And PaymentSchId = " + iPaySchId;
                            cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                            dr = cmd.ExecuteReader();
                            dtTN = new DataTable();
                            dtTN.Load(dr);
                            dr.Close();
                            cmd.Dispose();
                            if (dtTN.Rows.Count > 0)
                            {
                                dRowT["PaidNetAmount"] = Convert.ToDecimal(CommFun.IsNullCheck(dtTN.Rows[0]["PAmt"], CommFun.datatypes.vartypenumeric)) + Convert.ToDecimal(Convert.ToDecimal(dRowT["PaidNetAmount"]));
                                dRowT["PaidNetAmount"] = Convert.ToDecimal(dRowT["PaidNetAmount"]) * -1;
                                ////dAdv = Convert.ToDecimal(dRowT["PaidNetAmount"]);
                            }
                            
                            dtTN.Dispose();

                            dAdvAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[j]["Amount"], CommFun.datatypes.vartypenumeric)); dAdv = dAdvAmt;
                            dRowT["NetAmount"] = 0;
                            dRowT["GrossAmount"] = 0;
                        }
                        else
                        {

                            sSql = "Select SUM(PaidGrossAmount) PAmt,SUM(PaidNetAmount) PNAmt from ReceiptShTrans " +
                                   "Where FlatId = " + iFlatId + " and PaymentSchId = " + iPaySchId + " and ReceiptTypeId = " + iReceiptId + " and OtherCostId = " + iOthId;

                            cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                            dr = cmd.ExecuteReader();
                            dtTN = new DataTable();
                            dtTN.Load(dr);
                            dr.Close();
                            cmd.Dispose();
                            
                            if (dtTN.Rows.Count > 0)
                            {
                                dRowT["PaidAmount"] = CommFun.IsNullCheck(dtTN.Rows[0]["PAmt"], CommFun.datatypes.vartypenumeric);
                                dRowT["HPaidNetAmount"] = CommFun.IsNullCheck(dtTN.Rows[0]["PNAmt"], CommFun.datatypes.vartypenumeric);
                            }

                        }

                        dRowT["FlatId"] = iFlatId;
                        dRowT["PaymentSchId"] = iPaySchId;
                        dRowT["ReceiptTypeId"] = iReceiptId;
                        dRowT["OtherCostId"] = iOthId;
                        dRowT["Description"] = CommFun.IsNullCheck(dtT.Rows[j]["Description"], CommFun.datatypes.vartypestring).ToString();
                        dRowT["SchType"] = CommFun.IsNullCheck(dtT.Rows[j]["SchType"], CommFun.datatypes.vartypestring).ToString();
                        dRowT["AccountId"] = CommFun.IsNullCheck(dtT.Rows[j]["AccountId"], CommFun.datatypes.vartypenumeric);
                        dRowT["SOrder"] = 0;

                        //sSql = "Select QualifierId from CCReceiptQualifier Where ReceiptTypeId= " + iReceiptId + " and OtherCostId= " + iOthId + " and CostCentreId = " + argCCId;
                        //sSql = "Select QualifierId,IsNull(B.Service,0)Service From dbo.CCReceiptQualifier A " +
                        //        " Left Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId " +
                        //        " Where ReceiptTypeId= " + iReceiptId + " and A.OtherCostId= " + iOthId + " and CostCentreId = " + argCCId;
                        sSql = "Select C.QualTypeId,A.QualifierId,IsNull(B.Service,0)Service From dbo.CCReceiptQualifier A " +
                                " Left Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId " +
                                " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On C.QualifierId=A.QualifierId " +
                                " Where ReceiptTypeId= " + iReceiptId + " and A.OtherCostId= " + iOthId + " and CostCentreId = " + argCCId;
                        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                        dr = cmd.ExecuteReader();
                        DataTable dtTT = new DataTable();
                        dtTT.Load(dr);
                        dr.Close();
                        cmd.Dispose();

                        dQNetAmt = 0;
                        decimal dQBaseAmt = Convert.ToDecimal(dtT.Rows[j]["Amount"]);


                        //if (dtTT.Rows.Count == 0 && iReceiptId != 1) { dTNetAmt = dTNetAmt + dQBaseAmt; }
                        if (dtTT.Rows.Count == 0) { dTNetAmt = dTNetAmt + dQBaseAmt; }
                        for (int k = 0; k < dtTT.Rows.Count; k++)
                        {
                            iQualId = Convert.ToInt32(dtTT.Rows[k]["QualifierId"]);
                            bService = Convert.ToBoolean(dtTT.Rows[k]["Service"]);

                            RAQual = new cRateQualR();
                            QualVBC = new Collection();

                            //DataTable dtQ = new DataTable();
                            //dtQ = GetQual(iQualId, argDate);
                            DataTable dtTDS = new DataTable();
                            DataTable dtQ = new DataTable();
                            if (Convert.ToInt32(dtTT.Rows[k]["QualTypeId"]) == 2)
                            {
                                if (bService == true)
                                    dtTDS = GetSTSettings("G", argDate);
                                else dtTDS = GetSTSettings("F", argDate);
                            }
                            else { dtTDS = GetQual(iQualId, argDate); }

                            dtQ = QualifierSelect(iQualId, "B", argDate);

                            if (dtTDS.Rows.Count > 0)
                            {
                                RAQual.RateID = iQualId;
                                RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                                RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                                RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                                RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                                RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                                RAQual.HEDValue = 0;
                                RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Taxable"], CommFun.datatypes.vartypenumeric));
                            }
                            if (dtQ.Rows.Count > 0)
                            {
                                RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                                RAQual.Amount = 0;
                                RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                            }

                            QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);

                            Qualifier.frmQualifier qul = new Qualifier.frmQualifier();

                            dQNetAmt = 0;
                            dTaxAmt = 0;
                            decimal dVATAmt = 0;

                            DataRow dr1;

                            if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, argDate, ref dVATAmt) == true)
                            {
                                //dRTAmt = dRTAmt + dTaxAmt;

                                //dTNetAmt = dTNetAmt + dQNetAmt;
                                //dTTaxAmt = dTTaxAmt + dTaxAmt;
                                if (dQBaseAmt != 0)
                                {
                                    dRTAmt = dRTAmt + dTaxAmt;

                                    if (k != 0) { dTNetAmt = dTNetAmt + dTaxAmt; }
                                    else dTNetAmt = dTNetAmt + dQNetAmt;
                                    //dTNetAmt = dTNetAmt + dQNetAmt;
                                    dTTaxAmt = dTTaxAmt + dTaxAmt;
                                }
                                ////modified
                                //if (dQBaseAmt != 0)
                                //{
                                //    decimal dTax = dQNetAmt - dQBaseAmt;
                                //    dTTaxAmt = dTTaxAmt + dTax;

                                //    //if (k != 0) { dQBaseAmt = 0; }

                                //    dQNetAmt = dQBaseAmt + dTTaxAmt;
                                //    if (k != 0) { dQBaseAmt = dTNetAmt = dQNetAmt; }
                                //    else dTNetAmt = dTNetAmt + dQNetAmt;
                                //    //dTNetAmt = dTNetAmt + dQNetAmt;
                                //}

                                foreach (Qualifier.cRateQualR d in QualVBC)
                                {
                                    dr1 = dtQualifier.NewRow();

                                    dr1["FlatId"] = iFlatId;
                                    dr1["PaymentSchId"] = iPaySchId;
                                    dr1["QualifierId"] = d.RateID;
                                    dr1["QualTypeId"] = Convert.ToInt32(dtTT.Rows[k]["QualTypeId"]);
                                    dr1["Service"] = bService;
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
                                    dr1["TaxablePer"] = d.TaxablePer;
                                    dr1["TaxableValue"] = d.TaxableValue;

                                    dtQualifier.Rows.Add(dr1);
                                }

                            }
                        }
                        if (dQNetAmt == 0) { dQNetAmt = dQBaseAmt; }
                        
                        if (iReceiptId != 1)
                        {
                            dRowT["GrossAmount"] = dQBaseAmt;
                            dRowT["TaxAmount"] = dRTAmt;

                            dRowT["NetAmount"] = dQNetAmt;
                        }
                        
                        dtR.Rows.Add(dRowT);
                    }
                    
                    drow["NetAmount"] = dTNetAmt - dAdvAmt;
                    drow["BalanceAmount"] = dTNetAmt - Convert.ToDecimal(drow["PaidAmount"]) - dAdvAmt;
                    drow["HBalance"] = dTNetAmt - Convert.ToDecimal(drow["PaidAmount"]) - dAdvAmt - dSchAdvance;
                }
                else
                {
                    sSql = "Select A.ReceiptTypeId,A.OtherCostId,Case When A.SchType='R' Or A.SchType='A' then B.ReceiptTypeName " +
                            " When A.SchType='Q' Then Q.QualifierName Else C.OtherCostName End Description, A.Amount,isnull(QA.AccountId,0) AccountId,A.SchType," +
                            " Case When A.SchType='R' Or A.SchType='A' then 1  When A.SchType='Q' Then 3 Else 2 End SOrder from FlatReceiptType A " +
                            " Left Join ReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId " +
                            " Left Join OtherCostMaster C on A.OtherCostId=C.OtherCostId " +
                            " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp Q On Q.QualifierId=A.ReceiptTypeId " +
                            " Left Join dbo.QualifierAccount QA On QA.QualifierId=Q.QualifierId " +
                            " Where A.PaymentSchId = " + iPaySchId + " And A.Amount<>0 And A.FlatId= " + iFlatId + " Order by SOrder";
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);

                    dr = cmd.ExecuteReader();
                    DataTable dtT = new DataTable();
                    dtT.Load(dr);
                    dr.Close();
                    cmd.Dispose();

                    
                    for (int j = 0; j < dtT.Rows.Count; j++)
                    {

                        dRowT = dtR.NewRow();
                        dRTAmt = 0;

                        iReceiptId = Convert.ToInt32(dtT.Rows[j]["ReceiptTypeId"]);
                        iOthId = Convert.ToInt32(dtT.Rows[j]["OtherCostId"]);

                        DataTable dtTN;

                        if (iReceiptId == 1)
                        {
                            sSql = "Select Sum(Amount) Amount From ReceiptTrans Where ReceiptType='Advance' And FlatId= " + iFlatId;
                            cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                            dr = cmd.ExecuteReader();
                            dtTN = new DataTable();
                            dtTN.Load(dr);
                            dr.Close();
                            cmd.Dispose();
                            if (dtTN.Rows.Count > 0)
                            {
                                dRowT["PaidNetAmount"] = CommFun.IsNullCheck(dtTN.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric);
                            }
                            dtTN.Dispose();


                            sSql = "Select SUM(PaidNetAmount) PAmt from ReceiptShTrans where ReceiptTypeId=1  and PaymentSchId = " + iPaySchId;
                            cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                            dr = cmd.ExecuteReader();
                            dtTN = new DataTable();
                            dtTN.Load(dr);
                            dr.Close();
                            cmd.Dispose();
                            if (dtTN.Rows.Count > 0)
                            {
                                dRowT["PaidNetAmount"] = Convert.ToDecimal(CommFun.IsNullCheck(dtTN.Rows[0]["PAmt"], CommFun.datatypes.vartypenumeric)) + Convert.ToDecimal(Convert.ToDecimal(dRowT["PaidNetAmount"]));
                                dRowT["PaidNetAmount"] = Convert.ToDecimal(dRowT["PaidNetAmount"]) * -1;
                            }
                            dtTN.Dispose();

                            dAdvAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[j]["Amount"], CommFun.datatypes.vartypenumeric));
                            dRowT["NetAmount"] = 0;
                            dRowT["GrossAmount"] = 0;
                        }
                        else
                        {

                            sSql = "Select SUM(PaidGrossAmount) PAmt from ReceiptShTrans " +
                                   "Where FlatId = " + iFlatId + " and PaymentSchId = " + iPaySchId + " and ReceiptTypeId = " + iReceiptId + " and OtherCostId = " + iOthId;

                            cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                            dr = cmd.ExecuteReader();
                            dtTN = new DataTable();
                            dtTN.Load(dr);
                            dr.Close();
                            cmd.Dispose();
                            if (dtTN.Rows.Count > 0)
                            {
                                dRowT["PaidAmount"] = CommFun.IsNullCheck(dtTN.Rows[0]["PAmt"], CommFun.datatypes.vartypenumeric);
                            }
                        }

                        dRowT["FlatId"] = iFlatId;
                        dRowT["PaymentSchId"] = iPaySchId;
                        dRowT["ReceiptTypeId"] = iReceiptId;
                        dRowT["OtherCostId"] = iOthId;
                        dRowT["Description"] = CommFun.IsNullCheck(dtT.Rows[j]["Description"], CommFun.datatypes.vartypestring).ToString();
                        dRowT["SchType"] = CommFun.IsNullCheck(dtT.Rows[j]["SchType"], CommFun.datatypes.vartypestring).ToString();
                        dRowT["AccountId"] = CommFun.IsNullCheck(dtT.Rows[j]["AccountId"], CommFun.datatypes.vartypenumeric);

                        #region code
                        //sSql = "Select QualifierId from CCReceiptQualifier Where ReceiptTypeId= " + iReceiptId + " and OtherCostId= " + iOthId + " and CostCentreId = " + argCCId;
                        //cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                        //dr = cmd.ExecuteReader();
                        //DataTable dtTT = new DataTable();
                        //dtTT.Load(dr);
                        //dr.Close();
                        //cmd.Dispose();

                        dQNetAmt = 0;
                        decimal dQBaseAmt = Convert.ToDecimal(dtT.Rows[j]["Amount"]);

                        //if (dtTT.Rows.Count == 0)
                        { dTNetAmt = dTNetAmt + dQBaseAmt; }

                        //for (int k = 0; k < dtTT.Rows.Count; k++)
                        //{
                        //    iQualId = Convert.ToInt32(dtTT.Rows[k]["QualifierId"]);

                        //    RAQual = new cRateQualR();
                        //    QualVBC = new Collection();

                        //    DataTable dtQ = new DataTable();
                        //    dtQ = GetQual(iQualId, argDate);
                        //    if (dtQ.Rows.Count > 0)
                        //    {

                        //        RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                        //        RAQual.Amount = 0;
                        //        RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                        //        RAQual.RateID = iQualId;
                        //        RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                        //        RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                        //        RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                        //        RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                        //        RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                        //        RAQual.HEDValue = 0;
                        //    }

                        //    QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);

                        //    Qualifier.frmQualifier qul = new Qualifier.frmQualifier();

                        //    dQNetAmt = 0;
                        //    dTaxAmt = 0;

                        //    DataRow dr1;

                        //    if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, argDate) == true)
                        //    {
                        //        dRTAmt = dRTAmt + dTaxAmt;

                        dTNetAmt = dTNetAmt + dQNetAmt;
                        //        dTTaxAmt = dTTaxAmt + dTaxAmt;

                        //        foreach (Qualifier.cRateQualR d in QualVBC)
                        //        {
                        //            dr1 = dtQualifier.NewRow();

                        //            dr1["FlatId"] = iFlatId;
                        //            dr1["PaymentSchId"] = iPaySchId;
                        //            dr1["QualifierId"] = d.RateID;
                        //            dr1["Expression"] = d.Expression;
                        //            dr1["ExpPer"] = d.ExpPer;
                        //            dr1["NetPer"] = d.NetPer;
                        //            dr1["Add_Less_Flag"] = d.Add_Less_Flag;
                        //            dr1["SurCharge"] = d.SurPer;
                        //            dr1["EDCess"] = d.EDPer;
                        //            dr1["HEDPer"] = d.HEDPer;
                        //            dr1["ExpValue"] = d.ExpValue;
                        //            dr1["ExpPerValue"] = d.ExpPerValue;
                        //            dr1["SurValue"] = d.SurValue;
                        //            dr1["EDValue"] = d.EDValue;
                        //            dr1["Amount"] = d.Amount;
                        //            dr1["ReceiptTypeId"] = iReceiptId;
                        //            dr1["OtherCostId"] = iOthId;

                        //            dtQualifier.Rows.Add(dr1);
                        //        }

                        //    }
                        //}
                        if (dQNetAmt == 0) { dQNetAmt = dQBaseAmt; }
                        #endregion

                        if (iReceiptId != 1)
                        {
                            dRowT["GrossAmount"] = dQBaseAmt;
                            dRowT["TaxAmount"] = dRTAmt;

                            dRowT["NetAmount"] = dQNetAmt;
                        }
                        dtR.Rows.Add(dRowT);

                    }

                    DataRow dr1; decimal dAmt = 0;
                    DataTable dtQ = new DataTable();
                    DataView dv = new DataView(dtT);
                    dv.RowFilter = "SchType='Q'";
                    dtQ = dv.ToTable();
                    for (int x = 0; x < dtQ.Rows.Count; x++)
                    {
                        dAmt = dAmt + Convert.ToDecimal(dtQ.Rows[x]["Amount"]);
                    }
                    if (dtQ.Rows.Count > 0)
                    {
                        dr1 = dtTQ.NewRow();
                        dr1["FlatId"] = iFlatId;
                        dr1["PaymentSchId"] = iPaySchId;
                        dr1["QualifierId"] = iReceiptId;
                        dr1["SchType"] = "Q";
                        dr1["ReceiptTypeId"] = iReceiptId;
                        dr1["OtherCostId"] = 0;
                        dr1["AccountId"] = Convert.ToInt32(dtQ.Rows[0]["AccountId"]);
                        dr1["Add_Less_Flag"] = "+";
                        dr1["Amount"] = dAmt;

                        dtTQ.Rows.Add(dr1);
                    }

                    drow["NetAmount"] = dTNetAmt - dAdvAmt;
                    drow["BalanceAmount"] = dTNetAmt - Convert.ToDecimal(drow["PaidAmount"]) - dAdvAmt;

                    
                }
            }

            ds.Tables.Add(dtR);
            ds.Tables.Add(dtQualifier);

            if (bTypewise == true)
            {
                sSql = "Select B.FlatId,B.PaymentSchId,A.QualifierId,B.SchType,B.ReceiptTypeId,B.OtherCostId,isnull(C.AccountId,0) AccountId, A.Add_Less_Flag,Sum(A.Amount) Amount " +
                        "From dbo.FlatReceiptQualifier A " +
                        "Inner Join dbo.FlatReceiptType B on A.SchId=b.SchId " +
                        "Left Join dbo.QualifierAccount C on A.QualifierId=C.QualifierId " +
                        "Where A.Amount <>0 and B.PaymentSchId In (Select A.PaymentSchId From PaymentScheduleFlat A  Inner Join FlatDetails B On A.FlatId=B.FlatId " +
                        " Inner Join PaySchType C On C.TypeId=B.PayTypeId Where LeadId=" + argBuyerId + " And A.FlatId=" + argFlatId + " And TemplateId<>0 " +
                        " And (A.NetAmount-A.SurplusAmount)-A.PaidAmount>0 And A.BillPassed=0) " +
                        " Group by B.FlatId,B.PaymentSchId,A.QualifierId,B.SchType,B.ReceiptTypeId,B.OtherCostId,C.AccountId,A.Add_Less_Flag";

                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "QualifierAbs");
                sda.Dispose();

                for (int n = 0; n < ds.Tables["QualifierAbs"].Rows.Count; n++)
                {
                    decimal dAmt = 0;
                    DataTable dtAbs = new DataTable();
                    dtAbs = dtQualifier;
                    DataView dv = new DataView(dtAbs);
                    dv.RowFilter = "FlatId=" + ds.Tables["QualifierAbs"].Rows[n]["FlatId"] + " And PaymentSchId=" + ds.Tables["QualifierAbs"].Rows[n]["PaymentSchId"] + " And QualifierId=" + ds.Tables["QualifierAbs"].Rows[n]["QualifierId"] + " And ReceiptTypeId=" + ds.Tables["QualifierAbs"].Rows[n]["ReceiptTypeId"] + " And OtherCostId=" + ds.Tables["QualifierAbs"].Rows[n]["OtherCostId"] + "";
                    dtAbs = dv.ToTable();
                    for (int i = 0; i < dtAbs.Rows.Count; i++)
                    {
                        dAmt = dAmt + Convert.ToDecimal(dtAbs.Rows[i]["Amount"]);
                    }

                    ds.Tables["QualifierAbs"].Rows[n]["Amount"] = dAmt;
                }
            }
            else
            {
                ds.Tables.Add(dtTQ);
                //DataRow dr1; decimal dAmt = 0;
                //for (int x = 0; x < dtQ.Rows.Count; x++)
                //{
                //    dAmt = dAmt + Convert.ToDecimal(dtQ.Rows[x]["Amount"]);
                //}
                //dr1 = dtTQ.NewRow();
                //dr1["FlatId"] = iFlatId;
                //dr1["PaymentSchId"] = iPayId;
                //dr1["QualifierId"] = Convert.ToInt32(dtQ.Rows[0]["QualifierId"]);
                //dr1["SchType"] = "Q";
                //dr1["ReceiptTypeId"] = Convert.ToInt32(dtQ.Rows[0]["QualifierId"]);
                //dr1["OtherCostId"] = 0;
                //dr1["AccountId"] = Convert.ToInt32(dtQ.Rows[0]["AccountId"]);
                //dr1["Add_Less_Flag"] = "+";
                //dr1["Amount"] = dAmt;

                //dtTQ.Rows.Add(dr1);
            }

            return ds;
        }

        public static DataSet GetPBFlatReceiptNew(int argCCId, int argBuyerId, DateTime argDate, int argFlatId, bool argType)
        {
            BsfGlobal.OpenCRMDB();
            DataSet ds = null;

            try
            {
                string sSql = "Update FlatReceiptType Set Amount=NetAmount Where Amount=0 And NetAmount>0";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = " Select [SchDate/BillDate],CostCentreId,FlatId,FlatNo,ReceiptType,BillRegId,PaymentSchId,Typewise,SchType,BillNo,[Description], " +
                        " Gross,NetAmount,PaidAmount,BalanceAmount,Amount,HBalance,HAmount,Advance,RAmt,SortOrder from( " +
                        " Select A.PBDate [SchDate/BillDate],A.CostCentreId,A.FlatId,B.FlatNo,'ProgressBill' ReceiptType,A.PBillId BillRegId,A.PaySchId PaymentSchId," +
                        " PT.Typewise,PF.SchType,   A.PBNo BillNo,PF.Description,A.BillAmount Gross,A.NetAmount,PF.PaidAmount,A.NetAmount-PF.PaidAmount BalanceAmount, " +
                        " Cast(0 as Decimal(18,3)) Amount,A.NetAmount-PF.PaidAmount HBalance,0 HAmount,Isnull(PF.Advance,0)Advance,0 RAmt,PF.SortOrder From ProgressBillRegister A  " +
                        " Inner Join ProgressBillMaster M On A.ProgRegId=M.ProgRegId   Inner Join FlatDetails B On A.FlatId=B.FlatId " +
                        " Inner Join dbo.PaymentScheduleFlat PF On PF.PaymentSchId=A.PaySchId Inner Join PaySchType PT On PT.TypeId=B.PayTypeId " +
                        " Where A.LeadId=" + argBuyerId + " And A.FlatId=" + argFlatId + "  And (A.NetAmount)-PF.PaidAmount>0 And M.Approve='Y'  " +
                        " UNION ALL  " +
                        " Select PF.SchDate [SchDate/BillDate],B.CostCentreId,PF.FlatId,B.FlatNo,'ScheduleBill' ReceiptType,0 BillRegId,PF.PaymentSchId,  C.Typewise,PF.SchType," +
                        " '' BillNo,PF.Description,PF.Amount Gross,PF.NetAmount,PF.PaidAmount,PF.NetAmount-PF.PaidAmount BalanceAmount, Cast(0 as Decimal(18,3)) Amount," +
                        " PF.NetAmount-PF.PaidAmount HBalance,0 HAmount,IsNull(PF.Advance,0)Advance,0 RAmt,PF.SortOrder  From PaymentScheduleFlat PF " +
                        " Inner Join FlatDetails B On PF.FlatId=B.FlatId  Inner Join PaySchType C On C.TypeId=B.PayTypeId " +
                        " Where LeadId=" + argBuyerId + " And PF.FlatId=" + argFlatId + " " +
                        " And TemplateId<>0  And (PF.NetAmount)-PF.PaidAmount>0 And PF.BillPassed=0) X" +
                        " Order By SortOrder";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                ds = new DataSet();
                sda.Fill(ds, "PaymentSchedule");
                sda.Dispose();

                DataTable dtR = new DataTable();
                dtR.Columns.Add("FlatId", typeof(Int32));
                dtR.Columns.Add("PaymentSchId", typeof(Int32));
                dtR.Columns.Add("ReceiptTypeId", typeof(Int32));
                dtR.Columns.Add("OtherCostId", typeof(Int32));
                dtR.Columns.Add("Description", typeof(string));
                dtR.Columns.Add("SchType", typeof(string));
                dtR.Columns.Add("AccountId", typeof(Int32));
                dtR.Columns.Add("GrossAmount", typeof(Decimal));
                dtR.Columns.Add("TaxAmount", typeof(Decimal));
                dtR.Columns.Add("NetAmount", typeof(Decimal));
                dtR.Columns.Add("PaidAmount", typeof(Decimal));
                dtR.Columns.Add("PaidGrossAmount", typeof(Decimal));
                dtR.Columns.Add("PaidTaxAmount", typeof(Decimal));
                dtR.Columns.Add("PaidNetAmount", typeof(Decimal));
                dtR.Columns.Add("HPaidNetAmount", typeof(Decimal));
                dtR.Columns.Add("SOrder", typeof(Int32));

                DataTable dtQualifier = new DataTable();
                dtQualifier.Columns.Add("FlatId", typeof(int));
                dtQualifier.Columns.Add("PaymentSchId", typeof(int));
                dtQualifier.Columns.Add("QualifierId", typeof(int));
                dtQualifier.Columns.Add("QualTypeId", typeof(int));
                dtQualifier.Columns.Add("Service", typeof(bool));
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
                dtQualifier.Columns.Add("TaxablePer", typeof(decimal));
                dtQualifier.Columns.Add("TaxableValue", typeof(decimal));

                DataTable dtTQ = new DataTable("QualifierAbs");
                dtTQ.Columns.Add("FlatId", typeof(int));
                dtTQ.Columns.Add("PaymentSchId", typeof(int));
                dtTQ.Columns.Add("QualifierId", typeof(int));
                dtTQ.Columns.Add("SchType", typeof(string));
                dtTQ.Columns.Add("ReceiptTypeId", typeof(int));
                dtTQ.Columns.Add("OtherCostId", typeof(int));
                dtTQ.Columns.Add("AccountId", typeof(int));
                dtTQ.Columns.Add("Add_Less_Flag", typeof(string));
                dtTQ.Columns.Add("Amount", typeof(decimal));

                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                bool bTypewise = false;
                foreach (DataRow drow in dt.Rows)
                {
                    int iPaySchId = Convert.ToInt32(drow["PaymentSchId"]);
                    int iFlatId = Convert.ToInt32(drow["FlatId"]);
                    bTypewise = Convert.ToBoolean(drow["Typewise"]);
                    string sSchType = drow["SchType"].ToString();
                    decimal dTNetAmt = 0;
                    int iPBillId = Convert.ToInt32(drow["BillRegId"]);
                    decimal dTTaxAmt = 0;
                    decimal dAdvAmt = 0;
                    decimal dAdv = 0;
                    decimal dSchAdvance = Convert.ToDecimal(drow["Advance"]);

                    if (bTypewise == true && sSchType != "R")
                    {
                        sSql = "select y.SortOrder, x.ReceiptTypeId, x.OtherCostId, x.Description, x.Amount, x.SchType, x.AccountId from ( " +
                               "Select distinct A.ReceiptTypeId,A.OtherCostId, " +
                               " Case When A.ReceiptTypeId<>0 then B.ReceiptTypeName Else C.OtherCostName End Description, " +
                               " A.Amount,A.SchType,isnull(QA.AccountId,0) AccountId from FlatReceiptType A " +
                               " left Join ReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId " +                               
                               " Left Join OtherCostMaster C on A.OtherCostId=C.OtherCostId " +
                               " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp Q On Q.QualifierId=A.ReceiptTypeId " +
                               " Left Join dbo.QualifierAccount QA on QA.QualifierId=Q.QualifierId " +
                               " Where A.PaymentSchId = " + iPaySchId + " and A.FlatId= " + iFlatId + " ) x" +
                               " left join receipttypeorder y on x.receipttypeid=y.receipttypeid and x.othercostid=y.othercostid" +
                               " where y.costcentreid=" + argCCId + " and y.paytypeid in(select paytypeid from flatdetails where flatid=" + iFlatId + ") "+
                               " order by y.sortorder";
                        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);                        
                        SqlDataReader dreader = cmd.ExecuteReader();
                        DataTable dtT = new DataTable();
                        dtT.Load(dreader);
                        dreader.Close();
                        cmd.Dispose();

                        for (int j = 0; j < dtT.Rows.Count; j++)
                        {
                            DataRow dRowT = dtR.NewRow();
                            decimal dRTAmt = 0;

                            int iReceiptId = Convert.ToInt32(dtT.Rows[j]["ReceiptTypeId"]);
                            int iOthId = Convert.ToInt32(dtT.Rows[j]["OtherCostId"]);

                            if (iReceiptId == 1)
                            {
                                sSql = "Select Sum(Amount) Amount From ReceiptTrans Where ReceiptType='Advance' And FlatId= " + iFlatId;
                                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                                dreader = cmd.ExecuteReader();
                                DataTable dtTN = new DataTable();
                                dtTN.Load(dreader);
                                dreader.Close();
                                cmd.Dispose();

                                if (dtTN.Rows.Count > 0)
                                {
                                    dRowT["PaidNetAmount"] = CommFun.IsNullCheck(dtTN.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric);
                                    //dAdv = Convert.ToDecimal(dRowT["PaidNetAmount"]);
                                }
                                dtTN.Dispose();

                                sSql = "Select SUM(PaidNetAmount) PAmt from ReceiptShTrans where ReceiptTypeId=1  and PaymentSchId = " + iPaySchId;
                                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                                dreader = cmd.ExecuteReader();
                                dtTN = new DataTable();
                                dtTN.Load(dreader);
                                dreader.Close();
                                cmd.Dispose();

                                if (dtTN.Rows.Count > 0)
                                {
                                    dRowT["PaidNetAmount"] = Convert.ToDecimal(CommFun.IsNullCheck(dtTN.Rows[0]["PAmt"], CommFun.datatypes.vartypenumeric)) + Convert.ToDecimal(Convert.ToDecimal(dRowT["PaidNetAmount"]));
                                    dRowT["PaidNetAmount"] = Convert.ToDecimal(dRowT["PaidNetAmount"]) * -1;
                                    ////dAdv = Convert.ToDecimal(dRowT["PaidNetAmount"]);
                                }

                                dtTN.Dispose();

                                dAdvAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[j]["Amount"], CommFun.datatypes.vartypenumeric)); 
                                dAdv = dAdvAmt;
                                dRowT["NetAmount"] = 0;
                                dRowT["GrossAmount"] = 0;
                                //dRowT["PaidNetAmount"] = dAdvAmt * -1;
                            }
                            else
                            {

                                sSql = "Select SUM(PaidGrossAmount) PAmt,SUM(PaidNetAmount) PNAmt from ReceiptShTrans " +
                                       "Where FlatId = " + iFlatId + " and PaymentSchId = " + iPaySchId + " and ReceiptTypeId = " + iReceiptId + " and OtherCostId = " + iOthId;

                                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                                dreader = cmd.ExecuteReader();
                                DataTable dtTN = new DataTable();
                                dtTN.Load(dreader);
                                dreader.Close();
                                cmd.Dispose();

                                if (dtTN.Rows.Count > 0)
                                {
                                    dRowT["PaidAmount"] = CommFun.IsNullCheck(dtTN.Rows[0]["PAmt"], CommFun.datatypes.vartypenumeric);
                                    dRowT["HPaidNetAmount"] = CommFun.IsNullCheck(dtTN.Rows[0]["PNAmt"], CommFun.datatypes.vartypenumeric);
                                }
                            }

                            dRowT["FlatId"] = iFlatId;
                            dRowT["PaymentSchId"] = iPaySchId;
                            dRowT["ReceiptTypeId"] = iReceiptId;
                            dRowT["OtherCostId"] = iOthId;
                            dRowT["Description"] = CommFun.IsNullCheck(dtT.Rows[j]["Description"], CommFun.datatypes.vartypestring).ToString();
                            dRowT["SchType"] = CommFun.IsNullCheck(dtT.Rows[j]["SchType"], CommFun.datatypes.vartypestring).ToString();
                            dRowT["AccountId"] = CommFun.IsNullCheck(dtT.Rows[j]["AccountId"], CommFun.datatypes.vartypenumeric);
                            dRowT["SOrder"] = 0;

                            DataTable dtTT = new DataTable();
                            if (argType == false)
                            {
                                sSql = "Select C.QualTypeId,A.QualifierId,IsNull(B.Service,0)Service From dbo.CCReceiptQualifier A " +
                                        " Left Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId " +
                                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On C.QualifierId=A.QualifierId " +
                                        " Where ReceiptTypeId= " + iReceiptId + " and A.OtherCostId= " + iOthId + " and CostCentreId = " + argCCId;
                                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                                dreader = cmd.ExecuteReader();
                                dtTT = new DataTable();
                                dtTT.Load(dreader);
                                dreader.Close();
                                cmd.Dispose();
                            }
                            else
                            {
                                if (iPBillId == 0)
                                {
                                    sSql = "Select B.FlatId,B.PaymentSchId,A.QualifierId,A.Expression,A.ExpPer,A.NetPer Net,A.Add_Less_Flag,A.SurCharge, " +
                                              " A.EDCess,A.HEDPer HEDCess,A.ExpValue,A.ExpPerValue,A.SurValue,A.EDValue,A.TaxablePer Taxable,A.Amount,B.SchType,B.ReceiptTypeId,B.OtherCostId,IsNull(O.Service,0)Service,Q.QualTypeId " +
                                              " From dbo.FlatReceiptQualifier A Inner Join dbo.FlatReceiptType B on A.SchId=B.SchId " +
                                              " Left Join dbo.OtherCostMaster O On O.OtherCostId=B.OtherCostId Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp Q On Q.QualifierId=A.QualifierId " +
                                              " Where B.PaymentSchId=" + iPaySchId + " And B.FlatId=" + iFlatId + " And B.ReceiptTypeId= " + iReceiptId + " and B.OtherCostId= " + iOthId + " ";
                                }
                                else
                                {
                                    sSql = "Select B.FlatId,B.PaymentSchId,A.QualifierId,A.Expression,A.ExpPer,A.NetPer Net,A.Add_Less_Flag,A.SurCharge, " +
                                            " A.EDCess,A.HEDPer HEDCess,A.ExpValue,A.ExpPerValue,A.SurValue,A.EDValue,A.TaxablePer Taxable,A.Amount,B.SchType,B.ReceiptTypeId,B.OtherCostId,IsNull(O.Service,0)Service,Q.QualTypeId " +
                                            " From dbo.PBReceiptTypeQualifier A Inner Join dbo.PBReceiptType B on A.PBId=B.PBId " +
                                            " Left Join dbo.OtherCostMaster O On O.OtherCostId=B.OtherCostId Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp Q On Q.QualifierId=A.QualifierId " +
                                            " Where B.PaymentSchId=" + iPaySchId + " And B.FlatId=" + iFlatId + " And B.ReceiptTypeId= " + iReceiptId + " and B.OtherCostId= " + iOthId + " ";
                                }
                                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                                dreader = cmd.ExecuteReader();
                                dtTT = new DataTable();
                                dtTT.Load(dreader);
                                dreader.Close();
                                cmd.Dispose();
                            }

                            decimal dQNetAmt = 0;
                            decimal dQBaseAmt = 0;
                            if (dtT.Rows[j]["SchType"].ToString() != "A") { dQBaseAmt = Convert.ToDecimal(dtT.Rows[j]["Amount"]); }

                            //if (dtTT.Rows.Count == 0 && iReceiptId != 1) { dTNetAmt = dTNetAmt + dQBaseAmt; }
                            if (dtTT.Rows.Count == 0) { dTNetAmt = dTNetAmt + dQBaseAmt; }

                            cRateQualR RAQual = new cRateQualR();
                            Collection QualVBC = new Collection();
                            for (int k = 0; k < dtTT.Rows.Count; k++)
                            {
                                int iQualId = Convert.ToInt32(dtTT.Rows[k]["QualifierId"]);
                                bool bService = Convert.ToBoolean(dtTT.Rows[k]["Service"]);

                                RAQual = new cRateQualR();
                                QualVBC = new Collection();

                                DataTable dtTDS = new DataTable();
                                if (argType == false || iPBillId == 0)
                                {
                                    if (Convert.ToInt32(dtTT.Rows[k]["QualTypeId"]) == 2)
                                    {
                                        if (bService == true)
                                            dtTDS = GetSTSettings("G", argDate, BsfGlobal.g_CRMDB);
                                        else
                                            dtTDS = GetSTSettings("F", argDate, BsfGlobal.g_CRMDB);
                                    }
                                    else
                                    {
                                        dtTDS = GetQual(iQualId, argDate, BsfGlobal.g_CRMDB);
                                    }
                                }
                                else { dtTDS = dtTT; }

                                if (dtTDS.Rows.Count > 0)
                                {
                                    RAQual.RateID = iQualId;
                                    RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                                    RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                                    RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                                    RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                                    RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                                    RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Taxable"], CommFun.datatypes.vartypenumeric));
                                }

                                DataTable dtQ = new DataTable();
                                dtQ = QualifierSelect(iQualId, "B", argDate, BsfGlobal.g_CRMDB);
                                if (dtQ.Rows.Count > 0)
                                {
                                    RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                                    RAQual.Amount = 0;
                                    RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                                }
                                else
                                {
                                    dtQ = GetQual(iQualId, argDate, BsfGlobal.g_CRMDB);
                                    if (dtQ.Rows.Count > 0)
                                    {
                                        RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                                        RAQual.Amount = 0;
                                        RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                                    }
                                }

                                QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);

                                dQNetAmt = 0;
                                decimal dTaxAmt = 0;
                                decimal dVATAmt = 0;

                                Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                                if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, argDate, ref dVATAmt) == true)
                                {
                                    if (dQBaseAmt != 0)
                                    {
                                        dTaxAmt = dQNetAmt - dQBaseAmt;
                                        dRTAmt = dRTAmt + dTaxAmt;

                                        if (k != 0)
                                            dTNetAmt = dTNetAmt + dTaxAmt;
                                        else
                                            dTNetAmt = dTNetAmt + dQNetAmt;

                                        dTTaxAmt = dTTaxAmt + dTaxAmt;
                                    }

                                    foreach (Qualifier.cRateQualR d in QualVBC)
                                    {
                                        DataRow dr1 = dtQualifier.NewRow();

                                        dr1["FlatId"] = iFlatId;
                                        dr1["PaymentSchId"] = iPaySchId;
                                        dr1["QualifierId"] = d.RateID;
                                        dr1["QualTypeId"] = Convert.ToInt32(dtTT.Rows[k]["QualTypeId"]);
                                        dr1["Service"] = bService;
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
                                        dr1["TaxablePer"] = d.TaxablePer;
                                        dr1["TaxableValue"] = d.TaxableValue;

                                        dtQualifier.Rows.Add(dr1);
                                    }
                                }
                            }

                            if (dQNetAmt == 0) { dQNetAmt = dQBaseAmt; }

                            if (iReceiptId != 1)
                            {
                                dRowT["GrossAmount"] = dQBaseAmt;
                                dRowT["TaxAmount"] = dRTAmt;

                                dRowT["NetAmount"] = dQNetAmt;
                            }

                            dtR.Rows.Add(dRowT);
                        }

                        drow["NetAmount"] = dTNetAmt - dAdvAmt;
                        drow["BalanceAmount"] = dTNetAmt - Convert.ToDecimal(drow["PaidAmount"]) - dAdvAmt;
                        drow["HBalance"] = dTNetAmt - Convert.ToDecimal(drow["PaidAmount"]) - dAdvAmt;
                    }
                    else if(sSchType != "R")
                    {
                        sSql = "Select A.ReceiptTypeId,A.OtherCostId,Case When A.SchType='R' Or A.SchType='A' then B.ReceiptTypeName " +
                                " When A.SchType='Q' Then Q.QualifierName Else C.OtherCostName End Description, A.Amount,isnull(QA.AccountId,0) AccountId,A.SchType," +
                                " Case When A.SchType='R' Or A.SchType='A' then 1  When A.SchType='Q' Then 3 Else 2 End SOrder from dbo.FlatReceiptType A " +
                                " Left Join dbo.ReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId " +
                                " Left Join dbo.OtherCostMaster C on A.OtherCostId=C.OtherCostId " +
                                " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp Q on Q.QualifierId=A.ReceiptTypeId " +
                                " Left Join dbo.QualifierAccount QA on QA.QualifierId=Q.QualifierId " +
                                " Where A.PaymentSchId = " + iPaySchId + " And A.Amount<>0 And A.FlatId= " + iFlatId +
                                " Order by Sorder";
                        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                        SqlDataReader dreader = cmd.ExecuteReader();
                        DataTable dtT = new DataTable();
                        dtT.Load(dreader);
                        dreader.Close();
                        cmd.Dispose();

                        int iReceiptId = 0;
                        for (int j = 0; j < dtT.Rows.Count; j++)
                        {
                            DataRow dRowT = dtR.NewRow();
                            decimal dRTAmt = 0;

                            iReceiptId = Convert.ToInt32(dtT.Rows[j]["ReceiptTypeId"]);
                            int iOthId = Convert.ToInt32(dtT.Rows[j]["OtherCostId"]);

                            if (iReceiptId == 1)
                            {
                                sSql = "Select Sum(Amount) Amount From dbo.ReceiptTrans Where ReceiptType='Advance' And FlatId= " + iFlatId;
                                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                                dreader = cmd.ExecuteReader();
                                DataTable dtTN = new DataTable();
                                dtTN.Load(dreader);
                                dreader.Close();
                                cmd.Dispose();

                                if (dtTN.Rows.Count > 0)
                                {
                                    dRowT["PaidNetAmount"] = CommFun.IsNullCheck(dtTN.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric);
                                }
                                dtTN.Dispose();

                                sSql = "Select SUM(PaidNetAmount) PAmt from dbo.ReceiptShTrans where ReceiptTypeId=1 AND PaymentSchId = " + iPaySchId;
                                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                                dreader = cmd.ExecuteReader();
                                dtTN = new DataTable();
                                dtTN.Load(dreader);
                                dreader.Close();
                                cmd.Dispose();

                                if (dtTN.Rows.Count > 0)
                                {
                                    dRowT["PaidNetAmount"] = Convert.ToDecimal(CommFun.IsNullCheck(dtTN.Rows[0]["PAmt"], CommFun.datatypes.vartypenumeric))
                                                            + Convert.ToDecimal(CommFun.IsNullCheck(Convert.ToDecimal(dRowT["PaidNetAmount"]), CommFun.datatypes.vartypenumeric));
                                    dRowT["PaidNetAmount"] = Convert.ToDecimal(dRowT["PaidNetAmount"]) * -1;
                                }
                                dtTN.Dispose();

                                dAdvAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[j]["Amount"], CommFun.datatypes.vartypenumeric));
                                dRowT["NetAmount"] = 0;
                                dRowT["GrossAmount"] = 0;
                            }
                            else
                            {

                                sSql = "Select SUM(PaidGrossAmount) PAmt from dbo.ReceiptShTrans " +
                                       "Where FlatId = " + iFlatId + " and PaymentSchId = " + iPaySchId + " and ReceiptTypeId = " + iReceiptId + " and OtherCostId = " + iOthId;
                                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                                dreader = cmd.ExecuteReader();
                                DataTable dtTN = new DataTable();
                                dtTN.Load(dreader);
                                dreader.Close();
                                cmd.Dispose();

                                if (dtTN.Rows.Count > 0)
                                {
                                    dRowT["PaidAmount"] = CommFun.IsNullCheck(dtTN.Rows[0]["PAmt"], CommFun.datatypes.vartypenumeric);
                                }
                            }

                            dRowT["FlatId"] = iFlatId;
                            dRowT["PaymentSchId"] = iPaySchId;
                            dRowT["ReceiptTypeId"] = iReceiptId;
                            dRowT["OtherCostId"] = iOthId;
                            dRowT["Description"] = CommFun.IsNullCheck(dtT.Rows[j]["Description"], CommFun.datatypes.vartypestring).ToString();
                            dRowT["SchType"] = CommFun.IsNullCheck(dtT.Rows[j]["SchType"], CommFun.datatypes.vartypestring).ToString();
                            dRowT["AccountId"] = CommFun.IsNullCheck(dtT.Rows[j]["AccountId"], CommFun.datatypes.vartypenumeric);

                            #region code

                            decimal dQNetAmt = 0;
                            decimal dQBaseAmt = Convert.ToDecimal(dtT.Rows[j]["Amount"]);

                            dTNetAmt = dTNetAmt + dQBaseAmt;
                            dTNetAmt = dTNetAmt + dQNetAmt;
                            if (dQNetAmt == 0) { dQNetAmt = dQBaseAmt; }

                            #endregion

                            if (iReceiptId != 1)
                            {
                                dRowT["GrossAmount"] = dQBaseAmt;
                                dRowT["TaxAmount"] = dRTAmt;
                                dRowT["NetAmount"] = dQNetAmt;
                            }

                            dtR.Rows.Add(dRowT);
                        }

                        DataView dv = new DataView(dtT) { RowFilter = "SchType='Q'" };
                        DataTable dtQ = new DataTable();
                        dtQ = dv.ToTable();

                        decimal dAmt = 0;
                        for (int x = 0; x < dtQ.Rows.Count; x++)
                        {
                            dAmt = dAmt + Convert.ToDecimal(dtQ.Rows[x]["Amount"]);
                        }

                        if (dtQ.Rows.Count > 0)
                        {
                            DataRow dr1 = dtTQ.NewRow();
                            dr1["FlatId"] = iFlatId;
                            dr1["PaymentSchId"] = iPaySchId;
                            dr1["QualifierId"] = iReceiptId;
                            dr1["SchType"] = "Q";
                            dr1["ReceiptTypeId"] = iReceiptId;
                            dr1["OtherCostId"] = 0;
                            dr1["AccountId"] = Convert.ToInt32(dtQ.Rows[0]["AccountId"]);
                            dr1["Add_Less_Flag"] = "+";
                            dr1["Amount"] = dAmt;

                            dtTQ.Rows.Add(dr1);
                        }

                        drow["NetAmount"] = dTNetAmt - dAdvAmt;
                        drow["BalanceAmount"] = (dTNetAmt) - Convert.ToDecimal(drow["PaidAmount"]) - dAdvAmt;
                    }
                }

                ds.Tables.Add(dtR);
                ds.Tables.Add(dtQualifier);

                if (bTypewise == true)
                {
                    sSql = "Select B.FlatId,B.PaymentSchId,A.QualifierId,B.SchType,B.ReceiptTypeId,B.OtherCostId,isnull(C.AccountId,0) AccountId, A.Add_Less_Flag,Sum(A.Amount) Amount " +
                            " From dbo.PBReceiptTypeQualifier A " +
                            " Inner Join dbo.PBReceiptType B on A.PBId=B.PBId " +
                            " Left Join dbo.QualifierAccount C on A.QualifierId=C.QualifierId " +
                            " Where A.Amount <>0 and B.PaymentSchId In (Select A.PaymentSchId From dbo.PaymentScheduleFlat A  Inner Join dbo.FlatDetails B On A.FlatId=B.FlatId " +
                            " Inner Join dbo.PaySchType C On C.TypeId=B.PayTypeId Where LeadId=" + argBuyerId + " And A.FlatId=" + argFlatId + " And TemplateId<>0 " +
                            " And (A.NetAmount-A.SurplusAmount)-A.PaidAmount>0 And A.BillPassed=1) " +
                            " Group by B.FlatId,B.PaymentSchId,A.QualifierId,B.SchType,B.ReceiptTypeId,B.OtherCostId,C.AccountId,A.Add_Less_Flag" +
                            " Union All " +
                            " Select B.FlatId,B.PaymentSchId,A.QualifierId,B.SchType,B.ReceiptTypeId,B.OtherCostId,isnull(C.AccountId,0) AccountId, A.Add_Less_Flag,Sum(A.Amount) Amount " +
                            " From dbo.FlatReceiptQualifier A " +
                            " Inner Join dbo.FlatReceiptType B on A.SchId=B.SchId " +
                            " Left Join dbo.QualifierAccount C on A.QualifierId=C.QualifierId " +
                            " Where A.Amount <>0 and B.PaymentSchId In (Select A.PaymentSchId From dbo.PaymentScheduleFlat A  Inner Join dbo.FlatDetails B On A.FlatId=B.FlatId " +
                            " Inner Join dbo.PaySchType C On C.TypeId=B.PayTypeId Where LeadId=" + argBuyerId + " And A.FlatId=" + argFlatId + " And TemplateId<>0 " +
                            " And (A.NetAmount-A.SurplusAmount)-A.PaidAmount>0 And A.BillPassed=0) " +
                            " Group by B.FlatId,B.PaymentSchId,A.QualifierId,B.SchType,B.ReceiptTypeId,B.OtherCostId,C.AccountId,A.Add_Less_Flag";
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    sda.Fill(ds, "QualifierAbs");
                    sda.Dispose();

                    for (int n = 0; n < ds.Tables["QualifierAbs"].Rows.Count; n++)
                    {
                        decimal dAmt = 0;
                        DataTable dtAbs = new DataTable();
                        dtAbs = dtQualifier;
                        DataView dv = new DataView(dtAbs);
                        dv.RowFilter = "FlatId=" + ds.Tables["QualifierAbs"].Rows[n]["FlatId"] + " And PaymentSchId=" + ds.Tables["QualifierAbs"].Rows[n]["PaymentSchId"] + " And QualifierId=" + ds.Tables["QualifierAbs"].Rows[n]["QualifierId"] + " And ReceiptTypeId=" + ds.Tables["QualifierAbs"].Rows[n]["ReceiptTypeId"] + " And OtherCostId=" + ds.Tables["QualifierAbs"].Rows[n]["OtherCostId"] + "";
                        dtAbs = dv.ToTable();
                        for (int i = 0; i < dtAbs.Rows.Count; i++)
                        {
                            dAmt = dAmt + Convert.ToDecimal(dtAbs.Rows[i]["Amount"]);
                        }

                        ds.Tables["QualifierAbs"].Rows[n]["Amount"] = dAmt;
                    }
                }
                else
                {
                    ds.Tables.Add(dtTQ);
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
            return ds;
        }

        public static bool GetFAUpdateFound(int argCCId)
        {
            DataTable dt;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB(); bool bAns = false;
            try
            {
                sSql = " Select ProgressBillFAUpdate From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre " +
                    " Where CostCentreId=" + argCCId + " ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dt.Rows[0]["ProgressBillFAUpdate"]) == true) { bAns = true; }
                }
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
            return bAns;
        }

        public static DataTable GetFlat(int argCCId, int argBuyerId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql = "";

            sSql = " SELECT FlatId, FlatNo from ( " +
                    " SELECT F.FlatId,F.FlatNo FROM dbo.LeadRegister R" +
                    " Inner Join dbo.BuyerDetail C On C.LeadId=R.LeadId " +
                    " Inner Join dbo.FlatDetails F On F.LeadId=C.LeadId And F.FlatId=C.FlatId" +
                    " WHERE C.CostCentreId=" + argCCId + " AND C.LeadId=" + argBuyerId +
                    " UNION ALL " +
                    " SELECT F.PlotDetailsId FlatId,F.PlotNo FlatNo FROM dbo.LeadRegister R" +
                    " Inner Join dbo.BuyerDetail C On C.LeadId=R.LeadId " +
                    " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails F On F.BuyerId=C.LeadId And F.PlotDetailsId=C.PlotId" +
                    " WHERE C.CostCentreId=" + argCCId + " AND C.LeadId=" + argBuyerId + ") X ";
                    //" ORDER BY LeadName";
            try
            {
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;

        }

        public static DataSet GetFlatReceipt(int argCCId, int argBuyerId,DateTime argDate)
        {
            DataTable dt;
            DataSet ds = new DataSet(); 
            SqlDataAdapter sda;
            string sSql = "";
            string sStage = ""; int argSId = 0;

            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.PaymentSchId,B.CostCentreId,A.TemplateId,B.PayTypeId,C.Typewise,A.SchType,A.SchDescId,A.StageId,A.OtherCostId,B.FlatId,A.SchDate,B.FlatNo, " +
                            " 'ScheduleBill' ReceiptType,A.Description,A.Amount Gross,A.NetAmount,A.SurplusAmount,A.PaidAmount, " +
                            " (A.NetAmount-A.SurplusAmount)-A.PaidAmount BalanceAmount, Cast(0 as Decimal(18,3)) Amount " +
                            " From PaymentScheduleFlat A  Inner Join FlatDetails B On A.FlatId=B.FlatId  " +
                            " Inner Join PaySchType C On C.TypeId=B.PayTypeId  Where LeadId=" + argBuyerId + " And TemplateId<>0 " +
                            " And (A.NetAmount-A.SurplusAmount)-A.PaidAmount>0 And A.BillPassed=0 " +
                            " And A.SchDate <= '" + string.Format(Convert.ToDateTime(argDate).ToString("dd-MMM-yyyy")) + "' Order By A.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "PaymentScheduleFlat");
                sda.Dispose();


                DataTable dtO = new DataTable();
                DataTable dtN = new DataTable();
                DataTable dtQ;
                dtO = ds.Tables["PaymentScheduleFlat"];
                int iFlatId = 0;
                int iPayId = 0;
                bool bPayTypewise = false;

                DataTable dtT = new DataTable("ReceiptType");
                dtT.Columns.Add("FlatId", typeof(int));
                dtT.Columns.Add("PaymentSchId", typeof(int));
                dtT.Columns.Add("ReceiptTypeId", typeof(int));
                dtT.Columns.Add("OtherCostId", typeof(int));
                dtT.Columns.Add("SchType", typeof(string));
                dtT.Columns.Add("Sel", typeof(bool));
                dtT.Columns.Add("ReceiptType", typeof(string));
                dtT.Columns.Add("Percentage", typeof(decimal));
                dtT.Columns.Add("Amount", typeof(decimal));
                dtT.Columns.Add("NetAmount", typeof(decimal));

                DataRow dr;

                for (int i = 0; i < dtO.Rows.Count; i++)
                {
                    iFlatId = Convert.ToInt32(dtO.Rows[i]["FlatId"].ToString());
                    iPayId = Convert.ToInt32(dtO.Rows[i]["PaymentSchId"].ToString());
                    bPayTypewise = Convert.ToBoolean(dtO.Rows[i]["Typewise"].ToString());
                    if (dtO.Rows[i]["SchType"].ToString() == "O")
                    {
                        sStage = "OtherCost";
                        argSId = Convert.ToInt32(dtO.Rows[i]["OtherCostId"].ToString());
                    }
                    else if (dtO.Rows[i]["SchType"].ToString() == "D")
                    {
                        sStage = "SchDescription";
                        argSId = Convert.ToInt32(dtO.Rows[i]["SchDescId"].ToString());
                    }
                    else if (dtO.Rows[i]["SchType"].ToString() == "S")
                    {
                        sStage = "Stagewise";
                        argSId = Convert.ToInt32(dtO.Rows[i]["StageId"].ToString());
                    }

                    dtN = new DataTable();

                    if (sStage == "OtherCost")
                    {
                        sSql = "Select A.ReceiptTypeId,A.OtherCostId,A.SchType,Convert(bit,1,1) Sel, B.OtherCostName ReceiptType,A.Percentage,A.Amount,A.NetAmount from dbo.FlatReceiptType A " +
                               "Inner Join dbo.OtherCostMaster B on A.OtherCostId=B.OtherCostId " +
                               "Where PaymentSchId= " + iPayId + " And FlatId = " + iFlatId;
                    }
                    else
                    {
                        sSql = "Select A.ReceiptTypeId,0 OtherCostId,'R' SchType,Case When B.ReceiptTypeId is null then Convert(bit,0,0) " +
                                " Else Convert(bit,1,1) End Sel,A.ReceiptTypeName ReceiptType,ISNULL(B.Percentage,0) Percentage, " +
                                " isnull(B.Amount,0) Amount,isnull(B.NetAmount,0) NetAmount From dbo.ReceiptType A " +
                                " Left Join dbo.FlatReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId And B.SchType<>'Q' " +
                                " and B.PaymentSchId=" + iPayId + " and FlatId= " + iFlatId + " Where A.ReceiptTypeId <>1 " +
                                " Union All " +
                                " Select 0 ReceiptTypeId,A.OtherCostId,'O' SchType,Case When B.ReceiptTypeId is null " +
                                " then Convert(bit,0,0) Else Convert(bit,1,1) End Sel,A.OtherCostName ReceiptType,ISNULL(B.Percentage,0) Percentage, " +
                                " isnull(B.Amount,0) Amount,isnull(B.NetAmount,0) NetAmount from dbo.OtherCostMaster A " +
                                " Inner Join dbo.CCOtherCost CO On CO.OtherCostId=A.OtherCostId And CO.CostCentreId=" + argCCId + " " +
                                " Left Join dbo.FlatReceiptType B " +
                                " on A.OtherCostId=B.OtherCostId and B.PaymentSchId=" + iPayId + " and FlatId= " + iFlatId + " Where A.OtherCostId " +
                                " in (Select OtherCostId from dbo.OtherCostSetupTrans Where CostCentreId=" + argCCId + ")";
                    }
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    sda.Fill(dtN);
                    sda.Dispose();

                    foreach (DataRow drow in dtN.Rows)
                    {
                        dr = dtT.NewRow();

                        dr["FlatId"] = iFlatId;
                        dr["PaymentSchId"] = iPayId;
                        dr["ReceiptTypeId"] = Convert.ToInt32(drow["ReceiptTypeId"]);
                        dr["OtherCostId"] = Convert.ToInt32(drow["OtherCostId"]);
                        dr["SchType"] = drow["SchType"].ToString();
                        dr["Sel"] = Convert.ToBoolean(drow["Sel"]);
                        dr["ReceiptType"] = drow["ReceiptType"].ToString();
                        dr["Percentage"] = Convert.ToDecimal(drow["Percentage"]);
                        dr["Amount"] = Convert.ToDecimal(drow["Amount"]);
                        dr["NetAmount"] = Convert.ToDecimal(drow["NetAmount"]);

                        dtT.Rows.Add(dr);
                    }

                    sSql = "Select B.PaymentSchId,A.ReceiptTypeId,0 OtherCostId,'A' SchType,Case When B.ReceiptTypeId is null" +
                    " then Convert(bit,0,0) Else Convert(bit,1,1) End Sel,A.ReceiptTypeName ReceiptType," +
                    " ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount,isnull(B.NetAmount,0)" +
                    " NetAmount From dbo.ReceiptType A Left join dbo.FlatReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId" +
                    " and B.PaymentSchId=" + iPayId + " Where A.ReceiptTypeId = 1 ";

                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    sda.Fill(ds, "FlatReceiptTypeAdvance");
                    sda.Dispose();


                    if (bPayTypewise == false)
                    {
                        sSql = "Select B.Sel,A.QualifierId, A.QualifierName,B.Percentage,B.Amount,B.Amount QAmount " +
                            " from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A  " +
                            " Inner Join dbo.PaySchTaxFlat B On A.QualifierId=B.QualifierId " +
                            " Left Join dbo.FlatTax C On C.QualifierId=B.QualifierId and C.FlatId=B.FlatId" +
                            " Where QualType='B' And B.FlatId=" + iFlatId + " And PaymentSchId=" + iPayId + "";
                        sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                        dtQ = new DataTable();
                        sda.Fill(dtQ);
                        sda.Dispose();

                        foreach (DataRow drow in dtQ.Rows)
                        {
                            dr = dtT.NewRow();

                            dr["FlatId"] = iFlatId;
                            dr["PaymentSchId"] = iPayId;
                            dr["ReceiptTypeId"] = Convert.ToInt32(drow["QualifierId"]);
                            dr["OtherCostId"] = 0;
                            dr["SchType"] = "Q";
                            dr["Sel"] = Convert.ToBoolean(drow["Sel"]);
                            dr["ReceiptType"] = drow["QualifierName"].ToString();
                            dr["Percentage"] = Convert.ToDecimal(drow["Percentage"]);
                            dr["Amount"] = Convert.ToDecimal(drow["Amount"]);
                            dr["NetAmount"] = Convert.ToDecimal(drow["Amount"]);

                            dtT.Rows.Add(dr);
                        }
                    }
                    }

                    ds.Tables.Add(dtT);

                    
                        sSql = "Select B.FlatId,B.PaymentSchId,A.QualifierId,A.Expression,A.ExpPer,A.NetPer,A.Add_Less_Flag,A.SurCharge, " +
                              "A.EDCess,A.HEDPer,A.ExpValue,A.ExpPerValue,A.SurValue,A.EDValue,A.Amount,B.SchType,B.ReceiptTypeId,B.OtherCostId " +
                              "From dbo.FlatReceiptQualifier A " +
                              "Inner Join dbo.FlatReceiptType B on A.SchId=b.SchId Where B.PaymentSchId In ( " +
                              " Select A.PaymentSchId " +
                            " From PaymentScheduleFlat A  Inner Join FlatDetails B On A.FlatId=B.FlatId  " +
                            " Inner Join PaySchType C On C.TypeId=B.PayTypeId  Where LeadId=" + argBuyerId + " And TemplateId<>0 " +
                            " And (A.NetAmount-A.SurplusAmount)-A.PaidAmount>0 And A.BillPassed=0 " +
                            " And A.SchDate <= '" + string.Format(Convert.ToDateTime(argDate).ToString("dd-MMM-yyyy")) + "')";
                        
                        sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                        sda.Fill(ds, "FlatReceiptQualifier");
                        sda.Dispose();


                        sSql = "Select B.FlatId,B.PaymentSchId,A.QualifierId,B.SchType,B.ReceiptTypeId,B.OtherCostId,isnull(C.AccountId,0) AccountId, A.Add_Less_Flag,Sum(A.Amount) Amount " +
                              "From dbo.FlatReceiptQualifier A " +
                              "Inner Join dbo.FlatReceiptType B on A.SchId=b.SchId " +
                              "Left Join dbo.QualifierAccount C on A.QualifierId=C.QualifierId " +
                              "Where A.Amount <>0 and B.PaymentSchId In ( "+
                              " Select A.PaymentSchId " +
                            " From PaymentScheduleFlat A  Inner Join FlatDetails B On A.FlatId=B.FlatId  " +
                            " Inner Join PaySchType C On C.TypeId=B.PayTypeId  Where LeadId=" + argBuyerId + " And TemplateId<>0 " +
                            " And (A.NetAmount-A.SurplusAmount)-A.PaidAmount>0 And A.BillPassed=0 " +
                            " And A.SchDate <= '" + string.Format(Convert.ToDateTime(argDate).ToString("dd-MMM-yyyy")) + "' )";

                        sSql = sSql + " Group by B.FlatId,B.PaymentSchId,A.QualifierId,B.SchType,B.ReceiptTypeId,B.OtherCostId,C.AccountId,A.Add_Less_Flag";

                        sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                        sda.Fill(ds, "QualifierAbs");
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

            return ds;
        }

        //public static void InsertReceiptDetails(ReceiptDetailBO argReceiptContactBO, DataTable dtReceipt, string argType, DataTable dtQual, DataTable dtPayInfo, DataTable argdtRT, DataTable argdtRQ,DataTable dtQualAbs)
        //{
        //    int iRecpId = 0; int iRecpTransId = 0; bool bAns = false;
        //    SqlConnection conn = new SqlConnection();
        //    SqlCommand cmd;
        //    conn = BsfGlobal.OpenCRMDB();
        //    using (SqlTransaction tran = conn.BeginTransaction())
        //    {
        //        try
        //        {
        //            string sSql = String.Format("INSERT INTO dbo.ReceiptRegister(ReceiptDate,ReceiptNo,ChequeNo,ChequeDate,BankName,CostCentreId,LeadId,PaymentAgainst,Amount,Status,Narration,TenantId,PaymentMode,BuyerAccountId,BillType,SurplusAmount) VALUES('{0}', '{1}', '{2}','{3}','{4}',{5},{6},'{7}','{8}','{9}','{10}',{11},'{12}',{13},'{14}',{15}) SELECT SCOPE_IDENTITY()", ReceiptDetailBO.ReceiptDate, ReceiptDetailBO.ReceiptNo, ReceiptDetailBO.ChequeNo, ReceiptDetailBO.ChequeDate, ReceiptDetailBO.BankName, ReceiptDetailBO.CostCentreId, ReceiptDetailBO.BuyerId, ReceiptDetailBO.PaymentAgainst, ReceiptDetailBO.Amount, ReceiptDetailBO.Status, ReceiptDetailBO.Narration, ReceiptDetailBO.TenantId, ReceiptDetailBO.PaymentMode, ReceiptDetailBO.BuyerAcctId, ReceiptDetailBO.BillType, ReceiptDetailBO.SurplusAmount);
        //            cmd = new SqlCommand(sSql, conn, tran);
        //            iRecpId = int.Parse(cmd.ExecuteScalar().ToString());
        //            cmd.Dispose();

        //            if (dtReceipt.Rows.Count > 0)
        //            {
        //                for (int k = 0; k < dtReceipt.Rows.Count; k++)
        //                {
        //                    if (Convert.ToDecimal(dtReceipt.Rows[k]["Amount"]) != 0)
        //                    {
        //                        if (ReceiptDetailBO.PaymentAgainst == "B" || ReceiptDetailBO.PaymentAgainst == "PB")
        //                        {
        //                            sSql = "INSERT INTO dbo.ReceiptTrans(ReceiptId,ReceiptType,FlatId,BillRegId,GrossAmount,NetAmount,SurplusAmount,PaidAmount,BalanceAmount, " +
        //                                " Amount,CostCentreId,PaySchId) values(" + iRecpId + ",'" + dtReceipt.Rows[k]["ReceiptType"].ToString().Trim() + "'," + dtReceipt.Rows[k]["FlatId"] + "," +
        //                                " " + dtReceipt.Rows[k]["BillRegId"] + "," + dtReceipt.Rows[k]["Gross"] + "," + dtReceipt.Rows[k]["NetAmount"] + "," + dtReceipt.Rows[k]["SurplusAmount"] + "," +
        //                                " " + dtReceipt.Rows[k]["PaidAmount"] + "," + dtReceipt.Rows[k]["BalanceAmount"] + "," + dtReceipt.Rows[k]["Amount"] + ", " +
        //                                " " + dtReceipt.Rows[k]["CostCentreId"] + "," + dtReceipt.Rows[k]["PaymentSchId"] + ")SELECT SCOPE_IDENTITY()";

        //                        }
        //                        else if (ReceiptDetailBO.PaymentAgainst == "S")
        //                        {
        //                            sSql = "INSERT INTO dbo.ReceiptTrans(ReceiptId,ReceiptType,FlatId,GrossAmount,NetAmount,SurplusAmount,PaidAmount,BalanceAmount,Amount,CostCentreId,PaySchId) values(" + iRecpId + "," +
        //                                " '" + dtReceipt.Rows[k]["ReceiptType"].ToString().Trim() + "'," + dtReceipt.Rows[k]["FlatId"] + "," + dtReceipt.Rows[k]["Gross"] + ", " +
        //                                " " + dtReceipt.Rows[k]["NetAmount"] + "," + dtReceipt.Rows[k]["SurplusAmount"] + "," + dtReceipt.Rows[k]["PaidAmount"] + "," + dtReceipt.Rows[k]["BalanceAmount"] + "," +
        //                                " " + dtReceipt.Rows[k]["Amount"] + "," + dtReceipt.Rows[k]["CostCentreId"] + "," + dtReceipt.Rows[k]["PaymentSchId"] + ")SELECT SCOPE_IDENTITY()";

        //                        }
        //                        else if (ReceiptDetailBO.PaymentAgainst == "R")
        //                        {
        //                            sSql = String.Format("INSERT INTO dbo.ReceiptTrans(ReceiptId,ReceiptType,FlatId,Amount,CostCentreId,BillRegId) values({0},'{1}',{2},'{3}',{4},{5})SELECT SCOPE_IDENTITY()", iRecpId, dtReceipt.Rows[k]["ReceiptType"].ToString().Trim(), dtReceipt.Rows[k]["FlatId"], dtReceipt.Rows[k]["Amount"], dtReceipt.Rows[k]["CostCentreId"], dtReceipt.Rows[k]["TransId"]);

        //                        }
        //                        else if (ReceiptDetailBO.PaymentAgainst == "A")
        //                        {
        //                            sSql = String.Format("INSERT INTO dbo.ReceiptTrans(ReceiptId,FlatId,Amount,CostCentreId,ReceiptType,PaySchId) values({0},{1},'{2}',{3},'{4}',{5})SELECT SCOPE_IDENTITY()", iRecpId, dtReceipt.Rows[k]["FlatId"], dtReceipt.Rows[k]["Amount"], dtReceipt.Rows[k]["CostCentreId"], dtReceipt.Rows[k]["ReceiptType"].ToString().Trim(), dtReceipt.Rows[k]["PaymentSchId"]);
        //                        }
        //                        else
        //                        {
        //                            sSql = String.Format("INSERT INTO dbo.ReceiptTrans(ReceiptId,FlatId,Amount,CostCentreId,ReceiptType) values({0},{1},'{2}',{3},'{4}')SELECT SCOPE_IDENTITY()", iRecpId, dtReceipt.Rows[k]["FlatId"], dtReceipt.Rows[k]["Amount"], dtReceipt.Rows[k]["CostCentreId"], dtReceipt.Rows[k]["ReceiptType"].ToString().Trim());
        //                        }
        //                        cmd = new SqlCommand(sSql, conn, tran);
        //                        iRecpTransId = int.Parse(cmd.ExecuteScalar().ToString());
        //                        cmd.Dispose();

                               
        //                        if (ReceiptDetailBO.PaymentAgainst == "B" || ReceiptDetailBO.PaymentAgainst == "PB")
        //                        {
        //                            if (Convert.ToInt32(dtReceipt.Rows[k]["BillRegId"]) == 0)
        //                            {
        //                                sSql = "INSERT INTO dbo.ReceiptSurplusDet (ReceiptId,PaySchId,Amount) Values (" + iRecpId + "," +
        //                                    " " + dtReceipt.Rows[k]["PaymentSchId"] + "," + dtReceipt.Rows[k]["Amount"] + ")";
        //                                cmd = new SqlCommand(sSql, conn, tran);
        //                                cmd.ExecuteNonQuery();
        //                                cmd.Dispose();

        //                                sSql = "Update dbo.PaymentScheduleFlat Set SurplusAmount=" + dtReceipt.Rows[k]["Amount"] + ",PaidAmount=" + dtReceipt.Rows[k]["Amount"] + " " +
        //                                   " Where PaymentSchId= " + dtReceipt.Rows[k]["PaymentSchId"] + " ";
        //                                cmd = new SqlCommand(sSql, conn, tran);
        //                                cmd.ExecuteNonQuery();
        //                                cmd.Dispose();
        //                            }
        //                        }
        //                    }

        //                    if (ReceiptDetailBO.PaymentAgainst == "B" || ReceiptDetailBO.PaymentAgainst == "PB" || ReceiptDetailBO.PaymentAgainst == "S")
        //                    {
        //                        DataView dv = new DataView(dtQualAbs);

        //                        if (dv.ToTable().Rows.Count > 0)
        //                        {
        //                            dv.RowFilter = "FlatId = " + dtReceipt.Rows[k]["FlatId"] + " and PaymentSchId = " + dtReceipt.Rows[k]["PaymentSchId"];
        //                            DataTable dt = new DataTable();
        //                            if (dv.ToTable().Rows.Count > 0) { dt = dv.ToTable(); }
        //                            dv.Dispose();

        //                            for (int j = 0; j < dt.Rows.Count; j++)
        //                            {
        //                                if (Convert.ToDecimal(dt.Rows[j]["Amount"]) > 0)
        //                                {
        //                                    if (ReceiptDetailBO.PaymentAgainst == "S")
        //                                    {
        //                                        sSql = "Insert Into dbo.ReceiptQualifierAbs(QualifierId,Add_Less_Flag,ReceiptId,ReceiptTransId,PBillId,PaymentSchId,AccountId,Amount) " +
        //                                               "Values(" + Convert.ToInt32(dt.Rows[j]["QualifierId"]) + ",'" + dt.Rows[j]["Add_Less_Flag"].ToString() + "'," + iRecpId + "," + iRecpTransId + "," +
        //                                               "0," + dtReceipt.Rows[k]["PaymentSchId"] + "," + Convert.ToInt32(dt.Rows[j]["AccountId"]) + ", " + Convert.ToDecimal(dt.Rows[j]["Amount"]) + ")";
        //                                    }
        //                                    else if (ReceiptDetailBO.PaymentAgainst == "B" || ReceiptDetailBO.PaymentAgainst == "PB")
        //                                    {
        //                                        sSql = "Insert Into dbo.ReceiptQualifierAbs(QualifierId,Add_Less_Flag,ReceiptId,ReceiptTransId,PBillId,PaymentSchId,AccountId,Amount) " +
        //                                                "Values(" + Convert.ToInt32(dt.Rows[j]["QualifierId"]) + ",'" + dt.Rows[j]["Add_Less_Flag"].ToString() + "'," + iRecpId + "," + iRecpTransId + "," +
        //                                                "" + dtReceipt.Rows[k]["BillRegId"] + "," + dtReceipt.Rows[k]["PaymentSchId"] + "," + Convert.ToInt32(dt.Rows[j]["AccountId"]) + ", " + Convert.ToDecimal(dt.Rows[j]["Amount"]) + ")";
        //                                    }
        //                                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();
        //                                }
        //                            }

        //                        }
        //                    }



        //                    if (ReceiptDetailBO.PaymentAgainst == "S" || ReceiptDetailBO.PaymentAgainst == "B")
        //                    {
        //                        DataView dv = new DataView(argdtRT);

        //                        if (dv.ToTable().Rows.Count > 0)
        //                        {
        //                            dv.RowFilter = "FlatId = " + dtReceipt.Rows[k]["FlatId"] + " and PaymentSchId = " + dtReceipt.Rows[k]["PaymentSchId"];
        //                            DataTable dtRT = new DataTable();
        //                            if (dv.ToTable().Rows.Count > 0) { dtRT = dv.ToTable(); }
        //                            dv.Dispose();

        //                            for (int i = 0; i < dtRT.Rows.Count; i++)
        //                            {
        //                                sSql = "INSERT INTO ReceiptShTrans (ReceiptId,FlatId,PaymentSchId,ReceiptTypeId,OtherCostId,GrossAmount,TaxAmount,NetAmount," +
        //                                " PaidGrossAmount,PaidTaxAmount,PaidNetAmount) Values (" + iRecpId + "," + dtRT.Rows[i]["FlatId"] + "," + dtRT.Rows[i]["PaymentSchId"] + "," +
        //                                " " + dtRT.Rows[i]["ReceiptTypeId"] + "," + dtRT.Rows[i]["OtherCostId"] + "," + CommFun.IsNullCheck(dtRT.Rows[i]["GrossAmount"], CommFun.datatypes.vartypenumeric) + "," +
        //                                " " + CommFun.IsNullCheck(dtRT.Rows[i]["TaxAmount"], CommFun.datatypes.vartypenumeric) + "," + CommFun.IsNullCheck(dtRT.Rows[i]["NetAmount"], CommFun.datatypes.vartypenumeric) + "," + CommFun.IsNullCheck(dtRT.Rows[i]["PaidGrossAmount"], CommFun.datatypes.vartypenumeric) + "" +
        //                                " ," + CommFun.IsNullCheck(dtRT.Rows[i]["PaidTaxAmount"], CommFun.datatypes.vartypenumeric) + "," + CommFun.IsNullCheck(dtRT.Rows[i]["PaidNetAmount"], CommFun.datatypes.vartypenumeric) + ")";
        //                                cmd = new SqlCommand(sSql, conn, tran);
        //                                cmd.ExecuteNonQuery();
        //                                cmd.Dispose();
        //                            }
        //                        }

        //                        dv = new DataView(argdtRQ);

        //                        if (dv.ToTable().Rows.Count > 0)
        //                        {
        //                            dv.RowFilter = "FlatId = " + dtReceipt.Rows[k]["FlatId"] + " and PaymentSchId = " + dtReceipt.Rows[k]["PaymentSchId"];
        //                            DataTable dtRQ = new DataTable();
        //                            if (dv.ToTable().Rows.Count > 0) { dtRQ = dv.ToTable(); }
        //                            dv.Dispose();

        //                            for (int i = 0; i < dtRQ.Rows.Count; i++)
        //                            {
        //                                sSql = "INSERT INTO ReceiptQualifier (ReceiptId,FlatId,PaymentSchId,ReceiptTypeId,OtherCostId,QualifierId,Expression,ExpPer,NetPer,Add_Less_Flag,SurCharge," +
        //                                " EDCess,HEDPer,ExpValue,ExpPerValue,SurValue,EDValue,Amount,TaxablePer,TaxableValue) Values (" + iRecpId + "," + dtRQ.Rows[i]["FlatId"] + "," + dtRQ.Rows[i]["PaymentSchId"] + "," +
        //                               " " + dtRQ.Rows[i]["ReceiptTypeId"] + "," + dtRQ.Rows[i]["OtherCostId"] + "," + dtRQ.Rows[i]["QualifierId"] + ",'" + dtRQ.Rows[i]["Expression"] + "'," +
        //                                " " + dtRQ.Rows[i]["ExpPer"] + "," + dtRQ.Rows[i]["NetPer"] + ",'" + dtRQ.Rows[i]["Add_Less_Flag"] + "'," + dtRQ.Rows[i]["SurCharge"] + "," +
        //                                " " + dtRQ.Rows[i]["EDCess"] + "," + dtRQ.Rows[i]["HEDPer"] + "," + dtRQ.Rows[i]["ExpValue"] + "," + dtRQ.Rows[i]["ExpPerValue"] + "," +
        //                                " " + dtRQ.Rows[i]["SurValue"] + "," + dtRQ.Rows[i]["EDValue"] + "," + dtRQ.Rows[i]["Amount"] + "," + dtRQ.Rows[i]["TaxablePer"] + "," + dtRQ.Rows[i]["TaxableValue"] + ")";
        //                                cmd = new SqlCommand(sSql, conn, tran);
        //                                cmd.ExecuteNonQuery();
        //                                cmd.Dispose();
        //                            }
        //                        }
        //                    }

        //                }
        //            }

        //            if (ReceiptDetailBO.PaymentAgainst == "S" || ReceiptDetailBO.PaymentAgainst == "B")
        //            {
        //                //Inserting Excess Amount for particular flat
        //                if (ReceiptDetailBO.ExcessAmount > 0)
        //                {
        //                    sSql = "INSERT INTO dbo.ExtraPayment (ReceiptId,FlatId,LeadId,Amount) Values " +
        //                            " (" + iRecpId + "," + ReceiptDetailBO.FlatId + "," + ReceiptDetailBO.BuyerId + "," + ReceiptDetailBO.ExcessAmount + ")";
        //                    cmd = new SqlCommand(sSql, conn, tran);
        //                    cmd.ExecuteNonQuery();
        //                    cmd.Dispose();
        //                }
        //            }

        //            if (dtPayInfo != null)
        //            {
        //                for (int i = 0; i < dtPayInfo.Rows.Count; i++)
        //                {
        //                    if (Convert.ToDecimal(dtPayInfo.Rows[i]["SurplusAmount"]) > 0)
        //                    {
        //                        sSql = "INSERT INTO ReceiptSurplusDet (ReceiptId,PaySchId,Amount) Values (" + iRecpId + "," +
        //                            " " + dtPayInfo.Rows[i]["PaySchId"] + "," + dtPayInfo.Rows[i]["SurplusAmount"] + ")";
        //                        cmd = new SqlCommand(sSql, conn, tran);
        //                        cmd.ExecuteNonQuery();
        //                        cmd.Dispose();

        //                        sSql = "Update PaymentScheduleFlat Set SurplusAmount=" + dtPayInfo.Rows[i]["SurplusAmount"] + ",PaidAmount=" + dtPayInfo.Rows[i]["SurplusAmount"] + " " +
        //                           " Where PaymentSchId= " + dtPayInfo.Rows[i]["PaySchId"] + " ";
        //                        cmd = new SqlCommand(sSql, conn, tran);
        //                        cmd.ExecuteNonQuery();
        //                        cmd.Dispose();

        //                    }
        //                }
        //            }

        //            tran.Commit();
        //            UpdatePaidAmt(dtReceipt, iRecpId, argType);
        //            bAns = true;

        //            if (bAns == true)
        //            {
        //                BsfGlobal.InsertLog(DateTime.Now, "Buyer-Receipt-Add", "N", ReceiptDetailBO.Narration, iRecpId, ReceiptDetailBO.CostCentreId, 0, BsfGlobal.g_sCRMDBName, ReceiptDetailBO.ReceiptNo, BsfGlobal.g_lUserId, ReceiptDetailBO.Amount, 0);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            tran.Rollback();
        //            BsfGlobal.CustomException(ex.Message, ex.StackTrace);
        //        }
        //        finally
        //        {
        //            conn.Close();
        //            conn.Dispose();
        //        }
        //    }
        //}

        public static void InsertReceiptDetails(ReceiptDetailBO argReceiptContactBO, DataTable dtReceipt, string argType, DataTable dtQual, DataTable dtPayInfo, DataTable argdtRT, DataTable argdtRQ, DataTable dtQualAbs)
        {
            bool bAns = false;
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    BsfGlobal.VoucherType oVType = new BsfGlobal.VoucherType();
                    oVType = BsfGlobal.GetVoucherNo(26, Convert.ToDateTime(ReceiptDetailBO.ReceiptDate), 0, 0, conn, tran, "I");
                    if (oVType.GenType == true)
                    {
                        ReceiptDetailBO.ReceiptNo = oVType.VoucherNo;
                    }

                    string sSql = "INSERT INTO dbo.ReceiptRegister(ReceiptDate,ReceiptNo,ChequeNo,ChequeDate,BankName,CostCentreId,FlatId,LeadId,PaymentAgainst," +
                                    " Amount,Status,Narration,TenantId,PaymentMode,BuyerAccountId,BillType,Interest) VALUES('" + ReceiptDetailBO.ReceiptDate + "'," +
                                    " '" + ReceiptDetailBO.ReceiptNo + "','" + ReceiptDetailBO.ChequeNo + "',@ChequeDate," +
                                    " '" + ReceiptDetailBO.BankName + "'," + ReceiptDetailBO.CostCentreId + "," + ReceiptDetailBO.FlatId + "," + ReceiptDetailBO.BuyerId + "," +
                                    " '" + ReceiptDetailBO.PaymentAgainst + "'," + ReceiptDetailBO.Amount + ",'" + ReceiptDetailBO.Status + "'," +
                                    " '" + ReceiptDetailBO.Narration + "'," + ReceiptDetailBO.TenantId + ",'" + ReceiptDetailBO.PaymentMode + "'," +
                                    " " + ReceiptDetailBO.BuyerAcctId + ",'" + ReceiptDetailBO.BillType + "'," + ReceiptDetailBO.Interest + ") SELECT SCOPE_IDENTITY()";
                    SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                    SqlParameter parameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName="@ChequeDate" };
                    if (Convert.ToDateTime(ReceiptDetailBO.ChequeDate) == DateTime.MinValue)
                        parameter.Value = DBNull.Value;
                    else
                        parameter.Value = ReceiptDetailBO.ChequeDate;
                    cmd.Parameters.Add(parameter);
                    int iRecpId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();
                    
                    if (dtReceipt.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtReceipt.Rows.Count; k++)
                        {
                            int iRecpTransId = 0;
                            if (Convert.ToDecimal(dtReceipt.Rows[k]["Amount"]) != 0)
                            {
                                if (dtReceipt.Rows[k]["ReceiptType"].ToString() == "ProgressBill" && ReceiptDetailBO.PaymentAgainst == "B")
                                {
                                    decimal dBalanceAmt = Convert.ToDecimal(dtReceipt.Rows[k]["BalanceAmount"]) - Convert.ToDecimal(dtReceipt.Rows[k]["Advance"]);

                                    sSql = "INSERT INTO dbo.ReceiptTrans(ReceiptId,ReceiptType,FlatId,BillRegId,GrossAmount,NetAmount,PaidAmount,BalanceAmount, " +
                                            " Amount,CostCentreId,PaySchId) values(" + iRecpId + ",'" + dtReceipt.Rows[k]["ReceiptType"].ToString().Trim() +
                                            "'," + dtReceipt.Rows[k]["FlatId"] + "," + dtReceipt.Rows[k]["BillRegId"] + "," + dtReceipt.Rows[k]["Gross"] +
                                            "," + dtReceipt.Rows[k]["NetAmount"] + "," + dtReceipt.Rows[k]["PaidAmount"] + "," + dBalanceAmt +
                                            "," + dtReceipt.Rows[k]["Amount"] + "," + dtReceipt.Rows[k]["CostCentreId"] + "," + dtReceipt.Rows[k]["PaymentSchId"] +
                                            ") SELECT SCOPE_IDENTITY();";
                                }
                                else if (dtReceipt.Rows[k]["ReceiptType"].ToString() == "ScheduleBill" && ReceiptDetailBO.PaymentAgainst == "B")
                                {
                                    decimal dBalanceAmt = Convert.ToDecimal(dtReceipt.Rows[k]["BalanceAmount"]) - Convert.ToDecimal(dtReceipt.Rows[k]["Advance"]);

                                    sSql = "INSERT INTO dbo.ReceiptTrans(ReceiptId,ReceiptType,FlatId,GrossAmount,NetAmount,PaidAmount,BalanceAmount,Amount,CostCentreId,PaySchId) " +
                                            " values(" + iRecpId + ",'" + dtReceipt.Rows[k]["ReceiptType"].ToString().Trim() + "'," + dtReceipt.Rows[k]["FlatId"] +
                                            "," + dtReceipt.Rows[k]["Gross"] + "," + dtReceipt.Rows[k]["NetAmount"] + "," + dtReceipt.Rows[k]["PaidAmount"] +
                                            "," + dBalanceAmt + "," + dtReceipt.Rows[k]["Amount"] + "," + dtReceipt.Rows[k]["CostCentreId"] +
                                            "," + dtReceipt.Rows[k]["PaymentSchId"] + ") SELECT SCOPE_IDENTITY();";
                                }
                                else if (dtReceipt.Rows[k]["ReceiptType"].ToString() == "ExtraBill")
                                {
                                    decimal dNetAmt = Convert.ToDecimal(dtReceipt.Rows[k]["NetAmount"]);
                                    decimal dPaidAmt = Convert.ToDecimal(dtReceipt.Rows[k]["PaidAmount"]);
                                    decimal dCurrentAmt = Convert.ToDecimal(dtReceipt.Rows[k]["Amount"]);
                                    decimal dBalanceAmt = Convert.ToDecimal(dtReceipt.Rows[k]["BalanceAmount"]);

                                    sSql = "INSERT INTO dbo.ReceiptTrans(ReceiptId,ReceiptType,FlatId,BillRegId,GrossAmount,NetAmount,PaidAmount,BalanceAmount, " +
                                            " Amount,CostCentreId,PaySchId) values(" + iRecpId + ",'" + dtReceipt.Rows[k]["ReceiptType"].ToString().Trim() +
                                            "'," + dtReceipt.Rows[k]["FlatId"] + "," + dtReceipt.Rows[k]["BillRegId"] + "," + dtReceipt.Rows[k]["Gross"] +
                                            "," + dNetAmt + "," + (dPaidAmt + dCurrentAmt) + "," + dBalanceAmt + "," + dCurrentAmt +
                                            "," + dtReceipt.Rows[k]["CostCentreId"] + ",0) SELECT SCOPE_IDENTITY();";
                                }
                                else if (ReceiptDetailBO.PaymentAgainst == "R")
                                {
                                    sSql = String.Format("INSERT INTO dbo.ReceiptTrans(ReceiptId,ReceiptType,FlatId,Amount,CostCentreId,BillRegId) " +
                                                         " values({0},'{1}',{2},'{3}',{4},{5})SELECT SCOPE_IDENTITY()", iRecpId, dtReceipt.Rows[k]["ReceiptType"].ToString().Trim(),
                                                         dtReceipt.Rows[k]["FlatId"], dtReceipt.Rows[k]["Amount"], dtReceipt.Rows[k]["CostCentreId"],
                                                         dtReceipt.Rows[k]["TransId"]);

                                }
                                else if (ReceiptDetailBO.PaymentAgainst == "A")
                                {
                                    sSql = String.Format("INSERT INTO dbo.ReceiptTrans(ReceiptId,FlatId,Amount,CostCentreId,ReceiptType,PaySchId) " +
                                                         " values({0},{1},'{2}',{3},'{4}',{5})SELECT SCOPE_IDENTITY()", iRecpId, dtReceipt.Rows[k]["FlatId"],
                                                         dtReceipt.Rows[k]["Amount"], dtReceipt.Rows[k]["CostCentreId"], dtReceipt.Rows[k]["ReceiptType"].ToString().Trim(),
                                                         dtReceipt.Rows[k]["PaymentSchId"]);
                                }
                                else if (ReceiptDetailBO.PaymentAgainst == "PB")
                                {
                                    decimal dBalanceAmt = Convert.ToDecimal(dtReceipt.Rows[k]["BalanceAmount"]);

                                    sSql = "INSERT INTO dbo.ReceiptTrans(ReceiptId,ReceiptType,FlatId,BillRegId,NetAmount,PaidAmount,BalanceAmount, " +
                                            " Amount,CostCentreId,PaySchId) values(" + iRecpId + ",'" + dtReceipt.Rows[k]["ReceiptType"].ToString().Trim() +
                                            "'," + dtReceipt.Rows[k]["FlatId"] + "," + dtReceipt.Rows[k]["BillRegId"] + 
                                            "," + dtReceipt.Rows[k]["NetAmount"] + "," + dtReceipt.Rows[k]["PaidAmount"] + "," + dBalanceAmt +
                                            "," + dtReceipt.Rows[k]["Amount"] + "," + dtReceipt.Rows[k]["CostCentreId"] +
                                            "," + dtReceipt.Rows[k]["PaymentSchId"] + ") SELECT SCOPE_IDENTITY();";
                                }
                                else
                                {
                                    sSql = String.Format("INSERT INTO dbo.ReceiptTrans(ReceiptId,FlatId,Amount,CostCentreId,ReceiptType) " +
                                                         " values({0},{1},'{2}',{3},'{4}')SELECT SCOPE_IDENTITY()", iRecpId, dtReceipt.Rows[k]["FlatId"],
                                                         dtReceipt.Rows[k]["Amount"], dtReceipt.Rows[k]["CostCentreId"], dtReceipt.Rows[k]["ReceiptType"].ToString().Trim());
                                }
                                cmd = new SqlCommand(sSql, conn, tran);
                                iRecpTransId = int.Parse(cmd.ExecuteScalar().ToString());
                                cmd.Dispose();
                            }

                            if ((ReceiptDetailBO.PaymentAgainst == "B" || ReceiptDetailBO.PaymentAgainst == "PB" || ReceiptDetailBO.PaymentAgainst == "S") && dtQualAbs != null)
                            {
                                DataView dv = new DataView(dtQualAbs);

                                if (dv.ToTable().Rows.Count > 0)
                                {
                                    dv.RowFilter = "FlatId = " + dtReceipt.Rows[k]["FlatId"] + " and PaymentSchId = " + dtReceipt.Rows[k]["PaymentSchId"];
                                    DataTable dt = new DataTable();
                                    if (dv.ToTable().Rows.Count > 0) { dt = dv.ToTable(); }
                                    dv.Dispose();

                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        if (Convert.ToDecimal(dt.Rows[j]["Amount"]) > 0)
                                        {
                                            if (ReceiptDetailBO.PaymentAgainst == "S")
                                            {
                                                sSql = "Insert Into dbo.ReceiptQualifierAbs(QualifierId,Add_Less_Flag,ReceiptId,ReceiptTransId,PBillId,PaymentSchId,AccountId,Amount) " +
                                                       "Values(" + Convert.ToInt32(dt.Rows[j]["QualifierId"]) + ",'" + dt.Rows[j]["Add_Less_Flag"].ToString() + "'," + iRecpId + "," + iRecpTransId + "," +
                                                       "0," + dtReceipt.Rows[k]["PaymentSchId"] + "," + Convert.ToInt32(dt.Rows[j]["AccountId"]) + ", " + Convert.ToDecimal(dt.Rows[j]["Amount"]) + ")";
                                            }
                                            else if (ReceiptDetailBO.PaymentAgainst == "B" || ReceiptDetailBO.PaymentAgainst == "PB")
                                            {
                                                sSql = "Insert Into dbo.ReceiptQualifierAbs(QualifierId,Add_Less_Flag,ReceiptId,ReceiptTransId,PBillId,PaymentSchId,AccountId,Amount) " +
                                                        "Values(" + Convert.ToInt32(dt.Rows[j]["QualifierId"]) + ",'" + dt.Rows[j]["Add_Less_Flag"].ToString() + "'," + iRecpId + "," + iRecpTransId + "," +
                                                        "" + dtReceipt.Rows[k]["BillRegId"] + "," + dtReceipt.Rows[k]["PaymentSchId"] + "," + Convert.ToInt32(dt.Rows[j]["AccountId"]) + ", " + Convert.ToDecimal(dt.Rows[j]["Amount"]) + ")";
                                            }
                                            cmd = new SqlCommand(sSql, conn, tran);
                                            cmd.ExecuteNonQuery();
                                            cmd.Dispose();
                                        }
                                    }
                                }
                            }
                            else if (ReceiptDetailBO.PaymentAgainst == "E")
                            {
                                for (int j = 0; j < dtQualAbs.Rows.Count; j++)
                                {
                                    if (Convert.ToDecimal(dtQualAbs.Rows[j]["Amount"]) > 0)
                                    {
                                        sSql = "Insert Into dbo.ReceiptExtraBillQualifierAbs(QualifierId,Add_Less_Flag,ReceiptId,ReceiptTransId,BillRegId,Amount) " +
                                            "Values(" + Convert.ToInt32(dtQualAbs.Rows[j]["QualifierId"]) + ",'" + dtQualAbs.Rows[j]["Add_Less_Flag"].ToString() +
                                            "'," + iRecpId + "," + iRecpTransId + "," + dtQualAbs.Rows[j]["BillRegId"] + "," + Convert.ToDecimal(dtQualAbs.Rows[j]["Amount"]) + ")";
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                }
                            }

                            if (ReceiptDetailBO.PaymentAgainst == "S" || ReceiptDetailBO.PaymentAgainst == "B")
                            {
                                DataView dv = new DataView(argdtRT);

                                if (dv.ToTable().Rows.Count > 0)
                                {
                                    dv.RowFilter = "FlatId = " + dtReceipt.Rows[k]["FlatId"] + " and PaymentSchId = " + dtReceipt.Rows[k]["PaymentSchId"];
                                    DataTable dtRT = new DataTable();
                                    if (dv.ToTable().Rows.Count > 0) { dtRT = dv.ToTable(); }
                                    dv.Dispose();

                                    for (int i = 0; i < dtRT.Rows.Count; i++)
                                    {
                                        sSql = "INSERT INTO ReceiptShTrans(ReceiptId,FlatId,PaymentSchId,ReceiptTypeId,OtherCostId,GrossAmount,TaxAmount,NetAmount," +
                                                " PaidGrossAmount,PaidTaxAmount,PaidNetAmount,PaidAmount) Values (" + iRecpId + "," + dtRT.Rows[i]["FlatId"] +
                                                "," + dtRT.Rows[i]["PaymentSchId"] + "," + dtRT.Rows[i]["ReceiptTypeId"] + "," + dtRT.Rows[i]["OtherCostId"] +
                                                "," + CommFun.IsNullCheck(dtRT.Rows[i]["GrossAmount"], CommFun.datatypes.vartypenumeric) +
                                                "," + CommFun.IsNullCheck(dtRT.Rows[i]["TaxAmount"], CommFun.datatypes.vartypenumeric) +
                                                "," + CommFun.IsNullCheck(dtRT.Rows[i]["NetAmount"], CommFun.datatypes.vartypenumeric) +
                                                "," + CommFun.IsNullCheck(dtRT.Rows[i]["PaidGrossAmount"], CommFun.datatypes.vartypenumeric) + "" +
                                                " ," + CommFun.IsNullCheck(dtRT.Rows[i]["PaidTaxAmount"], CommFun.datatypes.vartypenumeric) +
                                                "," + CommFun.IsNullCheck(dtRT.Rows[i]["PaidNetAmount"], CommFun.datatypes.vartypenumeric) + "" +
                                                "," + CommFun.IsNullCheck(dtRT.Rows[i]["PaidAmount"], CommFun.datatypes.vartypenumeric) + ")";
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                }

                                dv = new DataView(argdtRQ);

                                if (dv.ToTable().Rows.Count > 0)
                                {
                                    dv.RowFilter = "FlatId = " + dtReceipt.Rows[k]["FlatId"] + " and PaymentSchId = " + dtReceipt.Rows[k]["PaymentSchId"];
                                    DataTable dtRQ = new DataTable();
                                    if (dv.ToTable().Rows.Count > 0) { dtRQ = dv.ToTable(); }
                                    dv.Dispose();

                                    for (int i = 0; i < dtRQ.Rows.Count; i++)
                                    {
                                        sSql = "INSERT INTO ReceiptQualifier(ReceiptId,FlatId,PaymentSchId,ReceiptTypeId,OtherCostId, " +
                                                " QualifierId,Expression,ExpPer,NetPer,Add_Less_Flag,SurCharge,EDCess,HEDPer,ExpValue,ExpPerValue,SurValue," +
                                                " EDValue,Amount,TaxablePer,TaxableValue) Values (" + iRecpId + "," + dtRQ.Rows[i]["FlatId"] +
                                                "," + dtRQ.Rows[i]["PaymentSchId"] + "," + dtRQ.Rows[i]["ReceiptTypeId"] + "," + dtRQ.Rows[i]["OtherCostId"] +
                                                "," + dtRQ.Rows[i]["QualifierId"] + ",'" + dtRQ.Rows[i]["Expression"] + "'," + dtRQ.Rows[i]["ExpPer"] +
                                                "," + dtRQ.Rows[i]["NetPer"] + ",'" + dtRQ.Rows[i]["Add_Less_Flag"] + "'," + dtRQ.Rows[i]["SurCharge"] + "," +
                                                " " + dtRQ.Rows[i]["EDCess"] + "," + dtRQ.Rows[i]["HEDPer"] + "," + dtRQ.Rows[i]["ExpValue"] +
                                                "," + dtRQ.Rows[i]["ExpPerValue"] + "," + dtRQ.Rows[i]["SurValue"] + "," + dtRQ.Rows[i]["EDValue"] +
                                                "," + dtRQ.Rows[i]["Amount"] + "," + dtRQ.Rows[i]["TaxablePer"] + "," + dtRQ.Rows[i]["TaxableValue"] + ")";
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                }
                            }
                            else if (ReceiptDetailBO.PaymentAgainst == "E")
                            {
                                for (int i = 0; i < argdtRQ.Rows.Count; i++)
                                {
                                    sSql = "INSERT INTO dbo.ReceiptExtraBillQualifier(ReceiptId,FlatId,BillRegId,AccountId, " +
                                            " QualifierId,Expression,ExpPer,NetPer,Add_Less_Flag,SurCharge,EDCess,HEDPer,ExpValue,ExpPerValue,SurValue," +
                                            " EDValue,Amount,TaxablePer,TaxableValue,HEDValue) Values (" + iRecpId + "," + argdtRQ.Rows[i]["FlatId"] +
                                            "," + argdtRQ.Rows[i]["BillRegId"] + "," + 0 + "," + argdtRQ.Rows[i]["QualifierId"] + 
                                            ",'" + argdtRQ.Rows[i]["Expression"] + "'," + argdtRQ.Rows[i]["ExpPer"] +
                                            "," + argdtRQ.Rows[i]["NetPer"] + ",'" + argdtRQ.Rows[i]["Add_Less_Flag"] + "'," + argdtRQ.Rows[i]["SurCharge"] + "," +
                                            " " + argdtRQ.Rows[i]["EDCess"] + "," + argdtRQ.Rows[i]["HEDPer"] + "," + argdtRQ.Rows[i]["ExpValue"] +
                                            "," + argdtRQ.Rows[i]["ExpPerValue"] + "," + argdtRQ.Rows[i]["SurValue"] + "," + argdtRQ.Rows[i]["EDValue"] +
                                            "," + argdtRQ.Rows[i]["Amount"] + "," + argdtRQ.Rows[i]["TaxablePer"] + "," + argdtRQ.Rows[i]["TaxableValue"] +
                                            "," + argdtRQ.Rows[i]["HEDValue"] + ")";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                            }
                        }
                    }

                    if (ReceiptDetailBO.PaymentAgainst == "S" || ReceiptDetailBO.PaymentAgainst == "B")
                    {
                        //Inserting Excess Amount for particular flat
                        if (ReceiptDetailBO.ExcessAmount > 0)
                        {
                            sSql = "INSERT INTO dbo.ExtraPayment(ReceiptId,FlatId,LeadId,Amount) Values " +
                                    " (" + iRecpId + "," + ReceiptDetailBO.FlatId + "," + ReceiptDetailBO.BuyerId + "," + ReceiptDetailBO.ExcessAmount + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }

                    UpdatePaidAmt(dtReceipt, iRecpId, argType, conn, tran);
                    tran.Commit();
                    bAns = true;

                    if (bAns == true)
                    {
                        BsfGlobal.InsertLog(DateTime.Now, "Buyer-Receipt-Add", "N", ReceiptDetailBO.Narration, iRecpId, ReceiptDetailBO.CostCentreId, 0,
                                            BsfGlobal.g_sCRMDBName, ReceiptDetailBO.ReceiptNo, BsfGlobal.g_lUserId, ReceiptDetailBO.Amount, 0);
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    bAns = false;
                    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static DataTable GetQualifierMaster(string argType)
        {
            DataTable dt = null;
            string sSql = "Select QualifierId,QualifierName from Qualifier_Temp Where QualType='" + argType + "'";
            BsfGlobal.OpenRateAnalDB();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_RateAnalDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                BsfGlobal.g_RateAnalDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static decimal GetReceiptAmount(int argRecId,int argPaySchId)
        {
            DataTable dt = null; decimal dAmt = 0;
            string sSql = "Select Amount From dbo.ReceiptTrans Where ReceiptId=" + argRecId + " And PaySchId=" + argPaySchId + "";
            BsfGlobal.OpenCRMDB();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    decimal dValue = Convert.ToDecimal(dt.Rows[i]["Amount"]);
                    dAmt = dAmt + dValue;
                }
                da.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dAmt;
        }

        public static void UpdateReceiptDetails(ReceiptDetailBO argReceiptContactBO, DataTable dtReceipt, string argType, DataTable dtPay, DataTable argdtRT, DataTable argdtRQ, DataTable dtQualAbs, bool argHiddenUpdate)
        {
            bool bAns = false; int iRecTransId = 0;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "UPDATE dbo.ReceiptRegister SET ReceiptDate='" + ReceiptDetailBO.ReceiptDate + "', ReceiptNo='" + ReceiptDetailBO.ReceiptNo + "'," +
                            " ChequeNo='" + ReceiptDetailBO.ChequeNo + "',ChequeDate='" + ReceiptDetailBO.ChequeDate + "',BankName='" + ReceiptDetailBO.BankName + "'," +
                            " CostCentreId=" + ReceiptDetailBO.CostCentreId + ",FlatId=" + ReceiptDetailBO.FlatId + ",LeadId=" + ReceiptDetailBO.BuyerId + "," +
                            " PaymentAgainst='" + ReceiptDetailBO.PaymentAgainst + "',Amount='" + ReceiptDetailBO.Amount + "',Status='" + ReceiptDetailBO.Status + "', " +
                            " Narration='" + ReceiptDetailBO.Narration + "', TenantId=" + ReceiptDetailBO.TenantId + ",PaymentMode ='" + ReceiptDetailBO.PaymentMode + "'," +
                            " BuyerAccountId=" + ReceiptDetailBO.BuyerAcctId + ",BillType='" + ReceiptDetailBO.BillType + "',Interest=" + ReceiptDetailBO.Interest + "" +
                            " Where ReceiptId=" + ReceiptDetailBO.ReceiptId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = String.Format("DELETE FROM dbo.ReceiptTrans WHERE ReceiptId={0}", ReceiptDetailBO.ReceiptId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = String.Format("DELETE FROM dbo.ReceiptQualifierAbs WHERE ReceiptId={0}", ReceiptDetailBO.ReceiptId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = String.Format("DELETE FROM dbo.ReceiptShTrans WHERE ReceiptId={0}", ReceiptDetailBO.ReceiptId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = String.Format("DELETE FROM dbo.ReceiptQualifier WHERE ReceiptId={0}", ReceiptDetailBO.ReceiptId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (dtReceipt.Rows.Count > 0)
                    {
                        for (int a = 0; a < dtReceipt.Rows.Count; a++)
                        {
                            sSql = "Delete FROM dbo.ReceiptExtraBillQualifier Where ReceiptId=" + ReceiptDetailBO.ReceiptId + " ";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            sSql = "Delete FROM dbo.ReceiptExtraBillQualifierAbs Where ReceiptId=" + ReceiptDetailBO.ReceiptId + " ";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            if (Convert.ToDecimal(CommFun.IsNullCheck(dtReceipt.Rows[a]["Amount"], CommFun.datatypes.vartypenumeric)) != 0)
                            {
                                if (dtReceipt.Rows[a]["ReceiptType"].ToString() == "ProgressBill")
                                {
                                    //sSql = String.Format("INSERT INTO ReceiptTrans(ReceiptId,ReceiptType,FlatId,BillRegId,Amount,CostCentreId,PaySchId) values({0},'{1}',{2},{3},'{4}',{5},{6}) SELECT SCOPE_IDENTITY()", ReceiptDetailBO.ReceiptId, dtReceipt.Rows[a]["ReceiptType"].ToString().Trim(), dtReceipt.Rows[a]["FlatId"], dtReceipt.Rows[a]["BillRegId"], dtReceipt.Rows[a]["Amount"], dtReceipt.Rows[a]["CostCentreId"], dtReceipt.Rows[a]["PaymentSchId"]);
                                    sSql = "INSERT INTO dbo.ReceiptTrans(ReceiptId,ReceiptType,FlatId,BillRegId,GrossAmount,NetAmount,PaidAmount,BalanceAmount,Amount,CostCentreId,PaySchId) values(" + ReceiptDetailBO.ReceiptId + "," +
                                           " '" + dtReceipt.Rows[a]["ReceiptType"].ToString().Trim() + "'," + dtReceipt.Rows[a]["FlatId"] + "," + dtReceipt.Rows[a]["BillRegId"] + "," + dtReceipt.Rows[a]["Gross"] + ", " +
                                           " " + dtReceipt.Rows[a]["NetAmount"] + "," + dtReceipt.Rows[a]["PaidAmount"] + "," + dtReceipt.Rows[a]["BalanceAmount"] + "," +
                                           " " + dtReceipt.Rows[a]["Amount"] + "," + dtReceipt.Rows[a]["CostCentreId"] + "," + dtReceipt.Rows[a]["PaymentSchId"] + ")SELECT SCOPE_IDENTITY()";
                                }
                                else if (dtReceipt.Rows[a]["ReceiptType"].ToString() == "ScheduleBill")
                                {
                                    sSql = "INSERT INTO dbo.ReceiptTrans(ReceiptId,ReceiptType,FlatId,GrossAmount,NetAmount,PaidAmount,BalanceAmount,Amount,CostCentreId,PaySchId) " +
                                            " values(" + ReceiptDetailBO.ReceiptId + "," +
                                            " '" + dtReceipt.Rows[a]["ReceiptType"].ToString().Trim() + "'," + dtReceipt.Rows[a]["FlatId"] + "," + dtReceipt.Rows[a]["Gross"] + ", " +
                                            " " + dtReceipt.Rows[a]["NetAmount"] + "," + dtReceipt.Rows[a]["PaidAmount"] + "," + dtReceipt.Rows[a]["BalanceAmount"] + "," +
                                            " " + dtReceipt.Rows[a]["Amount"] + "," + dtReceipt.Rows[a]["CostCentreId"] + "," + dtReceipt.Rows[a]["PaymentSchId"] +
                                            ")SELECT SCOPE_IDENTITY()";
                                }
                                else if (dtReceipt.Rows[a]["ReceiptType"].ToString() == "ExtraBill")
                                {
                                    decimal dNetAmt = Convert.ToDecimal(dtReceipt.Rows[a]["NetAmount"]);
                                    decimal dPaidAmt = Convert.ToDecimal(dtReceipt.Rows[a]["PaidAmount"]);
                                    decimal dCurrentAmt = Convert.ToDecimal(dtReceipt.Rows[a]["Amount"]);
                                    decimal dBalance = Convert.ToDecimal(dtReceipt.Rows[a]["BalanceAmount"]);

                                    sSql = "INSERT INTO dbo.ReceiptTrans(ReceiptId,ReceiptType,FlatId,BillRegId,GrossAmount,NetAmount,PaidAmount,BalanceAmount, " +
                                            " Amount,CostCentreId,PaySchId) values(" + ReceiptDetailBO.ReceiptId + ",'" + dtReceipt.Rows[a]["ReceiptType"].ToString().Trim() +
                                            "'," + dtReceipt.Rows[a]["FlatId"] + "," + dtReceipt.Rows[a]["BillRegId"] + "," + dtReceipt.Rows[a]["Gross"] +
                                            "," + dNetAmt + "," + dCurrentAmt + "," + dBalance + "," + dCurrentAmt + 
                                            "," + dtReceipt.Rows[a]["CostCentreId"] + ",0) SELECT SCOPE_IDENTITY();";
                                }
                                else if (ReceiptDetailBO.PaymentAgainst == "R")
                                {
                                    sSql = String.Format("INSERT INTO dbo.ReceiptTrans(ReceiptId,ReceiptType,FlatId,Amount,CostCentreId,BillRegId) values({0},'{1}',{2},'{3}',{4},{5})SELECT SCOPE_IDENTITY()", ReceiptDetailBO.ReceiptId, dtReceipt.Rows[a]["ReceiptType"].ToString().Trim(), dtReceipt.Rows[a]["FlatId"], dtReceipt.Rows[a]["Amount"], dtReceipt.Rows[a]["CostCentreId"], dtReceipt.Rows[a]["TransId"]);
                                }
                                else if (ReceiptDetailBO.PaymentAgainst == "A")
                                {
                                    sSql = String.Format("INSERT INTO dbo.ReceiptTrans(ReceiptId,FlatId,Amount,CostCentreId,ReceiptType,PaySchId) values({0},{1},'{2}',{3},'{4}',{5})SELECT SCOPE_IDENTITY()", ReceiptDetailBO.ReceiptId, dtReceipt.Rows[a]["FlatId"], dtReceipt.Rows[a]["Amount"], dtReceipt.Rows[a]["CostCentreId"], dtReceipt.Rows[a]["ReceiptType"].ToString().Trim(), dtReceipt.Rows[a]["PaymentSchId"]);
                                }
                                else
                                {
                                    sSql = String.Format("INSERT INTO dbo.ReceiptTrans(ReceiptId,FlatId,Amount,CostCentreId,ReceiptType) values({0},{1},'{2}',{3},'{4}')SELECT SCOPE_IDENTITY()", ReceiptDetailBO.ReceiptId, dtReceipt.Rows[a]["FlatId"], dtReceipt.Rows[a]["Amount"], dtReceipt.Rows[a]["CostCentreId"], dtReceipt.Rows[a]["ReceiptType"].ToString().Trim());
                                }

                                cmd = new SqlCommand(sSql, conn, tran);
                                iRecTransId = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                                cmd.Dispose();

                                if (ReceiptDetailBO.PaymentAgainst == "B" || ReceiptDetailBO.PaymentAgainst == "PB" || ReceiptDetailBO.PaymentAgainst == "S")
                                {
                                    DataView dv = new DataView(dtQualAbs);

                                    if (dv.ToTable().Rows.Count > 0)
                                    {
                                        dv.RowFilter = "FlatId = " + dtReceipt.Rows[a]["FlatId"] + " and PaymentSchId = " + dtReceipt.Rows[a]["PaymentSchId"];
                                        DataTable dt = new DataTable();
                                        if (dv.ToTable().Rows.Count > 0) { dt = dv.ToTable(); }
                                        dv.Dispose();

                                        for (int j = 0; j < dt.Rows.Count; j++)
                                        {
                                            if (Convert.ToDecimal(dt.Rows[j]["Amount"]) > 0)
                                            {
                                                if (ReceiptDetailBO.PaymentAgainst == "S")
                                                {
                                                    sSql = "Insert Into dbo.ReceiptQualifierAbs(QualifierId,Add_Less_Flag,ReceiptId,ReceiptTransId,PBillId,PaymentSchId,AccountId,Amount) " +
                                                           "Values(" + Convert.ToInt32(dt.Rows[j]["QualifierId"]) + ",'" + dt.Rows[j]["Add_Less_Flag"].ToString() + "'," + ReceiptDetailBO.ReceiptId + "," + iRecTransId + "," +
                                                           "0," + dtReceipt.Rows[a]["PaymentSchId"] + "," + Convert.ToInt32(dt.Rows[j]["AccountId"]) + ", " + Convert.ToDecimal(dt.Rows[j]["Amount"]) + ")";
                                                }
                                                else if (ReceiptDetailBO.PaymentAgainst == "B" || ReceiptDetailBO.PaymentAgainst == "PB")
                                                {
                                                    sSql = "Insert Into dbo.ReceiptQualifierAbs(QualifierId,Add_Less_Flag,ReceiptId,ReceiptTransId,PBillId,PaymentSchId,AccountId,Amount) " +
                                                            "Values(" + Convert.ToInt32(dt.Rows[j]["QualifierId"]) + ",'" + dt.Rows[j]["Add_Less_Flag"].ToString() + "'," + ReceiptDetailBO.ReceiptId + "," + iRecTransId + "," +
                                                            "" + dtReceipt.Rows[a]["BillRegId"] + "," + dtReceipt.Rows[a]["PaymentSchId"] + "," + Convert.ToInt32(dt.Rows[j]["AccountId"]) + ", " + Convert.ToDecimal(dt.Rows[j]["Amount"]) + ")";
                                                }
                                                cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();
                                            }
                                        }

                                    }
                                }
                                else if (ReceiptDetailBO.PaymentAgainst == "E")
                                {
                                    for (int j = 0; j < dtQualAbs.Rows.Count; j++)
                                    {
                                        if (Convert.ToDecimal(dtQualAbs.Rows[j]["Amount"]) > 0)
                                        {
                                            sSql = "Insert Into dbo.ReceiptExtraBillQualifierAbs(QualifierId,Add_Less_Flag,ReceiptId,ReceiptTransId,BillRegId,Amount) " +
                                                    "Values(" + Convert.ToInt32(dtQualAbs.Rows[j]["QualifierId"]) + ",'" + dtQualAbs.Rows[j]["Add_Less_Flag"].ToString() +
                                                    "'," + ReceiptDetailBO.ReceiptId + "," + iRecTransId + "," + dtQualAbs.Rows[j]["BillRegId"] +
                                                    "," + Convert.ToDecimal(dtQualAbs.Rows[j]["Amount"]) + ")";
                                            cmd = new SqlCommand(sSql, conn, tran);
                                            cmd.ExecuteNonQuery();
                                            cmd.Dispose();
                                        }
                                    }
                                }

                            }

                            if (ReceiptDetailBO.PaymentAgainst == "S" || ReceiptDetailBO.PaymentAgainst == "B")
                            {
                                DataView dv = new DataView(argdtRT);

                                if (dv.ToTable().Rows.Count > 0)
                                {
                                    dv.RowFilter = "FlatId = " + dtReceipt.Rows[a]["FlatId"] + " and PaymentSchId = " + dtReceipt.Rows[a]["PaymentSchId"];
                                    DataTable dtRT = new DataTable();
                                    if (dv.ToTable().Rows.Count > 0) { dtRT = dv.ToTable(); }
                                    dv.Dispose();

                                    for (int i = 0; i < dtRT.Rows.Count; i++)
                                    {
                                        sSql = "INSERT INTO dbo.ReceiptShTrans (ReceiptId,FlatId,PaymentSchId,ReceiptTypeId,OtherCostId,GrossAmount,TaxAmount,NetAmount," +
                                                " PaidGrossAmount,PaidTaxAmount,PaidNetAmount,PaidAmount) Values (" + ReceiptDetailBO.ReceiptId + "," + dtRT.Rows[i]["FlatId"] + "," + dtRT.Rows[i]["PaymentSchId"] + "," +
                                                " " + dtRT.Rows[i]["ReceiptTypeId"] + "," + dtRT.Rows[i]["OtherCostId"] + "," + CommFun.IsNullCheck(dtRT.Rows[i]["GrossAmount"], CommFun.datatypes.vartypenumeric) + "," +
                                                " " + CommFun.IsNullCheck(dtRT.Rows[i]["TaxAmount"], CommFun.datatypes.vartypenumeric) + "," + CommFun.IsNullCheck(dtRT.Rows[i]["NetAmount"], CommFun.datatypes.vartypenumeric) + "," + CommFun.IsNullCheck(dtRT.Rows[i]["PaidGrossAmount"], CommFun.datatypes.vartypenumeric) + "" +
                                                " ," + CommFun.IsNullCheck(dtRT.Rows[i]["PaidTaxAmount"], CommFun.datatypes.vartypenumeric) + 
                                                "," + CommFun.IsNullCheck(dtRT.Rows[i]["PaidNetAmount"], CommFun.datatypes.vartypenumeric) + ""+
                                                "," + CommFun.IsNullCheck(dtRT.Rows[i]["PaidAmount"], CommFun.datatypes.vartypenumeric) + ")";
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                }

                                dv = new DataView(argdtRQ);

                                if (dv.ToTable().Rows.Count > 0)
                                {
                                    dv.RowFilter = "FlatId = " + dtReceipt.Rows[a]["FlatId"] + " and PaymentSchId = " + dtReceipt.Rows[a]["PaymentSchId"];
                                    DataTable dtRQ = new DataTable();
                                    if (dv.ToTable().Rows.Count > 0) { dtRQ = dv.ToTable(); }
                                    dv.Dispose();
                                    for (int i = 0; i < dtRQ.Rows.Count; i++)
                                    {
                                        sSql = "INSERT INTO dbo.ReceiptQualifier (ReceiptId,FlatId,PaymentSchId,ReceiptTypeId,OtherCostId,QualifierId,Expression,ExpPer,NetPer,Add_Less_Flag,SurCharge," +
                                                " EDCess,HEDPer,ExpValue,ExpPerValue,SurValue,EDValue,Amount,TaxablePer,TaxableValue) Values (" + ReceiptDetailBO.ReceiptId + "," + dtRQ.Rows[i]["FlatId"] + "," + dtRQ.Rows[i]["PaymentSchId"] + "," +
                                               " " + dtRQ.Rows[i]["ReceiptTypeId"] + "," + dtRQ.Rows[i]["OtherCostId"] + "," + dtRQ.Rows[i]["QualifierId"] + ",'" + dtRQ.Rows[i]["Expression"] + "'," +
                                                " " + dtRQ.Rows[i]["ExpPer"] + "," + dtRQ.Rows[i]["NetPer"] + ",'" + dtRQ.Rows[i]["Add_Less_Flag"] + "'," + dtRQ.Rows[i]["SurCharge"] + "," +
                                                " " + dtRQ.Rows[i]["EDCess"] + "," + dtRQ.Rows[i]["HEDPer"] + "," + dtRQ.Rows[i]["ExpValue"] + "," + dtRQ.Rows[i]["ExpPerValue"] + "," +
                                                " " + dtRQ.Rows[i]["SurValue"] + "," + dtRQ.Rows[i]["EDValue"] + "," + dtRQ.Rows[i]["Amount"] + "," + dtRQ.Rows[i]["TaxablePer"] + "," + dtRQ.Rows[i]["TaxableValue"] + ")";
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                }
                            }
                            else if (ReceiptDetailBO.PaymentAgainst == "E")
                            {
                                for (int i = 0; i < argdtRQ.Rows.Count; i++)
                                {
                                    sSql = "INSERT INTO dbo.ReceiptExtraBillQualifier(ReceiptId,FlatId,BillRegId,AccountId, " +
                                            " QualifierId,Expression,ExpPer,NetPer,Add_Less_Flag,SurCharge,EDCess,HEDPer,ExpValue,ExpPerValue,SurValue," +
                                            " EDValue,Amount,TaxablePer,TaxableValue,HEDValue) Values (" + ReceiptDetailBO.ReceiptId + "," + argdtRQ.Rows[i]["FlatId"] +
                                            "," + argdtRQ.Rows[i]["BillRegId"] + "," + 0 + "," + argdtRQ.Rows[i]["QualifierId"] +
                                            ",'" + argdtRQ.Rows[i]["Expression"] + "'," + argdtRQ.Rows[i]["ExpPer"] +
                                            "," + argdtRQ.Rows[i]["NetPer"] + ",'" + argdtRQ.Rows[i]["Add_Less_Flag"] + "'," + argdtRQ.Rows[i]["SurCharge"] + "," +
                                            " " + argdtRQ.Rows[i]["EDCess"] + "," + argdtRQ.Rows[i]["HEDPer"] + "," + argdtRQ.Rows[i]["ExpValue"] +
                                            "," + argdtRQ.Rows[i]["ExpPerValue"] + "," + argdtRQ.Rows[i]["SurValue"] + "," + argdtRQ.Rows[i]["EDValue"] +
                                            "," + argdtRQ.Rows[i]["Amount"] + "," + argdtRQ.Rows[i]["TaxablePer"] + "," + argdtRQ.Rows[i]["TaxableValue"] +
                                            "," + argdtRQ.Rows[i]["HEDValue"] + ")";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                            }
                        }
                    }

                    //Deleting Excess Amount for particular flat
                    sSql = "Delete From dbo.ExtraPayment Where ReceiptId=" + ReceiptDetailBO.ReceiptId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Inserting Excess Amount for particular flat
                    if (ReceiptDetailBO.ExcessAmount > 0)
                    {
                        sSql = "INSERT INTO dbo.ExtraPayment (ReceiptId,FlatId,LeadId,Amount) Values " +
                                " (" + ReceiptDetailBO.ReceiptId + "," + ReceiptDetailBO.FlatId + "," + ReceiptDetailBO.BuyerId + "," + ReceiptDetailBO.ExcessAmount + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    tran.Commit();

                    UpdateEditPaidAmt(dtReceipt, ReceiptDetailBO.ReceiptId, argType);
                    bAns = true;
                    if (bAns == true && argHiddenUpdate == false)
                    {
                        BsfGlobal.InsertLog(DateTime.Now, "Buyer-Receipt-Edit", "E", ReceiptDetailBO.Narration, ReceiptDetailBO.ReceiptId, ReceiptDetailBO.CostCentreId, 0,
                                            BsfGlobal.g_sCRMDBName, ReceiptDetailBO.ReceiptNo, BsfGlobal.g_lUserId, ReceiptDetailBO.Amount, 0);
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    bAns = false;
                    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        private static void UpdatePaidAmt(DataTable dt, int argRecptId, string argType, SqlConnection argConn, SqlTransaction argTran)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    for (int a = 0; a < dt.Rows.Count; a++)
                    {
                        decimal dCurAmt = 0;
                        if (dt.Rows[a]["ReceiptType"].ToString().Trim() == "ProgressBill" || dt.Rows[a]["ReceiptType"].ToString().Trim() == "ScheduleBill" || dt.Rows[a]["ReceiptType"].ToString().Trim() == "PlotProgressBill" || dt.Rows[a]["ReceiptType"].ToString().Trim() == "ExtraBill" || dt.Rows[a]["ReceiptType"].ToString().Trim() == "PlotScheduleBill")
                        {
                            string sSql = "";
                            if (dt.Rows[a]["ReceiptType"].ToString().Trim() == "ExtraBill")
                                sSql = "Select Sum(Amount) From dbo.ReceiptTrans Where BillRegId=" + dt.Rows[a]["BillRegId"] + "";
                            else if (ReceiptDetailBO.PaymentAgainst == "PB" && dt.Rows[a]["ReceiptType"].ToString().Trim() == "PlotProgressBill")
                                sSql = "Select Sum(Amount) From dbo.ReceiptTrans Where BillRegId=" + dt.Rows[a]["BillRegId"] + "";
                            else if (ReceiptDetailBO.PaymentAgainst == "PB" && dt.Rows[a]["ReceiptType"].ToString().Trim() == "PlotScheduleBill")
                                sSql = "Select Sum(Amount) From dbo.ReceiptTrans Where BillRegId=" + dt.Rows[a]["FlatId"] + "";
                            else
                                sSql = "Select Sum(Amount) From dbo.ReceiptTrans Where PaySchId=" + dt.Rows[a]["PaymentSchId"] + "";

                            SqlCommand cmd = new SqlCommand(sSql, argConn, argTran);
                            dCurAmt = Convert.ToDecimal(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                            cmd.Dispose();
                        }
                        else if (dt.Rows[a]["ReceiptType"].ToString().Trim() == "ScheduleBill" || dt.Rows[a]["ReceiptType"].ToString().Trim() == "Advance")
                        {
                            string sSql = "Select Sum(Amount) From dbo.ReceiptTrans Where ReceiptType In('ScheduleBill','Advance') And PaySchId=" + dt.Rows[a]["PaymentSchId"].ToString() + "";
                            SqlCommand cmd = new SqlCommand(sSql, argConn, argTran);
                            dCurAmt = Convert.ToDecimal(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                            cmd.Dispose();
                        }

                        if (dt.Rows[a]["ReceiptType"].ToString().Trim() == "ProgressBill" || dt.Rows[a]["ReceiptType"].ToString().Trim() == "PlotProgressBill")
                        {
                            string sSql = "";
                            if (Convert.ToInt32(dt.Rows[a]["BillRegId"]) == 0)
                            {
                                sSql = "Update dbo.PaymentScheduleFlat Set PaidAmount= " + dCurAmt + " Where PaymentSchId= " + dt.Rows[a]["PaymentSchId"].ToString() + " ";
                            }
                            else
                            {
                                if (argType == "B")
                                    sSql = "Update ProgressBillRegister set PaidAmount= " + dCurAmt + " Where PBillId= " + dt.Rows[a]["BillRegId"].ToString() + " ";
                                else if (argType == "L")
                                    sSql = "Update PlotProgressBillRegister set PaidAmount= " + dCurAmt + " Where PBillId= " + dt.Rows[a]["BillRegId"].ToString() + " ";
                            }
                            SqlCommand cmd = new SqlCommand(sSql, argConn, argTran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            if (argType != "L")
                            {
                                sSql = "Update dbo.PaymentScheduleFlat Set PaidAmount= " + dCurAmt + " Where PaymentSchId= " + dt.Rows[a]["PaymentSchId"].ToString() + " ";
                                cmd = new SqlCommand(sSql, argConn, argTran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }
                        if (dt.Rows[a]["ReceiptType"].ToString().Trim() == "ScheduleBill" || dt.Rows[a]["ReceiptType"].ToString().Trim() == "Advance")
                        {
                            string sSql = "Update dbo.PaymentScheduleFlat Set PaidAmount= " + dCurAmt + " Where PaymentSchId= " + dt.Rows[a]["PaymentSchId"].ToString() + " ";
                            SqlCommand cmd = new SqlCommand(sSql, argConn, argTran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                        if (dt.Rows[a]["ReceiptType"].ToString().Trim() == "ExtraBill")
                        {
                            string sSql = "Select TOP 1 NetAmount From dbo.ReceiptTrans Where BillRegId=" + dt.Rows[a]["BillRegId"] + "";
                            SqlCommand cmd = new SqlCommand(sSql, argConn, argTran);
                            decimal dNetAmt = Convert.ToDecimal(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                            cmd.Dispose();

                            sSql = "Update ExtraBillRegister set PaidAmount= " + dCurAmt + ", NetAmount=" + dNetAmt + 
                                    " Where BillRegId= " + dt.Rows[a]["BillRegId"].ToString() + " ";
                            cmd = new SqlCommand(sSql, argConn, argTran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }

                        if (dt.Rows[a]["ReceiptType"].ToString().Trim() == "ServiceBill")
                        {
                            string sSql = "Update SerOrderBillReg set PaidAmount= " + dCurAmt + " Where RegBillId= " + dt.Rows[a]["BillRegId"].ToString() + " ";
                            SqlCommand cmd = new SqlCommand(sSql, argConn, argTran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                        // Rent paid amt                       

                        if (dt.Rows[a]["ReceiptType"].ToString().Trim() == "RentBill")
                        {
                            string sSql = "Update RentSchTrans set PaidAmount= " + dt.Rows[a]["Amount"].ToString() + " Where RentTransId=" + dt.Rows[a]["RentId"].ToString() + " and TransId=" + dt.Rows[a]["TransId"].ToString() + " ";
                            SqlCommand cmd = new SqlCommand(sSql, argConn, argTran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        private static void UpdateEditPaidAmt(DataTable dt, int argRecptId, string argType)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            decimal dCurAmt = 0;
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int a = 0; a < dt.Rows.Count; a++)
                        {
                            if (dt.Rows[a]["ReceiptType"].ToString().Trim() == "ProgressBill" || dt.Rows[a]["ReceiptType"].ToString().Trim() == "PlotProgressBill" || dt.Rows[a]["ReceiptType"].ToString().Trim() == "ExtraBill" || dt.Rows[a]["ReceiptType"].ToString().Trim() == "ServiceBill")
                            {
                                if (dt.Rows[a]["ReceiptType"].ToString() == "ExtraBill")
                                    sSql = "Select Sum(Amount) From ReceiptTrans Where BillRegId=" + dt.Rows[a]["BillRegId"] + " ";
                                else
                                    sSql = "Select Sum(Amount) From ReceiptTrans Where PaySchId=" + dt.Rows[a]["PaymentSchId"] + " And ReceiptType='" + dt.Rows[a]["ReceiptType"].ToString() + "' ";
                                cmd = new SqlCommand(sSql, conn, tran);
                                dCurAmt = Convert.ToDecimal(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                                cmd.Dispose();
                            }
                            else if (dt.Rows[a]["ReceiptType"].ToString().Trim() == "ScheduleBill" || dt.Rows[a]["ReceiptType"].ToString().Trim() == "Advance")
                            {
                                sSql = "Select Sum(Amount) from ReceiptTrans Where ReceiptType In('ScheduleBill','Advance') And PaySchId=" + dt.Rows[a]["PaymentSchId"].ToString() + "";
                                cmd = new SqlCommand(sSql, conn, tran);
                                dCurAmt = Convert.ToDecimal(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                                cmd.Dispose();
                            }

                            if (dt.Rows[a]["ReceiptType"].ToString().Trim() == "ProgressBill" || dt.Rows[a]["ReceiptType"].ToString().Trim() == "PlotProgressBill")
                            {
                                if (Convert.ToInt32(dt.Rows[a]["BillRegId"]) == 0)
                                { sSql = "Update dbo.PaymentScheduleFlat Set PaidAmount= " + dCurAmt + " Where PaymentSchId= " + dt.Rows[a]["PaymentSchId"].ToString() + " "; }
                                else
                                {
                                    if (argType == "B")
                                        sSql = "Update ProgressBillRegister set PaidAmount= " + dCurAmt + " Where PBillId= " + dt.Rows[a]["BillRegId"].ToString() + " ";
                                    else if (argType == "L")
                                        sSql = "Update PlotProgressBillRegister set PaidAmount= " + dCurAmt + " Where PBillId= " + dt.Rows[a]["BillRegId"].ToString() + " ";
                                }
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                            if (dt.Rows[a]["ReceiptType"].ToString().Trim() == "ScheduleBill" || dt.Rows[a]["ReceiptType"].ToString().Trim() == "Advance")
                            {
                                sSql = "Update dbo.PaymentScheduleFlat Set PaidAmount=" + dCurAmt + " Where PaymentSchId= " + dt.Rows[a]["PaymentSchId"].ToString() + " ";
                                //sSql = "UPDATE PaymentScheduleFlat SET PaidAmount=PaidAmount-SummedQty FROM " +
                                //        " PaymentScheduleFlat SLA JOIN (SELECT PaySchId, Sum(Amount) SummedQty FROM ReceiptTrans WHERE ReceiptId=" + argRecptId + " GROUP BY PaySchId " +
                                //        " ) CCA ON SLA.PaymentSchId=CCA.PaySchId";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                            if (dt.Rows[a]["ReceiptType"].ToString().Trim() == "ExtraBill")
                            {
                                sSql = "Select TOP 1 NetAmount From dbo.ReceiptTrans Where BillRegId=" + dt.Rows[a]["BillRegId"] + "";
                                cmd = new SqlCommand(sSql, conn, tran);
                                decimal dNetAmt = Convert.ToDecimal(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                                cmd.Dispose();

                                sSql = "Update ExtraBillRegister set PaidAmount= " + dCurAmt + ", NetAmount=" + dNetAmt + " "+
                                        " Where BillRegId= " + dt.Rows[a]["BillRegId"].ToString() + " ";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }

                            if (dt.Rows[a]["ReceiptType"].ToString().Trim() == "ServiceBill")
                            {
                                sSql = "Update SerOrderBillReg set PaidAmount= " + dCurAmt + " Where RegBillId= " + dt.Rows[a]["BillRegId"].ToString() + " ";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                            // Rent paid amt                       

                            if (dt.Rows[a]["ReceiptType"].ToString().Trim() == "RentBill")
                            {
                                sSql = "Update RentSchTrans set PaidAmount= " + dt.Rows[a]["Amount"].ToString() + " Where RentTransId=" + dt.Rows[a]["RentId"].ToString() + " and TransId=" + dt.Rows[a]["TransId"].ToString() + " ";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }
                        tran.Commit();
                    }
                }
                catch (SqlException ex)
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

        public static String GetApprove(int argRepId)
        {
            string sAppr = "";
            try
            {
                string sSql = "Select A.Approve From ReceiptRegister A " +
                    " Where A.ReceiptId=" + argRepId + " ";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0) { sAppr = dt.Rows[0]["Approve"].ToString(); }
                da.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return sAppr;
        }

        public static bool Check_ReceiptDet(int argRepId)
        {
            bool bAns = false;
            try
            {
                string sSql = "SELECT PaymentSchId From PaymentScheduleFlat " +
                    " Where BillPassed=1 And PaymentSchId In( Select PaySchId From ReceiptSurplusDet Where ReceiptId=" + argRepId + ") ";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0) { bAns = true; }
                da.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return bAns;
        }

        public static void DeleteReceiptDetails(int argRecpId,string argType)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    string sSql = "";
                    bool bAns = false;

                    if (argType == "Others" || argType == "Bill")
                        sSql = "Update dbo.ProgressBillRegister Set PaidAmount=dbo.ProgressBillRegister.PaidAmount-RT.Amount From dbo.ReceiptTrans RT " +
                               " Where RT.BillRegId=dbo.ProgressBillRegister.PBillId And RT.ReceiptId=" + argRecpId + "";
                    else if (argType == "Rent")
                        sSql = "Update dbo.RentSchTrans Set PaidAmount=RentSchTrans.PaidAmount-RT.Amount From dbo.ReceiptTrans RT" +
                               " Where RT.BillRegId=dbo.RentSchTrans.TransId And RT.ReceiptId=" + argRecpId + " ";
                    else if (argType == "ScheduleBill" || argType == "Advance")
                        sSql = "Update dbo.PaymentScheduleFlat Set PaidAmount=PaymentScheduleFlat.PaidAmount-RT.Amount From dbo.ReceiptTrans RT" +
                               " Where RT.PaySchId=dbo.PaymentScheduleFlat.PaymentSchId And RT.ReceiptId=" + argRecpId + " ";
                    else if (argType == "ExtraBill")
                        sSql = "Update dbo.ExtraBillRegister Set PaidAmount=RT.PaidAmount-RT.Amount From dbo.ReceiptTrans RT" +
                               " Where RT.BillRegId=dbo.ExtraBillRegister.BillRegId And RT.ReceiptId=" + argRecpId + " ";
                    else
                        sSql = "Update dbo.PlotProgressBillRegister Set PaidAmount=PlotProgressBillRegister.PaidAmount-RT.Amount From dbo.ReceiptTrans RT " +
                               " Where RT.BillRegId=dbo.PlotProgressBillRegister.PBillId And RT.ReceiptId=" + argRecpId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (argType == "Bill")
                    {
                        sSql = "Update dbo.PaymentScheduleFlat Set SurplusAmount=0,PaidAmount=0 Where PaymentSchId In( " +
                                " Select PaySchId From ReceiptSurplusDet Where ReceiptId=" + argRecpId + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Update dbo.PaymentScheduleFlat Set PaidAmount=PaymentScheduleFlat.PaidAmount-RT.Amount From ReceiptTrans RT" +
                              " Where RT.PaySchId=PaymentScheduleFlat.PaymentSchId And RT.ReceiptId=" + argRecpId + " ";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    sSql = "DELETE FROM dbo.ReceiptSurplusDet Where ReceiptId=" + argRecpId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "DELETE FROM dbo.ReceiptRegister Where ReceiptId=" + argRecpId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "DELETE FROM dbo.ReceiptTrans Where ReceiptId=" + argRecpId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "DELETE FROM dbo.ReceiptShTrans Where ReceiptId=" + argRecpId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "DELETE FROM dbo.ReceiptQualifier Where ReceiptId=" + argRecpId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "DELETE FROM dbo.ReceiptQualifierAbs Where ReceiptId=" + argRecpId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "DELETE FROM dbo.ReceiptExtraBillQualifier Where ReceiptId=" + argRecpId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "DELETE FROM dbo.ReceiptExtraBillQualifierAbs Where ReceiptId=" + argRecpId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Update < 0 Value
                    #region Update<0 Value

                    sSql = "Update dbo.ProgressBillRegister Set PaidAmount=0 Where PaidAmount<0";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Update dbo.RentSchTrans Set PaidAmount=0 Where PaidAmount<0";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Update dbo.PlotProgressBillRegister Set PaidAmount=0 Where PaidAmount<0";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Update dbo.PaymentScheduleFlat Set PaidAmount=0 Where PaidAmount<0";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Update dbo.ExtraBillRegister Set PaidAmount=0 Where PaidAmount<0";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    #endregion

                    tran.Commit();
                    bAns = true;

                    if (bAns == true)
                    {
                        BsfGlobal.InsertLog(DateTime.Now, "Buyer-Receipt-Delete", "D", "Delete Receipt Register", argRecpId, ReceiptDetailBO.CostCentreId, 0, BsfGlobal.g_sCRMDBName, ReceiptDetailBO.ReceiptNo, BsfGlobal.g_lUserId, ReceiptDetailBO.Amount, 0);
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

        //public static decimal GetNetAmt(int argRepId)
        //{
        //    string sAppr = "";
        //    string sSql = "Select A.Approve From ReceiptRegister A " +
        //        " Where A.ReceiptId=" + argRepId + " ";
        //    BsfGlobal.OpenCRMDB();
        //    SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    if (dt.Rows.Count > 0) { sAppr = dt.Rows[0]["Approve"].ToString(); }
        //    da.Dispose();
        //    BsfGlobal.g_CRMDB.Close();
        //    return sAppr;
        //}

        public static DataTable GetPayInfo(int argBuyerId)
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.PaymentSchId PaySchId,SchDate,B.FlatNo UnitNo,[Description],NetAmount,SurplusAmount,SurplusAmount HAmount From dbo.PaymentScheduleFlat A " +
                        " Inner Join dbo.FlatDetails B On A.FlatId=B.FlatId " +
                        " Where BillPassed=0 And PaidAmount=0 And LeadId=" + argBuyerId + " Order By A.SchDate";
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

        public static DataTable GetEditPayInfo(int argBuyerId)
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.PaymentSchId PaySchId,SchDate,B.FlatNo UnitNo,[Description],NetAmount, " +
                        " SurplusAmount,0 HAmount From dbo.PaymentScheduleFlat A " +
                        " Inner Join dbo.FlatDetails B On A.FlatId=B.FlatId  " +
                        " Where BillPassed=0 And SurplusAmount=0 And " +
                        " PaidAmount=0 And LeadId=" + argBuyerId + " " +
                        " UNION ALL " +
                        " Select A.PaymentSchId PaySchId,SchDate,B.FlatNo UnitNo,[Description],NetAmount, " +
                        " C.Amount SurplusAmount,IsNull(C.Amount,0) HAmount From dbo.PaymentScheduleFlat A " +
                        " Inner Join dbo.FlatDetails B On A.FlatId=B.FlatId  " +
                        " INNER Join ReceiptSurplusDet C On C.PaySchId=A.PaymentSchId Where " +
                        " PaidAmount=0 And LeadId=" + argBuyerId + " Order By A.SchDate ";
                //sSql = "Select A.PaymentSchId PaySchId,SchDate,B.FlatNo UnitNo,[Description],NetAmount,  " +
                //    " C.Amount SurplusAmount,IsNull(C.Amount,0) HAmount From dbo.PaymentScheduleFlat A  " +
                //    " Inner Join dbo.FlatDetails B On A.FlatId=B.FlatId Left Join ReceiptSurplusDet C " +
                //    " On C.PaySchId=A.PaymentSchId Where PaidAmount=0 And LeadId=" + argBuyerId + " Order By A.SchDate ";
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

        public static DataTable GetReceiptRegister()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Update dbo.ReceiptRegister Set PaymentAgainst='B',BillType='B' Where PaymentAgainst='S'";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "Update dbo.ReceiptRegister Set Approve='N' Where Approve=''";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "Update ReceiptTrans Set NetAmount=Isnull((Select IsNull(NetAmount,0) From dbo.PaymentScheduleFlat Where PaymentSchId=ReceiptTrans.PaySchId),0) "+
                       " Where NetAmount=0";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();                
                
                if (BsfGlobal.g_bFADB == true)
                {
                    sSql = "DELETE FROM [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister WHERE EntryId=0 AND " +
                           " ReceiptId NOT IN(SELECT MIN(ReceiptId)FROM [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister GROUP BY ReceiptNo,ReferenceId,ChequeNo,CostCentreId)";
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                if (BsfGlobal.g_bFADB == true)
                {
                    //sSql = "Select A.CostCentreId,A.ReceiptId,A.ReceiptDate,A.ReceiptNo,C.LeadName As BuyerName, " +
                    //        " Case When F.FlatNo IS NULL Then PL.PlotNo Else F.FlatNo End FlatNo, B.CostCentreName,A.Amount, " +
                    //        " CASE (A.PaymentAgainst) When 'A' Then 'Advance' When 'B' Then 'Bill' When 'E' Then 'ExtraBill' When 'R' Then 'Rent' When 'O' Then 'Others'  " +
                    //        " When 'PA' Then 'PlotAdvance' When 'PB' Then 'PlotBill' When 'PO' Then 'PlotOthers' END AS PaymentAgainst,A.Narration,A.Approve, " +
                    //        " Case When D.BRS IS NULL Then 'N' When D.BRS=1 Then 'Y' Else 'N' End ChequeDisbursement From dbo.ReceiptRegister A WITH(READPAST)  " +
                    //        " Left Join dbo.FlatDetails F WITH(READPAST) On F.FlatId=A.FlatId " +
                    //        " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails PL WITH(READPAST) On A.FlatId=PL.PlotDetailsId " +
                    //        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B WITH(READPAST) on A.CostCentreId=B.CostCentreId  " +
                    //        " Left Join dbo.LeadRegister C WITH(READPAST) on A.LeadId=C.LeadId " +
                    //        " Left Join [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister D WITH(READPAST) ON A.ReceiptId=D.ReferenceId AND D.EntryId<>0 " +
                    //        " Where B.CostCentreId NOT IN(Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                    //        " Group by A.CostCentreId,A.ReceiptId,A.ReceiptDate,A.ReceiptNo,B.CostCentreName,C.LeadName,F.FlatNo,PL.PlotNo, " +
                    //        " PaymentAgainst,A.Amount,A.Approve,A.Narration,D.BRS " +
                    //        " Order by A.ReceiptDate,A.ReceiptNo,LeadName,CostCentreName";

                    sSql = "Select A.CostCentreId,A.ReceiptId,A.ReceiptDate,A.ReceiptNo,C.LeadName As BuyerName, " +
                            " F.FlatNo, B.CostCentreName,A.Amount, " +
                            " CASE (A.PaymentAgainst) When 'A' Then 'Advance' When 'B' Then 'Bill' When 'E' Then 'ExtraBill' When 'R' Then 'Rent' When 'O' Then 'Others'  " +
                            " When 'PA' Then 'PlotAdvance' When 'PB' Then 'PlotBill' When 'PO' Then 'PlotOthers' END AS PaymentAgainst,A.Narration,A.Approve, " +
                            " Case When D.BRS IS NULL Then 'N' When D.BRS=1 Then 'Y' Else 'N' End ChequeDisbursement From dbo.ReceiptRegister A WITH(READPAST)  " +
                            " Left Join dbo.FlatDetails F WITH(READPAST) On F.FlatId=A.FlatId " +
                            " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B WITH(READPAST) on A.CostCentreId=B.CostCentreId  " +
                            " Left Join dbo.LeadRegister C WITH(READPAST) on A.LeadId=C.LeadId " +
                            " Left Join [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister D WITH(READPAST) ON A.ReceiptId=D.ReferenceId AND D.EntryId<>0 " +
                            " Where B.CostCentreId NOT IN(Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                            " Group by A.CostCentreId,A.ReceiptId,A.ReceiptDate,A.ReceiptNo,B.CostCentreName,C.LeadName,F.FlatNo, " +
                            " PaymentAgainst,A.Amount,A.Approve,A.Narration,D.BRS " +
                            " Order by A.ReceiptDate,A.ReceiptNo,LeadName,CostCentreName";
                }
                else
                {
                    //sSql = "Select A.CostCentreId,A.ReceiptId,A.ReceiptDate,A.ReceiptNo,C.LeadName As BuyerName, "+
                    //        " Case When F.FlatNo IS NULL Then PL.PlotNo Else F.FlatNo End FlatNo, B.CostCentreName, A.Amount, " +
                    //        " CASE (A.PaymentAgainst) When 'A' Then 'Advance' When 'B' Then 'Bill' When 'E' Then 'ExtraBill' When 'R' Then 'Rent' When 'O' Then 'Others'  " +
                    //        " When 'PA' Then 'PlotAdvance' When 'PB' Then 'PlotBill' When 'PO' Then 'PlotOthers' END AS PaymentAgainst,A.Narration,A.Approve, " +
                    //        " 'N' ChequeDisbursement From dbo.ReceiptRegister A WITH(READPAST)  " +
                    //        " Left Join dbo.FlatDetails F WITH(READPAST) On F.FlatId=A.FlatId " +
                    //        " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails PL WITH(READPAST) On A.FlatId=PL.PlotDetailsId " +
                    //        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B WITH(READPAST) on A.CostCentreId=B.CostCentreId  " +
                    //        " Left Join dbo.LeadRegister C WITH(READPAST) on A.LeadId=C.LeadId " +
                    //        " Where B.CostCentreId NOT IN(Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                    //        " Group by A.CostCentreId,A.ReceiptId,A.ReceiptDate,A.ReceiptNo,B.CostCentreName,C.LeadName,F.FlatNo,PL.PlotNo, " +
                    //        " PaymentAgainst,A.Amount,A.Approve,A.Narration " +
                    //        " Order by A.ReceiptDate,A.ReceiptNo,LeadName,CostCentreName";

                    sSql = "Select A.CostCentreId,A.ReceiptId,A.ReceiptDate,A.ReceiptNo,C.LeadName As BuyerName, " +
                            " F.FlatNo, B.CostCentreName, A.Amount, " +
                            " CASE (A.PaymentAgainst) When 'A' Then 'Advance' When 'B' Then 'Bill' When 'E' Then 'ExtraBill' When 'R' Then 'Rent' When 'O' Then 'Others'  " +
                            " When 'PA' Then 'PlotAdvance' When 'PB' Then 'PlotBill' When 'PO' Then 'PlotOthers' END AS PaymentAgainst,A.Narration,A.Approve, " +
                            " 'N' ChequeDisbursement From dbo.ReceiptRegister A WITH(READPAST)  " +
                            " Left Join dbo.FlatDetails F WITH(READPAST) On F.FlatId=A.FlatId " +
                            " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B WITH(READPAST) on A.CostCentreId=B.CostCentreId  " +
                            " Left Join dbo.LeadRegister C WITH(READPAST) on A.LeadId=C.LeadId " +
                            " Where B.CostCentreId NOT IN(Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                            " Group by A.CostCentreId,A.ReceiptId,A.ReceiptDate,A.ReceiptNo,B.CostCentreName,C.LeadName,F.FlatNo, " +
                            " PaymentAgainst,A.Amount,A.Approve,A.Narration " +
                            " Order by A.ReceiptDate,A.ReceiptNo,LeadName,CostCentreName";
                }
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
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

        public static string GetReceiptPrint(string argFlatNo, int argCCId, int argRecId)
        {
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            string sSql = "",sAns="";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select Distinct A.ReceiptId From dbo.ReceiptRegister A Left Join dbo.ReceiptTrans B On A.ReceiptId=B.ReceiptId " +
                        " Left Join dbo.FlatDetails C On C.FlatId=B.FlatId Where C.FlatNo='" + argFlatNo + "' And C.CostCentreId=" + argCCId + " And BillType<>'A' Order By A.ReceiptId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(dt);
                da.Dispose();
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["ReceiptId"]) == argRecId) { sAns = "Initial"; }
                    else if (Convert.ToInt32(dt.Rows[dt.Rows.Count-1]["ReceiptId"]) == argRecId) { sAns = "Final"; }
                    else { sAns = "Futher"; }
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
            return sAns;
        }

        public static DataTable GetChangeGridReceiptRegister(int argRecpId)
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.CostCentreId,A.ReceiptId,A.ReceiptDate,A.ReceiptNo,B.CostCentreName,C.LeadName as BuyerName,case (A.PaymentAgainst) When 'A' then 'Advance' when 'B' then 'Bill' when 'R' then 'Rent' When 'O' then 'Others' " +
                        " When 'S' then 'ScheduleBill' When 'PA' then 'PlotAdvance' When 'PB' then 'PlotBill' When 'PO' then 'PlotOthers' END AS PaymentAgainst,A.Amount,A.Approve from ReceiptRegister A " +
                        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B on A.CostCentreId=B.CostCentreId " +
                        " Left Join LeadRegister C on A.LeadId=C.LeadId Where A.ReceiptId=" + argRecpId + "";
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

        #region PopulateLookUpEdit

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
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Select Distinct A.LeadId,B.LeadName From BuyerDetail A " +
                              " Inner Join LeadRegister B on A.LeadId=B.LeadId " +
                              " Where A.CostCentreId=" + argCCId +
                              " Order by B.LeadName";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
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


        public static DataTable GetQual(int argQId, DateTime argDate)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
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
                    sSql = "Select B.Expression,B.ExpPer,A.Add_Less_Flag,B.SurCharge,B.EDCess,B.HEDCess,B.Net,0 Taxable From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A " +
                           "Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriodTrans B on A.QualifierId=B.QualifierId and B.PeriodId = " + iPeriodId + "" +
                           "Where A.QualifierId = " + argQId;
                }
                else
                {
                    sSql = "Select Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,HEDCess,Net,0 Taxable From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp " +
                           "Where QualifierId = " + argQId;
                }

                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
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

        public static DataTable GetQual(int argQId, DateTime argDate, SqlConnection argConn)
        {
            DataTable dt = null;
            try
            {
                string sSql = "Select PeriodId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriod Where QualType='B' and " +
                                "((TDate is not null and Fdate <= '" + argDate.ToString("dd MMM yyyy") + "' and TDate >= '" + argDate.ToString("dd MMM yyyy") + "') or " +
                                "(TDate is null  and FDate <= '" + argDate.ToString("dd MMM yyyy") + "'))";
                SqlCommand cmd = new SqlCommand(sSql, argConn);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dtT = new DataTable();
                dtT.Load(dr);
                dr.Close();
                cmd.Dispose();

                int iPeriodId = 0;
                if (dtT.Rows.Count > 0) { iPeriodId = Convert.ToInt32(dtT.Rows[0]["PeriodId"]); }
                dtT.Dispose();

                if (iPeriodId != 0)
                {
                    sSql = "Select B.Expression,B.ExpPer,A.Add_Less_Flag,B.SurCharge,B.EDCess,B.HEDCess,B.Net,0 Taxable From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A " +
                           "Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriodTrans B on A.QualifierId=B.QualifierId and B.PeriodId = " + iPeriodId + "" +
                           "Where A.QualifierId = " + argQId;
                }
                else
                {
                    sSql = "Select Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,HEDCess,Net,0 Taxable From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp " +
                           "Where QualifierId = " + argQId;
                }

                cmd = new SqlCommand(sSql, argConn);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable GetTenant(int argCCId)
        {
            SqlDataAdapter da;
            DataTable ds = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select TenantId,TenantName From dbo.TenantRegister Where CostCentreId=" + argCCId + "";
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

        #region ReceiptAcct

        public static DataTable GetQualifierAccount(string argBus)
        {
            string sSql = "";
            if (argBus == "B")
                sSql = "Select A.QualifierId,B.QualifierName,A.AccountId from dbo.QualifierAccount A " +
                             "Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp B on A.QualifierId=B.QualifierId " +
                             "Order by B.SortOrder";
            else
                sSql = "Select A.QualifierId,B.QualifierName,A.AccountId from dbo.PlotQualifierAccount A " +
                         "Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp B on A.QualifierId=B.QualifierId " +
                         "Order by B.SortOrder";
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            DataTable dt = new DataTable();
            da.Fill(dt);
            da.Dispose();
            BsfGlobal.g_CRMDB.Close();
            return dt;
        }

        public static void UpdateQualAccount(DataTable argDt, string argBType)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sSql = "";
                SqlCommand cmd;
                if (argBType == "B")
                    sSql = "Truncate Table dbo.QualifierAccount";
                else sSql = "Truncate Table dbo.PlotQualifierAccount";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                int iQualId = 0;
                int iAccountId = 0;

                for (int i = 0; i < argDt.Rows.Count; i++)
                {
                    iQualId = Convert.ToInt32(argDt.Rows[i]["QualifierId"]);
                    iAccountId = Convert.ToInt32(argDt.Rows[i]["AccountId"]);

                    if (argBType == "B")
                        sSql = "Insert Into dbo.QualifierAccount(QualifierId,AccountId) " +
                               "Values(" + iQualId + "," + iAccountId + ")";
                    else
                        sSql = "Insert Into dbo.PlotQualifierAccount(QualifierId,AccountId) " +
                           "Values(" + iQualId + "," + iAccountId + ")";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
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
            }
        }

        public static DataTable GetAccountQualifier(string argBType)
        {
            DataTable dt;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            if (argBType == "B")
                sSql = "SELECT QualifierId,AccountId FROM dbo.QualifierAccount WHERE isnull(AccountId,0)<>0";
            else sSql = "SELECT QualifierId,AccountId FROM dbo.PlotQualifierAccount WHERE isnull(AccountId,0)<>0";
            sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            dt = new DataTable();
            sda.Fill(dt);
            sda.Dispose();
            BsfGlobal.g_CRMDB.Close();
            return dt;
        }

        #endregion

        #region Qualifier Setting

        public static DataTable QualifierSelect(int argQId, string argQType, DateTime argDate)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = new DataTable();
            try
            {
                string sSql = "Select PeriodId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriod Where QualType='" + argQType + "' and " +
                        "((TDate is not null and Fdate <= '" + argDate.ToString("dd MMM yyyy") + "' and TDate >= '" + argDate.ToString("dd MMM yyyy") + "' ) or " +
                        "(TDate is null  and FDate <= '" + argDate.ToString("dd MMM yyyy") + "'))";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtT = new DataTable();
                sda.Fill(dtT);
                sda.Dispose();

                int iPeriodId = 0;
                if (dtT.Rows.Count > 0) { iPeriodId = Convert.ToInt32(dtT.Rows[0]["PeriodId"]); }
                dtT.Dispose();

                if (iPeriodId != 0)
                {
                    sSql = "Select A.QualifierId,A.QualId,C.Expression,C.Add_Less_Flag,C.Net," +
                        "C.ExpPer,C.SurCharge,C.EDCess,C.HEDCess,A.QualMId,A.QualTypeId From  " +
                        "[" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A  " +
                        "Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualifierType B on A.QualTypeId=B.QualTypeId " +
                        "Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriodTrans C on A.QualifierId=C.QualifierId and C.Sel=1 and C.PeriodId=" + iPeriodId + " " +
                        "Where A.QualType = '" + argQType + "' And A.QualifierId=" + argQId + " Order by  A.SortOrder";

                }
                else
                {
                    sSql = "Select Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,HEDCess,Net From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp " +
                                "Where QualifierId = " + argQId;
                }
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        public static DataTable QualifierSelect(int argQId, string argQType, DateTime argDate, SqlConnection argConn)
        {
            DataTable dt = new DataTable();            
            try
            {
                string sSql = "Select PeriodId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriod Where QualType='" + argQType + "' and " +
                        "((TDate is not null and Fdate <= '" + argDate.ToString("dd MMM yyyy") + "' and TDate >= '" + argDate.ToString("dd MMM yyyy") + "' ) or " +
                        "(TDate is null  and FDate <= '" + argDate.ToString("dd MMM yyyy") + "'))";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, argConn);
                DataTable dtT = new DataTable();
                sda.Fill(dtT);
                sda.Dispose();

                int iPeriodId = 0;
                if (dtT.Rows.Count > 0) { iPeriodId = Convert.ToInt32(dtT.Rows[0]["PeriodId"]); }
                dtT.Dispose();

                if (iPeriodId != 0)
                {
                    sSql = "Select A.QualifierId,A.QualId,C.Expression,C.Add_Less_Flag,C.Net," +
                        "C.ExpPer,C.SurCharge,C.EDCess,C.HEDCess,A.QualMId,A.QualTypeId From  " +
                        "[" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A  " +
                        "Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualifierType B on A.QualTypeId=B.QualTypeId " +
                        "Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriodTrans C on A.QualifierId=C.QualifierId and C.Sel=1 and C.PeriodId=" + iPeriodId + " " +
                        "Where A.QualType = '" + argQType + "' And A.QualifierId=" + argQId + " Order by  A.SortOrder";

                }
                else
                {
                    sSql = "Select Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,HEDCess,Net From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp " +
                                "Where QualifierId = " + argQId;
                }
                sda = new SqlDataAdapter(sSql, argConn);
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable GetSTSettings(string argWOType, DateTime argDate)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = new DataTable();
            try
            {
                string sSql = "Select PeriodId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriod Where QualType='S' and " +
                       "((TDate is not null and Fdate <= '" + argDate.ToString("dd MMM yyyy") + "' and TDate >= '" + argDate.ToString("dd MMM yyyy") + "' ) or " +
                       "(TDate is null  and FDate <= '" + argDate.ToString("dd MMM yyyy") + "'))";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dtT = new DataTable();
                dtT.Load(dr);
                dr.Close();
                cmd.Dispose();

                int iPeriodId = 0;
                if (dtT.Rows.Count > 0) { iPeriodId = Convert.ToInt32(dtT.Rows[0]["PeriodId"]); }
                dtT.Dispose();

                if (iPeriodId != 0)
                {
                    sSql = "Select * from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.STSetting  " +
                        "Where WorkType='" + argWOType + "' and PeriodId= " + iPeriodId;
                }
                else
                {
                    sSql = "Select * from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.STSetting  " +
                       "Where WorkType='" + argWOType + "' and PeriodId= " + iPeriodId;
                }

                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        public static DataTable GetSTSettings(string argWOType, DateTime argDate, SqlConnection argConn)
        {
            DataTable dt = new DataTable();
            try
            {
                string sSql = "Select PeriodId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriod Where QualType='S' and " +
                       "((TDate is not null and Fdate <= '" + argDate.ToString("dd MMM yyyy") + "' and TDate >= '" + argDate.ToString("dd MMM yyyy") + "' ) or " +
                       "(TDate is null  and FDate <= '" + argDate.ToString("dd MMM yyyy") + "'))";
                SqlCommand cmd = new SqlCommand(sSql, argConn);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dtT = new DataTable();
                dtT.Load(dr);
                dr.Close();
                cmd.Dispose();

                int iPeriodId = 0;
                if (dtT.Rows.Count > 0) { iPeriodId = Convert.ToInt32(dtT.Rows[0]["PeriodId"]); }
                dtT.Dispose();

                if (iPeriodId != 0)
                {
                    sSql = "Select * from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.STSetting  " +
                        "Where WorkType='" + argWOType + "' and PeriodId= " + iPeriodId;
                }
                else
                {
                    sSql = "Select * from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.STSetting  " +
                       "Where WorkType='" + argWOType + "' and PeriodId= " + iPeriodId;
                }

                SqlDataAdapter sda = new SqlDataAdapter(sSql, argConn);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        #endregion

        #region Narration

        internal static DataTable PopulateNarrationMaster()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            sSql = "Select NarrationId,Description From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.NarrationMaster ORDER BY Description";
            sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            dt = new DataTable();
            sda.Fill(dt);
            sda.Dispose();
            BsfGlobal.g_CRMDB.Close();
            return dt;
        }

        internal static void NarrationMasterDelete(int argNarrId)
        {
            string sSql = "";
            SqlCommand cmd;
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "Delete [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.NarrationMaster Where NarrationId=" + argNarrId + " ";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
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
        }

        internal static void InsertNarrationMaster(int argNarrId, string argDescription)
        {
            string sSql = "";
            SqlCommand cmd;
            try
            {
                BsfGlobal.OpenCRMDB();
                if (argNarrId == 0)
                    sSql = "Insert Into [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.NarrationMaster(Description) Values ('" + argDescription + "') ";
                else
                    sSql = "Update [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.NarrationMaster Set Description='" + argDescription + "' Where NarrationId=" + argNarrId + " ";

                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
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
        }

        public static DataTable PopulateNarr()
        {
            string sSql = "";
            DataTable dt = new DataTable();
            SqlDataAdapter sda;
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "Select NarrationId,Description,Convert(bit,0,1) Sel From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.NarrationMaster ORDER BY Description";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        #endregion

        internal static DataTable GetFlatInterest(int argCCId, int argFlatId, int argPaymentSchId, int argReceiptId)
        {
            BsfGlobal.OpenCRMDB();

            string sSql = "Select A.IntPercent, A.CreditDays, B.FinaliseDate, D.CompletionDate, '' LastReceiptDate from dbo.FlatDetails A " +
                          " INNER JOIN dbo.BuyerDetail B ON A.FlatId=B.FlatId" +
                          " LEFT JOIN dbo.PaymentScheduleFlat C ON A.FlatId=C.FlatId" +
                          " LEFT JOIN dbo.StageDetails D ON C.StageDetId=D.StageDetId" +
                          " Where A.CostCentreId= " + argCCId + " And A.FlatId=" + argFlatId + " AND C.PaymentSchId=" + argPaymentSchId + "";
            SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            sda.Dispose();

            if (dt.Rows.Count > 0)
            {
                if (argReceiptId == 0)
                {
                    sSql = "Select SUM(A.Amount), B.ReceiptDate from dbo.ReceiptTrans A " +
                           " INNER JOIN dbo.ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                           " Where A.CostCentreId= " + argCCId + " And A.FlatId=" + argFlatId +
                           " AND A.PaySchId=" + argPaymentSchId + " GROUP BY B.ReceiptDate";
                }
                else
                {
                    sSql = "Select SUM(A.Amount), B.ReceiptDate from dbo.ReceiptTrans A " +
                           " INNER JOIN dbo.ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                           " Where A.CostCentreId= " + argCCId + " And A.FlatId=" + argFlatId +
                           " AND A.PaySchId=" + argPaymentSchId + " AND A.ReceiptId<>" + argReceiptId + " AND A.ReceiptId<=" + argReceiptId +
                           " GROUP BY B.ReceiptDate";
                }

                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtReceipt = new DataTable();
                sda.Fill(dtReceipt);
                sda.Dispose();

                if (dtReceipt.Rows.Count > 0)
                {
                    dt.Rows[0]["CreditDays"] = 0;
                    dt.Rows[0]["LastReceiptDate"] = Convert.ToDateTime(CommFun.IsNullCheck(dtReceipt.Rows[0]["ReceiptDate"], CommFun.datatypes.VarTypeDate));
                }
            }

            BsfGlobal.g_CRMDB.Close();

            return dt;
        }

        internal static DataTable GetOtherCost()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = new DataTable();
            try
            {
                string sSql = "Select OtherCostId, OtherCostName from dbo.OtherCostMaster";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        internal static DataTable PopulateF1CollectionReport(int argReportType, DateTime argFrom, DateTime argTo)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sReceiptDate = "";
                DateTime dDate = DateTime.Now;
                if (argReportType == 0)
                {
                    sReceiptDate = " AND RR.ReceiptDate='" + dDate.ToString("dd-MMM-yyyy") + "' ";
                }
                else if (argReportType == 1)
                {
                    int iWeek = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dDate, System.Globalization.CalendarWeekRule.FirstFullWeek, dDate.DayOfWeek);
                    sReceiptDate = " AND DATEPART(WK, RR.ReceiptDate)=" + iWeek + " ";
                }
                else if (argReportType == 2)
                {
                    sReceiptDate = " AND MONTH(RR.ReceiptDate)=" + dDate.Month + " ";
                }
                else
                {
                    sReceiptDate = " AND RR.ReceiptDate BETWEEN '" + argFrom.ToString("dd-MMM-yyyy") + "' AND '" + argTo.ToString("dd-MMM-yyyy") + "' ";
                }

                string sSql = "Select ProjectName, FlatNo, BuyerName, [Bill/Schedule], Gross, ServiceTax, NetAmount from( " +
                               " Select DISTINCT ISNULL(C.CostCentreName,'') ProjectName, B.FlatNo, ISNULL(E.LeadName,'') BuyerName, " +
                               " 'Advance' [Bill/Schedule], ISNULL(ISNULL(SUM(DISTINCT A.Amount), 0) - ISNULL(SUM(DISTINCT G.Amount), 0), 0) Gross, " +
                               " ISNULL(SUM(DISTINCT G.Amount), 0) ServiceTax, ISNULL(SUM(DISTINCT A.Amount), 0) NetAmount from dbo.ReceiptTrans A  " +
                               " INNER JOIN dbo.ReceiptRegister RR ON A.ReceiptId=RR.ReceiptId  " +
                               " INNER JOIN dbo.FlatDetails B ON A.FlatId=B.FlatId " +
                               " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C ON B.CostCentreId=C.CostCentreId " +
                               " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CostCentre D ON C.FACostCentreId=D.CostCentreId " +
                               " INNER JOIN dbo.LeadRegister E ON B.LeadId=E.LeadId " +
                               " LEFT JOIN dbo.ReceiptQualifier G ON A.PaySchId=G.PaymentSchId " +
                               " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp I ON G.QualifierId=I.QualifierId " +
                               " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualifierType K ON I.QualTypeId=K.QualTypeId AND K.QualTypeId=2 " +
                               " Where A.ReceiptType='Advance' " + sReceiptDate + " " +
                               " GROUP BY A.CostCentreId, A.FlatId, C.CostCentreName, B.FlatNo, E.LeadName " +
                               " UNION ALL " +
                               " Select DISTINCT ISNULL(C.CostCentreName,'') ProjectName, B.FlatNo, ISNULL(E.LeadName,'') BuyerName,  'Schedule' [Bill/Schedule], " +
                               " ISNULL(ISNULL(SUM(DISTINCT A.Amount), 0) - ISNULL(SUM(DISTINCT G.Amount), 0), 0) Gross, ISNULL(SUM(DISTINCT G.Amount), 0) ServiceTax, " +
                               " ISNULL(SUM(DISTINCT A.Amount), 0) NetAmount  from dbo.ReceiptTrans A " +
                               " INNER JOIN dbo.ReceiptRegister RR ON A.ReceiptId=RR.ReceiptId " +
                               " INNER JOIN dbo.FlatDetails B ON A.FlatId=B.FlatId " +
                               " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C ON B.CostCentreId=C.CostCentreId " +
                               " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CostCentre D ON C.FACostCentreId=D.CostCentreId " +
                               " INNER JOIN dbo.LeadRegister E ON B.LeadId=E.LeadId " +
                               " LEFT JOIN dbo.ReceiptQualifier G ON A.PaySchId=G.PaymentSchId " +
                               " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp I ON G.QualifierId=I.QualifierId " +
                               " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualifierType K ON I.QualTypeId=K.QualTypeId AND K.QualTypeId=2 " +
                               " Where A.ReceiptType='ScheduleBill' " + sReceiptDate + " " +
                               " GROUP BY A.CostCentreId, A.FlatId, C.CostCentreName, B.FlatNo, E.LeadName " +
                               " UNION ALL " +
                               " Select DISTINCT ISNULL(C.CostCentreName,'') ProjectName, B.FlatNo, ISNULL(E.LeadName,'') BuyerName,  'Bill' [Bill/Schedule], " +
                               " ISNULL(ISNULL(SUM(DISTINCT A.Amount), 0) - ISNULL(SUM(DISTINCT G.Amount), 0), 0) Gross, ISNULL(SUM(DISTINCT G.Amount), 0) ServiceTax, " +
                               " ISNULL(SUM(DISTINCT A.Amount), 0) NetAmount from dbo.ReceiptTrans A " +
                               " INNER JOIN dbo.ReceiptRegister RR ON A.ReceiptId=RR.ReceiptId " +
                               " INNER JOIN dbo.FlatDetails B ON A.FlatId=B.FlatId " +
                               " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C ON B.CostCentreId=C.CostCentreId " +
                               " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CostCentre D ON C.FACostCentreId=D.CostCentreId " +
                               " INNER JOIN dbo.LeadRegister E ON B.LeadId=E.LeadId " +
                               " LEFT JOIN dbo.ReceiptQualifier G ON A.PaySchId=G.PaymentSchId " +
                               " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp I ON G.QualifierId=I.QualifierId " +
                               " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualifierType K ON I.QualTypeId=K.QualTypeId AND K.QualTypeId=2 " +
                               " Where A.ReceiptType='ProgressBill' " + sReceiptDate + " " +
                               " GROUP BY A.CostCentreId, A.FlatId, C.CostCentreName, B.FlatNo, E.LeadName " +
                               " ) X GROUP BY ProjectName, FlatNo, BuyerName, [Bill/Schedule], Gross, ServiceTax, NetAmount";


                if (sSql != "")
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

        internal static DataTable PopulateF2CollectionReport(int argReportType, DateTime argFrom, DateTime argTo, int argOCId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sReceiptDate = "";
                DateTime dDate = DateTime.Now;
                if (argReportType == 0)
                    sReceiptDate = " AND MONTH(RR.ReceiptDate)=" + dDate.Month + " ";
                else
                    sReceiptDate = " AND RR.ReceiptDate BETWEEN '" + argFrom.ToString("dd-MMM-yyyy") + "' AND '" + argTo.ToString("dd-MMM-yyyy") + "' ";

                string sSql = "Select DISTINCT ISNULL(F.CostCentreName,'') ProjectName, ISNULL(E.LeadName,'') BuyerName, ISNULL(D.FlatNo,'') FlatNo, " +
                              " ISNULL(C.[Description],'') OtherCost, ISNULL(ISNULL(SUM(DISTINCT A.Amount), 0) - ISNULL(SUM(DISTINCT G.Amount), 0), 0) Gross, " +
                              " ISNULL(SUM(DISTINCT G.Amount), 0) ServiceTax, ISNULL(SUM(DISTINCT A.Amount), 0) NetAmount from dbo.ReceiptTrans A " +
                              " INNER JOIN dbo.ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                              " INNER JOIN dbo.PaymentScheduleFlat C ON A.PaySchId=C.PaymentSchId " +
                              " INNER JOIN dbo.FlatDetails D ON A.FlatId=D.FlatId " +
                              " INNER JOIN dbo.LeadRegister E ON D.LeadId=E.LeadId " +
                              " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre F ON A.CostCentreId=F.CostCentreId " +
                              " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CostCentre H ON F.FACostCentreId=H.CostCentreId " +
                              " LEFT JOIN dbo.ReceiptQualifier G ON C.PaymentSchId=G.PaymentSchId " +
                              " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp I ON G.QualifierId=I.QualifierId " +
                              " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualifierType K ON I.QualTypeId=K.QualTypeId AND K.QualTypeId=2  " +
                              " Where C.SchType='O' AND C.OtherCostId=" + argOCId + "" +
                              " GROUP BY C.SortOrder, A.CostCentreId, A.FlatId, F.CostCentreName, E.LeadName, D.FlatNo, C.[Description] ";
                if (sSql != "")
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
    }
}
