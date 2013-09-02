using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using CRM.BO;

namespace CRM.DL
{
    class StageDetDL
    {
        #region Methods

        internal static DataTable PopulateBlock(int argCCId)
        {
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                sSql = "Select BlockId,BlockName from BlockMaster Where CostCentreId = " + argCCId + " Order By SortOrder";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        internal static DataTable PopulateStages(int argCCId)
        {
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                sSql = "Select StageId,StageName from Stages Where CostCentreId = " + argCCId + " Order By SortOrder";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        internal static DataTable PopulateLevel(int argCCId)
        {
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                sSql = "Select LevelId,LevelName from LevelMaster Where CostCentreId = " + argCCId + " Order By SortOrder";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        internal static DataTable PopulateStageBlock(int i_BlockId, int argCCId)
        {
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                sSql = string.Format("Select A.StageName, A.StageId from Stages A INNER JOIN BlockStageTrans B"+
                    " ON A.StageId=B.StageId AND B.BlockId={0} Where A.CostCentreId={1}", i_BlockId, argCCId);
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        internal static DataTable PopulateStageLevel(int i_LevelId, int argCCId)
        {
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                sSql = string.Format("Select A.StageName, A.StageId from Stages A INNER JOIN LevelStageTrans B" +
                    " ON A.StageId=B.StageId AND B.LevelId={0} Where A.CostCentreId={1}", i_LevelId, argCCId);
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        internal static DataTable PopulateLevelBlock(int i_BlockId, int argCCId)
        {
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                if (i_BlockId != 0)
                {
                    //sSql = string.Format("Select A.LevelName, A.LevelId, Case When B.LevelId IS NULL Then" +
                    //    " Convert(bit,0,0) else Convert(bit,1,1) END as Sel from LevelMaster A LEFT JOIN BlockLevelTrans B" +
                    //    " ON A.LevelId=B.LevelId AND B.BlockId={0} Where A.CostCentreId={1}", i_BlockId, argCCId);
                    sSql = string.Format("Select A.LevelName, A.LevelId From dbo.LevelMaster A LEFT JOIN dbo.BlockLevelTrans B" +
                        " ON A.LevelId=B.LevelId AND B.BlockId={0} Where A.CostCentreId={1}", i_BlockId, argCCId);
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    da.Fill(dt);
                    da.Dispose();
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
            return dt;
        }

        internal static DataTable PopulateBlockLevel(int i_BlockId, int argCCId)
        {
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                if (i_BlockId != 0)
                {
                    sSql = string.Format("Select A.LevelName, A.LevelId From dbo.LevelMaster A INNER JOIN dbo.BlockLevelTrans B" +
                        " ON A.LevelId=B.LevelId AND B.BlockId={0} Where A.CostCentreId={1}", i_BlockId, argCCId);
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    da.Fill(dt);
                    da.Dispose();
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
            return dt;
        }

        internal static void DeleteStageBlock(int i_SBID,int i_BlockId)
        {
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = null;
            try
            {
                sSql = string.Format("Delete BlockStageTrans Where StageId={0} And BlockId={1}", i_SBID,i_BlockId);
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
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
        }

        internal static void DeleteStageLevel(int i_SLID, int i_LevelId)
        {
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = null;
            try
            {
                sSql = string.Format("Delete LevelStageTrans Where StageId={0} And LevelId={1}", i_SLID,i_LevelId);
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
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
        }

        internal static void DeleteLevelBlock(StageDetBO BOstageDet)
        {
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = null;
            try
            {
                sSql = string.Format("Delete BlockLevelTrans Where BlockId={0} AND LevelId={1}", BOstageDet.i_BlockId, BOstageDet.i_LevelBlockId);
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
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
        }

        internal static void DeletBlock(int i_BID,int argCCId)
        {
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = null;
            try
            {
                sSql = string.Format("Delete BlockMaster Where BlockId={0}", i_BID);
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = " Update ProjectInfo Set TotalBlocks=(Select Count(BlockId) TotalBlock From BlockMaster " +
                        " Where CostCentreId=" + argCCId + ") Where CostCentreId=" + argCCId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
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
        }

        internal static void DeletStage(int i_SID)
        {
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = null;
            try
            {
                sSql = string.Format("Delete Stages Where StageId={0}", i_SID);
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
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
        }

        internal static void DeletLevel(int i_LID,int argCCId)
        {
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = null;
            try
            {
                sSql = string.Format("Delete LevelMaster Where LevelId={0}", i_LID);
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = " Update ProjectInfo Set NoOfFloors=(Select Count(LevelId) NoOfFloor From LevelMaster " +
                        " Where CostCentreId=" + argCCId + ") Where CostCentreId=" + argCCId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
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
        }

        internal static int InsertBlock(StageDetBO BOstageDet)
        {
            string sSql = "";
            int iId = 0;
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = null;
            try
            {
                sSql = string.Format("Insert into BlockMaster(BlockName, CostCentreId, SortOrder)"+
                    " Values('{0}', {1}, {2}) SELECT SCOPE_IDENTITY();", BOstageDet.s_BlockName,
                    BOstageDet.i_CostCentreId, BOstageDet.i_SortOrderBlock);

                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                iId = int.Parse(cmd.ExecuteScalar().ToString());
                cmd.Dispose();

                sSql = " Update ProjectInfo Set TotalBlocks=(Select Count(BlockId) TotalBlock From BlockMaster " +
                        " Where CostCentreId=" + BOstageDet.i_CostCentreId + ") Where CostCentreId=" + BOstageDet.i_CostCentreId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
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
            return iId;
        }

        internal static bool CheckBlockFound(StageDetBO BOstageDet)
        {
            string sSql = "";
            bool bAns = false;
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = null; SqlDataReader dr;
            try
            {
                sSql = "Select BlockName From BlockMaster Where BlockName='" + BOstageDet.s_BlockName + "' And CostCentreId=" + BOstageDet.i_CostCentreId + "";

                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    bAns = true;
                }
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

        internal static void UpdateBlock(StageDetBO BOstageDet,int argBlockId)
        {
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = null;
            try
            {
                sSql = "Update BlockMaster Set BlockName='" + BOstageDet.s_BlockName + "',SortOrder=" + BOstageDet.i_SortOrderBlock + " Where CostCentreId=" + BOstageDet.i_CostCentreId + "" +
                    " And BlockId=" + argBlockId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
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
        }

        internal static int InsertStage(StageDetBO BOstageDet)
        {
            string sSql = "";
            int iId = 0;
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = null;
            try
            {
                sSql = string.Format("Insert into Stages(StageName, CostCentreId, SortOrder)" +
                    " Values('{0}', {1}, {2})  SELECT SCOPE_IDENTITY();", BOstageDet.s_StageName,
                    BOstageDet.i_CostCentreId, BOstageDet.i_SorOrderStage);
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                iId = int.Parse(cmd.ExecuteScalar().ToString());
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
            return iId;
        }

        internal static void UpdateStage(StageDetBO BOstageDet,int argStageId)
        {
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = null;
            try
            {
                sSql = "Update Stages Set StageName='" + BOstageDet.s_StageName + "',SortOrder=" + BOstageDet.i_SorOrderStage + " Where CostCentreId=" + BOstageDet.i_CostCentreId + "" +
                    " And StageId=" + argStageId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
        }

        internal static void UpdateBlockSort(DataTable dt,string argType,int argCCId)
        {
            string sSql = "";
            
            SqlCommand cmd = null;
            try
            {
                dt.AcceptChanges();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (argType == "Block")
                    {
                        int iBlockId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["BlockId"].ToString(), CommFun.datatypes.vartypenumeric));
                        int iOrder = i + 1;
                        sSql = "Update BlockMaster Set SortOrder=" + iOrder + " Where BlockId=" + iBlockId + " And CostCentreId=" + argCCId + "";
                    }
                    else if (argType == "Stage")
                    {
                        int iStageId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["StageId"].ToString(), CommFun.datatypes.vartypenumeric));
                        int iOrder = i + 1;
                        sSql = "Update Stages Set SortOrder=" + iOrder + " Where StageId=" + iStageId + " And CostCentreId=" + argCCId + "";
                    }
                    else if (argType == "Level")
                    {
                        int iLevelId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["LevelId"].ToString(), CommFun.datatypes.vartypenumeric));
                        int iOrder = i + 1;
                        sSql = "Update LevelMaster Set SortOrder=" + iOrder + " Where LevelId=" + iLevelId + " And CostCentreId=" + argCCId + "";
                    }
                    cmd = new SqlCommand(sSql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
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
        }

        internal static int InsertLevel(StageDetBO BOstageDet)
        {
            string sSql = "";
            int iId = 0;
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = null;
            try
            {
                sSql = string.Format("Insert into LevelMaster(LevelName, CostCentreId, SortOrder)" +
                    " Values('{0}', {1}, {2}) SELECT SCOPE_IDENTITY();", BOstageDet.s_LevelName,
                    BOstageDet.i_CostCentreId, BOstageDet.i_SortOrderLevel);

                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                iId= int.Parse(cmd.ExecuteScalar().ToString());
                cmd.Dispose();

                sSql = " Update ProjectInfo Set NoOfFloors=(Select Count(LevelId) NoOfFloor From LevelMaster " +
                        " Where CostCentreId=" + BOstageDet.i_CostCentreId + ") Where CostCentreId=" + BOstageDet.i_CostCentreId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
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
            return iId;
        }

        internal static void UpdateLevel(StageDetBO BOstageDet,int argLevelId)
        {
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = null;
            try
            {
                sSql = "Update LevelMaster Set LevelName='" + BOstageDet.s_LevelName + "',SortOrder=" + BOstageDet.i_SortOrderLevel + " Where CostCentreId=" + BOstageDet.i_CostCentreId + "" +
                    " And LevelId=" + argLevelId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
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
        }


        internal static void InsertStageBlock(int argBlockId,string argStr)
        {
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = null;
            try
            {
                sSql = "Delete from BlockStageTrans Where BlockId = " + argBlockId + " AND StageId=" + argStr + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();

                if (argStr !="")
                {
                    sSql = "Insert into BlockStageTrans(BlockId,StageId) " +
                           "Select " + argBlockId + ",StageId from Stages Where StageId in (" + argStr + ")";
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
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
        }

        internal static void InsertStageLevel(int argLevelId, string argStr)
        {
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = null;
            try
            {
                sSql = "Delete from LevelStageTrans Where LevelId = " + argLevelId + " AND StageId=" + argStr + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();

                if (argStr != "")
                {
                    sSql = "Insert into LevelStageTrans(LevelId,StageId) " +
                           "Select " + argLevelId + ",StageId from Stages Where StageId in (" + argStr + ")";
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
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
        }

        internal static void InsertLevelBlock(StageDetBO BOstageDet)
        {
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = null;
            try
            {
                sSql = string.Format("Insert into BlockLevelTrans(LevelId, BlockId)" +
                    " Values({0}, {1})", BOstageDet.i_LevelBlockId, BOstageDet.i_BlockId);
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
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
        }

        #endregion

        internal static DataTable EditCarParkCodeSetup(int argCCId, int argBlockId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = new DataTable();
            try
            {
                string sSql = "Select Case When Type='A' Then 1 Else 0 End Type, Prefix, Suffix, StartNo, Width from dbo.CarParkCodeSetup " +
                              " Where CostCentreId=" + argCCId + " AND BlockId=" + argBlockId + "";
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

        internal static void InsertCarParkCodeSetUp(int argCCId, int argBlockId, string argType, string argPrefix, string argSuffix, int argStartNo, int argWidth)
        {
            SqlConnection conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sSql = "Delete dbo.CarParkCodeSetup Where BlockId=" + argBlockId + " AND CostCentreId=" + argCCId + "";
                SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                if (argType == "A")
                {
                    sSql = "Insert into dbo.CarParkCodeSetup(CostCentreId, BlockId, Type, Prefix, Suffix, StartNo, Width) " +
                           "Values(" + argCCId + "," + argBlockId + ",'" + argType + "','" + argPrefix + "','" + argSuffix + "'," + argStartNo + "," + argWidth + ")";
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
                conn.Dispose();
                conn.Close();
            }
        }
    }
}
