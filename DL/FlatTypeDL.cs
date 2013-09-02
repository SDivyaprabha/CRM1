using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessLayer;
using CRM.BusinessObjects;
using System.Collections;

namespace CRM.DataLayer
{
    class FlatTypeDL
    {
        #region Methods

        public static DataTable GetData()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            try
            {
                sda = new SqlDataAdapter("SELECT * FROM dbo.FlatType WHERE FlatTypeId=0", BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
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

        public static DataTable GetProject()
        {
            DataTable dtProject = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenWorkFlowDB();
            try
            {
                sda = new SqlDataAdapter("SELECT CostCentreId,CostCentreName FROM dbo.OperationalCostCentre ORDER BY CostCentreName", BsfGlobal.g_WorkFlowDB);
                dtProject = new DataTable();
                sda.Fill(dtProject);
                dtProject.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dtProject;

        }

        public static DataTable GetPayReport(int argCCId,int argPayTypeId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtT = new DataTable();
            try
            {
                string sSql = "Select ProjId CostCentreId,FlatTypeId,PayTypeId,TypeName,Area From dbo.FlatType " +
                         " Where ProjId=" + argCCId + " And PayTypeId=" + argPayTypeId + "";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();

                dtT = dt;

                sSql = "Select TemplateId,Description From dbo.PaymentSchedule Where CostCentreId=" + argCCId + " And TypeId=" + argPayTypeId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sColn = dt.Rows[i]["Description"].ToString();
                    dtT.Columns.Add(sColn, typeof(decimal));
                }
                dtT.Columns.Add("Total", typeof(decimal));

                sSql = "Select A.CostCentreId, B.FlatTypeId, B.PayTypeId, A.TemplateId, A.SchPercent, A.Description, " +
                       " B.NetAmt, (B.NetAmt * SchPercent)/100 Amount, A.SortOrder From dbo.PaymentSchedule A " +
                       " Inner Join FlatType B On A.CostCentreId=B.ProjId And A.TypeId=B.PayTypeId " +
                       " Where B.ProjId=" + argCCId + " And A.TypeId=" + argPayTypeId +
                       " Order By A.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();

                if (dtT.Columns.Count > 0)
                {
                    for (int i = 5; i < dtT.Columns.Count; i++)
                    {
                        string sName = dtT.Columns[i].Caption;
                        for (int j = 0; j < dtT.Rows.Count; j++)
                        {
                            int iCCId = Convert.ToInt32(dtT.Rows[j]["CostCentreId"].ToString());
                            int iFlatTypeId = Convert.ToInt32(dtT.Rows[j]["FlatTypeId"].ToString());
                            int iPayTypeId = Convert.ToInt32(dtT.Rows[j]["PayTypeId"].ToString());

                            DataView dv = new DataView(dt);
                            dv.RowFilter = "CostCentreId = " + iCCId + " And FlatTypeId=" + iFlatTypeId + " And PayTypeId=" + iPayTypeId + " And Description='" + sName + "'";
                            decimal dAmt = 0;
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                dAmt = Convert.ToDecimal(dv.ToTable().Rows[0]["Amount"].ToString());
                            }

                            DataRow[] drT = dt.Select("CostCentreId = " + iCCId + " And FlatTypeId=" + iFlatTypeId + " And PayTypeId=" + iPayTypeId + " And Description='" + sName + "'");
                            if (drT.Length > 0)
                            {
                                dtT.Rows[j][i] = decimal.Round(dAmt, 3);
                                drT[0]["Amount"] = decimal.Round(dAmt, 3);
                            }
                        }
                    }
                }

                decimal iTot = 0;
                if (dtT.Rows.Count > 0)
                {
                    for (int k = 0; k < dtT.Rows.Count; k++)
                    {
                        decimal iTTot = 0;
                        for (int j = 5; j < dtT.Columns.Count - 1; j++)
                        {
                            iTot = Convert.ToDecimal(dtT.Rows[k][j]);
                            iTTot = iTTot + iTot;
                        }
                        dtT.Rows[k]["Total"] = iTTot.ToString().Trim();
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
            return dtT;

        }
        
        public static DataTable GetType(int projId,string typeName)
        {
            DataTable dtType = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            try
            {
                sda = new SqlDataAdapter(String.Format("SELECT TypeId,TypeName,Area,Rate,BaseAmt,OtherCostAmt,TotalCarPark,ExtraBillAmt,VatAmt,SerTaxAmt,NetAmt FROM dbo.FlatType WHERE TypeName='{0}' AND ProjId='{1}'", typeName, projId), BsfGlobal.g_CRMDB);
                dtType = new DataTable();
                sda.Fill(dtType);
                dtType.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtType;

        }

        public static void UpdateFlatTypeDetails(FlatTypeBL OFlatType, DataTable dtAreatrans, DataTable dtSTaxtrans, DataTable dtOCosttrans, ArrayList argExtraItemtrans,DataTable dtCheck)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = String.Format("Update dbo.FlatType set ProjId={0},Typename='{1}',Area={2},USLandArea={3},LandRate={4},LandAmount={5},Rate={6},BaseAmt={7},AdvAmount={8},OtherCostAmt={9}, NoOfCarPark={10},VatAmt={11},SerTaxAmt={12},NetAmt={13},Remarks='{14}',STaxPer={15},VatPer={16} Where FlatTypeID={17}", OFlatType.ProjId, OFlatType.Typename, OFlatType.Area, OFlatType.UDSLandFType, OFlatType.LandRate, OFlatType.LandAmt, OFlatType.Rate, OFlatType.BaseAmt, OFlatType.AdvAmt, OFlatType.OtherCostAmt, OFlatType.NoOfCarPark, OFlatType.VatAmt, OFlatType.SerTaxAmt, OFlatType.NetAmt, OFlatType.Remarks, OFlatType.STaxPer, OFlatType.VatPer, OFlatType.TypeId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    sSql = String.Format("Delete from dbo.FlatTypeAreaTrans Where FlatTypeId={0} ", OFlatType.TypeId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    if (dtAreatrans.Rows.Count > 0)
                    {
                        for (int a = 0; a < dtAreatrans.Rows.Count; a++)
                        {
                            sSql = String.Format("INSERT INTO dbo.FlatTypeAreaTrans(FlatTypeId,AreaId,AreaSqft) Values({0},{1},{2})", OFlatType.TypeId, Convert.ToInt32(dtAreatrans.Rows[a]["AreaId"]), dtAreatrans.Rows[a]["AreaSqft"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    sSql = String.Format("Delete from dbo.FlatTypeSTaxTrans Where FlatTypeId={0} ", OFlatType.TypeId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    if (dtSTaxtrans.Rows.Count > 0)
                    {
                        for (int s = 0; s < dtSTaxtrans.Rows.Count; s++)
                        {
                            sSql = String.Format("INSERT INTO dbo.FlatTypeSTaxTrans(FlatTypeId,AccountId,TaxId,TaxDescp,TaxFormula,AddFlag) Values({0},{1},{2},'{3}','{4}','{5}')", OFlatType.TypeId, dtSTaxtrans.Rows[s]["AccId"], dtSTaxtrans.Rows[s]["TaxId"], dtSTaxtrans.Rows[s]["TaxDescp"], dtSTaxtrans.Rows[s]["TaxFormula"], dtSTaxtrans.Rows[s]["AddFlag"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    sSql = String.Format("Delete from dbo.FlatTypeOCostTrans Where FlatTypeId={0} ", OFlatType.TypeId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    if (dtOCosttrans.Rows.Count > 0)
                    {
                        for (int c = 0; c < dtOCosttrans.Rows.Count; c++)
                        {
                            sSql = String.Format("INSERT INTO dbo.FlatTypeOCostTrans(FlatTypeId,OtherCostId,OtherCostName,Area,Rate,Flag,Amount) Values({0},{1},'{2}',{3},{4},'{5}',{6}) ", OFlatType.TypeId, dtOCosttrans.Rows[c]["OtherCostId"], dtOCosttrans.Rows[c]["OtherCostName"], dtOCosttrans.Rows[c]["Area"], dtOCosttrans.Rows[c]["Rate"], dtOCosttrans.Rows[c]["Flag"], dtOCosttrans.Rows[c]["Amount"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    sSql = String.Format("Delete from dbo.FlatTypeExtraItemTrans Where FlatTypeId={0} ", OFlatType.TypeId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    if (argExtraItemtrans.Count > 0)
                    {
                        for (int c = 0; c < argExtraItemtrans.Count; c++)
                        {
                            sSql = String.Format("INSERT INTO dbo.FlatTypeExtraItemTrans(FlatTypeId,TransId) Values({0},{1}) ", OFlatType.TypeId, argExtraItemtrans[c]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    sSql = String.Format("DELETE FROM dbo.FlatTypeCheckList WHERE FlatTypeId={0}", OFlatType.TypeId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    if (dtCheck.Rows.Count > 0)
                    {
                        for (int c = 0; c < dtCheck.Rows.Count; c++)
                        {
                            sSql = String.Format("INSERT INTO dbo.FlatTypeCheckList(FlatTypeId,Description,ExecutiveId,ExpCompletionDate,Status,CompletionDate) Values({0},'{1}', {2},'{3:dd-MMM-yyyy}', '{4}','{5:dd-MMM-yyyy}') ", OFlatType.TypeId, dtCheck.Rows[c]["Description"], dtCheck.Rows[c]["Executive"], Convert.ToDateTime(dtCheck.Rows[c]["ExpCompletionDate"]), dtCheck.Rows[c]["Status"], Convert.ToDateTime(dtCheck.Rows[c]["CompletionDate"]));
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    tran.Commit();
                }
                catch (Exception e)
                {
                    BsfGlobal.CustomException(e.Message, e.StackTrace);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static void UpdateUnitFTDetails(int argCCId,FlatTypeBO OUnitDirBO)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = String.Format("Update dbo.FlatType set ProjId={0},Typename='{1}',Area={2},USLandArea={3},LandRate={4},LandAmount={5},Rate={6},BaseAmt={7},AdvAmount={8},OtherCostAmt={9}, TotalCarPark={10},NetAmt={11},PayTypeId={12},Remarks='{13}',CreditDays={14},IntPercent={15},FloorwiseRate='{16}',AdvPercent={17},Guidelinevalue={18},FacingId={19} Where FlatTypeID={20}", OUnitDirBO.ProjId, OUnitDirBO.TypeName, OUnitDirBO.Area, OUnitDirBO.USLandArea, OUnitDirBO.LandRate, OUnitDirBO.LandAmount, OUnitDirBO.Rate, OUnitDirBO.BaseAmt, OUnitDirBO.AdvAmount, OUnitDirBO.OtherCostAmt, OUnitDirBO.TotalCarpark, OUnitDirBO.NetAmt, OUnitDirBO.PayTypeId, OUnitDirBO.Remarks, OUnitDirBO.CreditDays, OUnitDirBO.InterestPercent, OUnitDirBO.LevelRate, OUnitDirBO.AdvPercent, OUnitDirBO.GuideLineValue, OUnitDirBO.Facing, OUnitDirBO.FlatTypeId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    
                    tran.Commit();
                }
                catch (Exception e)
                {
                    BsfGlobal.CustomException(e.Message, e.StackTrace);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static void InsertFlatTypeDetails(int blockid, FlatTypeBL OFlatType, DataTable dtAreatrans, DataTable dtSTaxtrans, DataTable dtOCosttrans, ArrayList argExtraItemtrans, DataTable dtCheck)
        {
            int iFTypeId = 0;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = String.Format("INSERT INTO dbo.FlatType(BlockId,ProjId,Typename,Area,USLandArea,LandRate,LandAmount,Rate,BaseAmt,AdvAmount,OtherCostAmt,NoOfCarPark,VatAmt,SerTaxAmt,NetAmt,Remarks,STaxPer,VatPer) Values({0},{1},'{2}',{3},{4},{5},{6},{7},{8},{9},{10},  {11},{12},{13},{14},'{15}',  {16},{17})SELECT SCOPE_IDENTITY(); ", blockid, OFlatType.ProjId, OFlatType.Typename, OFlatType.Area, OFlatType.UDSLandFType, OFlatType.LandRate, OFlatType.LandAmt, OFlatType.Rate, OFlatType.BaseAmt, OFlatType.AdvAmt, OFlatType.OtherCostAmt, OFlatType.NoOfCarPark, OFlatType.VatAmt, OFlatType.SerTaxAmt, OFlatType.NetAmt, OFlatType.Remarks, OFlatType.STaxPer, OFlatType.VatPer);
                    cmd = new SqlCommand(sSql, conn, tran);
                    iFTypeId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();
                    if (dtAreatrans.Rows.Count > 0)
                    {
                        for (int a = 0; a < dtAreatrans.Rows.Count; a++)
                        {
                            sSql = String.Format("INSERT INTO dbo.FlatTypeAreaTrans(FlatTypeId,AreaId,AreaSqft) Values({0},{1},{2})", iFTypeId, Convert.ToInt32(dtAreatrans.Rows[a]["AreaId"]), dtAreatrans.Rows[a]["AreaSqft"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    if (dtSTaxtrans.Rows.Count > 0)
                    {
                        for (int s = 0; s < dtSTaxtrans.Rows.Count; s++)
                        {
                            sSql = String.Format("INSERT INTO dbo.FlatTypeSTaxTrans(FlatTypeId,AccountId,TaxId,TaxDescp,TaxFormula,AddFlag) Values({0},{1},{2},'{3}','{4}','{5}')", iFTypeId, dtSTaxtrans.Rows[s]["AccId"], dtSTaxtrans.Rows[s]["TaxId"], dtSTaxtrans.Rows[s]["TaxDescp"], dtSTaxtrans.Rows[s]["TaxFormula"], dtSTaxtrans.Rows[s]["AddFlag"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    if (dtOCosttrans.Rows.Count > 0)
                    {
                        for (int c = 0; c < dtOCosttrans.Rows.Count; c++)
                        {
                            sSql = String.Format("INSERT INTO dbo.FlatTypeOCostTrans(FlatTypeId,OtherCostId,OtherCostName,Area,Rate,Flag,Amount) Values({0},{1},'{2}',{3},{4},'{5}',{6}) ", iFTypeId, dtOCosttrans.Rows[c]["OtherCostId"], dtOCosttrans.Rows[c]["OtherCostName"], dtOCosttrans.Rows[c]["Area"], dtOCosttrans.Rows[c]["Rate"], dtOCosttrans.Rows[c]["Flag"], dtOCosttrans.Rows[c]["Amount"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    if (argExtraItemtrans.Count > 0)
                    {
                        for (int c = 0; c < argExtraItemtrans.Count; c++)
                        {
                            sSql = String.Format("INSERT INTO dbo.FlatTypeExtraItemTrans(FlatTypeId,TransId) Values({0},{1}) ", iFTypeId, argExtraItemtrans[c]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    if (dtCheck.Rows.Count > 0)
                    {
                        for (int c = 0; c < dtCheck.Rows.Count; c++)
                        {
                            sSql = String.Format("INSERT INTO dbo.FlatTypeCheckList(FlatTypeId,Description,ExecutiveId,ExpCompletionDate,Status,CompletionDate) Values({0},'{1}', {2},'{3:dd-MMM-yyyy}', '{4}','{5:dd-MMM-yyyy}') ", iFTypeId, dtCheck.Rows[c]["Description"], dtCheck.Rows[c]["Executive"], Convert.ToDateTime(dtCheck.Rows[c]["ExpCompletionDate"]), dtCheck.Rows[c]["Status"], Convert.ToDateTime(dtCheck.Rows[c]["CompletionDate"]));
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    tran.Commit();
                }
                catch (Exception e)
                {
                    BsfGlobal.CustomException(e.Message, e.StackTrace);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static int InsertUnitFTDetails(int argCCId,string argTypeName)
        {
            int iFTypeId = 0;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = String.Format("INSERT INTO dbo.FlatType(ProjId,Typename) Values({0},'{1}')  SELECT SCOPE_IDENTITY(); ", argCCId, argTypeName);
                    cmd = new SqlCommand(sSql, conn, tran);
                    iFTypeId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();

                    sSql = "Insert into dbo.FlatTypeOtherCost(FlatTypeId,OtherCostId,Flag) " +
                           "Select " + iFTypeId + ",OtherCostId,'+' from dbo.OtherCostMaster Where SysDefault=1";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    tran.Commit();
                }
                catch (Exception e)
                {
                    BsfGlobal.CustomException(e.Message, e.StackTrace);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return iFTypeId;
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

        public static DataTable GetFlat(int argCCId, int argFlatTypeId,int argBlockId,int argLevelId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql = "";

            sSql = "SELECT F.FlatTypeId,F.FlatId,F.FlatNo FROM dbo.FlatDetails F " +
                    " INNER JOIN dbo.LevelMaster L ON F.LevelId=L.LevelId " +
                    " INNER JOIN dbo.BlockMaster B ON F.BlockId=B.BlockId " +
                    " Where Status='U' AND F.CostCentreId=" + argCCId + " " +
                    " Order By B.SortOrder,L.SortOrder,dbo.Val(F.FlatNo)";
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

        public static DataTable GetFTLevel(int argCCId)
        {
            SqlDataAdapter sda;
            DataTable dtLevel = null;
            BsfGlobal.OpenCRMDB();
            try
            {
                sda = new SqlDataAdapter(String.Format("SELECT CAST(0 as bit) Sel,LevelId,LevelName,CAST('' as varchar(3)) Title FROM dbo.LevelMaster Where CostCentreId= {0} ORDER BY SortOrder", argCCId), BsfGlobal.g_CRMDB);
                dtLevel = new DataTable();
                sda.Fill(dtLevel);
                dtLevel.Dispose();
            }

            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtLevel;
        }

        public static DataTable GetFTLevelwise(string arg_BId,string arg_LId,int argCCID)
        {
            SqlDataAdapter sda;
            DataTable dtLevel = null;
            BsfGlobal.OpenCRMDB();
            try
            {
                string sSql = "Select A.BlockId,A.LevelId,B.BlockName,C.LevelName,CAST(0 as int) TotalFlat from dbo.BlockLevelTrans A " +
                    " Inner Join dbo.BlockMaster B on A.BlockId=B.BlockId and B.CostCentreId= " + argCCID + "  " +
                    " Inner Join dbo.LevelMaster C on A.LevelId=C.LevelId and C.CostCentreId= " + argCCID + " " +
                    " Where A.BlockId in (" + arg_BId.Trim(',') + ") and A.LevelId in (" + arg_LId.Trim(',') + ")" +
                    " Order By B.Sortorder,C.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dtLevel = new DataTable();
                sda.Fill(dtLevel);
                dtLevel.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtLevel;
        }

        public static DataTable GetBlock(int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT BlockId,BlockName FROM dbo.BlockMaster Where CostCentreId= " + argCCId + " ORDER BY SortOrder", BsfGlobal.g_CRMDB);
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

        public static DataTable GetFlatBlock(int argCCID)
        {
            SqlDataAdapter sda;
            DataTable dtFlatBlock = null;
            BsfGlobal.OpenCRMDB();
            try
            {
                sda = new SqlDataAdapter("SELECT BlockId,BlockName FROM dbo.BlockMaster Where CostCentreId= " + argCCID + " ORDER BY SortOrder", BsfGlobal.g_CRMDB);
                dtFlatBlock = new DataTable();
                sda.Fill(dtFlatBlock);
                dtFlatBlock.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtFlatBlock;
        }

        public static DataTable GetFTBlock(int  argCCID)
        {
            SqlDataAdapter sda;
            DataTable dtFlatBlock = null;
            BsfGlobal.OpenCRMDB();
            try
            {
                sda = new SqlDataAdapter("SELECT CAST(0 as bit) Sel,BlockId,BlockName,CAST('' as varchar(3)) Title FROM dbo.BlockMaster Where CostCentreId= " + argCCID + " ORDER BY SortOrder", BsfGlobal.g_CRMDB);
                dtFlatBlock = new DataTable();
                sda.Fill(dtFlatBlock);
                dtFlatBlock.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtFlatBlock;
        }

        public static DataTable GetFTBlockwise(int argCCId,string arg_BId)
        {
            SqlDataAdapter sda;
            DataTable dtFlatBlock = null;
            BsfGlobal.OpenCRMDB();
            try
            {
                sda = new SqlDataAdapter("SELECT BlockId,BlockName,CAST(0 as int) TotalFlat FROM dbo.BlockMaster WHERE CostCentreId = " + argCCId + " and BlockId IN(" + arg_BId.Trim(',') + ") ORDER BY SortOrder", BsfGlobal.g_CRMDB);
                dtFlatBlock = new DataTable();
                sda.Fill(dtFlatBlock);
                dtFlatBlock.Dispose();
            }

            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtFlatBlock;
        }

        public static DataTable GetFlatTypeDetails(int argProjId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            string sql ="";
            try
            {
                sql = "SELECT FlatTypeId,TypeName FROM dbo.FlatType WHERE ProjId=" + argProjId;
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
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

        public static DataTable GetLeadFlatTypeDetails(int argProjId, string argsType, int argLandId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            string sql = "";
            try
            {
                if (argsType == "B")
                    sql = "SELECT FlatTypeId,TypeName FROM dbo.FlatType WHERE ProjId=" + argProjId;
                else
                    sql = "Select PlotTypeId FlatTypeId,PlotTypeName TypeName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotType " +
                        " Where LandRegisterId=" + argLandId + "";
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
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

        public static DataTable GetFlatTypeRateDetails(int argProjId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            try
            {
                //string sql = "SELECT ProjId,FlatTypeId,Area,OtherCostAmt,TypeName,Rate OldRate,Rate NewRate,FloorwiseRate FROM" +
                //    " dbo.FlatType WHERE ProjId=" + argProjId;
                string sSql = "SELECT ProjId,FlatTypeId,Area,OtherCostAmt,TypeName,A.Rate OldRate,A.Rate NewRate,FloorwiseRate,B.GuideLineValue,A.AdvPercent FROM" +
                             " FlatType A Inner Join ProjectInfo B On A.ProjId=B.CostCentreId Where A.ProjId=" + argProjId + " Order By TypeName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
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

        public static DataTable GetFlatTypeRateHistory(int argProjId,string argDate,string argType)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            string sql ="";
            try
            {
                if (argType == "LandRate")
                    sql = "SELECT Date,OldGuideLine,NewGuideLine,OldMarketValue,NewMarketValue,OldRegValue,NewRegValue From LandRateChange " +
                        " WHERE Date<='" + argDate + "' And CostCentreId=" + argProjId;
                else

                    sql = "SELECT B.TypeName,IsNull(C.LevelName,'All Levels')LevelName,A.Date,A.OldRate,A.NewRate FROM" +
                        " ChangeRate A Inner Join FlatType B On A.FlatTypeId=B.FlatTypeId" +
                        " Left Join LevelMaster C On C.LevelId=A.LevelId And C.CostCentreId=" + argProjId + "" +
                        " WHERE Date<='" + argDate + "' And ProjId=" + argProjId + " Order By B.TypeName,C.LevelName";
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
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

        public static DataTable GetCarDetails()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT TypeId,TypeName FROM dbo.CarParkTypeMaster Order By TypeName", BsfGlobal.g_CRMDB);
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

        public static DataTable GetFTType(int argProjId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            try
            {
                string sSql = "SELECT CAST(0 as bit) Sel,F.FlatTypeId,F.TypeName,F.PayTypeId,P.TypeName PaySchName,F.Area,CAST('' as varchar(3)) Title " +
                              "FROM dbo.FlatType F INNER JOIN dbo.PaySchType P ON F.PayTypeId=P.TypeId WHERE ProjId= " + argProjId;
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);

                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
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

        public static DataTable GetFTTypewise(string argTId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            try
            {
                string sSql = String.Format("SELECT FlatTypeId,TypeName FROM dbo.FlatType WHERE FlatTypeId IN({0})", argTId.Trim(','));
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
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

        public static DataTable GetFlatType(int argFlatTypeId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = String.Format("SELECT * FROM dbo.FlatType WHERE FlatTypeId={0}", argFlatTypeId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
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

        public static DataTable GetFlat_Type(int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = String.Format("SELECT FlatTypeId,Typename,Area,Rate,BaseAmt,NetAmt FROM dbo.FlatType WHERE ProjId={0} ORDER BY TypeName", argCCId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
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

        public static DataTable GetAddi(int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql="";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = String.Format("Select ExtraItemTypeId,ExtraItemTypeName from dbo.ExtraItemTypeMaster Order by ExtraItemTypeName");
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
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

        public static DataTable GetAddiItem()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql="";
            BsfGlobal.OpenCRMDB();
            try
            {
                //sSql = String.Format("Select tm.ExtraItemTypeId,ExtraItemId,ItemDescription,DefaultRate,RevisedRate,UnitId,RAQty from [{0}].dbo.ExtraItemMaster m left outer join  [{0}].dbo.ExtraItemTypeMaster tm on  tm.ExtraItemTypeId=m.ExtraItemTypeId", CommFun.projectDBName);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
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

        public static DataSet GetFlatTypeDetailsEdit(int argFlatTypeId,int argCCId,int argFlatId)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = String.Format("select * from dbo.FlatType where FlatTypeID={0} ", argFlatTypeId);
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "FlatType");
                da.Dispose();

                sSql = String.Format("SELECT A.AreaId,A.Description,F.AreaSqft from dbo.FlatTypeArea F INNER JOIN dbo.AreaMaster A ON F.AreaId=A.AreaId where FlatTypeID={0}", argFlatTypeId);
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "FlatTypeArea");
                da.Dispose();

                sSql = String.Format("Select * From dbo.FlatTypeQualifier where FlatTypeID={0}", argFlatTypeId);
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "FlatTypeQualifier");
                da.Dispose();

                sSql = String.Format("Select OtherCostId,OtherCostName,Area,Rate,Flag,Amount From dbo.FlatTypeOtherCost where FlatTypeID={0}", argFlatTypeId);
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "FlatTypeOtherCost");
                da.Dispose();

                sSql = String.Format("Select A.TransId,B.ItemCode,B.ExtraItemTypeId,B.ItemDescription,B.ExtraRate,C.ExtraItemTypeName from dbo.FlatTypeExtraItem A Inner Join dbo.ExtraItemMaster B on A.TransId=B.TransId and A.FlatTypeId={0} Inner Join dbo.ExtraItemTypeMaster C on B.ExtraItemTypeId=C.ExtraItemTypeId WHERE B.CostCentreId={1} AND C.CostCentreId={1}", argFlatTypeId, argCCId);
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "FlatTypeExtraItem");
                da.Dispose();

                sSql = String.Format("Select F.CheckListId,F.FlatTypeId,F.Description,F.ExpCompletionDate,F.CompletionDate," +
                    " F.ExecutiveId Executive,F.Status From dbo.FlatTypeCheckList F  Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A  on F.ExecutiveId=A.UserId Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId  Where B.PositionType='M' and FlatTypeId={0}", argFlatTypeId);
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "FlatTypeCheckList");
                da.Dispose();

                sSql = "SELECT DISTINCT PT.TypeId,PT.TypeName from dbo.PaySchType PT INNER JOIN dbo.PaymentScheduleFlat PF" +
                     " ON PT.TypeId=PF.TypeId WHERE PF.FlatId=" + argFlatId + "";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "PaySchType");
                da.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
                ds.Dispose();
            }
            return ds;
        }

        public static DataTable PopulateFlatType(DataTable dtT,int argCCId)
        {
            DataTable dt=new DataTable();
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
                for (int i = 0; i < dtT.Rows.Count; i++)
                {
                    sSql = "SELECT * FROM dbo.FlatType WHERE FlatTypeID=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND ProjId=" + argCCId + "";
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    sda.Fill(dt);
                    dt.Dispose();
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

        public static DataTable GetFlatCheck()
        {
            SqlDataAdapter sda;
            DataTable dtBlock = null;
            BsfGlobal.OpenCRMDB();
            try
            {
                sda = new SqlDataAdapter("SELECT FlatId,FlatNo FROM dbo.FlatDetails WHERE CostCentreId <> 0 and Status='S'", BsfGlobal.g_CRMDB);
                dtBlock = new DataTable();
                sda.Fill(dtBlock);
                dtBlock.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtBlock;
        }

        public static DataTable GetBlockCheck(int argCCId)
        {
            SqlDataAdapter sda;
            DataTable dtBlock = null;
            BsfGlobal.OpenCRMDB();
            try
            {
                sda = new SqlDataAdapter("SELECT BlockId,BlockName FROM dbo.BlockMaster Where CostCentreId = " + argCCId + " ORDER BY BlockName", BsfGlobal.g_CRMDB);
                dtBlock = new DataTable();
                sda.Fill(dtBlock);
                dtBlock.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtBlock;
        }

        public static bool GetFloorValue(int argCCId,int argFlatTypeId)
        {
            DataTable dt = new DataTable();
            decimal dRate;
            bool bAns = false;
            try
            {
                string sSql = "Select Sum(R.Rate) Rate From dbo.FloorRate R Inner Join dbo.FlatType T On R.FlatTypeId=T.FlatTypeId" +
                                " Where T.ProjId=" + argCCId + " And R.FlatTypeId=" + argFlatTypeId + "";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dRate = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Rate"], CommFun.datatypes.vartypenumeric));
                    if (dRate == 0) bAns = true;
                }
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
            return bAns;
        }

        public static void UpdateFloorValue(int argFlatTypeId)
        {
            DataTable dt = new DataTable();
            try
            {
                string sSql = "Update dbo.FloorRate Set Rate=0 Where FlatTypeId=" + argFlatTypeId + "";
                BsfGlobal.OpenCRMDB();
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
        }

        public static bool GetAreaFound(int argFlatTypeId)
        {
            DataTable dt = new DataTable();
            bool bAns = false;
            try
            {
                string sSql = "Select AreaSqft From dbo.FlatTypeArea Where FlatTypeId=" + argFlatTypeId + "";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    bAns = true;
                }
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
            return bAns;
        }

        public static bool GetFlatAreaFound(int argFlatId)
        {
            DataTable dt = new DataTable();
            bool bAns = false;
            try
            {
                string sSql = "Select AreaSqft From dbo.FlatArea Where FlatId=" + argFlatId + "";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    bAns = true;
                }
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
            
            return bAns;
        }

        public static void InsertChangeRate(DataTable argdt,string argDate,DataTable argdtF)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            decimal dFTBaseAmt = 0; decimal dFTNetAmt = 0; decimal dFTAdvAmt = 0;
            decimal dFBaseAmt = 0; decimal dFNetAmt = 0; decimal dFAdvAmt = 0;

            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    for (int i = 0; i < argdt.Rows.Count; i++)
                    {
                            if (Convert.ToDecimal(argdt.Rows[i]["OldRate"]) != Convert.ToDecimal(argdt.Rows[i]["NewRate"]))
                            {
                                sSql = "Insert Into ChangeRate (CostCentreId,FlatTypeId,Date,OldRate,NewRate)Values" +
                                " (" + argdt.Rows[i]["ProjId"] + "," + argdt.Rows[i]["FlatTypeId"] + ",'" + argDate + "'," +
                                " " + argdt.Rows[i]["OldRate"] + "," + argdt.Rows[i]["NewRate"] + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                dFTBaseAmt = Convert.ToDecimal(argdt.Rows[i]["Area"]) * Convert.ToDecimal(argdt.Rows[i]["NewRate"]);

                                int iFTPayTypeId = 0;
                                sSql = "Select PayTypeId,ProjId From dbo.FlatType Where FlatTypeId=" + argdt.Rows[i]["FlatTypeId"] + "";
                                cmd = new SqlCommand(sSql, conn, tran);
                                SqlDataReader dr = cmd.ExecuteReader();
                                DataTable dtFT = new DataTable();
                                dtFT.Load(dr);
                                cmd.Dispose();

                                if (dtFT.Rows.Count > 0)
                                {
                                    iFTPayTypeId = Convert.ToInt32(dtFT.Rows[0]["PayTypeId"]);
                                }

                                decimal dFTOtherAmt = 0;
                                //sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatTypeOtherCost " +
                                //        "Where FlatTypeId = " + argdt.Rows[i]["FlatTypeId"] + " and OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans " +
                                //        " Where PayTypeId=" + iFTPayTypeId + " and CostCentreId=" + argdt.Rows[i]["ProjId"] + ")";
                                sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatTypeOtherCost " +
                                        " Where FlatTypeId =" + argdt.Rows[i]["FlatTypeId"] + " and OtherCostId not in (Select OtherCostId from dbo.OXGross " +
                                        " Where CostCentreId=" + argdt.Rows[i]["ProjId"] + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                dr = cmd.ExecuteReader();
                                DataTable dtFTOCost = new DataTable();
                                dtFTOCost.Load(dr);
                                dr.Close();
                                cmd.Dispose();

                                if (dtFTOCost.Rows.Count > 0) { dFTOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtFTOCost.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }

                                dFTNetAmt = dFTBaseAmt + dFTOtherAmt;
                                dFTAdvAmt = decimal.Round(dFTNetAmt * Convert.ToDecimal(argdt.Rows[i]["AdvPercent"]) / 100, 3);

                                sSql = "Update dbo.FlatType Set Rate=" + argdt.Rows[i]["NewRate"] + ", " +
                                    " BaseAmt=" + dFTBaseAmt + ",NetAmt=" + dFTNetAmt + ",AdvAmount=" + dFTAdvAmt + " " +
                                    " Where FlatTypeId=" + argdt.Rows[i]["FlatTypeId"] + "";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                if (argdt.Rows[i]["FloorwiseRate"].ToString() == "N")
                                {

                                sSql = "Select FlatId,Area,AdvPercent,PayTypeId From dbo.FlatDetails Where FlatTypeId=" + argdt.Rows[i]["FlatTypeId"] + "" +
                                    " And CostCentreId=" + argdt.Rows[i]["ProjId"] + " And Status='U'";
                                cmd = new SqlCommand(sSql, conn, tran);
                                dr = cmd.ExecuteReader();
                                DataTable dt = new DataTable();
                                dt.Load(dr);
                                cmd.Dispose();

                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    dFBaseAmt = Convert.ToDecimal(dt.Rows[j]["Area"]) * Convert.ToDecimal(argdt.Rows[i]["NewRate"]);
                                    int iPayTypeId = Convert.ToInt32(dt.Rows[j]["PayTypeId"]);

                                    decimal dOtherAmt = 0;
                                    //sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                                    //        "Where FlatId = " + dt.Rows[j]["FlatId"] + " and OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans " +
                                    //        " Where PayTypeId=" + iPayTypeId + " and CostCentreId=" + argdt.Rows[i]["ProjId"] + ")";
                                    sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                                            " Where FlatId =" + dt.Rows[j]["FlatId"] + " and OtherCostId not in (Select OtherCostId from dbo.OXGross " +
                                            " Where CostCentreId=" + argdt.Rows[i]["ProjId"] + ")";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    dr = cmd.ExecuteReader();
                                    DataTable dtOCost = new DataTable();
                                    dtOCost.Load(dr);
                                    dr.Close();
                                    cmd.Dispose();

                                    if (dtOCost.Rows.Count > 0) { dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtOCost.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }


                                    dFNetAmt = dFBaseAmt + dOtherAmt;
                                    dFAdvAmt = decimal.Round(dFNetAmt * Convert.ToDecimal(dt.Rows[j]["AdvPercent"]) / 100, 3);

                                    sSql = "Update FlatDetails Set Rate=" + argdt.Rows[i]["NewRate"] + ", " +
                                        " BaseAmt=" + dFBaseAmt + ",NetAmt=" + dFNetAmt + ",AdvAmount=" + dFAdvAmt + " " +
                                        " Where FlatId=" + dt.Rows[j]["FlatId"] + " ";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();

                                    PaymentScheduleBL.InsertFlatScheduleI(Convert.ToInt32(dt.Rows[j]["FlatId"]), conn, tran);
                                }
                            }
                        }
                    }


                    if (argdtF != null)
                    {
                        for (int x = 0; x < argdtF.Rows.Count; x++)
                        {
                            if (Convert.ToDecimal(argdtF.Rows[x]["OldRate"]) != Convert.ToDecimal(argdtF.Rows[x]["NewRate"]))
                            {
                                sSql = "Delete From ChangeRate Where CostCentreId=" + argdtF.Rows[x]["CostCentreId"] + "" +
                                    " And FlatTypeId=" + argdtF.Rows[x]["FlatTypeId"] + " And LevelId=0";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                sSql = "Insert Into ChangeRate (CostCentreId,FlatTypeId,Date,LevelId,OldRate,NewRate)Values" +
                                " (" + argdtF.Rows[x]["CostCentreId"] + "," + argdtF.Rows[x]["FlatTypeId"] + ",'" + argDate + "'," +
                                " " + argdtF.Rows[x]["LevelId"] + "," + argdtF.Rows[x]["OldRate"] + "," + argdtF.Rows[x]["NewRate"] + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                                

                                sSql = "Update FloorRate Set Rate=" + argdtF.Rows[x]["NewRate"] + " " +
                                        " Where FlatTypeId=" + argdtF.Rows[x]["FlatTypeId"] + " And LevelId=" + argdtF.Rows[x]["LevelId"] + "";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                sSql = "Select FlatId,Area,AdvPercent,PayTypeId From dbo.FlatDetails Where FlatTypeId=" + argdtF.Rows[x]["FlatTypeId"] + "" +
                                      " And LevelId=" + argdtF.Rows[x]["LevelId"] + " And CostCentreId=" + argdtF.Rows[x]["CostCentreId"] + " And Status='U'";
                                cmd = new SqlCommand(sSql, conn, tran);
                                SqlDataReader dr = cmd.ExecuteReader();
                                DataTable dt = new DataTable();
                                dt.Load(dr);
                                cmd.Dispose();

                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    dFBaseAmt = Convert.ToDecimal(dt.Rows[j]["Area"]) * Convert.ToDecimal(argdtF.Rows[x]["NewRate"]);
                                    int iPayTypeId = Convert.ToInt32(dt.Rows[j]["PayTypeId"]);

                                    decimal dOtherAmt = 0;
                                    //sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount FROM dbo.FlatOtherCost " +
                                    //        "Where FlatId = " + dt.Rows[j]["FlatId"] + " and OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans " +
                                    //        " Where PayTypeId=" + iPayTypeId + " and CostCentreId=" + argdtF.Rows[x]["CostCentreId"] + ")";
                                    sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                                            " Where FlatId =" + dt.Rows[j]["FlatId"] + " and OtherCostId not in (Select OtherCostId from dbo.OXGross " +
                                            " Where CostCentreId=" + argdtF.Rows[x]["CostCentreId"] + ")";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    dr = cmd.ExecuteReader();
                                    DataTable dtOCost = new DataTable();
                                    dtOCost.Load(dr);
                                    dr.Close();
                                    cmd.Dispose();

                                    if (dtOCost.Rows.Count > 0) { dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtOCost.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }

                                    dFNetAmt = dFBaseAmt + dOtherAmt;
                                    dFAdvAmt = decimal.Round(dFNetAmt * Convert.ToDecimal(dt.Rows[j]["AdvPercent"]) / 100, 3);

                                    sSql = "Update dbo.FlatDetails Set Rate=" + argdtF.Rows[x]["NewRate"] + ", " +
                                        " BaseAmt=" + dFBaseAmt + ",NetAmt=" + dFNetAmt + ",AdvAmount=" + dFAdvAmt + " " +
                                        " Where FlatId=" + dt.Rows[j]["FlatId"] + " ";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();

                                    PaymentScheduleBL.InsertFlatScheduleI(Convert.ToInt32(dt.Rows[j]["FlatId"]), conn, tran);
                                }
                            }
                        }
                    }
                    
                    tran.Commit();
                }
                catch (Exception e)
                {
                    BsfGlobal.CustomException(e.Message, e.StackTrace);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static DataTable GetCostSht(int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            DataTable dtT = new DataTable();
            int iFlatId = 0;
            DataView dv; decimal dAmt = 0; DataRow[] drT;

            try
            {
                //sSql = "Select ProjId CostCentreId,FlatTypeId,PayTypeId,TypeName,Area From dbo.FlatType " +
                //         " Where ProjId=" + argCCId + " And PayTypeId=" + argPayTypeId + "";
                sSql = "Select A.FlatId,B.BlockName,C.LevelName,D.TypeName,A.FlatNo,A.Area,A.BaseAmt,A.USLandAmt LandCost From dbo.FlatDetails A " +
                        " Inner Join dbo.BlockMaster B On A.BlockId=B.BlockId " +
                        " Inner Join dbo.LevelMaster C On C.LevelId=A.LevelId " +
                        " Inner Join dbo.FlatType D On D.FlatTypeId=A.FlatTypeId Where A.CostCentreId=" + argCCId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
                dtT = dt;

                //sSql = "Select TemplateId,Description From dbo.PaymentSchedule Where CostCentreId=" + argCCId + " And TypeId=" + argPayTypeId + "";
                sSql = "Select OtherCostName From dbo.OtherCostMaster";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string s = dt.Rows[i]["OtherCostName"].ToString();
                    dtT.Columns.Add(s, typeof(string));
                }
                dtT.Columns.Add("Total", typeof(decimal));

                //sSql = "Select Distinct A.CostCentreId,FlatTypeId,PayTypeId,TemplateId,SchPercent,Description,B.NetAmt,(B.NetAmt*SchPercent)/100 Amount,SortOrder " +
                //        " From dbo.PaymentSchedule A Inner Join FlatType B On A.CostCentreId=B.ProjId And A.TypeId=B.PayTypeId " +
                //        " Where ProjId=" + argCCId + " And TypeId=" + argPayTypeId + " Order By A.SortOrder";
                sSql = "Select B.FlatId,A.OtherCostName,IsNull(B.Amount,0)Amount From dbo.OtherCostMaster A Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();

                if (dtT.Columns.Count > 0)
                {
                    for (int i = 8; i < dtT.Columns.Count; i++)
                    {
                        string sName = dtT.Columns[i].Caption;
                        for (int j = 0; j < dtT.Rows.Count; j++)
                        {
                            iFlatId = Convert.ToInt32(dtT.Rows[j]["FlatId"].ToString());

                            dv = new DataView(dt);
                            dv.RowFilter = "FlatId=" + iFlatId + " And OtherCostName='" + sName + "'";
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                dAmt = Convert.ToDecimal(CommFun.IsNullCheck(dv.ToTable().Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));

                            }

                            drT = dt.Select("FlatId=" + iFlatId + " And OtherCostName='" + sName + "'");
                            if (drT.Length > 0)
                            {
                                dtT.Rows[j][i] = decimal.Round(dAmt, 3);
                                drT[0]["Amount"] = decimal.Round(dAmt, 3);
                            }
                        }
                    }
                }

                decimal iTot = 0;
                if (dtT.Rows.Count > 0)
                {
                    for (int k = 0; k < dtT.Rows.Count; k++)
                    {
                        decimal iTTot = 0;
                        for (int j = 8; j < dtT.Columns.Count - 1; j++)
                        {
                            iTot = Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[k][j], CommFun.datatypes.vartypenumeric));
                            iTTot = iTTot + iTot;
                        }
                        dtT.Rows[k]["Total"] = iTTot.ToString().Trim();
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
            return dtT;

        }

        public static DataTable GetCostSheet(int argCCId, bool argTypewise)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtT = new DataTable();
            try
            {
                string sSql = "Select A.FlatId,B.BlockName,C.LevelName,D.TypeName,A.FlatNo," +
                        " Case When A.Status='S' Then 'Sold' When A.Status='B' Then 'Block' Else 'UnSold' End Status, " +
                        " A.Area,A.USLandAmt LandCost,A.BaseAmt From dbo.FlatDetails A " +
                        " Inner Join dbo.BlockMaster B On A.BlockId=B.BlockId " +
                        " Inner Join dbo.LevelMaster C On C.LevelId=A.LevelId " +
                        " Inner Join dbo.FlatType D On D.FlatTypeId=A.FlatTypeId " +
                        " Where A.CostCentreId=" + argCCId + 
                        " Order By B.SortOrder,C.SortOrder,dbo.Val(A.FlatNo)";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader sdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(sdr, LoadOption.OverwriteChanges);
                sdr.Close();
                cmd.Dispose();

                dtT = dt;

                bool b_Group = false;
                sSql = "Select * From dbo.OtherCostMaster Where GroupId<>0";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                sdr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(sdr, LoadOption.OverwriteChanges);
                sdr.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    b_Group = true;
                }

                if (b_Group == false)
                { 
                    sSql = "Select OtherCostName From dbo.OtherCostMaster A "+
                           " Inner Join  dbo.CCOtherCost B On A.OtherCostId=B.OtherCostId "+
                           " Where B.CostcentreId=" + argCCId + "" +
                           " Union All " +
                           " Select QualifierName OtherCostName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp "+
                           " Where QualType='B'";
                }
                else
                {
                    sSql = "Select C.GroupName OtherCostName From dbo.OtherCostMaster A " +
                            " Inner Join  dbo.CCOtherCost B On A.OtherCostId=B.OtherCostId " +
                            " Inner Join dbo.OtherCostGroup C On C.GroupId=A.GroupId" +
                            " Where B.CostcentreId=" + argCCId + " "+
                            " Group By C.GroupName" +
                            " Union All " +
                            " Select QualifierName OtherCostName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp "+
                            " Where QualType='B'";
                }
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                sdr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(sdr, LoadOption.OverwriteChanges);
                sdr.Close();
                dt.Dispose();

                if (argTypewise == true)
                {
                    if (b_Group == false)
                    {
                        sSql = "Select B.FlatId,A.OtherCostName,IsNull(B.Amount,0)Amount From dbo.OtherCostMaster A " +
                               " Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId " +
                               " Union All " +
                               " Select FlatId,C.QualifierName OtherCostName,Sum(A.Amount)Amount From dbo.FlatReceiptQualifier A  " +
                               " Inner Join dbo.FlatReceiptType B On A.SchId=B.SchId " +
                               " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On A.QualifierId=C.QualifierId  " +
                               " Where QualType='B' Group By FlatId,C.QualifierName";
                    }
                    else
                    {
                        sSql = "Select B.FlatId,C.GroupName OtherCostName,IsNull(SUM(B.Amount),0)Amount From dbo.OtherCostMaster A " +
                               " Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId " +
                               " Inner Join dbo.OtherCostGroup C On C.GroupId=A.GroupId "+
                               " Group By B.FlatId,C.GroupName" +
                               " Union All " +
                               " Select FlatId,C.QualifierName OtherCostName,Sum(A.Amount)Amount From dbo.FlatReceiptQualifier A  " +
                               " Inner Join dbo.FlatReceiptType B On A.SchId=B.SchId " +
                               " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On A.QualifierId=C.QualifierId  " +
                               " Where QualType='B' Group By FlatId,C.QualifierName";
                    }
                }
                else
                {
                    if (b_Group == false)
                    {
                        sSql = "Select B.FlatId,A.OtherCostName,IsNull(B.Amount,0)Amount From dbo.OtherCostMaster A "+
                               " Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId " +
                               " Union All " +
                               " Select B.FlatId,A.QualifierName OtherCostName,Amount from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A " +
                               " Inner Join dbo.FlatTax B On A.QualifierId=B.QualifierId " +
                               " Where QualType='B' ";
                    }
                    else
                    {
                        sSql = "Select B.FlatId,C.GroupName OtherCostName,IsNull(SUM(B.Amount),0)Amount From dbo.OtherCostMaster A " +
                                " Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId  " +
                                " Inner Join dbo.OtherCostGroup C On C.GroupId=A.GroupId "+
                                " Group By B.FlatId,C.GroupName" +
                                " Union All " +
                                " Select B.FlatId,A.QualifierName OtherCostName,Amount from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A " +
                                " Inner Join dbo.FlatTax B On A.QualifierId=B.QualifierId " +
                                " Where QualType='B' ";
                    }
                }
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                sdr = cmd.ExecuteReader();
                DataTable dtA = new DataTable();
                dtA.Load(sdr, LoadOption.OverwriteChanges);
                sdr.Close();
                cmd.Dispose();

                string sOCName = "";
                if (dtA.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sOCName = dt.Rows[i]["OtherCostName"].ToString();

                        DataColumn col1 = new DataColumn(sOCName) { DataType = typeof(decimal), DefaultValue = 0 };
                        dtT.Columns.Add(col1);

                        DataView dv = new DataView(dtA) { RowFilter = String.Format("OtherCostName='{0}'", sOCName) };
                        DataTable dtRecv = new DataTable(); 
                        dtRecv = dv.ToTable();

                        for (int j = 0; j < dtRecv.Rows.Count; j++)
                        {
                            int iFlatId = Convert.ToInt32(dtRecv.Rows[j]["FlatId"]);
                            decimal dAmt = Convert.ToDecimal(dtRecv.Rows[j]["Amount"]);

                            DataRow[] drT = dtT.Select(String.Format("FlatId={0}", iFlatId));
                            if (drT.Length > 0)
                            {
                                drT[0][sOCName] = dAmt;
                            }
                        }

                        dtRecv.Dispose();
                        dv.Dispose();
                    }
                }

                dtT.Columns.Add("Total", typeof(decimal));

                decimal iTot = 0;
                if (dtT.Rows.Count > 0)
                {
                    for (int k = 0; k < dtT.Rows.Count; k++)
                    {
                        decimal iTTot = 0;
                        for (int j = 8; j < dtT.Columns.Count - 1; j++)
                        {
                            iTot = Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[k][j], CommFun.datatypes.vartypenumeric));
                            iTTot = iTTot + iTot;
                        }
                        dtT.Rows[k]["Total"] = iTTot.ToString().Trim();
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
            return dtT;

        }

        //public static DataTable GetAECostSheet(int argCCId, bool argTypewise,DateTime argDate)
        //{
        //    DataTable dt = null;
        //    SqlCommand cmd; SqlDataReader dr;
        //    BsfGlobal.OpenCRMDB();
        //    string sSql = "";
        //    DataTable dtT = new DataTable();
        //    int iFlatId = 0;
        //    DataView dv; decimal dAmt = 0; DataRow[] drT;

        //    try
        //    {
        //        sSql = "Select A.FlatId,L.LeadName,A.FlatNo, " +
        //                " A.Area,A.Rate,A.USLand UDS,A.LandRate LandValue,A.BaseAmt From dbo.FlatDetails A " +
        //                " Inner Join dbo.BlockMaster B On A.BlockId=B.BlockId " +
        //                " Inner Join dbo.LevelMaster C On C.LevelId=A.LevelId " +
        //                " Inner Join dbo.FlatType D On D.FlatTypeId=A.FlatTypeId " +
        //                " Inner Join dbo.LeadRegister L On L.LeadId=A.LeadId " +
        //                " Where A.CostCentreId=" + argCCId + " And A.Status='S' " +
        //                " Order By B.SortOrder,C.SortOrder,dbo.Val(A.FlatNo)";
        //        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
        //        dr = cmd.ExecuteReader();
        //        dt = new DataTable();
        //        dt.Load(dr, LoadOption.OverwriteChanges);
        //        dt.Dispose();
        //        dr.Close();
        //        dtT = dt;

        //        //Add OtherCost Column with Amount
        //        sSql = "Select OtherCostName From dbo.OtherCostMaster A " +
        //                 " Inner Join dbo.CCOtherCost B On A.OtherCostId=B.OtherCostId Where B.CostcentreId=" + argCCId + " And A.OtherCostId<>6";
        //        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
        //        dr = cmd.ExecuteReader();
        //        dt = new DataTable();
        //        dt.Load(dr, LoadOption.OverwriteChanges);
        //        dr.Close();
        //        dt.Dispose();


        //        sSql = "Select B.FlatId,A.OtherCostName,IsNull(B.Amount,0)Amount From dbo.OtherCostMaster A " +
        //                 " Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId Where A.OtherCostId<>6";
        //        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
        //        dr = cmd.ExecuteReader();
        //        DataTable dtA = new DataTable();
        //        dtA.Load(dr, LoadOption.OverwriteChanges);
        //        dr.Close();
        //        dtA.Dispose();

        //        string sOCName = "";
        //        if (dtA.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                sOCName = dt.Rows[i]["OtherCostName"].ToString();

        //                DataColumn col1 = new DataColumn(sOCName) { DataType = typeof(decimal), DefaultValue = 0 };
        //                dtT.Columns.Add(col1);


        //                DataTable dtRecv = new DataTable();
        //                dv = new DataView(dtA) { RowFilter = String.Format("OtherCostName='{0}' ", sOCName) };
        //                dtRecv = dv.ToTable();

        //                for (int j = 0; j < dtRecv.Rows.Count; j++)
        //                {
        //                    iFlatId = Convert.ToInt32(dtRecv.Rows[j]["FlatId"]);
        //                    dAmt = Convert.ToDecimal(dtRecv.Rows[j]["Amount"]);

        //                    drT = dtT.Select(String.Format("FlatId={0}", iFlatId));

        //                    if (drT.Length > 0)
        //                    {
        //                        drT[0][sOCName] = dAmt;
        //                    }
        //                }

        //                dtRecv.Dispose();
        //                dv.Dispose();
        //            }
        //        }

        //        //Total Column
        //        dtT.Columns.Add("Total", typeof(decimal));

        //        decimal iTot = 0;
        //        if (dtT.Rows.Count > 0)
        //        {
        //            for (int k = 0; k < dtT.Rows.Count; k++)
        //            {
        //                decimal iTTot = 0;
        //                for (int j = 7; j < dtT.Columns.Count - 1; j++)
        //                {
        //                    iTot = Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[k][j], CommFun.datatypes.vartypenumeric));
        //                    iTTot = iTTot + iTot;
        //                }
        //                dtT.Rows[k]["Total"] = iTTot.ToString().Trim();
        //            }
        //        }

        //        //Add Separate Column for MaintenanceCharges
        //        dtT.Columns.Add("MaintenanceCharges", typeof(decimal));


        //        sSql = "Select B.FlatId,A.OtherCostName,IsNull(B.Amount,0)Amount From dbo.OtherCostMaster A " +
        //            " Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId Where A.OtherCostId=6";
        //        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
        //        dr = cmd.ExecuteReader();
        //        dt = new DataTable();
        //        dt.Load(dr, LoadOption.OverwriteChanges);
        //        dr.Close();
        //        dt.Dispose();

        //        for (int j = 0; j < dt.Rows.Count; j++)
        //        {
        //            iFlatId = Convert.ToInt32(dt.Rows[j]["FlatId"]);
        //            dAmt = Convert.ToDecimal(dt.Rows[j]["Amount"]);

        //            drT = dtT.Select(String.Format("FlatId={0}", iFlatId));

        //            if (drT.Length > 0)
        //            {
        //                drT[0]["MaintenanceCharges"] = dAmt;
        //            }
        //        }

        //        //GrandTotal
        //        dtT.Columns.Add("GrandTotal", typeof(decimal));

        //        if (dtT.Rows.Count > 0)
        //        {
        //            decimal iTot1 = 0;
        //            for (int k = 0; k < dtT.Rows.Count; k++)
        //            {
        //                decimal iTTot = 0;
        //                for (int j = dtT.Columns.Count-3; j < dtT.Columns.Count-1; j++)
        //                {
        //                    iTot1 = Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[k][j], CommFun.datatypes.vartypenumeric));
        //                    iTTot = iTTot + iTot1;
        //                }
        //                dtT.Rows[k]["GrandTotal"] = iTTot.ToString().Trim();
        //            }
        //        }

        //        //Add Qualifer Columns
        //        //if (argTypewise == true)
        //        //{
        //        sSql = "Select 'AmountReceivedLand' QualifierName,0 Amount   " +
        //                " UNION ALL " +
        //                "Select 'PaidGrossAmount' QualifierName,0 Amount   " +
        //                " UNION ALL " +
        //                " Select (QualifierName + ' @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),Net), 101) )+ ' % ' AS QualifierName,Net NetPer  From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp Where QualType='B'" +
        //                //" Select(QualifierName + ' @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ' AS QualifierName, "+
        //                //" Sum(B.Amount)Amount From ReceiptShTrans A  Inner Join ReceiptQualifier B On A.ReceiptId=B.ReceiptId "+
        //                //" And A.PaymentSchId=B.PaymentSchId And A.OtherCostId=B.OtherCostId And A.ReceiptTypeId=B.ReceiptTypeId "+ 
        //                //" Inner Join ReceiptRegister C On C.ReceiptId=A.ReceiptId  Inner Join [AERate].dbo.Qualifier_Temp D On D.QualifierId=B.QualifierId "+
        //                //" And QualType='B'  Where C.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1 " +
        //                //" Group By (QualifierName + ' @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ' "+
        //                " UNION ALL" +
        //                " Select 'PaidNetAmount' QualifierName,0 Amount   " +
        //                " UNION ALL " +
        //                " Select 'Balance' QualifierName,0 Amount   ";
        //        //}
        //        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
        //        dr = cmd.ExecuteReader();
        //        dt = new DataTable();
        //        dt.Load(dr, LoadOption.OverwriteChanges);
        //        dr.Close();
        //        dt.Dispose();

        //        //Add Qualifier Amounts
                
        //        sSql = "SELECT FlatId,0 QualifierId,'AmountReceivedLand' QualifierName,0 NetPer,isnull(SUM(PaidGrossAmount),0) Amount FROM ReceiptShTrans A   " +
        //                " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId WHERE B.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PaidNetAmount<>0 AND ReceiptTypeId=2  " +
        //                " GROUP BY FlatId " +
        //                " UNION ALL " +
        //                " SELECT FlatId,0 QualifierId,'PaidGrossAmount' QualifierName,0 NetPer,isnull(SUM(PaidGrossAmount),0) Amount FROM ReceiptShTrans A  " +
        //                " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId  " +
        //                " WHERE B.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PaidNetAmount<>0 AND ReceiptTypeId<>1 " +
        //                " GROUP BY FlatId" +
        //                " UNION ALL" +
        //                " Select A.FlatId,B.QualifierId,(D.QualifierName + ' @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),Net), 101) )+ ' % ' AS QualifierName,B.NetPer,Sum(B.Amount)Amount From ReceiptShTrans A " +
        //                " Inner Join ReceiptQualifier B On A.ReceiptId=B.ReceiptId And A.PaymentSchId=B.PaymentSchId And A.OtherCostId=B.OtherCostId And A.ReceiptTypeId=B.ReceiptTypeId " +
        //                " Inner Join ReceiptRegister C On C.ReceiptId=A.ReceiptId " +
        //                " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp D On D.QualifierId=B.QualifierId And QualType='B' " +
        //                " Where C.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' " +
        //                " AND A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1 " +
        //                " Group By A.FlatId,B.QualifierId,(D.QualifierName + ' @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),Net), 101) )+ ' % ',B.NetPer" +
        //                " UNION ALL " +
        //                " SELECT FlatId,0 QualifierId,'PaidNetAmount' QualifierName,0 NetPer,isnull(SUM(PaidNetAmount),0) Amount FROM ReceiptShTrans A  " +
        //                " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId  " +
        //                " WHERE B.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PaidNetAmount<>0 AND ReceiptTypeId<>1 " +
        //                " GROUP BY FlatId " +
        //                " UNION ALL  " +
        //                " SELECT FlatId,0 QualifierId,'Balance' QualifierName,0 NetPer,SUM(PaidNetAmount)-SUM(PaidGrossAmount) Amount FROM ReceiptShTrans A   " +
        //                " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId WHERE B.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PaidNetAmount<>0 AND ReceiptTypeId<>1  " +
        //                " GROUP BY FlatId";
        //        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
        //        dr = cmd.ExecuteReader();
        //        DataTable dtQ = new DataTable();
        //        dtQ.Load(dr, LoadOption.OverwriteChanges);
        //        dr.Close();
        //        dtQ.Dispose();

        //        string sQualName = ""; 
        //        if (dtQ.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                sQualName = dt.Rows[i]["QualifierName"].ToString();

        //                DataColumn col1 = new DataColumn(sQualName) { DataType = typeof(decimal), DefaultValue = 0 };
        //                dtT.Columns.Add(col1);


        //                DataTable dtRecv = new DataTable();
        //                dv = new DataView(dtQ) { RowFilter = String.Format("QualifierName='{0}' ", sQualName) };
        //                dtRecv = dv.ToTable();

        //                for (int j = 0; j < dtRecv.Rows.Count; j++)
        //                {
        //                    iFlatId = Convert.ToInt32(dtRecv.Rows[j]["FlatId"]);
        //                    dAmt = Convert.ToDecimal(dtRecv.Rows[j]["Amount"]);

        //                    drT = dtT.Select(String.Format("FlatId={0}", iFlatId)); 

        //                    if (drT.Length > 0)
        //                    {
        //                        drT[0][sQualName] = dAmt;
        //                    }
        //                }

        //                dtRecv.Dispose();
        //                dv.Dispose();
        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        BsfGlobal.CustomException(e.Message, e.StackTrace);
        //    }
        //    finally
        //    {
        //        BsfGlobal.g_CRMDB.Close();
        //    }
        //    return dtT;

        //}

        public static DataTable GetAECostSheet(int argCCId, bool argTypewise, DateTime argDate)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dtT = new DataTable();
            try
            {
                string sSql = "Select A.FlatId,L.LeadName,A.FlatNo,D.TypeName, " +
                        " E.FinaliseDate BookingDate,B.BlockName,C.LevelName, " +
                        " ISNULL(LPC.CampaignName,'') Campaign, ISNULL(LU.UserName,'') Executive," +
                        " A.Area,A.Rate,A.USLand UDS,A.LandRate LandValue,A.BaseAmt From dbo.FlatDetails A " +
                        " Inner Join dbo.BlockMaster B On A.BlockId=B.BlockId " +
                        " Inner Join dbo.LevelMaster C On C.LevelId=A.LevelId " +
                        " Inner Join dbo.FlatType D On D.FlatTypeId=A.FlatTypeId " +
                        " Inner Join dbo.LeadRegister L On L.LeadId=A.LeadId " +
                        " left Join dbo.LeadProjectInfo LP On L.LeadId=LP.LeadId AND LP.CampaignId<>0 and LP.CampaignId IS NOT NULL" +
                        " left Join dbo.CampaignDetails LPC On LP.CampaignId=LPC.CampaignId AND A.CostCentreId=LPC.CCId " +
                        " Inner Join dbo.BuyerDetail E On E.FlatId=A.FlatId And A.LeadId=E.LeadId And E.Status='S' " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users LU On E.ExecutiveId=LU.UserId " +
                        " Where A.CostCentreId=" + argCCId + " And A.Status='S' " +
                        " Order By B.SortOrder,C.SortOrder,dbo.Val(FlatNo)";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr, LoadOption.OverwriteChanges);
                dr.Close();
                cmd.Dispose();

                dtT = dt;

                //Add OtherCost Column with Amount
                sSql = "Select OtherCostName From dbo.OtherCostMaster A " +
                         " Inner Join dbo.CCOtherCost B On A.OtherCostId=B.OtherCostId " +
                         " Where B.CostcentreId=" + argCCId + " And A.OtherCostId<>6 Order By A.OtherCostId";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr, LoadOption.OverwriteChanges);
                dr.Close();
                cmd.Dispose();


                sSql = "Select B.FlatId,A.OtherCostName,IsNull(B.Amount,0)Amount From dbo.OtherCostMaster A " +
                         " Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId " +
                         " Where A.OtherCostId<>6 Order By A.OtherCostId";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                DataTable dtA = new DataTable();
                dtA.Load(dr, LoadOption.OverwriteChanges);
                dr.Close();
                cmd.Dispose();

                string sOCName = "";
                if (dtA.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sOCName = dt.Rows[i]["OtherCostName"].ToString();

                        DataColumn col1 = new DataColumn(sOCName) { DataType = typeof(decimal), DefaultValue = 0 };
                        dtT.Columns.Add(col1);

                        DataTable dtRecv = new DataTable();
                        DataView dv = new DataView(dtA) { RowFilter = String.Format("OtherCostName='{0}' ", sOCName) };
                        dtRecv = dv.ToTable();

                        for (int j = 0; j < dtRecv.Rows.Count; j++)
                        {
                            int iFlatId = Convert.ToInt32(dtRecv.Rows[j]["FlatId"]);
                            decimal dAmt = Convert.ToDecimal(dtRecv.Rows[j]["Amount"]);

                            DataRow[] drT = dtT.Select(String.Format("FlatId={0}", iFlatId));
                            if (drT != null)
                            {
                                if (drT.Length > 0)
                                {
                                    drT[0][sOCName] = dAmt;
                                }
                            }
                        }

                        dtRecv.Dispose();
                        dv.Dispose();
                    }
                }

                //Total Column
                dtT.Columns.Add("Total", typeof(decimal));

                //j=9 From BaseAmount to OC, adding total
                decimal iTot = 0;
                if (dtT.Rows.Count > 0)
                {
                    for (int k = 0; k < dtT.Rows.Count; k++)
                    {
                        decimal iTTot = 0;
                        for (int j = 9; j < dtT.Columns.Count - 2; j++)
                        {
                            iTot = Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[k][j], CommFun.datatypes.vartypenumeric));
                            iTTot = iTTot + iTot;
                        }
                        dtT.Rows[k]["Total"] = iTTot.ToString().Trim();
                    }
                }

                //Add Separate Column for MaintenanceCharges
                dtT.Columns.Add("MaintenanceCharges", typeof(decimal));


                sSql = "Select B.FlatId,A.OtherCostName,IsNull(B.Amount,0)Amount From dbo.OtherCostMaster A " +
                    " Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId " +
                    " Where A.OtherCostId=6";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr, LoadOption.OverwriteChanges);
                dr.Close();
                cmd.Dispose();

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    int iFlatId = Convert.ToInt32(dt.Rows[j]["FlatId"]);
                    decimal dAmt = Convert.ToDecimal(dt.Rows[j]["Amount"]);

                    DataRow[] drT = dtT.Select(String.Format("FlatId={0}", iFlatId));
                    if (drT != null)
                    {
                        if (drT.Length > 0)
                        {
                            drT[0]["MaintenanceCharges"] = dAmt;
                        }
                    }
                }

                //GrandTotal
                dtT.Columns.Add("GrandTotal", typeof(decimal));

                //calculating grand total except last 3 rows
                if (dtT.Rows.Count > 0)
                {
                    decimal iTot1 = 0;
                    for (int k = 0; k < dtT.Rows.Count; k++)
                    {
                        decimal iTTot = 0;
                        for (int j = dtT.Columns.Count - 3; j < dtT.Columns.Count - 1; j++)
                        {
                            iTot1 = Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[k][j], CommFun.datatypes.vartypenumeric));
                            iTTot = iTTot + iTot1;
                        }
                        dtT.Rows[k]["GrandTotal"] = iTTot.ToString().Trim();
                    }
                }

                //Add Qualifer Columns
                //if (argTypewise == true)
                //{
                sSql = "Select 'AmountReceivedLand' QualifierName,0 Amount   " +
                        " UNION ALL " +
                        "Select 'PaidGrossAmount' QualifierName,0 Amount   " +
                        " UNION ALL " +
                        " Select('PaidGross @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ' AS QualifierName, 0 Amount From ReceiptShTrans A " +
                        " Inner Join ReceiptQualifier B On A.ReceiptId=B.ReceiptId  And A.PaymentSchId=B.PaymentSchId And A.OtherCostId=B.OtherCostId And A.ReceiptTypeId=B.ReceiptTypeId " +
                        " Inner Join ReceiptRegister C On C.ReceiptId=A.ReceiptId  " +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp D On D.QualifierId=B.QualifierId And QualType='B' " +
                        " Where C.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' " +
                        " AND A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1 AND C.CostCentreId=" + argCCId +
                        " Group By ('PaidGross @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ' " +
                        " UNION ALL " +
                        " Select(QualifierName + ' @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ' AS QualifierName,Sum(B.Amount)Amount From ReceiptShTrans A " +
                        " Inner Join ReceiptQualifier B On A.ReceiptId=B.ReceiptId " +
                        " And A.PaymentSchId=B.PaymentSchId And A.OtherCostId=B.OtherCostId And A.ReceiptTypeId=B.ReceiptTypeId " +
                        " Inner Join ReceiptRegister C On C.ReceiptId=A.ReceiptId " +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp D On D.QualifierId=B.QualifierId And QualType='B' " +
                        " Where C.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1 AND C.CostCentreId=" + argCCId + " " +
                        " Group By (QualifierName + ' @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ' " +
                        " UNION ALL" +
                        " Select 'PaidNetAmount' QualifierName,0 Amount   " +
                        " UNION ALL " +
                        " Select 'Balance' QualifierName,0 Amount ";
                //}
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr, LoadOption.OverwriteChanges);
                dr.Close();
                cmd.Dispose();


                //Add Qualifier Amounts

                sSql = "SELECT DISTINCT B.FlatId,0 QualifierId,'AmountReceivedLand' QualifierName,0 NetPer,isnull(SUM(PaidGrossAmount),0) Amount FROM ReceiptShTrans A   " +
                        " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                        " WHERE B.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PaidNetAmount<>0 AND ReceiptTypeId=2 AND B.CostCentreId=" + argCCId + " " +
                        " GROUP BY B.FlatId " +
                        " UNION ALL " +
                        " SELECT DISTINCT B.FlatId,0 QualifierId,'PaidGrossAmount' QualifierName,0 NetPer,isnull(SUM(PaidGrossAmount),0) Amount FROM ReceiptShTrans A  " +
                        " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId  " +
                        " WHERE B.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PaidNetAmount<>0 AND ReceiptTypeId<>1 AND B.CostCentreId=" + argCCId + " " +
                        " GROUP BY B.FlatId" +
                        " UNION ALL" +
                        " Select DISTINCT FlatId,QualifierId,QualifierName,NetPer,SUM(PaidGross) Amount From ( " +
                        " Select A.ReceiptTypeId,A.OtherCostId,A.FlatId,B.QualifierId,('PaidGross @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ' AS QualifierName,B.NetPer, " +
                        " Sum(PaidGrossAmount) PaidGross,Sum(B.Amount)Amount From ReceiptShTrans A  Inner Join ReceiptQualifier B On A.ReceiptId=B.ReceiptId " +
                        " And A.PaymentSchId=B.PaymentSchId And A.OtherCostId=B.OtherCostId And A.ReceiptTypeId=B.ReceiptTypeId  " +
                        " Inner Join ReceiptRegister C On C.ReceiptId=A.ReceiptId  Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp D On D.QualifierId=B.QualifierId And QualType='B'  " +
                        " Where C.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "'  AND A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1 AND C.CostCentreId=" + argCCId + " " +
                        " Group By A.ReceiptTypeId,A.OtherCostId,A.FlatId,B.QualifierId, " +
                        " ('PaidGross @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ',B.NetPer )A " +
                        " Group By FlatId,QualifierId,QualifierName,NetPer " +
                        " UNION ALL " +
                        " Select DISTINCT A.FlatId,B.QualifierId,(D.QualifierName + ' @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ' AS QualifierName,B.NetPer,Sum(B.Amount)Amount From ReceiptShTrans A " +
                        " Inner Join ReceiptQualifier B On A.ReceiptId=B.ReceiptId And A.PaymentSchId=B.PaymentSchId And A.OtherCostId=B.OtherCostId And A.ReceiptTypeId=B.ReceiptTypeId " +
                        " Inner Join ReceiptRegister C On C.ReceiptId=A.ReceiptId " +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp D On D.QualifierId=B.QualifierId And QualType='B' " +
                        " Where C.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' " +
                        " AND A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1 AND C.CostCentreId=" + argCCId + " " +
                        " Group By A.FlatId,B.QualifierId,(D.QualifierName + ' @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ',B.NetPer" +
                        " UNION ALL " +
                        " SELECT DISTINCT B.FlatId,0 QualifierId,'PaidNetAmount' QualifierName,0 NetPer,isnull(SUM(PaidNetAmount),0) Amount FROM ReceiptShTrans A  " +
                        " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId  " +
                        " WHERE B.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PaidNetAmount<>0 AND ReceiptTypeId<>1 AND B.CostCentreId=" + argCCId + " " +
                        " GROUP BY B.FlatId " +
                        " UNION ALL  " +
                        " Select DISTINCT P.FlatId,0 QualifierId,'Balance' QualifierName,0 NetPer,SUM(P.Amount)-IsNull((SELECT SUM(PaidGrossAmount)rd FROM ReceiptShTrans A  " +
                        " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                        " WHERE PaidNetAmount<>0 AND ReceiptTypeId<>1 And A.FlatId=P.FlatId AND B.CostCentreId=" + argCCId + " " +
                        " GROUP BY A.FlatId),0)Amount From dbo.PaymentScheduleFlat P Where P.SchType<>'A' GROUP BY P.FlatId ";
                //" SELECT FlatId,0 QualifierId,'Balance' QualifierName,0 NetPer,SUM(PaidNetAmount)-SUM(PaidGrossAmount) Amount FROM ReceiptShTrans A   " +
                //" INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId WHERE B.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PaidNetAmount<>0 AND ReceiptTypeId<>1  " +
                //" GROUP BY FlatId";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                DataTable dtQ = new DataTable();
                dtQ.Load(dr, LoadOption.OverwriteChanges);
                dr.Close();
                cmd.Dispose();

                string sQualName = "";
                if (dtQ.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sQualName = dt.Rows[i]["QualifierName"].ToString();

                        DataColumn col1 = new DataColumn(sQualName) { DataType = typeof(decimal), DefaultValue = 0 };
                        dtT.Columns.Add(col1);

                        DataTable dtRecv = new DataTable();
                        DataView dv = new DataView(dtQ) { RowFilter = String.Format("QualifierName='{0}'", sQualName) };
                        dtRecv = dv.ToTable();

                        for (int j = 0; j < dtRecv.Rows.Count; j++)
                        {
                            int iFlatId = Convert.ToInt32(dtRecv.Rows[j]["FlatId"]);
                            decimal dAmt = Convert.ToDecimal(dtRecv.Rows[j]["Amount"]);

                            DataRow[] drT = dtT.Select(String.Format("FlatId={0}", iFlatId));
                            if (drT.Length > 0)
                            {
                                drT[0][sQualName] = dAmt;
                            }
                        }

                        dtRecv.Dispose();
                        dv.Dispose();
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
            return dtT;

        }

        public static DataTable GetBWAECostSheet(int argCCId, bool argTypewise, DateTime argFromDate,DateTime argToDate)
        {
            DataTable dt = null;
            SqlCommand cmd; SqlDataReader dr;
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            DataTable dtT = new DataTable();
            int iFlatId = 0;
            DataView dv; decimal dAmt = 0; DataRow[] drT;

            try
            {
                sSql = "Select A.FlatId,L.LeadName,A.FlatNo,D.TypeName, " +
                        " E.FinaliseDate BookingDate,B.BlockName,C.LevelName, " +
                        " ISNULL(LPC.CampaignName,'') Campaign, ISNULL(LU.UserName,'') Executive," +
                        " A.Area,A.Rate,A.USLand UDS,A.LandRate LandValue,A.BaseAmt From dbo.FlatDetails A " +
                        " Inner Join dbo.BlockMaster B On A.BlockId=B.BlockId " +
                        " Inner Join dbo.LevelMaster C On C.LevelId=A.LevelId " +
                        " Inner Join dbo.FlatType D On D.FlatTypeId=A.FlatTypeId " +
                        " Inner Join dbo.LeadRegister L On L.LeadId=A.LeadId " +
                        " left Join dbo.LeadProjectInfo LP On L.LeadId=LP.LeadId  AND LP.CampaignId<>0 and LP.CampaignId IS NOT NULL" +
                        " left Join dbo.CampaignDetails LPC On LP.CampaignId=LPC.CampaignId AND A.CostCentreId=LPC.CCId " +
                        " Inner Join dbo.BuyerDetail E On E.FlatId=A.FlatId And A.LeadId=E.LeadId And E.Status='S' " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users LU On E.ExecutiveId=LU.UserId " +
                        " Where A.CostCentreId=" + argCCId + " And A.Status='S' "+ 
                        " Order By B.SortOrder,C.SortOrder,dbo.Val(FlatNo)";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr, LoadOption.OverwriteChanges);
                dt.Dispose();
                dr.Close();
                dtT = dt;

                //Add OtherCost Column with Amount
                sSql = "Select OtherCostName From dbo.OtherCostMaster A " +
                         " Inner Join dbo.CCOtherCost B On A.OtherCostId=B.OtherCostId Where B.CostcentreId=" + argCCId + " And A.OtherCostId<>6 Order By A.OtherCostId";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr, LoadOption.OverwriteChanges);
                dr.Close();
                dt.Dispose();


                sSql = "Select B.FlatId,A.OtherCostName,IsNull(B.Amount,0)Amount From dbo.OtherCostMaster A " +
                         " Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId Where A.OtherCostId<>6 Order By A.OtherCostId";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                DataTable dtA = new DataTable();
                dtA.Load(dr, LoadOption.OverwriteChanges);
                dr.Close();
                dtA.Dispose();

                string sOCName = "";
                if (dtA.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sOCName = dt.Rows[i]["OtherCostName"].ToString();

                        DataColumn col1 = new DataColumn(sOCName) { DataType = typeof(decimal), DefaultValue = 0 };
                        dtT.Columns.Add(col1);


                        DataTable dtRecv = new DataTable();
                        dv = new DataView(dtA) { RowFilter = String.Format("OtherCostName='{0}' ", sOCName) };
                        dtRecv = dv.ToTable();

                        for (int j = 0; j < dtRecv.Rows.Count; j++)
                        {
                            iFlatId = Convert.ToInt32(dtRecv.Rows[j]["FlatId"]);
                            dAmt = Convert.ToDecimal(dtRecv.Rows[j]["Amount"]);

                            drT = dtT.Select(String.Format("FlatId={0}", iFlatId));

                            if (drT.Length > 0)
                            {
                                drT[0][sOCName] = dAmt;
                            }
                        }

                        dtRecv.Dispose();
                        dv.Dispose();
                    }
                }

                //Total Column
                dtT.Columns.Add("Total", typeof(decimal));

                //j=9 From BaseAmount to OC, adding total
                decimal iTot = 0;
                if (dtT.Rows.Count > 0)
                {
                    for (int k = 0; k < dtT.Rows.Count; k++)
                    {
                        decimal iTTot = 0;
                        for (int j = 9; j < dtT.Columns.Count - 2; j++)
                        {
                            iTot = Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[k][j], CommFun.datatypes.vartypenumeric));
                            iTTot = iTTot + iTot;
                        }
                        dtT.Rows[k]["Total"] = iTTot.ToString().Trim();
                    }
                }

                //Add Separate Column for MaintenanceCharges
                dtT.Columns.Add("MaintenanceCharges", typeof(decimal));


                sSql = "Select B.FlatId,A.OtherCostName,IsNull(B.Amount,0)Amount From dbo.OtherCostMaster A " +
                        " Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId Where A.OtherCostId=6";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr, LoadOption.OverwriteChanges);
                dr.Close();
                dt.Dispose();

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    iFlatId = Convert.ToInt32(dt.Rows[j]["FlatId"]);
                    dAmt = Convert.ToDecimal(dt.Rows[j]["Amount"]);

                    drT = dtT.Select(String.Format("FlatId={0}", iFlatId));

                    if (drT.Length > 0)
                    {
                        drT[0]["MaintenanceCharges"] = dAmt;
                    }
                }

                //GrandTotal
                dtT.Columns.Add("GrandTotal", typeof(decimal));

                //calculating grand total except last 3 rows
                if (dtT.Rows.Count > 0)
                {
                    decimal iTot1 = 0;
                    for (int k = 0; k < dtT.Rows.Count; k++)
                    {
                        decimal iTTot = 0;
                        for (int j = dtT.Columns.Count - 3; j < dtT.Columns.Count - 1; j++)
                        {
                            iTot1 = Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[k][j], CommFun.datatypes.vartypenumeric));
                            iTTot = iTTot + iTot1;
                        }
                        dtT.Rows[k]["GrandTotal"] = iTTot.ToString().Trim();
                    }
                }

                //Add Qualifer Columns
                //if (argTypewise == true)
                //{
                sSql = "Select 'AmountReceivedLand' QualifierName,0 Amount   " +
                        " UNION ALL " +
                        "Select 'PaidGrossAmount' QualifierName,0 Amount   " +
                        " UNION ALL " +
                        " Select('PaidGross @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ' AS QualifierName,  " +
                        " 0 Amount From ReceiptShTrans A  Inner Join ReceiptQualifier B On A.ReceiptId=B.ReceiptId  And A.PaymentSchId=B.PaymentSchId " +
                        " And A.OtherCostId=B.OtherCostId And A.ReceiptTypeId=B.ReceiptTypeId  Inner Join ReceiptRegister C On C.ReceiptId=A.ReceiptId  " +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp D On D.QualifierId=B.QualifierId  And QualType='B'  "+
                        " Where C.ReceiptDate Between '" + argFromDate.ToString("dd-MMM-yyyy") + "' And '" + argToDate.ToString("dd-MMM-yyyy") + "' " +
                        " AND A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1  Group By ('PaidGross @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ' " +
                        " UNION ALL " +
                    //" Select (QualifierName + ' @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),Net), 101) )+ ' % ' AS QualifierName,Net NetPer  From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp Where QualType='B'" +
                        " Select(QualifierName + ' @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ' AS QualifierName, " +
                        " Sum(B.Amount)Amount From ReceiptShTrans A  Inner Join ReceiptQualifier B On A.ReceiptId=B.ReceiptId " +
                        " And A.PaymentSchId=B.PaymentSchId And A.OtherCostId=B.OtherCostId And A.ReceiptTypeId=B.ReceiptTypeId " +
                        " Inner Join ReceiptRegister C On C.ReceiptId=A.ReceiptId  Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp D On D.QualifierId=B.QualifierId " +
                        " And QualType='B'  Where C.ReceiptDate Between '" + argFromDate.ToString("dd-MMM-yyyy") + "' And '" + argToDate.ToString("dd-MMM-yyyy") + "' AND A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1 " +
                        " Group By (QualifierName + ' @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ' " +
                        " UNION ALL" +
                        " Select 'PaidNetAmount' QualifierName,0 Amount   " +
                        " UNION ALL " +
                        " Select 'Balance' QualifierName,0 Amount ";
                //}
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr, LoadOption.OverwriteChanges);
                dr.Close();
                dt.Dispose();


                //Add Qualifier Amounts

                sSql = "SELECT A.FlatId,0 QualifierId,'AmountReceivedLand' QualifierName,0 NetPer,isnull(SUM(PaidGrossAmount),0) Amount FROM ReceiptShTrans A   " +
                        " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId WHERE B.ReceiptDate Between '" + argFromDate.ToString("dd-MMM-yyyy") + "' And '" + argToDate.ToString("dd-MMM-yyyy") + "' AND PaidNetAmount<>0 AND ReceiptTypeId=2  " +
                        " GROUP BY A.FlatId " +
                        " UNION ALL " +
                        " SELECT A.FlatId,0 QualifierId,'PaidGrossAmount' QualifierName,0 NetPer,isnull(SUM(PaidGrossAmount),0) Amount FROM ReceiptShTrans A  " +
                        " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId  " +
                        " WHERE B.ReceiptDate Between '" + argFromDate.ToString("dd-MMM-yyyy") + "' And '" + argToDate.ToString("dd-MMM-yyyy") + "' AND PaidNetAmount<>0 AND ReceiptTypeId<>1 " +
                        " GROUP BY A.FlatId" +
                        " UNION ALL" +
                        " Select FlatId,QualifierId,QualifierName,NetPer,SUM(PaidGross) Amount From ( " +
                        " Select A.ReceiptTypeId,A.OtherCostId,A.FlatId,B.QualifierId,('PaidGross @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ' AS QualifierName,B.NetPer, " +
                        " Sum(PaidGrossAmount) PaidGross,Sum(B.Amount)Amount From ReceiptShTrans A  Inner Join ReceiptQualifier B On A.ReceiptId=B.ReceiptId " +
                        " And A.PaymentSchId=B.PaymentSchId And A.OtherCostId=B.OtherCostId And A.ReceiptTypeId=B.ReceiptTypeId  " +
                        " Inner Join ReceiptRegister C On C.ReceiptId=A.ReceiptId  Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp D On D.QualifierId=B.QualifierId And QualType='B'  " +
                        " Where C.ReceiptDate Between '" + argFromDate.ToString("dd-MMM-yyyy") + "' And '" + argToDate.ToString("dd-MMM-yyyy") + "' AND A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1  " +
                        " Group By A.ReceiptTypeId,A.OtherCostId,A.FlatId,B.QualifierId, " +
                        " ('PaidGross @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ',B.NetPer )A " +
                        " Group By FlatId,QualifierId,QualifierName,NetPer " +
                        " UNION ALL " +
                        " Select A.FlatId,B.QualifierId,(D.QualifierName + ' @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ' AS QualifierName,B.NetPer,Sum(B.Amount)Amount From ReceiptShTrans A " +
                        " Inner Join ReceiptQualifier B On A.ReceiptId=B.ReceiptId And A.PaymentSchId=B.PaymentSchId And A.OtherCostId=B.OtherCostId And A.ReceiptTypeId=B.ReceiptTypeId " +
                        " Inner Join ReceiptRegister C On C.ReceiptId=A.ReceiptId " +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp D On D.QualifierId=B.QualifierId And QualType='B' " +
                        " Where C.ReceiptDate Between '" + argFromDate.ToString("dd-MMM-yyyy") + "' And '" + argToDate.ToString("dd-MMM-yyyy") + "' " +
                        " AND A.PaidNetAmount<>0 AND A.ReceiptTypeId<>1 " +
                        " Group By A.FlatId,B.QualifierId,(D.QualifierName + ' @ ' + CONVERT(varchar,CONVERT(DECIMAL(18,2),B.NetPer), 101) )+ ' % ',B.NetPer" +
                        " UNION ALL " +
                        " SELECT A.FlatId,0 QualifierId,'PaidNetAmount' QualifierName,0 NetPer,isnull(SUM(PaidNetAmount),0) Amount FROM ReceiptShTrans A  " +
                        " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId  " +
                        " WHERE B.ReceiptDate Between '" + argFromDate.ToString("dd-MMM-yyyy") + "' And '" + argToDate.ToString("dd-MMM-yyyy") + "' AND PaidNetAmount<>0 AND ReceiptTypeId<>1 " +
                        " GROUP BY A.FlatId " +
                        " UNION ALL  " +
                        " Select P.FlatId,0 QualifierId,'Balance' QualifierName,0 NetPer,SUM(P.Amount)-IsNull((SELECT SUM(PaidGrossAmount)rd FROM ReceiptShTrans A  " +
                        " INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId " +
                        " WHERE PaidNetAmount<>0 AND ReceiptTypeId<>1 And A.FlatId=P.FlatId " +
                        " GROUP BY A.FlatId),0)Amount From dbo.PaymentScheduleFlat P Where P.SchType<>'A' GROUP BY P.FlatId ";
                //" SELECT FlatId,0 QualifierId,'Balance' QualifierName,0 NetPer,SUM(PaidNetAmount)-SUM(PaidGrossAmount) Amount FROM ReceiptShTrans A   " +
                //" INNER JOIN ReceiptRegister B ON A.ReceiptId=B.ReceiptId WHERE B.ReceiptDate<='" + argDate.ToString("dd-MMM-yyyy") + "' AND PaidNetAmount<>0 AND ReceiptTypeId<>1  " +
                //" GROUP BY FlatId";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                DataTable dtQ = new DataTable();
                dtQ.Load(dr, LoadOption.OverwriteChanges);
                dr.Close();
                dtQ.Dispose();

                string sQualName = "";
                if (dtQ.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sQualName = dt.Rows[i]["QualifierName"].ToString();

                        DataColumn col1 = new DataColumn(sQualName) { DataType = typeof(decimal), DefaultValue = 0 };
                        dtT.Columns.Add(col1);


                        DataTable dtRecv = new DataTable();
                        dv = new DataView(dtQ) { RowFilter = String.Format("QualifierName='{0}' ", sQualName) };
                        dtRecv = dv.ToTable();

                        for (int j = 0; j < dtRecv.Rows.Count; j++)
                        {
                            iFlatId = Convert.ToInt32(dtRecv.Rows[j]["FlatId"]);
                            //dPaidAmt = Convert.ToDecimal(dtRecv.Rows[j]["PaidGross"]);
                            dAmt = Convert.ToDecimal(dtRecv.Rows[j]["Amount"]);

                            drT = dtT.Select(String.Format("FlatId={0}", iFlatId));

                            if (drT.Length > 0)
                            {
                                drT[0][sQualName] = dAmt;
                            }
                        }

                        dtRecv.Dispose();
                        dv.Dispose();
                    }
                }


                //Add Qualifier


            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtT;

        }

        public static DataTable BuyerGetCostSheet(int argCCId, bool argTypewise,int argFlatId)
        {
            DataTable dt = null;
            SqlCommand cmd; SqlDataReader dr; SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            string sSql = ""; int iFlatId = 0; int iFlatTypeId = 0;
            decimal dGrossAmt = 0, dLandCost = 0;
            DataTable dtT = new DataTable();
            DataView dv; decimal dAmt = 0; DataRow[] drT;
            DataTable dtRet = new DataTable();
            dtRet.Columns.Add("Description", typeof(string));
            dtRet.Columns.Add("Actual", typeof(string));
            dtRet.Columns.Add("Discount", typeof(string));
            dtRet.Columns.Add("ActualValue", typeof(decimal));
            dtRet.Columns.Add("DiscountValue", typeof(decimal));
            dtRet.Columns.Add("Type", typeof(bool));

            try
            {
                sSql = "Select A.FlatId,A.OtherCostAmt OCAmt,L.LeadName,B.BlockName,C.LevelName,D.TypeName,"+
                        " A.FlatNo,Case When A.Status='S' Then 'Sold' When A.Status='B'  Then 'Block' Else 'UnSold' End Status, " +
                        " A.Area,A.Rate [Rate(Basic+Floorise)],A.LandRate LandCost,A.BaseAmt [BaseAmt(Basic+Floorise)] From dbo.FlatDetails A " +
                        " Inner Join dbo.BlockMaster B On A.BlockId=B.BlockId " +
                        " Inner Join dbo.LevelMaster C On C.LevelId=A.LevelId " +
                        " Left Join dbo.LeadRegister L On L.LeadId=A.LeadId " +
                        " Inner Join dbo.FlatType D On D.FlatTypeId=A.FlatTypeId "+
                        " Where A.CostCentreId=" + argCCId + " And A.FlatId=" + argFlatId + " " +
                        " Order By B.SortOrder,C.SortOrder,dbo.Val(A.FlatNo)";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr, LoadOption.OverwriteChanges);
                dt.Dispose();
                dr.Close();
                dtT = dt;

                sSql = "Select A.FlatTypeId FlatId,D.OtherCostAmt OCAmt,L.LeadName,B.BlockName,C.LevelName,D.TypeName,A.FlatNo," +
                        " Case When A.Status='S' Then 'Sold' When A.Status='B'  Then 'Block' Else 'UnSold' End Status, " +
                        " D.Area,Case When F.Rate IS NULL Then A.Rate Else F.Rate End [Rate(Basic+Floorise)],A.LandRate LandCost," +
                        " D.BaseAmt [BaseAmt(Basic+Floorise)] From dbo.FlatDetails A " +
                        " Inner Join dbo.BlockMaster B On A.BlockId=B.BlockId " +
                        " Inner Join dbo.LevelMaster C On C.LevelId=A.LevelId " +
                        " Left Join dbo.LeadRegister L On L.LeadId=A.LeadId " +
                        " Inner Join dbo.FlatType D On D.FlatTypeId=A.FlatTypeId " +
                        " Left Join dbo.FloorRate F On F.FlatTypeId=A.FlatTypeId And A.LevelId=F.LevelId " +
                        " Where A.CostCentreId=" + argCCId + " And A.FlatId=" + argFlatId + " " +
                        " Order By B.SortOrder,C.SortOrder,dbo.Val(A.FlatNo)";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr, LoadOption.OverwriteChanges);
                dt.Dispose();
                dr.Close();
                DataRow drM;
                if (dt.Rows.Count > 0)
                {
                    iFlatTypeId = Convert.ToInt32(dt.Rows[0]["FlatId"]);
                    dGrossAmt = Convert.ToDecimal(dt.Rows[0]["BaseAmt(Basic+Floorise)"]) + Convert.ToDecimal(dt.Rows[0]["OCAmt"]);
                    dLandCost = Convert.ToDecimal(dt.Rows[0]["LandCost"]);

                    drM = dtT.NewRow();
                    drM["FlatId"] = iFlatTypeId;
                    drM["OCAmt"] = Convert.ToDecimal(dt.Rows[0]["OCAmt"]);
                    drM["LeadName"] = dt.Rows[0]["LeadName"].ToString();
                    drM["BlockName"] = dt.Rows[0]["BlockName"].ToString();
                    drM["LevelName"] = dt.Rows[0]["LevelName"].ToString();
                    drM["TypeName"] = dt.Rows[0]["TypeName"].ToString();
                    drM["FlatNo"] = dt.Rows[0]["FlatNo"].ToString();
                    drM["Status"] = dt.Rows[0]["Status"].ToString();
                    drM["Area"] = Convert.ToDecimal(dt.Rows[0]["Area"]);
                    drM["Rate(Basic+Floorise)"] = Convert.ToDecimal(dt.Rows[0]["Rate(Basic+Floorise)"]);
                    drM["LandCost"] = Convert.ToDecimal(dt.Rows[0]["LandCost"]);
                    drM["BaseAmt(Basic+Floorise)"] = Convert.ToDecimal(dt.Rows[0]["BaseAmt(Basic+Floorise)"]);

                    dtT.Rows.Add(drM);
                }
                //dtT = dt;

                sSql = "Select OtherCostName From dbo.OtherCostMaster A Inner Join dbo.CCOtherCost B On A.OtherCostId=B.OtherCostId Where B.CostcentreId=" + argCCId + "" +
                        " Union All " +
                        " Select QualifierName OtherCostName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp Where QualType='B'";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr, LoadOption.OverwriteChanges);
                dr.Close();
                dt.Dispose();

                if (argTypewise == true)
                {
                    sSql = "Select B.FlatId,A.OtherCostName,IsNull(CONVERT(DECIMAL(18,3),B.Amount),0)Amount From dbo.OtherCostMaster A "+
                            " Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId Where B.FlatId=" + argFlatId + " " +
                            " Union All " +
                            " Select FlatId,C.QualifierName OtherCostName,Sum(A.Amount)Amount From dbo.FlatReceiptQualifier A  " +
                            " Inner Join dbo.FlatReceiptType B On A.SchId=B.SchId " +
                            " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On A.QualifierId=C.QualifierId  " +
                            " Where QualType='B' And FlatId=" + argFlatId + " Group By FlatId,C.QualifierName";
                }
                else
                {
                    sSql = "Select B.FlatId,A.OtherCostName,IsNull(B.Amount,0)Amount From dbo.OtherCostMaster A Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId Where B.FlatId=" + argFlatId + " " +
                        " Union All " +
                        " Select B.FlatId,A.QualifierName OtherCostName,Amount from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A " +
                        " Inner Join dbo.FlatTax B On A.QualifierId=B.QualifierId " +
                        " Where QualType='B' And B.FlatId=" + argFlatId + " ";
                }
                DataTable dtA = new DataTable();
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dtA);
                dtA.Dispose();

                foreach (DataRow row in dtA.Rows)
                {
                    if (row["OtherCostName"].ToString() == "Work Service Tax")
                    {
                        row["OtherCostName"] = "Service Tax For Project";
                    }
                    else if (row["OtherCostName"].ToString() == "Service Tax")
                    {
                        row["OtherCostName"] = "Service Tax For Maintainance";
                    }
                }
                


                if (argTypewise == true)
                {
                    sSql = "Select " + iFlatTypeId + " FlatId,A.OtherCostId QualifierId,A.OCTypeId,0 NetPer,A.OtherCostName,IsNull(CONVERT(DECIMAL(18,3),C.Amount),0)Amount," +
                            " IsNull(CONVERT(DECIMAL(18,3),B.Amount),0)FlatAmount From dbo.OtherCostMaster A  " +
                            " Inner Join dbo.FlatOtherCost B On B.OtherCostId=A.OtherCostId " +
                            " Inner Join dbo.FlatTypeOtherCost C On A.OtherCostId=C.OtherCostId Where C.FlatTypeId=" + iFlatTypeId + "  " +
                            " Union All  " +
                            " Select " + iFlatTypeId + " FlatId,A.QualifierId,0 OCTypeId,A.NetPer,C.QualifierName OtherCostName,Sum(CONVERT(DECIMAL(18,3),A.Amount))Amount,0 From dbo.FlatReceiptQualifier A   " +
                            " Inner Join dbo.FlatReceiptType B On A.SchId=B.SchId  Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On A.QualifierId=C.QualifierId  " +
                            " Where QualType='B' And FlatId=" + argFlatId + " Group By FlatId,A.QualifierId,A.NetPer,C.QualifierName";
                }
                else
                {
                    sSql = "Select " + iFlatTypeId + " FlatId,A.OtherCostName,IsNull(CONVERT(DECIMAL(18,3),C.Amount),0)Amount From dbo.OtherCostMaster A " +
                        " Inner Join dbo.FlatTypeOtherCost C On A.OtherCostId=C.OtherCostId Where C.FlatTypeId=" + iFlatTypeId + " " +
                        " Union All " +
                        " Select " + iFlatTypeId + " FlatId,A.QualifierName OtherCostName,C.Amount From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A " +
                        " Inner Join dbo.FlatTypeTax C On A.QualifierId=C.QualifierId Where QualType='B' And C.FlatTypeId=" + iFlatTypeId + " ";
                }
                DataTable dtA1 = new DataTable();
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(dtA1);
                dtA1.Dispose();

                foreach (DataRow row in dtA1.Rows)
                {
                    if (row["OtherCostName"].ToString() == "Work Service Tax")
                    {
                        row["OtherCostName"] = "Service Tax For Project";
                    }
                    else if (row["OtherCostName"].ToString() == "Service Tax")
                    {
                        row["OtherCostName"] = "Service Tax For Maintainance";
                    }
                }

                if (argTypewise == true)
                {
                    decimal dMainFee = 0, dSertax = 0;
                    for (int p = 0; p < dtA1.Rows.Count; p++)
                    {
                        if (Convert.ToInt32(dtA1.Rows[p]["OCTypeId"]) == 3) dMainFee = Convert.ToDecimal(dtA1.Rows[p]["Amount"]);
                        else if (Convert.ToInt32(dtA1.Rows[p]["QualifierId"]) == 9 && Convert.ToDecimal(dtA1.Rows[p]["FlatAmount"])==0) 
                        {
                            dtA1.Rows[p]["Amount"] = 0;
                        }
                        else if (Convert.ToInt32(dtA1.Rows[p]["QualifierId"]) == 36)
                        {
                            dSertax = decimal.Round((dGrossAmt - (dMainFee + dLandCost)) * Convert.ToDecimal(dtA1.Rows[p]["NetPer"]) / 100, 3);
                            dtA1.Rows[p]["Amount"] = dSertax;
                        }
                    }
                }

                DataRow drM1;
                for (int y = 0; y < dtA1.Rows.Count;y++ )
                {
                    drM1 = dtA.NewRow();
                    drM1["FlatId"] = iFlatTypeId;
                    drM1["OtherCostName"] = dtA1.Rows[y]["OtherCostName"].ToString();
                    drM1["Amount"] = Convert.ToDecimal(dtA1.Rows[y]["Amount"]);

                    dtA.Rows.Add(drM1);
                }

                string sOCName = "";
                if (dtA.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sOCName = dt.Rows[i]["OtherCostName"].ToString();
                        if (sOCName == "Work Service Tax") { sOCName = "Service Tax For Project"; }
                        else if (sOCName == "Service Tax") { sOCName = "Service Tax For Maintainance"; }

                        DataColumn col1 = new DataColumn(sOCName) { DataType = typeof(decimal), DefaultValue = 0.000 };
                        dtT.Columns.Add(col1);


                        DataTable dtRecv = new DataTable();
                        dv = new DataView(dtA) { RowFilter = String.Format("OtherCostName='{0}' ", sOCName) };
                        dtRecv = dv.ToTable();

                        for (int j = 0; j < dtRecv.Rows.Count; j++)
                        {
                            iFlatId = Convert.ToInt32(dtRecv.Rows[j]["FlatId"]);
                            dAmt = Convert.ToDecimal(dtRecv.Rows[j]["Amount"]);

                            drT = dtT.Select(String.Format("FlatId={0}", iFlatId));

                            if (drT.Length > 0)
                            {
                                drT[0][sOCName] = decimal.Round(Convert.ToDecimal(dAmt),3);
                            }
                        }

                        dtRecv.Dispose();
                        dv.Dispose();
                    }
                }

                dtT.Columns.Add("Total", typeof(decimal));

                decimal iTot = 0;
                if (dtT.Rows.Count > 0)
                {
                    for (int k = 0; k < dtT.Rows.Count; k++)
                    {
                        decimal iTTot = 0;
                        for (int j = 11; j < dtT.Columns.Count - 1; j++)
                        {
                            iTot = Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[k][j], CommFun.datatypes.vartypenumeric));
                            iTTot = iTTot + iTot;
                        }
                        dtT.Rows[k]["Total"] = iTTot.ToString().Trim();
                    }
                }

                DataRow dr1;
                if (dtT.Rows.Count > 1)
                {
                    for (int x = 2; x < dtT.Columns.Count; x++)
                    {
                        dr1 = dtRet.NewRow();
                        string s = dtT.Columns[x].Caption.ToString();
                        dr1["Description"] = s;
                        dr1["Actual"] = dtT.Rows[1][x].ToString();
                        dr1["Discount"] = dtT.Rows[0][x].ToString();
                        if (x > 8)
                        {
                            dr1["ActualValue"] = Convert.ToDecimal(dtT.Rows[1][x]);
                            dr1["DiscountValue"] = Convert.ToDecimal(dtT.Rows[0][x]);
                            dr1["Type"] = true;
                        }
                        else
                        {
                            dr1["ActualValue"] = 0.000;
                            dr1["DiscountValue"] = 0.000;
                            dr1["Type"] = false;
                        }

                        dtRet.Rows.Add(dr1);
                    }
                }
                
                //CommFun.FormatNum1(dt.Rows[0]["Area"].ToString(), BsfGlobal.g_iCurrencyDigit);
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtRet;

        }

        public static DataTable GetBuyerTermSheetUnit(int argCCId, bool argTypewise, int argFlatId)
        {
            DataTable dt = null;
            SqlCommand cmd; SqlDataReader dr; 
            BsfGlobal.OpenCRMDB();
            string sSql = ""; 
            DataTable dtRet = new DataTable();
            dtRet.Columns.Add("Description", typeof(string));
            dtRet.Columns.Add("Actual", typeof(string));
            dtRet.Columns.Add("ActualValue", typeof(decimal));
            dtRet.Columns.Add("Type", typeof(bool));

            try
            {
                sSql = "Select L.LeadName,B.BlockName,C.LevelName,D.TypeName, A.FlatNo,O.CostCentreName,CM.CompanyName,A.Area,A.Rate BasicRate,A.BaseAmt TotalApCost," +
                        " PLC=IsNull((Select Amount From dbo.OtherCostMaster A Inner Join dbo.OCType B On A.OCTypeId=B.OCTypeId " +
                        " Left Join dbo.FlatOtherCost F On F.OtherCostId=A.OtherCostId Where A.OCTypeId=10 And F.FlatId=" + argFlatId + "),0)," +
                        " A.USLand UDS,A.USLandAmt UDSValue,(A.BaseAmt-A.USLandAmt)LessUDS," +
                        " ServiceTax=ISNULL((Select SUM(A.Amount) from FlatReceiptQualifier A INNER JOIN FlatReceiptType B ON A.SchId=B.SchId" +
                        " Where B.FlatId=" + argFlatId + " AND B.SchType<>'A' And B.SchType<>'O'),0) From dbo.FlatDetails A  " +
                        " Inner Join dbo.BlockMaster B On A.BlockId=B.BlockId  Inner Join dbo.LevelMaster C On C.LevelId=A.LevelId  " +
                        " Left Join dbo.LeadRegister L On L.LeadId=A.LeadId  Inner Join dbo.FlatType D On D.FlatTypeId=A.FlatTypeId  " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O On O.CostCentreId=A.CostCentreId " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CostCentre CC On CC.CostCentreId=O.FACostCentreId " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CompanyMaster CM On CM.CompanyId=CC.CompanyId" +
                        " Where A.CostCentreId=" + argCCId + " And A.FlatId=" + argFlatId + "  Order By B.SortOrder,C.SortOrder,dbo.Val(A.FlatNo)";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr, LoadOption.OverwriteChanges);
                dt.Dispose();
                dr.Close();

                DataRow dr1;
                if (dt.Rows.Count > 0)
                {
                    for (int x = 0; x < dt.Columns.Count; x++)
                    {
                        dr1 = dtRet.NewRow();
                        string s = dt.Columns[x].Caption.ToString();
                        dr1["Description"] = s;
                        dr1["Actual"] = dt.Rows[0][x].ToString();
                        if (x > 6)
                        {
                            dr1["ActualValue"] = Convert.ToDecimal(dt.Rows[0][x]);
                            dr1["Type"] = true;
                        }
                        else
                        {
                            dr1["ActualValue"] = 0.000;
                            dr1["Type"] = false;
                        }

                        dtRet.Rows.Add(dr1);
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
            return dtRet;

        }

        public static DataTable GetBuyerTermSheetOC(int argCCId, bool argTypewise, int argFlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            string sSql = "";

            try
            {
                sSql = "Select X.SortOrder,X.OCTypeId,X.OtherCostId,[Description],DuePeriod,Amount,Tax,Total From( " +
                        " Select A.SortOrder,A.OCTypeId,A.OtherCostId,A.OtherCostName [Description],IsNull(C.Description,'')DuePeriod," +
                        " Sum(IsNull(D.Amount,0))Amount,Sum(IsNull(D.NetAmount-D.Amount,0))Tax,Sum(IsNull(D.NetAmount,0)) Total From dbo.OtherCostMaster A  " +
                        " Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId  Inner Join dbo.FlatReceiptType D On B.FlatId=D.FlatId And D.OtherCostId=B.OtherCostId  " +
                        " Left Join dbo.PaymentScheduleFlat C On B.FlatId=C.FlatId And C.PaymentSchId=D.PaymentSchId  And C.OtherCostId=D.OtherCostId  " +
                        " Where B.FlatId=" + argFlatId + " Group By A.SortOrder,A.OCTypeId,A.OtherCostId,A.OtherCostName,C.Description " +
                        " UNION ALL " +
                        " Select A.SortOrder,A.OCTypeId,A.OtherCostId,A.OtherCostName [Description],''DuePeriod,0 Amount,0 Tax,B.Amount Total From dbo.OtherCostMaster A  " +
                        " Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId Where B.FlatId=" + argFlatId + " And A.OtherCostId=2" +
                        //" AND A.OtherCostId NOT IN(Select OtherCostId from OXGross Where CostCentreId=" + argCCId + ")" +
                        " )X " +
                        " Inner Join dbo.OtherCostMaster Y On X.OtherCostId=Y.OtherCostId Order By X.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dt.Rows[i]["OCTypeId"]) == 4) { dt.Rows[i]["DuePeriod"] = "To be paid on 45th day from the due date of 2nd installment"; }
                    else if (Convert.ToInt32(dt.Rows[i]["OCTypeId"]) == 5) { dt.Rows[i]["DuePeriod"] = "To be  paid after completion of Brick work"; }
                    else if (Convert.ToInt32(dt.Rows[i]["OCTypeId"]) == 6) { dt.Rows[i]["DuePeriod"] = "To be paid after completion of Plastering"; }
                    else if (Convert.ToInt32(dt.Rows[i]["OCTypeId"]) == 7) { dt.Rows[i]["DuePeriod"] = "To be paid before Registration"; }
                    else if (Convert.ToInt32(dt.Rows[i]["OCTypeId"]) == 1) { dt.Rows[i]["DuePeriod"] = ""; }
                    else if (Convert.ToInt32(dt.Rows[i]["OCTypeId"]) == 8) { dt.Rows[i]["DuePeriod"] = "To be paid before HandingOver"; }
                    else if (Convert.ToInt32(dt.Rows[i]["OCTypeId"]) == 3) { dt.Rows[i]["DuePeriod"] = "To be paid before HandingOver"; }
                    
                    DataView dv = new DataView(dt);
                    dv.RowFilter = "OtherCostId=2";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        decimal s = decimal.Round(Convert.ToDecimal(dv.ToTable().Rows[0]["Total"]), 0); //Convert.ToDecimal(dt.Compute("SUM(Amount)", dv.RowFilter));

                        if (Convert.ToInt32(dt.Rows[i]["OCTypeId"]) == 2)
                        {
                            dt.Rows[i]["Description"] = "Others If any (Rs." + s + "/-)";
                            dt.Rows[i]["Total"] = 0;
                        }
                    }
                }
                    dt.Dispose();
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

        public static DataTable GetBuyerTermSheetPayment(int argCCId, bool argTypewise, int argFlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            BsfGlobal.OpenCRMDB();
            string sSql = "";

            try
            {
                sSql = "Select '' Installment,A.Description,A.SchPercent Percentage,A.SortOrder,Sum(B.Amount)Amount,SUM(B.NetAmount-B.Amount)Tax,SUM(B.NetAmount-A.Advance)Total " +
                        " From dbo.PaymentScheduleFlat A Inner Join FlatReceiptType B On A.PaymentSchId=B.PaymentSchId And B.SchType<>'A' And B.SchType<>'O' " +
                        " Where A.FlatId=" + argFlatId + " Group By A.Description,A.SchPercent,A.SortOrder " +
                        " Order By A.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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
                dt.Dispose();
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

        public static DataTable GetOpCostCentre()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select X.CostCentreId,X.CostCentreName,Typewise=IsNull((Select Top 1 Typewise From dbo.PaySchType A " +
                        " Inner Join dbo.PaymentSchedule B On A.TypeId=B.TypeId Where CostCentreId=X.CostCentreId),0) From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre X" +
                        " Where ProjectDB In(Select ProjectName From " +
                        " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType in('B','L'))" +
                        " and CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                        " Where UserId=" + BsfGlobal.g_lUserId + ") Order By X.CostCentreName";
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

        #endregion
    }
}
