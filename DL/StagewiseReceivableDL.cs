using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace CRM.DataLayer
{
    class StagewiseReceivableDL
    {

        public static DataTable GetProject()
        {
            BsfGlobal.OpenWorkFlowDB();
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            try
            {
                dt = new DataTable();
                sSql = "Select CostCentreId,CostCentreName,CRMActual From dbo.OperationalCostCentre" +
                        " Where ProjectDB In(Select ProjectName From " +
                        " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType IN('B'))" +
                        " and CostCentreId Not In (Select CostCentreId From dbo.UserCostCentreTrans " +
                        " Where UserId=" + BsfGlobal.g_lUserId + ") Order By CostCentreName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }

            return dt;
        }

        public static DataTable GetBlock(int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            try
            {
                dt = new DataTable();
                sSql = "Select BlockId,BlockName From dbo.BlockMaster Where CostCentreId=" + argCCId + " Order By BlockName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }

            return dt;
        }

        public static DataTable GetPayment(int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            try
            {
                dt = new DataTable();
                sSql = "Select Distinct A.TypeId,TypeName From PaySchType A INNER JOIN PaymentSchedule B "+
                       " On A.TypeId=B.TypeId Where B.CostCentreId="+ argCCId +" ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }

            return dt;
        }

        public static DataSet GetProjectStageRec(int argCCId, int argPayTypeId, DateTime argDate, int argFromActual)
        {
            BsfGlobal.OpenCRMDB();
            DataSet ds = new DataSet();
            SqlCommand cmd; 
            SqlDataReader sdr;
            decimal dTotalRecv = 0;
            decimal dTotalRevd = 0;
            string sStageName = ""; 
            int iStageId = 0;
            String sSql = string.Empty;
            string sFromActual = string.Empty;
            DataRow[] drT;
            DataTable dt;
            int iCRMRecv;
            string sCond = string.Empty;

            try
            {
                sSql = "Select CRMReceivable From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CostCentreId=" + argCCId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    iCRMRecv = Convert.ToInt32(dt.Rows[0]["CRMReceivable"]);
                }
                else
                {
                    iCRMRecv = 0;
                }

                if (iCRMRecv == 1)
                {
                    sCond = "AND PSF.StageDetId<>0";
                }
                else { sCond = ""; }
                dt.Dispose();

                sSql = "Select CostCentreId,CostCentreName,[O/B]=-1*(SELECT SUM(OBReceipt) FROM " +
                         " FlatDetails F Where F.CostCentreId=O.CostCentreId),Advance=(Select IsNull(SUM(B.Amount),0)Amount From ReceiptRegister A " +
                         " Inner Join ReceiptTrans B On A.ReceiptId=B.ReceiptId Where A.PaymentAgainst='A' And A.CostCentreId=" + argCCId + " " +
                         " And ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "'),ExtraBillAmt=(Select Isnull(SUM(Amount),0) From ReceiptTrans " +
                         " Where ReceiptType='ExtraBill' And CostCentreId=" + argCCId + ") " +
                         " From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O " +
                         " Where ProjectDB In(Select ProjectName From " +
                         " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType IN('B')) " +
                         " and CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                         " Where UserId=" + BsfGlobal.g_lUserId + ") And CostCentreId=" + argCCId + " Order By CostCentreName";

                SqlDataAdapter sda = new SqlDataAdapter(sSql,BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Project");

                sSql = "Select TemplateId,Description From PaymentSchedule Where CostCentreId=" + argCCId + " And TypeId=" + argPayTypeId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();

                ds.Load(sdr, LoadOption.OverwriteChanges, "Stages");
                sdr.Close();

                if (argFromActual == 0)
                {
                   sFromActual = " Select PSF.CostCentreId,PS.TemplateId,0,Sum(RT.Amount)Received From ReceiptTrans RT " +
                                " INNER JOIN ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                " INNER JOIN PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                " RIGHT JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                " WHERE RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PSF.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + " " +
                                " AND RR.Cancel=0 GROUP BY PSF.CostCentreId ,PS.TemplateId";
                 }
                else if (argFromActual == 1)
                {

                   sFromActual = " Select PSF.CostCentreId,PS.TemplateId,0,Sum(RT.Amount)Received From ReceiptTrans RT " +
                                 " INNER JOIN ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                 " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=RR.ReceiptId " +
                                 " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=RT.CostCentreId " +
                                 " INNER JOIN PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                 " RIGHT JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                 " WHERE O.CRMActual=1 AND R.Cancel=0 And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PSF.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + " " +
                                 " GROUP BY PSF.CostCentreId ,PS.TemplateId";
                }
                else
                {
                   sFromActual = " Select PSF.CostCentreId,PS.TemplateId,0,Sum(RT.Amount)Received From ReceiptTrans RT " +
                                " INNER JOIN ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=RR.ReceiptId " +
                                " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=RT.CostCentreId " +
                                " INNER JOIN PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                " RIGHT JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                " WHERE O.CRMActual=1 AND R.BRS=1 And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PSF.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + " " +
                                " GROUP BY PSF.CostCentreId ,PS.TemplateId";
                }

                sSql = "SELECT ProjectId,TemplateId, SUM(Receivable) Receivable,  " +
                        " SUM(Received) Received ,SUM(Receivable)-SUM(Received) Balance FROM " +
                        " (" +
                        " SELECT PB.CostCentreId ProjectId,PS.TemplateId,SUM(PB.NetAmount) Receivable,0 Received  " +
                        " FROM ProgressBillRegister PB INNER JOIN FlatDetails FD ON FD.FlatId=PB.FlatId " +
                        " INNER JOIN BuyerDetail BD ON PB.FlatId=BD.FlatId " +
                        " INNER JOIN LeadRegister LR ON LR.LeadId=FD.LeadId " +
                        " INNER JOIN PaymentScheduleFlat PSF ON PSF.PaymentSchId=PB.PaySchId" +
                        " RIGHT JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                        " WHERE PSF.BillPassed=1 And PBDate<='" + argDate.ToString("dd-MMM-yyyy") + "' And PB.CostCentreId=" + argCCId + " AND " +
                        " PS.TypeId=" + argPayTypeId + " GROUP BY PB.CostCentreId,PS.TemplateId" +
                        " UNION ALL  " +
                        " SELECT PSF.CostCentreId ProjectId,PS.TemplateId,SUM(NetAmount) Receivable,0   " +
                        " FROM PaymentScheduleFlat PSF INNER JOIN FlatDetails FD ON PSF.FlatId=FD.FlatId " +
                        " INNER JOIN BuyerDetail BD ON PSF.FlatId=BD.FlatId  " +
                        " INNER JOIN LeadRegister LR ON LR.LeadId=BD.LeadId " +
                        " RIGHT JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                        " WHERE PSF.BillPassed=0 And PSF.SchDate<='" + argDate.ToString("dd-MMM-yyyy") + "' " + sCond + " AND PSF.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + " " +
                        " GROUP BY PSF.CostCentreId ,PS.TemplateId" +
                        " UNION ALL  " +
                        " " + sFromActual + " " +
                        " ) A  INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B ON A.ProjectId=B.CostCentreId   " +
                        " GROUP BY ProjectId,TemplateId ";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();

                ds.Load(sdr, LoadOption.OverwriteChanges, "Recv");
                sdr.Close();

                string sColName1 = "", sColName2 = "", sColName3 = "";
                if (ds.Tables["Recv"].Rows.Count > 0)
                {
                    //for (int i = 0; i < ds.Tables["Stages"].Rows.Count; i++)
                    //{
                    //    sStageName = ds.Tables["Stages"].Rows[i]["Description"].ToString();
                    //    int iTemplateId = (int)ds.Tables["Recv"].Rows[i]["TemplateId"];
                    //    iStageId = iTemplateId;
                    //    DataTable dtRecv = new DataTable();
                    //    DataView dv = new DataView(ds.Tables["Recv"]) { RowFilter = String.Format("TemplateId={0}", iTemplateId) };
                    //    dtRecv = dv.ToTable();

                    //    sColName = iStageId + "- Recv";
                    //    ds.Tables["Project"].Columns.Add(sColName);

                    //    if (dtRecv.Rows.Count != 0)
                    //        ds.Tables["Project"].Rows[0][sColName] = Convert.ToDecimal(dtRecv.Rows[0]["Receivable"]);
                    //    else
                    //        ds.Tables["Project"].Rows[0][sColName] = Convert.ToDecimal(0);

                    //    sColName = iStageId + "- Recd";
                    //    ds.Tables["Project"].Columns.Add(sColName);

                    //    if (dtRecv.Rows.Count != 0)
                    //        ds.Tables["Project"].Rows[0][sColName] = Convert.ToDecimal(dtRecv.Rows[0]["Received"]);
                    //    else
                    //        ds.Tables["Project"].Rows[0][sColName] = Convert.ToDecimal(0);

                    //    sColName = iStageId + "- Bal";
                    //    ds.Tables["Project"].Columns.Add(sColName);

                    //    if (dtRecv.Rows.Count != 0)
                    //        ds.Tables["Project"].Rows[0][sColName] = Convert.ToDecimal(dtRecv.Rows[0]["Balance"]);
                    //    else
                    //        ds.Tables["Project"].Rows[0][sColName] = Convert.ToDecimal(0);

                    //    dtRecv.Dispose();
                    //    dv.Dispose();
                    //}
                    for (int i = 0; i < ds.Tables["Stages"].Rows.Count; i++)
                    {
                        sStageName = ds.Tables["Stages"].Rows[i]["Description"].ToString();
                        int iTemplateId = (int)ds.Tables["Stages"].Rows[i]["TemplateId"];
                        int iProjectId = argCCId;
                        iStageId = iTemplateId;

                        //sColName1 = iStageId + "- Recv";
                        //ds.Tables["Project"].Columns.Add(sColName1);

                        //sColName2 = iStageId + "- Recd";
                        //ds.Tables["Project"].Columns.Add(sColName2);

                        //sColName3 = iStageId + "- Bal";
                        //ds.Tables["Project"].Columns.Add(sColName3);
                        sColName1 = iStageId + "- Recv";
                        DataColumn col1 = new DataColumn(sColName1) { DataType = typeof(decimal), DefaultValue = 0 };
                        ds.Tables["Project"].Columns.Add(col1);

                        sColName2 = iStageId + "- Recd";
                        DataColumn col2 = new DataColumn(sColName2) { DataType = typeof(decimal), DefaultValue = 0 };
                        ds.Tables["Project"].Columns.Add(col2);

                        sColName3 = iStageId + "- Bal";
                        DataColumn col3 = new DataColumn(sColName3) { DataType = typeof(decimal), DefaultValue = 0 };
                        ds.Tables["Project"].Columns.Add(col3);

                        DataTable dtRecv = new DataTable();
                        DataView dv = new DataView(ds.Tables["Recv"]) { RowFilter = String.Format("TemplateId={0} ", iTemplateId) };
                        dtRecv = dv.ToTable();

                        for (int j = 0; j < dtRecv.Rows.Count; j++)
                        {
                            iProjectId = Convert.ToInt32(dtRecv.Rows[j]["ProjectId"]);
                            decimal dRecv = Convert.ToDecimal(dtRecv.Rows[j]["Receivable"]);
                            decimal dRecd = Convert.ToDecimal(dtRecv.Rows[j]["Received"]);
                            decimal dBal = Convert.ToDecimal(dtRecv.Rows[j]["Balance"]);

                            drT = ds.Tables["Project"].Select(String.Format("CostCentreId={0}", iProjectId));

                            if (drT.Length > 0)
                            {
                                drT[0][sColName1] = dRecv;
                                dTotalRecv = dTotalRecv + dRecv;
                                dTotalRevd = dTotalRevd + dRecd;
                                drT[0][sColName2] = dRecd;
                                drT[0][sColName3] = dBal;
                            }
                        }
                    }
                    
                    DataColumn col4 = new DataColumn("TotalReceivable") { DataType = typeof(decimal), DefaultValue = 0 };
                    ds.Tables["Project"].Columns.Add(col4);

                    DataColumn col5 = new DataColumn("TotalReceived") { DataType = typeof(decimal), DefaultValue = 0 };
                    ds.Tables["Project"].Columns.Add(col5);

                    DataColumn col6 = new DataColumn("Balance") { DataType = typeof(decimal), DefaultValue = 0 };
                    ds.Tables["Project"].Columns.Add(col6);

                    drT = ds.Tables["Project"].Select(String.Format("CostCentreId={0}", argCCId));
                    if (drT.Length > 0)
                    {
                        drT[0]["TotalReceivable"] = dTotalRecv;
                        drT[0]["TotalReceived"] = (decimal)drT[0]["O/B"] + (decimal)drT[0]["Advance"] + (decimal)drT[0]["ExtraBillAmt"] + dTotalRevd;
                        //drT[0]["TotalReceived"] = (decimal)ds.Tables["Project"].Rows[0]["O/B"] + (decimal)ds.Tables["Project"].Rows[0]["Advance"] + dTotalRevd;
                        drT[0]["Balance"] = (decimal)drT[0]["TotalReceivable"] - (decimal)drT[0]["TotalReceived"];
                    }

                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            BsfGlobal.g_CRMDB.Close();
            return ds;
        }

        public static DataSet GetBlockStageRec(int argCCId, int argPayTypeId, DateTime argDate, int argFromActual)
        {
            BsfGlobal.OpenCRMDB();
            DataSet ds = new DataSet();
            SqlCommand cmd;
            SqlDataReader sdr;
            string sStageName = "";
            string sFromActual = string.Empty;
            decimal dTotalRecv = 0;
            decimal dTotalRevd = 0;
            int iStageId = 0;
            String sSql = string.Empty;
            DataRow[] drT;
            DataTable dt;
            int iCRMRecv;
            string sCond = string.Empty;

            try
            {
                sSql = "Select CRMReceivable From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CostCentreId=" + argCCId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    iCRMRecv = Convert.ToInt32(dt.Rows[0]["CRMReceivable"]);
                }
                else
                {
                    iCRMRecv = 0;
                }

                if (iCRMRecv == 1)
                {
                    sCond = "AND PSF.StageDetId<>0";
                }
                else { sCond = ""; }
                dt.Dispose();

                sSql = "SELECT B.BlockId,B.BlockName,[O/B]=ISNULL(SUM(OBReceipt),0),Advance=ISNULL(SUM(Advance),0), " +
                        " ExtraBillAmt=ISNULL(SUM(ExtraBillAmt),0) FROM ( " +
                        " SELECT FD.BlockId,OBReceipt=0,Advance=RT.Amount,ExtraBillAmt=0 FROM ReceiptTrans RT " +
                        " LEFT JOIN ReceiptRegister RR ON RR.ReceiptId=RT.ReceiptId  " +
                        " INNER JOIN FlatDetails FD ON FD.FlatId=RT.FlatId WHERE PaymentAgainst='A' And ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' " +
                        " AND RT.CostCentreId=" + argCCId + "  " +
                        " UNION ALL " +
                        " SELECT FD.BlockId,-1*OBReceipt,0,0 FROM FlatDetails FD WHERE FD.CostCentreId=" + argCCId + " " +
                        " UNION All " +
                        " SELECT FD.BlockId,OBReceipt=0,Advance=0,ExtraBillAmt=RT.Amount FROM ReceiptTrans RT LEFT JOIN ReceiptRegister RR " +
                        " ON RR.ReceiptId=RT.ReceiptId  INNER JOIN FlatDetails FD ON FD.FlatId=RT.FlatId WHERE ReceiptType='ExtraBill' " +
                        " And ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND RT.CostCentreId=" + argCCId + "  " +
                        " ) A RIGHT JOIN BlockMaster B ON A.BlockId=B.BlockId WHERE B.CostCentreId=" + argCCId + " " +
                        " GROUP BY B.BlockId,B.BlockName ORDER BY BlockName";

                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                ds.Load(sdr, LoadOption.OverwriteChanges, "Block");
                sdr.Close();

                sSql = "Select TemplateId,Description From PaymentSchedule Where CostCentreId=" + argCCId + " And TypeId=" + argPayTypeId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();

                ds.Load(sdr, LoadOption.OverwriteChanges, "Stages");
                sdr.Close();

                if (argFromActual == 0)
                {
                    sFromActual = " Select FD.BlockId,PS.TemplateId,0,Sum(RT.Amount)Received From ReceiptTrans RT " +
                                " INNER JOIN ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                " INNER JOIN PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                " INNER JOIN FlatDetails FD ON FD.FlatId=PSF.FlatId" +
                                " RIGHT JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                " WHERE RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PSF.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + " " +
                                " AND RR.Cancel=0 GROUP BY FD.BlockId ,PS.TemplateId";
                }
                else if (argFromActual == 1)
                {

                    sFromActual = " Select FD.BlockId,PS.TemplateId,0,Sum(RT.Amount)Received From ReceiptTrans RT " +
                                 " INNER JOIN ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                 " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=RR.ReceiptId " +
                                 " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=RT.CostCentreId " +
                                 " INNER JOIN PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                 " INNER JOIN FlatDetails FD ON FD.FlatId=PSF.FlatId" +
                                 " RIGHT JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                 " WHERE O.CRMActual=1 AND R.Cancel=0 And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PSF.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + " " +
                                 " GROUP BY FD.BlockId ,PS.TemplateId";
                }
                else
                {
                    sFromActual = " Select FD.BlockId,PS.TemplateId,0,Sum(RT.Amount)Received From ReceiptTrans RT " +
                                " INNER JOIN ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=RR.ReceiptId " +
                                " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=RT.CostCentreId " +
                                " INNER JOIN PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                " INNER JOIN FlatDetails FD ON FD.FlatId=PSF.FlatId" +
                                " RIGHT JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                " WHERE O.CRMActual=1 AND R.BRS=1 And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PSF.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + " " +
                                " GROUP BY FD.BlockId ,PS.TemplateId";
                }

                sSql = "SELECT B.BlockId,B.BlockName,TemplateId,SUM(Receivable) Receivable,SUM(Received) Received,SUM(Receivable)-SUM(Received) Balance " +
                        " FROM  ( " +
                        " SELECT FD.BlockId,PS.TemplateId,SUM(PB.NetAmount) Receivable,0 Received   " +
                        " FROM ProgressBillRegister PB INNER JOIN FlatDetails FD ON FD.FlatId=PB.FlatId  " +
                        " INNER JOIN BuyerDetail BD ON PB.FlatId=BD.FlatId  INNER JOIN LeadRegister LR ON LR.LeadId=FD.LeadId  " +
                        " INNER JOIN PaymentScheduleFlat PSF ON PSF.PaymentSchId=PB.PaySchId " +
                        " INNER JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId " +
                        " WHERE PSF.BillPassed=1 And PBDate<='" + argDate.ToString("dd-MMM-yyyy") + "' And PB.CostCentreId=" + argCCId + " AND  PS.TypeId=" + argPayTypeId + " " +
                        " GROUP BY FD.BlockId,PS.TemplateId " +
                        " UNION ALL   " +
                        " SELECT FD.BlockId,PS.TemplateId,SUM(NetAmount) Receivable,0    " +
                        " FROM PaymentScheduleFlat PSF INNER JOIN FlatDetails FD ON PSF.FlatId=FD.FlatId  " +
                        " INNER JOIN BuyerDetail BD ON PSF.FlatId=BD.FlatId   INNER JOIN LeadRegister LR ON LR.LeadId=BD.LeadId  " +
                        " INNER JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId " +
                        " WHERE PSF.BillPassed=0 And PSF.SchDate<='" + argDate.ToString("dd-MMM-yyyy") + "' " + sCond + " AND PSF.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + "  " +
                        " GROUP BY FD.BlockId ,PS.TemplateId " +
                        " UNION ALL   " +
                    //" Select FD.BlockId,PS.TemplateId,0,Sum(RT.Amount)Received From ReceiptTrans RT  " +
                    //" INNER JOIN ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId " +
                    //" INNER JOIN PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                    //" INNER JOIN FlatDetails FD ON FD.FlatId=PSF.FlatId" +
                    //" INNER JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId " +
                    //" WHERE RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PSF.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + "  " +
                    //" GROUP BY FD.BlockId ,PS.TemplateId " +
                        " " + sFromActual + " " +
                        " ) A  " +
                        " INNER JOIN dbo.BlockMaster B ON A.BlockId=B.BlockId    " +
                        " GROUP BY B.BlockId,B.BlockName,TemplateId ";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();

                ds.Load(sdr, LoadOption.OverwriteChanges, "Recv");
                sdr.Close();

                string sColName1 = ""; string sColName2 = ""; string sColName3 = "";
                if (ds.Tables["Recv"].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables["Stages"].Rows.Count; i++)
                    {
                        sStageName = ds.Tables["Stages"].Rows[i]["Description"].ToString();
                        int iTemplateId = (int)ds.Tables["Stages"].Rows[i]["TemplateId"];
                        int iBlockId = 0;
                        iStageId = iTemplateId;

                        //sColName1 = iStageId + "- Recv";
                        //ds.Tables["Block"].Columns.Add(sColName1);
                       
                        //sColName2 = iStageId + "- Recd";
                        //ds.Tables["Block"].Columns.Add(sColName2);
                                               
                        //sColName3 = iStageId + "- Bal";
                        //ds.Tables["Block"].Columns.Add(sColName3);
                        sColName1 = iStageId + "- Recv";
                        DataColumn col1 = new DataColumn(sColName1) { DataType = typeof(decimal), DefaultValue = 0 };
                        ds.Tables["Block"].Columns.Add(col1);

                        sColName2 = iStageId + "- Recd";
                        DataColumn col2 = new DataColumn(sColName2) { DataType = typeof(decimal), DefaultValue = 0 };
                        ds.Tables["Block"].Columns.Add(col2);

                        sColName3 = iStageId + "- Bal";
                        DataColumn col3 = new DataColumn(sColName3) { DataType = typeof(decimal), DefaultValue = 0 };
                        ds.Tables["Block"].Columns.Add(col3);
                       
                        DataTable dtRecv = new DataTable();
                        DataView dv = new DataView(ds.Tables["Recv"]) { RowFilter = String.Format("TemplateId={0} ", iTemplateId) };
                        dtRecv = dv.ToTable();

                        for (int j = 0; j < dtRecv.Rows.Count; j++)
                        {
                            iBlockId = Convert.ToInt32(dtRecv.Rows[j]["BlockId"]);
                            decimal dRecv = Convert.ToDecimal(dtRecv.Rows[j]["Receivable"]);
                            decimal dRecd = Convert.ToDecimal(dtRecv.Rows[j]["Received"]);
                            decimal dBal = Convert.ToDecimal(dtRecv.Rows[j]["Balance"]);

                            drT = ds.Tables["Block"].Select(String.Format("BlockId={0}", iBlockId));

                            if (drT.Length > 0)
                            {
                                drT[0][sColName1] = dRecv;
                                drT[0][sColName2] = dRecd;
                                drT[0][sColName3] = dBal;
                            }
                        }

                        dtRecv.Dispose();
                        dv.Dispose();
                    }

                    DataColumn col4 = new DataColumn("TotalReceivable") { DataType = typeof(decimal), DefaultValue = 0 };
                    ds.Tables["Block"].Columns.Add(col4);

                    DataColumn col5 = new DataColumn("TotalReceived") { DataType = typeof(decimal), DefaultValue = 0 };
                    ds.Tables["Block"].Columns.Add(col5);

                    DataColumn col6 = new DataColumn("Balance") { DataType = typeof(decimal), DefaultValue = 0 };
                    ds.Tables["Block"].Columns.Add(col6);

                    
                    for (int i = 0; i < ds.Tables["Block"].Rows.Count; i++)
                    {
                        dTotalRecv=0;
                        dTotalRevd=0;

                        //int j = 3;
                        int j = 5;
                        
                        while (j<ds.Tables["Block"].Columns.Count)
                        {
                            dTotalRecv = dTotalRecv +(decimal) ds.Tables["Block"].Rows[i][j];
                            if(j==3)
                                dTotalRevd = dTotalRevd + (decimal)ds.Tables["Block"].Rows[i][j]+ (decimal)ds.Tables["Block"].Rows[i][2];
                            else
                                dTotalRevd = dTotalRevd + (decimal)ds.Tables["Block"].Rows[i][j+1];

                            j = j + 3;
                        }

                        ds.Tables["Block"].Rows[i]["TotalReceivable"] = dTotalRecv;
                        ds.Tables["Block"].Rows[i]["TotalReceived"] = (decimal)ds.Tables["Block"].Rows[i]["O/B"] + (decimal)ds.Tables["Block"].Rows[i]["Advance"] + (decimal)ds.Tables["Block"].Rows[i]["ExtraBillAmt"] + dTotalRevd;
                        ds.Tables["Block"].Rows[i]["Balance"] = (decimal)ds.Tables["Block"].Rows[i]["TotalReceivable"] - (decimal)ds.Tables["Block"].Rows[i]["TotalReceived"];
                    }                    
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            BsfGlobal.g_CRMDB.Close();
            return ds;
        }

        public static DataSet GetBuyerStageRec(int argCCId, int argBlockId, int argPayTypeId, DateTime argDate, int argFromActual)
        {
            BsfGlobal.OpenCRMDB();
            DataSet ds = new DataSet();
            SqlCommand cmd;
            SqlDataReader sdr;
            decimal dTotalRecv = 0;
            decimal dTotalRevd = 0;
            string sStageName = "";
            string sFromActual = string.Empty;
            int iStageId = 0;
            String sSql = string.Empty;
            DataRow[] drT;
            DataTable dt;
            int iCRMRecv;
            string sCond = string.Empty;

            try
            {
                sSql = "Select CRMReceivable From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CostCentreId=" + argCCId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    iCRMRecv = Convert.ToInt32(dt.Rows[0]["CRMReceivable"]);
                }
                else
                {
                    iCRMRecv = 0;
                }

                if (iCRMRecv == 1)
                {
                    sCond = "AND PSF.StageDetId<>0";
                }
                else { sCond = ""; }
                dt.Dispose();

                sSql = "Select B.FlatId,B.FlatNo,[O/B]= -1*B.OBReceipt,Advance=ISNULL(SUM(Advance),0), " +
                        " ExtraBillAmt=ISNULL(SUM(A.ExtraBillAmt),0) FROM (  " +
                        " SELECT FD.FlatId,FD.FlatNo,OBReceipt=0,Advance=RT.Amount,ExtraBillAmt=0 FROM ReceiptTrans RT " +
                        " LEFT JOIN ReceiptRegister RR ON RR.ReceiptId=RT.ReceiptId  " +
                        " INNER JOIN FlatDetails FD ON FD.FlatId=RT.FlatId WHERE PaymentAgainst='A' And ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' " +
                        " AND FD.BlockId=" + argBlockId + "  " +
                        " UNION ALL  " +
                        " SELECT FD.FlatId,FD.FlatNo,-1*OBReceipt,0,0 FROM FlatDetails FD WHERE FD.BlockId=" + argBlockId + " " +
                        " UNION All " +
                        " SELECT FD.FlatId,FD.FlatNo,OBReceipt=0,Advance=0,ExtraBillAmt=RT.Amount FROM ReceiptTrans RT LEFT JOIN ReceiptRegister RR " +
                        " ON RR.ReceiptId=RT.ReceiptId INNER JOIN FlatDetails FD ON FD.FlatId=RT.FlatId WHERE ReceiptType='ExtraBill' " +
                        " And ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND FD.BlockId=" + argBlockId + "  " +
                        " ) A  INNER JOIN FlatDetails B ON B.FlatId=A.FlatId " +
                        " INNER JOIN dbo.BlockMaster BM On BM.BlockId=B.BlockId " +
                        " INNER JOIN dbo.LevelMaster LM ON LM.LevelId=B.LevelId " +
                        " Where B.LeadId<>0 And B.BlockId=" + argBlockId + "  " +
                        " GROUP BY BM.SortOrder,LM.SortOrder,B.SortOrder,B.FlatId,B.FlatNo,B.OBReceipt " +
                        " Order By BM.SortOrder,LM.SortOrder,B.SortOrder,dbo.Val(B.FlatNo)";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                ds.Load(sdr, LoadOption.OverwriteChanges, "Flat");
                sdr.Close();

                sSql = "Select TemplateId,Description From PaymentSchedule Where CostCentreId=" + argCCId + " And TypeId=" + argPayTypeId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();

                ds.Load(sdr, LoadOption.OverwriteChanges, "Stages");
                sdr.Close();


                if (argFromActual == 0)
                {
                    sFromActual = " Select FD.FlatId,PS.TemplateId,0,Sum(RT.Amount)Received From ReceiptTrans RT " +
                                " INNER JOIN ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                " INNER JOIN PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                " INNER JOIN FlatDetails FD ON FD.FlatId=PSF.FlatId" +
                                " RIGHT JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                " WHERE RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PSF.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + " " +
                                " AND RR.Cancel=0 GROUP BY FD.FlatId ,PS.TemplateId";
                }
                else if (argFromActual == 1)
                {

                    sFromActual = " Select FD.FlatId,PS.TemplateId,0,Sum(RT.Amount)Received From ReceiptTrans RT " +
                                 " INNER JOIN ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                 " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=RR.ReceiptId " +
                                 " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=RT.CostCentreId " +
                                 " INNER JOIN PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                 " INNER JOIN FlatDetails FD ON FD.FlatId=PSF.FlatId" +
                                 " RIGHT JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                 " WHERE O.CRMActual=1 AND R.Cancel=0 And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PSF.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + " " +
                                 " GROUP BY FD.FlatId ,PS.TemplateId";
                }
                else
                {
                    sFromActual = " Select FD.FlatId,PS.TemplateId,0,Sum(RT.Amount)Received From ReceiptTrans RT " +
                                " INNER JOIN ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=RR.ReceiptId " +
                                " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=RT.CostCentreId " +
                                " INNER JOIN PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                " INNER JOIN FlatDetails FD ON FD.FlatId=PSF.FlatId" +
                                " RIGHT JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                " WHERE O.CRMActual=1 AND R.BRS=1 And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PSF.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + " " +
                                " GROUP BY FD.FlatId ,PS.TemplateId";
                }

                sSql = "SELECT B.FlatId,B.FlatNo,TemplateId, SUM(Receivable) Receivable,SUM(Received) Received,SUM(Receivable)-SUM(Received) Balance " +
                        " FROM  ( " +
                        " SELECT FD.FlatId,PS.TemplateId,SUM(PB.NetAmount) Receivable,0 Received   " +
                        " FROM ProgressBillRegister PB INNER JOIN FlatDetails FD ON FD.FlatId=PB.FlatId  " +
                        " INNER JOIN BuyerDetail BD ON PB.FlatId=BD.FlatId  INNER JOIN LeadRegister LR ON LR.LeadId=FD.LeadId  " +
                        " INNER JOIN PaymentScheduleFlat PSF ON PSF.PaymentSchId=PB.PaySchId " +
                        " INNER JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId " +
                        " WHERE PSF.BillPassed=1 And PBDate<='" + argDate.ToString("dd-MMM-yyyy") + "' And FD.BlockId=" + argBlockId + " AND PS.TypeId=" + argPayTypeId + " GROUP BY FD.FlatId,PS.TemplateId " +
                        " UNION ALL   " +
                        " SELECT FD.FlatId,PS.TemplateId,SUM(NetAmount) Receivable,0    " +
                        " FROM PaymentScheduleFlat PSF INNER JOIN FlatDetails FD ON PSF.FlatId=FD.FlatId  " +
                        " INNER JOIN BuyerDetail BD ON PSF.FlatId=BD.FlatId   INNER JOIN LeadRegister LR ON LR.LeadId=BD.LeadId  " +
                        " INNER JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId " +
                        " WHERE PSF.BillPassed=0 And PSF.SchDate<='" + argDate.ToString("dd-MMM-yyyy") + "' " + sCond + " AND FD.BlockId=" + argBlockId + " AND PS.TypeId=" + argPayTypeId + "  " +
                        " GROUP BY FD.FlatId ,PS.TemplateId " +
                        " UNION ALL   " +
                        " " + sFromActual + " " +
                        " ) A  " +
                        " INNER JOIN dbo.FlatDetails B ON A.FlatId=B.FlatId    " +
                        " GROUP BY B.FlatId,B.FlatNo,TemplateId";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();

                ds.Load(sdr, LoadOption.OverwriteChanges, "Recv");
                sdr.Close();

                string sColName1 = ""; string sColName2 = ""; string sColName3 = "";
                if (ds.Tables["Recv"].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables["Stages"].Rows.Count; i++)
                    {
                        sStageName = ds.Tables["Stages"].Rows[i]["Description"].ToString();
                        int iTemplateId = (int)ds.Tables["Stages"].Rows[i]["TemplateId"];
                        int iFlatId = 0;
                        iStageId = iTemplateId;

                        sColName1 = iStageId + "- Recv";
                        DataColumn col1 = new DataColumn(sColName1) { DataType = typeof(decimal), DefaultValue = 0 };
                        ds.Tables["Flat"].Columns.Add(col1);
             

                        sColName2 = iStageId + "- Recd";
                        DataColumn col2 = new DataColumn(sColName2) { DataType = typeof(decimal), DefaultValue = 0 };
                        ds.Tables["Flat"].Columns.Add(col2);

                        sColName3 = iStageId + "- Bal";
                        DataColumn col3 = new DataColumn(sColName3) { DataType = typeof(decimal), DefaultValue = 0 };
                        ds.Tables["Flat"].Columns.Add(col3);

                        DataTable dtRecv = new DataTable();
                        DataView dv = new DataView(ds.Tables["Recv"]) { RowFilter = String.Format("TemplateId={0} ", iTemplateId) };
                        dtRecv = dv.ToTable();

                        for (int j = 0; j < dtRecv.Rows.Count; j++)
                        {
                            iFlatId = Convert.ToInt32(dtRecv.Rows[j]["FlatId"]);
                            decimal dRecv = Convert.ToDecimal(dtRecv.Rows[j]["Receivable"]);
                            decimal dRecd = Convert.ToDecimal(dtRecv.Rows[j]["Received"]);
                            decimal dBal = Convert.ToDecimal(dtRecv.Rows[j]["Balance"]);

                            drT = ds.Tables["Flat"].Select(String.Format("FlatId={0}", iFlatId));

                            if (drT.Length > 0)
                            {
                                drT[0][sColName1] = dRecv;
                                drT[0][sColName2] = dRecd;
                                drT[0][sColName3] = dBal;
                            }
                        }

                        dtRecv.Dispose();
                        dv.Dispose();
                    }

                    DataColumn col4 = new DataColumn("TotalReceivable") { DataType = typeof(decimal), DefaultValue = 0 };
                    ds.Tables["Flat"].Columns.Add(col4);

                    DataColumn col5 = new DataColumn("TotalReceived") { DataType = typeof(decimal), DefaultValue = 0 };
                    ds.Tables["Flat"].Columns.Add(col5);

                    DataColumn col6 = new DataColumn("Balance") { DataType = typeof(decimal), DefaultValue = 0 };
                    ds.Tables["Flat"].Columns.Add(col6);


                    for (int i = 0; i < ds.Tables["Flat"].Rows.Count; i++)
                    {
                        dTotalRecv = 0;
                        dTotalRevd = 0;

                        ////int j = 3;
                        //int j = 5;

                        //while (j < ds.Tables["Flat"].Columns.Count)
                        //{
                        //    dTotalRecv = dTotalRecv + (decimal)ds.Tables["Flat"].Rows[i][j];
                        //    if (j == 5)
                        //        dTotalRevd = dTotalRevd + (decimal)ds.Tables["Flat"].Rows[i][j] + (decimal)ds.Tables["Flat"].Rows[i][2];
                        //    else
                        //        dTotalRevd = dTotalRevd + (decimal)ds.Tables["Flat"].Rows[i][j + 1];

                        //    j = j + 3;
                        //}
                        int j = 5;

                        while (j < ds.Tables["Flat"].Columns.Count)
                        {
                            dTotalRecv = dTotalRecv + (decimal)ds.Tables["Flat"].Rows[i][j];
                            if (j == 6)
                                dTotalRevd = dTotalRevd + (decimal)ds.Tables["Flat"].Rows[i][j]; //+ (decimal)ds.Tables["Flat"].Rows[i][3];
                            else
                                dTotalRevd = dTotalRevd + (decimal)ds.Tables["Flat"].Rows[i][j + 1];

                            j = j + 3;
                        }

                        ds.Tables["Flat"].Rows[i]["TotalReceivable"] = dTotalRecv;
                        ds.Tables["Flat"].Rows[i]["TotalReceived"] = (decimal)ds.Tables["Flat"].Rows[i]["O/B"] + (decimal)ds.Tables["Flat"].Rows[i]["Advance"] + (decimal)ds.Tables["Flat"].Rows[i]["ExtraBillAmt"] + dTotalRevd;
                        ds.Tables["Flat"].Rows[i]["Balance"] = (decimal)ds.Tables["Flat"].Rows[i]["TotalReceivable"] - (decimal)ds.Tables["Flat"].Rows[i]["TotalReceived"];
                    }                    

                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            BsfGlobal.g_CRMDB.Close();
            return ds;
        }

        public static DataSet GetBuyerStageRecReport(int argCCId, int argPayTypeId, DateTime argDate, int argFromActual)
        {
            BsfGlobal.OpenCRMDB();
            DataSet ds = new DataSet();
            SqlCommand cmd;
            SqlDataReader sdr;
            decimal dTotalRecv = 0;
            decimal dTotalRevd = 0;
            string sFromActual = string.Empty;
            string sStageName = "";
            int iStageId = 0;
            String sSql = string.Empty;
            DataRow[] drT;
            DataTable dt;
            int iCRMRecv;
            string sCond = string.Empty;

            try
            {
                sSql = "Select CRMReceivable From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CostCentreId=" + argCCId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    iCRMRecv = Convert.ToInt32(dt.Rows[0]["CRMReceivable"]);
                }
                else
                {
                    iCRMRecv = 0;
                }

                if (iCRMRecv == 1)
                {
                    sCond = "AND PSF.StageDetId<>0";
                }
                else { sCond = ""; }
                dt.Dispose();

                sSql = "Select BM.BlockId,BM.BlockName,B.FlatId,LR.LeadName BuyerName,B.FlatNo,[O/B]= -1*B.OBReceipt,Advance=ISNULL(SUM(Advance),0), " +
                        " ExtraBillAmt=ISNULL(SUM(A.ExtraBillAmt),0) FROM (  " +
                        " SELECT FD.FlatId,FD.FlatNo,OBReceipt=0,Advance=RT.Amount,ExtraBillAmt=0 FROM ReceiptTrans RT " +
                        " LEFT JOIN ReceiptRegister RR ON RR.ReceiptId=RT.ReceiptId  INNER JOIN FlatDetails FD ON FD.FlatId=RT.FlatId  " +
                        " WHERE  PaymentAgainst='A' And ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND FD.CostCentreId=" + argCCId + "  " +
                        " UNION ALL  " +
                        " SELECT FD.FlatId,FD.FlatNo,-1*OBReceipt,0,0 FROM FlatDetails FD WHERE FD.CostCentreId=" + argCCId + " " +
                        " UNION All " +
                        " SELECT FD.FlatId,FD.FlatNo,OBReceipt=0,Advance=0,ExtraBillAmt=RT.Amount FROM ReceiptTrans RT LEFT JOIN ReceiptRegister RR " +
                        " ON RR.ReceiptId=RT.ReceiptId INNER JOIN FlatDetails FD ON FD.FlatId=RT.FlatId WHERE ReceiptType='ExtraBill' " +
                        " And ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND FD.CostCentreId=" + argCCId + " " +
                        " ) A  INNER JOIN FlatDetails B ON B.FlatId=A.FlatId INNER JOIN BlockMaster BM ON BM.BlockId=B.BlockId  " +
                        " INNER JOIN LeadRegister LR ON LR.LeadId=B.LeadId " +
                        " INNER JOIN dbo.LevelMaster LM ON LM.LevelId=B.LevelId " +
                        " Where B.LeadId<>0 And B.CostCentreId=" + argCCId + " " +
                        " GROUP BY BM.SortOrder,LM.SortOrder,B.SortOrder,BM.BlockId,BM.BlockName,B.FlatId,LR.LeadName,B.FlatNo,B.OBReceipt " +
                        " Order By BM.SortOrder,LM.SortOrder,B.SortOrder,dbo.Val(B.FlatNo) ";
                        //" Where B.LeadId<>0 And B.CostCentreId=" + argCCId + "  " +
                        //" GROUP BY BM.BlockId,BM.BlockName,B.FlatId,LR.LeadName,B.FlatNo,B.OBReceipt";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                ds.Load(sdr, LoadOption.OverwriteChanges, "Flat");
                sdr.Close();

                sSql = "Select TemplateId,Description From PaymentSchedule Where CostCentreId=" + argCCId + " And TypeId=" + argPayTypeId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();

                ds.Load(sdr, LoadOption.OverwriteChanges, "Stages");
                sdr.Close();

                if (argFromActual == 0)
                {
                    sFromActual = " Select FD.FlatId,PS.TemplateId,0,Sum(RT.Amount)Received From ReceiptTrans RT " +
                                " INNER JOIN ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                " INNER JOIN PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                " INNER JOIN FlatDetails FD ON FD.FlatId=PSF.FlatId" +
                                " RIGHT JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                " WHERE RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PSF.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + " " +
                                " AND RR.Cancel=0 GROUP BY FD.FlatId ,PS.TemplateId";
                }
                else if (argFromActual == 1)
                {

                    sFromActual = " Select FD.FlatId,PS.TemplateId,0,Sum(RT.Amount)Received From ReceiptTrans RT " +
                                 " INNER JOIN ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                 " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=RR.ReceiptId " +
                                 " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=RT.CostCentreId " +
                                 " INNER JOIN PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                 " INNER JOIN FlatDetails FD ON FD.FlatId=PSF.FlatId" +
                                 " RIGHT JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                 " WHERE O.CRMActual=1 AND R.Cancel=0 And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PSF.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + " " +
                                 " GROUP BY FD.FlatId ,PS.TemplateId";
                }
                else
                {
                    sFromActual = " Select FD.FlatId,PS.TemplateId,0,Sum(RT.Amount)Received From ReceiptTrans RT " +
                                " INNER JOIN ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=RR.ReceiptId " +
                                " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=RT.CostCentreId " +
                                " INNER JOIN PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                " INNER JOIN FlatDetails FD ON FD.FlatId=PSF.FlatId" +
                                " RIGHT JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                " WHERE O.CRMActual=1 AND R.BRS=1 And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PSF.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + " " +
                                " GROUP BY FD.FlatId ,PS.TemplateId";
                }

                sSql = "SELECT B.FlatId,B.FlatNo,B.BlockId,D.BlockName,C.LeadName BuyerName,TemplateId, SUM(Receivable) Receivable,   SUM(Received) Received ,SUM(Receivable)-SUM(Received) Balance " +
                        " FROM  ( " +
                        " SELECT FD.FlatId,PS.TemplateId,SUM(PB.NetAmount) Receivable,0 Received   " +
                        " FROM ProgressBillRegister PB INNER JOIN FlatDetails FD ON FD.FlatId=PB.FlatId  " +
                        " INNER JOIN BuyerDetail BD ON PB.FlatId=BD.FlatId  INNER JOIN LeadRegister LR ON LR.LeadId=FD.LeadId  " +
                        " INNER JOIN PaymentScheduleFlat PSF ON PSF.PaymentSchId=PB.PaySchId " +
                        " INNER JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId " +
                        " WHERE PSF.BillPassed=1 And PBDate<='" + argDate.ToString("dd-MMM-yyyy") + "' And FD.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + " GROUP BY FD.FlatId,PS.TemplateId " +
                        " UNION ALL   " +
                        " SELECT FD.FlatId,PS.TemplateId,SUM(NetAmount) Receivable,0    " +
                        " FROM PaymentScheduleFlat PSF INNER JOIN FlatDetails FD ON PSF.FlatId=FD.FlatId  " +
                        " INNER JOIN BuyerDetail BD ON PSF.FlatId=BD.FlatId   INNER JOIN LeadRegister LR ON LR.LeadId=BD.LeadId  " +
                        " INNER JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId " +
                        " WHERE PSF.BillPassed=0 And PSF.SchDate<='" + argDate.ToString("dd-MMM-yyyy") + "' " + sCond + " AND FD.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + "  " +
                        " GROUP BY FD.FlatId ,PS.TemplateId " +
                        " UNION ALL   " +
                        " " + sFromActual + " " +
                    //" Select FD.FlatId,PS.TemplateId,0,Sum(RT.Amount)Received From ReceiptTrans RT  " +
                    //" INNER JOIN ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId " +
                    //" INNER JOIN PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                    //" INNER JOIN FlatDetails FD ON FD.FlatId=PSF.FlatId" +
                    //" INNER JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId " +
                    //" WHERE RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND FD.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + "  " +
                    //" GROUP BY FD.FlatId ,PS.TemplateId " +
                        " ) A  " +
                        " INNER JOIN dbo.FlatDetails B ON A.FlatId=B.FlatId    " +
                        " INNER JOIN dbo.LeadRegister C On C.LeadId=B.LeadId " +
                        " INNER JOIN dbo.BlockMaster D On D.BlockId=B.BlockId" +
                        " GROUP BY B.FlatId,B.FlatNo,B.BlockId,D.BlockName,C.LeadName,TemplateId";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();

                ds.Load(sdr, LoadOption.OverwriteChanges, "Recv");
                sdr.Close();

                string sColName1 = ""; string sColName2 = ""; string sColName3 = "";
                if (ds.Tables["Recv"].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables["Stages"].Rows.Count; i++)
                    {
                        sStageName = ds.Tables["Stages"].Rows[i]["Description"].ToString();
                        int iTemplateId = (int)ds.Tables["Stages"].Rows[i]["TemplateId"];
                        int iFlatId = 0;
                        iStageId = iTemplateId;

                        sColName1 = iStageId + "- Recv";
                        DataColumn col1 = new DataColumn(sColName1) { DataType = typeof(decimal), DefaultValue = 0 };
                        ds.Tables["Flat"].Columns.Add(col1);

                        sColName2 = iStageId + "- Recd";
                        DataColumn col2 = new DataColumn(sColName2) { DataType = typeof(decimal), DefaultValue = 0 };
                        ds.Tables["Flat"].Columns.Add(col2);

                        sColName3 = iStageId + "- Bal";
                        DataColumn col3 = new DataColumn(sColName3) { DataType = typeof(decimal), DefaultValue = 0 };
                        ds.Tables["Flat"].Columns.Add(col3);

                        DataTable dtRecv = new DataTable();
                        DataView dv = new DataView(ds.Tables["Recv"]) { RowFilter = String.Format("TemplateId={0} ", iTemplateId) };
                        dtRecv = dv.ToTable();

                        for (int j = 0; j < dtRecv.Rows.Count; j++)
                        {
                            iFlatId = Convert.ToInt32(dtRecv.Rows[j]["FlatId"]);
                            decimal dRecv = Convert.ToDecimal(dtRecv.Rows[j]["Receivable"]);
                            decimal dRecd = Convert.ToDecimal(dtRecv.Rows[j]["Received"]);
                            decimal dBal = Convert.ToDecimal(dtRecv.Rows[j]["Balance"]);

                            drT = ds.Tables["Flat"].Select(String.Format("FlatId={0}", iFlatId));

                            if (drT.Length > 0)
                            {
                                drT[0][sColName1] = dRecv;
                                drT[0][sColName2] = dRecd;
                                drT[0][sColName3] = dBal;
                            }
                        }

                        dtRecv.Dispose();
                        dv.Dispose();
                    }

                    DataColumn col4 = new DataColumn("TotalReceivable") { DataType = typeof(decimal), DefaultValue = 0 };
                    ds.Tables["Flat"].Columns.Add(col4);

                    DataColumn col5 = new DataColumn("TotalReceived") { DataType = typeof(decimal), DefaultValue = 0 };
                    ds.Tables["Flat"].Columns.Add(col5);

                    DataColumn col6 = new DataColumn("Balance") { DataType = typeof(decimal), DefaultValue = 0 };
                    ds.Tables["Flat"].Columns.Add(col6);


                    for (int i = 0; i < ds.Tables["Flat"].Rows.Count; i++)
                    {
                        dTotalRecv = 0;
                        dTotalRevd = 0;

                        //int j = 6;
                        int j = 8;

                        while (j < ds.Tables["Flat"].Columns.Count)
                        {
                            dTotalRecv = dTotalRecv + (decimal)ds.Tables["Flat"].Rows[i][j];
                            if (j == 3)
                                dTotalRevd = dTotalRevd + (decimal)ds.Tables["Flat"].Rows[i][j] + (decimal)ds.Tables["Flat"].Rows[i][2];
                            else
                                dTotalRevd = dTotalRevd + (decimal)ds.Tables["Flat"].Rows[i][j + 1];

                            j = j + 3;
                        }

                        ds.Tables["Flat"].Rows[i]["TotalReceivable"] = dTotalRecv;
                        ds.Tables["Flat"].Rows[i]["TotalReceived"] = (decimal)ds.Tables["Flat"].Rows[i]["O/B"] + (decimal)ds.Tables["Flat"].Rows[i]["Advance"] + (decimal)ds.Tables["Flat"].Rows[i]["ExtraBillAmt"] + dTotalRevd;
                        ds.Tables["Flat"].Rows[i]["Balance"] = (decimal)ds.Tables["Flat"].Rows[i]["TotalReceivable"] - (decimal)ds.Tables["Flat"].Rows[i]["TotalReceived"];
                    } 

                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            BsfGlobal.g_CRMDB.Close();
            return ds;
        }

    }
}
