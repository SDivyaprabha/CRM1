using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace CRM.DataLayer
{
    class FlatUnitDL
    {
        public static DataTable GetBlock(int argCCID)
        {
            DataTable dt = new DataTable();
            try
            {
                string sSql = "Select BlockId,BlockName From dbo.BlockMaster Where CostCentreId=" + argCCID + " ";
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
            return dt;
        }

        public static DataTable GetLevel(int argCCID)
        {
            DataTable dt = new DataTable();
            try
            {
                string sSql = "Select LevelId,LevelName From dbo.LevelMaster Where CostCentreId=" + argCCID + " ";
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
            return dt;
        }

        public static DataTable GetFlatType(int argCCID)
        {
            DataTable dt = new DataTable();
            try
            {
                string sSql = "Select FlatTypeId,TypeName From dbo.FlatType Where ProjId=" + argCCID + " Order By TypeName";
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
            return dt;
        }

        public static bool GetFlatNoFound(int argCCId,string argFlatNo)
        {
            DataTable dt = new DataTable();
            bool bAns = false;
            try
            {
                string sSql = "Select FlatNo From dbo.FlatDetails Where FlatNo='" + argFlatNo + "' And CostCentreId=" + argCCId + "";
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

        public static bool GetFNoFound(int argCCId,string argFlatNo,SqlConnection conn,SqlTransaction tran)
        {
            DataTable dt = new DataTable();
            SqlDataReader sdr;
            SqlCommand cmd;
            bool bAns = false;
            try
            {
                string sSql = "Select FlatNo From dbo.FlatDetails Where FlatNo='" + argFlatNo + "' And CostCentreId=" + argCCId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                if (dt.Rows.Count == 1)
                {
                    bAns = true;
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
           
            return bAns;
        }

        public static string GetUniqueNoFound(int argCCId,int argFlatId,SqlConnection conn,SqlTransaction tran)
        {
            DataTable dt = new DataTable();
            SqlDataReader sdr; SqlCommand cmd;
            string bAns = "";
            try
            {
                string sSql = "Select FlatNo From dbo.FlatDetails Where FlatId='" + argFlatId + "' And CostCentreId=" + argCCId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                if (dt.Rows.Count > 0)
                {
                    bAns = dt.Rows[0]["FlatNo"].ToString();
                }
                cmd.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            
            return bAns;
        }

        public static void InsertFlatUnit(int argCCId, int argBlockId,int argLevelId,int argFlatTypeId,string argFlatNo)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            int iFlatId = 0;

            try
            {
                string sSql = "Select * From dbo.FlatType Where FlatTypeId = " + argFlatTypeId;
                SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                SqlDataReader sdreader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(sdreader);
                sdreader.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["FloorwiseRate"].ToString() == "N")
                    {
                        sSql = "Insert into dbo.FlatDetails(FlatNo,FlatTypeId,PayTypeId,BlockId,LevelId,Area,Rate,BaseAmt,AdvPercent," +
                                " AdvAmount,Guidelinevalue,USLand,USLandAmt,OtherCostAmt,LandRate,TotalCarPark,NetAmt,IntPercent,CreditDays,CostCentreId,Status,FacingId)" +
                                " Values('" + argFlatNo + "'," + argFlatTypeId + "," + dt.Rows[0]["PayTypeId"] + "," + argBlockId + "," +
                                " " + argLevelId + "," + dt.Rows[0]["Area"] + "," + dt.Rows[0]["Rate"] + "," + dt.Rows[0]["BaseAmt"] + "," +
                                " " + dt.Rows[0]["AdvPercent"] + "," + dt.Rows[0]["AdvAmount"] + "," + dt.Rows[0]["Guidelinevalue"] + "," + dt.Rows[0]["USLandArea"] + "," + dt.Rows[0]["LandAmount"] + "," +
                                " " + dt.Rows[0]["OtherCostAmt"] + "," + dt.Rows[0]["LandRate"] + "," + dt.Rows[0]["TotalCarpark"] + "," + dt.Rows[0]["NetAmt"] + "," +
                                " " + dt.Rows[0]["IntPercent"] + "," + dt.Rows[0]["CreditDays"] + "," + argCCId + ",'U'," + dt.Rows[0]["FacingId"] + ") SELECT SCOPE_IDENTITY();";
                    }
                    else
                    {
                        sSql = "SELECT F.FlatTypeId,R.LevelId,R.Rate FROM dbo.FloorRate R INNER JOIN dbo.FlatType F ON F.FlatTypeId=R.FlatTypeId" +
                                " WHERE R.LevelId=" + argLevelId + " AND R.FlatTypeId=" + argFlatTypeId + " AND F.ProjId=" + argCCId + "";
                        DataTable dtR = new DataTable();
                        dtR = CommFun.FillRcd(sSql, conn, tran);
                        
                        decimal dRate = 0;
                        if (dtR.Rows.Count > 0)
                            dRate = Convert.ToDecimal(dtR.Rows[0]["Rate"]);
                        else
                            dRate = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Rate"], CommFun.datatypes.vartypenumeric));
                        
                        decimal dBAmt = Convert.ToDecimal(dt.Rows[0]["Area"]) * dRate;
                        decimal dNAmt = dBAmt + Convert.ToDecimal(dt.Rows[0]["OtherCostAmt"]);

                        decimal dAdvAmt = Convert.ToDecimal(dt.Rows[0]["AdvAmount"]);
                        decimal dAdvPer = decimal.Round((dAdvAmt / dNAmt) * 100, 2);

                        sSql = "Insert Into dbo.FlatDetails(FlatNo,FlatTypeId,PayTypeId,BlockId,LevelId,Area,Rate,BaseAmt,AdvPercent," +
                                " AdvAmount,Guidelinevalue,USLand,USLandAmt,OtherCostAmt,LandRate,TotalCarPark,NetAmt,IntPercent,CreditDays,CostCentreId,Status,FacingId)" +
                                " Values('" + argFlatNo + "'," + argFlatTypeId + "," + dt.Rows[0]["PayTypeId"] + "," + argBlockId + "," +
                                " " + argLevelId + "," + dt.Rows[0]["Area"] + "," + dRate + "," + dBAmt + "," + dAdvPer + "," +
                                " " + dAdvAmt + "," + dt.Rows[0]["Guidelinevalue"] + "," + dt.Rows[0]["USLandArea"] + "," + dt.Rows[0]["LandAmount"] + "," +
                                " " + dt.Rows[0]["OtherCostAmt"] + "," + dt.Rows[0]["LandRate"] + "," + dt.Rows[0]["TotalCarpark"] + "," + dNAmt + "," +
                                " " + dt.Rows[0]["IntPercent"] + "," + dt.Rows[0]["CreditDays"] + "," + argCCId + ",'U'," + dt.Rows[0]["FacingId"] + ") SELECT SCOPE_IDENTITY();";
                    }
                    cmd = new SqlCommand(sSql, conn, tran);
                    iFlatId = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    sSql = "SELECT Max(SortOrder)SortOrder FROM dbo.FlatDetails Where CostCentreId=" + argCCId + " And BlockId=" + argBlockId + " And LevelId=" + argLevelId + " ";
                    DataTable dtSO = new DataTable();
                    dtSO = CommFun.FillRcd(sSql, conn, tran);

                    if (dtSO.Rows.Count > 0)
                    {
                        int iSortOrder = Convert.ToInt32(dtSO.Rows[0]["SortOrder"]) + 1;
                        sSql = "Update dbo.FlatDetails Set SortOrder=" + iSortOrder + " Where FlatId=" + iFlatId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    sSql = "SELECT * FROM dbo.FlatTypeArea WHERE FlatTypeId=" + argFlatTypeId + " AND CostCentreId=" + argCCId + "";
                    DataTable dtFT = new DataTable();
                    dtFT = CommFun.FillRcd(sSql, conn, tran);

                    if (dtFT.Rows.Count > 0)
                    {
                        for (int x = 0; x < dt.Rows.Count; x++)
                        {
                            sSql = "INSERT INTO dbo.FlatArea (CostCentreId,FlatId,AreaId,AreaSqft)VALUES (" + argCCId + "," + iFlatId + "," + dt.Rows[x]["AreaId"] + "," + dt.Rows[x]["AreaSqft"] + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    sSql = "SELECT F.*,M.OtherCostName FROM dbo.FlatTypeOtherCost F Inner Join dbo.OtherCostMaster M On F.OtherCostId=M.OtherCostId " +
                        " WHERE FlatTypeId=" + argFlatTypeId + " ";
                    DataTable dt1 = new DataTable();
                    dt1 = CommFun.FillRcd(sSql, conn, tran);

                    if (dt1.Rows.Count > 0)
                    {
                        for (int x = 0; x < dt1.Rows.Count; x++)
                        {
                            sSql = "INSERT INTO dbo.FlatOtherCost (FlatId,OtherCostId,Area,Rate,Flag,Amount)VALUES (" + iFlatId + "," + dt1.Rows[x]["OtherCostId"] + "," +
                            " " + dt1.Rows[x]["Area"] + "," + dt1.Rows[x]["Rate"] + ",'" + dt1.Rows[x]["Flag"] + "'," + dt1.Rows[x]["Amount"] + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    sSql = "SELECT * FROM FlatTypeOtherArea WHERE FlatTypeId=" + argFlatTypeId;
                    DataTable dtOA = new DataTable();
                    dtOA = CommFun.FillRcd(sSql, conn, tran);

                    if (dtOA.Rows.Count > 0)
                    {
                        for (int x = 0; x < dtOA.Rows.Count; x++)
                        {
                            sSql = "INSERT INTO FlatOtherArea (FlatId,OtherCostId,Area,Unit,Rate,Amount)VALUES (" + iFlatId + "," + dtOA.Rows[x]["OtherCostId"] + "," +
                            " " + dtOA.Rows[x]["Area"] + "," + dtOA.Rows[x]["Unit"] + "," + dtOA.Rows[x]["Rate"] + "," + dtOA.Rows[x]["Amount"] + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    sSql = "SELECT * FROM FlatTypeOtherInfra WHERE FlatTypeId=" + argFlatTypeId;
                    DataTable dtOI = new DataTable();
                    dtOI = CommFun.FillRcd(sSql, conn, tran);

                    if (dtOI.Rows.Count > 0)
                    {
                        for (int x = 0; x < dtOI.Rows.Count; x++)
                        {
                            sSql = "INSERT INTO FlatOtherInfra (FlatId,OtherCostId,AmountType,[Percent],Amount)VALUES (" + iFlatId + "," + dtOI.Rows[x]["OtherCostId"] + "," +
                            " '" + dtOI.Rows[x]["AmountType"] + "'," + dtOI.Rows[x]["Percent"] + "," + dtOI.Rows[x]["Amount"] + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    sSql = "SELECT * FROM FlatTypeTax WHERE FlatTypeId=" + argFlatTypeId;
                    DataTable dtTax = new DataTable();
                    dtTax = CommFun.FillRcd(sSql, conn, tran);

                    if (dtTax.Rows.Count > 0)
                    {
                        for (int x = 0; x < dtTax.Rows.Count; x++)
                        {
                            sSql = "INSERT INTO FlatTax (FlatId,QualifierId,Amount)VALUES (" + iFlatId + "," + dtTax.Rows[x]["QualifierId"] + "," +
                            " " + dtTax.Rows[x]["Amount"] + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    sSql = "SELECT * FROM dbo.FlatTypeQualifier WHERE FlatTypeId=" + argFlatTypeId + " AND CostCentreId=" + argCCId + "";
                    DataTable dt4 = new DataTable();
                    dt4 = CommFun.FillRcd(sSql, conn, tran);

                    if (dt4.Rows.Count > 0)
                    {
                        for (int x = 0; x < dt4.Rows.Count; x++)
                        {
                            sSql = "INSERT INTO dbo.FlatQualifier (CostCentreId,FlatId,OtherCostId,QualiId,Expression,QualiAmt,Flag)VALUES (" + argCCId + "," + iFlatId + "," + dt4.Rows[x]["OtherCostId"] + "," + dt4.Rows[x]["QualiId"] + "," +
                            " '" + dt4.Rows[x]["Expression"] + "'," + dt4.Rows[x]["QualiAmt"] + ",'" + dt4.Rows[x]["Flag"] + "')";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    sSql = "SELECT A.CostCentreId,A.FlatTypeId,A.TypeId,A.TotalCP,B.NoOfSlots,B.AllottedSlots" +
                            " FROM FlatTypeCarPark A Inner Join CarParkMaster B On A.TypeId=B.TypeId And A.CostCentreId=B.CostCentreId" +
                            " WHERE FlatTypeId=" + argFlatTypeId + " AND" +
                            " A.CostCentreId=" + argCCId + " And BlockId=" + argBlockId + "";
                    DataTable dtCP = new DataTable();
                    dtCP = CommFun.FillRcd(sSql, conn, tran);

                    CRM.BL.ProjectInfoBL.UpdateCarParkSlot(argBlockId, argCCId, conn, tran);
                    int iSlots = 0; int iTotCP = 0;
                    if (dtCP.Rows.Count > 0)
                    {
                        for (int x = 0; x < dtCP.Rows.Count; x++)
                        {
                            iSlots = Convert.ToInt32(dtCP.Rows[x]["NoOfSlots"]) - Convert.ToInt32(dtCP.Rows[x]["AllottedSlots"]);
                            if (iSlots < 0) { iSlots = 0; }
                            if (iSlots >= Convert.ToInt32(dtCP.Rows[x]["TotalCP"]))
                            {
                                iSlots = Convert.ToInt32(dtCP.Rows[x]["TotalCP"]);
                                sSql = "INSERT INTO FlatCarPark (CostCentreId,FlatId,TypeId,TotalCP)VALUES (" + argCCId + "," + iFlatId + "," +
                                       " " + dtCP.Rows[x]["TypeId"] + "," + iSlots + ")";
                            }
                            else
                                sSql = "INSERT INTO FlatCarPark (CostCentreId,FlatId,TypeId,TotalCP)VALUES (" + argCCId + "," + iFlatId + "," +
                                       " " + dtCP.Rows[x]["TypeId"] + "," + iSlots + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            iTotCP = iTotCP + iSlots;
                        }
                    }
                    if (iTotCP < 0) iTotCP = 0;
                    sSql = "Update FlatDetails Set TotalCarPark=" + iTotCP + " Where FlatId=" + iFlatId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();

                    CRM.BL.ProjectInfoBL.UpdateCarParkSlot(argBlockId, argCCId, conn, tran);
                    CRM.BusinessLayer.UnitDirBL.InsertFlatCar(iFlatId, argCCId, conn, tran);


                    sSql = "Select * From dbo.FlatTypeExtraItem " +
                        " Where FlatTypeId=" + argFlatTypeId;
                    DataTable dt2 = new DataTable();
                    dt2 = CommFun.FillRcd(sSql, conn, tran);

                    if (dt2.Rows.Count > 0)
                    {
                        for (int x = 0; x < dt2.Rows.Count; x++)
                        {
                            sSql = "INSERT INTO dbo.FlatExtraItem (FlatId,ExtraItemId,Quantity,Rate,Amount)VALUES " +
                                " (" + iFlatId + "," + dt2.Rows[x]["ExtraItemId"] + "," + dt2.Rows[x]["Qty"] + "," +
                                " " + dt2.Rows[x]["ExtraRate"] + "," + dt2.Rows[x]["Amount"] + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    sSql = "SELECT F.* FROM dbo.FlatTypeCheckList F WHERE FlatTypeId=" + argFlatTypeId + " AND CostCentreId=" + argCCId + "";
                    DataTable dt3 = new DataTable();
                    dt3 = CommFun.FillRcd(sSql, conn, tran);

                    if (dt3.Rows.Count > 0)
                    {
                        for (int x = 0; x < dt3.Rows.Count; x++)
                        {
                            sSql = "INSERT INTO dbo.FlatCheckList (CheckListId,FlatId,Status,ExpCompletionDate)VALUES" +
                                " (" + dt3.Rows[x]["CheckListId"] + "," + iFlatId + ",'" + dt3.Rows[x]["Status"] + "','" + Convert.ToDateTime(dt3.Rows[x]["ExpCompletionDate"]).ToString("dd-MMM-yyyy") + "')";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }

                    sSql = " Update dbo.ProjectInfo Set TotalFlats=(Select Count(FlatId) TotalFlat From dbo.FlatDetails" +
                           " Where CostCentreId=" + argCCId + ") Where CostCentreId=" + argCCId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    decimal dBaseAmt = 0;
                    sSql = "SELECT BaseAmt FROM dbo.FlatDetails WHERE FlatId=" + iFlatId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    sdreader = cmd.ExecuteReader();
                    DataTable dtB = new DataTable();
                    dtB.Load(sdreader);
                    sdreader.Close();
                    cmd.Dispose();

                    if (dtB.Rows.Count > 0) { dBaseAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtB.Rows[0]["BaseAmt"], CommFun.datatypes.vartypenumeric)); }

                    decimal dOtherAmt = 0;
                    sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                            " Where FlatId =" + iFlatId + " and OtherCostId not in (Select OtherCostId from dbo.OXGross " +
                            " Where CostCentreId=" + argCCId + ")";
                    cmd = new SqlCommand(sSql, conn, tran);
                    sdreader = cmd.ExecuteReader();
                    DataTable dtOCost = new DataTable();
                    dtOCost.Load(sdreader);
                    sdreader.Close();
                    cmd.Dispose();

                    if (dtOCost.Rows.Count > 0) { dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtOCost.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }

                    decimal dNetAmt = dBaseAmt + dOtherAmt;
                    sSql = "UPDATE dbo.FlatDetails SET NetAmt=" + dNetAmt + " WHERE FlatId=" + iFlatId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    PaymentScheduleDL.InsertFlatScheduleI(iFlatId, conn, tran);
                    FlatDetailsDL.UpdateFlatQualAmt(Convert.ToInt32(dt.Rows[0]["PayTypeId"]), iFlatId, conn, tran);
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

        #region SortOrder

        public static DataTable GetSOBlock(int argCCId)
        {
            DataTable dt = new DataTable();
            try
            {
                string sSql = "Select BlockId,BlockName From dbo.BlockMaster Where CostCentreId=" + argCCId + " ";
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
            return dt;
        }

        public static DataTable GetSOLevel(int argCCId)
        {
            DataTable dt = new DataTable();
            try
            {
                string sSql = "Select LevelId,LevelName From dbo.LevelMaster Where CostCentreId=" + argCCId + " ";
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
            return dt;
        }

        public static DataTable GetSOFlat(int argCCId,int argBlockId,int argLevelId)
        {
            DataTable dt = new DataTable();
            try
            {
                string sSql = "Select A.FlatId,A.FlatNo From dbo.FlatDetails A " +
                    " Inner Join dbo.BlockMaster B On B.BlockId=A.BlockId Inner Join dbo.LevelMaster L On L.LevelId=A.LevelId " +
                    " Where A.CostCentreId=" + argCCId + " And A.BlockId=" + argBlockId + " And A.LevelId=" + argLevelId + " " +
                    " Order By B.SortOrder,L.SortOrder,A.SortOrder,dbo.Val(A.FlatNo)";
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
            return dt;
        }

        public static void UpdateSortOrder(DataTable dt)
        {
            SqlCommand cmd;
            string sSql = "";
            try
            {
                dt.AcceptChanges();
                BsfGlobal.OpenCRMDB();
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    int iFlatId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["FlatId"].ToString(), CommFun.datatypes.vartypenumeric));
                    int iOrder = i + 1;
                    sSql = "Update dbo.FlatDetails Set SortOrder=" + iOrder + " Where FlatId=" + iFlatId + " ";
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
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
        }

        #endregion

    }
}
