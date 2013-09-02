using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace CRM.DataLayer
{
    class ReceivableDL
    {
        public static DataSet Get_Receivable(DateTime arg_dAsOn, int arg_iBlockId, DateTime arg_dSchDate, bool arg_bSch, int arg_iCCId, string argBuyer)
        {
            string sSql = "";
            string sCond = string.Empty;
            string sCond1 = string.Empty;
            SqlDataAdapter sda = null;
            DataSet ds = new DataSet();
            string sAgeName = string.Empty;
            int iAgeDays = 0;
            int iSLId = 0;
            string fStr = "";
            DataRow[] drowT;
            decimal dAmt = 0;
            
            try
            {
                BsfGlobal.OpenCRMDB();
                ds = new DataSet();

                if (arg_iCCId > 0)
                {
                    sCond = "AND A.CostCentreId=" + arg_iCCId;
                    sCond1 = "AND B.CostCentreId=" + arg_iCCId;
                }

                sSql = String.Format("SELECT AgeId, AgeDesc, FromDays, ToDays, CAST(0 as Decimal(18,3)) Amount FROM AgeSetUp");
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "AgeSetup");
                sda.Dispose();

                if (argBuyer == "B")
                {
                    if (arg_bSch == true)
                    {

                        sSql = String.Format("SELECT D.LeadId,L.LeadName BuyerName,L.LeadType,D.Status,SUM(NetAmount)-SUM(PaidAmount) Receivable FROM PaymentScheduleFlat A " +
                                            "INNER JOIN  FlatDetails B ON A.FlatId=B.FlatId INNER JOIN BuyerDetail D ON A.FlatId=D.FlatId  " +
                                            "INNER JOIN LeadRegister L ON L.LeadId=D.LeadId " +
                                            "WHERE L.LeadType='Buyer' and D.Status='S' and A.SchDate <='{0}' " + sCond + " And A.StageDetId<>0 " +
                                            "GROUP BY D.LeadId,L.LeadName,D.Status,L.LeadType", String.Format("{0:dd/MMM/yyyy}", arg_dSchDate));
                        sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);

                    }
                    else
                    {
                        sSql = String.Format("SELECT E.LeadId,E.LeadName BuyerName,E.LeadType,D.Status,Sum(A.NetAmount)-SUM(A.PaidAmount) Receivable From dbo.ProgressBillRegister A " +
                                            "INNER JOIN PaymentScheduleFlat B on A.PaySchId=B.PaymentSchId INNER JOIN FlatDetails C on C.FlatId=A.FlatId " +
                                            "INNER JOIN BuyerDetail D on B.FlatId=D.FlatId INNER JOIN LeadRegister E on E.LeadId=A.LeadId " +
                                            "WHERE B.BillPassed=1 and  E.LeadType='Buyer' AND D.Status='S' And A.AsOnDate <='{0}' " + sCond1 + " And B.StageDetId<>0 " +
                                            "GROUP BY E.LeadName,E.LeadType,E.LeadId,D.Status", String.Format("{0:dd/MMM/yyyy}", arg_dAsOn));
                        sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);

                    }
                }
                else
                {
                    if (arg_bSch == true)
                    {

                        sSql = String.Format("SELECT B.FlatId,D.LeadId,L.LeadName BuyerName,B.FlatNo,L.LeadType,D.Status,SUM(NetAmount)-SUM(PaidAmount) Receivable FROM dbo.PaymentScheduleFlat A " +
                                            "INNER JOIN  dbo.FlatDetails B ON A.FlatId=B.FlatId INNER JOIN dbo.BuyerDetail D ON A.FlatId=D.FlatId  " +
                                            "INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId " +
                                            "WHERE L.LeadType='Buyer' and D.Status='S' and A.SchDate <='{0}' " + sCond + " And A.StageDetId<>0 " +
                                            "GROUP BY B.FlatId,D.LeadId,L.LeadName,B.FlatNo,D.Status,L.LeadType", String.Format("{0:dd/MMM/yyyy}", arg_dSchDate));
                        sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);

                    }
                    else
                    {
                        sSql = String.Format("SELECT C.FlatId,E.LeadId,E.LeadName BuyerName,C.FlatNo,E.LeadType,D.Status,Sum(A.NetAmount)-SUM(A.PaidAmount) Receivable From dbo.ProgressBillRegister A " +
                                            "INNER JOIN dbo.PaymentScheduleFlat B on A.PaySchId=B.PaymentSchId INNER JOIN dbo.FlatDetails C on C.FlatId=A.FlatId " +
                                            "INNER JOIN dbo.BuyerDetail D on B.FlatId=D.FlatId INNER JOIN dbo.LeadRegister E on E.LeadId=A.LeadId " +
                                            "WHERE B.BillPassed=1 and  E.LeadType='Buyer' AND D.Status='S' And A.AsOnDate <='{0}' " + sCond1 + " And B.StageDetId<>0 " +
                                            "GROUP BY C.FlatId,E.LeadName,C.FlatNo,E.LeadType,E.LeadId,D.Status", String.Format("{0:dd/MMM/yyyy}", arg_dAsOn));
                        sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);

                    }
                }
                sda.Fill(ds, "Receivable");
                sda.Fill(ds, "ReceivableDet");

                if (argBuyer == "B")
                {
                    if (arg_bSch == true)
                    {
                        sSql = "Select LeadId,SUM(Receivable)Receivable,AgeDays FROM ( " +
                                " SELECT C.LeadId,A.NetAmount-A.PaidAmount Receivable, " +
                                " ISNULL((SELECT ToDays FROM AgeSetup Where ToDays >= Datediff(d,DateAdd(d,0,SchDate),'" + String.Format("{0:dd/MMM/yyyy}", arg_dAsOn) + "') " +
                                " AND FromDays <= Datediff(d,DateAdd(d,0,SchDate),'" + String.Format("{0:dd/MMM/yyyy}", arg_dAsOn) + "')),0) AgeDays " +
                                " FROM PaymentScheduleFlat A INNER JOIN  FlatDetails  B on A.FlatID=B.FlatId INNER JOIN LeadRegister C ON C.LeadId=B.LeadId " +
                                " WHERE DateAdd(d,0,SchDate)<'" + String.Format("{0:dd/MMM/yyyy}", arg_dAsOn) + "' " + sCond1 + " AND A.StageDetId<>0) A " +
                                " Group By LeadId,AgeDays";
                        sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);

                    }
                    else
                    {
                        sSql = "Select LeadId,SUM(Receivable)Receivable,AgeDays FROM ( " +
                                " SELECT C.LeadId,A.NetAmount-A.PaidAmount Receivable, ISNULL((SELECT ToDays FROM AgeSetup " +
                                " Where ToDays >= Datediff(d,DateAdd(d,0,AsOnDate),'" + String.Format("{0:dd/MMM/yyyy}", arg_dAsOn) + "') " +
                                " AND FromDays <= Datediff(d,DateAdd(d,0,AsOnDate),'" + String.Format("{0:dd/MMM/yyyy}", arg_dAsOn) + "')),0) AgeDays FROM dbo.ProgressBillRegister A  " +
                                " INNER JOIN dbo.PaymentScheduleFlat P ON P.PaymentSchId=A.PaySchId " +
                                " INNER JOIN FlatDetails B on A.FlatID=B.FlatId INNER JOIN LeadRegister C ON C.LeadId=B.LeadId " +
                                " WHERE DateAdd(d,0,AsOnDate)<'" + String.Format("{0:dd/MMM/yyyy}", arg_dAsOn) + "' " + sCond1 + " AND P.StageDetId<>0) A " +
                                " Group By LeadId,AgeDays";
                        sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);

                    }
                }
                else 
                {
                    if (arg_bSch == true)
                    {
                        sSql = "Select FlatId,LeadId,SUM(Receivable)Receivable,AgeDays FROM ( " +
                                " SELECT B.FlatId,C.LeadId,A.NetAmount-A.PaidAmount Receivable, " +
                                " ISNULL((SELECT ToDays FROM AgeSetup Where ToDays >= Datediff(d,DateAdd(d,0,SchDate),'" + String.Format("{0:dd/MMM/yyyy}", arg_dAsOn) + "') " +
                                " AND FromDays <= Datediff(d,DateAdd(d,0,SchDate),'" + String.Format("{0:dd/MMM/yyyy}", arg_dAsOn) + "')),0) AgeDays " +
                                " FROM PaymentScheduleFlat A INNER JOIN  FlatDetails  B on A.FlatID=B.FlatId INNER JOIN LeadRegister C ON C.LeadId=B.LeadId " +
                                " WHERE DateAdd(d,0,SchDate)<'" + String.Format("{0:dd/MMM/yyyy}", arg_dAsOn) + "' " + sCond1 + " AND A.StageDetId<>0) A " +
                                " Group By FlatId,LeadId,AgeDays";
                        sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);

                    }
                    else
                    {
                        sSql = "Select FlatId,LeadId,SUM(Receivable)Receivable,AgeDays FROM ( " +
                                " SELECT B.FlatId,C.LeadId,A.NetAmount-A.PaidAmount Receivable, ISNULL((SELECT ToDays FROM AgeSetup " +
                                " Where ToDays >= Datediff(d,DateAdd(d,0,AsOnDate),'" + String.Format("{0:dd/MMM/yyyy}", arg_dAsOn) + "') " +
                                " AND FromDays <= Datediff(d,DateAdd(d,0,AsOnDate),'" + String.Format("{0:dd/MMM/yyyy}", arg_dAsOn) + "')),0) AgeDays FROM dbo.ProgressBillRegister A  " +
                                " INNER JOIN dbo.PaymentScheduleFlat P ON P.PaymentSchId=A.PaySchId " +
                                " INNER JOIN FlatDetails B on A.FlatID=B.FlatId INNER JOIN LeadRegister C ON C.LeadId=B.LeadId " +
                                " WHERE DateAdd(d,0,AsOnDate)<'" + String.Format("{0:dd/MMM/yyyy}", arg_dAsOn) + "' " + sCond1 + " AND P.StageDetId<>0) A " +
                                " Group By FlatId,LeadId,AgeDays";
                        sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);

                    }
                }
                    sda.Fill(ds, "AgeDet");


                    if (arg_bSch == true)
                    {
                        sSql = String.Format("SELECT A.FlatId,B.LeadId,C.SchDate,A.FlatNo,C.Description,F.CostCentreName,C.NetAmount,C.PaidAmount,(C.NetAmount-C.PaidAmount) Balance FROM FlatDetails A INNER JOIN LeadRegister B " +
                                            "On A.LeadId=B.LeadId INNER JOIN PaymentScheduleFlat C On A.FlatId=C.FlatId INNER JOIN BuyerDetail D ON C.FlatId=D.FlatId  " +
                                            "INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre F On C.CostCentreId=F.CostCentreId " +
                                            "WHERE LeadType='Buyer' and D.Status='S' and C.SchDate <='{0}' " + sCond + " And C.StageDetId<>0 " +
                                            " ORDER BY C.SortOrder", String.Format("{0:dd/MMM/yyyy}", arg_dSchDate));
                        sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);

                    }
                    else
                    {
                        sSql = String.Format("SELECT C.FlatId,E.LeadId,A.PBNo,A.PBDate,C.FlatNo,B.Description,F.CostCentreName,A.NetAmount,A.PaidAmount,(A.NetAmount-A.PaidAmount) Balance " +
                                       "FROM ProgressBillRegister A INNER JOIN PaymentScheduleFlat B On A.PaySchId=B.PaymentSchId INNER JOIN FlatDetails C On " +
                                       "C.FlatId=A.FlatId INNER JOIN BuyerDetail D On B.FlatId=D.FlatId INNER JOIN LeadRegister E " +
                                       "On E.LeadId=A.LeadId INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre F on F.CostCentreId=A.CostCentreId " +
                                       "WHERE B.BillPassed=1 and E.LeadType='Buyer' AND D.Status='S' AND A.AsOnDate <='{0}' " + sCond + " " +
                                       "And B.StageDetId<>0 ORDER BY B.SortOrder", String.Format("{0:dd/MMM/yyyy}", arg_dAsOn));
                        sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);

                    }
                   
                   
                    sda.Fill(ds, "BillDet");

                    //dAmt = 0;
                   
                    for (int i = 0; i < ds.Tables["AgeSetup"].Rows.Count; i++)
                    {
                        sAgeName = ds.Tables["AgeSetup"].Rows[i]["AgeDesc"].ToString();
                        iAgeDays = Convert.ToInt32(ds.Tables["AgeSetup"].Rows[i]["ToDays"]);
                        ds.Tables["ReceivableDet"].Columns.Add(sAgeName, typeof(decimal));

                        for (int j = 0; j < ds.Tables["ReceivableDet"].Rows.Count; j++)
                        {
                            iSLId = (int)ds.Tables["ReceivableDet"].Rows[j]["LeadId"];
                            fStr = String.Format("LeadId={0} AND AgeDays={1}", iSLId, iAgeDays);
                            drowT = ds.Tables["AgeDet"].Select(fStr);
                            if (drowT.Length > 0)
                            {
                                dAmt = (decimal)drowT[0]["Receivable"];
                            }
                            else
                            {
                                dAmt = (decimal)0;
                            }

                            ds.Tables["ReceivableDet"].Rows[j][sAgeName] = CommFun.FormatNum(dAmt, CommFun.g_iCurrencyDigit); 

                        }
                    }
                    BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "CRM", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information); 
            }
            return ds;
        }

        public static DataSet Get_SchReceivable(DateTime arg_dAsOn, int arg_iBlockId, DateTime arg_dSchDate, bool arg_bSch, int arg_iCCId,int argPaySchId)
        {
            string sSql = "";
            string sCond = string.Empty;
            string sCond1 = string.Empty;
            SqlDataAdapter sda = null;
            DataSet ds = new DataSet();
            string sAgeName = string.Empty;
            int iAgeDays = 0;
            int iSLId = 0;
            string fStr = "";
            DataRow[] drowT;
            decimal dAmt = 0;

            try
            {
                BsfGlobal.OpenCRMDB();
                ds = new DataSet();

                if (arg_iCCId > 0)
                {
                    sCond = "AND A.CostCentreId=" + arg_iCCId;
                    sCond1 = "AND B.CostCentreId=" + arg_iCCId;
                }

                sSql = String.Format("SELECT AgeId, AgeDesc, FromDays, ToDays, CAST(0 as Decimal(18,3)) Amount FROM AgeSetUp");
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "AgeSetup");

                if (arg_bSch == true)
                {
                    //sSql = "SELECT 0 TemplateId,'Advance' Description,L.LeadType,D.Status,SUM(P.NetAmount)-SUM(P.PaidAmount) Receivable FROM PaymentSchedule A " +
                    //        " INNER JOIN PaymentScheduleFlat P ON P.TemplateId=A.TemplateId" +
                    //        " INNER JOIN  FlatDetails B ON P.FlatId=B.FlatId INNER JOIN BuyerDetail D ON P.FlatId=D.FlatId  " +
                    //        " INNER JOIN LeadRegister L ON L.LeadId=D.LeadId WHERE L.LeadType='Buyer' and D.Status='S' and A.SchDate <='" + arg_dSchDate.ToString("dd-MMM-yyyy") + "'  " +
                    //        " AND A.CostCentreId=" + arg_iCCId + " AND A.TypeId=" + argPaySchId + " " +
                    //        " GROUP BY D.Status,L.LeadType " +
                    //        " UNION ALL " +
                   sSql=" SELECT A.TemplateId,A.Description,L.LeadType,D.Status,SUM(P.NetAmount)-SUM(P.PaidAmount) Receivable FROM PaymentSchedule A " +
                        " INNER JOIN PaymentScheduleFlat P ON P.TemplateId=A.TemplateId " +
                        " INNER JOIN  FlatDetails B ON P.FlatId=B.FlatId INNER JOIN BuyerDetail D ON P.FlatId=D.FlatId  " +
                        " INNER JOIN LeadRegister L ON L.LeadId=D.LeadId WHERE L.LeadType='Buyer' and D.Status='S' and A.SchDate <='" + arg_dSchDate.ToString("dd-MMM-yyyy") + "'  " +
                        " AND A.CostCentreId=" + arg_iCCId + " And A.TypeId=" + argPaySchId + " And P.StageDetId<>0 " +
                        " GROUP BY A.TemplateId,A.Description,D.Status,L.LeadType";
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);

                }
                else
                {
                    //sSql = String.Format("SELECT E.LeadId ,E.LeadName BuyerName,E.LeadType,D.Status,Sum(A.NetAmount)-SUM(A.PaidAmount) Receivable From dbo.ProgressBillRegister A " +
                    //                    "INNER JOIN PaymentScheduleFlat B on A.PaySchId=B.PaymentSchId INNER JOIN FlatDetails C on C.FlatId=A.FlatId " +
                    //                    "INNER JOIN BuyerDetail D on B.FlatId=D.FlatId INNER JOIN LeadRegister E on E.LeadId=A.LeadId " +
                    //                    "WHERE B.BillPassed=1 and  E.LeadType='Buyer' AND D.Status='S' And A.AsOnDate <='{0}' " + sCond1 + " " +
                    //                    "GROUP BY E.LeadName,E.LeadType,E.LeadId,D.Status", String.Format("{0:dd/MMM/yyyy}", arg_dAsOn));
                    sSql = "SELECT B.TemplateId,P.Description,E.LeadType,D.Status,Sum(A.NetAmount)-SUM(A.PaidAmount) Receivable From dbo.ProgressBillRegister A " +
                            " INNER JOIN PaymentScheduleFlat B on A.PaySchId=B.PaymentSchId INNER JOIN PaymentSchedule P ON P.TemplateId=B.TemplateId " +
                            " INNER JOIN FlatDetails C on C.FlatId=A.FlatId " +
                            " INNER JOIN BuyerDetail D on B.FlatId=D.FlatId INNER JOIN LeadRegister E on E.LeadId=A.LeadId " +
                            " WHERE B.BillPassed=1 and  E.LeadType='Buyer' AND D.Status='S' And A.AsOnDate <='" + arg_dSchDate.ToString("dd-MMM-yyyy") + "' " +
                            " AND B.CostCentreId=" + arg_iCCId + " And P.TypeId=" + argPaySchId + " And B.StageDetId<>0 " +
                            " GROUP BY B.TemplateId,P.Description,E.LeadType,D.Status";
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);

                }
                sda.Fill(ds, "Receivable");
                sda.Fill(ds, "ReceivableDet");

                if (arg_bSch == true)
                {
                    //sSql = "SELECT A.TemplateId,SUM(Receivable)Receivable,AgeDays FROM (  SELECT 0 TemplateId,A.NetAmount-A.PaidAmount Receivable, " +
                    //        " ISNULL((SELECT ToDays FROM AgeSetup Where ToDays >= Datediff(d,DateAdd(d,0,A.SchDate),'" + arg_dSchDate.ToString("dd-MMM-yyyy") + "')  " +
                    //        " AND FromDays <= Datediff(d,DateAdd(d,0,A.SchDate),'" + arg_dSchDate.ToString("dd-MMM-yyyy") + "')),0) AgeDays  FROM PaymentScheduleFlat A  " +
                    //        " INNER JOIN PaymentSchedule P ON P.TemplateId=A.TemplateId " +
                    //        " INNER JOIN  FlatDetails  B on A.FlatID=B.FlatId INNER JOIN LeadRegister C ON C.LeadId=B.LeadId  " +
                    //        " WHERE DateAdd(d,0,A.SchDate)<'" + arg_dSchDate.ToString("dd-MMM-yyyy") + "' AND B.CostCentreId=" + arg_iCCId + " And P.TypeId=" + argPaySchId + " ) A  Group By TemplateId,AgeDays " +
                    //        " UNION ALL " +
                    sSql = " SELECT A.TemplateId,SUM(Receivable)Receivable,AgeDays FROM (  SELECT A.TemplateId,A.NetAmount-A.PaidAmount Receivable,  " +
                        " ISNULL((SELECT ToDays FROM AgeSetup Where ToDays >= Datediff(d,DateAdd(d,0,A.SchDate),'" + arg_dSchDate.ToString("dd-MMM-yyyy") + "')  " +
                        " AND FromDays <= Datediff(d,DateAdd(d,0,A.SchDate),'" + arg_dSchDate.ToString("dd-MMM-yyyy") + "')),0) AgeDays  FROM PaymentScheduleFlat A " +
                        " INNER JOIN PaymentSchedule P ON P.TemplateId=A.TemplateId " +
                        " INNER JOIN  FlatDetails  B on A.FlatID=B.FlatId INNER JOIN LeadRegister C ON C.LeadId=B.LeadId  " +
                        " WHERE DateAdd(d,0,A.SchDate)<'" + arg_dSchDate.ToString("dd-MMM-yyyy") + "' AND B.CostCentreId=" + arg_iCCId + " " +
                        " And P.TypeId=" + argPaySchId + " AND A.StageDetId<>0) A  Group By TemplateId,AgeDays";
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);

                }
                else
                {
                    //sSql = "Select LeadId,SUM(Receivable)Receivable,AgeDays FROM ( " +
                    //        " SELECT C.LeadId,A.NetAmount-A.PaidAmount Receivable, ISNULL((SELECT ToDays FROM AgeSetup " +
                    //        " Where ToDays >= Datediff(d,DateAdd(d,0,AsOnDate),'" + String.Format("{0:dd/MMM/yyyy}", arg_dAsOn) + "') " +
                    //        " AND FromDays <= Datediff(d,DateAdd(d,0,AsOnDate),'" + String.Format("{0:dd/MMM/yyyy}", arg_dAsOn) + "')),0) AgeDays FROM dbo.ProgressBillRegister A  " +
                    //        " INNER JOIN FlatDetails  B on A.FlatID=B.FlatId INNER JOIN LeadRegister C ON C.LeadId=B.LeadId " +
                    //        " WHERE DateAdd(d,0,AsOnDate)<'" + String.Format("{0:dd/MMM/yyyy}", arg_dAsOn) + "' " + sCond1 + " ) A " +
                    //        " Group By LeadId,AgeDays";
                    sSql = "Select A.TemplateId,SUM(Receivable)Receivable,AgeDays FROM (SELECT P.TemplateId,A.NetAmount-A.PaidAmount Receivable, " +
                            " ISNULL((SELECT ToDays FROM AgeSetup  Where ToDays >= Datediff(d,DateAdd(d,0,AsOnDate),'" + arg_dSchDate.ToString("dd-MMM-yyyy") + "')  " +
                            " AND FromDays <= Datediff(d,DateAdd(d,0,AsOnDate),'" + arg_dSchDate.ToString("dd-MMM-yyyy") + "')),0) AgeDays FROM dbo.ProgressBillRegister A  " +
                            " INNER JOIN PaymentScheduleFlat P ON P.PaymentSchId=A.PaySchId INNER JOIN PaymentSchedule P1 ON P1.TemplateId=P.TemplateId " +
                            " INNER JOIN FlatDetails B ON A.FlatID=B.FlatId INNER JOIN LeadRegister C ON C.LeadId=B.LeadId  " +
                            " WHERE DateAdd(d,0,AsOnDate)<'" + arg_dSchDate.ToString("dd-MMM-yyyy") + "' " +
                            " AND B.CostCentreId=" + arg_iCCId + " AND P1.TypeId=" + argPaySchId + " AND P.StageDetId<>0) A  Group By A.TemplateId,AgeDays";
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);

                }
                sda.Fill(ds, "AgeDet");


                if (arg_bSch == true)
                {
                    //sSql = String.Format("SELECT B.LeadId,C.SchDate,A.FlatNo,C.Description,F.CostCentreName,C.NetAmount,C.PaidAmount,(C.NetAmount-C.PaidAmount) Balance FROM FlatDetails A INNER JOIN LeadRegister B " +
                    //                    "On A.LeadId=B.LeadId INNER JOIN PaymentScheduleFlat C On A.FlatId=C.FlatId INNER JOIN BuyerDetail D ON C.FlatId=D.FlatId  " +
                    //                    "INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre F On C.CostCentreId=F.CostCentreId " +
                    //                    "WHERE LeadType='Buyer' and D.Status='S' and C.SchDate <='{0}' " + sCond + " And C.StageDetId<>0 " +
                    //                    " ORDER BY C.SortOrder", String.Format("{0:dd/MMM/yyyy}", arg_dSchDate));
                    sSql = "SELECT C.TemplateId,B.LeadId,C.SchDate,A.FlatNo,B.LeadName,F.CostCentreName,C.NetAmount,C.PaidAmount,(C.NetAmount-C.PaidAmount) Balance FROM FlatDetails A " +
                            " INNER JOIN LeadRegister B On A.LeadId=B.LeadId INNER JOIN PaymentScheduleFlat C On A.FlatId=C.FlatId " +
                            " INNER JOIN BuyerDetail D ON C.FlatId=D.FlatId  INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre F On C.CostCentreId=F.CostCentreId " +
                            " WHERE LeadType='Buyer' AND D.Status='S' AND C.SchDate <='" + arg_dSchDate.ToString("dd-MMM-yyyy") + "' AND A.CostCentreId=" + arg_iCCId + " " +
                            " AND A.PayTypeId=" + argPaySchId + " AND C.StageDetId<>0  ORDER BY C.SortOrder";
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);

                }
                else
                {
                    //sSql = String.Format("SELECT E.LeadId,A.PBNo,A.PBDate,C.FlatNo,B.Description,F.CostCentreName,A.NetAmount,A.PaidAmount,(A.NetAmount-A.PaidAmount) Balance " +
                    //               "FROM ProgressBillRegister A INNER JOIN PaymentScheduleFlat B On A.PaySchId=B.PaymentSchId INNER JOIN FlatDetails C On " +
                    //               "C.FlatId=A.FlatId INNER JOIN BuyerDetail D On B.FlatId=D.FlatId INNER JOIN LeadRegister E " +
                    //               "On E.LeadId=A.LeadId INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre F on F.CostCentreId=A.CostCentreId " +
                    //               "WHERE B.BillPassed=1 and E.LeadType='Buyer' AND D.Status='S' AND A.AsOnDate <='{0}' " + sCond + " " +
                    //               "And B.StageDetId<>0 ORDER BY B.SortOrder", String.Format("{0:dd/MMM/yyyy}", arg_dAsOn));
                    sSql = "SELECT B.TemplateId,E.LeadId,A.PBNo,A.PBDate,C.FlatNo,B.Description,F.CostCentreName,A.NetAmount,A.PaidAmount,(A.NetAmount-A.PaidAmount) Balance " +
                            " FROM ProgressBillRegister A INNER JOIN PaymentScheduleFlat B On A.PaySchId=B.PaymentSchId INNER JOIN FlatDetails C On C.FlatId=A.FlatId " +
                            " INNER JOIN BuyerDetail D On B.FlatId=D.FlatId INNER JOIN LeadRegister E On E.LeadId=A.LeadId " +
                            " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre F on F.CostCentreId=A.CostCentreId " +
                            " WHERE B.BillPassed=1 AND E.LeadType='Buyer' AND D.Status='S' AND A.AsOnDate <='" + arg_dSchDate.ToString("dd-MMM-yyyy") + "' " +
                            " AND A.CostCentreId=" + arg_iCCId + " And C.PayTypeId=" + argPaySchId + " And B.StageDetId<>0 " +
                            " ORDER BY B.SortOrder";
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);

                }
                sda.Fill(ds, "BillDet");

                //dAmt = 0;

                for (int i = 0; i < ds.Tables["AgeSetup"].Rows.Count; i++)
                {
                    sAgeName = ds.Tables["AgeSetup"].Rows[i]["AgeDesc"].ToString();
                    iAgeDays = Convert.ToInt32(ds.Tables["AgeSetup"].Rows[i]["ToDays"]);
                    ds.Tables["ReceivableDet"].Columns.Add(sAgeName, typeof(decimal));

                    for (int j = 0; j < ds.Tables["ReceivableDet"].Rows.Count; j++)
                    {
                        iSLId = (int)ds.Tables["ReceivableDet"].Rows[j]["TemplateId"];
                        fStr = String.Format("TemplateId={0} AND AgeDays={1}", iSLId, iAgeDays);
                        drowT = ds.Tables["AgeDet"].Select(fStr);
                        if (drowT.Length > 0)
                        {
                            dAmt = (decimal)drowT[0]["Receivable"];
                        }
                        else
                        {
                            dAmt = (decimal)0;
                        }

                        ds.Tables["ReceivableDet"].Rows[j][sAgeName] = CommFun.FormatNum(dAmt, CommFun.g_iCurrencyDigit);

                    }
                }
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "CRM", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            return ds;
        }

        public static DataTable PaymentSchType()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = "select TypeId,TypeName,Typewise from dbo.PaySchType ORDER BY TypeName";
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

    }
}
