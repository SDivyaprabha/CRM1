using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Qualifier;
using Microsoft.VisualBasic;

namespace CRM.DataLayer
{
    class MISDL
    {
        #region Projectwise Sales

        internal static DataTable Get_Project_Sales(DateTime argAsOnDate)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                decimal dUnit = BsfGlobal.g_iSummaryUnit;

                string sSql = "SELECT A.CostCentreId,B.CostCentreName,SUM(A.FinalisedFlat)SoldFlat,SUM(A.UnSoldFlat)UnSoldFlat,SUM(A.BlockFlat)BlockFlat,SUM(A.ReserveFlat)ReserveFlat," +
                        "SUM(A.SoldFlat+A.UnSoldFlat+A.BlockFlat+A.ReserveFlat) TotalFlat, SUM(A.SoldArea)SoldArea,SUM(A.UnSoldArea)UnSoldArea,  " +
                        " SUM(A.SoldArea+A.UnSoldArea) TotalArea,SUM(A.SoldAmt)/" + dUnit + " SoldAmt,SUM(A.UnSoldAmt)/" + dUnit + " UnSoldAmt ,"+
                        " SUM (A.SoldAmt+A.UnSoldAmt)/" + dUnit + " TotalAmt FROM (" +
                        " Select A.CostCentreId,Count(A.FlatId) FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,0 BlockFlat,0 ReserveFlat, "+
                        " SUM(Area)SoldArea,0 UnsoldArea,Sum(NetAmt)+Sum(QualifierAmt)SoldAmt,0 UnSoldAmt From dbo.FlatDetails A " +
                        " INNER JOIN dbo.BuyerDetail B On A.FlatId=B.FlatId AND A.LeadId=B.LeadId " +
                        " Where A.Status='S' And B.FinaliseDate<='" + argAsOnDate.ToString("dd-MMM-yyyy") + "' "+
                        " Group By A.CostCentreId " +
                        " UNION ALL  " +
                        " Select B.CostCentreId,Count(A.PlotDetailsId) FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,0 BlockFlat,0 ReserveFlat, "+
                        " SUM(Area)SoldArea,0 UnsoldArea,Sum(NetAmount)+Sum(QualifierAmount)SoldAmt,0 UnSoldAmt From ["+BsfGlobal.g_sRateAnalDBName+"].dbo.LandPlotDetails A " +
                        " INNER JOIN dbo.BuyerDetail B On A.PlotDetailsId=B.PlotId AND A.BuyerId=B.LeadId " +
                        " Where A.Status='S' And B.FinaliseDate<='" + argAsOnDate.ToString("dd-MMM-yyyy") + "' "+
                        " Group By B.CostCentreId " +
                        " UNION ALL  " +
                        " Select A.CostCentreId,0 FinalisedFlat,Count(A.FlatId) SoldFlat,0 UnSoldFlat,0 BlockFlat,0 ReserveFlat,0 SoldArea,0 UnsoldArea, "+
                        " 0 SoldAmt,0 UnSoldAmt From dbo.FlatDetails A " +
                        " INNER JOIN dbo.BuyerDetail B On A.FlatId=B.FlatId AND A.LeadId=B.LeadId " +
                        " Where A.Status='S' Group By A.CostCentreId " +
                        " UNION ALL " +
                        " Select B.CostCentreId,0 FinalisedFlat,Count(A.PlotDetailsId) SoldFlat,0 UnSoldFlat,0 BlockFlat,0 ReserveFlat,0 SoldArea,0 UnsoldArea, " +
                        " 0 SoldAmt,0 UnSoldAmt From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A " +
                        " INNER JOIN dbo.BuyerDetail B On A.PlotDetailsId=B.PlotId AND A.BuyerId=B.LeadId " +
                        " Where A.Status='S' Group By B.CostCentreId " +
                        " UNION ALL " +
                        " Select CostCentreId,0 FinalisedFlat, 0 SoldFlat,Count(A.FlatId) UnSoldFlat,0 BlockFlat,0 ReserveFlat,0 SoldArea,SUM(Area) UnsoldArea, " +
                        " 0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt From dbo.FlatDetails A " +
                        " Where A.Status='U' Group By CostCentreId" +
                        " UNION ALL " +
                        " Select C.CostCentreId,0 FinalisedFlat, 0 SoldFlat,Count(A.PlotDetailsId) UnSoldFlat,0 BlockFlat,0 ReserveFlat,0 SoldArea,SUM(Area) UnsoldArea, " +
                        " 0 SoldAmt,Sum(NetAmount)+Sum(QualifierAmount) UnSoldAmt From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A " +
                        " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B On A.LandRegisterId=B.LandId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On B.ProjectName=C.ProjectDB " +
                        " Where A.Status='U' Group By C.CostCentreId" +
                        " UNION ALL " +
                        " Select CostCentreId,0 FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,Count(A.FlatId) BlockFlat,0 ReserveFlat,0 SoldArea,SUM(Area) UnsoldArea, "+
                        " 0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt From dbo.FlatDetails A " +
                        " Where A.Status='B' Group By CostCentreId" +
                        " UNION ALL " +
                        " Select C.CostCentreId,0 FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,Count(A.PlotDetailsId) BlockFlat,0 ReserveFlat,0 SoldArea,SUM(Area) UnsoldArea, " +
                        " 0 SoldAmt,Sum(NetAmount)+Sum(QualifierAmount) UnSoldAmt From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A " +
                        " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B On A.LandRegisterId=B.LandId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On B.ProjectName=C.ProjectDB " +
                        " Where A.Status='B' Group By C.CostCentreId" +
                        " UNION ALL " +
                        " Select CostCentreId,0 FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,0 BlockFlat,Count(A.FlatId) ReserveFlat,0 SoldArea,SUM(Area) UnsoldArea, "+
                        " 0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt From dbo.FlatDetails A " +
                        " Where A.Status='R' Group By CostCentreId" +
                        " UNION ALL " +
                        " Select C.CostCentreId,0 FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,0 BlockFlat,Count(A.PlotDetailsId) ReserveFlat,0 SoldArea,SUM(Area) UnsoldArea, " +
                        " 0 SoldAmt,Sum(NetAmount)+Sum(QualifierAmount) UnSoldAmt From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A " +
                        " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B On A.LandRegisterId=B.LandId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On B.ProjectName=C.ProjectDB " +
                        " Where A.Status='R' Group By C.CostCentreId" +
                        " )A "+
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B On A.CostCentreId=B.CostCentreId " +
                        " AND B.CostCentreId NOT IN(Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                        " GROUP BY A.CostCentreId,B.CostCentreName Order By B.CostCentreName";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        internal static DataTable Get_Block_Sales(int arg_iProjectId, DateTime argAsOnDate)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                decimal dUnit = BsfGlobal.g_iSummaryUnit;

                String sSql = "SELECT A.BlockId,B.BlockName,SUM(A.FinalisedFlat)SoldFlat,SUM(A.UnSoldFlat)UnSoldFlat,SUM(A.BlockFlat)BlockFlat,SUM(A.ReserveFlat)ReserveFlat," +
                        "SUM(A.SoldFlat+A.UnSoldFlat+A.BlockFlat+A.ReserveFlat) TotalFlat, SUM( A.SoldArea)SoldArea,SUM(A.UnSoldArea)UnSoldArea,  " +
                        " SUM(A.SoldArea+A.UnSoldArea) TotalArea,SUM(A.SoldAmt)/" + dUnit + " SoldAmt,SUM(A.UnSoldAmt)/" + dUnit + " UnSoldAmt,"+
                        " SUM (A.SoldAmt+A.UnSoldAmt)/" + dUnit + " TotalAmt FROM (" +
                        " Select BlockId,Count(A.FlatId) FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,0 BlockFlat,0 ReserveFlat,SUM(Area)SoldArea,0 UnsoldArea,Sum(NetAmt)+Sum(QualifierAmt)SoldAmt,0 UnSoldAmt From FlatDetails A " +
                        " left Join BuyerDetail B On A.FlatId=B.FlatId " +
                        " Where A.Status='S' And A.CostCentreId=" + arg_iProjectId + " And B.FinaliseDate<='" + argAsOnDate.ToString("dd-MMM-yyyy") + "' Group By BlockId " +
                        " UNION ALL  " +
                        " Select BlockId,0 FinalisedFlat,Count(A.FlatId) SoldFlat,0 UnSoldFlat,0 BlockFlat,0 ReserveFlat,0 SoldArea,0 UnsoldArea,0 SoldAmt,0 UnSoldAmt From FlatDetails A " +
                        " left Join BuyerDetail B On A.FlatId=B.FlatId " +
                        " Where A.Status='S' And A.CostCentreId=" + arg_iProjectId + " Group By BlockId " +
                        " UNION ALL " +
                        " Select BlockId,0 FinalisedFlat, 0 SoldFlat,Count(A.FlatId) UnSoldFlat,0 BlockFlat,0 ReserveFlat,0 SoldArea,SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt From FlatDetails A " +
                        " Where A.Status='U' And A.CostCentreId=" + arg_iProjectId + "  Group By BlockId"+
                        " UNION ALL " +
                        " Select BlockId,0 FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,Count(A.FlatId) BlockFlat,0 ReserveFlat,0 SoldArea,SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt From FlatDetails A " +
                        " Where A.Status='B' And A.CostCentreId=" + arg_iProjectId + "  Group By BlockId"+
                        " UNION ALL " +
                        " Select BlockId,0 FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,0 BlockFlat,Count(A.FlatId) ReserveFlat,0 SoldArea,SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt From FlatDetails A " +
                        " Where A.Status='R' And A.CostCentreId=" + arg_iProjectId + "  Group By BlockId)A INNER JOIN BlockMaster B On A.BlockId=B.BlockId Group By A.BlockId,B.BlockName";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        internal static DataTable Get_Level_Sales(int arg_iProjectId, int arg_iBlockId, DateTime argAsOnDate)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                decimal dUnit = BsfGlobal.g_iSummaryUnit;

                String sSql = "SELECT A.LevelId,B.LevelName,SUM(A.FinalisedFlat)SoldFlat,SUM(A.UnSoldFlat)UnSoldFlat,SUM(A.BlockFlat)BlockFlat,SUM(A.ReserveFlat)ReserveFlat," +
                        "SUM(A.SoldFlat+A.UnSoldFlat+A.BlockFlat+A.ReserveFlat) TotalFlat, " +
                        " SUM( A.SoldArea)SoldArea,SUM(A.UnSoldArea)UnSoldArea,   SUM(A.SoldArea+A.UnSoldArea) TotalArea,SUM( A.SoldAmt)/" + dUnit + " SoldAmt, " +
                        " SUM(A.UnSoldAmt)/" + dUnit + " UnSoldAmt , SUM (A.SoldAmt+A.UnSoldAmt)/" + dUnit + " TotalAmt FROM ("+
                        " Select LevelId,Count(A.FlatId) FinalisedFlat, 0 SoldFlat, 0 UnSoldFlat, 0 BlockFlat, 0 ReserveFlat," +
                        " SUM(Area)SoldArea,0 UnsoldArea,Sum(NetAmt)+Sum(QualifierAmt)SoldAmt,0 UnSoldAmt From FlatDetails A  " +
                        " left Join BuyerDetail B On A.FlatId=B.FlatId  Where A.Status='S' And A.CostCentreId=" + arg_iProjectId + " And A.BlockId=" + arg_iBlockId + " " +
                        " And B.FinaliseDate<='" + argAsOnDate.ToString("dd-MMM-yyyy") + "' " +
                        " Group By LevelId  " +
                        " UNION ALL  " +
                        " Select LevelId,0 FinalisedFlat,Count(A.FlatId) SoldFlat, 0 UnSoldFlat, 0 BlockFlat, 0 ReserveFlat," +
                        " 0 SoldArea,0 UnsoldArea,0 SoldAmt,0 UnSoldAmt From FlatDetails A  " +
                        " left Join BuyerDetail B On A.FlatId=B.FlatId  Where A.Status='S' And A.CostCentreId=" + arg_iProjectId + " And A.BlockId=" + arg_iBlockId + " " +
                        " Group By LevelId  " +
                        " UNION ALL  " +
                        " Select LevelId,0 FinalisedFlat, 0 SoldFlat,Count(A.FlatId) UnSoldFlat,0 BlockFlat,0 ReserveFlat,0 SoldArea,SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt " +
                        " From FlatDetails A  Where A.Status='U' And A.CostCentreId=" + arg_iProjectId + " And A.BlockId=" + arg_iBlockId + " " +
                        " Group By LevelId"+
                        " UNION ALL  " +
                        " Select LevelId,0 FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,Count(A.FlatId) BlockFlat,0 ReserveFlat,0 SoldArea,SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt " +
                        " From FlatDetails A  Where A.Status='B' And A.CostCentreId=" + arg_iProjectId + " And A.BlockId=" + arg_iBlockId + " " +
                        " Group By LevelId"+
                        " UNION ALL  " +
                        " Select LevelId,0 FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,0 BlockFlat,Count(A.FlatId) ReserveFlat,0 SoldArea,SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt " +
                        " From FlatDetails A  Where A.Status='R' And A.CostCentreId=" + arg_iProjectId + " And A.BlockId=" + arg_iBlockId + " " +
                        " Group By LevelId)A INNER JOIN LevelMaster B On A.LevelId=B.LevelId Group By A.LevelId,B.LevelName";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        internal static DataTable Get_Flat_Sales(int arg_iProjectId, int arg_iBlockId, int m_iLevelId, DateTime argAsOnDate)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                decimal dUnit = BsfGlobal.g_iSummaryUnit;

                String sSql = "Select BusinessType From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre A" +
                              " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B ON A.ProjectDB=B.ProjectName" +
                              " Where CostCentreId=" + arg_iProjectId + " ";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();

                string sBusinessType = "";
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count > 0)
                    {
                        sBusinessType = CommFun.IsNullCheck(dt.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                    }
                }

                if (sBusinessType == "L")
                {
                    sSql = "SELECT D.AllotmentNo FANo,A.PlotDetailsId FlatId,A.PlotNo FlatNo,FT.PlotTypeName TypeName,E.LeadName BuyerName,E.Mobile,A.Area,A.Rate," +
                            " 0 TotalCarPark, (A.NetAmount+A.QualifierAmount)/" + dUnit + " NetAmt,A.PlotDetailsId FlatId,0 AccountId," +
                            " D.Status,0 LevelId,D.RegDate,D.LeadId, ISNULL(LC.CampaignName,'') Campaign FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotType FT ON FT.PlotTypeId=A.PlotTypeId " +
                            " LEFT JOIN BuyerDetail D ON D.PlotId=A.PlotDetailsId AND A.BuyerId=D.LeadId " +
                            " LEFT JOIN LeadRegister E ON E.LeadId=D.LeadId " +
                            " LEFT JOIN LeadProjectInfo LP ON E.LeadId=LP.LeadId " +
                            " LEFT JOIN CampaignDetails LC ON LP.CampaignId=LC.CampaignId " +
                            " WHERE D.CostCentreId=" + arg_iProjectId + " AND A.Status='S' " +
                            " And FinaliseDate<='" + argAsOnDate.ToString("dd-MMM-yyyy") + "' " +
                            " ORDER BY A.PlotNo,dbo.Val(A.PlotNo) ";
                }
                else
                {
                    sSql = "SELECT D.AllotmentNo FANo,B.FlatId,B.FlatNo,C.LevelName FloorName,FT.TypeName,E.LeadName BuyerName,E.Mobile,B.Area,B.Rate," +
                            " B.TotalCarPark, (B.NetAmt+B.QualifierAmt)/" + dUnit + " NetAmt,B.FlatId,B.AccountId," +
                            " B.Status,B.LevelId,B.RegDate,B.LeadId, ISNULL(LC.CampaignName,'') Campaign FROM FlatDetails B "+
                            " INNER JOIN LevelMaster C ON B.LevelId=C.LevelId " +
                            " INNER JOIN dbo.BlockMaster BM ON BM.BlockId=B.BlockId "+
                            " INNER JOIN dbo.FlatType FT ON FT.FlatTypeId=B.FlatTypeId " +
                            " LEFT JOIN BuyerDetail D ON D.FlatId=B.FlatId AND B.LeadId=D.LeadId " +
                            " LEFT JOIN LeadRegister E ON E.LeadId=D.LeadId " +
                            " LEFT JOIN LeadProjectInfo LP ON E.LeadId=LP.LeadId " +
                            " LEFT JOIN CampaignDetails LC ON LP.CampaignId=LC.CampaignId " +
                            " WHERE B.CostCentreId=" + arg_iProjectId + " And B.BlockId=" + arg_iBlockId + " And B.LevelId=" + m_iLevelId + " And B.Status='S' " +
                            " And FinaliseDate<='" + argAsOnDate.ToString("dd-MMM-yyyy") + "' " +
                            " ORDER BY BM.SortOrder,C.SortOrder,B.SortOrder,dbo.Val(B.FlatNo) ";
                }
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        internal static DataTable Get_BWProject_Sales(string argFromDate, string argToDate)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                decimal dUnit = BsfGlobal.g_iSummaryUnit;

                String sSql = "SELECT A.CostCentreId,B.CostCentreName,SUM(A.FinalisedFlat)SoldFlat,SUM(A.UnSoldFlat)UnSoldFlat,SUM(A.BlockFlat)BlockFlat,SUM(A.ReserveFlat)ReserveFlat," +
                        "SUM(A.SoldFlat+A.UnSoldFlat+A.BlockFlat+A.ReserveFlat) TotalFlat, SUM( A.SoldArea)SoldArea,SUM(A.UnSoldArea)UnSoldArea,  " +
                        " SUM(A.SoldArea+A.UnSoldArea) TotalArea,SUM(A.SoldAmt)/" + dUnit + " SoldAmt,SUM(A.UnSoldAmt)/" + dUnit + " UnSoldAmt,"+
                        " SUM (A.SoldAmt+A.UnSoldAmt)/" + dUnit + " TotalAmt FROM (" +
                        " Select A.CostCentreId,Count(A.FlatId) FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,0 BlockFlat,0 ReserveFlat, "+
                        " SUM(Area)SoldArea,0 UnsoldArea,Sum(NetAmt)+Sum(QualifierAmt)SoldAmt,0 UnSoldAmt From dbo.FlatDetails A " +
                        " INNER JOIN dbo.BuyerDetail B On A.FlatId=B.FlatId AND A.LeadId=B.LeadId " +
                        " Where A.Status='S' And B.FinaliseDate Between '" + argFromDate + "' And '" + argToDate + "' Group By A.CostCentreId " +
                        " UNION ALL " +
                        " Select B.CostCentreId,Count(A.PlotDetailsId) FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,0 BlockFlat,0 ReserveFlat, " +
                        " SUM(Area)SoldArea,0 UnsoldArea,Sum(NetAmount)+Sum(QualifierAmount)SoldAmt,0 UnSoldAmt From ["+ BsfGlobal.g_sRateAnalDBName +"].dbo.LandPlotDetails A " +
                        " INNER JOIN dbo.BuyerDetail B On A.PlotDetailsId=B.PlotId AND A.BuyerId=B.LeadId " +
                        " Where A.Status='S' And B.FinaliseDate Between '" + argFromDate + "' And '" + argToDate + "' Group By B.CostCentreId " +
                        " UNION ALL " +
                        " Select A.CostCentreId, 0 FinalisedFlat,Count(A.FlatId) SoldFlat,0 UnSoldFlat,0 BlockFlat,0 ReserveFlat,0 SoldArea,0 UnsoldArea, "+
                        " 0 SoldAmt,0 UnSoldAmt From dbo.FlatDetails A " +
                        " INNER JOIN dbo.BuyerDetail B On A.FlatId=B.FlatId AND A.LeadId=B.LeadId " +
                        " Where A.Status='S' Group By A.CostCentreId " +
                        " UNION ALL " +
                        " Select B.CostCentreId, 0 FinalisedFlat,Count(A.PlotDetailsId) SoldFlat,0 UnSoldFlat,0 BlockFlat,0 ReserveFlat,0 SoldArea,0 UnsoldArea, " +
                        " 0 SoldAmt,0 UnSoldAmt From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A " +
                        " INNER JOIN dbo.BuyerDetail B On A.PlotDetailsId=B.PlotId AND A.BuyerId=B.LeadId " +
                        " Where A.Status='S' Group By B.CostCentreId " +
                        " UNION ALL " +
                        " Select CostCentreId,0 FinalisedFlat, 0 SoldFlat,Count(A.FlatId) UnSoldFlat,0 BlockFlat,0 ReserveFlat,0 SoldArea, "+
                        " SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt From dbo.FlatDetails A " +
                        " Where A.Status='U' Group By CostCentreId"+
                        " UNION ALL " +
                        " Select C.CostCentreId,0 FinalisedFlat, 0 SoldFlat,Count(A.PlotDetailsId) UnSoldFlat,0 BlockFlat,0 ReserveFlat,0 SoldArea, " +
                        " SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmount)+Sum(QualifierAmount) UnSoldAmt From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A " +
                        " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B On A.LandRegisterId=B.LandId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On B.ProjectName=C.ProjectDB " +
                        " Where A.Status='U' Group By C.CostCentreId" +
                        " UNION ALL " +
                        " Select CostCentreId,0 FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,Count(A.FlatId) BlockFlat,0 ReserveFlat,0 SoldArea, "+
                        " SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt From dbo.FlatDetails A " +
                        " Where A.Status='B' Group By CostCentreId" +
                        " UNION ALL " +
                        " Select C.CostCentreId,0 FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,Count(A.PlotDetailsId) BlockFlat,0 ReserveFlat,0 SoldArea, " +
                        " SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmount)+Sum(QualifierAmount) UnSoldAmt From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A " +
                        " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B On A.LandRegisterId=B.LandId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On B.ProjectName=C.ProjectDB " +
                        " Where A.Status='B' Group By C.CostCentreId" +
                        " UNION ALL " +
                        " Select CostCentreId,0 FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,0 BlockFlat,Count(A.FlatId) ReserveFlat,0 SoldArea, "+
                        " SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt From dbo.FlatDetails A " +
                        " Where A.Status='R' Group By CostCentreId" +
                        " UNION ALL " +
                        " Select C.CostCentreId,0 FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,0 BlockFlat,Count(A.PlotDetailsId) ReserveFlat,0 SoldArea, " +
                        " SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmount)+Sum(QualifierAmount) UnSoldAmt From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A " +
                        " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B On A.LandRegisterId=B.LandId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On B.ProjectName=C.ProjectDB " +
                        " Where A.Status='R' Group By C.CostCentreId" +
                        ")A INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B On A.CostCentreId=B.CostCentreId " +
                        " And B.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                        " Group By A.CostCentreId,B.CostCentreName Order By B.CostCentreName";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        internal static DataTable Get_BWBlock_Sales(int arg_iProjectId, string argFromDate, string argToDate)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                decimal dUnit = BsfGlobal.g_iSummaryUnit;

                String sSql = "SELECT A.BlockId,B.BlockName,SUM(A.FinalisedFlat)SoldFlat,SUM(A.UnSoldFlat)UnSoldFlat,SUM(A.BlockFlat)BlockFlat,SUM(A.ReserveFlat)ReserveFlat," +
                        "SUM(A.SoldFlat+A.UnSoldFlat+A.BlockFlat+A.ReserveFlat) TotalFlat, SUM( A.SoldArea)SoldArea,SUM(A.UnSoldArea)UnSoldArea,  " +
                        " SUM(A.SoldArea+A.UnSoldArea) TotalArea,SUM( A.SoldAmt)/" + dUnit + " SoldAmt,SUM(A.UnSoldAmt)/" + dUnit + " UnSoldAmt," +
                        " SUM (A.SoldAmt+A.UnSoldAmt)/" + dUnit + " TotalAmt FROM (" +
                        " Select BlockId,Count(A.FlatId) FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,0 BlockFlat,0 ReserveFlat,SUM(Area)SoldArea,0 UnsoldArea,Sum(NetAmt)+Sum(QualifierAmt)SoldAmt,0 UnSoldAmt From FlatDetails A " +
                        " left Join BuyerDetail B On A.FlatId=B.FlatId " +
                        " Where A.Status='S' And A.CostCentreId=" + arg_iProjectId + " And B.FinaliseDate Between '" + argFromDate + "' And '" + argToDate + "' Group By BlockId " +
                        " UNION ALL " +
                        " Select BlockId,0 FinalisedFlat,Count(A.FlatId) SoldFlat,0 UnSoldFlat,0 BlockFlat,0 ReserveFlat,0 SoldArea,0 UnsoldArea,0 SoldAmt,0 UnSoldAmt From FlatDetails A " +
                        " left Join BuyerDetail B On A.FlatId=B.FlatId " +
                        " Where A.Status='S' And A.CostCentreId=" + arg_iProjectId + " Group By BlockId " +
                        " UNION ALL " +
                        " Select BlockId,0 FinalisedFlat, 0 SoldFlat,Count(A.FlatId) UnSoldFlat,0 BlockFlat,0 ReserveFlat,0 SoldArea,SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt From FlatDetails A " +
                        " Where A.Status='U' And A.CostCentreId=" + arg_iProjectId + "  Group By BlockId" +
                        " UNION ALL " +
                        " Select BlockId,0 FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,Count(A.FlatId) BlockFlat,0 ReserveFlat,0 SoldArea,SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt From FlatDetails A " +
                        " Where A.Status='B' And A.CostCentreId=" + arg_iProjectId + "  Group By BlockId" +
                        " UNION ALL " +
                        " Select BlockId,0 FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,0 BlockFlat,Count(A.FlatId) ReserveFlat,0 SoldArea,SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt From FlatDetails A " +
                        " Where A.Status='R' And A.CostCentreId=" + arg_iProjectId + "  Group By BlockId" +
                        ")A INNER JOIN BlockMaster B On A.BlockId=B.BlockId Group By A.BlockId,B.BlockName";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        internal static DataTable Get_BWLevel_Sales(int arg_iProjectId, int arg_iBlockId, string argFromDate, string argToDate)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                decimal dUnit = BsfGlobal.g_iSummaryUnit;

                String sSql = "SELECT A.LevelId,B.LevelName,SUM(A.FinalisedFlat)SoldFlat,SUM(A.UnSoldFlat)UnSoldFlat,SUM(A.BlockFlat)BlockFlat,SUM(A.ReserveFlat)ReserveFlat," +
                        "SUM(A.SoldFlat+A.UnSoldFlat+A.BlockFlat+A.ReserveFlat) TotalFlat, " +
                        " SUM( A.SoldArea)SoldArea,SUM(A.UnSoldArea)UnSoldArea,SUM(A.SoldArea+A.UnSoldArea) TotalArea,SUM( A.SoldAmt)/" + dUnit + " SoldAmt, " +
                        " SUM(A.UnSoldAmt)/" + dUnit + " UnSoldAmt,SUM (A.SoldAmt+A.UnSoldAmt)/" + dUnit + " TotalAmt FROM (" +
                        " Select LevelId,Count(A.FlatId) FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,0 BlockFlat,0 ReserveFlat, " +
                        " SUM(Area)SoldArea,0 UnsoldArea,Sum(NetAmt)+Sum(QualifierAmt)SoldAmt,0 UnSoldAmt From FlatDetails A  " +
                        " left Join BuyerDetail B On A.FlatId=B.FlatId  Where A.Status='S' And A.CostCentreId=" + arg_iProjectId + " And A.BlockId=" + arg_iBlockId + " And B.FinaliseDate " +
                        " Between '" + argFromDate + "' And '" + argToDate + "' Group By LevelId  " +
                        " UNION ALL  " +
                        " Select LevelId,0 FinalisedFlat,Count(A.FlatId) SoldFlat,0 UnSoldFlat,0 BlockFlat,0 ReserveFlat, " +
                        " 0 SoldArea,0 UnsoldArea,0 SoldAmt,0 UnSoldAmt From FlatDetails A  " +
                        " left Join BuyerDetail B On A.FlatId=B.FlatId  Where A.Status='S' And A.CostCentreId=" + arg_iProjectId + " And A.BlockId=" + arg_iBlockId + " Group By LevelId  " +
                        " UNION ALL  " +
                        " Select LevelId,0 FinalisedFlat, 0 SoldFlat,Count(A.FlatId) UnSoldFlat,0 BlockFlat,0 ReserveFlat,0 SoldArea,SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt " +
                        " From FlatDetails A  Where A.Status='U' And A.CostCentreId=" + arg_iProjectId + " And A.BlockId=" + arg_iBlockId + " Group By LevelId " +
                        " UNION ALL  " +
                        " Select LevelId,0 FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,Count(A.FlatId) BlockFlat,0 ReserveFlat,0 SoldArea,SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt " +
                        " From FlatDetails A  Where A.Status='B' And A.CostCentreId=" + arg_iProjectId + " And A.BlockId=" + arg_iBlockId + " Group By LevelId " +
                        " UNION ALL  " +
                        " Select LevelId,0 FinalisedFlat, 0 SoldFlat,0 UnSoldFlat,0 BlockFlat,Count(A.FlatId) ReserveFlat,0 SoldArea,SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt " +
                        " From FlatDetails A  Where A.Status='R' And A.CostCentreId=" + arg_iProjectId + " And A.BlockId=" + arg_iBlockId + " Group By LevelId " +
                        " )A INNER JOIN LevelMaster B On A.LevelId=B.LevelId " +
                        " Group By A.LevelId,B.LevelName";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        internal static DataTable Get_BWFlat_Sales(int arg_iProjectId, int arg_iBlockId, int arg_iLevelId, string argFromDate, string argToDate)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                decimal dUnit = BsfGlobal.g_iSummaryUnit;

                String sSql = "Select BusinessType From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre A" +
                              " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B ON A.ProjectDB=B.ProjectName" +
                              " Where CostCentreId=" + arg_iProjectId + " ";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();

                string sBusinessType = "";
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count > 0)
                    {
                        sBusinessType = CommFun.IsNullCheck(dt.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                    }
                }

                if (sBusinessType == "L")
                {
                    sSql = "SELECT D.AllotmentNo FANo,B.PlotDetailsId FlatId,B.PlotNo FlatNo,'' FloorName,FT.PlotTypeName TypeName, "+ 
                            " E.LeadName BuyerName,E.Mobile,B.Area,B.Rate,0 TotalCarPark,(B.NetAmount+B.QualifierAmount)/" + dUnit + " NetAmt, "+
                            " B.PlotDetailsId FlatId,0 AccountId,B.Status,0 LevelId,D.RegDate,B.BuyerId LeadId, ISNULL(LC.CampaignName,'') Campaign FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails B " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotType FT ON FT.PlotTypeId=B.PlotTypeId " +
                            " LEFT JOIN BuyerDetail D ON D.PlotId=B.PlotDetailsId AND B.BuyerId=D.LeadId " +
                            " LEFT JOIN LeadRegister E ON E.LeadId=D.LeadId " +
                            " LEFT JOIN LeadProjectInfo LP ON E.LeadId=LP.LeadId " +
                            " LEFT JOIN CampaignDetails LC ON LP.CampaignId=LC.CampaignId " +
                            " WHERE D.CostCentreId=" + arg_iProjectId + " And B.Status='S' " +
                            " And FinaliseDate Between '" + argFromDate + "' And '" + argToDate + "' " +
                            " Order By B.PlotNo,dbo.Val(B.PlotNo) ";
                }
                else
                {
                    sSql = "SELECT D.AllotmentNo FANo,B.FlatId,B.FlatNo,C.LevelName FloorName,FT.TypeName,E.LeadName BuyerName,E.Mobile,B.Area,B.Rate," +
                            " B.TotalCarPark,(B.NetAmt+B.QualifierAmt)/" + dUnit + " NetAmt,B.FlatId,B.AccountId," +
                            " B.Status,B.LevelId,B.RegDate,B.LeadId, ISNULL(LC.CampaignName,'') Campaign FROM FlatDetails B INNER JOIN LevelMaster C ON B.LevelId=C.LevelId " +
                            " INNER JOIN dbo.BlockMaster BM ON BM.BlockId=B.BlockId INNER JOIN dbo.FlatType FT ON FT.FlatTypeId=B.FlatTypeId " +
                            " left JOIN BuyerDetail D ON D.FlatId=B.FlatId AND B.LeadId=D.LeadId " +
                            " left JOIN LeadRegister E ON E.LeadId=D.LeadId " +
                            " LEFT JOIN LeadProjectInfo LP ON E.LeadId=LP.LeadId " +
                            " LEFT JOIN CampaignDetails LC ON LP.CampaignId=LC.CampaignId " +
                            " WHERE B.CostCentreId=" + arg_iProjectId + " And B.BlockId=" + arg_iBlockId + " And B.LevelId=" + arg_iLevelId + " And B.Status='S' " +
                            " And FinaliseDate Between '" + argFromDate + "' And '" + argToDate + "' " +
                            " Order By BM.SortOrder,C.SortOrder,B.SortOrder,dbo.Val(B.FlatNo) ";
                }
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        internal static DataTable GetCustomerPrint()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Select B.AllotmentNo FANo,C.LeadName BuyerName,D.Address1,D.Address2,C.Mobile PhoneNo1, " +
                        " E.CoMobile PhoneNo2,C.Email Email1,E.CoEmail Email2,D.Locality From dbo.FlatDetails A " +
                        " Inner Join dbo.BuyerDetail B On A.FlatId=B.FlatId " +
                        " Inner Join dbo.LeadRegister C On C.LeadId=A.LeadId " +
                        " Inner Join dbo.LeadCommAddressInfo D On D.LeadId=C.LeadId " +
                        " Inner Join dbo.LeadCoAppAddressInfo E On E.LeadId=C.LeadId Order By C.LeadName ";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
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

        internal static DataTable GetTypewiseSalesPrint()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "SELECT B.CostCentreName,A.FlatTypeId,C.TypeName,SUM(A.SoldFlat)SoldFlat,SUM(A.UnSoldFlat)UnSoldFlat,SUM(A.SoldFlat+A.UnSoldFlat) TotalFlat, " +
                            " SUM( A.SoldArea)SoldArea,SUM(A.UnSoldArea)UnSoldArea,   SUM(A.SoldArea+A.UnSoldArea) TotalArea,SUM( A.SoldAmt) SoldAmt, " +
                            " SUM(A.UnSoldAmt) UnSoldAmt , SUM (A.SoldAmt+A.UnSoldAmt) TotalAmt FROM ( " +
                            " Select A.CostCentreId,A.FlatTypeId,Count(A.FlatId) SoldFlat, " +
                            " 0 UnSoldFlat,SUM(Area)SoldArea,0 UnsoldArea,Sum(NetAmt)+Sum(QualifierAmt)SoldAmt,0 UnSoldAmt From dbo.FlatDetails A " +
                            " Left Join dbo.BuyerDetail B On A.FlatId=B.FlatId  Where A.Status='S' Group By A.CostCentreId,A.FlatTypeId " +
                            " UNION ALL  " +
                            " Select A.CostCentreId,A.FlatTypeId,0,Count(A.FlatId) UnSoldFlat,0 SoldArea,SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt " +
                            " From dbo.FlatDetails A  Where A.Status<>'S' Group By A.CostCentreId,A.FlatTypeId)A " +
                            " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostcentre B On A.CostCentreId=B.CostCentreId " +
                            " INNER JOIN dbo.FlatType C ON C.FlatTypeId=A.FlatTypeId Group By B.CostCentreName,A.FlatTypeId,C.TypeName";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
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

        #endregion

        #region  Projectwise Receivable

        internal static DataTable Get_Project_Receivable(DateTime arg_dAsOn)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            string sActual = string.Empty;
            try
            {
                decimal dUnit = BsfGlobal.g_iSummaryUnit;

                string sSql = "SELECT ProjectId, CostCentreName,ProjectDB, SUM(AgreementValue)/" + dUnit + " AgreementValue, SUM(Receivable)/" + dUnit + " ReceivableAsOn, " +
                        " SUM(Received)/" + dUnit + " Received,  (SUM(Receivable)-SUM(Received))/" + dUnit + " DueAsOn, (SUM(AgreementValue)-SUM(Received))/" + dUnit + " TotalReceivable," +
                        " Case When SUM(Receivable)<>0 Then (SUM(Receivable)-SUM(Received))/SUM(Receivable)*100 Else 0 End [Recv%] " +
                        " FROM ( " +
                        " SELECT A.CostCentreId ProjectId, SUM(NetAmt)+SUM(A.QualifierAmt) AgreementValue,0 Receivable, 0 Received, 0 Due,0 TotReceivable FROM dbo.FlatDetails A" +
                        " INNER JOIN dbo.BuyerDetail B ON A.FlatId=B.FlatId" +
                        " INNER JOIN dbo.LeadRegister L ON L.LeadId=B.LeadId " +
                        " WHERE A.LeadId<>0 GROUP BY A.CostCentreId  " +
                        " UNION ALL   " +
                        " SELECT OC.CostCentreId ProjectId, SUM(A.NetAmount)+SUM(A.QualifierAmount) AgreementValue, " +
                        " 0 Receivable, 0 Received, 0 Due,0 TotReceivable FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A" +
                        " INNER JOIN dbo.BuyerDetail B ON A.PlotDetailsId=B.PlotId" +
                        " INNER JOIN dbo.LeadRegister L ON L.LeadId=B.LeadId " +
                        " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister CR ON A.LandRegisterId=CR.LandId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON CR.ProjectName=OC.ProjectDB " +
                        " WHERE A.BuyerId<>0 " +
                        " GROUP BY OC.CostCentreId " +
                        " UNION ALL   " +
                        " SELECT DISTINCT A.CostCentreId ProjectId,0 AgreementValue,SUM(A.NetAmount) Receivable,0 Received,0 Due,0 TotReceivable FROM dbo.ProgressBillRegister A" +
                        " INNER JOIN dbo.FlatDetails B ON A.FlatId=B.FlatId" +
                        " INNER JOIN dbo.BuyerDetail D ON A.FlatId=D.FlatId " +
                        " INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId" +
                        " Left JOIN dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId " +
                        " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                        " WHERE PBDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dAsOn) + "' AND S.BillPassed=1 " +
                        " AND A.PBDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                        " GROUP BY A.CostCentreId " +
                        " UNION ALL   " +
                        " SELECT DISTINCT OC.CostCentreId ProjectId,0 AgreementValue,SUM(A.NetAmount) Receivable,0 Received,0 Due,0 TotReceivable FROM dbo.PlotProgressBillRegister A" +
                        " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails B ON A.PlotDetailsId=B.PlotDetailsId" +
                        " INNER JOIN dbo.BuyerDetail D ON A.PlotDetailsId=D.PlotId " +
                        " INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId" +
                        " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister CR ON B.LandRegisterId=CR.LandId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON CR.ProjectName=OC.ProjectDB " +
                        " Left JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot S On S.PaymentSchId=A.PaySchId " +
                        " LEFT JOIN dbo.AllotmentCancel AC ON A.PlotDetailsId=AC.FlatId AND AC.Approve='Y' " +
                        " WHERE PBDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dAsOn) + "' AND S.BillPassed=1 " +
                        " AND A.PBDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                        " GROUP BY OC.CostCentreId " +
                        " UNION ALL " +
                        " SELECT DISTINCT A.CostCentreId ProjectId,0 AgreementValue,SUM(A.NetAmount) Receivable,0,0 Due,0 TotReceivable FROM dbo.PaymentScheduleFlat A" +
                        " INNER JOIN dbo.FlatDetails B ON A.FlatId=B.FlatId" +
                        " INNER JOIN dbo.BuyerDetail D ON A.FlatId=D.FlatId " +
                        " INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId" +
                        " LEFT JOIN dbo.StageDetails F ON F.StageDetId=A.StageDetId" +
                        " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                        " WHERE ((A.CostCentreId IN(SELECT CostCentreId FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CRMReceivable=0) AND SchDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dAsOn) + "' And A.BillPassed=0) " +
                        " OR (A.CostCentreId IN (SELECT CostCentreId FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CRMReceivable=1) " +
                        " AND A.StageDetId<>0 AND F.CompletionDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dAsOn) + "' AND F.CompletionDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END)) " +
                        " GROUP BY A.CostCentreId" +
                        " UNION ALL " +
                        " SELECT DISTINCT OC.CostCentreId ProjectId,0 AgreementValue,SUM(A.NetAmount) Receivable,0,0 Due,0 TotReceivable FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot A" +
                        " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails B ON A.PlotDetailsId=B.PlotDetailsId" +
                        " INNER JOIN dbo.BuyerDetail D ON A.PlotDetailsId=D.PlotId " +
                        " INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId" +
                        " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister CR ON B.LandRegisterId=CR.LandId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON CR.ProjectName=OC.ProjectDB " +
                        " LEFT JOIN dbo.AllotmentCancel AC ON A.PlotDetailsId=AC.FlatId AND AC.Approve='Y' " +
                        " WHERE ((OC.CostCentreId IN(SELECT CostCentreId FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CRMReceivable=0) AND A.SchDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dAsOn) + "' And A.BillPassed=0) " +
                        " OR (OC.CostCentreId IN (SELECT CostCentreId FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CRMReceivable=1) " +
                        " AND A.SchDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dAsOn) + "' AND A.SchDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END)) " +
                        " GROUP BY OC.CostCentreId";

                sSql = sSql + " UNION ALL " +
                              " Select CostCentreId, 0, 0, ISNULL(Sum(Received), 0) Received, 0, 0 From( "+
                              " Select OC.CostCentreId, ISNULL((SELECT ISNULL(SUM(A.Amount),0) FROM ( " +
                              " SELECT (-1*C.OBReceipt)+ISNULL(A.Amount,0) Amount FROM dbo.ReceiptTrans A " +
                              " INNER JOIN dbo.ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                              " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                              " WHERE A.FlatId=C.FlatId AND A.CancelDate IS NULL AND O.CRMActual=0 AND B.Cancel=0 " +
                              " AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dAsOn) + "' "+
                              " UNION ALL "+
                              " SELECT ISNULL(A.Amount,0) Amount FROM dbo.ReceiptTrans A " +
                              " INNER JOIN dbo.ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                              " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                              " WHERE A.FlatId=LP.PlotDetailsId AND A.CancelDate IS NULL AND O.CRMActual=0 AND B.Cancel=0 " +
                              " AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dAsOn) + "' ";

                if (BsfGlobal.g_bFADB == true)
                {
                    sSql = sSql + " UNION ALL "+
                                  " SELECT (-1*C.OBReceipt)+ISNULL(SUM(A.Amount),0) FROM dbo.ReceiptTrans A " +
                                  " INNER JOIN dbo.ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                  " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                                  " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                  " WHERE A.FlatId=C.FlatId AND A.CancelDate IS NULL AND O.CRMActual=1 AND R.Cancel=0 " +
                                  " AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dAsOn) + "' " +
                                  " UNION ALL " +
                                  " SELECT ISNULL(SUM(A.Amount),0) FROM dbo.ReceiptTrans A " +
                                  " INNER JOIN dbo.ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                  " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                                  " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                  " WHERE A.FlatId=LP.PlotDetailsId AND A.CancelDate IS NULL AND O.CRMActual=1 AND R.Cancel=0 " +
                                  " AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dAsOn) + "' " +
                                  " UNION ALL " +
                                  " SELECT (-1*C.OBReceipt)+ISNULL(SUM(A.Amount),0) FROM dbo.ReceiptTrans A " +
                                  " INNER JOIN dbo.ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                  " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                                  " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                  " WHERE A.FlatId=C.FlatId AND A.CancelDate IS NULL AND O.CRMActual=2 AND R.BRS=1 " +
                                  " AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dAsOn) + "' "+
                                  " UNION ALL " +
                                  " SELECT ISNULL(SUM(A.Amount),0) FROM dbo.ReceiptTrans A " +
                                  " INNER JOIN dbo.ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                  " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                                  " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                  " WHERE A.FlatId=LP.PlotDetailsId AND A.CancelDate IS NULL AND O.CRMActual=2 AND R.BRS=1 " +
                                  " AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dAsOn) + "' " +
                                  " ) A ), 0) Received From dbo.BuyerDetail BD "+
                                  " LEFT JOIN dbo.FlatDetails C ON BD.FlatId=C.FlatId AND BD.LeadId=C.LeadId AND BD.Status=C.Status " +
                                  " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails LP ON BD.PlotId=LP.PlotDetailsId AND BD.LeadId=LP.BuyerId AND BD.Status=LP.Status " +
                                  " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON BD.CostCentreId=OC.CostCentreId " +
                                  " ) A GROUP BY CostCentreId " +
                                  " ) A " +
                                  " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B ON A.ProjectId=B.CostCentreId  " +
                                  " AND B.CostCentreId NOT IN(Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                                  " GROUP BY ProjectId,ProjectDB,B.CostCentreName";
                }
                else
                {
                    sSql = sSql + ") A ),0) Received From BuyerDetail D "+
                                  " LEFT JOIN dbo.FlatDetails C ON D.LeadId=C.LeadId AND D.FlatId=C.FlatId AND D.Status=C.Status " +
                                  " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails LP ON D.LeadId=LP.BuyerId "+
                                  " AND D.PlotId=LP.PlotDetailsId AND D.Status=LP.Status " +
                                  " INNER JOIN dbo.LeadRegister E ON D.LeadId=E.LeadId " +
                                  " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON D.CostCentreId=OC.CostCentreId " +
                                  " ) A GROUP BY CostCentreId  " +
                                  " ) A " +
                                  " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B ON A.ProjectId=B.CostCentreId  " +
                                  " And B.CostCentreId NOT IN(Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                                  " Where UserId=" + BsfGlobal.g_lUserId + ") " +
                                  " GROUP BY ProjectId,ProjectDB,B.CostCentreName";
                }

                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        internal static DataTable Get_Block_Receivable(int arg_iProjectId, DateTime arg_dtAsOn)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "Select CRMReceivable From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CostCentreId=" + arg_iProjectId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();

                int iCRMRecv = 0;
                if (dt.Rows.Count > 0)
                    iCRMRecv = Convert.ToInt32(dt.Rows[0]["CRMReceivable"]);
                else
                    iCRMRecv = 0;
                dt.Dispose();

                decimal dUnit = BsfGlobal.g_iSummaryUnit;

                sSql = "SELECT A.BlockId, B.BlockName,  SUM(AgreementVale)/" + dUnit + " AgreementValue, SUM(Receivable)/" + dUnit + " ReceivableAsOn, " +
                        " SUM(Received)/" + dUnit + " Received, (SUM(Receivable)-SUM(Received))/" + dUnit + " DueAsOn, (SUM(AgreementVale)-SUM(Received))/" + dUnit + " TotalReceivable," +
                        " Case When SUM(Receivable)<>0 Then (SUM(Receivable)-SUM(Received))/SUM(Receivable)*100 Else 0 End [Recv%] " +
                        " FROM ( " +
                        " SELECT BlockId, SUM(NetAmt)+SUM(A.QualifierAmt) AgreementVale, 0 Receivable, 0 Received, 0 Due,0 TotReceivable  " +
                        " FROM FlatDetails A INNER JOIN BuyerDetail B ON A.FlatId=B.FlatId INNER JOIN LeadRegister L ON L.LeadId=B.LeadId" +
                        " WHERE A.LeadId<>0 AND A.CostCentreId=" + arg_iProjectId + " GROUP BY BlockId   " +
                        " UNION ALL   " +
                        " SELECT DISTINCT BlockId,0, SUM(A.NetAmount),0,0,0 FROM ProgressBillRegister A" +
                        " INNER JOIN FlatDetails B ON A.FlatId=B.FlatId " +
                        " INNER JOIN BuyerDetail D ON A.FlatId=D.FlatId INNER JOIN LeadRegister L ON L.LeadId=D.LeadId " +
                        " Left JOIN dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId " +
                        " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                        " WHERE A.CostCentreId=" + arg_iProjectId + " AND A.PBDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' AND S.BillPassed=1 " +
                        " AND A.PBDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                        " GROUP BY B.BlockId  " +
                        " Union All ";

                if (iCRMRecv == 1)
                {
                    sSql = sSql + " SELECT DISTINCT B.BlockId,0, SUM(A.NetAmount),0,0,0 FROM PaymentScheduleFlat A INNER JOIN FlatDetails B ON A.FlatId=B.FlatId  " +
                                   " INNER JOIN BuyerDetail D ON A.FlatId=D.FlatId INNER JOIN LeadRegister L ON L.LeadId=D.LeadId  " +
                                   " INNER JOIN dbo.StageDetails M ON A.StageDetId=M.StageDetId" +
                                   " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                                   " WHERE A.CostCentreId=" + arg_iProjectId + " And A.BillPassed=0 AND A.StageDetId<>0 AND " +
                                   " M.CompletionDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' " +
                                   " AND M.CompletionDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                                   " GROUP BY B.BlockId";
                }
                else
                {
                    sSql = sSql + " SELECT DISTINCT B.BlockId,0, SUM(A.NetAmount),0,0,0 FROM PaymentScheduleFlat A INNER JOIN FlatDetails B ON A.FlatId=B.FlatId  " +
                                   " INNER JOIN BuyerDetail D ON A.FlatId=D.FlatId INNER JOIN LeadRegister L ON L.LeadId=D.LeadId  " +
                                   " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                                   " WHERE A.CostCentreId=" + arg_iProjectId + " And A.BillPassed=0 AND " +
                                   " A.SchDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' " +
                                   " AND A.SchDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                                   " GROUP BY B.BlockId";
                }

                sSql = sSql + " Union All" +
                                " Select BlockId,0,0,Sum(Received)Received ,0,0 From( Select BlockId,(-1*C.OBReceipt)+ISNULL((SELECT SUM(A.Amount) FROM ( " +
                                " SELECT A.Amount FROM ReceiptTrans A  INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                " WHERE A.FlatId=C.FlatId AND A.CancelDate IS NULL AND O.CRMActual=0 AND B.Cancel=0 " +
                                " AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' ";

                if (BsfGlobal.g_bFADB == true)
                {
                    sSql = sSql + " UNION ALL SELECT SUM(A.Amount) FROM ReceiptTrans A " +
                                    " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                    " WHERE A.FlatId=C.FlatId AND A.CancelDate IS NULL AND O.CRMActual=1 AND R.Cancel=0 AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' " +
                                    " UNION ALL " +
                                    " SELECT SUM(A.Amount) FROM ReceiptTrans A " +
                                    " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                    " WHERE A.FlatId=C.FlatId AND A.CancelDate IS NULL AND O.CRMActual=2 AND R.BRS=1 AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' ) A ),0) Received  From FlatDetails C " +
                                    " INNER JOIN BuyerDetail D ON D.LeadId=C.LeadId And C.FlatId=D.FlatId And D.Status=C.Status   " +
                                    " INNER JOIN LeadRegister E ON E.LeadId=D.LeadId Where C.CostCentreId=" + arg_iProjectId + " ) A Group By BlockId  " +
                                    " ) A " +
                                    " INNER JOIN BlockMaster B  ON A.BlockId=B.BlockId GROUP BY A.BlockId, B.BlockName";
                }
                else
                {
                    sSql = sSql + ") A ),0) Received  From FlatDetails C " +
                                    " INNER JOIN BuyerDetail D ON D.LeadId=C.LeadId And C.FlatId=D.FlatId And D.Status=C.Status   " +
                                    " INNER JOIN LeadRegister E ON E.LeadId=D.LeadId Where C.CostCentreId=" + arg_iProjectId + " ) A Group By BlockId  " +
                                    " ) A " +
                                    " INNER JOIN BlockMaster B  ON A.BlockId=B.BlockId GROUP BY A.BlockId, B.BlockName ";
                }

                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        internal static DataTable Get_Flat_Receivable(int argCCId, int arg_iBlockId, DateTime arg_dtAsOn)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;

            try
            {
                String sSql = "Select CRMReceivable, BusinessType From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre A" +
                              " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B ON A.ProjectDB=B.ProjectName" +
                              " Where BusinessType IN('B', 'L') AND CostCentreId=" + argCCId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dtRxble = new DataTable();
                dtRxble.Load(dr);
                dr.Close();
                cmd.Dispose();

                int iCRMRecv = 0;
                string sBusinessType = "";
                if (dtRxble.Rows.Count > 0)
                {
                    iCRMRecv = Convert.ToInt32(dtRxble.Rows[0]["CRMReceivable"]);
                    sBusinessType = CommFun.IsNullCheck(dtRxble.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                }
                dtRxble.Dispose();

                decimal dUnit = BsfGlobal.g_iSummaryUnit;

                if (sBusinessType == "B")
                {
                    sSql = "SELECT A.FlatId,A.FlatNo,BuyerName,A.Type,LR.Mobile,FD.Rate, SUM(AgreementVale)/" + dUnit + " AgreementValue, " +
                            " SUM(Receivable)/" + dUnit + " ReceivableAsOn, SUM(Received)/" + dUnit + " DummyReceived, " +
                            " Case When PSF.WriteOff>0 Then SUM(AgreementVale)/" + dUnit + " Else SUM(Received)/" + dUnit + " End Received, " +
                            " Case When PSF.WriteOff>0 Then SUM(AgreementVale)/" + dUnit + " Else SUM(Received)/" + dUnit + " End ReceivedWriteOff, " +
                            " Case When PSF.WriteOff>0 Then CONVERT(bit, 1, 0) Else CONVERT(bit, 0, 1) End Sel, " +
                            " Case When PSF.WriteOff>0 Then (PSF.WriteOff/" + dUnit + ") Else 0.00 End WriteOff, " +
                            " (SUM(Receivable)-SUM(Received))/" + dUnit + " DueAsOn, (SUM(AgreementVale)-SUM(Received))/" + dUnit + " TotalReceivable," +
                            " Case When SUM(Receivable)<>0 Then (SUM(Receivable)-SUM(Received))/SUM(Receivable)*100 Else 0 End [Recv%] FROM ( " +
                            " SELECT A.FlatId, A.FlatNo, L.LeadName BuyerName, CASE WHEN A.Status='S' THEN 'Buyer' ELSE 'Investor' END Type,  " +
                            " (A.NetAmt+A.QualifierAmt) AgreementVale, 0 Receivable, 0 Received,0.00 ReceivedWriteOff,0.00 WriteOff, 0 Due,0 TotReceivable FROM dbo.FlatDetails A  " +
                            " INNER JOIN dbo.BuyerDetail B ON A.FlatId=B.FlatId    " +
                            " INNER JOIN dbo.LeadRegister L ON L.LeadId=B.LeadId " +
                            " WHERE BlockId=" + arg_iBlockId + " And A.LeadId<>0   " +
                            " UNION ALL   " +
                            " SELECT A.FlatId,B.FlatNo,L.LeadName BuyerName, CASE WHEN B.Status='S' THEN 'Buyer' ELSE 'Investor' END Type, " +
                            " 0 AgreementVale, SUM(A.NetAmount) Receivable,0 Received,0.00 ReceivedWriteOff,0.00 WriteOff,0 Due,0 TotReceivable FROM dbo.ProgressBillRegister A " +
                            " INNER JOIN dbo.FlatDetails B ON A.FlatId=B.FlatId   " +
                            " INNER JOIN dbo.BuyerDetail D ON A.FlatId=D.FlatId " +
                            " INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId  " +
                            " Left JOIN dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId " +
                            " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                            " WHERE B.BlockId=" + arg_iBlockId + " AND A.PBDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' AND S.BillPassed=1 " +
                            " AND A.PBDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                            " GROUP BY A.FlatId,B.FlatNo,L.LeadName, B.Status" +
                            " UNION ALL ";
                }
                else
                {
                    sSql = "SELECT A.FlatId,A.FlatNo,BuyerName,A.Type,LR.Mobile,FD.Rate, SUM(AgreementVale)/" + dUnit + " AgreementValue, " +
                            " SUM(Receivable)/" + dUnit + " ReceivableAsOn, SUM(Received)/" + dUnit + " DummyReceived, "+
                            " SUM(Received)/" + dUnit + " Received, 0.00 ReceivedWriteOff, CONVERT(bit, 0, 1) Sel, 0.00 WriteOff, " +
                            " (SUM(Receivable)-SUM(Received))/" + dUnit + " DueAsOn, (SUM(AgreementVale)-SUM(Received))/" + dUnit + " TotalReceivable," +
                            " Case When SUM(Receivable)<>0 Then (SUM(Receivable)-SUM(Received))/SUM(Receivable)*100 Else 0 End [Recv%] FROM ( " +
                            " SELECT A.PlotDetailsId FlatId, A.PlotNo FlatNo, L.LeadName BuyerName, CASE WHEN A.Status='S' THEN 'Buyer' ELSE 'Investor' END Type,  " +
                            " (A.NetAmount+A.QualifierAmount) AgreementVale, 0 Receivable, 0 Received,0.00 ReceivedWriteOff, 0.00 WriteOff, " +
                            " 0 Due,0 TotReceivable FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A  " +
                            " INNER JOIN dbo.BuyerDetail B ON A.PlotDetailsId=B.PlotId " +
                            " INNER JOIN dbo.LeadRegister L ON L.LeadId=B.LeadId " +
                            " WHERE B.CostCentreId=" + argCCId + " And A.BuyerId<>0   " +
                            " UNION ALL   " +
                            " SELECT DISTINCT A.PlotDetailsId FlatId,B.PlotNo FlatNo,L.LeadName BuyerName, CASE WHEN B.Status='S' THEN 'Buyer' ELSE 'Investor' END Type, " +
                            " 0 AgreementVale, SUM(A.NetAmount) Receivable,0 Received,0.00 ReceivedWriteOff,0.00 WriteOff,0 Due,0 TotReceivable FROM dbo.PlotProgressBillRegister A " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails B ON A.PlotDetailsId=B.PlotDetailsId   " +
                            " INNER JOIN dbo.BuyerDetail D ON A.PlotDetailsId=D.PlotId " +
                            " INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId  " +
                            " Left JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot S On S.PaymentSchId=A.PaySchId " +
                            " LEFT JOIN dbo.AllotmentCancel AC ON A.PlotDetailsId=AC.FlatId AND AC.Approve='Y' " +
                            " WHERE D.CostCentreId=" + argCCId + " AND A.PBDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' AND S.BillPassed=1 " +
                            " AND A.PBDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                            " GROUP BY A.PlotDetailsId,B.PlotNo,L.LeadName,B.Status" +
                            " UNION ALL ";
                }

                if (iCRMRecv == 1)
                {
                    if (sBusinessType == "B")
                    {
                        sSql = sSql + " SELECT DISTINCT A.FlatId,B.FlatNo,L.LeadName BuyerName, CASE WHEN B.Status='S' THEN 'Buyer' ELSE 'Investor' END Type," +
                                      " 0 AgreementVale, SUM(A.NetAmount) Receivable,0 Received,0.00 ReceivedWriteOff,0.00 WriteOff, " +
                                      " 0 Due,0 TotReceivable FROM dbo.PaymentScheduleFlat A " +
                                      " INNER JOIN dbo.FlatDetails B ON A.FlatId=B.FlatId    " +
                                      " INNER JOIN dbo.BuyerDetail D ON A.FlatId=D.FlatId " +
                                      " INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId   " +
                                      " INNER JOIN dbo.StageDetails M ON A.StageDetId=M.StageDetId " +
                                      " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                                      " WHERE B.BlockId=" + arg_iBlockId + " And BillPassed=0 AND A.StageDetId<>0 " +
                                      " AND M.CompletionDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' " +
                                      " AND M.CompletionDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                                      " GROUP BY A.FlatId,B.FlatNo,L.LeadName, B.Status ";
                    }
                    else
                    {
                        sSql = sSql + " SELECT DISTINCT A.PlotDetailsId FlatId,B.PlotNo FlatNo,L.LeadName BuyerName, " +
                                      " CASE WHEN B.Status='S' THEN 'Buyer' ELSE 'Investor' END Type," +
                                      " 0 AgreementVale, SUM(A.NetAmount) Receivable,0 Received,0.00 ReceivedWriteOff,0.00 WriteOff, " +
                                      " 0 Due,0 TotReceivable FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot A " +
                                      " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails B ON A.PlotDetailsId=B.PlotDetailsId    " +
                                      " INNER JOIN BuyerDetail D ON A.PlotDetailsId=D.PlotId " +
                                      " INNER JOIN LeadRegister L ON L.LeadId=D.LeadId   " +
                                      " LEFT JOIN dbo.AllotmentCancel AC ON A.PlotDetailsId=AC.FlatId AND AC.Approve='Y' " +
                                      " WHERE D.CostCentreId=" + argCCId + " And A.BillPassed=0 " +
                                      " AND A.SchDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' " +
                                      " AND A.SchDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                                      " GROUP BY A.PlotDetailsId,B.PlotNo,L.LeadName,B.Status ";
                    }
                }
                else
                {
                    if (sBusinessType == "B")
                    {
                        sSql = sSql + " SELECT DISTINCT A.FlatId,B.FlatNo,L.LeadName BuyerName, CASE WHEN B.Status='S' THEN 'Buyer' ELSE 'Investor' END Type," +
                                      " 0 AgreementVale, SUM(A.NetAmount) Receivable,0 Received,0.00 ReceivedWriteOff,0.00 WriteOff, " +
                                      " 0 Due,0 TotReceivable FROM dbo.PaymentScheduleFlat A " +
                                      " INNER JOIN  FlatDetails B ON A.FlatId=B.FlatId    " +
                                      " INNER JOIN BuyerDetail D ON A.FlatId=D.FlatId  INNER JOIN LeadRegister L ON L.LeadId=D.LeadId   " +
                                      " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                                      " WHERE B.BlockId=" + arg_iBlockId + " And BillPassed=0 " +
                                      " AND A.SchDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' " +
                                      " AND A.SchDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                                      " GROUP BY A.FlatId,B.FlatNo,L.LeadName, B.Status ";
                    }
                    else
                    {
                        sSql = sSql + " SELECT DISTINCT A.PlotDetailsId FlatId,B.PlotNo FlatNo,L.LeadName BuyerName, " +
                                      " CASE WHEN B.Status='S' THEN 'Buyer' ELSE 'Investor' END Type," +
                                      " 0 AgreementVale, SUM(A.NetAmount) Receivable,0 Received,0.00 ReceivedWriteOff,0.00 WriteOff, " +
                                      " 0 Due,0 TotReceivable FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot A " +
                                      " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails B ON A.PlotDetailsId=B.PlotDetailsId    " +
                                      " INNER JOIN BuyerDetail D ON A.PlotDetailsId=D.PlotId " +
                                      " INNER JOIN LeadRegister L ON L.LeadId=D.LeadId   " +
                                      " LEFT JOIN dbo.AllotmentCancel AC ON A.PlotDetailsId=AC.FlatId AND AC.Approve='Y' " +
                                      " WHERE D.CostCentreId=" + argCCId + " And A.BillPassed=0 " +
                                      " AND A.SchDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' " +
                                      " AND A.SchDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                                      " GROUP BY A.PlotDetailsId,B.PlotNo,L.LeadName,B.Status ";
                    }
                }

                if (sBusinessType == "B")
                {
                    sSql = sSql + " UNION ALL " +
                                   " Select BD.FlatId, C.FlatNo, LR.LeadName BuyerName, CASE WHEN BD.Status='S' THEN 'Buyer' ELSE 'Investor' END Type," +
                                   " 0 AgreementVale, 0 Receivable, ISNULL((SELECT (-1*OBReceipt)+ISNULL(SUM(A.Amount),0) FROM ( " +
                                   " SELECT ISNULL(A.Amount,0) Amount FROM dbo.ReceiptTrans A " +
                                   " INNER JOIN dbo.ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                   " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                   " WHERE A.FlatId=C.FlatId AND A.CancelDate IS NULL AND O.CRMActual=0 AND B.Cancel=0 " +
                                   " AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' ";
                }
                else
                {
                    sSql = sSql + " UNION ALL " +
                                   " Select BD.PlotId FlatId, LP.PlotNo FlatNo, LR.LeadName BuyerName, CASE WHEN BD.Status='S' THEN 'Buyer' ELSE 'Investor' END Type," +
                                   " 0 AgreementVale, 0 Receivable, ISNULL((SELECT ISNULL(SUM(A.Amount),0) FROM ( " +
                                   " SELECT ISNULL(A.Amount,0) Amount FROM dbo.ReceiptTrans A " +
                                   " INNER JOIN dbo.ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                   " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                   " WHERE A.FlatId=LP.PlotDetailsId AND A.CancelDate IS NULL AND O.CRMActual=0 AND B.Cancel=0 " +
                                   " AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' ";
                }

                if (BsfGlobal.g_bFADB == true)
                {
                    if (sBusinessType == "B")
                    {
                        sSql = sSql + " UNION ALL " +
                                      " SELECT ISNULL(SUM(RT.Amount),0) FROM dbo.ReceiptTrans RT " +
                                      " INNER JOIN dbo.ReceiptRegister B ON RT.ReceiptId=B.ReceiptId " +
                                      " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                                      " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=RT.CostCentreId " +
                                      " WHERE RT.FlatId=C.FlatId AND RT.CancelDate IS NULL AND O.CRMActual=1 AND R.Cancel=0 AND " +
                                      " B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' " +
                                      " UNION ALL " +
                                      " SELECT ISNULL(SUM(RT.Amount),0) FROM dbo.ReceiptTrans RT " +
                                      " INNER JOIN dbo.ReceiptRegister B ON RT.ReceiptId=B.ReceiptId " +
                                      " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                                      " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=RT.CostCentreId " +
                                      " WHERE RT.FlatId=C.FlatId AND RT.CancelDate IS NULL AND O.CRMActual=2 AND R.BRS=1 AND " +
                                      " B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' " +
                                      " ) A ),0) Received, 0.00 ReceivedWriteOff, 0.00 WriteOff,0,0 from dbo.BuyerDetail BD " +
                                      " INNER JOIN dbo.FlatDetails C ON BD.FlatId=C.FlatId AND BD.LeadId=C.LeadId AND BD.Status=C.Status " +
                                      " INNER JOIN dbo.LeadRegister LR ON BD.LeadId=LR.LeadId " +
                                      " Where BlockId=" + arg_iBlockId + " AND BD.CostCentreId=" + argCCId + " " +
                                      " ) A " +
                                      " INNER JOIN dbo.FlatDetails FD On FD.FlatId=A.FlatId " +
                                      " INNER JOIN dbo.BlockMaster BM On BM.BlockId=FD.BlockId " +
                                      " INNER JOIN dbo.LevelMaster LM ON LM.LevelId=FD.LevelId " +
                                      " INNER JOIN dbo.LeadRegister LR ON LR.LeadId=FD.LeadId " +
                                      " LEFT JOIN dbo.PaymentScheduleFlat PSF ON PSF.FlatId=FD.FlatId AND PSF.WriteOff<>0 " +
                                      " Where A.FlatNo IS NOT NULL " +
                                      " GROUP BY BM.SortOrder,LM.SortOrder,FD.SortOrder,A.FlatId,A.FlatNo,BuyerName,A.Type,LR.Mobile,FD.Rate,A.WriteOff,A.ReceivedWriteOff,PSF.WriteOff " +
                                      " Order By BM.SortOrder,LM.SortOrder,FD.SortOrder,dbo.Val(A.FlatNo) ";
                    }
                    else
                    {
                        sSql = sSql + " UNION ALL " +
                                      " SELECT ISNULL(SUM(RT.Amount),0) FROM dbo.ReceiptTrans RT " +
                                      " INNER JOIN dbo.ReceiptRegister B ON RT.ReceiptId=B.ReceiptId " +
                                      " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                                      " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=RT.CostCentreId " +
                                      " WHERE RT.FlatId=LP.PlotDetailsId AND RT.CancelDate IS NULL AND O.CRMActual=1 AND R.Cancel=0 AND " +
                                      " B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' " +
                                      " UNION ALL " +
                                      " SELECT ISNULL(SUM(RT.Amount),0) FROM dbo.ReceiptTrans RT " +
                                      " INNER JOIN dbo.ReceiptRegister B ON RT.ReceiptId=B.ReceiptId " +
                                      " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                                      " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=RT.CostCentreId " +
                                      " WHERE RT.FlatId=LP.PlotDetailsId AND RT.CancelDate IS NULL AND O.CRMActual=2 AND R.BRS=1 AND " +
                                      " B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' " +
                                      " ) A ),0) Received, 0.00 ReceivedWriteOff, 0.00 WriteOff,0,0 from dbo.BuyerDetail BD " +
                                      " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails LP ON BD.PlotId=LP.PlotDetailsId AND BD.LeadId=LP.BuyerId AND BD.Status=LP.Status " +
                                      " INNER JOIN dbo.LeadRegister LR ON BD.LeadId=LR.LeadId " +
                                      " Where BD.CostCentreId=" + argCCId + " " +
                                      " ) A " +
                                      " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails FD On FD.PlotDetailsId=A.FlatId " +
                                      " INNER JOIN dbo.LeadRegister LR ON LR.LeadId=FD.BuyerId " +
                                      " Where A.FlatNo IS NOT NULL " +
                                      " GROUP BY A.FlatId,A.FlatNo,BuyerName,A.Type,LR.Mobile,FD.Rate,A.WriteOff,A.ReceivedWriteOff " +
                                      " ORDER BY A.FlatNo,dbo.Val(A.FlatNo) ";
                    }
                }
                else
                {
                    if (sBusinessType == "B")
                    {
                        sSql = sSql + ") A ),0) Received,0.00 ReceivedWriteOff,0.00 WriteOff,0 Due,0 TotReceivable from dbo.BuyerDetail BD " +
                                      " INNER JOIN dbo.FlatDetails C ON BD.FlatId=C.FlatId AND BD.LeadId=C.LeadId AND BD.Status=C.Status AND BlockId=" + arg_iBlockId + " " +
                                      " INNER JOIN dbo.LeadRegister LR ON LR.LeadId=BD.LeadId " +
                                      " Where BD.CostCentreId=" + argCCId + "" +
                                      " ) A " +
                                      " INNER JOIN dbo.FlatDetails FD On FD.FlatId=A.FlatId " +
                                      " INNER JOIN dbo.BlockMaster BM On BM.BlockId=FD.BlockId " +
                                      " INNER JOIN dbo.LevelMaster LM ON LM.LevelId=FD.LevelId " +
                                      " INNER JOIN dbo.LeadRegister LR ON LR.LeadId=FD.LeadId " +
                                      " LEFT JOIN dbo.PaymentScheduleFlat PSF ON PSF.FlatId=FD.FlatId AND PSF.WriteOff<>0 " +
                                      " Where A.FlatNo IS NOT NULL " +
                                      " GROUP BY BM.SortOrder,LM.SortOrder,FD.SortOrder,A.FlatId,A.FlatNo,BuyerName,A.Type,LR.Mobile,FD.Rate,PSF.WriteOff,A.ReceivedWriteOff " +
                                      " Order By BM.SortOrder,LM.SortOrder,FD.SortOrder,dbo.Val(A.FlatNo) ";
                    }
                    else
                    {
                        sSql = sSql + ") A ),0) Received,0.00 ReceivedWriteOff,0.00 WriteOff,0 Due,0 TotReceivable from dbo.BuyerDetail BD " +
                                         " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails LP ON BD.PlotId=LP.PlotDetailsId AND BD.LeadId=LP.BuyerId And BD.Status=LP.Status " +
                                         " INNER JOIN dbo.LeadRegister LR ON LR.LeadId=BD.LeadId " +
                                         " Where BD.CostCentreId=" + argCCId + "" +
                                         " ) A " +
                                         " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails FD On FD.PlotDetailsId=A.FlatId " +
                                         " INNER JOIN dbo.LeadRegister LR ON LR.LeadId=FD.BuyerId " +
                                         " Where A.FlatNo IS NOT NULL " +
                                         " GROUP BY A.FlatId,A.FlatNo,BuyerName,A.Type,LR.Mobile,FD.Rate,A.WriteOff,A.ReceivedWriteOff " +
                                         " ORDER BY A.FlatNo,dbo.Val(A.FlatNo) ";
                    }
                }
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        internal static DataTable Get_Flat_Receivable_WithInterest(int argCCId, int arg_iBlockId, DateTime arg_dtAsOn)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "Select CRMReceivable From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CostCentreId=" + argCCId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();

                int iCRMRecv;
                if (dt.Rows.Count > 0)
                    iCRMRecv = Convert.ToInt32(dt.Rows[0]["CRMReceivable"]);
                else
                    iCRMRecv = 0;

                string sCond = string.Empty;
                if (iCRMRecv == 1)
                    sCond = "AND A.StageDetId<>0";
                else
                    sCond = "";
                dt.Dispose();

                decimal dUnit = BsfGlobal.g_iSummaryUnit;

                sSql = "Select InterestBasedOn From dbo.ProjectInfo Where CostCentreId=" + argCCId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();

                string c_InterestBasedOn = "";
                if (dt.Rows.Count > 0)
                    c_InterestBasedOn = dt.Rows[0]["InterestBasedOn"].ToString();
                else
                    c_InterestBasedOn = "";
                dt.Dispose();

                sSql = "SELECT A.FlatId,A.FlatNo ,BuyerName,A.Type,LR.Mobile,FD.Rate, SUM(AgreementVale)/" + dUnit + " AgreementValue, " +
                        " SUM(Receivable)/" + dUnit + " ReceivableAsOn, SUM(Received)/" + dUnit + " DummyReceived, "+
                        " Case When PSF.WriteOff>0 Then SUM(AgreementVale)/" + dUnit + " Else SUM(Received)/" + dUnit + " End Received, " +
                        " Case When PSF.WriteOff>0 Then SUM(AgreementVale)/" + dUnit + " Else SUM(Received)/" + dUnit + " End ReceivedWriteOff, " +
                        " Case When PSF.WriteOff>0 Then CONVERT(bit, 1, 0) Else CONVERT(bit, 0, 1) End Sel, " +
                        " Case When PSF.WriteOff>0 Then (PSF.WriteOff/" + dUnit + ") Else 0.00 End WriteOff, " +
                        " (SUM(Receivable)-SUM(Received))/" + dUnit + " DueAsOn, (SUM(AgreementVale)-SUM(Received))/" + dUnit + " TotalReceivable," +
                        " Case When SUM(Receivable)<>0 Then (SUM(Receivable)-SUM(Received))/SUM(Receivable)*100 Else 0 End [Recv%],0.000 Interest FROM ( " +
                        " SELECT A.FlatId, A.FlatNo, L.LeadName BuyerName, CASE WHEN A.Status='S' THEN 'Buyer' ELSE 'Investor' END Type,  " +
                        " A.NetAmt+A.QualifierAmt AgreementVale, 0 Receivable, 0 Received, 0 Due,0 TotReceivable FROM dbo.FlatDetails A  " +
                        " INNER JOIN dbo.BuyerDetail B ON A.FlatId=B.FlatId    " +
                        " INNER JOIN dbo.LeadRegister L ON L.LeadId=B.LeadId WHERE BlockId=" + arg_iBlockId + " And A.LeadId<>0   " +
                        " UNION ALL   " +
                        " SELECT DISTINCT A.FlatId,B.FlatNo,L.LeadName BuyerName, CASE WHEN B.Status='S' THEN 'Buyer' ELSE 'Investor' END Type,0,   " +
                        " SUM(A.NetAmount),0,0,0 FROM dbo.ProgressBillRegister A INNER JOIN dbo.FlatDetails B ON A.FlatId=B.FlatId   " +
                        " INNER JOIN dbo.BuyerDetail D ON A.FlatId=D.FlatId INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId  " +
                        " Left JOIN dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId " +
                        " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                        " WHERE B.BlockId=" + arg_iBlockId + " AND A.PBDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' AND S.BillPassed=1 " +
                        " AND A.PBDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                        " GROUP BY A.FlatId,B.FlatNo,L.LeadName, B.Status" +
                        " UNION ALL";

                if (iCRMRecv == 1)
                {
                    sSql = sSql + " SELECT DISTINCT A.FlatId,B.FlatNo,L.LeadName BuyerName, CASE WHEN B.Status='S' THEN 'Buyer' ELSE 'Investor' END Type,0,    " +
                                 " SUM(A.NetAmount),0,0,0 FROM dbo.PaymentScheduleFlat A INNER JOIN  dbo.FlatDetails B ON A.FlatId=B.FlatId    " +
                                 " INNER JOIN dbo.BuyerDetail D ON A.FlatId=D.FlatId  INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId   " +
                                 " INNER JOIN dbo.StageDetails M ON A.StageDetId=M.StageDetId " +
                                 " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                                 " WHERE B.BlockId=" + arg_iBlockId + " And A.BillPassed=0 AND A.StageDetId<>0 AND " +
                                 " M.CompletionDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' " + 
                                 " AND M.CompletionDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                                 " GROUP BY A.FlatId,B.FlatNo,L.LeadName, B.Status";
                }
                else
                {
                    sSql = sSql + " SELECT DISTINCT A.FlatId,B.FlatNo,L.LeadName BuyerName, CASE WHEN B.Status='S' THEN 'Buyer' ELSE 'Investor' END Type,0,    " +
                                 " SUM(A.NetAmount),0,0,0 FROM dbo.PaymentScheduleFlat A INNER JOIN  dbo.FlatDetails B ON A.FlatId=B.FlatId    " +
                                 " INNER JOIN dbo.BuyerDetail D ON A.FlatId=D.FlatId  INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId   " +
                                 " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                                 " WHERE B.BlockId=" + arg_iBlockId + " And A.BillPassed=0 AND " +
                                 " A.SchDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' " + sCond +
                                 " AND A.SchDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                                 " GROUP BY A.FlatId,B.FlatNo,L.LeadName, B.Status";
                }

                sSql = sSql + " UNION ALL " +
                             " Select D.FlatId,C.FlatNo,E.LeadName BuyerName, CASE WHEN D.Status='S' THEN 'Buyer' ELSE 'Investor' END Type,0,0,   " +
                             " (-1 *OBReceipt)+ISNULL((SELECT SUM(A.Amount) FROM ( " +
                             " SELECT A.Amount FROM dbo.ReceiptTrans A INNER JOIN dbo.ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                             " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                             " WHERE A.FlatId=C.FlatId AND A.CancelDate IS NULL AND O.CRMActual=0 AND B.Cancel=0 " +
                             " AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' ";

                if (BsfGlobal.g_bFADB == true)
                {
                    sSql = sSql + " UNION ALL SELECT SUM(A.Amount) FROM dbo.ReceiptTrans A "+
                                    " INNER JOIN dbo.ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                    " WHERE A.FlatId=C.FlatId AND A.CancelDate IS NULL AND O.CRMActual=1 AND R.Cancel=0 "+
                                    " AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' " +
                                    " UNION ALL " +
                                    " SELECT SUM(A.Amount) FROM dbo.ReceiptTrans A "+
                                    " INNER JOIN dbo.ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                    " WHERE A.FlatId=C.FlatId AND A.CancelDate IS NULL AND O.CRMActual=2 AND R.BRS=1 "+
                                    " AND B.ReceiptDate<='" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' ) A ),0) Received,0,0  from dbo.FlatDetails C " +
                                    " INNER JOIN dbo.BuyerDetail D ON D.LeadId=C.LeadId And C.FlatId=D.FlatId And D.Status=C.Status   " +
                                    " INNER JOIN dbo.LeadRegister E ON E.LeadId=D.LeadId Where BlockId=" + arg_iBlockId + " " +
                                    " ) A " +
                                    " INNER JOIN dbo.FlatDetails FD On FD.FlatId=A.FlatId " +
                                    " INNER JOIN dbo.BlockMaster BM On BM.BlockId=FD.BlockId " +
                                    " INNER JOIN dbo.LevelMaster LM ON LM.LevelId=FD.LevelId " +
                                    " INNER JOIN dbo.LeadRegister LR ON LR.LeadId=FD.LeadId " +
                                    " LEFT JOIN dbo.PaymentScheduleFlat PSF ON PSF.FlatId=FD.FlatId AND PSF.WriteOff<>0 " +
                                    " GROUP BY BM.SortOrder,LM.SortOrder,FD.SortOrder,A.FlatId,A.FlatNo,BuyerName,A.Type,LR.Mobile,FD.Rate,PSF.WriteOff " +
                                    " Order By BM.SortOrder,LM.SortOrder,FD.SortOrder,dbo.Val(A.FlatNo) ";
                }
                else
                {
                    sSql = sSql + ") A ),0) Received,0,0  from dbo.FlatDetails C " +
                                    " INNER JOIN dbo.BuyerDetail D ON D.LeadId=C.LeadId And C.FlatId=D.FlatId And D.Status=C.Status   " +
                                    " INNER JOIN dbo.LeadRegister E ON E.LeadId=D.LeadId Where BlockId=" + arg_iBlockId + " " +
                                    " ) A " +
                                    " INNER JOIN dbo.FlatDetails FD On FD.FlatId=A.FlatId " +
                                    " INNER JOIN dbo.BlockMaster BM On BM.BlockId=FD.BlockId " +
                                    " INNER JOIN dbo.LevelMaster LM ON LM.LevelId=FD.LevelId " +
                                    " INNER JOIN dbo.LeadRegister LR ON LR.LeadId=FD.LeadId " +
                                    " GROUP BY BM.SortOrder,LM.SortOrder,FD.SortOrder,A.FlatId,A.FlatNo,BuyerName,A.Type,LR.Mobile,FD.Rate  " +
                                    " Order By BM.SortOrder,LM.SortOrder,FD.SortOrder,dbo.Val(A.FlatNo) ";
                    //" GROUP BY A.FlatId,A.FlatNo,BuyerName,A.Type";
                }
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                cmd.Dispose();

                sSql = "Update dbo.ReceiptTrans Set NetAmount=Isnull((Select IsNull(NetAmount,0) From dbo.PaymentScheduleFlat Where PaymentSchId=ReceiptTrans.PaySchId),0) Where NetAmount=0";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                if (c_InterestBasedOn == "B")
                {
                    sSql = " Select FlatId,PaymentSchId,SortOrder,[Date],[Description],AsOnDate,Receivable,Received,CreditDays,IntPercent,FinaliseDate,[Type],0.00 Interest,0 Age FROM( " +
                             " Select DISTINCT A.FlatId,S.PaymentSchId,S.SortOrder,A.PBDate [Date],A.AsOnDate,S.[Description],A.NetAmount Receivable,0 Received,D.CreditDays,D.IntPercent,E.FinaliseDate,'P' [Type] " +
                             " From dbo.ProgressBillRegister A INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                             " INNER JOIN dbo.ProgressBillMaster M On M.ProgRegId=A.ProgRegId INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId " +
                             " Left JOIN dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId " +
                             " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                             " Where M.Approve='Y' And S.BillPassed=1 And A.PBDate<='" + arg_dtAsOn.ToString("dd-MMM-yyyy") + "'" +
                             " AND A.PBDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                             " AND A.CostCentreId=" + argCCId + " AND D.BlockId=" + arg_iBlockId + " " +
                             " UNION ALL " +
                             " Select DISTINCT A.FlatId,S.PaymentSchId,S.SortOrder,RR.ReceiptDate [Date],NULL AsOnDate,RR.Narration [Description],0 Receivable,RT.Amount Received,D.CreditDays,D.IntPercent,E.FinaliseDate,'R' [Type] " +
                             " From dbo.ProgressBillRegister A INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                             " INNER JOIN dbo.ProgressBillMaster M On M.ProgRegId=A.ProgRegId INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId " +
                             " Left JOIN dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId INNER JOIN dbo.LeadRegister L ON L.LeadId=D.LeadId " +
                             " INNER JOIN dbo.ReceiptTrans RT ON RT.PaySchId=S.PaymentSchId " +
                             " INNER JOIN dbo.ReceiptRegister RR ON RR.ReceiptId=RT.ReceiptId And RR.ReceiptDate<='" + arg_dtAsOn.ToString("dd-MMM-yyyy") + "' " +
                             " Where M.Approve='Y' And S.BillPassed=1 And A.PBDate<='" + arg_dtAsOn.ToString("dd-MMM-yyyy") + "'" +
                             " AND A.CostCentreId=" + argCCId + " AND D.BlockId=" + arg_iBlockId + " AND RT.CancelDate IS NULL " +
                             " ) X Order By X.SortOrder,X.[Type],X.[Date]";
                }
                else
                {
                    sSql = " Select FlatId,PaymentSchId,SortOrder,[Date],[Description],Receivable,Received,CreditDays,IntPercent,FinaliseDate,[Type],0.00 Interest,0 Age FROM( " +
                             " Select DISTINCT A.FlatId,A.PaymentSchId,A.SortOrder,F.CompletionDate [Date],A.[Description],A.NetAmount Receivable,0 Received,D.CreditDays,D.IntPercent,E.FinaliseDate,'P' [Type] " +
                             " From dbo.PaymentScheduleFlat A INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                             " INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId" +
                             " INNER JOIN dbo.StageDetails F ON F.StageDetId=A.StageDetId" +
                             " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                             " Where A.StageDetId>0 And A.SchDate<='" + arg_dtAsOn.ToString("dd-MMM-yyyy") + "'" +
                             " AND F.CompletionDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                             " AND A.CostCentreId=" + argCCId + " AND D.BlockId=" + arg_iBlockId + " " +
                             " UNION ALL" +
                             " Select A.FlatId,A.PaymentSchId,A.SortOrder,RR.ReceiptDate [Date],RR.Narration [Description],0 Receivable,RT.Amount Received,D.CreditDays,D.IntPercent,E.FinaliseDate,'R' [Type] " +
                             " From dbo.PaymentScheduleFlat A INNER JOIN dbo.FlatDetails D On A.FlatId=D.FlatId " +
                             " INNER JOIN dbo.BuyerDetail E ON D.FlatId=E.FlatId " +
                             " INNER JOIN dbo.StageDetails F ON F.StageDetId=A.StageDetId" +
                             " INNER JOIN dbo.ReceiptTrans RT ON RT.PaySchId=A.PaymentSchId " +
                             " INNER JOIN dbo.ReceiptRegister RR ON RR.ReceiptId=RT.ReceiptId And RR.ReceiptDate<='" + arg_dtAsOn.ToString("dd-MMM-yyyy") + "' " +
                             " Where A.StageDetId>0 And A.SchDate<='" + arg_dtAsOn.ToString("dd-MMM-yyyy") + "'" +
                             " AND A.CostCentreId=" + argCCId + " AND D.BlockId=" + arg_iBlockId + " AND RT.CancelDate IS NULL " +
                             " ) X Order By X.SortOrder,X.[Type],X.[Date]";
                }

                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtStageComp = new DataTable();
                sda.Fill(dtStageComp);
                cmd.Dispose();

                DataTable dtInterest = new DataTable();
                dtInterest = dtStageComp.Clone();

                #region for Interest Calculation

                for (int i = 0; i < dtStageComp.Rows.Count; i++)
                {
                    string sType = Convert.ToString(dtStageComp.Rows[i]["Type"]);

                    if (sType == "P")
                    {
                        DataRow dRow = dtInterest.NewRow();
                        dRow["FlatId"] = dtStageComp.Rows[i]["FlatId"];
                        dRow["PaymentSchId"] = dtStageComp.Rows[i]["PaymentSchId"];
                        int iSortOrder = Convert.ToInt32(dtStageComp.Rows[i]["SortOrder"]);
                        dRow["SortOrder"] = iSortOrder;
                        dRow["IntPercent"] = Convert.ToDecimal(dtStageComp.Rows[i]["IntPercent"]);
                        dRow["Interest"] = Convert.ToDecimal(dtStageComp.Rows[i]["Interest"]);
                        dRow["Age"] = Convert.ToDecimal(dtStageComp.Rows[i]["Age"]);

                        DateTime dCompletionDate = Convert.ToDateTime(CommFun.IsNullCheck(dtStageComp.Rows[i]["Date"], CommFun.datatypes.VarTypeDate));
                        dRow["Date"] = dCompletionDate;

                        dRow["Description"] = dtStageComp.Rows[i]["Description"];
                        decimal dReceivale = Convert.ToDecimal(dtStageComp.Rows[i]["Receivable"]);
                        dRow["Receivable"] = dReceivale;
                        dRow["Received"] = Convert.ToDecimal(dtStageComp.Rows[i]["Received"]);
                        dRow["CreditDays"] = dtStageComp.Rows[i]["CreditDays"];

                        DateTime dFinalisedDate = Convert.ToDateTime(CommFun.IsNullCheck(dtStageComp.Rows[i]["FinaliseDate"], CommFun.datatypes.VarTypeDate));
                        dRow["FinaliseDate"] = dFinalisedDate;

                        dRow["Type"] = dtStageComp.Rows[i]["Type"];

                        dtInterest.Rows.Add(dRow);

                        string sPayDesc = dtStageComp.Rows[i]["Description"].ToString();
                        int iCreditDays = Convert.ToInt32(dtStageComp.Rows[i]["CreditDays"]);
                        int iFlatId = Convert.ToInt32(dtStageComp.Rows[i]["FlatId"]);
                        int iPaySchId = Convert.ToInt32(dtStageComp.Rows[i]["PaymentSchId"]);
                        decimal dIntPer = Convert.ToDecimal(dtStageComp.Rows[i]["IntPercent"]);

                        DateTime dInterestDate = arg_dtAsOn;
                        if (c_InterestBasedOn == "S")
                        {
                            if (dCompletionDate > dFinalisedDate)
                                dInterestDate = dCompletionDate;
                            else
                                dInterestDate = dFinalisedDate;
                        }
                        else
                        {
                            dInterestDate = dCompletionDate;
                        }

                        DataView dv = new DataView(dtStageComp) { RowFilter = String.Format("PaymentSchId={0} AND Type='R'", iPaySchId) };
                        DataTable dtRec = new DataTable();
                        dtRec = dv.ToTable();

                        decimal dReceivable = Convert.ToDecimal(dtStageComp.Rows[i]["Receivable"]);
                        decimal dBalance = dReceivable;
                        DateTime dCalInterestDate = dInterestDate;

                        if (dtRec.Rows.Count == 0)
                            dInterestDate = dInterestDate.AddDays(iCreditDays);
                        else
                        {
                            for (int j = 0; j < dtRec.Rows.Count; j++)
                            {
                                dRow = dtInterest.NewRow();

                                dRow["FlatId"] = dtRec.Rows[j]["FlatId"];
                                dRow["PaymentSchId"] = dtRec.Rows[j]["PaymentSchId"];
                                dRow["SortOrder"] = dtRec.Rows[j]["SortOrder"];
                                dRow["Date"] = dtRec.Rows[j]["Date"];
                                dRow["Description"] = dtRec.Rows[j]["Description"];
                                dRow["IntPercent"] = Convert.ToInt32(dtRec.Rows[j]["IntPercent"]);

                                decimal dReceived = Convert.ToDecimal(dtRec.Rows[j]["Received"]);
                                dRow["Received"] = dReceived;
                                dRow["Receivable"] = Convert.ToDecimal(dtRec.Rows[j]["Receivable"]);

                                dRow["CreditDays"] = dtRec.Rows[j]["CreditDays"];
                                dRow["FinaliseDate"] = dtRec.Rows[j]["FinaliseDate"];
                                dRow["Type"] = dtRec.Rows[j]["Type"];

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
                                dRow["Age"] = iDays;

                                decimal dInterest = 0;
                                if (dBalance == 0)
                                    dInterest = 0;
                                else if (iDays == 0)
                                    dInterest = 0;
                                else if (dIntPer == 0)
                                    dInterest = 0;
                                else
                                    dInterest = decimal.Round((dBalance * dIntPer / 36500) * iDays, 3);
                                dRow["Interest"] = dInterest;

                                dBalance = dBalance - dReceived;
                                if (dBalance < 0) { dBalance = 0; }
                                //dRow["Balance"] = dBalance;

                                dtInterest.Rows.Add(dRow);

                                dInterestDate = dRecdDate;
                            }
                        }

                        if (dBalance > 0)
                        {
                            dRow = dtInterest.NewRow();
                            dRow["FlatId"] = iFlatId;
                            dRow["PaymentSchId"] = iPaySchId;
                            dRow["Date"] = arg_dtAsOn.ToString("dd-MMM-yyyy");
                            dRow["Description"] = "Interest Payable (" + sPayDesc + ") As On -" + arg_dtAsOn.ToShortDateString();
                            dRow["CreditDays"] = iCreditDays;
                            dRow["IntPercent"] = dIntPer;
                            dRow["SortOrder"] = iSortOrder;
                            dRow["Receivable"] = dReceivable;
                            dRow["Received"] = Convert.ToDecimal(dtStageComp.Rows[i]["Received"]);
                            dRow["Type"] = dtStageComp.Rows[i]["Type"];

                            //dRow["Balance"] = dBalance;

                            //DateTime dPBAsOnDate = dCompletionDate;
                            //if (c_InterestBasedOn == "B")
                            //{
                            //    if (dtStageComp.Rows[i]["AsOnDate"] == DBNull.Value)
                            //        dPBAsOnDate = DateTime.Now;
                            //    else
                            //        dPBAsOnDate = Convert.ToDateTime(CommFun.IsNullCheck(dtStageComp.Rows[i]["AsOnDate"], CommFun.datatypes.VarTypeDate));
                            //}

                            TimeSpan ts = arg_dtAsOn - dInterestDate;
                            int iDays = ts.Days;
                            if (iDays < 0) { iDays = 0; }
                            dRow["Age"] = iDays;

                            decimal dInterest = 0;
                            if (dBalance == 0)
                                dInterest = 0;
                            else if (iDays == 0)
                                dInterest = 0;
                            else if (dIntPer == 0)
                                dInterest = 0;
                            else
                                dInterest = decimal.Round((dBalance * dIntPer / 36500) * iDays, 3);
                            dRow["Interest"] = dInterest;

                            dtInterest.Rows.Add(dRow);
                        }

                        dBalance = 0;
                    }

                }

                #endregion

                #region Old Calc

                //for (int i = 0; i < dtStageComp.Rows.Count; i++)
                //{
                //    DateTime dFinaliseDate = Convert.ToDateTime(CommFun.IsNullCheck(dtStageComp.Rows[i]["FinaliseDate"], CommFun.datatypes.VarTypeDate));
                //    decimal dIntPer = Convert.ToDecimal(CommFun.IsNullCheck(dtStageComp.Rows[i]["IntPercent"], CommFun.datatypes.vartypenumeric));
                //    int iCreditDays = Convert.ToInt32(CommFun.IsNullCheck(dtStageComp.Rows[i]["CreditDays"], CommFun.datatypes.vartypenumeric));
                //    decimal dReceivable = Convert.ToDecimal(CommFun.IsNullCheck(dtStageComp.Rows[i]["Amount"], CommFun.datatypes.vartypenumeric));

                //    int iBillPassed = Convert.ToInt32(CommFun.IsNullCheck(dtStageComp.Rows[i]["BillPassed"], CommFun.datatypes.vartypenumeric));
                //    if (iBillPassed == 1)
                //    {
                //        DateTime dPBDate = Convert.ToDateTime(CommFun.IsNullCheck(dtStageComp.Rows[i]["PBDate"], CommFun.datatypes.VarTypeDate));
                //        DateTime dPBAsOnDate = Convert.ToDateTime(CommFun.IsNullCheck(dtStageComp.Rows[i]["AsOnDate"], CommFun.datatypes.VarTypeDate));

                //        DateTime dRecpDate;
                //        if (dtStageComp.Rows[i]["ReceiptDate"].ToString().Trim() == "" || dtStageComp.Rows[i]["ReceiptDate"].ToString().ToUpper() == "NULL")
                //            dRecpDate = arg_dtAsOn;
                //        else if (Convert.ToDateTime(dtStageComp.Rows[i]["ReceiptDate"]) == null || Convert.ToDateTime(dtStageComp.Rows[i]["ReceiptDate"]) == DateTime.MinValue)
                //            dRecpDate = arg_dtAsOn;
                //        else
                //            dRecpDate = Convert.ToDateTime(CommFun.IsNullCheck(dtStageComp.Rows[i]["ReceiptDate"], CommFun.datatypes.VarTypeDate));

                //        DateTime dInterestDate;
                //        if (dFinaliseDate >= dPBDate)
                //            dInterestDate = dFinaliseDate;
                //        else
                //            dInterestDate = dPBDate;
                //        dInterestDate = dInterestDate.AddDays(iCreditDays);

                //        int iAsonDays = 0;
                //        TimeSpan ts1 = dPBAsOnDate - dPBDate;
                //        iAsonDays = ts1.Days;

                //        int iFlatId = Convert.ToInt32(CommFun.IsNullCheck(dtStageComp.Rows[i]["FlatId"], CommFun.datatypes.vartypenumeric));

                //        TimeSpan ts2 = dRecpDate - dInterestDate;
                //        int iDays = ts2.Days + iAsonDays;
                //        if (iDays < 0) { iDays = 0; }

                //        decimal dIntAmt = 0;
                //        if (dIntPer == 0)
                //            dIntAmt = 0;
                //        else if (iDays == 0)
                //            dIntAmt = 0;
                //        else if (dReceivable == 0)
                //            dIntAmt = 0;
                //        else
                //            dIntAmt = decimal.Round((dReceivable * dIntPer / 36500) * iDays, 3);

                //        dtStageComp.Rows[i]["Interest"] = dIntAmt;
                //        dtStageComp.Rows[i]["Age"] = iDays;
                //    }
                //    else
                //    {
                //        DateTime dStageDate = Convert.ToDateTime(CommFun.IsNullCheck(dtStageComp.Rows[i]["StageDate"], CommFun.datatypes.VarTypeDate));

                //        DateTime dRecpDate;
                //        if (dtStageComp.Rows[i]["ReceiptDate"].ToString().Trim() == "" || dtStageComp.Rows[i]["ReceiptDate"].ToString().ToUpper() == "NULL")
                //            dRecpDate = arg_dtAsOn;
                //        else if (Convert.ToDateTime(dtStageComp.Rows[i]["ReceiptDate"]) == null || Convert.ToDateTime(dtStageComp.Rows[i]["ReceiptDate"]) == DateTime.MinValue)
                //            dRecpDate = arg_dtAsOn;
                //        else
                //            dRecpDate = Convert.ToDateTime(CommFun.IsNullCheck(dtStageComp.Rows[i]["ReceiptDate"], CommFun.datatypes.VarTypeDate));

                //        DateTime dInterestDate;
                //        if (dFinaliseDate >= dStageDate)
                //            dInterestDate = dFinaliseDate;
                //        else
                //            dInterestDate = dStageDate;
                //        dInterestDate = dInterestDate.AddDays(iCreditDays);

                //        TimeSpan ts = dRecpDate - dInterestDate;
                //        int iDays = ts.Days;
                //        if (iDays < 0) { iDays = 0; }

                //        decimal dIntAmt = 0;
                //        if (dIntPer == 0)
                //            dIntAmt = 0;
                //        else if (iDays == 0)
                //            dIntAmt = 0;
                //        else if (dReceivable == 0)
                //            dIntAmt = 0;
                //        else
                //            dIntAmt = decimal.Round((dReceivable * dIntPer / 36500) * iDays, 3);

                //        dtStageComp.Rows[i]["Interest"] = dIntAmt;
                //        dtStageComp.Rows[i]["Age"] = iDays;
                //    }
                //}

                #endregion

                DataTable dtGroupedBy = CommFun.GetGroupedBy(dtInterest, "FlatId,Interest", "FlatId", "Sum");
                dtInterest = dtGroupedBy;

                DataRow[] drT;
                for (int j = 0; j < dtInterest.Rows.Count; j++)
                {
                    int iFlatId = Convert.ToInt32(CommFun.IsNullCheck(dtInterest.Rows[j]["FlatId"], CommFun.datatypes.vartypenumeric));
                    decimal dInt = Convert.ToDecimal(CommFun.IsNullCheck(dtInterest.Rows[j]["Interest"], CommFun.datatypes.vartypenumeric));
                    drT = dt.Select(String.Format("FlatId={0}", iFlatId));
                    if (drT.Length > 0)
                    {
                        if (dInt > 0) { drT[0]["Interest"] = dInt; }
                    }
                }

                #region PB Calc

                //sSql = "Select PB.FlatId,PB.PBDate,PB.AsOnDate,Case When D.ReceiptDate Is Null Then '" + String.Format("{0:dd-MMM-yyyy}", arg_dtAsOn) + "' " +
                //        " Else D.ReceiptDate End ReceiptDate,E.FinaliseDate,A.CreditDays,A.IntPercent,Case When C.Amount IS NULL Then B.Amount Else C.Amount End Amount,"+
                //        " 0.000 Interest,0 Age From dbo.ProgressBillRegister PB" +
                //        " Inner Join dbo.FlatDetails A ON PB.FlatId=A.FlatId" +
                //        " Inner Join dbo.PaymentScheduleFlat B On PB.FlatId=B.FlatId AND B.PaymentSchId=PB.PaySchId" +
                //        " LEFT Join dbo.ReceiptTrans C On B.FlatId=C.FlatId And C.PaySchId=B.PaymentSchId " +
                //        " LEFT Join dbo.ReceiptRegister D On D.ReceiptId=C.ReceiptId" +
                //        " Inner Join dbo.BuyerDetail E On E.FlatId=PB.FlatId" +
                //        " Where B.BillPassed=1 AND A.BlockId=" + arg_iBlockId + " AND A.CostCentreId=" + argCCId +
                //        " AND (PB.AsOnDate<='" + arg_dtAsOn.ToString("dd-MMM-yyyy") + "' OR D.ReceiptDate<='" + arg_dtAsOn.ToString("dd-MMM-yyyy") + "') ";
                //sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                //DataTable dtProgressBill = new DataTable();
                //sda.Fill(dtProgressBill);
                //sda.Dispose();

                //for (int i = 0; i < dtProgressBill.Rows.Count; i++)
                //{
                //    DateTime dFinaliseDate = Convert.ToDateTime(CommFun.IsNullCheck(dtProgressBill.Rows[i]["FinaliseDate"], CommFun.datatypes.VarTypeDate));
                //    decimal dIntPer = Convert.ToDecimal(CommFun.IsNullCheck(dtProgressBill.Rows[i]["IntPercent"], CommFun.datatypes.vartypenumeric));
                //    int iCreditDays = Convert.ToInt32(CommFun.IsNullCheck(dtProgressBill.Rows[i]["CreditDays"], CommFun.datatypes.vartypenumeric));
                //    DateTime dPBDate = Convert.ToDateTime(CommFun.IsNullCheck(dtProgressBill.Rows[i]["PBDate"], CommFun.datatypes.VarTypeDate));
                //    DateTime dAsOnDate = Convert.ToDateTime(CommFun.IsNullCheck(dtProgressBill.Rows[i]["AsOnDate"], CommFun.datatypes.VarTypeDate));
                //    decimal dTotAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtProgressBill.Rows[i]["Amount"], CommFun.datatypes.vartypenumeric));

                //    DateTime dInterestDate = DateTime.MinValue;
                //    if (dFinaliseDate >= dPBDate)
                //        dInterestDate = dFinaliseDate;
                //    else
                //        dInterestDate = dPBDate;
                //    dInterestDate = dInterestDate.AddDays(iCreditDays);

                //    DateTime dRecpDate;
                //    if (dtProgressBill.Rows[i]["ReceiptDate"].ToString().Trim() == "" || dtProgressBill.Rows[i]["ReceiptDate"].ToString().ToUpper() == "NULL")
                //        dRecpDate = arg_dtAsOn;
                //    else if (Convert.ToDateTime(dtProgressBill.Rows[i]["ReceiptDate"]) == null || Convert.ToDateTime(dtProgressBill.Rows[i]["ReceiptDate"]) == DateTime.MinValue)
                //        dRecpDate = arg_dtAsOn;
                //    else
                //        dRecpDate = Convert.ToDateTime(CommFun.IsNullCheck(dtProgressBill.Rows[i]["ReceiptDate"], CommFun.datatypes.VarTypeDate));

                //    TimeSpan ts1 = dAsOnDate - dPBDate;
                //    int i_AsOnDays = ts1.Days;

                //    TimeSpan ts = dRecpDate - dInterestDate;
                //    int iDays = ts.Days + i_AsOnDays;
                //    if (iDays < 0) { iDays = 0; }

                //    decimal dIntAmt = 0;
                //    if (dIntPer == 0)
                //        dIntAmt = 0;
                //    else
                //        dIntAmt = decimal.Round((dTotAmt * dIntPer / 36500) * iDays, 3);

                //    dtProgressBill.Rows[i]["Interest"] = dIntAmt;
                //    dtProgressBill.Rows[i]["Age"] = iDays;
                //}

                //dtGroupedBy = CommFun.GetGroupedBy(dtProgressBill, "FlatId,Interest", "FlatId", "Sum");
                //dtProgressBill = dtGroupedBy;

                //drT = new DataRow[] { };
                //for (int j = 0; j < dtProgressBill.Rows.Count; j++)
                //{
                //    int iFlatId = Convert.ToInt32(dtProgressBill.Rows[j]["FlatId"]);
                //    decimal dInt = Convert.ToDecimal(dtProgressBill.Rows[j]["Interest"]);
                //    drT = dt.Select(String.Format("FlatId={0}", iFlatId));
                //    if (drT.Length > 0)
                //    {
                //        if (dInt > 0) { drT[0]["Interest"] = dInt; }
                //    }
                //}

                #endregion

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

        internal static void UpdateFlatProjectReceivableDiscount(int argCCId, int argFlatId, decimal argNetAmount, decimal argWriteOff)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                if (argWriteOff == 0)
                {
                    String sSql = "Update dbo.PaymentScheduleFlat Set WriteOff=0" +
                                      " WHERE FlatId=" + argFlatId + " AND CostCentreId=" + argCCId + "";
                    SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    tran.Commit();
                }
                else
                {
                    String sSql = "Select * From dbo.PaymentScheduleFlat Where CostCentreId=" + argCCId + " AND FlatId=" + argFlatId + " ORDER BY SortOrder";
                    SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    dr.Close();
                    cmd.Dispose();

                    decimal dNetAmount = 0;
                    decimal dRxdAmount = argNetAmount * BsfGlobal.g_iSummaryUnit;
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        dNetAmount = dNetAmount + Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["NetAmount"], CommFun.datatypes.vartypenumeric));
                        if (dNetAmount > dRxdAmount)
                        {
                            //dNetAmount = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["NetAmount"], CommFun.datatypes.vartypenumeric));
                            //decimal dPaidAmount = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["PaidAmount"], CommFun.datatypes.vartypenumeric));
                            //decimal dWriteOff = (dNetAmount - dPaidAmount) * BsfGlobal.g_iSummaryUnit;

                            decimal dWriteOff = argWriteOff * BsfGlobal.g_iSummaryUnit;
                            
                            int iPaySchId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["PaymentSchId"], CommFun.datatypes.vartypenumeric));

                            sSql = "Update dbo.PaymentScheduleFlat Set WriteOff=" + dWriteOff +
                                   " WHERE FlatId=" + argFlatId + " AND CostCentreId=" + argCCId + " AND PaymentSchId=" + iPaySchId + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            tran.Commit();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }
        }


        internal static DataTable Get_Flat_ReceivableReport(int argCCId, DateTime arg_dtAsOn)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;

            decimal dUnit = BsfGlobal.g_iSummaryUnit;

            try
            {
                String sSql = "Select CRMReceivable From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CostCentreId=" + argCCId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();

                int iCRMRecv = 0;
                if (dt.Rows.Count > 0) { iCRMRecv = Convert.ToInt32(dt.Rows[0]["CRMReceivable"]); }
                string sCond = string.Empty;
                if (iCRMRecv == 1) { sCond = "AND A.StageDetId<>0"; }

                dt.Dispose();

                sSql = "SELECT A.CostCentreId,A.BlockName,A.BlockId,A.FlatId,A.FlatNo ,BuyerName,A.Type,  SUM(AgreementVale)/" + dUnit + " AgreementValue,  SUM(Receivable)/" + dUnit + " ReceivableAsOn," +
                        " SUM(Received)/" + dUnit + " Received, (SUM(Receivable)-SUM(Received))/" + dUnit + " DueAsOn, (SUM(AgreementVale)-SUM(Received))/" + dUnit + " TotalReceivable," +
                        " Case When SUM(Receivable)<>0 Then (SUM(Receivable)-SUM(Received))/SUM(Receivable)*100 Else 0 End [Recv%]  FROM ( " +
                        " SELECT A.CostCentreId,BlockName,A.BlockId,A.FlatId, A.FlatNo, L.LeadName BuyerName, CASE WHEN A.Status='S' THEN 'Buyer' ELSE 'Investor' END Type,  " +
                        " A.NetAmt+A.QualifierAmt AgreementVale, 0 Receivable, 0 Received, 0 Due,0 TotReceivable FROM FlatDetails A  " +
                        " INNER JOIN BuyerDetail B ON A.FlatId=B.FlatId    " +
                        " INNER JOIN LeadRegister L ON L.LeadId=B.LeadId "+
                        " INNER JOIN BlockMaster M ON M.BlockId=A.BlockId " +
                        " WHERE A.CostCentreId=" + argCCId + " And A.LeadId<>0   " +
                        " UNION ALL   " +
                        " SELECT B.CostCentreId,BlockName,B.BlockId,A.FlatId,B.FlatNo,L.LeadName BuyerName, CASE WHEN B.Status='S' THEN 'Buyer' ELSE 'Investor' END Type,0,   " +
                        " SUM(A.NetAmount),0,0,0 FROM ProgressBillRegister A " +
                        " INNER JOIN FlatDetails B ON A.FlatId=B.FlatId   " +
                        " INNER JOIN BuyerDetail D ON B.FlatId=D.FlatId " +
                        " INNER JOIN LeadRegister L ON B.LeadId=L.LeadId  " +
                        " INNER JOIN BlockMaster M ON M.BlockId=B.BlockId " +
                        " LEFT JOIN dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId " +
                        " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                        " WHERE B.CostCentreId=" + argCCId + " And A.PBDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' AND S.BillPassed=1 " +
                        " AND A.PBDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                        " GROUP BY B.CostCentreId,BlockName,B.BlockId,A.FlatId,B.FlatNo,L.LeadName, B.Status";

                if (iCRMRecv == 1)
                {
                    sSql = sSql + " UNION ALL " +
                                " SELECT A.CostCentreId,BlockName,B.BlockId,A.FlatId,B.FlatNo,L.LeadName BuyerName, CASE WHEN B.Status='S' THEN 'Buyer' ELSE 'Investor' END Type,0,    " +
                                " SUM(A.NetAmount),0,0,0 FROM PaymentScheduleFlat A "+
                                " INNER JOIN  FlatDetails B ON A.FlatId=B.FlatId    " +
                                " INNER JOIN BuyerDetail D ON B.FlatId=D.FlatId "+
                                " INNER JOIN LeadRegister L ON B.LeadId=L.LeadId   " +
                                " INNER JOIN BlockMaster M ON M.BlockId=B.BlockId " +
                                " INNER JOIN dbo.StageDetails SM ON A.StageDetId=SM.StageDetId " +
                                " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                                " WHERE B.CostCentreId=" + argCCId + " And A.BillPassed=0 AND A.StageDetId<>0 "+
                                " And SM.CompletionDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' " +
                                " AND SM.CompletionDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                                " GROUP BY A.CostCentreId,BlockName,B.BlockId,A.FlatId,B.FlatNo,L.LeadName, B.Status ";
                }
                else
                {
                    sSql = sSql + " UNION ALL " +
                                " SELECT A.CostCentreId,BlockName,B.BlockId,A.FlatId,B.FlatNo,L.LeadName BuyerName, CASE WHEN B.Status='S' THEN 'Buyer' ELSE 'Investor' END Type,0,    " +
                                " SUM(A.NetAmount),0,0,0 FROM PaymentScheduleFlat A "+
                                " INNER JOIN  FlatDetails B ON A.FlatId=B.FlatId " +
                                " INNER JOIN BuyerDetail D ON B.FlatId=D.FlatId "+
                                " INNER JOIN LeadRegister L ON B.LeadId=L.LeadId   " +
                                " INNER JOIN BlockMaster M ON M.BlockId=B.BlockId " +
                                " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                                " WHERE B.CostCentreId=" + argCCId + " And A.BillPassed=0 And A.SchDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' " +
                                " AND A.SchDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                                " GROUP BY A.CostCentreId,BlockName,B.BlockId,A.FlatId,B.FlatNo,L.LeadName, B.Status ";
                }                        


                sSql = sSql + " UNION ALL " +
                            " Select C.CostCentreId,BM.BlockName,BM.BlockId,D.FlatId,C.FlatNo,E.LeadName BuyerName, CASE WHEN D.Status='S' THEN 'Buyer' ELSE 'Investor' END Type,0,0,   " +
                            " (-1*OBReceipt)+ISNULL((SELECT SUM(A.Amount) FROM ( " +
                            " SELECT A.Amount FROM ReceiptTrans A  INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                            " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                            " WHERE A.FlatId=C.FlatId AND O.CRMActual=0 AND B.Cancel=0 AND A.CancelDate IS NULL " +
                            " AND B.ReceiptDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' " +
                            " UNION ALL ";

                if (BsfGlobal.g_bFADB == true)
                {
                    sSql = sSql + " SELECT SUM(A.Amount) FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                                " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                " WHERE A.FlatId=C.FlatId AND O.CRMActual=1 AND R.Cancel=0 AND A.CancelDate IS NULL" +
                                " AND B.ReceiptDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' " +
                                " UNION ALL " +
                                " SELECT SUM(A.Amount) FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                                " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                " WHERE A.FlatId=C.FlatId AND O.CRMActual=2 AND R.BRS=1 AND A.CancelDate IS NULL " +
                                " AND B.ReceiptDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' ) A ),0) Received,0,0  from FlatDetails C " +
                                " INNER JOIN BuyerDetail D ON D.LeadId=C.LeadId And C.FlatId=D.FlatId And D.Status=C.Status   " +
                                " INNER JOIN LeadRegister E ON E.LeadId=D.LeadId INNER JOIN BlockMaster BM ON BM.BlockId=C.BlockId Where C.CostCentreId=" + argCCId + " " +
                                " ) A " +
                                " INNER JOIN dbo.FlatDetails FD On FD.FlatId=A.FlatId " +
                                " INNER JOIN dbo.BlockMaster BM On BM.BlockId=FD.BlockId " +
                                " INNER JOIN dbo.LevelMaster LM ON LM.LevelId=FD.LevelId " +
                                " Where A.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ")  " +
                                " GROUP BY BM.SortOrder,LM.SortOrder,FD.SortOrder,A.CostCentreId,A.BlockName,A.BlockId,A.FlatId,A.FlatNo,BuyerName,A.Type " +
                                " Order By BM.SortOrder,LM.SortOrder,FD.SortOrder,dbo.Val(A.FlatNo)";
                }
                else
                {
                    sSql = sSql + " SELECT SUM(A.Amount) FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                    " WHERE A.FlatId=C.FlatId AND A.CancelDate IS NULL AND B.ReceiptDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' " +
                                    " UNION ALL " +
                                    " SELECT SUM(A.Amount) FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                    " WHERE A.FlatId=C.FlatId AND A.CancelDate IS NULL AND B.ReceiptDate<='" + Convert.ToDateTime(arg_dtAsOn).ToString("dd-MMM-yyyy") + "' ) A ),0) Received,0,0  from FlatDetails C " +
                                    " INNER JOIN BuyerDetail D ON D.LeadId=C.LeadId And C.FlatId=D.FlatId And D.Status=C.Status   " +
                                    " INNER JOIN LeadRegister E ON E.LeadId=D.LeadId INNER JOIN BlockMaster BM ON BM.BlockId=C.BlockId Where C.CostCentreId=" + argCCId + " " +
                                    " ) A " +
                                    " INNER JOIN dbo.FlatDetails FD On FD.FlatId=A.FlatId " +
                                    " INNER JOIN dbo.BlockMaster BM On BM.BlockId=FD.BlockId " +
                                    " INNER JOIN dbo.LevelMaster LM ON LM.LevelId=FD.LevelId " +
                                    " Where A.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ")  " +
                                    " GROUP BY BM.SortOrder,LM.SortOrder,FD.SortOrder,A.CostCentreId,A.BlockName,A.BlockId,A.FlatId,A.FlatNo,BuyerName,A.Type " +
                                    " Order By BM.SortOrder,LM.SortOrder,FD.SortOrder,dbo.Val(A.FlatNo)";
                }

                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
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

        #endregion

        #region Receivable Statement Tax

        internal static DataTable Get_CC_RecStmt_Tax(DateTime argStart, DateTime argEnd)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtT = null;
            try
            {
                DateTime FromDate = argStart;
                DateTime ToDate = argEnd;
                decimal dUnit = BsfGlobal.g_iSummaryUnit;

                string sSql = "Select A.CostCentreId,C.CostCentreName,Sum(A.LastYear)LastYear,Sum(A.Apr" + FromDate.Year + ")Apr" + FromDate.Year + "," +
                        " Sum(A.May" + FromDate.Year + ")May" + FromDate.Year + ",Sum(A.Jun" + FromDate.Year + ")Jun" + FromDate.Year +
                        ",Sum(A.Jul" + FromDate.Year + ")Jul" + FromDate.Year + ",Sum(A.Aug" + FromDate.Year + ")Aug" + FromDate.Year + "," +
                        " Sum(A.Sep" + FromDate.Year + ")Sep" + FromDate.Year + ",Sum(A.Oct" + FromDate.Year + ")Oct" + FromDate.Year +
                        ",Sum(A.Nov" + FromDate.Year + ")Nov" + FromDate.Year + ",Sum(A.Dec" + FromDate.Year + ")Dec" + FromDate.Year + "," +
                        " Sum(A.Jan" + ToDate.Year + ")Jan" + ToDate.Year + ",Sum(A.Feb" + ToDate.Year + ")Feb" + ToDate.Year +
                        ",Sum(A.Mar" + ToDate.Year + ")Mar" + ToDate.Year + ",Sum(A.Total)Total,0 Bal From(" +
                        " Select C.CostCentreId, Sum(case when SchDate<'" + FromDate.ToString("dd-MMM-yyyy") + "' then A.NetAmount else 0 end)/" + dUnit +
                        " + (SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.CostCentreId=C.CostCentreId) as LastYear,   " +
                        " Sum(case when Month(SchDate)=4 AND Year(SchDate)=" + FromDate.Year + " then A.NetAmount else 0 end)/" + dUnit + " as Apr" + FromDate.Year + ",  " +
                        " Sum(case when Month(SchDate)=5 AND Year(SchDate)=" + FromDate.Year + " then A.NetAmount else 0 end)/" + dUnit + " as May" + FromDate.Year + ",  " +
                        " Sum(case when Month(SchDate)=6 AND Year(SchDate)=" + FromDate.Year + " then A.NetAmount else 0 end)/" + dUnit + " as Jun" + FromDate.Year + ",  " +
                        " Sum(case when Month(SchDate)=7 AND Year(SchDate)=" + FromDate.Year + " then A.NetAmount else 0 end)/" + dUnit + " as Jul" + FromDate.Year + ",  " +
                        " Sum(case when Month(SchDate)=8 AND Year(SchDate)=" + FromDate.Year + " then A.NetAmount else 0 end)/" + dUnit + " as Aug" + FromDate.Year + ",  " +
                        " Sum(case when Month(SchDate)=9 AND Year(SchDate)=" + FromDate.Year + " then A.NetAmount else 0 end)/" + dUnit + " as Sep" + FromDate.Year + ",  " +
                        " Sum(case when Month(SchDate)=10 AND Year(SchDate)=" + FromDate.Year + " then A.NetAmount else 0 end)/" + dUnit + " as Oct" + FromDate.Year + ",  " +
                        " Sum(case when Month(SchDate)=11 AND Year(SchDate)=" + FromDate.Year + " then A.NetAmount else 0 end)/" + dUnit + " as Nov" + FromDate.Year + ",  " +
                        " Sum(case when Month(SchDate)=12 AND Year(SchDate)=" + FromDate.Year + " then A.NetAmount else 0 end)/" + dUnit + " as Dec" + FromDate.Year + ",  " +
                        " Sum(case when Month(SchDate)=1 AND Year(SchDate)=" + ToDate.Year + " then A.NetAmount else 0 end)/" + dUnit + " as Jan" + ToDate.Year + ",  " +
                        " Sum(case when Month(SchDate)=2 AND Year(SchDate)=" + ToDate.Year + " then A.NetAmount else 0 end)/" + dUnit + " as Feb" + ToDate.Year + ",  " +
                        " Sum(case when Month(SchDate)=3 AND Year(SchDate)=" + ToDate.Year + " then A.NetAmount else 0 end)/" + dUnit + " as Mar" + ToDate.Year + ",  " +
                        " Sum(A.NetAmount)/" + dUnit + " +(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.CostCentreId=C.CostCentreId) Total, " +
                        " 0 Bal From dbo.PaymentScheduleFlat A  " +
                        " Inner Join FlatDetails C On C.CostCentreId=A.CostCentreId And A.FlatId=C.FlatId  " +
                        " Inner Join BlockMaster D On D.BlockId=C.BlockId And C.CostCentreId=D.CostCentreId  " +
                        " Inner Join LeadRegister E On E.LeadId=C.LeadId "+
                        " LEFT JOIN dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId AND AC.Approve='Y' " +
                        " Left Join dbo.StageDetails F On A.StageDetId=F.StageDetId " +
                        " WHERE ((C.CostCentreId IN (SELECT CostCentreId FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CRMReceivable=0) AND A.SchDate<='" + ToDate.ToString("dd-MMM-yyyy") + "') And A.BillPassed=0" +
                        " OR (C.CostCentreId IN (SELECT CostCentreId FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CRMReceivable=1) "+
                        " AND A.StageDetId<>0 AND F.CompletionDate<='" + ToDate.ToString("dd-MMM-yyyy") + "' AND F.CompletionDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END) "+
                        ") GROUP BY C.CostCentreId ";

                if (BsfGlobal.g_bFADB == true)
                {
                    sSql = sSql + " ) A INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On A.CostCentreId=C.CostCentreId  " +
                                    " And C.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans "+
                                    " Where UserId=" + BsfGlobal.g_lUserId + ") " +
                                    " Group By A.CostCentreId,C.CostCentreName";
                }
                else
                {
                    sSql = sSql + ") A " +
                                   " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On A.CostCentreId=C.CostCentreId  " +
                                   " And C.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans "+
                                   " Where UserId=" + BsfGlobal.g_lUserId + ") " +
                                   " Group By A.CostCentreId,C.CostCentreName";
                }

                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dtT = new DataTable();
                dtT.Load(dreader);
                dreader.Close();
                cmd.Dispose();

                sSql = "SELECT C.CostCentreId,C.CostCentreName, " +
                        " Sum(case when ReceiptDate<'" + FromDate.ToString("dd-MMM-yyyy") + "' then B.Amount else 0 end)/" + dUnit +
                        " -(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.CostCentreId=C.CostCentreId) as LastYear, " +
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
                        " WHERE F.CostCentreId=C.CostCentreId) Total,0 Bal FROM ( " +
                        " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                        " WHERE O.CRMActual=0 AND B.Cancel=0 AND A.CancelDate IS NULL "+
                        " AND B.ReceiptDate<='" + ToDate.ToString("dd-MMM-yyyy") + "' ";


                if (BsfGlobal.g_bFADB == true)
                {
                    sSql = sSql + " UNION ALL SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                    " WHERE O.CRMActual=1 AND R.Cancel=0 AND A.CancelDate IS NULL " +
                                    " UNION ALL " +
                                    " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                    " WHERE O.CRMActual=2 AND R.BRS=1 AND A.CancelDate IS NULL " +
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

                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dreader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();

                string sOCaption = ""; string sNCaption = ""; DataView dv; int iCCId = 0; decimal dAmt = 0;
                for (int i = 2; i < dt.Columns.Count; i++)
                {
                    sOCaption = dt.Columns[i].Caption.ToString();
                    sNCaption = "Recd" + dt.Columns[i].Caption.ToString();

                    DataColumn col1 = new DataColumn(sNCaption) { DataType = typeof(decimal), DefaultValue = 0 };
                    dtT.Columns.Add(col1);

                    DataTable dtRecv = new DataTable();
                    dtRecv = dt;

                    for (int j = 0; j < dtRecv.Rows.Count; j++)
                    {
                        iCCId = Convert.ToInt32(dtRecv.Rows[j]["CostCentreId"]);
                        dAmt = Convert.ToDecimal(dtRecv.Rows[j][sOCaption]);

                        DataRow[] drT = dtT.Select(String.Format("CostCentreId={0}", iCCId));
                        if (drT.Length > 0)
                        {
                            drT[0][sNCaption] = dAmt;
                            if (sNCaption == "RecdBal") { drT[0][sNCaption] = (decimal)drT[0]["Total"] - (decimal)drT[0]["RecdTotal"]; }
                        }
                    }

                    dtRecv.Dispose();
                }
            }
            catch (Exception ce)
            {
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtT;
        }

        internal static DataTable Get_Block_RecStmt_Tax(int argCCId, DateTime argStart, DateTime argEnd)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtT = null;

            try
            {
                String sSql = "Select CRMReceivable From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CostCentreId=" + argCCId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();

                int iCRMRecv = 0;
                if (dt.Rows.Count > 0) { iCRMRecv = Convert.ToInt32(dt.Rows[0]["CRMReceivable"]); }

                string sCond = string.Empty;
                if (iCRMRecv == 1) { sCond = " AND A.StageDetId<>0"; }

                dt.Dispose();

                decimal dUnit = BsfGlobal.g_iSummaryUnit;
                DateTime FromDate = argStart;
                DateTime ToDate = argEnd;

                if (iCRMRecv == 1)
                {
                    sSql = "Select A.BlockId,E.BlockName,Sum(A.LastYear)LastYear,Sum(A.Apr" + FromDate.Year + ")Apr" + FromDate.Year + "," +
                            " Sum(A.May" + FromDate.Year + ")May" + FromDate.Year + ",Sum(A.Jun" + FromDate.Year + ")Jun" + FromDate.Year +
                            ",Sum(A.Jul" + FromDate.Year + ")Jul" + FromDate.Year + ",Sum(A.Aug" + FromDate.Year + ")Aug" + FromDate.Year + "," +
                            " Sum(A.Sep" + FromDate.Year + ")Sep" + FromDate.Year + ",Sum(A.Oct" + FromDate.Year + ")Oct" + FromDate.Year +
                            ",Sum(A.Nov" + FromDate.Year + ")Nov" + FromDate.Year + ",Sum(A.Dec" + FromDate.Year + ")Dec" + FromDate.Year + "," +
                            " Sum(A.Jan" + ToDate.Year + ")Jan" + ToDate.Year + ",Sum(A.Feb" + ToDate.Year + ")Feb" + ToDate.Year +
                            ",Sum(A.Mar" + ToDate.Year + ")Mar" + ToDate.Year + ",Sum(A.Total)Total,0 Bal From(" +
                            " Select C.BlockId, " +
                            " Sum(case when F.CompletionDate<='" + FromDate.ToString("dd-MMM-yyyy") + "' then NetAmount else 0 end)/" + dUnit +
                            "+(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails FD WHERE FD.BlockId=C.BlockId) as LastYear,  " +
                            " Sum(case when Month(F.CompletionDate)=4 AND Year(F.CompletionDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Apr" + FromDate.Year + ", " +
                            " Sum(case when Month(F.CompletionDate)=5 AND Year(F.CompletionDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as May" + FromDate.Year + ", " +
                            " Sum(case when Month(F.CompletionDate)=6 AND Year(F.CompletionDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Jun" + FromDate.Year + ", " +
                            " Sum(case when Month(F.CompletionDate)=7 AND Year(F.CompletionDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Jul" + FromDate.Year + ", " +
                            " Sum(case when Month(F.CompletionDate)=8 AND Year(F.CompletionDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Aug" + FromDate.Year + ", " +
                            " Sum(case when Month(F.CompletionDate)=9 AND Year(F.CompletionDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Sep" + FromDate.Year + ", " +
                            " Sum(case when Month(F.CompletionDate)=10 AND Year(F.CompletionDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Oct" + FromDate.Year + ", " +
                            " Sum(case when Month(F.CompletionDate)=11 AND Year(F.CompletionDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Nov" + FromDate.Year + ", " +
                            " Sum(case when Month(F.CompletionDate)=12 AND Year(F.CompletionDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Dec" + FromDate.Year + ", " +
                            " Sum(case when Month(F.CompletionDate)=1 AND Year(F.CompletionDate)=" + ToDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Jan" + ToDate.Year + ", " +
                            " Sum(case when Month(F.CompletionDate)=2 AND Year(F.CompletionDate)=" + ToDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Feb" + ToDate.Year + ", " +
                            " Sum(case when Month(F.CompletionDate)=3 AND Year(F.CompletionDate)=" + ToDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Mar" + ToDate.Year + ", " +
                            " Sum(A.NetAmount)/" + dUnit + " +(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F " +
                            " WHERE F.BlockId=C.BlockId) Total,0 Bal From dbo.PaymentScheduleFlat A " +
                            " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B On A.CostCentreId=B.CostCentreId " +
                            " INNER JOIN FlatDetails C On C.CostCentreId=A.CostCentreId And A.FlatId=C.FlatId " +
                            " INNER JOIN LeadRegister E On E.LeadId=C.LeadId " +
                            " INNER JOIN dbo.StageDetails F On A.StageDetId=F.StageDetId " +
                            " Where B.CostCentreId=" + argCCId + " AND F.CompletionDate<='" + ToDate.ToString("dd-MMM-yyyy") + "' AND A.StageDetId<>0 "+
                            " Group By C.BlockId";
                }
                else
                {
                    sSql = "Select A.BlockId,E.BlockName,Sum(A.LastYear)LastYear,Sum(A.Apr" + FromDate.Year + ")Apr" + FromDate.Year + "," +
                            " Sum(A.May" + FromDate.Year + ")May" + FromDate.Year + ",Sum(A.Jun" + FromDate.Year + ")Jun" + FromDate.Year +
                            ",Sum(A.Jul" + FromDate.Year + ")Jul" + FromDate.Year + ",Sum(A.Aug" + FromDate.Year + ")Aug" + FromDate.Year + "," +
                            " Sum(A.Sep" + FromDate.Year + ")Sep" + FromDate.Year + ",Sum(A.Oct" + FromDate.Year + ")Oct" + FromDate.Year +
                            ",Sum(A.Nov" + FromDate.Year + ")Nov" + FromDate.Year + ",Sum(A.Dec" + FromDate.Year + ")Dec" + FromDate.Year + "," +
                            " Sum(A.Jan" + ToDate.Year + ")Jan" + ToDate.Year + ",Sum(A.Feb" + ToDate.Year + ")Feb" + ToDate.Year +
                            ",Sum(A.Mar" + ToDate.Year + ")Mar" + ToDate.Year + ",Sum(A.Total)Total,0 Bal From(" +
                            " Select C.BlockId, " +
                            " Sum(case when SchDate<='" + FromDate.ToString("dd-MMM-yyyy") + "' then NetAmount else 0 end)/" + dUnit +
                            "+(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails FD WHERE FD.BlockId=C.BlockId) as LastYear,  " +
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
                            " Sum(A.NetAmount)/" + dUnit + " +(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F " +
                            " WHERE F.BlockId=C.BlockId) Total,0 Bal From dbo.PaymentScheduleFlat A " +
                            " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B On A.CostCentreId=B.CostCentreId " +
                            " INNER JOIN FlatDetails C On C.CostCentreId=A.CostCentreId And A.FlatId=C.FlatId " +
                            " INNER JOIN LeadRegister E On E.LeadId=C.LeadId " +
                            " Where B.CostCentreId=" + argCCId + " AND A.SchDate<='" + ToDate.ToString("dd-MMM-yyyy") + 
                            "' Group By C.BlockId";
                }
                
               
                if (BsfGlobal.g_bFADB == true)
                {
                    sSql = sSql + " ) A Inner Join BlockMaster E On A.BlockId=E.BlockId "+
                                " Group By A.BlockId,E.SortOrder,E.BlockName " +
                                " Order By E.SortOrder";
                }
                else
                {
                    sSql = sSql + ")A " +
                                  " Inner Join BlockMaster E On A.BlockId=E.BlockId "+
                                  " Group By A.BlockId,E.SortOrder,E.BlockName " +
                                  " Order by E.SortOrder";
                }
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dreader = cmd.ExecuteReader();
                dtT = new DataTable();
                dtT.Load(dreader);
                dreader.Close();
                cmd.Dispose();

                sSql = "Select E.BlockId,E.BlockName, " +
                       " Sum(case when ReceiptDate<='" + FromDate.ToString("dd-MMM-yyyy") + "' then B.Amount else 0 end)/" + dUnit +
                       " -(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.BlockId=E.BlockId) as LastYear, " +
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
                       " FROM FlatDetails F WHERE F.BlockId=E.BlockId)  Total,0 Bal " +
                       " FROM BlockMaster E INNER JOIN FlatDetails D ON D.BlockId=E.BlockId LEFT JOIN ( " +
                       " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                       " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId "+
                       " WHERE O.CRMActual=0 AND B.Cancel=0 AND A.CancelDate IS NULL ";

                if (BsfGlobal.g_bFADB == true)
                {
                    sSql = sSql + " UNION ALL " +
                                    " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                    " WHERE O.CRMActual=1 AND R.Cancel=0 AND A.CancelDate IS NULL  " +
                                    " UNION ALL " +
                                    " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                    " WHERE  O.CRMActual=2 AND R.BRS=1 AND A.CancelDate IS NULL  " +
                                    " )B On B.FlatId=D.FlatId " +
                                    " WHERE E.CostCentreId=" + argCCId + " Group By E.BlockId,E.SortOrder,E.BlockName " +
                                    " Order By E.SortOrder";
                }
                else
                {
                    sSql = sSql + " )B On B.FlatId=D.FlatId " +
                                  " WHERE E.CostCentreId=" + argCCId + " Group By E.BlockId,E.SortOrder,E.BlockName " +
                                  " Order By E.SortOrder";
                }
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();

                string sOCaption = ""; string sNCaption = ""; int iBlockId = 0; decimal dAmt = 0;
                for (int i = 2; i < dt.Columns.Count; i++)
                {
                    sOCaption = dt.Columns[i].Caption.ToString();
                    sNCaption = "Recd" + dt.Columns[i].Caption.ToString();

                    DataColumn col1 = new DataColumn(sNCaption) { DataType = typeof(decimal), DefaultValue = 0 };
                    dtT.Columns.Add(col1);

                    DataTable dtRecv = new DataTable();
                    dtRecv = dt;

                    for (int j = 0; j < dtRecv.Rows.Count; j++)
                    {
                        iBlockId = Convert.ToInt32(dtRecv.Rows[j]["BlockId"]);
                        dAmt = Convert.ToDecimal(dtRecv.Rows[j][sOCaption]);

                        DataRow[] drT = dtT.Select(String.Format("BlockId={0}", iBlockId));
                        if (drT.Length > 0)
                        {
                            drT[0][sNCaption] = dAmt;
                            if (sNCaption == "RecdBal") { drT[0][sNCaption] = (decimal)drT[0]["Total"] - (decimal)drT[0]["RecdTotal"]; }
                        }
                    }

                    dtRecv.Dispose();
                }
            }
            catch (Exception ce)
            {
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtT;
        }

        internal static DataTable Get_Flat_RecStmt_Tax(int argCCId, int argBlockId, DateTime argStart, DateTime argEnd)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtT = new DataTable();

            try
            {
                String sSql = "Select CRMReceivable From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CostCentreId=" + argCCId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();

                int iCRMRecv = 0;
                if (dt.Rows.Count > 0) { iCRMRecv = Convert.ToInt32(dt.Rows[0]["CRMReceivable"]); }

                string sCond = string.Empty;
                if (iCRMRecv == 1) { sCond = "AND A.StageDetId<>0"; }

                dt.Dispose();

                DateTime FromDate = argStart;
                DateTime ToDate = argEnd;
                decimal dUnit = BsfGlobal.g_iSummaryUnit;

                if (iCRMRecv == 1)
                {
                    sSql = "Select B.FlatId,B.FlatNo,C.LeadName BuyerName,Sum(A.LastYear)LastYear,Sum(A.Apr" + FromDate.Year + ")Apr" + FromDate.Year + "," +
                            " Sum(A.May" + FromDate.Year + ")May" + FromDate.Year + ",Sum(A.Jun" + FromDate.Year + ")Jun" + FromDate.Year +
                            ",Sum(A.Jul" + FromDate.Year + ")Jul" + FromDate.Year + ",Sum(A.Aug" + FromDate.Year + ")Aug" + FromDate.Year + "," +
                            " Sum(A.Sep" + FromDate.Year + ")Sep" + FromDate.Year + ",Sum(A.Oct" + FromDate.Year + ")Oct" + FromDate.Year +
                            ",Sum(A.Nov" + FromDate.Year + ")Nov" + FromDate.Year + ",Sum(A.Dec" + FromDate.Year + ")Dec" + FromDate.Year + "," +
                            " Sum(A.Jan" + ToDate.Year + ")Jan" + ToDate.Year + ",Sum(A.Feb" + ToDate.Year + ")Feb" + ToDate.Year +
                            ",Sum(A.Mar" + ToDate.Year + ")Mar" + ToDate.Year + ",Sum(A.Total)Total,0 Bal From(" +
                            " Select C.FlatId, " +
                            " Sum(case when D.CompletionDate<='" + FromDate.ToString("dd-MMM-yyyy") + "' then NetAmount else 0 end)/" + dUnit +
                            " +(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.FlatId=C.FlatId) as LastYear,  " +
                            " Sum(case when Month(D.CompletionDate)=4 AND Year(D.CompletionDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Apr" + FromDate.Year + ", " +
                            " Sum(case when Month(D.CompletionDate)=5 AND Year(D.CompletionDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as May" + FromDate.Year + ", " +
                            " Sum(case when Month(D.CompletionDate)=6 AND Year(D.CompletionDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Jun" + FromDate.Year + ", " +
                            " Sum(case when Month(D.CompletionDate)=7 AND Year(D.CompletionDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Jul" + FromDate.Year + ", " +
                            " Sum(case when Month(D.CompletionDate)=8 AND Year(D.CompletionDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Aug" + FromDate.Year + ", " +
                            " Sum(case when Month(D.CompletionDate)=9 AND Year(D.CompletionDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Sep" + FromDate.Year + ", " +
                            " Sum(case when Month(D.CompletionDate)=10 AND Year(D.CompletionDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Oct" + FromDate.Year + ", " +
                            " Sum(case when Month(D.CompletionDate)=11 AND Year(D.CompletionDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Nov" + FromDate.Year + ", " +
                            " Sum(case when Month(D.CompletionDate)=12 AND Year(D.CompletionDate)=" + FromDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Dec" + FromDate.Year + ", " +
                            " Sum(case when Month(D.CompletionDate)=1 AND Year(D.CompletionDate)=" + ToDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Jan" + ToDate.Year + ", " +
                            " Sum(case when Month(D.CompletionDate)=2 AND Year(D.CompletionDate)=" + ToDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Feb" + ToDate.Year + ", " +
                            " Sum(case when Month(D.CompletionDate)=3 AND Year(D.CompletionDate)=" + ToDate.Year + " then NetAmount else 0 end)/" + dUnit + " as Mar" + ToDate.Year + ", " +
                            " Sum(A.NetAmount)/" + dUnit + " +(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.FlatId=C.FlatId) Total, " +
                            " 0 Bal From dbo.PaymentScheduleFlat A " +
                            " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B On A.CostCentreId=B.CostCentreId " +
                            " Inner Join FlatDetails C On C.CostCentreId=A.CostCentreId And A.FlatId=C.FlatId " +
                            " Inner Join dbo.StageDetails D ON A.StageDetId=D.StageDetId "+
                            " WHERE D.CompletionDate<='" + ToDate.ToString("dd-MMM-yyyy") + "' AND A.CostCentreId=" + argCCId +
                            " And C.BlockId=" + argBlockId + " AND A.StageDetId<>0 Group By C.FlatId";
                }
                else
                {
                    sSql = "Select B.FlatId,B.FlatNo,C.LeadName BuyerName,Sum(A.LastYear)LastYear,Sum(A.Apr" + FromDate.Year + ")Apr" + FromDate.Year + "," +
                            " Sum(A.May" + FromDate.Year + ")May" + FromDate.Year + ",Sum(A.Jun" + FromDate.Year + ")Jun" + FromDate.Year +
                            ",Sum(A.Jul" + FromDate.Year + ")Jul" + FromDate.Year + ",Sum(A.Aug" + FromDate.Year + ")Aug" + FromDate.Year + "," +
                            " Sum(A.Sep" + FromDate.Year + ")Sep" + FromDate.Year + ",Sum(A.Oct" + FromDate.Year + ")Oct" + FromDate.Year +
                            ",Sum(A.Nov" + FromDate.Year + ")Nov" + FromDate.Year + ",Sum(A.Dec" + FromDate.Year + ")Dec" + FromDate.Year + "," +
                            " Sum(A.Jan" + ToDate.Year + ")Jan" + ToDate.Year + ",Sum(A.Feb" + ToDate.Year + ")Feb" + ToDate.Year +
                            ",Sum(A.Mar" + ToDate.Year + ")Mar" + ToDate.Year + ",Sum(A.Total)Total,0 Bal From(" +
                            " Select C.FlatId, " +
                            " Sum(case when SchDate<='" + FromDate.ToString("dd-MMM-yyyy") + "' then NetAmount else 0 end)/" + dUnit +
                            " +(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.FlatId=C.FlatId) as LastYear,  " +
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
                            " Sum(A.NetAmount)/" + dUnit + " +(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.FlatId=C.FlatId) Total, " +
                            " 0 Bal From dbo.PaymentScheduleFlat A " +
                            " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B On A.CostCentreId=B.CostCentreId " +
                            " Inner Join FlatDetails C On C.CostCentreId=A.CostCentreId And A.FlatId=C.FlatId " +
                            " WHERE A.SchDate<='" + ToDate.ToString("dd-MMM-yyyy") + "' AND A.CostCentreId=" + argCCId +
                            " And C.BlockId=" + argBlockId + " Group By C.FlatId";
                }

                if (BsfGlobal.g_bFADB == true)
                {
                    sSql = sSql + " )A INNER JOIN FlatDetails B On B.FlatId=A.FlatId " +
                                    " INNER JOIN LeadRegister C On C.LeadId=B.LeadId " +
                                    " INNER JOIN dbo.BlockMaster BM On BM.BlockId=B.BlockId " +
                                    " INNER JOIN dbo.LevelMaster LM On LM.LevelId=B.LevelId " +
                                    " Group By BM.SortOrder,LM.SortOrder,B.SortOrder,C.LeadName,B.FlatNo,B.FlatId "+
                                    " Order By BM.SortOrder,LM.SortOrder,B.SortOrder, " +
                                    " dbo.Val(B.FlatNo)";
                }
                else
                {
                    sSql = sSql + ") A " +
                                   " INNER JOIN FlatDetails B On B.FlatId=A.FlatId " +
                                   " INNER JOIN LeadRegister C On C.LeadId=B.LeadId " +
                                   " INNER JOIN dbo.BlockMaster BM On BM.BlockId=B.BlockId " +
                                   " INNER JOIN dbo.LevelMaster LM On LM.LevelId=B.LevelId " +
                                   " Group By BM.SortOrder,LM.SortOrder,B.SortOrder,C.LeadName,B.FlatNo,B.FlatId Order By " +
                                   " BM.SortOrder,LM.SortOrder,B.SortOrder, " +
                                   " dbo.Val(B.FlatNo)";
                }

                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dreader = cmd.ExecuteReader();
                dtT = new DataTable();
                dtT.Load(dreader);
                dreader.Close();
                cmd.Dispose();

                sSql = "Select D.FlatId,D.FlatNo,L.LeadName BuyerName, " +
                        " Sum(case when ReceiptDate<='" + FromDate.ToString("dd-MMM-yyyy") + "' then B.Amount else 0 end)/" + dUnit +
                        "-(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.FlatId=D.FlatId) as LastYear, " +
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
                        " then B.Amount else 0 end)/" + dUnit + " as Mar" + ToDate.Year + ", Sum(IsNull(B.Amount,0))/" + dUnit +
                        "-(SELECT SUM(OBReceipt)/" + dUnit + " FROM FlatDetails F WHERE F.FlatId=D.FlatId) Total,0 Bal " +
                        " FROM FlatDetails D Inner Join BuyerDetail E ON D.FlatId=E.FlatId   Inner Join LeadRegister L On L.LeadId=D.LeadId " +
                        " LEFT JOIN ( " +
                        " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A  INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                        " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                        " WHERE O.CRMActual=0 AND B.Cancel=0 AND A.CancelDate IS NULL ";

                if (BsfGlobal.g_bFADB == true)
                {
                    sSql = sSql + " UNION ALL SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                    " WHERE O.CRMActual=1 AND R.Cancel=0 AND A.CancelDate IS NULL " +
                                    " UNION ALL " +
                                    " SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=B.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=A.CostCentreId " +
                                    " WHERE  O.CRMActual=2 AND R.BRS=1 AND A.CancelDate IS NULL " +
                                    " )B On B.FlatId=D.FlatId " +
                                    " INNER JOIN dbo.BlockMaster BM On BM.BlockId=D.BlockId " +
                                    " INNER JOIN dbo.LevelMaster LM On LM.LevelId=D.LevelId Where D.BlockId=" + argBlockId + "  " +
                                    " GROUP BY BM.SortOrder,LM.SortOrder,D.SortOrder,L.LeadName,D.FlatNo, D.FlatId " +
                                    " Order By BM.SortOrder,LM.SortOrder,D.SortOrder,dbo.Val(D.FlatNo) ";
                }
                else
                {
                    sSql = sSql + ")B On B.FlatId=D.FlatId " +
                                    " INNER JOIN dbo.BlockMaster BM On BM.BlockId=D.BlockId " +
                                    " INNER JOIN dbo.LevelMaster LM On LM.LevelId=D.LevelId Where D.BlockId=" + argBlockId + "  " +
                                    " GROUP BY BM.SortOrder,LM.SortOrder,D.SortOrder,L.LeadName,D.FlatNo, D.FlatId " +
                                    " Order By BM.SortOrder,LM.SortOrder,D.SortOrder,dbo.Val(D.FlatNo) ";
                }
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();

                string sOCaption = ""; string sNCaption = ""; int iFlatId = 0; decimal dAmt = 0;

                for (int i = 3; i < dt.Columns.Count; i++)
                {
                    sOCaption = dt.Columns[i].Caption.ToString();
                    sNCaption = "Recd" + dt.Columns[i].Caption.ToString();

                    DataColumn col1 = new DataColumn(sNCaption) { DataType = typeof(decimal), DefaultValue = 0 };
                    dtT.Columns.Add(col1);


                    DataTable dtRecv = new DataTable();
                    dtRecv = dt;

                    for (int j = 0; j < dtRecv.Rows.Count; j++)
                    {
                        iFlatId = Convert.ToInt32(dtRecv.Rows[j]["FlatId"]);
                        dAmt = Convert.ToDecimal(dtRecv.Rows[j][sOCaption]);

                        DataRow[] drT = dtT.Select(String.Format("FlatId={0}", iFlatId));
                        if (drT.Length > 0)
                        {
                            drT[0][sNCaption] = dAmt;
                            if (sNCaption == "RecdBal") { drT[0][sNCaption] = (decimal)drT[0]["Total"] - (decimal)drT[0]["RecdTotal"]; }
                        }
                    }

                    dtRecv.Dispose();
                }
            }
            catch (Exception ce)
            {
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtT;
        }

        internal static DataTable Get_CC_ActStmt(DateTime argStart, DateTime argEnd)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            DateTime FromDate = argStart;
            DateTime ToDate = argEnd;
            decimal dUnit = BsfGlobal.g_iSummaryUnit;
            try
            {
                String sSql = "SELECT C.CostCentreId,C.CostCentreName, " +
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
                    sSql = sSql + " UNION ALL SELECT A.FlatId,B.ReceiptDate, A.Amount FROM ReceiptTrans A INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
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
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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
                                    " WHERE E.CostCentreId=" + argCCId + " Group By E.BlockId,E.SortOrder,E.BlockName " +
                                    " Order By E.SortOrder";
                }
                else
                {
                    sSql = sSql + " )B On B.FlatId=D.FlatId " +
                                    " WHERE E.CostCentreId=" + argCCId + " Group By E.BlockId,E.SortOrder,E.BlockName " +
                                    " Order By E.SortOrder";
                }
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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
            DataTable dt = null;
            String sSql = string.Empty;
            DateTime FromDate = argStart;
            DateTime ToDate = argEnd;
            decimal dUnit = BsfGlobal.g_iSummaryUnit;
            try
            {
                sSql = "Select D.FlatId,D.FlatNo,L.LeadName BuyerName, " +
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
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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
            DataTable dt = null;
            try
            {
                String sSql = "Select CRMReceivable From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CostCentreId=" + argCCId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();

                int iCRMRecv = 0;
                if (dt.Rows.Count > 0) { iCRMRecv = Convert.ToInt32(dt.Rows[0]["CRMReceivable"]); }

                string sCond = string.Empty;
                if (iCRMRecv == 1) { sCond = "AND A.StageDetId<>0"; }

                dt.Dispose();

                decimal dUnit = BsfGlobal.g_iSummaryUnit;
                DateTime FromDate = argStart;
                DateTime ToDate = argEnd;

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
                        " Where A.CostCentreId=" + argCCId + " " + sCond + " Group By C.FlatId" +
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

                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dreader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();
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

        internal static DataTable GetFlatPaymentInfo(int argCCId,int argBlockId,int argFlatId, DateTime argFrom, DateTime argTo, string argType)
        {
            DataTable dt = null;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                if (argType == "Project")
                {
                    sSql = "SELECT X.RowId,X.QualifierId,X.QualifierName,X.NetPer,X.Amount FROM ( " +
                            " SELECT 1 RowId,0 QualifierId,'PaidGrossAmount' QualifierName,0 NetPer,isnull(SUM(PaidGrossAmount),0) Amount " +
                            " FROM ReceiptShTrans A  INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId INNER JOIN dbo.FlatDetails F ON F.FlatId=A.FlatId " +
                            " WHERE PaidNetAmount<>0 AND A.ReceiptTypeId<>1 And F.CostCentreId=" + argCCId + "  " +
                            " And B.ReceiptDate Between '" + argFrom.ToString("dd-MMM-yyyy") + "' And '" + argTo.ToString("dd-MMM-yyyy") + "'  GROUP BY F.CostCentreId " +
                            " UNION ALL  " +
                            " Select 2 RowId,B.QualifierId,D.QualifierName,B.NetPer,Sum(B.Amount)Amount From ReceiptShTrans A   " +
                            " Inner Join ReceiptQualifier B On A.ReceiptId=B.ReceiptId And A.PaymentSchId=B.PaymentSchId And A.OtherCostId=B.OtherCostId  " +
                            " And A.ReceiptTypeId=B.ReceiptTypeId  Inner Join ReceiptRegister C On C.ReceiptId=A.ReceiptId   " +
                            " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp D On D.QualifierId=B.QualifierId And QualType='B' " +
                            " INNER JOIN dbo.FlatDetails F ON F.FlatId=A.FlatId  " +
                            " Where A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1 And F.CostCentreId=" + argCCId + " And C.ReceiptDate " +
                            " Between '" + argFrom.ToString("dd-MMM-yyyy") + "' And '" + argTo.ToString("dd-MMM-yyyy") + "'  " +
                            " GROUP BY B.QualifierId,D.QualifierName,B.NetPer " +
                            " UNION ALL   " +
                            " SELECT 3 RowId,0 QualifierId,'PaidNetAmount' QualifierName,0 NetPer,isnull(SUM(A.Amount),0) Amount FROM ReceiptTrans A    " +
                            " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId INNER JOIN dbo.FlatDetails F ON F.FlatId=A.FlatId  WHERE A.Amount<>0 And F.CostCentreId=" + argCCId + "  " +
                            " AND B.ReceiptDate Between '" + argFrom.ToString("dd-MMM-yyyy") + "' And '" + argTo.ToString("dd-MMM-yyyy") + "'  GROUP BY F.CostCentreId " +
                            " ) X ORDER BY X.RowId";
                }
                else if (argType == "Block")
                {
                    sSql = "SELECT X.RowId,X.QualifierId,X.QualifierName,X.NetPer,X.Amount FROM ( " +
                            " SELECT 1 RowId,0 QualifierId,'PaidGrossAmount' QualifierName,0 NetPer,isnull(SUM(PaidGrossAmount),0) Amount " +
                            " FROM ReceiptShTrans A  INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId INNER JOIN dbo.FlatDetails F ON F.FlatId=A.FlatId " +
                            " WHERE PaidNetAmount<>0 AND A.ReceiptTypeId<>1 And F.BlockId=" + argBlockId + "  " +
                            " And B.ReceiptDate Between '" + argFrom.ToString("dd-MMM-yyyy") + "' And '" + argTo.ToString("dd-MMM-yyyy") + "'  GROUP BY F.BlockId " +
                            " UNION ALL  " +
                            " Select 2 RowId,B.QualifierId,D.QualifierName,B.NetPer,Sum(B.Amount)Amount From ReceiptShTrans A   " +
                            " Inner Join ReceiptQualifier B On A.ReceiptId=B.ReceiptId And A.PaymentSchId=B.PaymentSchId And A.OtherCostId=B.OtherCostId  " +
                            " And A.ReceiptTypeId=B.ReceiptTypeId  Inner Join ReceiptRegister C On C.ReceiptId=A.ReceiptId   " +
                            " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp D On D.QualifierId=B.QualifierId And QualType='B' " +
                            " INNER JOIN dbo.FlatDetails F ON F.FlatId=A.FlatId  " +
                            " Where A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1 And F.BlockId=" + argBlockId + " And C.ReceiptDate " +
                            " Between '" + argFrom.ToString("dd-MMM-yyyy") + "' And '" + argTo.ToString("dd-MMM-yyyy") + "' " +
                            " GROUP BY B.QualifierId,D.QualifierName,B.NetPer " +
                            " UNION ALL   " +
                            " SELECT 3 RowId,0 QualifierId,'PaidNetAmount' QualifierName,0 NetPer,isnull(SUM(A.Amount),0) Amount FROM ReceiptTrans A    " +
                            " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId INNER JOIN dbo.FlatDetails F ON F.FlatId=A.FlatId  WHERE A.Amount<>0 And F.BlockId=" + argBlockId + "   " +
                            " AND B.ReceiptDate Between '" + argFrom.ToString("dd-MMM-yyyy") + "' And '" + argTo.ToString("dd-MMM-yyyy") + "'  GROUP BY F.BlockId " +
                            " ) X ORDER BY X.RowId";
                }
                else if (argType == "Buyer")
                {
                    sSql = "SELECT X.RowId,X.ReceiptId,X.ReceiptDate,Y.ReceiptNo,Y.ChequeDate,Y.ChequeNo,Y.BankName,X.QualifierId,X.QualifierName,X.NetPer,X.Amount FROM (" +
                            " SELECT 1 RowId,A.ReceiptId,B.ReceiptDate,0 QualifierId,'PaidGrossAmount' QualifierName,0 NetPer,isnull(SUM(PaidGrossAmount),0) Amount FROM ReceiptShTrans A   " +
                            " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                            " WHERE PaidNetAmount<>0 AND A.ReceiptTypeId<>1 And A.FlatId=" + argFlatId + " " +
                            " And B.ReceiptDate Between '" + argFrom.ToString("dd-MMM-yyyy") + "' And '" + argTo.ToString("dd-MMM-yyyy") + "' " +
                            " GROUP BY A.ReceiptId,B.ReceiptDate" +
                            " UNION ALL " +
                            " Select 2 RowId,A.ReceiptId,C.ReceiptDate,B.QualifierId,D.QualifierName,B.NetPer,Sum(B.Amount)Amount From ReceiptShTrans A  " +
                            " Inner Join ReceiptQualifier B On A.ReceiptId=B.ReceiptId And A.PaymentSchId=B.PaymentSchId And A.OtherCostId=B.OtherCostId " +
                            " And A.ReceiptTypeId=B.ReceiptTypeId  Inner Join ReceiptRegister C On C.ReceiptId=A.ReceiptId  " +
                            " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp D On D.QualifierId=B.QualifierId And QualType='B' " +
                            " Where A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1 And A.FlatId=" + argFlatId + "" +
                            " And C.ReceiptDate Between '" + argFrom.ToString("dd-MMM-yyyy") + "' And '" + argTo.ToString("dd-MMM-yyyy") + "' " +
                            " GROUP BY A.ReceiptId,C.ReceiptDate,B.QualifierId,D.QualifierName,B.NetPer" +
                            " UNION ALL  " +
                            " SELECT 3 RowId,A.ReceiptId,B.ReceiptDate,0 QualifierId,'PaidNetAmount' QualifierName,0 NetPer,isnull(SUM(A.Amount),0) Amount FROM ReceiptTrans A   " +
                            " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId   " +
                            " WHERE A.Amount<>0 And A.FlatId=" + argFlatId + " " +
                            " AND B.ReceiptDate Between '" + argFrom.ToString("dd-MMM-yyyy") + "' And '" + argTo.ToString("dd-MMM-yyyy") + "' " +
                            " GROUP BY A.ReceiptId,B.ReceiptDate) X INNER JOIN ReceiptRegister Y ON X.ReceiptId=Y.ReceiptId " +
                            " ORDER BY X.ReceiptId,X.RowId";
                }
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
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


        #endregion

        #region Stagewise Receivable

        public static DataTable GetProject()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "Select CostCentreId,CostCentreName,CRMActual, BusinessType From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre A" +
                              " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B ON A.ProjectDB=B.ProjectName" +
                              " Where BusinessType IN('B', 'L') AND CostCentreId NOT IN(Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                              " Where UserId=" + BsfGlobal.g_lUserId + ") Order By CostCentreName";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
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

        public static DataTable GetBlock(int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "Select BlockId,BlockName From dbo.BlockMaster Where CostCentreId=" + argCCId + " Order By BlockName";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
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

        public static DataTable GetPayment(int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "Select Distinct A.TypeId,TypeName From PaySchType A " +
                              " INNER JOIN PaymentSchedule B On A.TypeId=B.TypeId " +
                              " Where B.CostCentreId=" + argCCId +
                              " UNION ALL " +
                              " Select Distinct A.TypeId,TypeName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaySchType A " +
                              " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedule B On A.TypeId=B.TypeId " +
                              " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister C On B.LandRegId=C.LandId " +
                              " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre D On C.ProjectName=D.ProjectDB " +
                              " Where D.CostCentreId=" + argCCId + " ";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
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

        public static DataSet GetProjectStageRec(int argCCId, int argPayTypeId, DateTime argDate, int argFromActual, string argBusinessType)
        {
            BsfGlobal.OpenCRMDB();
            DataSet ds = new DataSet();            
            decimal dUnit = BsfGlobal.g_iSummaryUnit;
            try
            {
                String sSql = "Select CRMReceivable From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CostCentreId=" + argCCId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();

                int iCRMRecv;
                if (dt.Rows.Count > 0)
                    iCRMRecv = Convert.ToInt32(dt.Rows[0]["CRMReceivable"]);
                else
                    iCRMRecv = 0;

                string sCond = "";
                string sDate = "";
                if (iCRMRecv == 1)
                {
                    sCond = "AND PSF.StageDetId<>0";
                    sDate = " AND SD.CompletionDate";
                }
                else
                {
                    sCond = "";
                    sDate = " AND PSF.SchDate";
                }

                dt.Dispose();
                                
                sSql = "Select CostCentreId, CostCentreName, [O/B]=-1*(SELECT SUM(OBReceipt) FROM FlatDetails F Where F.CostCentreId=O.CostCentreId)/" + dUnit + ", " +
                       " Advance=(Select IsNull(SUM(B.Amount),0) Amount From ReceiptRegister A " +
                       " Inner Join ReceiptTrans B On A.ReceiptId=B.ReceiptId " +
                       " Inner Join FlatDetails C On A.FlatId=C.FlatId " +
                       " Where A.PaymentAgainst='A' And A.CostCentreId=" + argCCId + " AND C.PayTypeId=" + argPayTypeId +
                       " And ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "')/" + dUnit + ", " +
                       " ExtraBillAmt=(Select Isnull(SUM(NetAmount),0) From ExtraBillRegister Where CostCentreId=" + argCCId + ")/" + dUnit + " " +
                       " From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O " +
                       " Where ProjectDB In(Select ProjectName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType IN('B', 'L')) " +
                       " AND CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                       " Where UserId=" + BsfGlobal.g_lUserId + ") And CostCentreId=" + argCCId + " Order By CostCentreName";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Project");
                sda.Dispose();

                if (argBusinessType == "B")
                {
                    sSql = "Select TemplateId,Description From dbo.PaymentSchedule " +
                           " Where CostCentreId=" + argCCId + " And TypeId=" + argPayTypeId + " ORDER BY SortOrder";
                }
                else
                {
                    sSql = "Select TemplateId,Description From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedule A " +
                           " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B ON A.LandRegId=B.LandId " +
                           " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C ON B.ProjectName=C.ProjectDB " +
                           " Where C.CostCentreId=" + argCCId + " AND A.TypeId=" + argPayTypeId + " ORDER BY SortOrder";
                }
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                SqlDataReader sdr = cmd.ExecuteReader();
                ds.Load(sdr, LoadOption.OverwriteChanges, "Stages");
                sdr.Close();
                cmd.Dispose();

                string sFromActual = "";
                if (argFromActual == 0)
                {
                    if (argBusinessType == "B")
                    {
                        sFromActual = " Select PSF.CostCentreId,PS.TemplateId,0,Sum(RT.Amount) Received From dbo.ReceiptTrans RT " +
                                     " INNER JOIN dbo.ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                     " INNER JOIN dbo.FlatDetails FD On RT.FlatId=FD.FlatId " +
                                     " INNER JOIN dbo.PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                     " RIGHT JOIN dbo.PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                     " WHERE RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PSF.CostCentreId=" + argCCId + " AND FD.PayTypeId=" + argPayTypeId + " " +
                                     " AND RR.Cancel=0 GROUP BY PSF.CostCentreId,PS.TemplateId";
                    }
                    else
                    {
                        sFromActual = " Select OC.CostCentreId,PS.TemplateId,0,Sum(RT.Amount) Received From ReceiptTrans RT " +
                                     " INNER JOIN dbo.ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                     " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails FD On RT.FlatId=FD.PlotDetailsId " +
                                     " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot PSF On RT.PaySchId=PSF.PaymentSchId" +
                                     " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B ON PSF.LandRegId=B.LandId " +
                                     " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON B.ProjectName=OC.ProjectDB " +
                                     " RIGHT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                     " WHERE RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' " +
                                     " AND OC.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + " AND RR.Cancel=0 " +
                                     " GROUP BY OC.CostCentreId,PS.TemplateId";
                    }
                }
                else if (argFromActual == 1)
                {
                    if (argBusinessType == "B")
                    {
                        sFromActual = " Select PSF.CostCentreId,PS.TemplateId,0,Sum(RT.Amount) Received From dbo.ReceiptTrans RT " +
                                      " INNER JOIN dbo.ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                      " INNER JOIN dbo.FlatDetails FD On RT.FlatId=FD.FlatId " +
                                      " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=RR.ReceiptId " +
                                      " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=RT.CostCentreId " +
                                      " INNER JOIN dbo.PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                      " RIGHT JOIN dbo.PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                      " WHERE O.CRMActual=1 AND R.Cancel=0 And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") +
                                      "' AND PSF.CostCentreId=" + argCCId + " AND FD.PayTypeId=" + argPayTypeId + " " +
                                      " GROUP BY PSF.CostCentreId,PS.TemplateId";
                    }
                    else
                    {
                        sFromActual = " Select OC.CostCentreId,PS.TemplateId,0,Sum(RT.Amount) Received From dbo.ReceiptTrans RT " +
                                     " INNER JOIN dbo.ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                     " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails FD On RT.FlatId=FD.PlotDetailsId " +
                                     " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=RR.ReceiptId " +
                                     " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot PSF On RT.PaySchId=PSF.PaymentSchId" +
                                     " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B ON PSF.LandRegId=B.LandId " +
                                     " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON B.ProjectName=OC.ProjectDB " +
                                     " RIGHT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                     " WHERE OC.CRMActual=1 AND R.Cancel=0 And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") +
                                     "' AND OC.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + " " +
                                     " GROUP BY OC.CostCentreId,PS.TemplateId";
                    }
                }
                else
                {
                    if (argBusinessType == "B")
                    {
                        sFromActual = " Select PSF.CostCentreId,PS.TemplateId,0,Sum(RT.Amount) Received From dbo.ReceiptTrans RT " +
                                     " INNER JOIN dbo.ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                     " INNER JOIN dbo.FlatDetails FD On RT.FlatId=FD.FlatId " +
                                     " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=RR.ReceiptId " +
                                     " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=RT.CostCentreId " +
                                     " INNER JOIN dbo.PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                     " RIGHT JOIN dbo.PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                     " WHERE O.CRMActual=2 AND R.BRS=1 And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") +
                                     "' AND PSF.CostCentreId=" + argCCId + " AND FD.PayTypeId=" + argPayTypeId + " " +
                                     " GROUP BY PSF.CostCentreId,PS.TemplateId";
                    }
                    else
                    {
                        sFromActual = " Select OC.CostCentreId,PS.TemplateId,0,Sum(RT.Amount) Received From dbo.ReceiptTrans RT " +
                                     " INNER JOIN dbo.ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                     " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails FD On RT.FlatId=FD.PlotDetailsId " +
                                     " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=RR.ReceiptId " +
                                     " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot PSF On RT.PaySchId=PSF.PaymentSchId" +
                                     " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B ON PSF.LandRegId=B.LandId " +
                                     " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON B.ProjectName=OC.ProjectDB " +
                                     " RIGHT JOIN dbo.PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                     " WHERE OC.CRMActual=2 AND R.BRS=1 And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") +
                                     "' AND OC.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + " " +
                                     " GROUP BY OC.CostCentreId,PS.TemplateId";
                    }
                }

                if (argBusinessType == "B")
                {
                    sSql = "SELECT ProjectId,TemplateId, SUM(Receivable)/" + dUnit + " Receivable,  " +
                            " SUM(Received)/" + dUnit + " Received ,(SUM(Receivable)-SUM(Received))/" + dUnit + " Balance FROM (" +
                            " SELECT PB.CostCentreId ProjectId,PS.TemplateId,SUM(PB.NetAmount) Receivable,0 Received FROM dbo.ProgressBillRegister PB" +
                            " INNER JOIN dbo.FlatDetails FD ON FD.FlatId=PB.FlatId " +
                            " INNER JOIN dbo.BuyerDetail BD ON FD.FlatId=BD.FlatId " +
                            " INNER JOIN dbo.LeadRegister LR ON LR.LeadId=FD.LeadId " +
                            " INNER JOIN dbo.PaymentScheduleFlat PSF ON PSF.PaymentSchId=PB.PaySchId" +
                            " RIGHT JOIN dbo.PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                            " WHERE PSF.BillPassed=1 And PBDate<='" + argDate.ToString("dd-MMM-yyyy") + "' And PB.CostCentreId=" + argCCId +
                            " AND FD.PayTypeId=" + argPayTypeId + " GROUP BY PB.CostCentreId,PS.TemplateId" +
                            " UNION ALL  " +
                            " SELECT PSF.CostCentreId ProjectId,PS.TemplateId,SUM(PSF.NetAmount) Receivable,0 FROM dbo.PaymentScheduleFlat PSF" +
                            " INNER JOIN dbo.FlatDetails FD ON PSF.FlatId=FD.FlatId " +
                            " INNER JOIN dbo.BuyerDetail BD ON BD.FlatId=FD.FlatId  " +
                            " INNER JOIN dbo.LeadRegister LR ON LR.LeadId=FD.LeadId " +
                            " LEFT JOIN dbo.StageDetails SD ON PSF.StageDetId=SD.StageDetId " +
                            " LEFT JOIN dbo.AllotmentCancel AC ON PSF.FlatId=AC.FlatId AND AC.Approve='Y' " +
                            " RIGHT JOIN dbo.PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                            " WHERE PSF.BillPassed=0 " + sDate + "<='" + argDate.ToString("dd-MMM-yyyy") + "' " + sCond +
                            " " + sDate + ">Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                            " AND PSF.CostCentreId=" + argCCId + " AND FD.PayTypeId=" + argPayTypeId + " " +
                            " GROUP BY PSF.CostCentreId ,PS.TemplateId" +
                            " UNION ALL  " +
                            " " + sFromActual + " " +
                            " ) A  INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B ON A.ProjectId=B.CostCentreId   " +
                            " GROUP BY ProjectId,TemplateId ";
                }
                else
                {
                    sSql = "SELECT ProjectId,TemplateId, SUM(Receivable)/" + dUnit + " Receivable,  " +
                            " SUM(Received)/" + dUnit + " Received ,(SUM(Receivable)-SUM(Received))/" + dUnit + " Balance FROM (" +
                            " SELECT PB.CostCentreId ProjectId,PS.TemplateId,SUM(PB.NetAmount) Receivable,0 Received FROM dbo.PlotProgressBillRegister PB" +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails FD ON FD.PlotDetailsId=PB.PlotDetailsId " +
                            " LEFT JOIN dbo.BuyerDetail BD ON FD.PlotDetailsId=BD.PlotId " +
                            " LEFT JOIN dbo.LeadRegister LR ON LR.LeadId=FD.BuyerId " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot PSF ON PSF.PaymentSchId=PB.PaySchId" +
                            " RIGHT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister CR ON PSF.LandRegId=CR.LandId" +
                            " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON CR.ProjectName=OC.ProjectDB" +
                            " WHERE PSF.BillPassed=1 And PBDate<='" + argDate.ToString("dd-MMM-yyyy") + "' And PB.CostCentreId=" + argCCId +
                            " AND PS.TypeId=" + argPayTypeId + " GROUP BY PB.CostCentreId,PS.TemplateId" +
                            " UNION ALL  " +
                            " SELECT OC.CostCentreId ProjectId,PS.TemplateId,SUM(PSF.NetAmount) Receivable,0 FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot PSF" +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails FD ON PSF.PlotDetailsId=FD.PlotDetailsId " +
                            " INNER JOIN dbo.BuyerDetail BD ON BD.PlotId=FD.PlotDetailsId  " +
                            " INNER JOIN dbo.LeadRegister LR ON LR.LeadId=FD.BuyerId " +
                            " LEFT JOIN dbo.AllotmentCancel AC ON PSF.PlotDetailsId=AC.FlatId AND AC.Approve='Y' " +
                            " RIGHT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister CR ON PSF.LandRegId=CR.LandId" +
                            " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON CR.ProjectName=OC.ProjectDB" +
                            " WHERE PSF.BillPassed=0 AND PSF.SchDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND " + 
                            " PSF.SchDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                            " AND OC.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + " " +
                            " GROUP BY OC.CostCentreId,PS.TemplateId" +
                            " UNION ALL  " +
                            " " + sFromActual + " " +
                            " ) A  INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B ON A.ProjectId=B.CostCentreId   " +
                            " GROUP BY ProjectId,TemplateId ";
                }
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                ds.Load(sdr, LoadOption.OverwriteChanges, "Recv");
                sdr.Close();
                cmd.Dispose();

                if (ds.Tables["Recv"].Rows.Count > 0)
                {
                    decimal dTotalRecv = 0;
                    decimal dTotalRevd = 0;
                    for (int i = 0; i < ds.Tables["Stages"].Rows.Count; i++)
                    {
                        string sStageName = ds.Tables["Stages"].Rows[i]["Description"].ToString();
                        int iTemplateId = (int)ds.Tables["Stages"].Rows[i]["TemplateId"];
                        int iProjectId = argCCId;
                        int iStageId = iTemplateId;

                        string sColName1 = iStageId + "- Recv";
                        DataColumn col1 = new DataColumn(sColName1) { DataType = typeof(decimal), DefaultValue = 0 };
                        ds.Tables["Project"].Columns.Add(col1);

                        string sColName2 = iStageId + "- Recd";
                        DataColumn col2 = new DataColumn(sColName2) { DataType = typeof(decimal), DefaultValue = 0 };
                        ds.Tables["Project"].Columns.Add(col2);

                        string sColName3 = iStageId + "- Bal";
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

                            DataRow[] drT = ds.Tables["Project"].Select(String.Format("CostCentreId={0}", iProjectId));
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

                    DataRow[] drows = ds.Tables["Project"].Select(String.Format("CostCentreId={0}", argCCId));
                    if (drows.Length > 0)
                    {
                        drows[0]["TotalReceivable"] = dTotalRecv;
                        //drows[0]["TotalReceived"] = (decimal)drows[0]["O/B"] + (decimal)drows[0]["Advance"] + (decimal)drows[0]["ExtraBillAmt"] + dTotalRevd;
                        drows[0]["TotalReceived"] = dTotalRevd;
                        drows[0]["Balance"] = (decimal)drows[0]["TotalReceivable"] - (decimal)drows[0]["TotalReceived"];
                    }
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
            return ds;
        }

        public static DataSet GetBlockStageRec(int argCCId, int argPayTypeId, DateTime argDate, int argFromActual, string argBusinessType)
        {
            BsfGlobal.OpenCRMDB();
            DataSet ds = new DataSet();
            
            decimal dUnit = BsfGlobal.g_iSummaryUnit;

            try
            {
                String sSql = "Select CRMReceivable From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CostCentreId=" + argCCId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();

                int iCRMRecv = 0;
                if (dt.Rows.Count > 0)
                    iCRMRecv = Convert.ToInt32(dt.Rows[0]["CRMReceivable"]);
                else
                    iCRMRecv = 0;

                string sCond = "";
                string sDate = "";
                if (iCRMRecv == 1)
                {
                    sCond = "AND PSF.StageDetId<>0";
                    sDate = " AND SD.CompletionDate";
                }
                else
                {
                    sCond = "";
                    sDate = " AND PSF.SchDate";
                }
                
                /* Parama Commented ON 20-06-2013
                sSql = "SELECT DISTINCT B.BlockId,B.BlockName,[O/B]=ISNULL(SUM(OBReceipt),0)/" + dUnit + ",Advance=ISNULL(SUM(Advance),0)/" + dUnit + ", " +
                        " ExtraBillAmt=ISNULL(SUM(ExtraBillAmt),0)/" + dUnit + ", AdvNetAmt=ISNULL(SUM(AdvNetAmt),0)/" + dUnit + " FROM ( " +
                        " SELECT DISTINCT FD.BlockId, OBReceipt=0, Advance=RT.Amount, ExtraBillAmt=0, AdvNetAmt=PSF.NetAmount FROM ReceiptTrans RT " +
                        " INNER JOIN ReceiptRegister RR ON RR.ReceiptId=RT.ReceiptId  " +
                        " INNER JOIN FlatDetails FD ON FD.FlatId=RT.FlatId "+
                        " INNER JOIN PaymentScheduleFlat PSF ON PSF.PaymentSchId=RT.PaySchId  " +
                        " WHERE RR.PaymentAgainst='A' And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND RT.CostCentreId=" + argCCId + "  " +
                        " UNION ALL " +
                        " SELECT DISTINCT FD.BlockId,-1*OBReceipt OBReceipt,0 Advance,0 ExtraBillAmt, 0 AdvNetAmt FROM FlatDetails FD"+
                        " WHERE FD.CostCentreId=" + argCCId + " AND OBReceipt<>0 " +
                        " UNION All " +
                        " SELECT DISTINCT FD.BlockId, OBReceipt=0, Advance=0, ExtraBillAmt=RT.Amount, 0 AdvNetAmt FROM ReceiptTrans RT " +
                        " INNER JOIN ReceiptRegister RR ON RR.ReceiptId=RT.ReceiptId "+
                        " INNER JOIN FlatDetails FD ON FD.FlatId=RT.FlatId"+
                        " WHERE ReceiptType='ExtraBill' AND ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND RT.CostCentreId=" + argCCId + "  " +
                        " ) A RIGHT JOIN BlockMaster B ON A.BlockId=B.BlockId "+
                        " WHERE B.CostCentreId=" + argCCId + " " +
                        " GROUP BY B.BlockId,B.BlockName ORDER BY BlockName";
                 */

                sSql = "SELECT DISTINCT B.SortOrder,B.BlockId,B.BlockName,[O/B]=ISNULL(SUM(OBReceipt),0)/" + dUnit + ",Advance=ISNULL(SUM(Advance),0)/" + dUnit + ", " +
                        " ExtraBillAmt=ISNULL(SUM(ExtraBillAmt),0)/" + dUnit + " FROM ( " +
                        " SELECT DISTINCT FD.BlockId, OBReceipt=0, Advance=RT.Amount, ExtraBillAmt=0 FROM ReceiptTrans RT " +
                        " INNER JOIN ReceiptRegister RR ON RR.ReceiptId=RT.ReceiptId  " +
                        " INNER JOIN FlatDetails FD ON FD.FlatId=RT.FlatId " +
                        " WHERE RR.PaymentAgainst='A' And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' "+
                        " AND RT.CostCentreId=" + argCCId + " AND FD.PayTypeId="+ argPayTypeId +
                        " UNION ALL " +
                        " SELECT DISTINCT FD.BlockId,-1*OBReceipt OBReceipt,0 Advance,0 ExtraBillAmt FROM FlatDetails FD" +
                        " WHERE FD.CostCentreId=" + argCCId + " AND OBReceipt<>0 " + " AND FD.PayTypeId=" + argPayTypeId +
                        " UNION All " +
                        " SELECT DISTINCT FD.BlockId, OBReceipt=0, Advance=0, ExtraBillAmt=RT.NetAmount FROM ExtraBillRegister RT " +
                        " INNER JOIN FlatDetails FD ON FD.FlatId=RT.FlatId" +
                        " WHERE RT.BillDate<='" + argDate.ToString("dd-MMM-yyyy") + "' "+
                        " AND RT.CostCentreId=" + argCCId + " AND FD.PayTypeId=" + argPayTypeId +
                        " ) A RIGHT JOIN BlockMaster B ON A.BlockId=B.BlockId " +
                        " WHERE B.CostCentreId=" + argCCId + " " +
                        " GROUP BY B.SortOrder,B.BlockId,B.BlockName ORDER BY B.SortOrder";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                SqlDataReader sdreader = cmd.ExecuteReader();
                ds.Load(sdreader, LoadOption.OverwriteChanges, "Block");
                sdreader.Close();
                cmd.Dispose();

                sSql = "Select TemplateId,Description From PaymentSchedule Where CostCentreId=" + argCCId + " And TypeId=" + argPayTypeId + " ORDER BY SortOrder";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                sdreader = cmd.ExecuteReader();
                ds.Load(sdreader, LoadOption.OverwriteChanges, "Stages");
                sdreader.Close();
                cmd.Dispose();

                string sFromActual = string.Empty;
                if (argFromActual == 0)
                {
                    sFromActual = " Select FD.BlockId,PSF.TemplateId,0,SUM(RT.Amount) Received From ReceiptTrans RT " +
                                    " INNER JOIN ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                    " INNER JOIN FlatDetails FD ON FD.FlatId=RT.FlatId" +
                                    " INNER JOIN PaymentScheduleFlat PSF ON RT.PaySchId=PSF.PaymentSchId " +
                                    " WHERE RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND RT.CostCentreId=" + argCCId + 
                                    " AND FD.PayTypeId=" + argPayTypeId + " " +
                                    " AND RR.Cancel=0 GROUP BY FD.BlockId,PSF.TemplateId";
                }
                else if (argFromActual == 1)
                {
                    sFromActual = " Select FD.BlockId,PSF.TemplateId,0,Sum(RT.Amount) Received From ReceiptTrans RT " +
                                     " INNER JOIN ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                     " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=RR.ReceiptId " +
                                     " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=RT.CostCentreId " +
                                     " INNER JOIN PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId " +
                                     " INNER JOIN FlatDetails FD ON FD.FlatId=PSF.FlatId " +
                                     " WHERE O.CRMActual=1 AND R.Cancel=0 And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + 
                                     "' AND PSF.CostCentreId=" + argCCId + " AND FD.PayTypeId=" + argPayTypeId + " " +
                                     " GROUP BY FD.BlockId,PSF.TemplateId";
                }
                else
                {
                    sFromActual = " Select FD.BlockId,PSF.TemplateId,0,Sum(RT.Amount) Received From ReceiptTrans RT " +
                                    " INNER JOIN ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=RR.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=RT.CostCentreId " +
                                    " INNER JOIN PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId " +
                                    " INNER JOIN FlatDetails FD ON FD.FlatId=RT.FlatId" +
                                    " WHERE O.CRMActual=2 AND R.BRS=1 And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + 
                                    "' AND RT.CostCentreId=" + argCCId + " AND FD.PayTypeId=" + argPayTypeId + " " +
                                    " GROUP BY FD.BlockId,PSF.TemplateId";
                }

                sSql = "SELECT B.BlockId,B.BlockName,TemplateId,SUM(Receivable)/" + dUnit + " Receivable,SUM(Received)/" + dUnit + " Received," +
                        "(SUM(Receivable)-SUM(Received))/" + dUnit + " Balance FROM  ( " +
                        " SELECT FD.BlockId,PS.TemplateId,SUM(PB.NetAmount) Receivable,0 Received FROM ProgressBillRegister PB" +
                        " INNER JOIN FlatDetails FD ON FD.FlatId=PB.FlatId  " +
                        " INNER JOIN BuyerDetail BD ON FD.FlatId=BD.FlatId" +
                        " INNER JOIN LeadRegister LR ON LR.LeadId=FD.LeadId  " +
                        " INNER JOIN PaymentScheduleFlat PSF ON PSF.PaymentSchId=PB.PaySchId " +
                        " INNER JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId " +
                        " WHERE PSF.BillPassed=1 And PBDate<='" + argDate.ToString("dd-MMM-yyyy") + "' And PB.CostCentreId=" + argCCId +
                        " AND FD.PayTypeId=" + argPayTypeId + " " +
                        " GROUP BY FD.BlockId,PS.TemplateId " +
                        " UNION ALL   " +
                        " SELECT FD.BlockId,PS.TemplateId,SUM(PSF.NetAmount) Receivable,0 Received FROM PaymentScheduleFlat PSF" +
                        " INNER JOIN FlatDetails FD ON PSF.FlatId=FD.FlatId  " +
                        " INNER JOIN BuyerDetail BD ON FD.FlatId=BD.FlatId " +
                        " INNER JOIN LeadRegister LR ON LR.LeadId=BD.LeadId  " +
                        " INNER JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId " +
                        " LEFT JOIN dbo.StageDetails SD ON PSF.StageDetId=SD.StageDetId " +
                        " LEFT JOIN dbo.AllotmentCancel AC ON PSF.FlatId=AC.FlatId AND AC.Approve='Y' " +
                        " WHERE PSF.BillPassed=0 " + sDate + "<='" + argDate.ToString("dd-MMM-yyyy") + "' " + sCond +
                        " " + sDate + ">Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                        " AND PSF.CostCentreId=" + argCCId + " AND FD.PayTypeId=" + argPayTypeId + "  " +
                        " GROUP BY FD.BlockId ,PS.TemplateId " +
                        " UNION ALL   " +
                        " " + sFromActual + " " +
                        " ) A  " +
                        " INNER JOIN dbo.BlockMaster B ON A.BlockId=B.BlockId    " +
                        " GROUP BY B.SortOrder,B.BlockId,B.BlockName,TemplateId " +
                        " ORDER BY B.SortOrder";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                sdreader = cmd.ExecuteReader();
                ds.Load(sdreader, LoadOption.OverwriteChanges, "Recv");
                sdreader.Close();
                cmd.Dispose();
                                
                if (ds.Tables["Recv"].Rows.Count > 0)
                {
                    decimal dTotalRecv = 0;
                    decimal dTotalRevd = 0;
                    for (int i = 0; i < ds.Tables["Stages"].Rows.Count; i++)
                    {
                        string sStageName = ds.Tables["Stages"].Rows[i]["Description"].ToString();
                        int iTemplateId = Convert.ToInt32(CommFun.IsNullCheck(ds.Tables["Stages"].Rows[i]["TemplateId"], CommFun.datatypes.vartypenumeric));
                        int iBlockId = 0;
                        int iStageId = iTemplateId;

                        string sColName1 = iStageId + "- Recv";
                        DataColumn col1 = new DataColumn(sColName1) { DataType = typeof(decimal), DefaultValue = 0 };
                        ds.Tables["Block"].Columns.Add(col1);

                        string sColName2 = iStageId + "- Recd";
                        DataColumn col2 = new DataColumn(sColName2) { DataType = typeof(decimal), DefaultValue = 0 };
                        ds.Tables["Block"].Columns.Add(col2);

                        string sColName3 = iStageId + "- Bal";
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

                            DataRow[] drT = ds.Tables["Block"].Select(String.Format("BlockId={0}", iBlockId));
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
                        dTotalRecv = 0;
                        dTotalRevd = 0;

                        int j = 6;
                        while (j < ds.Tables["Block"].Columns.Count)
                        {
                            dTotalRecv = dTotalRecv + (decimal)ds.Tables["Block"].Rows[i][j];
                            if (j == 7)
                                dTotalRevd = dTotalRevd + (decimal)ds.Tables["Block"].Rows[i][j] ;//+ (decimal)ds.Tables["Block"].Rows[i][2];
                            else
                                dTotalRevd = dTotalRevd + (decimal)ds.Tables["Block"].Rows[i][j + 1];

                            j = j + 3;
                        }

                        /* Parama Commented ON 20-06-2013
                        decimal d_TotalReceivable = dTotalRecv + (decimal)ds.Tables["Block"].Rows[i]["AdvNetAmt"];
                         */

                        decimal d_TotalReceivable = dTotalRecv;
                        ds.Tables["Block"].Rows[i]["TotalReceivable"] = d_TotalReceivable;

                        /* Parama Commented ON 20-06-2013
                        decimal d_TotalReceived = (decimal)ds.Tables["Block"].Rows[i]["O/B"]
                                                                      + (decimal)ds.Tables["Block"].Rows[i]["Advance"]
                                                                      + (decimal)ds.Tables["Block"].Rows[i]["ExtraBillAmt"]
                                                                      + dTotalRevd;
                         */

                        decimal d_TotalReceived = dTotalRevd;
                        ds.Tables["Block"].Rows[i]["TotalReceived"] = d_TotalReceived;

                        ds.Tables["Block"].Rows[i]["Balance"] = d_TotalReceivable - d_TotalReceived;
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

        public static DataSet GetBuyerStageRec(int argCCId, int argBlockId, int argPayTypeId, DateTime argDate, int argFromActual, string argBusinessType)
        {
            BsfGlobal.OpenCRMDB();
            DataSet ds = new DataSet();
            decimal dUnit = BsfGlobal.g_iSummaryUnit;
            try
            {
                String sSql = "Select CRMReceivable From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CostCentreId=" + argCCId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();

                int iCRMRecv = 0;
                if (dt.Rows.Count > 0)
                    iCRMRecv = Convert.ToInt32(dt.Rows[0]["CRMReceivable"]);
                else
                    iCRMRecv = 0;

                string sCond = "";
                string sDate = "";
                if (iCRMRecv == 1)
                {
                    sCond = "AND PSF.StageDetId<>0";
                    sDate = " AND SD.CompletionDate";
                }
                else
                {
                    sCond = "";
                    sDate = " AND PSF.SchDate";
                }

                if (argBusinessType == "B")
                {
                    sSql = "Select B.FlatId,B.FlatNo,LR.LeadName,[O/B]= -1*B.OBReceipt/" + dUnit + ",Advance=ISNULL(SUM(Advance),0)/" + dUnit + ", " +
                            " ExtraBillAmt=ISNULL(SUM(A.ExtraBillAmt),0)/" + dUnit + " FROM (  " +
                            " SELECT FD.FlatId,FD.FlatNo,OBReceipt=0,Advance=RT.Amount,ExtraBillAmt=0 FROM ReceiptTrans RT " +
                            " INNER JOIN ReceiptRegister RR ON RR.ReceiptId=RT.ReceiptId  " +
                            " INNER JOIN FlatDetails FD ON FD.FlatId=RT.FlatId " +
                            " WHERE PaymentAgainst='A' And ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' " +
                            " AND FD.BlockId=" + argBlockId + " AND FD.PayTypeId=" + argPayTypeId + " AND FD.CostCentreId=" + argCCId + "" +
                            " UNION ALL  " +
                            " SELECT FD.FlatId,FD.FlatNo,-1*OBReceipt OBReceipt,0 Advance,0 ExtraBillAmt FROM FlatDetails FD" +
                            " WHERE FD.BlockId=" + argBlockId + " AND FD.PayTypeId=" + argPayTypeId + " AND OBReceipt<>0 " + " AND FD.CostCentreId=" + argCCId + "" +
                            " UNION All " +
                            " SELECT FD.FlatId,FD.FlatNo,OBReceipt=0,Advance=0,ExtraBillAmt=RT.NetAmount FROM ExtraBillRegister RT " +
                            " INNER JOIN FlatDetails FD ON FD.FlatId=RT.FlatId " +
                            " WHERE RT.BillDate<='" + argDate.ToString("dd-MMM-yyyy") +
                            "' AND FD.BlockId=" + argBlockId + " AND FD.CostCentreId=" + argCCId + "" +
                            " UNION All " +
                            " SELECT FD.FlatId,FD.FlatNo,OBReceipt=0,Advance=0,0 ExtraBillAmt FROM FlatDetails FD " +
                            " WHERE FD.BlockId=" + argBlockId + " AND FD.CostCentreId=" + argCCId + " AND FD.OBReceipt=0 AND " +
                            " FD.FlatId NOT IN(Select FlatId from dbo.ReceiptTrans Where ReceiptType IN('ExtraBill', 'Advance')) " +
                            " ) A  LEFT JOIN FlatDetails B ON B.FlatId=A.FlatId " +
                            " INNER JOIN dbo.BlockMaster BM On BM.BlockId=B.BlockId " +
                            " INNER JOIN dbo.LevelMaster LM ON LM.LevelId=B.LevelId " +
                            " INNER JOIN dbo.LeadRegister LR ON LR.LeadId=B.LeadId " +
                            " Where B.LeadId<>0 And B.BlockId=" + argBlockId + " AND B.PayTypeId=" + argPayTypeId + " AND B.CostCentreId=" + argCCId + "" +
                            " GROUP BY BM.SortOrder,LM.SortOrder,B.SortOrder,B.FlatId,B.FlatNo,LR.LeadName,B.OBReceipt " +
                            " Order By BM.SortOrder,LM.SortOrder,B.SortOrder,dbo.Val(B.FlatNo)";
                }
                else
                {
                    sSql = "Select B.PlotDetailsId FlatId,B.PlotNo FlatNo, LR.LeadName, [O/B]=0, Advance=ISNULL(SUM(Advance),0)/" + dUnit + ", " +
                            " ExtraBillAmt=ISNULL(SUM(A.ExtraBillAmt),0)/" + dUnit + " FROM (  " +
                            " SELECT FD.PlotDetailsId FlatId, FD.PlotNo FlatNo, OBReceipt=0, Advance=RT.Amount, ExtraBillAmt=0 FROM ReceiptTrans RT " +
                            " INNER JOIN ReceiptRegister RR ON RR.ReceiptId=RT.ReceiptId  " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails FD ON FD.PlotDetailsId=RT.FlatId " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B ON FD.LandRegisterId=B.LandId " +
                            " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON B.ProjectName=OC.ProjectDB " +
                            " WHERE PaymentAgainst='PA' And ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' " +
                            " AND FD.PaymentScheduleId=" + argPayTypeId + " AND OC.CostCentreId=" + argCCId + "" +
                            " UNION ALL  " +
                            " SELECT FD.PlotDetailsId FlatId, FD.PlotNo FlatNo, 0 OBReceipt, "+
                            " 0 Advance, 0 ExtraBillAmt FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails FD" +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B ON FD.LandRegisterId=B.LandId " +
                            " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON B.ProjectName=OC.ProjectDB " +
                            " WHERE FD.PaymentScheduleId=" + argPayTypeId + " AND OC.CostCentreId=" + argCCId + "" +
                            " UNION All " +
                            " SELECT FD.PlotDetailsId FlatId, FD.PlotNo FlatNo, OBReceipt=0, Advance=0, ExtraBillAmt=RT.NetAmount FROM dbo.ExtraBillRegister RT " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails FD ON FD.PlotDetailsId=RT.FlatId " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B ON FD.LandRegisterId=B.LandId " +
                            " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON B.ProjectName=OC.ProjectDB " +
                            " WHERE RT.BillDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND OC.CostCentreId=" + argCCId + "" +
                            " UNION ALL " +
                            " SELECT FD.PlotDetailsId FlatId,FD.PlotNo FlatNo,OBReceipt=0, Advance=0, 0 ExtraBillAmt FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails FD " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B ON FD.LandRegisterId=B.LandId " +
                            " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON B.ProjectName=OC.ProjectDB " +
                            " WHERE OC.CostCentreId=" + argCCId + " AND FD.PlotDetailsId NOT IN(Select FlatId from dbo.ReceiptTrans Where ReceiptType IN('PlotExtraBill', 'PlotAdvance')) " +
                            " ) A "+
                            "  LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails B ON B.PlotDetailsId=A.FlatId " +
                            " INNER JOIN dbo.LeadRegister LR ON LR.LeadId=B.BuyerId " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister CR ON B.LandRegisterId=CR.LandId " +
                            " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON CR.ProjectName=OC.ProjectDB " +
                            " Where B.BuyerId<>0 AND B.PaymentScheduleId=" + argPayTypeId + " AND OC.CostCentreId=" + argCCId + "" +
                            " GROUP BY B.PlotDetailsId,B.PlotNo,LR.LeadName " +
                            " ORDER BY B.PlotNo,dbo.Val(B.PlotNo)";
                }
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                SqlDataReader sdreader = cmd.ExecuteReader();
                ds = new DataSet();
                ds.Load(sdreader, LoadOption.OverwriteChanges, "Flat");
                sdreader.Close();
                cmd.Dispose();

                if (argBusinessType == "B")
                {
                    sSql = "Select TemplateId,Description From dbo.PaymentSchedule " +
                           " Where CostCentreId=" + argCCId + " And TypeId=" + argPayTypeId + " ORDER BY SortOrder";
                }
                else
                {
                    sSql = "Select TemplateId,Description From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedule A " +
                           " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B ON A.LandRegId=B.LandId " +
                           " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C ON B.ProjectName=C.ProjectDB " +
                           " Where C.CostCentreId=" + argCCId + " AND A.TypeId=" + argPayTypeId + " ORDER BY SortOrder";
                }
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                sdreader = cmd.ExecuteReader();
                ds.Load(sdreader, LoadOption.OverwriteChanges, "Stages");
                sdreader.Close();
                cmd.Dispose();

                string sFromActual = string.Empty;
                if (argFromActual == 0)
                {
                    if (argBusinessType == "B")
                    {
                        sFromActual = " Select FD.FlatId,PS.TemplateId,0,Sum(RT.Amount) Received From dbo.ReceiptTrans RT " +
                                    " INNER JOIN dbo.ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                    " INNER JOIN dbo.PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                    " INNER JOIN dbo.FlatDetails FD ON FD.FlatId=RT.FlatId" +
                                    " RIGHT JOIN dbo.PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                    " WHERE RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PSF.CostCentreId=" + argCCId +
                                    " AND FD.PayTypeId=" + argPayTypeId + " AND FD.BlockId=" + argBlockId + " " +
                                    " AND RR.Cancel=0 GROUP BY FD.FlatId ,PS.TemplateId";
                    }
                    else
                    {
                        sFromActual = " Select FD.PlotDetailsId FlatId,PS.TemplateId,0,Sum(RT.Amount) Received From dbo.ReceiptTrans RT " +
                                    " INNER JOIN dbo.ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                    " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot PSF On RT.PaySchId=PSF.PaymentSchId" +
                                    " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails FD ON FD.PlotDetailsId=RT.FlatId" +
                                    " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B ON FD.LandRegisterId=B.LandId " +
                                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON B.ProjectName=OC.ProjectDB " +
                                    " RIGHT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                    " WHERE RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND OC.CostCentreId=" + argCCId +
                                    " AND PS.TypeId=" + argPayTypeId + " AND RR.Cancel=0 " +
                                    " GROUP BY FD.PlotDetailsId,PS.TemplateId";
                    }
                }
                else if (argFromActual == 1)
                {
                    if (argBusinessType == "B")
                    {
                        sFromActual = " Select FD.FlatId,PS.TemplateId,0,Sum(RT.Amount) Received From dbo.ReceiptTrans RT " +
                                     " INNER JOIN dbo.ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                     " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=RR.ReceiptId " +
                                     " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=RT.CostCentreId " +
                                     " INNER JOIN dbo.PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                     " INNER JOIN dbo.FlatDetails FD ON FD.FlatId=RT.FlatId" +
                                     " RIGHT JOIN dbo.PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                     " WHERE O.CRMActual=1 AND R.Cancel=0 And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") +
                                     "' AND PSF.CostCentreId=" + argCCId + " AND FD.PayTypeId=" + argPayTypeId + " AND FD.BlockId=" + argBlockId + " " +
                                     " GROUP BY FD.FlatId,PS.TemplateId";
                    }
                    else
                    {
                        sFromActual = " Select FD.PlotDetailsId FlatId,PS.TemplateId,0,Sum(RT.Amount) Received From dbo.ReceiptTrans RT " +
                                     " INNER JOIN dbo.ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                     " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=RR.ReceiptId " +
                                     " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot PSF On RT.PaySchId=PSF.PaymentSchId" +
                                     " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails FD ON FD.PlotDetailsId=RT.FlatId" +
                                     " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B ON FD.LandRegisterId=B.LandId " +
                                     " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON B.ProjectName=OC.ProjectDB " +
                                     " RIGHT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                     " WHERE OC.CRMActual=1 AND R.Cancel=0 And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' " +
                                     " AND OC.CostCentreId=" + argCCId + " AND PS.TypeId=" + argPayTypeId + " " +
                                     " GROUP BY FD.PlotDetailsId,PS.TemplateId";
                    }
                }
                else
                {
                    if (argBusinessType == "B")
                    {
                        sFromActual = " Select FD.FlatId,PS.TemplateId,0,Sum(RT.Amount) Received From dbo.ReceiptTrans RT " +
                                    " INNER JOIN dbo.ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                    " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=RR.ReceiptId " +
                                    " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=RT.CostCentreId " +
                                    " INNER JOIN dbo.PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                    " INNER JOIN dbo.FlatDetails FD ON FD.FlatId=RT.FlatId" +
                                    " RIGHT JOIN dbo.PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                    " WHERE O.CRMActual=2 AND R.BRS=1 And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PSF.CostCentreId=" + argCCId +
                                    " AND FD.PayTypeId=" + argPayTypeId + " AND FD.BlockId=" + argBlockId + " " +
                                    " GROUP BY FD.FlatId,PS.TemplateId";
                    }
                    else
                    {
                        sFromActual = " Select FD.PlotDetailsId FlatId,PS.TemplateId,0,Sum(RT.Amount) Received From dbo.ReceiptTrans RT " +
                                   " INNER JOIN dbo.ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                   " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=RR.ReceiptId " +
                                   " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot PSF On RT.PaySchId=PSF.PaymentSchId" +
                                   " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails FD ON FD.PlotDetailsId=RT.FlatId" +
                                   " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B ON FD.LandRegisterId=B.LandId " +
                                   " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON B.ProjectName=OC.ProjectDB " +
                                   " RIGHT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                   " WHERE OC.CRMActual=2 AND R.BRS=1 And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND OC.CostCentreId=" + argCCId +
                                   " AND PS.TypeId=" + argPayTypeId + " " +
                                   " GROUP BY FD.PlotDetailsId,PS.TemplateId";
                    }
                }

                if (argBusinessType == "B")
                {
                    sSql = "SELECT B.FlatId,B.FlatNo,TemplateId, SUM(Receivable)/" + dUnit + " Receivable,SUM(Received)/" + dUnit + " Received," +
                            "(SUM(Receivable)-SUM(Received))/" + dUnit + " Balance FROM  ( " +
                            " SELECT FD.FlatId,PS.TemplateId,ISNULL(SUM(PB.NetAmount),0) Receivable, 0 Received FROM dbo.ProgressBillRegister PB " +
                            " INNER JOIN dbo.FlatDetails FD ON FD.FlatId=PB.FlatId  " +
                            " INNER JOIN dbo.BuyerDetail BD ON FD.FlatId=BD.FlatId " +
                            " INNER JOIN dbo.LeadRegister LR ON LR.LeadId=FD.LeadId  " +
                            " INNER JOIN dbo.PaymentScheduleFlat PSF ON PSF.PaymentSchId=PB.PaySchId " +
                            " INNER JOIN dbo.PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId " +
                            " WHERE PSF.BillPassed=1 And PBDate<='" + argDate.ToString("dd-MMM-yyyy") + "' And FD.BlockId=" + argBlockId + " AND FD.CostCentreId=" + argCCId +
                            " AND FD.PayTypeId=" + argPayTypeId + " GROUP BY FD.FlatId,PS.TemplateId " +
                            " UNION ALL   " +
                            " SELECT FD.FlatId,PS.TemplateId,ISNULL(SUM(PSF.NetAmount),0) Receivable, 0 Received FROM dbo.PaymentScheduleFlat PSF" +
                            " INNER JOIN dbo.FlatDetails FD ON PSF.FlatId=FD.FlatId  " +
                            " INNER JOIN dbo.BuyerDetail BD ON FD.FlatId=BD.FlatId  " +
                            " INNER JOIN dbo.LeadRegister LR ON LR.LeadId=BD.LeadId  " +
                            " INNER JOIN dbo.PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId " +
                            " LEFT JOIN dbo.StageDetails SD ON PSF.StageDetId=SD.StageDetId " +
                            " LEFT JOIN dbo.AllotmentCancel AC ON PSF.FlatId=AC.FlatId AND AC.Approve='Y' " +
                            " WHERE PSF.BillPassed=0 " + sDate + "<='" + argDate.ToString("dd-MMM-yyyy") + "' " + sCond +
                            " " + sDate + ">Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                            " AND FD.BlockId=" + argBlockId + " AND FD.PayTypeId=" + argPayTypeId + " AND FD.CostCentreId=" + argCCId +
                            " GROUP BY FD.FlatId ,PS.TemplateId " +
                            " UNION ALL   " +
                            " " + sFromActual + " " +
                            " ) A  " +
                            " INNER JOIN dbo.FlatDetails B ON A.FlatId=B.FlatId " +
                            " Where B.BlockId=" + argBlockId + " AND B.CostCentreId=" + argCCId +
                            " GROUP BY B.FlatId,B.FlatNo,TemplateId";
                }
                else
                {
                    sSql = "SELECT B.PlotDetailsId FlatId,B.PlotNo FlatNo,A.TemplateId, SUM(Receivable)/" + dUnit + " Receivable,SUM(Received)/" + dUnit + " Received," +
                            " (SUM(Receivable)-SUM(Received))/" + dUnit + " Balance FROM  ( " +
                            " SELECT FD.PlotDetailsId FlatId,PS.TemplateId,ISNULL(SUM(PB.NetAmount),0) Receivable, 0 Received FROM dbo.PlotProgressBillRegister PB " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails FD ON FD.PlotDetailsId=PB.PlotDetailsId  " +
                            " INNER JOIN dbo.BuyerDetail BD ON FD.PlotDetailsId=BD.PlotId " +
                            " INNER JOIN dbo.LeadRegister LR ON LR.LeadId=FD.BuyerId  " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot PSF ON PSF.PaymentSchId=PB.PaySchId " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B ON FD.LandRegisterId=B.LandId " +
                            " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON B.ProjectName=OC.ProjectDB " +
                            " WHERE PSF.BillPassed=1 And PBDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND OC.CostCentreId=" + argCCId +
                            " AND PS.TypeId=" + argPayTypeId + " GROUP BY FD.PlotDetailsId,PS.TemplateId " +
                            " UNION ALL   " +
                            " SELECT FD.PlotDetailsId FlatId,PS.TemplateId,ISNULL(SUM(PSF.NetAmount),0) Receivable, 0 Received FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot PSF" +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails FD ON PSF.PlotDetailsId=FD.PlotDetailsId  " +
                            " INNER JOIN dbo.BuyerDetail BD ON FD.PlotDetailsId=BD.PlotId  " +
                            " INNER JOIN dbo.LeadRegister LR ON LR.LeadId=BD.LeadId  " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B ON FD.LandRegisterId=B.LandId " +
                            " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON B.ProjectName=OC.ProjectDB " +
                            " LEFT JOIN dbo.AllotmentCancel AC ON PSF.PlotDetailsId=AC.FlatId AND AC.Approve='Y' " +
                            " WHERE PSF.BillPassed=0 AND PSF.SchDate<='" + argDate.ToString("dd-MMM-yyyy") + "' " +
                            " AND PSF.SchDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                            " AND PS.TypeId=" + argPayTypeId + " AND OC.CostCentreId=" + argCCId +
                            " GROUP BY FD.PlotDetailsId,PS.TemplateId " +
                            " UNION ALL   " +
                            " " + sFromActual + " " +
                            " ) A  " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails B ON A.FlatId=B.PlotDetailsId " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister CR ON B.LandRegisterId=CR.LandId " +
                            " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OC ON CR.ProjectName=OC.ProjectDB " +
                            " Where OC.CostCentreId=" + argCCId +
                            " GROUP BY B.PlotDetailsId,B.PlotNo,A.TemplateId";
                }
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                sdreader = cmd.ExecuteReader();
                ds.Load(sdreader, LoadOption.OverwriteChanges, "Recv");
                sdreader.Close();
                cmd.Dispose();

                if (ds.Tables["Recv"].Rows.Count > 0)
                {
                    decimal dTotalRecv = 0;
                    decimal dTotalRevd = 0;
                    for (int i = 0; i < ds.Tables["Stages"].Rows.Count; i++)
                    {
                        string sStageName = ds.Tables["Stages"].Rows[i]["Description"].ToString();
                        int iTemplateId = (int)ds.Tables["Stages"].Rows[i]["TemplateId"];
                        int iFlatId = 0;
                        int iStageId = iTemplateId;

                        string sColName1 = iStageId + "- Recv";
                        DataColumn col1 = new DataColumn(sColName1) { DataType = typeof(decimal), DefaultValue = 0 };
                        ds.Tables["Flat"].Columns.Add(col1);

                        string sColName2 = iStageId + "- Recd";
                        DataColumn col2 = new DataColumn(sColName2) { DataType = typeof(decimal), DefaultValue = 0 };
                        ds.Tables["Flat"].Columns.Add(col2);

                        string sColName3 = iStageId + "- Bal";
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

                            DataRow[] drT = ds.Tables["Flat"].Select(String.Format("FlatId={0}", iFlatId));
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
                        //int j = 6;

                        //while (j < ds.Tables["Flat"].Columns.Count)
                        //{
                        //    dTotalRecv = dTotalRecv + (decimal)ds.Tables["Flat"].Rows[i][j];
                        //    if (j == 6)
                        //        dTotalRevd = dTotalRevd + (decimal)ds.Tables["Flat"].Rows[i][j] + (decimal)ds.Tables["Flat"].Rows[i][3];
                        //    else
                        //        dTotalRevd = dTotalRevd + (decimal)ds.Tables["Flat"].Rows[i][j + 1];

                        //    j = j + 3;
                        //}
                        int j = 6;

                        while (j < ds.Tables["Flat"].Columns.Count)
                        {
                            dTotalRecv = dTotalRecv + (decimal)ds.Tables["Flat"].Rows[i][j];
                            if (j == 7)
                                dTotalRevd = dTotalRevd + (decimal)ds.Tables["Flat"].Rows[i][j]; //+ (decimal)ds.Tables["Flat"].Rows[i][3];
                            else
                                dTotalRevd = dTotalRevd + (decimal)ds.Tables["Flat"].Rows[i][j + 1];

                            j = j + 3;
                        }

                        ds.Tables["Flat"].Rows[i]["TotalReceivable"] = dTotalRecv;
                        //ds.Tables["Flat"].Rows[i]["TotalReceived"] = (decimal)ds.Tables["Flat"].Rows[i]["O/B"] + (decimal)ds.Tables["Flat"].Rows[i]["Advance"] + (decimal)ds.Tables["Flat"].Rows[i]["ExtraBillAmt"] + dTotalRevd;
                        ds.Tables["Flat"].Rows[i]["TotalReceived"] = dTotalRevd;
                        ds.Tables["Flat"].Rows[i]["Balance"] = (decimal)ds.Tables["Flat"].Rows[i]["TotalReceivable"] - (decimal)ds.Tables["Flat"].Rows[i]["TotalReceived"];
                    }

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
            return ds;
        }

        public static DataSet GetBuyerStageRecReport(int argCCId, int argPayTypeId, DateTime argDate, int argFromActual, string argBusinessType)
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
            decimal dUnit = BsfGlobal.g_iSummaryUnit;

            try
            {
                sSql = "Select CRMReceivable From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Where CostCentreId=" + argCCId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();

                int iCRMRecv;
                if (dt.Rows.Count > 0)
                    iCRMRecv = Convert.ToInt32(dt.Rows[0]["CRMReceivable"]);
                else
                    iCRMRecv = 0;

                string sCond = "";
                string sDate = "";
                if (iCRMRecv == 1)
                {
                    sCond = "AND PSF.StageDetId<>0";
                    sDate = " AND SD.CompletionDate";
                }
                else
                {
                    sCond = "";
                    sDate = " AND PSF.SchDate";
                }

                dt.Dispose();

                sSql = "Select BM.BlockId,BM.BlockName,B.FlatId,LR.LeadName BuyerName,B.FlatNo,[O/B]= -1*B.OBReceipt/" + dUnit + ",Advance=ISNULL(SUM(Advance),0)/" + dUnit + ", " +
                        " ExtraBillAmt=ISNULL(SUM(A.ExtraBillAmt),0)/" + dUnit + " FROM (  " +
                        " SELECT FD.FlatId,FD.FlatNo,OBReceipt=0,Advance=RT.Amount,ExtraBillAmt=0 FROM ReceiptTrans RT " +
                        " LEFT JOIN ReceiptRegister RR ON RR.ReceiptId=RT.ReceiptId  INNER JOIN FlatDetails FD ON FD.FlatId=RT.FlatId  " +
                        " WHERE  PaymentAgainst='A' And ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND FD.CostCentreId=" + argCCId + " And FD.PayTypeId=" + argPayTypeId + " " +
                        " UNION ALL  " +
                        " SELECT FD.FlatId,FD.FlatNo,-1*OBReceipt,0,0 FROM FlatDetails FD WHERE FD.CostCentreId=" + argCCId + " And FD.PayTypeId=" + argPayTypeId + " " +
                        " UNION All " +
                        " SELECT FD.FlatId,FD.FlatNo,OBReceipt=0,Advance=0,ExtraBillAmt=RT.NetAmount FROM ExtraBillRegister RT "+
                        " INNER JOIN FlatDetails FD ON FD.FlatId=RT.FlatId "+
                        " WHERE BillDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND FD.CostCentreId=" + argCCId + "  And FD.PayTypeId=" + argPayTypeId + " " +
                        " ) A  INNER JOIN FlatDetails B ON B.FlatId=A.FlatId INNER JOIN BlockMaster BM ON BM.BlockId=B.BlockId  " +
                        " INNER JOIN LeadRegister LR ON LR.LeadId=B.LeadId " +
                        " INNER JOIN dbo.LevelMaster LM ON LM.LevelId=B.LevelId " +
                        " Where B.LeadId<>0 And B.CostCentreId=" + argCCId + " AND B.PayTypeId=" + argPayTypeId + " " +
                        " GROUP BY BM.SortOrder,LM.SortOrder,B.SortOrder,BM.BlockId,BM.BlockName,B.FlatId,LR.LeadName,B.FlatNo,B.OBReceipt " +
                        " Order By BM.SortOrder,LM.SortOrder,B.SortOrder,dbo.Val(B.FlatNo) ";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                ds.Load(sdr, LoadOption.OverwriteChanges, "Flat");
                sdr.Close();

                sSql = "Select TemplateId,Description From PaymentSchedule Where CostCentreId=" + argCCId + " And TypeId=" + argPayTypeId + " ORDER BY SortOrder";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                ds.Load(sdr, LoadOption.OverwriteChanges, "Stages");
                sdr.Close();
                cmd.Dispose();

                if (argFromActual == 0)
                {
                    sFromActual = " Select FD.FlatId,PS.TemplateId,0,Sum(RT.Amount) Received From ReceiptTrans RT " +
                                " INNER JOIN ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                " INNER JOIN PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                " INNER JOIN FlatDetails FD ON FD.FlatId=PSF.FlatId" +
                                " RIGHT JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                " WHERE RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PSF.CostCentreId=" + argCCId + " AND FD.PayTypeId=" + argPayTypeId + " " +
                                " AND RR.Cancel=0 GROUP BY FD.FlatId ,PS.TemplateId";
                }
                else if (argFromActual == 1)
                {

                    sFromActual = " Select FD.FlatId,PS.TemplateId,0,Sum(RT.Amount) Received From ReceiptTrans RT " +
                                 " INNER JOIN ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                 " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=RR.ReceiptId " +
                                 " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=RT.CostCentreId " +
                                 " INNER JOIN PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                 " INNER JOIN FlatDetails FD ON FD.FlatId=PSF.FlatId" +
                                 " RIGHT JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                 " WHERE O.CRMActual=1 AND R.Cancel=0 And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") +
                                 "' AND PSF.CostCentreId=" + argCCId + " AND FD.PayTypeId=" + argPayTypeId + " " +
                                 " GROUP BY FD.FlatId ,PS.TemplateId";
                }
                else
                {
                    sFromActual = " Select FD.FlatId,PS.TemplateId,0,Sum(RT.Amount) Received From ReceiptTrans RT " +
                                " INNER JOIN ReceiptRegister RR On RR.ReceiptId=RT.ReceiptId" +
                                " INNER JOIN [" + BsfGlobal.g_sFaDBName + "].dbo.ReceiptRegister R ON R.ReferenceId=RR.ReceiptId " +
                                " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O ON O.CostCentreId=RT.CostCentreId " +
                                " INNER JOIN PaymentScheduleFlat PSF On RT.PaySchId=PSF.PaymentSchId" +
                                " INNER JOIN FlatDetails FD ON FD.FlatId=PSF.FlatId" +
                                " RIGHT JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId" +
                                " WHERE O.CRMActual=2 AND R.BRS=1 And RR.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") +
                                "' AND PSF.CostCentreId=" + argCCId + " AND FD.PayTypeId=" + argPayTypeId + " " +
                                " GROUP BY FD.FlatId ,PS.TemplateId";
                }

                sSql = "SELECT B.FlatId,B.FlatNo,B.BlockId,D.BlockName,C.LeadName BuyerName,TemplateId, SUM(Receivable)/" + dUnit + " Receivable,   SUM(Received)/" + dUnit + " Received ,(SUM(Receivable)-SUM(Received))/" + dUnit + " Balance " +
                        " FROM  ( " +
                        " SELECT FD.FlatId,PS.TemplateId,SUM(PB.NetAmount) Receivable,0 Received   " +
                        " FROM ProgressBillRegister PB INNER JOIN FlatDetails FD ON FD.FlatId=PB.FlatId  " +
                        " INNER JOIN BuyerDetail BD ON PB.FlatId=BD.FlatId  INNER JOIN LeadRegister LR ON LR.LeadId=FD.LeadId  " +
                        " INNER JOIN PaymentScheduleFlat PSF ON PSF.PaymentSchId=PB.PaySchId " +
                        " INNER JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId " +
                        " WHERE PSF.BillPassed=1 And PBDate<='" + argDate.ToString("dd-MMM-yyyy") + "' And FD.CostCentreId=" + argCCId +
                        " AND FD.PayTypeId=" + argPayTypeId + " GROUP BY FD.FlatId,PS.TemplateId " +
                        " UNION ALL   " +
                        " SELECT FD.FlatId,PS.TemplateId,SUM(PSF.NetAmount) Receivable,0    " +
                        " FROM PaymentScheduleFlat PSF INNER JOIN FlatDetails FD ON PSF.FlatId=FD.FlatId  " +
                        " INNER JOIN BuyerDetail BD ON PSF.FlatId=BD.FlatId   INNER JOIN LeadRegister LR ON LR.LeadId=BD.LeadId  " +
                        " INNER JOIN PaymentSchedule PS ON PS.TemplateId=PSF.TemplateId " +
                        " LEFT JOIN dbo.StageDetails SD ON PSF.StageDetId=SD.StageDetId " +
                        " LEFT JOIN dbo.AllotmentCancel AC ON PSF.FlatId=AC.FlatId AND AC.Approve='Y' " +
                        " WHERE PSF.BillPassed=0 " + sDate + "<='" + argDate.ToString("dd-MMM-yyyy") + "' " + sCond +
                        " " + sDate + ">Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                        " AND FD.CostCentreId=" + argCCId + " AND FD.PayTypeId=" + argPayTypeId + "  " +
                        " GROUP BY FD.FlatId ,PS.TemplateId " +
                        " UNION ALL   " +
                        " " + sFromActual + " " +
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

        public static DataSet GetCCSOADet(int argCCId, DateTime argAsOn)
        {
            DataSet dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            string frmdat = string.Format("{0:dd MMM yyyy}", argAsOn);
            string sSql = "";
            try
            {
                sSql = "SELECT A.SortOrder,PaymentSchId, [Description] ,Case When PBDate is Null Then convert(varchar(10),SchDate,103) Else convert(varchar(10),PBDate,103) End SchDate,Convert(Varchar(10), " +
                        " B.FinaliseDate,103) FinaliseDate,isnull(Amount,0) Amount,cast(0 as decimal(18,3)) as Interest,cast(0 as int) as Age,Tax=(SELECT  isnull(SUM(A1.Amount),0) Amount " +
                        " FROM FlatReceiptQualifier A1 INNER JOIN dbo.FlatReceiptType A2 ON A1.SchId=A2.SchId WHERE A.PaymentSchId=A2.PaymentSchId),A.NetAmount,F.CreditDays,F.IntPercent,A.SchType," +
                        " AdvSchType=Isnull((Select X.SchType From FlatReceiptType X Where X.FlatId=F.FlatId And X.SchType='A' And X.PaymentSchId=A.PaymentSchId),'') " +
                        " FROM PaymentScheduleFlat A INNER JOIN BuyerDetail B On A.FlatId=B.FlatId " +
                        " INNER JOIN FlatDetails F ON F.FlatId=B.FlatId LEFT JOIN dbo.ProgressBillRegister PR ON PR.PaySchId=A.PaymentSchId " +
                        " WHERE F.CostCentreId=" + argCCId + " " +//And A.SchDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' " +
                        " ORDER BY A.SortOrder ASC";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataSet();
                sda.Fill(dt, "PaymentSch");

                sSql = "SELECT PaySchId,B.ReceiptDate,isnull(A.Amount,0) PaidGrossAmount,Cast(0  as decimal(18,3)) PaidTaxAmount,cast(0 as decimal(18,3)) as PaidInterest," +
                        " cast(0 as decimal(18,3)) Balance FROM ReceiptTrans A  INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId INNER JOIN dbo.FlatDetails F ON F.FlatId=A.FlatId " +
                        " WHERE F.BlockId=" + argCCId + " AND B.ReceiptDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' AND ReceiptType='Advance' " +
                        " UNION ALL " +
                        " SELECT PaymentSchId,ReceiptDate,PaidGrossAmount,PaidTaxAmount,PaidInterest,cast(0 as decimal(18,3)) Balance FROM( " +
                        " SELECT PaymentSchId,B.ReceiptDate,isnull(SUM(PaidGrossAmount),0) PaidGrossAmount, " +
                        " isnull(SUM(PaidTaxAmount),0) PaidTaxAmount,cast(0 as decimal(18,3)) PaidInterest " +
                        " FROM ReceiptShTrans A  INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId INNER JOIN dbo.FlatDetails F ON F.FlatId=A.FlatId " +
                        " WHERE F.CostCentreId=" + argCCId + " " +//AND B.ReceiptDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' "+
                        " AND PaidNetAmount<>0 and ReceiptTypeId<>1 " +
                        " GROUP BY PaymentSchId,B.ReceiptDate)A";

                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dt, "Receivable");
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

        public static DataSet GetBlockSOADet(int argBlockId, DateTime argAsOn)
        {
            DataSet dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            string frmdat = string.Format("{0:dd MMM yyyy}", argAsOn);
            string sSql = "";
            try
            {
                sSql = "SELECT A.SortOrder,PaymentSchId, [Description] ,Case When PBDate is Null Then convert(varchar(10),SchDate,103) Else convert(varchar(10),PBDate,103) End SchDate,Convert(Varchar(10), " +
                        " B.FinaliseDate,103) FinaliseDate,isnull(Amount,0) Amount,cast(0 as decimal(18,3)) as Interest,cast(0 as int) as Age,Tax=(SELECT  isnull(SUM(A1.Amount),0) Amount " +
                        " FROM FlatReceiptQualifier A1 INNER JOIN dbo.FlatReceiptType A2 ON A1.SchId=A2.SchId WHERE A.PaymentSchId=A2.PaymentSchId),A.NetAmount,F.CreditDays,F.IntPercent,A.SchType," +
                        " AdvSchType=Isnull((Select X.SchType From FlatReceiptType X Where X.FlatId=F.FlatId And X.SchType='A' And X.PaymentSchId=A.PaymentSchId),'') " +
                        " FROM PaymentScheduleFlat A INNER JOIN BuyerDetail B On A.FlatId=B.FlatId " +
                        " INNER JOIN FlatDetails F ON F.FlatId=B.FlatId LEFT JOIN dbo.ProgressBillRegister PR ON PR.PaySchId=A.PaymentSchId " +
                        " WHERE F.BlockId=" + argBlockId + " " +//And A.SchDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' " +
                        " ORDER BY A.SortOrder ASC";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataSet();
                sda.Fill(dt, "PaymentSch");

                sSql = "SELECT PaySchId,B.ReceiptDate,isnull(A.Amount,0) PaidGrossAmount,Cast(0  as decimal(18,3)) PaidTaxAmount,cast(0 as decimal(18,3)) as PaidInterest," +
                        " cast(0 as decimal(18,3)) Balance FROM ReceiptTrans A  INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId INNER JOIN dbo.FlatDetails F ON F.FlatId=A.FlatId " +
                        " WHERE F.BlockId=" + argBlockId + " AND B.ReceiptDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' AND ReceiptType='Advance' " +
                        " UNION ALL " +
                        " SELECT PaymentSchId,ReceiptDate,PaidGrossAmount,PaidTaxAmount,PaidInterest,cast(0 as decimal(18,3)) Balance FROM( " +
                        " SELECT PaymentSchId,B.ReceiptDate,isnull(SUM(PaidGrossAmount),0) PaidGrossAmount, " +
                        " isnull(SUM(PaidTaxAmount),0) PaidTaxAmount,cast(0 as decimal(18,3)) PaidInterest " +
                        " FROM ReceiptShTrans A  INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId INNER JOIN dbo.FlatDetails F ON F.FlatId=A.FlatId " +
                        " WHERE F.BlockId=" + argBlockId + " " +//AND B.ReceiptDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' "+
                        " AND PaidNetAmount<>0 and ReceiptTypeId<>1 " +
                        " GROUP BY PaymentSchId,B.ReceiptDate)A";

                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dt, "Receivable");
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

        public static DataSet GetSOADet(int argFlatId, DateTime argAsOn)
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


                sSql = "SELECT A.SortOrder,A.PaymentSchId,A.[Description],CASE WHEN PBDate IS NULL THEN CONVERT(VARCHAR(10),SchDate,103) "+
                        " ELSE CONVERT(VARCHAR(10),PBDate,103) END SchDate,Convert(Varchar(10),B.FinaliseDate,103) FinaliseDate, "+
                        " ISNULL(Amount,0) Amount,CAST(0 AS DECIMAL(18,3)) as Interest,CAST(0 AS INT) as Age," +
                        " Tax=(SELECT ISNULL(SUM(A1.Amount),0) Amount FROM FlatReceiptQualifier A1 " +
                        " INNER JOIN dbo.FlatReceiptType A2 ON A1.SchId=A2.SchId WHERE A.PaymentSchId=A2.PaymentSchId), "+
                        " A.NetAmount,F.CreditDays,F.IntPercent,A.SchType," +
                        " AdvSchType=ISNULL((Select X.SchType From FlatReceiptType X Where X.FlatId=F.FlatId And X.SchType='A' And X.PaymentSchId=A.PaymentSchId),'') " +
                        " FROM PaymentScheduleFlat A INNER JOIN BuyerDetail B On A.FlatId=B.FlatId " +
                        " INNER JOIN FlatDetails F ON F.FlatId=B.FlatId "+
                        " LEFT JOIN dbo.ProgressBillRegister PR ON PR.PaySchId=A.PaymentSchId " +
                        " WHERE A.FlatId=" + argFlatId + " " +//And A.SchDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' " +
                        " ORDER BY A.SortOrder ASC";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataSet();
                sda.Fill(dt, "PaymentSch");
                sda.Dispose();

                sSql = "SELECT A.PaySchId,B.ReceiptDate,ISNULL(A.Amount,0) PaidGrossAmount,CAST(0  as decimal(18,3)) PaidTaxAmount, " +
                        " CAST(0 as decimal(18,3)) as PaidInterest,CAST(0 as decimal(18,3)) Balance FROM ReceiptTrans A " +
                        " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                        " WHERE A.FlatId=" + argFlatId + " AND B.ReceiptDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' AND ReceiptType='Advance' " +
                        " UNION ALL " +
                        " SELECT PaymentSchId,ReceiptDate,PaidGrossAmount,PaidTaxAmount,PaidInterest,CAST(0 as decimal(18,3)) Balance FROM( " +
                        " SELECT A.PaymentSchId,B.ReceiptDate,ISNULL(SUM(A.PaidGrossAmount),0) PaidGrossAmount, " +
                        " ISNULL(SUM(A.PaidTaxAmount),0) PaidTaxAmount,CAST(0 as decimal(18,3)) PaidInterest " +
                        " FROM ReceiptShTrans A  INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                        " WHERE A.FlatId=" + argFlatId + " " +//AND B.ReceiptDate<='" + argAsOn.ToString("dd-MMM-yyyy") + "' "+
                        " AND A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1 " +
                        " GROUP BY PaymentSchId,B.ReceiptDate) A";
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

        public static DataTable GetStages(int argBlockId)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            try
            {
                dt = new DataTable();
                sSql = "Select Distinct Description From PaymentScheduleFlat A "+
                    " Inner Join dbo.FlatDetails B On A.FlatId=B.FlatId Where B.BlockId=" + argBlockId + " ";
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

        public static DataSet GetCCReceiptType(int argCCId, DateTime argDate)
        {
            DataSet ds = new DataSet();
            string sSql = "";

            cRateQualR RAQual = new cRateQualR();
            Collection QualVBC = new Collection();

            BsfGlobal.OpenCRMDB();

            sSql = "Update FlatReceiptType Set Amount = NetAmount Where Amount=0 And NetAmount>0";
            SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            SqlDataReader dr;

            int iPaySchId = 0;
            int iQualId = 0; string sQualName = "";
            int iCCId = argCCId;

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
            dtQualifier.Columns.Add("CCId", typeof(int));
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

            sSql = "Select A.ReceiptTypeId,A.OtherCostId,Case When A.ReceiptTypeId <>0 then B.ReceiptTypeName Else C.OtherCostName End Description,SUM(A.Amount)Amount From FlatReceiptType A " +
                      "Left Join ReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId " +
                      "Left Join OtherCostMaster C on A.OtherCostId=C.OtherCostId Inner Join dbo.FlatDetails F ON F.FlatId=A.FlatId Where F.CostCentreId=" + argCCId + " " +
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
                    " Inner Join dbo.FlatDetails F ON F.FlatId=A.FlatId Where F.CostCentreId= " + argCCId + " " +
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
                        " Inner Join dbo.FlatDetails F ON F.FlatId=A.FlatId Where F.CostCentreId=" + argCCId + " " +
                        " And A.ReceiptTypeId= " + iReceiptId + " And A.OtherCostId= " + iOthId + " And P.CostCentreId = " + argCCId;
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

                            dr1["CCId"] = iCCId;
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

        public static DataSet GetAsOnCCReceiptType(int argCCId,DateTime argDate)
        {
            DataSet ds = new DataSet();
            string sSql = "";

            cRateQualR RAQual = new cRateQualR();
            Collection QualVBC = new Collection();

            BsfGlobal.OpenCRMDB();

            sSql = "Update FlatReceiptType Set Amount = NetAmount Where Amount=0 And NetAmount>0";
            SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            SqlDataReader dr;

            int iPaySchId = 0;
            int iQualId = 0; string sQualName = "";
            int iCCId = argCCId;

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
            dtQualifier.Columns.Add("CCId", typeof(int));
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

            sSql = "Select A.ReceiptTypeId,A.OtherCostId,Case When A.ReceiptTypeId <>0 then B.ReceiptTypeName Else C.OtherCostName End Description,SUM(A.Amount)Amount From FlatReceiptType A " +
                      " Left Join ReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId " +
                      " Left Join OtherCostMaster C on A.OtherCostId=C.OtherCostId " +
                      " Left Join dbo.PaymentScheduleFlat P On P.PaymentSchId=A.PaymentSchId Inner Join dbo.FlatDetails F ON F.FlatId=A.FlatId " +
                      " Where F.CostCentreId= " + argCCId + " And P.SchDate<='" + Convert.ToDateTime(argDate).ToString("dd-MMM-yyyy") + "'" +
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
                    " Inner Join dbo.FlatDetails F ON F.FlatId=A.FlatId " +
                    " Where F.CostCentreId= " + argCCId + " " +
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
                        " Inner Join dbo.FlatDetails F ON F.FlatId=A.FlatId " +
                        " Where F.CostCentreId=" + argCCId + " And A.ReceiptTypeId= " + iReceiptId + " And A.OtherCostId= " + iOthId + " And P.CostCentreId = " + argCCId;
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

                            dr1["CCId"] = iCCId;
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

        public static DataSet GetBlockReceiptType(int argCCId, int argBlockId, DateTime argDate)
        {
            DataSet ds = new DataSet();
            string sSql = "";

            cRateQualR RAQual = new cRateQualR();
            Collection QualVBC = new Collection();

            BsfGlobal.OpenCRMDB();

            sSql = "Update FlatReceiptType Set Amount = NetAmount Where Amount=0 And NetAmount>0";
            SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            SqlDataReader dr;

            int iPaySchId = 0;
            int iQualId = 0; string sQualName = "";
            int iBlockId = argBlockId;

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
            dtQualifier.Columns.Add("BlockId", typeof(int));
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

            sSql = "Select A.ReceiptTypeId,A.OtherCostId,Case When A.ReceiptTypeId <>0 then B.ReceiptTypeName Else C.OtherCostName End Description,SUM(A.Amount)Amount From FlatReceiptType A " +
                      "Left Join ReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId " +
                      "Left Join OtherCostMaster C on A.OtherCostId=C.OtherCostId Inner Join dbo.FlatDetails F ON F.FlatId=A.FlatId Where F.BlockId=" + argBlockId + " " +
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
                    " Left Join OtherCostMaster C on A.OtherCostId=C.OtherCostId "+
                    " Inner Join dbo.FlatDetails F ON F.FlatId=A.FlatId Where F.BlockId= " + iBlockId + " " +
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
                        " Inner Join dbo.FlatDetails F ON F.FlatId=A.FlatId Where F.BlockId=" + argBlockId + " " +
                        " And A.ReceiptTypeId= " + iReceiptId + " And A.OtherCostId= " + iOthId + " And P.CostCentreId = " + argCCId;
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

                            dr1["BlockId"] = iBlockId;
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

        public static DataSet GetAsOnBlockReceiptType(int argCCId, int argBlockId, DateTime argDate)
        {
            DataSet ds = new DataSet();
            string sSql = "";

            cRateQualR RAQual = new cRateQualR();
            Collection QualVBC = new Collection();

            BsfGlobal.OpenCRMDB();

            sSql = "Update FlatReceiptType Set Amount = NetAmount Where Amount=0 And NetAmount>0";
            SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            SqlDataReader dr;

            int iPaySchId = 0;
            int iQualId = 0; string sQualName = "";
            int iBlockId = argBlockId;

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
            dtQualifier.Columns.Add("BlockId", typeof(int));
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

            sSql = "Select A.ReceiptTypeId,A.OtherCostId,Case When A.ReceiptTypeId <>0 then B.ReceiptTypeName Else C.OtherCostName End Description,SUM(A.Amount)Amount From FlatReceiptType A " +
                      " Left Join ReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId " +
                      " Left Join OtherCostMaster C on A.OtherCostId=C.OtherCostId " +
                      " Left Join dbo.PaymentScheduleFlat P On P.PaymentSchId=A.PaymentSchId Inner Join dbo.FlatDetails F ON F.FlatId=A.FlatId " +
                      " Where F.BlockId= " + argBlockId + " And P.SchDate<='" + Convert.ToDateTime(argDate).ToString("dd-MMM-yyyy") + "'" +
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
                    " Inner Join dbo.FlatDetails F ON F.FlatId=A.FlatId " +
                    " Where F.BlockId= " + argBlockId + " " +
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
                        " Inner Join dbo.FlatDetails F ON F.FlatId=A.FlatId " +
                        " Where F.BlockId=" + argBlockId + " And A.ReceiptTypeId= " + iReceiptId + " And A.OtherCostId= " + iOthId + " And P.CostCentreId = " + argCCId;
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

                            dr1["BlockId"] = iBlockId;
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

        #endregion

        #region Sales Report

        public static DataTable GetBlockwiseSalesReport()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "Select CostCentreId,CostCentreName,CRMActual From dbo.OperationalCostCentre" +
                        " Where ProjectDB In(Select ProjectName From " +
                        " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType IN('B'))" +
                        " and CostCentreId Not In (Select CostCentreId From dbo.UserCostCentreTrans " +
                        " Where UserId=" + BsfGlobal.g_lUserId + ") Order By CostCentreName";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
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

        public static DataTable GetCustomerSalesReport()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "Select CostCentreId,CostCentreName,CRMActual From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                        " Where ProjectDB In(Select ProjectName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType IN('B'))" +
                        " and CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                        " Where UserId=" + BsfGlobal.g_lUserId + ") Order By CostCentreName";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
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

        public static DataTable GetLevelwiseSalesReport()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "Select CostCentreId,CostCentreName,CRMActual From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                        " Where ProjectDB In(Select ProjectName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType IN('B'))" +
                        " and CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                        " Where UserId=" + BsfGlobal.g_lUserId + ") Order By CostCentreName";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                dt = new DataTable();
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

        #endregion
    }
}
