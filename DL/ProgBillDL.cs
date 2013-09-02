using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using CRM.BusinessLayer;
using CRM.BusinessObjects;
using Qualifier;
using Microsoft.VisualBasic;

namespace CRM.DataLayer
{
    class ProgBillDL
    {
        public static DataTable GetOpCostCentre()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Select CostCentreId,CostCentreName,ProjectDB,FACostCentreId,0 CompanyId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                              " Where ProjectDB IN(Select ProjectName from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType in('B','L'))" +
                              " AND CostCentreId NOT IN(Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                              " Order by CostCentreName";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        public static DataTable GetCostCentre()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Select CostCentreId,CostCentreName from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                              " Where ProjectDB IN(Select ProjectName from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType in('B','L'))" +
                              " AND CostCentreId NOT IN(Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                              " Order by CostCentreName";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        public static DataTable GetBlock(int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "SELECT BlockId,BlockName FROM dbo.BlockMaster Where CostCentreId= " + argCCId + " ORDER BY SortOrder";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
                dt.Load(dreader);
                dreader.Close();
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

        public static DataTable GetLevel(int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT LevelId,LevelName FROM dbo.LevelMaster Where CostCentreId= " + argCCId + " ORDER BY SortOrder", BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
                dt.Load(dreader);
                dreader.Close();
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

        public static DataTable GetAcct(int argTypeId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "";
                if (BsfGlobal.g_bFADB == true)
                {
                    sSql = "SELECT AccountId, AccountName FROM [" + BsfGlobal.g_sFaDBName + "].dbo.AccountMaster " +
                           "WHERE LastLevel='Y' AND TypeId = " + argTypeId + " Order by AccountName";
                }
                else
                {
                    sSql = "SELECT 0 AccountId, '' AccountName";
                }
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        public static DataTable GetPBAccountSetup(string argBType)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "";

                if (argBType == "B")
                    sSql = "SELECT * from dbo.PBAccountSetup";
                else 
                    sSql = "SELECT * from dbo.PlotPBAccountSetup";

                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        public static DataTable GetSoldFlat()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "SELECT FlatId,BlockId,LevelId FROM dbo.FlatDetails Where Status='S'";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        public static DataTable GetPBMaster(int argCCId,string argBType)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "";
                if (argBType == "B")
                {
                    sSql = "Select A.ProgRegId,A.PDate,A.PNo,A.AsOnDate,B.CostCentreId,B.CostCentreName,A.NetAmount,A.Approve From dbo.ProgressBillMaster A" +
                            " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B " +
                            " On A.CostCentreId=B.CostCentreId Where " +
                            " A.CostCentreId= " + argCCId + " Order By A.PDate,A.PNo";
                }
                else
                {
                    sSql = "Select A.ProgRegId,A.PDate,A.PNo,A.AsOnDate,B.CostCentreId,B.CostCentreName,A.NetAmount,A.Approve From dbo.PlotProgressBillMaster A" +
                            " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B " +
                            " On A.CostCentreId=B.CostCentreId Where " +
                            " A.CostCentreId= " + argCCId + " Order By A.PDate,A.PNo";
                }
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        public static DataTable GetPBRegister(int argCCId,int argProgRegId, string argBType)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "";
                if (argBType == "B")
                {
                    sSql = " SELECT A.ProgRegId,A.PBillId,A.PBDate,A.PBNo,A.AsOnDate,B.CostCentreId,B.CostCentreName,ISNULL(PSF.Description, '') Description, " +
                          " A.FlatId,A.LeadId,E.FlatNo,D.LeadName BuyerName,A.BillAmount Amount,A.NetAmount,F.Approve FROM dbo.ProgressBillRegister A " +
                          " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B on A.CostCentreId=B.CostCentreId  " +
                          " Inner Join dbo.BuyerDetail C on A.FlatId=C.FlatId " +
                          " Inner Join dbo.LeadRegister D on C.LeadId=D.LeadId " +
                          " Inner Join dbo.FlatDetails E on A.FlatId=E.FlatId " +
                          " Inner Join dbo.ProgressBillMaster F on A.ProgRegId=F.ProgRegId " +
                          " Inner Join dbo.PaymentScheduleFlat PSF on A.PaySchId=PSF.PaymentSchId " +
                          " Where A.CostCentreId= " + argCCId + "" +
                          " And A.ProgRegId=" + argProgRegId + " And C.Status='S' " +
                          " Order by A.PBDate,A.PBNo";
                }
                else
                {
                    sSql = " SELECT A.ProgRegId,A.PBillId,A.PBDate,A.PBNo,A.AsOnDate,B.CostCentreId,B.CostCentreName, ISNULL(PSF.Description, '') Description, "+
                          " A.PlotDetailsId,A.BuyerId LeadId,E.PlotNo,D.LeadName BuyerName,A.BillAmount Amount,A.NetAmount,F.Approve FROM dbo.PlotProgressBillRegister A " +
                          " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B on A.CostCentreId=B.CostCentreId  " +
                          " Inner Join dbo.BuyerDetail C on A.PlotDetailsId=C.PlotId  " +
                          " Inner Join dbo.LeadRegister D on C.LeadId=D.LeadId " +
                          " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails E on A.PlotDetailsId=E.PlotDetailsId " +
                          " Inner Join dbo.PlotProgressBillMaster F on A.ProgRegId = F.ProgRegId " +
                          " Inner Join dbo.PaymentScheduleFlat PSF on A.PaySchId=PSF.PaymentSchId " +
                          " Where A.CostCentreId= " + argCCId + "" +
                          " And A.ProgRegId=" + argProgRegId + " And C.Status='S' " +
                          " Order by A.PBDate,A.PBNo";
                }
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        public static DataTable GetPaySchSoldFlat(int argCCId, int argSId, string arg_sStage)
        {
            DataTable dt = null; int iLevelId = 0;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                if (arg_sStage == "SchDescription")
                {
                    sSql = "Select Distinct LevelId From dbo.StageDetails Where CostCentreId=" + argCCId + " And SchType='D' And StageId=" + argSId + "";
                }
                else if (arg_sStage == "Stagewise")
                {
                    sSql = "Select Distinct LevelId From dbo.StageDetails Where CostCentreId=" + argCCId + " And SchType='S' And StageId=" + argSId + "";
                }
                else if (arg_sStage == "OtherCost")
                {
                    sSql = "Select Distinct LevelId From dbo.StageDetails Where CostCentreId=" + argCCId + " And SchType='O' And StageId=" + argSId + "";
                }
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    iLevelId = Convert.ToInt32(dt.Rows[0]["LevelId"]);
                }
                if (iLevelId > 0)
                {
                    if (arg_sStage == "SchDescription")
                    {
                        sSql = String.Format("SELECT CAST('' as Varchar(20))BillNo,P.PaymentSchId,F.FlatId,F.FlatNo,F.PayTypeId,B.LeadId,R.LeadName BuyerName,P.SchType,P.SchDescId StageId,P.Amount,P.NetAmount,P.BillAmount, 0 CurrentAmount,Convert(bit,0,0)Sel FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId=B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND S.LevelId =F.LevelId AND P.SchType=S.SchType AND S.StageId={0} WHERE P.CostCentreId={1} AND P.SchDescId={0} AND  P.Amount<>P.BillAmount ORDER BY PaymentSchId", argSId, argCCId);
                    }
                    else if (arg_sStage == "Stagewise")
                    {
                        sSql = String.Format("SELECT CAST('' as Varchar(20))BillNo,P.PaymentSchId,F.FlatId,F.FlatNo,F.PayTypeId,B.LeadId,R.LeadName BuyerName,P.SchType,P.StageId StageId,P.Amount,P.NetAmount,P.BillAmount, 0 CurrentAmount,Convert(bit,0,0)Sel FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId =B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND S.LevelId =F.LevelId AND P.SchType=S.SchType AND S.StageId={0} WHERE P.CostCentreId={1} AND P.StageId={0} AND  P.Amount<>P.BillAmount ORDER BY PaymentSchId", argSId, argCCId);
                    }
                    else if (arg_sStage == "OtherCost")
                    {
                        sSql = String.Format("SELECT CAST('' as Varchar(20))BillNo,P.PaymentSchId,F.FlatId,F.FlatNo,F.PayTypeId,B.LeadId,R.LeadName BuyerName,P.SchType,P.OtherCostId StageId,P.Amount,P.NetAmount,P.BillAmount, 0 CurrentAmount,Convert(bit,0,0)Sel FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId=B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND S.LevelId=F.LevelId AND P.SchType=S.SchType AND S.StageId={0} WHERE P.CostCentreId={1} AND P.OtherCostId={0} AND  P.Amount<>P.BillAmount ORDER BY PaymentSchId", argSId, argCCId);
                    }
                }
                else
                {
                    if (arg_sStage == "SchDescription")
                    {
                        sSql = String.Format("SELECT CAST('' as Varchar(20))BillNo,P.PaymentSchId,F.FlatId,F.FlatNo,F.PayTypeId,B.LeadId,R.LeadName BuyerName,P.SchType,P.SchDescId StageId,P.Amount,P.NetAmount,P.BillAmount, 0 CurrentAmount,Convert(bit,0,0)Sel FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId=B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND P.SchType=S.SchType AND S.StageId={0} WHERE P.CostCentreId={1} AND P.SchDescId={0} AND  P.Amount<>P.BillAmount ORDER BY PaymentSchId", argSId, argCCId);
                    }
                    else if (arg_sStage == "Stagewise")
                    {
                        sSql = String.Format("SELECT CAST('' as Varchar(20))BillNo,P.PaymentSchId,F.FlatId,F.FlatNo,F.PayTypeId,B.LeadId,R.LeadName BuyerName,P.SchType,P.StageId StageId,P.Amount,P.NetAmount,P.BillAmount, 0 CurrentAmount,Convert(bit,0,0)Sel FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId =B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND P.SchType=S.SchType AND S.StageId={0} WHERE P.CostCentreId={1} AND P.StageId={0} AND  P.Amount<>P.BillAmount ORDER BY PaymentSchId", argSId, argCCId);
                    }
                    else if (arg_sStage == "OtherCost")
                    {
                        sSql = String.Format("SELECT CAST('' as Varchar(20))BillNo,P.PaymentSchId,F.FlatId,F.FlatNo,F.PayTypeId,B.LeadId,R.LeadName BuyerName,P.SchType,P.OtherCostId StageId,P.Amount,P.NetAmount,P.BillAmount, 0 CurrentAmount,Convert(bit,0,0)Sel FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId=B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND P.SchType=S.SchType AND S.StageId={0} WHERE P.CostCentreId={1} AND P.OtherCostId={0} AND  P.Amount<>P.BillAmount ORDER BY PaymentSchId", argSId, argCCId);
                    }
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

        public static DataSet GetPBReceipt(int argCCId, int argBlockId, int argLevelId, int argSId, string arg_sStage, DateTime argDate)
        {
            cRateQualR RAQual;
            Collection QualVBC;
            DataTable dt; string sCond = "";
            DataSet ds=new DataSet(); int iLevelId = 0;
            SqlDataAdapter sda;
            string sSql = "";

            DataTable dtQualifier = new DataTable("FlatReceiptQualifier");
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
            dtQualifier.Columns.Add("SchType", typeof(string));
            dtQualifier.Columns.Add("ReceiptTypeId", typeof(int));
            dtQualifier.Columns.Add("OtherCostId", typeof(int));
            dtQualifier.Columns.Add("TaxablePer", typeof(decimal));
            dtQualifier.Columns.Add("TaxableValue", typeof(decimal));

            BsfGlobal.OpenCRMDB();
            try
            {
                if (arg_sStage == "SchDescription")
                {
                    sSql = "Select Distinct LevelId From dbo.StageDetails Where CostCentreId=" + argCCId + " And SchType='D' And StageId=" + argSId + "";
                }
                else if (arg_sStage == "Stagewise")
                {
                    sSql = "Select Distinct LevelId From dbo.StageDetails Where CostCentreId=" + argCCId + " And SchType='S' And StageId=" + argSId + "";
                }
                else if (arg_sStage == "OtherCost")
                {
                    sSql = "Select Distinct LevelId From dbo.StageDetails Where CostCentreId=" + argCCId + " And SchType='O' And StageId=" + argSId + "";
                }

                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    iLevelId = Convert.ToInt32(dt.Rows[0]["LevelId"]);
                }
                dt.Dispose();

                if (argBlockId == 0 && argLevelId != 0) { sCond = "And F.LevelId=" + argLevelId + ""; }
                if (argBlockId != 0 && argLevelId == 0) { sCond = "And F.BlockId=" + argBlockId + ""; }
                if (argBlockId != 0 && argLevelId != 0) { sCond = "And F.BlockId=" + argBlockId + " And F.LevelId=" + argLevelId + ""; }
                if (argBlockId == 0 && argLevelId == 0) { sCond = ""; }

                if (iLevelId > 0)
                {
                    if (arg_sStage == "SchDescription")
                    {
                        sSql = String.Format("SELECT CAST('' as Varchar(20))BillNo,P.PaymentSchId,F.PayTypeId,T.Typewise,F.FlatId,F.FlatNo,P.Description,B.LeadId,R.LeadName BuyerName,P.SchType,P.SchDescId StageId,P.Amount,Net=P.NetAmount,P.PaidAmount,NetAmount=P.NetAmount-P.PaidAmount,Convert(bit,0,0)Sel,P.SortOrder FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F ON P.FlatId=F.FlatId INNER JOIN PaySchType T ON F.PayTypeId=T.TypeId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId=B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND S.LevelId =F.LevelId AND P.SchType=S.SchType AND S.StageId={0} " + sCond + " And F.Status='S' WHERE P.CostCentreId={1} AND P.SchDescId={0} AND P.BillPassed=0 And S.DueDate<='" + string.Format(Convert.ToDateTime(argDate).ToString("yyyy-MM-dd")) + "' ORDER BY P.SortOrder", argSId, argCCId);
                    }
                    else if (arg_sStage == "Stagewise")
                    {
                        sSql = String.Format("SELECT CAST('' as Varchar(20))BillNo,P.PaymentSchId,F.PayTypeId,T.Typewise,F.FlatId,F.FlatNo,P.Description,B.LeadId,R.LeadName BuyerName,P.SchType,P.StageId StageId,P.Amount,Net=P.NetAmount,P.PaidAmount,NetAmount=P.NetAmount-P.PaidAmount,Convert(bit,0,0)Sel,P.SortOrder FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F ON P.FlatId=F.FlatId INNER JOIN PaySchType T ON F.PayTypeId=T.TypeId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId =B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND S.LevelId =F.LevelId AND P.SchType=S.SchType AND S.StageId={0} " + sCond + " And F.Status='S' WHERE P.CostCentreId={1} AND P.StageId={0} AND P.BillPassed=0 And S.DueDate<='" + string.Format(Convert.ToDateTime(argDate).ToString("yyyy-MM-dd")) + "' ORDER BY P.SortOrder", argSId, argCCId);
                    }
                    else if (arg_sStage == "OtherCost")
                    {
                        sSql = String.Format("SELECT CAST('' as Varchar(20))BillNo,P.PaymentSchId,F.PayTypeId,T.Typewise,F.FlatId,F.FlatNo,P.Description,B.LeadId,R.LeadName BuyerName,P.SchType,P.OtherCostId StageId,P.Amount,Net=P.NetAmount,P.PaidAmount,NetAmount=P.NetAmount-P.PaidAmount,Convert(bit,0,0)Sel,P.SortOrder FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F ON P.FlatId=F.FlatId INNER JOIN PaySchType T ON F.PayTypeId=T.TypeId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId=B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND S.LevelId=F.LevelId AND P.SchType=S.SchType AND S.StageId={0} " + sCond + " And F.Status='S' WHERE P.CostCentreId={1} AND P.OtherCostId={0} AND P.BillPassed=0 And S.DueDate<='" + string.Format(Convert.ToDateTime(argDate).ToString("yyyy-MM-dd")) + "' ORDER BY P.SortOrder", argSId, argCCId);
                    }
                }
                else
                {
                    if (arg_sStage == "SchDescription")
                    {
                        sSql = String.Format("SELECT Distinct CAST('' as Varchar(20))BillNo,T.Typewise,P.PaymentSchId,F.PayTypeId,F.FlatId,F.FlatNo,P.Description,B.LeadId,R.LeadName BuyerName,P.SchType,P.SchDescId StageId,P.Amount,Net=P.NetAmount,P.PaidAmount,NetAmount=P.NetAmount-P.PaidAmount,Convert(bit,0,0)Sel,P.SortOrder FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F ON P.FlatId=F.FlatId INNER JOIN PaySchType T ON F.PayTypeId=T.TypeId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId=B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND P.SchType=S.SchType AND S.StageId={0} " + sCond + " And F.Status='S' WHERE P.CostCentreId={1} AND P.SchDescId={0} AND P.BillPassed=0 And S.DueDate<='" + string.Format(Convert.ToDateTime(argDate).ToString("yyyy-MM-dd")) + "' ORDER BY P.SortOrder", argSId, argCCId);
                    }
                    else if (arg_sStage == "Stagewise")
                    {
                        sSql = String.Format("SELECT Distinct CAST('' as Varchar(20))BillNo,T.Typewise,P.PaymentSchId,F.PayTypeId,F.FlatId,F.FlatNo,P.Description,B.LeadId,R.LeadName BuyerName,P.SchType,P.StageId StageId,P.Amount,Net=P.NetAmount,P.PaidAmount,NetAmount=P.NetAmount-P.PaidAmount,Convert(bit,0,0)Sel,P.SortOrder FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F ON P.FlatId=F.FlatId INNER JOIN PaySchType T ON F.PayTypeId=T.TypeId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId =B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND P.SchType=S.SchType AND S.StageId={0} " + sCond + " And F.Status='S' WHERE P.CostCentreId={1} AND P.StageId={0} AND P.BillPassed=0 And S.DueDate<='" + string.Format(Convert.ToDateTime(argDate).ToString("yyyy-MM-dd")) + "' ORDER BY P.SortOrder", argSId, argCCId);
                    }
                    else if (arg_sStage == "OtherCost")
                    {
                        sSql = String.Format("SELECT Distinct CAST('' as Varchar(20))BillNo,T.Typewise,P.PaymentSchId,F.PayTypeId,F.FlatId,F.FlatNo,P.Description,B.LeadId,R.LeadName BuyerName,P.SchType,P.OtherCostId StageId,P.Amount,Net=P.NetAmount,P.PaidAmount,NetAmount=P.NetAmount-P.PaidAmount,Convert(bit,0,0)Sel,P.SortOrder FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F ON P.FlatId=F.FlatId INNER JOIN PaySchType T ON F.PayTypeId=T.TypeId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId=B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND P.SchType=S.SchType AND S.StageId={0} " + sCond + " And F.Status='S' WHERE P.CostCentreId={1} AND P.OtherCostId={0} AND P.BillPassed=0 And S.DueDate<='" + string.Format(Convert.ToDateTime(argDate).ToString("yyyy-MM-dd")) + "' ORDER BY P.SortOrder", argSId, argCCId);
                    }
                }
                //CAST(0 AS decimal(18,3)) CurrentAmount,
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

                DataRow dr;

                for (int i = 0; i < dtO.Rows.Count; i++)
                {
                    iFlatId = Convert.ToInt32(dtO.Rows[i]["FlatId"].ToString());
                    iPayId = Convert.ToInt32(dtO.Rows[i]["PaymentSchId"].ToString());
                    bPayTypewise = Convert.ToBoolean(dtO.Rows[i]["Typewise"].ToString());

                    dtN = new DataTable();

                    if (arg_sStage == "OtherCost")
                    {
                        sSql = "Select A.ReceiptTypeId,A.OtherCostId,A.SchType,Convert(bit,1,1) Sel, B.OtherCostName ReceiptType,A.Percentage,A.Amount,A.NetAmount from dbo.FlatReceiptType A " +
                               "Inner Join dbo.OtherCostMaster B on A.OtherCostId=B.OtherCostId " +
                               "Where PaymentSchId= " + iPayId + " And FlatId = " + iFlatId;
                    }
                    else
                    {
                        //sSql = "Select A.ReceiptTypeId,0 OtherCostId,'R' SchType,Case When B.ReceiptTypeId is null then Convert(bit,0,0) Else Convert(bit,1,1) End Sel,A.ReceiptTypeName ReceiptType,ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount,isnull(B.NetAmount,0) NetAmount From dbo.ReceiptType A " +
                        //       "Left Join dbo.FlatReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId And B.SchType<>'Q' and B.PaymentSchId=" + iPayId + " and FlatId  = " + iFlatId + " " +
                        //       "Where A.ReceiptTypeId <>1 " +
                        //       "Union All " +
                        //       "Select 0 ReceiptTypeId,A.OtherCostId,'O' SchType,Case When B.ReceiptTypeId is null then Convert(bit,0,0) Else Convert(bit,1,1) End Sel,A.OtherCostName ReceiptType,ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount,isnull(B.NetAmount,0) NetAmount from dbo.OtherCostMaster A " +
                        //       "Left Join dbo.FlatReceiptType B on A.OtherCostId=B.OtherCostId and B.PaymentSchId=" + iPayId + " and FlatId  = " + iFlatId + " " +
                        //       "Where A.OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans Where CostCentreId=" + argCCId + ")";
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
                        sSql = "Select B.Sel,A.QualifierId, A.QualifierName,B.Percentage,B.Amount,B.Amount QAmount,isnull(Q.AccountId,0) AccountId " +
                            " from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A  " +
                            " Inner Join dbo.PaySchTaxFlat B On A.QualifierId=B.QualifierId " +
                            " Left Join dbo.QualifierAccount Q on Q.QualifierId=A.QualifierId " +
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

                        DataRow dr1; decimal dAmt = 0;
                        for (int x = 0; x < dtQ.Rows.Count; x++)
                        {
                            dAmt = dAmt + Convert.ToDecimal(dtQ.Rows[x]["Amount"]);
                        }
                        if (dtTQ.Rows.Count > 0)
                        {
                            dr1 = dtTQ.NewRow();
                            dr1["FlatId"] = iFlatId;
                            dr1["PaymentSchId"] = iPayId;
                            dr1["QualifierId"] = Convert.ToInt32(dtQ.Rows[0]["QualifierId"]);
                            dr1["SchType"] = "Q";
                            dr1["ReceiptTypeId"] = Convert.ToInt32(dtQ.Rows[0]["QualifierId"]);
                            dr1["OtherCostId"] = 0;
                            dr1["AccountId"] = Convert.ToInt32(dtQ.Rows[0]["AccountId"]);
                            dr1["Add_Less_Flag"] = "+";
                            dr1["Amount"] = dAmt;

                            dtTQ.Rows.Add(dr1);
                        }
                    }

                }

                ds.Tables.Add(dtT);

                //if (bPayTypewise == false)
                //{
                //    DataTable dtQAbs = new DataTable(); decimal dAmt = 0;
                //    dtQAbs = dtT;
                //    DataView dv = new DataView(dtQAbs);
                //    dv.RowFilter = "SchType='Q'";
                //    dtQAbs = dv.ToTable();
                //    DataRow dr1;
                //    for (int i = 0; i < dtQAbs.Rows.Count; i++)
                //    {
                //        dtTQ.NewRow();
                //        dr1["FlatId"] = Convert.ToInt32(dtQAbs.Rows[i]["FlatId"]);
                //        dr1["PaymentSchId"] = Convert.ToInt32(dtQAbs.Rows[i]["PaymentSchId"]);
                //        dr1["QualifierId"] = Convert.ToInt32(dtQAbs.Rows[i]["QualifierId"]);
                //        dr1["SchType"] = dtQAbs.Rows[i]["SchType"].ToString();
                //        dr1["ReceiptTypeId"] = Convert.ToInt32(dtQAbs.Rows[i]["ReceiptTypeId"]);
                //        dr1["OtherCostId"] = Convert.ToInt32(dtQAbs.Rows[i]["OtherCostId"]);
                //        dr1["AccountId"] = Convert.ToInt32(dtQAbs.Rows[i]["AccountId"]);
                //        dr1["Add_Less_Flag"] = dtQAbs.Rows[i]["Add_Less_Flag"].ToString();
                //        dr1["Amount"] = Convert.ToInt32(dtQAbs.Rows[i]["Amount"]);

                //        dAmt = dAmt + Convert.ToDecimal(dtQAbs.Rows[i]["Amount"]);
                //    }

                //    //ds.Tables["QualifierAbs"].Rows[n]["Amount"] = dAmt;
                //}


                //sSql ="Select B.FlatId,B.PaymentSchId,A.QualifierId,A.Expression,A.ExpPer,A.NetPer,A.Add_Less_Flag,A.SurCharge, " +
                //      "A.EDCess,A.HEDPer,A.ExpValue,A.ExpPerValue,A.SurValue,A.EDValue,A.Amount,B.SchType,B.ReceiptTypeId,B.OtherCostId " +
                //      "From dbo.FlatReceiptQualifier A " +
                //      "Inner Join dbo.FlatReceiptType B on A.SchId=b.SchId Where B.PaymentSchId In (";
                //if (arg_sStage == "SchDescription")
                //    sSql = sSql + "SELECT P.PaymentSchId FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId=B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND P.SchType=S.SchType AND S.StageId=" + argSId + " WHERE P.CostCentreId=" + argCCId + " AND P.SchDescId=" + argSId + " AND P.Amount<>P.BillAmount)";
                //else if (arg_sStage == "Stagewise")
                //    sSql = sSql + "SELECT P.PaymentSchId FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId =B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND P.SchType=S.SchType AND S.StageId=" + argSId + " WHERE P.CostCentreId=" + argCCId + " AND P.StageId=" + argSId + " AND P.Amount<>P.BillAmount)";
                //else if (arg_sStage == "OtherCost")
                //    sSql = sSql + "SELECT P.PaymentSchId FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId=B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND P.SchType=S.SchType AND S.StageId=" + argSId + " WHERE P.CostCentreId=" + argCCId + " AND P.OtherCostId=" + argSId + " AND P.Amount<>P.BillAmount)";
                //sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                //sda.Fill(ds, "FlatReceiptQualifier");
                //sda.Dispose();

                if (bPayTypewise == true)
                {
                    sSql = "Select B.FlatId,B.PaymentSchId,A.QualifierId,B.SchType,B.ReceiptTypeId,B.OtherCostId,isnull(C.AccountId,0) AccountId, A.Add_Less_Flag,Sum(A.Amount) Amount " +
                          "From dbo.FlatReceiptQualifier A " +
                          "Inner Join dbo.FlatReceiptType B on A.SchId=b.SchId " +
                          "Left Join dbo.QualifierAccount C on A.QualifierId=C.QualifierId " +
                          "Where A.Amount <>0 and B.PaymentSchId In (";
                    if (arg_sStage == "SchDescription")
                    {
                        //sSql = sSql + "SELECT P.PaymentSchId FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId=B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND P.SchType=S.SchType AND S.StageId=" + argSId + " WHERE P.CostCentreId=" + argCCId + " AND P.SchDescId=" + argSId + " AND P.Amount<>P.BillAmount)";
                        sSql = sSql + "SELECT P.PaymentSchId FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN PaySchType T ON F.PayTypeId=T.TypeId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId =B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND P.SchType=S.SchType AND S.StageId=" + argSId + " And F.Status='S' WHERE P.CostCentreId=" + argCCId + " AND P.SchDescId=" + argSId + " AND NOT (P.NetAmount-P.PaidAmount)<=P.BillAmount And P.PaidAmount=0 And P.SchDate<='" + string.Format(Convert.ToDateTime(argDate).ToString("yyyy-MM-dd")) + "') ";
                    }
                    else if (arg_sStage == "Stagewise")
                    {
                        //sSql = sSql + "SELECT P.PaymentSchId FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId =B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND P.SchType=S.SchType AND S.StageId=" + argSId + " WHERE P.CostCentreId=" + argCCId + " AND P.StageId=" + argSId + " AND P.Amount<>P.BillAmount)";
                        sSql = sSql + "SELECT P.PaymentSchId FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN PaySchType T ON F.PayTypeId=T.TypeId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId =B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND P.SchType=S.SchType AND S.StageId=" + argSId + " And F.Status='S' WHERE P.CostCentreId=" + argCCId + " AND P.StageId=" + argSId + " AND NOT (P.NetAmount-P.PaidAmount)<=P.BillAmount And P.PaidAmount=0 And P.SchDate<='" + string.Format(Convert.ToDateTime(argDate).ToString("yyyy-MM-dd")) + "') ";

                    }
                    else if (arg_sStage == "OtherCost")
                    {
                        //sSql = sSql + "SELECT P.PaymentSchId FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId=B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND P.SchType=S.SchType AND S.StageId=" + argSId + " WHERE P.CostCentreId=" + argCCId + " AND P.OtherCostId=" + argSId + " AND P.Amount<>P.BillAmount)";
                        sSql = sSql + "SELECT P.PaymentSchId FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN PaySchType T ON F.PayTypeId=T.TypeId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId =B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND P.SchType=S.SchType AND S.StageId=" + argSId + " And F.Status='S' WHERE P.CostCentreId=" + argCCId + " AND P.OtherCostId=" + argSId + " AND NOT (P.NetAmount-P.PaidAmount)<=P.BillAmount And P.PaidAmount=0 And P.SchDate<='" + string.Format(Convert.ToDateTime(argDate).ToString("yyyy-MM-dd")) + "') ";

                    }

                    sSql = sSql + " Group by B.FlatId,B.PaymentSchId,A.QualifierId,B.SchType,B.ReceiptTypeId,B.OtherCostId,C.AccountId,A.Add_Less_Flag";

                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    sda.Fill(ds, "QualifierAbs");
                    sda.Dispose();
                }
                else
                {
                    ds.Tables.Add(dtTQ);
                }

                //if (bPayTypewise == true)
                //{
                //Qualifier
                DataRow dRowT; int iQualId = 0; decimal dQNetAmt = 0; decimal dAdvAmt = 0; bool bAns = false; bool bService = false;
                for (int n = 0; n < dtO.Rows.Count; n++)
                {
                    int iPaySchId = Convert.ToInt32(dtO.Rows[n]["PaymentSchId"]);
                    int i_FlatId = Convert.ToInt32(dtO.Rows[n]["FlatId"]);
                    bool b_Typewise = Convert.ToBoolean(dtO.Rows[n]["Typewise"]);

                    if (b_Typewise == true)
                    {
                        decimal dTNetAmt = 0; decimal dTotalNetAmt = 0;
                        decimal dTaxAmt = 0, dTTaxAmt = 0;

                        for (int p = 0; p < dtT.Rows.Count; p++)
                        {
                            if (Convert.ToInt32(dtT.Rows[p]["PaymentSchId"]) == iPaySchId && Convert.ToInt32(dtT.Rows[p]["FlatId"]) == i_FlatId)
                            {
                                int iReceiptId = Convert.ToInt32(dtT.Rows[p]["ReceiptTypeId"]);
                                int iOthId = Convert.ToInt32(dtT.Rows[p]["OtherCostId"]);
                                string sSchType = dtT.Rows[p]["SchType"].ToString();
                                dTTaxAmt = 0;
                                //DataTable dtTT = new DataTable();
                                //dtTT = ds.Tables["FlatReceiptQualifier"];
                                //sSql = "Select QualifierId from CCReceiptQualifier Where ReceiptTypeId= " + iReceiptId + " and OtherCostId= " + iOthId + " and CostCentreId = " + argCCId;
                                //sSql = "Select QualifierId,IsNull(B.Service,0)Service From dbo.CCReceiptQualifier A " +
                                //        " Left Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId " +
                                //        " Where ReceiptTypeId= " + iReceiptId + " And A.OtherCostId= " + iOthId + "  And CostCentreId = " + argCCId;
                                sSql = "Select C.QualTypeId,A.QualifierId,IsNull(B.Service,0)Service From dbo.CCReceiptQualifier A " +
                                        " Left Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId " +
                                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On C.QualifierId=A.QualifierId " +
                                        " Where ReceiptTypeId= " + iReceiptId + " And A.OtherCostId= " + iOthId + "  And CostCentreId = " + argCCId;
                                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                                SqlDataReader sdr = cmd.ExecuteReader();
                                DataTable dtTT = new DataTable();
                                dtTT.Load(sdr);
                                sdr.Close();
                                cmd.Dispose();

                                dQNetAmt = 0;
                                decimal dQBaseAmt = Convert.ToDecimal(dtT.Rows[p]["Amount"]);

                                if (dtTT.Rows.Count == 0) { dTNetAmt = dTNetAmt + dQBaseAmt; dTotalNetAmt = dTNetAmt; }

                                for (int k = 0; k < dtTT.Rows.Count; k++)
                                {
                                    iQualId = Convert.ToInt32(dtTT.Rows[k]["QualifierId"]);
                                    bService = Convert.ToBoolean(dtTT.Rows[k]["Service"]);

                                    RAQual = new cRateQualR();
                                    QualVBC = new Collection();

                                    DataTable dtTDS = new DataTable();
                                    DataTable dtQ1 = new DataTable();
                                    if (Convert.ToInt32(dtTT.Rows[k]["QualTypeId"]) == 2)
                                    {
                                        if (bService == true)
                                            dtTDS = GetSTSettings("G", argDate);
                                        else dtTDS = GetSTSettings("F", argDate);
                                    }
                                    else { dtTDS = GetQual(iQualId, argDate); }

                                    dtQ1 = QualifierSelect(iQualId, "B", argDate);

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
                                    if (dtQ1.Rows.Count > 0)
                                    {
                                        RAQual.Add_Less_Flag = dtQ1.Rows[0]["Add_Less_Flag"].ToString();
                                        RAQual.Amount = 0;
                                        RAQual.Expression = dtQ1.Rows[0]["Expression"].ToString();
                                    }
                                    else
                                    {
                                        dtQ1 = GetQual(iQualId, argDate);
                                        if (dtQ1.Rows.Count > 0)
                                        {
                                            RAQual.Add_Less_Flag = dtQ1.Rows[0]["Add_Less_Flag"].ToString();
                                            RAQual.Amount = 0;
                                            RAQual.Expression = dtQ1.Rows[0]["Expression"].ToString();
                                        }
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
                                        //dQNetAmt = dQBaseAmt + dTaxAmt;

                                        //if (dQBaseAmt != 0)
                                        //{
                                        //    //dRTAmt = dRTAmt + dTaxAmt;

                                        //    if (k != 0) { dTNetAmt = dTNetAmt + dTaxAmt; }
                                        //    else dTNetAmt = dTNetAmt + dQNetAmt;
                                        //    //dTNetAmt = dTNetAmt + dQNetAmt;
                                        //    dTTaxAmt = dTTaxAmt + dTaxAmt;
                                        //}
                                        if (dQBaseAmt != 0)
                                        {
                                            decimal dTax = dQNetAmt - dQBaseAmt;
                                            dTTaxAmt = dTTaxAmt + dTax;

                                            //if (k != 0) { dQBaseAmt = 0; }

                                            dQNetAmt = dQBaseAmt + dTTaxAmt;
                                            if (k != 0) { dQBaseAmt = dQNetAmt; dTNetAmt = dTotalNetAmt + dQNetAmt; }
                                            else { dTNetAmt = dTNetAmt + dQNetAmt; }
                                            //dTNetAmt = dTNetAmt + dQNetAmt;
                                        }

                                        foreach (Qualifier.cRateQualR d in QualVBC)
                                        {
                                            dr1 = dtQualifier.NewRow();
                                            dr1["FlatId"] = i_FlatId;
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
                                            dr1["SchType"] = sSchType;
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
                                    dtT.Rows[p]["NetAmount"] = dQNetAmt;
                                    //dTNetAmt = dTNetAmt + dQNetAmt;
                                }
                            }
                        }

                        //if(bAns = GetAdvFound(iPaySchId, argCCId)==true)
                        //{
                        dAdvAmt = GetAdvAmt(iPaySchId);
                        //}
                        dtO.Rows[n]["NetAmount"] = dTNetAmt - dAdvAmt;
                        //dtO.Rows[n]["CurrentAmount"] = dTNetAmt - dAdvAmt - dSurplusAmt;
                    }
                }

                ds.Tables.Add(dtQualifier);

                if (bPayTypewise == true)
                {
                    for (int n = 0; n < ds.Tables["QualifierAbs"].Rows.Count; n++)
                    {
                        decimal dAmt = 0;
                        DataTable dtAbs = new DataTable();
                        dtAbs = dtQualifier;
                        DataView dv = new DataView(dtAbs);
                        dv.RowFilter = "FlatId=" + ds.Tables["QualifierAbs"].Rows[n]["FlatId"] + " And PaymentSchId=" + ds.Tables["QualifierAbs"].Rows[n]["PaymentSchId"] + " And QualifierId=" + ds.Tables["QualifierAbs"].Rows[n]["QualifierId"] + " And SchType='" + ds.Tables["QualifierAbs"].Rows[n]["SchType"] + "' And ReceiptTypeId=" + ds.Tables["QualifierAbs"].Rows[n]["ReceiptTypeId"] + " And OtherCostId=" + ds.Tables["QualifierAbs"].Rows[n]["OtherCostId"] + "";
                        dtAbs = dv.ToTable();
                        for (int i = 0; i < dtAbs.Rows.Count; i++)
                        {
                            dAmt = dAmt + Convert.ToDecimal(dtAbs.Rows[i]["Amount"]);
                        }

                        ds.Tables["QualifierAbs"].Rows[n]["Amount"] = dAmt;
                    }
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

            return ds;
        }

        public static bool GetAdvFound(int argPayTypeId, int argCCId)
        {
            SqlDataAdapter da;
            bool bAdvance = false;
            string sSql = "";
            
            sSql = "Select TemplateId from dbo.PaymentSchedule Where SchType='A' and TypeId=" + argPayTypeId + " and CostCentreId= " + argCCId;

            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            DataTable dtT = new DataTable();
            da.Fill(dtT);
            if (dtT.Rows.Count > 0) { bAdvance = true; }
            da.Dispose();
            dtT.Dispose();
            return bAdvance;
        }

        public static decimal GetAdvAmt(int argPaySchId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            decimal dAdvAmt = 0;
            
            sSql = "Select Sum(NetAmount)Amount From dbo.FlatReceiptType Where SchType='A' And PaymentSchId=" + argPaySchId + "";
            sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                dAdvAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
            }
            sda.Dispose();
           
            return dAdvAmt;
        }

        public static DataSet GetPBReceiptPlot(int argLandId, int argSId, string arg_sStage,DateTime argDate)
        {
            DataSet ds = new DataSet(); 
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            try
            {
                string sSql = "";
                if (arg_sStage == "SchDescription")
                {
                    sSql = "SELECT Distinct CAST('' as Varchar(20))BillNo,P.PaymentSchId,F.PlotDetailsId FlatId,F.PlotNo,B.LeadId,R.LeadName BuyerName," +
                            " P.SchType,P.SchDescId StageId,P.Amount,P.NetAmount,Convert(bit,0,0)Sel FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot P " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails F ON P.PlotDetailsId=F.PlotDetailsId " +
                            " INNER JOIN dbo.BuyerDetail B ON F.BuyerId=B.LeadId AND F.PlotDetailsId=B.PlotId" +
                            " INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId And F.Status='S' " +
                            " WHERE P.LandRegId=" + argLandId + " AND P.SchDescId=" + argSId + " AND P.SchDate<='" + string.Format(Convert.ToDateTime(argDate).ToString("yyyy-MM-dd")) + "'" +
                            " AND NOT P.NetAmount<=P.BillAmount ORDER BY PaymentSchId";
                }
                else if (arg_sStage == "OtherCost")
                {
                    sSql = "SELECT Distinct CAST('' as Varchar(20))BillNo,P.PaymentSchId,F.PlotDetailsId FlatId,F.PlotNo,B.LeadId,R.LeadName BuyerName," +
                            " P.SchType,P.OtherCostId StageId,P.Amount,P.NetAmount,Convert(bit,0,0)Sel FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot P " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails F ON P.PlotDetailsId=F.PlotDetailsId " +
                            " INNER JOIN dbo.BuyerDetail B ON F.BuyerId=B.LeadId AND F.PlotDetailsId=B.PlotId" +
                            " INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId And F.Status='S' " +
                            " WHERE P.LandRegId=" + argLandId + " AND P.OtherCostId=" + argSId + " And P.SchDate<='" + string.Format(Convert.ToDateTime(argDate).ToString("yyyy-MM-dd")) + "'" +
                            " AND NOT P.NetAmount<=P.BillAmount ORDER BY PaymentSchId";
                }

                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "PaymentSchedulePlot");
                sda.Dispose();


                DataTable dtO = new DataTable();
                DataTable dtN = new DataTable();
                dtO = ds.Tables["PaymentSchedulePlot"];
                int iPlotId = 0;
                int iPayId = 0;

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
                    iPlotId = Convert.ToInt32(dtO.Rows[i]["FlatId"].ToString());
                    iPayId = Convert.ToInt32(dtO.Rows[i]["PaymentSchId"].ToString());

                    dtN = new DataTable();

                    if (arg_sStage == "OtherCost")
                    {
                        sSql = "Select A.ReceiptTypeId,A.OtherCostId,A.SchType,Convert(bit,1,1) Sel, B.OtherCostName ReceiptType,A.Percentage," +
                                " A.Amount,A.NetAmount from ["+ BsfGlobal.g_sRateAnalDBName +"].dbo.PlotReceiptType A " +
                                " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.OtherCostMaster B on A.OtherCostId=B.OtherCostId Where PaymentSchId= " + iPayId + " And PlotDetailsId = " + iPlotId + "";
                    }
                    else
                    {
                        sSql = "Select A.ReceiptTypeId,0 OtherCostId,'R' SchType,Case When B.ReceiptTypeId " +
                                " is null then Convert(bit,0,0) Else Convert(bit,1,1) End Sel,A.ReceiptTypeName ReceiptType," +
                                " ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount,isnull(B.NetAmount,0) NetAmount From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ReceiptType A " +
                                " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId and B.PaymentSchId=" + iPayId + " and PlotDetailsId  = " + iPlotId + " " +
                                " Where A.ReceiptTypeId <>1 " +
                                " Union All " +
                                " Select 0 ReceiptTypeId,A.OtherCostId,'O' SchType,Case When B.ReceiptTypeId is null then Convert(bit,0,0) " +
                                " Else Convert(bit,1,1) End Sel,A.OtherCostName ReceiptType,ISNULL(B.Percentage,0) Percentage," +
                                " isnull(B.Amount,0) Amount,isnull(B.NetAmount,0) NetAmount from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.OtherCostMaster A " +
                                " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType B on A.OtherCostId=B.OtherCostId and B.PaymentSchId=" + iPayId + " and PlotDetailsId  = " + iPlotId + " " +
                                " Where A.OtherCostId in (Select OtherCostId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.OtherCostSetupTrans Where LandRegId=" + argLandId + ")";
                    }
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    sda.Fill(dtN);
                    sda.Dispose();

                    foreach (DataRow drow in dtN.Rows)
                    {
                        dr = dtT.NewRow();

                        dr["FlatId"] = iPlotId;
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
                }

                ds.Tables.Add(dtT);


                sSql = "Select B.PlotDetailsId FlatId,B.PaymentSchId,A.QualifierId,A.Expression,A.ExpPer,A.Add_Less_Flag,A.SurCharge, " +
                        " A.EDCess,A.ExpValue,A.ExpPerValue,A.SurValue,A.EDValue,A.Amount,B.SchType,B.ReceiptTypeId,B.OtherCostId " +
                        " From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptQualifier A Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType B on A.SchId=b.SchId " +
                        " Where B.PaymentSchId In (";
                if (arg_sStage == "SchDescription")
                    sSql = sSql + "SELECT P.PaymentSchId FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot P " +
                                    " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails F ON P.PlotDetailsId=F.PlotDetailsId " +
                                    " INNER JOIN dbo.BuyerDetail B ON F.BuyerId=B.LeadId AND F.PlotDetailsId=B.PlotId " +
                                    " INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId " +
                                    " WHERE P.LandRegId=" + argLandId + " AND P.SchDescId=" + argSId + " AND P.Amount<>P.BillAmount)";
                else if (arg_sStage == "OtherCost")
                    sSql = sSql + "SELECT P.PaymentSchId FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot P " +
                                    " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails F ON P.PlotDetailsId=F.PlotDetailsId " +
                                    " INNER JOIN dbo.BuyerDetail B ON F.BuyerId=B.LeadId AND F.PlotDetailsId=B.PlotId " +
                                    " INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId " +
                                    " WHERE P.LandRegId=" + argLandId + " AND P.OtherCostId=" + argSId + " AND P.Amount<>P.BillAmount)";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "PlotReceiptQualifier");
                sda.Dispose();


                sSql = "Select B.PlotDetailsId FlatId,B.PaymentSchId,A.QualifierId,isnull(C.AccountId,0) AccountId, A.Add_Less_Flag,Sum(A.Amount) Amount " +
                        " From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptQualifier A Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType B on A.SchId=b.SchId " +
                        " Left Join dbo.PlotQualifierAccount C on A.QualifierId=C.QualifierId Where A.Amount <>0 and B.PaymentSchId In (";
                if (arg_sStage == "SchDescription")
                {
                    sSql = sSql + "SELECT P.PaymentSchId FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot P " +
                                  " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails F ON P.PlotDetailsId=F.PlotDetailsId " +
                                  " INNER JOIN dbo.BuyerDetail B ON F.BuyerId=B.LeadId AND F.PlotDetailsId=B.PlotId " +
                                  " INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId " +
                                  " WHERE P.LandRegId=" + argLandId + " AND P.SchDescId=" + argSId + " AND P.Amount<>P.BillAmount)";
                }
                else if (arg_sStage == "OtherCost")
                {
                    sSql = sSql + "SELECT P.PaymentSchId FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot P " +
                                " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails F ON P.PlotDetailsId=F.PlotDetailsId " +
                                " INNER JOIN dbo.BuyerDetail B ON F.BuyerId=B.LeadId AND F.PlotDetailsId=B.PlotId "+
                                " INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId "+
                                " WHERE P.LandRegId=" + argLandId + " AND P.OtherCostId=" + argSId + " AND P.Amount<>P.BillAmount)";
                }

                sSql = sSql + " Group by B.PlotDetailsId,B.PaymentSchId,A.QualifierId,C.AccountId,A.Add_Less_Flag";

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

        public static DataSet GetAllPBReceipt(int argCCId, int argBlockId, int argLevelId, DateTime argDate)
        {
            cRateQualR RAQual = new cRateQualR();
            Collection QualVBC = new Collection();
            DataSet ds = new DataSet();
            SqlDataAdapter sda;
            string sSql = ""; string sCond = "";

            DataTable dtQualifier = new DataTable("FlatReceiptQualifier");
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
            dtQualifier.Columns.Add("SchType", typeof(string));
            dtQualifier.Columns.Add("ReceiptTypeId", typeof(int));
            dtQualifier.Columns.Add("OtherCostId", typeof(int));
            dtQualifier.Columns.Add("TaxablePer", typeof(decimal));
            dtQualifier.Columns.Add("TaxableValue", typeof(decimal));

            if (argBlockId == 0 && argLevelId != 0) { sCond = "And F.LevelId=" + argLevelId + ""; }
            if (argBlockId != 0 && argLevelId == 0) { sCond = "And F.BlockId=" + argBlockId + ""; }
            if (argBlockId != 0 && argLevelId != 0) { sCond = "And F.BlockId=" + argBlockId + " And F.LevelId=" + argLevelId + ""; }
            if (argBlockId == 0 && argLevelId == 0) { sCond = ""; }

            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "SELECT Distinct CAST('' as Varchar(20))BillNo,T.Typewise,P.PaymentSchId,F.PayTypeId,F.FlatId,F.FlatNo,P.Description,B.LeadId,  " +
                        " R.LeadName BuyerName,P.SchType,P.OtherCostId StageId,P.Amount,Net=P.NetAmount,P.PaidAmount,NetAmount=P.NetAmount-P.PaidAmount,Convert(bit,0,0)Sel,P.SortOrder FROM dbo.PaymentScheduleFlat P " +
                        " INNER JOIN dbo.FlatDetails F ON P.FlatId=F.FlatId INNER JOIN PaySchType T ON F.PayTypeId=T.TypeId  " +
                        " INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId=B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId  " +
                        " INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND P.SchType=S.SchType " +
                        " AND P.StageId In(Select StageId From StageDetails Where SchType='S' And CostCentreId=" + argCCId + ") " +
                        " OR P.SchDescId In(Select StageId From StageDetails Where SchType='D' And CostCentreId=" + argCCId + ") " +
                        " OR P.OtherCostId In(Select StageId From StageDetails Where SchType='O' And CostCentreId=" + argCCId + ") " +
                        " WHERE P.CostCentreId=" + argCCId + " And P.BillPassed=0 " + sCond + " " + //AND P.NetAmount<>P.PaidAmount  " +
                        " And S.DueDate<='" + string.Format(Convert.ToDateTime(argDate).ToString("yyyy-MM-dd")) + "'  ORDER BY P.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "PaymentScheduleFlat");
                sda.Dispose();

                DataTable dtO = new DataTable();
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
                            //" Union All "+
                            //" Select A.ReceiptTypeId,A.OtherCostId,A.SchType,Convert(bit,1,1) Sel, B.OtherCostName ReceiptType,A.Percentage,A.Amount,A.NetAmount from dbo.FlatReceiptType A " +
                            //" Inner Join dbo.OtherCostMaster B on A.OtherCostId=B.OtherCostId " +
                            //" Where PaymentSchId= " + iPayId + " And FlatId = " + iFlatId;

                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    DataTable dtN = new DataTable();
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
                            " Then Convert(bit,0,0) Else Convert(bit,1,1) End Sel,A.ReceiptTypeName ReceiptType," +
                            " ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount,isnull(B.NetAmount,0)" +
                            " NetAmount From dbo.ReceiptType A Left join dbo.FlatReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId" +
                            " and B.PaymentSchId=" + iPayId + " Where A.ReceiptTypeId=1 ";
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
                        DataTable dtQ = new DataTable();
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

                //sSql = "Select B.FlatId,B.PaymentSchId,A.QualifierId,A.Expression,A.ExpPer,A.NetPer,A.Add_Less_Flag,A.SurCharge, " +
                //        " A.EDCess,A.HEDPer,A.ExpValue,A.ExpPerValue,A.SurValue,A.EDValue,A.Amount,B.SchType,B.ReceiptTypeId,B.OtherCostId " +
                //        " From dbo.FlatReceiptQualifier A Inner Join dbo.FlatReceiptType B on A.SchId=B.SchId Where B.PaymentSchId In " +
                //        " (SELECT P.PaymentSchId FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F ON P.FlatId=F.FlatId " +
                //        " INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId=B.FlatId INNER JOIN dbo.LeadRegister R " +
                //        " ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId " +
                //        " AND P.SchType=S.SchType WHERE P.CostCentreId=" + argCCId + " AND P.Amount<>P.BillAmount)";
               
                //sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                //sda.Fill(ds, "FlatReceiptQualifier");
                //sda.Dispose();

                sSql = "Select B.FlatId,B.PaymentSchId,A.QualifierId,B.SchType,B.ReceiptTypeId,B.OtherCostId,isnull(C.AccountId,0) AccountId, " +
                        " A.Add_Less_Flag,Sum(A.Amount) Amount From dbo.FlatReceiptQualifier A Inner Join dbo.FlatReceiptType B on A.SchId=b.SchId " +
                        " Left Join dbo.QualifierAccount C on A.QualifierId=C.QualifierId Where A.Amount <>0 and B.PaymentSchId In ( " +
                        " SELECT P.PaymentSchId FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F ON P.FlatId=F.FlatId " +
                        " INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId=B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId " +
                        " INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND P.SchType=S.SchType " +
                        " WHERE P.CostCentreId=" + argCCId + " AND P.Amount<>P.BillAmount) " +
                        " Group by B.FlatId,B.PaymentSchId,A.QualifierId,B.SchType,B.ReceiptTypeId,B.OtherCostId,C.AccountId,A.Add_Less_Flag";
                
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "QualifierAbs");
                sda.Dispose();


                //Qualifier
                DataRow dRowT; int iQualId = 0; decimal dQNetAmt = 0; decimal dAdvAmt = 0; bool bAns = false; bool bService = false;
                for (int n = 0; n < dtO.Rows.Count; n++)
                {
                    int iPaySchId = Convert.ToInt32(dtO.Rows[n]["PaymentSchId"]);
                    int i_FlatId = Convert.ToInt32(dtO.Rows[n]["FlatId"]);
                    bool b_Typewise = Convert.ToBoolean(dtO.Rows[n]["Typewise"]);

                    if (b_Typewise == true)
                    {
                        decimal dTNetAmt = 0; decimal dTotalNetAmt = 0;
                        decimal dTaxAmt = 0, dTTaxAmt = 0;

                        DataView dview = new DataView(dtT) { RowFilter = "PaymentSchId=" + iPaySchId + " AND FlatId=" + i_FlatId + "" };
                        DataTable dtFilter = new DataTable();
                        dtFilter = dview.ToTable();
                        if (dtFilter != null)
                        {
                            for (int pk = 0; pk <= dtFilter.Rows.Count - 1; pk++)
                            {
                                int iReceiptId = Convert.ToInt32(dtFilter.Rows[pk]["ReceiptTypeId"]);
                                int iOthId = Convert.ToInt32(dtFilter.Rows[pk]["OtherCostId"]);
                                string sSchType = dtFilter.Rows[pk]["SchType"].ToString();
                                dTTaxAmt = 0;
                                //DataTable dtTT = new DataTable();
                                //dtTT = ds.Tables["FlatReceiptQualifier"];
                                //sSql = "Select QualifierId from CCReceiptQualifier Where ReceiptTypeId= " + iReceiptId + " and OtherCostId= " + iOthId + " and CostCentreId = " + argCCId;
                                //sSql = "Select QualifierId,IsNull(B.Service,0)Service From CCReceiptQualifier A " +
                                //        " Left Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId " +
                                //        " Where ReceiptTypeId= " + iReceiptId + " And A.OtherCostId= " + iOthId + " And CostCentreId = " + argCCId;
                                sSql = "Select C.QualTypeId,A.QualifierId,IsNull(B.Service,0)Service From CCReceiptQualifier A " +
                                        " Left Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId " +
                                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On C.QualifierId=A.QualifierId " +
                                        " Where ReceiptTypeId= " + iReceiptId + " And A.OtherCostId= " + iOthId + " And CostCentreId = " + argCCId;
                                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                                SqlDataReader sdr = cmd.ExecuteReader();
                                DataTable dtTT = new DataTable();
                                dtTT.Load(sdr);
                                sdr.Close();
                                cmd.Dispose();

                                dQNetAmt = 0;
                                decimal dQBaseAmt = Convert.ToDecimal(dtFilter.Rows[pk]["Amount"]);

                                if (dtTT.Rows.Count == 0) { dTNetAmt = dTNetAmt + dQBaseAmt; dTotalNetAmt = dTNetAmt; }

                                for (int k = 0; k < dtTT.Rows.Count; k++)
                                {
                                    iQualId = Convert.ToInt32(dtTT.Rows[k]["QualifierId"]);
                                    bService = Convert.ToBoolean(dtTT.Rows[k]["Service"]);

                                    RAQual = new cRateQualR();
                                    QualVBC = new Collection();

                                    //DataTable dtQ1 = new DataTable();
                                    //dtQ1 = GetQual(iQualId, argDate);
                                    DataTable dtTDS = new DataTable();
                                    DataTable dtQ1 = new DataTable();
                                    if (Convert.ToInt32(dtTT.Rows[k]["QualTypeId"]) == 2)
                                    {
                                        if (bService == true)
                                            dtTDS = GetSTSettings("G", argDate);
                                        else dtTDS = GetSTSettings("F", argDate);
                                    }
                                    else { dtTDS = GetQual(iQualId, argDate); }

                                    dtQ1 = QualifierSelect(iQualId, "B", argDate);

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
                                    if (dtQ1.Rows.Count > 0)
                                    {
                                        RAQual.Add_Less_Flag = dtQ1.Rows[0]["Add_Less_Flag"].ToString();
                                        RAQual.Amount = 0;
                                        RAQual.Expression = dtQ1.Rows[0]["Expression"].ToString();
                                    }

                                    QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);

                                    Qualifier.frmQualifier qul = new Qualifier.frmQualifier();

                                    dQNetAmt = 0;
                                    dTaxAmt = 0;
                                    decimal dVATAmt = 0;

                                    DataRow dr1;

                                    if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, argDate, ref dVATAmt) == true)
                                    {
                                        //////dRTAmt = dRTAmt + dTaxAmt;

                                        ////dTNetAmt = dTNetAmt + dQNetAmt;
                                        //////dTTaxAmt = dTTaxAmt + dTaxAmt;
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
                                        if (dQBaseAmt != 0)
                                        {
                                            decimal dTax = dQNetAmt - dQBaseAmt;
                                            dTTaxAmt = dTTaxAmt + dTax;

                                            //if (k != 0) { dQBaseAmt = 0; }

                                            dQNetAmt = dQBaseAmt + dTTaxAmt;
                                            if (k != 0) { dQBaseAmt = dQNetAmt; dTNetAmt = dTotalNetAmt + dQNetAmt; }
                                            else { dTNetAmt = dTNetAmt + dQNetAmt; }
                                            //dTNetAmt = dTNetAmt + dQNetAmt;
                                        }

                                        foreach (Qualifier.cRateQualR d in QualVBC)
                                        {
                                            dr1 = dtQualifier.NewRow();
                                            dr1["FlatId"] = i_FlatId;
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
                                            dr1["SchType"] = sSchType;
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
                                    dtFilter.Rows[pk]["NetAmount"] = dQNetAmt;
                                    //dTNetAmt = dTNetAmt + dQNetAmt;
                                }
                            }
                        }
                        //if(bAns = GetAdvFound(iPaySchId, argCCId)==true)
                        //{
                        dAdvAmt = GetAdvAmt(iPaySchId);
                        //}
                        dtO.Rows[n]["NetAmount"] = dTNetAmt - dAdvAmt;
                    }
                }
                ds.Tables.Add(dtQualifier);
                ////Qualifier
                //DataRow dRowT; int iQualId = 0;
                //foreach (DataRow drow in dtO.Rows)
                //{
                //    int iPaySchId = Convert.ToInt32(drow["PaymentSchId"]);
                //    int i_FlatId = Convert.ToInt32(drow["FlatId"]);
                //    decimal dTNetAmt = 0; decimal dQNetAmt = 0;
                //    decimal dTaxAmt = 0;

                //    foreach (DataRow drowT in dtT.Rows)
                //    {
                //        int iReceiptId = Convert.ToInt32(drowT["ReceiptTypeId"]);
                //        int iOthId = Convert.ToInt32(drowT["OtherCostId"]);

                //        DataTable dtTT = new DataTable();
                //        dtTT = ds.Tables["FlatReceiptQualifier"];

                //        dQNetAmt = 0;
                //        decimal dQBaseAmt = Convert.ToDecimal(drowT["Amount"]);

                //        //if (dtTT.Rows.Count == 0) { dTNetAmt = dTNetAmt + dQBaseAmt; }

                //        foreach (DataRow drowTT in dtTT.Rows)
                //        {
                //            iQualId = Convert.ToInt32(drowTT["QualifierId"]);

                //            RAQual = new cRateQualR();
                //            QualVBC = new Collection();

                //            DataTable dtQ1 = new DataTable();
                //            dtQ1 = GetQual(iQualId, argDate);
                //            if (dtQ1.Rows.Count > 0)
                //            {

                //                RAQual.Add_Less_Flag = dtQ1.Rows[0]["Add_Less_Flag"].ToString();
                //                RAQual.Amount = 0;
                //                RAQual.Expression = dtQ1.Rows[0]["Expression"].ToString();
                //                RAQual.RateID = iQualId;
                //                RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ1.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                //                RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ1.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                //                RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ1.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                //                RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ1.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                //                RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ1.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                //                RAQual.HEDValue = 0;
                //            }

                //            QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);

                //            Qualifier.frmQualifier qul = new Qualifier.frmQualifier();

                //            dQNetAmt = 0;
                //            dTaxAmt = 0;

                //            DataRow dr1;

                //            if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, argDate) == true)
                //            {
                //                dTNetAmt = dTNetAmt + dQNetAmt;
                //                drowTT["Amount"] = dTNetAmt;
                //            }
                //        }
                //        drowT["Amount"] = dTNetAmt;
                //    }

                //    drow["NetAmount"] = dTNetAmt;

                //}
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
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            cmd.Dispose();

            return dt;
        }

        public static DataSet GetAllPBReceiptPlot(int argLandId,DateTime argDate, string argStageDesc)
        {
            BsfGlobal.OpenCRMDB();
            DataSet ds = new DataSet();    
            int argSId = 0;
            try
            {
                string sSql = "";
                if (argStageDesc == "SchDescription")
                {
                    sSql = "SELECT Distinct CAST('' as Varchar(20))BillNo,P.PaymentSchId,F.PlotDetailsId FlatId,F.PlotNo,B.LeadId,R.LeadName BuyerName," +
                            " P.SchType,P.SchDescId StageId,P.Amount,P.NetAmount,Convert(bit,0,0)Sel FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot P " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails F ON P.PlotDetailsId=F.PlotDetailsId " +
                            " INNER JOIN dbo.BuyerDetail B ON F.BuyerId=B.LeadId AND F.PlotDetailsId=B.PlotId" +
                            " INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId And F.Status='S' " +
                            " WHERE P.LandRegId=" + argLandId + " AND P.SchDescId=" + argSId + " AND P.SchDate<='" + string.Format(Convert.ToDateTime(argDate).ToString("yyyy-MM-dd")) + "'" +
                            " AND NOT P.NetAmount<=P.BillAmount ORDER BY PaymentSchId";
                }
                else if (argStageDesc == "OtherCost")
                {
                    sSql = "SELECT Distinct CAST('' as Varchar(20))BillNo,P.PaymentSchId,F.PlotDetailsId FlatId,F.PlotNo,B.LeadId,R.LeadName BuyerName," +
                            " P.SchType,P.OtherCostId StageId,P.Amount,P.NetAmount,Convert(bit,0,0)Sel FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot P " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails F ON P.PlotDetailsId=F.PlotDetailsId " +
                            " INNER JOIN dbo.BuyerDetail B ON F.BuyerId=B.LeadId AND F.PlotDetailsId=B.PlotId" +
                            " INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId And F.Status='S' " +
                            " WHERE P.LandRegId=" + argLandId + " AND P.OtherCostId=" + argSId + " And P.SchDate<='" + string.Format(Convert.ToDateTime(argDate).ToString("yyyy-MM-dd")) + "'" +
                            " AND NOT P.NetAmount<=P.BillAmount ORDER BY PaymentSchId";
                }

                if (sSql == "") { return null; }

                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "PaymentSchedulePlot");
                sda.Dispose();

                DataTable dtO = new DataTable();
                DataTable dtN = new DataTable();
                dtO = ds.Tables["PaymentSchedulePlot"];
                int iPlotId = 0;
                int iPayId = 0;

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
                    iPlotId = Convert.ToInt32(dtO.Rows[i]["FlatId"].ToString());
                    iPayId = Convert.ToInt32(dtO.Rows[i]["PaymentSchId"].ToString());

                    dtN = new DataTable();

                    if (argStageDesc == "OtherCost")
                    {
                        sSql = "Select A.ReceiptTypeId,A.OtherCostId,A.SchType,Convert(bit,1,1) Sel, B.OtherCostName ReceiptType,A.Percentage," +
                                " A.Amount,A.NetAmount from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType A " +
                                " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.OtherCostMaster B on A.OtherCostId=B.OtherCostId Where PaymentSchId= " + iPayId + " And PlotDetailsId = " + iPlotId + "";
                    }
                    else
                    {
                        sSql = "Select A.ReceiptTypeId,0 OtherCostId,'R' SchType,Case When B.ReceiptTypeId " +
                                " is null then Convert(bit,0,0) Else Convert(bit,1,1) End Sel,A.ReceiptTypeName ReceiptType," +
                                " ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount,isnull(B.NetAmount,0) NetAmount From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ReceiptType A " +
                                " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId and B.PaymentSchId=" + iPayId + " and PlotDetailsId  = " + iPlotId + " " +
                                " Where A.ReceiptTypeId <>1 " +
                                " Union All " +
                                " Select 0 ReceiptTypeId,A.OtherCostId,'O' SchType,Case When B.ReceiptTypeId is null then Convert(bit,0,0) " +
                                " Else Convert(bit,1,1) End Sel,A.OtherCostName ReceiptType,ISNULL(B.Percentage,0) Percentage," +
                                " isnull(B.Amount,0) Amount,isnull(B.NetAmount,0) NetAmount from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.OtherCostMaster A " +
                                " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType B on A.OtherCostId=B.OtherCostId and B.PaymentSchId=" + iPayId + " and PlotDetailsId  = " + iPlotId + " " +
                                " Where A.OtherCostId in (Select OtherCostId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.OtherCostSetupTrans Where LandRegId=" + argLandId + ")";
                    }
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    sda.Fill(dtN);
                    sda.Dispose();

                    foreach (DataRow drow in dtN.Rows)
                    {
                        dr = dtT.NewRow();

                        dr["FlatId"] = iPlotId;
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
                }

                ds.Tables.Add(dtT);


                sSql = "Select B.PlotDetailsId FlatId,B.PaymentSchId,A.QualifierId,A.Expression,A.ExpPer,A.Add_Less_Flag,A.SurCharge, " +
                        " A.EDCess,A.ExpValue,A.ExpPerValue,A.SurValue,A.EDValue,A.Amount,B.SchType,B.ReceiptTypeId,B.OtherCostId " +
                        " From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptQualifier A Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType B on A.SchId=b.SchId " +
                        " Where B.PaymentSchId In (";
                if (argStageDesc == "SchDescription")
                    sSql = sSql + "SELECT P.PaymentSchId FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot P " +
                                    " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails F ON P.PlotDetailsId=F.PlotDetailsId " +
                                    " INNER JOIN dbo.BuyerDetail B ON F.BuyerId=B.LeadId AND F.PlotDetailsId=B.PlotId " +
                                    " INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId " +
                                    " WHERE P.LandRegId=" + argLandId + " AND P.SchDescId=" + argSId + " AND P.Amount<>P.BillAmount)";
                else if (argStageDesc == "OtherCost")
                    sSql = sSql + "SELECT P.PaymentSchId FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot P " +
                                    " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails F ON P.PlotDetailsId=F.PlotDetailsId " +
                                    " INNER JOIN dbo.BuyerDetail B ON F.BuyerId=B.LeadId AND F.PlotDetailsId=B.PlotId " +
                                    " INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId " +
                                    " WHERE P.LandRegId=" + argLandId + " AND P.OtherCostId=" + argSId + " AND P.Amount<>P.BillAmount)";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "PlotReceiptQualifier");
                sda.Dispose();


                sSql = "Select B.PlotDetailsId FlatId,B.PaymentSchId,A.QualifierId,isnull(C.AccountId,0) AccountId, A.Add_Less_Flag,Sum(A.Amount) Amount " +
                        " From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptQualifier A Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType B on A.SchId=b.SchId " +
                        " Left Join dbo.PlotQualifierAccount C on A.QualifierId=C.QualifierId Where A.Amount <>0 and B.PaymentSchId In (";
                if (argStageDesc == "SchDescription")
                {
                    sSql = sSql + "SELECT P.PaymentSchId FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot P " +
                                  " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails F ON P.PlotDetailsId=F.PlotDetailsId " +
                                  " INNER JOIN dbo.BuyerDetail B ON F.BuyerId=B.LeadId AND F.PlotDetailsId=B.PlotId " +
                                  " INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId " +
                                  " WHERE P.LandRegId=" + argLandId + " AND P.SchDescId=" + argSId + " AND P.Amount<>P.BillAmount)";
                }
                else if (argStageDesc == "OtherCost")
                {
                    sSql = sSql + "SELECT P.PaymentSchId FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot P " +
                                " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails F ON P.PlotDetailsId=F.PlotDetailsId " +
                                " INNER JOIN dbo.BuyerDetail B ON F.BuyerId=B.LeadId AND F.PlotDetailsId=B.PlotId " +
                                " INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId " +
                                " WHERE P.LandRegId=" + argLandId + " AND P.OtherCostId=" + argSId + " AND P.Amount<>P.BillAmount)";
                }

                sSql = sSql + " Group by B.PlotDetailsId,B.PaymentSchId,A.QualifierId,C.AccountId,A.Add_Less_Flag";

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

        public static DataSet GetPBPlot(int argLandId)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter sda;
            string sSql = "";

            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select CAST('' as Varchar(20)) BillNo,A.PlotDetailsId,A.PlotTypeId,A.BuyerId,A.PlotNo,B.LeadName Buyer," +
                    " A.BaseAmount,A.OtherCost,A.QualifierAmount," +
                    " A.NetAmount,Convert(bit,0,0)Sel " +
                    " From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A " +
                    " Inner Join LeadRegister B On A.BuyerId=B.LeadId " +
                    " Where A.Status='S' And A.BillPassed=0 " +
                    " And A.LandRegisterId=" + argLandId + "";

                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Plot");
                sda.Dispose();


                DataTable dtO = new DataTable();
                DataTable dtN = new DataTable();
                dtO = ds.Tables["Plot"];
                int iPlotId = 0; decimal dBaseAmt = 0;

                DataTable dtT = new DataTable("OtherCost");
                dtT.Columns.Add("PlotDetailsId", typeof(int));
                dtT.Columns.Add("OtherCostId", typeof(int));
                dtT.Columns.Add("Sign", typeof(string));
                dtT.Columns.Add("Amount", typeof(decimal));

                DataRow dr;

                for (int i = 0; i < dtO.Rows.Count; i++)
                {
                    iPlotId = Convert.ToInt32(dtO.Rows[i]["PlotDetailsId"].ToString());
                    dBaseAmt = Convert.ToDecimal(dtO.Rows[i]["BaseAmount"].ToString());

                    dtN = new DataTable();

                    dr = dtT.NewRow();
                    dr["PlotDetailsId"] = iPlotId;
                    dr["OtherCostId"] = 0;
                    dr["Sign"] = "+";
                    dr["Amount"] = dBaseAmt;

                    dtT.Rows.Add(dr);
                    sSql = "Select * From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotDetailsOtherCost Where PlotDetailsId=" + iPlotId + "";
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    sda.Fill(dtN);
                    sda.Dispose();

                    foreach (DataRow drow in dtN.Rows)
                    {
                        dr = dtT.NewRow();

                        dr["PlotDetailsId"] = iPlotId;
                        dr["OtherCostId"] = Convert.ToInt32(drow["OtherCostId"]);
                        dr["Sign"] = drow["Sign"].ToString();
                        dr["Amount"] = Convert.ToDecimal(drow["Amount"]);

                        dtT.Rows.Add(dr);
                    }


                    sSql = "Select * From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotDetailsQualifier Where PlotDetailsId=" + iPlotId + "";

                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    sda.Fill(ds, "PlotQualifier");
                    sda.Dispose();


                    sSql = "Select B.PlotDetailsId,B.QualifierId,isnull(A.AccountId,0) AccountId, B.Add_Less_Flag,Sum(B.Amount)Amount " +
                            " From PlotQualifierAccount A Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotDetailsQualifier B On A.QualifierId=B.QualifierId" +
                            " Where B.Amount<>0 And B.PlotDetailsId=" + iPlotId + "" +
                            " Group by B.PlotDetailsId,B.QualifierId,A.AccountId,B.Add_Less_Flag";
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    sda.Fill(ds, "QualifierAbs");
                    sda.Dispose();
                }
                ds.Tables.Add(dtT);

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

        public static DataTable GetBillNo()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "SELECT MAX(PBNo) PBNo FROM dbo.ProgressBillRegister";
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
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable GetPaySchStage(int argCCId,int argBlockId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sCond = "";
                if (argBlockId != 0) { sCond = "And BlockId=" + argBlockId + ""; }
                if (argBlockId == 0) { sCond = ""; }

                string sSql = "Select StageName Description From dbo.Stages Where StageId In(Select StageId From dbo.StageDetails Where CostCentreId=" + argCCId + " " + sCond + " And SchType='S')" +
                        " Union All" +
                        " Select OtherCostName Description From dbo.OtherCostMaster Where OtherCostId In(Select StageId From dbo.StageDetails Where CostCentreId=" + argCCId + " " + sCond + " And SchType='O')" +
                        " Union All" +
                        " Select SchDescName Description From dbo.SchDescription Where SchDescId In(Select StageId From dbo.StageDetails Where CostCentreId=" + argCCId + " " + sCond + " And SchType='D')";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        public static DataTable GetStage(int argCCId,string argType,string argBusType)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "";

                if (argBusType == "B")
                {
                    if (argType == "SchDescription")
                    {
                        sSql = "Select SchDescId Id,SchDescName Name,'D' SchType from dbo.SchDescription " +
                               " Where SchDescId IN(Select SchDescId from dbo.PaymentSchedule Where CostCentreId=" + argCCId + ")";
                    }
                    else if (argType == "Stagewise")
                    {
                        sSql = "Select StageId Id,StageName Name,'S' SchType from dbo.Stages " +
                               "Where StageId in (Select StageId from dbo.PaymentSchedule Where CostCentreId=" + argCCId + ")";
                    }
                    else if (argType == "OtherCost")
                    {
                        sSql = "Select OtherCostId Id,OtherCostName Name,'O' SchType from dbo.OtherCostMaster " +
                               "Where OtherCostId in (Select OtherCostId from dbo.PaymentSchedule Where CostCentreId=" + argCCId + ")";
                    }
                }
                else
                {
                    if (argType == "SchDescription")
                    {
                        sSql = "Select SchDescId Id,SchDescName Name,'D' SchType from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.SchDescription " +
                               "Where SchDescId in (Select SchDescId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedule Where LandRegId=" + argCCId + ")";
                    }
                    else if (argType == "OtherCost")
                    {
                        sSql = "Select OtherCostId Id,OtherCostName Name,'O' SchType from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.OtherCostMaster " +
                               "Where OtherCostId in (Select OtherCostId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedule Where LandRegId=" + argCCId + ")";
                    }
                }
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        public static DataTable GetPlot(int argLandId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                //sSql = "Select PlotNo From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails Where Status='S' And BillPassed=0 And LandRegisterId=" + argLandId + "";
                string sSql = "Select OtherCostName Description From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.OtherCostMaster Where OtherCostId" +
                        " In(Select OthercostId From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot B On A.PlotDetailsId=B.PlotDetailsId" +
                        " Where Status='S' And OtherCostId<>0 And LandRegisterId=" + argLandId + ")" +
                        " Union All " +
                        " Select SchDescName Description From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.SchDescription Where SchDescId" +
                        " In(Select SchDescId From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot B On A.PlotDetailsId=B.PlotDetailsId" +
                        " Where Status='S' And SchDescId<>0 And LandRegisterId=" + argLandId + ")";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        public static DataTable GetTax()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "SELECT QualiAmt FROM dbo.ReceiptTypeQualifier";
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

        public static int StageComplete(int argBlockId, int argLevelId, int argStageId)
        {
            int rtnVal = 0;
            SqlCommand cmd;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = String.Format("SELECT StageDetId FROM dbo.StageDetails Where BlockId={0} And LevelId={1} And StageId={2}", argBlockId, argLevelId, argStageId);
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                rtnVal = Convert.ToInt32(cmd.ExecuteScalar());
                //if (rtnVal == 1)
                //    compStage = true;
                //if (rtnVal == 0)
                //    compStage = 0;
                //else
                //    compStage = true;
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return rtnVal;
        }

        public static bool InsertProgressBillRegister(ProgressBillRegister argPBReg, DataTable argM, DataTable argRec, DataTable dtQual, DataTable dtQualAbs)
        {
            bool bAns = false;
            DataTable dtP = new DataTable();
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            string sSql = "";

            int iProgRegId = 0;
            string sPVNo = "";
            decimal dNAmt = 0;

            try
            {

                dtP.Columns.Add("PBillId", typeof(int));
                string sVNo = "";
                string sCCVNo = ""; string sPBNo = ""; string scCPBNo = "";
                int iPBillRegId = 0;
                BsfGlobal.VoucherType oVType;
                BsfGlobal.VoucherType oVCType; BsfGlobal.VoucherType oVPBType; BsfGlobal.VoucherType oVCCPBType;
                DataView dv;
                DataTable dt;
                DataTable dtQ;

                int iFlatId = 0;
                int iPayId = 0;
                int iPRId = 0;
                string sType = "";
                int iOId = 0;
                int iRId = 0;
                decimal dAmt = 0;
                DataRow dr1;

                sVNo = "";
                oVType = new BsfGlobal.VoucherType();
                oVType = BsfGlobal.GetVoucherNo(34, Convert.ToDateTime(argPBReg.BillDate), 0, 0, conn, tran, "I");
                if (oVType.GenType == true)
                {
                    sVNo = oVType.VoucherNo;
                }

                sPVNo = sVNo;

                sCCVNo = "";
                oVCType = new BsfGlobal.VoucherType();
                oVCType = BsfGlobal.GetVoucherNo(34, Convert.ToDateTime(argPBReg.BillDate), 0, argPBReg.CCId, conn, tran, "I");
                if (oVCType.GenType == true)
                {
                    sCCVNo = oVCType.VoucherNo;
                }

                sSql = "Insert into dbo.ProgressBillMaster(PDate,PNo,PCCNo,AsOnDate,CostCentreId,SchType,StageId,Remarks) " +
                       "Values('" + argPBReg.BillDate.ToString("dd-MMM-yyyy") + "','" + sVNo + "', '" + sCCVNo + "'," +
                       "'" + argPBReg.AsOnDate.ToString("dd-MMM-yyyy") + "'," + argPBReg.CCId + "," +
                       "'" + argPBReg.SchType + "'," + argPBReg.StageId + "," +
                       "'" + argPBReg.Remarks + "') Select Scope_Identity();";
                cmd = new SqlCommand(sSql, conn, tran);
                iProgRegId = int.Parse(cmd.ExecuteScalar().ToString());
                cmd.Dispose();

                for (int i = 0; i < argM.Rows.Count; i++)
                {

                    if (Convert.ToBoolean(argM.Rows[i]["Sel"]) == true)
                    {
                        sPBNo = "";
                        oVPBType = new BsfGlobal.VoucherType();
                        oVPBType = BsfGlobal.GetVoucherNo(16, Convert.ToDateTime(argPBReg.BillDate), 0, 0, conn, tran, "I");
                        if (oVPBType.GenType == true)
                        {
                            sPBNo = oVPBType.VoucherNo;
                        }

                        scCPBNo = "";
                        oVCCPBType = new BsfGlobal.VoucherType();
                        oVCCPBType = BsfGlobal.GetVoucherNo(16, Convert.ToDateTime(argPBReg.BillDate), 0, argPBReg.CCId, conn, tran, "I");
                        if (oVCCPBType.GenType == true)
                        {
                            scCPBNo = oVCCPBType.VoucherNo;
                        }


                        iFlatId = Convert.ToInt32(argM.Rows[i]["FlatId"]);
                        iPayId = Convert.ToInt32(argM.Rows[i]["PaymentSchId"]);

                        sSql = "Insert Into dbo.ProgressBillRegister(ProgRegId,PBDate,PBNo,CostCentreId,CCPBNo,AsOnDate,FlatId,LeadId,PaySchId," +
                               "BillAmount,NetAmount,SchType,StageId,IncomeId,Remarks,AdvanceId,BuyerAccountId) " +
                               "Values(" + iProgRegId + ",'" + argPBReg.BillDate.ToString("dd-MMM-yyyy") + "','" + sPBNo + "'," + argPBReg.CCId + "," +
                               "'" + scCPBNo + "','" + argPBReg.AsOnDate.ToString("dd-MMM-yyyy") + "'," + iFlatId + "," +
                               "" + Convert.ToInt32(argM.Rows[i]["LeadId"]) + "," + iPayId + "," +
                               "" + Convert.ToDecimal(argM.Rows[i]["Amount"]) + "," + Convert.ToDecimal(argM.Rows[i]["NetAmount"]) + ", " +
                               "'" + argM.Rows[i]["SchType"].ToString() + "'," + Convert.ToInt32(argM.Rows[i]["StageId"]) + "," +
                               "" + argPBReg.IncomeId + ",'" + argPBReg.Remarks + "'," + argPBReg.AdvanceId + "," + argPBReg.BuyerAccountId + ") Select Scope_Identity();";
                        cmd = new SqlCommand(sSql, conn, tran);
                        iPBillRegId = int.Parse(cmd.ExecuteScalar().ToString());
                        cmd.Dispose();

                        dr1 = dtP.NewRow();
                        dr1["PBillId"] = iPBillRegId;
                        dtP.Rows.Add(dr1);

                        dAmt = Convert.ToDecimal(argM.Rows[i]["NetAmount"]);
                        sSql = "UPDATE dbo.PaymentScheduleFlat SET BillPassed=1, BillAmount=BillAmount + " + dAmt + " " +
                               "WHERE FlatId= " + iFlatId + " AND PaymentSchId= " + iPayId;
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();

                        //BsfGlobal.UpdateMaxNo(16, oVType, 0, 0, conn, tran);
                        //BsfGlobal.UpdateMaxNo(16, oVType, 0, argPBReg.CCId, conn, tran);

                        dv = new DataView(dtQualAbs);

                        if (dv.ToTable().Rows.Count > 0)
                        {
                            dv.RowFilter = "FlatId = " + iFlatId + " and PaymentSchId = " + iPayId;
                            dt = new DataTable();
                            if (dv.ToTable().Rows.Count > 0) { dt = dv.ToTable(); }
                            dv.Dispose();

                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                if (Convert.ToDecimal(dt.Rows[j]["Amount"]) > 0)
                                {
                                    sSql = "Insert into dbo.PBQualifierAbs(QualifierId,Add_Less_Flag,PBillId,AccountId,Amount) " +
                                           "Values(" + Convert.ToInt32(dt.Rows[j]["QualifierId"]) + ",'" + dt.Rows[j]["Add_Less_Flag"].ToString() + "'," +
                                           "" + iPBillRegId + "," + Convert.ToInt32(dt.Rows[j]["AccountId"]) + ", " + Convert.ToDecimal(dt.Rows[j]["Amount"]) + ")";
                                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();
                                }
                            }

                        }

                            dv = new DataView(argRec);
                            dv.RowFilter = "FlatId = " + iFlatId + " and PaymentSchId = " + iPayId;
                            dt = new DataTable();
                            if (dv.ToTable().Rows.Count > 0) { dt = dv.ToTable(); }
                            dv.Dispose();

                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                if (Convert.ToDecimal(dt.Rows[j]["Amount"]) > 0 || Convert.ToDecimal(dt.Rows[j]["NetAmount"]) > 0)
                                {
                                    sType = dt.Rows[j]["SchType"].ToString();
                                    iRId = Convert.ToInt32(dt.Rows[j]["ReceiptTypeId"]);
                                    iOId = Convert.ToInt32(dt.Rows[j]["OtherCostId"]);


                                    sSql = "Insert into dbo.PBReceiptType(PBillId,PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                           "Values(" + iPBillRegId + "," + iPayId + "," + iFlatId + "," + iRId + "," +
                                           "" + iOId + ",'" + sType + "'," +
                                           "" + Convert.ToDecimal(dt.Rows[j]["Percentage"]) + "," + Convert.ToDecimal(dt.Rows[j]["Amount"]) + "," +
                                           "" + Convert.ToDecimal(dt.Rows[j]["NetAmount"]) + ") Select Scope_Identity();";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    iPRId = int.Parse(cmd.ExecuteScalar().ToString());
                                    cmd.Dispose();


                                    dv = new DataView(dtQual);

                                    if (sType == "A")
                                    {
                                        dv.RowFilter = "FlatId = " + iFlatId + " and PaymentSchId = " + iPayId + " and SchType ='A'";
                                    }
                                    else if (sType == "O")
                                    {
                                        dv.RowFilter = "FlatId = " + iFlatId + " and PaymentSchId = " + iPayId + " and SchType ='O' and OtherCostId = " + iOId;
                                    }
                                    else
                                    {
                                        dv.RowFilter = "FlatId = " + iFlatId + " and PaymentSchId = " + iPayId + " and SchType ='R' and ReceiptTypeId = " + iRId;
                                    }

                                    dtQ = new DataTable();
                                    if (dv.ToTable().Rows.Count > 0) { dtQ = dv.ToTable(); }
                                    dv.Dispose();


                                    for (int k = 0; k < dtQ.Rows.Count; k++)
                                    {
                                        sSql = "Insert Into dbo.PBReceiptTypeQualifier(PBillId,PBId,QualifierId,Expression,ExpPer," +
                                           " Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount,NetPer,HEDPer,TaxablePer,TaxableValue) " +
                                           "Values (" + iPBillRegId + "," + iPRId + "," + Convert.ToInt32(dtQ.Rows[k]["QualifierId"]) + "," +
                                           " '" + dtQ.Rows[k]["Expression"].ToString() + "'," + Convert.ToDecimal(dtQ.Rows[k]["ExpPer"]) + ",'" + dtQ.Rows[k]["Add_Less_Flag"].ToString() + "'," +
                                           " " + Convert.ToDecimal(dtQ.Rows[k]["SurCharge"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["EDCess"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["ExpValue"]) + "," +
                                           " " + Convert.ToDecimal(dtQ.Rows[k]["ExpPerValue"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["SurValue"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["EDValue"]) + "," +
                                           " " + Convert.ToDecimal(dtQ.Rows[k]["Amount"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["NetPer"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["HEDPer"]) + ","+
                                           " " + Convert.ToDecimal(dtQ.Rows[k]["TaxablePer"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["TaxableValue"]) + ")";
                                        cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();
                                    }
                                }
                            }
                    }
                }


                sSql = "Select Sum(NetAmount) Amount from dbo.ProgressBillRegister Where ProgRegId = " + iProgRegId;
                cmd = new SqlCommand(sSql, conn, tran);
                SqlDataReader dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0) { dNAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }
                dt.Dispose();

                sSql = "Update dbo.ProgressBillMaster Set NetAmount = " + dNAmt + " Where ProgRegId = " + iProgRegId;
                cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();

                tran.Commit();
                tran.Dispose();

                bAns = true;
                if (bAns == true)
                {
                    BsfGlobal.InsertLog(DateTime.Now, "Progress Bill-Add", "N", argPBReg.Remarks, iProgRegId, argPBReg.CCId, 0, BsfGlobal.g_sCRMDBName,
                                        sPVNo, BsfGlobal.g_lUserId, dNAmt, 0);
                }
            }
            catch (SqlException ex)
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
            return bAns;
        }

        public static bool InsertPlotProgressBillRegister(ProgressBillRegister argPBReg, DataTable argM, DataTable argRec, DataTable dtQual, DataTable dtQualAbs)
        {
            bool bAns = false;
            DataTable dtP = new DataTable();
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            string sSql = "";


            int iProgRegId = 0;
            string sPVNo = "";
            decimal dNAmt = 0;

            try
            {

                dtP.Columns.Add("PBillId", typeof(int));
                string sVNo = "";
                string sCCVNo = ""; string sPBNo = ""; string scCPBNo = "";
                int iPBRegId = 0;
                BsfGlobal.VoucherType oVType;
                BsfGlobal.VoucherType oVCType; BsfGlobal.VoucherType oVPBType; BsfGlobal.VoucherType oVCCPBType;
                DataView dv;
                DataTable dt;
                DataTable dtQ;

                int iPlotId = 0;
                int iPayId = 0;
                int iPRId = 0;
                string sType = "";
                int iOId = 0;
                int iRId = 0;
                decimal dAmt = 0;
                DataRow dr1;



                sVNo = "";
                oVType = new BsfGlobal.VoucherType();
                oVType = BsfGlobal.GetVoucherNo(34, Convert.ToDateTime(argPBReg.BillDate), 0, 0, conn, tran, "I");
                if (oVType.GenType == true)
                {
                    sVNo = oVType.VoucherNo;
                }

                sPVNo = sVNo;

                sCCVNo = "";
                oVCType = new BsfGlobal.VoucherType();
                oVCType = BsfGlobal.GetVoucherNo(34, Convert.ToDateTime(argPBReg.BillDate), 0, argPBReg.CCId, conn, tran, "I");
                if (oVCType.GenType == true)
                {
                    sCCVNo = oVCType.VoucherNo;
                }

                sSql = "Insert into dbo.PlotProgressBillMaster(PDate,PNo,PCCNo,AsOnDate,CostCentreId,LandRegisterId,SchType,StageId,Remarks) " +
                       "Values('" + argPBReg.BillDate.ToString("dd-MMM-yyyy") + "','" + sVNo + "', '" + sCCVNo + "'," +
                       "'" + argPBReg.AsOnDate.ToString("dd-MMM-yyyy") + "'," + argPBReg.CCId + "," + argPBReg.LandId + "," +
                       "'" + argPBReg.SchType + "'," + argPBReg.StageId + "," +
                       "'" + argPBReg.Remarks + "') Select Scope_Identity();";
                cmd = new SqlCommand(sSql, conn, tran);
                iProgRegId = int.Parse(cmd.ExecuteScalar().ToString());
                cmd.Dispose();

                //BsfGlobal.UpdateMaxNo(34, oVType, 0, 0, conn, tran);
                //BsfGlobal.UpdateMaxNo(34, oVCType, 0, argPBReg.CCId, conn, tran);


                for (int i = 0; i < argM.Rows.Count; i++)
                {

                    if (Convert.ToBoolean(argM.Rows[i]["Sel"]) == true)
                    {
                        sPBNo = "";
                        oVPBType = new BsfGlobal.VoucherType();
                        oVPBType = BsfGlobal.GetVoucherNo(16, Convert.ToDateTime(argPBReg.BillDate), 0, 0, conn, tran, "I");
                        if (oVPBType.GenType == true)
                        {
                            sPBNo = oVPBType.VoucherNo;
                        }

                        scCPBNo = "";
                        oVCCPBType = new BsfGlobal.VoucherType();
                        oVCCPBType = BsfGlobal.GetVoucherNo(16, Convert.ToDateTime(argPBReg.BillDate), 0, argPBReg.CCId, conn, tran, "I");
                        if (oVCCPBType.GenType == true)
                        {
                            scCPBNo = oVCCPBType.VoucherNo;
                        }


                        iPlotId = Convert.ToInt32(argM.Rows[i]["FlatId"]);
                        iPayId = Convert.ToInt32(argM.Rows[i]["PaymentSchId"]);

                        sSql = "Insert Into dbo.PlotProgressBillRegister(ProgRegId,PBDate,PBNo,CostCentreId,LandRegisterId,CCPBNo,AsOnDate,PlotDetailsId,BuyerId,PaySchId," +
                               "BillAmount,NetAmount,SchType,StageId,IncomeId,Remarks,AdvanceId,BuyerAccountId) " +
                               "Values(" + iProgRegId + ",'" + argPBReg.BillDate.ToString("dd-MMM-yyyy") + "','" + sPBNo + "'," + argPBReg.CCId + "," + argPBReg.LandId + "," +
                               "'" + scCPBNo + "','" + argPBReg.AsOnDate.ToString("dd-MMM-yyyy") + "'," + iPlotId + "," +
                               "" + Convert.ToInt32(argM.Rows[i]["LeadId"]) + "," + iPayId + "," + Convert.ToDecimal(argM.Rows[i]["Amount"]) + "," +
                               "" + Convert.ToDecimal(argM.Rows[i]["NetAmount"]) + "," +
                               "'" + argM.Rows[i]["SchType"].ToString() + "'," + Convert.ToInt32(argM.Rows[i]["StageId"]) + "," +
                               "" + argPBReg.IncomeId + ",'" + argPBReg.Remarks + "'," + argPBReg.AdvanceId + "," + argPBReg.BuyerAccountId + ") Select Scope_Identity();";
                        cmd = new SqlCommand(sSql, conn, tran);
                        iPBRegId = int.Parse(cmd.ExecuteScalar().ToString());
                        cmd.Dispose();

                        dr1 = dtP.NewRow();
                        dr1["PBillId"] = iPBRegId;
                        dtP.Rows.Add(dr1);

                        dAmt = Convert.ToDecimal(argM.Rows[i]["NetAmount"]);
                        sSql = "UPDATE [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot SET BillPassed=1, BillAmount=BillAmount + " + dAmt + " " +
                               "WHERE PlotDetailsId= " + iPlotId + " AND PaymentSchId= " + iPayId;
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();

                        dv = new DataView(dtQualAbs);

                        if (dv.ToTable().Rows.Count > 0)
                        {
                            dv.RowFilter = "FlatId = " + iPlotId + " AND PaymentSchId= " + iPayId;
                            dt = new DataTable();
                            if (dv.ToTable().Rows.Count > 0) { dt = dv.ToTable(); }
                            dv.Dispose();

                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                if (Convert.ToDecimal(dt.Rows[j]["Amount"]) > 0)
                                {
                                    sSql = "Insert into dbo.PlotPBQualifierAbs(QualifierId,Add_Less_Flag,PBillId,AccountId,Amount) " +
                                           "Values(" + Convert.ToInt32(dt.Rows[j]["QualifierId"]) + ",'" + dt.Rows[j]["Add_Less_Flag"].ToString() + "'," +
                                           "" + iPBRegId + "," + Convert.ToInt32(dt.Rows[j]["AccountId"]) + ", " + Convert.ToDecimal(dt.Rows[j]["Amount"]) + ")";
                                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();
                                }
                            }

                            dv = new DataView(argRec);
                            dv.RowFilter = "FlatId = " + iPlotId + " AND PaymentSchId= " + iPayId;
                            dt = new DataTable();
                            if (dv.ToTable().Rows.Count > 0) { dt = dv.ToTable(); }
                            dv.Dispose();

                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                if (Convert.ToDecimal(dt.Rows[j]["Amount"]) > 0 || Convert.ToDecimal(dt.Rows[j]["NetAmount"]) > 0)
                                {
                                    sType = dt.Rows[j]["SchType"].ToString();
                                    iRId = Convert.ToInt32(dt.Rows[j]["ReceiptTypeId"]);
                                    iOId = Convert.ToInt32(dt.Rows[j]["OtherCostId"]);


                                    sSql = "Insert into dbo.PlotPBReceiptType(PBillId,PaymentSchId,PlotId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                           "Values(" + iPBRegId + "," + iPayId + "," + iPlotId + "," + iRId + "," +
                                           "" + iOId + ",'" + sType + "'," +
                                           "" + Convert.ToDecimal(dt.Rows[j]["Percentage"]) + "," + Convert.ToDecimal(dt.Rows[j]["Amount"]) + "," +
                                           "" + Convert.ToDecimal(dt.Rows[j]["NetAmount"]) + ") Select Scope_Identity();";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    iPRId = int.Parse(cmd.ExecuteScalar().ToString());
                                    cmd.Dispose();

                                    dv = new DataView(dtQual);

                                    if (sType == "A")
                                    {
                                        dv.RowFilter = "FlatId = " + iPlotId + " and PaymentSchId = " + iPayId + " and SchType ='A'";
                                    }
                                    else if (sType == "O")
                                    {
                                        dv.RowFilter = "FlatId = " + iPlotId + " and PaymentSchId = " + iPayId + " and SchType ='O' and OtherCostId = " + iOId;
                                    }
                                    else
                                    {
                                        dv.RowFilter = "FlatId = " + iPlotId + " and PaymentSchId = " + iPayId + " and SchType ='R' and ReceiptTypeId = " + iRId;
                                    }

                                    dtQ = new DataTable();
                                    if (dv.ToTable().Rows.Count > 0) { dtQ = dv.ToTable(); }
                                    dv.Dispose();


                                    for (int k = 0; k < dtQ.Rows.Count; k++)
                                    {
                                        sSql = "Insert Into dbo.PlotPBQualifier (PBId,QualifierId,Expression,ExpPer," +
                                           " Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount) " +
                                           "Values (" + iPRId + "," + Convert.ToInt32(dtQ.Rows[k]["QualifierId"]) + "," +
                                           " '" + dtQ.Rows[k]["Expression"].ToString() + "'," + Convert.ToDecimal(dtQ.Rows[k]["ExpPer"]) + ",'" + dtQ.Rows[k]["Add_Less_Flag"].ToString() + "'," +
                                           " " + Convert.ToDecimal(dtQ.Rows[k]["SurCharge"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["EDCess"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["ExpValue"]) + "," +
                                           " " + Convert.ToDecimal(dtQ.Rows[k]["ExpPerValue"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["SurValue"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["EDValue"]) + "," +
                                           " " + Convert.ToDecimal(dtQ.Rows[k]["Amount"]) + ")";
                                        cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();
                                    }
                                }
                            }
                        }
                    }
                }


                sSql = "Select Sum(NetAmount) Amount from dbo.PlotProgressBillRegister Where ProgRegId = " + iProgRegId;
                cmd = new SqlCommand(sSql, conn, tran);
                SqlDataReader dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0) { dNAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }
                dt.Dispose();

                sSql = "Update dbo.PlotProgressBillMaster Set NetAmount = " + dNAmt + " Where ProgRegId = " + iProgRegId;
                cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();

                tran.Commit();
                tran.Dispose();

                bAns = true;
                if (bAns == true)
                {
                    BsfGlobal.InsertLog(DateTime.Now, "Plot-Progress-Bill-Add", "N", argPBReg.Remarks, iProgRegId, argPBReg.CCId, 0, 
                                        BsfGlobal.g_sCRMDBName, sPVNo, BsfGlobal.g_lUserId, dNAmt, 0);
                }
            }
            catch (SqlException ex)
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

            return bAns;
        }

        public static void UpdateProgressBillRegister(ProgressBillRegister argPBReg, DataTable argdt, DataTable argdtQ, DataTable dtQualAbs, bool argHiddenUpdate)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            bool bLog = false;
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "Update dbo.PaymentScheduleFlat Set PaymentScheduleFlat.BillAmount = PaymentScheduleFlat.BillAmount - ProgressBillRegister.NetAmount from dbo.ProgressBillRegister " +
                           "Where PaymentScheduleFlat.PaymentSchId=ProgressBillRegister.PaySchId and PaymentScheduleFlat.FlatId = ProgressBillRegister.FlatId " +
                           "and ProgressBillRegister.PBillId = " + argPBReg.PBillId;
                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();

                    sSql = "Update dbo.ProgressBillRegister Set NetAmount=" + argPBReg.TotalAmount + ",Remarks = '" + argPBReg.Remarks + "', " +
                           "IncomeId = " + argPBReg.IncomeId + ",AdvanceId = " + argPBReg.AdvanceId + ",BuyerAccountId = " + argPBReg.BuyerAccountId + " " +
                           "Where PBillId= " + argPBReg.PBillId;
                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();

                    sSql = "Update dbo.PaymentScheduleFlat Set PaymentScheduleFlat.BillAmount = PaymentScheduleFlat.BillAmount + ProgressBillRegister.NetAmount from dbo.ProgressBillRegister " +
                           "Where PaymentScheduleFlat.PaymentSchId=ProgressBillRegister.PaySchId and PaymentScheduleFlat.FlatId = ProgressBillRegister.FlatId " +
                           "and ProgressBillRegister.PBillId = " + argPBReg.PBillId;
                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();

                    sSql = "Delete From dbo.PBReceiptTypeQualifier Where PBId in (Select PBId from PBReceiptType Where PBillId = " + argPBReg.PBillId + ")";
                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();

                    sSql = "Delete From dbo.PBReceiptType Where PBillId= " + argPBReg.PBillId;
                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();

                    sSql = "Delete From dbo.PBQualifierAbs Where PBillId= " + argPBReg.PBillId;
                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();


                    for (int j = 0; j < dtQualAbs.Rows.Count; j++)
                    {
                        if (Convert.ToDecimal(dtQualAbs.Rows[j]["Amount"]) > 0)
                        {
                            sSql = "Insert into dbo.PBQualifierAbs(QualifierId,Add_Less_Flag,PBillId,AccountId,Amount) " +
                                   "Values(" + Convert.ToInt32(dtQualAbs.Rows[j]["QualifierId"]) + ",'" + dtQualAbs.Rows[j]["Add_Less_Flag"].ToString() + "'," +
                                   "" + argPBReg.PBillId + "," + Convert.ToInt32(dtQualAbs.Rows[j]["AccountId"]) + ", " + Convert.ToDecimal(dtQualAbs.Rows[j]["Amount"]) + ")";
                            cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();
                        }
                    }

                    int iPRId = 0;
                    DataView dv;
                    string sType = "";
                    int iRId = 0;
                    int iOId = 0;
                    int iFlatId = 0;
                    int iPayId = 0;
                    DataTable dtQ;

                    for (int i = 0; i < argdt.Rows.Count; i++)
                    {
                        if (Convert.ToDecimal(argdt.Rows[i]["Amount"]) > 0 || Convert.ToDecimal(argdt.Rows[i]["NetAmount"]) > 0)
                        {
                            sType = argdt.Rows[i]["SchType"].ToString();
                            iOId = Convert.ToInt32(argdt.Rows[i]["OtherCostId"]);
                            iRId = Convert.ToInt32(argdt.Rows[i]["ReceiptTypeId"]);
                            iFlatId = Convert.ToInt32(argdt.Rows[i]["FlatId"]);
                            iPayId = Convert.ToInt32(argdt.Rows[i]["PaymentSchId"]);


                            sSql = "Insert into dbo.PBReceiptType(PBillId,PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                    "Values(" + argPBReg.PBillId + "," + iPayId + "," + iFlatId + "," + iRId + "," +
                                    "" + iOId + ",'" + sType + "'," +
                                    "" + Convert.ToDecimal(argdt.Rows[i]["Percentage"]) + "," + Convert.ToDecimal(argdt.Rows[i]["Amount"]) + "," +
                                    "" + Convert.ToDecimal(argdt.Rows[i]["NetAmount"]) + ") Select Scope_Identity();";
                            cmd = new SqlCommand(sSql, conn, tran);
                            iPRId = int.Parse(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();

                            dv = new DataView(argdtQ);
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                if (sType == "A")
                                {
                                    dv.RowFilter = "FlatId = " + iFlatId + " and PaymentSchId = " + iPayId + " and SchType ='A'";
                                }
                                else if (sType == "O")
                                {
                                    dv.RowFilter = "FlatId = " + iFlatId + " and PaymentSchId = " + iPayId + " and SchType ='O' and OtherCostId = " + iOId;
                                }
                                else
                                {
                                    dv.RowFilter = "FlatId = " + iFlatId + " and PaymentSchId = " + iPayId + " and SchType ='R' and ReceiptTypeId = " + iRId;
                                }

                                dtQ = new DataTable();
                                if (dv.ToTable().Rows.Count > 0) { dtQ = dv.ToTable(); }
                                dv.Dispose();

                                for (int k = 0; k < dtQ.Rows.Count; k++)
                                {
                                    sSql = "Insert Into dbo.PBReceiptTypeQualifier(PBillId,PBId,QualifierId,Expression,ExpPer," +
                                       " Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount,TaxablePer,TaxableValue) " +
                                       "Values (" + argPBReg.PBillId + "," + iPRId + "," + Convert.ToInt32(dtQ.Rows[k]["QualifierId"]) + "," +
                                       " '" + dtQ.Rows[k]["Expression"].ToString() + "'," + Convert.ToDecimal(dtQ.Rows[k]["ExpPer"]) + ",'" + dtQ.Rows[k]["Add_Less_Flag"].ToString() + "'," +
                                       " " + Convert.ToDecimal(dtQ.Rows[k]["SurCharge"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["EDCess"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["ExpValue"]) + "," +
                                       " " + Convert.ToDecimal(dtQ.Rows[k]["ExpPerValue"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["SurValue"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["EDValue"]) + "," +
                                       " " + Convert.ToDecimal(dtQ.Rows[k]["Amount"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["TaxablePer"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["TaxableValue"]) + ")";
                                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();
                                }
                            }
                        }
                    }

                    int iProgRegId = 0;

                    sSql = "Select ProgRegId from dbo.ProgressBillRegister Where PBillId = " + argPBReg.PBillId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    dr.Close();
                    cmd.Dispose();

                    if (dt.Rows.Count > 0)
                    {
                        iProgRegId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["ProgRegId"], CommFun.datatypes.vartypenumeric));
                    }
                    dt.Dispose();


                    sSql = "Select Sum(NetAmount) Amount from dbo.ProgressBillRegister Where ProgRegId = " + iProgRegId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    dr = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(dr);
                    dr.Close();
                    cmd.Dispose();

                    decimal dNAmt = 0;
                    if (dt.Rows.Count > 0) { dNAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }
                    dt.Dispose();

                    sSql = "Update dbo.ProgressBillMaster Set NetAmount = " + dNAmt + " Where ProgRegId = " + iProgRegId;
                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();

                    sSql = "Select PNo from dbo.ProgressBillMaster Where ProgRegId = " + iProgRegId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    dr = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(dr);
                    dr.Close();
                    cmd.Dispose();

                    string sVNo = "";
                    if (dt.Rows.Count > 0)
                    {
                        sVNo = CommFun.IsNullCheck(dt.Rows[0]["PNo"], CommFun.datatypes.vartypestring).ToString();
                    }
                    dt.Dispose();

                    tran.Commit();

                    bLog = true;
                    if (argHiddenUpdate == false && bLog == true)
                    {
                        BsfGlobal.InsertLog(DateTime.Now, "Progress Bill-Modify", "E", argPBReg.Remarks, argPBReg.PBillId, argPBReg.CCId, 0,
                                            BsfGlobal.g_sCRMDBName, sVNo, BsfGlobal.g_lUserId);
                    }

                    //UpdateAccount(argPBReg.PBillId);
                }
                catch (SqlException ex)
                {
                    tran.Rollback();
                    bLog = false;
                    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static void UpdatePlotProgressBillRegister(ProgressBillRegister argPBReg, DataTable argdt, DataTable argdtQ, DataTable dtQualAbs, bool argHiddenUpdate)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot Set [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot.BillAmount = [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot.BillAmount - PlotProgressBillRegister.NetAmount " +
                            " From dbo.PlotProgressBillRegister Where [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot.PaymentSchId=PlotProgressBillRegister.PaySchId " +
                            " And [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot.PlotDetailsId = PlotProgressBillRegister.PlotDetailsId And PlotProgressBillRegister.PBillId = " + argPBReg.PBillId;
                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();

                    sSql = "Update dbo.PlotProgressBillRegister Set NetAmount=" + argPBReg.TotalAmount + ",Remarks = '" + argPBReg.Remarks + "', " +
                           "IncomeId = " + argPBReg.IncomeId + ",AdvanceId = " + argPBReg.AdvanceId + ",BuyerAccountId = " + argPBReg.BuyerAccountId + " " +
                           "Where PBillId= " + argPBReg.PBillId;
                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();

                    sSql = "Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot Set [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot.BillAmount = [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot.BillAmount + PlotProgressBillRegister.NetAmount " +
                            " From dbo.PlotProgressBillRegister Where [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot.PaymentSchId=PlotProgressBillRegister.PaySchId " +
                            " And [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot.PlotDetailsId = PlotProgressBillRegister.PlotDetailsId and PlotProgressBillRegister.PBillId = " + argPBReg.PBillId;
                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();

                    sSql = "Delete From dbo.PlotPBQualifier Where PBId in (Select PBId From PlotPBReceiptType Where PBillId = " + argPBReg.PBillId + ")";
                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();

                    sSql = "Delete From dbo.PlotPBReceiptType Where PBillId= " + argPBReg.PBillId;
                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();

                    sSql = "Delete From dbo.PlotPBQualifierAbs Where PBillId= " + argPBReg.PBillId;
                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();


                    for (int j = 0; j < dtQualAbs.Rows.Count; j++)
                    {
                        if (Convert.ToDecimal(dtQualAbs.Rows[j]["Amount"]) > 0)
                        {
                            sSql = "Insert into dbo.PlotPBQualifierAbs(QualifierId,Add_Less_Flag,PBillId,AccountId,Amount) " +
                                   "Values(" + Convert.ToInt32(dtQualAbs.Rows[j]["QualifierId"]) + ",'" + dtQualAbs.Rows[j]["Add_Less_Flag"].ToString() + "'," +
                                   "" + argPBReg.PBillId + "," + Convert.ToInt32(dtQualAbs.Rows[j]["AccountId"]) + ", " + Convert.ToDecimal(dtQualAbs.Rows[j]["Amount"]) + ")";
                            cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();
                        }
                    }

                    int iPRId = 0;
                    DataView dv;
                    string sType = "";
                    int iRId = 0;
                    int iOId = 0;
                    int iPlotId = 0;
                    int iPayId = 0;
                    DataTable dtQ;

                    for (int i = 0; i < argdt.Rows.Count; i++)
                    {
                        if (Convert.ToDecimal(argdt.Rows[i]["Amount"]) > 0 || Convert.ToDecimal(argdt.Rows[i]["NetAmount"]) > 0)
                        {
                            sType = argdt.Rows[i]["SchType"].ToString();
                            iOId = Convert.ToInt32(argdt.Rows[i]["OtherCostId"]);
                            iRId = Convert.ToInt32(argdt.Rows[i]["ReceiptTypeId"]);
                            iPlotId = Convert.ToInt32(argdt.Rows[i]["FlatId"]);
                            iPayId = Convert.ToInt32(argdt.Rows[i]["PaymentSchId"]);

                            sSql = "Insert into dbo.PlotPBReceiptType(PBillId,PaymentSchId,PlotId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                   "Values(" + argPBReg.PBillId + "," + iPayId + "," + iPlotId + "," + iRId + "," +
                                   "" + iOId + ",'" + sType + "'," +
                                   "" + Convert.ToDecimal(argdt.Rows[i]["Percentage"]) + "," + Convert.ToDecimal(argdt.Rows[i]["Amount"]) + "," +
                                   "" + Convert.ToDecimal(argdt.Rows[i]["NetAmount"]) + ") Select Scope_Identity();";
                            cmd = new SqlCommand(sSql, conn, tran);
                            iPRId = int.Parse(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();


                            dv = new DataView(argdtQ);

                            if (dv.ToTable().Rows.Count > 0)
                            {
                                if (sType == "A")
                                {
                                    dv.RowFilter = "FlatId = " + iPlotId + " and PaymentSchId = " + iPayId + " and SchType ='A'";
                                }
                                else if (sType == "O")
                                {
                                    dv.RowFilter = "FlatId = " + iPlotId + " and PaymentSchId = " + iPayId + " and SchType ='O' and OtherCostId = " + iOId;
                                }
                                else
                                {
                                    dv.RowFilter = "FlatId = " + iPlotId + " and PaymentSchId = " + iPayId + " and SchType ='R' and ReceiptTypeId = " + iRId;
                                }

                                dtQ = new DataTable();
                                if (dv.ToTable().Rows.Count > 0) { dtQ = dv.ToTable(); }
                                dv.Dispose();

                                for (int k = 0; k < dtQ.Rows.Count; k++)
                                {
                                    sSql = "Insert Into dbo.PlotPBQualifier(PBId,QualifierId,Expression,ExpPer," +
                                        " Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount) " +
                                        "Values (" + iPRId + "," + Convert.ToInt32(dtQ.Rows[k]["QualifierId"]) + "," +
                                        " '" + dtQ.Rows[k]["Expression"].ToString() + "'," + Convert.ToDecimal(dtQ.Rows[k]["ExpPer"]) + ",'" + dtQ.Rows[k]["Add_Less_Flag"].ToString() + "'," +
                                        " " + Convert.ToDecimal(dtQ.Rows[k]["SurCharge"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["EDCess"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["ExpValue"]) + "," +
                                        " " + Convert.ToDecimal(dtQ.Rows[k]["ExpPerValue"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["SurValue"]) + "," + Convert.ToDecimal(dtQ.Rows[k]["EDValue"]) + "," +
                                        " " + Convert.ToDecimal(dtQ.Rows[k]["Amount"]) + ")";
                                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();
                                }
                            }
                        }
                    }

                    int iProgRegId = 0;

                    sSql = "Select ProgRegId from dbo.PlotProgressBillRegister Where PBillId = " + argPBReg.PBillId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    dr.Close();
                    cmd.Dispose();
                    if (dt.Rows.Count > 0) { iProgRegId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["ProgRegId"], CommFun.datatypes.vartypenumeric)); }
                    dt.Dispose();

                    sSql = "Select Sum(NetAmount) Amount from dbo.PlotProgressBillRegister Where ProgRegId = " + iProgRegId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    dr = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(dr);
                    dr.Close();
                    cmd.Dispose();
                    decimal dNAmt = 0;
                    if (dt.Rows.Count > 0) { dNAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }
                    dt.Dispose();

                    sSql = "Update dbo.PlotProgressBillMaster Set NetAmount = " + dNAmt + " Where ProgRegId = " + iProgRegId;
                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();

                    tran.Commit();
                    if (argHiddenUpdate == false)
                    {
                        BsfGlobal.InsertLog(DateTime.Now, "Plot-Progress-Bill-Modify", "E", argPBReg.Remarks, argPBReg.PBillId, argPBReg.CCId, 0,
                                            BsfGlobal.g_sCRMDBName, argPBReg.BillNo, BsfGlobal.g_lUserId, dNAmt, 0);
                    }
                    //UpdateAccount(argPBReg.PBillId);
                }
                catch (SqlException ex)
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

        public static void UpdateAccount(int argPBillId)
        {

            SqlDataReader dr;
            SqlCommand cmd;
            DataTable dt;

            DateTime dDate = DateTime.Now;
            decimal dAmt = 0;
            decimal dGAmt = 0;
            int iIncomeId = 0;
            int iAdvanceId = 0;
            int iBuyerAccountId = 0;

            int iCCid = 0;
            int iRefId = 0;
            string sDBName = "";
            int iFACCId = 0;
            int iCompId = 0;
            int iLeadId = 0;
            int iFYearId=0;
            int iQualId = 0;

            BsfGlobal.OpenCRMDB();

            string sSql = "Select PBDate,NetAmount,IncomeId,AdvanceId,BuyerAccountId,CostCentreId,RefId,DBName,LeadId,PBNO,BillAmount from dbo.ProgressBillRegister " +
                          "Where PBillId= " + argPBillId;
            cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            dr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                dDate = Convert.ToDateTime(dt.Rows[0]["PBDate"]);
                dAmt = Convert.ToDecimal(dt.Rows[0]["NetAmount"]);
                dGAmt = Convert.ToDecimal(dt.Rows[0]["BillAmount"]);
                iIncomeId = Convert.ToInt32(dt.Rows[0]["IncomeId"]);
                iAdvanceId = Convert.ToInt32(dt.Rows[0]["AdvanceId"]);
                iBuyerAccountId = Convert.ToInt32(dt.Rows[0]["BuyerAccountId"]);
                iCCid = Convert.ToInt32(dt.Rows[0]["CostCentreId"]);
                iRefId = Convert.ToInt32(dt.Rows[0]["RefId"]);
                sDBName = CommFun.IsNullCheck(dt.Rows[0]["DBName"], CommFun.datatypes.vartypestring).ToString();
                iLeadId = Convert.ToInt32(dt.Rows[0]["LeadId"]);
                ProgBillBL.BillNo = CommFun.IsNullCheck(dt.Rows[0]["PBNO"], CommFun.datatypes.vartypestring).ToString();
            }
            dt.Dispose();


            sSql = "Select A.FACostCentreId,B.CompanyId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre A " +
                   "Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CostCentre B on A.FACostCentreId=B.CostCentreId " +
                   "Where A.CostCentreId = " + iCCid;
            cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            dr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                iFACCId = Convert.ToInt32(dt.Rows[0]["FACostCentreId"]);
                iCompId = Convert.ToInt32(dt.Rows[0]["CompanyId"]);
            }
            dt.Dispose();

            //ProgBillBL.BuyerTypeId = CommonBL.Get_BuyerType();
            //ProgBillBL.BuyerSLTypeId = CommonBL.Get_SubLedgerType("Buyer");
            //ProgBillBL.IncomeSLTypeId = CommonBL.Get_SubLedgerType("Income");

            sSql = "SELECT FYearId FROM [" + BsfGlobal.g_sFaDBName + "].dbo.FiscalYearTrans WHERE CompanyId= " + iCompId + " and " +
                   "'" + dDate.ToString("dd-MMM-yyyy") + "' BETWEEN FromDate AND ToDate";
             cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            dr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                iFYearId = Convert.ToInt32(dt.Rows[0]["FYearId"]);
            }
            dt.Dispose();
            BsfGlobal.g_CRMDB.Close();

            ProgBillBL.CompanyDBName = CommonBL.Get_CompanyDB(iCompId, dDate);
            
            ProgBillBL.BuyerSLId = CommonBL.Get_Buyer_SL(iLeadId);

            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenFaDB();
            SqlTransaction tran;

            tran = conn.BeginTransaction();

            int iBillRefId = 0;
            int iSubLedgerId = 0;
            int iQualSubLedgerId = 0;
            decimal dAdvance = 0;
            int iMQualId = 0;

            try
            {

                iSubLedgerId = GetSubLedgerId(iLeadId, 3, conn, tran);

                if (iRefId != 0)
                {
                    iBillRefId = iRefId;

                    sSql = "Update dbo.BillRegister Set BillDate ='" + dDate.ToString("dd-MMM-yyyy") + "'," +
                           "BillNo ='" + ProgBillBL.BillNo + "',AccountId = " + iIncomeId + ",SubLedgerId = " + iSubLedgerId + ", " +
                           "BillAmount = " + dAmt + ",CostCentreId = " + iFACCId + ",CompanyId = " + iCompId + "," +
                           "TransType = 'R',FYearId = " + iFYearId + " Where BillRegisterId = " + iBillRefId;
                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();


                    sSql = "Delete from [" + sDBName + "].dbo.EntryTrans Where RefId = " + iBillRefId + " and RefType = 'PB'";
                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();

                }

                else
                {
                    sSql = "INSERT INTO dbo.BillRegister(BillDate,BillNo,RefTypeId, RefType,ReferenceId," +
                           "AccountId, SubLedgerId, BillAmount,CostCentreId,CompanyId,TransType,FYearId) VALUES(" +
                           "'" + dDate.ToString("dd-MMM-yyyy") + "','" + ProgBillBL.BillNo + "',6,'PB'," + argPBillId + "," + iIncomeId + "," +
                           "" + iSubLedgerId + ", " + dAmt + ", " + iFACCId + ", " + iCompId + ",'R'," + iFYearId + ") Select Scope_Identity();";
                    cmd = new SqlCommand(sSql, conn, tran);
                    iBillRefId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();
                }

                sSql = "Update [" + BsfGlobal.g_sCRMDBName + "].dbo.ProgressBillRegister Set RefId= " + iBillRefId + ",DBName = '" + ProgBillBL.CompanyDBName + "' Where PBillId= " + argPBillId;
                cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();


                sSql = "INSERT INTO [" + ProgBillBL.CompanyDBName + "].dbo.EntryTrans(RefId,VoucherDate,TransType,RefType,AccountId,RelatedAccountId,SubLedgerTypeId,SubLedgerId,CostCentreId,Amount,CompanyId)  " +
                        "VALUES (" + iBillRefId + ",'" + dDate.ToString("dd-MMM-yyyy") + "','C','PB'," + iIncomeId + "," + iBuyerAccountId + "," + 3 + "," + iSubLedgerId + "," +
                        "" + iFACCId + "," + dGAmt + "," + iCompId + ")";
                cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();


                sSql = "INSERT INTO [" + ProgBillBL.CompanyDBName + "].dbo.EntryTrans(RefId,VoucherDate,TransType,RefType,AccountId,RelatedAccountId,SubLedgerTypeId,SubLedgerId,CostCentreId,Amount,CompanyId)  " +
                       "VALUES (" + iBillRefId + ",'" + dDate.ToString("dd-MMM-yyyy") + "','D','PB'," + iBuyerAccountId + "," + iIncomeId + "," +  3 + "," + iSubLedgerId + "," +
                       "" + iFACCId + "," + dGAmt + "," + iCompId + ")";
                cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();

                dAdvance = 0;

                sSql = "Select Amount from [" + BsfGlobal.g_sCRMDBName + "].dbo.PBReceiptType Where PBillId=" + argPBillId + " and SchType='A'";
                cmd = new SqlCommand(sSql, conn, tran);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    dAdvance = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
                }
                dt.Dispose();

                if (dAdvance > 0)
                {
                    sSql = "INSERT INTO [" + ProgBillBL.CompanyDBName + "].dbo.EntryTrans(RefId,VoucherDate,TransType,RefType,AccountId,RelatedAccountId,SubLedgerTypeId,SubLedgerId,CostCentreId,Amount,CompanyId)  " +
                        "VALUES (" + iBillRefId + ",'" + dDate.ToString("dd-MMM-yyyy") + "','C','PB'," + iBuyerAccountId + "," + iAdvanceId + "," + 3 + "," + iSubLedgerId + "," +
                        "" + iFACCId + "," + dGAmt + "," + iCompId + ")";
                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();


                    sSql = "INSERT INTO [" + ProgBillBL.CompanyDBName + "].dbo.EntryTrans(RefId,VoucherDate,TransType,RefType,AccountId,RelatedAccountId,SubLedgerTypeId,SubLedgerId,CostCentreId,Amount,CompanyId)  " +
                           "VALUES (" + iBillRefId + ",'" + dDate.ToString("dd-MMM-yyyy") + "','D','PB'," + iAdvanceId + "," + iBuyerAccountId + "," + 3 + "," + iSubLedgerId + "," +
                           "" + iFACCId + "," + dGAmt + "," + iCompId + ")";
                    cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();
                }


                sSql = "Select QualifierId,Add_Less_Flag,AccountId,Amount from [" + BsfGlobal.g_sCRMDBName + "].dbo.PBQualifierAbs " +
                       "Where PBillId= " + argPBillId;
                cmd = new SqlCommand(sSql, conn, tran);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    iQualId = Convert.ToInt32(dt.Rows[i]["QualifierId"]);
                    iMQualId = GetQualId(iQualId,conn,tran);
                    iQualSubLedgerId = GetSubLedgerId(iMQualId, 8, conn, tran);

                    if (dt.Rows[0]["Add_Less_Flag"].ToString() == "-")
                    {

                        sSql = "INSERT INTO [" + ProgBillBL.CompanyDBName + "].dbo.EntryTrans(RefId,VoucherDate,TransType,RefType,AccountId,RelatedAccountId,SubLedgerTypeId,SubLedgerId,CostCentreId,Amount,CompanyId)  " +
                                "VALUES (" + iBillRefId + ",'" + dDate.ToString("dd-MMM-yyyy") + "','C','PB'," + iBuyerAccountId + "," + Convert.ToInt32(dt.Rows[i]["AccountId"]) + "," + 3 + "," + iSubLedgerId + "," +
                                 "" + iFACCId + "," + Convert.ToDecimal(dt.Rows[i]["Amount"]) + "," + iCompId + ")";
                        cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();


                        sSql = "INSERT INTO [" + ProgBillBL.CompanyDBName + "].dbo.EntryTrans(RefId,VoucherDate,TransType,RefType,AccountId,RelatedAccountId,SubLedgerTypeId,SubLedgerId,CostCentreId,Amount,CompanyId)  " +
                               "VALUES (" + iBillRefId + ",'" + dDate.ToString("dd-MMM-yyyy") + "','D','PB'," + Convert.ToInt32(dt.Rows[i]["AccountId"]) + "," + iBuyerAccountId + "," + 8 + "," + iQualSubLedgerId + "," +
                               "" + iFACCId + "," + Convert.ToDecimal(dt.Rows[i]["Amount"]) + "," + iCompId + ")";
                        cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();

                    }
                    else
                    {
                        sSql = "INSERT INTO [" + ProgBillBL.CompanyDBName + "].dbo.EntryTrans(RefId,VoucherDate,TransType,RefType,AccountId,RelatedAccountId,SubLedgerTypeId,SubLedgerId,CostCentreId,Amount,CompanyId)  " +
                                "VALUES (" + iBillRefId + ",'" + dDate.ToString("dd-MMM-yyyy") + "','D','PB'," + iBuyerAccountId + "," + Convert.ToInt32(dt.Rows[i]["AccountId"]) + "," + 3 + "," + iSubLedgerId + "," +
                                 "" + iFACCId + "," + Convert.ToDecimal(dt.Rows[i]["Amount"]) + "," + iCompId + ")";
                        cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();


                        sSql = "INSERT INTO [" + ProgBillBL.CompanyDBName + "].dbo.EntryTrans(RefId,VoucherDate,TransType,RefType,AccountId,RelatedAccountId,SubLedgerTypeId,SubLedgerId,CostCentreId,Amount,CompanyId)  " +
                               "VALUES (" + iBillRefId + ",'" + dDate.ToString("dd-MMM-yyyy") + "','C','PB'," + Convert.ToInt32(dt.Rows[i]["AccountId"]) + "," + iBuyerAccountId + "," + 8 + "," + iQualSubLedgerId + "," +
                               "" + iFACCId + "," + Convert.ToDecimal(dt.Rows[i]["Amount"]) + "," + iCompId + ")";
                        cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();

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
            }

        }

        public static int GetSubLedgerId(int argId, int argTypeId,SqlConnection conn,SqlTransaction tran)
        {
            int iSubLedgerId = 0;
            try
            {
                string sSql = "Select SubLedgerId from [" + BsfGlobal.g_sFaDBName + "].dbo.SubLedgerMaster Where SubLedgerTypeId=" + argTypeId + " and RefId=" + argId;
                SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    iSubLedgerId = Convert.ToInt32(dt.Rows[0]["SubLedgerId"]);
                }
                dt.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
            return iSubLedgerId;
        }

        public static DataSet GetPBillDetailsEdit(int argPBillId,int argCCId)
        {
            DataSet ds = new DataSet();
            try
            {
                DataTable dtQ = new DataTable();
                SqlDataAdapter da;
                string sSql = "";
                string SchType = "";

                int iPayId = 0;
                int iFlatId = 0;
                bool bPayTypewise = false;

                BsfGlobal.OpenCRMDB();

                //sSql = "Select PBDate,AsOnDate,CostCentreId,SchType,StageId Id,Remarks,PaySchId,FlatId,CCPBNo,IncomeId,AdvanceId,BuyerAccountId from dbo.ProgressBillRegister Where PBillId= " + argPBillId;
                sSql = "Select A.PBDate,A.AsOnDate,A.CostCentreId,A.SchType,A.StageId Id,A.Remarks,A.PaySchId," +
                        " A.FlatId,A.CCPBNo,A.IncomeId,A.AdvanceId,A.BuyerAccountId,B.BlockId,B.LevelId From dbo.ProgressBillRegister A " +
                        " Left Join dbo.FlatDetails B On A.FlatId=B.FlatId Where A.PBillId= " + argPBillId + "";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "PBRegister");
                da.Dispose();

                if (ds.Tables["PBRegister"].Rows.Count > 0)
                {
                    SchType = ds.Tables["PBRegister"].Rows[0]["SchType"].ToString();
                    iFlatId = Convert.ToInt32(ds.Tables["PBRegister"].Rows[0]["FlatId"].ToString());
                    iPayId = Convert.ToInt32(ds.Tables["PBRegister"].Rows[0]["PaySchId"].ToString());
                }

                sSql = "Select PBNo BillNo,F.PayTypeId,A.PaySchId PaymentSchId,T.Typewise,A.FlatId,F.FlatNo,R.LeadName BuyerName,A.LeadId, " +
                        "A.SchType,A.StageId,A.BillAmount Amount,A.NetAmount,Convert(bit,1,1)Sel FROM dbo.ProgressBillRegister A " +
                        "INNER JOIN dbo.FlatDetails F  ON A.FlatId=F.FlatId " +
                        "INNER JOIN dbo.LeadRegister R ON A.LeadId=R.LeadId " +
                        " INNER JOIN PaySchType T ON F.PayTypeId=T.TypeId " +
                        "Where A.PBillId= " + argPBillId;
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "PBTrans");
                da.Dispose();

                if (ds.Tables["PBTrans"].Rows.Count > 0)
                {
                    bPayTypewise = Convert.ToBoolean(ds.Tables["PBTrans"].Rows[0]["Typewise"].ToString());
                }

                if (SchType == "O")
                {
                    sSql = "Select PaymentSchId,FlatId,ReceiptTypeId,A.OtherCostId,SchType,Percentage,Amount,NetAmount from dbo.PBReceiptType A " +
                           "Inner Join dbo.OtherCostMaster B on A.OtherCostId=B.OtherCostId " +
                           "Where A.PBillId = " + argPBillId;
                }
                else
                {
                    //sSql = "Select " + iPayId + " PaymentSchId, " + iFlatId + " FlatId,A.ReceiptTypeId,0 OtherCostId,'R' SchType," +
                    //        " Case When B.ReceiptTypeId is null then Convert(bit,0,0) Else Convert(bit,1,1) End Sel," +
                    //        " A.ReceiptTypeName ReceiptType,ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount," +
                    //        " isnull(B.NetAmount,0) NetAmount From dbo.ReceiptType A " +
                    //        " Left Join dbo.PBReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId and B.PBillId = " + argPBillId + " " +
                    //        " Where A.ReceiptTypeId <>1 " +
                    //        " Union All " +
                    //        " Select " + iPayId + " PaymentSchId, " + iFlatId + " FlatId,0 ReceiptTypeId,A.OtherCostId,'O' SchType," +
                    //        " Case When B.ReceiptTypeId is null then Convert(bit,0,0) Else Convert(bit,1,1) End Sel," +
                    //        " A.OtherCostName ReceiptType,ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount," +
                    //        " isnull(B.NetAmount,0) NetAmount From dbo.OtherCostMaster A " +
                    //        " Left Join dbo.PBReceiptType B on A.OtherCostId=B.OtherCostId and B.PBillId = " + argPBillId + " " +
                    //        " Where A.OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans Where CostCentreId=" + argCCId + ")";
                    sSql = "Select " + iPayId + " PaymentSchId, " + iFlatId + " FlatId,A.ReceiptTypeId,0 OtherCostId,'R' SchType," +
                            " Case When B.ReceiptTypeId is null then Convert(bit,0,0) Else Convert(bit,1,1) End Sel," +
                            " A.ReceiptTypeName ReceiptType,ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount," +
                            " isnull(B.NetAmount,0) NetAmount From dbo.ReceiptType A " +
                            " Left Join dbo.PBReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId and B.PBillId = " + argPBillId + " " +
                            " Where A.ReceiptTypeId <>1 " +
                            " Union All " +
                            " Select " + iPayId + " PaymentSchId, " + iFlatId + " FlatId,0 ReceiptTypeId,A.OtherCostId,'O' SchType," +
                            " Case When B.ReceiptTypeId is null then Convert(bit,0,0) Else Convert(bit,1,1) End Sel," +
                            " A.OtherCostName ReceiptType,ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount," +
                            " isnull(B.NetAmount,0) NetAmount From dbo.OtherCostMaster A " +
                            " Inner Join dbo.CCOtherCost CO On CO.OtherCostId=A.OtherCostId And Co.CostCentreId=" + argCCId + " " +
                            " Left Join dbo.PBReceiptType B on A.OtherCostId=B.OtherCostId and B.PBillId = " + argPBillId + " " +
                            " Where A.OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans Where CostCentreId=" + argCCId + ")";
                }
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "ReceiptType");
                da.Dispose();

                sSql = "Select B.Sel,A.QualifierId, A.QualifierName,B.Percentage,B.Amount,B.Amount QAmount " +
                        " from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A  " +
                        " Inner Join dbo.PaySchTaxFlat B On A.QualifierId=B.QualifierId " +
                        " Left Join dbo.FlatTax C On C.QualifierId=B.QualifierId and C.FlatId=B.FlatId" +
                        " Where QualType='B' And B.FlatId=" + iFlatId + " And PaymentSchId=" + iPayId + "";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dtQ = new DataTable();
                da.Fill(dtQ);
                da.Dispose();

                DataRow dr;
                foreach (DataRow drow in dtQ.Rows)
                {
                    dr = ds.Tables["ReceiptType"].NewRow();

                    dr["PaymentSchId"] = iPayId;
                    dr["FlatId"] = iFlatId;
                    dr["ReceiptTypeId"] = Convert.ToInt32(drow["QualifierId"]);
                    dr["OtherCostId"] = 0;
                    dr["SchType"] = "Q";
                    dr["Sel"] = Convert.ToBoolean(drow["Sel"]);
                    dr["ReceiptType"] = drow["QualifierName"].ToString();
                    dr["Percentage"] = Convert.ToDecimal(drow["Percentage"]);
                    dr["Amount"] = Convert.ToDecimal(drow["Amount"]);
                    dr["NetAmount"] = Convert.ToDecimal(drow["Amount"]);

                    ds.Tables["ReceiptType"].Rows.Add(dr);
                }
                //ds.Tables.Add(ds.Tables["ReceiptType"]);

                sSql = "Select B.FlatId,B.PaymentSchId,A.QualifierId,A.Expression,A.ExpPer,A.NetPer,A.Add_Less_Flag,A.SurCharge, " +
                       "A.EDCess,A.HEDPer,A.ExpValue,A.ExpPerValue,A.SurValue,A.EDValue,A.Amount,B.SchType,B.ReceiptTypeId,B.OtherCostId,A.TaxablePer,A.TaxableValue " +
                       "From dbo.PBReceiptTypeQualifier A " +
                       "Inner Join dbo.PBReceiptType B on A.PBid = B.PBId " +
                       "Where B.PBillId= " + argPBillId;

                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Qualifier");
                da.Dispose();


                //sSql = "Select B.PlotDetailsId,B.PaySchId PaymentSchId,A.QualifierId,A.AccountId,A.Add_Less_Flag,A.Amount From dbo.PlotPBQualifierAbs A " +
                //       " Inner Join dbo.PlotProgressBillRegister B on A.PBillId=B.PBillId Where A.PBillId= " + argPBillId;
                sSql = "Select B.FlatId,B.PaySchId PaymentSchId,A.QualifierId,A.AccountId,A.Add_Less_Flag,A.Amount From dbo.PBQualifierAbs A  "+
                        " Inner Join dbo.ProgressBillRegister B on A.PBillId=B.PBillId Where A.PBillId= " + argPBillId;
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "QualifierAbs");
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

        public static DataSet GetPBillDetailsMultipleEdit(int argProgRegId, int argCCId)
        {
            DataSet ds = new DataSet();
            try
            {
                DataTable dtQ = new DataTable();
                SqlDataAdapter da;
                string sSql = "";
                string SchType = "";
                int PBillId = 0;

                int iPayId = 0;
                int iFlatId = 0;
                bool bPayTypewise = false;

                BsfGlobal.OpenCRMDB();

                sSql = "Select PBDate,A.AsOnDate,A.CostCentreId,A.SchType,A.StageId Id,A.Remarks,A.PaySchId,A.FlatId,A.CCPBNo,A.IncomeId, " +
                        " A.AdvanceId,A.BuyerAccountId,C.BlockId,C.LevelId From dbo.ProgressBillRegister A " +
                        " Right Join dbo.ProgressBillMaster B  On A.ProgRegId=B.ProgRegId " +
                        " Left Join dbo.FlatDetails C On A.FlatId=C.FlatId Where A.ProgRegId=" + argProgRegId + " ";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "PBRegister");
                da.Dispose();

                if (ds.Tables["PBRegister"].Rows.Count > 0)
                {
                    sSql = "Select A.PBillId,PBNo BillNo,F.PayTypeId,A.PaySchId PaymentSchId,T.Typewise,A.FlatId,F.FlatNo,R.LeadName BuyerName,A.LeadId, " +
                            "A.SchType,A.StageId,A.BillAmount Amount,A.NetAmount,Convert(bit,1,1)Sel FROM dbo.ProgressBillRegister A " +
                            "INNER JOIN dbo.FlatDetails F  ON A.FlatId=F.FlatId " +
                            "INNER JOIN dbo.LeadRegister R ON A.LeadId=R.LeadId " +
                            " INNER JOIN PaySchType T ON F.PayTypeId=T.TypeId " +
                            "Where A.ProgRegId= " + argProgRegId;
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    da.Fill(ds, "PBTrans");
                    da.Dispose();

                    if (ds.Tables["PBTrans"].Rows.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables["PBTrans"].Rows.Count; j++)
                        {
                            SchType = ds.Tables["PBTrans"].Rows[j]["SchType"].ToString();
                            bPayTypewise = Convert.ToBoolean(ds.Tables["PBTrans"].Rows[j]["Typewise"].ToString());
                            PBillId = Convert.ToInt32(ds.Tables["PBTrans"].Rows[j]["PBillId"]);
                            iFlatId = Convert.ToInt32(ds.Tables["PBTrans"].Rows[j]["FlatId"].ToString());
                            iPayId = Convert.ToInt32(ds.Tables["PBTrans"].Rows[j]["PaymentSchId"].ToString());

                            if (SchType == "O")
                            {
                                sSql = "Select PaymentSchId,FlatId,ReceiptTypeId,A.OtherCostId,SchType,Percentage,Amount,NetAmount from dbo.PBReceiptType A " +
                                       "Inner Join dbo.OtherCostMaster B on A.OtherCostId=B.OtherCostId " +
                                       "Where A.PBillId = " + PBillId;
                            }
                            else
                            {
                                sSql = "Select " + iPayId + " PaymentSchId, " + iFlatId + " FlatId,A.ReceiptTypeId,0 OtherCostId,'R' SchType," +
                                        " Case When B.ReceiptTypeId is null then Convert(bit,0,0) Else Convert(bit,1,1) End Sel," +
                                        " A.ReceiptTypeName ReceiptType,ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount," +
                                        " isnull(B.NetAmount,0) NetAmount From dbo.ReceiptType A " +
                                        " Left Join dbo.PBReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId and B.PBillId = " + PBillId + " " +
                                        " Where A.ReceiptTypeId <>1 " +
                                        " Union All " +
                                        " Select " + iPayId + " PaymentSchId, " + iFlatId + " FlatId,0 ReceiptTypeId,A.OtherCostId,'O' SchType," +
                                        " Case When B.ReceiptTypeId is null then Convert(bit,0,0) Else Convert(bit,1,1) End Sel," +
                                        " A.OtherCostName ReceiptType,ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount," +
                                        " isnull(B.NetAmount,0) NetAmount From dbo.OtherCostMaster A " +
                                        " Inner Join dbo.CCOtherCost CO On CO.OtherCostId=A.OtherCostId And Co.CostCentreId=" + argCCId + " " +
                                        " Left Join dbo.PBReceiptType B on A.OtherCostId=B.OtherCostId and B.PBillId = " + PBillId + " " +
                                        " Where A.OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans Where CostCentreId=" + argCCId + ")";
                            }
                            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                            da.Fill(ds, "ReceiptType");
                            da.Dispose();

                            sSql = "Select B.Sel,A.QualifierId, A.QualifierName,B.Percentage,B.Amount,B.Amount QAmount " +
                                    " from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A  " +
                                    " Inner Join dbo.PaySchTaxFlat B On A.QualifierId=B.QualifierId " +
                                    " Left Join dbo.FlatTax C On C.QualifierId=B.QualifierId and C.FlatId=B.FlatId" +
                                    " Where QualType='B' And B.FlatId=" + iFlatId + " And PaymentSchId=" + iPayId + "";
                            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                            dtQ = new DataTable();
                            da.Fill(dtQ);
                            da.Dispose();

                            DataRow dr;
                            foreach (DataRow drow in dtQ.Rows)
                            {
                                dr = ds.Tables["ReceiptType"].NewRow();

                                dr["PaymentSchId"] = iPayId;
                                dr["FlatId"] = iFlatId;
                                dr["ReceiptTypeId"] = Convert.ToInt32(drow["QualifierId"]);
                                dr["OtherCostId"] = 0;
                                dr["SchType"] = "Q";
                                dr["Sel"] = Convert.ToBoolean(drow["Sel"]);
                                dr["ReceiptType"] = drow["QualifierName"].ToString();
                                dr["Percentage"] = Convert.ToDecimal(drow["Percentage"]);
                                dr["Amount"] = Convert.ToDecimal(drow["Amount"]);
                                dr["NetAmount"] = Convert.ToDecimal(drow["Amount"]);

                                ds.Tables["ReceiptType"].Rows.Add(dr);
                            }

                            sSql = "Select B.FlatId,B.PaymentSchId,A.QualifierId,A.Expression,A.ExpPer,A.NetPer,A.Add_Less_Flag,A.SurCharge, " +
                                   "A.EDCess,A.HEDPer,A.ExpValue,A.ExpPerValue,A.SurValue,A.EDValue,A.Amount,B.SchType,B.ReceiptTypeId,B.OtherCostId,A.TaxablePer,A.TaxableValue " +
                                   "From dbo.PBReceiptTypeQualifier A " +
                                   "Inner Join dbo.PBReceiptType B on A.PBid = B.PBId " +
                                   "Where B.PBillId= " + PBillId;

                            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                            da.Fill(ds, "Qualifier");
                            da.Dispose();

                            sSql = "Select B.FlatId,B.PaySchId PaymentSchId,A.QualifierId,A.AccountId,A.Add_Less_Flag,A.Amount From dbo.PBQualifierAbs A  " +
                                     " Inner Join dbo.ProgressBillRegister B on A.PBillId=B.PBillId Where A.PBillId= " + PBillId;
                            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                            da.Fill(ds, "QualifierAbs");
                            da.Dispose();
                        }
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
            return ds;
        }

        public static DataSet GetPlotPBillDetailsEdit(int argPBillId,int argLandId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter da;
                string sSql = "";
                int iPlotId = 0;
                string SchType = "";
                int iPayId = 0;

                BsfGlobal.OpenCRMDB();

                sSql = "Select PBDate,AsOnDate,CostCentreId,SchType,StageId Id,Remarks,PaySchId,PlotDetailsId FlatId,CCPBNo,IncomeId,AdvanceId,BuyerAccountId from dbo.PlotProgressBillRegister Where PBillId= " + argPBillId;
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "PBRegister");
                da.Dispose();

                if (ds.Tables["PBRegister"].Rows.Count > 0)
                {
                    SchType = ds.Tables["PBRegister"].Rows[0]["SchType"].ToString();
                    iPlotId = Convert.ToInt32(ds.Tables["PBRegister"].Rows[0]["FlatId"].ToString());
                    iPayId = Convert.ToInt32(ds.Tables["PBRegister"].Rows[0]["PaySchId"].ToString());
                }

                sSql = "Select PBNo BillNo,A.PaySchId PaymentSchId,A.PlotDetailsId FlatId,F.PlotNo,R.LeadName BuyerName,A.BuyerId LeadId, " +
                        "A.SchType,A.StageId,A.BillAmount Amount,A.NetAmount,Convert(bit,1,1)Sel FROM dbo.PlotProgressBillRegister A " +
                        "INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails F  ON A.PlotDetailsId=F.PlotDetailsId " +
                        "INNER JOIN dbo.LeadRegister R ON A.BuyerId=R.LeadId " +
                        "Where A.PBillId= " + argPBillId;
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "PBTrans");
                da.Dispose();

                if (SchType == "O")
                {
                    sSql = "Select PaymentSchId,PlotId FlatId,ReceiptTypeId,A.OtherCostId,SchType,Percentage,Amount,NetAmount " +
                            " From dbo.PlotPBReceiptType A Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.OtherCostMaster B " +
                            " On A.OtherCostId=B.OtherCostId Where A.PBillId = " + argPBillId;
                }
                else
                {
                    sSql = "Select " + iPayId + " PaymentSchId, " + iPlotId + " FlatId,A.ReceiptTypeId,0 OtherCostId,'R' SchType, " +
                            " Case When B.ReceiptTypeId is null then Convert(bit,0,0) Else Convert(bit,1,1) End Sel, " +
                            " A.ReceiptTypeName ReceiptType,ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount, " +
                            " isnull(B.NetAmount,0) NetAmount From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ReceiptType A  " +
                            " Left Join dbo.PlotPBReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId and B.PBillId = " + argPBillId + "  " +
                            " Where A.ReceiptTypeId <>1  " +
                            " Union All  " +
                            " Select " + iPayId + " PaymentSchId, " + iPlotId + " FlatId,0 ReceiptTypeId,A.OtherCostId,'O' SchType, " +
                            " Case When B.ReceiptTypeId is null then Convert(bit,0,0) Else Convert(bit,1,1) End Sel, " +
                            " A.OtherCostName ReceiptType,ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount, " +
                            " isnull(B.NetAmount,0) NetAmount From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.OtherCostMaster A  " +
                            " Left Join dbo.PlotPBReceiptType B on A.OtherCostId=B.OtherCostId and B.PBillId = " + argPBillId + "  " +
                            " Where A.OtherCostId in (Select OtherCostId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.OtherCostSetupTrans " +
                            " Where LandRegId=" + argLandId + ")";
                }
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "ReceiptType");
                da.Dispose();

                sSql = "Select B.PlotId FlatId,B.PaymentSchId,A.QualifierId,A.Expression,A.ExpPer,A.Add_Less_Flag,A.SurCharge, " +
                        " A.EDCess,A.ExpValue,A.ExpPerValue,A.SurValue,A.EDValue,A.Amount,B.SchType,B.ReceiptTypeId,B.OtherCostId,A.TaxablePer,A.Taxablevalue " +
                        " From dbo.PlotPBQualifier A Inner Join dbo.PlotPBReceiptType B on A.PBId = B.PBId " +
                        " Where B.PBillId=" + argPBillId;

                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Qualifier");
                da.Dispose();


                sSql = "Select B.PlotDetailsId FlatId,A.QualifierId,A.AccountId,A.Add_Less_Flag,A.Amount from dbo.PlotPBQualifierAbs A " +
                      "Inner Join dbo.PlotProgressBillRegister B on A.PBillId=B.PBillId Where A.PBillId= " + argPBillId;
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "QualifierAbs");
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

        public static DataSet GetPlotPBillDetailsMultipleEdit(int argProgRegId, int argLandId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter da;
                string sSql = "";
                int iPlotId = 0;
                string SchType = "";
                int iPayId = 0;
                int PBillId = 0;

                BsfGlobal.OpenCRMDB();

                sSql = "Select PBDate,A.AsOnDate,A.CostCentreId,A.SchType,A.StageId Id,A.Remarks,PaySchId,PlotDetailsId FlatId, " +
                        " CCPBNo,IncomeId,AdvanceId,BuyerAccountId From dbo.PlotProgressBillRegister A " +
                        " Inner Join dbo.PlotProgressBillMaster B On A.ProgRegId=B.ProgRegId " +
                        " Where A.ProgRegId=" + argProgRegId + "";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "PBRegister");
                da.Dispose();

                if (ds.Tables["PBRegister"].Rows.Count > 0)
                {
                    //SchType = ds.Tables["PBRegister"].Rows[0]["SchType"].ToString();
                    //iPlotId = Convert.ToInt32(ds.Tables["PBRegister"].Rows[0]["FlatId"].ToString());
                    //iPayId = Convert.ToInt32(ds.Tables["PBRegister"].Rows[0]["PaySchId"].ToString());

                    sSql = "Select PBillId,PBNo BillNo,A.PaySchId PaymentSchId,A.PlotDetailsId FlatId,F.PlotNo,R.LeadName BuyerName,A.BuyerId LeadId, " +
                            "A.SchType,A.StageId,A.BillAmount Amount,A.NetAmount,Convert(bit,1,1)Sel FROM dbo.PlotProgressBillRegister A " +
                            "INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails F ON A.PlotDetailsId=F.PlotDetailsId " +
                            "INNER JOIN dbo.LeadRegister R ON A.BuyerId=R.LeadId " +
                            "Where A.ProgRegId= " + argProgRegId;
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    da.Fill(ds, "PBTrans");
                    da.Dispose();

                    if (ds.Tables["PBTrans"].Rows.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables["PBTrans"].Rows.Count; j++)
                        {
                            SchType = ds.Tables["PBTrans"].Rows[j]["SchType"].ToString();
                            iPlotId = Convert.ToInt32(ds.Tables["PBTrans"].Rows[j]["FlatId"].ToString());
                            iPayId = Convert.ToInt32(ds.Tables["PBTrans"].Rows[j]["PaymentSchId"].ToString());
                            PBillId = Convert.ToInt32(ds.Tables["PBTrans"].Rows[j]["PBillId"].ToString());

                            if (SchType == "O")
                            {
                                sSql = "Select PaymentSchId,PlotId FlatId,ReceiptTypeId,A.OtherCostId,SchType,Percentage,Amount,NetAmount " +
                                        " From dbo.PlotPBReceiptType A Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.OtherCostMaster B " +
                                        " On A.OtherCostId=B.OtherCostId Where A.PBillId = " + PBillId;
                            }
                            else
                            {
                                sSql = "Select " + iPayId + " PaymentSchId, " + iPlotId + " FlatId,A.ReceiptTypeId,0 OtherCostId,'R' SchType, " +
                                        " Case When B.ReceiptTypeId is null then Convert(bit,0,0) Else Convert(bit,1,1) End Sel, " +
                                        " A.ReceiptTypeName ReceiptType,ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount, " +
                                        " isnull(B.NetAmount,0) NetAmount From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ReceiptType A  " +
                                        " Left Join dbo.PlotPBReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId and B.PBillId = " + PBillId + "  " +
                                        " Where A.ReceiptTypeId <>1  " +
                                        " Union All  " +
                                        " Select " + iPayId + " PaymentSchId, " + iPlotId + " FlatId,0 ReceiptTypeId,A.OtherCostId,'O' SchType, " +
                                        " Case When B.ReceiptTypeId is null then Convert(bit,0,0) Else Convert(bit,1,1) End Sel, " +
                                        " A.OtherCostName ReceiptType,ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount, " +
                                        " isnull(B.NetAmount,0) NetAmount From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.OtherCostMaster A  " +
                                        " Left Join dbo.PlotPBReceiptType B on A.OtherCostId=B.OtherCostId and B.PBillId = " + PBillId + "  " +
                                        " Where A.OtherCostId in (Select OtherCostId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.OtherCostSetupTrans " +
                                        " Where LandRegId=" + argLandId + ")";
                            }
                            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                            da.Fill(ds, "ReceiptType");
                            da.Dispose();

                            sSql = "Select B.PlotId FlatId,B.PaymentSchId,A.QualifierId,A.Expression,A.ExpPer,A.Add_Less_Flag,A.SurCharge, " +
                                    " A.EDCess,A.ExpValue,A.ExpPerValue,A.SurValue,A.EDValue,A.Amount,B.SchType,B.ReceiptTypeId,B.OtherCostId " +
                                    " From dbo.PlotPBQualifier A Inner Join dbo.PlotPBReceiptType B on A.PBId = B.PBId " +
                                    " Where B.PBillId=" + PBillId;

                            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                            da.Fill(ds, "Qualifier");
                            da.Dispose();


                            sSql = "Select B.PlotDetailsId FlatId,A.QualifierId,A.AccountId,A.Add_Less_Flag,A.Amount from dbo.PlotPBQualifierAbs A " +
                                  "Inner Join dbo.PlotProgressBillRegister B on A.PBillId=B.PBillId Where A.PBillId= " + PBillId;
                            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                            da.Fill(ds, "QualifierAbs");
                            da.Dispose();
                        }
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
            return ds;
        }

        public static DataTable GetQual(int argTempId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = String.Format("Select *,B.PaymentSchId,B.ReceiptTypeId,B.OtherCostId,B.SchType From dbo.FlatReceiptQualifier A" +
                    " Inner Join dbo.FlatReceiptType B on A.SchId=B.SchId Where B.PaymentSchId= {0}", argTempId);
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

        public static void DeletePBill(int argId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction trans = conn.BeginTransaction();

            try
            {
                string sSql = "";
                SqlCommand cmd; SqlDataReader dr; DataTable dt = new DataTable();

                //int iRefId = 0;
                //string sDBName = "";
                //sSql = "Select RefId,DBName from dbo.ProgressBillRegister Where PBillId= " + argId;
                //cmd = new SqlCommand(sSql, conn,trans);
                //SqlDataReader dr = cmd.ExecuteReader();
                //DataTable dt = new DataTable();
                //dt.Load(dr);
                //dr.Close();
                //cmd.Dispose();
                //if (dt.Rows.Count > 0)
                //{
                //    iRefId = Convert.ToInt32(dt.Rows[0]["RefId"]);
                //    sDBName = CommFun.IsNullCheck(dt.Rows[0]["DBName"], CommFun.datatypes.vartypestring).ToString();
                //}
                //dt.Dispose();

                sSql = "Update dbo.PaymentScheduleFlat Set PaymentScheduleFlat.BillAmount = PaymentScheduleFlat.BillAmount - ProgressBillRegister.NetAmount,BillPassed=0 From dbo.ProgressBillRegister " +
                       "Where PaymentScheduleFlat.PaymentSchId=ProgressBillRegister.PaySchId and PaymentScheduleFlat.FlatId = ProgressBillRegister.FlatId " +
                       "and ProgressBillRegister.PBillId = " + argId;
                cmd = new SqlCommand(sSql,conn,trans); cmd.ExecuteNonQuery(); cmd.Dispose();

                sSql = "Delete From dbo.PBReceiptTypeQualifier Where PBId in (Select PBId from dbo.PBReceiptType Where PBillId  = " + argId + ")";
                cmd = new SqlCommand(sSql, conn, trans); cmd.ExecuteNonQuery(); cmd.Dispose();

                sSql = "Delete from dbo.PBReceiptType Where PBillId = " + argId;
                cmd = new SqlCommand(sSql,conn,trans); cmd.ExecuteNonQuery(); cmd.Dispose();

                sSql = "Delete from dbo.ProgressBillRegister Where PBillId = " + argId;
                cmd = new SqlCommand(sSql,conn,trans); cmd.ExecuteNonQuery(); cmd.Dispose();


                //sSql = "Delete from [" + sDBName + "].dbo.EntryTrans Where RefId = " + iRefId + " and RefType = 'PB'";
                //cmd = new SqlCommand(sSql, conn, trans); cmd.ExecuteNonQuery(); cmd.Dispose();

                //sSql = "Delete from [" + BsfGlobal.g_sFaDBName + "].dbo.BillRegister Where BillRegisterId = " + iRefId;
                //cmd = new SqlCommand(sSql, conn, trans); cmd.ExecuteNonQuery(); cmd.Dispose();

                int iProgRegId = 0;

                sSql = "Select ProgRegId from dbo.ProgressBillRegister Where PBillId = " + argId;
                cmd = new SqlCommand(sSql, conn, trans);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0) { iProgRegId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["ProgRegId"], CommFun.datatypes.vartypenumeric)); }
                dt.Dispose();

                sSql = "Select Sum(NetAmount) Amount from dbo.ProgressBillRegister Where ProgRegId = " + iProgRegId;
                cmd = new SqlCommand(sSql, conn, trans);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();
                decimal dNAmt = 0;
                if (dt.Rows.Count > 0) { dNAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }
                dt.Dispose();

                sSql = "Update dbo.ProgressBillMaster Set NetAmount = " + dNAmt + " Where ProgRegId = " + iProgRegId;
                cmd = new SqlCommand(sSql, conn, trans); cmd.ExecuteNonQuery(); cmd.Dispose();

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        public static void DeletePlotPBill(int argId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction trans = conn.BeginTransaction();

            try
            {
                string sSql = "";
                SqlCommand cmd; SqlDataReader dr; DataTable dt = new DataTable();

                sSql = "Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot Set [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot.BillAmount = [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot.BillAmount - PlotProgressBillRegister.NetAmount " +
                        " From dbo.PlotProgressBillRegister Where [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot.PaymentSchId=PlotProgressBillRegister.PaySchId " +
                        " And [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot.PlotDetailsId = PlotProgressBillRegister.PlotDetailsId and PlotProgressBillRegister.PBillId= " + argId;
                cmd = new SqlCommand(sSql, conn, trans); cmd.ExecuteNonQuery(); cmd.Dispose();

                sSql = "Delete From dbo.PlotPBQualifier Where PBId in (Select PBId from dbo.PlotPBReceiptType Where PBillId = " + argId + " )";
                cmd = new SqlCommand(sSql, conn, trans); cmd.ExecuteNonQuery(); cmd.Dispose();

                sSql = "Delete from dbo.PlotPBReceiptType Where PBillId = " + argId;
                cmd = new SqlCommand(sSql, conn, trans); cmd.ExecuteNonQuery(); cmd.Dispose();

                sSql = "Delete from dbo.PlotProgressBillRegister Where PBillId = " + argId;
                cmd = new SqlCommand(sSql, conn, trans); cmd.ExecuteNonQuery(); cmd.Dispose();

                int iProgRegId = 0;

                sSql = "Select ProgRegId from dbo.PlotProgressBillRegister Where PBillId = " + argId;
                cmd = new SqlCommand(sSql, conn, trans);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0) { iProgRegId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["ProgRegId"], CommFun.datatypes.vartypenumeric)); }
                dt.Dispose();

                sSql = "Select Sum(NetAmount) Amount from dbo.PlotProgressBillRegister Where ProgRegId = " + iProgRegId;
                cmd = new SqlCommand(sSql, conn, trans);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();
                decimal dNAmt = 0;
                if (dt.Rows.Count > 0) { dNAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }
                dt.Dispose();

                sSql = "Update dbo.PlotProgressBillMaster Set NetAmount = " + dNAmt + " Where ProgRegId = " + iProgRegId;
                cmd = new SqlCommand(sSql, conn, trans); cmd.ExecuteNonQuery(); cmd.Dispose();

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        public static void DeletePBillMaster(int argId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction trans = conn.BeginTransaction();

            try
            {
                string sSql = "";
                SqlCommand cmd; SqlDataReader dr; DataTable dt = new DataTable();

                sSql = "Delete From ProgressBillMaster Where ProgRegId=" + argId + "";
                cmd = new SqlCommand(sSql, conn, trans); cmd.ExecuteNonQuery(); cmd.Dispose();

                int iProgRegId = 0; int iPBillId = 0;

                sSql = "Select ProgRegId,PBillId from dbo.ProgressBillRegister Where ProgRegId = " + argId;
                cmd = new SqlCommand(sSql, conn, trans);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0) 
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        iProgRegId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["ProgRegId"], CommFun.datatypes.vartypenumeric));
                        iPBillId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["PBillId"], CommFun.datatypes.vartypenumeric));

                        sSql = "Update dbo.PaymentScheduleFlat Set PaymentScheduleFlat.BillAmount = PaymentScheduleFlat.BillAmount - ProgressBillRegister.NetAmount,BillPassed=0 from dbo.ProgressBillRegister " +
                               "Where PaymentScheduleFlat.PaymentSchId=ProgressBillRegister.PaySchId and PaymentScheduleFlat.FlatId = ProgressBillRegister.FlatId " +
                               "and ProgressBillRegister.PBillId = " + iPBillId;
                        cmd = new SqlCommand(sSql, conn, trans); cmd.ExecuteNonQuery(); cmd.Dispose();

                        sSql = "Delete From dbo.PBReceiptTypeQualifier Where PBId in (Select PBId from dbo.PBReceiptType Where PBillId  = " + iPBillId + ")";
                        cmd = new SqlCommand(sSql, conn, trans); cmd.ExecuteNonQuery(); cmd.Dispose();

                        sSql = "Delete from dbo.PBReceiptType Where PBillId = " + iPBillId;
                        cmd = new SqlCommand(sSql, conn, trans); cmd.ExecuteNonQuery(); cmd.Dispose();

                        sSql = "Delete from dbo.ProgressBillRegister Where PBillId = " + iPBillId;
                        cmd = new SqlCommand(sSql, conn, trans); cmd.ExecuteNonQuery(); cmd.Dispose();
                    }
                }
                dt.Dispose();

                
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        public static void DeletePlotPBillMaster(int argId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                string sSql = "Delete From dbo.PlotProgressBillMaster Where ProgRegId=" + argId + "";
                SqlCommand cmd = new SqlCommand(sSql, conn, trans); 
                cmd.ExecuteNonQuery(); 
                cmd.Dispose();

                sSql = "Select ProgRegId,PBillId From dbo.PlotProgressBillRegister Where ProgRegId = " + argId;
                cmd = new SqlCommand(sSql, conn, trans);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int iProgRegId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["ProgRegId"], CommFun.datatypes.vartypenumeric));
                        int iPBillId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["PBillId"], CommFun.datatypes.vartypenumeric));

                        string sDB = BsfGlobal.g_sRateAnalDBName;

                        sSql = "Update [" + sDB + "].dbo.PaymentSchedulePlot " +
                               " Set BillPassed=0, [" + sDB + "].dbo.PaymentSchedulePlot.BillAmount=[" + sDB + "].dbo.PaymentSchedulePlot.NetAmount-PlotProgressBillRegister.NetAmount " +
                               " From dbo.PlotProgressBillRegister Where [" + sDB + "].dbo.PaymentSchedulePlot.PaymentSchId=PlotProgressBillRegister.PaySchId " +
                               " AND [" + sDB + "].dbo.PaymentSchedulePlot.PlotDetailsId=PlotProgressBillRegister.PlotDetailsId AND dbo.PlotProgressBillRegister.PBillId=" + argId;
                        cmd = new SqlCommand(sSql, conn, trans); cmd.ExecuteNonQuery(); cmd.Dispose();

                        sSql = "Delete From dbo.PlotPBQualifier Where PBId IN(Select PBId from dbo.PlotPBReceiptType Where PBillId=" + iPBillId + ") ";
                        cmd = new SqlCommand(sSql, conn, trans); cmd.ExecuteNonQuery(); cmd.Dispose();

                        sSql = "Delete from dbo.PlotPBReceiptType Where PBillId=" + iPBillId;
                        cmd = new SqlCommand(sSql, conn, trans); cmd.ExecuteNonQuery(); cmd.Dispose();

                        sSql = "Delete from dbo.PlotProgressBillRegister Where ProgRegId=" + iProgRegId;
                        cmd = new SqlCommand(sSql, conn, trans); cmd.ExecuteNonQuery(); cmd.Dispose();
                    }
                }
                dt.Dispose();
                
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }
        }

        public static bool GetApprFound(int argRegId,int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            bool bAns = false;
            try
            {
                string sSql = " Select A.Approve From ProgressBillMaster A Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B " +
                       " On A.CostCentreId=B.CostCentreId Where A.CostCentreId= " + argCCId + " And A.ProgRegId=" + argRegId + " ";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Approve"].ToString() == "Y" || dt.Rows[0]["Approve"].ToString() == "P") { bAns = true; }
                }
                dt.Dispose();
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


        internal static bool GetReceiptRaised(int argRegId, int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            bool bAns = false;
            try
            {
                string sSql = " Select COUNT(*) From ProgressBillRegister A" +
                              " Inner Join dbo.ReceiptTrans B ON A.PaySchId=B.PaySchId" +
                              " Where A.CostCentreId= " + argCCId + " And A.ProgRegId=" + argRegId + " ";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                int i_Count = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                cmd.Dispose();

                if (i_Count > 0)
                    bAns = true;
                else
                    bAns = false;
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

        public static DataTable GetPendingPBills(int argCCId, int argSId, string arg_sStage)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            //string arg_sStage = "";

            BsfGlobal.OpenCRMDB();
            try
            {
                int iLevelId = 0;
                sSql = "Select * From dbo.StageDetails Where CostCentreId= " + argCCId;
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);

                DataTable dtT = new DataTable();
                dtT.Columns.Add("Type", typeof(string));
                dtT.Columns.Add("FlatNo", typeof(string));
                dtT.Columns.Add("Id", typeof(int));
                dtT.Columns.Add("ParentId", typeof(int));
                //DataRow dr;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    iLevelId = Convert.ToInt32(dt.Rows[i]["LevelId"].ToString());



                    if (iLevelId > 0)
                    {
                        if (arg_sStage == "SchDescription")
                        {
                            sSql = String.Format("SELECT CAST('' as Varchar(20))BillNo,P.PaymentSchId,F.FlatId,F.FlatNo,F.PayTypeId,B.LeadId,R.LeadName BuyerName,P.SchType,P.SchDescId StageId,P.Amount,P.NetAmount,P.BillAmount, 0 CurrentAmount,Convert(bit,0,0)Sel FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId=B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND S.LevelId =F.LevelId AND P.SchType=S.SchType AND S.StageId={0} WHERE P.CostCentreId={1} AND P.SchDescId={0} AND  P.Amount<>P.BillAmount ORDER BY PaymentSchId", argSId, argCCId);
                        }
                        else if (arg_sStage == "Stagewise")
                        {
                            sSql = String.Format("SELECT CAST('' as Varchar(20))BillNo,P.PaymentSchId,F.FlatId,F.FlatNo,F.PayTypeId,B.LeadId,R.LeadName BuyerName,P.SchType,P.StageId StageId,P.Amount,P.NetAmount,P.BillAmount, 0 CurrentAmount,Convert(bit,0,0)Sel FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId =B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND S.LevelId =F.LevelId AND P.SchType=S.SchType AND S.StageId={0} WHERE P.CostCentreId={1} AND P.StageId={0} AND  P.Amount<>P.BillAmount ORDER BY PaymentSchId", argSId, argCCId);
                        }
                        else if (arg_sStage == "OtherCost")
                        {
                            sSql = String.Format("SELECT CAST('' as Varchar(20))BillNo,P.PaymentSchId,F.FlatId,F.FlatNo,F.PayTypeId,B.LeadId,R.LeadName BuyerName,P.SchType,P.OtherCostId StageId,P.Amount,P.NetAmount,P.BillAmount, 0 CurrentAmount,Convert(bit,0,0)Sel FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId=B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND S.LevelId=F.LevelId AND P.SchType=S.SchType AND S.StageId={0} WHERE P.CostCentreId={1} AND P.OtherCostId={0} AND  P.Amount<>P.BillAmount ORDER BY PaymentSchId", argSId, argCCId);
                        }
                    }
                    else
                    {
                        if (arg_sStage == "SchDescription")
                        {
                            sSql = String.Format("SELECT CAST('' as Varchar(20))BillNo,P.PaymentSchId,F.FlatId,F.FlatNo,F.PayTypeId,B.LeadId,R.LeadName BuyerName,P.SchType,P.SchDescId StageId,P.Amount,P.NetAmount,P.BillAmount, 0 CurrentAmount,Convert(bit,0,0)Sel FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId=B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND P.SchType=S.SchType AND S.StageId={0} WHERE P.CostCentreId={1} AND P.SchDescId={0} AND  P.Amount<>P.BillAmount ORDER BY PaymentSchId", argSId, argCCId);
                        }
                        else if (arg_sStage == "Stagewise")
                        {
                            sSql = String.Format("SELECT CAST('' as Varchar(20))BillNo,P.PaymentSchId,F.FlatId,F.FlatNo,F.PayTypeId,B.LeadId,R.LeadName BuyerName,P.SchType,P.StageId StageId,P.Amount,P.NetAmount,P.BillAmount, 0 CurrentAmount,Convert(bit,0,0)Sel FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId =B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND P.SchType=S.SchType AND S.StageId={0} WHERE P.CostCentreId={1} AND P.StageId={0} AND  P.Amount<>P.BillAmount ORDER BY PaymentSchId", argSId, argCCId);
                        }
                        else if (arg_sStage == "OtherCost")
                        {
                            sSql = String.Format("SELECT CAST('' as Varchar(20))BillNo,P.PaymentSchId,F.FlatId,F.FlatNo,F.PayTypeId,B.LeadId,R.LeadName BuyerName,P.SchType,P.OtherCostId StageId,P.Amount,P.NetAmount,P.BillAmount, 0 CurrentAmount,Convert(bit,0,0)Sel FROM dbo.PaymentScheduleFlat P INNER JOIN dbo.FlatDetails F  ON P.FlatId=F.FlatId INNER JOIN dbo.BuyerDetail B ON F.LeadId=B.LeadId AND F.FlatId=B.FlatId INNER JOIN dbo.LeadRegister R ON B.LeadId=R.LeadId INNER JOIN dbo.StageDetails S ON S.CostCentreId=F.CostCentreId AND S.BlockId=F.BlockId AND P.SchType=S.SchType AND S.StageId={0} WHERE P.CostCentreId={1} AND P.OtherCostId={0} AND  P.Amount<>P.BillAmount ORDER BY PaymentSchId", argSId, argCCId);
                        }
                    }

                    //sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    //sda.Fill(ds, "PaymentScheduleFlat");
                    //sda.Dispose();
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

        public static void UpdateQualifiers()
        {

            string sSql = "Insert dbo.QualifierAccount(QualifierId) " +
                          "Select QualifierId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp " +
                          "Where QualType='B' and QualifierId Not in (Select QualifierId from dbo.QualifierAccount)";
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            cmd.ExecuteNonQuery();
            cmd.Dispose();


            sSql = "Insert dbo.PlotQualifierAccount(QualifierId) " +
                   "Select QualifierId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp " +
                   "Where QualType='B' and QualifierId Not in (Select QualifierId from dbo.PlotQualifierAccount)";
            cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            BsfGlobal.g_CRMDB.Close();


        }

        public static DataTable GetQualifierAccount(string argBus)
        {
            string sSql ="";
            if(argBus=="B")
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

        public static DataTable GetAccountQualifier(string argBType)
        {
            DataTable dt;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            if(argBType=="B")
            sSql = "SELECT QualifierId,AccountId FROM dbo.QualifierAccount WHERE isnull(AccountId,0)<>0";
            else sSql = "SELECT QualifierId,AccountId FROM dbo.PlotQualifierAccount WHERE isnull(AccountId,0)<>0";
            sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            dt = new DataTable();
            sda.Fill(dt);
            sda.Dispose();
            BsfGlobal.g_CRMDB.Close();
            return dt;
        }

        public static void UpdateQualAccount(DataTable argDt,string argBType)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sSql = "";
                SqlCommand cmd;
                if(argBType=="B")
                sSql = "Truncate Table dbo.QualifierAccount";
                else sSql = "Truncate Table dbo.PlotQualifierAccount";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                int iQualId = 0;
                int iAccountId = 0;

                for (int i = 0; i < argDt.Rows.Count; i++)
                {
                    iQualId  = Convert.ToInt32(argDt.Rows[i]["QualifierId"]);
                    iAccountId  = Convert.ToInt32(argDt.Rows[i]["AccountId"]);

                    if(argBType=="B")
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

        public static DataTable GetQualAcct()
        {
            DataTable dt;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            if(BsfGlobal.g_bFADB==true)
                sSql = "SELECT AccountId,AccountName FROM [" + BsfGlobal.g_sFaDBName + "].dbo.AccountMaster WHERE LastLevel='Y' AND TypeId=15";
            else
                sSql = "SELECT 0 AccountId,'' AccountName Where 1=0";
            sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            dt = new DataTable();
            sda.Fill(dt);
            sda.Dispose();
            BsfGlobal.g_CRMDB.Close();
            return dt;
        }

        public static int GetQualId(int argId,SqlConnection conn,SqlTransaction tran)
        {
            string sSql = "Select QualMId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp Where QualifierId= " + argId;
            SqlCommand cmd = new SqlCommand(sSql, conn, tran);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            cmd.Dispose();

            int iQualId = 0;
            if (dt.Rows.Count > 0)
            {
                iQualId = Convert.ToInt32(dt.Rows[0]["QualMId"]);
            }
            dt.Dispose();

            return iQualId;
        }

        public static void UpdatePBAccountSetup(int argIncomeId, int argBuyerId, int argAdvanceId,string argBType)
        {
            SqlConnection conn = new SqlConnection();
            SqlTransaction tran;
            conn = BsfGlobal.OpenCRMDB();
            tran = conn.BeginTransaction();
            string sSql = "";
            try
            {
                if(argBType=="B")
                sSql = "Truncate Table dbo.PBAccountSetup";
                else sSql = "Truncate Table dbo.PlotPBAccountSetup";
                SqlCommand cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();

                if (argBType == "B")
                sSql = "Insert into dbo.PBAccountSetup(IncomeAccountId,BuyerAccountId,AdvanceAccountId) " +
                       "Values(" + argIncomeId + "," + argBuyerId + "," + argAdvanceId + ")";
                else
                    sSql = "Insert into dbo.PlotPBAccountSetup(IncomeAccountId,BuyerAccountId,AdvanceAccountId) " +
                       "Values(" + argIncomeId + "," + argBuyerId + "," + argAdvanceId + ")";
                cmd = new SqlCommand(sSql, conn, tran); cmd.ExecuteNonQuery(); cmd.Dispose();
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
                    if (Convert.ToBoolean(dt.Rows[0]["ProgressBillFAUpdate"])==true) { bAns = true; }
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

        #region DemandLetter

        public static DataTable GetAgeDesc(int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select AgeId,AgeDesc,FromDays,ToDays,ReportName From dbo.DemandLetterSetup Where CostCentreId=" + argCCId + "";
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

        public static DataSet GetDemandAge(int argFromDays,int argToDays,int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            DataSet dsAge = new DataSet("Age");

            string s_MinValue = string.Format("{0:dd-MMM-yyyy}", DateTime.MinValue);
            try
            {
                string sSql = " SELECT DISTINCT A.PBillId,A.PBNo BillNo,A.CostCentreId,E.FlatId,A.PaySchId,A.LeadId,E.FlatNo,F.Description,D.LeadName BuyerName," +
                                " Count=IsNull((Select Count(PBillId) From dbo.DLSentDet X Where X.PBillId=A.PBillId Group By PBillId),0)," +
                                " NoOfDays=ISNULL(DATEDIFF(d,DateAdd(d,0,Case When A.PBDate>GETDATE() Then NULL Else A.PBDate End), "+
                                " Case When A.PBDate>GETDATE() Then NULL Else GETDATE() End),0)," +
                                " Cast(0 as bit)Sel,F.SortOrder FROM ProgressBillRegister A  " +
                                " INNER Join BuyerDetail C on A.FlatId=C.FlatId AND A.LeadId=C.LeadId " +
                                " INNER Join LeadRegister D on A.LeadId=D.LeadId " +
                                " INNER Join FlatDetails E on A.FlatId=E.FlatId AND A.LeadId=E.LeadId AND A.CostCentreId=E.CostCentreId  " +
                                " INNER Join PaymentScheduleFlat F On A.PaySchId=F.PaymentSchId " +
                                " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND A.LeadId=AC.BuyerId " +
                                " Where A.CostCentreId=" + argCCId + " And C.Status='S' And A.NetAmount<>F.PaidAmount AND A.NetAmount>F.PaidAmount " +
                                " AND A.PBDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                                " ORDER BY F.SortOrder, D.LeadName";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();

                sSql = " SELECT Distinct A.PBillId,A.PBNo BillNo,0 CostCentreId,0 FlatId,A.PaySchId,A.LeadId,E.FlatNo,0 NoOfDays,D.LeadName BuyerName, " +
                      " Count=IsNull((Select Count(PBillId) From dbo.DLSentDet X Where X.PBillId=A.PBillId Group By PBillId),0)," +
                      " Cast(0 as bit)Sel,F.SortOrder FROM ProgressBillRegister A   " +
                      " INNER Join BuyerDetail C on A.FlatId=C.FlatId AND A.LeadId=C.LeadId " +
                      " INNER Join LeadRegister D on C.LeadId=D.LeadId " +
                      " INNER Join FlatDetails E on A.FlatId=E.FlatId AND A.LeadId=E.LeadId AND A.CostCentreId=E.CostCentreId " +
                      " INNER Join PaymentScheduleFlat F On A.PaySchId=F.PaymentSchId " +
                      " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND A.LeadId=AC.BuyerId " +
                      " Where A.CostCentreId=" + argCCId + " And C.Status='S' And A.NetAmount<>F.PaidAmount AND A.NetAmount>F.PaidAmount " +
                      " AND A.PBDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                      " ORDER BY F.SortOrder, D.LeadName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtB = new DataTable();
                sda.Fill(dtB);
                sda.Dispose();

                DataTable dtAge = new DataTable();
                dtAge = dt.Clone();

                if (dt.Rows.Count > 0)
                {
                    DataView dv = new DataView(dt);
                    if (argToDays == 0)
                        dv.RowFilter = "NoOfDays>=" + argFromDays + "";
                    else
                        dv.RowFilter = "NoOfDays>=" + argFromDays + " And NoOfDays<=" + argToDays + "";

                    dtAge = dv.ToTable();
                }

                dsAge.Tables.Add(dt);
                dsAge.Tables.Add(dtAge);
                dsAge.Tables.Add(dtB);
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dsAge;
        }

        public static DataSet GetDemandAgeStatus(int argFromDays, int argToDays, int argCCId)
        {
            DataSet dsAge = new DataSet("Age");

            DataSet ds = new DataSet();
            DataTable dt = null;
            DataTable dtT = new DataTable();
            SqlDataAdapter sda;

            string sSql = "";
            BsfGlobal.OpenCRMDB();

            try
            {
                dtT.Columns.Add("PBillId", typeof(int));
                dtT.Columns.Add("BillNo", typeof(string));
                dtT.Columns.Add("PBDate", typeof(DateTime));
                dtT.Columns.Add("CostCentreId", typeof(int));
                dtT.Columns.Add("FlatId", typeof(int));
                dtT.Columns.Add("PaySchId", typeof(int));
                dtT.Columns.Add("LeadId", typeof(int));
                dtT.Columns.Add("FlatNo", typeof(string));
                dtT.Columns.Add("Description", typeof(string));
                dtT.Columns.Add("BuyerName", typeof(string));
                dtT.Columns.Add("SentDate", typeof(DateTime));
                dtT.Columns.Add("Count", typeof(int));
                dtT.Columns.Add("NoOfDays", typeof(int));
                dtT.Columns.Add("Sel", typeof(bool));

                sSql = " SELECT A.PBillId,A.PBNo BillNo,A.PBDate,A.CostCentreId,E.FlatId,A.PaySchId,A.LeadId, " +
                         " E.FlatNo,F.Description,D.LeadName BuyerName,GETDATE() SentDate,Count=IsNull((Select Count(PBillId) From dbo.DLSentDet X Where X.PBillId=A.PBillId Group By PBillId),0)," +
                         " Cast(0 as bit)Sel FROM ProgressBillRegister A  " +
                         " Inner Join BuyerDetail C on A.FlatId=C.FlatId Inner Join LeadRegister D on C.LeadId=D.LeadId  " +
                         " Inner Join FlatDetails E on A.FlatId=E.FlatId  " +
                         " Inner Join PaymentScheduleFlat F On A.PaySchId=F.PaymentSchId   " +
                         " Where A.CostCentreId=" + argCCId + " And C.Status='S' And A.NetAmount<>A.PaidAmount order by LeadName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();


                sSql = " SELECT Distinct 0 PBillId,0 CostCentreId,0 FlatId,A.PaySchId,A.LeadId,E.FlatNo,0 NoOfDays,D.LeadName BuyerName,Cast(0 as bit)Sel FROM ProgressBillRegister A  " +
                         " Inner Join BuyerDetail C on A.FlatId=C.FlatId Inner Join LeadRegister D on C.LeadId=D.LeadId  " +
                         " Inner Join FlatDetails E on A.FlatId=E.FlatId  " +
                         " Inner Join PaymentScheduleFlat F On A.PaySchId=F.PaymentSchId   " +
                         " Where A.CostCentreId=" + argCCId + " And C.Status='S' And A.NetAmount<>A.PaidAmount order by LeadName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtB = new DataTable();
                sda.Fill(dtB);
                sda.Dispose();

                int iNoofDays = 0;
                DataRow dr;
                foreach (DataRow drow in dt.Rows)
                {
                    dr = dtT.NewRow();

                    sSql = " Select Distinct NoOfDays=Datediff(d,DateAdd(d,0,BD.FinaliseDate),GETDATE()) From PaymentScheduleFlat A " +
                           " Inner Join StageDetails B On A.CostCentreId=B.CostCentreId Inner Join dbo.BuyerDetail BD On BD.FlatId=A.FlatId " +
                           " where B.StageId In(Select Case  When C.SchType='D' Then C.SchDescId " +
                           " When C.SchType='S' Then C.StageId else C.OtherCostId End StageId " +
                           " From PaymentScheduleFlat C Where C.PaymentSchId=" + drow["PaySchId"] + ") And A.PaymentSchId=" + drow["PaySchId"] + "";
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    DataTable dt1 = new DataTable();
                    sda.Fill(dt1);
                    sda.Dispose();
                    if (dt1.Rows.Count > 0) { iNoofDays = Convert.ToInt32(dt1.Rows[0]["NoOfDays"]); }

                    dr["PBillId"] = Convert.ToInt32(drow["PBillId"]);
                    dr["BillNo"] = drow["BillNo"].ToString();
                    dr["PBDate"] = Convert.ToDateTime(drow["PBDate"]).ToString("dd-MMM-yyyy");
                    dr["CostCentreId"] = Convert.ToInt32(drow["CostCentreId"]);
                    dr["FlatId"] = Convert.ToInt32(drow["FlatId"]); ;
                    dr["PaySchId"] = Convert.ToInt32(drow["PaySchId"]);
                    dr["LeadId"] = Convert.ToInt32(drow["LeadId"]);
                    dr["FlatNo"] = drow["FlatNo"].ToString();
                    dr["Description"] = drow["Description"].ToString();
                    dr["BuyerName"] = drow["BuyerName"].ToString();
                    dr["SentDate"] = Convert.ToDateTime(drow["SentDate"]).ToString("dd-MMM-yyyy");
                    dr["Count"] = Convert.ToInt32(drow["Count"]);
                    dr["NoOfDays"] = iNoofDays;
                    dr["Sel"] = Convert.ToBoolean(drow["Sel"]);

                    dtT.Rows.Add(dr);
                }
                //ds.Tables.Add(dtT);

                DataView dv = new DataView(dtT);
                DataTable dtAge = new DataTable();
                if (argToDays == 0)
                    dv.RowFilter = "NoOfDays>=" + argFromDays + "";
                else dv.RowFilter = "NoOfDays>=" + argFromDays + " And NoOfDays<=" + argToDays + "";
                //dtT = dv.ToTable();
                dtAge = dv.ToTable();

                dsAge.Tables.Add(dtT);
                dsAge.Tables.Add(dtAge);
                dsAge.Tables.Add(dtB);
                //ds.Tables[1].Add(dtAge);
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dsAge;
        }

        public static DataTable GetSentDate(int argPBillId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select B.PBNo,A.SentDate From dbo.DLSentDet A Inner Join ProgressBillRegister B On A.PBillId=B.PBillId Where A.PBillId=" + argPBillId + "";
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

        public static void InsertDLStatus(DataTable argdt,int argAgeId)
        {
            SqlConnection conn = new SqlConnection();
            SqlTransaction tran;
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            tran = conn.BeginTransaction();
            string sSql = "";
            try
            {
                DataView dv = new DataView(argdt);
                dv.RowFilter = "Sel=" + true + "";
                argdt = dv.ToTable();

                for (int i = 0; i < argdt.Rows.Count; i++)
                {
                    sSql = "Insert Into dbo.DLSentDet(PBillId,AgeId,UserId,SentDate)Values(" + argdt.Rows[i]["PBillId"] + "," +
                    " " + argAgeId + "," + BsfGlobal.g_lUserId + ",'" + Convert.ToDateTime(argdt.Rows[i]["SentDate"]).ToString("dd-MMM-yyyy") + "')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Update dbo.ProgressBillRegister Set SentDate='" + Convert.ToDateTime(argdt.Rows[i]["SentDate"]).ToString("dd-MMM-yyyy") + "'" +
                        " Where PBillId=" + argdt.Rows[i]["PBillId"] + "";
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

        public static void InsertDLDate(string argsLeadId, string argBillId,DataTable argdt)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "Truncate Table dbo.RptTemp ";
            SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            BsfGlobal.g_CRMDB.Close();

            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sDate = "";
                int iFlatId = 0;

                //sSql = "Delete From dbo.RptTemp Where LeadId In(" + argsLeadId.TrimEnd(',') + ")";
               

                    sSql = "Select A.FlatId,C.LeadId,D.FinaliseDate AppDate,CompletionDate BuilderDate From FlatCheckList A " +
                            " Inner Join CheckListMaster B On A.CheckListId=B.CheckListId " +
                            " Inner Join dbo.Flatdetails C On C.FlatId=A.FlatId " +
                            " Inner Join dbo.BuyerDetail D On D.FlatId=C.FlatId " +
                            " Where C.LeadId In(" + argsLeadId.TrimEnd(',') + ") And TypeName='F' And CheckListName='Builders Agreement'";
                cmd = new SqlCommand(sSql, conn, tran);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                cmd.Dispose();
                dr.Close();
                dt.Dispose();

                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        iFlatId = Convert.ToInt32(dt.Rows[i]["FlatId"]);
                        string sAppDate = "", sBuilderDate = "";
                        if (dt.Rows[i]["AppDate"].ToString() == "") { sAppDate = "NULL"; } else { sAppDate = string.Format(Convert.ToDateTime(dt.Rows[i]["AppDate"]).ToString("dd-MMM-yyyy")); }
                        if (dt.Rows[i]["BuilderDate"].ToString() == "") { sBuilderDate = "NULL"; } else { sBuilderDate = string.Format(Convert.ToDateTime(dt.Rows[i]["BuilderDate"]).ToString("dd-MMM-yyyy")); }

                        sSql = "Insert Into dbo.RptTemp(FlatId,LeadId ";
                        if (dt.Rows[i]["AppDate"].ToString() != "")
                        {
                        sSql=sSql+ ",AppDate";
                        }
                        if (dt.Rows[i]["BuilderDate"].ToString() != "")
                        {
                        sSql=sSql+ ",BuilderDate";
                        }
                        sSql = sSql + ")Values(" + dt.Rows[i]["FlatId"] + "," + dt.Rows[i]["LeadId"] + "";
                             if (dt.Rows[i]["AppDate"].ToString() != "")
                        {
                            sSql = sSql + ",'" + sAppDate + "'";
                        }
                        if (dt.Rows[i]["BuilderDate"].ToString() != "")
                        {
                            sSql = sSql + ",'" + sBuilderDate + "'";
                        }
                        sSql = sSql + " )";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Select Distinct B.FlatId,B.PBNo,A.SentDate From dbo.DLSentDet A " +
                              " Inner Join ProgressBillRegister B On A.PBillId=B.PBillId " +
                              " Where A.PBillId In(" + argBillId.TrimEnd(',') + ") And B.FlatId=" + iFlatId + " Order By A.SentDate";
                        cmd = new SqlCommand(sSql, conn, tran);
                        dr = cmd.ExecuteReader();
                        DataTable dtD = new DataTable();
                        dtD.Load(dr);
                        cmd.Dispose();
                        dr.Close();
                        dtD.Dispose();

                        sDate = "";
                        for (int x = 0; x < dtD.Rows.Count; x++)
                        {
                            string Date = Convert.ToDateTime(dtD.Rows[x]["SentDate"]).ToString("dd-MMM-yyyy") + ",";
                            sDate = sDate + Date;
                        }

                        sDate = sDate.TrimEnd(',');

                        sSql = "Update dbo.RptTemp Set Remarks='" + sDate + "' Where FlatId=" + iFlatId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                    }

                    /* TO GET MAX RECEIPT DATE */

                    //sSql = "Select MAX(B.ReceiptDate) ReceiptDate from dbo.ReceiptTrans A " +
                    //        " INNER JOIN dbo.ReceiptRegister B ON A.ReceiptId=B.ReceiptId" +
                    //        " Where A.FlatId=" + Convert.ToInt32(CommFun.IsNullCheck(argdt.Rows[0]["FlatId"], CommFun.datatypes.vartypenumeric)) +
                    //        " AND B.LeadId=" + Convert.ToInt32(CommFun.IsNullCheck(argdt.Rows[0]["LeadId"], CommFun.datatypes.vartypenumeric));
                    //        //" GROUP BY A.PaySchId";
                    //cmd = new SqlCommand(sSql, conn, tran);
                    //SqlDataReader dreader = cmd.ExecuteReader();
                    //DataTable dtReceiptDate = new DataTable();
                    //dtReceiptDate.Load(dreader);
                    //dreader.Close();
                    //cmd.Dispose();

                    //DateTime dReceiptDate = DateTime.MinValue;
                    //if (dtReceiptDate.Rows.Count > 0)
                    //{
                    //    dReceiptDate = Convert.ToDateTime(CommFun.IsNullCheck(dtReceiptDate.Rows[0]["ReceiptDate"], CommFun.datatypes.VarTypeDate));
                    //}

                    for (int i = 0; i < argdt.Rows.Count; i++)
                    {
                        int i_LeadId = Convert.ToInt32(CommFun.IsNullCheck(argdt.Rows[i]["LeadId"], CommFun.datatypes.vartypenumeric));
                        int i_FlatId = Convert.ToInt32(CommFun.IsNullCheck(argdt.Rows[i]["FlatId"], CommFun.datatypes.vartypenumeric));
                        int i_PaySchId = Convert.ToInt32(CommFun.IsNullCheck(argdt.Rows[i]["PaySchId"], CommFun.datatypes.vartypenumeric));
                        int i_PBillId = Convert.ToInt32(CommFun.IsNullCheck(argdt.Rows[i]["PBillId"], CommFun.datatypes.vartypenumeric));

                        #region for Interest Calculation

                        sSql = " Select FlatId,PaymentSchId,SortOrder,[Date],[Description],AsOnDate,Receivable,Received,CreditDays,IntPercent,FinaliseDate,[Type] FROM( " +
                               " Select DISTINCT S.FlatId,S.PaymentSchId,S.SortOrder,A.PBDate [Date],A.AsOnDate,S.[Description]," +
                               " A.NetAmount Receivable,0 Received,D.CreditDays,D.IntPercent,E.FinaliseDate,'P' [Type] " +
                                " From dbo.ProgressBillRegister A INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                                " INNER JOIN dbo.ProgressBillMaster M On M.ProgRegId=A.ProgRegId INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId " +
                                " Left JOIN dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId" +
                                " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId " +
                                " Where M.Approve='Y' And A.FlatId=" + i_FlatId + " AND A.PaySchId=" + i_PaySchId + " And S.BillPassed=1 And A.PBDate<='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' " +
                                " AND A.PBDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                                " UNION ALL " +
                                " Select DISTINCT S.FlatId,S.PaymentSchId,S.SortOrder,RR.ReceiptDate [Date],NULL AsOnDate,RR.Narration [Description]," +
                                " 0 Receivable,RT.Amount Received,D.CreditDays,D.IntPercent,E.FinaliseDate,'R' [Type] " +
                                " From dbo.ProgressBillRegister A INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                                " INNER JOIN dbo.ProgressBillMaster M On M.ProgRegId=A.ProgRegId INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId " +
                                " Left JOIN dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId " +
                                " INNER JOIN dbo.ReceiptTrans RT ON RT.PaySchId=S.PaymentSchId " +
                                " INNER JOIN dbo.ReceiptRegister RR ON RR.ReceiptId=RT.ReceiptId And RR.ReceiptDate<='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' " +
                                " Where M.Approve='Y' And A.FlatId=" + i_FlatId + " AND A.PaySchId=" + i_PaySchId + " AND RT.CancelDate IS NULL AND S.BillPassed=1" +
                                " AND A.PBDate<='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' " +
                                " ) X Order By X.SortOrder,X.[Type],X.[Date]";
                        cmd = new SqlCommand(sSql, conn, tran);
                        SqlDataReader dreader = cmd.ExecuteReader();
                        DataTable dtInterest = new DataTable();
                        dtInterest.Load(dreader);
                        dreader.Close();
                        cmd.Dispose();

                        decimal dInterest = 0;
                        DateTime dInterestDate = DateTime.MinValue;
                        for (int k = 0; k < dtInterest.Rows.Count; k++)
                        {
                            string sType = Convert.ToString(dtInterest.Rows[k]["Type"]);
                            if (sType == "P")
                            {
                                DateTime dFinaliseDate = Convert.ToDateTime(CommFun.IsNullCheck(dtInterest.Rows[k]["FinaliseDate"], CommFun.datatypes.VarTypeDate));
                                DateTime dCompletionDate = Convert.ToDateTime(CommFun.IsNullCheck(dtInterest.Rows[k]["Date"], CommFun.datatypes.VarTypeDate));
                                int iCreditDays = Convert.ToInt32(dtInterest.Rows[k]["CreditDays"]);
                                decimal dIntPer = Convert.ToDecimal(dtInterest.Rows[k]["IntPercent"]);

                                dInterestDate = dCompletionDate;

                                DataView dv = new DataView(dtInterest) { RowFilter = String.Format("PaymentSchId={0} AND Type='R'", i_PaySchId) };
                                DataTable dtRec = new DataTable();
                                dtRec = dv.ToTable();

                                decimal dReceivable = Convert.ToDecimal(dtInterest.Rows[k]["Receivable"]);
                                decimal dBalance = dReceivable;
                                DateTime dCalInterestDate = dInterestDate;

                                if (dtRec.Rows.Count == 0)
                                    dInterestDate = dInterestDate.AddDays(iCreditDays);
                                else
                                {
                                    for (int j = 0; j < dtRec.Rows.Count; j++)
                                    {
                                        decimal dReceived = Convert.ToDecimal(dtRec.Rows[j]["Received"]);
                                        DateTime dRecdDate = Convert.ToDateTime(dtRec.Rows[j]["Date"]);
                                        if (dRecdDate < dInterestDate) { dRecdDate = dCompletionDate; }

                                        if (j == 0)
                                        {
                                            dCalInterestDate = dInterestDate;
                                            dCalInterestDate = dCalInterestDate.AddDays(iCreditDays);
                                        }
                                        else
                                        {
                                            dCalInterestDate = dInterestDate;
                                        }

                                        TimeSpan ts = dRecdDate - dCalInterestDate;
                                        int iDays = ts.Days;
                                        if (iDays < 0) { iDays = 0; }

                                        if (dBalance == 0)
                                            dInterest = 0;
                                        else if (iDays == 0)
                                            dInterest = 0;
                                        else if (dIntPer == 0)
                                            dInterest = 0;
                                        else
                                            dInterest = dInterest + decimal.Round((dBalance * dIntPer * iDays) / 36500, 3);

                                        dBalance = dBalance - dReceived;

                                        dInterestDate = dRecdDate;
                                    }
                                }

                                if (dBalance > 0)
                                {
                                    TimeSpan ts = DateTime.Now - dInterestDate;
                                    int iDays = ts.Days;
                                    if (iDays < 0) { iDays = 0; }

                                    if (dBalance == 0)
                                        dInterest = 0;
                                    else if (iDays == 0)
                                        dInterest = 0;
                                    else if (dIntPer == 0)
                                        dInterest = 0;
                                    else
                                        dInterest = dInterest + decimal.Round((dBalance * dIntPer / 36500) * iDays, 3);
                                }
                                dBalance = 0;
                            }
                        }
                        #endregion

                        sSql = "Insert Into dbo.RptTemp(FlatId,LeadId,PBillId,Description,ReceiptDate,Interest)Values(" + i_FlatId + "," +
                                i_LeadId + "," + i_PBillId + ",'" + argdt.Rows[i]["Description"] + "', @ReceiptDate," + dInterest + ") ";

                        cmd = new SqlCommand(sSql, conn, tran);
                        SqlParameter parameter = new SqlParameter() { ParameterName = "@ReceiptDate", DbType = DbType.DateTime };
                        if (dInterestDate == DateTime.MinValue)
                            parameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            parameter.Value = dInterestDate;
                        cmd.Parameters.Add(parameter);
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
            }

        }

        public static decimal GetSchPer(int argFlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            decimal m_dSchPer = 0;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select ISNULL(SUM(SchPercent),0) SchPercent From dbo.PaymentScheduleFlat Where FlatId=" + argFlatId + " And StageDetId<>0";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0) { m_dSchPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["SchPercent"])); }
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
            return m_dSchPer;
        }

        public static DataTable GetDNPrint(int argFlatId,int argPBillId,int argProgRegId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select C.PaymentSchId,A.FlatId,A.FlatNo,A.Area,A.Rate,A.BaseAmt,A.LandRate,A.USLand,A.USLandAmt,FT.TypeName, "+
                        " C.Description StageName,D.LeadName,G.CompanyName,E.CostCentreName,FC.CityName CCCity, "+
                        " J.BankName,I.Branch,I.IFSCCode,I.Address1 BranchAddress1,I.Address2 BranchAddress2,I.City BankCity,H.LoanAccNo, " +
                        " G.Address1 CompAddress1,G.Address2 CompAddress2,CM.CityName CompCity,G.Pincode CompPincode,G.STNo CompSTNo,G.PANNo CompPANNo,B.PBNo,B.PBDate, " +
                        " LA.Address1,LA.Address2,LA.City,LA.Pincode,D.Mobile,H.FinaliseDate, " +
                        " TotNetAmount=(Select Sum(NetAmount) From dbo.ProgressBillRegister Where ProgRegId=" + argProgRegId + "), " +
                        " BillNetAmount=(Select Sum(NetAmount) From dbo.ProgressBillRegister Where PBillId=" + argPBillId + "), " +
                        " BillAmount=(Select Sum(BillAmount) From dbo.ProgressBillRegister Where PBillId=" + argPBillId + ")-C.Advance, " +
                        " TotPaidAmount =(Select Sum(PaidAmount) From dbo.ProgressBillRegister Where ProgRegId=" + argProgRegId + "), " +
                        " TotPercent=(Select ISNULL(SUM(SchPercent),0) SchPercent From dbo.PaymentScheduleFlat Where FlatId=" + argFlatId + " And StageDetId<>0), " +
                        " A.NetAmt+A.QualifierAmt FlatCost,Sum(K.Amount)Amount,SUM(K.NetAmount-K.Amount)Tax,SUM(K.NetAmount-C.Advance)NetAmount,C.PaidAmount, " +
                        " CF=IsNull((Select Amount From dbo.FlatOtherCost A " +
                        " Inner Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId Where A.FlatId=" + argFlatId + " And OCTypeId=8),0), " +
                        " MC=IsNull((Select Amount From dbo.FlatOtherCost A " +
                        " Inner Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId Where A.FlatId=" + argFlatId + " And OCTypeId=3),0), " +
                        " CP=IsNull((Select SUM(Amount) From dbo.FlatOtherCost A " +
                        " Inner Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId Where A.FlatId=" + argFlatId + " And OCTypeId=9),0), " +
                        " LC=IsNull((Select Amount From dbo.FlatOtherCost A " +
                        " Inner Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId Where A.FlatId=" + argFlatId + " And OCTypeId=7),0) , " +
                        " ST=(Select Sum(A.Amount)Amount From dbo.FlatReceiptQualifier A " +
                        " Inner Join dbo.FlatReceiptType B On A.SchId=B.SchId  " +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On A.QualifierId=C.QualifierId " +
                        " Where FlatId=" + argFlatId + " AND C.QualType='B' AND C.QualId=1), " +
                        " VT=(Select Sum(A.Amount)Amount From dbo.FlatReceiptQualifier A " +
                        " Inner Join dbo.FlatReceiptType B On A.SchId=B.SchId  " +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On A.QualifierId=C.QualifierId " +
                        " Where FlatId=" + argFlatId + " AND C.QualType='B'AND C.QualId=3) " +
                        " ,ReceiptNo=(Select Top 1 ReceiptNo From dbo.ReceiptRegister A Inner Join dbo.ReceiptTrans B On A.ReceiptId=B.ReceiptId " +
                        " Where B.FlatId=" + argFlatId + " And BillRegId=" + argPBillId + "),ReceiptDate=(Select Top 1 ReceiptDate From dbo.ReceiptRegister A " +
                        " Inner Join dbo.ReceiptTrans B On A.ReceiptId=B.ReceiptId Where B.FlatId=" + argFlatId + " And BillRegId=" + argPBillId + "), " +
                        " H.AllotmentNo,H.CCAllotNo,H.COAllotNo"+
                        " From dbo.FlatDetails A Inner Join dbo.FlatType FT On FT.FlatTypeId=A.FlatTypeId Inner Join dbo.ProgressBillRegister B On A.FlatId=B.FlatId " +
                        " Inner Join dbo.PaymentScheduleFlat C On C.FlatId=B.FlatId And B.PaySchId=C.PaymentSchId " +
                        " Inner Join dbo.LeadRegister D On D.LeadId=A.LeadId " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre E On E.CostCentreId=A.CostCentreId " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CostCentre F On F.CostCentreId=E.FACostCentreId " +
                        " LEFT Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CityMaster FC On F.CityId=FC.CityId " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CompanyMaster G On G.CompanyId=F.CompanyId " +
                        " Inner Join dbo.BuyerDetail H On H.FlatId=A.FlatId And H.LeadId=A.LeadId And H.Status='S' " +
                        " Left Join dbo.BankDetails I On I.BranchId=H.BranchId Left Join dbo.BankMaster J On J.BankId=I.BankId " +
                        " Inner Join dbo.FlatReceiptType K On K.PaymentSchId=C.PaymentSchId And K.SchType<>'A' " +
                        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CityMaster CM On CM.CityId=G.CityId Inner Join dbo.LeadCommAddressInfo LA On LA.LeadId=A.LeadId " +
                        " Where B.PBillId=" + argPBillId + " And A.FlatId=" + argFlatId + " " +
                        " Group By C.PaymentSchId,A.FlatId,A.FlatNo,A.Area,A.Rate,A.BaseAmt,A.LandRate,A.USLand,A.USLandAmt,FT.TypeName,C.PaidAmount,"+
                        "C.Description,D.LeadName,G.CompanyName,E.CostCentreName,FC.CityName,J.BankName,I.Branch,I.IFSCCode,I.Address1,I.Address2,I.City,H.LoanAccNo,G.Address1,G.Address2, " +
                        " CM.CityName,G.Pincode,G.STNo,G.PANNo,B.PBNo,B.PBDate,LA.Address1,LA.Address2,LA.City,LA.Pincode,D.Mobile,H.FinaliseDate,A.NetAmt+A.QualifierAmt,C.Advance,H.AllotmentNo,H.CCAllotNo,H.COAllotNo";
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

        //public static DataTable GetDNPaymentSchPrint(int argFlatId, int argPBillId, int argProgRegId)
        //{
        //    DataTable dt = null;
        //    SqlDataAdapter sda;
        //    string sSql = "";
        //    BsfGlobal.OpenCRMDB();
        //    try
        //    {
        //        DataTable dtT = new DataTable();
        //        dtT.Columns.Add("PaymentSchId", typeof(int));
        //        dtT.Columns.Add("Description", typeof(string));
        //        dtT.Columns.Add("SchPercent", typeof(decimal));
        //        dtT.Columns.Add("SchDate", typeof(DateTime));

        //        sSql = "Select Distinct A.ReceiptTypeId,0 OtherCostId,ReceiptTypeName From dbo.FlatReceiptType A " +
        //                " Left Join dbo.ReceiptType B On A.ReceiptTypeId=B.ReceiptTypeId " +
        //                " Where A.FlatId=" + argFlatId + " And A.SchType='R' " +
        //                " UNION ALL " +
        //                " Select Distinct A.ReceiptTypeId,C.OtherCostId,OtherCostName From dbo.FlatReceiptType A " +
        //                " Left Join dbo.OtherCostMaster C On A.OtherCostId=C.OtherCostId " +
        //                " Where A.FlatId=" + argFlatId + " And A.OtherCostId<>0 " +
        //                " UNION ALL " +
        //                " Select 0 ReceiptTypeId,D.QualifierId OtherCostId,(QualifierName + ' @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ' AS Name " +
        //                " From dbo.ReceiptShTrans A  Inner Join dbo.ReceiptQualifier B On A.ReceiptId=B.ReceiptId " +
        //                " And A.PaymentSchId=B.PaymentSchId And A.OtherCostId=B.OtherCostId And A.ReceiptTypeId=B.ReceiptTypeId " +
        //                " Inner Join dbo.ReceiptRegister C On C.ReceiptId=A.ReceiptId  Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp D On D.QualifierId=B.QualifierId " +
        //                " And QualType='B' Where A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1 " +
        //                " Group By D.QualifierId,(QualifierName + ' @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % '";
        //        sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
        //        dt = new DataTable();
        //        sda.Fill(dt);

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            dtT.Columns.Add(dt.Rows[i]["ReceiptTypeName"].ToString(), typeof(decimal));
        //        }

        //        dtT.Columns.Add("TotalCost", typeof(decimal));
        //        dtT.Columns.Add("InstAmount", typeof(decimal));
        //        dtT.Columns.Add("ChNo", typeof(string));
        //        dtT.Columns.Add("Date", typeof(DateTime));
        //        dtT.Columns.Add("ChAmount", typeof(decimal));
        //        dtT.Columns.Add("Balance", typeof(decimal));

        //        sda.Dispose();

        //        sSql = "Select PaymentSchId,Description,SchPercent,SchDate From dbo.PaymentScheduleFlat " +
        //                 " Where FlatId=" + argFlatId + " And SortOrder<>0";
        //        sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
        //        dt = new DataTable();
        //        sda.Fill(dt);
        //        dt.Dispose();
        //        dtT = dt;

        //        sSql = "Select Distinct A.ReceiptTypeId,0 OtherCostId,ReceiptTypeName From dbo.FlatReceiptType A " +
        //                " Left Join dbo.ReceiptType B On A.ReceiptTypeId=B.ReceiptTypeId " +
        //                " Where A.FlatId=" + argFlatId + " And A.SchType='R' " +
        //                " UNION ALL " +
        //                " Select Distinct A.ReceiptTypeId,C.OtherCostId,OtherCostName From dbo.FlatReceiptType A " +
        //                " Left Join dbo.OtherCostMaster C On A.OtherCostId=C.OtherCostId " +
        //                " Where A.FlatId=" + argFlatId + " And A.OtherCostId<>0 " +
        //                " UNION ALL " +
        //                " Select 0 ReceiptTypeId,D.QualifierId OtherCostId,(QualifierName + ' @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ' AS Name " +
        //                " From dbo.ReceiptShTrans A  Inner Join dbo.ReceiptQualifier B On A.ReceiptId=B.ReceiptId " +
        //                " And A.PaymentSchId=B.PaymentSchId And A.OtherCostId=B.OtherCostId And A.ReceiptTypeId=B.ReceiptTypeId " +
        //                " Inner Join dbo.ReceiptRegister C On C.ReceiptId=A.ReceiptId  Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp D On D.QualifierId=B.QualifierId " +
        //                " And QualType='B' Where A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1 " +
        //                " Group By D.QualifierId,(QualifierName + ' @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % '";
        //        sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
        //        dt = new DataTable();
        //        sda.Fill(dt);
        //        dt.Dispose();

        //    }
        //    catch (SqlException ex)
        //    {
        //        BsfGlobal.CustomException(ex.Message, ex.StackTrace);
        //    }
        //    finally
        //    {
        //        BsfGlobal.g_CRMDB.Close();
        //    }
        //    return dt;
        //}

        public static DataTable GetDNPaymentSchPrint(int argFlatId, int argPBillId, int argProgRegId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            DataRow[] drT;
            DataTable dtT = new DataTable();
            try
            {
                sSql = "Select FlatId,PaymentSchId,Description,SchPercent,SchDate From dbo.PaymentScheduleFlat " +
                         " Where FlatId=" + argFlatId + " And SortOrder<>0";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
                dtT = dt;

                sSql = "Select Distinct A.ReceiptTypeId,0 OtherCostId,ReceiptTypeName From dbo.FlatReceiptType A " +
                        " Left Join dbo.ReceiptType B On A.ReceiptTypeId=B.ReceiptTypeId " +
                        " Where A.FlatId=" + argFlatId + " And A.SchType='R' " +
                        " UNION ALL " +
                        " Select Distinct A.ReceiptTypeId,C.OtherCostId,OtherCostName From dbo.FlatReceiptType A " +
                        " Left Join dbo.OtherCostMaster C On A.OtherCostId=C.OtherCostId " +
                        " Where A.FlatId=" + argFlatId + " And A.OtherCostId<>0 " +
                        " UNION ALL " +
                        " Select 0 ReceiptTypeId,A.QualifierId OtherCostId, D.QualifierName Name from FlatReceiptQualifier A" +
                        " Inner Join ["+ BsfGlobal.g_sRateAnalDBName +"].dbo.Qualifier_Temp D On D.QualifierId=A.QualifierId  And QualType='B'" +
                        " INNER JOIN FlatReceiptType C ON A.SchId=C.SchId " +
                        " Where C.FlatId=" + argFlatId + " GROUP BY A.QualifierId, D.QualifierName";

                        //" Select 0 ReceiptTypeId,D.QualifierId OtherCostId,QualifierName Name " +
                        //" From dbo.ReceiptShTrans A  Inner Join dbo.ReceiptQualifier B On A.ReceiptId=B.ReceiptId " +
                        //" And A.PaymentSchId=B.PaymentSchId And A.OtherCostId=B.OtherCostId And A.ReceiptTypeId=B.ReceiptTypeId " +
                        //" Inner Join dbo.ReceiptRegister C On C.ReceiptId=A.ReceiptId  Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp D On D.QualifierId=B.QualifierId " +
                        //" And QualType='B' Where A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1 " +
                        //" Group By D.QualifierId,QualifierName";

                        //" Select 0 ReceiptTypeId,D.QualifierId OtherCostId,(QualifierName + ' @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ' AS Name " +
                        //" From dbo.ReceiptShTrans A  Inner Join dbo.ReceiptQualifier B On A.ReceiptId=B.ReceiptId " +
                        //" And A.PaymentSchId=B.PaymentSchId And A.OtherCostId=B.OtherCostId And A.ReceiptTypeId=B.ReceiptTypeId " +
                        //" Inner Join dbo.ReceiptRegister C On C.ReceiptId=A.ReceiptId  Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp D On D.QualifierId=B.QualifierId " +
                        //" And QualType='B' Where A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1 " +
                        //" Group By D.QualifierId,(QualifierName + ' @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % '";
                
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);

                sSql = "Select Distinct A.PaymentSchId,A.ReceiptTypeId,0 OtherCostId,ReceiptTypeName,SUM(A.Amount)Amount From dbo.FlatReceiptType A  " +
                        " Left Join dbo.ReceiptType B On A.ReceiptTypeId=B.ReceiptTypeId  Where A.FlatId=" + argFlatId + " And A.SchType='R' " +
                        " Group By A.PaymentSchId,A.ReceiptTypeId,ReceiptTypeName  " +
                        " UNION ALL  " +
                        " Select Distinct A.PaymentSchId,A.ReceiptTypeId,C.OtherCostId,OtherCostName,SUM(A.Amount)Amount From dbo.FlatReceiptType A  " +
                        " Left Join dbo.OtherCostMaster C On A.OtherCostId=C.OtherCostId  Where A.FlatId=" + argFlatId + " And A.OtherCostId<>0  " +
                        " Group By A.PaymentSchId,A.ReceiptTypeId,C.OtherCostId,OtherCostName " +
                        " UNION ALL  " +
                        "Select C.PaymentSchId,0 ReceiptTypeId,B.QualifierId OtherCostId,B.QualifierName Name, SUM(A.Amount) Amount from FlatReceiptQualifier A" +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp B On A.QualifierId=B.QualifierId  And QualType='B'" +
                        " INNER JOIN FlatReceiptType C ON A.SchId=C.SchId " +
                        " Where C.FlatId=" + argFlatId + " AND C.ReceiptTypeId<>1 GROUP BY C.PaymentSchId,B.QualifierId, B.QualifierName";

                        //" Select A.PaymentSchId,0 ReceiptTypeId,D.QualifierId OtherCostId,QualifierName Name,SUM(B.Amount)Amount  "+
                        //" From dbo.ReceiptShTrans A  Inner Join dbo.ReceiptQualifier B On A.ReceiptId=B.ReceiptId  And A.PaymentSchId=B.PaymentSchId "+
                        //" And A.OtherCostId=B.OtherCostId And A.ReceiptTypeId=B.ReceiptTypeId  Inner Join dbo.ReceiptRegister C On C.ReceiptId=A.ReceiptId  "+
                        //" Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp D On D.QualifierId=B.QualifierId  And QualType='B' Where A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1  " +
                        //" Group By A.PaymentSchId,D.QualifierId,QualifierName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtA = new DataTable();
                sda.Fill(dtA);
                dtA.Dispose();

                string sHeader = ""; int iPaymentSchId = 0; decimal dAmt = 0;
                if (dtA.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sHeader = dt.Rows[i]["ReceiptTypeName"].ToString();

                        DataColumn col1 = new DataColumn(sHeader) { DataType = typeof(decimal), DefaultValue = 0 };
                        dtT.Columns.Add(col1);

                        DataTable dtRecv = new DataTable();
                        DataView dv = new DataView(dtA) { RowFilter = String.Format("ReceiptTypeName='{0}' ", sHeader) };
                        dtRecv = dv.ToTable();

                        for (int j = 0; j < dtRecv.Rows.Count; j++)
                        {
                            iPaymentSchId = Convert.ToInt32(dtRecv.Rows[j]["PaymentSchId"]);
                            dAmt = Convert.ToDecimal(dtRecv.Rows[j]["Amount"]);

                            drT = dtT.Select(String.Format("PaymentSchId={0}", iPaymentSchId));

                            if (drT.Length > 0)
                            {
                                drT[0][sHeader] = dAmt;
                            }
                        }

                        dtRecv.Dispose();
                        dv.Dispose();
                    }
                }
                dt.Dispose();

                dtT.Columns.Add("TotalCost", typeof(decimal));
                dtT.Columns.Add("InstAmount", typeof(decimal));

                sSql = "Select PaymentSchId,NetAmount,PaidAmount From dbo.PaymentScheduleFlat " +
                        " Where FlatId=" + argFlatId + " And SchType<>'A' ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtC = new DataTable();
                sda.Fill(dtC);

                if (dtC.Rows.Count > 0)
                {
                    for (int j = 0; j < dtC.Rows.Count; j++)
                    {
                        iPaymentSchId = Convert.ToInt32(dtC.Rows[j]["PaymentSchId"]);
                        decimal dTCost = Convert.ToDecimal(dtC.Rows[j]["NetAmount"]);
                        decimal dInsAmt = Convert.ToDecimal(dtC.Rows[j]["PaidAmount"]);

                        drT = dtT.Select(String.Format("PaymentSchId={0}", iPaymentSchId));

                        if (drT.Length > 0)
                        {
                            drT[0]["TotalCost"] = dTCost;
                            drT[0]["InstAmount"] = dInsAmt;
                        }
                    }
                }
                dtC.Dispose();


                dtT.Columns.Add("ChNo", typeof(string));
                dtT.Columns.Add("Date", typeof(DateTime));
                dtT.Columns.Add("ChAmount", typeof(decimal));
                dtT.Columns.Add("Balance", typeof(decimal));

                sSql = "Select B.FlatId,B.PaySchId PaymentSchId,IsNull(ChequeNo,'')ChequeNo,IsNull(ChequeDate,'')ChequeDate,IsNull(B.Amount,0)Amount,0 Balance From dbo.ReceiptRegister A " +
                        " Inner Join dbo.ReceiptTrans B On A.ReceiptId=B.ReceiptId " +
                        " Where B.FlatId=" + argFlatId + " ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtCA = new DataTable();
                sda.Fill(dtCA);
              
                int iPaySchId = 0; 

                DataTable dtRetn = new DataTable();
                dtRetn = dtT.GetChanges().Copy();
                DataRow dr = dtT.NewRow();
                if (dtCA.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRetn.Rows.Count; i++)
                    {
                        iPaySchId = Convert.ToInt32(dtRetn.Rows[i]["PaymentSchId"]);
                       
                        drT = dtCA.Select(String.Format("PaymentSchId={0}", iPaySchId));

                        if (drT.Length > 0)
                        {
                            foreach (DataRow drv in drT)
                            {
                                dr = dtT.NewRow();

                                dr["FlatId"] = CommFun.IsNullCheck(drv["FlatId"], CommFun.datatypes.vartypenumeric);
                                dr["PaymentSchId"] = CommFun.IsNullCheck(drv["PaymentSchId"], CommFun.datatypes.vartypenumeric);
                                dr["ChNo"] =CommFun.IsNullCheck(drv["ChequeNo"], CommFun.datatypes.vartypestring).ToString();
                                dr["Date"] = Convert.ToDateTime(drv["ChequeDate"]).ToString("dd-MMM-yyyy");
                                dr["ChAmount"] = Convert.ToDecimal(CommFun.IsNullCheck(drv["Amount"], CommFun.datatypes.vartypenumeric));
                                dr["Balance"] = Convert.ToDecimal(CommFun.IsNullCheck(drv["Balance"], CommFun.datatypes.vartypenumeric));

                                dtT.Rows.InsertAt(dr, iPaySchId);
                            }
                        }
                    }
                }
                dtCA.Dispose();

            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtT;
        }

        #endregion

        #region Qualifier Setting

        public static DataTable QualifierSelect(int argQId, string argQType, DateTime argDate)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt = new DataTable();
            DataTable dtT = new DataTable();
            string sSql = "";
            int iPeriodId = 0;

            try
            {
                sSql = "Select PeriodId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriod Where QualType='" + argQType + "' and " +
                        "((TDate is not null and Fdate <= '" + argDate.ToString("dd MMM yyyy") + "' and TDate >= '" + argDate.ToString("dd MMM yyyy") + "' ) or " +
                        "(TDate is null  and FDate <= '" + argDate.ToString("dd MMM yyyy") + "'))";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dtT);
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
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    sda.Fill(dt);
                }
                else
                {
                    sSql = "Select Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,HEDCess,Net From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp " +
                                "Where QualifierId = " + argQId;
                }
                sda.Dispose();
                //BsfGlobal.g_CRMDB.Close();
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable GetSTSettings(string argWOType, DateTime argDate)
        {
            SqlDataAdapter sda;
            DataTable dt = new DataTable();
            string sSql = "";
            int iPeriodId = 0;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select PeriodId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriod Where QualType='S' and " +
                       "((TDate is not null and Fdate <= '" + argDate.ToString("dd MMM yyyy") + "' and TDate >= '" + argDate.ToString("dd MMM yyyy") + "' ) or " +
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
                    sSql = "Select * from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.STSetting  " +
                        "Where WorkType='" + argWOType + "' and PeriodId= " + iPeriodId;
                }
                else
                {
                    sSql = "Select * from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.STSetting  " +
                       "Where WorkType='" + argWOType + "' and PeriodId= " + iPeriodId;
                }

                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dt);
                sda.Dispose();
                //BsfGlobal.g_CRMDB.Close();
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        #endregion

        internal static DataTable GetDemandLetterFirstPrint(int argiCCId, string argsBillId, string argsLeadId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtMaster = null;
            try
            {
                string sSql = "Select A.PBillId, A.PBDate BillDate, A.CostCentreId, A.FlatId, A.LeadId, A.PaySchId, ISNULL(L.CompanyName, '') CompanyName, " +
                              " ISNULL(I.CostCentreName, '') CostCentreName, ISNULL(B.FlatNo, '') FlatNo, ISNULL(FB.BlockName, '') Block, " +
                              " ISNULL(FL.LevelName, '') Level, ISNULL(D.LeadName, '') LeadName, ISNULL(E.Address1, '') Address1, " +
                              " ISNULL(E.Address2, '') Address2, ISNULL(K.CityName, '') CityName, ISNULL(E.PinCode, '') PinCode, " +
                              " ISNULL(C.[Description], '') [Description], ISNULL(C.Amount, 0) Gross, ISNULL(C.Advance,0) Advance, ISNULL(C.NetAmount, 0) NetAmount, "+
                              " ISNULL(A.NetAmount,0) BillNetAmount, SUM(DISTINCT ISNULL(F.Amount,0)) ServiceTax, "+
                              " ISNULL(SUM(DISTINCT ISNULL(F.Amount,0))-SUM(DISTINCT ISNULL(N.Amount,0)),0) BalanceST, " +
                              " ISNULL(C.PaidAmount, 0) PaidAmount, '' DueDate, A.AsOnDate, ISNULL(B.IntPercent,0) LateInterest, " +
                              " ISNULL(B.CreditDays,0) CreditDays, 0.000 Interest,C.SortOrder from dbo.ProgressBillRegister A " +
                              " INNER JOIN dbo.FlatDetails B ON A.FlatId=B.FlatId AND A.LeadId=B.LeadId AND A.CostCentreId=B.CostCentreId " +
                              " INNER JOIN dbo.BlockMaster FB ON B.BlockId=FB.BlockId " +
                              " INNER JOIN dbo.LevelMaster FL ON B.LevelId=FL.LevelId " +
                              " INNER JOIN dbo.PaymentScheduleFlat C ON A.FlatId=C.FlatId AND A.PaySchId=C.PaymentSchId AND A.CostCentreId=C.CostCentreId " +
                              " INNER JOIN dbo.LeadRegister D ON B.LeadId=D.LeadId " +
                              " LEFT JOIN dbo.LeadCommAddressInfo E ON D.LeadId=E.LeadId " +
                              " LEFT JOIN dbo.PBReceiptTypeQualifier F ON A.PBillId=F.PBillId " +
                              " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp G ON F.QualifierId=G.QualifierId " +
                              " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualifierType H ON G.QualTypeId=H.QualTypeId AND H.QualTypeId=2 " +
                              " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre I ON B.CostCentreId=I.CostCentreId " +
                              " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CostCentre J ON I.FACostCentreId=J.CostCentreId " +
                              " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CityMaster K ON E.CityId=K.CityId " +
                              " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CompanyMaster L ON J.CompanyId=L.CompanyId " +
                              " LEFT JOIN dbo.ReceiptTrans M ON A.FlatId=M.FlatId AND A.PaySchId=M.PaySchId "+
                              " LEFT JOIN dbo.ReceiptQualifier N ON M.ReceiptId=N.ReceiptId AND M.PaySchId=N.PaymentSchId AND M.FlatId=N.FlatId " +
                              " Where A.PBillId IN(" + argsBillId + ") AND A.LeadId IN(" + argsLeadId + ") AND A.CostCentreId=" + argiCCId +
                              " And A.NetAmount<>C.PaidAmount AND A.NetAmount>C.PaidAmount " +
                              " GROUP BY C.SortOrder, A.PBDate, A.LeadId, A.PBillId, A.CostCentreId, A.FlatId, A.PaySchId, L.CompanyName, I.CostCentreName, " +
                              " B.FlatNo, D.LeadName, E.Address1, E.Address2,K.CityName, E.PinCode, FB.BlockName, FL.LevelName, C.[Description], " +
                              " C.Amount, C.Advance, C.NetAmount, A.NetAmount, C.Advance, C.PaidAmount, A.AsOnDate, B.IntPercent, B.CreditDays" +
                              " ORDER BY A.LeadId, C.SortOrder";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();

                #region for Interest Calculation

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int i_LeadId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["LeadId"], CommFun.datatypes.vartypenumeric));
                    int i_FlatId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["FlatId"], CommFun.datatypes.vartypenumeric));
                    int i_PaySchId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["PaySchId"], CommFun.datatypes.vartypenumeric));
                    int i_PBillId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["PBillId"], CommFun.datatypes.vartypenumeric));

                    sSql = " Select FlatId,PaymentSchId,SortOrder,[Date],[Description],AsOnDate,Receivable,Received,CreditDays,IntPercent,FinaliseDate,[Type] FROM( " +
                           " Select DISTINCT S.FlatId,S.PaymentSchId,S.SortOrder,A.PBDate [Date],A.AsOnDate,S.[Description]," +
                           " A.NetAmount Receivable,0 Received,D.CreditDays,D.IntPercent,E.FinaliseDate,'P' [Type] " +
                            " From dbo.ProgressBillRegister A INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                            " INNER JOIN dbo.ProgressBillMaster M On M.ProgRegId=A.ProgRegId INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId " +
                            " Left JOIN dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId" +
                            " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId " +
                            " Where M.Approve='Y' And A.FlatId=" + i_FlatId + " AND A.PaySchId=" + i_PaySchId + " And S.BillPassed=1 And A.PBDate<='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' " +
                            " AND A.PBDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                            " UNION ALL " +
                            " Select DISTINCT S.FlatId,S.PaymentSchId,S.SortOrder,RR.ReceiptDate [Date],NULL AsOnDate,RR.Narration [Description]," +
                            " 0 Receivable,RT.Amount Received,D.CreditDays,D.IntPercent,E.FinaliseDate,'R' [Type] " +
                            " From dbo.ProgressBillRegister A INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                            " INNER JOIN dbo.ProgressBillMaster M On M.ProgRegId=A.ProgRegId INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId " +
                            " Left JOIN dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId " +
                            " INNER JOIN dbo.ReceiptTrans RT ON RT.PaySchId=S.PaymentSchId " +
                            " INNER JOIN dbo.ReceiptRegister RR ON RR.ReceiptId=RT.ReceiptId And RR.ReceiptDate<='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' " +
                            " Where M.Approve='Y' And A.FlatId=" + i_FlatId + " AND A.PaySchId=" + i_PaySchId + " AND RT.CancelDate IS NULL AND S.BillPassed=1" +
                            " AND A.PBDate<='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' " +
                            " ) X Order By X.SortOrder,X.[Type],X.[Date]";
                    SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    SqlDataReader dreader = cmd.ExecuteReader();
                    DataTable dtInterest = new DataTable();
                    dtInterest.Load(dreader);
                    dreader.Close();
                    cmd.Dispose();

                    decimal dInterest = 0;
                    DateTime dInterestDate = DateTime.MinValue;
                    for (int k = 0; k < dtInterest.Rows.Count; k++)
                    {
                        string sType = Convert.ToString(dtInterest.Rows[k]["Type"]);
                        if (sType == "P")
                        {
                            DateTime dFinaliseDate = Convert.ToDateTime(CommFun.IsNullCheck(dtInterest.Rows[k]["FinaliseDate"], CommFun.datatypes.VarTypeDate));
                            DateTime dCompletionDate = Convert.ToDateTime(CommFun.IsNullCheck(dtInterest.Rows[k]["Date"], CommFun.datatypes.VarTypeDate));
                            int iCreditDays = Convert.ToInt32(dtInterest.Rows[k]["CreditDays"]);
                            decimal dIntPer = Convert.ToDecimal(dtInterest.Rows[k]["IntPercent"]);

                            dInterestDate = dCompletionDate;

                            DataView dv = new DataView(dtInterest) { RowFilter = String.Format("PaymentSchId={0} AND Type='R'", i_PaySchId) };
                            DataTable dtRec = new DataTable();
                            dtRec = dv.ToTable();

                            decimal dReceivable = Convert.ToDecimal(dtInterest.Rows[k]["Receivable"]);
                            decimal dBalance = dReceivable;
                            DateTime dCalInterestDate = dInterestDate;

                            if (dtRec.Rows.Count == 0)
                                dInterestDate = dInterestDate.AddDays(iCreditDays);
                            else
                            {
                                for (int j = 0; j < dtRec.Rows.Count; j++)
                                {
                                    decimal dReceived = Convert.ToDecimal(dtRec.Rows[j]["Received"]);
                                    DateTime dRecdDate = Convert.ToDateTime(dtRec.Rows[j]["Date"]);
                                    if (dRecdDate < dInterestDate) { dRecdDate = dCompletionDate; }

                                    if (j == 0)
                                    {
                                        dCalInterestDate = dInterestDate;
                                        dCalInterestDate = dCalInterestDate.AddDays(iCreditDays);
                                    }
                                    else
                                    {
                                        dCalInterestDate = dInterestDate;
                                    }

                                    TimeSpan ts = dRecdDate - dCalInterestDate;
                                    int iDays = ts.Days;
                                    if (iDays < 0) { iDays = 0; }

                                    if (dBalance == 0)
                                        dInterest = 0;
                                    else if (iDays == 0)
                                        dInterest = 0;
                                    else if (dIntPer == 0)
                                        dInterest = 0;
                                    else
                                        dInterest = dInterest + decimal.Round((dBalance * dIntPer * iDays) / 36500, 3);

                                    dBalance = dBalance - dReceived;

                                    dInterestDate = dRecdDate;
                                }
                            }

                            if (dBalance > 0)
                            {
                                TimeSpan ts = DateTime.Now - dInterestDate;
                                int iDays = ts.Days;
                                if (iDays < 0) { iDays = 0; }

                                if (dBalance == 0)
                                    dInterest = 0;
                                else if (iDays == 0)
                                    dInterest = 0;
                                else if (dIntPer == 0)
                                    dInterest = 0;
                                else
                                    dInterest = dInterest + decimal.Round((dBalance * dIntPer / 36500) * iDays, 3);
                            }
                            dBalance = 0;
                        }
                    }

                    dt.Rows[i]["Interest"] = Convert.ToDecimal(dInterest);
                }

                #endregion

                dtMaster = new DataTable();
                dtMaster = dt.Clone();

                int iOldLeadId = 0;
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    int iLeadId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["LeadId"], CommFun.datatypes.vartypenumeric));
                    if (iOldLeadId != iLeadId)
                    {
                        DataView dview = new DataView(dt) { RowFilter = "LeadId=" + iLeadId + "" };
                        DataTable dtFilter = new DataTable();
                        dtFilter = dview.ToTable();
                        if (dtFilter != null)
                        {
                            for (int j = 0; j <= dtFilter.Rows.Count - 1; j++)
                            {
                                if ((j + 1) == dtFilter.Rows.Count)
                                    dtFilter.Rows[j]["DueDate"] = "Within One Week";
                                else
                                    dtFilter.Rows[j]["DueDate"] = "Immediate";
                            }

                            DataRow[] drow = dtFilter.Select("LeadId=" + iLeadId + "");
                            foreach (DataRow dAddRow in drow)
                            {
                                object[] dCopyRow = dAddRow.ItemArray;
                                dtMaster.Rows.Add(dCopyRow);
                            }
                        }
                    }
                    iOldLeadId = iLeadId;
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtMaster;
        }

        internal static DataTable GetProgressBillAlert(int argiCCId, int argPBillId, int argFlatId, int argLeadId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtMaster = null;
            try
            {
                string sSql = "Select A.PBillId, A.PBDate BillDate, A.CostCentreId, A.FlatId, A.LeadId, A.PaySchId, ISNULL(L.CompanyName, '') CompanyName, " +
                              " ISNULL(I.CostCentreName, '') CostCentreName, ISNULL(B.FlatNo, '') FlatNo, ISNULL(FB.BlockName, '') Block, " +
                              " ISNULL(FL.LevelName, '') Level, ISNULL(D.LeadName, '') LeadName, ISNULL(D.Email, '') Email, ISNULL(E.Address1, '') Address1, " +
                              " ISNULL(E.Address2, '') Address2, ISNULL(K.CityName, '') CityName, ISNULL(E.PinCode, '') PinCode, " +
                              " ISNULL(C.[Description], '') [Description], ISNULL(C.SchPercent, 0) Percentage, " +
                              " ISNULL(C.Amount, 0) Gross, ISNULL(C.Advance,0) Advance, ISNULL(C.NetAmount, 0) NetAmount, " +
                              " ISNULL(A.NetAmount,0) BillNetAmount, SUM(DISTINCT ISNULL(F.Amount,0)) ServiceTax, " +
                              " ISNULL(SUM(DISTINCT ISNULL(F.Amount,0))-SUM(DISTINCT ISNULL(N.Amount,0)),0) BalanceST, " +
                              " ISNULL(C.PaidAmount, 0) PaidAmount, '' DueDate, A.AsOnDate, ISNULL(B.IntPercent,0) LateInterest, " +
                              " ISNULL(B.CreditDays,0) CreditDays, 0.000 Interest,C.SortOrder from dbo.ProgressBillRegister A " +
                              " INNER JOIN dbo.FlatDetails B ON A.FlatId=B.FlatId AND A.LeadId=B.LeadId AND A.CostCentreId=B.CostCentreId " +
                              " INNER JOIN dbo.BlockMaster FB ON B.BlockId=FB.BlockId " +
                              " INNER JOIN dbo.LevelMaster FL ON B.LevelId=FL.LevelId " +
                              " INNER JOIN dbo.PaymentScheduleFlat C ON A.FlatId=C.FlatId AND A.PaySchId=C.PaymentSchId AND A.CostCentreId=C.CostCentreId " +
                              " INNER JOIN dbo.LeadRegister D ON B.LeadId=D.LeadId " +
                              " LEFT JOIN dbo.LeadCommAddressInfo E ON D.LeadId=E.LeadId " +
                              " LEFT JOIN dbo.PBReceiptTypeQualifier F ON A.PBillId=F.PBillId " +
                              " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp G ON F.QualifierId=G.QualifierId " +
                              " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualifierType H ON G.QualTypeId=H.QualTypeId AND H.QualTypeId=2 " +
                              " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre I ON B.CostCentreId=I.CostCentreId " +
                              " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CostCentre J ON I.FACostCentreId=J.CostCentreId " +
                              " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CityMaster K ON E.CityId=K.CityId " +
                              " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CompanyMaster L ON J.CompanyId=L.CompanyId " +
                              " LEFT JOIN dbo.ReceiptTrans M ON A.FlatId=M.FlatId AND A.PaySchId=M.PaySchId " +
                              " LEFT JOIN dbo.ReceiptQualifier N ON M.ReceiptId=N.ReceiptId AND M.PaySchId=N.PaymentSchId AND M.FlatId=N.FlatId " +
                              " Where A.PBillId<=" + argPBillId + " AND A.FlatId=" + argFlatId + " AND A.LeadId=" + argLeadId + " AND A.CostCentreId=" + argiCCId +
                              " And A.NetAmount<>C.PaidAmount AND A.NetAmount>C.PaidAmount " +
                              " GROUP BY C.SortOrder, A.PBDate, A.LeadId, A.PBillId, A.CostCentreId, A.FlatId, A.PaySchId, L.CompanyName, I.CostCentreName, " +
                              " B.FlatNo, D.LeadName, D.Email, E.Address1, E.Address2,K.CityName, E.PinCode, FB.BlockName, FL.LevelName, C.[Description], " +
                              " C.SchPercent, C.Amount, C.Advance, C.NetAmount, A.NetAmount, C.Advance, C.PaidAmount, A.AsOnDate, B.IntPercent, B.CreditDays" +
                              " ORDER BY A.LeadId, C.SortOrder";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();

                #region for Interest Calculation

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int i_LeadId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["LeadId"], CommFun.datatypes.vartypenumeric));
                    int i_FlatId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["FlatId"], CommFun.datatypes.vartypenumeric));
                    int i_PaySchId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["PaySchId"], CommFun.datatypes.vartypenumeric));
                    int i_PBillId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["PBillId"], CommFun.datatypes.vartypenumeric));

                    sSql = " Select FlatId,PaymentSchId,SortOrder,[Date],[Description],AsOnDate,Receivable,Received,CreditDays,IntPercent,FinaliseDate,[Type] FROM( " +
                           " Select DISTINCT S.FlatId,S.PaymentSchId,S.SortOrder,A.PBDate [Date],A.AsOnDate,S.[Description]," +
                           " A.NetAmount Receivable,0 Received,D.CreditDays,D.IntPercent,E.FinaliseDate,'P' [Type] " +
                            " From dbo.ProgressBillRegister A INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                            " INNER JOIN dbo.ProgressBillMaster M On M.ProgRegId=A.ProgRegId INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId " +
                            " Left JOIN dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId" +
                            " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId " +
                            " Where M.Approve='Y' And A.FlatId=" + i_FlatId + " AND A.PaySchId=" + i_PaySchId + " And S.BillPassed=1 And A.PBDate<='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' " +
                            " AND A.PBDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                            " UNION ALL " +
                            " Select DISTINCT S.FlatId,S.PaymentSchId,S.SortOrder,RR.ReceiptDate [Date],NULL AsOnDate,RR.Narration [Description]," +
                            " 0 Receivable,RT.Amount Received,D.CreditDays,D.IntPercent,E.FinaliseDate,'R' [Type] " +
                            " From dbo.ProgressBillRegister A INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                            " INNER JOIN dbo.ProgressBillMaster M On M.ProgRegId=A.ProgRegId INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId " +
                            " Left JOIN dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId " +
                            " INNER JOIN dbo.ReceiptTrans RT ON RT.PaySchId=S.PaymentSchId " +
                            " INNER JOIN dbo.ReceiptRegister RR ON RR.ReceiptId=RT.ReceiptId And RR.ReceiptDate<='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' " +
                            " Where M.Approve='Y' And A.FlatId=" + i_FlatId + " AND A.PaySchId=" + i_PaySchId + " AND RT.CancelDate IS NULL AND S.BillPassed=1" +
                            " AND A.PBDate<='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' " +
                            " ) X Order By X.SortOrder,X.[Type],X.[Date]";
                    SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    SqlDataReader dreader = cmd.ExecuteReader();
                    DataTable dtInterest = new DataTable();
                    dtInterest.Load(dreader);
                    dreader.Close();
                    cmd.Dispose();

                    decimal dInterest = 0;
                    DateTime dInterestDate = DateTime.MinValue;
                    for (int k = 0; k < dtInterest.Rows.Count; k++)
                    {
                        string sType = Convert.ToString(dtInterest.Rows[k]["Type"]);
                        if (sType == "P")
                        {
                            DateTime dFinaliseDate = Convert.ToDateTime(CommFun.IsNullCheck(dtInterest.Rows[k]["FinaliseDate"], CommFun.datatypes.VarTypeDate));
                            DateTime dCompletionDate = Convert.ToDateTime(CommFun.IsNullCheck(dtInterest.Rows[k]["Date"], CommFun.datatypes.VarTypeDate));
                            int iCreditDays = Convert.ToInt32(dtInterest.Rows[k]["CreditDays"]);
                            decimal dIntPer = Convert.ToDecimal(dtInterest.Rows[k]["IntPercent"]);

                            dInterestDate = dCompletionDate;

                            DataView dv = new DataView(dtInterest) { RowFilter = String.Format("PaymentSchId={0} AND Type='R'", i_PaySchId) };
                            DataTable dtRec = new DataTable();
                            dtRec = dv.ToTable();

                            decimal dReceivable = Convert.ToDecimal(dtInterest.Rows[k]["Receivable"]);
                            decimal dBalance = dReceivable;
                            DateTime dCalInterestDate = dInterestDate;

                            if (dtRec.Rows.Count == 0)
                                dInterestDate = dInterestDate.AddDays(iCreditDays);
                            else
                            {
                                for (int j = 0; j < dtRec.Rows.Count; j++)
                                {
                                    decimal dReceived = Convert.ToDecimal(dtRec.Rows[j]["Received"]);
                                    DateTime dRecdDate = Convert.ToDateTime(dtRec.Rows[j]["Date"]);
                                    if (dRecdDate < dInterestDate) { dRecdDate = dCompletionDate; }

                                    if (j == 0)
                                    {
                                        dCalInterestDate = dInterestDate;
                                        dCalInterestDate = dCalInterestDate.AddDays(iCreditDays);
                                    }
                                    else
                                    {
                                        dCalInterestDate = dInterestDate;
                                    }

                                    TimeSpan ts = dRecdDate - dCalInterestDate;
                                    int iDays = ts.Days;
                                    if (iDays < 0) { iDays = 0; }

                                    if (dBalance == 0)
                                        dInterest = 0;
                                    else if (iDays == 0)
                                        dInterest = 0;
                                    else if (dIntPer == 0)
                                        dInterest = 0;
                                    else
                                        dInterest = dInterest + decimal.Round((dBalance * dIntPer * iDays) / 36500, 3);

                                    dBalance = dBalance - dReceived;

                                    dInterestDate = dRecdDate;
                                }
                            }

                            if (dBalance > 0)
                            {
                                TimeSpan ts = DateTime.Now - dInterestDate;
                                int iDays = ts.Days;
                                if (iDays < 0) { iDays = 0; }

                                if (dBalance == 0)
                                    dInterest = 0;
                                else if (iDays == 0)
                                    dInterest = 0;
                                else if (dIntPer == 0)
                                    dInterest = 0;
                                else
                                    dInterest = dInterest + decimal.Round((dBalance * dIntPer / 36500) * iDays, 3);
                            }
                            dBalance = 0;
                        }
                    }

                    dt.Rows[i]["Interest"] = Convert.ToDecimal(dInterest);
                }

                #endregion

                dtMaster = new DataTable();
                dtMaster = dt.Clone();

                int iOldLeadId = 0;
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    int iLeadId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["LeadId"], CommFun.datatypes.vartypenumeric));
                    if (iOldLeadId != iLeadId)
                    {
                        DataView dview = new DataView(dt) { RowFilter = "LeadId=" + iLeadId + "" };
                        DataTable dtFilter = new DataTable();
                        dtFilter = dview.ToTable();
                        if (dtFilter != null)
                        {
                            for (int j = 0; j <= dtFilter.Rows.Count - 1; j++)
                            {
                                if ((j + 1) == dtFilter.Rows.Count)
                                    dtFilter.Rows[j]["DueDate"] = "Within One Week";
                                else
                                    dtFilter.Rows[j]["DueDate"] = "Immediate";
                            }

                            DataRow[] drow = dtFilter.Select("LeadId=" + iLeadId + "");
                            foreach (DataRow dAddRow in drow)
                            {
                                object[] dCopyRow = dAddRow.ItemArray;
                                dtMaster.Rows.Add(dCopyRow);
                            }
                        }
                    }
                    iOldLeadId = iLeadId;
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtMaster;
        }
    }
}
