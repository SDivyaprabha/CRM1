using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace CRM.DataLayer
{
    class ProjReceivableDL
    {
        #region  Projectwise Receivable

        internal static DataTable Get_Project_Receivable(DateTime arg_dAsOn)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            string sActual = string.Empty;
            try
            {
                dt = new DataTable();

                sSql = "SELECT ProjectId, CostCentreName,ProjectDB, SUM(AgreementValue) AgreementValue, SUM(Receivable) ReceivableAsOn, " +
                        " SUM(Received) Received,  SUM(Receivable)-SUM(Received) DueAsOn, SUM(AgreementValue)-SUM(Received) TotalReceivable,Case When SUM(Receivable)<>0 Then (SUM(Receivable)-SUM(Received))/SUM(Receivable)*100 Else 0 End [Recv%] " +
                        " FROM ( " +
                        " SELECT A.CostCentreId ProjectId, SUM(NetAmt)+SUM(A.QualifierAmt) AgreementValue,0 Receivable, 0 Received, 0 Due,0 TotReceivable" +
                        " FROM FlatDetails A INNER JOIN BuyerDetail B ON A.FlatId=B.FlatId INNER JOIN LeadRegister L ON L.LeadId=B.LeadId " +
                        " WHERE A.LeadId<>0 GROUP BY A.CostCentreId  " +
                        " UNION ALL   " +
                        " SELECT A.CostCentreId ProjectId,0 AgreementValue,SUM(NetAmount) Receivable,0 Received,0 Due,0 TotReceivable " +
                        " FROM ProgressBillRegister A INNER JOIN FlatDetails B ON A.FlatId=B.FlatId" +
                        " INNER JOIN BuyerDetail D ON A.FlatId=D.FlatId INNER JOIN LeadRegister L ON L.LeadId=D.LeadId" +
                        " WHERE PBDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dAsOn) + "' GROUP BY A.CostCentreId " +
                        " UNION ALL " +
                        " SELECT A.CostCentreId ProjectId,0 AgreementValue,SUM(NetAmount) Receivable,0,0 Due,0 TotReceivable  " +
                        " FROM PaymentScheduleFlat A INNER JOIN FlatDetails B ON A.FlatId=B.FlatId INNER JOIN BuyerDetail D ON A.FlatId=D.FlatId " +
                        " INNER JOIN LeadRegister L ON L.LeadId=D.LeadId WHERE SchDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dAsOn) + "' " +
                        " AND ((A.CostCentreId IN (SELECT CostCentreId FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CRMReceivable=0)) " +
                        " OR(A.CostCentreId IN (SELECT CostCentreId FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CRMReceivable=1) " +
                        " AND A.StageDetId<>0))  "+
                        " GROUP BY A.CostCentreId  " +
                        " UNION ALL " +
                        " Select CostCentreId,0,0,Sum(Received)Received ,0,0 From( Select C.CostCentreId,(-1*C.OBReceipt)+ISNULL((SELECT SUM(A.Amount) FROM ( " +
                        " SELECT A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                        " WHERE A.FlatId=C.FlatId AND O.CRMActual=0 AND B.Cancel=0 AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dAsOn) + "' ";
                       
                    if (BsfGlobal.g_bFADB==true)
                    {
                        sSql=sSql+ " UNION ALL SELECT SUM(A.Amount) FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                            " INNER JOIN ["+ BsfGlobal.g_sFaDBName +"].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                            " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                            " WHERE A.FlatId=C.FlatId AND O.CRMActual=1 AND R.Cancel=0 AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dAsOn) + "' " +
                            " UNION ALL " +
                            " SELECT SUM(A.Amount) FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                            " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                            " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                            " WHERE A.FlatId=C.FlatId AND O.CRMActual=2 AND R.BRS=1 AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dAsOn) + "' ) A ),0) Received  From FlatDetails C INNER JOIN BuyerDetail D " +
                            " ON D.LeadId=C.LeadId And C.FlatId=D.FlatId And D.Status=C.Status INNER JOIN LeadRegister E ON E.LeadId=D.LeadId " +
                            " ) A Group By CostCentreId  " +
                            " ) A " +
                            " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B ON A.ProjectId=B.CostCentreId  " +
                            " And B.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                            " GROUP BY ProjectId,ProjectDB,B.CostCentreName";
                    }
                    else
                    {
                        sSql = sSql + ") A ),0) Received  From FlatDetails C INNER JOIN BuyerDetail D " +
                               " ON D.LeadId=C.LeadId And C.FlatId=D.FlatId And D.Status=C.Status INNER JOIN LeadRegister E ON E.LeadId=D.LeadId " +
                               " ) A Group By CostCentreId  " +
                               " ) A " +
                               " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B ON A.ProjectId=B.CostCentreId  " +
                               " And B.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                               " GROUP BY ProjectId,ProjectDB,B.CostCentreName";
                    }
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "CRM", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }

            return dt;
        }

        internal static DataTable Get_Block_Receivable(int arg_iProjectId, DateTime arg_dtAsOn)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            int iCRMRecv;
            string sCond = string.Empty;

            try
            {
                sSql = "Select CRMReceivable From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CostCentreId=" + arg_iProjectId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
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
                    sCond = "AND A.StageDetId<>0";
                }
                else { sCond = ""; }
                dt.Dispose();


                sSql = "SELECT A.BlockId, B.BlockName,  SUM(AgreementVale) AgreementValue, SUM(Receivable) ReceivableAsOn, " +
                        " SUM(Received) Received, SUM(Receivable)-SUM(Received) DueAsOn, SUM(AgreementVale)-SUM(Received) TotalReceivable, Case When SUM(Receivable)<>0 Then (SUM(Receivable)-SUM(Received))/SUM(Receivable)*100 Else 0 End [Recv%] " +
                        " FROM ( " +
                        " SELECT BlockId, SUM(NetAmt)+SUM(A.QualifierAmt) AgreementVale, 0 Receivable, 0 Received, 0 Due,0 TotReceivable  " +
                        " FROM FlatDetails A INNER JOIN BuyerDetail B ON A.FlatId=B.FlatId INNER JOIN LeadRegister L ON L.LeadId=B.LeadId" +
                        " WHERE A.LeadId<>0 AND A.CostCentreId=" + arg_iProjectId + " GROUP BY BlockId   " +
                        " UNION ALL   " +
                        " SELECT BlockId,0, SUM(NetAmount),0,0,0 FROM ProgressBillRegister A" +
                        " INNER JOIN FlatDetails B ON A.FlatId=B.FlatId " +
                        " INNER JOIN BuyerDetail D ON A.FlatId=D.FlatId INNER JOIN LeadRegister L ON L.LeadId=D.LeadId " +
                        " WHERE A.CostCentreId=" + arg_iProjectId + " AND A.PBDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' GROUP BY B.BlockId  " +
                        " Union All " +
                        " SELECT BlockId,0, SUM(NetAmount),0,0,0 FROM PaymentScheduleFlat A INNER JOIN FlatDetails B ON A.FlatId=B.FlatId  " +
                        " INNER JOIN BuyerDetail D ON A.FlatId=D.FlatId INNER JOIN LeadRegister L ON L.LeadId=D.LeadId  " +
                        " WHERE A.CostCentreId=" + arg_iProjectId + " AND A.SchDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' " + sCond + " GROUP BY B.BlockId   " +
                        " Union All" +
                        " Select BlockId,0,0,Sum(Received)Received ,0,0 From( Select BlockId,(-1*C.OBReceipt)+ISNULL((SELECT SUM(A.Amount) FROM ( " +
                        " SELECT A.Amount FROM ReceiptTrans A  INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                        " WHERE A.FlatId=C.FlatId AND O.CRMActual=0 AND B.Cancel=0 AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' ";
                        
                if (BsfGlobal.g_bFADB == true)
                {
                    sSql = sSql + " UNION ALL SELECT SUM(A.Amount) FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE A.FlatId=C.FlatId AND O.CRMActual=1 AND R.Cancel=0 AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' " +
                    " UNION ALL " +
                    " SELECT SUM(A.Amount) FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE A.FlatId=C.FlatId AND O.CRMActual=2 AND R.BRS=1 AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' ) A ),0) Received  From FlatDetails C " +
                    " INNER JOIN BuyerDetail D ON D.LeadId=C.LeadId And C.FlatId=D.FlatId And D.Status=C.Status   " +
                    " INNER JOIN LeadRegister E ON E.LeadId=D.LeadId Where C.CostCentreId=" + arg_iProjectId + " ) A Group By BlockId  " +
                    " ) A " +
                    " INNER JOIN BlockMaster B  ON A.BlockId=B.BlockId GROUP BY A.BlockId, B.BlockName ";
                }
                else
                {
                    sSql = sSql + ") A ),0) Received  From FlatDetails C " +
                        " INNER JOIN BuyerDetail D ON D.LeadId=C.LeadId And C.FlatId=D.FlatId And D.Status=C.Status   " +
                        " INNER JOIN LeadRegister E ON E.LeadId=D.LeadId Where C.CostCentreId=" + arg_iProjectId + " ) A Group By BlockId  " +
                        " ) A " +
                        " INNER JOIN BlockMaster B  ON A.BlockId=B.BlockId GROUP BY A.BlockId, B.BlockName ";
                }
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "CRM", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }

            return dt;
        }

        internal static DataTable Get_Flat_Receivable(int argCCId, int arg_iBlockId, DateTime arg_dtAsOn)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            int iCRMRecv;
            string sCond = string.Empty;

            try
            {
                sSql = "Select CRMReceivable From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CostCentreId=" + argCCId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
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
                    sCond = "AND A.StageDetId<>0";
                }
                else { sCond = ""; }
                dt.Dispose();

                sSql = "SELECT A.FlatId,A.FlatNo ,BuyerName,A.Type,  SUM(AgreementVale) AgreementValue,  SUM(Receivable) ReceivableAsOn," +
                        " SUM(Received) Received,  SUM(Receivable)-SUM(Received) DueAsOn, SUM(AgreementVale)-SUM(Received) TotalReceivable, Case When SUM(Receivable)<>0 Then (SUM(Receivable)-SUM(Received))/SUM(Receivable)*100 Else 0 End [Recv%]  FROM ( " +
                        " SELECT A.FlatId, A.FlatNo, L.LeadName BuyerName, CASE WHEN A.Status='S' THEN 'Buyer' ELSE 'Investor' END Type,  " +
                        " A.NetAmt+A.QualifierAmt AgreementVale, 0 Receivable, 0 Received, 0 Due,0 TotReceivable FROM FlatDetails A  " +
                        " INNER JOIN BuyerDetail B ON A.FlatId=B.FlatId    " +
                        " INNER JOIN LeadRegister L ON L.LeadId=B.LeadId WHERE BlockId=" + arg_iBlockId + " And A.LeadId<>0   " +
                        " UNION ALL   " +
                        " SELECT A.FlatId,B.FlatNo,L.LeadName BuyerName, CASE WHEN B.Status='S' THEN 'Buyer' ELSE 'Investor' END Type,0,   " +
                        " SUM(NetAmount),0,0,0 FROM ProgressBillRegister A INNER JOIN  FlatDetails B ON A.FlatId=B.FlatId   " +
                        " INNER JOIN BuyerDetail D ON A.FlatId=D.FlatId  INNER JOIN LeadRegister L ON L.LeadId=D.LeadId  " +
                        " WHERE B.BlockId=" + arg_iBlockId + " AND A.PBDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' GROUP BY A.FlatId,B.FlatNo,L.LeadName, B.Status" +
                        " UNION ALL " +
                        " SELECT A.FlatId,B.FlatNo,L.LeadName BuyerName, CASE WHEN B.Status='S' THEN 'Buyer' ELSE 'Investor' END Type,0,    " +
                        " SUM(NetAmount),0,0,0 FROM PaymentScheduleFlat A INNER JOIN  FlatDetails B ON A.FlatId=B.FlatId    " +
                        " INNER JOIN BuyerDetail D ON A.FlatId=D.FlatId  INNER JOIN LeadRegister L ON L.LeadId=D.LeadId   " +
                        " WHERE B.BlockId=" + arg_iBlockId + " And BillPassed=0 AND A.SchDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' " + sCond + " GROUP BY A.FlatId,B.FlatNo,L.LeadName, B.Status " +
                        " UNION ALL " +
                        " Select D.FlatId,C.FlatNo,E.LeadName BuyerName, CASE WHEN D.Status='S' THEN 'Buyer' ELSE 'Investor' END Type,0,0,   " +
                        " (-1 *OBReceipt)+ISNULL((SELECT SUM(A.Amount) FROM ( " +
                        " SELECT A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                        " WHERE A.FlatId=C.FlatId AND O.CRMActual=0 AND B.Cancel=0 AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' ";
                       
                if (BsfGlobal.g_bFADB == true)
                {
                    sSql = sSql + " UNION ALL SELECT SUM(A.Amount) FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE A.FlatId=C.FlatId AND O.CRMActual=1 AND R.Cancel=0 AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' " +
                    " UNION ALL " +
                    " SELECT SUM(A.Amount) FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE A.FlatId=C.FlatId AND O.CRMActual=2 AND R.BRS=1 AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' ) A ),0) Received,0,0  from FlatDetails C " +
                    " INNER JOIN BuyerDetail D ON D.LeadId=C.LeadId And C.FlatId=D.FlatId And D.Status=C.Status   " +
                    " INNER JOIN LeadRegister E ON E.LeadId=D.LeadId Where BlockId=" + arg_iBlockId + " " +
                    " ) A " +
                    " INNER JOIN dbo.FlatDetails FD On FD.FlatId=A.FlatId " +
                    " INNER JOIN dbo.BlockMaster BM On BM.BlockId=FD.BlockId " +
                    " INNER JOIN dbo.LevelMaster LM ON LM.LevelId=FD.LevelId " +
                    " GROUP BY BM.SortOrder,LM.SortOrder,FD.SortOrder,A.FlatId,A.FlatNo,BuyerName,A.Type " +
                    " Order By BM.SortOrder,LM.SortOrder,FD.SortOrder,dbo.Val(A.FlatNo) ";
                    //" GROUP BY A.FlatId,A.FlatNo,BuyerName,A.Type";
                }
                else
                {
                    sSql = sSql + ") A ),0) Received,0,0  from FlatDetails C " +
                            " INNER JOIN BuyerDetail D ON D.LeadId=C.LeadId And C.FlatId=D.FlatId And D.Status=C.Status   " +
                            " INNER JOIN LeadRegister E ON E.LeadId=D.LeadId Where BlockId=" + arg_iBlockId + " " +
                            " ) A " +
                            " INNER JOIN dbo.FlatDetails FD On FD.FlatId=A.FlatId " +
                            " INNER JOIN dbo.BlockMaster BM On BM.BlockId=FD.BlockId " +
                            " INNER JOIN dbo.LevelMaster LM ON LM.LevelId=FD.LevelId " +
                            " GROUP BY BM.SortOrder,LM.SortOrder,FD.SortOrder,A.FlatId,A.FlatNo,BuyerName,A.Type " +
                            " Order By BM.SortOrder,LM.SortOrder,FD.SortOrder,dbo.Val(A.FlatNo) ";
                    //" GROUP BY A.FlatId,A.FlatNo,BuyerName,A.Type";
                }
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "CRM", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }

            return dt;
        }

        internal static DataTable Get_Flat_ReceivableReport(int argCCId,DateTime arg_dtAsOn)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            int iCRMRecv;
            string sCond = string.Empty;

            try
            {
                sSql = "Select CRMReceivable From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CostCentreId=" + argCCId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
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
                    sCond = "AND A.StageDetId<>0";
                }
                else { sCond = ""; }
                dt.Dispose();

                sSql = "SELECT A.CostCentreId,A.BlockName,A.BlockId,A.FlatId,A.FlatNo ,BuyerName,A.Type,  SUM(AgreementVale) AgreementValue,  SUM(Receivable) ReceivableAsOn," +
                        " SUM(Received) Received, SUM(Receivable)-SUM(Received) DueAsOn, SUM(AgreementVale)-SUM(Received) TotalReceivable, Case When SUM(Receivable)<>0 Then (SUM(Receivable)-SUM(Received))/SUM(Receivable)*100 Else 0 End [Recv%]  FROM ( " +
                        " SELECT A.CostCentreId,BlockName,A.BlockId,A.FlatId, A.FlatNo, L.LeadName BuyerName, CASE WHEN A.Status='S' THEN 'Buyer' ELSE 'Investor' END Type,  " +
                        " A.NetAmt+A.QualifierAmt AgreementVale, 0 Receivable, 0 Received, 0 Due,0 TotReceivable FROM FlatDetails A  " +
                        " INNER JOIN BuyerDetail B ON A.FlatId=B.FlatId    " +
                        " INNER JOIN LeadRegister L ON L.LeadId=B.LeadId INNER JOIN BlockMaster M ON M.BlockId=A.BlockId WHERE A.CostCentreId=" + argCCId + " And A.LeadId<>0   " +
                        " UNION ALL   " +
                        " SELECT B.CostCentreId,BlockName,B.BlockId,A.FlatId,B.FlatNo,L.LeadName BuyerName, CASE WHEN B.Status='S' THEN 'Buyer' ELSE 'Investor' END Type,0,   " +
                        " SUM(NetAmount),0,0,0 FROM ProgressBillRegister A INNER JOIN  FlatDetails B ON A.FlatId=B.FlatId   " +
                        " INNER JOIN BuyerDetail D ON A.FlatId=D.FlatId  INNER JOIN LeadRegister L ON L.LeadId=D.LeadId  " +
                        " INNER JOIN BlockMaster M ON M.BlockId=B.BlockId " +
                        " WHERE B.CostCentreId=" + argCCId + " And A.PBDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' GROUP BY B.CostCentreId,BlockName,B.BlockId,A.FlatId,B.FlatNo,L.LeadName, B.Status" +
                        " UNION ALL " +
                        " SELECT A.CostCentreId,BlockName,B.BlockId,A.FlatId,B.FlatNo,L.LeadName BuyerName, CASE WHEN B.Status='S' THEN 'Buyer' ELSE 'Investor' END Type,0,    " +
                        " SUM(NetAmount),0,0,0 FROM PaymentScheduleFlat A INNER JOIN  FlatDetails B ON A.FlatId=B.FlatId    " +
                        " INNER JOIN BuyerDetail D ON A.FlatId=D.FlatId  INNER JOIN LeadRegister L ON L.LeadId=D.LeadId   " +
                        " INNER JOIN BlockMaster M ON M.BlockId=B.BlockId " +
                        " WHERE B.CostCentreId=" + argCCId + " And A.SchDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' " + sCond + " GROUP BY A.CostCentreId,BlockName,B.BlockId,A.FlatId,B.FlatNo,L.LeadName, B.Status " +
                        " UNION ALL " +
                        " Select C.CostCentreId,BM.BlockName,BM.BlockId,D.FlatId,C.FlatNo,E.LeadName BuyerName, CASE WHEN D.Status='S' THEN 'Buyer' ELSE 'Investor' END Type,0,0,   " +
                        " (-1*OBReceipt)+ISNULL((SELECT SUM(A.Amount) FROM ( " +
                        " SELECT A.Amount FROM ReceiptTrans A  INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                        " WHERE A.FlatId=C.FlatId AND O.CRMActual=0 AND B.Cancel=0 AND B.ReceiptDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' " +
                        " UNION ALL ";
                if (BsfGlobal.g_bFADB == true)
                {
                    sSql = sSql + " SELECT SUM(A.Amount) FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE A.FlatId=C.FlatId AND O.CRMActual=1 AND R.Cancel=0 AND B.ReceiptDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' " +
                    " UNION ALL " +
                    " SELECT SUM(A.Amount) FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE A.FlatId=C.FlatId AND O.CRMActual=2 AND R.BRS=1 AND B.ReceiptDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' ) A ),0) Received,0,0  from FlatDetails C " +
                    " INNER JOIN BuyerDetail D ON D.LeadId=C.LeadId And C.FlatId=D.FlatId And D.Status=C.Status   " +
                    " INNER JOIN LeadRegister E ON E.LeadId=D.LeadId INNER JOIN BlockMaster BM ON BM.BlockId=C.BlockId Where C.CostCentreId=" + argCCId + " " +
                    " ) A " +
                    " INNER JOIN dbo.FlatDetails FD On FD.FlatId=A.FlatId " +
                    " INNER JOIN dbo.BlockMaster BM On BM.BlockId=FD.BlockId " +
                    " INNER JOIN dbo.LevelMaster LM ON LM.LevelId=FD.LevelId " +
                    " Where A.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ")  " +
                    " GROUP BY BM.SortOrder,LM.SortOrder,FD.SortOrder,A.CostCentreId,A.BlockName,A.BlockId,A.FlatId,A.FlatNo,BuyerName,A.Type " +
                    " Order By BM.SortOrder,LM.SortOrder,FD.SortOrder,dbo.Val(A.FlatNo)";
                    //" Where A.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                    //" GROUP BY A.CostCentreId,BlockName,BlockId,A.FlatId,A.FlatNo,BuyerName,A.Type";
                }
                else
                {
                    sSql = sSql + " SELECT SUM(A.Amount) FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                        " WHERE A.FlatId=C.FlatId AND B.ReceiptDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' " +
                        " UNION ALL " +
                        " SELECT SUM(A.Amount) FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                        " WHERE A.FlatId=C.FlatId AND B.ReceiptDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' ) A ),0) Received,0,0  from FlatDetails C " +
                        " INNER JOIN BuyerDetail D ON D.LeadId=C.LeadId And C.FlatId=D.FlatId And D.Status=C.Status   " +
                        " INNER JOIN LeadRegister E ON E.LeadId=D.LeadId INNER JOIN BlockMaster BM ON BM.BlockId=C.BlockId Where C.CostCentreId=" + argCCId + " " +
                        " ) A " +
                        " INNER JOIN dbo.FlatDetails FD On FD.FlatId=A.FlatId " +
                        " INNER JOIN dbo.BlockMaster BM On BM.BlockId=FD.BlockId " +
                        " INNER JOIN dbo.LevelMaster LM ON LM.LevelId=FD.LevelId " +
                        " Where A.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ")  " +
                        " GROUP BY BM.SortOrder,LM.SortOrder,FD.SortOrder,A.CostCentreId,A.BlockName,A.BlockId,A.FlatId,A.FlatNo,BuyerName,A.Type " +
                        " Order By BM.SortOrder,LM.SortOrder,FD.SortOrder,dbo.Val(A.FlatNo)";
                        //" Where A.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                        //" GROUP BY A.CostCentreId,BlockName,BlockId,A.FlatId,A.FlatNo,BuyerName,A.Type";
                }
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "CRM", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }

            return dt;
        }

        #endregion

        #region Receivable Statement

        internal static DataTable Get_CC_RecStmt(DateTime argStart,DateTime argEnd)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            DateTime FromDate = argStart;
            DateTime ToDate = argEnd;
            decimal dUnit = BsfGlobal.g_iSummaryUnit;
            try
            {
                dt = new DataTable();

                sSql = "Select A.CostCentreId,C.CostCentreName,Sum(A.LastYear)LastYear,Sum(A.Apr" + FromDate.Year + ")Apr" + FromDate.Year + "," +
                    " Sum(A.May" + FromDate.Year + ")May" + FromDate.Year + ",Sum(A.Jun" + FromDate.Year + ")Jun" + FromDate.Year + ",Sum(A.Jul" + FromDate.Year + ")Jul" + FromDate.Year + ",Sum(A.Aug" + FromDate.Year + ")Aug" + FromDate.Year + "," +
                    " Sum(A.Sep" + FromDate.Year + ")Sep" + FromDate.Year + ",Sum(A.Oct" + FromDate.Year + ")Oct" + FromDate.Year + ",Sum(A.Nov" + FromDate.Year + ")Nov" + FromDate.Year + ",Sum(A.Dec" + FromDate.Year + ")Dec" + FromDate.Year + "," +
                    " Sum(A.Jan" + ToDate.Year + ")Jan" + ToDate.Year + ",Sum(A.Feb" + ToDate.Year + ")Feb" + ToDate.Year + ",Sum(A.Mar" + ToDate.Year + ")Mar" + ToDate.Year + ",Sum(A.Total)Total From(" +
                    " Select C.CostCentreId,  " +
                    " Sum(case when SchDate<'" + FromDate.ToString("dd-MMM-yyyy") + "' then NetAmount else 0 end)/" + dUnit + " +(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.CostCentreId=C.CostCentreId) as LastYear,   " +
                    " Sum(case when Month(SchDate)=4 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Apr" + FromDate.Year + ",  " +
                    " Sum(case when Month(SchDate)=5 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as May" + FromDate.Year + ",  " +
                    " Sum(case when Month(SchDate)=6 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Jun" + FromDate.Year + ",  " +
                    " Sum(case when Month(SchDate)=7 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Jul" + FromDate.Year + ",  " +
                    " Sum(case when Month(SchDate)=8 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Aug" + FromDate.Year + ",  " +
                    " Sum(case when Month(SchDate)=9 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Sep" + FromDate.Year + ",  " +
                    " Sum(case when Month(SchDate)=10 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Oct" + FromDate.Year + ",  " +
                    " Sum(case when Month(SchDate)=11 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Nov" + FromDate.Year + ",  " +
                    " Sum(case when Month(SchDate)=12 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Dec" + FromDate.Year + ",  " +
                    " Sum(case when Month(SchDate)=1 AND Year(SchDate)=" + ToDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Jan" + ToDate.Year + ",  " +
                    " Sum(case when Month(SchDate)=2 AND Year(SchDate)=" + ToDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Feb" + ToDate.Year + ",  " +
                    " Sum(case when Month(SchDate)=3 AND Year(SchDate)=" + ToDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Mar" + ToDate.Year + ",  " +
                    " Sum(A.NetAmount)/" + dUnit + " +(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.CostCentreId=C.CostCentreId) Total From dbo.PaymentScheduleFlat A  " +
                    " Inner Join FlatDetails C On C.CostCentreId=A.CostCentreId And A.FlatId=C.FlatId  " +
                    " Inner Join BlockMaster D On D.BlockId=C.BlockId And C.CostCentreId=D.CostCentreId  " +
                    " Inner Join LeadRegister E On E.LeadId=C.LeadId WHERE A.SchDate<='" + ToDate.ToString("dd-MMM-yyyy") + "' " +
                    //" AND C.CostCentreId IN (SELECT CostCentreId FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CRMReceivable=0)  "+
                    " AND ((C.CostCentreId IN (SELECT CostCentreId FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CRMReceivable=0)) " +
                    " OR(C.CostCentreId IN (SELECT CostCentreId FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CRMReceivable=1) " +
                    " AND A.StageDetId<>0))  " +
                    " Group By C.CostCentreId " +
                    " UNION ALL" +
                    " Select D.CostCentreId,  " +
                    " -1*Sum(case when ReceiptDate<'" + FromDate.ToString("dd-MMM-yyyy") + "' then B.Amount else 0 end)/" + dUnit + " as LastYear,  " +
                    " -1*Sum(case when Month(ReceiptDate)=4 AND Year(ReceiptDate)=" + FromDate.Year + "   then B.Amount else 0 end)/" + dUnit + " as Apr" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=5 AND Year(ReceiptDate)=" + FromDate.Year + "   then B.Amount else 0 end)/" + dUnit + " as May" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=6 AND Year(ReceiptDate)=" + FromDate.Year + "   then B.Amount else 0 end)/" + dUnit + " as Jun" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=7 AND Year(ReceiptDate)=" + FromDate.Year + "   then B.Amount else 0 end)/" + dUnit + " as Jul" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=8 AND Year(ReceiptDate)=" + FromDate.Year + "   then B.Amount else 0 end)/" + dUnit + " as Aug" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=9 AND Year(ReceiptDate)=" + FromDate.Year + "   then B.Amount else 0 end)/" + dUnit + " as Sep" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=10 AND Year(ReceiptDate)=" + FromDate.Year + "   then B.Amount else 0 end)/" + dUnit + " as Oct" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=11 AND Year(ReceiptDate)=" + FromDate.Year + "   then B.Amount else 0 end)/" + dUnit + " as Nov" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=12 AND Year(ReceiptDate)=" + FromDate.Year + "   then B.Amount else 0 end)/" + dUnit + " as Dec" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=1 AND Year(ReceiptDate)=" + ToDate.Year + "   then B.Amount else 0 end)/" + dUnit + " as Jan" + ToDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=2 AND Year(ReceiptDate)=" + ToDate.Year + "   then B.Amount else 0 end)/" + dUnit + " as Feb" + ToDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=3 AND Year(ReceiptDate)=" + ToDate.Year + "   then B.Amount else 0 end)/" + dUnit + " as Mar" + ToDate.Year + ", " +
                    " -1*Sum(B.Amount)/" + dUnit + " Total From ( " +
                    " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE O.CRMActual=0 AND B.Cancel=0  AND B.ReceiptDate<='" + ToDate.ToString("dd-MMM-yyyy") + "'";
                if (BsfGlobal.g_bFADB == true)
                {
                    sSql=sSql+" UNION ALL SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE O.CRMActual=1 AND R.Cancel=0 " +
                    " UNION ALL " +
                    " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE  O.CRMActual=2 AND R.BRS=1 " +
                    " )B " +
                    " INNER JOIN FlatDetails D On B.FlatId=D.FlatId  " +
                    " Group By D.CostCentreId) A INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On A.CostCentreId=C.CostCentreId  " +
                    " And C.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                    " Group By A.CostCentreId,C.CostCentreName";
                }
                else
                {
                    sSql = sSql + ")B " +
                        " INNER JOIN FlatDetails D On B.FlatId=D.FlatId  " +
                        " Group By D.CostCentreId) A INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On A.CostCentreId=C.CostCentreId  " +
                        " And C.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                        " Group By A.CostCentreId,C.CostCentreName";
                }
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

        internal static DataTable Get_Block_RecStmt(int argCCId, DateTime argStart, DateTime argEnd)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            DateTime FromDate = argStart;
            DateTime ToDate = argEnd;
            decimal dUnit = BsfGlobal.g_iSummaryUnit;
            int iCRMRecv;
            string sCond = string.Empty;
           
            try
            {
                sSql = "Select CRMReceivable From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CostCentreId=" + argCCId + "";
                SqlCommand cmd = new SqlCommand(sSql,BsfGlobal.g_CRMDB);
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
                    sCond = "AND A.StageDetId<>0";
                }
                else { sCond = ""; }
                dt.Dispose();

                sSql = "Select A.BlockId,E.BlockName,Sum(A.LastYear)LastYear,Sum(A.Apr" + FromDate.Year + ")Apr" + FromDate.Year + "," +
                    " Sum(A.May" + FromDate.Year + ")May" + FromDate.Year + ",Sum(A.Jun" + FromDate.Year + ")Jun" + FromDate.Year + ",Sum(A.Jul" + FromDate.Year + ")Jul" + FromDate.Year + ",Sum(A.Aug" + FromDate.Year + ")Aug" + FromDate.Year + "," +
                    " Sum(A.Sep" + FromDate.Year + ")Sep" + FromDate.Year + ",Sum(A.Oct" + FromDate.Year + ")Oct" + FromDate.Year + ",Sum(A.Nov" + FromDate.Year + ")Nov" + FromDate.Year + ",Sum(A.Dec" + FromDate.Year + ")Dec" + FromDate.Year + "," +
                    " Sum(A.Jan" + ToDate.Year + ")Jan" + ToDate.Year + ",Sum(A.Feb" + ToDate.Year + ")Feb" + ToDate.Year + ",Sum(A.Mar" + ToDate.Year + ")Mar" + ToDate.Year + ",Sum(A.Total)Total From(" +
                    " Select C.BlockId, " +
                    " Sum(case when SchDate<'" + FromDate.ToString("dd-MMM-yyyy") + "' then NetAmount else 0 end)/" + dUnit + "+(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.BlockId=C.BlockId) as LastYear,  " +
                    " Sum(case when Month(SchDate)=4 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Apr" + FromDate.Year + ", " +
                    " Sum(case when Month(SchDate)=5 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as May" + FromDate.Year + ", " +
                    " Sum(case when Month(SchDate)=6 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Jun" + FromDate.Year + ", " +
                    " Sum(case when Month(SchDate)=7 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Jul" + FromDate.Year + ", " +
                    " Sum(case when Month(SchDate)=8 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Aug" + FromDate.Year + ", " +
                    " Sum(case when Month(SchDate)=9 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Sep" + FromDate.Year + ", " +
                    " Sum(case when Month(SchDate)=10 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Oct" + FromDate.Year + ", " +
                    " Sum(case when Month(SchDate)=11 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Nov" + FromDate.Year + ", " +
                    " Sum(case when Month(SchDate)=12 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Dec" + FromDate.Year + ", " +
                    " Sum(case when Month(SchDate)=1 AND Year(SchDate)=" + ToDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Jan" + ToDate.Year + ", " +
                    " Sum(case when Month(SchDate)=2 AND Year(SchDate)=" + ToDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Feb" + ToDate.Year + ", " +
                    " Sum(case when Month(SchDate)=3 AND Year(SchDate)=" + ToDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Mar" + ToDate.Year + ", " +
                    " Sum(A.NetAmount)/" + dUnit + " +(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.BlockId=C.BlockId) Total From dbo.PaymentScheduleFlat A " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B On A.CostCentreId=B.CostCentreId " +
                    " INNER JOIN FlatDetails C On C.CostCentreId=A.CostCentreId And A.FlatId=C.FlatId " +
                    " INNER JOIN LeadRegister E On E.LeadId=C.LeadId Where B.CostCentreId=" + argCCId + " AND A.SchDate<='" + ToDate.ToString("dd-MMM-yyyy") + "'"+
                    " "+ sCond +" Group By C.BlockId" +
                    " UNION ALL" +
                    " Select D.BlockId,  " +
                    " -1*Sum(case when ReceiptDate<'" + FromDate.ToString("dd-MMM-yyyy") + "' then B.Amount else 0 end)/" + dUnit + " as LastYear,  " +
                    " -1*Sum(case when Month(ReceiptDate)=4 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Apr" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=5 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as May" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=6 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Jun" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=7 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Jul" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=8 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Aug" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=9 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Sep" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=10 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Oct" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=11 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Nov" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=12 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Dec" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=1 AND Year(ReceiptDate)=" + ToDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Jan" + ToDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=2 AND Year(ReceiptDate)=" + ToDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Feb" + ToDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=3 AND Year(ReceiptDate)=" + ToDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Mar" + ToDate.Year + ", " +
                    " -1*Sum(B.Amount)/" + dUnit + " Total from ( " +
                    " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A  INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE O.CRMActual=0 AND B.Cancel=0 AND B.ReceiptDate<='" + ToDate.ToString("dd-MMM-yyyy") + "' ";
                if (BsfGlobal.g_bFADB == true)
                {
                    sSql=sSql+" UNION ALL SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE O.CRMActual=1 AND R.Cancel=0 " +
                    " UNION ALL " +
                    " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE  O.CRMActual=2 AND R.BRS=1 " +
                    " )B " +
                    " Inner Join FlatDetails D On B.FlatId=D.FlatId Where D.CostCentreId=" + argCCId + " Group By D.BlockId" +
                    " ) A Inner Join BlockMaster E On A.BlockId=E.BlockId  Group By A.BlockId,E.BlockName Order By E.BlockName";
                }
                else
                {
                    sSql = sSql + ")B " +
                    " Inner Join FlatDetails D On B.FlatId=D.FlatId Where D.CostCentreId=" + argCCId + " Group By D.BlockId" +
                    " ) A Inner Join BlockMaster E On A.BlockId=E.BlockId  Group By A.BlockId,E.BlockName Order by E.BlockName";
                }
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
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

        internal static DataTable Get_Flat_RecStmt(int argCCId, int argBlockId, DateTime argStart, DateTime argEnd)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            DateTime FromDate = argStart;
            DateTime ToDate = argEnd;
            decimal dUnit = BsfGlobal.g_iSummaryUnit;
            int iCRMRecv;
            string sCond = string.Empty;

            try
            {
                sSql = "Select CRMReceivable From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CostCentreId=" + argCCId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
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
                    sCond = "AND A.StageDetId<>0";
                }
                else { sCond = ""; }
                dt.Dispose();

                sSql = "Select B.FlatNo,C.LeadName BuyerName,Sum(A.LastYear)LastYear,Sum(A.Apr" + FromDate.Year + ")Apr" + FromDate.Year + "," +
                    " Sum(A.May" + FromDate.Year + ")May" + FromDate.Year + ",Sum(A.Jun" + FromDate.Year + ")Jun" + FromDate.Year + ",Sum(A.Jul" + FromDate.Year + ")Jul" + FromDate.Year + ",Sum(A.Aug" + FromDate.Year + ")Aug" + FromDate.Year + "," +
                    " Sum(A.Sep" + FromDate.Year + ")Sep" + FromDate.Year + ",Sum(A.Oct" + FromDate.Year + ")Oct" + FromDate.Year + ",Sum(A.Nov" + FromDate.Year + ")Nov" + FromDate.Year + ",Sum(A.Dec" + FromDate.Year + ")Dec" + FromDate.Year + "," +
                    " Sum(A.Jan" + ToDate.Year + ")Jan" + ToDate.Year + ",Sum(A.Feb" + ToDate.Year + ")Feb" + ToDate.Year + ",Sum(A.Mar" + ToDate.Year + ")Mar" + ToDate.Year + ",Sum(A.Total)Total From(" +
                    " Select C.FlatId, " +
                    " Sum(case when SchDate<'" + FromDate.ToString("dd-MMM-yyyy") + "' then NetAmount else 0 end)/" + dUnit + " +(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.FlatId=C.FlatId) as LastYear,  " +
                    " Sum(case when Month(SchDate)=4 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Apr" + FromDate.Year + ", " +
                    " Sum(case when Month(SchDate)=5 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as May" + FromDate.Year + ", " +
                    " Sum(case when Month(SchDate)=6 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Jun" + FromDate.Year + ", " +
                    " Sum(case when Month(SchDate)=7 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Jul" + FromDate.Year + ", " +
                    " Sum(case when Month(SchDate)=8 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Aug" + FromDate.Year + ", " +
                    " Sum(case when Month(SchDate)=9 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Sep" + FromDate.Year + ", " +
                    " Sum(case when Month(SchDate)=10 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Oct" + FromDate.Year + ", " +
                    " Sum(case when Month(SchDate)=11 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Nov" + FromDate.Year + ", " +
                    " Sum(case when Month(SchDate)=12 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Dec" + FromDate.Year + ", " +
                    " Sum(case when Month(SchDate)=1 AND Year(SchDate)=" + ToDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Jan" + ToDate.Year + ", " +
                    " Sum(case when Month(SchDate)=2 AND Year(SchDate)=" + ToDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Feb" + ToDate.Year + ", " +
                    " Sum(case when Month(SchDate)=3 AND Year(SchDate)=" + ToDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Mar" + ToDate.Year + ", " +
                    " Sum(A.NetAmount)/" + dUnit + " +(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.FlatId=C.FlatId) Total From dbo.PaymentScheduleFlat A " +
                    " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B On A.CostCentreId=B.CostCentreId " +
                    " Inner Join FlatDetails C On C.CostCentreId=A.CostCentreId And A.FlatId=C.FlatId " +
                    " WHERE A.SchDate<='" + ToDate.ToString("dd-MMM-yyyy") + "' AND A.CostCentreId=" + argCCId + " And C.BlockId=" + argBlockId + " "+ sCond +" Group By C.FlatId" +
                    " UNION ALL" +
                    " Select D.FlatId,  " +
                    " -1*Sum(case when ReceiptDate<'" + FromDate.ToString("dd-MMM-yyyy") + "' then B.Amount else 0 end)/" + dUnit + " as LastYear,  " +
                    " -1*Sum(case when Month(ReceiptDate)=4 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Apr" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=5 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as May" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=6 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Jun" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=7 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Jul" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=8 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Aug" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=9 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Sep" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=10 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Oct" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=11 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Nov" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=12 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Dec" + FromDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=1 AND Year(ReceiptDate)=" + ToDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Jan" + ToDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=2 AND Year(ReceiptDate)=" + ToDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Feb" + ToDate.Year + ", " +
                    " -1*Sum(case when Month(ReceiptDate)=3 AND Year(ReceiptDate)=" + ToDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as Mar" + ToDate.Year + ", " +
                    " -1*Sum(B.Amount)/" + dUnit + " Total from ( " +
                    " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A  INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE O.CRMActual=0 AND B.Cancel=0 AND B.ReceiptDate<='" + ToDate.ToString("dd-MMM-yyyy") + "' ";
                if (BsfGlobal.g_bFADB == true)
                {
                    sSql = sSql + " UNION ALL SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE O.CRMActual=1 AND R.Cancel=0 " +
                    " UNION ALL " +
                    " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE  O.CRMActual=2 AND R.BRS=1 " +
                    " ) B " +
                    " Inner Join FlatDetails D On B.FlatId=D.FlatId " +
                    " Where BlockId=" + argBlockId + " Group By D.FlatId)A INNER JOIN FlatDetails B On B.FlatId=A.FlatId " +
                    " INNER JOIN LeadRegister C On C.LeadId=B.LeadId "+
                    " INNER JOIN dbo.BlockMaster BM On BM.BlockId=B.BlockId "+
                    " INNER JOIN dbo.LevelMaster LM On LM.LevelId=B.LevelId "+
                    " Group By BM.SortOrder,LM.SortOrder,B.SortOrder,C.LeadName,B.FlatNo Order By "+
                    " BM.SortOrder,LM.SortOrder,B.SortOrder, "+
                    " dbo.Val(B.FlatNo)";
                    //" Group By C.LeadName,B.FlatNo Order By dbo.Val(B.FlatNo)";
                }
                else
                {
                    sSql = sSql + ") B " +
                        " INNER JOIN FlatDetails D On B.FlatId=D.FlatId " +
                        " Where BlockId=" + argBlockId + " Group By D.FlatId)A INNER JOIN FlatDetails B On B.FlatId=A.FlatId " +
                        " INNER JOIN LeadRegister C On C.LeadId=B.LeadId " +
                        " INNER JOIN dbo.BlockMaster BM On BM.BlockId=B.BlockId " +
                        " INNER JOIN dbo.LevelMaster LM On LM.LevelId=B.LevelId " +
                        " Group By BM.SortOrder,LM.SortOrder,B.SortOrder,C.LeadName,B.FlatNo Order By " +
                        " BM.SortOrder,LM.SortOrder,B.SortOrder, " +
                        " dbo.Val(B.FlatNo)";
                        //" Group By C.LeadName,B.FlatNo Order By dbo.Val(B.FlatNo)";
                }
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
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

        internal static DataTable Get_CC_ActStmt(DateTime argStart, DateTime argEnd)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            DateTime FromDate = argStart;
            DateTime ToDate = argEnd;
            decimal dUnit = BsfGlobal.g_iSummaryUnit;

            try
            {
                dt = new DataTable();

                sSql = "SELECT C.CostCentreId,C.CostCentreName, " +
                        " Sum(case when ReceiptDate<'" + FromDate.ToString("dd-MMM-yyyy") + "' then B.Amount else 0 end)/" + dUnit + " -(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.CostCentreId=C.CostCentreId) as LastYear, " +
                        " Sum(case when Month(ReceiptDate)=4 AND Year(ReceiptDate)=" + FromDate.Year + "  " +
                        " then B.Amount else 0 end)/" + dUnit + " as Apr" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=5 AND Year(ReceiptDate)=" + FromDate.Year + "  " +
                        " then B.Amount else 0 end)/" + dUnit + " as May" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=6 AND Year(ReceiptDate)=" + FromDate.Year + "  " +
                        " then B.Amount else 0 end)/" + dUnit + " as Jun" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=7 AND Year(ReceiptDate)=" + FromDate.Year + "  " +
                        " then B.Amount else 0 end)/" + dUnit + " as Jul" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=8 AND Year(ReceiptDate)=" + FromDate.Year + "  " +
                        " then B.Amount else 0 end)/" + dUnit + " as Aug" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=9 AND Year(ReceiptDate)=" + FromDate.Year + "  " +
                        " then B.Amount else 0 end)/" + dUnit + " as Sep" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=10 AND Year(ReceiptDate)=" + FromDate.Year + "  " +
                        " then B.Amount else 0 end)/" + dUnit + " as Oct" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=11 AND Year(ReceiptDate)=" + FromDate.Year + "  " +
                        " then B.Amount else 0 end)/" + dUnit + " as Nov" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=12 AND Year(ReceiptDate)=" + FromDate.Year + "  " +
                        " then B.Amount else 0 end)/" + dUnit + " as Dec" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=1 AND Year(ReceiptDate)=" + ToDate.Year + "  " +
                        " then B.Amount else 0 end)/" + dUnit + " as Jan" + ToDate.Year + ", Sum(case when Month(ReceiptDate)=2 AND Year(ReceiptDate)=" + ToDate.Year + "  " +
                        " then B.Amount else 0 end)/" + dUnit + " as Feb" + ToDate.Year + ", Sum(case when Month(ReceiptDate)=3 AND Year(ReceiptDate)=" + ToDate.Year + "  " +
                        " then B.Amount else 0 end)/" + dUnit + " as Mar" + ToDate.Year + ", Sum(B.Amount)/" + dUnit + " -(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F " +
                        " WHERE F.CostCentreId=C.CostCentreId) Total FROM ( " +
                        " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                        " WHERE O.CRMActual=0 AND B.Cancel=0 AND B.ReceiptDate<='" + ToDate.ToString("dd-MMM-yyyy") + "' ";
                       
                if (BsfGlobal.g_bFADB == true)
                {
                    sSql=sSql+" UNION ALL SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE O.CRMActual=1 AND R.Cancel=0 " +
                    " UNION ALL " +
                    " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE O.CRMActual=2 AND R.BRS=1 " +
                    " )B " +
                    " INNER JOIN FlatDetails D On B.FlatId=D.FlatId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On D.CostCentreId=C.CostCentreId " +
                    " And C.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                    " Group By C.CostCentreId,C.CostCentreName";
                }
                else
                {
                    sSql = sSql + " )B " +
                    " INNER JOIN FlatDetails D On B.FlatId=D.FlatId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On D.CostCentreId=C.CostCentreId " +
                    " And C.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                    " Group By C.CostCentreId,C.CostCentreName";
                }
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dt);

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

        internal static DataTable Get_Block_ActStmt(int argCCId, DateTime argStart, DateTime argEnd)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            DateTime FromDate = argStart;
            DateTime ToDate = argEnd;
            decimal dUnit = BsfGlobal.g_iSummaryUnit;
            

            try
            {
                sSql = "Select E.BlockId,E.BlockName, " +
                        " Sum(case when ReceiptDate<'" + FromDate.ToString("dd-MMM-yyyy") + "' then B.Amount else 0 end)/" + dUnit + " -(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.BlockId=E.BlockId) as LastYear, " +
                        " Sum(case when Month(ReceiptDate)=4 AND Year(ReceiptDate)=" + FromDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Apr" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=5 AND Year(ReceiptDate)=" + FromDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as May" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=6 AND Year(ReceiptDate)=" + FromDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Jun" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=7 AND Year(ReceiptDate)=" + FromDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Jul" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=8 AND Year(ReceiptDate)=" + FromDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Aug" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=9 AND Year(ReceiptDate)=" + FromDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Sep" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=10 AND Year(ReceiptDate)=" + FromDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Oct" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=11 AND Year(ReceiptDate)=" + FromDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Nov" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=12 AND Year(ReceiptDate)=" + FromDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Dec" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=1 AND Year(ReceiptDate)=" + ToDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Jan" + ToDate.Year + ", Sum(case when Month(ReceiptDate)=2 AND Year(ReceiptDate)=" + ToDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Feb" + ToDate.Year + ", Sum(case when Month(ReceiptDate)=3 AND Year(ReceiptDate)=" + ToDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Mar" + ToDate.Year + ", ISNULL(Sum(B.Amount),0)/" + dUnit + "-(SELECT SUM(OBReceipt)/" + dUnit + " " +
                        " FROM FlatDetails F WHERE F.BlockId=E.BlockId)  Total " +
                        " FROM BlockMaster E INNER JOIN FlatDetails D ON D.BlockId=E.BlockId LEFT JOIN ( " +
                        " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                        " WHERE O.CRMActual=0 AND B.Cancel=0 ";
                        
                if (BsfGlobal.g_bFADB == true)
                {
                    sSql=sSql+" UNION ALL SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE O.CRMActual=1 AND R.Cancel=0 " +
                    " UNION ALL " +
                    " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE  O.CRMActual=2 AND R.BRS=1 " +
                    " )B On B.FlatId=D.FlatId " +
                    " WHERE E.CostCentreId=" + argCCId + " Group By E.BlockId,E.BlockName Order By E.BlockName";
                }
                else
                {
                    sSql = sSql + " )B On B.FlatId=D.FlatId " +
                    " WHERE E.CostCentreId=" + argCCId + " Group By E.BlockId,E.BlockName Order By E.BlockName";
                }
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
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

        internal static DataTable Get_Flat_ActStmt(int argBlockId, DateTime argStart, DateTime argEnd)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            DateTime FromDate = argStart;
            DateTime ToDate = argEnd;
            decimal dUnit = BsfGlobal.g_iSummaryUnit;

            try
            {
                dt = new DataTable();

                sSql = "Select D.FlatNo,L.LeadName BuyerName, " +
                        " Sum(case when ReceiptDate<'" + FromDate.ToString("dd-MMM-yyyy") + "' then B.Amount else 0 end)/" + dUnit + "-(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.FlatId=D.FlatId) as LastYear, " +
                        " Sum(case when Month(ReceiptDate)=4 AND Year(ReceiptDate)=" + FromDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Apr" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=5 AND Year(ReceiptDate)=" + FromDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as May" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=6 AND Year(ReceiptDate)=" + FromDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Jun" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=7 AND Year(ReceiptDate)=" + FromDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Jul" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=8 AND Year(ReceiptDate)=" + FromDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Aug" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=9 AND Year(ReceiptDate)=" + FromDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Sep" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=10 AND Year(ReceiptDate)=" + FromDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Oct" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=11 AND Year(ReceiptDate)=" + FromDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Nov" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=12 AND Year(ReceiptDate)=" + FromDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Dec" + FromDate.Year + ", Sum(case when Month(ReceiptDate)=1 AND Year(ReceiptDate)=" + ToDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Jan" + ToDate.Year + ", Sum(case when Month(ReceiptDate)=2 AND Year(ReceiptDate)=" + ToDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Feb" + ToDate.Year + ", Sum(case when Month(ReceiptDate)=3 AND Year(ReceiptDate)=" + ToDate.Year + " " +
                        " then B.Amount else 0 end)/" + dUnit + " as Mar" + ToDate.Year + ", Sum(IsNull(B.Amount,0))/" + dUnit + "-(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.FlatId=D.FlatId) Total " +
                        " FROM FlatDetails D Inner Join BuyerDetail E ON D.FlatId=E.FlatId   Inner Join LeadRegister L On L.LeadId=D.LeadId " +
                        " LEFT JOIN ( " +
                        " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A  INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                        " WHERE O.CRMActual=0 AND B.Cancel=0 ";
                      
                if (BsfGlobal.g_bFADB == true)
                {
                    sSql = sSql + " UNION ALL SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE O.CRMActual=1 AND R.Cancel=0 " +
                    " UNION ALL " +
                    " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE  O.CRMActual=2 AND R.BRS=1 " +
                    " )B On B.FlatId=D.FlatId " +
                    " INNER JOIN dbo.BlockMaster BM On BM.BlockId=D.BlockId " +
                    " INNER JOIN dbo.LevelMaster LM On LM.LevelId=D.LevelId Where D.BlockId=" + argBlockId + "  " +
                    " GROUP BY BM.SortOrder,LM.SortOrder,D.SortOrder,L.LeadName,D.FlatNo, D.FlatId " +
                    " Order By BM.SortOrder,LM.SortOrder,D.SortOrder,dbo.Val(D.FlatNo) ";
                    //" Where BlockId=" + argBlockId + " GROUP BY L.LeadName,D.FlatNo, D.FlatId Order By dbo.Val(D.FlatNo)";
                }
                else
                {
                    sSql = sSql + ")B On B.FlatId=D.FlatId " +
                        " INNER JOIN dbo.BlockMaster BM On BM.BlockId=D.BlockId " +
                        " INNER JOIN dbo.LevelMaster LM On LM.LevelId=D.LevelId Where D.BlockId=" + argBlockId + "  " +
                        " GROUP BY BM.SortOrder,LM.SortOrder,D.SortOrder,L.LeadName,D.FlatNo, D.FlatId " +
                        " Order By BM.SortOrder,LM.SortOrder,D.SortOrder,dbo.Val(D.FlatNo) ";
                        //" Where BlockId=" + argBlockId + " GROUP BY L.LeadName,D.FlatNo, D.FlatId Order By dbo.Val(D.FlatNo)";
                }
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

        internal static DataTable Get_Flat_RecStmtReport(int argCCId, DateTime argStart, DateTime argEnd)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            DateTime FromDate = argStart;
            DateTime ToDate = argEnd;
            decimal dUnit = BsfGlobal.g_iSummaryUnit;
            int iCRMRecv;
            string sCond = string.Empty;

            try
            {
                sSql = "Select CRMReceivable From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CostCentreId=" + argCCId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
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
                    sCond = "AND A.StageDetId<>0";
                }
                else { sCond = ""; }
                dt.Dispose();

                sSql = "Select D.BlockId,D.BlockName,B.FlatNo,C.LeadName BuyerName," +
                        " Sum(A.LastYearRcvble)Rcvble,Sum(A.LastYearRecvd)+(SELECT SUM(-OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.FlatNo=B.FlatNo And F.BlockId=D.BlockId)Recvd," +
                        " Sum(A.AprRcvble)AprRcvble,Sum(A.AprRecvd)AprRecvd," +
                        " Sum(A.MayRcvble)MayRcvble,Sum(A.MayRecvd)MayRecvd," +
                        " Sum(A.JunRcvble)JunRcvble,Sum(A.JunRecvd)JunRecvd," +
                        " Sum(A.JulRcvble)JulRcvble,Sum(A.JulRecvd)JulRecvd," +
                        " Sum(A.AugRcvble)AugRcvble,Sum(A.AugRecvd)AugRecvd," +
                        " Sum(A.SepRcvble)SepRcvble,Sum(A.SepRecvd)SepRecvd," +
                        " Sum(A.OctRcvble)OctRcvble,Sum(A.OctRecvd)OctRecvd," +
                        " Sum(A.NovRcvble)NovRcvble,Sum(A.NovRecvd)NovRecvd," +
                        " Sum(A.DecRcvble)DecRcvble,Sum(A.DecRecvd)DecRecvd," +
                        " Sum(A.JanRcvble)JanRcvble,Sum(A.JanRecvd)JanRecvd," +
                        " Sum(A.FebRcvble)FebRcvble,Sum(A.FebRecvd)FebRecvd," +
                        " Sum(A.MarRcvble)MarRcvble,Sum(A.MarRecvd)MarRecvd," +
                        " Sum(A.TotalRcvble)TotRcvble,Sum(A.TotalRecvd)+(SELECT SUM(-OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.FlatNo=B.FlatNo And F.BlockId=D.BlockId)TotRecvd From(" +
                        " Select C.FlatId, " +
                        " Sum(case when SchDate<'" + FromDate.ToString("dd-MMM-yyyy") + "' then NetAmount else 0 end)/" + dUnit + " as LastYearRcvble , 0 LastYearRecvd, " +
                        " Sum(case when Month(SchDate)=4 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as AprRcvble, 0 AprRecvd," +
                        " Sum(case when Month(SchDate)=5 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as MayRcvble, 0 MayRecvd," +
                        " Sum(case when Month(SchDate)=6 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as JunRcvble, 0 JunRecvd," +
                        " Sum(case when Month(SchDate)=7 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as JulRcvble, 0 JulRecvd," +
                        " Sum(case when Month(SchDate)=8 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as AugRcvble, 0 AugRecvd," +
                        " Sum(case when Month(SchDate)=9 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as SepRcvble, 0 SepRecvd," +
                        " Sum(case when Month(SchDate)=10 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as OctRcvble,0  OctRecvd," +
                        " Sum(case when Month(SchDate)=11 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as NovRcvble, 0 NovRecvd," +
                        " Sum(case when Month(SchDate)=12 AND Year(SchDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as DecRcvble,0 DecRecvd," +
                        " Sum(case when Month(SchDate)=1 AND Year(SchDate)=" + ToDate.Year + " then NetAmount else 0 end)/" + dUnit + " as JanRcvble, 0 JanRecvd," +
                        " Sum(case when Month(SchDate)=2 AND Year(SchDate)=" + ToDate.Year + " then NetAmount else 0 end)/" + dUnit + " as FebRcvble, 0 FebRecvd," +
                        " Sum(case when Month(SchDate)=3 AND Year(SchDate)=" + ToDate.Year + " then NetAmount else 0 end)/" + dUnit + " as MarRcvble, 0 MarRecvd," +
                        " Sum(A.NetAmount)/" + dUnit + " TotalRcvble,0 TotalRecvd From dbo.PaymentScheduleFlat A " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B On A.CostCentreId=B.CostCentreId " +
                        " Inner Join FlatDetails C On C.CostCentreId=A.CostCentreId And A.FlatId=C.FlatId " +
                        " Where A.CostCentreId=" + argCCId + " "+ sCond +" Group By C.FlatId" +
                        " UNION ALL" +
                        //" SELECT A.FlatId, 0, (A.LastYearRecvd+(-B.OBReceipt/" + dUnit + ")) LastYearRecvd ,0,AprRecvd,0,MayRecvd,0,JunRecvd,0,A.JulRecvd,0,A.AugRecvd," +
                        " SELECT A.FlatId, 0, A.LastYearRecvd,0,AprRecvd,0,MayRecvd,0,JunRecvd,0,A.JulRecvd,0,A.AugRecvd," +
                        " 0,A.SepRecvd,0,A.OctRecvd,0,A.NovRecvd,0,A.DecRecvd,0,A.JanRecvd,0,A.FebRecvd,0,A.MarRecvd,0,TotalRecvd FROM ( " +
                        " Select D.FlatId,  " +
                        " Sum(case when ReceiptDate<'" + FromDate.ToString("dd-MMM-yyyy") + "' then B.Amount else 0 end)/" + dUnit + " as LastYearRecvd,  " +
                        " Sum(case when Month(ReceiptDate)=4 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as AprRecvd, " +
                        " Sum(case when Month(ReceiptDate)=5 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as MayRecvd, " +
                        " Sum(case when Month(ReceiptDate)=6 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as JunRecvd, " +
                        " Sum(case when Month(ReceiptDate)=7 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as JulRecvd, " +
                        " Sum(case when Month(ReceiptDate)=8 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as AugRecvd, " +
                        " Sum(case when Month(ReceiptDate)=9 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as SepRecvd, " +
                        " Sum(case when Month(ReceiptDate)=10 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as OctRecvd, " +
                        " Sum(case when Month(ReceiptDate)=11 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as NovRecvd, " +
                        " Sum(case when Month(ReceiptDate)=12 AND Year(ReceiptDate)=" + FromDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as DecRecvd, " +
                       "  Sum(case when Month(ReceiptDate)=1 AND Year(ReceiptDate)=" + ToDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as JanRecvd, " +
                       "  Sum(case when Month(ReceiptDate)=2 AND Year(ReceiptDate)=" + ToDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as FebRecvd, " +
                       "  Sum(case when Month(ReceiptDate)=3 AND Year(ReceiptDate)=" + ToDate.Year + "  then B.Amount else 0 end)/" + dUnit + " as MarRecvd, " +
                       "  Sum(B.Amount)/" + dUnit + " TotalRecvd from ( " +
                        " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A  INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                        " WHERE O.CRMActual=0 AND B.Cancel=0 " +
                        " UNION ALL ";
                if (BsfGlobal.g_bFADB == true)
                {
                    sSql = sSql + " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE O.CRMActual=1 AND R.Cancel=0 " +
                    " UNION ALL " +
                    " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " WHERE  O.CRMActual=2 AND R.BRS=1 " +
                    " ) B " +
                   "  Inner Join FlatDetails D On B.FlatId=D.FlatId " +
                   "  Where D.CostCentreId=" + argCCId + " Group By D.FlatId ) A INNER JOIN FlatDetails B On B.FlatId=A.FlatId " +
                   "  )A INNER JOIN FlatDetails B On B.FlatId=A.FlatId " +
                   "  INNER JOIN BlockMaster D On D.BlockId=B.BlockId" +
                   "  INNER JOIN LeadRegister C On C.LeadId=B.LeadId " +
                   "  INNER JOIN dbo.LevelMaster LM On LM.LevelId=B.LevelId " +
                   "  And B.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                   "  GROUP BY D.SortOrder,LM.SortOrder,B.SortOrder,C.LeadName,B.FlatNo,D.BlockName,D.BlockId " +
                   "  Order By D.SortOrder,LM.SortOrder,B.SortOrder,dbo.Val(B.FlatNo)";
                   //"  Group By C.LeadName,B.FlatNo,D.BlockName,D.BlockId Order By dbo.Val(B.FlatNo)";
                }
                else
                {
                    sSql = sSql + " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " UNION ALL " +
                    " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                    " ) B " +
                    "  Inner Join FlatDetails D On B.FlatId=D.FlatId " +
                    "  Where D.CostCentreId=" + argCCId + " Group By D.FlatId ) A INNER JOIN FlatDetails B On B.FlatId=A.FlatId " +
                    "  )A INNER JOIN FlatDetails B On B.FlatId=A.FlatId " +
                    "  INNER JOIN BlockMaster D On D.BlockId=B.BlockId" +
                    "  INNER JOIN LeadRegister C On C.LeadId=B.LeadId " +
                    "  INNER JOIN dbo.LevelMaster LM On LM.LevelId=B.LevelId " +
                    "  And B.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                    "  GROUP BY D.SortOrder,LM.SortOrder,B.SortOrder,C.LeadName,B.FlatNo,D.BlockName,D.BlockId " +
                    "  Order By D.SortOrder,LM.SortOrder,B.SortOrder,dbo.Val(B.FlatNo)";
                    //"  Group By C.LeadName,B.FlatNo,D.BlockName,D.BlockId Order By dbo.Val(B.FlatNo)";
                }
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
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

        #endregion

        #region Fiscal Master

        internal static DataTable GetFiscalMaster()
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            try
            {
                dt = new DataTable();
                sSql = "Select * From dbo.FiscalYear";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "CRM", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }

            return dt;
        }

        internal static void InsertFiscalYear(DataTable argdt)
        {
            SqlCommand cmd;
            String sSql = string.Empty;
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            int iFYearId = 0;
            try
            {
                sSql = "Truncate Table dbo.FiscalYear";
                cmd = new SqlCommand(sSql, conn,tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                for (int i = 0; i < argdt.Rows.Count; i++)
                {
                    sSql = "SELECT ISNULL(MAX(FYearId),0)+1 as Id FROM dbo.FiscalYear";
                    cmd = new SqlCommand(sSql, conn, tran);
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    if (dt.Rows.Count > 0) { iFYearId = Convert.ToInt32(dt.Rows[0]["Id"]); }
                    cmd.Dispose();

                    sSql = "Insert Into dbo.FiscalYear (FYearId,FName,StartDate,EndDate) Values (" + iFYearId + ",'" + argdt.Rows[i]["FName"] + "'," +
                             " '" + Convert.ToDateTime(argdt.Rows[i]["StartDate"]).ToString("dd-MMM-yyyy") + "'," +
                             " '" + Convert.ToDateTime(argdt.Rows[i]["EndDate"]).ToString("dd-MMM-yyyy") + "')";
                    cmd = new SqlCommand(sSql, conn,tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                tran.Commit();
            }
            catch (Exception ce)
            {
                tran.Rollback();
                System.Windows.Forms.MessageBox.Show(ce.Message, "CRM", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

    }
}
