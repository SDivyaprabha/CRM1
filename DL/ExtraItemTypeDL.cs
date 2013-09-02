using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessObjects;
using Microsoft.VisualBasic;

namespace CRM.DataLayer
{
    class ExtraItemTypeDL
    {
        public static DataTable GetExtraItem()
        {
            DataTable dtService=null;
            SqlDataAdapter sda;
            string sSql = "";
            
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "Select ExtraItemTypeId,ExtraItemTypeName from dbo.ExtraItemTypeMaster";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dtService = new DataTable();
                sda.Fill(dtService);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtService;
        }

        public static DataTable GetExtraItemProject(int argCCID)
        {
            DataTable dt = new DataTable();
            BsfGlobal.OpenCRMDB();
            try
            {
                string sSql = "Select A.ExtraItemId,B.ItemCode,B.ItemDescription,C.ExtraItemTypeName,D.Unit_Name Unit,A.Rate from dbo.CCExtraItems  A " +
                              "Inner Join dbo.ExtraItemMaster B on A.ExtraItemId=B.ExtraItemId " +
                              "Inner Join dbo.ExtraItemTypeMaster C on B.ExtraItemTypeId=C.ExtraItemTypeId " +
                              "Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.UOM D on B.UnitId=D.Unit_Id " +
                              "Where A.CostCentreId= " + argCCID;
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;                
        }

        public static DataTable GetUnit()
        {
            DataTable dt = new DataTable();
            try
            {
                string sSql = "Select Unit_ID,Unit_Name from dbo.UOM Order by Unit_Name";
                BsfGlobal.OpenRateAnalDB();

                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_RateAnalDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_RateAnalDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable GetExtraItemFlatType(int argCCID,int argFlatTypeId)
        {
            DataTable dt = new DataTable();
            try
            {
                BsfGlobal.OpenCRMDB();
                //string sSql = "Select A.ExtraItemId,B.ItemCode,B.ItemDescription,C.ExtraItemTypeName,D.Unit_Name Unit,A.ExtraRate Rate " +
                //            " from dbo.FlatTypeExtraItem A INNER JOIN dbo.CCExtraItems E ON E.ExtraItemId=A.ExtraItemId" +
                //            " Inner Join dbo.ExtraItemMaster B on A.ExtraItemId=B.ExtraItemId Inner Join " +
                //            " dbo.ExtraItemTypeMaster C on B.ExtraItemTypeId=C.ExtraItemTypeId " +
                //            " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.UOM D on B.UnitId=D.Unit_Id " +
                //            " Where E.CostCentreId= " + argCCID + " And A.FlatTypeId=" + argFlatTypeId + "";

                string sSql = "Select A.ExtraItemId,B.ItemCode,B.ItemDescription,C.ExtraItemTypeName," +
                                " D.Unit_Name Unit,A.ExtraRate Rate,A.Qty,A.Amount,A.NetAmount From FlatTypeExtraItem A" +
                                " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ExtraItemMaster B On A.ExtraItemId=B.ExtraItemId " +
                                " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ExtraItemTypeMaster C On C.ExtraItemTypeId=B.ExtraItemTypeId" +
                                " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.UOM D On D.Unit_ID=B.UnitId Where FlatTypeId=" + argFlatTypeId + "";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable GetExtraItemFlat(int argCCID, int argFlatId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Select A.FlatExtraItemId,A.ExtraItemId,B.ItemCode,B.ItemDescription,C.ExtraItemTypeName,D.Unit_Name Unit," +
                                " A.Rate,A.Quantity Qty,A.Amount,A.NetAmount,A.UpdateFrom,A.Sel,A.Approve From FlatExtraItem A" +
                                " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ExtraItemMaster B On A.ExtraItemId=B.ExtraItemId " +
                                " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ExtraItemTypeMaster C On C.ExtraItemTypeId=B.ExtraItemTypeId" +
                                " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.UOM D On D.Unit_ID=B.UnitId " +
                                " Where FlatId=" + argFlatId + "";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();

                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable GetExtraItemFlatQualifier(int argFlatId, string argType)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "";
                if (argType == "FlatType")
                {
                    sSql = "Select DISTINCT TransId, FlatTypeId, ExtraItemId, QualifierId, Expression, ExpPer, Add_Less_Flag, SurCharge, EDCess, ExpValue, ExpPerValue, " +
                           " SurValue, EDValue, Amount, HEDPer, HEDValue, NetPer, TaxablePer, TaxableValue from dbo.FlatTypeExtraItemQualifier "+
                           " Where FlatTypeId=" + argFlatId + " ";
                }
                else
                {
                    sSql = "Select DISTINCT TransId, FlatId, ExtraItemId, QualifierId, Expression, ExpPer, Add_Less_Flag, SurCharge, EDCess, ExpValue, ExpPerValue, " +
                           " SurValue, EDValue, Amount, HEDPer, HEDValue, NetPer, TaxablePer, TaxableValue from dbo.FlatExtraItemQualifier " +
                           " Where FlatId=" + argFlatId + " ORDER BY QualifierId ";
                }
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();

                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static void InsertExtraItemProjects(DataTable dt,int argCCID)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd; string sSql="";
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {

                try
                {
                    sSql = "Delete from dbo.CCExtraItems Where CostCentreId= " + argCCID;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sSql = "INSERT INTO dbo.CCExtraItems(ExtraItemId,CostCentreId,Rate) VALUES" +
                            " (" + dt.Rows[i]["ExtraItemId"] + "," + argCCID + "," + dt.Rows[i]["Rate"] + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                    tran.Rollback();
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

        }

        public static void InsertExtraItemFlatType(DataTable dt, int argFlatTypeId, DataTable argdtQualifier)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd; string sSql = "";
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    sSql = "Delete from dbo.FlatTypeExtraItem Where FlatTypeId= " + argFlatTypeId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Delete from dbo.FlatTypeExtraItemQualifier Where FlatTypeId= " + argFlatTypeId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sSql = "INSERT INTO dbo.FlatTypeExtraItem(FlatTypeId,ExtraItemId,ExtraRate,Qty,Amount,NetAmount) VALUES" +
                            " (" + argFlatTypeId + "," + dt.Rows[i]["ExtraItemId"] + "," + dt.Rows[i]["Rate"] + "," +
                        " " + dt.Rows[i]["Qty"] + "," + dt.Rows[i]["Amount"] + "," + dt.Rows[i]["NetAmount"] + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        if (argdtQualifier != null && argdtQualifier.Rows.Count > 0)
                        {
                            DataRow[] drow = argdtQualifier.Select("ExtraItemId=" + dt.Rows[i]["ExtraItemId"] + "");
                            for (int j = 0; j <= drow.Length - 1; j++)
                            {
                                sSql = "Insert into dbo.FlatTypeExtraItemQualifier(FlatTypeId,ExtraItemId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge," +
                                       "EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount,HEDPer,HEDValue,NetPer,TaxableValue,TaxablePer) " +
                                        "Values(" + argFlatTypeId + "," + Convert.ToInt32(CommFun.IsNullCheck(drow[j]["ExtraItemId"], CommFun.datatypes.vartypenumeric)) +
                                        "," + Convert.ToInt32(CommFun.IsNullCheck(drow[j]["QualifierId"], CommFun.datatypes.vartypenumeric)) +
                                        ",'" + CommFun.IsNullCheck(drow[j]["Expression"], CommFun.datatypes.vartypestring).ToString() +
                                        "'," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["ExpPer"], CommFun.datatypes.vartypenumeric)) +
                                        ",'" + CommFun.IsNullCheck(drow[j]["Add_Less_Flag"], CommFun.datatypes.vartypestring).ToString() +
                                        "'," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["SurCharge"], CommFun.datatypes.vartypenumeric)) +
                                        "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["EDCess"], CommFun.datatypes.vartypenumeric)) +
                                        "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["ExpValue"], CommFun.datatypes.vartypenumeric)) +
                                        "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["ExpPerValue"], CommFun.datatypes.vartypenumeric)) +
                                        "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["SurValue"], CommFun.datatypes.vartypenumeric)) +
                                        "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["EDValue"], CommFun.datatypes.vartypenumeric)) +
                                        "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["Amount"], CommFun.datatypes.vartypenumeric)) +
                                        "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["HEDPer"], CommFun.datatypes.vartypenumeric)) +
                                        "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["HEDValue"], CommFun.datatypes.vartypenumeric)) +
                                        "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["NetPer"], CommFun.datatypes.vartypenumeric)) +
                                        "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["TaxableValue"], CommFun.datatypes.vartypenumeric)) +
                                        "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["TaxablePer"], CommFun.datatypes.vartypenumeric)) + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                    tran.Rollback();
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

        }

        public static void InsertExtraItemFlat(DataTable dt, int argFlatId, DataTable argdtQualifier, int argCCId)
        {
            bool bInsert = false;
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            int iFlatExtraItemId = 0;
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    string sSql = "";
                    SqlCommand cmd = null;
                    decimal dNetAmt = 0;
                    string sLog = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        iFlatExtraItemId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["FlatExtraItemId"], CommFun.datatypes.vartypenumeric));

                        dNetAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["NetAmount"], CommFun.datatypes.vartypenumeric));

                        if (iFlatExtraItemId == 0)
                        {
                            sSql = "INSERT INTO dbo.FlatExtraItem(FlatId,ExtraItemId,Quantity,Rate,Amount,NetAmount,Updatefrom,Sel,Approve) VALUES" +
                                    " (" + argFlatId + "," + dt.Rows[i]["ExtraItemId"] + "," + dt.Rows[i]["Qty"] + "," + dt.Rows[i]["Rate"] + "," +
                                    " " + dt.Rows[i]["Amount"] + "," + dNetAmt + ",'" + dt.Rows[i]["Updatefrom"] +
                                    "','" + Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Sel"], CommFun.datatypes.varTypeBoolean)) +
                                    "','" + CommFun.IsNullCheck(dt.Rows[i]["Approve"], CommFun.datatypes.vartypestring).ToString() + "') SELECT SCOPE_IDENTITY();";
                            cmd = new SqlCommand(sSql, conn, tran);
                            iFlatExtraItemId = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                            cmd.Dispose();
                            sLog = "Y";
                        }
                        else
                        {
                            sSql = "Update dbo.FlatExtraItem Set FlatId=" + argFlatId + ",ExtraItemId=" + dt.Rows[i]["ExtraItemId"] + "," +
                                    "Quantity=" + dt.Rows[i]["Qty"] + ",Rate=" + dt.Rows[i]["Rate"] + ",Amount=" + dt.Rows[i]["Amount"] + "," +
                                    "NetAmount=" + dNetAmt + ",Updatefrom='" + dt.Rows[i]["Updatefrom"] + "',WebUpdate=0," +
                                    "Sel='" + Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Sel"], CommFun.datatypes.varTypeBoolean)) + "'," +
                                    "Approve='" + CommFun.IsNullCheck(dt.Rows[i]["Approve"], CommFun.datatypes.vartypestring).ToString() + "'" +
                                    " Where FlatExtraItemId=" + iFlatExtraItemId + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            if (sLog == "") { sLog = "N"; }
                        }

                        sSql = "Delete dbo.FlatExtraItemQualifier Where FlatId=" + argFlatId + " AND ExtraItemId=" + dt.Rows[i]["ExtraItemId"];
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        if (argdtQualifier != null && argdtQualifier.Rows.Count > 0)
                        {
                            DataRow[] drow = argdtQualifier.Select("ExtraItemId=" + dt.Rows[i]["ExtraItemId"] + "");
                            for (int j = 0; j <= drow.Length - 1; j++)
                            {
                                sSql = "Insert into dbo.FlatExtraItemQualifier(FlatId,ExtraItemId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge," +
                                       "EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount,HEDPer,HEDValue,NetPer,TaxableValue,TaxablePer) " +
                                                "Values(" + argFlatId + "," + Convert.ToInt32(CommFun.IsNullCheck(drow[j]["ExtraItemId"], CommFun.datatypes.vartypenumeric)) +
                                                "," + Convert.ToInt32(CommFun.IsNullCheck(drow[j]["QualifierId"], CommFun.datatypes.vartypenumeric)) +
                                                ",'" + CommFun.IsNullCheck(drow[j]["Expression"], CommFun.datatypes.vartypestring).ToString() +
                                                "'," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["ExpPer"], CommFun.datatypes.vartypenumeric)) +
                                                ",'" + CommFun.IsNullCheck(drow[j]["Add_Less_Flag"], CommFun.datatypes.vartypestring).ToString() +
                                                "'," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["SurCharge"], CommFun.datatypes.vartypenumeric)) +
                                                "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["EDCess"], CommFun.datatypes.vartypenumeric)) +
                                                "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["ExpValue"], CommFun.datatypes.vartypenumeric)) +
                                                "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["ExpPerValue"], CommFun.datatypes.vartypenumeric)) +
                                                "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["SurValue"], CommFun.datatypes.vartypenumeric)) +
                                                "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["EDValue"], CommFun.datatypes.vartypenumeric)) +
                                                "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["Amount"], CommFun.datatypes.vartypenumeric)) +
                                                "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["HEDPer"], CommFun.datatypes.vartypenumeric)) +
                                                "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["HEDValue"], CommFun.datatypes.vartypenumeric)) +
                                                "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["NetPer"], CommFun.datatypes.vartypenumeric)) +
                                                "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["TaxableValue"], CommFun.datatypes.vartypenumeric)) +
                                                "," + Convert.ToDecimal(CommFun.IsNullCheck(drow[j]["TaxablePer"], CommFun.datatypes.vartypenumeric)) + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }
                    }

                    //sSql = "Delete from dbo.FlatExtraItem Where FlatId=" + argFlatId + " AND FlatExtraItemId NOT IN(" + sFlatExtraItemId + ")";
                    //cmd = new SqlCommand(sSql, conn, tran);
                    //cmd.ExecuteNonQuery();
                    //cmd.Dispose();

                    //sSql = "Delete from dbo.FlatExtraItemQualifier Where FlatId=" + argFlatId + " AND FlatExtraItemId NOT IN(" + sFlatExtraItemId + ")";
                    //cmd = new SqlCommand(sSql, conn, tran);
                    //cmd.ExecuteNonQuery();
                    //cmd.Dispose();

                    sSql = "Select ISNULL(FlatNo,'') FlatNo from dbo.FlatDetails Where FlatId=" + argFlatId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    SqlDataReader dreader = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(dreader);
                    dreader.Close();
                    cmd.Dispose();

                    string sFlatNo = "";
                    if (dt.Rows.Count > 0) { sFlatNo = CommFun.IsNullCheck(dt.Rows[0]["FlatNo"], CommFun.datatypes.vartypestring).ToString(); }

                    tran.Commit();

                    bInsert = true;
                    if (bInsert == true && sLog == "Y")
                    {
                        BsfGlobal.InsertLog(DateTime.Now, "Extra Item-Add", "N", "Extra Item", argFlatId, argCCId, 0, BsfGlobal.g_sCRMDBName,
                                           sFlatNo, BsfGlobal.g_lUserId, dNetAmt, 0);
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    bInsert = false;
                    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

        }

        public static void InsertExtraItemDetails(ExtraItemTypeBO argExtraItemContactBO)
        {
            int iFTypeId = 0;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {

                try
                {
                    string sSql = String.Format("INSERT INTO dbo.ExtraItemMaster(ExtraItemTypeId,ItemCode,ItemDescription,UnitName,ExtraRate) VALUES({0}, '{1}', '{2}','{3}','{4}' ) SELECT SCOPE_IDENTITY();", ExtraItemTypeBO.ExtraItemTypeId, ExtraItemTypeBO.ItemCode, ExtraItemTypeBO.ItemDescription, ExtraItemTypeBO.UnitName, ExtraItemTypeBO.ExtraRate);
                    cmd = new SqlCommand(sSql, conn, tran);
                    iFTypeId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();      
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                    tran.Rollback();
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static void UpdateExtraItemDetails(ExtraItemTypeBO argExtraItemContactBO)
        {

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = String.Format("UPDATE dbo.ExtraItemMaster SET ExtraItemTypeId={0}, ItemCode='{1}',ItemDescription='{2}',UnitName='{3}', ExtraRate='{4}' "+
                                         " WHERE ExtraItemId={5}", ExtraItemTypeBO.ExtraItemTypeId, ExtraItemTypeBO.ItemCode, ExtraItemTypeBO.ItemDescription, 
                                         ExtraItemTypeBO.UnitName, ExtraItemTypeBO.ExtraRate, ExtraItemTypeBO.ExtraItemId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    tran.Commit();

                }
                catch (Exception ex)
                {
                    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static int GetLeadId(int argCCID, int argFlatId)
        {
            DataTable dt = new DataTable();
            int iLeadId = 0;

            try
            {
                BsfGlobal.OpenCRMDB();
                string sSql = "Select LeadId From LeadRegister A Inner Join FlatDetails B On A.CostCentreId=B.CostCentreId" +
                                " Where A.CostCentreId=" + argCCID + " And B.FlatId=" + argFlatId + "";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return iLeadId;
        }

        internal static DataTable GetQuotation(int argiCCId, int argiFlatId, int argiFlatTypeId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = new DataTable();
            try
            {
                if (argiFlatId == 0)
                {
                    string sSql = "Select CompanyName, Address1, Address2, MobileNo, Email, Website, ItemCode, ItemDescription, WorkingQty, Amount, NetAmount, "+
                                  " SUM(ServiceTax) ServiceTax from ( "+
                                  " Select ISNULL(I.CompanyName,'') CompanyName, ISNULL(I.Address1,'') Address1, ISNULL(I.Address2,'') Address2, " +
                                  " ISNULL(I.Mobile,'') MobileNo, ISNULL(I.Email,'') Email, ISNULL(I.Website,'') Website, ISNULL(C.ItemCode,'') ItemCode, " +
                                  " ISNULL(C.ItemDescription,'') ItemDescription, ISNULL(A.Qty,'') WorkingQty, ISNULL(A.Amount,0) Amount, " +
                                  " ISNULL(A.NetAmount,0) NetAmount, Case When F.QualTypeId=2 THEN SUM(ISNULL(E.Amount,0)) Else 0 End ServiceTax from dbo.FlatTypeExtraItem A " +
                                  " INNER JOIN dbo.FlatType B ON A.FlatTypeId=B.FlatTypeId " +
                                  " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ExtraItemMaster C ON A.ExtraItemId=C.ExtraItemId " +
                                  " LEFT JOIN dbo.FlatTypeExtraItemQualifier E ON A.FlatTypeId=E.FlatTypeId AND A.ExtraItemId=E.ExtraItemId " +
                                  " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp F ON E.QualifierId=F.QualifierId " +
                                  " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CostCentre H ON B.ProjId=H.CostCentreId " +
                                  " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CompanyMaster I ON H.CompanyId=I.CompanyId " +
                                  " Where A.FlatTypeId=" + argiFlatTypeId + "  " +
                                  " GROUP BY F.QualTypeId, C.ItemCode, C.ItemDescription, I.CompanyName, I.Address1, I.Address2, I.Mobile, I.Email, I.Website, " +
                                  " A.Qty, A.Amount, A.NetAmount) X"+
                                  " GROUP BY CompanyName, Address1, Address2, MobileNo, Email, Website, ItemCode, ItemDescription, WorkingQty, Amount, NetAmount";
                    SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    dt = new DataTable();
                    sda.Fill(dt);
                    sda.Dispose();
                }
                else
                {
                    string sSql = "Select CompanyName, Address1, Address2, MobileNo, Email, Website, ItemCode, ItemDescription, WorkingQty, Amount, NetAmount, " +
                                  " SUM(ServiceTax) ServiceTax, BuyerName, LAddress1, LAddress2, City, State from ( " + 
                                  " Select ISNULL(I.CompanyName,'') CompanyName, ISNULL(I.Address1,'') Address1, ISNULL(I.Address2,'') Address2, " +
                                  " ISNULL(I.Mobile,'') MobileNo, ISNULL(I.Email,'') Email, ISNULL(I.Website,'') Website, ISNULL(C.ItemCode,'') ItemCode, " +
                                  " ISNULL(C.ItemDescription,'') ItemDescription, ISNULL(A.Quantity,'') WorkingQty, ISNULL(A.Amount,0) Amount, " +
                                  " ISNULL(A.NetAmount,0) NetAmount, Case When F.QualTypeId=2 THEN SUM(ISNULL(E.Amount,0)) Else 0 End ServiceTax, "+
                                  " ISNULL(J.LeadName,'') BuyerName, ISNULL(K.Address1,'') LAddress1, ISNULL(K.Address2,'') LAddress2, ISNULL(L.CityName,'') City, "+
                                  " ISNULL(M.StateName,'') State from dbo.FlatExtraItem A " +
                                  " INNER JOIN dbo.FlatDetails B ON A.FlatId=B.FlatId " +
                                  " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ExtraItemMaster C ON A.ExtraItemId=C.ExtraItemId " +
                                  " LEFT JOIN dbo.FlatExtraItemQualifier E ON A.FlatId=E.FlatId AND A.ExtraItemId=E.ExtraItemId " +
                                  " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp F ON E.QualifierId=F.QualifierId " +
                                  " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CostCentre H ON B.CostCentreId=H.CostCentreId " +
                                  " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CompanyMaster I ON H.CompanyId=I.CompanyId " +
                                  " LEFT JOIN dbo.LeadRegister J ON B.LeadId=J.LeadId " +
                                  " LEFT JOIN dbo.LeadCommAddressInfo K ON J.LeadId=K.LeadId " +
                                  " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CityMaster L ON K.CityId=L.CityId " +
                                  " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.StateMaster M ON K.StateId=M.StateId " +
                                  " Where A.FlatId=" + argiFlatId + " "+
                                  " GROUP BY F.QualTypeId, C.ItemCode, C.ItemDescription, I.CompanyName, I.Address1, I.Address2, I.Mobile, I.Email, I.Website, " +
                                  " A.Quantity, A.Amount, A.NetAmount, J.LeadName, K.Address1, K.Address2, L.CityName, M.StateName) X"+
                                  " GROUP BY CompanyName, Address1, Address2, MobileNo, Email, Website, ItemCode, ItemDescription, WorkingQty, Amount, NetAmount, " +
                                  " BuyerName, LAddress1, LAddress2, City, State";
                    SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    dt = new DataTable();
                    sda.Fill(dt);
                    sda.Dispose();
                }
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }
    }
}
