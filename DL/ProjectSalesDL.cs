using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace CRM.DataLayer
{
    class ProjectSalesDL
    {
        internal static DataTable Get_Project_Sales(DateTime argAsOnDate)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            try
            {
                dt = new DataTable();
                //sSql = "SELECT A.ProjectId,A.ProjectName,A.ProjectDB,A.SoldFlat,A.UnSoldFlat,(A.SoldFlat+A.UnSoldFlat) TotalFlat,  " +
                //        " A.SoldArea,A.UnSoldArea, (A.SoldArea+A.UnSoldArea) TotalArea, A.SoldAmt,A.UnSoldAmt ,  " +
                //        " (A.SoldAmt+A.UnSoldAmt) TotalAmt FROM ( " +
                //        " SELECT A.CostCentreId ProjectId,A.CostCentreName ProjectName,A.ProjectDB, " +
                //        " SUM(Case When B.Status='S' And B.LeadId<>0 Then 1 Else 0 End) as SoldFlat, " +
                //        " SUM(Case When B.Status='U' Then 1 Else 0 End) as UnSoldFlat,   " +
                //        " SUM(Case When B.Status='S' And B.LeadId<>0 Then Area Else 0 End) as SoldArea, " +
                //        " SUM(Case When B.Status='U' Then Area Else 0 End) as UnSoldArea,   " +
                //        " SUM(Case When B.Status='S' And B.LeadId<>0 Then NetAmt Else 0 End)+SUM(Case When B.Status='S' And C.LeadId<>0 Then QualifierAmt Else 0 End) as SoldAmt,  " +
                //        " SUM(Case When B.Status='U' Then NetAmt Else 0 End)+SUM(Case When B.Status='U' Then QualifierAmt Else 0 End) as UnSoldAmt   " +
                //        " FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre A INNER JOIN FlatDetails B ON A.CostCentreId=B.CostCentreId  " +
                //        " Left Join BuyerDetail C On C.FlatId=B.FlatId And C.FinaliseDate<='" + argAsOnDate.ToString("dd-MMM-yyyy") + "' " +
                //        " GROUP By A.CostCentreId,A.CostCentreName,A.ProjectDB ) A ";
                sSql = "SELECT A.CostCentreId,B.CostCentreName,SUM(A.SoldFlat)SoldFlat,SUM(A.UnSoldFlat)UnSoldFlat,SUM(A.SoldFlat+A.UnSoldFlat) TotalFlat, SUM( A.SoldArea)SoldArea,SUM(A.UnSoldArea)UnSoldArea,  " +
                        " SUM(A.SoldArea+A.UnSoldArea) TotalArea,SUM( A.SoldAmt)SoldAmt,SUM(A.UnSoldAmt)UnSoldAmt , SUM (A.SoldAmt+A.UnSoldAmt) TotalAmt FROM (  " +
                        " Select A.CostCentreId,Count(A.FlatId) SoldFlat,0 UnSoldFlat,SUM(Area)SoldArea,0 UnsoldArea,Sum(NetAmt)+Sum(QualifierAmt)SoldAmt,0 UnSoldAmt From FlatDetails A " +
                        " Inner Join BuyerDetail B On A.FlatId=B.FlatId " +
                        " Where A.Status='S' And B.FinaliseDate<='" + argAsOnDate.ToString("dd-MMM-yyyy") + "' Group By A.CostCentreId " +
                        " UNION ALL " +
                        " Select CostCentreId,0,Count(A.FlatId) UnSoldFlat,0 SoldArea,SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt From FlatDetails A " +
                        " Where A.Status='U' Group By CostCentreId)A INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B On A.CostCentreId=B.CostCentreId " +
                        " And B.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                        " Group By A.CostCentreId,B.CostCentreName Order By B.CostCentreName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "FA", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
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
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            try
            {
                dt = new DataTable();
                //sSql = "SELECT A.BlockId,A.BlockName,A.SoldFlat,A.UnSoldFlat,(A.SoldFlat+A.UnSoldFlat) TotalFlat,  A.SoldArea,A.UnSoldArea, " +
                //        " (A.SoldArea+A.UnSoldArea) TotalArea, A.SoldAmt,A.UnSoldAmt ,  (A.SoldAmt+A.UnSoldAmt) TotalAmt FROM ( " +
                //        " SELECT B.BlockId,C.BlockName,  SUM(Case When B.Status='S' Then 1 Else 0 End) as SoldFlat, " +
                //        " SUM(Case When B.Status='U' Then 1 Else 0 End) as UnSoldFlat,   " +
                //        " SUM(Case When B.Status='S' And D.LeadId<>0 Then Area Else 0 End) as SoldArea,  " +
                //        " SUM(Case When B.Status='U' Then Area Else 0 End) as UnSoldArea, " +
                //        " SUM(Case When B.Status='S' And D.LeadId<>0 Then NetAmt Else 0 End)+SUM(Case When B.Status='S' And D.LeadId<>0 Then QualifierAmt Else 0 End) as SoldAmt,  " +
                //        " SUM(Case When B.Status='U' Then NetAmt Else 0 End)+SUM(Case When B.Status='U' Then QualifierAmt Else 0 End) as UnSoldAmt   " +
                //        " FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre A INNER JOIN FlatDetails B ON A.CostCentreId=B.CostCentreId  " +
                //        " INNER JOIN BlockMaster C ON C.BlockId=B.BlockId " +
                //        " Left Join dbo.BuyerDetail D On D.FlatId=B.FlatId And FinaliseDate<='" + argAsOnDate.ToString("dd-MMM-yyyy") + "' " +
                //        " WHERE A.CostCentreId=" + arg_iProjectId + " GROUP By B.BlockId,C.BlockName) A ";
                sSql = "SELECT A.BlockId,B.BlockName,SUM(A.SoldFlat)SoldFlat,SUM(A.UnSoldFlat)UnSoldFlat,SUM(A.SoldFlat+A.UnSoldFlat) TotalFlat, SUM( A.SoldArea)SoldArea,SUM(A.UnSoldArea)UnSoldArea,  " +
                        " SUM(A.SoldArea+A.UnSoldArea) TotalArea,SUM( A.SoldAmt)SoldAmt,SUM(A.UnSoldAmt)UnSoldAmt , SUM (A.SoldAmt+A.UnSoldAmt) TotalAmt FROM (  " +
                        " Select BlockId,Count(A.FlatId) SoldFlat,0 UnSoldFlat,SUM(Area)SoldArea,0 UnsoldArea,Sum(NetAmt)+Sum(QualifierAmt)SoldAmt,0 UnSoldAmt From FlatDetails A " +
                        " Inner Join BuyerDetail B On A.FlatId=B.FlatId " +
                        " Where A.Status='S' And A.CostCentreId=" + arg_iProjectId + " And B.FinaliseDate<='" + argAsOnDate.ToString("dd-MMM-yyyy") + "' Group By BlockId " +
                        " UNION ALL " +
                        " Select BlockId,0,Count(A.FlatId) UnSoldFlat,0 SoldArea,SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt From FlatDetails A " +
                        " Where A.Status='U' And A.CostCentreId=" + arg_iProjectId + "  Group By BlockId)A INNER JOIN BlockMaster B On A.BlockId=B.BlockId Group By A.BlockId,B.BlockName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "FA", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
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
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            try
            {
                dt = new DataTable();
                sSql = "SELECT A.LevelId,B.LevelName,SUM(A.SoldFlat)SoldFlat,SUM(A.UnSoldFlat)UnSoldFlat,SUM(A.SoldFlat+A.UnSoldFlat) TotalFlat, " +
                        " SUM( A.SoldArea)SoldArea,SUM(A.UnSoldArea)UnSoldArea,   SUM(A.SoldArea+A.UnSoldArea) TotalArea,SUM( A.SoldAmt)SoldAmt, " +
                        " SUM(A.UnSoldAmt)UnSoldAmt , SUM (A.SoldAmt+A.UnSoldAmt) TotalAmt FROM (Select LevelId,Count(A.FlatId) SoldFlat, " +
                        " 0 UnSoldFlat,SUM(Area)SoldArea,0 UnsoldArea,Sum(NetAmt)+Sum(QualifierAmt)SoldAmt,0 UnSoldAmt From FlatDetails A  " +
                        " Inner Join BuyerDetail B On A.FlatId=B.FlatId  Where A.Status='S' And A.CostCentreId=" + arg_iProjectId + " And A.BlockId=" + arg_iBlockId + " " +
                        " And B.FinaliseDate<='" + argAsOnDate.ToString("dd-MMM-yyyy") + "' " +
                        " Group By LevelId  " +
                        " UNION ALL  " +
                        " Select LevelId,0,Count(A.FlatId) UnSoldFlat,0 SoldArea,SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt " +
                        " From FlatDetails A  Where A.Status='U' And A.CostCentreId=" + arg_iProjectId + " And A.BlockId=" + arg_iBlockId + " " +
                        " Group By LevelId)A INNER JOIN LevelMaster B On A.LevelId=B.LevelId Group By A.LevelId,B.LevelName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "FA", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
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
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            try
            {
                dt = new DataTable();
                sSql = "SELECT B.FlatId,B.FlatNo,C.LevelName FloorName ,E.LeadName BuyerName,B.Area,B.Rate,B.NetAmt+B.QualifierAmt NetAmt,B.FlatId,B.AccountId," +
                        " B.Status,B.LevelId,B.RegDate,B.LeadId FROM FlatDetails B INNER JOIN LevelMaster C ON B.LevelId=C.LevelId " +
                        " INNER JOIN dbo.BlockMaster BM ON BM.BlockId=B.BlockId " +
                        " INNER JOIN BuyerDetail D ON D.FlatId=B.FlatId " +
                        " INNER JOIN LeadRegister E ON E.LeadId=D.LeadId " +
                        " WHERE B.CostCentreId=" + arg_iProjectId + " And B.BlockId=" + arg_iBlockId + " And B.LevelId=" + m_iLevelId + " And B.Status='S' " +
                        " And FinaliseDate<='" + argAsOnDate.ToString("dd-MMM-yyyy") + "' " +
                        " Order By BM.SortOrder,C.SortOrder,B.SortOrder,dbo.Val(B.FlatNo) ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "FA", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }



        internal static DataTable Get_BWProject_Sales(string argFromDate,string argToDate)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            try
            {
                dt = new DataTable();

                sSql = "SELECT A.CostCentreId,B.CostCentreName,SUM(A.SoldFlat)SoldFlat,SUM(A.UnSoldFlat)UnSoldFlat,SUM(A.SoldFlat+A.UnSoldFlat) TotalFlat, SUM( A.SoldArea)SoldArea,SUM(A.UnSoldArea)UnSoldArea,  " +
                        " SUM(A.SoldArea+A.UnSoldArea) TotalArea,SUM( A.SoldAmt)SoldAmt,SUM(A.UnSoldAmt)UnSoldAmt , SUM (A.SoldAmt+A.UnSoldAmt) TotalAmt FROM (  " +
                        " Select A.CostCentreId,Count(A.FlatId) SoldFlat,0 UnSoldFlat,SUM(Area)SoldArea,0 UnsoldArea,Sum(NetAmt)+Sum(QualifierAmt)SoldAmt,0 UnSoldAmt From FlatDetails A " +
                        " Inner Join BuyerDetail B On A.FlatId=B.FlatId " +
                        " Where A.Status='S' And B.FinaliseDate Between '" + argFromDate + "' And '" + argToDate + "' Group By A.CostCentreId " +
                        " UNION ALL " +
                        " Select CostCentreId,0,Count(A.FlatId) UnSoldFlat,0 SoldArea,SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt From FlatDetails A " +
                        " Where A.Status='U' Group By CostCentreId)A INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B On A.CostCentreId=B.CostCentreId " +
                        " And B.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                        " Group By A.CostCentreId,B.CostCentreName Order By B.CostCentreName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "FA", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
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
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            try
            {
                dt = new DataTable();
               
                sSql = "SELECT A.BlockId,B.BlockName,SUM(A.SoldFlat)SoldFlat,SUM(A.UnSoldFlat)UnSoldFlat,SUM(A.SoldFlat+A.UnSoldFlat) TotalFlat, SUM( A.SoldArea)SoldArea,SUM(A.UnSoldArea)UnSoldArea,  " +
                        " SUM(A.SoldArea+A.UnSoldArea) TotalArea,SUM( A.SoldAmt)SoldAmt,SUM(A.UnSoldAmt)UnSoldAmt , SUM (A.SoldAmt+A.UnSoldAmt) TotalAmt FROM (  " +
                        " Select BlockId,Count(A.FlatId) SoldFlat,0 UnSoldFlat,SUM(Area)SoldArea,0 UnsoldArea,Sum(NetAmt)+Sum(QualifierAmt)SoldAmt,0 UnSoldAmt From FlatDetails A " +
                        " Inner Join BuyerDetail B On A.FlatId=B.FlatId " +
                        " Where A.Status='S' And A.CostCentreId=" + arg_iProjectId + " And B.FinaliseDate Between '" + argFromDate + "' And '" + argToDate + "' Group By BlockId " +
                        " UNION ALL " +
                        " Select BlockId,0,Count(A.FlatId) UnSoldFlat,0 SoldArea,SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt From FlatDetails A " +
                        " Where A.Status='U' And A.CostCentreId=" + arg_iProjectId + "  Group By BlockId)A INNER JOIN BlockMaster B On A.BlockId=B.BlockId Group By A.BlockId,B.BlockName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "FA", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
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
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            try
            {
                dt = new DataTable();

                //sSql = "SELECT A.BlockId,B.BlockName,SUM(A.SoldFlat)SoldFlat,SUM(A.UnSoldFlat)UnSoldFlat,SUM(A.SoldFlat+A.UnSoldFlat) TotalFlat, SUM( A.SoldArea)SoldArea,SUM(A.UnSoldArea)UnSoldArea,  " +
                //        " SUM(A.SoldArea+A.UnSoldArea) TotalArea,SUM( A.SoldAmt)SoldAmt,SUM(A.UnSoldAmt)UnSoldAmt , SUM (A.SoldAmt+A.UnSoldAmt) TotalAmt FROM (  " +
                //        " Select BlockId,Count(A.FlatId) SoldFlat,0 UnSoldFlat,SUM(Area)SoldArea,0 UnsoldArea,Sum(NetAmt)+Sum(QualifierAmt)SoldAmt,0 UnSoldAmt From FlatDetails A " +
                //        " Inner Join BuyerDetail B On A.FlatId=B.FlatId " +
                //        " Where A.Status='S' And A.CostCentreId=" + arg_iProjectId + " And B.FinaliseDate Between '" + argFromDate + "' And '" + argToDate + "' Group By BlockId " +
                //        " UNION ALL " +
                //        " Select BlockId,0,Count(A.FlatId) UnSoldFlat,0 SoldArea,SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt From FlatDetails A " +
                //        " Where A.Status='U' And A.CostCentreId=" + arg_iProjectId + "  Group By BlockId)A INNER JOIN BlockMaster B On A.BlockId=B.BlockId Group By A.BlockId,B.BlockName";
                sSql = "SELECT A.LevelId,B.LevelName,SUM(A.SoldFlat)SoldFlat,SUM(A.UnSoldFlat)UnSoldFlat,SUM(A.SoldFlat+A.UnSoldFlat) TotalFlat, " +
                        " SUM( A.SoldArea)SoldArea,SUM(A.UnSoldArea)UnSoldArea,SUM(A.SoldArea+A.UnSoldArea) TotalArea,SUM( A.SoldAmt)SoldAmt, " +
                        " SUM(A.UnSoldAmt)UnSoldAmt,SUM (A.SoldAmt+A.UnSoldAmt) TotalAmt FROM (   " +
                        " Select LevelId,Count(A.FlatId) SoldFlat,0 UnSoldFlat, " +
                        " SUM(Area)SoldArea,0 UnsoldArea,Sum(NetAmt)+Sum(QualifierAmt)SoldAmt,0 UnSoldAmt From FlatDetails A  " +
                        " Inner Join BuyerDetail B On A.FlatId=B.FlatId  Where A.Status='S' And A.CostCentreId=" + arg_iProjectId + " And A.BlockId=" + arg_iBlockId + " And B.FinaliseDate " +
                        " Between '" + argFromDate + "' And '" + argToDate + "' Group By LevelId  " +
                        " UNION ALL  " +
                        " Select LevelId,0,Count(A.FlatId) UnSoldFlat,0 SoldArea,SUM(Area) UnsoldArea,0 SoldAmt,Sum(NetAmt)+Sum(QualifierAmt) UnSoldAmt " +
                        " From FlatDetails A  Where A.Status='U' And A.CostCentreId=" + arg_iProjectId + " And A.BlockId=" + arg_iBlockId + " Group By LevelId " +
                        " )A INNER JOIN LevelMaster B On A.LevelId=B.LevelId " +
                        " Group By A.LevelId,B.LevelName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "FA", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
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
            SqlDataAdapter sda;
            DataTable dt = null;
            String sSql = string.Empty;
            try
            {
                dt = new DataTable();
                sSql = "SELECT B.FlatId,B.FlatNo,C.LevelName FloorName ,E.LeadName BuyerName,B.Area,B.Rate,B.NetAmt+B.QualifierAmt NetAmt,B.FlatId,B.AccountId," +
                        " B.Status,B.LevelId,B.RegDate,B.LeadId FROM FlatDetails B INNER JOIN LevelMaster C ON B.LevelId=C.LevelId " +
                        " INNER JOIN dbo.BlockMaster BM ON BM.BlockId=B.BlockId " +
                        " INNER JOIN BuyerDetail D ON D.FlatId=B.FlatId " +
                        " INNER JOIN LeadRegister E ON E.LeadId=D.LeadId " +
                        " WHERE B.CostCentreId=" + arg_iProjectId + " And B.BlockId=" + arg_iBlockId + " And B.LevelId=" + arg_iLevelId + " And B.Status='S' " +
                        " And FinaliseDate Between '" + argFromDate + "' And '" + argToDate + "' " +
                        " Order By BM.SortOrder,C.SortOrder,B.SortOrder,dbo.Val(B.FlatNo) ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "FA", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

    }
}
