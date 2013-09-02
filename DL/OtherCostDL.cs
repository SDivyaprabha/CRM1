using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessLayer;

namespace CRM.DataLayer
{
    class OtherCostDL
    {
        #region Methods

        public static DataTable GetData(OtherCostBL oOtherCostBL, SqlConnection Con)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            try
            {
                string ssql = "";
                ssql = String.Format("SELECT O.OtherCostId,OtherCostName,Flag,Amount FROM dbo.OtherCostMaster O INNER JOIN dbo.FlatType_OtherCostQ F ON O.OtherCostId=F.OtherCostId WHERE FlatTypeId={0}", oOtherCostBL.TypeId);
                sda = new SqlDataAdapter(ssql, Con);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return dt;
        }

        public static DataTable GetCarpark(OtherCostBL oOtherCostBL, SqlConnection Con)
        {
            DataTable dt = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("GetCarPark", Con);
                sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand.Parameters.Clear();
                sda.SelectCommand.Parameters.AddWithValue("@FlatTypeId", oOtherCostBL.TypeId);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return dt;
        }

        public static DataTable GetCarTemplate(SqlConnection Con)
        {
            DataTable dt = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT * FROM dbo.CarParkSlotDetails ORDER BY CarParkSlotId", Con);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }

            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return dt;
        }

        public static int Update(OtherCostBL oOtherCostBL)
        {
            using (DataTable dtA = oOtherCostBL.DtOC.GetChanges(DataRowState.Added))
            {
                using (DataTable dtM = oOtherCostBL.DtOC.GetChanges(DataRowState.Modified))
                {
                    SqlCommand cmd;
                    if (dtA != null)
                    {
                        for (int i = 0; i < dtA.Rows.Count; i++)
                        {
                            cmd = new SqlCommand(String.Format("INSERT INTO dbo.OtherCostMaster(OtherCostName) Values ('{0}')", dtA.Rows[i]["OtherCostName"]), BsfGlobal.OpenCRMDB());
                            cmd.ExecuteNonQuery();
                        }
                    }
                    if (dtM != null)
                    {
                        for (int i = 0; i < dtM.Rows.Count; i++)
                        {
                            cmd = new SqlCommand(String.Format("Update dbo.OtherCostMaster SET OtherCostName='{0}' WHERE OtherCostId='{1}'", dtM.Rows[i]["OtherCostName"], dtM.Rows[i]["OtherCostID"]), BsfGlobal.OpenCRMDB());
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            return 0;
        }

        public static int UpdateCarPark(OtherCostBL oOtherCostBL) 
        {
            using (DataTable dtA = oOtherCostBL.DtOC.GetChanges(DataRowState.Added))
            {
                using (DataTable dtM = oOtherCostBL.DtOC.GetChanges(DataRowState.Modified))
                {
                    //const int a = 0;
                    SqlCommand cmd;
                    BsfGlobal.OpenCRMDB();
                    if (dtA != null)
                    {
                        for (int i = 0; i < dtA.Rows.Count; i++)
                        {
                            cmd = new SqlCommand(String.Format("INSERT INTO dbo.CarParkSlotDetails(CostCentreId,CarParkSlotNo,Area,Rate,Amount,Freeze) Values ('{0}','{1}','{2}','{3}','{4}','N')", dtA.Rows[i]["CostCentreId"], dtA.Rows[i]["CarParkSlotNo"], dtA.Rows[i]["Area"], dtA.Rows[i]["Rate"], dtA.Rows[i]["Amount"]), BsfGlobal.OpenCRMDB());
                            //cmd.ExecuteNonQuery();
                        }
                        if (dtM != null)
                        {
                            for (int i = 0; i < dtM.Rows.Count; i++)
                            {
                                cmd = new SqlCommand(String.Format("Update dbo.CarParkSlotDetails SET CostCentreId='{0}', CarParkSlotNo='{1}',Area='{2}', Rate='{3}',Amount='{4}'  WHERE CarParkSlotId='{5}'", dtM.Rows[i]["CostCentreId"], dtM.Rows[i]["CarParkSlotNo"], dtM.Rows[i]["Area"], dtM.Rows[i]["Rate"], dtM.Rows[i]["Amount"], dtM.Rows[i]["CarParkSlotId"]), BsfGlobal.OpenCRMDB());
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    return 0;
                }
            }
        }

        public static DataTable GetOCList(int argCCId,int argPayTypeId)
        {
            DataTable dt = new DataTable(); 
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                //sql = "SELECT A.OtherCostId,A.OtherCostName,Case when B.OtherCostId is null then Convert(bit,0,0) else Convert(bit,1,1) end Sel " +
                //      "FROM dbo.OtherCostMaster A Left Join dbo.OtherCostSetupTrans B on A.OtherCostId=B.OtherCostId and B.CostCentreId= " + argCCId + " and B.PayTypeId= " + argPayTypeId + " " +
                //      "Where A.OtherCostId not in (Select OtherCostId From dbo.PaymentSchedule Where CostCentreId=" + argCCId + " and TypeId=" + argPayTypeId + ")";
                sql = "SELECT A.OtherCostId,A.OtherCostName,Case when B.OtherCostId is null then Convert(bit,0,0) " +
                        " else Convert(bit,1,1) end Sel FROM dbo.OtherCostMaster A " +
                        " Inner Join dbo.CCOtherCost CO On CO.OtherCostId=A.OtherCostId " +
                        " Left Join dbo.OtherCostSetupTrans B " +
                        " on A.OtherCostId=B.OtherCostId and B.CostCentreId=" + argCCId + " " +
                        " and B.PayTypeId=" + argPayTypeId + " Where A.OtherCostId " +
                        " not in (Select OtherCostId From dbo.PaymentSchedule " +
                        " Where CostCentreId=" + argCCId + " and TypeId=" + argPayTypeId + ") And CO.CostCentreId=" + argCCId + "";
               
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        public static DataTable GetQualifierMaster(string argType, int argId, string argFlatType)
        {
            string sSql = "";
            DataTable dt = null; DataTable dt1 = new DataTable();
            try
            {
                if (argFlatType == "FlatType")
                    sSql = "Select A.QualifierId,A.QualifierName,Amount from dbo.Qualifier_Temp A " +
                        " Inner Join [" + BsfGlobal.g_sCRMDBName + "].dbo.FlatTypeTax B On A.QualifierId=B.QualifierId " +
                        " Where QualType='B' And FlatTypeId=" + argId + "";
                else
                    sSql = "Select A.QualifierId,A.QualifierName,Amount from dbo.Qualifier_Temp A " +
                        " Inner Join [" + BsfGlobal.g_sCRMDBName + "].dbo.FlatTax B On A.QualifierId=B.QualifierId " +
                        " Where QualType='B' And FlatId=" + argId + "";
                BsfGlobal.OpenRateAnalDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_RateAnalDB);
                dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    sSql = "Select QualifierId,QualifierName,0 Amount from Qualifier_Temp Where QualType='" + argType + "'";
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_RateAnalDB);
                    da.Fill(dt1);
                    dt = dt1;
                }
                else
                {
                    if (argFlatType == "FlatType")
                        sSql = "Select QualifierId,QualifierName,0 Amount from Qualifier_Temp Where QualType='" + argType + "'" +
                            " And QualifierId Not In(Select A.QualifierId from dbo.Qualifier_Temp A " +
                            " Inner Join [" + BsfGlobal.g_sCRMDBName + "].dbo.FlatTypeTax B On A.QualifierId=B.QualifierId " +
                            " Where QualType='B' And FlatTypeId=" + argId + ")";
                    else
                        sSql = "Select QualifierId,QualifierName,0 Amount from Qualifier_Temp Where QualType='" + argType + "'" +
                        " And QualifierId Not In(Select A.QualifierId from dbo.Qualifier_Temp A " +
                        " Inner Join [" + BsfGlobal.g_sCRMDBName + "].dbo.FlatTax B On A.QualifierId=B.QualifierId " +
                        " Where QualType='B' And FlatId=" + argId + ")";
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_RateAnalDB);
                    da.Fill(dt1);
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataRow dr;
                        dr = dt.NewRow();
                        dr["QualifierId"] = dt1.Rows[i]["QualifierId"];
                        dr["QualifierName"] = dt1.Rows[i]["QualifierName"];
                        dr["Amount"] = dt1.Rows[i]["Amount"];

                        dt.Rows.Add(dr);
                    }
                }
                da.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_RateAnalDB.Close();
            }
            
            return dt;
        }

        public static DataTable GetOCListOption(int argCCId, int argPayTypeId)
        {
            DataTable dt = new DataTable(); ;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                //sql = "SELECT OtherCostId,OtherCostName,Convert(bit,0,0) Sel " +
                //      "FROM dbo.OtherCostMaster " +
                //      "Where OtherCostId not in (Select OtherCostId from dbo.PaymentSchedule Where CostCentreId=" + argCCId + " and TypeId=" + argPayTypeId + ") " +
                //      "and OtherCostId Not in (Select OtherCostId from dbo.OtherCostSetupTrans Where CostCentreId=" + argCCId + " and PayTypeId=" + argPayTypeId + ")";
                sql = "SELECT A.OtherCostId,OtherCostName,Convert(bit,0,0) Sel FROM dbo.OtherCostMaster A " +
                        " Inner Join dbo.CCOtherCost CO On CO.OtherCostId=A.OtherCostId Where A.OtherCostId not in ( " +
                        " Select OtherCostId from dbo.PaymentSchedule Where CostCentreId=" + argCCId + " and TypeId=" + argPayTypeId + ") and A.OtherCostId Not in ( " +
                        " Select OtherCostId from dbo.OtherCostSetupTrans Where CostCentreId=" + argCCId + " and PayTypeId=" + argPayTypeId + ") And CO.CostCentreId=" + argCCId + " ";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        public static bool GetAdvance(int argCCId, int argPayTypeId)
        {
            bool bAns = false;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "Select TemplateId From dbo.PaymentSchedule Where SchType='A' and TypeId=" + argPayTypeId + " and CostCentreId= " + argCCId;
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                DataTable dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                if (dt.Rows.Count > 0) { bAns = true; }
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
            return bAns;
        }

        public static bool GetTypewise(int argPayTypeId)
        {
            bool bAns = false;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "Select Typewise From dbo.PaySchType Where TypeId=" + argPayTypeId + " ";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                DataTable dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                if (dt.Rows.Count > 0) { bAns = Convert.ToBoolean(dt.Rows[0]["Typewise"]); }
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
            return bAns;
        }

        public static bool GetQualifier(int argCCId, int argPayTypeId)
        {
            bool bAns = false;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "Select TemplateId From dbo.PaymentSchedule Where SchType='Q' and TypeId=" + argPayTypeId + " and CostCentreId= " + argCCId;
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                DataTable dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                if (dt.Rows.Count > 0) { bAns = true; }
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
            return bAns;
        }

        public static DataTable GetFTOCNull()
        {
            DataTable dt = null;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "SELECT FlatTypeId,F.OtherCostId,O.OtherCostName,Flag,Amount" +
                    " FROM dbo.FlatTypeOtherCost F INNER JOIN dbo.OtherCostMaster O ON" +
                    " F.OtherCostId =O.OtherCostId WHERE FlatTypeId=null";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        public static DataTable GetFDOCNull()
        {
            DataTable dt = null;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "SELECT FlatId,F.OtherCostId,O.OtherCostName,Flag,Amount" +
                    " FROM dbo.FlatOtherCost F INNER JOIN dbo.OtherCostMaster O ON" +
                    " F.OtherCostId =O.OtherCostId WHERE FlatId=null";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        public static DataTable GetFTOC(int argFlatTypeId)
        {
            DataTable dt = null;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "SELECT F.OtherCostId,O.OtherCostName,F.Flag,F.Amount,O.SysDefault,O.Area FROM dbo.FlatTypeOtherCost F" +
                      " INNER JOIN dbo.OtherCostMaster O ON F.OtherCostId = O.OtherCostId " +
                      "Where F.FlatTypeId= " + argFlatTypeId + " ORDER BY O.SortOrder";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        public static DataTable GetFDOC(int argFlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "SELECT A.FlatId,A.OtherCostId,B.OtherCostName,A.Flag,A.Amount,B.SysDefault,B.Area FROM dbo.FlatOtherCost A" +
                      " INNER JOIN dbo.OtherCostMaster B ON A.OtherCostId=B.OtherCostId" +
                      " WHERE A.FlatId=" + argFlatId + " ORDER BY B.SortOrder";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        public static DataTable GetGlobalOC(int argFlatTypeId)
        {
            DataTable dt = null;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "SELECT 0 FlatId,F.OtherCostId,O.OtherCostName,F.Flag,F.Amount,O.SysDefault,O.Area " +
                      "FROM dbo.FlatTypeOtherCost F INNER JOIN dbo.OtherCostMaster O ON " +
                      "F.OtherCostId = O.OtherCostId " +
                      "Where F.FlatTypeId= " + argFlatTypeId;
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        public static DataTable GetFTReg(decimal argReg,int iTotCP,int argFTId,int argCCId)
        {
            DataTable dtT = new DataTable();
            SqlDataAdapter sda = null;
            string sql = "";
            DataTable dt = new DataTable();
            BsfGlobal.OpenCRMDB();

            int iOpenCP=0, iClosedCP=0, iTerracecCP=0;
            decimal dOpenAmt=0, dClosedAmt=0, dTerraceAmt=0, dTotalAmt=0;
            try
            {

                sql = "SELECT 0 FlatTypeId,OtherCostId,OtherCostName,'+/-' Flag,0.00 Amount FROM dbo.OtherCostMaster" +
                    " WHERE OtherCostId IN(1,2)";
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dtT = new DataTable();
                sda.Fill(dtT);

                sql = "Select OpenCP,ClosedCP,TerraceCP From dbo.FlatTypeCarPark WHERE FlatTypeId=" + argFTId + "";
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                DataTable dt1 = new DataTable();
                sda.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    iOpenCP = Convert.ToInt32(dt1.Rows[0]["OpenCP"]);
                    iClosedCP = Convert.ToInt32(dt1.Rows[0]["ClosedCP"]);
                    iTerracecCP = Convert.ToInt32(dt1.Rows[0]["TerraceCP"]);
                }
                if (iOpenCP >= 1)
                {
                    sql = "Select Cost,AddCost From dbo.CarParkCost WHERE CarParkSlotCostId=1 AND TypeId=1 AND CostCentreId=" + argCCId + "";
                    dt = CommFun.FillRecord(sql);
                    if (dt.Rows.Count > 0)
                    {
                        dOpenAmt = Convert.ToDecimal(dt.Rows[0]["Cost"].ToString());

                        if (iOpenCP > 1)
                        {
                            if (Convert.ToDecimal(dt.Rows[0]["AddCost"].ToString()) > 0)
                            {

                                dOpenAmt = dOpenAmt + (Convert.ToDecimal(dt.Rows[0]["AddCost"].ToString()) * (iOpenCP - 1));
                            }
                            else
                            {
                                dOpenAmt = dOpenAmt + (Convert.ToDecimal(dt.Rows[0]["Cost"].ToString()) * (iOpenCP - 1));
                            }
                        }

                    }
                    dt.Dispose();
                }


                if (iClosedCP >= 1)
                {
                    sql = "Select Cost,AddCost From dbo.CarParkCost WHERE CarParkSlotCostId=1 AND TypeId=2 AND CostCentreId=" + argCCId + "";
                    dt = CommFun.FillRecord(sql);
                    if (dt.Rows.Count > 0)
                    {
                        dClosedAmt = Convert.ToDecimal(dt.Rows[0]["Cost"].ToString());

                        if (iClosedCP > 1)
                        {
                            if (Convert.ToDecimal(dt.Rows[0]["AddCost"].ToString()) > 0)
                            {

                                dClosedAmt = dClosedAmt + (Convert.ToDecimal(dt.Rows[0]["AddCost"].ToString()) * (iClosedCP - 1));
                            }
                            else
                            {
                                dClosedAmt = dClosedAmt + (Convert.ToDecimal(dt.Rows[0]["Cost"].ToString()) * (iClosedCP - 1));
                            }
                        }

                    }
                    dt.Dispose();
                }

                if (iTerracecCP >= 1)
                {
                    sql = "Select Cost,AddCost From dbo.CarParkCost WHERE CarParkSlotCostId=1 AND TypeId=3 AND CostCentreId=" + argCCId + "";
                    dt = CommFun.FillRecord(sql);
                    if (dt.Rows.Count > 0)
                    {
                        dTerraceAmt = Convert.ToDecimal(dt.Rows[0]["Cost"].ToString());

                        if (iTerracecCP > 1)
                        {
                            if (Convert.ToDecimal(dt.Rows[0]["AddCost"].ToString()) > 0)
                            {

                                dTerraceAmt = dTerraceAmt + (Convert.ToDecimal(dt.Rows[0]["AddCost"].ToString()) * (iTerracecCP - 1));
                            }
                            else
                            {
                                dTerraceAmt = dTerraceAmt + (Convert.ToDecimal(dt.Rows[0]["Cost"].ToString()) * (iTerracecCP - 1));
                            }
                        }

                    }
                    dt.Dispose();
                }

                dTotalAmt = dOpenAmt + dClosedAmt + dTerraceAmt;

                if (dtT.Rows.Count > 0)
                {

                    dtT.Rows[0]["Amount"] = argReg;
                    dtT.Rows[1]["Amount"] = dTotalAmt;

                }
                sda.Dispose();
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

        public static DataTable GetFDReg(decimal argReg, int iTotCP, int argFId, int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda = null;
            BsfGlobal.OpenCRMDB();
            int iOpenCP = 0, iClosedCP = 0, iTerracecCP = 0;
            decimal dOpenAmt = 0, dClosedAmt = 0, dTerraceAmt = 0, dTotalAmt = 0;
            string sql = "";
            try
            {
                sql = "SELECT 0 FlatId,OtherCostId,OtherCostName,'+/-' Flag,0.00 Amount FROM dbo.OtherCostMaster" +
                    " WHERE OtherCostId IN(1,2)";
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);


                sql = "Select OpenCP,ClosedCP,TerraceCP From dbo.FlatCarPark WHERE FlatId=" + argFId + "";
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                DataTable dt1 = new DataTable();
                sda.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    iOpenCP = Convert.ToInt32(dt1.Rows[0]["OpenCP"]);
                    iClosedCP = Convert.ToInt32(dt1.Rows[0]["ClosedCP"]);
                    iTerracecCP = Convert.ToInt32(dt1.Rows[0]["TerraceCP"]);
                }
                if (iOpenCP == 1)
                {
                    sql = "Select Cost From dbo.CarParkCost WHERE CarParkSlotCostId=1 AND TypeId=1 AND CostCentreId=" + argCCId + "";
                }
                else if (iOpenCP > 1)
                {
                    sql = "Select Cost From dbo.CarParkCost WHERE CarParkSlotCostId=1 AND TypeId=1 AND CostCentreId=" + argCCId + "";
                    sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                    DataTable dtOP = new DataTable();
                    sda.Fill(dtOP);
                    if (dtOP.Rows.Count > 0)
                    {
                        dOpenAmt = Convert.ToDecimal(dtOP.Rows[0]["Cost"]);
                    }
                    iOpenCP = iOpenCP - 1;
                    sql = "Select Cost From dbo.CarParkCost WHERE CarParkSlotCostId=2 AND TypeId=1 AND CostCentreId=" + argCCId + "";
                }
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                DataTable dt2 = new DataTable();
                sda.Fill(dt2);
                if (dt2.Rows.Count > 0)
                {
                    decimal dPerOpenAmt = Convert.ToDecimal(dt2.Rows[0]["Cost"]);
                    dOpenAmt = dOpenAmt + (dPerOpenAmt * iOpenCP);
                }

                if (iClosedCP == 1)
                {
                    sql = "Select Cost From dbo.CarParkCost WHERE CarParkSlotCostId=1 AND TypeId=2 AND CostCentreId=" + argCCId + "";
                }
                else if (iClosedCP > 1)
                {
                    sql = "Select Cost From dbo.CarParkCost WHERE CarParkSlotCostId=1 AND TypeId=2 AND CostCentreId=" + argCCId + "";
                    sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                    DataTable dtCP = new DataTable();
                    sda.Fill(dtCP);
                    if (dtCP.Rows.Count > 0)
                    {
                        dClosedAmt = Convert.ToDecimal(dtCP.Rows[0]["Cost"]);
                    }
                    iClosedCP = iClosedCP - 1;
                    sql = "Select Cost From dbo.CarParkCost WHERE CarParkSlotCostId=2 AND TypeId=2 AND CostCentreId=" + argCCId + "";
                }
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                DataTable dt3 = new DataTable();
                sda.Fill(dt3);
                if (dt3.Rows.Count > 0)
                {
                    decimal dPerClosedAmt = Convert.ToDecimal(dt3.Rows[0]["Cost"]);
                    dClosedAmt = dClosedAmt + (dPerClosedAmt * iClosedCP);
                }

                if (iTerracecCP == 1)
                {
                    sql = "Select Cost From dbo.CarParkCost WHERE CarParkSlotCostId=1 AND TypeId=3 AND CostCentreId=" + argCCId + "";
                }
                else if (iTerracecCP > 1)
                {
                    sql = "Select Cost From dbo.CarParkCost WHERE CarParkSlotCostId=1 AND TypeId=3 AND CostCentreId=" + argCCId + "";
                    sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                    DataTable dtTP = new DataTable();
                    sda.Fill(dtTP);
                    if (dtTP.Rows.Count > 0)
                    {
                        dTerraceAmt = Convert.ToDecimal(dtTP.Rows[0]["Cost"]);
                    }
                    iTerracecCP = iTerracecCP - 1;
                    sql = "Select Cost From dbo.CarParkCost WHERE CarParkSlotCostId=2 AND TypeId=3 AND CostCentreId=" + argCCId + "";
                }
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                DataTable dt4 = new DataTable();
                sda.Fill(dt4);
                if (dt4.Rows.Count > 0)
                {
                    decimal dPerTerraceAmt = Convert.ToDecimal(dt4.Rows[0]["Cost"]);
                    dTerraceAmt = dTerraceAmt + (dPerTerraceAmt * iTerracecCP);
                }

                dTotalAmt = dOpenAmt + dClosedAmt + dTerraceAmt;


                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[0]["Amount"] = argReg;
                        dt.Rows[1]["Amount"] = dTotalAmt;
                    }
                }
                sda.Dispose();
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

        private static void UpdateReceiptOrder(int argPayTypeId,int argCCId)
        {
            DataTable dt = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd;
            bool bAns = false;
            try
            {
                sSql = "Select TransId From dbo.ReceiptTypeOrder Where PayTypeId=" + argPayTypeId + " and CostCentreId= " + argCCId;
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                if (dt.Rows.Count > 0) { bAns = true; }
                dt.Dispose();
                if (bAns == false)
                {
                    sSql = "Select TemplateId From dbo.PaymentSchedule Where SchType='A' and TypeId=" + argPayTypeId + " and CostCentreId= " + argCCId;
                    bool bAdvance = false;
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    DataTable dtT = new DataTable();
                    da.Fill(dtT);
                    if (dtT.Rows.Count > 0) { bAdvance = true; }
                    da.Dispose();
                    dtT.Dispose();


                    if (bAdvance == false)
                    {

                        sSql = "Insert Into dbo.ReceiptTypeOrder(PayTypeId,CostCentreId,ReceiptTypeId,OtherCostId,SchType,SortOrder) " +
                               "Values(" + argPayTypeId + "," + argCCId + ",1,0,'A',1)";
                        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    //sSql = "Select TemplateId from PaymentSchedule Where SchType='Q' and TypeId=" + argPayTypeId + " and CostCentreId= " + argCCId;
                    //bool bQualifier = false;
                    //da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    //DataTable dtQ = new DataTable();
                    //da.Fill(dtQ);
                    //if (dtQ.Rows.Count > 0) { bQualifier = true; }
                    //da.Dispose();
                    //dtQ.Dispose();


                    //if (bQualifier == false)
                    //{
                    //    int iRow = 0;
                    //    sSql = "Select * from PaymentSchedule Where TypeId=" + argPayTypeId + " and CostCentreId= " + argCCId;
                    //    da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    //    DataTable dtR = new DataTable();
                    //    da.Fill(dtR);
                    //    if (dtR.Rows.Count > 0) { iRow = dtR.Rows.Count; iRow = iRow + 1; }
                    //    da.Dispose();
                    //    dtR.Dispose();

                    //    sSql = "Insert into ReceiptTypeOrder(PayTypeId,CostCentreId,ReceiptTypeId,OtherCostId,SchType,SortOrder) " +
                    //           "Values(" + argPayTypeId + "," + argCCId + ",6,0,'Q',"+iRow+")";
                    //    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    //    cmd.ExecuteNonQuery();
                    //    cmd.Dispose();
                    //}

                    sSql = "Insert Into dbo.ReceiptTypeOrder(PayTypeId,CostCentreId,ReceiptTypeId,OtherCostId,SchType) " +
                           "Select " + argPayTypeId + "," + argCCId + ",ReceiptTypeId,0,'R' from ReceiptType Where ReceiptTypeId <>1 And ReceiptTypeId <>6";
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Insert Into dbo.ReceiptTypeOrder(PayTypeId,CostCentreId,ReceiptTypeId,OtherCostId,SchType) " +
                           "Select " + argPayTypeId + "," + argCCId + ",0,OtherCostId,'O' from OtherCostSetupTrans Where CostCentreId=" + argCCId + " and PayTypeId=" + argPayTypeId;
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

        public static void InsertOCSetup(DataTable argDtOCSetup, int ccid, int argPayTypeId, bool argAdvance, bool argQualifier, int argTypewise)
        {
            UpdateReceiptOrder(argPayTypeId, ccid);

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran  = conn.BeginTransaction();
            
            string sSql = "";
            try
            {
                sSql = "DELETE FROM dbo.OtherCostSetupTrans WHERE CostCentreId=" + ccid + " and PayTypeId=" + argPayTypeId;
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                for (int c = 0; c < argDtOCSetup.Rows.Count; c++)
                {
                    sSql = String.Format("INSERT INTO dbo.OtherCostSetupTrans(CostCentreId,OtherCostId,PayTypeId) Values({0},{1},{2})", ccid, argDtOCSetup.Rows[c]["OtherCostId"],argPayTypeId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                DataTable dt;
                SqlDataReader sdr;

                if (argAdvance == true)
                {
                    dt = new DataTable();
                    bool bAns = false;

                    sSql = "Select TemplateId From dbo.PaymentSchedule Where SchType='A' and TypeId= " + argPayTypeId + " and CostCentreId = " + ccid;
                    cmd = new SqlCommand(sSql, conn, tran);
                    dt = new DataTable();
                    sdr = cmd.ExecuteReader();
                    dt.Load(sdr);
                    if (dt.Rows.Count > 0) { bAns = true; }
                    dt.Dispose();
                    sdr.Close();

                    if (bAns == false)
                    {
                        int iTempId = 0;
                        sSql = "Insert Into dbo.PaymentSchedule (TypeId,CostCentreId,SchType,Description,SortOrder,SchDate) " +
                               "Values(" + argPayTypeId + "," + ccid + ",'A','Advance',1,null) SELECT SCOPE_IDENTITY();";
                        cmd = new SqlCommand(sSql, conn, tran);
                        iTempId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                        cmd.Dispose();

                        sSql = "Insert Into dbo.CCReceiptType(TemplateId,Percentage,SchType) Values(" + iTempId +",100,'A')";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();


                        sSql = "Select TemplateId From dbo.PaymentSchedule Where SchType<>'A' and TypeId= " + argPayTypeId + " and CostCentreId = " + ccid + " Order by SortOrder,SchDate";
                        cmd = new SqlCommand(sSql, conn, tran);
                        dt = new DataTable();
                        sdr = cmd.ExecuteReader();
                        dt.Load(sdr);

                        
                        int iSortOrder = 0;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            iTempId = Convert.ToInt32(dt.Rows[i]["TemplateId"].ToString());
                            iSortOrder = i + 2;

                            sSql = "Update dbo.PaymentSchedule Set SortOrder = " + iSortOrder + " Where TemplateId = " + iTempId;
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                        dt.Dispose();
                        sdr.Close();
                    }
                }
                //if (argQualifier == true)
                //{
                //    dt = new DataTable(); int iTempId = 0;
                //    bool bAns = false;

                //    sSql = "Select TemplateId from PaymentSchedule Where SchType='Q' and TypeId= " + argPayTypeId + " and CostCentreId = " + ccid;
                //    cmd = new SqlCommand(sSql, conn, tran);
                //    dt = new DataTable();
                //    sdr = cmd.ExecuteReader();
                //    dt.Load(sdr);
                //    if (dt.Rows.Count > 0) { bAns = true; }
                //    dt.Dispose();
                //    sdr.Close();

                //    if (bAns == false)
                //    {
                //        sSql = "Insert into PaymentSchedule (TypeId,CostCentreId,SchType,Description,SchDate) " +
                //                                      "Values(" + argPayTypeId + "," + ccid + ",'Q','Qualifier',null) SELECT SCOPE_IDENTITY();";
                //        cmd = new SqlCommand(sSql, conn, tran);
                //        iTempId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                //        cmd.Dispose();
                //    }
                //    sSql = "Insert into CCReceiptType(TemplateId,Percentage,SchType) Values(" + iTempId + ",0,'Q')";
                //    cmd = new SqlCommand(sSql, conn, tran);
                //    cmd.ExecuteNonQuery();
                //    cmd.Dispose();

                //    sSql = "Select TemplateId from PaymentSchedule Where SchType<>'Q' and TypeId= " + argPayTypeId + " and CostCentreId = " + ccid + " Order by SortOrder,SchDate";
                //    cmd = new SqlCommand(sSql, conn, tran);
                //    dt = new DataTable();
                //    sdr = cmd.ExecuteReader();
                //    dt.Load(sdr);


                //    int iSortOrder = dt.Rows.Count;

                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        iTempId = Convert.ToInt32(dt.Rows[i]["TemplateId"].ToString());
                //        iSortOrder = i + 2;

                //        sSql = "Update PaymentSchedule Set SortOrder = " + iSortOrder + " Where TemplateId = " + iTempId;
                //        cmd = new SqlCommand(sSql, conn, tran);
                //        cmd.ExecuteNonQuery();
                //        cmd.Dispose();
                //    }
                //    dt.Dispose();
                //    sdr.Close();
                //}


                sSql = "Insert Into dbo.ReceiptTypeOrder(PayTypeId,CostCentreId,OtherCostId,SchType) " +
                       "Select " + argPayTypeId + "," + ccid + ",OtherCostId,'O' From dbo.OtherCostSetupTrans Where OtherCostId Not in  " +
                       "(Select OtherCostId From dbo.ReceiptTypeOrder Where SchType='O' and PayTypeId=" + argPayTypeId + " and CostCentreId=" + ccid + ") " +
                       "and PayTypeId=" + argPayTypeId + " and CostCentreId=" + ccid;
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();


                sSql = "Delete From dbo.ReceiptTypeOrder Where OtherCostId " +
                      "Not In (Select OtherCostId From dbo.OtherCostSetupTrans Where PayTypeId=" + argPayTypeId + " and CostCentreId=" + ccid + ") " +
                      "and PayTypeId=" + argPayTypeId + " and CostCentreId=" + ccid + " and SchType='O'";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();


                bool bFound = false;
                sSql = "Select * From dbo.ReceiptTypeOrder Where SchType='A' and CostCentreId=" + ccid + " and PayTypeId=" + argPayTypeId;
                cmd = new SqlCommand(sSql, conn, tran);
                dt = new DataTable();
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                if (dt.Rows.Count > 0) { bFound = true; }
                dt.Dispose();
                sdr.Close();

                if (argAdvance == true && bFound == true)
                {
                    sSql = "Delete From dbo.ReceiptTypeOrder Where SchType='A' and CostCentreId=" + ccid + " and PayTypeId=" + argPayTypeId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                if (argAdvance == false && bFound == false)
                {
                    sSql = "Insert Into dbo.ReceiptTypeOrder(PayTypeId,CostCentreId,ReceiptTypeId,SchType) " +
                           "Values(" + argPayTypeId + "," + ccid + ",1,'A')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                if (argTypewise == 0)
                    sSql = "Update PaySchType Set Typewise='" + true + "' Where TypeId=" + argPayTypeId + "";
                else
                    sSql = "Update PaySchType Set Typewise='" + false + "' Where TypeId=" + argPayTypeId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                //modified
                
                //bool bQFound = false;
                //sSql = "Select * from ReceiptTypeOrder Where SchType='Q' and CostCentreId=" + ccid + " and PayTypeId=" + argPayTypeId;
                //cmd = new SqlCommand(sSql, conn, tran);
                //dt = new DataTable();
                //sdr = cmd.ExecuteReader();
                //dt.Load(sdr);
                //if (dt.Rows.Count > 0) { bQFound = true; }
                //dt.Dispose();
                //sdr.Close();

                //if (argAdvance == true && bQFound == true)
                //{
                //    sSql = "Delete from ReceiptTypeOrder Where SchType='Q' and CostCentreId=" + ccid + " and PayTypeId=" + argPayTypeId;
                //    cmd = new SqlCommand(sSql, conn, tran);
                //    cmd.ExecuteNonQuery();
                //    cmd.Dispose();
                //}

                //if (argAdvance == false && bQFound == false)
                //{
                //    sSql = "Insert into ReceiptTypeOrder(PayTypeId,CostCentreId,ReceiptTypeId,SchType) " +
                //           "Values(" + argPayTypeId + "," + ccid + ",6,'Q')";
                //    cmd = new SqlCommand(sSql, conn, tran);
                //    cmd.ExecuteNonQuery();
                //    cmd.Dispose();
                //}

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

        public static void InsertOCOption(DataTable argDt, int ccid, int argPayTypeId, int argRow)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();

            string sSql = "";
            try
            {

                int iTempId = 0;
                string sName="";
                int iSortOrder = 0;
                int iOCId=0;

                for (int i = 0; i < argDt.Rows.Count; i++)
                {
                    iOCId = Convert.ToInt32(argDt.Rows[i]["OtherCostId"].ToString());
                    sName = argDt.Rows[i]["OtherCostName"].ToString();
                    iSortOrder = argRow + i + 1;

                    sSql = "Insert Into dbo.PaymentSchedule (TypeId,CostCentreId,SchType,Description,SortOrder,OtherCostId,SchDate) " +
                           "Values(" + argPayTypeId + "," + ccid + ",'O','" + sName + "'," + iSortOrder + "," + iOCId + ",null) SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(sSql, conn, tran);
                    iTempId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();

                    sSql = "Insert Into dbo.CCReceiptType(TemplateId,Percentage,SchType,OtherCostId) Values(" + iTempId + ",100,'O'," + iOCId + ")";
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

        public static void DeleteOCSetup(int ccid)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "DELETE FROM dbo.OtherCostSetupTrans WHERE CostCentreId=" + ccid + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                }
                catch (Exception e)
                {
                    BsfGlobal.CustomException(e.Message, e.StackTrace);
                }
                finally
                {
                    conn.Dispose();
                    conn.Close();
                }
            }
        }

        public static bool GetOCAdv(int argCCId)
        {
            DataTable dt; bool bOCId=false;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "SELECT OtherCostId FROM dbo.OtherCostSetupTrans WHERE CostCentreId=" + argCCId + "";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dt.Rows[0]["OtherCostId"]) == -1)
                        {
                            bOCId = true;
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
            return bOCId;
        }

        public static DataTable GetFTQuali(int argFlatTypeId)
        {
            DataTable dt = null;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "SELECT OtherCostId,QualiId,Expression,QualiAmt,Flag FROM dbo.FlatTypeQualifier WHERE FlatTypeId=" + argFlatTypeId + "";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        public static DataTable GetFDQuali(int argFlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "SELECT OtherCostId,QualiId,Expression,QualiAmt,Flag FROM dbo.FlatQualifier WHERE FlatId=" + argFlatId + "";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        public static void InsertFlatTypeOC(DataTable dtOC, int argFTId,DataTable dtOA,DataTable dtOI,DataTable dtTax)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "DELETE FROM dbo.FlatTypeOtherCost WHERE FlatTypeId=" + argFTId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                  
                    for (int a = 0; a < dtOC.Rows.Count; a++)
                    {
                        sSql = "INSERT INTO dbo.FlatTypeOtherCost(FlatTypeId,OtherCostId,Flag,Amount) Values" +
                            "(" + argFTId + "," + dtOC.Rows[a]["OtherCostId"] + "," +
                            "'" + dtOC.Rows[a]["Flag"] + "'," + dtOC.Rows[a]["Amount"] + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    sSql = "DELETE FROM dbo.FlatTypeOtherArea WHERE FlatTypeId=" + argFTId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    for (int a = 0; a < dtOA.Rows.Count; a++)
                    {
                        sSql = "INSERT INTO dbo.FlatTypeOtherArea(FlatTypeId,OtherCostId,Area,Unit,Rate,Amount) Values" +
                            "(" + argFTId + "," + dtOA.Rows[a]["OtherCostId"] + "," + dtOA.Rows[a]["Area"] + "," +
                            "" + dtOA.Rows[a]["Unit"] + "," + dtOA.Rows[a]["Rate"] + "," + dtOA.Rows[a]["Amount"] + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    sSql = "DELETE FROM dbo.FlatTypeOtherInfra WHERE FlatTypeId=" + argFTId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    for (int a = 0; a < dtOI.Rows.Count; a++)
                    {
                        sSql = "INSERT INTO dbo.FlatTypeOtherInfra(FlatTypeId,OtherCostId,AmountType,[Percent],Amount) Values" +
                            "(" + argFTId + "," + dtOI.Rows[a]["OtherCostId"] + ",'" + dtOI.Rows[a]["AmountType"] + "'," +
                            "" + dtOI.Rows[a]["Percent"] + "," + dtOI.Rows[a]["Amount"] + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    sSql = "DELETE FROM dbo.FlatTypeTax WHERE FlatTypeId=" + argFTId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    for (int a = 0; a < dtTax.Rows.Count; a++)
                    {
                        sSql = "INSERT INTO dbo.FlatTypeTax(FlatTypeId,QualifierId,Amount) Values" +
                            "(" + argFTId + "," + dtTax.Rows[a]["QualifierId"] + "," + Convert.ToDecimal(CommFun.IsNullCheck(dtTax.Rows[a]["Amount"], CommFun.datatypes.vartypenumeric)) + ")";
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
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static void InsertFlatOC(DataTable dtOC, int argFId,DataTable dtOA,DataTable dtOI,DataTable dtTax)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "DELETE FROM dbo.FlatOtherCost WHERE FlatId=" + argFId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    
                    for (int a = 0; a < dtOC.Rows.Count; a++)
                    {
                        sSql = "INSERT INTO dbo.FlatOtherCost(FlatId,OtherCostId,Flag,Amount) Values" +
                            " (" + argFId + "," + dtOC.Rows[a]["OtherCostId"] + "," +
                            " '" + dtOC.Rows[a]["Flag"] + "'," + dtOC.Rows[a]["Amount"] + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    sSql = "DELETE FROM dbo.FlatOtherArea WHERE FlatId=" + argFId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    for (int a = 0; a < dtOA.Rows.Count; a++)
                    {
                        sSql = "INSERT INTO dbo.FlatOtherArea(FlatId,OtherCostId,Area,Unit,Rate,Amount) Values" +
                            " (" + argFId + "," + dtOA.Rows[a]["OtherCostId"] + "," + dtOA.Rows[a]["Area"] + "," +
                            " " + dtOA.Rows[a]["Unit"] + "," + dtOA.Rows[a]["Rate"] + "," + dtOA.Rows[a]["Amount"] + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    sSql = "DELETE FROM dbo.FlatOtherInfra WHERE FlatId=" + argFId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    for (int a = 0; a < dtOI.Rows.Count; a++)
                    {
                        sSql = "INSERT INTO dbo.FlatOtherInfra(FlatId,OtherCostId,AmountType,[Percent],Amount) Values" +
                            " (" + argFId + "," + dtOI.Rows[a]["OtherCostId"] + ",'" + dtOI.Rows[a]["AmountType"] + "'," +
                            " " + dtOI.Rows[a]["Percent"] + "," + dtOI.Rows[a]["Amount"] + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    sSql = "DELETE FROM dbo.FlatTax WHERE FlatId=" + argFId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    for (int a = 0; a < dtTax.Rows.Count; a++)
                    {
                        sSql = "INSERT INTO dbo.FlatTax(FlatId,QualifierId,Amount) Values" +
                            " (" + argFId + "," + dtTax.Rows[a]["QualifierId"] + "," + Convert.ToDecimal(CommFun.IsNullCheck(dtTax.Rows[a]["Amount"], CommFun.datatypes.vartypenumeric)) + ")";
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
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static void InsertGlobalOC(DataTable dtFlat, DataTable dtOC, int argCCId, DataTable dtOA, DataTable dtOI, DataTable dtTax, decimal argAmt)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            int iFlatId = 0; decimal dOCAmt = 0;
            decimal dBaseAmt = 0, dNetAmt = 0,dQualAmt=0;
            bool bPayTypewise = false; int iPayTypeId = 0;
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    if (dtFlat != null)
                    {
                        for (int i = 0; i < dtFlat.Rows.Count; i++)
                        {                            
                            iFlatId = Convert.ToInt32(dtFlat.Rows[i]["FlatId"]);

                            //sSql = "Select A.*,B.Typewise From dbo.FlatDetails A Inner Join PaySchType B On A.PayTypeId=B.TypeId " +
                            //    " Where FlatId Not In(SELECT Distinct FlatId FROM dbo.PaymentScheduleFlat " +
                            //    " WHERE CostCentreId=" + argCCId + " And (BillPassed=1 Or PaidAmount>0)) And A.CostCentreId=" + argCCId + " ";
                            sSql = "Select A.*,B.Typewise,A.PayTypeId From dbo.FlatDetails A Inner Join PaySchType B On A.PayTypeId=B.TypeId " +
                                " Where FlatId=" + iFlatId + " And A.CostCentreId=" + argCCId + " ";
                            cmd = new SqlCommand(sSql, conn, tran);
                            SqlDataReader dr = cmd.ExecuteReader();
                            DataTable dt = new DataTable();
                            dt.Load(dr);
                            cmd.Dispose();

                            if (dt.Rows.Count > 0)
                            {
                                sSql = "DELETE FROM dbo.FlatOtherCost WHERE FlatId=" + iFlatId;
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                for (int a = 0; a < dtOC.Rows.Count; a++)
                                {
                                    sSql = "INSERT INTO dbo.FlatOtherCost(FlatId,OtherCostId,Flag,Amount) Values" +
                                        " (" + iFlatId + "," + dtOC.Rows[a]["OtherCostId"] + "," +
                                        " '" + dtOC.Rows[a]["Flag"] + "'," + dtOC.Rows[a]["Amount"] + ")";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }

                                sSql = "DELETE FROM dbo.FlatOtherArea WHERE FlatId=" + iFlatId;
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                for (int a = 0; a < dtOA.Rows.Count; a++)
                                {
                                    sSql = "INSERT INTO dbo.FlatOtherArea(FlatId,OtherCostId,Area,Unit,Rate,Amount) Values" +
                                        " (" + iFlatId + "," + dtOA.Rows[a]["OtherCostId"] + "," + dtOA.Rows[a]["Area"] + "," +
                                        " " + dtOA.Rows[a]["Unit"] + "," + dtOA.Rows[a]["Rate"] + "," + dtOA.Rows[a]["Amount"] + ")";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }

                                sSql = "DELETE FROM dbo.FlatOtherInfra WHERE FlatId=" + iFlatId;
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                for (int a = 0; a < dtOI.Rows.Count; a++)
                                {
                                    sSql = "INSERT INTO dbo.FlatOtherInfra(FlatId,OtherCostId,AmountType,[Percent],Amount) Values" +
                                        " (" + iFlatId + "," + dtOI.Rows[a]["OtherCostId"] + ",'" + dtOI.Rows[a]["AmountType"] + "'," +
                                        " " + dtOI.Rows[a]["Percent"] + "," + dtOI.Rows[a]["Amount"] + ")";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }

                                sSql = "DELETE FROM dbo.FlatTax WHERE FlatId=" + iFlatId;
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                for (int a = 0; a < dtTax.Rows.Count; a++)
                                {
                                    sSql = "INSERT INTO dbo.FlatTax(FlatId,QualifierId,Amount) Values" +
                                        " (" + iFlatId + "," + dtTax.Rows[a]["QualifierId"] + "," + dtTax.Rows[a]["Amount"] + ")";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }

                                iPayTypeId = Convert.ToInt32(dt.Rows[0]["PayTypeId"]);

                                decimal dOtherAmt = 0;
                                //sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                                //        "Where FlatId = " + iFlatId + " and OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans " +
                                //        " Where PayTypeId=" + iPayTypeId + " and CostCentreId=" + argCCId + ")";
                                sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                                        " Where FlatId =" + iFlatId + " and OtherCostId not in (Select OtherCostId from dbo.OXGross " +
                                        " Where CostCentreId=" + argCCId + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                dr = cmd.ExecuteReader();
                                DataTable dtOCost = new DataTable();
                                dtOCost.Load(dr);
                                dr.Close();
                                cmd.Dispose();

                                if (dtOCost.Rows.Count > 0) { dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtOCost.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }

                                dOCAmt = argAmt;
                                dBaseAmt = Convert.ToDecimal(dt.Rows[0]["BaseAmt"]);
                                dNetAmt = dBaseAmt + dOtherAmt;
                                dQualAmt = Convert.ToDecimal(dt.Rows[0]["QualifierAmt"]);
                                bPayTypewise = Convert.ToBoolean(dt.Rows[0]["Typewise"]);

                                sSql = "Update dbo.FlatDetails Set OtherCostAmt=" + dOCAmt + ",NetAmt=" + dNetAmt + ",QualifierAmt=" + dQualAmt + " " +
                                       " Where FlatId=" + iFlatId + "";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                PaymentScheduleBL.InsertFlatScheduleI(iFlatId, conn, tran);

                                dQualAmt = UnitDirBL.GetGlobalQualifierAmt(iFlatId, bPayTypewise, conn, tran);

                                sSql = "Update dbo.FlatDetails Set QualifierAmt=" + dQualAmt + " " +
                                    " Where FlatId=" + iFlatId + "";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }
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
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static void UpdateUnitOC(int argFlatId, DataTable dtOC, int argCCId, DataTable dtOA, DataTable dtOI, DataTable dtTax, decimal argAmt)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            int iFlatId = 0; decimal dOCAmt = 0;
            decimal dBaseAmt = 0, dNetAmt = 0, dQualAmt = 0;
            bool bPayTypewise = false; int iPayTypeId = 0;
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    iFlatId = argFlatId;

                    sSql = "Select A.*,B.Typewise,A.PayTypeId From dbo.FlatDetails A Inner Join PaySchType B On A.PayTypeId=B.TypeId " +
                        " Where FlatId=" + iFlatId + " And A.CostCentreId=" + argCCId + " ";
                    cmd = new SqlCommand(sSql, conn, tran);
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    cmd.Dispose();

                    if (dt.Rows.Count > 0)
                    {
                        sSql = "DELETE FROM dbo.FlatOtherCost WHERE FlatId=" + iFlatId;
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        for (int a = 0; a < dtOC.Rows.Count; a++)
                        {
                            sSql = "INSERT INTO dbo.FlatOtherCost(FlatId,OtherCostId,Flag,Amount) Values" +
                                " (" + iFlatId + "," + dtOC.Rows[a]["OtherCostId"] + "," +
                                " '" + dtOC.Rows[a]["Flag"] + "'," + dtOC.Rows[a]["Amount"] + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }

                        sSql = "DELETE FROM dbo.FlatOtherArea WHERE FlatId=" + iFlatId;
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        for (int a = 0; a < dtOA.Rows.Count; a++)
                        {
                            sSql = "INSERT INTO dbo.FlatOtherArea(FlatId,OtherCostId,Area,Unit,Rate,Amount) Values" +
                                " (" + iFlatId + "," + dtOA.Rows[a]["OtherCostId"] + "," + dtOA.Rows[a]["Area"] + "," +
                                " " + dtOA.Rows[a]["Unit"] + "," + dtOA.Rows[a]["Rate"] + "," + dtOA.Rows[a]["Amount"] + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }

                        sSql = "DELETE FROM dbo.FlatOtherInfra WHERE FlatId=" + iFlatId;
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        for (int a = 0; a < dtOI.Rows.Count; a++)
                        {
                            sSql = "INSERT INTO dbo.FlatOtherInfra(FlatId,OtherCostId,AmountType,[Percent],Amount) Values" +
                                " (" + iFlatId + "," + dtOI.Rows[a]["OtherCostId"] + ",'" + dtOI.Rows[a]["AmountType"] + "'," +
                                " " + dtOI.Rows[a]["Percent"] + "," + dtOI.Rows[a]["Amount"] + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }

                        sSql = "DELETE FROM dbo.FlatTax WHERE FlatId=" + iFlatId;
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        for (int a = 0; a < dtTax.Rows.Count; a++)
                        {
                            sSql = "INSERT INTO dbo.FlatTax(FlatId,QualifierId,Amount) Values" +
                                " (" + iFlatId + "," + dtTax.Rows[a]["QualifierId"] + "," + dtTax.Rows[a]["Amount"] + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }

                        iPayTypeId = Convert.ToInt32(dt.Rows[0]["PayTypeId"]);

                        decimal dOtherAmt = 0;
                        //sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount FROM dbo.FlatOtherCost " +
                        //        "Where FlatId = " + iFlatId + " and OtherCostId in (Select OtherCostId FROM dbo.OtherCostSetupTrans " +
                        //        " Where PayTypeId=" + iPayTypeId + " and CostCentreId=" + argCCId + ")";
                        sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                                " Where FlatId =" + iFlatId + " and OtherCostId not in (Select OtherCostId from dbo.OXGross " +
                                " Where CostCentreId=" + argCCId + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        dr = cmd.ExecuteReader();
                        DataTable dtOCost = new DataTable();
                        dtOCost.Load(dr);
                        dr.Close();
                        cmd.Dispose();

                        if (dtOCost.Rows.Count > 0) { dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtOCost.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }

                        dOCAmt = argAmt;
                        dBaseAmt = Convert.ToDecimal(dt.Rows[0]["BaseAmt"]);
                        dNetAmt = dBaseAmt + dOtherAmt;
                        dQualAmt = Convert.ToDecimal(dt.Rows[0]["QualifierAmt"]);
                        bPayTypewise = Convert.ToBoolean(dt.Rows[0]["Typewise"]);

                        sSql = "Update dbo.FlatDetails Set OtherCostAmt=" + dOCAmt + ",NetAmt=" + dNetAmt + ",QualifierAmt=" + dQualAmt + " " +
                            " Where FlatId=" + iFlatId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Select PaymentSchId,FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate," +
                                " DateAfter, Duration,DurationType,SchPercent,Amount,NetAmount,PreStageTypeId,SortOrder,BillPassed,PaidAmount,StageDetId From dbo.PaymentScheduleFlat " +
                                " Where FlatId=" + iFlatId + " And TemplateId<>0 Order By SortOrder";
                        cmd = new SqlCommand(sSql, conn, tran);
                        dr = cmd.ExecuteReader();
                        DataTable dtP = new DataTable();
                        dtP.Load(dr);
                        cmd.Dispose();

                        if (bPayTypewise == true)
                            PaymentScheduleDL.UpdateReceiptBuyerSchedule(iFlatId, dtP, conn, tran);
                        else PaymentScheduleDL.UpdateReceiptBuyerScheduleQual(iFlatId, dtP, conn, tran);
                        //PaymentScheduleBL.InsertFlatScheduleI(iFlatId, conn, tran);

                        dQualAmt = UnitDirBL.GetGlobalQualifierAmt(iFlatId, bPayTypewise, conn, tran);

                        sSql = "Update dbo.FlatDetails Set QualifierAmt=" + dQualAmt + " " +
                            " Where FlatId=" + iFlatId + "";
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
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static DataTable GetFlatTypeOtherArea(int argFlatTypeId,int argOCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "SELECT * FROM dbo.FlatTypeOtherArea WHERE FlatTypeId=" + argFlatTypeId + " And OtherCostId=" + argOCId + "";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        public static DataTable GetFlatOtherArea(int argFlatId, int argOCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "SELECT * FROM dbo.FlatOtherArea WHERE FlatId=" + argFlatId + " And OtherCostId=" + argOCId + "";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        public static DataTable GetFTOtherArea(int argFlatTypeId)
        {
            DataTable dt = null;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "SELECT 0 FlatId,* FROM dbo.FlatTypeOtherArea WHERE FlatTypeId=" + argFlatTypeId + " ";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        public static DataTable GetFOtherArea(int argFlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "SELECT 0 FlatTypeId,* FROM dbo.FlatOtherArea WHERE FlatId=" + argFlatId + " ";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        public static DataTable GetGlobalOtherArea(int argFlatTypeId)
        {
            DataTable dt = null;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "SELECT 0 FlatId,0 FlatTypeId,* FROM dbo.FlatTypeOtherArea WHERE FlatTypeId=" + argFlatTypeId + " ";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        public static bool GetOCStatus(int argFlatId)
        {
            DataTable dt; bool bAns = false;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "SELECT FlatId FROM dbo.PaymentScheduleFlat WHERE FlatId=" + argFlatId + " And (BillPassed=1 Or PaidAmount>0) " +
                        " Select FlatId From ReceiptTrans Where FlatId=" + argFlatId + " " +
                        " UNION ALL " +
                        " Select FlatId From ProgressBillRegister Where FlatId=" + argFlatId + "";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        bAns = true;
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
            return bAns;
        }

        #endregion

        #region Other Infra

        public static DataTable GetOtherInfraAmount(int argFlatTypeId,int argFlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                if (argFlatId == 0)
                    sql = "SELECT BaseAmt,NetAmt FROM dbo.FlatType WHERE FlatTypeId=" + argFlatTypeId + " ";
                else
                    sql = "SELECT BaseAmt,NetAmt FROM dbo.FlatDetails WHERE FlatId=" + argFlatId + " ";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        public static DataTable GetFlatTypeOtherInfra(int argFlatTypeId, int argOCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "SELECT * FROM dbo.FlatTypeOtherArea WHERE FlatTypeId=" + argFlatTypeId + " And OtherCostId=" + argOCId + "";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        public static DataTable GetFlatOtherInfra(int argFlatId, int argOCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "SELECT * FROM dbo.FlatOtherArea WHERE FlatId=" + argFlatId + " And OtherCostId=" + argOCId + "";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        public static DataTable GetFTOtherInfra(int argFlatTypeId)
        {
            DataTable dt = null;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "SELECT 0 FlatId,* FROM dbo.FlatTypeOtherInfra WHERE FlatTypeId=" + argFlatTypeId + " ";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        public static DataTable GetFOtherInfra(int argFlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "SELECT 0 FlatTypeId,* FROM dbo.FlatOtherInfra WHERE FlatId=" + argFlatId + " ";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        public static DataTable GetGlobalOtherInfra(int argFlatTypeId)
        {
            DataTable dt = null;
            SqlDataAdapter sda = null;
            string sql = "";
            try
            {
                sql = "SELECT 0 FlatId,* FROM dbo.FlatTypeOtherInfra WHERE FlatTypeId=" + argFlatTypeId + " ";
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        #endregion

    }
}
