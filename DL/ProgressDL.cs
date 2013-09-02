using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
namespace CRM.DataLayer
{
    class ProgressDL
    {
        public DataTable GetCostCentreList()
        {
            DataTable dt = new DataTable();
            BsfGlobal.OpenWorkFlowDB();
            try
            {
                //string sSql = "Select CostCentreId,CostCentreName from OperationalCostCentre Order by CostCentreName ";
                string sSql = "Select CostCentreId,CostCentreName From dbo.OperationalCostCentre" +
                    " Where ProjectDB in(Select ProjectName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister " +
                    " Where BusinessType In('B','L')) and CostCentreId not in (Select CostCentreId " +
                    " From dbo.UserCostCentreTrans Where " +
                    " UserId=" + BsfGlobal.g_lUserId + ") Order By CostCentreName";
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dt;
        }

        public DataTable GetFlatType(int argCCId,string argType,int argLandId)
        {
            DataTable dt = new DataTable();
            BsfGlobal.OpenCRMDB();
            try
            {
                string sSql = "";
                if (argType == "B")
                    sSql = "Select FlatTypeId,TypeName from FlatType Where ProjId= " + argCCId;
                else
                    sSql = "Select PlotTypeId FlatTypeId,PlotTypeName TypeName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotType Where LandRegisterId= " + argLandId;
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


        public DataTable GetProjectCheckList(int argCCID)
        {
            DataTable dt = new DataTable();
            BsfGlobal.OpenCRMDB();
            try
            {
                string sSql = "Select A.CheckListId,A.CheckListName,Case When B.Status=1 then Convert(bit,1,1) " +
                             "else Convert(bit,0,0) End Status from CheckListMaster A " +
                             "Left Join CCCheckListTrans B on A.CheckListId=B.CheckListId and CostCentreId= " + argCCID + " " +
                             "Where A.TypeName='P' order by A.SortOrder";
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

        public DataTable GetPlotProjectCheckList(int arglandId,string argTypeId,string argType)
        {
            DataTable dt = new DataTable();
            BsfGlobal.OpenCRMDB();
            try
            {
                string sSql = "Select CheckListId,CheckListName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.CheckListMaster" +
                        " Where Type='" + argType + "' and CheckListId " +
                        " in (Select CheckListId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotTypeCheckList A " +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotType B on A.PlotTypeId = B.PlotTypeId " +
                        " Where B.LandRegisterId =" + arglandId + " and A.PlotTypeId in (" + argTypeId + ")) order by SortOrder";
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

        public DataTable GetFinalCheckList(int argCCID)
        {
            DataTable dt = new DataTable();
            BsfGlobal.OpenCRMDB();
            try
            {
                string sSql = "Select A.CheckListId,A.CheckListName,Case When B.Status=1 then Convert(bit,1,1) " +
                             "else Convert(bit,0,0) End Status from CheckListMaster A " +
                             "Left Join CCCheckListTrans B on A.CheckListId=B.CheckListId and CostCentreId= " + argCCID + " " +
                             "Where A.TypeName='F' order by A.SortOrder";
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

        public DataSet GetHandingCheckList(int argCCId,string argFlatTypeId,string argType)
        {
            DataSet ds = new DataSet();
            try
            {
                string sSql = "Select CheckListId,CheckListName,'' Total from CheckListMaster Where TypeName='" + argType + "' " +
                              "and CheckListId in (Select CheckListId from FlatTypeChecklist A " +
                              "Inner Join FlatType B on A.FlatTypeId = B.FlatTypeId  " +
                              "Where B.ProjId = " + argCCId + " and A.FlatTypeId in (" + argFlatTypeId + ")) order by SortOrder";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "CheckList");
                da.Dispose();


                sSql = "SELECT BlockId,BlockName From dbo.BlockMaster Where CostCentreId= " + argCCId + " Order by SortOrder";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Block");
                da.Dispose();

                sSql = "SELECT 0 LevelId,'(All)' LevelName, 0 SortOrder  UNION ALL " +
                       "SELECT LevelId,LevelName, SortOrder FROM LevelMaster WHERE CostCentreId= " + argCCId + " ORDER BY SortOrder";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Level");
                da.Dispose();

                //sSql = "Select FlatId,FlatNo,BlockId,LevelId from FlatDetails " +
                //       "Where CostCentreId=" + argCCId + " and FlatTypeId in (" + argFlatTypeId + ") Order by FlatNo, dbo.Val(FlatNo)";
                sSql = "Select A.FlatId,A.FlatNo,A.BlockId,A.LevelId From FlatDetails A " +
                        " INNER JOIN dbo.BlockMaster BM On BM.BlockId=A.BlockId " +
                        " INNER JOIN dbo.LevelMaster LM On LM.LevelId=A.LevelId " +
                        " Where A.CostCentreId=" + argCCId + " And A.FlatTypeId In (" + argFlatTypeId + ") And A.Status='S' " +
                        " Order By BM.SortOrder,LM.SortOrder,A.SortOrder,dbo.Val(A.FlatNo)";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Flats");
                da.Dispose();

                sSql = "Select A.CheckListId,A.FlatId from FlatChecklist A " +
                       "Inner Join FlatDetails B on A.FlatId=B.FlatId " +
                       "Where A.Status=1 and B.CostCentreId=" + argCCId + " and B.FlatTypeId in (" + argFlatTypeId + ")";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "FlatCheckList");
                da.Dispose();


                sSql = "Select G.BlockId,G.CheckListId,Count(G.FlatId) CFlat from " +
                       "(Select Distinct B.BlockId,B.LevelId,A.CheckListId,B.FlatId From FlatTypeChecklist A " +
                       "Inner join FlatDetails B on A.FlatTypeId=B.FlatTypeId " +
                       "Where B.CostCentreId=" + argCCId + " And B.Status='S' And B.FlatTypeId In (" + argFlatTypeId + ")) G " +
                       "Group by G.BlockId,G.CheckListId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "BlockTotal");
                da.Dispose();

                sSql = "Select G.BlockId,G.CheckListId,Count(G.FlatId) CFlat from " +
                       "(Select Distinct B.BlockId,B.LevelId,A.CheckListId,B.FlatId From FlatChecklist A " +
                       "Inner join FlatDetails B on A.FlatId=B.FlatId " +
                       "Where A.Status=1 and B.CostCentreId=" + argCCId + " and B.FlatTypeId in (" + argFlatTypeId + ")) G " +
                       "Group by G.BlockId,G.CheckListId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "BlockComp");
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

        public DataSet GetPlotHandingCheckList(int argLandId, string argFlatTypeId, string argType)
        {
            DataSet ds = new DataSet();
            try
            {
                string sSql = "Select CheckListId,CheckListName from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.CheckListMaster Where Type='" + argType + "' and CheckListId " +
                    " in (Select CheckListId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotTypeCheckList A" +
                    " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotType B on A.PlotTypeId = B.PlotTypeId " +
                    " Where B.LandRegisterId = " + argLandId + " and A.PlotTypeId in (" + argFlatTypeId + ")) order by SortOrder";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "CheckList");
                da.Dispose();

                sSql = "Select PlotDetailsId,PlotNo from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails Where LandRegisterId=" + argLandId + " " +
                    " and PlotTypeId in (" + argFlatTypeId + ")";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Flats");
                da.Dispose();

                sSql = "Select A.CheckListId,A.PlotDetailsId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotDetailsCheckList A " +
                    " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails B on A.PlotDetailsId=B.PlotDetailsId " +
                    " Where B.LandRegisterId=" + argLandId + " and B.PlotTypeId in (" + argFlatTypeId + ")";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "FlatCheckList");
                da.Dispose();


                sSql = "Select G.CheckListId,Count(G.PlotDetailsId) CFlat from " +
                    " (Select Distinct A.CheckListId,B.PlotDetailsId From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotTypeCheckList A" +
                    " Inner join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails B on A.PlotTypeId=B.PlotTypeId Where B.LandRegisterId=" + argLandId + " " +
                    " and B.PlotTypeId in (" + argFlatTypeId + ")) G Group by G.CheckListId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "BlockTotal");
                da.Dispose();

                sSql = " Select G.CheckListId,Count(G.PlotDetailsId) CFlat from " +
                    " (Select Distinct A.CheckListId,B.PlotDetailsId From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotDetailsCheckList A " +
                    " Inner join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails B on A.PlotDetailsId=B.PlotDetailsId Where B.LandRegisterId=" + argLandId + " " +
                    " and B.PlotTypeId in (" + argFlatTypeId + ")) G Group by G.CheckListId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "BlockComp");
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

    }
}
