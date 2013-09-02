using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualBasic;
using Qualifier;

namespace CRM.DataLayer
{
    class PaymentScheduleDL
    {
        #region Methods

        public static DataTable GetReceiptType()
        {
            SqlDataAdapter sda; DataTable dtReceiptType = null;
            string sSql = "SELECT R.ReceiptTypeId,R.ReceiptTypeName FROM dbo.ReceiptType R";
            try
            {
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dtReceiptType = new DataTable();
                sda.Fill(dtReceiptType);
                dtReceiptType.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtReceiptType;
        }

        public static DataTable GetOCRec(int argFlatId, int argCCId)
        {
            SqlDataAdapter sda; DataTable dtRec = null;
            string sSql = "SELECT OtherCostId,Amount FROM dbo.PaymentScheduleFlat WHERE FlatId=" + argFlatId + " AND SchType='O' AND CostCentreId=" + argCCId + "";
            try
            {
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dtRec = new DataTable();
                sda.Fill(dtRec);
                dtRec.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtRec;
        }

        public static DataTable GetStages(int argCCId, int argPayTypeId)
        {
            DataTable dtStage = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT StageId,StageName FROM dbo.Stages Where CostCentreId = " + argCCId + " " +
                       "And StageId Not In (Select StageId From dbo.PaymentSchedule Where CostCentreId= " + argCCId + " and TypeId = " + argPayTypeId + ") " +
                       "Order by SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dtStage = new DataTable();
                sda.Fill(dtStage);
                dtStage.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtStage;
        }

        public static DataTable GetDesc(int argCCId, int argPayTypeId, string argDescType)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = " SELECT SchDescId, SchDescName, CONVERT(bit, 0, 1) Sel FROM dbo.SchDescription " +
                              " Where SchDescId NOT IN(Select SchDescId From dbo.PaymentSchedule Where CostCentreId= " + argCCId + " AND TypeId= " + argPayTypeId + ") " +
                              " AND Type='" + argDescType + "' ORDER BY SchDescName";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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

        internal static DataTable GetEMISchedule(int argCCId, int argPayTypeId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = " SELECT * FROM dbo.PaymentSchedule " +
                              " Where CostCentreId= " + argCCId + " AND TypeId=" + argPayTypeId + " AND SchType='E' ORDER BY SortOrder";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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

        internal static DataTable PopulateDescriptionMaster(string argDescType)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = " SELECT SchDescId, SchDescName FROM dbo.SchDescription " +
                              " Where Type='" + argDescType + "' ORDER BY SchDescName";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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

        public static DataTable GetOCSetup(int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT CostCentreId,OtherCostId FROM dbo.OtherCostSetupTrans WHERE CostCentreId=" + argCCId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (SqlException e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static void InsertPaymentSchedule(DataTable argPayTrans, int argCCId, int argTId)
        {
            int ipaySchId = 0;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = String.Format("DELETE FROM dbo.PaymentSchedule WHERE CostCentreId={0} AND TypeId={1}", argCCId, argTId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    for (int t = 0; t < argPayTrans.Rows.Count; t++)
                    {
                        string nxtSchDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(argPayTrans.Rows[t]["SchDate"]));
                        sSql = String.Format("INSERT INTO dbo.PaymentSchedule(CostCentreId,TypeId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId) Values({0},{1},'{2}','{3}',{4},{5},{6},'{7}',{8},{9},'{10}',{11},{12},{13})SELECT SCOPE_IDENTITY();", argPayTrans.Rows[t]["CCId"], argTId, argPayTrans.Rows[t]["EntryType"], argPayTrans.Rows[t]["Description"], argPayTrans.Rows[t]["DescId"], argPayTrans.Rows[t]["StageId"], argPayTrans.Rows[t]["OtherCostId"], nxtSchDate, Convert.ToInt32(argPayTrans.Rows[t]["DateAfter"].ToString()), argPayTrans.Rows[t]["Duration"], Convert.ToChar(argPayTrans.Rows[t]["DurationType"].ToString()), argPayTrans.Rows[t]["AmtPercent"], argPayTrans.Rows[t]["Amount"], argPayTrans.Rows[t]["PreStageTypeId"]);
                        cmd = new SqlCommand(sSql, conn, tran);
                        ipaySchId = int.Parse(cmd.ExecuteScalar().ToString());
                        cmd.Dispose();
                        cmd.Dispose();
                    }
                    tran.Commit();
                }
                catch (SqlException ex)
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

        public static void InsertPayScheduleDes(DataTable argDt, int argCCId, int argPayTypeId, int argRow, string argDescType)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    string sSql = "Delete dbo.PaymentSchedule Where TypeId=" + argPayTypeId + " AND CostCentreId=" + argCCId + " AND SchType='" + argDescType + "'";
                    SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (argDescType == "E")
                    {
                        for (int i = 0; i < argDt.Rows.Count; i++)
                        {
                            string sShName = argDt.Rows[i]["Description"].ToString();

                            sSql = "Delete dbo.SchDescription Where SchDescName='" + sShName + "' AND Type='" + argDescType + "'";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            sSql = "Insert into dbo.SchDescription(SchDescName, Type) Values('" + sShName + "','E') SELECT SCOPE_IDENTITY();";
                            cmd = new SqlCommand(sSql, conn, tran);
                            int iSchDescId = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                            cmd.Dispose();

                            int iSortOrder = i + 1;

                            sSql = "Insert into dbo.PaymentSchedule(TypeId,CostCentreId,SchType,Description,SchDescId,SchDate,SortOrder,FlatTypeId,BlockId) " +
                                          "Values(" + argPayTypeId + "," + argCCId + ",'" + argDescType + "','" + sShName + "'," + iSchDescId + ",NULL," +
                                          iSortOrder + ",'" + argDt.Rows[i]["FlatTypeId"].ToString() + "','" + argDt.Rows[i]["BlockId"].ToString() + "')";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    else
                    {
                        for (int i = 0; i < argDt.Rows.Count; i++)
                        {
                            int iShId = Convert.ToInt32(argDt.Rows[i]["SchDescId"].ToString());
                            string sShName = argDt.Rows[i]["SchDescName"].ToString();
                            int iSortOrder = argRow + i + 1;

                            sSql = "Insert into dbo.PaymentSchedule(TypeId,CostCentreId,SchType,Description,SchDescId,SchDate,SortOrder) " +
                                          "Values(" + argPayTypeId + "," + argCCId + ",'" + argDescType + "','" + sShName + "'," + iShId + ",null," + iSortOrder + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    tran.Commit();
                }
                catch (SqlException e)
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

        public static void InsertPayScheduleStage(DataTable argDt, int argCCId, int argPayTypeId, int argRow)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    string sSql = "Delete dbo.PaymentSchedule Where TypeId=" + argPayTypeId + ",CostCentreId=" + argCCId + "";
                    SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    for (int i = 0; i < argDt.Rows.Count; i++)
                    {
                        int iStageId = Convert.ToInt32(argDt.Rows[i]["StageId"].ToString());
                        string sStageName = argDt.Rows[i]["StageName"].ToString();
                        int iSortOrder = argRow + i + 1;

                        sSql = "Insert into dbo.PaymentSchedule(TypeId,CostCentreId,SchType,Description,StageId,SchDate,SortOrder) " +
                               "Values(" + argPayTypeId + "," + argCCId + ",'S','" + sStageName + "'," + iStageId + ",null," + iSortOrder + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    tran.Commit();
                }
                catch (SqlException e)
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

        public static void UpdatePayDate(int argCCId, int argTempId, DataTable argPayTrans)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    for (int t = 0; t < argPayTrans.Rows.Count; t++)
                    {
                        string nxtSchDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(argPayTrans.Rows[t]["DurDate"]));
                        sSql = "Update dbo.PaymentSchedule set DateAfter=" + argPayTrans.Rows[t]["DateAfter"] + ",Duration=" + argPayTrans.Rows[t]["Duration"] + "," +
                            " DurationType='" + argPayTrans.Rows[t]["DurationType"] + "',PreStageTypeId=" + argPayTrans.Rows[t]["PreStageTypeId"] + "," +
                            " SchDate='" + nxtSchDate + "',PreStageType='" + argPayTrans.Rows[t]["PreStageType"] + "' WHERE " +
                            " TemplateId=" + argTempId + " AND CostCentreId=" + argCCId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    tran.Commit();
                }
                catch (SqlException e)
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

        public static void UpdatePayPercent(int argCCId, int argTempId, decimal argPer)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "Update dbo.PaymentSchedule Set SchPercent=" + argPer + " WHERE CostCentreId=" + argCCId + " AND TemplateId=" + argTempId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    tran.Commit();
                }
                catch (SqlException e)
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

        public static void UpdatePaymentSchedule(DataTable argPayTrans, int argTId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    for (int t = 0; t < argPayTrans.Rows.Count; t++)
                    {
                        string nxtSchDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(argPayTrans.Rows[t]["SchDate"]));
                        sSql = String.Format("Update dbo.PaymentSchedule set SchType='{0}',Description='{1}',SchDescId={2},StageId={3}, " +
                                             " OtherCostId={4},SchDate='{5}',PreStageType='{6}',DateAfter={7},Duration={8},DurationType='{9}', " +
                                             " SchPercent={10},Amount={11},PreStageTypeId={12} Where TemplateId={13} AND  CostCentreId={14} AND TypeId={15} ",
                                             argPayTrans.Rows[t]["EntryType"], argPayTrans.Rows[t]["Description"], argPayTrans.Rows[t]["DescId"],
                                             argPayTrans.Rows[t]["StageId"], argPayTrans.Rows[t]["OtherCostId"], nxtSchDate, argPayTrans.Rows[t]["PreStageType"],
                                             Convert.ToInt32(argPayTrans.Rows[t]["DateAfter"].ToString()), argPayTrans.Rows[t]["Duration"],
                                             Convert.ToChar(argPayTrans.Rows[t]["DurationType"].ToString()), argPayTrans.Rows[t]["AmtPercent"],
                                             argPayTrans.Rows[t]["Amount"], argPayTrans.Rows[t]["PreStageTypeId"], argPayTrans.Rows[t]["PaymentSchId"],
                                             argPayTrans.Rows[t]["CCId"], argTId);
                        SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    tran.Commit();
                }
                catch (SqlException e)
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

        public void pay(DataTable argPayTrans, int argFTId, int argFId, DataTable argdtR)
        {
            int tr = 0;
            Decimal dNetAmt;
            Decimal dSchAmt;
            Decimal dAdvAmt;
            decimal dOther; decimal dOCAmt;
            decimal dLandAmt = 0; decimal dBaseAmt = 0;
            decimal dTotLAmt = 0, dTotBAmt = 0, dTotAdvAmt = 0;
            int OCTransId;
            DataView dv;
            DataTable dtRec;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;

            conn = BsfGlobal.OpenCRMDB();

            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "DELETE FROM PaymentScheduleFlat WHERE FlatId=" + argFId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();

                    sSql = "SELECT LandRate FROM FlatDetails WHERE FlatId=" + argFId;
                    dLandAmt = GetLandAmt(sSql, conn, tran);
                    sSql = "SELECT BaseAmt FROM FlatDetails WHERE FlatId=" + argFId;
                    dBaseAmt = GetBaseAmt(sSql, conn, tran);

                    sSql = "SELECT NetAmt FROM FlatDetails WHERE FlatId=" + argFId;
                    dNetAmt = GetNetAmt(sSql, conn, tran);
                    sSql = "SELECT AdvAmount FROM FlatDetails WHERE FlatId=" + argFId;
                    dAdvAmt = GetAdvAmt(sSql, conn, tran);


                    for (int t = 0; t < argPayTrans.Rows.Count; t++)
                    {
                        string nxtSchDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(argPayTrans.Rows[t]["SchDate"].ToString()));
                        if (Convert.ToBoolean(argPayTrans.Rows[t]["DateAfter"]) == true)
                        {
                            tr = 1;
                        }
                        else
                        {
                            tr = 0;
                        }


                        if (Convert.ToInt32(argPayTrans.Rows[t]["OtherCostId"].ToString()) == 0)
                        {
                            //sSql = "SELECT NetAmt FROM FlatDetails WHERE FlatId=" + argFId;
                            //dNetAmt = GetNetAmt(sSql);
                            //sSql = "SELECT AdvAmount FROM FlatDetails WHERE FlatId=" + argFId;
                            //dAdvAmt = GetAdvAmt(sSql);
                            sSql = "SELECT Amount FROM FlatOtherCost WHERE OtherCostId=" + Convert.ToInt32(argPayTrans.Rows[t]["OtherCostId"].ToString()) + " and  FlatId=" + argFId;
                            dOCAmt = GetOtherCost(sSql, conn, tran);
                            if (dNetAmt != 0 && dAdvAmt != 0)
                            {
                                //dNetAmt = GetNetAmt(sSql);
                                sSql = "SELECT F.Amount FROM FlatOtherCost F INNER JOIN PaymentSchedule P" +
                                   " ON F.OtherCostId=P.OtherCostId WHERE FlatId=" + argFId + " AND P.TypeId=" + argPayTrans.Rows[t]["TypeId"] + "";
                                dOther = GetNetOC(sSql, conn, tran);

                                sSql = "SELECT OtherCostId FROM PaymentSchedule WHERE CostCentreId=" + argPayTrans.Rows[t]["CostCentreId"] + " AND OtherCostId=-1 AND TypeId=" + argPayTrans.Rows[t]["TypeId"] + "";
                                OCTransId = GetOCTransId(sSql, conn, tran);
                                if (OCTransId != 0)
                                    if (OCTransId == -1)
                                        dNetAmt = dNetAmt - dOther - dAdvAmt;
                                    else
                                        dNetAmt = dNetAmt - dOther;
                            }
                            else
                            {
                                dNetAmt = 0;
                            }
                            dSchAmt = (Convert.ToDecimal(argPayTrans.Rows[t]["SchPercent"].ToString()) * dNetAmt) / 100;
                            sSql = String.Format("INSERT INTO PaymentScheduleFlat(TemplateId,TypeId,FlatId,FlatTypeId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,PreStageType,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId) Values({0},{1},{2},{3},{4},'{5}','{6}',{7},{8},{9},'{10}','{11}',{12},{13},'{14}',{15},{16},{17})", argPayTrans.Rows[t]["TemplateId"], argPayTrans.Rows[t]["TypeId"], argFId, argFTId, argPayTrans.Rows[t]["CostCentreId"], argPayTrans.Rows[t]["SchType"], argPayTrans.Rows[t]["Description"], argPayTrans.Rows[t]["SchDescId"], argPayTrans.Rows[t]["StageId"], argPayTrans.Rows[t]["OtherCostId"], nxtSchDate, argPayTrans.Rows[t]["PreStageType"], tr, argPayTrans.Rows[t]["Duration"], Convert.ToChar(argPayTrans.Rows[t]["DurationType"].ToString()), argPayTrans.Rows[t]["SchPercent"], dSchAmt, argPayTrans.Rows[t]["PreStageTypeId"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();

                            if (Convert.ToInt32(argPayTrans.Rows[t]["OtherCostId"].ToString()) == 0)
                            {
                                dv = new DataView(argdtR) { RowFilter = String.Format("TemplateId={0}", argPayTrans.Rows[t]["TemplateId"]) };
                                dtRec = new DataTable();
                                dtRec = dv.ToTable();
                                decimal Amt = 0;
                                if (dtRec != null)
                                {
                                    if (dtRec.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dtRec.Rows.Count; i++)
                                        {
                                            Amt = (dSchAmt * Convert.ToDecimal(dtRec.Rows[i]["Percentage"])) / 100;
                                            if (Convert.ToInt32(dtRec.Rows[i]["ReceiptTypeId"]) == 1)
                                            {
                                                dTotAdvAmt = dAdvAmt;
                                                if (Amt > dAdvAmt)
                                                {
                                                    decimal dPer; bool Sel = false;
                                                    //if (dTotAdvAmt >= dAdvAmt)
                                                    //{
                                                    //    dTotAdvAmt = 0; dPer = 0; Sel = false;
                                                    //}
                                                    //else
                                                    //{
                                                    dTotAdvAmt = dAdvAmt;
                                                    dPer = dTotAdvAmt * 100 / dSchAmt; Sel = true;
                                                    //}
                                                    sSql = "INSERT INTO FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId)VALUES" +
                                                        " (" + dtRec.Rows[i]["SchId"] + "," + dtRec.Rows[i]["TemplateId"] + "," + argFId + "," + dtRec.Rows[i]["ReceiptTypeId"] + "," +
                                                    " " + dPer + "," + dTotAdvAmt + "," +
                                                    " '" + Sel + "'," + dtRec.Rows[i]["OtherCostId"] + ")";
                                                    cmd = new SqlCommand(sSql, conn, tran);
                                                    cmd.ExecuteNonQuery();

                                                    dAdvAmt = dAdvAmt - dTotAdvAmt;
                                                }
                                                else
                                                {
                                                    sSql = "INSERT INTO FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId)VALUES" +
                                                    " (" + dtRec.Rows[i]["SchId"] + "," + dtRec.Rows[i]["TemplateId"] + "," + argFId + "," + dtRec.Rows[i]["ReceiptTypeId"] + "," +
                                                " " + dtRec.Rows[i]["Percentage"] + "," + Amt + "," +
                                                " '" + dtRec.Rows[i]["Sel"] + "'," + dtRec.Rows[i]["OtherCostId"] + ")";
                                                    cmd = new SqlCommand(sSql, conn, tran);
                                                    cmd.ExecuteNonQuery();
                                                    dAdvAmt = dTotAdvAmt - Amt;
                                                    dTotAdvAmt = Amt;
                                                }
                                            }
                                            else if (Convert.ToInt32(dtRec.Rows[i]["ReceiptTypeId"]) == 2)
                                            {
                                                if (Amt > dLandAmt)
                                                {
                                                    decimal dPer; bool Sel = false;
                                                    if (dTotLAmt >= dLandAmt)
                                                    {
                                                        dTotLAmt = 0; dPer = 0; Sel = false;
                                                    }
                                                    else
                                                    {
                                                        dTotLAmt = dLandAmt;
                                                        dPer = dTotLAmt * 100 / dSchAmt; Sel = true;
                                                    }
                                                    sSql = "INSERT INTO FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId)VALUES" +
                                                        " (" + dtRec.Rows[i]["SchId"] + "," + dtRec.Rows[i]["TemplateId"] + "," + argFId + "," + dtRec.Rows[i]["ReceiptTypeId"] + "," +
                                                    " " + dPer + "," + dTotLAmt + "," +
                                                    " '" + Sel + "'," + dtRec.Rows[i]["OtherCostId"] + ")";
                                                    cmd = new SqlCommand(sSql, conn, tran);
                                                    cmd.ExecuteNonQuery();

                                                    dLandAmt = dLandAmt - dTotLAmt;
                                                }
                                                else
                                                {
                                                    sSql = "INSERT INTO FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId)VALUES" +
                                                    " (" + dtRec.Rows[i]["SchId"] + "," + dtRec.Rows[i]["TemplateId"] + "," + argFId + "," + dtRec.Rows[i]["ReceiptTypeId"] + "," +
                                                " " + dtRec.Rows[i]["Percentage"] + "," + Amt + "," +
                                                " '" + dtRec.Rows[i]["Sel"] + "'," + dtRec.Rows[i]["OtherCostId"] + ")";
                                                    cmd = new SqlCommand(sSql, conn, tran);
                                                    cmd.ExecuteNonQuery();
                                                    dLandAmt = dLandAmt - Amt;
                                                    dTotLAmt = Amt;
                                                }
                                            }
                                            else if (Convert.ToInt32(dtRec.Rows[i]["ReceiptTypeId"]) == 3)
                                            {
                                                decimal dPer = 0; bool Sel = false;
                                                if (Amt > dBaseAmt)
                                                {
                                                    if (dTotBAmt >= dBaseAmt)
                                                    {
                                                        dTotBAmt = 0; dPer = 0; Sel = false;
                                                    }
                                                    else
                                                    {
                                                        dTotBAmt = dBaseAmt;
                                                        dPer = dTotBAmt * 100 / dSchAmt; Sel = true;
                                                    }
                                                }
                                                else
                                                {
                                                    dTotBAmt = Amt;
                                                    dPer = dTotBAmt * 100 / dSchAmt; Sel = true;
                                                }
                                                //if (dPer>0)
                                                sSql = "INSERT INTO FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId)VALUES" +
                                                " (" + dtRec.Rows[i]["SchId"] + "," + dtRec.Rows[i]["TemplateId"] + "," + argFId + "," + dtRec.Rows[i]["ReceiptTypeId"] + "," +
                                                " " + dPer + "," + dTotBAmt + "," +
                                                " '" + Sel + "'," + dtRec.Rows[i]["OtherCostId"] + ")";
                                                cmd = new SqlCommand(sSql, conn, tran);
                                                cmd.ExecuteNonQuery();

                                                dBaseAmt = dBaseAmt - dTotBAmt;

                                            }
                                            else
                                            {
                                                decimal dper = 0; bool Sel = false;
                                                if (Convert.ToInt32(dtRec.Rows[i]["ReceiptTypeId"]) == 5)
                                                {
                                                    //Amt = dSchAmt - (dAdvAmt + dLandAmt + dBaseAmt);
                                                    Amt = dSchAmt - (dTotAdvAmt + dTotLAmt + dTotBAmt);
                                                    if (Amt > 0)
                                                    { dper = Amt * 100 / dSchAmt; Sel = true; }
                                                    else { Amt = 0; dper = 0; Sel = false; }
                                                }
                                                else
                                                {
                                                    Amt = 0; dper = 0; Sel = false;
                                                }
                                                sSql = "INSERT INTO FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId)VALUES" +
                                                    " (" + dtRec.Rows[i]["SchId"] + "," + dtRec.Rows[i]["TemplateId"] + "," + argFId + "," + dtRec.Rows[i]["ReceiptTypeId"] + "," +
                                                " " + dper + "," + Amt + "," +
                                                " '" + Sel + "'," + dtRec.Rows[i]["OtherCostId"] + ")";
                                                cmd = new SqlCommand(sSql, conn, tran);
                                                cmd.ExecuteNonQuery();
                                                dBaseAmt = dBaseAmt - Amt;
                                            }
                                        }
                                    }
                                }
                            }
                            else if (Convert.ToInt32(argPayTrans.Rows[t]["OtherCostId"].ToString()) > 0)
                            {
                                sSql = "INSERT INTO FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId)VALUES" +
                                        " (0, 0," + argFId + ",0,100," + dOCAmt + "," +
                                    " 'true'," + argPayTrans.Rows[t]["OtherCostId"] + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                            }
                            else if (Convert.ToInt32(argPayTrans.Rows[t]["OtherCostId"].ToString()) == -1)
                            {
                                sSql = "INSERT INTO FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId)VALUES" +
                                    " (0, 0," + argFId + ",0,100," + dAdvAmt + "," +
                                " 'true'," + argPayTrans.Rows[t]["OtherCostId"] + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        else if (Convert.ToInt32(argPayTrans.Rows[t]["OtherCostId"].ToString()) != 0)
                        {
                            sSql = "SELECT Amount FROM FlatOtherCost WHERE OtherCostId=" + Convert.ToInt32(argPayTrans.Rows[t]["OtherCostId"].ToString()) + " and  FlatId=" + argFId;
                            dNetAmt = GetOtherCost(sSql, conn, tran);
                            sSql = "SELECT AdvAmount FROM FlatDetails WHERE FlatId=" + argFId;
                            dAdvAmt = GetAdvAmt(sSql, conn, tran);
                            if (Convert.ToInt32(argPayTrans.Rows[t]["OtherCostId"].ToString()) == -1)
                            {
                                dSchAmt = dAdvAmt;
                                sSql = String.Format("INSERT INTO PaymentScheduleFlat(TemplateId,TypeId,FlatId,FlatTypeId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,PreStageType,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId) Values({0},{1},{2},{3},{4},'{5}','{6}',{7},{8},{9},'{10}','{11}',{12},{13},'{14}',{15},{16},{17})", argPayTrans.Rows[t]["TemplateId"], argPayTrans.Rows[t]["TypeId"], argFId, argFTId, argPayTrans.Rows[t]["CostCentreId"], argPayTrans.Rows[t]["SchType"], argPayTrans.Rows[t]["Description"], argPayTrans.Rows[t]["SchDescId"], argPayTrans.Rows[t]["StageId"], argPayTrans.Rows[t]["OtherCostId"], nxtSchDate, argPayTrans.Rows[t]["PreStageType"], tr, argPayTrans.Rows[t]["Duration"], Convert.ToChar(argPayTrans.Rows[t]["DurationType"].ToString()), argPayTrans.Rows[t]["SchPercent"], dSchAmt, argPayTrans.Rows[t]["PreStageTypeId"]);
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();

                                sSql = "INSERT INTO FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId)VALUES" +
                                    " (0, 0," + argFId + ",0,100," + dSchAmt + "," +
                                " 'true'," + argPayTrans.Rows[t]["OtherCostId"] + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                dSchAmt = dNetAmt;
                                sSql = String.Format("INSERT INTO PaymentScheduleFlat(TemplateId,TypeId,FlatId,FlatTypeId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,PreStageType,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId) Values({0},{1},{2},{3},{4},'{5}','{6}',{7},{8},{9},'{10}','{11}',{12},{13},'{14}',{15},{16},{17})", argPayTrans.Rows[t]["TemplateId"], argPayTrans.Rows[t]["TypeId"], argFId, argFTId, argPayTrans.Rows[t]["CostCentreId"], argPayTrans.Rows[t]["SchType"], argPayTrans.Rows[t]["Description"], argPayTrans.Rows[t]["SchDescId"], argPayTrans.Rows[t]["StageId"], argPayTrans.Rows[t]["OtherCostId"], nxtSchDate, argPayTrans.Rows[t]["PreStageType"], tr, argPayTrans.Rows[t]["Duration"], Convert.ToChar(argPayTrans.Rows[t]["DurationType"].ToString()), argPayTrans.Rows[t]["SchPercent"], dSchAmt, argPayTrans.Rows[t]["PreStageTypeId"]);
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                if (dSchAmt != 0)
                                {
                                    sSql = "INSERT INTO FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId)VALUES" +
                                        " (0, 0," + argFId + ",0,100," + dSchAmt + "," +
                                    " 'true'," + argPayTrans.Rows[t]["OtherCostId"] + ")";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        else
                        {
                            sSql = "SELECT NetAmt FROM FlatDetails WHERE FlatId=" + argFId;
                            dNetAmt = GetNetAmt(sSql, conn, tran);
                            sSql = "SELECT AdvAmount FROM FlatDetails WHERE FlatId=" + argFId;
                            dAdvAmt = GetAdvAmt(sSql, conn, tran);

                            dSchAmt = (Convert.ToDecimal(argPayTrans.Rows[t]["SchPercent"].ToString()) * (dNetAmt)) / 100;
                            sSql = String.Format("INSERT INTO PaymentScheduleFlat(TemplateId,TypeId,FlatId,FlatTypeId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,PreStageType,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId) Values({0},{1},{2},{3},{4},'{5}','{6}',{7},{8},{9},'{10}','{11}',{12},{13},'{14}',{15},{16},{17})", argPayTrans.Rows[t]["TemplateId"], argPayTrans.Rows[t]["TypeId"], argFId, argFTId, argPayTrans.Rows[t]["CostCentreId"], argPayTrans.Rows[t]["SchType"], argPayTrans.Rows[t]["Description"], argPayTrans.Rows[t]["SchDescId"], argPayTrans.Rows[t]["StageId"], argPayTrans.Rows[t]["OtherCostId"], nxtSchDate, argPayTrans.Rows[t]["PreStageType"], tr, argPayTrans.Rows[t]["Duration"], Convert.ToChar(argPayTrans.Rows[t]["DurationType"].ToString()), argPayTrans.Rows[t]["SchPercent"], dSchAmt, argPayTrans.Rows[t]["PreStageTypeId"]);
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                        }
                    }


                    tran.Commit();
                }
                catch (SqlException e)
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

        public static void InsertNoOfFlats(int argCCId, int argFlatId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmd;

            string sSql = " Update dbo.ProjectInfo Set TotalFlats=(Select Count(FlatId) TotalFlat From dbo.FlatDetails" +
                " Where CostCentreId=" + argCCId + ") Where CostCentreId=" + argCCId + "";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        #region PaymentSchedule

        public static void InsertFlatScheduleI(int argFlatId, SqlConnection conn, SqlTransaction tran)
        {
            string sSql = "";

            SqlDataReader sdr;
            SqlCommand cmd;
            DataTable dt = new DataTable();
            DataTable dtTax = new DataTable();

            int iCCId = 0;
            int iFlatTypeId = 0;
            int iPayTypeId = 0;
            decimal dBaseAmt = 0;
            decimal dAdvAmt = 0;
            decimal dAdvBalAmt = 0;
            decimal dLandAmt = 0;
            decimal dNetAmt = 0;
            decimal dOtherAmt = 0;
            decimal dRAmt = 0;
            int iReceiptId = 0;
            int iROtherCostId = 0;
            string sRSchType = "";
            bool bAdvance = false;
            int iPaymentSchId = 0;
            string sSchType = "";
            int iOtherCostId = 0;
            decimal dRPer = 0;
            decimal dSchPercent = 0;
            decimal dQBaseAmt = 0;
            decimal dQNetAmt = 0;
            int iTemplateId = 0;
            int iSchId = 0;
            int iRSchId = 0;
            decimal dTNetAmt = 0;
            decimal dBalAmt = 0;
            bool bPayTypewise = false;
            //decimal dTaxPer = 0; decimal dTotSerTaxamt = 0; decimal dTotVatTaxamt = 0; decimal dSerTaxamt = 0; decimal dVatTaxamt = 0; bool bTaxSel = false;
            decimal dTotalTax = 0;
            //decimal dServiceTax = 0;
            //decimal dVATTax = 0;
            bool bService = false, bLCBon = false;
            DataRow[] drT;
            cRateQualR RAQual;
            Collection QualVBC;
            decimal dAdv = 0;
            DateTime m_dSchDate = DateTime.Now;

            DataTable dtReceipt = new DataTable();

            sSql = "Delete From dbo.PaymentSchedule Where TypeId IN(Select PayTypeId from dbo.FlatDetails Where FlatId=" + argFlatId + ") " +
                   " AND CostCentreId IN(Select CostCentreId from dbo.FlatDetails Where FlatId=" + argFlatId + ") AND SchType='R'";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete From dbo.PaymentScheduleFlat Where FlatId=" + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete From dbo.FlatReceiptQualifier Where SchId IN(Select SchId from dbo.FlatReceiptType Where FlatId= " + argFlatId + ")";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete From dbo.FlatReceiptType Where FlatId=" + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete From dbo.PaySchTaxFlat Where FlatId=" + argFlatId + " ";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            //sSql = "Select FlatTypeId,CostCentreId,PayTypeId,BaseAmt,AdvAmount,USLandAmt from dbo.FlatDetails Where FlatId= " + argFlatId;//modified
            sSql = "Select FlatTypeId,CostCentreId,PayTypeId,BaseAmt,AdvAmount,LandRate,Guidelinevalue,USLandAmt From dbo.FlatDetails Where FlatId= " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0)
            {
                iCCId = Convert.ToInt32(dt.Rows[0]["CostCentreId"].ToString());
                iFlatTypeId = Convert.ToInt32(dt.Rows[0]["FlatTypeId"].ToString());
                iPayTypeId = Convert.ToInt32(dt.Rows[0]["PayTypeId"].ToString());
                bPayTypewise = FlatDetailsDL.GetTypewise(iPayTypeId);
                dBaseAmt = Convert.ToDecimal(dt.Rows[0]["BaseAmt"].ToString());
                dAdvAmt = Convert.ToDecimal(dt.Rows[0]["AdvAmount"].ToString());

                sSql = "Select LCBasedon From dbo.ProjectInfo Where CostCentreId= " + iCCId;
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                DataTable dtPI = new DataTable();
                dtPI.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                if (dtPI.Rows.Count > 0) { bLCBon = Convert.ToBoolean(dtPI.Rows[0]["LCBasedon"]); }
                if (bLCBon == false) { dLandAmt = Convert.ToDecimal(dt.Rows[0]["LandRate"].ToString()); }
                else { dLandAmt = Convert.ToDecimal(dt.Rows[0]["USLandAmt"].ToString()); }

                //dLandAmt = Convert.ToDecimal(dt.Rows[0]["USLandAmt"].ToString());

            }
            dt.Dispose();

            sSql = "Select SUM(RoundValue) From dbo.PaySchType Where TypeId=" + iPayTypeId + " ";
            cmd = new SqlCommand(sSql, conn, tran);
            decimal dRoundValue = Convert.ToDecimal(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
            cmd.Dispose();

            if (iPayTypeId > 0 && dRoundValue > 0)
            {
                sSql = "Select COUNT(*) From dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " AND CostCentreId=" + iCCId + " AND SchType='R'";
                cmd = new SqlCommand(sSql, conn, tran);
                int iCount = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                cmd.Dispose();

                if (iCount == 0)
                {
                    sSql = "Insert Into dbo.PaymentSchedule(TypeId,CostCentreId,SchType,Description,SchDescId,StageId, " +
                          " OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId,SortOrder,FlatTypeId,BlockId) " +
                          " Select TOP 1 " + iPayTypeId + "," + iCCId + ",'R','Final Amount to be Collect from Buyer',0,0,0,NULL,0,0,'',0,0,0, "+
                          " SortOrder+1,FlatTypeId,BlockId from dbo.PaymentSchedule " +
                          " Where TypeId=" + iPayTypeId + " AND CostCentreId=" + iCCId + " ORDER BY SortOrder DESC";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            else
            {
                sSql = "Delete From dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " AND CostCentreId=" + iCCId + " AND SchType='R'";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }

            sSql = "Select TemplateId From dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " and CostCentreId = " + iCCId + " and SchType='A'";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0) { bAdvance = true; }
            dt.Dispose();

            sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                    "Where FlatId = " + argFlatId + " and OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans " +
                    " Where PayTypeId=" + iPayTypeId + " and CostCentreId=" + iCCId + ")";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0) { dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }
            dt.Dispose();

            sSql = "Select QualifierId,Amount from dbo.FlatTax Where FlatId = " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtTx = new DataTable();
            dtTx.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            decimal dT = 0;
            if (dtTx.Rows.Count > 0)
            {
                for (int i = 0; i < dtTx.Rows.Count; i++)
                {
                    dTotalTax = Convert.ToDecimal(dtTx.Rows[i]["Amount"]);
                    dT = dT + dTotalTax;
                }
            }

            if (bPayTypewise == false)
            { dNetAmt = dBaseAmt + dOtherAmt + dT; }
            else
            { dNetAmt = dBaseAmt + dOtherAmt; }
            if (bAdvance == true) { dNetAmt = dNetAmt - dAdvAmt; }

            sSql = "Insert Into dbo.PaymentScheduleFlat(FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId, "+
                   " OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId,SortOrder) " +
                   " Select " + argFlatId + ",TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId, "+
                   " SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId,SortOrder from dbo.PaymentSchedule " +
                   " Where TypeId=" + iPayTypeId + " AND CostCentreId=" + iCCId+ " AND OtherCostId NOT IN(Select OtherCostId from dbo.OXGross Where CostCentreId=" + iCCId + ")";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Select ReceiptTypeId,OtherCostId,SchType from dbo.ReceiptTypeOrder " +
                    "Where PayTypeId = " + iPayTypeId + " and CostCentreId=" + iCCId + " and SchType <>'A' Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtReceiptOrder = new DataTable();
            dtReceiptOrder.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            sSql = "Select OtherCostId,Flag,Amount from dbo.FlatOtherCost Where FlatId = " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtT = new DataTable();
            dtT.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            dtReceipt.Columns.Add("Id", typeof(int));
            dtReceipt.Columns.Add("SchType", typeof(string));
            dtReceipt.Columns.Add("Amount", typeof(decimal));
            dtReceipt.Columns.Add("RAmount", typeof(decimal));

            DataRow drR;
            drR = dtReceipt.NewRow();
            drR["Id"] = 1;
            drR["SchType"] = "A";
            drR["Amount"] = dAdvAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            drR = dtReceipt.NewRow();
            drR["Id"] = 2;
            drR["SchType"] = "R";
            drR["Amount"] = dLandAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            drR = dtReceipt.NewRow();
            drR["Id"] = 3;
            drR["SchType"] = "R";
            drR["Amount"] = dBaseAmt - dLandAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            for (int i = 0; i < dtT.Rows.Count; i++)
            {
                drR = dtReceipt.NewRow();
                drR["Id"] = Convert.ToInt32(dtT.Rows[i]["OtherCostId"].ToString());
                drR["SchType"] = "O";
                drR["Amount"] = Convert.ToDecimal(dtT.Rows[i]["Amount"].ToString());
                drR["RAmount"] = 0;
                dtReceipt.Rows.Add(drR);
            }

            if (bPayTypewise == false)
            {
                for (int i = 0; i < dtTx.Rows.Count; i++)
                {
                    drR = dtReceipt.NewRow();
                    drR["Id"] = Convert.ToInt32(dtTx.Rows[i]["QualifierId"].ToString());
                    drR["SchType"] = "Q";
                    drR["Amount"] = Convert.ToDecimal(dtTx.Rows[i]["Amount"].ToString());
                    drR["RAmount"] = 0;
                    dtReceipt.Rows.Add(drR);
                }
            }

            sSql = "Select SchId,TemplateId,ReceiptTypeId,Percentage,OtherCostId,SchType from dbo.CCReceiptType " +
                    "Where TemplateId IN(Select TemplateId from dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " and CostCentreId=" + iCCId + ") Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtTemp = new DataTable();
            dtTemp.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            //sSql = "Select A.*,IsNull(B.Service,0)Service From dbo.CCReceiptQualifier A " +
            //        " Left Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId Where CostCentreId=" + iCCId;
            sSql = "Select C.QualTypeId,A.*,IsNull(B.Service,0)Service From dbo.CCReceiptQualifier A " +
                    " Left Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId " +
                    " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On C.QualifierId=A.QualifierId " +
                    " Where CostCentreId=" + iCCId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtQual = new DataTable();
            dtQual.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            sSql = "Select PaymentSchId,TemplateId,SchType,SchDate,OtherCostId,SchPercent from dbo.PaymentScheduleFlat Where FlatId = " + argFlatId + " Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            decimal dAmt = 0;
            DataView dv;
            decimal dAdvRAmt = 0;

            DataTable dtTempT;
            DataTable dtQualT;
            dAdvBalAmt = dAdvAmt;

            decimal dEMIRoundOff = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iPaymentSchId = Convert.ToInt32(dt.Rows[i]["PaymentSchId"].ToString());
                iTemplateId = Convert.ToInt32(dt.Rows[i]["TemplateId"].ToString());
                sSchType = dt.Rows[i]["SchType"].ToString();
                m_dSchDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[i]["SchDate"], CommFun.datatypes.VarTypeDate));
                iOtherCostId = Convert.ToInt32(dt.Rows[i]["OtherCostId"].ToString());
                dSchPercent = Convert.ToDecimal(dt.Rows[i]["SchPercent"].ToString());
                dTNetAmt = 0;

                if (m_dSchDate == DateTime.MinValue) { m_dSchDate = DateTime.Now; }

                dAmt = 0;

                if (sSchType == "A")
                {
                    dAmt = dAdvAmt;
                }
                else if (sSchType == "O")
                {
                    dv = new DataView(dtT);
                    dv.RowFilter = "OtherCostId = " + iOtherCostId;
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        dAmt = Convert.ToDecimal(dv.ToTable().Rows[0]["Amount"].ToString());
                        if (dv.ToTable().Rows[0]["Flag"].ToString() == "-") { dAmt = dAmt * (-1); }
                    }
                    dv.Dispose();
                }
                else
                {
                    dAmt = dNetAmt * dSchPercent / 100;
                }

                dtTempT = new DataTable();
                dv = new DataView(dtTemp);
                dv.RowFilter = "TemplateId = " + iTemplateId;
                dtTempT = dv.ToTable();
                dv.Dispose();

                decimal dRoundOff = 0; 
                decimal dRound = 0;
                if (dRoundValue > 0)
                {
                    dRoundOff = Math.Truncate(dAmt / dRoundValue)* dRoundValue;
                    dRound = dAmt - dRoundOff;
                    dEMIRoundOff = dEMIRoundOff + dRound;
                    dAmt = dRoundOff;
                }

                if (sSchType == "R")
                {
                    sSql = "Update dbo.PaymentScheduleFlat Set Amount=" + dEMIRoundOff + ",NetAmount=" + dEMIRoundOff +
                           " Where PaymentSchId=" + iPaymentSchId + " AND FlatId=" + argFlatId + " AND CostCentreId=" + iCCId + " AND SchType='R'";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    if (dtTempT.Rows.Count == 1 && sSchType == "O")
                    {
                        sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                "Values(" + iPaymentSchId + "," + argFlatId + ",0," + iOtherCostId + ",'" + sSchType + "',100," + dAmt + "," + dAmt + ") SELECT SCOPE_IDENTITY();";
                        cmd = new SqlCommand(sSql, conn, tran);
                        iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                        cmd.Dispose();

                        drT = dtReceipt.Select("SchType = 'O' and Id = " + iOtherCostId + "");

                        if (drT.Length > 0)
                        {
                            drT[0]["RAmount"] = dAmt;
                        }

                        dQNetAmt = dAmt;

                        dtQualT = new DataTable();
                        dv = new DataView(dtQual);
                        dv.RowFilter = "SchType = '" + sSchType + "' and OtherCostId = " + iOtherCostId;
                        dtQualT = dv.ToTable();
                        dv.Dispose();

                        if (dtQualT.Rows.Count > 0)
                        {
                            QualVBC = new Collection();

                            for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                            {
                                RAQual = new cRateQualR();
                                bService = Convert.ToBoolean(dtQualT.Rows[Q]["Service"]);

                                DataTable dtTDS = new DataTable();
                                if (Convert.ToInt32(dtQualT.Rows[Q]["QualTypeId"]) == 2)
                                {
                                    if (bService == true)
                                        dtTDS = GetSTSettings("G", m_dSchDate, conn, tran);
                                    else
                                        dtTDS = GetSTSettings("F", m_dSchDate, conn, tran);
                                }
                                else
                                {
                                    dtTDS = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), m_dSchDate, "B", conn, tran);
                                }

                                RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                                if (dtTDS.Rows.Count > 0)
                                {
                                    RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                                    RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                                    RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                                    RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                                    RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                                    RAQual.HEDValue = Convert.ToDecimal(dtQualT.Rows[Q]["HEDValue"].ToString());
                                    RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Taxable"], CommFun.datatypes.vartypenumeric));
                                }

                                DataTable dtQ = new DataTable();
                                dtQ = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), m_dSchDate, "B", conn, tran);
                                //dtQ = QualifierSelect(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), "B", m_dSchDate, conn, tran);
                                if (dtQ.Rows.Count > 0)
                                {
                                    RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                                    RAQual.Amount = 0;
                                    RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                                }

                                QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                            }

                            Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                            dQBaseAmt = dAmt;
                            dQNetAmt = dAmt; decimal dTaxAmt = 0;
                            decimal dVATAmt = 0;

                            if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, m_dSchDate, ref dVATAmt) == true)
                            {
                                foreach (Qualifier.cRateQualR d in QualVBC)
                                {
                                    sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount,HEDPer,HEDValue,NetPer,TaxableValue,TaxablePer) " +
                                            "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                            "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + "," +
                                            " " + d.HEDPer + "," + d.HEDValue + "," + d.NetPer + "," + d.TaxableValue + "," + d.TaxablePer + ")";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                            }

                            if (bPayTypewise == true)
                            {
                                sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }

                        if (bPayTypewise == true)
                            sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dQNetAmt + "  Where PaymentSchId = " + iPaymentSchId;
                        else
                            sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dAmt + "  Where PaymentSchId = " + iPaymentSchId;
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        dTNetAmt = dTNetAmt + dQNetAmt;
                    }
                    else
                    {
                        dBalAmt = dAmt;
                        for (int j = 0; j < dtTempT.Rows.Count; j++)
                        {
                            iSchId = Convert.ToInt32(dtTempT.Rows[j]["SchId"].ToString());
                            dRPer = Convert.ToDecimal(dtTempT.Rows[j]["Percentage"].ToString());
                            sRSchType = dtTempT.Rows[j]["SchType"].ToString();
                            iReceiptId = Convert.ToInt32(dtTempT.Rows[j]["ReceiptTypeId"].ToString());
                            iROtherCostId = Convert.ToInt32(dtTempT.Rows[j]["OtherCostId"].ToString());

                            if (dRPer != 0) { dRAmt = dAmt * dRPer / 100; }
                            else { dRAmt = dBalAmt; }

                            if (dRAmt > dBalAmt) { dRAmt = dBalAmt; }

                            if (sRSchType == "A" && bAdvance == false)
                            {
                                dAdvRAmt = dAdvAmt * dRPer / 100;
                                if (dAdvRAmt > dAdvBalAmt) { dAdvRAmt = dAdvBalAmt; }
                                dAdvBalAmt = dAdvBalAmt - dAdvRAmt;
                                dTNetAmt = dTNetAmt - dAdvRAmt;

                                dAdv = dAdvRAmt;
                                sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                        "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + ", 0," + dAdvRAmt + ") SELECT SCOPE_IDENTITY();";
                                cmd = new SqlCommand(sSql, conn, tran);
                                iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                                cmd.Dispose();

                            }
                            else
                            {
                                dAdv = 0;
                                if (sRSchType == "A")
                                    drT = dtReceipt.Select("SchType = 'A'");
                                else if (sRSchType == "O")
                                    drT = dtReceipt.Select("SchType = 'O' and Id = " + iROtherCostId + "");
                                else if (sRSchType == "Q")
                                    drT = dtReceipt.Select("SchType = 'Q' and Id = " + iReceiptId + "");
                                else
                                    drT = dtReceipt.Select("SchType = 'R' and Id = " + iReceiptId + "");

                                decimal dRTAmt = 0;
                                decimal dRRAmt = 0;

                                if (drT.Length > 0)
                                {
                                    dRTAmt = Convert.ToDecimal(drT[0]["Amount"].ToString());
                                    dRRAmt = Convert.ToDecimal(drT[0]["RAmount"].ToString());
                                }

                                if (dRAmt > dRTAmt - dRRAmt)
                                {
                                    dRAmt = dRTAmt - dRRAmt;
                                }

                                if (drT.Length > 0)
                                {
                                    drT[0]["RAmount"] = dRRAmt + dRAmt;
                                }

                                if (dAmt == 0)
                                    dRPer = 0;
                                else
                                    dRPer = (dRAmt / dAmt) * 100;

                                dBalAmt = dBalAmt - dRAmt;

                                sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                        "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType +
                                        "'," + dRPer + "," + dRAmt + "," + dRAmt + ") SELECT SCOPE_IDENTITY();";
                                cmd = new SqlCommand(sSql, conn, tran);
                                iRSchId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                                cmd.Dispose();

                                if (bPayTypewise == false && sRSchType == "Q")
                                {
                                    sSql = "Insert Into dbo.PaySchTaxFlat(PaymentSchId,FlatId,QualifierId,Percentage,Amount,Sel) " +
                                            "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + dRPer + "," + dRAmt + ",'" + true + "')";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }

                                dQNetAmt = dRAmt;

                                dtQualT = new DataTable();
                                dv = new DataView(dtQual);
                                dv.RowFilter = "SchType = '" + sRSchType + "' and ReceiptTypeId = " + iReceiptId + " and OtherCostId = " + iROtherCostId;
                                dtQualT = dv.ToTable();
                                dv.Dispose();
                                if (dtQualT.Rows.Count > 0)
                                {
                                    QualVBC = new Collection();

                                    for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                                    {
                                        RAQual = new cRateQualR();
                                        bService = Convert.ToBoolean(dtQualT.Rows[Q]["Service"]);

                                        DataTable dtTDS = new DataTable();
                                        if (Convert.ToInt32(dtQualT.Rows[Q]["QualTypeId"]) == 2)
                                        {
                                            if (bService == true)
                                                dtTDS = GetSTSettings("G", m_dSchDate, conn, tran);
                                            else
                                                dtTDS = GetSTSettings("F", m_dSchDate, conn, tran);
                                        }
                                        else
                                        {
                                            dtTDS = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), m_dSchDate, "B", conn, tran);
                                        }

                                        RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                                        if (dtTDS.Rows.Count > 0)
                                        {
                                            RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                                            RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                                            RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                                            RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                                            RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                                            RAQual.HEDValue = Convert.ToDecimal(dtQualT.Rows[Q]["HEDValue"].ToString());
                                            RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Taxable"], CommFun.datatypes.vartypenumeric));
                                        }

                                        DataTable dtQ = new DataTable();
                                        dtQ = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), m_dSchDate, "B", conn, tran);
                                        //dtQ = QualifierSelect(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), "B", m_dSchDate, conn, tran);
                                        if (dtQ.Rows.Count > 0)
                                        {
                                            RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                                            RAQual.Amount = 0;
                                            RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                                        }

                                        QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                                    }

                                    Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                                    dQBaseAmt = dRAmt;
                                    dQNetAmt = dRAmt; decimal dTaxAmt = 0;
                                    decimal dVATAmt = 0;

                                    if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                                    {
                                        foreach (Qualifier.cRateQualR d in QualVBC)
                                        {
                                            sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount,HEDPer,HEDValue,NetPer,TaxableValue,TaxablePer) " +
                                                    "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                                    "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," +
                                                    " " + d.Amount + "," + d.HEDPer + "," + d.HEDValue + "," + d.NetPer + "," + d.TaxableValue + "," + d.TaxablePer + ")";
                                            cmd = new SqlCommand(sSql, conn, tran);
                                            cmd.ExecuteNonQuery();
                                            cmd.Dispose();
                                        }
                                    }

                                    if (bPayTypewise == true)
                                    {
                                        sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                }

                                dTNetAmt = dTNetAmt + dQNetAmt;

                            }

                            //if (dBalAmt <= 0) { break; }
                        }

                        if (dBalAmt > 0)
                        {
                            for (int j = 0; j < dtReceiptOrder.Rows.Count; j++)
                            {
                                dRAmt = dBalAmt;

                                sRSchType = dtReceiptOrder.Rows[j]["SchType"].ToString();
                                iReceiptId = Convert.ToInt32(dtReceiptOrder.Rows[j]["ReceiptTypeId"].ToString());
                                iROtherCostId = Convert.ToInt32(dtReceiptOrder.Rows[j]["OtherCostId"].ToString());

                                if (sRSchType == "O")
                                    drT = dtReceipt.Select("SchType = 'O' and Id = " + iROtherCostId + "");
                                else
                                    drT = dtReceipt.Select("SchType = 'R' and Id = " + iReceiptId + "");

                                decimal dRTAmt = 0;
                                decimal dRRAmt = 0;

                                if (drT.Length > 0)
                                {
                                    dRTAmt = Convert.ToDecimal(drT[0]["Amount"].ToString());
                                    dRRAmt = Convert.ToDecimal(drT[0]["RAmount"].ToString());
                                }

                                if (dRAmt > dRTAmt - dRRAmt)
                                {
                                    dRAmt = dRTAmt - dRRAmt;
                                }

                                if (drT.Length > 0)
                                {
                                    drT[0]["RAmount"] = dRRAmt + dRAmt;
                                }

                                if (dRAmt > 0)
                                {
                                    decimal dPCAmt = 0;
                                    bool bAns = false;
                                    sSql = "Select SchId,Amount,NetAmount from dbo.FlatReceiptType Where PaymentSchId = " + iPaymentSchId + " and " +
                                            "FlatId= " + argFlatId + " and ReceiptTypeId= " + iReceiptId + " and OtherCostId = " + iROtherCostId + " and SchType= '" + sRSchType + "'";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    sdr = cmd.ExecuteReader();
                                    DataTable dtP = new DataTable();
                                    dtP.Load(sdr);
                                    sdr.Close();
                                    cmd.Dispose();

                                    if (dtP.Rows.Count > 0)
                                    {
                                        dPCAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtP.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
                                        dTNetAmt = dTNetAmt - dPCAmt;
                                        dBalAmt = dBalAmt + dPCAmt;
                                        iRSchId = Convert.ToInt32(dtP.Rows[0]["SchId"].ToString());
                                        bAns = true;
                                    }
                                    dtP.Dispose();

                                    if (bAns == true)
                                    {
                                        dRAmt = dRAmt + dPCAmt;
                                        //modified
                                        if (dAmt == 0) { dRPer = 0; }
                                        else { dRPer = (dRAmt / dAmt) * 100; }

                                        sSql = "Update dbo.FlatReceiptType Set Amount= " + dRAmt + ",Percentage = " + dRPer + ",NetAmount = " + dRAmt + " Where SchId = " + iRSchId;
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();

                                        sSql = "Delete from dbo.FlatReceiptQualifier Where SchId = " + iRSchId;
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                    else
                                    {
                                        dRPer = (dRAmt / dAmt) * 100;

                                        sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                                "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + "," + dRAmt + "," + dRAmt + ") SELECT SCOPE_IDENTITY();";
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                                        cmd.Dispose();
                                    }

                                    dQNetAmt = dRAmt;

                                    dtQualT = new DataTable();
                                    dv = new DataView(dtQual);

                                    if (sRSchType == "O")
                                    {
                                        dv.RowFilter = "SchType = 'O' and ReceiptTypeId = 0 and OtherCostId = " + iROtherCostId + "";

                                    }
                                    else
                                    {
                                        dv.RowFilter = "SchType = 'R' and ReceiptTypeId = " + iReceiptId + " and OtherCostId = 0";
                                    }

                                    dtQualT = dv.ToTable();
                                    dv.Dispose();
                                    if (dtQualT.Rows.Count > 0)
                                    {
                                        QualVBC = new Collection();

                                        for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                                        {
                                            RAQual = new cRateQualR();
                                            bService = Convert.ToBoolean(dtQualT.Rows[Q]["Service"]);

                                            DataTable dtTDS = new DataTable();
                                            if (Convert.ToInt32(dtQualT.Rows[Q]["QualTypeId"]) == 2)
                                            {
                                                if (bService == true)
                                                    dtTDS = GetSTSettings("G", m_dSchDate, conn, tran);
                                                else
                                                    dtTDS = GetSTSettings("F", m_dSchDate, conn, tran);
                                            }
                                            else
                                            {
                                                dtTDS = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), m_dSchDate, "B", conn, tran);
                                            }

                                            RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                                            if (dtTDS.Rows.Count > 0)
                                            {
                                                RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                                                RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                                                RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                                                RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                                                RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                                                RAQual.HEDValue = Convert.ToDecimal(dtQualT.Rows[Q]["HEDValue"].ToString());
                                                RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Taxable"], CommFun.datatypes.vartypenumeric));
                                            }

                                            DataTable dtQ = new DataTable();
                                            dtQ = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), m_dSchDate, "B", conn, tran);
                                            //dtQ = QualifierSelect(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), "B", m_dSchDate, conn, tran);
                                            if (dtQ.Rows.Count > 0)
                                            {
                                                RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                                                RAQual.Amount = 0;
                                                RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                                            }

                                            QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                                        }

                                        Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                                        dQBaseAmt = dRAmt;
                                        dQNetAmt = dRAmt; decimal dTaxAmt = 0;
                                        decimal dVATAmt = 0;

                                        if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                                        {
                                            foreach (Qualifier.cRateQualR d in QualVBC)
                                            {
                                                sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount,HEDPer,HEDValue,NetPer,TaxableValue,TaxablePer) " +
                                                        "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                                        "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + "," + d.HEDPer + "," + d.HEDValue + "," + d.NetPer + "," + d.TaxableValue + "," + d.TaxablePer + ")";
                                                cmd = new SqlCommand(sSql, conn, tran);
                                                cmd.ExecuteNonQuery();
                                                cmd.Dispose();
                                            }
                                        }
                                        if (bPayTypewise == true)
                                        {
                                            sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                                            cmd = new SqlCommand(sSql, conn, tran);
                                            cmd.ExecuteNonQuery();
                                            cmd.Dispose();
                                        }
                                    }

                                    dTNetAmt = dTNetAmt + dQNetAmt;
                                    dBalAmt = dBalAmt - dRAmt;
                                    if (dBalAmt <= 0) { break; }
                                }
                            }
                        }

                        decimal dA = dAmt - dAdv;
                        if (sSchType != "R")
                        {
                            if (bPayTypewise == true)
                                sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dTNetAmt + "  Where PaymentSchId = " + iPaymentSchId;
                            else
                                sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dA + "  Where PaymentSchId = " + iPaymentSchId;
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                }
            }


            dt.Dispose();

            if (bAdvance == false)
            {
                sSql = "Insert into dbo.PaymentScheduleFlat(FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,Amount,NetAmount,PreStageTypeId,SortOrder) " +
                        "Values(" + argFlatId + ",0," + iCCId + ",'A','Advance',0,0,0,NULL,0," + dAdvAmt + ",0,0)";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }

            sSql = "Update dbo.PaymentScheduleFlat Set Advance=0";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "UPDATE PaymentScheduleFlat SET Advance=SummedQty FROM " +
                    " PaymentScheduleFlat A JOIN (SELECT PaymentSchId, SUM(NetAmount) SummedQty " +
                    " FROM FlatReceiptType WHERE SchType='A' GROUP BY PaymentSchId ) CCA ON A.PaymentSchId=CCA.PaymentSchId";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            //Schedule Date
            SqlDataReader dr, sdr1, sdr2; DataTable dt1; int iStgId = 0, iTempId = 0;
            int iDateAfter = 0, iDuration = 0; string sDurType = ""; DateTime SchDate; int iSortOrder = 0;
            DateTime StartDate = DateTime.Now; DateTime EndDate = DateTime.Now; DateTime FinaliseDate = DateTime.Now; int ipre = 0;


            sSql = "Update dbo.PaymentScheduleFlat Set PreStageTypeId=-1 Where FlatId=" + argFlatId + " And TemplateId In(  " +
                    " Select TemplateId From dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " " +
                    " And CostCentreId=" + iCCId + " And PreStageTypeId=-1)";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Select FinaliseDate from dbo.BuyerDetail Where FlatId=" + argFlatId + "";
            cmd = new SqlCommand(sSql, conn, tran);
            dr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(dr); cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                FinaliseDate = Convert.ToDateTime(dt.Rows[0]["FinaliseDate"]);


                sSql = "Select TemplateId,PreStageTypeId from dbo.PaymentScheduleFlat Where FlatId=" + argFlatId + " And PreStageTypeId=-1";
                cmd = new SqlCommand(sSql, conn, tran);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr); cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    iStgId = Convert.ToInt32(dt.Rows[0]["PreStageTypeId"]);
                    iTempId = Convert.ToInt32(dt.Rows[0]["TemplateId"]);
                }
                dt.Dispose();

                sSql = "Select SortOrder From dbo.PaymentScheduleFlat Where FlatId=" + argFlatId + " And TemplateId=" + iTempId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                sdr2 = cmd.ExecuteReader();
                dt1 = new DataTable();
                dt1.Load(sdr2); cmd.Dispose();
                dt1.Dispose();

                if (dt1.Rows.Count > 0)
                {
                    iSortOrder = Convert.ToInt32(dt1.Rows[0]["SortOrder"]);
                }

                sSql = "select StartDate,EndDate From ProjectInfo Where CostCentreId= " + iCCId;
                cmd = new SqlCommand(sSql, conn, tran);
                dt = new DataTable();
                dr = cmd.ExecuteReader();
                dt.Load(dr);
                dt.Dispose();

                if (dt.Rows.Count > 0)
                {
                    StartDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["StartDate"], CommFun.datatypes.VarTypeDate));
                    EndDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["EndDate"], CommFun.datatypes.VarTypeDate));
                }

                sSql = "Update dbo.PaymentScheduleFlat Set SchDate='" + FinaliseDate.ToString("dd-MMM-yyyy") + "'" +
                    " Where TemplateId=" + iTempId + " And FlatId=" + argFlatId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "Update dbo.PaymentScheduleFlat Set SchDate='" + FinaliseDate.ToString("dd-MMM-yyyy") + "'" +
                    " Where TemplateId=0 And FlatId=" + argFlatId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                if (iStgId == -1)
                {
                    if (iStgId == -1)
                    {
                        sSql = "Select A.PreStageTypeId,A.CostCentreId,A.TemplateId,A.DateAfter,A.Duration,A.Durationtype from dbo.PaymentScheduleFlat A" +
                        " Left Join dbo.ProgressBillRegister B On A.FlatId=B.FlatId " +
                        " Where A.FlatId=" + argFlatId + " And A.SortOrder>=" + iSortOrder + "" +
                        " And A.PaymentSchId Not In " +
                           " (Select PaySchId From dbo.ProgressBillRegister Where FlatId=" + argFlatId + ") Order By A.SortOrder";
                    }
                    cmd = new SqlCommand(sSql, conn, tran);
                    sdr1 = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(sdr1);
                    cmd.Dispose();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        iTempId = Convert.ToInt32(dt.Rows[i]["TemplateId"]);
                        ipre = Convert.ToInt32(dt.Rows[i]["PreStageTypeId"]);
                        iDateAfter = Convert.ToInt32(dt.Rows[i]["DateAfter"]);
                        iDuration = Convert.ToInt32(dt.Rows[i]["Duration"]);
                        sDurType = dt.Rows[i]["DurationType"].ToString();

                        if (ipre == -1) { } else if (ipre == -2) { } else if (ipre == -3) { } else if (ipre == 0) { } else { iTempId = ipre; }

                        sSql = "Select SchDate From dbo.PaymentScheduleFlat Where CostCentreId=" + dt.Rows[i]["CostCentreId"] + " And FlatId=" + argFlatId + "" +
                                " And TemplateId=" + iTempId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        DataTable dtDate = new DataTable();
                        dr = cmd.ExecuteReader();
                        dtDate.Load(dr);
                        dtDate.Dispose();

                        if (ipre == -1) { SchDate = Convert.ToDateTime(CommFun.IsNullCheck(FinaliseDate, CommFun.datatypes.VarTypeDate)); }
                        else if (ipre == -2) { SchDate = StartDate; }
                        else if (ipre == -3) { SchDate = EndDate; }
                        else
                            SchDate = Convert.ToDateTime(CommFun.IsNullCheck(dtDate.Rows[0]["SchDate"], CommFun.datatypes.VarTypeDate));

                        if (sDurType == "D")
                        { if (iDateAfter == 0) SchDate = SchDate.AddDays(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }
                        else if (sDurType == "M")
                        { if (iDateAfter == 0) SchDate = SchDate.AddMonths(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }


                        sSql = "Update dbo.PaymentScheduleFlat Set SchDate=@SchDate" +
                               " Where TemplateId=" + dt.Rows[i]["TemplateId"] + " And FlatId=" + argFlatId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        SqlParameter dateParameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@SchDate" };
                        if (SchDate == DateTime.MinValue)
                            dateParameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            dateParameter.Value = SchDate;
                        cmd.Parameters.Add(dateParameter);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                    }
                }
            }
        }

        public static void InsertFinalFlatScheduleI(int argFlatId, string argType, SqlConnection conn, SqlTransaction tran)
        {
            string sSql = "";

            SqlDataReader sdr;
            SqlCommand cmd;
            DataTable dt = new DataTable();
            DataTable dtTax = new DataTable();

            int iCCId = 0;
            int iFlatTypeId = 0;
            int iPayTypeId = 0;
            decimal dBaseAmt = 0;
            decimal dAdvAmt = 0;
            decimal dAdvBalAmt = 0;
            decimal dLandAmt = 0;
            decimal dNetAmt = 0;
            decimal dOtherAmt = 0;
            decimal dRAmt = 0;
            int iReceiptId = 0;
            int iROtherCostId = 0;
            string sRSchType = "";
            bool bAdvance = false;
            int iPaymentSchId = 0;
            string sSchType = "";
            int iOtherCostId = 0;
            decimal dRPer = 0;
            decimal dSchPercent = 0;
            decimal dQBaseAmt = 0;
            decimal dQNetAmt = 0;
            int iTemplateId = 0;
            int iSchId = 0;
            int iRSchId = 0;
            decimal dTNetAmt = 0;
            decimal dBalAmt = 0;
            bool bPayTypewise = false;
            //decimal dTaxPer = 0; decimal dTotSerTaxamt = 0; decimal dTotVatTaxamt = 0; decimal dSerTaxamt = 0; decimal dVatTaxamt = 0; bool bTaxSel = false;
            decimal dTotalTax = 0;
            //decimal dServiceTax = 0;
            //decimal dVATTax = 0;
            bool bService = false, bLCBon = false;
            DataRow[] drT;
            cRateQualR RAQual;
            Collection QualVBC;
            decimal dAdv = 0;
            DateTime m_dSchDate = DateTime.Now;
            DateTime FinaliseDate = DateTime.Now;
            DateTime BlockDate = DateTime.Now;

            DataTable dtReceipt = new DataTable();

            sSql = "Delete From dbo.PaymentSchedule Where TypeId IN(Select PayTypeId from dbo.FlatDetails Where FlatId=" + argFlatId + ") " +
                   " AND CostCentreId IN(Select CostCentreId from dbo.FlatDetails Where FlatId=" + argFlatId + ") AND SchType='R'";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete from dbo.PaymentScheduleFlat Where FlatId= " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete from dbo.FlatReceiptQualifier Where SchId in (Select SchId from dbo.FlatReceiptType Where FlatId= " + argFlatId + ")";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete from dbo.FlatReceiptType Where FlatId= " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete From dbo.PaySchTaxFlat Where FlatId=" + argFlatId + " ";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            if (argType == "S")
                sSql = "Select FinaliseDate from dbo.BuyerDetail Where Status='S' And FlatId=" + argFlatId + "";
            else if (argType == "B")
                sSql = "Select Date From dbo.BlockUnits Where BlockType='B' And FlatId=" + argFlatId + "";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0)
            {
                if (argType == "S") { FinaliseDate = Convert.ToDateTime(dt.Rows[0]["FinaliseDate"]); }
                else if (argType == "B") { BlockDate = Convert.ToDateTime(dt.Rows[0]["Date"]); }
            }
            dt.Dispose();

            //sSql = "Select FlatTypeId,CostCentreId,PayTypeId,BaseAmt,AdvAmount,USLandAmt from dbo.FlatDetails Where FlatId= " + argFlatId;//modified
            sSql = "Select FlatTypeId,CostCentreId,PayTypeId,BaseAmt,AdvAmount,LandRate,Guidelinevalue,USLandAmt from dbo.FlatDetails Where FlatId= " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0)
            {
                iCCId = Convert.ToInt32(dt.Rows[0]["CostCentreId"].ToString());
                iFlatTypeId = Convert.ToInt32(dt.Rows[0]["FlatTypeId"].ToString());
                iPayTypeId = Convert.ToInt32(dt.Rows[0]["PayTypeId"].ToString());
                bPayTypewise = FlatDetailsDL.GetTypewise(iPayTypeId);
                dBaseAmt = Convert.ToDecimal(dt.Rows[0]["BaseAmt"].ToString());
                dAdvAmt = Convert.ToDecimal(dt.Rows[0]["AdvAmount"].ToString());

                sSql = "Select LCBasedon From dbo.ProjectInfo Where CostCentreId= " + iCCId;
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                DataTable dtPI = new DataTable();
                dtPI.Load(sdr);
                sdr.Close();
                cmd.Dispose();
                if (dtPI.Rows.Count > 0) { bLCBon = Convert.ToBoolean(dtPI.Rows[0]["LCBasedon"]); }
                if (bLCBon == false) { dLandAmt = Convert.ToDecimal(dt.Rows[0]["LandRate"].ToString()); }
                else { dLandAmt = Convert.ToDecimal(dt.Rows[0]["USLandAmt"].ToString()); }
                //dLandAmt = Convert.ToDecimal(dt.Rows[0]["USLandAmt"].ToString());
                //dLandAmt = Convert.ToDecimal(dt.Rows[0]["LandRate"].ToString());
            }
            dt.Dispose();

            sSql = "Select SUM(RoundValue) From dbo.PaySchType Where TypeId=" + iPayTypeId + " ";
            cmd = new SqlCommand(sSql, conn, tran);
            decimal dRoundValue = Convert.ToDecimal(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
            cmd.Dispose();

            if (iPayTypeId > 0 && dRoundValue > 0)
            {
                sSql = "Select COUNT(*) From dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " AND CostCentreId=" + iCCId + " AND SchType='R'";
                cmd = new SqlCommand(sSql, conn, tran);
                int iCount = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                cmd.Dispose();

                if (iCount == 0)
                {
                    sSql = "Insert Into dbo.PaymentSchedule(TypeId,CostCentreId,SchType,Description,SchDescId,StageId, " +
                          " OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId,SortOrder,FlatTypeId,BlockId) " +
                          " Select TOP 1 " + iPayTypeId + "," + iCCId + ",'R','Final Amount to be Collect from Buyer',0,0,0,NULL,0,0,'',0,0,0, "+
                          " SortOrder+1,FlatTypeId,BlockId from dbo.PaymentSchedule " +
                          " Where TypeId=" + iPayTypeId + " AND CostCentreId=" + iCCId + " ORDER BY SortOrder DESC";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            else
            {
                sSql = "Delete From dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " AND CostCentreId=" + iCCId + " AND SchType='R'";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }

            sSql = "Select TemplateId From dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " and CostCentreId = " + iCCId + " and SchType='A'";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0) { bAdvance = true; }
            dt.Dispose();

            sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                    "Where FlatId = " + argFlatId + " and OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans Where PayTypeId=" + iPayTypeId + " and CostCentreId=" + iCCId + ")";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0) { dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }
            dt.Dispose();

            sSql = "Select QualifierId,Amount from dbo.FlatTax Where FlatId = " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtTx = new DataTable();
            dtTx.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            decimal dT = 0;
            if (dtTx.Rows.Count > 0)
            {
                for (int i = 0; i < dtTx.Rows.Count; i++)
                {
                    dTotalTax = Convert.ToDecimal(dtTx.Rows[i]["Amount"]);
                    dT = dT + dTotalTax;
                }
            }

            if (bPayTypewise == false)
            { dNetAmt = dBaseAmt + dOtherAmt + dT; }
            else
            { dNetAmt = dBaseAmt + dOtherAmt; }
            if (bAdvance == true) { dNetAmt = dNetAmt - dAdvAmt; }

            sSql = "Insert into dbo.PaymentScheduleFlat(FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId,SortOrder) " +
                    "Select " + argFlatId + ",TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId,SortOrder from dbo.PaymentSchedule " +
                    "Where TypeId=" + iPayTypeId + " and CostCentreId=" + iCCId +
                    " AND OtherCostId NOT IN(Select OtherCostId from dbo.OXGross Where CostCentreId=" + iCCId + ")";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();


            sSql = "Select ReceiptTypeId,OtherCostId,SchType from dbo.ReceiptTypeOrder " +
                    "Where PayTypeId = " + iPayTypeId + " and CostCentreId=" + iCCId + " and SchType <>'A' Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtReceiptOrder = new DataTable();
            dtReceiptOrder.Load(sdr);
            sdr.Close();
            cmd.Dispose();


            sSql = "Select OtherCostId,Flag,Amount from dbo.FlatOtherCost Where FlatId = " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtT = new DataTable();
            dtT.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            dtReceipt.Columns.Add("Id", typeof(int));
            dtReceipt.Columns.Add("SchType", typeof(string));
            dtReceipt.Columns.Add("Amount", typeof(decimal));
            dtReceipt.Columns.Add("RAmount", typeof(decimal));

            DataRow drR;
            drR = dtReceipt.NewRow();
            drR["Id"] = 1;
            drR["SchType"] = "A";
            drR["Amount"] = dAdvAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            drR = dtReceipt.NewRow();
            drR["Id"] = 2;
            drR["SchType"] = "R";
            drR["Amount"] = dLandAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            drR = dtReceipt.NewRow();
            drR["Id"] = 3;
            drR["SchType"] = "R";
            drR["Amount"] = dBaseAmt - dLandAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            for (int i = 0; i < dtT.Rows.Count; i++)
            {
                drR = dtReceipt.NewRow();
                drR["Id"] = Convert.ToInt32(dtT.Rows[i]["OtherCostId"].ToString());
                drR["SchType"] = "O";
                drR["Amount"] = Convert.ToDecimal(dtT.Rows[i]["Amount"].ToString());
                drR["RAmount"] = 0;
                dtReceipt.Rows.Add(drR);
            }

            if (bPayTypewise == false)
            {
                for (int i = 0; i < dtTx.Rows.Count; i++)
                {
                    drR = dtReceipt.NewRow();
                    drR["Id"] = Convert.ToInt32(dtTx.Rows[i]["QualifierId"].ToString());
                    drR["SchType"] = "Q";
                    drR["Amount"] = Convert.ToDecimal(dtTx.Rows[i]["Amount"].ToString());
                    drR["RAmount"] = 0;
                    dtReceipt.Rows.Add(drR);
                }
            }

            sSql = "Select SchId,TemplateId,ReceiptTypeId,Percentage,OtherCostId,SchType from dbo.CCReceiptType " +
                    "Where TemplateId in (Select TemplateId from dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " and CostCentreId=" + iCCId + ") Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtTemp = new DataTable();
            dtTemp.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            //sSql = "Select A.*,IsNull(B.Service,0)Service From dbo.CCReceiptQualifier A " +
            //        " Left Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId Where CostCentreId=" + iCCId;
            sSql = "Select C.QualTypeId,A.*,IsNull(B.Service,0)Service From dbo.CCReceiptQualifier A " +
                    " Left Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId " +
                    " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On C.QualifierId=A.QualifierId " +
                    " Where CostCentreId=" + iCCId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtQual = new DataTable();
            dtQual.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            sSql = "Select PaymentSchId,TemplateId,SchType,SchDate,OtherCostId,SchPercent from dbo.PaymentScheduleFlat Where FlatId = " + argFlatId + " Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            decimal dAmt = 0;
            DataView dv;
            decimal dAdvRAmt = 0;

            DataTable dtTempT;
            DataTable dtQualT;
            dAdvBalAmt = dAdvAmt;

            decimal dEMIRoundOff = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iPaymentSchId = Convert.ToInt32(dt.Rows[i]["PaymentSchId"].ToString());
                iTemplateId = Convert.ToInt32(dt.Rows[i]["TemplateId"].ToString());
                sSchType = dt.Rows[i]["SchType"].ToString();
                if (argType == "S")
                    m_dSchDate = FinaliseDate;
                else if (argType == "B")
                    m_dSchDate = BlockDate;
                if (m_dSchDate == DateTime.MinValue) { m_dSchDate = DateTime.Now; }
                //Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[i]["SchDate"], CommFun.datatypes.VarTypeDate));
                iOtherCostId = Convert.ToInt32(dt.Rows[i]["OtherCostId"].ToString());
                dSchPercent = Convert.ToDecimal(dt.Rows[i]["SchPercent"].ToString());
                dTNetAmt = 0;

                dAmt = 0;
                if (sSchType == "A")
                {
                    dAmt = dAdvAmt;
                }
                else if (sSchType == "O")
                {
                    dv = new DataView(dtT);
                    dv.RowFilter = "OtherCostId = " + iOtherCostId;
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        dAmt = Convert.ToDecimal(dv.ToTable().Rows[0]["Amount"].ToString());
                        if (dv.ToTable().Rows[0]["Flag"].ToString() == "-") { dAmt = dAmt * (-1); }
                    }
                    dv.Dispose();
                }
                else
                {
                    dAmt = dNetAmt * dSchPercent / 100;
                }

                dtTempT = new DataTable();
                dv = new DataView(dtTemp);
                dv.RowFilter = "TemplateId = " + iTemplateId;
                dtTempT = dv.ToTable();
                dv.Dispose();

                decimal dRoundOff = 0;
                decimal dRound = 0;
                if (dRoundValue > 0)
                {
                    dRoundOff = Math.Truncate(dAmt / dRoundValue) * dRoundValue;
                    dRound = dAmt - dRoundOff;
                    dEMIRoundOff = dEMIRoundOff + dRound;
                    dAmt = dRoundOff;
                }

                if (sSchType == "R")
                {
                    sSql = "Update dbo.PaymentScheduleFlat Set Amount=" + dEMIRoundOff + ",NetAmount=" + dEMIRoundOff +
                           " Where PaymentSchId=" + iPaymentSchId + " AND FlatId=" + argFlatId + " AND CostCentreId=" + iCCId + " AND SchType='R'";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    if (dtTempT.Rows.Count == 1 && sSchType == "O")
                    {
                        sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                "Values(" + iPaymentSchId + "," + argFlatId + ",0," + iOtherCostId + ",'" + sSchType + "',100," + dAmt + "," + dAmt + ") SELECT SCOPE_IDENTITY();";
                        cmd = new SqlCommand(sSql, conn, tran);
                        iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                        cmd.Dispose();

                        drT = dtReceipt.Select("SchType = 'O' and Id = " + iOtherCostId + "");

                        if (drT.Length > 0)
                        {
                            drT[0]["RAmount"] = dAmt;
                        }

                        dQNetAmt = dAmt;

                        dtQualT = new DataTable();
                        dv = new DataView(dtQual);
                        dv.RowFilter = "SchType = '" + sSchType + "' and OtherCostId = " + iOtherCostId;
                        dtQualT = dv.ToTable();
                        dv.Dispose();

                        if (dtQualT.Rows.Count > 0)
                        {
                            QualVBC = new Collection();

                            for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                            {
                                RAQual = new cRateQualR();
                                bService = Convert.ToBoolean(dtQualT.Rows[Q]["Service"]);

                                DataTable dtTDS = new DataTable();
                                if (Convert.ToInt32(dtQualT.Rows[Q]["QualTypeId"]) == 2)
                                {
                                    if (bService == true)
                                        dtTDS = GetSTSettings("G", m_dSchDate, conn, tran);
                                    else
                                        dtTDS = GetSTSettings("F", m_dSchDate, conn, tran);
                                }
                                else
                                {
                                    dtTDS = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), m_dSchDate, "B", conn, tran);
                                }

                                RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                                if (dtTDS.Rows.Count > 0)
                                {
                                    RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                                    RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                                    RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                                    RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                                    RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                                    RAQual.HEDValue = Convert.ToDecimal(dtQualT.Rows[Q]["HEDValue"].ToString());
                                    RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Taxable"], CommFun.datatypes.vartypenumeric));
                                }

                                DataTable dtQ = new DataTable();
                                dtQ = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), m_dSchDate, "B", conn, tran);
                                //dtQ = QualifierSelect(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), "B", m_dSchDate, conn, tran);
                                if (dtQ.Rows.Count > 0)
                                {
                                    RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                                    RAQual.Amount = 0;
                                    RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                                }

                                QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                            }

                            Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                            dQBaseAmt = dAmt;
                            dQNetAmt = dAmt; decimal dTaxAmt = 0;
                            decimal dVATAmt = 0;

                            if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, m_dSchDate, ref dVATAmt) == true)
                            {
                                foreach (Qualifier.cRateQualR d in QualVBC)
                                {
                                    sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount,HEDPer,HEDValue,NetPer,TaxablePer,TaxableValue) " +
                                            "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                            "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + "," +
                                            " " + d.HEDPer + "," + d.HEDValue + "," + d.NetPer + "," + d.TaxablePer + "," + d.TaxableValue + ")";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                            }

                            if (bPayTypewise == true)
                            {
                                sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }

                        if (bPayTypewise == true)
                            sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dQNetAmt + "  Where PaymentSchId = " + iPaymentSchId;
                        else
                            sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dAmt + "  Where PaymentSchId = " + iPaymentSchId;
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        dTNetAmt = dTNetAmt + dQNetAmt;
                    }
                    else
                    {
                        dBalAmt = dAmt;
                        for (int j = 0; j < dtTempT.Rows.Count; j++)
                        {
                            iSchId = Convert.ToInt32(dtTempT.Rows[j]["SchId"].ToString());
                            dRPer = Convert.ToDecimal(dtTempT.Rows[j]["Percentage"].ToString());
                            sRSchType = dtTempT.Rows[j]["SchType"].ToString();
                            iReceiptId = Convert.ToInt32(dtTempT.Rows[j]["ReceiptTypeId"].ToString());
                            iROtherCostId = Convert.ToInt32(dtTempT.Rows[j]["OtherCostId"].ToString());

                            if (dRPer != 0) { dRAmt = dAmt * dRPer / 100; }
                            else { dRAmt = dBalAmt; }

                            if (dRAmt > dBalAmt) { dRAmt = dBalAmt; }


                            if (sRSchType == "A" && bAdvance == false)
                            {
                                dAdvRAmt = dAdvAmt * dRPer / 100;
                                if (dAdvRAmt > dAdvBalAmt) { dAdvRAmt = dAdvBalAmt; }
                                dAdvBalAmt = dAdvBalAmt - dAdvRAmt;
                                dTNetAmt = dTNetAmt - dAdvRAmt;

                                dAdv = dAdvRAmt;
                                sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                        "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + ", 0," + dAdvRAmt + ") SELECT SCOPE_IDENTITY();";
                                cmd = new SqlCommand(sSql, conn, tran);
                                iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                                cmd.Dispose();

                            }

                            else
                            {
                                dAdv = 0;
                                if (sRSchType == "A")
                                {
                                    drT = dtReceipt.Select("SchType='A'");
                                }
                                else if (sRSchType == "O")
                                {
                                    drT = dtReceipt.Select("SchType='O' and Id = " + iROtherCostId + "");
                                }
                                else if (sRSchType == "Q")
                                {
                                    drT = dtReceipt.Select("SchType='Q' and Id = " + iReceiptId + "");
                                }
                                else
                                {
                                    drT = dtReceipt.Select("SchType='R' and Id = " + iReceiptId + "");
                                }


                                decimal dRTAmt = 0;
                                decimal dRRAmt = 0;

                                if (drT.Length > 0)
                                {
                                    dRTAmt = Convert.ToDecimal(drT[0]["Amount"].ToString());
                                    dRRAmt = Convert.ToDecimal(drT[0]["RAmount"].ToString());
                                }

                                if (dRAmt > dRTAmt - dRRAmt)
                                {
                                    dRAmt = dRTAmt - dRRAmt;
                                }

                                if (drT.Length > 0)
                                {
                                    drT[0]["RAmount"] = dRRAmt + dRAmt;
                                }

                                if (dAmt == 0) { dRPer = 0; }
                                else dRPer = (dRAmt / dAmt) * 100;

                                dBalAmt = dBalAmt - dRAmt;

                                sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                        "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + "," + dRAmt + "," + dRAmt + ") SELECT SCOPE_IDENTITY();";
                                cmd = new SqlCommand(sSql, conn, tran);
                                iRSchId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                                cmd.Dispose();

                                if (bPayTypewise == false && sRSchType == "Q")
                                {
                                    sSql = "Insert Into dbo.PaySchTaxFlat(PaymentSchId,FlatId,QualifierId,Percentage,Amount,Sel) " +
                                            "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + dRPer + "," + dRAmt + ",'" + true + "')";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                                dQNetAmt = dRAmt;

                                dtQualT = new DataTable();
                                dv = new DataView(dtQual);
                                dv.RowFilter = "SchType = '" + sRSchType + "' and ReceiptTypeId = " + iReceiptId + " and OtherCostId = " + iROtherCostId;
                                dtQualT = dv.ToTable();
                                dv.Dispose();
                                if (dtQualT.Rows.Count > 0)
                                {
                                    QualVBC = new Collection();

                                    for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                                    {
                                        RAQual = new cRateQualR();
                                        bService = Convert.ToBoolean(dtQualT.Rows[Q]["Service"]);

                                        DataTable dtTDS = new DataTable();
                                        if (Convert.ToInt32(dtQualT.Rows[Q]["QualTypeId"]) == 2)
                                        {
                                            if (bService == true)
                                                dtTDS = GetSTSettings("G", m_dSchDate, conn, tran);
                                            else
                                                dtTDS = GetSTSettings("F", m_dSchDate, conn, tran);
                                        }
                                        else
                                        {
                                            dtTDS = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), m_dSchDate, "B", conn, tran);
                                        }

                                        RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                                        if (dtTDS.Rows.Count > 0)
                                        {
                                            RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                                            RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                                            RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                                            RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                                            RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                                            RAQual.HEDValue = Convert.ToDecimal(dtQualT.Rows[Q]["HEDValue"].ToString());
                                            RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Taxable"], CommFun.datatypes.vartypenumeric));
                                        }

                                        DataTable dtQ = new DataTable();
                                        dtQ = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), m_dSchDate, "B", conn, tran);
                                        //dtQ = QualifierSelect(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), "B", m_dSchDate, conn, tran);
                                        if (dtQ.Rows.Count > 0)
                                        {
                                            RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                                            RAQual.Amount = 0;
                                            RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                                        }

                                        QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                                    }

                                    Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                                    dQBaseAmt = dRAmt;
                                    dQNetAmt = dRAmt; decimal dTaxAmt = 0;
                                    decimal dVATAmt = 0;

                                    if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                                    {
                                        foreach (Qualifier.cRateQualR d in QualVBC)
                                        {
                                            sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount,HEDPer,HEDValue,NetPer,TaxablePer,TaxableValue) " +
                                                    "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                                    "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," +
                                                    " " + d.Amount + "," + d.HEDPer + "," + d.HEDValue + "," + d.NetPer + "," + d.TaxablePer + "," + d.TaxableValue + ")";
                                            cmd = new SqlCommand(sSql, conn, tran);
                                            cmd.ExecuteNonQuery();
                                            cmd.Dispose();
                                        }
                                    }

                                    if (bPayTypewise == true)
                                    {
                                        sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                }

                                dTNetAmt = dTNetAmt + dQNetAmt;

                            }

                            //if (dBalAmt <= 0) { break; }
                        }

                        if (dBalAmt > 0)
                        {
                            for (int j = 0; j < dtReceiptOrder.Rows.Count; j++)
                            {
                                dRAmt = dBalAmt;

                                sRSchType = dtReceiptOrder.Rows[j]["SchType"].ToString();
                                iReceiptId = Convert.ToInt32(dtReceiptOrder.Rows[j]["ReceiptTypeId"].ToString());
                                iROtherCostId = Convert.ToInt32(dtReceiptOrder.Rows[j]["OtherCostId"].ToString());

                                if (sRSchType == "O")
                                {
                                    drT = dtReceipt.Select("SchType = 'O' and Id = " + iROtherCostId + "");
                                }
                                else
                                {
                                    drT = dtReceipt.Select("SchType = 'R' and Id = " + iReceiptId + "");
                                }

                                decimal dRTAmt = 0;
                                decimal dRRAmt = 0;

                                if (drT.Length > 0)
                                {
                                    dRTAmt = Convert.ToDecimal(drT[0]["Amount"].ToString());
                                    dRRAmt = Convert.ToDecimal(drT[0]["RAmount"].ToString());
                                }

                                if (dRAmt > dRTAmt - dRRAmt)
                                {
                                    dRAmt = dRTAmt - dRRAmt;
                                }

                                if (drT.Length > 0)
                                {
                                    drT[0]["RAmount"] = dRRAmt + dRAmt;
                                }

                                if (dRAmt > 0)
                                {
                                    decimal dPCAmt = 0;
                                    bool bAns = false;
                                    sSql = "Select SchId,Amount,NetAmount from dbo.FlatReceiptType Where PaymentSchId = " + iPaymentSchId + " and " +
                                            "FlatId= " + argFlatId + " and ReceiptTypeId= " + iReceiptId + " and OtherCostId = " + iROtherCostId + " and SchType= '" + sRSchType + "'";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    sdr = cmd.ExecuteReader();
                                    DataTable dtP = new DataTable();
                                    dtP.Load(sdr);
                                    sdr.Close();
                                    cmd.Dispose();

                                    if (dtP.Rows.Count > 0)
                                    {
                                        dPCAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtP.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
                                        dTNetAmt = dTNetAmt - dPCAmt;
                                        dBalAmt = dBalAmt + dPCAmt;
                                        iRSchId = Convert.ToInt32(dtP.Rows[0]["SchId"].ToString());
                                        bAns = true;
                                    }
                                    dtP.Dispose();

                                    if (bAns == true)
                                    {
                                        dRAmt = dRAmt + dPCAmt;
                                        //modified
                                        if (dAmt == 0) { dRPer = 0; }
                                        else { dRPer = (dRAmt / dAmt) * 100; }

                                        sSql = "Update dbo.FlatReceiptType Set Amount= " + dRAmt + ",Percentage = " + dRPer + ",NetAmount = " + dRAmt + " Where SchId = " + iRSchId;
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();

                                        sSql = "Delete from dbo.FlatReceiptQualifier Where SchId = " + iRSchId;
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                    else
                                    {
                                        dRPer = (dRAmt / dAmt) * 100;

                                        sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                                "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + "," + dRAmt + "," + dRAmt + ") SELECT SCOPE_IDENTITY();";
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                                        cmd.Dispose();
                                    }

                                    dQNetAmt = dRAmt;

                                    dtQualT = new DataTable();
                                    dv = new DataView(dtQual);

                                    if (sRSchType == "O")
                                        dv.RowFilter = "SchType = 'O' and ReceiptTypeId = 0 and OtherCostId = " + iROtherCostId + "";
                                    else
                                        dv.RowFilter = "SchType = 'R' and ReceiptTypeId = " + iReceiptId + " and OtherCostId = 0";

                                    dtQualT = dv.ToTable();
                                    dv.Dispose();
                                    if (dtQualT.Rows.Count > 0)
                                    {
                                        QualVBC = new Collection();

                                        for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                                        {
                                            RAQual = new cRateQualR();
                                            bService = Convert.ToBoolean(dtQualT.Rows[Q]["Service"]);

                                            DataTable dtTDS = new DataTable();
                                            if (Convert.ToInt32(dtQualT.Rows[Q]["QualTypeId"]) == 2)
                                            {
                                                if (bService == true)
                                                    dtTDS = GetSTSettings("G", m_dSchDate, conn, tran);
                                                else
                                                    dtTDS = GetSTSettings("F", m_dSchDate, conn, tran);
                                            }
                                            else
                                            {
                                                dtTDS = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), m_dSchDate, "B", conn, tran);
                                            }

                                            RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                                            if (dtTDS.Rows.Count > 0)
                                            {
                                                RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                                                RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                                                RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                                                RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                                                RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                                                RAQual.HEDValue = Convert.ToDecimal(dtQualT.Rows[Q]["HEDValue"].ToString());
                                                RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Taxable"], CommFun.datatypes.vartypenumeric));
                                            }

                                            DataTable dtQ = new DataTable();
                                            dtQ = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), m_dSchDate, "B", conn, tran);
                                            //dtQ = QualifierSelect(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), "B", m_dSchDate, conn, tran);
                                            if (dtQ.Rows.Count > 0)
                                            {
                                                RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                                                RAQual.Amount = 0;
                                                RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                                            }

                                            QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                                        }

                                        Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                                        dQBaseAmt = dRAmt;
                                        dQNetAmt = dRAmt; decimal dTaxAmt = 0;
                                        decimal dVATAmt = 0;

                                        if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                                        {
                                            foreach (Qualifier.cRateQualR d in QualVBC)
                                            {
                                                sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount,HEDPer,HEDValue,NetPer,TaxablePer,TaxableValue) " +
                                                        "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                                        "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + "," + d.HEDPer + "," + d.HEDValue + "," + d.NetPer + "," + d.TaxablePer + "," + d.TaxableValue + ")";
                                                cmd = new SqlCommand(sSql, conn, tran);
                                                cmd.ExecuteNonQuery();
                                                cmd.Dispose();
                                            }
                                        }
                                        if (bPayTypewise == true)
                                        {
                                            sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                                            cmd = new SqlCommand(sSql, conn, tran);
                                            cmd.ExecuteNonQuery();
                                            cmd.Dispose();
                                        }
                                    }

                                    dTNetAmt = dTNetAmt + dQNetAmt;
                                    dBalAmt = dBalAmt - dRAmt;
                                    if (dBalAmt <= 0) { break; }
                                }
                            }
                        }

                        decimal dA = dAmt - dAdv;
                        if (sSchType != "R")
                        {
                            if (bPayTypewise == true)
                                sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dTNetAmt + "  Where PaymentSchId = " + iPaymentSchId;
                            else
                                sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dA + "  Where PaymentSchId = " + iPaymentSchId;
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                }
            }
            dt.Dispose();

            if (bAdvance == false)
            {
                sSql = "Insert into dbo.PaymentScheduleFlat(FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,Amount,NetAmount,PreStageTypeId,SortOrder) " +
                        "Values(" + argFlatId + ",0," + iCCId + ",'A','Advance',0,0,0,NULL,0," + dAdvAmt + ",0,0)";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }

            sSql = "Update dbo.PaymentScheduleFlat Set Advance=0";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "UPDATE PaymentScheduleFlat SET Advance=SummedQty FROM " +
                    " PaymentScheduleFlat A JOIN (SELECT PaymentSchId, SUM(NetAmount) SummedQty " +
                    " FROM FlatReceiptType WHERE SchType='A' GROUP BY PaymentSchId ) CCA ON A.PaymentSchId=CCA.PaymentSchId";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            //Schedule Date
            SqlDataReader dr, sdr1, sdr2; DataTable dt1; int iStgId = 0, iTempId = 0;
            int iDateAfter = 0, iDuration = 0; string sDurType = ""; DateTime SchDate; int iSortOrder = 0;
            DateTime StartDate = DateTime.Now; DateTime EndDate = DateTime.Now; int ipre = 0;


            sSql = "Update dbo.PaymentScheduleFlat Set PreStageTypeId=-1 Where FlatId=" + argFlatId + " And TemplateId In(  " +
                    " Select TemplateId From dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " " +
                    " And CostCentreId=" + iCCId + " And PreStageTypeId=-1)";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Select FinaliseDate from dbo.BuyerDetail Where Status='S' And FlatId=" + argFlatId + "";
            cmd = new SqlCommand(sSql, conn, tran);
            dr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(dr); cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                FinaliseDate = Convert.ToDateTime(dt.Rows[0]["FinaliseDate"]);


                sSql = "Select TemplateId,PreStageTypeId from dbo.PaymentScheduleFlat Where FlatId=" + argFlatId + " And PreStageTypeId=-1";
                cmd = new SqlCommand(sSql, conn, tran);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr); cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    iStgId = Convert.ToInt32(dt.Rows[0]["PreStageTypeId"]);
                    iTempId = Convert.ToInt32(dt.Rows[0]["TemplateId"]);
                }
                dt.Dispose();

                sSql = "Select SortOrder From dbo.PaymentScheduleFlat Where FlatId=" + argFlatId + " And TemplateId=" + iTempId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                sdr2 = cmd.ExecuteReader();
                dt1 = new DataTable();
                dt1.Load(sdr2); cmd.Dispose();
                dt1.Dispose();

                if (dt1.Rows.Count > 0)
                {
                    iSortOrder = Convert.ToInt32(dt1.Rows[0]["SortOrder"]);
                }

                sSql = "select StartDate,EndDate From ProjectInfo Where CostCentreId= " + iCCId;
                cmd = new SqlCommand(sSql, conn, tran);
                dt = new DataTable();
                dr = cmd.ExecuteReader();
                dt.Load(dr);
                dt.Dispose();

                if (dt.Rows.Count > 0)
                {
                    StartDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["StartDate"], CommFun.datatypes.VarTypeDate));
                    EndDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["EndDate"], CommFun.datatypes.VarTypeDate));
                }

                sSql = "Update dbo.PaymentScheduleFlat Set SchDate='" + FinaliseDate.ToString("dd-MMM-yyyy") + "'" +
                    " Where TemplateId=" + iTempId + " And FlatId=" + argFlatId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "Update dbo.PaymentScheduleFlat Set SchDate='" + FinaliseDate.ToString("dd-MMM-yyyy") + "'" +
                    " Where TemplateId=0 And FlatId=" + argFlatId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                if (iStgId == -1)
                {
                    if (iStgId == -1)
                        sSql = "Select A.PreStageTypeId,A.CostCentreId,A.TemplateId,A.DateAfter,A.Duration,A.Durationtype from dbo.PaymentScheduleFlat A" +
                        " Left Join dbo.ProgressBillRegister B On A.FlatId=B.FlatId " +
                        " Where A.FlatId=" + argFlatId + " And A.SortOrder>=" + iSortOrder + "" +
                        " And A.PaymentSchId Not In " +
                        " (Select PaySchId From dbo.ProgressBillRegister Where FlatId=" + argFlatId + ") Order By A.SortOrder";

                    cmd = new SqlCommand(sSql, conn, tran);
                    sdr1 = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(sdr1);
                    cmd.Dispose();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        iTempId = Convert.ToInt32(dt.Rows[i]["TemplateId"]);
                        ipre = Convert.ToInt32(dt.Rows[i]["PreStageTypeId"]);
                        iDateAfter = Convert.ToInt32(dt.Rows[i]["DateAfter"]);
                        iDuration = Convert.ToInt32(dt.Rows[i]["Duration"]);
                        sDurType = dt.Rows[i]["DurationType"].ToString();

                        if (ipre == -1) { } else if (ipre == -2) { } else if (ipre == -3) { } else if (ipre == 0) { } else { iTempId = ipre; }

                        sSql = "Select SchDate From dbo.PaymentScheduleFlat Where CostCentreId=" + dt.Rows[i]["CostCentreId"] + " And FlatId=" + argFlatId + "" +
                                " And TemplateId=" + iTempId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        DataTable dtDate = new DataTable();
                        dr = cmd.ExecuteReader();
                        dtDate.Load(dr);
                        dr.Close();
                        cmd.Dispose();

                        if (ipre == -1) { SchDate = Convert.ToDateTime(CommFun.IsNullCheck(FinaliseDate, CommFun.datatypes.VarTypeDate)); }
                        else if (ipre == -2) { SchDate = StartDate; }
                        else if (ipre == -3) { SchDate = EndDate; }
                        else
                            SchDate = Convert.ToDateTime(CommFun.IsNullCheck(dtDate.Rows[0]["SchDate"], CommFun.datatypes.VarTypeDate));

                        if (sDurType == "D")
                        { if (iDateAfter == 0) SchDate = SchDate.AddDays(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }
                        else if (sDurType == "M")
                        { if (iDateAfter == 0) SchDate = SchDate.AddMonths(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }


                        sSql = "Update dbo.PaymentScheduleFlat Set SchDate=@SchDate" +
                            " Where TemplateId=" + dt.Rows[i]["TemplateId"] + " And FlatId=" + argFlatId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        SqlParameter dateParameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName="@SchDate" };
                        if (SchDate == DateTime.MinValue)
                            dateParameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            dateParameter.Value = SchDate;
                        cmd.Parameters.Add(dateParameter);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                    }
                }
            }
        }

        public static void UpdateBuyerSchedule(int argFlatId, DataTable argdt, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                string sSql = "";

                SqlDataReader sdr;
                SqlCommand cmd;
                DataTable dt = new DataTable();

                int iCCId = 0;
                int iFlatTypeId = 0;
                int iPayTypeId = 0;
                decimal dBaseAmt = 0;
                decimal dAdvAmt = 0;
                decimal dAdvBalAmt = 0;
                decimal dLandAmt = 0;
                decimal dNetAmt = 0;
                decimal dOtherAmt = 0;
                decimal dRAmt = 0;
                int iReceiptId = 0;
                int iROtherCostId = 0;
                string sRSchType = "";
                bool bAdvance = false;
                int iPaymentSchId = 0;
                DateTime dSchDate = DateTime.Now;
                string sSchType = "";
                int iOtherCostId = 0;
                decimal dRPer = 0;
                decimal dSchPercent = 0;
                decimal dQBaseAmt = 0;
                decimal dQNetAmt = 0;
                int iTemplateId = 0;
                int iSchId = 0;
                int iRSchId = 0;
                decimal dTNetAmt = 0;
                decimal dBalAmt = 0;
                bool bService = false, bLCBon = false;
                DataRow[] drT;
                cRateQualR RAQual;
                Collection QualVBC;

                DataTable dtReceipt = new DataTable();

                sSql = "Delete From dbo.PaymentSchedule Where TypeId IN(Select PayTypeId from dbo.FlatDetails Where FlatId=" + argFlatId + ") " +
                       " AND CostCentreId IN(Select CostCentreId from dbo.FlatDetails Where FlatId=" + argFlatId + ") AND SchType='R'";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "Delete from dbo.PaymentScheduleFlat Where FlatId= " + argFlatId;
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "Delete from dbo.FlatReceiptQualifier Where SchId in (Select SchId from dbo.FlatReceiptType Where FlatId= " + argFlatId + ")";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "Delete from dbo.FlatReceiptType Where FlatId= " + argFlatId;
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();


                //sSql = "Select FlatTypeId,CostCentreId,PayTypeId,BaseAmt,AdvAmount,USLandAmt from dbo.FlatDetails Where FlatId= " + argFlatId;//modified
                sSql = "Select FlatTypeId,CostCentreId,PayTypeId,BaseAmt,AdvAmount,LandRate,Guidelinevalue,USLandAmt from dbo.FlatDetails Where FlatId= " + argFlatId;
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    iCCId = Convert.ToInt32(dt.Rows[0]["CostCentreId"].ToString());
                    iFlatTypeId = Convert.ToInt32(dt.Rows[0]["FlatTypeId"].ToString());
                    iPayTypeId = Convert.ToInt32(dt.Rows[0]["PayTypeId"].ToString());
                    dBaseAmt = Convert.ToDecimal(dt.Rows[0]["BaseAmt"].ToString());
                    dAdvAmt = Convert.ToDecimal(dt.Rows[0]["AdvAmount"].ToString());

                    sSql = "Select LCBasedon From dbo.ProjectInfo Where CostCentreId= " + iCCId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    sdr = cmd.ExecuteReader();
                    DataTable dtPI = new DataTable();
                    dtPI.Load(sdr);
                    sdr.Close();
                    cmd.Dispose();
                    if (dtPI.Rows.Count > 0) { bLCBon = Convert.ToBoolean(dtPI.Rows[0]["LCBasedon"]); }
                    if (bLCBon == false) { dLandAmt = Convert.ToDecimal(dt.Rows[0]["LandRate"].ToString()); }
                    else { dLandAmt = Convert.ToDecimal(dt.Rows[0]["USLandAmt"].ToString()); }

                    //dLandAmt = Convert.ToDecimal(dt.Rows[0]["USLandAmt"].ToString());
                    //dLandAmt = Convert.ToDecimal(dt.Rows[0]["LandRate"].ToString());
                }
                dt.Dispose();

                sSql = "Select SUM(RoundValue) From dbo.PaySchType Where TypeId=" + iPayTypeId + " ";
                cmd = new SqlCommand(sSql, conn, tran);
                decimal dRoundValue = Convert.ToDecimal(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                cmd.Dispose();

                if (iPayTypeId > 0 && dRoundValue > 0)
                {
                    sSql = "Select COUNT(*) From dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " AND CostCentreId=" + iCCId + " AND SchType='R'";
                    cmd = new SqlCommand(sSql, conn, tran);
                    int iCount = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                    cmd.Dispose();

                    if (iCount == 0)
                    {
                        sSql = "Insert Into dbo.PaymentSchedule(TypeId,CostCentreId,SchType,Description,SchDescId,StageId, " +
                              " OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId,SortOrder,FlatTypeId,BlockId) " +
                              " Select TOP 1 " + iPayTypeId + "," + iCCId + ",'R','Final Amount to be Collect from Buyer',0,0,0,NULL,0,0,'',0,0,0, " +
                              " SortOrder+1,FlatTypeId,BlockId from dbo.PaymentSchedule " +
                              " Where TypeId=" + iPayTypeId + " AND CostCentreId=" + iCCId + " ORDER BY SortOrder DESC";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
                else
                {
                    sSql = "Delete From dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " AND CostCentreId=" + iCCId + " AND SchType='R'";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                sSql = "Select TemplateId From dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " and CostCentreId = " + iCCId + " and SchType='A'";
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0) { bAdvance = true; }
                dt.Dispose();

                sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                        "Where FlatId = " + argFlatId + " and OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans Where PayTypeId=" + iPayTypeId + " and CostCentreId=" + iCCId + ")";
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0) { dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }
                dt.Dispose();

                dNetAmt = dBaseAmt + dOtherAmt;
                if (bAdvance == true) { dNetAmt = dNetAmt - dAdvAmt; }

                if (argdt.Rows.Count > 0)
                {
                    for (int i = 0; i < argdt.Rows.Count; i++)
                    {
                        string sDate = string.Format(Convert.ToDateTime(CommFun.IsNullCheck(argdt.Rows[i]["SchDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MMM-yyyy"));
                        if (argdt.Rows[i]["SchDate"].ToString() == "")
                        {
                            sDate = "NULL";
                            sSql = "Insert into dbo.PaymentScheduleFlat(FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId," +
                                    " OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId,SortOrder) " +
                                    " Values(" + argdt.Rows[i]["FlatId"] + "," + argdt.Rows[i]["TemplateId"] + "," + argdt.Rows[i]["CostCentreId"] + "," +
                                    " '" + argdt.Rows[i]["SchType"] + "','" + argdt.Rows[i]["Description"] + "'," + argdt.Rows[i]["SchDescId"] + "," +
                                    " " + argdt.Rows[i]["StageId"] + "," + argdt.Rows[i]["OtherCostId"] + "," + sDate + "," +
                                    " '" + argdt.Rows[i]["DateAfter"] + "'," + argdt.Rows[i]["Duration"] + ",'" + argdt.Rows[i]["DurationType"] + "'," +
                                    " " + argdt.Rows[i]["SchPercent"] + "," + argdt.Rows[i]["Amount"] + "," + argdt.Rows[i]["PreStageTypeId"] + "," +
                                    " " + argdt.Rows[i]["SortOrder"] + ")";
                        }
                        else
                        {
                            sSql = "Insert into dbo.PaymentScheduleFlat(FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId," +
                                    " OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId,SortOrder) " +
                                    " Values(" + argdt.Rows[i]["FlatId"] + "," + argdt.Rows[i]["TemplateId"] + "," + argdt.Rows[i]["CostCentreId"] + "," +
                                    " '" + argdt.Rows[i]["SchType"] + "','" + argdt.Rows[i]["Description"] + "'," + argdt.Rows[i]["SchDescId"] + "," +
                                    " " + argdt.Rows[i]["StageId"] + "," + argdt.Rows[i]["OtherCostId"] + ",'" + sDate + "'," +
                                    " '" + argdt.Rows[i]["DateAfter"] + "'," + argdt.Rows[i]["Duration"] + ",'" + argdt.Rows[i]["DurationType"] + "'," +
                                    " " + argdt.Rows[i]["SchPercent"] + "," + argdt.Rows[i]["Amount"] + "," + argdt.Rows[i]["PreStageTypeId"] + "," +
                                    " " + argdt.Rows[i]["SortOrder"] + ")";
                        }
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }

                sSql = "Select ReceiptTypeId,OtherCostId,SchType from dbo.ReceiptTypeOrder " +
                        "Where PayTypeId = " + iPayTypeId + " and CostCentreId=" + iCCId + " and SchType <>'A' Order by SortOrder";
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                DataTable dtReceiptOrder = new DataTable();
                dtReceiptOrder.Load(sdr);
                sdr.Close();
                cmd.Dispose();


                sSql = "Select OtherCostId,Flag,Amount from dbo.FlatOtherCost Where FlatId = " + argFlatId;
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                DataTable dtT = new DataTable();
                dtT.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                dtReceipt.Columns.Add("Id", typeof(int));
                dtReceipt.Columns.Add("SchType", typeof(string));
                dtReceipt.Columns.Add("Amount", typeof(decimal));
                dtReceipt.Columns.Add("RAmount", typeof(decimal));

                DataRow drR;
                drR = dtReceipt.NewRow();
                drR["Id"] = 1;
                drR["SchType"] = "A";
                drR["Amount"] = dAdvAmt;
                drR["RAmount"] = 0;
                dtReceipt.Rows.Add(drR);

                drR = dtReceipt.NewRow();
                drR["Id"] = 2;
                drR["SchType"] = "R";
                drR["Amount"] = dLandAmt;
                drR["RAmount"] = 0;
                dtReceipt.Rows.Add(drR);

                drR = dtReceipt.NewRow();
                drR["Id"] = 3;
                drR["SchType"] = "R";
                drR["Amount"] = dBaseAmt - dLandAmt;
                drR["RAmount"] = 0;
                dtReceipt.Rows.Add(drR);

                for (int i = 0; i < dtT.Rows.Count; i++)
                {
                    drR = dtReceipt.NewRow();
                    drR["Id"] = Convert.ToInt32(dtT.Rows[i]["OtherCostId"].ToString());
                    drR["SchType"] = "O";
                    drR["Amount"] = Convert.ToDecimal(dtT.Rows[i]["Amount"].ToString());
                    drR["RAmount"] = 0;
                    dtReceipt.Rows.Add(drR);
                }

                sSql = "Select SchId,TemplateId,ReceiptTypeId,Percentage,OtherCostId,SchType from dbo.CCReceiptType " +
                        "Where TemplateId in (Select TemplateId from dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " and CostCentreId=" + iCCId + ") Order by SortOrder";
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                DataTable dtTemp = new DataTable();
                dtTemp.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                //sSql = "Select A.*,IsNull(B.Service,0)Service From dbo.CCReceiptQualifier A " +
                //        " Left Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId Where CostCentreId=" + iCCId;
                sSql = "Select C.QualTypeId,A.*,IsNull(B.Service,0)Service From dbo.CCReceiptQualifier A " +
                        " Left Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId " +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On C.QualifierId=A.QualifierId " +
                        " Where CostCentreId=" + iCCId;
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                DataTable dtQual = new DataTable();
                dtQual.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                sSql = "Select PaymentSchId,TemplateId,SchDate,SchType,OtherCostId,SchPercent from dbo.PaymentScheduleFlat Where FlatId = " + argFlatId + " Order by SortOrder";
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                decimal dAmt = 0;
                DataView dv;
                decimal dAdvRAmt = 0;

                DataTable dtTempT;
                DataTable dtQualT;
                dAdvBalAmt = dAdvAmt;

                decimal dEMIRoundOff = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    iPaymentSchId = Convert.ToInt32(dt.Rows[i]["PaymentSchId"].ToString());
                    iTemplateId = Convert.ToInt32(dt.Rows[i]["TemplateId"].ToString());
                    sSchType = dt.Rows[i]["SchType"].ToString();
                    dSchDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[i]["SchDate"], CommFun.datatypes.VarTypeDate));
                    if (dSchDate == DateTime.MinValue) { dSchDate = DateTime.Now; }
                    iOtherCostId = Convert.ToInt32(dt.Rows[i]["OtherCostId"].ToString());
                    dSchPercent = Convert.ToDecimal(dt.Rows[i]["SchPercent"].ToString());
                    dTNetAmt = 0;

                    dAmt = 0;
                    if (sSchType == "A")
                    {
                        dAmt = dAdvAmt;
                    }
                    else if (sSchType == "O")
                    {
                        dv = new DataView(dtT);
                        dv.RowFilter = "OtherCostId = " + iOtherCostId;
                        if (dv.ToTable().Rows.Count > 0)
                        {
                            dAmt = Convert.ToDecimal(dv.ToTable().Rows[0]["Amount"].ToString());
                            if (dv.ToTable().Rows[0]["Flag"].ToString() == "-") { dAmt = dAmt * (-1); }
                        }
                        dv.Dispose();
                    }
                    else
                    {
                        dAmt = dNetAmt * dSchPercent / 100;
                    }

                    dtTempT = new DataTable();
                    dv = new DataView(dtTemp);
                    dv.RowFilter = "TemplateId = " + iTemplateId;
                    dtTempT = dv.ToTable();
                    dv.Dispose();

                    decimal dRoundOff = 0;
                    decimal dRound = 0;
                    if (dRoundValue > 0)
                    {
                        dRoundOff = Math.Truncate(dAmt / dRoundValue) * dRoundValue;
                        dRound = dAmt - dRoundOff;
                        dEMIRoundOff = dEMIRoundOff + dRound;
                        dAmt = dRoundOff;
                    }

                    if (sSchType == "R")
                    {
                        sSql = "Update dbo.PaymentScheduleFlat Set Amount=" + dEMIRoundOff + ",NetAmount=" + dEMIRoundOff +
                               " Where PaymentSchId=" + iPaymentSchId + " AND FlatId=" + argFlatId + " AND CostCentreId=" + iCCId + " AND SchType='R'";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    else
                    {
                        if (dtTempT.Rows.Count == 1 && sSchType == "O")
                        {
                            sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                    "Values(" + iPaymentSchId + "," + argFlatId + ",0," + iOtherCostId + ",'" + sSchType + "',100," + dAmt + "," + dAmt + ") SELECT SCOPE_IDENTITY();";
                            cmd = new SqlCommand(sSql, conn, tran);
                            iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();

                            drT = dtReceipt.Select("SchType = 'O' and Id = " + iOtherCostId + "");

                            if (drT.Length > 0)
                            {
                                drT[0]["RAmount"] = dAmt;
                            }

                            dQNetAmt = dAmt;

                            dtQualT = new DataTable();
                            dv = new DataView(dtQual);
                            dv.RowFilter = "SchType = '" + sSchType + "' and OtherCostId = " + iOtherCostId;
                            dtQualT = dv.ToTable();
                            dv.Dispose();

                            if (dtQualT.Rows.Count > 0)
                            {
                                QualVBC = new Collection();

                                for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                                {
                                    RAQual = new cRateQualR();
                                    bService = Convert.ToBoolean(dtQualT.Rows[Q]["Service"]);

                                    DataTable dtTDS = new DataTable();
                                    if (Convert.ToInt32(dtQualT.Rows[Q]["QualTypeId"]) == 2)
                                    {
                                        if (bService == true)
                                            dtTDS = GetSTSettings("G", dSchDate, conn, tran);
                                        else
                                            dtTDS = GetSTSettings("F", dSchDate, conn, tran);
                                    }
                                    else
                                    {
                                        dtTDS = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), dSchDate, "B", conn, tran);
                                    }

                                    RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                                    if (dtTDS.Rows.Count > 0)
                                    {
                                        RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                                        RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                                        RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                                        RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                                        RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                                        RAQual.HEDValue = Convert.ToDecimal(dtQualT.Rows[Q]["HEDValue"].ToString());
                                        RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Taxable"], CommFun.datatypes.vartypenumeric));
                                    }

                                    DataTable dtQ = new DataTable();
                                    dtQ = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), dSchDate, "B", conn, tran);
                                    //dtQ = QualifierSelect(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), "B", dSchDate, conn, tran);
                                    if (dtQ.Rows.Count > 0)
                                    {
                                        RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                                        RAQual.Amount = 0;
                                        RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                                    }

                                    QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                                }

                                Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                                dQBaseAmt = dAmt;
                                dQNetAmt = dAmt; decimal dTaxAmt = 0;
                                decimal dVATAmt = 0;

                                if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, dSchDate, ref dVATAmt) == true)
                                {
                                    foreach (Qualifier.cRateQualR d in QualVBC)
                                    {
                                        sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount,NetPer,HEDPer,HEDValue,TaxablePer,TaxableValue) " +
                                                "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                                "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + "," + d.NetPer + "," + d.HEDPer + "," + d.HEDValue + "," + d.TaxablePer + "," + d.TaxableValue + ")";
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                }

                                sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }

                            sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dQNetAmt + "  Where PaymentSchId = " + iPaymentSchId;
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            dTNetAmt = dTNetAmt + dQNetAmt;
                        }
                        else
                        {
                            dBalAmt = dAmt;
                            for (int j = 0; j < dtTempT.Rows.Count; j++)
                            {
                                iSchId = Convert.ToInt32(dtTempT.Rows[j]["SchId"].ToString());
                                dRPer = Convert.ToDecimal(dtTempT.Rows[j]["Percentage"].ToString());
                                sRSchType = dtTempT.Rows[j]["SchType"].ToString();
                                iReceiptId = Convert.ToInt32(dtTempT.Rows[j]["ReceiptTypeId"].ToString());
                                iROtherCostId = Convert.ToInt32(dtTempT.Rows[j]["OtherCostId"].ToString());

                                if (dRPer != 0) { dRAmt = dAmt * dRPer / 100; }
                                else { dRAmt = dBalAmt; }

                                if (dRAmt > dBalAmt) { dRAmt = dBalAmt; }

                                if (sRSchType == "A" && bAdvance == false)
                                {

                                    dAdvRAmt = dAdvAmt * dRPer / 100;
                                    if (dAdvRAmt > dAdvBalAmt) { dAdvRAmt = dAdvBalAmt; }
                                    dAdvBalAmt = dAdvBalAmt - dAdvRAmt;
                                    dTNetAmt = dTNetAmt - dAdvRAmt;

                                    sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                            "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + ", 0," + dAdvRAmt + ") SELECT SCOPE_IDENTITY();";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                                    cmd.Dispose();
                                }

                                else
                                {
                                    if (sRSchType == "A")
                                    {
                                        drT = dtReceipt.Select("SchType = 'A'");
                                    }
                                    else if (sRSchType == "O")
                                    {
                                        drT = dtReceipt.Select("SchType = 'O' and Id = " + iROtherCostId + "");
                                    }
                                    else
                                    {
                                        drT = dtReceipt.Select("SchType = 'R' and Id = " + iReceiptId + "");
                                    }


                                    decimal dRTAmt = 0;
                                    decimal dRRAmt = 0;

                                    if (drT.Length > 0)
                                    {
                                        dRTAmt = Convert.ToDecimal(drT[0]["Amount"].ToString());
                                        dRRAmt = Convert.ToDecimal(drT[0]["RAmount"].ToString());
                                    }

                                    if (dRAmt > dRTAmt - dRRAmt)
                                    {
                                        dRAmt = dRTAmt - dRRAmt;
                                    }

                                    if (drT.Length > 0)
                                    {
                                        drT[0]["RAmount"] = dRRAmt + dRAmt;
                                    }

                                    if (dAmt == 0) { dRPer = 0; }
                                    else dRPer = (dRAmt / dAmt) * 100;

                                    dBalAmt = dBalAmt - dRAmt;

                                    sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                            "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + "," + dRAmt + "," + dRAmt + ") SELECT SCOPE_IDENTITY();";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    iRSchId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                                    cmd.Dispose();

                                    dQNetAmt = dRAmt;

                                    dtQualT = new DataTable();
                                    dv = new DataView(dtQual);
                                    dv.RowFilter = "SchType = '" + sRSchType + "' and ReceiptTypeId = " + iReceiptId + " and OtherCostId = " + iROtherCostId;
                                    dtQualT = dv.ToTable();
                                    dv.Dispose();
                                    if (dtQualT.Rows.Count > 0)
                                    {
                                        QualVBC = new Collection();

                                        for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                                        {
                                            RAQual = new cRateQualR();
                                            bService = Convert.ToBoolean(dtQualT.Rows[Q]["Service"]);

                                            DataTable dtTDS = new DataTable();
                                            if (Convert.ToInt32(dtQualT.Rows[Q]["QualTypeId"]) == 2)
                                            {
                                                if (bService == true)
                                                    dtTDS = GetSTSettings("G", dSchDate, conn, tran);
                                                else
                                                    dtTDS = GetSTSettings("F", dSchDate, conn, tran);
                                            }
                                            else
                                            {
                                                dtTDS = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), dSchDate, "B", conn, tran);
                                            }

                                            RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                                            if (dtTDS.Rows.Count > 0)
                                            {
                                                RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                                                RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                                                RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                                                RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                                                RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                                                RAQual.HEDValue = Convert.ToDecimal(dtQualT.Rows[Q]["HEDValue"].ToString());
                                                RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Taxable"], CommFun.datatypes.vartypenumeric));
                                            }

                                            DataTable dtQ = new DataTable();
                                            dtQ = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), dSchDate, "B", conn, tran);
                                            //dtQ = QualifierSelect(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), "B", dSchDate, conn, tran);
                                            if (dtQ.Rows.Count > 0)
                                            {
                                                RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                                                RAQual.Amount = 0;
                                                RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                                            }

                                            QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                                        }

                                        Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                                        dQBaseAmt = dRAmt;
                                        dQNetAmt = dRAmt; decimal dTaxAmt = 0;
                                        decimal dVATAmt = 0;

                                        if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, dSchDate, ref dVATAmt) == true)
                                        {
                                            foreach (Qualifier.cRateQualR d in QualVBC)
                                            {
                                                sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount,NetPer,HEDPer,HEDValue,TaxablePer,TaxableValue) " +
                                                        "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                                        "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + "," + d.NetPer + "," + d.HEDPer + "," + d.HEDValue + "," + d.TaxablePer + "," + d.TaxableValue + ")";
                                                cmd = new SqlCommand(sSql, conn, tran);
                                                cmd.ExecuteNonQuery();
                                                cmd.Dispose();
                                            }
                                        }

                                        sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }


                                    dTNetAmt = dTNetAmt + dQNetAmt;

                                }

                                //if (dBalAmt <= 0) { break; }
                            }

                            if (dBalAmt > 0)
                            {
                                for (int j = 0; j < dtReceiptOrder.Rows.Count; j++)
                                {
                                    dRAmt = dBalAmt;

                                    sRSchType = dtReceiptOrder.Rows[j]["SchType"].ToString();
                                    iReceiptId = Convert.ToInt32(dtReceiptOrder.Rows[j]["ReceiptTypeId"].ToString());
                                    iROtherCostId = Convert.ToInt32(dtReceiptOrder.Rows[j]["OtherCostId"].ToString());

                                    if (sRSchType == "O")
                                    {
                                        drT = dtReceipt.Select("SchType = 'O' and Id = " + iROtherCostId + "");
                                    }
                                    else
                                    {
                                        drT = dtReceipt.Select("SchType = 'R' and Id = " + iReceiptId + "");
                                    }

                                    decimal dRTAmt = 0;
                                    decimal dRRAmt = 0;

                                    if (drT.Length > 0)
                                    {
                                        dRTAmt = Convert.ToDecimal(drT[0]["Amount"].ToString());
                                        dRRAmt = Convert.ToDecimal(drT[0]["RAmount"].ToString());
                                    }

                                    if (dRAmt > dRTAmt - dRRAmt)
                                    {
                                        dRAmt = dRTAmt - dRRAmt;
                                    }

                                    if (drT.Length > 0)
                                    {
                                        drT[0]["RAmount"] = dRRAmt + dRAmt;
                                    }

                                    if (dRAmt > 0)
                                    {
                                        decimal dPCAmt = 0;
                                        bool bAns = false;
                                        sSql = "Select SchId,Amount,NetAmount from dbo.FlatReceiptType Where PaymentSchId = " + iPaymentSchId + " and " +
                                                "FlatId= " + argFlatId + " and ReceiptTypeId= " + iReceiptId + " and OtherCostId = " + iROtherCostId + " and SchType= '" + sRSchType + "'";
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        sdr = cmd.ExecuteReader();
                                        DataTable dtP = new DataTable();
                                        dtP.Load(sdr);
                                        sdr.Close();
                                        cmd.Dispose();

                                        if (dtP.Rows.Count > 0)
                                        {
                                            dPCAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtP.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
                                            dTNetAmt = dTNetAmt - dPCAmt;
                                            dBalAmt = dBalAmt + dPCAmt;
                                            iRSchId = Convert.ToInt32(dtP.Rows[0]["SchId"].ToString());
                                            bAns = true;
                                        }
                                        dtP.Dispose();

                                        if (bAns == true)
                                        {
                                            dRAmt = dRAmt + dPCAmt;
                                            dRPer = (dRAmt / dAmt) * 100;

                                            sSql = "Update dbo.FlatReceiptType Set Amount= " + dRAmt + ",Percentage = " + dRPer + ",NetAmount = " + dRAmt + " Where SchId = " + iRSchId;
                                            cmd = new SqlCommand(sSql, conn, tran);
                                            cmd.ExecuteNonQuery();
                                            cmd.Dispose();

                                            sSql = "Delete from dbo.FlatReceiptQualifier Where SchId = " + iRSchId;
                                            cmd = new SqlCommand(sSql, conn, tran);
                                            cmd.ExecuteNonQuery();
                                            cmd.Dispose();
                                        }
                                        else
                                        {
                                            dRPer = (dRAmt / dAmt) * 100;

                                            sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                                    "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + "," + dRAmt + "," + dRAmt + ") SELECT SCOPE_IDENTITY();";
                                            cmd = new SqlCommand(sSql, conn, tran);
                                            iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                                            cmd.Dispose();
                                        }

                                        dQNetAmt = dRAmt;

                                        dtQualT = new DataTable();
                                        dv = new DataView(dtQual);

                                        if (sRSchType == "O")
                                        {
                                            dv.RowFilter = "SchType = 'O' and ReceiptTypeId = 0 and OtherCostId = " + iROtherCostId + "";

                                        }
                                        else
                                        {
                                            dv.RowFilter = "SchType = 'R' and ReceiptTypeId = " + iReceiptId + " and OtherCostId = 0";
                                        }

                                        dtQualT = dv.ToTable();
                                        dv.Dispose();
                                        if (dtQualT.Rows.Count > 0)
                                        {
                                            QualVBC = new Collection();

                                            for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                                            {
                                                RAQual = new cRateQualR();
                                                bService = Convert.ToBoolean(dtQualT.Rows[Q]["Service"]);

                                                DataTable dtTDS = new DataTable();
                                                if (Convert.ToInt32(dtQualT.Rows[Q]["QualTypeId"]) == 2)
                                                {
                                                    if (bService == true)
                                                        dtTDS = GetSTSettings("G", dSchDate, conn, tran);
                                                    else
                                                        dtTDS = GetSTSettings("F", dSchDate, conn, tran);
                                                }
                                                else
                                                {
                                                    dtTDS = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), dSchDate, "B", conn, tran);
                                                }

                                                RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                                                if (dtTDS.Rows.Count > 0)
                                                {
                                                    RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                                                    RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                                                    RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                                                    RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                                                    RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                                                    RAQual.HEDValue = Convert.ToDecimal(dtQualT.Rows[Q]["HEDValue"].ToString());
                                                    RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Taxable"], CommFun.datatypes.vartypenumeric));
                                                }

                                                DataTable dtQ = new DataTable();
                                                dtQ = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), dSchDate, "B", conn, tran);
                                                //dtQ = QualifierSelect(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), "B", dSchDate, conn, tran);
                                                if (dtQ.Rows.Count > 0)
                                                {
                                                    RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                                                    RAQual.Amount = 0;
                                                    RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                                                }

                                                QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                                            }

                                            Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                                            dQBaseAmt = dRAmt;
                                            dQNetAmt = dRAmt; decimal dTaxAmt = 0;
                                            decimal dVATAmt = 0;

                                            if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                                            {
                                                foreach (Qualifier.cRateQualR d in QualVBC)
                                                {
                                                    sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount,NetPer,HEDPer,HEDValue,TaxablePer,TaxableValue) " +
                                                            "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                                            "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + "," + d.NetPer + "," + d.HEDPer + "," + d.HEDValue + "," + d.TaxablePer + "," + d.TaxableValue + ")";
                                                    cmd = new SqlCommand(sSql, conn, tran);
                                                    cmd.ExecuteNonQuery();
                                                    cmd.Dispose();
                                                }
                                            }
                                            sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                                            cmd = new SqlCommand(sSql, conn, tran);
                                            cmd.ExecuteNonQuery();
                                            cmd.Dispose();
                                        }

                                        dTNetAmt = dTNetAmt + dQNetAmt;
                                        dBalAmt = dBalAmt - dRAmt;
                                        if (dBalAmt <= 0) { break; }
                                    }
                                }

                            }

                            if (sSchType != "R")
                            {
                                //modified
                                sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dTNetAmt + "  Where PaymentSchId = " + iPaymentSchId;
                                //sSql = "Update PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dQNetAmt + "  Where PaymentSchId = " + iPaymentSchId;
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }
                    }
                }
                dt.Dispose();

                if (bAdvance == false)
                {
                    sSql = "Insert into dbo.PaymentScheduleFlat(FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,Amount,NetAmount,PreStageTypeId,SortOrder) " +
                            "Values(" + argFlatId + ",0," + iCCId + ",'A','Advance',0,0,0,NULL,0," + dAdvAmt + ",0,0)";
                    //sSql = "Insert Into dbo.PaymentScheduleFlat(FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,Amount,NetAmount,PreStageTypeId,SortOrder) " +
                    //      "Values(" + argFlatId + ",0," + iCCId + ",'A','Advance',0,0,0,0," + dAdvAmt + ",0,0)";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                sSql = "Update dbo.PaymentScheduleFlat Set Advance=0";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "UPDATE PaymentScheduleFlat SET Advance=SummedQty FROM " +
                        " PaymentScheduleFlat A JOIN (SELECT PaymentSchId, SUM(NetAmount) SummedQty " +
                        " FROM FlatReceiptType WHERE SchType='A' GROUP BY PaymentSchId ) CCA ON A.PaymentSchId=CCA.PaymentSchId";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                //Schedule Date
                SqlDataReader dr, sdr1, sdr2; DataTable dt1; int iStgId = 0, iTempId = 0;
                int iDateAfter = 0, iDuration = 0; string sDurType = ""; DateTime SchDate; int iSortOrder = 0;
                DateTime StartDate = DateTime.Now; DateTime EndDate = DateTime.Now; DateTime FinaliseDate = DateTime.Now; int ipre = 0;


                sSql = "Update dbo.PaymentScheduleFlat Set PreStageTypeId=-1 Where FlatId=" + argFlatId + " And TemplateId In(  " +
                        " Select TemplateId From dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " " +
                        " And CostCentreId=" + iCCId + " And PreStageTypeId=-1)";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                //tran.Commit();
                //conn.Close();

                //conn = new SqlConnection();
                //conn = BsfGlobal.OpenCRMDB();
                //tran = conn.BeginTransaction();

                sSql = "Select FinaliseDate from dbo.BuyerDetail Where FlatId=" + argFlatId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["FinaliseDate"] == DBNull.Value || dt.Rows[0]["FinaliseDate"].ToString() == "")
                        FinaliseDate = DateTime.MinValue;
                    else
                        FinaliseDate = Convert.ToDateTime(dt.Rows[0]["FinaliseDate"]);

                    sSql = "Select TemplateId,PreStageTypeId from dbo.PaymentScheduleFlat Where FlatId=" + argFlatId + " And PreStageTypeId=-1";
                    cmd = new SqlCommand(sSql, conn, tran);
                    dr = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(dr);
                    cmd.Dispose();

                    if (dt.Rows.Count > 0)
                    {
                        iStgId = Convert.ToInt32(dt.Rows[0]["PreStageTypeId"]);
                        iTempId = Convert.ToInt32(dt.Rows[0]["TemplateId"]);
                    }
                    dt.Dispose();

                    sSql = "Select SortOrder From dbo.PaymentScheduleFlat Where FlatId=" + argFlatId + " And TemplateId=" + iTempId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    sdr2 = cmd.ExecuteReader();
                    dt1 = new DataTable();
                    dt1.Load(sdr2); cmd.Dispose();
                    dt1.Dispose();

                    if (dt1.Rows.Count > 0)
                    {
                        iSortOrder = Convert.ToInt32(dt1.Rows[0]["SortOrder"]);
                    }

                    sSql = "select StartDate,EndDate From ProjectInfo Where CostCentreId= " + iCCId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    dt = new DataTable();
                    dr = cmd.ExecuteReader();
                    dt.Load(dr);
                    dt.Dispose();

                    if (dt.Rows.Count > 0)
                    {
                        StartDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["StartDate"], CommFun.datatypes.VarTypeDate));
                        EndDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["EndDate"], CommFun.datatypes.VarTypeDate));
                    }

                    sSql = "Update dbo.PaymentScheduleFlat Set SchDate='" + FinaliseDate.ToString("dd-MMM-yyyy") + "'" +
                            " Where TemplateId=" + iTempId + " And FlatId=" + argFlatId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Update dbo.PaymentScheduleFlat Set SchDate='" + FinaliseDate.ToString("dd-MMM-yyyy") + "'" +
                            " Where TemplateId=0 And FlatId=" + argFlatId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (iStgId == -1)
                    {
                        if (iStgId == -1)
                        {
                            sSql = "Select A.PreStageTypeId,A.CostCentreId,A.TemplateId,A.DateAfter,A.Duration,A.Durationtype from dbo.PaymentScheduleFlat A" +
                                    " Left Join dbo.ProgressBillRegister B On A.FlatId=B.FlatId " +
                                    " Where A.FlatId=" + argFlatId + " And A.SortOrder>=" + iSortOrder + "" +
                                    " And A.PaymentSchId Not In " +
                                    " (Select PaySchId From dbo.ProgressBillRegister Where FlatId=" + argFlatId + ") Order By A.SortOrder";
                            cmd = new SqlCommand(sSql, conn, tran);
                            sdr1 = cmd.ExecuteReader();
                            dt = new DataTable();
                            dt.Load(sdr1);
                            cmd.Dispose();
                        }

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            iTempId = Convert.ToInt32(dt.Rows[i]["TemplateId"]);
                            ipre = Convert.ToInt32(dt.Rows[i]["PreStageTypeId"]);
                            iDateAfter = Convert.ToInt32(dt.Rows[i]["DateAfter"]);
                            iDuration = Convert.ToInt32(dt.Rows[i]["Duration"]);
                            sDurType = dt.Rows[i]["DurationType"].ToString();

                            if (ipre == -1) { } else if (ipre == -2) { } else if (ipre == -3) { } else if (ipre == 0) { } else { iTempId = ipre; }

                            sSql = "Select SchDate From dbo.PaymentScheduleFlat Where CostCentreId=" + dt.Rows[i]["CostCentreId"] + " And FlatId=" + argFlatId + "" +
                                    " And TemplateId=" + iTempId + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            DataTable dtDate = new DataTable();
                            dr = cmd.ExecuteReader();
                            dtDate.Load(dr);
                            cmd.Dispose();

                            if (ipre == -1)
                            {
                                if (FinaliseDate != DateTime.MinValue)
                                    SchDate = Convert.ToDateTime(CommFun.IsNullCheck(FinaliseDate, CommFun.datatypes.VarTypeDate));
                                else
                                    SchDate = DateTime.MinValue;
                            }
                            else if (ipre == -2) { SchDate = StartDate; }
                            else if (ipre == -3) { SchDate = EndDate; }
                            else
                                SchDate = Convert.ToDateTime(CommFun.IsNullCheck(dtDate.Rows[0]["SchDate"], CommFun.datatypes.VarTypeDate));

                            if (sDurType == "D")
                            {
                                if (SchDate != DateTime.MinValue)
                                {
                                    if (iDateAfter == 0)
                                        SchDate = SchDate.AddDays(iDuration);
                                    else
                                        SchDate = SchDate.AddDays(-iDuration);
                                }
                            }
                            else if (sDurType == "M")
                            {
                                if (SchDate != DateTime.MinValue)
                                {
                                    if (iDateAfter == 0)
                                        SchDate = SchDate.AddMonths(iDuration);
                                    else
                                        SchDate = SchDate.AddDays(-iDuration);
                                }
                            }


                            sSql = "Update dbo.PaymentScheduleFlat Set SchDate=@SchDate" +
                                " Where TemplateId=" + dt.Rows[i]["TemplateId"] + " And FlatId=" + argFlatId + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            SqlParameter sqlPara = new SqlParameter() { ParameterName = "@SchDate", DbType = DbType.DateTime };
                            if (SchDate == DateTime.MinValue)
                                sqlPara.Value = System.Data.SqlTypes.SqlDateTime.Null;
                            else
                                sqlPara.Value = SchDate;
                            cmd.Parameters.Add(sqlPara);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                        }
                    }
                }

                //tran.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateFinalBuyerSchedule(int argFlatId, string argsType, DataTable argdt, SqlConnection conn, SqlTransaction tran)
        {
            string sSql = "";

            SqlDataReader sdr;
            SqlCommand cmd;
            DataTable dt = new DataTable();

            int iCCId = 0;
            int iFlatTypeId = 0;
            int iPayTypeId = 0;
            decimal dBaseAmt = 0;
            decimal dAdvAmt = 0;
            decimal dAdvBalAmt = 0;
            decimal dLandAmt = 0;
            decimal dNetAmt = 0;
            decimal dOtherAmt = 0;
            decimal dRAmt = 0;
            int iReceiptId = 0;
            int iROtherCostId = 0;
            string sRSchType = "";
            bool bAdvance = false;
            int iPaymentSchId = 0;
            DateTime dSchDate = DateTime.Now;
            string sSchType = "";
            int iOtherCostId = 0;
            decimal dRPer = 0;
            decimal dSchPercent = 0;
            decimal dQBaseAmt = 0;
            decimal dQNetAmt = 0;
            int iTemplateId = 0;
            int iSchId = 0;
            int iRSchId = 0;
            decimal dTNetAmt = 0;
            decimal dBalAmt = 0;
            bool bService = false, bLCBon = false;
            DataRow[] drT;
            cRateQualR RAQual;
            Collection QualVBC;
            DateTime FinaliseDate = DateTime.Now;
            DateTime BlockDate = DateTime.Now;

            DataTable dtReceipt = new DataTable();

            sSql = "Delete from dbo.PaymentScheduleFlat Where FlatId= " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete from dbo.FlatReceiptQualifier Where SchId IN(Select SchId from dbo.FlatReceiptType Where FlatId= " + argFlatId + ")";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete from dbo.FlatReceiptType Where FlatId= " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            if (argsType == "S")
                sSql = "Select FinaliseDate From dbo.BuyerDetail Where Status='S' And FlatId=" + argFlatId;
            else if (argsType == "B")
                sSql = "Select Date From dbo.BlockUnits Where BlockType='B' And FlatId=" + argFlatId + "";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0)
            {
                if (argsType == "S")
                    FinaliseDate = Convert.ToDateTime(dt.Rows[0]["FinaliseDate"]);
                else if (argsType == "B")
                    BlockDate = Convert.ToDateTime(dt.Rows[0]["Date"]);
            }
            dt.Dispose();

            //sSql = "Select FlatTypeId,CostCentreId,PayTypeId,BaseAmt,AdvAmount,USLandAmt from dbo.FlatDetails Where FlatId= " + argFlatId;//modified
            sSql = "Select FlatTypeId,CostCentreId,PayTypeId,BaseAmt,AdvAmount,LandRate,Guidelinevalue,USLandAmt from dbo.FlatDetails Where FlatId= " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0)
            {
                iCCId = Convert.ToInt32(dt.Rows[0]["CostCentreId"].ToString());
                iFlatTypeId = Convert.ToInt32(dt.Rows[0]["FlatTypeId"].ToString());
                iPayTypeId = Convert.ToInt32(dt.Rows[0]["PayTypeId"].ToString());
                dBaseAmt = Convert.ToDecimal(dt.Rows[0]["BaseAmt"].ToString());
                dAdvAmt = Convert.ToDecimal(dt.Rows[0]["AdvAmount"].ToString());

                sSql = "Select LCBasedon From dbo.ProjectInfo Where CostCentreId= " + iCCId;
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                DataTable dtPI = new DataTable();
                dtPI.Load(sdr);
                sdr.Close();
                cmd.Dispose();
                if (dtPI.Rows.Count > 0) { bLCBon = Convert.ToBoolean(dtPI.Rows[0]["LCBasedon"]); }
                if (bLCBon == false) { dLandAmt = Convert.ToDecimal(dt.Rows[0]["LandRate"].ToString()); }
                else { dLandAmt = Convert.ToDecimal(dt.Rows[0]["USLandAmt"].ToString()); }

                //dLandAmt = Convert.ToDecimal(dt.Rows[0]["USLandAmt"].ToString());
                //dLandAmt = Convert.ToDecimal(dt.Rows[0]["LandRate"].ToString());
            }
            dt.Dispose();

            sSql = "Select TemplateId From dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " and CostCentreId = " + iCCId + " and SchType='A'";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0) { bAdvance = true; }
            dt.Dispose();

            sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                    "Where FlatId = " + argFlatId + " and OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans Where PayTypeId=" + iPayTypeId + " and CostCentreId=" + iCCId + ")";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0) { dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }
            dt.Dispose();

            dNetAmt = dBaseAmt + dOtherAmt;
            if (bAdvance == true) { dNetAmt = dNetAmt - dAdvAmt; }

            if (argdt.Rows.Count > 0)
            {
                for (int i = 0; i < argdt.Rows.Count; i++)
                {
                    string sDate = string.Format(Convert.ToDateTime(CommFun.IsNullCheck(argdt.Rows[i]["SchDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MMM-yyyy"));
                    if (argdt.Rows[i]["SchDate"].ToString() == "")
                    {
                        sDate = "NULL";
                        sSql = "Insert into dbo.PaymentScheduleFlat(FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId," +
                        " OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId,SortOrder) " +
                        " Values(" + argdt.Rows[i]["FlatId"] + "," + argdt.Rows[i]["TemplateId"] + "," + argdt.Rows[i]["CostCentreId"] + "," +
                        " '" + argdt.Rows[i]["SchType"] + "','" + argdt.Rows[i]["Description"] + "'," + argdt.Rows[i]["SchDescId"] + "," +
                        " " + argdt.Rows[i]["StageId"] + "," + argdt.Rows[i]["OtherCostId"] + "," + sDate + "," +
                        " '" + argdt.Rows[i]["DateAfter"] + "'," + argdt.Rows[i]["Duration"] + ",'" + argdt.Rows[i]["DurationType"] + "'," +
                        " " + argdt.Rows[i]["SchPercent"] + "," + argdt.Rows[i]["Amount"] + "," + argdt.Rows[i]["PreStageTypeId"] + "," +
                        " " + argdt.Rows[i]["SortOrder"] + ")";
                    }
                    else
                    {
                        sSql = "Insert into dbo.PaymentScheduleFlat(FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId," +
                        " OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId,SortOrder) " +
                        " Values(" + argdt.Rows[i]["FlatId"] + "," + argdt.Rows[i]["TemplateId"] + "," + argdt.Rows[i]["CostCentreId"] + "," +
                        " '" + argdt.Rows[i]["SchType"] + "','" + argdt.Rows[i]["Description"] + "'," + argdt.Rows[i]["SchDescId"] + "," +
                        " " + argdt.Rows[i]["StageId"] + "," + argdt.Rows[i]["OtherCostId"] + ",'" + sDate + "'," +
                        " '" + argdt.Rows[i]["DateAfter"] + "'," + argdt.Rows[i]["Duration"] + ",'" + argdt.Rows[i]["DurationType"] + "'," +
                        " " + argdt.Rows[i]["SchPercent"] + "," + argdt.Rows[i]["Amount"] + "," + argdt.Rows[i]["PreStageTypeId"] + "," +
                        " " + argdt.Rows[i]["SortOrder"] + ")";
                    }
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }

            sSql = "Select ReceiptTypeId,OtherCostId,SchType from dbo.ReceiptTypeOrder " +
                    "Where PayTypeId = " + iPayTypeId + " and CostCentreId=" + iCCId + " and SchType <>'A' Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtReceiptOrder = new DataTable();
            dtReceiptOrder.Load(sdr);
            sdr.Close();
            cmd.Dispose();


            sSql = "Select OtherCostId,Flag,Amount from dbo.FlatOtherCost Where FlatId = " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtT = new DataTable();
            dtT.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            dtReceipt.Columns.Add("Id", typeof(int));
            dtReceipt.Columns.Add("SchType", typeof(string));
            dtReceipt.Columns.Add("Amount", typeof(decimal));
            dtReceipt.Columns.Add("RAmount", typeof(decimal));

            DataRow drR;
            drR = dtReceipt.NewRow();
            drR["Id"] = 1;
            drR["SchType"] = "A";
            drR["Amount"] = dAdvAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            drR = dtReceipt.NewRow();
            drR["Id"] = 2;
            drR["SchType"] = "R";
            drR["Amount"] = dLandAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            drR = dtReceipt.NewRow();
            drR["Id"] = 3;
            drR["SchType"] = "R";
            drR["Amount"] = dBaseAmt - dLandAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            for (int i = 0; i < dtT.Rows.Count; i++)
            {
                drR = dtReceipt.NewRow();
                drR["Id"] = Convert.ToInt32(dtT.Rows[i]["OtherCostId"].ToString());
                drR["SchType"] = "O";
                drR["Amount"] = Convert.ToDecimal(dtT.Rows[i]["Amount"].ToString());
                drR["RAmount"] = 0;
                dtReceipt.Rows.Add(drR);
            }

            sSql = "Select SchId,TemplateId,ReceiptTypeId,Percentage,OtherCostId,SchType from dbo.CCReceiptType " +
                    "Where TemplateId in (Select TemplateId from dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " and CostCentreId=" + iCCId + ") Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtTemp = new DataTable();
            dtTemp.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            //sSql = "Select A.*,IsNull(B.Service,0)Service From dbo.CCReceiptQualifier A " +
            //        " Left Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId Where CostCentreId=" + iCCId;
            sSql = "Select C.QualTypeId,A.*,IsNull(B.Service,0)Service From dbo.CCReceiptQualifier A " +
                    " Left Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId " +
                    " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On C.QualifierId=A.QualifierId " +
                    " Where CostCentreId=" + iCCId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtQual = new DataTable();
            dtQual.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            sSql = "Select PaymentSchId,TemplateId,SchDate,SchType,OtherCostId,SchPercent from dbo.PaymentScheduleFlat Where FlatId = " + argFlatId + " Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            decimal dAmt = 0;
            DataView dv;
            decimal dAdvRAmt = 0;

            DataTable dtTempT;
            DataTable dtQualT;
            dAdvBalAmt = dAdvAmt;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iPaymentSchId = Convert.ToInt32(dt.Rows[i]["PaymentSchId"].ToString());
                iTemplateId = Convert.ToInt32(dt.Rows[i]["TemplateId"].ToString());
                sSchType = dt.Rows[i]["SchType"].ToString();
                if (argsType == "S")
                    dSchDate = FinaliseDate;
                else if (argsType == "B")
                    dSchDate = BlockDate;
                if (dSchDate == DateTime.MinValue) { dSchDate = DateTime.Now; }
                //Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[i]["SchDate"], CommFun.datatypes.VarTypeDate));
                iOtherCostId = Convert.ToInt32(dt.Rows[i]["OtherCostId"].ToString());
                dSchPercent = Convert.ToDecimal(dt.Rows[i]["SchPercent"].ToString());
                dTNetAmt = 0;

                dAmt = 0;
                if (sSchType == "A")
                {
                    dAmt = dAdvAmt;
                }
                else if (sSchType == "O")
                {
                    dv = new DataView(dtT);
                    dv.RowFilter = "OtherCostId = " + iOtherCostId;
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        dAmt = Convert.ToDecimal(dv.ToTable().Rows[0]["Amount"].ToString());
                        if (dv.ToTable().Rows[0]["Flag"].ToString() == "-") { dAmt = dAmt * (-1); }
                    }
                    dv.Dispose();
                }
                else
                {
                    dAmt = dNetAmt * dSchPercent / 100;
                }

                dtTempT = new DataTable();
                dv = new DataView(dtTemp);
                dv.RowFilter = "TemplateId = " + iTemplateId;
                dtTempT = dv.ToTable();
                dv.Dispose();

                if (dtTempT.Rows.Count == 1 && sSchType == "O")
                {

                    sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                            "Values(" + iPaymentSchId + "," + argFlatId + ",0," + iOtherCostId + ",'" + sSchType + "',100," + dAmt + "," + dAmt + ") SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(sSql, conn, tran);
                    iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();

                    drT = dtReceipt.Select("SchType = 'O' and Id = " + iOtherCostId + "");

                    if (drT.Length > 0)
                    {
                        drT[0]["RAmount"] = dAmt;
                    }

                    dQNetAmt = dAmt;

                    dtQualT = new DataTable();
                    dv = new DataView(dtQual);
                    dv.RowFilter = "SchType = '" + sSchType + "' and OtherCostId = " + iOtherCostId;
                    dtQualT = dv.ToTable();
                    dv.Dispose();

                    if (dtQualT.Rows.Count > 0)
                    {
                        QualVBC = new Collection();

                        for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                        {
                            RAQual = new cRateQualR();
                            bService = Convert.ToBoolean(dtQualT.Rows[Q]["Service"]);

                            DataTable dtTDS = new DataTable();
                            if (Convert.ToInt32(dtQualT.Rows[Q]["QualTypeId"]) == 2)
                            {
                                if (bService == true)
                                    dtTDS = GetSTSettings("G", dSchDate, conn, tran);
                                else
                                    dtTDS = GetSTSettings("F", dSchDate, conn, tran);
                            }
                            else
                            {
                                dtTDS = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), dSchDate, "B", conn, tran);
                            }

                            RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                            if (dtTDS.Rows.Count > 0)
                            {
                                RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                                RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                                RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                                RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                                RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                                RAQual.HEDValue = (dAmt * Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric))) / 100;
                                RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Taxable"], CommFun.datatypes.vartypenumeric));
                            }

                            DataTable dtQ = new DataTable();
                            dtQ = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), dSchDate, "B", conn, tran);
                            //dtQ = QualifierSelect(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), "B", dSchDate, conn, tran);
                            if (dtQ.Rows.Count > 0)
                            {
                                RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                                RAQual.Amount = 0;
                                RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                            }

                            QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                        }

                        Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                        dQBaseAmt = dAmt;
                        dQNetAmt = dAmt; decimal dTaxAmt = 0;
                        decimal dVATAmt = 0;

                        if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, dSchDate, ref dVATAmt) == true)
                        {
                            foreach (Qualifier.cRateQualR d in QualVBC)
                            {
                                sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount,NetPer,HEDPer,HEDValue,TaxablePer,TaxableValue) " +
                                        "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                        "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + "," + d.NetPer + "," + d.HEDPer + "," + d.HEDValue + "," + d.TaxablePer + "," + d.TaxableValue + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }

                        sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dQNetAmt + "  Where PaymentSchId = " + iPaymentSchId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    dTNetAmt = dTNetAmt + dQNetAmt;
                }

                else
                {
                    dBalAmt = dAmt;
                    for (int j = 0; j < dtTempT.Rows.Count; j++)
                    {
                        iSchId = Convert.ToInt32(dtTempT.Rows[j]["SchId"].ToString());
                        dRPer = Convert.ToDecimal(dtTempT.Rows[j]["Percentage"].ToString());
                        sRSchType = dtTempT.Rows[j]["SchType"].ToString();
                        iReceiptId = Convert.ToInt32(dtTempT.Rows[j]["ReceiptTypeId"].ToString());
                        iROtherCostId = Convert.ToInt32(dtTempT.Rows[j]["OtherCostId"].ToString());

                        if (dRPer != 0) { dRAmt = dAmt * dRPer / 100; }
                        else { dRAmt = dBalAmt; }

                        if (dRAmt > dBalAmt) { dRAmt = dBalAmt; }


                        if (sRSchType == "A" && bAdvance == false)
                        {

                            dAdvRAmt = dAdvAmt * dRPer / 100;
                            if (dAdvRAmt > dAdvBalAmt) { dAdvRAmt = dAdvBalAmt; }
                            dAdvBalAmt = dAdvBalAmt - dAdvRAmt;
                            dTNetAmt = dTNetAmt - dAdvRAmt;

                            sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                    "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + ", 0," + dAdvRAmt + ") SELECT SCOPE_IDENTITY();";
                            cmd = new SqlCommand(sSql, conn, tran);
                            iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();
                        }

                        else
                        {
                            if (sRSchType == "A")
                            {
                                drT = dtReceipt.Select("SchType = 'A'");
                            }
                            else if (sRSchType == "O")
                            {
                                drT = dtReceipt.Select("SchType = 'O' and Id = " + iROtherCostId + "");
                            }
                            else
                            {
                                drT = dtReceipt.Select("SchType = 'R' and Id = " + iReceiptId + "");
                            }


                            decimal dRTAmt = 0;
                            decimal dRRAmt = 0;

                            if (drT.Length > 0)
                            {
                                dRTAmt = Convert.ToDecimal(drT[0]["Amount"].ToString());
                                dRRAmt = Convert.ToDecimal(drT[0]["RAmount"].ToString());
                            }

                            if (dRAmt > dRTAmt - dRRAmt)
                            {
                                dRAmt = dRTAmt - dRRAmt;
                            }

                            if (drT.Length > 0)
                            {
                                drT[0]["RAmount"] = dRRAmt + dRAmt;
                            }

                            if (dAmt == 0) { dRPer = 0; }
                            else dRPer = (dRAmt / dAmt) * 100;

                            dBalAmt = dBalAmt - dRAmt;

                            sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                    "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + "," + dRAmt + "," + dRAmt + ") SELECT SCOPE_IDENTITY();";
                            cmd = new SqlCommand(sSql, conn, tran);
                            iRSchId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();

                            dQNetAmt = dRAmt;

                            dtQualT = new DataTable();
                            dv = new DataView(dtQual);
                            dv.RowFilter = "SchType = '" + sRSchType + "' and ReceiptTypeId = " + iReceiptId + " and OtherCostId = " + iROtherCostId;
                            dtQualT = dv.ToTable();
                            dv.Dispose();
                            if (dtQualT.Rows.Count > 0)
                            {
                                QualVBC = new Collection();

                                for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                                {
                                    RAQual = new cRateQualR();
                                    bService = Convert.ToBoolean(dtQualT.Rows[Q]["Service"]);

                                    DataTable dtTDS = new DataTable();
                                    if (Convert.ToInt32(dtQualT.Rows[Q]["QualTypeId"]) == 2)
                                    {
                                        if (bService == true)
                                            dtTDS = GetSTSettings("G", dSchDate, conn, tran);
                                        else
                                            dtTDS = GetSTSettings("F", dSchDate, conn, tran);
                                    }
                                    else
                                    {
                                        dtTDS = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), dSchDate, "B", conn, tran);
                                    }

                                    RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                                    if (dtTDS.Rows.Count > 0)
                                    {
                                        RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                                        RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                                        RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                                        RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                                        RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                                        RAQual.HEDValue = (dRAmt * Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric))) / 100;
                                        RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Taxable"], CommFun.datatypes.vartypenumeric));
                                    }

                                    DataTable dtQ = new DataTable();
                                    dtQ = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), dSchDate, "B", conn, tran);
                                    //dtQ = QualifierSelect(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), "B", dSchDate, conn, tran);
                                    if (dtQ.Rows.Count > 0)
                                    {
                                        RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                                        RAQual.Amount = 0;
                                        RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                                    }

                                    QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                                }

                                Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                                dQBaseAmt = dRAmt;
                                dQNetAmt = dRAmt; decimal dTaxAmt = 0;
                                decimal dVATAmt = 0;

                                if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, dSchDate, ref dVATAmt) == true)
                                {
                                    foreach (Qualifier.cRateQualR d in QualVBC)
                                    {
                                        sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount,NetPer,HEDPer,HEDValue,TaxablePer,TaxableValue) " +
                                                "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                                "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + "," + d.NetPer + "," + d.HEDPer + "," + d.HEDValue + "," + d.TaxablePer + "," + d.TaxableValue + ")";
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                }

                                sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }


                            dTNetAmt = dTNetAmt + dQNetAmt;

                        }

                        //if (dBalAmt <= 0) { break; }
                    }

                    if (dBalAmt > 0)
                    {
                        for (int j = 0; j < dtReceiptOrder.Rows.Count; j++)
                        {
                            dRAmt = dBalAmt;

                            sRSchType = dtReceiptOrder.Rows[j]["SchType"].ToString();
                            iReceiptId = Convert.ToInt32(dtReceiptOrder.Rows[j]["ReceiptTypeId"].ToString());
                            iROtherCostId = Convert.ToInt32(dtReceiptOrder.Rows[j]["OtherCostId"].ToString());

                            if (sRSchType == "O")
                            {
                                drT = dtReceipt.Select("SchType = 'O' and Id = " + iROtherCostId + "");
                            }
                            else
                            {
                                drT = dtReceipt.Select("SchType = 'R' and Id = " + iReceiptId + "");
                            }

                            decimal dRTAmt = 0;
                            decimal dRRAmt = 0;

                            if (drT.Length > 0)
                            {
                                dRTAmt = Convert.ToDecimal(drT[0]["Amount"].ToString());
                                dRRAmt = Convert.ToDecimal(drT[0]["RAmount"].ToString());
                            }

                            if (dRAmt > dRTAmt - dRRAmt)
                            {
                                dRAmt = dRTAmt - dRRAmt;
                            }

                            if (drT.Length > 0)
                            {
                                drT[0]["RAmount"] = dRRAmt + dRAmt;
                            }

                            if (dRAmt > 0)
                            {
                                decimal dPCAmt = 0;
                                bool bAns = false;
                                sSql = "Select SchId,Amount,NetAmount from dbo.FlatReceiptType Where PaymentSchId = " + iPaymentSchId + " and " +
                                        "FlatId= " + argFlatId + " and ReceiptTypeId= " + iReceiptId + " and OtherCostId = " + iROtherCostId + " and SchType= '" + sRSchType + "'";
                                cmd = new SqlCommand(sSql, conn, tran);
                                sdr = cmd.ExecuteReader();
                                DataTable dtP = new DataTable();
                                dtP.Load(sdr);
                                sdr.Close();
                                cmd.Dispose();

                                if (dtP.Rows.Count > 0)
                                {
                                    dPCAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtP.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
                                    dTNetAmt = dTNetAmt - dPCAmt;
                                    dBalAmt = dBalAmt + dPCAmt;
                                    iRSchId = Convert.ToInt32(dtP.Rows[0]["SchId"].ToString());
                                    bAns = true;
                                }
                                dtP.Dispose();

                                if (bAns == true)
                                {
                                    dRAmt = dRAmt + dPCAmt;
                                    dRPer = (dRAmt / dAmt) * 100;

                                    sSql = "Update dbo.FlatReceiptType Set Amount= " + dRAmt + ",Percentage = " + dRPer + ",NetAmount = " + dRAmt + " Where SchId = " + iRSchId;
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();

                                    sSql = "Delete from dbo.FlatReceiptQualifier Where SchId = " + iRSchId;
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                                else
                                {
                                    dRPer = (dRAmt / dAmt) * 100;

                                    sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                            "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + "," + dRAmt + "," + dRAmt + ") SELECT SCOPE_IDENTITY();";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                                    cmd.Dispose();
                                }

                                dQNetAmt = dRAmt;

                                dtQualT = new DataTable();
                                dv = new DataView(dtQual);

                                if (sRSchType == "O")
                                    dv.RowFilter = "SchType = 'O' and ReceiptTypeId = 0 and OtherCostId = " + iROtherCostId + "";
                                else
                                    dv.RowFilter = "SchType = 'R' and ReceiptTypeId = " + iReceiptId + " and OtherCostId = 0";

                                dtQualT = dv.ToTable();
                                dv.Dispose();
                                if (dtQualT.Rows.Count > 0)
                                {
                                    QualVBC = new Collection();

                                    for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                                    {
                                        RAQual = new cRateQualR();
                                        bService = Convert.ToBoolean(dtQualT.Rows[Q]["Service"]);

                                        DataTable dtTDS = new DataTable();                                        
                                        if (Convert.ToInt32(dtQualT.Rows[Q]["QualTypeId"]) == 2)
                                        {
                                            if (bService == true)
                                                dtTDS = GetSTSettings("G", dSchDate, conn, tran);
                                            else
                                                dtTDS = GetSTSettings("F", dSchDate, conn, tran);
                                        }
                                        else
                                        {
                                            dtTDS = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), dSchDate, "B", conn, tran);
                                        }

                                        RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                                        if (dtTDS.Rows.Count > 0)
                                        {
                                            RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                                            RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                                            RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                                            RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                                            RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                                            RAQual.HEDValue = (dRAmt * Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric))) / 100;
                                            RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Taxable"], CommFun.datatypes.vartypenumeric));
                                        }

                                        DataTable dtQ = new DataTable();
                                        dtQ = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), dSchDate, "B", conn, tran);
                                        //dtQ = QualifierSelect(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), "B", dSchDate, conn, tran);
                                        if (dtQ.Rows.Count > 0)
                                        {
                                            RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                                            RAQual.Amount = 0;
                                            RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                                        }

                                        QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                                    }

                                    Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                                    dQBaseAmt = dRAmt;
                                    dQNetAmt = dRAmt; decimal dTaxAmt = 0;
                                    decimal dVATAmt = 0;

                                    if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                                    {
                                        foreach (Qualifier.cRateQualR d in QualVBC)
                                        {
                                            sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount,NetPer,HEDPer,HEDValue,TaxablePer,TaxableValue) " +
                                                    "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                                    "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + "," + d.NetPer + "," + d.HEDPer + "," + d.HEDValue + "," + d.TaxablePer + "," + d.TaxableValue + ")";
                                            cmd = new SqlCommand(sSql, conn, tran);
                                            cmd.ExecuteNonQuery();
                                            cmd.Dispose();
                                        }
                                    }
                                    sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }

                                dTNetAmt = dTNetAmt + dQNetAmt;
                                dBalAmt = dBalAmt - dRAmt;
                                if (dBalAmt <= 0) { break; }
                            }
                        }

                    }

                    //modified
                    sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dTNetAmt + "  Where PaymentSchId = " + iPaymentSchId;
                    //sSql = "Update PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dQNetAmt + "  Where PaymentSchId = " + iPaymentSchId;

                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            dt.Dispose();


            if (bAdvance == false)
            {
                sSql = "Insert into dbo.PaymentScheduleFlat(FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,Amount,NetAmount,PreStageTypeId,SortOrder) " +
                        "Values(" + argFlatId + ",0," + iCCId + ",'A','Advance',0,0,0,NULL,0," + dAdvAmt + ",0,0)";
                //sSql = "Insert Into dbo.PaymentScheduleFlat(FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,Amount,NetAmount,PreStageTypeId,SortOrder) " +
                //      "Values(" + argFlatId + ",0," + iCCId + ",'A','Advance',0,0,0,0," + dAdvAmt + ",0,0)";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }

            sSql = "Update dbo.PaymentScheduleFlat Set Advance=0";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "UPDATE PaymentScheduleFlat SET Advance=SummedQty FROM " +
                    " PaymentScheduleFlat A JOIN (SELECT PaymentSchId, SUM(NetAmount) SummedQty " +
                    " FROM FlatReceiptType WHERE SchType='A' GROUP BY PaymentSchId ) CCA ON A.PaymentSchId=CCA.PaymentSchId";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            //Schedule Date
            SqlDataReader dr, sdr1, sdr2; DataTable dt1; int iStgId = 0, iTempId = 0;
            int iDateAfter = 0, iDuration = 0; string sDurType = ""; DateTime SchDate; int iSortOrder = 0;
            DateTime StartDate = DateTime.Now; DateTime EndDate = DateTime.Now; int ipre = 0;


            sSql = "Update dbo.PaymentScheduleFlat Set PreStageTypeId=-1 Where FlatId=" + argFlatId + " And TemplateId In(  " +
                    " Select TemplateId From dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " " +
                    " And CostCentreId=" + iCCId + " And PreStageTypeId=-1)";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Select FinaliseDate from dbo.BuyerDetail Where Status='S' And FlatId=" + argFlatId + "";
            cmd = new SqlCommand(sSql, conn, tran);
            dr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(dr); cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                FinaliseDate = Convert.ToDateTime(dt.Rows[0]["FinaliseDate"]);


                sSql = "Select TemplateId,PreStageTypeId from dbo.PaymentScheduleFlat Where FlatId=" + argFlatId + " And PreStageTypeId=-1";
                cmd = new SqlCommand(sSql, conn, tran);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr); cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    iStgId = Convert.ToInt32(dt.Rows[0]["PreStageTypeId"]);
                    iTempId = Convert.ToInt32(dt.Rows[0]["TemplateId"]);
                }
                dt.Dispose();

                sSql = "Select SortOrder From dbo.PaymentScheduleFlat Where FlatId=" + argFlatId + " And TemplateId=" + iTempId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                sdr2 = cmd.ExecuteReader();
                dt1 = new DataTable();
                dt1.Load(sdr2); cmd.Dispose();
                dt1.Dispose();

                if (dt1.Rows.Count > 0)
                {
                    iSortOrder = Convert.ToInt32(dt1.Rows[0]["SortOrder"]);
                }

                sSql = "select StartDate,EndDate From ProjectInfo Where CostCentreId= " + iCCId;
                cmd = new SqlCommand(sSql, conn, tran);
                dt = new DataTable();
                dr = cmd.ExecuteReader();
                dt.Load(dr);
                dt.Dispose();

                if (dt.Rows.Count > 0)
                {
                    StartDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["StartDate"], CommFun.datatypes.VarTypeDate));
                    EndDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["EndDate"], CommFun.datatypes.VarTypeDate));
                }

                sSql = "Update dbo.PaymentScheduleFlat Set SchDate='" + FinaliseDate.ToString("dd-MMM-yyyy") + "'" +
                    " Where TemplateId=" + iTempId + " And FlatId=" + argFlatId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "Update dbo.PaymentScheduleFlat Set SchDate='" + FinaliseDate.ToString("dd-MMM-yyyy") + "'" +
                    " Where TemplateId=0 And FlatId=" + argFlatId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                if (iStgId == -1)
                {
                    if (iStgId == -1)
                        sSql = "Select A.PreStageTypeId,A.CostCentreId,A.TemplateId,A.DateAfter,A.Duration,A.Durationtype from dbo.PaymentScheduleFlat A" +
                        " Left Join dbo.ProgressBillRegister B On A.FlatId=B.FlatId " +
                        " Where A.FlatId=" + argFlatId + " And A.SortOrder>=" + iSortOrder + "" +
                        " And A.PaymentSchId Not In " +
                        " (Select PaySchId From dbo.ProgressBillRegister Where FlatId=" + argFlatId + ") Order By A.SortOrder";

                    cmd = new SqlCommand(sSql, conn, tran);
                    sdr1 = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(sdr1);
                    cmd.Dispose();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        iTempId = Convert.ToInt32(dt.Rows[i]["TemplateId"]);
                        ipre = Convert.ToInt32(dt.Rows[i]["PreStageTypeId"]);
                        iDateAfter = Convert.ToInt32(dt.Rows[i]["DateAfter"]);
                        iDuration = Convert.ToInt32(dt.Rows[i]["Duration"]);
                        sDurType = dt.Rows[i]["DurationType"].ToString();

                        if (ipre == -1) { } else if (ipre == -2) { } else if (ipre == -3) { } else if (ipre == 0) { } else { iTempId = ipre; }

                        sSql = "Select SchDate From dbo.PaymentScheduleFlat Where CostCentreId=" + dt.Rows[i]["CostCentreId"] + " And FlatId=" + argFlatId + "" +
                                " And TemplateId=" + iTempId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        DataTable dtDate = new DataTable();
                        dr = cmd.ExecuteReader();
                        dtDate.Load(dr);
                        dtDate.Dispose();

                        if (ipre == -1) { SchDate = Convert.ToDateTime(CommFun.IsNullCheck(FinaliseDate, CommFun.datatypes.VarTypeDate)); }
                        else if (ipre == -2) { SchDate = StartDate; }
                        else if (ipre == -3) { SchDate = EndDate; }
                        else
                            SchDate = Convert.ToDateTime(CommFun.IsNullCheck(dtDate.Rows[0]["SchDate"], CommFun.datatypes.VarTypeDate));

                        if (sDurType == "D")
                        { if (iDateAfter == 0) SchDate = SchDate.AddDays(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }
                        else if (sDurType == "M")
                        { if (iDateAfter == 0) SchDate = SchDate.AddMonths(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }


                        sSql = "Update dbo.PaymentScheduleFlat Set SchDate=@SchDate" +
                            " Where TemplateId=" + dt.Rows[i]["TemplateId"] + " And FlatId=" + argFlatId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        SqlParameter dateParameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@SchDate" };
                        if (SchDate == DateTime.MinValue)
                            dateParameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            dateParameter.Value = SchDate;
                        cmd.Parameters.Add(dateParameter);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                    }
                }
            }
        }

        public static void UpdateBuyerScheduleQual(int argFlatId, DataTable argdt, SqlConnection conn, SqlTransaction tran)
        {
            string sSql = "";

            SqlDataReader sdr;
            SqlCommand cmd;
            DataTable dt = new DataTable();

            int iCCId = 0;
            int iFlatTypeId = 0;
            int iPayTypeId = 0;
            decimal dBaseAmt = 0;
            decimal dAdvAmt = 0;
            decimal dAdvBalAmt = 0;
            decimal dLandAmt = 0;
            decimal dNetAmt = 0;
            decimal dOtherAmt = 0;
            decimal dRAmt = 0;
            int iReceiptId = 0;
            int iROtherCostId = 0;
            string sRSchType = "";
            bool bAdvance = false;
            int iPaymentSchId = 0;
            string sSchType = "";
            int iOtherCostId = 0;
            decimal dRPer = 0;
            decimal dSchPercent = 0;
            decimal dQBaseAmt = 0;
            decimal dQNetAmt = 0;
            int iTemplateId = 0;
            int iSchId = 0;
            int iRSchId = 0;
            decimal dTNetAmt = 0;
            decimal dBalAmt = 0;
            bool bPayTypewise = false, bLCBon = false;
            decimal dTotalTax = 0;
            decimal dAdv = 0;
            DataRow[] drT;
            cRateQualR RAQual;
            Collection QualVBC;

            DataTable dtReceipt = new DataTable();

            sSql = "Delete from dbo.PaymentScheduleFlat Where FlatId= " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete from dbo.FlatReceiptQualifier Where SchId in (Select SchId from dbo.FlatReceiptType Where FlatId= " + argFlatId + ")";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete From dbo.PaySchTaxFlat Where FlatId=" + argFlatId + " ";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete from dbo.FlatReceiptType Where FlatId= " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();


            //sSql = "Select FlatTypeId,CostCentreId,PayTypeId,BaseAmt,AdvAmount,USLandAmt from dbo.FlatDetails Where FlatId= " + argFlatId;//modified
            sSql = "Select FlatTypeId,CostCentreId,PayTypeId,BaseAmt,AdvAmount,LandRate,Guidelinevalue,USLandAmt from dbo.FlatDetails Where FlatId= " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0)
            {
                iCCId = Convert.ToInt32(dt.Rows[0]["CostCentreId"].ToString());
                iFlatTypeId = Convert.ToInt32(dt.Rows[0]["FlatTypeId"].ToString());
                iPayTypeId = Convert.ToInt32(dt.Rows[0]["PayTypeId"].ToString());
                bPayTypewise = FlatDetailsDL.GetTypewise(iPayTypeId);
                dBaseAmt = Convert.ToDecimal(dt.Rows[0]["BaseAmt"].ToString());
                dAdvAmt = Convert.ToDecimal(dt.Rows[0]["AdvAmount"].ToString());

                sSql = "Select LCBasedon From dbo.ProjectInfo Where CostCentreId= " + iCCId;
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                DataTable dtPI = new DataTable();
                dtPI.Load(sdr);
                sdr.Close();
                cmd.Dispose();
                if (dtPI.Rows.Count > 0) { bLCBon = Convert.ToBoolean(dtPI.Rows[0]["LCBasedon"]); }
                if (bLCBon == false) { dLandAmt = Convert.ToDecimal(dt.Rows[0]["LandRate"].ToString()); }
                else { dLandAmt = Convert.ToDecimal(dt.Rows[0]["USLandAmt"].ToString()); }
            }
            dt.Dispose();

            sSql = "Select TemplateId From dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " and CostCentreId = " + iCCId + " and SchType='A'";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0) { bAdvance = true; }
            dt.Dispose();

            sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                    "Where FlatId = " + argFlatId + " and OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans Where PayTypeId=" + iPayTypeId + " and CostCentreId=" + iCCId + ")";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0) { dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }
            dt.Dispose();

            sSql = "Select QualifierId,Amount from dbo.FlatTax Where FlatId = " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtTx = new DataTable();
            dtTx.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            decimal dT = 0;
            if (dtTx.Rows.Count > 0)
            {
                for (int i = 0; i < dtTx.Rows.Count; i++)
                {
                    dTotalTax = Convert.ToDecimal(dtTx.Rows[i]["Amount"]);
                    dT = dT + dTotalTax;
                }
            }

            if (bPayTypewise == false)
            { dNetAmt = dBaseAmt + dOtherAmt + dT; }
            else
            { dNetAmt = dBaseAmt + dOtherAmt; }
            if (bAdvance == true) { dNetAmt = dNetAmt - dAdvAmt; }

            if (argdt.Rows.Count > 0)
            {
                for (int i = 0; i < argdt.Rows.Count; i++)
                {
                    string sDate = string.Format(Convert.ToDateTime(CommFun.IsNullCheck(argdt.Rows[i]["SchDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MMM-yyyy"));
                    if (argdt.Rows[i]["SchDate"].ToString() == "")
                    {
                        sDate = "NULL";
                        sSql = "Insert into dbo.PaymentScheduleFlat(FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId," +
                        " OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId,SortOrder) " +
                        " Values(" + argdt.Rows[i]["FlatId"] + "," + argdt.Rows[i]["TemplateId"] + "," + argdt.Rows[i]["CostCentreId"] + "," +
                        " '" + argdt.Rows[i]["SchType"] + "','" + argdt.Rows[i]["Description"] + "'," + argdt.Rows[i]["SchDescId"] + "," +
                        " " + argdt.Rows[i]["StageId"] + "," + argdt.Rows[i]["OtherCostId"] + "," + sDate + "," +
                        " '" + argdt.Rows[i]["DateAfter"] + "'," + argdt.Rows[i]["Duration"] + ",'" + argdt.Rows[i]["DurationType"] + "'," +
                        " " + argdt.Rows[i]["SchPercent"] + "," + argdt.Rows[i]["Amount"] + "," + argdt.Rows[i]["PreStageTypeId"] + "," +
                        " " + argdt.Rows[i]["SortOrder"] + ")";
                    }
                    else
                    {
                        sSql = "Insert into dbo.PaymentScheduleFlat(FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId," +
                        " OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId,SortOrder) " +
                        " Values(" + argdt.Rows[i]["FlatId"] + "," + argdt.Rows[i]["TemplateId"] + "," + argdt.Rows[i]["CostCentreId"] + "," +
                        " '" + argdt.Rows[i]["SchType"] + "','" + argdt.Rows[i]["Description"] + "'," + argdt.Rows[i]["SchDescId"] + "," +
                        " " + argdt.Rows[i]["StageId"] + "," + argdt.Rows[i]["OtherCostId"] + ",'" + sDate + "'," +
                        " '" + argdt.Rows[i]["DateAfter"] + "'," + argdt.Rows[i]["Duration"] + ",'" + argdt.Rows[i]["DurationType"] + "'," +
                        " " + argdt.Rows[i]["SchPercent"] + "," + argdt.Rows[i]["Amount"] + "," + argdt.Rows[i]["PreStageTypeId"] + "," +
                        " " + argdt.Rows[i]["SortOrder"] + ")";
                    }
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }

            sSql = "Select ReceiptTypeId,OtherCostId,SchType from dbo.ReceiptTypeOrder " +
                    "Where PayTypeId = " + iPayTypeId + " and CostCentreId=" + iCCId + " and SchType <>'A' Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtReceiptOrder = new DataTable();
            dtReceiptOrder.Load(sdr);
            sdr.Close();
            cmd.Dispose();


            sSql = "Select OtherCostId,Flag,Amount from dbo.FlatOtherCost Where FlatId = " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtT = new DataTable();
            dtT.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            dtReceipt.Columns.Add("Id", typeof(int));
            dtReceipt.Columns.Add("SchType", typeof(string));
            dtReceipt.Columns.Add("Amount", typeof(decimal));
            dtReceipt.Columns.Add("RAmount", typeof(decimal));

            DataRow drR;
            drR = dtReceipt.NewRow();
            drR["Id"] = 1;
            drR["SchType"] = "A";
            drR["Amount"] = dAdvAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            drR = dtReceipt.NewRow();
            drR["Id"] = 2;
            drR["SchType"] = "R";
            drR["Amount"] = dLandAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            drR = dtReceipt.NewRow();
            drR["Id"] = 3;
            drR["SchType"] = "R";
            drR["Amount"] = dBaseAmt - dLandAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            for (int i = 0; i < dtT.Rows.Count; i++)
            {
                drR = dtReceipt.NewRow();
                drR["Id"] = Convert.ToInt32(dtT.Rows[i]["OtherCostId"].ToString());
                drR["SchType"] = "O";
                drR["Amount"] = Convert.ToDecimal(dtT.Rows[i]["Amount"].ToString());
                drR["RAmount"] = 0;
                dtReceipt.Rows.Add(drR);
            }

            if (bPayTypewise == false)
            {
                for (int i = 0; i < dtTx.Rows.Count; i++)
                {
                    drR = dtReceipt.NewRow();
                    drR["Id"] = Convert.ToInt32(dtTx.Rows[i]["QualifierId"].ToString());
                    drR["SchType"] = "Q";
                    drR["Amount"] = Convert.ToDecimal(dtTx.Rows[i]["Amount"].ToString());
                    drR["RAmount"] = 0;
                    dtReceipt.Rows.Add(drR);
                }
            }

            sSql = "Select SchId,TemplateId,ReceiptTypeId,Percentage,OtherCostId,SchType from dbo.CCReceiptType " +
                    "Where TemplateId in (Select TemplateId from dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " and CostCentreId=" + iCCId + ") Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtTemp = new DataTable();
            dtTemp.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            sSql = "Select * from dbo.CCReceiptQualifier Where CostCentreId=" + iCCId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtQual = new DataTable();
            dtQual.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            sSql = "Select PaymentSchId,TemplateId,SchType,OtherCostId,SchPercent from dbo.PaymentScheduleFlat Where FlatId = " + argFlatId + " Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            decimal dAmt = 0;
            DataView dv;
            decimal dAdvRAmt = 0;

            DataTable dtTempT;
            DataTable dtQualT;
            dAdvBalAmt = dAdvAmt;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iPaymentSchId = Convert.ToInt32(dt.Rows[i]["PaymentSchId"].ToString());
                iTemplateId = Convert.ToInt32(dt.Rows[i]["TemplateId"].ToString());
                sSchType = dt.Rows[i]["SchType"].ToString();
                iOtherCostId = Convert.ToInt32(dt.Rows[i]["OtherCostId"].ToString());
                dSchPercent = Convert.ToDecimal(dt.Rows[i]["SchPercent"].ToString());
                dTNetAmt = 0;

                dAmt = 0;
                if (sSchType == "A")
                {
                    dAmt = dAdvAmt;
                }
                else if (sSchType == "O")
                {
                    dv = new DataView(dtT);
                    dv.RowFilter = "OtherCostId = " + iOtherCostId;
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        dAmt = Convert.ToDecimal(dv.ToTable().Rows[0]["Amount"].ToString());
                        if (dv.ToTable().Rows[0]["Flag"].ToString() == "-") { dAmt = dAmt * (-1); }
                    }
                    dv.Dispose();
                }
                else
                {
                    dAmt = dNetAmt * dSchPercent / 100;
                }

                dtTempT = new DataTable();
                dv = new DataView(dtTemp);
                dv.RowFilter = "TemplateId = " + iTemplateId;
                dtTempT = dv.ToTable();
                dv.Dispose();

                if (dtTempT.Rows.Count == 1 && sSchType == "O")
                {

                    sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                            "Values(" + iPaymentSchId + "," + argFlatId + ",0," + iOtherCostId + ",'" + sSchType + "',100," + dAmt + "," + dAmt + ") SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(sSql, conn, tran);
                    iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();

                    drT = dtReceipt.Select("SchType = 'O' and Id = " + iOtherCostId + "");

                    if (drT.Length > 0)
                    {
                        drT[0]["RAmount"] = dAmt;
                    }

                    dQNetAmt = dAmt;

                    dtQualT = new DataTable();
                    dv = new DataView(dtQual);
                    dv.RowFilter = "SchType = '" + sSchType + "' and OtherCostId = " + iOtherCostId;
                    dtQualT = dv.ToTable();
                    dv.Dispose();

                    if (dtQualT.Rows.Count > 0)
                    {
                        QualVBC = new Collection();

                        for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                        {
                            RAQual = new cRateQualR();

                            RAQual.Add_Less_Flag = dtQualT.Rows[Q]["Add_Less_Flag"].ToString();
                            RAQual.Amount = 0;
                            RAQual.Expression = dtQualT.Rows[Q]["Expression"].ToString();
                            RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                            RAQual.ExpPer = Convert.ToDecimal(dtQualT.Rows[Q]["ExpPer"].ToString());
                            RAQual.SurPer = Convert.ToDecimal(dtQualT.Rows[Q]["SurCharge"].ToString());
                            RAQual.EDPer = Convert.ToDecimal(dtQualT.Rows[Q]["EDCess"].ToString());

                            QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                        }

                        Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                        dQBaseAmt = dAmt;
                        dQNetAmt = dAmt; decimal dTaxAmt = 0;
                        decimal dVATAmt = 0;

                        if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                        {
                            foreach (Qualifier.cRateQualR d in QualVBC)
                            {
                                sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount) " +
                                        "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                        "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }

                        if (bPayTypewise == true)
                        {
                            sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }

                    if (bPayTypewise == true)
                        sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dQNetAmt + "  Where PaymentSchId = " + iPaymentSchId;
                    else
                        sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dAmt + "  Where PaymentSchId = " + iPaymentSchId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    dTNetAmt = dTNetAmt + dQNetAmt;
                }

                else
                {
                    dBalAmt = dAmt;
                    for (int j = 0; j < dtTempT.Rows.Count; j++)
                    {
                        iSchId = Convert.ToInt32(dtTempT.Rows[j]["SchId"].ToString());
                        dRPer = Convert.ToDecimal(dtTempT.Rows[j]["Percentage"].ToString());
                        sRSchType = dtTempT.Rows[j]["SchType"].ToString();
                        iReceiptId = Convert.ToInt32(dtTempT.Rows[j]["ReceiptTypeId"].ToString());
                        iROtherCostId = Convert.ToInt32(dtTempT.Rows[j]["OtherCostId"].ToString());

                        if (dRPer != 0) { dRAmt = dAmt * dRPer / 100; }
                        else { dRAmt = dBalAmt; }

                        if (dRAmt > dBalAmt) { dRAmt = dBalAmt; }


                        if (sRSchType == "A" && bAdvance == false)
                        {

                            dAdvRAmt = dAdvAmt * dRPer / 100;
                            if (dAdvRAmt > dAdvBalAmt) { dAdvRAmt = dAdvBalAmt; }
                            dAdvBalAmt = dAdvBalAmt - dAdvRAmt;
                            dTNetAmt = dTNetAmt - dAdvRAmt;

                            dAdv = dAdvRAmt;
                            sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                    "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + ", 0," + dAdvRAmt + ") SELECT SCOPE_IDENTITY();";
                            cmd = new SqlCommand(sSql, conn, tran);
                            iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();
                        }

                        else
                        {
                            dAdv = 0;
                            if (sRSchType == "A")
                            {
                                drT = dtReceipt.Select("SchType = 'A'");
                            }
                            else if (sRSchType == "O")
                            {
                                drT = dtReceipt.Select("SchType = 'O' and Id = " + iROtherCostId + "");
                            }
                            else if (sRSchType == "Q")
                            {
                                drT = dtReceipt.Select("SchType = 'Q' and Id = " + iReceiptId + "");
                            }
                            else
                            {
                                drT = dtReceipt.Select("SchType = 'R' and Id = " + iReceiptId + "");
                            }


                            decimal dRTAmt = 0;
                            decimal dRRAmt = 0;

                            if (drT.Length > 0)
                            {
                                dRTAmt = Convert.ToDecimal(drT[0]["Amount"].ToString());
                                dRRAmt = Convert.ToDecimal(drT[0]["RAmount"].ToString());
                            }

                            if (dRAmt > dRTAmt - dRRAmt)
                            {
                                dRAmt = dRTAmt - dRRAmt;
                            }

                            if (drT.Length > 0)
                            {
                                drT[0]["RAmount"] = dRRAmt + dRAmt;
                            }

                            if (dAmt == 0) { dRPer = 0; }
                            else dRPer = (dRAmt / dAmt) * 100;

                            dBalAmt = dBalAmt - dRAmt;

                            sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                    "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + "," + dRAmt + "," + dRAmt + ") SELECT SCOPE_IDENTITY();";
                            cmd = new SqlCommand(sSql, conn, tran);
                            iRSchId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();

                            if (bPayTypewise == false && sRSchType == "Q")
                            {
                                sSql = "Insert Into dbo.PaySchTaxFlat(PaymentSchId,FlatId,QualifierId,Percentage,Amount,Sel) " +
                                        "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + dRPer + "," + dRAmt + ",'" + true + "')";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }

                            dQNetAmt = dRAmt;

                            dtQualT = new DataTable();
                            dv = new DataView(dtQual);
                            dv.RowFilter = "SchType = '" + sRSchType + "' and ReceiptTypeId = " + iReceiptId + " and OtherCostId = " + iROtherCostId;
                            dtQualT = dv.ToTable();
                            dv.Dispose();
                            if (dtQualT.Rows.Count > 0)
                            {
                                QualVBC = new Collection();

                                for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                                {
                                    RAQual = new cRateQualR();

                                    RAQual.Add_Less_Flag = dtQualT.Rows[Q]["Add_Less_Flag"].ToString();
                                    RAQual.Amount = 0;
                                    RAQual.Expression = dtQualT.Rows[Q]["Expression"].ToString();
                                    RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                                    RAQual.ExpPer = Convert.ToDecimal(dtQualT.Rows[Q]["ExpPer"].ToString());
                                    RAQual.SurPer = Convert.ToDecimal(dtQualT.Rows[Q]["SurCharge"].ToString());
                                    RAQual.EDPer = Convert.ToDecimal(dtQualT.Rows[Q]["EDCess"].ToString());

                                    QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                                }

                                Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                                dQBaseAmt = dRAmt;
                                dQNetAmt = dRAmt; decimal dTaxAmt = 0;
                                decimal dVATAmt = 0;

                                if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                                {
                                    foreach (Qualifier.cRateQualR d in QualVBC)
                                    {
                                        sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount) " +
                                                "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                                "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + ")";
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                }
                                if (bPayTypewise == true)
                                {
                                    sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                            }
                            dTNetAmt = dTNetAmt + dQNetAmt;
                        }
                    }

                    if (dBalAmt > 0)
                    {
                        for (int j = 0; j < dtReceiptOrder.Rows.Count; j++)
                        {
                            dRAmt = dBalAmt;

                            sRSchType = dtReceiptOrder.Rows[j]["SchType"].ToString();
                            iReceiptId = Convert.ToInt32(dtReceiptOrder.Rows[j]["ReceiptTypeId"].ToString());
                            iROtherCostId = Convert.ToInt32(dtReceiptOrder.Rows[j]["OtherCostId"].ToString());

                            if (sRSchType == "O")
                            {
                                drT = dtReceipt.Select("SchType = 'O' and Id = " + iROtherCostId + "");
                            }
                            else
                            {
                                drT = dtReceipt.Select("SchType = 'R' and Id = " + iReceiptId + "");
                            }

                            decimal dRTAmt = 0;
                            decimal dRRAmt = 0;

                            if (drT.Length > 0)
                            {
                                dRTAmt = Convert.ToDecimal(drT[0]["Amount"].ToString());
                                dRRAmt = Convert.ToDecimal(drT[0]["RAmount"].ToString());
                            }

                            if (dRAmt > dRTAmt - dRRAmt)
                            {
                                dRAmt = dRTAmt - dRRAmt;
                            }

                            if (drT.Length > 0)
                            {
                                drT[0]["RAmount"] = dRRAmt + dRAmt;
                            }

                            if (dRAmt > 0)
                            {
                                decimal dPCAmt = 0;
                                bool bAns = false;
                                sSql = "Select SchId,Amount,NetAmount from dbo.FlatReceiptType Where PaymentSchId = " + iPaymentSchId + " and " +
                                        "FlatId= " + argFlatId + " and ReceiptTypeId= " + iReceiptId + " and OtherCostId = " + iROtherCostId + " and SchType= '" + sRSchType + "'";
                                cmd = new SqlCommand(sSql, conn, tran);
                                sdr = cmd.ExecuteReader();
                                DataTable dtP = new DataTable();
                                dtP.Load(sdr);
                                sdr.Close();
                                cmd.Dispose();

                                if (dtP.Rows.Count > 0)
                                {
                                    dPCAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtP.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
                                    dTNetAmt = dTNetAmt - dPCAmt;
                                    dBalAmt = dBalAmt + dPCAmt;
                                    iRSchId = Convert.ToInt32(dtP.Rows[0]["SchId"].ToString());
                                    bAns = true;
                                }
                                dtP.Dispose();

                                if (bAns == true)
                                {
                                    dRAmt = dRAmt + dPCAmt;
                                    dRPer = (dRAmt / dAmt) * 100;

                                    sSql = "Update dbo.FlatReceiptType Set Amount= " + dRAmt + ",Percentage = " + dRPer + ",NetAmount = " + dRAmt + " Where SchId = " + iRSchId;
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();

                                    sSql = "Delete from dbo.FlatReceiptQualifier Where SchId = " + iRSchId;
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                                else
                                {
                                    dRPer = (dRAmt / dAmt) * 100;

                                    sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                            "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + "," + dRAmt + "," + dRAmt + ") SELECT SCOPE_IDENTITY();";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                                    cmd.Dispose();
                                }

                                dQNetAmt = dRAmt;

                                dtQualT = new DataTable();
                                dv = new DataView(dtQual);

                                if (sRSchType == "O")
                                {
                                    dv.RowFilter = "SchType = 'O' and ReceiptTypeId = 0 and OtherCostId = " + iROtherCostId + "";

                                }
                                else
                                {
                                    dv.RowFilter = "SchType = 'R' and ReceiptTypeId = " + iReceiptId + " and OtherCostId = 0";
                                }

                                dtQualT = dv.ToTable();
                                dv.Dispose();
                                if (dtQualT.Rows.Count > 0)
                                {
                                    QualVBC = new Collection();

                                    for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                                    {
                                        RAQual = new cRateQualR();

                                        RAQual.Add_Less_Flag = dtQualT.Rows[Q]["Add_Less_Flag"].ToString();
                                        RAQual.Amount = 0;
                                        RAQual.Expression = dtQualT.Rows[Q]["Expression"].ToString();
                                        RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                                        RAQual.ExpPer = Convert.ToDecimal(dtQualT.Rows[Q]["ExpPer"].ToString());
                                        RAQual.SurPer = Convert.ToDecimal(dtQualT.Rows[Q]["SurCharge"].ToString());
                                        RAQual.EDPer = Convert.ToDecimal(dtQualT.Rows[Q]["EDCess"].ToString());

                                        QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                                    }

                                    Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                                    dQBaseAmt = dRAmt;
                                    dQNetAmt = dRAmt; decimal dTaxAmt = 0;
                                    decimal dVATAmt = 0;

                                    if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                                    {
                                        foreach (Qualifier.cRateQualR d in QualVBC)
                                        {
                                            sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount) " +
                                                    "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                                    "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + ")";
                                            cmd = new SqlCommand(sSql, conn, tran);
                                            cmd.ExecuteNonQuery();
                                            cmd.Dispose();
                                        }
                                    }

                                    if (bPayTypewise == true)
                                    {
                                        sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                }

                                dTNetAmt = dTNetAmt + dQNetAmt;
                                dBalAmt = dBalAmt - dRAmt;
                                if (dBalAmt <= 0) { break; }
                            }
                        }

                    }
                    decimal dA = (dAmt - dAdv);
                    if (bPayTypewise == true)
                        sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dTNetAmt + "  Where PaymentSchId = " + iPaymentSchId;
                    else
                        sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dA + "  Where PaymentSchId = " + iPaymentSchId;

                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            dt.Dispose();


            if (bAdvance == false)
            {
                sSql = "Insert into dbo.PaymentScheduleFlat(FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,Amount,NetAmount,PreStageTypeId,SortOrder) " +
                        "Values(" + argFlatId + ",0," + iCCId + ",'A','Advance',0,0,0,NULL,0," + dAdvAmt + ",0,0)";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }

            sSql = "Update dbo.PaymentScheduleFlat Set Advance=0";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "UPDATE PaymentScheduleFlat SET Advance=SummedQty FROM " +
                    " PaymentScheduleFlat A JOIN (SELECT PaymentSchId, SUM(NetAmount) SummedQty " +
                    " FROM FlatReceiptType WHERE SchType='A' GROUP BY PaymentSchId ) CCA ON A.PaymentSchId=CCA.PaymentSchId";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            //Schedule Date
            SqlDataReader dr, sdr1, sdr2; DataTable dt1; int iStgId = 0, iTempId = 0;
            int iDateAfter = 0, iDuration = 0; string sDurType = ""; DateTime SchDate; int iSortOrder = 0;
            DateTime StartDate = DateTime.Now; DateTime EndDate = DateTime.Now; DateTime FinaliseDate = DateTime.Now; int ipre = 0;


            sSql = "Update dbo.PaymentScheduleFlat Set PreStageTypeId=-1 Where FlatId=" + argFlatId + " And TemplateId In(  " +
                    " Select TemplateId From dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " " +
                    " And CostCentreId=" + iCCId + " And PreStageTypeId=-1)";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Select FinaliseDate from dbo.BuyerDetail Where FlatId=" + argFlatId + "";
            cmd = new SqlCommand(sSql, conn, tran);
            dr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(dr); cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                FinaliseDate = Convert.ToDateTime(dt.Rows[0]["FinaliseDate"]);


                sSql = "Select TemplateId,PreStageTypeId from dbo.PaymentScheduleFlat Where FlatId=" + argFlatId + " And PreStageTypeId=-1";
                cmd = new SqlCommand(sSql, conn, tran);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr); cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    iStgId = Convert.ToInt32(dt.Rows[0]["PreStageTypeId"]);
                    iTempId = Convert.ToInt32(dt.Rows[0]["TemplateId"]);
                }
                dt.Dispose();

                sSql = "Select SortOrder From dbo.PaymentScheduleFlat Where FlatId=" + argFlatId + " And TemplateId=" + iTempId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                sdr2 = cmd.ExecuteReader();
                dt1 = new DataTable();
                dt1.Load(sdr2); cmd.Dispose();
                dt1.Dispose();

                if (dt1.Rows.Count > 0)
                {
                    iSortOrder = Convert.ToInt32(dt1.Rows[0]["SortOrder"]);
                }

                sSql = "select StartDate,EndDate From ProjectInfo Where CostCentreId= " + iCCId;
                cmd = new SqlCommand(sSql, conn, tran);
                dt = new DataTable();
                dr = cmd.ExecuteReader();
                dt.Load(dr);
                dt.Dispose();

                if (dt.Rows.Count > 0)
                {
                    StartDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["StartDate"], CommFun.datatypes.VarTypeDate));
                    EndDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["EndDate"], CommFun.datatypes.VarTypeDate));
                }

                sSql = "Update dbo.PaymentScheduleFlat Set SchDate='" + FinaliseDate.ToString("dd-MMM-yyyy") + "'" +
                    " Where TemplateId=" + iTempId + " And FlatId=" + argFlatId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "Update dbo.PaymentScheduleFlat Set SchDate='" + FinaliseDate.ToString("dd-MMM-yyyy") + "'" +
                    " Where TemplateId=0 And FlatId=" + argFlatId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                if (iStgId == -1)
                {
                    if (iStgId == -1)
                        sSql = "Select A.PreStageTypeId,A.CostCentreId,A.TemplateId,A.DateAfter,A.Duration,A.Durationtype from dbo.PaymentScheduleFlat A" +
                        " Left Join dbo.ProgressBillRegister B On A.FlatId=B.FlatId " +
                        " Where A.FlatId=" + argFlatId + " And A.SortOrder>=" + iSortOrder + "" +
                        " And A.PaymentSchId Not In " +
                        " (Select PaySchId From dbo.ProgressBillRegister Where FlatId=" + argFlatId + ") Order By A.SortOrder";

                    cmd = new SqlCommand(sSql, conn, tran);
                    sdr1 = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(sdr1);
                    cmd.Dispose();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        iTempId = Convert.ToInt32(dt.Rows[i]["TemplateId"]);
                        ipre = Convert.ToInt32(dt.Rows[i]["PreStageTypeId"]);
                        iDateAfter = Convert.ToInt32(dt.Rows[i]["DateAfter"]);
                        iDuration = Convert.ToInt32(dt.Rows[i]["Duration"]);
                        sDurType = dt.Rows[i]["DurationType"].ToString();

                        if (ipre == -1) { } else if (ipre == -2) { } else if (ipre == -3) { } else if (ipre == 0) { } else { iTempId = ipre; }

                        sSql = "Select SchDate From dbo.PaymentScheduleFlat Where CostCentreId=" + dt.Rows[i]["CostCentreId"] + " And FlatId=" + argFlatId + "" +
                                " And TemplateId=" + iTempId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        DataTable dtDate = new DataTable();
                        dr = cmd.ExecuteReader();
                        dtDate.Load(dr);
                        dtDate.Dispose();

                        if (ipre == -1) { SchDate = Convert.ToDateTime(CommFun.IsNullCheck(FinaliseDate, CommFun.datatypes.VarTypeDate)); }
                        else if (ipre == -2) { SchDate = StartDate; }
                        else if (ipre == -3) { SchDate = EndDate; }
                        else
                            SchDate = Convert.ToDateTime(CommFun.IsNullCheck(dtDate.Rows[0]["SchDate"], CommFun.datatypes.VarTypeDate));

                        if (sDurType == "D")
                        { if (iDateAfter == 0) SchDate = SchDate.AddDays(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }
                        else if (sDurType == "M")
                        { if (iDateAfter == 0) SchDate = SchDate.AddMonths(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }


                        sSql = "Update dbo.PaymentScheduleFlat Set SchDate=@SchDate" +
                            " Where TemplateId=" + dt.Rows[i]["TemplateId"] + " And FlatId=" + argFlatId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        SqlParameter dateParameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@SchDate" };
                        if (SchDate == DateTime.MinValue)
                            dateParameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            dateParameter.Value = SchDate;
                        cmd.Parameters.Add(dateParameter);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                    }
                }
            }
        }

        public static void UpdateReceiptBuyerSchedule(int argFlatId, DataTable argdt, SqlConnection conn, SqlTransaction tran)
        {
            string sSql = "";

            SqlDataReader sdr;
            SqlCommand cmd;
            DataTable dt = new DataTable();

            int iCCId = 0;
            int iFlatTypeId = 0;
            int iPayTypeId = 0;
            decimal dBaseAmt = 0;
            decimal dAdvAmt = 0;
            decimal dAdvBalAmt = 0;
            decimal dLandAmt = 0;
            decimal dNetAmt = 0;
            decimal dOtherAmt = 0;
            decimal dRAmt = 0;
            int iReceiptId = 0;
            int iROtherCostId = 0;
            string sRSchType = "";
            bool bAdvance = false;
            int iPaymentSchId = 0;
            DateTime dSchDate = DateTime.Now;
            string sSchType = "";
            int iOtherCostId = 0;
            decimal dRPer = 0;
            decimal dSchPercent = 0;
            decimal dQBaseAmt = 0;
            decimal dQNetAmt = 0;
            int iTemplateId = 0;
            int iSchId = 0;
            int iRSchId = 0;
            decimal dTNetAmt = 0;
            decimal dBalAmt = 0;
            bool bService = false, bLCBon = false;
            DataRow[] drT;
            cRateQualR RAQual;
            Collection QualVBC;
            DateTime FinaliseDate = DateTime.Now;


            DataTable dtReceipt = new DataTable();

            sSql = "Select * From dbo.PaymentScheduleFlat Where SchType='A' And FlatId=" + argFlatId + "";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtAdv = new DataTable();
            dtAdv.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            sSql = "Delete from dbo.PaymentScheduleFlat Where FlatId= " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete from dbo.FlatReceiptQualifier Where SchId in (Select SchId from dbo.FlatReceiptType Where FlatId= " + argFlatId + ")";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete from dbo.FlatReceiptType Where FlatId= " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Select FinaliseDate from dbo.BuyerDetail Where Status='S' And FlatId=" + argFlatId + "";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0) { FinaliseDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["FinaliseDate"], CommFun.datatypes.VarTypeDate)); }
            dt.Dispose();

            //sSql = "Select FlatTypeId,CostCentreId,PayTypeId,BaseAmt,AdvAmount,USLandAmt from dbo.FlatDetails Where FlatId= " + argFlatId;//modified
            sSql = "Select FlatTypeId,CostCentreId,PayTypeId,BaseAmt,AdvAmount,LandRate,Guidelinevalue,USLandAmt from dbo.FlatDetails Where FlatId= " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0)
            {
                iCCId = Convert.ToInt32(dt.Rows[0]["CostCentreId"].ToString());
                iFlatTypeId = Convert.ToInt32(dt.Rows[0]["FlatTypeId"].ToString());
                iPayTypeId = Convert.ToInt32(dt.Rows[0]["PayTypeId"].ToString());
                dBaseAmt = Convert.ToDecimal(dt.Rows[0]["BaseAmt"].ToString());
                dAdvAmt = Convert.ToDecimal(dt.Rows[0]["AdvAmount"].ToString());

                sSql = "Select LCBasedon From dbo.ProjectInfo Where CostCentreId= " + iCCId;
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                DataTable dtPI = new DataTable();
                dtPI.Load(sdr);
                sdr.Close();
                cmd.Dispose();
                if (dtPI.Rows.Count > 0) { bLCBon = Convert.ToBoolean(dtPI.Rows[0]["LCBasedon"]); }
                if (bLCBon == false) { dLandAmt = Convert.ToDecimal(dt.Rows[0]["LandRate"].ToString()); }
                else { dLandAmt = Convert.ToDecimal(dt.Rows[0]["USLandAmt"].ToString()); }

                //dLandAmt = Convert.ToDecimal(dt.Rows[0]["USLandAmt"].ToString());
                //dLandAmt = Convert.ToDecimal(dt.Rows[0]["LandRate"].ToString());
            }
            dt.Dispose();

            sSql = "Select TemplateId From dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " and CostCentreId = " + iCCId + " and SchType='A'";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0) { bAdvance = true; }
            dt.Dispose();

            sSql = "Select ISNULL(SUM(CASE WHEN Flag='-' THEN Amount*(-1) ELSE Amount END),0) Amount from dbo.FlatOtherCost " +
                   " Where FlatId = " + argFlatId + " AND OtherCostId IN(Select OtherCostId from dbo.OtherCostSetupTrans "+
                   " Where PayTypeId=" + iPayTypeId + " AND CostCentreId=" + iCCId + ")" ;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0) { dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }
            dt.Dispose();

            dNetAmt = dBaseAmt + dOtherAmt;
            if (bAdvance == true) { dNetAmt = dNetAmt - dAdvAmt; }

            if (argdt.Rows.Count > 0)
            {
                for (int i = 0; i < argdt.Rows.Count; i++)
                {
                    string sDate = string.Format(Convert.ToDateTime(CommFun.IsNullCheck(argdt.Rows[i]["SchDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MMM-yyyy"));
                    int iInsertPaymentSchId = Convert.ToInt32(CommFun.IsNullCheck(argdt.Rows[i]["PaymentSchId"], CommFun.datatypes.vartypenumeric));
                    if (iInsertPaymentSchId == 0)
                    {
                        if (argdt.Rows[i]["SchDate"].ToString() == "")
                        {
                            sDate = "NULL";
                            sSql = "Insert into dbo.PaymentScheduleFlat(FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId," +
                                    " OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,NetAmount,PreStageTypeId,SortOrder,BillPassed,PaidAmount,StageDetId)" +
                                    " Values(" + argdt.Rows[i]["FlatId"] + "," + argdt.Rows[i]["TemplateId"] + "," + argdt.Rows[i]["CostCentreId"] + "," +
                                    " '" + argdt.Rows[i]["SchType"] + "','" + argdt.Rows[i]["Description"] + "'," + argdt.Rows[i]["SchDescId"] + "," +
                                    " " + argdt.Rows[i]["StageId"] + "," + argdt.Rows[i]["OtherCostId"] + "," + sDate + "," +
                                    " '" + argdt.Rows[i]["DateAfter"] + "'," + argdt.Rows[i]["Duration"] + ",'" + argdt.Rows[i]["DurationType"] + "'," +
                                    " " + argdt.Rows[i]["SchPercent"] + "," + argdt.Rows[i]["Amount"] + "," + argdt.Rows[i]["NetAmount"] + "," + argdt.Rows[i]["PreStageTypeId"] + "," +
                                    " " + argdt.Rows[i]["SortOrder"] + ",'" + argdt.Rows[i]["BillPassed"] + "'," + argdt.Rows[i]["PaidAmount"] + "," + argdt.Rows[i]["StageDetId"] + ") SELECT SCOPE_IDENTITY();";
                        }
                        else
                        {
                            sSql = sSql + "Insert into dbo.PaymentScheduleFlat(FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId," +
                                        " OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,NetAmount,PreStageTypeId,SortOrder,BillPassed,PaidAmount,StageDetId) " +
                                        " Values(" + argdt.Rows[i]["FlatId"] + "," + argdt.Rows[i]["TemplateId"] + "," + argdt.Rows[i]["CostCentreId"] + "," +
                                        " '" + argdt.Rows[i]["SchType"] + "','" + argdt.Rows[i]["Description"] + "'," + argdt.Rows[i]["SchDescId"] + "," +
                                        " " + argdt.Rows[i]["StageId"] + "," + argdt.Rows[i]["OtherCostId"] + ",'" + sDate + "'," +
                                        " '" + argdt.Rows[i]["DateAfter"] + "'," + argdt.Rows[i]["Duration"] + ",'" + argdt.Rows[i]["DurationType"] + "'," +
                                        " " + argdt.Rows[i]["SchPercent"] + "," + argdt.Rows[i]["Amount"] + "," + argdt.Rows[i]["NetAmount"] + "," + argdt.Rows[i]["PreStageTypeId"] + "," +
                                        " " + argdt.Rows[i]["SortOrder"] + ",'" + argdt.Rows[i]["BillPassed"] + "'," + argdt.Rows[i]["PaidAmount"] + "," + argdt.Rows[i]["StageDetId"] + ") SELECT SCOPE_IDENTITY();";
                        }
                    }
                    else
                    {
                        if (argdt.Rows[i]["SchDate"].ToString() == "")
                        {
                            sDate = "NULL";
                            sSql = "Set Identity_Insert PaymentScheduleFlat On ";
                            sSql = sSql + "Insert into dbo.PaymentScheduleFlat(PaymentSchId,FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId," +
                                            " OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,NetAmount,PreStageTypeId,SortOrder,BillPassed,PaidAmount,StageDetId) " +
                                            " Values(" + argdt.Rows[i]["PaymentSchId"] + "," + argdt.Rows[i]["FlatId"] + "," + argdt.Rows[i]["TemplateId"] + "," + argdt.Rows[i]["CostCentreId"] + "," +
                                            " '" + argdt.Rows[i]["SchType"] + "','" + argdt.Rows[i]["Description"] + "'," + argdt.Rows[i]["SchDescId"] + "," +
                                            " " + argdt.Rows[i]["StageId"] + "," + argdt.Rows[i]["OtherCostId"] + "," + sDate + "," +
                                            " '" + argdt.Rows[i]["DateAfter"] + "'," + argdt.Rows[i]["Duration"] + ",'" + argdt.Rows[i]["DurationType"] + "'," +
                                            " " + argdt.Rows[i]["SchPercent"] + "," + argdt.Rows[i]["Amount"] + "," + argdt.Rows[i]["NetAmount"] + "," + argdt.Rows[i]["PreStageTypeId"] + "," +
                                            " " + argdt.Rows[i]["SortOrder"] + ",'" + argdt.Rows[i]["BillPassed"] + "'," + argdt.Rows[i]["PaidAmount"] + "," + argdt.Rows[i]["StageDetId"] + ")";
                            sSql = sSql + "Set Identity_Insert PaymentscheduleFlat Off ";
                        }
                        else
                        {

                            sSql = "Set Identity_Insert PaymentScheduleFlat On ";
                            sSql = sSql + "Insert into dbo.PaymentScheduleFlat(PaymentSchId,FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId," +
                                            " OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,NetAmount,PreStageTypeId,SortOrder,BillPassed,PaidAmount,StageDetId) " +
                                            " Values(" + argdt.Rows[i]["PaymentSchId"] + "," + argdt.Rows[i]["FlatId"] + "," + argdt.Rows[i]["TemplateId"] + "," + argdt.Rows[i]["CostCentreId"] + "," +
                                            " '" + argdt.Rows[i]["SchType"] + "','" + argdt.Rows[i]["Description"] + "'," + argdt.Rows[i]["SchDescId"] + "," +
                                            " " + argdt.Rows[i]["StageId"] + "," + argdt.Rows[i]["OtherCostId"] + ",'" + sDate + "'," +
                                            " '" + argdt.Rows[i]["DateAfter"] + "'," + argdt.Rows[i]["Duration"] + ",'" + argdt.Rows[i]["DurationType"] + "'," +
                                            " " + argdt.Rows[i]["SchPercent"] + "," + argdt.Rows[i]["Amount"] + "," + argdt.Rows[i]["NetAmount"] + "," + argdt.Rows[i]["PreStageTypeId"] + "," +
                                            " " + argdt.Rows[i]["SortOrder"] + ",'" + argdt.Rows[i]["BillPassed"] + "'," + argdt.Rows[i]["PaidAmount"] + "," + argdt.Rows[i]["StageDetId"] + ")";
                            sSql = sSql + "Set Identity_Insert PaymentscheduleFlat Off ";
                        }
                    }
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }

            sSql = "Select ReceiptTypeId,OtherCostId,SchType from dbo.ReceiptTypeOrder " +
                    "Where PayTypeId = " + iPayTypeId + " and CostCentreId=" + iCCId + " and SchType <>'A' Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtReceiptOrder = new DataTable();
            dtReceiptOrder.Load(sdr);
            sdr.Close();
            cmd.Dispose();


            sSql = "Select OtherCostId,Flag,Amount from dbo.FlatOtherCost Where FlatId = " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtT = new DataTable();
            dtT.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            dtReceipt.Columns.Add("Id", typeof(int));
            dtReceipt.Columns.Add("SchType", typeof(string));
            dtReceipt.Columns.Add("Amount", typeof(decimal));
            dtReceipt.Columns.Add("RAmount", typeof(decimal));

            DataRow drR;
            drR = dtReceipt.NewRow();
            drR["Id"] = 1;
            drR["SchType"] = "A";
            drR["Amount"] = dAdvAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            drR = dtReceipt.NewRow();
            drR["Id"] = 2;
            drR["SchType"] = "R";
            drR["Amount"] = dLandAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            drR = dtReceipt.NewRow();
            drR["Id"] = 3;
            drR["SchType"] = "R";
            drR["Amount"] = dBaseAmt - dLandAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            for (int i = 0; i < dtT.Rows.Count; i++)
            {
                drR = dtReceipt.NewRow();
                drR["Id"] = Convert.ToInt32(dtT.Rows[i]["OtherCostId"].ToString());
                drR["SchType"] = "O";
                drR["Amount"] = Convert.ToDecimal(dtT.Rows[i]["Amount"].ToString());
                drR["RAmount"] = 0;
                dtReceipt.Rows.Add(drR);
            }

            sSql = "Select SchId,TemplateId,ReceiptTypeId,Percentage,OtherCostId,SchType from dbo.CCReceiptType " +
                    "Where TemplateId in (Select TemplateId from dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " and CostCentreId=" + iCCId + ") Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtTemp = new DataTable();
            dtTemp.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            //sSql = "Select A.*,IsNull(B.Service,0)Service From dbo.CCReceiptQualifier A " +
            //         " Left Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId Where CostCentreId=" + iCCId;
            sSql = "Select C.QualTypeId,A.*,IsNull(B.Service,0)Service From dbo.CCReceiptQualifier A " +
                    " Left Join dbo.OtherCostMaster B On A.OtherCostId=B.OtherCostId " +
                    " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On C.QualifierId=A.QualifierId " +
                    " Where CostCentreId=" + iCCId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtQual = new DataTable();
            dtQual.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            sSql = "Select PaymentSchId,TemplateId,SchDate,SchType,OtherCostId,SchPercent from dbo.PaymentScheduleFlat Where FlatId = " + argFlatId + " Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            decimal dAmt = 0;
            DataView dv;
            decimal dAdvRAmt = 0;

            DataTable dtTempT;
            DataTable dtQualT;
            dAdvBalAmt = dAdvAmt;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iPaymentSchId = Convert.ToInt32(dt.Rows[i]["PaymentSchId"].ToString());
                iTemplateId = Convert.ToInt32(dt.Rows[i]["TemplateId"].ToString());
                sSchType = dt.Rows[i]["SchType"].ToString();
                dSchDate = FinaliseDate; // Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[i]["SchDate"], CommFun.datatypes.VarTypeDate));
                if (dSchDate == DateTime.MinValue) { dSchDate = DateTime.Now; }
                iOtherCostId = Convert.ToInt32(dt.Rows[i]["OtherCostId"].ToString());
                dSchPercent = Convert.ToDecimal(dt.Rows[i]["SchPercent"].ToString());
                dTNetAmt = 0;

                dAmt = 0;
                if (sSchType == "A")
                {
                    dAmt = dAdvAmt;
                }
                else if (sSchType == "O")
                {
                    dv = new DataView(dtT);
                    dv.RowFilter = "OtherCostId = " + iOtherCostId;
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        dAmt = Convert.ToDecimal(dv.ToTable().Rows[0]["Amount"].ToString());
                        if (dv.ToTable().Rows[0]["Flag"].ToString() == "-") { dAmt = dAmt * (-1); }
                    }
                    dv.Dispose();
                }
                else
                {
                    dAmt = dNetAmt * dSchPercent / 100;
                }

                dtTempT = new DataTable();
                dv = new DataView(dtTemp);
                dv.RowFilter = "TemplateId = " + iTemplateId;
                dtTempT = dv.ToTable();
                dv.Dispose();

                if (dtTempT.Rows.Count == 1 && sSchType == "O")
                {

                    sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                            "Values(" + iPaymentSchId + "," + argFlatId + ",0," + iOtherCostId + ",'" + sSchType + "',100," + dAmt + "," + dAmt + ") SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(sSql, conn, tran);
                    iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();

                    drT = dtReceipt.Select("SchType = 'O' and Id = " + iOtherCostId + "");

                    if (drT.Length > 0)
                    {
                        drT[0]["RAmount"] = dAmt;
                    }

                    dQNetAmt = dAmt;

                    dtQualT = new DataTable();
                    dv = new DataView(dtQual);
                    dv.RowFilter = "SchType = '" + sSchType + "' and OtherCostId = " + iOtherCostId;
                    dtQualT = dv.ToTable();
                    dv.Dispose();

                    if (dtQualT.Rows.Count > 0)
                    {
                        QualVBC = new Collection();

                        for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                        {
                            RAQual = new cRateQualR();
                            bService = Convert.ToBoolean(dtQualT.Rows[Q]["Service"]);

                            DataTable dtTDS = new DataTable();
                            if (Convert.ToInt32(dtQualT.Rows[Q]["QualTypeId"]) == 2)
                            {
                                if (bService == true)
                                    dtTDS = GetSTSettings("G", dSchDate, conn, tran);
                                else
                                    dtTDS = GetSTSettings("F", dSchDate, conn, tran);
                            }
                            else
                            {
                                dtTDS = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), dSchDate, "B", conn, tran);
                            }

                            RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                            if (dtTDS.Rows.Count > 0)
                            {
                                RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                                RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                                RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                                RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                                RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                                RAQual.HEDValue = (dAmt * Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric))) / 100;
                                RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Taxable"], CommFun.datatypes.vartypenumeric));
                            }

                            DataTable dtQ = new DataTable();
                            dtQ = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), dSchDate, "B", conn, tran);
                            //dtQ = QualifierSelect(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), "B", dSchDate, conn, tran);
                            if (dtQ.Rows.Count > 0)
                            {
                                RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                                RAQual.Amount = 0;
                                RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                            }

                            QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                        }

                        Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                        dQBaseAmt = dAmt;
                        dQNetAmt = dAmt; decimal dTaxAmt = 0;
                        decimal dVATAmt = 0;

                        if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, dSchDate, ref dVATAmt) == true)
                        {
                            foreach (Qualifier.cRateQualR d in QualVBC)
                            {
                                sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount,NetPer,HEDPer,HEDValue,TaxablePer,TaxableValue) " +
                                        "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                        "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + "," + d.NetPer + "," + d.HEDPer + "," + d.HEDValue + "," + d.TaxablePer + "," + d.TaxableValue + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }

                        sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dQNetAmt + "  Where PaymentSchId = " + iPaymentSchId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    dTNetAmt = dTNetAmt + dQNetAmt;
                }

                else
                {
                    dBalAmt = dAmt;
                    for (int j = 0; j < dtTempT.Rows.Count; j++)
                    {
                        iSchId = Convert.ToInt32(dtTempT.Rows[j]["SchId"].ToString());
                        dRPer = Convert.ToDecimal(dtTempT.Rows[j]["Percentage"].ToString());
                        sRSchType = dtTempT.Rows[j]["SchType"].ToString();
                        iReceiptId = Convert.ToInt32(dtTempT.Rows[j]["ReceiptTypeId"].ToString());
                        iROtherCostId = Convert.ToInt32(dtTempT.Rows[j]["OtherCostId"].ToString());

                        if (dRPer != 0) { dRAmt = dAmt * dRPer / 100; }
                        else { dRAmt = dBalAmt; }

                        if (dRAmt > dBalAmt) { dRAmt = dBalAmt; }


                        if (sRSchType == "A" && bAdvance == false)
                        {

                            dAdvRAmt = dAdvAmt * dRPer / 100;
                            if (dAdvRAmt > dAdvBalAmt) { dAdvRAmt = dAdvBalAmt; }
                            dAdvBalAmt = dAdvBalAmt - dAdvRAmt;
                            dTNetAmt = dTNetAmt - dAdvRAmt;

                            sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                    "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + ", 0," + dAdvRAmt + ") SELECT SCOPE_IDENTITY();";
                            cmd = new SqlCommand(sSql, conn, tran);
                            iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();
                        }

                        else
                        {
                            if (sRSchType == "A")
                            {
                                drT = dtReceipt.Select("SchType = 'A'");
                            }
                            else if (sRSchType == "O")
                            {
                                drT = dtReceipt.Select("SchType = 'O' and Id = " + iROtherCostId + "");
                            }
                            else
                            {
                                drT = dtReceipt.Select("SchType = 'R' and Id = " + iReceiptId + "");
                            }


                            decimal dRTAmt = 0;
                            decimal dRRAmt = 0;

                            if (drT.Length > 0)
                            {
                                dRTAmt = Convert.ToDecimal(drT[0]["Amount"].ToString());
                                dRRAmt = Convert.ToDecimal(drT[0]["RAmount"].ToString());
                            }

                            if (dRAmt > dRTAmt - dRRAmt)
                            {
                                dRAmt = dRTAmt - dRRAmt;
                            }

                            if (drT.Length > 0)
                            {
                                drT[0]["RAmount"] = dRRAmt + dRAmt;
                            }

                            if (dAmt == 0) { dRPer = 0; }
                            else dRPer = (dRAmt / dAmt) * 100;

                            dBalAmt = dBalAmt - dRAmt;

                            sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                    "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + "," + dRAmt + "," + dRAmt + ") SELECT SCOPE_IDENTITY();";
                            cmd = new SqlCommand(sSql, conn, tran);
                            iRSchId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();

                            dQNetAmt = dRAmt;

                            dtQualT = new DataTable();
                            dv = new DataView(dtQual);
                            dv.RowFilter = "SchType = '" + sRSchType + "' and ReceiptTypeId = " + iReceiptId + " and OtherCostId = " + iROtherCostId;
                            dtQualT = dv.ToTable();
                            dv.Dispose();
                            if (dtQualT.Rows.Count > 0)
                            {
                                QualVBC = new Collection();

                                for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                                {
                                    RAQual = new cRateQualR();
                                    bService = Convert.ToBoolean(dtQualT.Rows[Q]["Service"]);

                                    DataTable dtTDS = new DataTable();
                                    if (Convert.ToInt32(dtQualT.Rows[Q]["QualTypeId"]) == 2)
                                    {
                                        if (bService == true)
                                            dtTDS = GetSTSettings("G", dSchDate, conn, tran);
                                        else
                                            dtTDS = GetSTSettings("F", dSchDate, conn, tran);
                                    }
                                    else
                                    {
                                        dtTDS = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), dSchDate, "B", conn, tran);
                                    }

                                    RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                                    if (dtTDS.Rows.Count > 0)
                                    {
                                        RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                                        RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                                        RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                                        RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                                        RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                                        RAQual.HEDValue = (dRAmt * Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric))) / 100;
                                        RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Taxable"], CommFun.datatypes.vartypenumeric));
                                    }

                                    DataTable dtQ = new DataTable();
                                    dtQ = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), dSchDate, "B", conn, tran);
                                    //dtQ = QualifierSelect(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), "B", dSchDate, conn, tran);
                                    if (dtQ.Rows.Count > 0)
                                    {
                                        RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                                        RAQual.Amount = 0;
                                        RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                                    }

                                    QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                                }

                                Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                                dQBaseAmt = dRAmt;
                                dQNetAmt = dRAmt; decimal dTaxAmt = 0;
                                decimal dVATAmt = 0;

                                if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, dSchDate, ref dVATAmt) == true)
                                {
                                    foreach (Qualifier.cRateQualR d in QualVBC)
                                    {
                                        sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount,NetPer,HEDPer,HEDValue,TaxablePer,TaxableValue) " +
                                                "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                                "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + "," + d.NetPer + "," + d.HEDPer + "," + d.HEDValue + "," + d.TaxablePer + "," + d.TaxableValue + ")";
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                }

                                sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }


                            dTNetAmt = dTNetAmt + dQNetAmt;

                        }

                        //if (dBalAmt <= 0) { break; }
                    }

                    if (dBalAmt > 0)
                    {
                        for (int j = 0; j < dtReceiptOrder.Rows.Count; j++)
                        {
                            dRAmt = dBalAmt;

                            sRSchType = dtReceiptOrder.Rows[j]["SchType"].ToString();
                            iReceiptId = Convert.ToInt32(dtReceiptOrder.Rows[j]["ReceiptTypeId"].ToString());
                            iROtherCostId = Convert.ToInt32(dtReceiptOrder.Rows[j]["OtherCostId"].ToString());

                            if (sRSchType == "O")
                            {
                                drT = dtReceipt.Select("SchType = 'O' and Id = " + iROtherCostId + "");
                            }
                            else
                            {
                                drT = dtReceipt.Select("SchType = 'R' and Id = " + iReceiptId + "");
                            }

                            decimal dRTAmt = 0;
                            decimal dRRAmt = 0;

                            if (drT.Length > 0)
                            {
                                dRTAmt = Convert.ToDecimal(drT[0]["Amount"].ToString());
                                dRRAmt = Convert.ToDecimal(drT[0]["RAmount"].ToString());
                            }

                            if (dRAmt > dRTAmt - dRRAmt)
                            {
                                dRAmt = dRTAmt - dRRAmt;
                            }

                            if (drT.Length > 0)
                            {
                                drT[0]["RAmount"] = dRRAmt + dRAmt;
                            }

                            if (dRAmt > 0)
                            {
                                decimal dPCAmt = 0;
                                bool bAns = false;
                                sSql = "Select SchId,Amount,NetAmount from dbo.FlatReceiptType Where PaymentSchId = " + iPaymentSchId + " and " +
                                        "FlatId= " + argFlatId + " and ReceiptTypeId= " + iReceiptId + " and OtherCostId = " + iROtherCostId + " and SchType= '" + sRSchType + "'";
                                cmd = new SqlCommand(sSql, conn, tran);
                                sdr = cmd.ExecuteReader();
                                DataTable dtP = new DataTable();
                                dtP.Load(sdr);
                                sdr.Close();
                                cmd.Dispose();

                                if (dtP.Rows.Count > 0)
                                {
                                    dPCAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtP.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
                                    dTNetAmt = dTNetAmt - dPCAmt;
                                    dBalAmt = dBalAmt + dPCAmt;
                                    iRSchId = Convert.ToInt32(dtP.Rows[0]["SchId"].ToString());
                                    bAns = true;
                                }
                                dtP.Dispose();

                                if (bAns == true)
                                {
                                    dRAmt = dRAmt + dPCAmt;
                                    dRPer = (dRAmt / dAmt) * 100;

                                    sSql = "Update dbo.FlatReceiptType Set Amount= " + dRAmt + ",Percentage = " + dRPer + ",NetAmount = " + dRAmt + " Where SchId = " + iRSchId;
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();

                                    sSql = "Delete from dbo.FlatReceiptQualifier Where SchId = " + iRSchId;
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                                else
                                {
                                    dRPer = (dRAmt / dAmt) * 100;

                                    sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                            "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + "," + dRAmt + "," + dRAmt + ") SELECT SCOPE_IDENTITY();";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                                    cmd.Dispose();
                                }

                                dQNetAmt = dRAmt;

                                dtQualT = new DataTable();
                                dv = new DataView(dtQual);

                                if (sRSchType == "O")
                                {
                                    dv.RowFilter = "SchType = 'O' and ReceiptTypeId = 0 and OtherCostId = " + iROtherCostId + "";

                                }
                                else
                                {
                                    dv.RowFilter = "SchType = 'R' and ReceiptTypeId = " + iReceiptId + " and OtherCostId = 0";
                                }

                                dtQualT = dv.ToTable();
                                dv.Dispose();
                                if (dtQualT.Rows.Count > 0)
                                {
                                    QualVBC = new Collection();

                                    for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                                    {
                                        RAQual = new cRateQualR();
                                        bService = Convert.ToBoolean(dtQualT.Rows[Q]["Service"]);

                                        DataTable dtTDS = new DataTable();
                                        if (Convert.ToInt32(dtQualT.Rows[Q]["QualTypeId"]) == 2)
                                        {
                                            if (bService == true)
                                                dtTDS = GetSTSettings("G", dSchDate, conn, tran);
                                            else
                                                dtTDS = GetSTSettings("F", dSchDate, conn, tran);
                                        }
                                        else
                                        {
                                            dtTDS = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), dSchDate, "B", conn, tran);
                                        }

                                        RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                                        if (dtTDS.Rows.Count > 0)
                                        {
                                            RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                                            RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                                            RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                                            RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                                            RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                                            RAQual.HEDValue = (dRAmt * Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric))) / 100;
                                            RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Taxable"], CommFun.datatypes.vartypenumeric));
                                        }

                                        DataTable dtQ = new DataTable();
                                        dtQ = PaymentScheduleDL.GetQual(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), dSchDate, "B", conn, tran);
                                        //dtQ = QualifierSelect(Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]), "B", dSchDate, conn, tran);
                                        if (dtQ.Rows.Count > 0)
                                        {
                                            RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                                            RAQual.Amount = 0;
                                            RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                                        }

                                        QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                                    }

                                    Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                                    dQBaseAmt = dRAmt;
                                    dQNetAmt = dRAmt; decimal dTaxAmt = 0;
                                    decimal dVATAmt = 0;

                                    if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                                    {
                                        foreach (Qualifier.cRateQualR d in QualVBC)
                                        {
                                            sSql = "Insert Into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount,NetPer,HEDPer,HEDValue,TaxablePer,TaxableValue) " +
                                                    "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                                    "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + "," + d.NetPer + "," + d.HEDPer + "," + d.HEDValue + "," + d.TaxablePer + "," + d.TaxableValue + ")";
                                            cmd = new SqlCommand(sSql, conn, tran);
                                            cmd.ExecuteNonQuery();
                                            cmd.Dispose();
                                        }
                                    }
                                    sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }

                                dTNetAmt = dTNetAmt + dQNetAmt;
                                dBalAmt = dBalAmt - dRAmt;
                                if (dBalAmt <= 0) { break; }
                            }
                        }

                    }

                    //modified
                    sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dTNetAmt + "  Where PaymentSchId = " + iPaymentSchId;
                    //sSql = "Update PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dQNetAmt + "  Where PaymentSchId = " + iPaymentSchId;

                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            dt.Dispose();


            if (bAdvance == false)
            {
                if (dtAdv.Rows.Count > 0)
                {
                    string sDate = string.Format(Convert.ToDateTime(CommFun.IsNullCheck(dtAdv.Rows[0]["SchDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MMM-yyyy"));

                    sSql = "Set Identity_Insert PaymentScheduleFlat On ";
                    if (sDate == "NULL")
                        sSql = sSql + "Insert into dbo.PaymentScheduleFlat(PaymentSchId,FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,NetAmount,PreStageTypeId,SortOrder,BillPassed,PaidAmount,StageDetId) " +
                                "Values(" + dtAdv.Rows[0]["PaymentSchId"] + "," + argFlatId + ",0," + iCCId + ",'A','Advance',0,0,0,NULL," +
                                 " '" + dtAdv.Rows[0]["DateAfter"] + "'," + dtAdv.Rows[0]["Duration"] + ",'" + dtAdv.Rows[0]["DurationType"] + "'," +
                                " " + dtAdv.Rows[0]["SchPercent"] + "," + dAdvAmt + "," + dAdvAmt + "," + dtAdv.Rows[0]["PreStageTypeId"] + "," +
                                " " + dtAdv.Rows[0]["SortOrder"] + ",'" + dtAdv.Rows[0]["BillPassed"] + "'," + dtAdv.Rows[0]["PaidAmount"] + "," + dtAdv.Rows[0]["StageDetId"] + ")";

                    else
                        sSql = sSql + "Insert into dbo.PaymentScheduleFlat(PaymentSchId,FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,NetAmount,PreStageTypeId,SortOrder,BillPassed,PaidAmount,StageDetId) " +
                                "Values(" + dtAdv.Rows[0]["PaymentSchId"] + "," + argFlatId + ",0," + iCCId + ",'A','Advance',0,0,0,'" + sDate + "'," +
                                 " '" + dtAdv.Rows[0]["DateAfter"] + "'," + dtAdv.Rows[0]["Duration"] + ",'" + dtAdv.Rows[0]["DurationType"] + "'," +
                                " " + dtAdv.Rows[0]["SchPercent"] + "," + dAdvAmt + "," + dAdvAmt + "," + dtAdv.Rows[0]["PreStageTypeId"] + "," +
                                " " + dtAdv.Rows[0]["SortOrder"] + ",'" + dtAdv.Rows[0]["BillPassed"] + "'," + dtAdv.Rows[0]["PaidAmount"] + "," + dtAdv.Rows[0]["StageDetId"] + ")";
                    sSql = sSql + "Set Identity_Insert PaymentscheduleFlat Off ";
                    //sSql = "Insert Into dbo.PaymentScheduleFlat(FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,Amount,NetAmount,PreStageTypeId,SortOrder) " +
                    //      "Values(" + argFlatId + ",0," + iCCId + ",'A','Advance',0,0,0,0," + dAdvAmt + ",0,0)";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }

            sSql = "Update dbo.PaymentScheduleFlat Set Advance=0";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "UPDATE PaymentScheduleFlat SET Advance=SummedQty FROM " +
                    " PaymentScheduleFlat A JOIN (SELECT PaymentSchId, SUM(NetAmount) SummedQty " +
                    " FROM FlatReceiptType WHERE SchType='A' GROUP BY PaymentSchId ) CCA ON A.PaymentSchId=CCA.PaymentSchId";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            //Schedule Date
            
            sSql = "Select FinaliseDate from dbo.BuyerDetail Where Status='S' And FlatId=" + argFlatId + "";
            cmd = new SqlCommand(sSql, conn, tran);
            SqlDataReader dr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0)
            {
                FinaliseDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["FinaliseDate"], CommFun.datatypes.VarTypeDate));

                sSql = "Update dbo.PaymentScheduleFlat Set SchDate=@FinaliseDate" +
                        " Where TemplateId=0 And FlatId=" + argFlatId + "";

                cmd = new SqlCommand(sSql, conn, tran);
                SqlParameter dateParameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@FinaliseDate" };
                if (FinaliseDate == DateTime.MinValue)
                    dateParameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                else
                    dateParameter.Value = FinaliseDate;
                cmd.Parameters.Add(dateParameter);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            else
            {
                sSql = "Update dbo.PaymentScheduleFlat Set SchDate=NULL" +
                        " Where TemplateId=0 And FlatId=" + argFlatId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }            
        }

        public static void UpdateReceiptBuyerScheduleQual(int argFlatId, DataTable argdt, SqlConnection conn, SqlTransaction tran)
        {
            string sSql = "";

            SqlDataReader sdr;
            SqlCommand cmd;
            DataTable dt = new DataTable();

            int iCCId = 0;
            int iFlatTypeId = 0;
            int iPayTypeId = 0;
            decimal dBaseAmt = 0;
            decimal dAdvAmt = 0;
            decimal dAdvBalAmt = 0;
            decimal dLandAmt = 0;
            decimal dNetAmt = 0;
            decimal dOtherAmt = 0;
            decimal dRAmt = 0;
            int iReceiptId = 0;
            int iROtherCostId = 0;
            string sRSchType = "";
            bool bAdvance = false;
            int iPaymentSchId = 0;
            string sSchType = "";
            int iOtherCostId = 0;
            decimal dRPer = 0;
            decimal dSchPercent = 0;
            decimal dQBaseAmt = 0;
            decimal dQNetAmt = 0;
            int iTemplateId = 0;
            int iSchId = 0;
            int iRSchId = 0;
            decimal dTNetAmt = 0;
            decimal dBalAmt = 0;
            bool bPayTypewise = false, bLCBon = false;
            decimal dTotalTax = 0;
            decimal dAdv = 0;
            DataRow[] drT;
            cRateQualR RAQual;
            Collection QualVBC;

            DataTable dtReceipt = new DataTable();

            sSql = "Select * From dbo.PaymentScheduleFlat Where SchType='A' And FlatId=" + argFlatId + "";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtAdv = new DataTable();
            dtAdv.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            sSql = "Delete from dbo.PaymentScheduleFlat Where FlatId= " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete from dbo.FlatReceiptQualifier Where SchId in (Select SchId from dbo.FlatReceiptType Where FlatId= " + argFlatId + ")";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete From dbo.PaySchTaxFlat Where FlatId=" + argFlatId + " ";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete from dbo.FlatReceiptType Where FlatId= " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();


            //sSql = "Select FlatTypeId,CostCentreId,PayTypeId,BaseAmt,AdvAmount,USLandAmt from dbo.FlatDetails Where FlatId= " + argFlatId;//modified
            sSql = "Select FlatTypeId,CostCentreId,PayTypeId,BaseAmt,AdvAmount,LandRate,Guidelinevalue,USLandAmt From dbo.FlatDetails Where FlatId= " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0)
            {
                iCCId = Convert.ToInt32(dt.Rows[0]["CostCentreId"].ToString());
                iFlatTypeId = Convert.ToInt32(dt.Rows[0]["FlatTypeId"].ToString());
                iPayTypeId = Convert.ToInt32(dt.Rows[0]["PayTypeId"].ToString());
                bPayTypewise = FlatDetailsDL.GetTypewise(iPayTypeId);
                dBaseAmt = Convert.ToDecimal(dt.Rows[0]["BaseAmt"].ToString());
                dAdvAmt = Convert.ToDecimal(dt.Rows[0]["AdvAmount"].ToString());

                sSql = "Select LCBasedon From dbo.ProjectInfo Where CostCentreId= " + iCCId;
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                DataTable dtPI = new DataTable();
                dtPI.Load(sdr);
                sdr.Close();
                cmd.Dispose();
                if (dtPI.Rows.Count > 0) { bLCBon = Convert.ToBoolean(dtPI.Rows[0]["LCBasedon"]); }
                if (bLCBon == false) { dLandAmt = Convert.ToDecimal(dt.Rows[0]["LandRate"].ToString()); }
                else { dLandAmt = Convert.ToDecimal(dt.Rows[0]["USLandAmt"].ToString()); }
            }
            dt.Dispose();

            sSql = "Select TemplateId From dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " and CostCentreId = " + iCCId + " and SchType='A'";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0) { bAdvance = true; }
            dt.Dispose();

            sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                    "Where FlatId = " + argFlatId + " and OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans Where PayTypeId=" + iPayTypeId + " and CostCentreId=" + iCCId + ")";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0) { dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }
            dt.Dispose();

            sSql = "Select QualifierId,Amount from dbo.FlatTax Where FlatId = " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtTx = new DataTable();
            dtTx.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            decimal dT = 0;
            if (dtTx.Rows.Count > 0)
            {
                for (int i = 0; i < dtTx.Rows.Count; i++)
                {
                    dTotalTax = Convert.ToDecimal(dtTx.Rows[i]["Amount"]);
                    dT = dT + dTotalTax;
                }
            }

            if (bPayTypewise == false)
            { dNetAmt = dBaseAmt + dOtherAmt + dT; }
            else
            { dNetAmt = dBaseAmt + dOtherAmt; }
            if (bAdvance == true) { dNetAmt = dNetAmt - dAdvAmt; }

            if (argdt.Rows.Count > 0)
            {
                for (int i = 0; i < argdt.Rows.Count; i++)
                {
                    string sDate = string.Format(Convert.ToDateTime(CommFun.IsNullCheck(argdt.Rows[i]["SchDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MMM-yyyy"));
                    if (argdt.Rows[i]["SchDate"].ToString() == "")
                    {
                        sDate = "NULL";
                        sSql = "Set Identity_Insert PaymentScheduleFlat On ";
                        sSql = sSql + "Insert into dbo.PaymentScheduleFlat(PaymentSchId,FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId," +
                        " OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,NetAmount,PreStageTypeId,SortOrder,BillPassed,PaidAmount,StageDetId) " +
                        " Values(" + argdt.Rows[i]["PaymentSchId"] + "," + argdt.Rows[i]["FlatId"] + "," + argdt.Rows[i]["TemplateId"] + "," + argdt.Rows[i]["CostCentreId"] + "," +
                        " '" + argdt.Rows[i]["SchType"] + "','" + argdt.Rows[i]["Description"] + "'," + argdt.Rows[i]["SchDescId"] + "," +
                        " " + argdt.Rows[i]["StageId"] + "," + argdt.Rows[i]["OtherCostId"] + "," + sDate + "," +
                        " '" + argdt.Rows[i]["DateAfter"] + "'," + argdt.Rows[i]["Duration"] + ",'" + argdt.Rows[i]["DurationType"] + "'," +
                        " " + argdt.Rows[i]["SchPercent"] + "," + argdt.Rows[i]["Amount"] + "," + argdt.Rows[i]["NetAmount"] + "," + argdt.Rows[i]["PreStageTypeId"] + "," +
                        " " + argdt.Rows[i]["SortOrder"] + ",'" + argdt.Rows[i]["BillPassed"] + "'," + argdt.Rows[i]["PaidAmount"] + "," + argdt.Rows[i]["StageDetId"] + ")";
                        sSql = sSql + "Set Identity_Insert PaymentscheduleFlat Off ";
                    }
                    else
                    {
                        sSql = "Set Identity_Insert PaymentScheduleFlat On ";
                        sSql = sSql + "Insert into dbo.PaymentScheduleFlat(PaymentSchId,FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId," +
                        " OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,NetAmount,PreStageTypeId,SortOrder,BillPassed,PaidAmount,StageDetId) " +
                        " Values(" + argdt.Rows[i]["PaymentSchId"] + "," + argdt.Rows[i]["FlatId"] + "," + argdt.Rows[i]["TemplateId"] + "," + argdt.Rows[i]["CostCentreId"] + "," +
                        " '" + argdt.Rows[i]["SchType"] + "','" + argdt.Rows[i]["Description"] + "'," + argdt.Rows[i]["SchDescId"] + "," +
                        " " + argdt.Rows[i]["StageId"] + "," + argdt.Rows[i]["OtherCostId"] + ",'" + sDate + "'," +
                        " '" + argdt.Rows[i]["DateAfter"] + "'," + argdt.Rows[i]["Duration"] + ",'" + argdt.Rows[i]["DurationType"] + "'," +
                        " " + argdt.Rows[i]["SchPercent"] + "," + argdt.Rows[i]["Amount"] + "," + argdt.Rows[i]["NetAmount"] + "," + argdt.Rows[i]["PreStageTypeId"] + "," +
                        " " + argdt.Rows[i]["SortOrder"] + ",'" + argdt.Rows[i]["BillPassed"] + "'," + argdt.Rows[i]["PaidAmount"] + "," + argdt.Rows[i]["StageDetId"] + ")";
                        sSql = sSql + "Set Identity_Insert PaymentscheduleFlat Off ";
                    }
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }

            sSql = "Select ReceiptTypeId,OtherCostId,SchType from dbo.ReceiptTypeOrder " +
                    "Where PayTypeId = " + iPayTypeId + " and CostCentreId=" + iCCId + " and SchType <>'A' Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtReceiptOrder = new DataTable();
            dtReceiptOrder.Load(sdr);
            sdr.Close();
            cmd.Dispose();


            sSql = "Select OtherCostId,Flag,Amount from dbo.FlatOtherCost Where FlatId = " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtT = new DataTable();
            dtT.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            dtReceipt.Columns.Add("Id", typeof(int));
            dtReceipt.Columns.Add("SchType", typeof(string));
            dtReceipt.Columns.Add("Amount", typeof(decimal));
            dtReceipt.Columns.Add("RAmount", typeof(decimal));

            DataRow drR;
            drR = dtReceipt.NewRow();
            drR["Id"] = 1;
            drR["SchType"] = "A";
            drR["Amount"] = dAdvAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            drR = dtReceipt.NewRow();
            drR["Id"] = 2;
            drR["SchType"] = "R";
            drR["Amount"] = dLandAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            drR = dtReceipt.NewRow();
            drR["Id"] = 3;
            drR["SchType"] = "R";
            drR["Amount"] = dBaseAmt - dLandAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            for (int i = 0; i < dtT.Rows.Count; i++)
            {
                drR = dtReceipt.NewRow();
                drR["Id"] = Convert.ToInt32(dtT.Rows[i]["OtherCostId"].ToString());
                drR["SchType"] = "O";
                drR["Amount"] = Convert.ToDecimal(dtT.Rows[i]["Amount"].ToString());
                drR["RAmount"] = 0;
                dtReceipt.Rows.Add(drR);
            }

            if (bPayTypewise == false)
            {
                for (int i = 0; i < dtTx.Rows.Count; i++)
                {
                    drR = dtReceipt.NewRow();
                    drR["Id"] = Convert.ToInt32(dtTx.Rows[i]["QualifierId"].ToString());
                    drR["SchType"] = "Q";
                    drR["Amount"] = Convert.ToDecimal(dtTx.Rows[i]["Amount"].ToString());
                    drR["RAmount"] = 0;
                    dtReceipt.Rows.Add(drR);
                }
            }

            sSql = "Select SchId,TemplateId,ReceiptTypeId,Percentage,OtherCostId,SchType from dbo.CCReceiptType " +
                    "Where TemplateId in (Select TemplateId from dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " and CostCentreId=" + iCCId + ") Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtTemp = new DataTable();
            dtTemp.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            sSql = "Select * from dbo.CCReceiptQualifier Where CostCentreId=" + iCCId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtQual = new DataTable();
            dtQual.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            sSql = "Select PaymentSchId,TemplateId,SchType,OtherCostId,SchPercent from dbo.PaymentScheduleFlat Where FlatId = " + argFlatId + " Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            decimal dAmt = 0;
            DataView dv;
            decimal dAdvRAmt = 0;

            DataTable dtTempT;
            DataTable dtQualT;
            dAdvBalAmt = dAdvAmt;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iPaymentSchId = Convert.ToInt32(dt.Rows[i]["PaymentSchId"].ToString());
                iTemplateId = Convert.ToInt32(dt.Rows[i]["TemplateId"].ToString());
                sSchType = dt.Rows[i]["SchType"].ToString();
                iOtherCostId = Convert.ToInt32(dt.Rows[i]["OtherCostId"].ToString());
                dSchPercent = Convert.ToDecimal(dt.Rows[i]["SchPercent"].ToString());
                dTNetAmt = 0;

                dAmt = 0;
                if (sSchType == "A")
                {
                    dAmt = dAdvAmt;
                }
                else if (sSchType == "O")
                {
                    dv = new DataView(dtT);
                    dv.RowFilter = "OtherCostId = " + iOtherCostId;
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        dAmt = Convert.ToDecimal(dv.ToTable().Rows[0]["Amount"].ToString());
                        if (dv.ToTable().Rows[0]["Flag"].ToString() == "-") { dAmt = dAmt * (-1); }
                    }
                    dv.Dispose();
                }
                else
                {
                    dAmt = dNetAmt * dSchPercent / 100;
                }

                dtTempT = new DataTable();
                dv = new DataView(dtTemp);
                dv.RowFilter = "TemplateId = " + iTemplateId;
                dtTempT = dv.ToTable();
                dv.Dispose();

                if (dtTempT.Rows.Count == 1 && sSchType == "O")
                {

                    sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                            "Values(" + iPaymentSchId + "," + argFlatId + ",0," + iOtherCostId + ",'" + sSchType + "',100," + dAmt + "," + dAmt + ") SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(sSql, conn, tran);
                    iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();

                    drT = dtReceipt.Select("SchType = 'O' and Id = " + iOtherCostId + "");

                    if (drT.Length > 0)
                    {
                        drT[0]["RAmount"] = dAmt;
                    }

                    dQNetAmt = dAmt;

                    dtQualT = new DataTable();
                    dv = new DataView(dtQual);
                    dv.RowFilter = "SchType = '" + sSchType + "' and OtherCostId = " + iOtherCostId;
                    dtQualT = dv.ToTable();
                    dv.Dispose();

                    if (dtQualT.Rows.Count > 0)
                    {
                        QualVBC = new Collection();

                        for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                        {
                            RAQual = new cRateQualR();

                            RAQual.Add_Less_Flag = dtQualT.Rows[Q]["Add_Less_Flag"].ToString();
                            RAQual.Amount = 0;
                            RAQual.Expression = dtQualT.Rows[Q]["Expression"].ToString();
                            RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                            RAQual.ExpPer = Convert.ToDecimal(dtQualT.Rows[Q]["ExpPer"].ToString());
                            RAQual.SurPer = Convert.ToDecimal(dtQualT.Rows[Q]["SurCharge"].ToString());
                            RAQual.EDPer = Convert.ToDecimal(dtQualT.Rows[Q]["EDCess"].ToString());

                            QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                        }

                        Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                        dQBaseAmt = dAmt;
                        dQNetAmt = dAmt; decimal dTaxAmt = 0;
                        decimal dVATAmt = 0;

                        if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                        {
                            foreach (Qualifier.cRateQualR d in QualVBC)
                            {
                                sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount) " +
                                        "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                        "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }

                        if (bPayTypewise == true)
                        {
                            sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }

                    if (bPayTypewise == true)
                        sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dQNetAmt + "  Where PaymentSchId = " + iPaymentSchId;
                    else
                        sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dAmt + "  Where PaymentSchId = " + iPaymentSchId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    dTNetAmt = dTNetAmt + dQNetAmt;
                }

                else
                {
                    dBalAmt = dAmt;
                    for (int j = 0; j < dtTempT.Rows.Count; j++)
                    {
                        iSchId = Convert.ToInt32(dtTempT.Rows[j]["SchId"].ToString());
                        dRPer = Convert.ToDecimal(dtTempT.Rows[j]["Percentage"].ToString());
                        sRSchType = dtTempT.Rows[j]["SchType"].ToString();
                        iReceiptId = Convert.ToInt32(dtTempT.Rows[j]["ReceiptTypeId"].ToString());
                        iROtherCostId = Convert.ToInt32(dtTempT.Rows[j]["OtherCostId"].ToString());

                        if (dRPer != 0) { dRAmt = dAmt * dRPer / 100; }
                        else { dRAmt = dBalAmt; }

                        if (dRAmt > dBalAmt) { dRAmt = dBalAmt; }


                        if (sRSchType == "A" && bAdvance == false)
                        {

                            dAdvRAmt = dAdvAmt * dRPer / 100;
                            if (dAdvRAmt > dAdvBalAmt) { dAdvRAmt = dAdvBalAmt; }
                            dAdvBalAmt = dAdvBalAmt - dAdvRAmt;
                            dTNetAmt = dTNetAmt - dAdvRAmt;

                            dAdv = dAdvRAmt;
                            sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                    "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + ", 0," + dAdvRAmt + ") SELECT SCOPE_IDENTITY();";
                            cmd = new SqlCommand(sSql, conn, tran);
                            iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();
                        }

                        else
                        {
                            dAdv = 0;
                            if (sRSchType == "A")
                            {
                                drT = dtReceipt.Select("SchType = 'A'");
                            }
                            else if (sRSchType == "O")
                            {
                                drT = dtReceipt.Select("SchType = 'O' and Id = " + iROtherCostId + "");
                            }
                            else if (sRSchType == "Q")
                            {
                                drT = dtReceipt.Select("SchType = 'Q' and Id = " + iReceiptId + "");
                            }
                            else
                            {
                                drT = dtReceipt.Select("SchType = 'R' and Id = " + iReceiptId + "");
                            }


                            decimal dRTAmt = 0;
                            decimal dRRAmt = 0;

                            if (drT.Length > 0)
                            {
                                dRTAmt = Convert.ToDecimal(drT[0]["Amount"].ToString());
                                dRRAmt = Convert.ToDecimal(drT[0]["RAmount"].ToString());
                            }

                            if (dRAmt > dRTAmt - dRRAmt)
                            {
                                dRAmt = dRTAmt - dRRAmt;
                            }

                            if (drT.Length > 0)
                            {
                                drT[0]["RAmount"] = dRRAmt + dRAmt;
                            }

                            if (dAmt == 0) { dRPer = 0; }
                            else dRPer = (dRAmt / dAmt) * 100;

                            dBalAmt = dBalAmt - dRAmt;

                            sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                    "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + "," + dRAmt + "," + dRAmt + ") SELECT SCOPE_IDENTITY();";
                            cmd = new SqlCommand(sSql, conn, tran);
                            iRSchId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();

                            if (bPayTypewise == false && sRSchType == "Q")
                            {
                                sSql = "Insert Into dbo.PaySchTaxFlat(PaymentSchId,FlatId,QualifierId,Percentage,Amount,Sel) " +
                                        "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + dRPer + "," + dRAmt + ",'" + true + "')";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }

                            dQNetAmt = dRAmt;

                            dtQualT = new DataTable();
                            dv = new DataView(dtQual);
                            dv.RowFilter = "SchType = '" + sRSchType + "' and ReceiptTypeId = " + iReceiptId + " and OtherCostId = " + iROtherCostId;
                            dtQualT = dv.ToTable();
                            dv.Dispose();
                            if (dtQualT.Rows.Count > 0)
                            {
                                QualVBC = new Collection();

                                for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                                {
                                    RAQual = new cRateQualR();

                                    RAQual.Add_Less_Flag = dtQualT.Rows[Q]["Add_Less_Flag"].ToString();
                                    RAQual.Amount = 0;
                                    RAQual.Expression = dtQualT.Rows[Q]["Expression"].ToString();
                                    RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                                    RAQual.ExpPer = Convert.ToDecimal(dtQualT.Rows[Q]["ExpPer"].ToString());
                                    RAQual.SurPer = Convert.ToDecimal(dtQualT.Rows[Q]["SurCharge"].ToString());
                                    RAQual.EDPer = Convert.ToDecimal(dtQualT.Rows[Q]["EDCess"].ToString());

                                    QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                                }

                                Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                                dQBaseAmt = dRAmt;
                                dQNetAmt = dRAmt; decimal dTaxAmt = 0;
                                decimal dVATAmt = 0;

                                if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                                {
                                    foreach (Qualifier.cRateQualR d in QualVBC)
                                    {
                                        sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount) " +
                                                "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                                "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + ")";
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                }
                                if (bPayTypewise == true)
                                {
                                    sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                            }


                            dTNetAmt = dTNetAmt + dQNetAmt;

                        }

                        //if (dBalAmt <= 0) { break; }
                    }

                    if (dBalAmt > 0)
                    {
                        for (int j = 0; j < dtReceiptOrder.Rows.Count; j++)
                        {
                            dRAmt = dBalAmt;

                            sRSchType = dtReceiptOrder.Rows[j]["SchType"].ToString();
                            iReceiptId = Convert.ToInt32(dtReceiptOrder.Rows[j]["ReceiptTypeId"].ToString());
                            iROtherCostId = Convert.ToInt32(dtReceiptOrder.Rows[j]["OtherCostId"].ToString());

                            if (sRSchType == "O")
                            {
                                drT = dtReceipt.Select("SchType = 'O' and Id = " + iROtherCostId + "");
                            }
                            else
                            {
                                drT = dtReceipt.Select("SchType = 'R' and Id = " + iReceiptId + "");
                            }

                            decimal dRTAmt = 0;
                            decimal dRRAmt = 0;

                            if (drT.Length > 0)
                            {
                                dRTAmt = Convert.ToDecimal(drT[0]["Amount"].ToString());
                                dRRAmt = Convert.ToDecimal(drT[0]["RAmount"].ToString());
                            }

                            if (dRAmt > dRTAmt - dRRAmt)
                            {
                                dRAmt = dRTAmt - dRRAmt;
                            }

                            if (drT.Length > 0)
                            {
                                drT[0]["RAmount"] = dRRAmt + dRAmt;
                            }

                            if (dRAmt > 0)
                            {
                                decimal dPCAmt = 0;
                                bool bAns = false;
                                sSql = "Select SchId,Amount,NetAmount from dbo.FlatReceiptType Where PaymentSchId = " + iPaymentSchId + " and " +
                                        "FlatId= " + argFlatId + " and ReceiptTypeId= " + iReceiptId + " and OtherCostId = " + iROtherCostId + " and SchType= '" + sRSchType + "'";
                                cmd = new SqlCommand(sSql, conn, tran);
                                sdr = cmd.ExecuteReader();
                                DataTable dtP = new DataTable();
                                dtP.Load(sdr);
                                sdr.Close();
                                cmd.Dispose();

                                if (dtP.Rows.Count > 0)
                                {
                                    dPCAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtP.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
                                    dTNetAmt = dTNetAmt - dPCAmt;
                                    dBalAmt = dBalAmt + dPCAmt;
                                    iRSchId = Convert.ToInt32(dtP.Rows[0]["SchId"].ToString());
                                    bAns = true;
                                }
                                dtP.Dispose();

                                if (bAns == true)
                                {
                                    dRAmt = dRAmt + dPCAmt;
                                    dRPer = (dRAmt / dAmt) * 100;

                                    sSql = "Update dbo.FlatReceiptType Set Amount= " + dRAmt + ",Percentage = " + dRPer + ",NetAmount = " + dRAmt + " Where SchId = " + iRSchId;
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();

                                    sSql = "Delete from dbo.FlatReceiptQualifier Where SchId = " + iRSchId;
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                                else
                                {
                                    dRPer = (dRAmt / dAmt) * 100;

                                    sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                            "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + "," + dRAmt + "," + dRAmt + ") SELECT SCOPE_IDENTITY();";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                                    cmd.Dispose();
                                }

                                dQNetAmt = dRAmt;

                                dtQualT = new DataTable();
                                dv = new DataView(dtQual);

                                if (sRSchType == "O")
                                {
                                    dv.RowFilter = "SchType = 'O' and ReceiptTypeId = 0 and OtherCostId = " + iROtherCostId + "";

                                }
                                else
                                {
                                    dv.RowFilter = "SchType = 'R' and ReceiptTypeId = " + iReceiptId + " and OtherCostId = 0";
                                }

                                dtQualT = dv.ToTable();
                                dv.Dispose();
                                if (dtQualT.Rows.Count > 0)
                                {
                                    QualVBC = new Collection();

                                    for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                                    {
                                        RAQual = new cRateQualR();

                                        RAQual.Add_Less_Flag = dtQualT.Rows[Q]["Add_Less_Flag"].ToString();
                                        RAQual.Amount = 0;
                                        RAQual.Expression = dtQualT.Rows[Q]["Expression"].ToString();
                                        RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                                        RAQual.ExpPer = Convert.ToDecimal(dtQualT.Rows[Q]["ExpPer"].ToString());
                                        RAQual.SurPer = Convert.ToDecimal(dtQualT.Rows[Q]["SurCharge"].ToString());
                                        RAQual.EDPer = Convert.ToDecimal(dtQualT.Rows[Q]["EDCess"].ToString());

                                        QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                                    }

                                    Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                                    dQBaseAmt = dRAmt;
                                    dQNetAmt = dRAmt; decimal dTaxAmt = 0;
                                    decimal dVATAmt = 0;

                                    if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                                    {
                                        foreach (Qualifier.cRateQualR d in QualVBC)
                                        {
                                            sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount) " +
                                                    "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                                    "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + ")";
                                            cmd = new SqlCommand(sSql, conn, tran);
                                            cmd.ExecuteNonQuery();
                                            cmd.Dispose();
                                        }
                                    }

                                    if (bPayTypewise == true)
                                    {
                                        sSql = "Update dbo.FlatReceiptType Set NetAmount = " + dQNetAmt + " Where SchId = " + iRSchId;
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                }

                                dTNetAmt = dTNetAmt + dQNetAmt;
                                dBalAmt = dBalAmt - dRAmt;
                                if (dBalAmt <= 0) { break; }
                            }
                        }

                    }
                    decimal dA = (dAmt - dAdv);
                    if (bPayTypewise == true)
                        sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dTNetAmt + "  Where PaymentSchId = " + iPaymentSchId;
                    else
                        sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + dAmt + ",NetAmount=" + dA + "  Where PaymentSchId = " + iPaymentSchId;

                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            dt.Dispose();

            if (bAdvance == false)
            {
                if (dtAdv.Rows.Count > 0)
                {
                    string sDate = string.Format(Convert.ToDateTime(CommFun.IsNullCheck(dtAdv.Rows[0]["SchDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MMM-yyyy"));

                    sSql = "Set Identity_Insert PaymentScheduleFlat On ";
                    if (sDate == "NULL")
                        //sSql = "Insert into dbo.PaymentScheduleFlat(FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,Amount,NetAmount,PreStageTypeId,SortOrder) " +
                        //        "Values(" + argFlatId + ",0," + iCCId + ",'A','Advance',0,0,0,NULL,0," + dAdvAmt + ",0,0)";
                        sSql = sSql + "Insert into dbo.PaymentScheduleFlat(PaymentSchId,FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,NetAmount,PreStageTypeId,SortOrder,BillPassed,PaidAmount,StageDetId) " +
                               "Values(" + dtAdv.Rows[0]["PaymentSchId"] + "," + argFlatId + ",0," + iCCId + ",'A','Advance',0,0,0,NULL," +
                                " '" + dtAdv.Rows[0]["DateAfter"] + "'," + dtAdv.Rows[0]["Duration"] + ",'" + dtAdv.Rows[0]["DurationType"] + "'," +
                               " " + dtAdv.Rows[0]["SchPercent"] + "," + dAdvAmt + "," + dAdvAmt + "," + dtAdv.Rows[0]["PreStageTypeId"] + "," +
                               " " + dtAdv.Rows[0]["SortOrder"] + ",'" + dtAdv.Rows[0]["BillPassed"] + "'," + dtAdv.Rows[0]["PaidAmount"] + "," + dtAdv.Rows[0]["StageDetId"] + ")";
                    else
                        sSql = sSql + "Insert into dbo.PaymentScheduleFlat(PaymentSchId,FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,NetAmount,PreStageTypeId,SortOrder,BillPassed,PaidAmount,StageDetId) " +
                                "Values(" + dtAdv.Rows[0]["PaymentSchId"] + "," + argFlatId + ",0," + iCCId + ",'A','Advance',0,0,0,'" + sDate + "'," +
                                 " '" + dtAdv.Rows[0]["DateAfter"] + "'," + dtAdv.Rows[0]["Duration"] + ",'" + dtAdv.Rows[0]["DurationType"] + "'," +
                                " " + dtAdv.Rows[0]["SchPercent"] + "," + dAdvAmt + "," + dAdvAmt + "," + dtAdv.Rows[0]["PreStageTypeId"] + "," +
                                " " + dtAdv.Rows[0]["SortOrder"] + ",'" + dtAdv.Rows[0]["BillPassed"] + "'," + dtAdv.Rows[0]["PaidAmount"] + "," + dtAdv.Rows[0]["StageDetId"] + ")";
                    sSql = sSql + "Set Identity_Insert PaymentscheduleFlat Off ";
                }
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }

            sSql = "Update dbo.PaymentScheduleFlat Set Advance=0";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "UPDATE PaymentScheduleFlat SET Advance=SummedQty FROM " +
                    " PaymentScheduleFlat A JOIN (SELECT PaymentSchId, SUM(NetAmount) SummedQty " +
                    " FROM FlatReceiptType WHERE SchType='A' GROUP BY PaymentSchId ) CCA ON A.PaymentSchId=CCA.PaymentSchId";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();


            //Schedule Date
            SqlDataReader dr;
            DateTime FinaliseDate = DateTime.Now;

            sSql = "Select FinaliseDate from dbo.BuyerDetail Where FlatId=" + argFlatId + "";
            cmd = new SqlCommand(sSql, conn, tran);
            dr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0)
            {
                FinaliseDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["FinaliseDate"], CommFun.datatypes.VarTypeDate));

                sSql = "Update dbo.PaymentScheduleFlat Set SchDate=@FinaliseDate " +
                        " Where TemplateId=0 And FlatId=" + argFlatId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                SqlParameter dateParameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@FinaliseDate" };
                if (FinaliseDate == DateTime.MinValue)
                    dateParameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                else
                    dateParameter.Value = FinaliseDate;
                cmd.Parameters.Add(dateParameter);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            else
            {
                sSql = "Update dbo.PaymentScheduleFlat Set SchDate=NULL " +
                        " Where TemplateId=0 And FlatId=" + argFlatId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
        }

        public static void UpdateCashBuyerScheduleQual(int argFlatId, DataTable argdt, SqlConnection conn, SqlTransaction tran)
        {
            string sSql = "";

            SqlDataReader sdr;
            SqlCommand cmd;
            DataTable dt = new DataTable();

            int iCCId = 0;
            int iFlatTypeId = 0;
            int iPayTypeId = 0;
            decimal dBaseAmt = 0;
            decimal dAdvAmt = 0;
            decimal dAdvBalAmt = 0;
            decimal dLandAmt = 0;
            decimal dNetAmt = 0;
            decimal dOtherAmt = 0;
            decimal dRAmt = 0;
            int iReceiptId = 0;
            int iROtherCostId = 0;
            string sRSchType = "";
            bool bAdvance = false;
            int iPaymentSchId = 0;
            string sSchType = "";
            int iOtherCostId = 0;
            decimal dRPer = 0;
            decimal dSchPercent = 0;
            decimal dQBaseAmt = 0;
            decimal dQNetAmt = 0;
            int iTemplateId = 0;
            int iSchId = 0;
            int iRSchId = 0;
            decimal dTNetAmt = 0;
            decimal dBalAmt = 0;
            bool bPayTypewise = false, bLCBon = false;
            decimal dTotalTax = 0;
            decimal dAdv = 0;
            DataRow[] drT;
            cRateQualR RAQual;
            Collection QualVBC;

            DataTable dtReceipt = new DataTable();

            sSql = "Select PaymentSchId,FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate," +
                    " DateAfter, Duration,DurationType,SchPercent,Amount,PreStageTypeId,SortOrder,BillPassed,PaidAmount from dbo.PaymentScheduleFlat " +
                    " Where FlatId=" + argFlatId + " And TemplateId<>0 Order By SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtPayTemp = new DataTable();
            dtPayTemp.Load(sdr);
            cmd.Dispose();

            sSql = "Delete from dbo.PaymentScheduleFlat Where FlatId= " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete from dbo.FlatReceiptQualifier Where SchId in (Select SchId from dbo.FlatReceiptType Where FlatId= " + argFlatId + ")";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete From dbo.PaySchTaxFlat Where FlatId=" + argFlatId + " ";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Delete from dbo.FlatReceiptType Where FlatId= " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();


            //sSql = "Select FlatTypeId,CostCentreId,PayTypeId,BaseAmt,AdvAmount,USLandAmt from dbo.FlatDetails Where FlatId= " + argFlatId;//modified
            sSql = "Select FlatTypeId,CostCentreId,PayTypeId,BaseAmt,AdvAmount,LandRate,Guidelinevalue,USLandAmt From dbo.FlatDetails Where FlatId= " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0)
            {
                iCCId = Convert.ToInt32(dt.Rows[0]["CostCentreId"].ToString());
                iFlatTypeId = Convert.ToInt32(dt.Rows[0]["FlatTypeId"].ToString());
                iPayTypeId = Convert.ToInt32(dt.Rows[0]["PayTypeId"].ToString());
                bPayTypewise = FlatDetailsDL.GetTypewise(iPayTypeId);
                dBaseAmt = Convert.ToDecimal(dt.Rows[0]["BaseAmt"].ToString());
                dAdvAmt = Convert.ToDecimal(dt.Rows[0]["AdvAmount"].ToString());

                sSql = "Select LCBasedon From dbo.ProjectInfo Where CostCentreId= " + iCCId;
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                DataTable dtPI = new DataTable();
                dtPI.Load(sdr);
                sdr.Close();
                cmd.Dispose();
                if (dtPI.Rows.Count > 0) { bLCBon = Convert.ToBoolean(dtPI.Rows[0]["LCBasedon"]); }
                if (bLCBon == false) { dLandAmt = Convert.ToDecimal(dt.Rows[0]["LandRate"].ToString()); }
                else { dLandAmt = Convert.ToDecimal(dt.Rows[0]["USLandAmt"].ToString()); }

                //dLandAmt = Convert.ToDecimal(dt.Rows[0]["USLandAmt"].ToString());
                //dLandAmt = Convert.ToDecimal(dt.Rows[0]["LandRate"].ToString());
            }
            dt.Dispose();

            sSql = "Select TemplateId From dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " and CostCentreId = " + iCCId + " and SchType='A'";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0) { bAdvance = true; }
            dt.Dispose();

            sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                    "Where FlatId = " + argFlatId + " and OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans Where PayTypeId=" + iPayTypeId + " and CostCentreId=" + iCCId + ")"+
                    " AND OtherCostId NOT IN(Select OtherCostId from dbo.OXGross Where CostCentreId=" + iCCId + ")";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            if (dt.Rows.Count > 0) { dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }
            dt.Dispose();

            sSql = "Select QualifierId,Amount from dbo.FlatTax Where FlatId = " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtTx = new DataTable();
            dtTx.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            decimal dT = 0;
            if (dtTx.Rows.Count > 0)
            {
                for (int i = 0; i < dtTx.Rows.Count; i++)
                {
                    dTotalTax = Convert.ToDecimal(dtTx.Rows[i]["Amount"]);
                    dT = dT + dTotalTax;
                }
            }

            if (bPayTypewise == false)
            { dNetAmt = dBaseAmt + dOtherAmt + dT; }
            else
            { dNetAmt = dBaseAmt + dOtherAmt; }
            if (bAdvance == true) { dNetAmt = dNetAmt - dAdvAmt; }

            if (argdt.Rows.Count > 0)
            {
                for (int i = 0; i < argdt.Rows.Count; i++)
                {
                    string sDate = string.Format(Convert.ToDateTime(CommFun.IsNullCheck(argdt.Rows[i]["SchDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MMM-yyyy"));
                    //for (int j = 0; j < dtPayTemp.Rows.Count; j++)
                    //{
                    if (argdt.Rows[i]["SchDate"].ToString() == "")
                    {
                        sDate = "NULL";
                        sSql = "Set Identity_Insert PaymentScheduleFlat On ";
                        sSql = sSql + "Insert into dbo.PaymentScheduleFlat(PaymentSchId,FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId," +
                        " OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId,SortOrder,BillPassed,PaidAmount) " +
                        " Values(" + argdt.Rows[i]["PaymentSchId"] + "," + argdt.Rows[i]["FlatId"] + "," + argdt.Rows[i]["TemplateId"] + "," + argdt.Rows[i]["CostCentreId"] + "," +
                        " '" + argdt.Rows[i]["SchType"] + "','" + argdt.Rows[i]["Description"] + "'," + argdt.Rows[i]["SchDescId"] + "," +
                        " " + argdt.Rows[i]["StageId"] + "," + argdt.Rows[i]["OtherCostId"] + "," + sDate + "," +
                        " '" + argdt.Rows[i]["DateAfter"] + "'," + argdt.Rows[i]["Duration"] + ",'" + argdt.Rows[i]["DurationType"] + "'," +
                        " " + argdt.Rows[i]["SchPercent"] + "," + argdt.Rows[i]["Amount"] + "," + argdt.Rows[i]["PreStageTypeId"] + "," +
                        " " + argdt.Rows[i]["SortOrder"] + ",'" + argdt.Rows[i]["BillPassed"] + "'," + argdt.Rows[i]["PaidAmount"] + ")";
                        sSql = sSql + "Set Identity_Insert PaymentscheduleFlat Off ";
                    }
                    else
                    {
                        sSql = "Set Identity_Insert PaymentScheduleFlat On ";
                        sSql = sSql + "Insert into dbo.PaymentScheduleFlat(PaymentSchId,FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId," +
                        " OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId,SortOrder,BillPassed,PaidAmount) " +
                        " Values(" + argdt.Rows[i]["PaymentSchId"] + "," + argdt.Rows[i]["FlatId"] + "," + argdt.Rows[i]["TemplateId"] + "," + argdt.Rows[i]["CostCentreId"] + "," +
                        " '" + argdt.Rows[i]["SchType"] + "','" + argdt.Rows[i]["Description"] + "'," + argdt.Rows[i]["SchDescId"] + "," +
                        " " + argdt.Rows[i]["StageId"] + "," + argdt.Rows[i]["OtherCostId"] + ",'" + sDate + "'," +
                        " '" + argdt.Rows[i]["DateAfter"] + "'," + argdt.Rows[i]["Duration"] + ",'" + argdt.Rows[i]["DurationType"] + "'," +
                        " " + argdt.Rows[i]["SchPercent"] + "," + argdt.Rows[i]["Amount"] + "," + argdt.Rows[i]["PreStageTypeId"] + "," +
                        " " + argdt.Rows[i]["SortOrder"] + ",'" + argdt.Rows[i]["BillPassed"] + "'," + argdt.Rows[i]["PaidAmount"] + ")";
                        sSql = sSql + "Set Identity_Insert PaymentscheduleFlat Off ";
                    }
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    //}
                }
            }

            sSql = "Select ReceiptTypeId,OtherCostId,SchType from dbo.ReceiptTypeOrder " +
                    "Where PayTypeId = " + iPayTypeId + " and CostCentreId=" + iCCId + " and SchType <>'A' Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtReceiptOrder = new DataTable();
            dtReceiptOrder.Load(sdr);
            sdr.Close();
            cmd.Dispose();


            sSql = "Select OtherCostId,Flag,Amount from dbo.FlatOtherCost Where FlatId = " + argFlatId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtT = new DataTable();
            dtT.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            dtReceipt.Columns.Add("Id", typeof(int));
            dtReceipt.Columns.Add("SchType", typeof(string));
            dtReceipt.Columns.Add("Amount", typeof(decimal));
            dtReceipt.Columns.Add("RAmount", typeof(decimal));

            DataRow drR;
            drR = dtReceipt.NewRow();
            drR["Id"] = 1;
            drR["SchType"] = "A";
            drR["Amount"] = dAdvAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            drR = dtReceipt.NewRow();
            drR["Id"] = 2;
            drR["SchType"] = "R";
            drR["Amount"] = dLandAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            drR = dtReceipt.NewRow();
            drR["Id"] = 3;
            drR["SchType"] = "R";
            drR["Amount"] = dBaseAmt - dLandAmt;
            drR["RAmount"] = 0;
            dtReceipt.Rows.Add(drR);

            for (int i = 0; i < dtT.Rows.Count; i++)
            {
                drR = dtReceipt.NewRow();
                drR["Id"] = Convert.ToInt32(dtT.Rows[i]["OtherCostId"].ToString());
                drR["SchType"] = "O";
                drR["Amount"] = Convert.ToDecimal(dtT.Rows[i]["Amount"].ToString());
                drR["RAmount"] = 0;
                dtReceipt.Rows.Add(drR);
            }

            if (bPayTypewise == false)
            {
                for (int i = 0; i < dtTx.Rows.Count; i++)
                {
                    drR = dtReceipt.NewRow();
                    drR["Id"] = Convert.ToInt32(dtTx.Rows[i]["QualifierId"].ToString());
                    drR["SchType"] = "Q";
                    drR["Amount"] = Convert.ToDecimal(dtTx.Rows[i]["Amount"].ToString());
                    drR["RAmount"] = 0;
                    dtReceipt.Rows.Add(drR);
                }
            }

            sSql = "Select SchId,TemplateId,ReceiptTypeId,Percentage,OtherCostId,SchType from dbo.CCReceiptType " +
                    "Where TemplateId in (Select TemplateId from dbo.PaymentSchedule Where TypeId=" + iPayTypeId + " and CostCentreId=" + iCCId + ") Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtTemp = new DataTable();
            dtTemp.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            sSql = "Select * from dbo.CCReceiptQualifier Where CostCentreId=" + iCCId;
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            DataTable dtQual = new DataTable();
            dtQual.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            sSql = "Select PaymentSchId,TemplateId,SchType,OtherCostId,SchPercent from dbo.PaymentScheduleFlat Where FlatId = " + argFlatId + " Order by SortOrder";
            cmd = new SqlCommand(sSql, conn, tran);
            sdr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            sdr.Close();
            cmd.Dispose();

            decimal dAmt = 0;
            DataView dv;
            decimal dAdvRAmt = 0;

            DataTable dtTempT;
            DataTable dtQualT;
            dAdvBalAmt = dAdvAmt;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iPaymentSchId = Convert.ToInt32(dt.Rows[i]["PaymentSchId"].ToString());
                iTemplateId = Convert.ToInt32(dt.Rows[i]["TemplateId"].ToString());
                sSchType = dt.Rows[i]["SchType"].ToString();
                iOtherCostId = Convert.ToInt32(dt.Rows[i]["OtherCostId"].ToString());
                dSchPercent = Convert.ToDecimal(dt.Rows[i]["SchPercent"].ToString());
                dTNetAmt = 0;

                dAmt = 0;
                if (sSchType == "A")
                {
                    dAmt = dAdvAmt;
                }
                else if (sSchType == "O")
                {
                    dv = new DataView(dtT);
                    dv.RowFilter = "OtherCostId = " + iOtherCostId;
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        dAmt = Convert.ToDecimal(dv.ToTable().Rows[0]["Amount"].ToString());
                        if (dv.ToTable().Rows[0]["Flag"].ToString() == "-") { dAmt = dAmt * (-1); }
                    }
                    dv.Dispose();
                }
                else
                {
                    dAmt = (dNetAmt * dSchPercent) / 100;
                }

                dtTempT = new DataTable();
                dv = new DataView(dtTemp);
                dv.RowFilter = "TemplateId = " + iTemplateId;
                dtTempT = dv.ToTable();
                dv.Dispose();

                if (dtTempT.Rows.Count == 1 && sSchType == "O")
                {

                    sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                            "Values(" + iPaymentSchId + "," + argFlatId + ",0," + iOtherCostId + ",'" + sSchType + "',100," + dAmt + "," + dAmt + ") SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(sSql, conn, tran);
                    iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();

                    drT = dtReceipt.Select("SchType = 'O' and Id = " + iOtherCostId + "");

                    if (drT.Length > 0)
                    {
                        drT[0]["RAmount"] = dAmt;
                    }

                    dQNetAmt = dAmt;

                    dtQualT = new DataTable();
                    dv = new DataView(dtQual);
                    dv.RowFilter = "SchType = '" + sSchType + "' and OtherCostId = " + iOtherCostId;
                    dtQualT = dv.ToTable();
                    dv.Dispose();

                    if (dtQualT.Rows.Count > 0)
                    {
                        QualVBC = new Collection();

                        for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                        {
                            RAQual = new cRateQualR();

                            RAQual.Add_Less_Flag = dtQualT.Rows[Q]["Add_Less_Flag"].ToString();
                            RAQual.Amount = 0;
                            RAQual.Expression = dtQualT.Rows[Q]["Expression"].ToString();
                            RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                            RAQual.ExpPer = Convert.ToDecimal(dtQualT.Rows[Q]["ExpPer"].ToString());
                            RAQual.SurPer = Convert.ToDecimal(dtQualT.Rows[Q]["SurCharge"].ToString());
                            RAQual.EDPer = Convert.ToDecimal(dtQualT.Rows[Q]["EDCess"].ToString());

                            QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                        }

                        Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                        dQBaseAmt = dAmt;
                        dQNetAmt = dAmt; decimal dTaxAmt = 0;
                        decimal dVATAmt = 0;

                        if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                        {
                            foreach (Qualifier.cRateQualR d in QualVBC)
                            {
                                sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount) " +
                                        "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                        "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + ")";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }

                        if (bPayTypewise == true)
                        {
                            sSql = "Update dbo.FlatReceiptType Set NetAmount = " + Math.Round(dQNetAmt, 0) + " Where SchId = " + iRSchId;
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }

                    if (bPayTypewise == true)
                        sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + Math.Round(dAmt, 0) + ",NetAmount=" + Math.Round(dQNetAmt, 0) + "  Where PaymentSchId = " + iPaymentSchId;
                    else
                        sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + Math.Round(dAmt, 0) + ",NetAmount=" + Math.Round(dAmt, 0) + "  Where PaymentSchId = " + iPaymentSchId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    dTNetAmt = dTNetAmt + dQNetAmt;
                }

                else
                {
                    dBalAmt = dAmt;
                    for (int j = 0; j < dtTempT.Rows.Count; j++)
                    {
                        iSchId = Convert.ToInt32(dtTempT.Rows[j]["SchId"].ToString());
                        dRPer = Convert.ToDecimal(dtTempT.Rows[j]["Percentage"].ToString());
                        sRSchType = dtTempT.Rows[j]["SchType"].ToString();
                        iReceiptId = Convert.ToInt32(dtTempT.Rows[j]["ReceiptTypeId"].ToString());
                        iROtherCostId = Convert.ToInt32(dtTempT.Rows[j]["OtherCostId"].ToString());

                        if (dRPer != 0) { dRAmt = (dAmt * dRPer) / 100; }
                        else { dRAmt = dBalAmt; }

                        if (dRAmt > dBalAmt) { dRAmt = dBalAmt; }


                        if (sRSchType == "A" && bAdvance == false)
                        {

                            dAdvRAmt = (dAdvAmt * dRPer) / 100;
                            if (dAdvRAmt > dAdvBalAmt) { dAdvRAmt = dAdvBalAmt; }
                            dAdvBalAmt = dAdvBalAmt - dAdvRAmt;
                            dTNetAmt = dTNetAmt - dAdvRAmt;

                            dAdv = dAdvRAmt;
                            sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                    "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + ", 0," + dAdvRAmt + ") SELECT SCOPE_IDENTITY();";
                            cmd = new SqlCommand(sSql, conn, tran);
                            iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();
                        }

                        else
                        {
                            dAdv = 0;
                            if (sRSchType == "A")
                            {
                                drT = dtReceipt.Select("SchType = 'A'");
                            }
                            else if (sRSchType == "O")
                            {
                                drT = dtReceipt.Select("SchType = 'O' and Id = " + iROtherCostId + "");
                            }
                            else if (sRSchType == "Q")
                            {
                                drT = dtReceipt.Select("SchType = 'Q' and Id = " + iReceiptId + "");
                            }
                            else
                            {
                                drT = dtReceipt.Select("SchType = 'R' and Id = " + iReceiptId + "");
                            }


                            decimal dRTAmt = 0;
                            decimal dRRAmt = 0;

                            if (drT.Length > 0)
                            {
                                dRTAmt = Convert.ToDecimal(drT[0]["Amount"].ToString());
                                dRRAmt = Convert.ToDecimal(drT[0]["RAmount"].ToString());
                            }

                            if (dRAmt > dRTAmt - dRRAmt)
                            {
                                dRAmt = dRTAmt - dRRAmt;
                            }

                            if (drT.Length > 0)
                            {
                                drT[0]["RAmount"] = dRRAmt + dRAmt;
                            }

                            if (dAmt == 0) { dRPer = 0; }
                            else dRPer = (dRAmt / dAmt) * 100;

                            dBalAmt = dBalAmt - dRAmt;

                            sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                    "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + "," + Math.Round(dRAmt, 0) + "," + Math.Round(dRAmt, 0) + ") SELECT SCOPE_IDENTITY();";
                            cmd = new SqlCommand(sSql, conn, tran);
                            iRSchId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();

                            if (bPayTypewise == false && sRSchType == "Q")
                            {
                                sSql = "Insert Into dbo.PaySchTaxFlat(PaymentSchId,FlatId,QualifierId,Percentage,Amount,Sel) " +
                                        "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + dRPer + "," + dRAmt + ",'" + true + "')";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }

                            dQNetAmt = dRAmt;

                            dtQualT = new DataTable();
                            dv = new DataView(dtQual);
                            dv.RowFilter = "SchType = '" + sRSchType + "' and ReceiptTypeId = " + iReceiptId + " and OtherCostId = " + iROtherCostId;
                            dtQualT = dv.ToTable();
                            dv.Dispose();
                            if (dtQualT.Rows.Count > 0)
                            {
                                QualVBC = new Collection();

                                for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                                {
                                    RAQual = new cRateQualR();

                                    RAQual.Add_Less_Flag = dtQualT.Rows[Q]["Add_Less_Flag"].ToString();
                                    RAQual.Amount = 0;
                                    RAQual.Expression = dtQualT.Rows[Q]["Expression"].ToString();
                                    RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                                    RAQual.ExpPer = Convert.ToDecimal(dtQualT.Rows[Q]["ExpPer"].ToString());
                                    RAQual.SurPer = Convert.ToDecimal(dtQualT.Rows[Q]["SurCharge"].ToString());
                                    RAQual.EDPer = Convert.ToDecimal(dtQualT.Rows[Q]["EDCess"].ToString());

                                    QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                                }

                                Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                                dQBaseAmt = dRAmt;
                                dQNetAmt = dRAmt; decimal dTaxAmt = 0;
                                decimal dVATAmt = 0;

                                if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                                {
                                    foreach (Qualifier.cRateQualR d in QualVBC)
                                    {
                                        sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount) " +
                                                "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                                "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + ")";
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                }
                                if (bPayTypewise == true)
                                {
                                    sSql = "Update dbo.FlatReceiptType Set NetAmount = " + Math.Round(dQNetAmt, 0) + " Where SchId = " + iRSchId;
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                            }
                            dTNetAmt = dTNetAmt + dQNetAmt;
                        }
                    }

                    if (dBalAmt > 0)
                    {
                        for (int j = 0; j < dtReceiptOrder.Rows.Count; j++)
                        {
                            dRAmt = dBalAmt;

                            sRSchType = dtReceiptOrder.Rows[j]["SchType"].ToString();
                            iReceiptId = Convert.ToInt32(dtReceiptOrder.Rows[j]["ReceiptTypeId"].ToString());
                            iROtherCostId = Convert.ToInt32(dtReceiptOrder.Rows[j]["OtherCostId"].ToString());

                            if (sRSchType == "O")
                            {
                                drT = dtReceipt.Select("SchType = 'O' and Id = " + iROtherCostId + "");
                            }
                            else
                            {
                                drT = dtReceipt.Select("SchType = 'R' and Id = " + iReceiptId + "");
                            }

                            decimal dRTAmt = 0;
                            decimal dRRAmt = 0;

                            if (drT.Length > 0)
                            {
                                dRTAmt = Convert.ToDecimal(drT[0]["Amount"].ToString());
                                dRRAmt = Convert.ToDecimal(drT[0]["RAmount"].ToString());
                            }

                            if (dRAmt > dRTAmt - dRRAmt)
                            {
                                dRAmt = dRTAmt - dRRAmt;
                            }

                            if (drT.Length > 0)
                            {
                                drT[0]["RAmount"] = dRRAmt + dRAmt;
                            }

                            if (dRAmt > 0)
                            {
                                decimal dPCAmt = 0;
                                bool bAns = false;
                                sSql = "Select SchId,Amount,NetAmount from dbo.FlatReceiptType Where PaymentSchId = " + iPaymentSchId + " and " +
                                        "FlatId= " + argFlatId + " and ReceiptTypeId= " + iReceiptId + " and OtherCostId = " + iROtherCostId + " and SchType= '" + sRSchType + "'";
                                cmd = new SqlCommand(sSql, conn, tran);
                                sdr = cmd.ExecuteReader();
                                DataTable dtP = new DataTable();
                                dtP.Load(sdr);
                                sdr.Close();
                                cmd.Dispose();

                                if (dtP.Rows.Count > 0)
                                {
                                    dPCAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtP.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
                                    dTNetAmt = dTNetAmt - dPCAmt;
                                    dBalAmt = dBalAmt + dPCAmt;
                                    iRSchId = Convert.ToInt32(dtP.Rows[0]["SchId"].ToString());
                                    bAns = true;
                                }
                                dtP.Dispose();

                                if (bAns == true)
                                {
                                    dRAmt = dRAmt + dPCAmt;
                                    dRPer = (dRAmt / dAmt) * 100;

                                    sSql = "Update dbo.FlatReceiptType Set Amount= " + dRAmt + ",Percentage = " + dRPer + ",NetAmount = " + Math.Round(dRAmt, 0) + " Where SchId = " + iRSchId;
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();

                                    sSql = "Delete from dbo.FlatReceiptQualifier Where SchId = " + iRSchId;
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                                else
                                {
                                    dRPer = (dRAmt / dAmt) * 100;

                                    sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,OtherCostId,SchType,Percentage,Amount,NetAmount) " +
                                            "Values(" + iPaymentSchId + "," + argFlatId + "," + iReceiptId + "," + iROtherCostId + ",'" + sRSchType + "'," + dRPer + "," + Math.Round(dRAmt, 0) + "," + Math.Round(dRAmt, 0) + ") SELECT SCOPE_IDENTITY();";
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    iRSchId = int.Parse(cmd.ExecuteScalar().ToString());
                                    cmd.Dispose();
                                }

                                dQNetAmt = dRAmt;

                                dtQualT = new DataTable();
                                dv = new DataView(dtQual);

                                if (sRSchType == "O")
                                {
                                    dv.RowFilter = "SchType = 'O' and ReceiptTypeId = 0 and OtherCostId = " + iROtherCostId + "";

                                }
                                else
                                {
                                    dv.RowFilter = "SchType = 'R' and ReceiptTypeId = " + iReceiptId + " and OtherCostId = 0";
                                }

                                dtQualT = dv.ToTable();
                                dv.Dispose();
                                if (dtQualT.Rows.Count > 0)
                                {
                                    QualVBC = new Collection();

                                    for (int Q = 0; Q < dtQualT.Rows.Count; Q++)
                                    {
                                        RAQual = new cRateQualR();

                                        RAQual.Add_Less_Flag = dtQualT.Rows[Q]["Add_Less_Flag"].ToString();
                                        RAQual.Amount = 0;
                                        RAQual.Expression = dtQualT.Rows[Q]["Expression"].ToString();
                                        RAQual.RateID = Convert.ToInt32(dtQualT.Rows[Q]["QualifierId"]);
                                        RAQual.ExpPer = Convert.ToDecimal(dtQualT.Rows[Q]["ExpPer"].ToString());
                                        RAQual.SurPer = Convert.ToDecimal(dtQualT.Rows[Q]["SurCharge"].ToString());
                                        RAQual.EDPer = Convert.ToDecimal(dtQualT.Rows[Q]["EDCess"].ToString());

                                        QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                                    }

                                    Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                                    dQBaseAmt = dRAmt;
                                    dQNetAmt = dRAmt; decimal dTaxAmt = 0;
                                    decimal dVATAmt = 0;

                                    if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, DateTime.Now,ref dVATAmt) == true)
                                    {
                                        foreach (Qualifier.cRateQualR d in QualVBC)
                                        {
                                            sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount) " +
                                                    "Values(" + iRSchId + "," + d.RateID + ",'" + d.Expression + "'," + d.ExpPer + ",'" + d.Add_Less_Flag + "'," +
                                                    "" + d.SurPer + "," + d.EDPer + "," + d.ExpValue + "," + d.ExpPerValue + "," + d.SurValue + "," + d.EDValue + "," + d.Amount + ")";
                                            cmd = new SqlCommand(sSql, conn, tran);
                                            cmd.ExecuteNonQuery();
                                            cmd.Dispose();
                                        }
                                    }

                                    if (bPayTypewise == true)
                                    {
                                        sSql = "Update dbo.FlatReceiptType Set NetAmount = " + Math.Round(dQNetAmt, 0) + " Where SchId = " + iRSchId;
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                }

                                dTNetAmt = dTNetAmt + dQNetAmt;
                                dBalAmt = dBalAmt - dRAmt;
                                if (dBalAmt <= 0) { break; }
                            }
                        }

                    }
                    decimal dA = (dAmt - dAdv);
                    if (bPayTypewise == true)
                        sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + Math.Round(dAmt, 0) + ",NetAmount=" + Math.Round(dTNetAmt, 0) + "  Where PaymentSchId = " + iPaymentSchId;
                    else
                        sSql = "Update dbo.PaymentScheduleFlat Set Amount= " + Math.Round(dAmt, 0) + ",NetAmount=" + Math.Round(dA, 0) + "  Where PaymentSchId = " + iPaymentSchId;

                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            dt.Dispose();


            if (bAdvance == false)
            {
                sSql = "Insert into dbo.PaymentScheduleFlat(FlatId,TemplateId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,Amount,NetAmount,PreStageTypeId,SortOrder) " +
                        "Values(" + argFlatId + ",0," + iCCId + ",'A','Advance',0,0,0,NULL,0," + dAdvAmt + ",0,0)";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }

            sSql = "Update dbo.PaymentScheduleFlat Set Advance=0";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "UPDATE PaymentScheduleFlat SET Advance=SummedQty FROM " +
                    " PaymentScheduleFlat A JOIN (SELECT PaymentSchId, SUM(NetAmount) SummedQty " +
                    " FROM FlatReceiptType WHERE SchType='A' GROUP BY PaymentSchId ) CCA ON A.PaymentSchId=CCA.PaymentSchId";
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            //Schedule Date
            //SqlDataReader dr;
            //DateTime FinaliseDate = DateTime.Now;

            //sSql = "Select FinaliseDate from dbo.BuyerDetail Where FlatId=" + argFlatId + "";
            //cmd = new SqlCommand(sSql, conn, tran);
            //dr = cmd.ExecuteReader();
            //dt = new DataTable();
            //dt.Load(dr); cmd.Dispose();
            //if (dt.Rows.Count > 0)
            //{
            //    FinaliseDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["FinaliseDate"], CommFun.datatypes.VarTypeDate));

            //    sSql = "Update dbo.PaymentScheduleFlat Set SchDate='" + FinaliseDate.ToString("dd-MMM-yyyy") + "' " +
            //            " Where TemplateId=0 And FlatId=" + argFlatId + "";
            //}
            //else
            //{
            //    sSql = "Update dbo.PaymentScheduleFlat Set SchDate=NULL " +
            //            " Where TemplateId=0 And FlatId=" + argFlatId + "";
            //}
            //cmd = new SqlCommand(sSql, conn, tran);
            //cmd.ExecuteNonQuery();
            //cmd.Dispose();
        }

        #endregion


        public static DataTable GetCommPaySchFlat(int argCCId, int argFlatId, int argPayTypeId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;            
            try
            {

                string sSql = "Select PaymentSchId,FlatId,TemplateId,CostCentreId,SchType,[Description],SchDescId,StageId,OtherCostId,SchDate," +
                                " DateAfter, Duration,DurationType,SchPercent,Amount,NetAmount,BillPassed,PaidAmount,StageDetId,PreStageTypeId,SortOrder from dbo.PaymentScheduleFlat " +
                                " Where FlatId=" + argFlatId + " AND CostCentreId=" + argCCId + " And TemplateId<>0 " +
                                " AND OtherCostId NOT IN(Select OtherCostId from dbo.OXGross Where CostCentreId=" + argCCId + ")" +
                                " UNION ALL " +
                                " Select 0 PaymentSchId,FlatId,C.TemplateId,C.CostCentreId,C.SchType,C.Description,C.SchDescId,C.StageId,A.OtherCostId, " +
                                " C.SchDate, C.DateAfter, C.Duration, C.DurationType, C.SchPercent, A.Amount, 0 NetAmount,0 BillPassed,0 PaidAmount, " +
                                " 0 StageDetId,C.PreStageTypeId,C.SortOrder from dbo.FlatOtherCost A " +
                                " INNER JOIN dbo.PaymentSchedule C ON A.OtherCostId=C.OtherCostId " +
                                " Where A.FlatId=" + argFlatId + "  AND C.CostCentreId=" + argCCId + "  AND C.TypeId=" + argPayTypeId + "  AND C.SchType NOT IN('A', 'S') " +
                                " AND C.OtherCostId NOT IN(Select OtherCostId from dbo.PaymentScheduleFlat  Where FlatId=" + argFlatId + "  AND CostCentreId=" + argCCId + "  " +
                                " And TemplateId<>0 AND SchType NOT IN('A', 'S')) " +
                                " AND A.OtherCostId NOT IN(Select OtherCostId from dbo.PaymentScheduleFlat  Where FlatId=" + argFlatId + "  AND CostCentreId=" + argCCId + "   " +
                                " And TemplateId<>0 AND SchType NOT IN('A', 'S'))" +
                                " AND A.OtherCostId NOT IN(Select OtherCostId from dbo.OXGross Where CostCentreId=" + argCCId + ")" +
                                " Order By SortOrder";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (SqlException e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable GetReceiptCommPaySchFlat(int argCCId, int argFlatId, int argPayTypeId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Select PaymentSchId,FlatId,TemplateId,CostCentreId,SchType,[Description],SchDescId,StageId,OtherCostId,SchDate," +
                                " DateAfter, Duration,DurationType,SchPercent,Amount,NetAmount,BillPassed,PaidAmount,StageDetId,PreStageTypeId,SortOrder from dbo.PaymentScheduleFlat " +
                                " Where FlatId=" + argFlatId + " AND CostCentreId=" + argCCId + " And TemplateId<>0 " +
                                " AND OtherCostId NOT IN(Select OtherCostId from dbo.OXGross Where CostCentreId=" + argCCId + ")" +
                                " UNION ALL " +
                                " Select 0 PaymentSchId,FlatId,C.TemplateId,C.CostCentreId,C.SchType,C.Description,C.SchDescId,C.StageId,A.OtherCostId, " +
                                " C.SchDate, C.DateAfter, C.Duration, C.DurationType, C.SchPercent, A.Amount, 0 NetAmount,0 BillPassed,0 PaidAmount, " +
                                " 0 StageDetId,C.PreStageTypeId,C.SortOrder from dbo.FlatOtherCost A " +
                                " INNER JOIN dbo.PaymentSchedule C ON A.OtherCostId=C.OtherCostId " +
                                " Where A.FlatId=" + argFlatId + "  AND C.CostCentreId=" + argCCId + "  AND C.TypeId=" + argPayTypeId + "  AND C.SchType NOT IN('A', 'S') " +
                                " AND C.OtherCostId NOT IN(Select OtherCostId from dbo.PaymentScheduleFlat  Where FlatId=" + argFlatId + "  AND CostCentreId=" + argCCId + "  " +
                                " And TemplateId<>0 AND SchType NOT IN('A', 'S')) " +
                                " AND A.OtherCostId NOT IN(Select OtherCostId from dbo.PaymentScheduleFlat  Where FlatId=" + argFlatId + "  AND CostCentreId=" + argCCId + "   " +
                                " And TemplateId<>0 AND SchType NOT IN('A', 'S'))" +
                                " AND A.OtherCostId NOT IN(Select OtherCostId from dbo.OXGross Where CostCentreId=" + argCCId + ")" +
                                " Order By SortOrder";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (SqlException e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataSet GetReceiptQ(int argCCId)
        {
            DataSet ds = new DataSet();
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            try
            {
                sSql = "Select ReceiptTypeId,0 OtherCostId,'R' SchType,ReceiptTypeName from dbo.ReceiptType Where ReceiptTypeId <>1 " +
                        " Union all " +
                        " Select 0 ReceiptTypeId,A.OtherCostId,'O' Schtype,OtherCostName ReceiptTypeName from dbo.OtherCostMaster A " +
                        " Inner Join dbo.CCOtherCost CO On Co.OtherCostId=A.OtherCostId Where CO.CostCentreId=" + argCCId + " ";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "ReceiptType");
                sda.Dispose();

                sSql = "Select QTransId, CostCentreId, SchType, ReceiptTypeId, OtherCostId, QualifierId, Expression, ExpPer, Add_Less_Flag, " +
                       " SurCharge, EDCess, HEDPer HEDCess, HEDValue, NetPer Net, TaxablePer Taxable, TaxableValue From dbo.CCReceiptQualifier " +
                       " Where CostCentreId= " + argCCId;
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Qualifier");
                sda.Dispose();
            }
            catch (SqlException e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return ds;
        }

        public static void InsertFlatSchedule(int argFlatId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                InsertFlatScheduleI(argFlatId, conn, trans);
                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        public static void InsertFinalFlatSchedule(int argFlatId, string argsType)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                InsertFinalFlatScheduleI(argFlatId, argsType, conn, trans);
                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        public static void UpdateFlatSchedule(int argFlatId, DataTable argdt)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                UpdateBuyerSchedule(argFlatId, argdt, conn, trans);
                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        public static void UpdateFinalFlatSchedule(int argFlatId, string argsType, DataTable argdt)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                UpdateFinalBuyerSchedule(argFlatId, argsType, argdt, conn, trans);
                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }
        }

        public static void UpdateFlatScheduleQual(int argFlatId, DataTable argdt)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                UpdateBuyerScheduleQual(argFlatId, argdt, conn, trans);
                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        public static void UpdateReceiptFlatSchedule(int argFlatId, DataTable argdt)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                UpdateReceiptBuyerSchedule(argFlatId, argdt, conn, trans);
                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        public static void UpdateReceiptFlatScheduleQual(int argFlatId, DataTable argdt)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                UpdateReceiptBuyerScheduleQual(argFlatId, argdt, conn, trans);
                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        public static void InsertPaymentSchFlat(DataTable argPayTrans, int argFTId, int argFId, DataTable argdtR, SqlConnection conn, SqlTransaction tran)
        {
            int tr = 0;
            Decimal dNetAmt;
            Decimal dSchAmt;
            Decimal dAdvAmt;
            decimal dOther; decimal dOCAmt;
            decimal dLandAmt = 0; decimal dBaseAmt = 0;
            decimal dTotLAmt = 0, dTotBAmt = 0, dTotAdvAmt = 0;
            int OCTransId;
            DataView dv;
            DataTable dtRec;
            SqlCommand cmd;


            string sSql = "";

            sSql = "DELETE FROM dbo.PaymentScheduleFlat WHERE FlatId=" + argFId;
            cmd = new SqlCommand(sSql, conn, tran);
            cmd.ExecuteNonQuery();

            sSql = "SELECT LandRate FROM dbo.FlatDetails WHERE FlatId=" + argFId;
            dLandAmt = GetLandAmt(sSql, conn, tran);
            sSql = "SELECT BaseAmt FROM dbo.FlatDetails WHERE FlatId=" + argFId;
            dBaseAmt = GetBaseAmt(sSql, conn, tran);

            sSql = "SELECT NetAmt FROM dbo.FlatDetails WHERE FlatId=" + argFId;
            dNetAmt = GetNetAmt(sSql, conn, tran);
            sSql = "SELECT AdvAmount FROM dbo.FlatDetails WHERE FlatId=" + argFId;
            dAdvAmt = GetAdvAmt(sSql, conn, tran);


            for (int t = 0; t < argPayTrans.Rows.Count; t++)
            {
                string nxtSchDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(argPayTrans.Rows[t]["SchDate"].ToString()));
                if (Convert.ToBoolean(argPayTrans.Rows[t]["DateAfter"]) == true)
                {
                    tr = 1;
                }
                else
                {
                    tr = 0;
                }

                sSql = "SELECT NetAmt FROM dbo.FlatDetails WHERE FlatId=" + argFId;
                dNetAmt = GetNetAmt(sSql, conn, tran);

                if (Convert.ToInt32(argPayTrans.Rows[t]["OtherCostId"].ToString()) == 0)
                {
                    sSql = "SELECT Amount FROM dbo.FlatOtherCost WHERE OtherCostId=" + Convert.ToInt32(argPayTrans.Rows[t]["OtherCostId"].ToString()) + " and  FlatId=" + argFId;
                    dOCAmt = GetOtherCost(sSql, conn, tran);
                    if (dNetAmt != 0 && dAdvAmt != 0)
                    {
                        sSql = "SELECT F.Amount FROM dbo.FlatOtherCost F INNER JOIN dbo.PaymentSchedule P" +
                            " ON F.OtherCostId=P.OtherCostId WHERE FlatId=" + argFId + " AND P.TypeId=" + argPayTrans.Rows[t]["TypeId"] + " AND F.CostCentreId=P.CostCentreId";
                        dOther = GetNetOC(sSql, conn, tran);

                        sSql = "SELECT OtherCostId FROM dbo.PaymentSchedule WHERE CostCentreId=" + argPayTrans.Rows[t]["CostCentreId"] + " AND OtherCostId=-1 AND TypeId=" + argPayTrans.Rows[t]["TypeId"] + "";
                        OCTransId = GetOCTransId(sSql, conn, tran);

                        if (OCTransId == -1)
                            dNetAmt = dNetAmt - dOther - dAdvAmt;
                        else if (OCTransId == 0 || OCTransId != 0)
                            dNetAmt = dNetAmt - dOther;
                    }
                    else
                    {
                        dNetAmt = 0;
                    }
                    dSchAmt = (Convert.ToDecimal(argPayTrans.Rows[t]["SchPercent"].ToString()) * dNetAmt) / 100;
                    sSql = String.Format("INSERT INTO dbo.PaymentScheduleFlat(TemplateId,TypeId,FlatId,FlatTypeId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,PreStageType,DateAfter,Duration,DurationType,SchPercent,Amount,NetAmount,PreStageTypeId) Values({0},{1},{2},{3},{4},'{5}','{6}',{7},{8},{9},'{10}','{11}',{12},{13},'{14}',{15},{16},{17},{18})", argPayTrans.Rows[t]["TemplateId"], argPayTrans.Rows[t]["TypeId"], argFId, argFTId, argPayTrans.Rows[t]["CostCentreId"], argPayTrans.Rows[t]["SchType"], argPayTrans.Rows[t]["Description"], argPayTrans.Rows[t]["SchDescId"], argPayTrans.Rows[t]["StageId"], argPayTrans.Rows[t]["OtherCostId"], nxtSchDate, argPayTrans.Rows[t]["PreStageType"], tr, argPayTrans.Rows[t]["Duration"], Convert.ToChar(argPayTrans.Rows[t]["DurationType"].ToString()), argPayTrans.Rows[t]["SchPercent"], dSchAmt, dSchAmt, argPayTrans.Rows[t]["PreStageTypeId"]);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();

                    if (Convert.ToInt32(argPayTrans.Rows[t]["OtherCostId"].ToString()) == 0)
                    {
                        dv = new DataView(argdtR) { RowFilter = String.Format("TemplateId={0}", argPayTrans.Rows[t]["TemplateId"]) };
                        dtRec = new DataTable();
                        dtRec = dv.ToTable();
                        decimal Amt = 0;
                        if (dtRec != null)
                        {
                            if (dtRec.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtRec.Rows.Count; i++)
                                {
                                    Amt = (dSchAmt * Convert.ToDecimal(dtRec.Rows[i]["Percentage"])) / 100;
                                    if (Convert.ToInt32(dtRec.Rows[i]["ReceiptTypeId"]) == 1)
                                    {
                                        dTotAdvAmt = dAdvAmt;
                                        if (Amt > dAdvAmt)
                                        {
                                            decimal dPer; bool Sel = false;
                                            //if (dTotAdvAmt >= dAdvAmt)
                                            //{
                                            //    dTotAdvAmt = 0; dPer = 0; Sel = false;
                                            //}
                                            //else
                                            //{
                                            dTotAdvAmt = dAdvAmt;
                                            dPer = dTotAdvAmt * 100 / dSchAmt; Sel = true;
                                            //}
                                            sSql = "INSERT INTO dbo.FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId)VALUES" +
                                                " (" + dtRec.Rows[i]["SchId"] + "," + dtRec.Rows[i]["TemplateId"] + "," + argFId + "," + dtRec.Rows[i]["ReceiptTypeId"] + "," +
                                            " " + dPer + "," + dTotAdvAmt + "," +
                                            " '" + Sel + "'," + dtRec.Rows[i]["OtherCostId"] + ")";
                                            cmd = new SqlCommand(sSql, conn, tran);
                                            cmd.ExecuteNonQuery();

                                            dAdvAmt = dAdvAmt - dTotAdvAmt;
                                        }
                                        else
                                        {
                                            sSql = "INSERT INTO dbo.FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId)VALUES" +
                                            " (" + dtRec.Rows[i]["SchId"] + "," + dtRec.Rows[i]["TemplateId"] + "," + argFId + "," + dtRec.Rows[i]["ReceiptTypeId"] + "," +
                                        " " + dtRec.Rows[i]["Percentage"] + "," + Amt + "," +
                                        " '" + dtRec.Rows[i]["Sel"] + "'," + dtRec.Rows[i]["OtherCostId"] + ")";
                                            cmd = new SqlCommand(sSql, conn, tran);
                                            cmd.ExecuteNonQuery();
                                            dAdvAmt = dTotAdvAmt - Amt;
                                            dTotAdvAmt = Amt;
                                        }
                                    }
                                    else if (Convert.ToInt32(dtRec.Rows[i]["ReceiptTypeId"]) == 2)
                                    {
                                        if (Amt > dLandAmt)
                                        {
                                            decimal dPer; bool Sel = false;
                                            if (dTotLAmt >= dLandAmt)
                                            {
                                                dTotLAmt = 0; dPer = 0; Sel = false;
                                            }
                                            else
                                            {
                                                dTotLAmt = dLandAmt;
                                                dPer = dTotLAmt * 100 / dSchAmt; Sel = true;
                                            }
                                            sSql = "INSERT INTO dbo.FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId)VALUES" +
                                                " (" + dtRec.Rows[i]["SchId"] + "," + dtRec.Rows[i]["TemplateId"] + "," + argFId + "," + dtRec.Rows[i]["ReceiptTypeId"] + "," +
                                            " " + dPer + "," + dTotLAmt + "," +
                                            " '" + Sel + "'," + dtRec.Rows[i]["OtherCostId"] + ")";
                                            cmd = new SqlCommand(sSql, conn, tran);
                                            cmd.ExecuteNonQuery();

                                            dLandAmt = dLandAmt - dTotLAmt;
                                        }
                                        else
                                        {
                                            sSql = "INSERT INTO dbo.FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId)VALUES" +
                                            " (" + dtRec.Rows[i]["SchId"] + "," + dtRec.Rows[i]["TemplateId"] + "," + argFId + "," + dtRec.Rows[i]["ReceiptTypeId"] + "," +
                                        " " + dtRec.Rows[i]["Percentage"] + "," + Amt + "," +
                                        " '" + dtRec.Rows[i]["Sel"] + "'," + dtRec.Rows[i]["OtherCostId"] + ")";
                                            cmd = new SqlCommand(sSql, conn, tran);
                                            cmd.ExecuteNonQuery();
                                            dLandAmt = dLandAmt - Amt;
                                            dTotLAmt = Amt;
                                        }
                                    }
                                    else if (Convert.ToInt32(dtRec.Rows[i]["ReceiptTypeId"]) == 3)
                                    {
                                        decimal dPer = 0; bool Sel = false;
                                        if (Amt > dBaseAmt)
                                        {
                                            if (dTotBAmt >= dBaseAmt)
                                            {
                                                dTotBAmt = 0; dPer = 0; Sel = false;
                                            }
                                            else
                                            {
                                                dTotBAmt = dBaseAmt;
                                                dPer = dTotBAmt * 100 / dSchAmt; Sel = true;
                                            }
                                        }
                                        else
                                        {
                                            dTotBAmt = Amt;
                                            if (dTotBAmt == 0)
                                            { dTotBAmt = 0; dPer = 0; Sel = false; }
                                            else { dPer = dTotBAmt * 100 / dSchAmt; Sel = true; }
                                        }
                                        //if (dPer>0)
                                        sSql = "INSERT INTO dbo.FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId)VALUES" +
                                        " (" + dtRec.Rows[i]["SchId"] + "," + dtRec.Rows[i]["TemplateId"] + "," + argFId + "," + dtRec.Rows[i]["ReceiptTypeId"] + "," +
                                        " " + dPer + "," + dTotBAmt + "," +
                                        " '" + Sel + "'," + dtRec.Rows[i]["OtherCostId"] + ")";
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();

                                        dBaseAmt = dBaseAmt - dTotBAmt;

                                    }
                                    else
                                    {
                                        decimal dper = 0; bool Sel = false;
                                        if (Convert.ToInt32(dtRec.Rows[i]["ReceiptTypeId"]) == 5)
                                        {
                                            //Amt = dSchAmt - (dAdvAmt + dLandAmt + dBaseAmt);
                                            Amt = dSchAmt - (dTotAdvAmt + dTotLAmt + dTotBAmt);
                                            if (Amt > 0)
                                            { dper = Amt * 100 / dSchAmt; Sel = true; }
                                            else { Amt = 0; dper = 0; Sel = false; }
                                        }
                                        else
                                        {
                                            Amt = 0; dper = 0; Sel = false;
                                        }
                                        sSql = "INSERT INTO dbo.FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId)VALUES" +
                                            " (" + dtRec.Rows[i]["SchId"] + "," + dtRec.Rows[i]["TemplateId"] + "," + argFId + "," + dtRec.Rows[i]["ReceiptTypeId"] + "," +
                                        " " + dper + "," + Amt + "," +
                                        " '" + Sel + "'," + dtRec.Rows[i]["OtherCostId"] + ")";
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        dBaseAmt = dBaseAmt - Amt;
                                    }
                                }
                            }
                        }
                    }
                    else if (Convert.ToInt32(argPayTrans.Rows[t]["OtherCostId"].ToString()) > 0)
                    {
                        sSql = "INSERT INTO dbo.FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId)VALUES" +
                                " (0, 0," + argFId + ",0,100," + dOCAmt + "," +
                            " 'true'," + argPayTrans.Rows[t]["OtherCostId"] + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                    }
                    else if (Convert.ToInt32(argPayTrans.Rows[t]["OtherCostId"].ToString()) == -1)
                    {
                        sSql = "INSERT INTO dbo.FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId)VALUES" +
                            " (0, 0," + argFId + ",0,100," + dAdvAmt + "," +
                        " 'true'," + argPayTrans.Rows[t]["OtherCostId"] + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                    }
                }

                else if (Convert.ToInt32(argPayTrans.Rows[t]["OtherCostId"].ToString()) != 0)
                {
                    sSql = "SELECT Amount FROM dbo.FlatOtherCost WHERE OtherCostId=" + Convert.ToInt32(argPayTrans.Rows[t]["OtherCostId"].ToString()) + " and  FlatId=" + argFId;
                    dNetAmt = GetOtherCost(sSql, conn, tran);
                    sSql = "SELECT AdvAmount FROM dbo.FlatDetails WHERE FlatId=" + argFId;
                    dAdvAmt = GetAdvAmt(sSql, conn, tran);
                    if (Convert.ToInt32(argPayTrans.Rows[t]["OtherCostId"].ToString()) == -1)
                    {
                        dSchAmt = dAdvAmt;
                        sSql = String.Format("INSERT INTO dbo.PaymentScheduleFlat(TemplateId,TypeId,FlatId,FlatTypeId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,PreStageType,DateAfter,Duration,DurationType,SchPercent,Amount,NetAmount,PreStageTypeId) Values({0},{1},{2},{3},{4},'{5}','{6}',{7},{8},{9},'{10}','{11}',{12},{13},'{14}',{15},{16},{17},{18})", argPayTrans.Rows[t]["TemplateId"], argPayTrans.Rows[t]["TypeId"], argFId, argFTId, argPayTrans.Rows[t]["CostCentreId"], argPayTrans.Rows[t]["SchType"], argPayTrans.Rows[t]["Description"], argPayTrans.Rows[t]["SchDescId"], argPayTrans.Rows[t]["StageId"], argPayTrans.Rows[t]["OtherCostId"], nxtSchDate, argPayTrans.Rows[t]["PreStageType"], tr, argPayTrans.Rows[t]["Duration"], Convert.ToChar(argPayTrans.Rows[t]["DurationType"].ToString()), argPayTrans.Rows[t]["SchPercent"], dSchAmt, dSchAmt, argPayTrans.Rows[t]["PreStageTypeId"]);
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();

                        sSql = "INSERT INTO dbo.FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId)VALUES" +
                            " (0, 0," + argFId + ",0,100," + dSchAmt + "," +
                        " 'true'," + argPayTrans.Rows[t]["OtherCostId"] + ")";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        dSchAmt = dNetAmt;
                        sSql = String.Format("INSERT INTO dbo.PaymentScheduleFlat(TemplateId,TypeId,FlatId,FlatTypeId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,PreStageType,DateAfter,Duration,DurationType,SchPercent,Amount,NetAmount,PreStageTypeId) Values({0},{1},{2},{3},{4},'{5}','{6}',{7},{8},{9},'{10}','{11}',{12},{13},'{14}',{15},{16},{17},{18})", argPayTrans.Rows[t]["TemplateId"], argPayTrans.Rows[t]["TypeId"], argFId, argFTId, argPayTrans.Rows[t]["CostCentreId"], argPayTrans.Rows[t]["SchType"], argPayTrans.Rows[t]["Description"], argPayTrans.Rows[t]["SchDescId"], argPayTrans.Rows[t]["StageId"], argPayTrans.Rows[t]["OtherCostId"], nxtSchDate, argPayTrans.Rows[t]["PreStageType"], tr, argPayTrans.Rows[t]["Duration"], Convert.ToChar(argPayTrans.Rows[t]["DurationType"].ToString()), argPayTrans.Rows[t]["SchPercent"], dSchAmt, dSchAmt, argPayTrans.Rows[t]["PreStageTypeId"]);
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        if (dSchAmt != 0)
                        {
                            sSql = "INSERT INTO dbo.FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId)VALUES" +
                                " (0, 0," + argFId + ",0,100," + dSchAmt + "," +
                            " 'true'," + argPayTrans.Rows[t]["OtherCostId"] + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    sSql = "SELECT NetAmt FROM dbo.FlatDetails WHERE FlatId=" + argFId;
                    dNetAmt = GetNetAmt(sSql, conn, tran);
                    sSql = "SELECT AdvAmount FROM dbo.FlatDetails WHERE FlatId=" + argFId;
                    dAdvAmt = GetAdvAmt(sSql, conn, tran);

                    dSchAmt = (Convert.ToDecimal(argPayTrans.Rows[t]["SchPercent"].ToString()) * (dNetAmt)) / 100;
                    sSql = String.Format("INSERT INTO dbo.PaymentScheduleFlat(TemplateId,TypeId,FlatId,FlatTypeId,CostCentreId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,PreStageType,DateAfter,Duration,DurationType,SchPercent,Amount,NetAmount,PreStageTypeId) Values({0},{1},{2},{3},{4},'{5}','{6}',{7},{8},{9},'{10}','{11}',{12},{13},'{14}',{15},{16},{17},{18},{19})", argPayTrans.Rows[t]["TemplateId"], argPayTrans.Rows[t]["TypeId"], argFId, argFTId, argPayTrans.Rows[t]["CostCentreId"], argPayTrans.Rows[t]["SchType"], argPayTrans.Rows[t]["Description"], argPayTrans.Rows[t]["SchDescId"], argPayTrans.Rows[t]["StageId"], argPayTrans.Rows[t]["OtherCostId"], nxtSchDate, argPayTrans.Rows[t]["PreStageType"], tr, argPayTrans.Rows[t]["Duration"], Convert.ToChar(argPayTrans.Rows[t]["DurationType"].ToString()), argPayTrans.Rows[t]["SchPercent"], dSchAmt, dSchAmt, dSchAmt, argPayTrans.Rows[t]["PreStageTypeId"]);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static decimal GetNetAmt(string argqry, SqlConnection conn, SqlTransaction tran)
        {
            decimal netamt = 0;
            SqlDataReader sdr;
            SqlCommand cmd;
            DataTable dt = new DataTable();

            try
            {
                cmd = new SqlCommand(argqry, conn, tran);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);

                if (dt.Rows.Count > 0)
                { netamt = Convert.ToDecimal(dt.Rows[0]["NetAmt"].ToString()); }
                dt.Dispose();
            }
            catch (SqlException e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return netamt;
        }

        public static decimal GetAdvAmt(string argqry, SqlConnection conn, SqlTransaction tran)
        {
            decimal advamt = 0;
            SqlDataReader sdr;
            SqlCommand cmd;
            DataTable dt = new DataTable();

            try
            {
                cmd = new SqlCommand(argqry, conn, tran);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);

                if (dt.Rows.Count > 0)
                { advamt = Convert.ToDecimal(dt.Rows[0]["AdvAmount"].ToString()); }
                dt.Dispose();
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return advamt;
        }

        public static decimal GetOtherCost(string argqry, SqlConnection conn, SqlTransaction tran)
        {

            decimal netamt = 0;
            SqlCommand sda;

            try
            {
                sda = new SqlCommand(argqry, conn, tran);
                netamt = Convert.ToDecimal(sda.ExecuteScalar());
            }
            catch (SqlException e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return netamt;
        }

        public static decimal GetNetOC(string argqry, SqlConnection conn, SqlTransaction tran)
        {
            decimal netOCamt = 0;
            SqlDataReader sdr;
            SqlCommand cmd;
            DataTable dt = new DataTable();

            try
            {
                cmd = new SqlCommand(argqry, conn, tran);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        netOCamt = Convert.ToDecimal(dt.Rows[i]["Amount"].ToString()) + netOCamt;
                    }
                }
                dt.Dispose();
            }
            catch (SqlException e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return netOCamt;
        }

        public static decimal GetLandAmt(string argqry, SqlConnection conn, SqlTransaction tran)
        {
            decimal Landamt = 0;
            SqlDataReader sdr;
            SqlCommand cmd;
            DataTable dt = new DataTable();

            try
            {
                cmd = new SqlCommand(argqry, conn, tran);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);

                if (dt.Rows.Count > 0)
                {
                    Landamt = Convert.ToDecimal(dt.Rows[0]["LandRate"].ToString());
                }
                dt.Dispose();
            }
            catch (SqlException e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return Landamt;
        }

        public static decimal GetBaseAmt(string argqry, SqlConnection conn, SqlTransaction tran)
        {
            decimal baseamt = 0;
            SqlDataReader sdr;
            SqlCommand cmd;
            DataTable dt = new DataTable();

            try
            {
                cmd = new SqlCommand(argqry, conn, tran);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                if (dt.Rows.Count > 0)
                {
                    baseamt = Convert.ToDecimal(dt.Rows[0]["BaseAmt"].ToString());
                }
                dt.Dispose();
            }
            catch (SqlException e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return baseamt;
        }

        public static int GetOCTransId(string argqry, SqlConnection conn, SqlTransaction tran)
        {
            int iOCId = 0;
            SqlDataReader sdr;
            SqlCommand cmd;
            DataTable dt = new DataTable();

            try
            {
                cmd = new SqlCommand(argqry, conn, tran);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                if (dt.Rows.Count > 0)
                {
                    iOCId = Convert.ToInt32(dt.Rows[0]["OtherCostId"].ToString());
                }
                dt.Dispose();
            }
            catch (SqlException e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return iOCId;
        }

        public static DataTable GetPaymentSchFlat(int argCCId, int argTId, SqlConnection conn, SqlTransaction tran)
        {
            DataTable dt = null;
            SqlDataReader sdr;
            SqlCommand cmd;
            string sSql = "";
            try
            {
                sSql = "SELECT * FROM dbo.PaymentSchedule A WHERE CostCentreId=" + argCCId + " AND TypeId=" + argTId + " ORDER BY Description ";
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(sdr);
                dt.Dispose();
            }
            catch (SqlException e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return dt;
        }

        public static DataTable GetCCReceipt(int argCCId, int argTId, SqlConnection conn, SqlTransaction tran)
        {
            DataTable dt = null;
            SqlDataReader sdr;
            SqlCommand cmd;
            string sSql = "";
            try
            {
                sSql = "SELECT * FROM dbo.CCReceiptType A WHERE CCId=" + argCCId + " AND SchTypeId=" + argTId + " ";
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(sdr);
                dt.Dispose();
            }
            catch (SqlException e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return dt;
        }

        public static DataTable GetPaymentScheduleFlat(int argCCId, int argFlatId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                //sSql = "Select TemplateId,PaymentSchId,Description,SchType,SchDate,SchPercent,Amount,NetAmount from dbo.PaymentScheduleFlat A WHERE CostCentreId=" + argCCId + " AND FlatId=" + argFlatId + "  ORDER BY SortOrder,SchDate";
                string sSql = "Select TemplateId,PaymentSchId,Description,SchType,SchDate,SchPercent,A.Amount,A.NetAmount," +
                                " D.LevelName,C.BlockName,0.000 CumulativeAmount " +
                                " from dbo.PaymentScheduleFlat A" +
                                " Inner Join FlatDetails B On A.FlatId=B.FlatId" +
                                " Inner Join BlockMaster C On C.BlockId=B.BlockId" +
                                " Inner Join LevelMaster D On D.LevelId=B.LevelId" +
                                " WHERE A.CostCentreId=" + argCCId + " AND A.FlatId=" + argFlatId + " AND A.NetAmount<>0"+
                                " ORDER BY A.SortOrder";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (SqlException e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable GetPaymentSchedulePlot(int argLandRegId, int argPlotId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                BsfGlobal.OpenCRMDB();
                //sSql = "Select TemplateId,PaymentSchId,Description,SchType,SchDate,SchPercent,Amount,NetAmount from dbo.PaymentScheduleFlat A WHERE CostCentreId=" + argCCId + " AND FlatId=" + argFlatId + "  ORDER BY SortOrder,SchDate";
                sSql = "Select TemplateId,PaymentSchId,Description,SchType,SchDate,SchPercent,Amount,A.NetAmount, " +
                        " 0.000 CumulativeAmount From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot A Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails B " +
                        " On A.PlotDetailsId=B.PlotDetailsId WHERE A.LandRegId=" + argLandRegId + " AND A.PlotDetailsId=" + argPlotId + " ORDER BY A.SortOrder,SchDate";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (SqlException e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static string GetPaySchFlatValidate(int argCCId, int argFlatId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt;
            SqlCommand cmd; SqlDataReader sdr;
            string sSql = "";
            int iCCId, iFlatTypeId, iPayTypeId = 0; decimal dBaseAmt = 0, dAdvAmt = 0, dLandAmt = 0, dOtherAmt = 0;
            decimal dConst = 0, dLand = 0, dOCost = 0, dBookAdv = 0; string sMsg = "";
            try
            {
                sSql = "Select FlatTypeId,CostCentreId,PayTypeId,BaseAmt,AdvAmount,LandRate from dbo.FlatDetails Where FlatId= " + argFlatId;
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                sdr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    iCCId = Convert.ToInt32(dt.Rows[0]["CostCentreId"].ToString());
                    iFlatTypeId = Convert.ToInt32(dt.Rows[0]["FlatTypeId"].ToString());
                    iPayTypeId = Convert.ToInt32(dt.Rows[0]["PayTypeId"].ToString());
                    dBaseAmt = Convert.ToDecimal(dt.Rows[0]["BaseAmt"].ToString());
                    dAdvAmt = Convert.ToDecimal(dt.Rows[0]["AdvAmount"].ToString());
                    dLandAmt = Convert.ToDecimal(dt.Rows[0]["LandRate"].ToString());
                }
                dt.Dispose();

                sSql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                   "Where FlatId = " + argFlatId + " and OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans Where PayTypeId=" + iPayTypeId + " and CostCentreId=" + argCCId + ")";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                sdr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0) { dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }
                dt.Dispose();


                //Land Value
                sSql = "Select Sum(Amount)Amount From dbo.FlatReceiptType Where FlatId= " + argFlatId + " And ReceiptTypeId=2 And SchType='R'";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                sdr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    dLand = Convert.ToDecimal(dt.Rows[0]["Amount"]);
                }
                dt.Dispose();

                //Construction Value
                sSql = "Select Sum(Amount)Amount From dbo.FlatReceiptType Where FlatId= " + argFlatId + " And ReceiptTypeId=3 And SchType='R'";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                sdr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    dConst = Convert.ToDecimal(dt.Rows[0]["Amount"]);
                }
                dt.Dispose();

                //OtherCost Value
                sSql = "Select Sum(Amount)Amount From dbo.FlatReceiptType Where FlatId= " + argFlatId + " And OtherCostId<>0 And SchType='O'";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                sdr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    dOCost = Convert.ToDecimal(dt.Rows[0]["Amount"]);
                }
                dt.Dispose();

                //Advance Value
                sSql = "Select Sum(Amount)Amount From dbo.FlatReceiptType Where FlatId= " + argFlatId + " And OtherCostId=0 And SchType='A'";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                sdr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    dBookAdv = Convert.ToDecimal(dt.Rows[0]["Amount"]);
                }
                dt.Dispose();

                if (dLandAmt != dLand)
                { sMsg = "Land"; }
                else if (dBaseAmt != dConst)
                { sMsg = "Base"; }
                else if (dAdvAmt != dBookAdv)
                { sMsg = "Advance"; }
                else if (dOtherAmt != dOCost)
                { sMsg = "OtherCost"; }

            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return sMsg;
        }

        public static DataTable GetPayRec(int argSchId, int argFlatId, int argRId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT Amount FROM dbo.FlatReceiptType WHERE FlatId=" + argFlatId + " AND ReceiptTypeId=" + argRId + " AND SchId NOT IN(" + argSchId + ")";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (SqlException e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable GetPayOCRec(int argSchId, int argFlatId, int argRId, int argOCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT OtherCostId,Amount FROM dbo.FlatReceiptType WHERE FlatId=" + argFlatId + " AND ReceiptTypeId=" + argRId + " AND OtherCostId=" + argOCId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (SqlException e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable PopulatePaySchTemp(int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = String.Format("select * from dbo.PaymentSchedule where CostCentreId={0} ", argCCId);
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

        public static DataTable Payment(int argCCId, int argTId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = String.Format("Select TemplateId,SchType,Description,SchDate,FlatTypeId,BlockId, " +
                                     " Case When SchPercent=0 then null else SchPercent End SchPercent from dbo.PaymentSchedule " +
                                     " Where CostCentreId={0} AND TypeId={1} ORDER BY SortOrder,SchDate", argCCId, argTId);
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

        public static bool CheckPaymentScheduleUsed(int argId)
        {
            bool bAns = false;

            DataTable dt;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select TypeId from dbo.PaymentSchedule where TypeId= " + argId;
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0) { bAns = true; }
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

            return bAns;
        }

        public static bool CheckPaymentScheduleDesUsed(int argId)
        {
            bool bAns = false;

            DataTable dt;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select SchDescId from dbo.PaymentSchedule Where SchDescId= " + argId;
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0) { bAns = true; }
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

            return bAns;
        }

        public static bool CheckUp(int argTempId, int argCheckId)
        {
            bool bAns = false;
            SqlDataAdapter sda;
            try
            {
                BsfGlobal.OpenCRMDB();
                string sSql = "Select TemplateId From dbo.PaymentSchedule Where TemplateId=" + argTempId + " and PreStageTypeId= " + argCheckId;
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0) { bAns = true; }
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

            return bAns;
        }

        public static DataTable GetPreviousStages(int argTempId, int argCCId, int argPayTypeId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select TemplateId,Description,SchDate from dbo.PaymentSchedule " +
                       "Where CostCentreId=" + argCCId + " and TypeId= " + argPayTypeId + " and " +
                       "SortOrder < (Select SortOrder from dbo.PaymentSchedule Where TemplateId= " + argTempId + ") Order by SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();

                sSql = "Select StartDate,EndDate from dbo.ProjectInfo Where CostCentreId= " + argCCId;
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtT = new DataTable();
                sda.Fill(dtT);
                sda.Dispose();

                DataRow dr;

                dr = dt.NewRow();
                dr["TemplateId"] = -1;
                dr["Description"] = "Finalize Date";
                dt.Rows.InsertAt(dr, 0);

                dr = dt.NewRow();
                dr["TemplateId"] = -2;
                dr["Description"] = "Start Date";
                if (dtT.Rows.Count > 0)
                {
                    if (Information.IsDBNull(dtT.Rows[0]["StartDate"]) == false)
                    {
                        dr["SchDate"] = dtT.Rows[0]["StartDate"];
                    }
                }

                dt.Rows.InsertAt(dr, 1);

                dr = dt.NewRow();
                dr["TemplateId"] = -3;
                dr["Description"] = "End Date";
                if (dtT.Rows.Count > 0)
                {
                    if (Information.IsDBNull(dtT.Rows[0]["EndDate"]) == false)
                    {
                        dr["SchDate"] = dtT.Rows[0]["EndDate"];
                    }
                }
                dt.Rows.InsertAt(dr, 2);


                dr = dt.NewRow();
                dr["TemplateId"] = 0;
                dr["Description"] = "None";
                dt.Rows.InsertAt(dr, 0);


                dtT.Dispose();
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

        public static DataTable GetFlatPreviousStages(int argTempId, int argCCId, int argPayTypeId, int argFlatId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "Select TemplateId,Description,SchDate from dbo.PaymentScheduleFlat " +
                               "Where CostCentreId=" + argCCId + " and FlatId= " + argFlatId + " and " +
                               "SortOrder<(Select SortOrder from dbo.PaymentScheduleFlat Where PaymentSchId= " + argPayTypeId + ") Order by SortOrder";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();

                DataRow dr = dt.NewRow();
                dr["TemplateId"] = -1;
                dr["Description"] = "Finalize Date";
                dr["SchDate"] = DBNull.Value;
                dt.Rows.InsertAt(dr, 0);

                sSql = "Select FinaliseDate from dbo.BuyerDetail Where CostCentreId= " + argCCId + " AND FlatId=" + argFlatId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtFinDate = new DataTable();
                sda.Fill(dtFinDate);
                sda.Dispose();
                if (dtFinDate.Rows.Count > 0)
                {
                    if (Information.IsDBNull(dtFinDate.Rows[0]["FinaliseDate"]) == false)
                    {
                        dr["SchDate"] = dtFinDate.Rows[0]["FinaliseDate"];
                    }
                }
                dtFinDate.Dispose();

                dr = dt.NewRow();
                dr["TemplateId"] = -2;
                dr["Description"] = "Start Date";
                dr["SchDate"] = DBNull.Value;
                dt.Rows.InsertAt(dr, 1);

                sSql = "Select StartDate,EndDate from dbo.ProjectInfo Where CostCentreId= " + argCCId;
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtT = new DataTable();
                sda.Fill(dtT);
                sda.Dispose();
                if (dtT.Rows.Count > 0)
                {
                    if (Information.IsDBNull(dtT.Rows[0]["StartDate"]) == false)
                    {
                        dr["SchDate"] = dtT.Rows[0]["StartDate"];
                    }
                }

                dr = dt.NewRow();
                dr["TemplateId"] = -3;
                dr["Description"] = "End Date";
                dr["SchDate"] = DBNull.Value;
                dt.Rows.InsertAt(dr, 2);

                if (dtT.Rows.Count > 0)
                {
                    if (Information.IsDBNull(dtT.Rows[0]["EndDate"]) == false)
                    {
                        dr["SchDate"] = dtT.Rows[0]["EndDate"];
                    }
                }
                dtT.Dispose();

                dr = dt.NewRow();
                dr["TemplateId"] = 0;
                dr["Description"] = "None";
                dr["SchDate"] = DBNull.Value;
                dt.Rows.InsertAt(dr, 0);
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

        public static bool CheckDown(int argTempId, int argCheckId)
        {
            bool bAns = false;
            SqlDataAdapter sda;
            try
            {
                BsfGlobal.OpenCRMDB();
                string sSql = "Select TemplateId From dbo.PaymentSchedule Where TemplateId=" + argCheckId + " and PreStageTypeId= " + argTempId;
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0) { bAns = true; }
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
            return bAns;
        }

        public static bool CheckNetAmt(int argFlatId, int argPaySchId, DataTable dtAmt)
        {
            bool bAns = false;
            SqlDataAdapter sda;
            try
            {
                BsfGlobal.OpenCRMDB();
                string sSql = "Select A.QualifierId,Sum(A.Amount) Amount,B.Amount TotAmount From PaySchTaxFlat A" +
                    " Inner Join FlatTax B On A.QualifierId=B.QualifierId And A.FlatId=B.FlatId" +
                    " Where A.FlatId=" + argFlatId + " And A.PaymentSchId Not In(" + argPaySchId + ") Group By A.QualifierId,B.Amount";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dtAmt.Rows.Count; j++)
                    {
                        if (Convert.ToInt32(dt.Rows[i]["QualifierId"]) == Convert.ToInt32(dtAmt.Rows[j]["QualifierId"]))
                        {
                            decimal dFlatAmt = Convert.ToInt32(dt.Rows[i]["Amount"]);
                            decimal dNAmt = Convert.ToInt32(dtAmt.Rows[j]["Amount"]);
                            decimal dAmt = dFlatAmt + dNAmt;
                            if (dAmt > Convert.ToInt32(dt.Rows[i]["TotAmount"])) { bAns = true; }
                        }
                    }
                }

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

            return bAns;
        }

        public static void UpdateSortOrder(DataTable argDt)
        {
            string sSql = "";
            SqlCommand cmd;
            BsfGlobal.OpenCRMDB();
            try
            {
                int iTempId = 0;
                int iSort = 0;
                for (int i = 0; i < argDt.Rows.Count; i++)
                {
                    iTempId = Convert.ToInt32(argDt.Rows[i]["TemplateId"].ToString());
                    iSort = i + 1;
                    sSql = "Update dbo.PaymentSchedule Set SortOrder= " + iSort + " Where TemplateId = " + iTempId;
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

        public static void UpdateReceiptTypeSortOrder(DataTable argDt)
        {
            string sSql = "";
            SqlCommand cmd;
            BsfGlobal.OpenCRMDB();
            try
            {
                int iTempId = 0;
                int iSort = 0;
                for (int i = 0; i < argDt.Rows.Count; i++)
                {
                    iTempId = Convert.ToInt32(argDt.Rows[i]["TransId"].ToString());
                    iSort = i + 1;
                    sSql = "Update dbo.ReceiptTypeOrder Set SortOrder= " + iSort + " Where TransId = " + iTempId;
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

        public static DataTable GetPayScheduleDate(int argTempId)
        {

            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select PreStageTypeId,DateAfter,DurationType,Duration,SchDate from dbo.PaymentSchedule Where TemplateId= " + argTempId;
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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

        public static DataTable GetFlatPayScheduleDate(int argPaySchId)
        {

            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select PreStageTypeId,DateAfter,DurationType,Duration,SchDate from dbo.PaymentScheduleFlat Where PaymentSchId= " + argPaySchId;
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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

        public static void UpdatePayScheduleDate(int argTempId, int argTypeId, bool argAfter, string argDurType, int argduration, string argDate, int argCCId, int argPayTypeId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                int i = 0;
                if (argAfter == true) { i = 1; }

                string sSql = "";
                if (argDate == null)
                {
                    sSql = "Update dbo.PaymentSchedule Set PreStageTypeId = " + argTypeId + ",DateAfter = " + i + ",DurationType = '" + argDurType + "', " +
                           " Duration= " + argduration + ",SchDate = null " +
                           "Where TemplateId = " + argTempId;
                }
                else
                {
                    sSql = "Update dbo.PaymentSchedule Set PreStageTypeId = " + argTypeId + ",DateAfter = " + i + ",DurationType = '" + argDurType + "',Duration= " + argduration + ", SchDate = '" + argDate + "' " +
                           "Where TemplateId = " + argTempId;
                }
                SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                //Update SchDate

                sSql = "Select StartDate,EndDate from dbo.ProjectInfo Where CostCentreId= " + argCCId;
                cmd = new SqlCommand(sSql, conn, tran);
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                DateTime StartDate = DateTime.MinValue;
                DateTime EndDate = DateTime.MinValue;
                if (dt.Rows.Count > 0)
                {
                    StartDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["StartDate"], CommFun.datatypes.VarTypeDate));
                    EndDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["EndDate"], CommFun.datatypes.VarTypeDate));
                }

                sSql = "Select SortOrder From dbo.PaymentSchedule Where CostCentreId=" + argCCId + " And TypeId=" + argPayTypeId + " And TemplateId=" + argTempId + "";
                cmd = new SqlCommand(sSql, conn, tran);
                dt = new DataTable();
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                int iSortOrder = 0;
                if (dt.Rows.Count > 0) { iSortOrder = Convert.ToInt32(dt.Rows[0]["SortOrder"]); }

                sSql = "Select * From dbo.PaymentSchedule Where CostCentreId=" + argCCId + " And TypeId=" + argPayTypeId + " And SortOrder>" + iSortOrder + " Order By SortOrder";
                cmd = new SqlCommand(sSql, conn, tran);
                dt = new DataTable();
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                dt.Dispose();

                int ipre = 0;
                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        int iTemplateId = Convert.ToInt32(dt.Rows[j]["TemplateId"]);
                        ipre = Convert.ToInt32(dt.Rows[j]["PreStageTypeId"]);
                        int iDateAfter = Convert.ToInt32(dt.Rows[j]["DateAfter"]);
                        int iDuration = Convert.ToInt32(dt.Rows[j]["Duration"]);
                        string sDurType = dt.Rows[j]["DurationType"].ToString();

                        if (ipre == -1) { } else if (ipre == -2) { } else if (ipre == -3) { } else if (ipre == 0) { } else { iTemplateId = ipre; }

                        sSql = "Select SchDate From dbo.PaymentSchedule Where CostCentreId=" + argCCId + " And TypeId=" + argPayTypeId + "" +
                               " And TemplateId=" + iTemplateId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        DataTable dtDate = new DataTable();
                        sdr = cmd.ExecuteReader();
                        dtDate.Load(sdr);
                        sdr.Close();
                        cmd.Dispose();

                        DateTime SchDate;
                        if (ipre == -1)
                            SchDate = DateTime.MinValue;
                        else if (ipre == -2)
                            SchDate = StartDate;
                        else if (ipre == -3)
                            SchDate = EndDate;
                        else
                            SchDate = Convert.ToDateTime(CommFun.IsNullCheck(dtDate.Rows[0]["SchDate"], CommFun.datatypes.VarTypeDate));

                        if (sDurType == "D" && SchDate != DateTime.MinValue)
                        {
                            if (iDateAfter == 0)
                                SchDate = SchDate.AddDays(iDuration);
                            else
                                SchDate = SchDate.AddDays(-iDuration);
                        }
                        else if (sDurType == "M" && SchDate != DateTime.MinValue)
                        {
                            if (iDateAfter == 0)
                                SchDate = SchDate.AddMonths(iDuration);
                            else
                                SchDate = SchDate.AddDays(-iDuration);
                        }

                        sSql = "Update dbo.PaymentSchedule Set SchDate=@SchDate Where TemplateId=" + dt.Rows[j]["TemplateId"] + " And TypeId=" + argPayTypeId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        SqlParameter parameter = new SqlParameter() { ParameterName = "@SchDate", DbType = DbType.DateTime };
                        if (SchDate == DateTime.MinValue)
                            parameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            parameter.Value = SchDate;
                        cmd.Parameters.Add(parameter);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    dt.Dispose();
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

        public static void UpdateTemplatePer(int argId, decimal argPer)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            try
            {
                sSql = "Update dbo.PaymentSchedule Set SchPercent = " + argPer + " Where TemplateId = " + argId;
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
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

        public static bool CheckTemplateUsed(int argId)
        {
            bool bAns = false;

            DataTable dt;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select TemplateId From dbo.PaymentScheduleFlat Where TemplateId= " + argId + " " +
                       "Union All " +
                       "Select TemplateId from dbo.PaymentSchedule Where PreStageTypeId = " + argId;
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0) { bAns = true; }
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

            return bAns;
        }

        public static DataTable PaySchType(int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql;
            BsfGlobal.OpenCRMDB();

            try
            {
                sSql = "SELECT TypeId,TypeName FROM dbo.PaySchType WHERE TypeId IN (SELECT DISTINCT TypeId FROM dbo.PaymentSchedule Where CostCentreId = " + argCCId + ")";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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

        public static void DeletePay(int argTempId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = "DELETE FROM dbo.PaymentSchedule WHERE TemplateId=" + argTempId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();

                    sSql = "DELETE FROM dbo.CCReceiptType WHERE TemplateId=" + argTempId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();

                    tran.Commit();
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

        internal static DataTable GetStagesBlock(int argBlockId, int argCostCentreId)
        {
            DataTable dtStage = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select A.StageName, A.StageId, Convert(bit,0,0) as Sel from dbo.Stages A " +
                       "Where StageId not in (Select StageId from dbo.BlockStageTrans Where BlockId = " + argBlockId +
                       " AND A.CostCentreId= " + argCostCentreId + ") AND A.CostCentreId= " + argCostCentreId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dtStage = new DataTable();
                sda.Fill(dtStage);
                dtStage.Dispose();
            }

            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtStage;
        }

        public static void UpdateReceiptTypeM(int argTempId, DataTable argR, DataTable dtTax)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                string sSql = "";
                int iRowId = 0;

                sSql = "Delete From dbo.CCReceiptType Where TemplateId= " + argTempId;
                cmd = new SqlCommand(sSql, conn, trans);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                for (int i = 0; i < argR.Rows.Count; i++)
                {
                    iRowId = i + 1;
                    sSql = "Insert into dbo.CCReceiptType(TemplateId,ReceiptTypeId,Percentage,OtherCostId,SchType,SortOrder) " +
                            "Values(" + argTempId + ", " + Convert.ToInt32(argR.Rows[i]["ReceiptTypeId"].ToString()) + "," +
                            "" + Convert.ToDecimal(argR.Rows[i]["Percentage"].ToString()) + "," + Convert.ToInt32(argR.Rows[i]["OtherCostId"].ToString()) + ", " +
                            "'" + argR.Rows[i]["SchType"].ToString() + "'," + iRowId + ") ";

                    cmd = new SqlCommand(sSql, conn, trans);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                sSql = "Delete From dbo.PaySchTax Where TemplateId= " + argTempId;
                cmd = new SqlCommand(sSql, conn, trans);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                for (int i = 0; i < dtTax.Rows.Count; i++)
                {
                    sSql = "Insert Into dbo.PaySchTax(TemplateId,QualifierId,Percentage,Sel) " +
                            "Values(" + argTempId + ", " + dtTax.Rows[i]["ReceiptTypeId"] + "," +
                            "" + dtTax.Rows[i]["Percentage"].ToString() + ",'" + dtTax.Rows[i]["Sel"] + "') ";
                    cmd = new SqlCommand(sSql, conn, trans);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

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
                conn.Dispose();
            }
        }

        public static void UpdateCCQualifier(int argCCId, DataTable argR, DataTable argQ)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                string sSql = "Delete From dbo.CCReceiptQualifier Where CostCentreId= " + argCCId;
                SqlCommand cmd = new SqlCommand(sSql, conn, trans);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                for (int i = 0; i < argR.Rows.Count; i++)
                {
                    string sType = argR.Rows[i]["SchType"].ToString();
                    int iRId = Convert.ToInt32(argR.Rows[i]["ReceiptTypeId"].ToString());
                    int iOId = Convert.ToInt32(argR.Rows[i]["OtherCostId"].ToString());

                    DataView dv = new DataView(argQ);
                    dv.RowFilter = "RowId = " + i;
                    if (dv.ToTable() != null)
                    {
                        DataTable dTQ = new DataTable();
                        dTQ = dv.ToTable();
                        for (int j = 0; j < dTQ.Rows.Count; j++)
                        {
                            sSql = "Insert into dbo.CCReceiptQualifier(CostCentreId,SchType,ReceiptTypeId,OtherCostId,QualifierId,Expression,ExpPer, " +
                                    " Add_Less_Flag,SurCharge,EDCess,NetPer,HEDPer,TaxablePer,TaxableValue) Values(" + argCCId + ",'" + sType + "'," + iRId +
                                    "," + iOId + "," + Convert.ToInt32(CommFun.IsNullCheck(dTQ.Rows[j]["QualifierId"], CommFun.datatypes.vartypenumeric)) +
                                    ",'" + CommFun.IsNullCheck(dTQ.Rows[j]["Expression"], CommFun.datatypes.vartypestring).ToString() + "'," +
                                    Convert.ToDecimal(CommFun.IsNullCheck(dTQ.Rows[j]["ExpPer"], CommFun.datatypes.vartypenumeric)) +
                                    ",'" + CommFun.IsNullCheck(dTQ.Rows[j]["Add_Less_Flag"], CommFun.datatypes.vartypestring).ToString() + "'," +
                                    Convert.ToDecimal(CommFun.IsNullCheck(dTQ.Rows[j]["SurCharge"], CommFun.datatypes.vartypenumeric)) +
                                    "," + Convert.ToDecimal(CommFun.IsNullCheck(dTQ.Rows[j]["EDCess"], CommFun.datatypes.vartypenumeric)) + "," +
                                    Convert.ToDecimal(CommFun.IsNullCheck(dTQ.Rows[j]["Net"], CommFun.datatypes.vartypenumeric)) +
                                    "," + Convert.ToDecimal(CommFun.IsNullCheck(dTQ.Rows[j]["HEDCess"], CommFun.datatypes.vartypenumeric)) + "," +
                                    Convert.ToDecimal(CommFun.IsNullCheck(dTQ.Rows[j]["Taxable"], CommFun.datatypes.vartypenumeric)) +
                                    "," + Convert.ToDecimal(CommFun.IsNullCheck(dTQ.Rows[j]["TaxableValue"], CommFun.datatypes.vartypenumeric)) + ")";
                            cmd = new SqlCommand(sSql, conn, trans);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                }

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
                conn.Dispose();
            }
        }

        public static void UpdateReceiptTypeF(int argTempId, DataTable argR, DataTable argQ, decimal argNetAmt, int argFlatId, DataTable dtTax)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction trans = conn.BeginTransaction();
            try
            {

                int iSchId = 0;
                string sSql = "";
                string sType = "";
                DataTable dTQ;
                DataView dv;
                //decimal dNetAmount = 0;

                sSql = "Delete From dbo.FlatReceiptQualifier Where SchId in (Select SchId from dbo.FlatReceiptType Where PaymentSchId= " + argTempId + ")";
                cmd = new SqlCommand(sSql, conn, trans);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "Delete From dbo.FlatReceiptType Where PaymentSchId= " + argTempId;
                cmd = new SqlCommand(sSql, conn, trans);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                for (int i = 0; i < argR.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(argR.Rows[i]["Sel"]) == true)
                    {
                        sSql = "Insert into dbo.FlatReceiptType(PaymentSchId,FlatId,ReceiptTypeId,Percentage,OtherCostId,SchType,Amount,NetAmount) " +
                                "Values(" + argTempId + ", " + argFlatId + "," + Convert.ToInt32(argR.Rows[i]["ReceiptTypeId"].ToString()) + "," +
                                "" + Convert.ToDecimal(argR.Rows[i]["Percentage"].ToString()) + "," + Convert.ToInt32(argR.Rows[i]["OtherCostId"].ToString()) + ", " +
                                "'" + argR.Rows[i]["SchType"].ToString() + "'," + Convert.ToDecimal(argR.Rows[i]["Amount"].ToString()) + "," + Convert.ToDecimal(argR.Rows[i]["NetAmount"].ToString()) + ") SELECT SCOPE_IDENTITY();";

                        cmd = new SqlCommand(sSql, conn, trans);
                        iSchId = int.Parse(cmd.ExecuteScalar().ToString());
                        cmd.Dispose();


                        sType = argR.Rows[i]["SchType"].ToString();
                        int iRId = Convert.ToInt32(argR.Rows[i]["ReceiptTypeId"].ToString());
                        int iOId = Convert.ToInt32(argR.Rows[i]["OtherCostId"].ToString());
                        //int iQId = Convert.ToInt32(argR.Rows[i]["QualifierId"].ToString());

                        if (argR.Rows.Count > 1)
                        {
                            dv = new DataView(argQ);

                            dv.RowFilter = "RowId = " + i;
                        }
                        else
                        {
                            dv = new DataView(argQ);
                        }

                        if (dv.ToTable() != null)
                        {
                            dTQ = new DataTable();
                            dTQ = dv.ToTable();

                            for (int j = 0; j < dTQ.Rows.Count; j++)
                            {
                                sSql = "Insert into dbo.FlatReceiptQualifier(SchId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,ExpValue,ExpPerValue,SurValue,EDValue,Amount,NetPer,HEDPer,TaxablePer,TaxableValue) " +
                                       "Values (" + iSchId + "," + Convert.ToInt32(dTQ.Rows[j]["QualifierId"]) + "," +
                                       "'" + dTQ.Rows[j]["Expression"].ToString() + "'," + Convert.ToDecimal(CommFun.IsNullCheck(dTQ.Rows[j]["ExpPer"], CommFun.datatypes.vartypenumeric)) + "," +
                                       "'" + dTQ.Rows[j]["Add_Less_Flag"].ToString() + "'," + Convert.ToDecimal(CommFun.IsNullCheck(dTQ.Rows[j]["SurCharge"], CommFun.datatypes.vartypenumeric)) + "," +
                                       "" + Convert.ToDecimal(CommFun.IsNullCheck(dTQ.Rows[j]["EDCess"], CommFun.datatypes.vartypenumeric)) + "," +
                                       "" + Convert.ToDecimal(CommFun.IsNullCheck(dTQ.Rows[j]["ExpValue"], CommFun.datatypes.vartypenumeric)) + "," +
                                       "" + Convert.ToDecimal(CommFun.IsNullCheck(dTQ.Rows[j]["ExpPerValue"], CommFun.datatypes.vartypenumeric)) + "," +
                                       "" + Convert.ToDecimal(CommFun.IsNullCheck(dTQ.Rows[j]["SurValue"], CommFun.datatypes.vartypenumeric)) + "," +
                                       "" + Convert.ToDecimal(CommFun.IsNullCheck(dTQ.Rows[j]["EDValue"], CommFun.datatypes.vartypenumeric)) + "," +
                                       "" + Convert.ToDecimal(CommFun.IsNullCheck(dTQ.Rows[j]["Amount"], CommFun.datatypes.vartypenumeric)) + "," +
                                        " " + Convert.ToDecimal(CommFun.IsNullCheck(dTQ.Rows[j]["NetPer"], CommFun.datatypes.vartypenumeric)) + "," +
                                        " " + Convert.ToDecimal(CommFun.IsNullCheck(dTQ.Rows[j]["HEDCess"], CommFun.datatypes.vartypenumeric)) + "," +
                                        " " + Convert.ToDecimal(CommFun.IsNullCheck(dTQ.Rows[j]["TaxablePer"], CommFun.datatypes.vartypenumeric)) + "," +
                                        " " + Convert.ToDecimal(CommFun.IsNullCheck(dTQ.Rows[j]["TaxableValue"], CommFun.datatypes.vartypenumeric)) + ")";
                                cmd = new SqlCommand(sSql, conn, trans);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }

                        sSql = "Update dbo.PaymentScheduleFlat Set NetAmount = " + argNetAmt + " Where PaymentSchId= " + argTempId;
                        cmd = new SqlCommand(sSql, conn, trans);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Delete From dbo.PaySchTaxFlat Where PaymentSchId= " + argTempId + " And FlatId=" + argFlatId + "";
                        cmd = new SqlCommand(sSql, conn, trans);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        for (int j = 0; j < dtTax.Rows.Count; j++)
                        {
                            sSql = "Insert Into dbo.PaySchTaxFlat(PaymentSchId,FlatId,QualifierId,Percentage,Amount,Sel) " +
                                   "Values (" + argTempId + "," + argFlatId + "," + dtTax.Rows[j]["ReceiptTypeId"] + "," +
                                   "" + dtTax.Rows[j]["Percentage"] + "," + dtTax.Rows[j]["Amount"] + ",'" + dtTax.Rows[j]["Sel"] + "')";
                            cmd = new SqlCommand(sSql, conn, trans);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            //dNetAmount = dNetAmount + Convert.ToDecimal(dtTax.Rows[j]["Amount"]);
                        }

                        //argNetAmt = argNetAmt + dNetAmount;
                        //sSql = "Update dbo.PaymentScheduleFlat Set NetAmount = " + argNetAmt + " Where PaymentSchId= " + argTempId;
                        //cmd = new SqlCommand(sSql, conn, trans);
                        //cmd.ExecuteNonQuery();
                        //cmd.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                trans.Rollback();
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                trans.Commit();
                conn.Close();
                conn.Dispose();
            }
        }

        public static DataTable GetReceiptTypeOrder(int argPayTypeId, int argCCId)
        {
            DataTable dt = new DataTable();
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd;
            bool bAns = false;
            try
            {
                //sSql = "Select G.TransId,G.Id,G.ReceiptType,G.SchType,G.SortOrder from " +
                //       "(Select A.TransId,A.ReceiptTypeId Id,B.ReceiptTypeName ReceiptType, A.SchType,A.SortOrder From dbo.ReceiptTypeOrder A " +
                //       "Inner Join dbo.ReceiptType B on A.ReceiptTypeId= B.ReceiptTypeId " +
                //       "Where A.PayTypeId=" + argPayTypeId + " and A.CostCentreId= " + argCCId + " " +
                //       "Union All " +
                //       "Select A.TransId,A.OtherCostId Id,B.OtherCostName ReceiptType, A.SchType,A.SortOrder From dbo.ReceiptTypeOrder A " +
                //       "Inner Join dbo.OtherCostMaster B on A.OtherCostId= B.OtherCostId " +
                //       "Where A.PayTypeId=" + argPayTypeId + " and A.CostCentreId= " + argCCId + ") G Order by G.SortOrder";
                sSql = "Select G.TransId,G.Id,G.ReceiptType,G.SchType,G.SortOrder from ( " +
                        " Select A.TransId,A.ReceiptTypeId Id,B.ReceiptTypeName ReceiptType, A.SchType,A.SortOrder From dbo.ReceiptTypeOrder A " +
                        " Inner Join dbo.ReceiptType B on A.ReceiptTypeId= B.ReceiptTypeId Where A.PayTypeId=" + argPayTypeId + " and A.CostCentreId= " + argCCId + " " +
                        " Union All " +
                        " Select A.TransId,A.OtherCostId Id,B.OtherCostName ReceiptType, A.SchType,A.SortOrder From dbo.ReceiptTypeOrder A " +
                        " Inner Join dbo.OtherCostMaster B on A.OtherCostId= B.OtherCostId Inner Join dbo.CCOtherCost CO On " +
                        " CO.OtherCostId=A.OtherCostId And A.CostCentreId=CO.CostCentreId Where A.PayTypeId=" + argPayTypeId + " and A.CostCentreId= " + argCCId + " " +
                        " ) G Order by G.SortOrder";
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                if (dt.Rows.Count > 0) { bAns = true; }
                if (bAns == false)
                {
                    dt.Dispose();

                    sSql = "Select TemplateId from dbo.PaymentSchedule Where SchType='A' and TypeId=" + argPayTypeId + " and CostCentreId= " + argCCId;
                    bool bAdvance = false;
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    DataTable dtT = new DataTable();
                    da.Fill(dtT);
                    if (dtT.Rows.Count > 0) { bAdvance = true; }
                    da.Dispose();
                    dtT.Dispose();


                    if (bAdvance == false)
                    {
                        sSql = "Insert into dbo.ReceiptTypeOrder(PayTypeId,CostCentreId,ReceiptTypeId,OtherCostId,SchType,SortOrder) " +
                               "Values(" + argPayTypeId + "," + argCCId + ",1,0,'A',1)";
                        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    sSql = "Insert into dbo.ReceiptTypeOrder(PayTypeId,CostCentreId,ReceiptTypeId,OtherCostId,SchType) " +
                           "Select " + argPayTypeId + "," + argCCId + ",ReceiptTypeId,0,'R' from dbo.ReceiptType Where ReceiptTypeId <>1";
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Insert into dbo.ReceiptTypeOrder(PayTypeId,CostCentreId,ReceiptTypeId,OtherCostId,SchType) " +
                           "Select " + argPayTypeId + "," + argCCId + ",0,OtherCostId,'O' from dbo.OtherCostSetupTrans Where CostCentreId=" + argCCId + " and PayTypeId=" + argPayTypeId;
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Select G.TransId,G.Id,G.ReceiptType,G.SchType,G.SortOrder from " +
                      "(Select A.TransId,A.ReceiptTypeId Id,B.ReceiptTypeName ReceiptType, A.SchType,A.SortOrder From dbo.ReceiptTypeOrder A " +
                      "Inner Join dbo.ReceiptType B on A.ReceiptTypeId= B.ReceiptTypeId " +
                      "Where A.PayTypeId=" + argPayTypeId + " and A.CostCentreId= " + argCCId + " " +
                      "Union All " +
                      "Select A.TransId,A.OtherCostId Id,B.OtherCostName ReceiptType, A.SchType,A.SortOrder From dbo.ReceiptTypeOrder A " +
                      "Inner Join dbo.OtherCostMaster B on A.OtherCostId= B.OtherCostId " +
                      "Where A.PayTypeId=" + argPayTypeId + " and A.CostCentreId= " + argCCId + ") G Order by G.SortOrder";
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    dt = new DataTable();
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

        public static bool GetAdvFound(int argPayTypeId, int argCCId)
        {
            SqlDataAdapter da;
            bool bAdvance = false;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select TemplateId from dbo.PaymentSchedule Where SchType='A' and TypeId=" + argPayTypeId + " and CostCentreId= " + argCCId;

                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtT = new DataTable();
                da.Fill(dtT);
                if (dtT.Rows.Count > 0) { bAdvance = true; }
                da.Dispose();
                dtT.Dispose();

            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return bAdvance;
        }

        public static decimal GetAdvance(int argFlatId, int argPaySchId)
        {
            SqlDataAdapter da;
            decimal dAdvAmt = 0;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select Percentage From dbo.FlatReceiptType Where SchType='A' And FlatId=" + argFlatId + " " +
                " And PaymentSchId Not In (" + argPaySchId + ")";

                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtT = new DataTable();
                da.Fill(dtT);
                for (int i = 0; i < dtT.Rows.Count; i++)
                {
                    decimal dAmt = Convert.ToDecimal(dtT.Rows[i]["Percentage"]);
                    dAdvAmt = dAdvAmt + dAmt;
                }
                da.Dispose();
                dtT.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dAdvAmt;
        }

        public static DataTable GetReceiptTypes(int argTempId, string argType, int argPayTypeId, int argCCId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                if (argType == "O")
                {
                    sSql = "Select A.ReceiptTypeId,A.OtherCostId,A.SchType,Convert(bit,1,1) Sel, B.OtherCostName ReceiptType,A.Percentage from dbo.CCReceiptType A " +
                           "Inner Join dbo.OtherCostMaster B on A.OtherCostId=B.OtherCostId " +
                           "Where TemplateId= " + argTempId;
                }
                else
                {
                    sSql = "Select TemplateId from dbo.PaymentSchedule Where SchType='A' and TypeId=" + argPayTypeId + " and CostCentreId= " + argCCId;
                    bool bAdvance = false;
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    DataTable dtT = new DataTable();
                    da.Fill(dtT);
                    if (dtT.Rows.Count > 0) { bAdvance = true; }
                    da.Dispose();
                    dtT.Dispose();

                    if (bAdvance == false)
                    {
                        sSql = "";
                    }
                    else { sSql = ""; }

                    sSql = sSql + "Select A.ReceiptTypeId,0 OtherCostId,'R' SchType,Case When B.ReceiptTypeId is null then Convert(bit,0,0) Else Convert(bit,1,1) End Sel,A.ReceiptTypeName ReceiptType,ISNULL(B.Percentage,0) Percentage,C.SortOrder From dbo.ReceiptType A " +
                           "Left join dbo.CCReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId And B.SchType<>'Q' and B.TemplateId=" + argTempId + " " +
                           "Left Join dbo.ReceiptTypeOrder C on C.ReceiptTypeId = A.ReceiptTypeId and C.PayTypeId= " + argPayTypeId + " and C.CostCentreId= " + argCCId + " " +
                           "Where A.ReceiptTypeId <>1 " +
                           "Union All " +
                           "Select 0 ReceiptTypeId,A.OtherCostId,'O' SchType,Case When B.ReceiptTypeId is null then Convert(bit,0,0) Else Convert(bit,1,1) End Sel,A.OtherCostName ReceiptType,ISNULL(B.Percentage,0) Percentage,C.SortOrder from dbo.OtherCostMaster A " +
                           "Left Join dbo.CCReceiptType B on A.OtherCostId=B.OtherCostId and B.TemplateId=" + argTempId + " " +
                           "Left Join dbo.ReceiptTypeOrder C on C.OtherCostId = A.OtherCostId and C.PayTypeId= " + argPayTypeId + " and C.CostCentreId= " + argCCId + " " +
                           "Where A.OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans Where CostCentreId=" + argCCId + " and PayTypeId=" + argPayTypeId + ")";

                    sSql = "Select G.ReceiptTypeId,G.OtherCostId,G.SchType,G.Sel,G.ReceiptType,G.Percentage from( " + sSql + " ) G Order by G.SortOrder";
                }
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

        public static DataTable GetAdvReceipt(int argTempId, string argType, int argPayTypeId, int argCCId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            try
            {
                string sSql = "";
                BsfGlobal.OpenCRMDB();
                sSql = "Select A.ReceiptTypeId,0 OtherCostId,'A' SchType,Case When B.ReceiptTypeId is null" +
                    " then Convert(bit,0,0) Else Convert(bit,1,1) End Sel,A.ReceiptTypeName ReceiptType," +
                    " ISNULL(B.Percentage,0) Percentage From dbo.ReceiptType A " +
                    " Left Join dbo.CCReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId and B.TemplateId=" + argTempId + " " +
                    " Left Join dbo.ReceiptTypeOrder C on C.ReceiptTypeId = A.ReceiptTypeId and C.PayTypeId=" + argPayTypeId + " " +
                    " and C.CostCentreId= " + argCCId + " Where A.ReceiptTypeId = 1";

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

        public static DataTable GetQualifierMaster(string argType, int argId, string argFlatType, int argFlatId, string argQualType)
        {
            string sSql = "";
            DataTable dt = null;
            try
            {
                if (argFlatType == "M")
                    sSql = "Select TemplateId,Case When B.Sel=1 then Convert(bit,1,1) else Convert(bit,0,0) End Sel,A.QualifierId, " +
                            " A.QualifierName,IsNull(Percentage,0)Percentage From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A  " +
                            " Left Join dbo.PaySchTax B On A.QualifierId=B.QualifierId Where QualType='" + argQualType + "' And TemplateId=" + argId + " " +
                            " Union All " +
                            " Select " + argId + " TemplateId,Convert(bit,0,0) Sel,A.QualifierId, A.QualifierName,0 Percentage From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A  " +
                            " Where QualType='" + argQualType + "' And QualifierId Not In(Select A.QualifierId From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A  " +
                            " Left Join dbo.PaySchTax B On A.QualifierId=B.QualifierId Where QualType='" + argQualType + "' And TemplateId=" + argId + ") ";
                //sSql = "Select Case When B.Sel=1 then Convert(bit,1,1) else Convert(bit,0,0) End Sel,A.QualifierId," +
                //    " A.QualifierName,Percentage from dbo.Qualifier_Temp A " +
                //    " Inner Join [" + BsfGlobal.g_sCRMDBName + "].dbo.PaySchTax B On A.QualifierId=B.QualifierId " +
                //    " Where QualType='B' And TemplateId=" + argId + "";
                else
                    sSql = "Select B.Sel,A.QualifierId, A.QualifierName,B.Percentage,B.Amount,B.Amount QAmount " +
                            " from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A  " +
                            " Inner Join dbo.PaySchTaxFlat B On A.QualifierId=B.QualifierId " +
                            " Left Join dbo.FlatTax C On C.QualifierId=B.QualifierId and C.FlatId=B.FlatId" +
                            " Where QualType='" + argQualType + "' And B.FlatId=" + argFlatId + " And PaymentSchId=" + argId + "";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    if (argFlatType == "M")
                        sSql = "Select Convert(bit,0,0) as Sel,QualifierId,QualifierName,0 Percentage from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp Where QualType='" + argType + "'";
                    else
                        sSql = "Select Convert(bit,0,0) as Sel,A.QualifierId,A.QualifierName,0 Percentage,0 Amount,B.Amount QAmount," +
                            " 0 VAmt From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A Inner Join dbo.FlatTax B " +
                            " On A.QualifierId=B.QualifierId Where QualType='" + argQualType + "' And FlatId=" + argFlatId + "";
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    dt = new DataTable();
                    da.Fill(dt);
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

            return dt;
        }

        public static DataSet GetFlatAdvReceipt(int argTempId, string argType, int argPayTypeId, int argCCId)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da;

            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.ReceiptTypeId,0 OtherCostId,'A' SchType,Case When B.ReceiptTypeId is null" +
                    " then Convert(bit,0,0) Else Convert(bit,1,1) End Sel,A.ReceiptTypeName ReceiptType," +
                    " ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount,isnull(B.NetAmount,0)" +
                    " NetAmount From dbo.ReceiptType A Left join dbo.FlatReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId" +
                    " and B.PaymentSchId=" + argTempId + " Where A.ReceiptTypeId = 1 ";

                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "ReceiptType");
                da.Dispose();

                sSql = String.Format("Select *,B.PaymentSchId,B.ReceiptTypeId,B.OtherCostId,B.SchType From dbo.FlatReceiptQualifier A Inner Join dbo.FlatReceiptType B on A.SchId=B.SchId Where B.PaymentSchId= {0}", argTempId);
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Qualifier");
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

        public static string GetAdvPer(int argPayTypeId, int argCCId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da; string bAns = "";

            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select TemplateId from dbo.PaymentSchedule Where SchType='A' and TypeId=" + argPayTypeId + " and CostCentreId= " + argCCId;
                bool bAdvance = false;
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtT = new DataTable();
                da.Fill(dtT);
                if (dtT.Rows.Count > 0) { bAdvance = true; }
                da.Dispose();
                dtT.Dispose();

                if (bAdvance == false)
                {
                    sSql = "Select SUM(A.Percentage)Percentage from dbo.CCReceiptType A Inner Join dbo.PaymentSchedule B" +
                            " On A.TemplateId=B.TemplateId Where ReceiptTypeId=1 And CostCentreId=" + argCCId + " And TypeId=" + argPayTypeId + "";

                    da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Percentage"], CommFun.datatypes.vartypenumeric)) < 100)
                        { bAns = "L"; }
                        else if (Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Percentage"], CommFun.datatypes.vartypenumeric)) == 100)
                        { bAns = "E"; }
                        else { bAns = "G"; }
                    }
                    else { bAns = "L"; }
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
            return bAns;
        }

        public static string GetTAXPer(int argPayTypeId, int argCCId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da; string bAns = "";

            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select SUM(B.Percentage)Percentage From PaymentSchedule A Inner Join PaySchTax B " +
                    " On A.TemplateId=B.TemplateId Where TypeId=" + argPayTypeId + "  And A.CostCentreId=" + argCCId + " Group By QualifierId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["Percentage"], CommFun.datatypes.vartypenumeric)) < 100)
                        { bAns = "L"; return bAns; }
                        else if (Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["Percentage"], CommFun.datatypes.vartypenumeric)) == 100)
                        { bAns = "E"; }
                        else { bAns = "G"; return bAns; }
                    }
                }
                else { bAns = "L"; }
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

        public static bool GetRecOrder(int argPayTypeId, int argCCId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da; bool bAns = false;

            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select * From dbo.ReceiptTypeOrder Where CostCentreId=" + argCCId + " And PayTypeId=" + argPayTypeId + "";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(dt);
                if (dt.Rows.Count > 0) { bAns = true; }
                da.Dispose();
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

        public static DataSet GetReceiptTypeFlat(int argTempId, string argType, int argPayTypeId, int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            DataSet ds = new DataSet();
            try
            {
                string sSql = "";
                if (argType == "O")
                {
                    sSql = "Select A.ReceiptTypeId,A.OtherCostId,A.SchType,Convert(bit,1,1) Sel, B.OtherCostName ReceiptType, "+
                           "A.Percentage,A.Amount,A.NetAmount from dbo.FlatReceiptType A " +
                           "Inner Join dbo.OtherCostMaster B on A.OtherCostId=B.OtherCostId " +
                           "Where PaymentSchId= " + argTempId;
                }
                else
                {
                    sSql = "Select A.ReceiptTypeId,0 OtherCostId,'R' SchType,Case When B.ReceiptTypeId is null then Convert(bit,0,0) Else Convert(bit,1,1) End Sel, " +
                           "A.ReceiptTypeName ReceiptType,ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount,isnull(B.NetAmount,0) NetAmount From dbo.ReceiptType A " +
                           "Left Join dbo.FlatReceiptType B ON A.ReceiptTypeId=B.ReceiptTypeId And B.SchType<>'Q' and B.PaymentSchId=" + argTempId + " " +
                           "Where A.ReceiptTypeId<>1 " +
                           "Union All " +
                           "Select 0 ReceiptTypeId,A.OtherCostId,'O' SchType,Case When B.ReceiptTypeId is null then Convert(bit,0,0) Else Convert(bit,1,1) End Sel," +
                           "A.OtherCostName ReceiptType,ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount,isnull(B.NetAmount,0) NetAmount from dbo.OtherCostMaster A " +
                           "Left Join dbo.FlatReceiptType B on A.OtherCostId=B.OtherCostId and B.PaymentSchId=" + argTempId + " " +
                           "Where A.OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans Where CostCentreId=" + argCCId + " and PayTypeId=" + argPayTypeId + ")";
                }

                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "ReceiptType");
                da.Dispose();

                sSql = String.Format("Select *,B.PaymentSchId,B.ReceiptTypeId,B.OtherCostId,B.SchType From dbo.FlatReceiptQualifier A " +
                                     " Inner Join dbo.FlatReceiptType B on A.SchId=B.SchId " +
                                     " Where B.PaymentSchId={0}", argTempId);
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Qualifier");
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

        public static DataTable GetReceiptQualFlat(int argPaySchId, string argType, int argPayTypeId, int argCCId, int argFlatId, bool argTypewise, string argQualType)
        {
            DataTable dt = new DataTable();
            DataTable dtQ = new DataTable();
            SqlDataAdapter da;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            bool bAns = false; bool bAnsQual = false;
            try
            {
                sSql = "Select * From ReceiptShTrans Where PaymentSchId=" + argPaySchId + " And QualifierId=0";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtM = new DataTable();
                da.Fill(dtM);
                da.Dispose();

                if (dtM.Rows.Count == 0)
                {
                    bAns = true;
                }

                if (bAns == true)
                {
                    if (argType == "O")
                    {
                        sSql = "Select A.ReceiptTypeId,A.PaymentSchId,0 QualifierId,A.FlatId,B.OtherCostName ReceiptType,A.Amount, " +
                                " isnull(D.PaidAmount,0)PaidAmount,isnull(Sum(A.Amount),0)-isnull(Sum(D.PaidAmount),0) BalanceAmount,0 CurrentAmount" +
                                " From dbo.FlatReceiptType A" +
                                " Inner Join dbo.OtherCostMaster B on A.OtherCostId=B.OtherCostId " +
                                " Left Join ReceiptTrans C On C.FlatId=A.FlatId" +
                                " Left Join ReceiptShTrans D On C.ReceiptTransId=D.ReceiptTransId" +
                                " Where A.PaymentSchId= " + argPaySchId + " Group By A.ReceiptTypeId,A.PaymentSchId,QualifierId,A.FlatId,B.OtherCostName,D.Amount";
                    }
                    else
                    {
                        sSql = "Select A.ReceiptTypeId,0 QualifierId,B.PaymentSchId,B.FlatId,A.ReceiptTypeName ReceiptType,isnull(Sum(B.Amount),0) Amount," +
                                " isnull(Sum(D.PaidAmount),0)PaidAmount,isnull(Sum(B.Amount),0)-isnull(Sum(D.PaidAmount),0) BalanceAmount,0 CurrentAmount From dbo.ReceiptType A " +
                                " Left Join dbo.FlatReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId And B.SchType<>'Q' and B.PaymentSchId=" + argPaySchId + " " +
                                " Left Join ReceiptTrans C On C.FlatId=B.FlatId" +
                                " left Join ReceiptShTrans D On C.ReceiptTransId=D.ReceiptTransId And D.PaymentSchId=" + argPaySchId + " " +
                                " Where A.ReceiptTypeId <>1 And B.Amount<>0 Group By A.ReceiptTypeId,QualifierId,B.PaymentSchId,B.FlatId,A.ReceiptTypeName " +
                                " Union All " +
                                " Select B.ReceiptTypeId,0 QualifierId,B.PaymentSchId,B.FlatId,A.OtherCostName ReceiptType,isnull(Sum(B.Amount),0) Amount," +
                                " isnull(Sum(D.PaidAmount),0)PaidAmount,isnull(Sum(B.Amount),0)-isnull(Sum(D.PaidAmount),0) BalanceAmount,0 CurrentAmount from dbo.OtherCostMaster A" +
                                " Left Join dbo.FlatReceiptType B on A.OtherCostId=B.OtherCostId and B.PaymentSchId=" + argPaySchId + " " +
                                " Left Join ReceiptTrans C On C.FlatId=B.FlatId" +
                                " left Join ReceiptShTrans D On C.ReceiptTransId=D.ReceiptTransId" +
                                " Where A.OtherCostId " +
                                " in (Select OtherCostId from dbo.OtherCostSetupTrans Where CostCentreId=" + argCCId + " " +
                                " and PayTypeId=" + argPayTypeId + ") And B.Amount<>0 Group By B.ReceiptTypeId,QualifierId,B.PaymentSchId,B.FlatId,A.OtherCostName ";
                    }
                }
                else
                {
                    if (argType == "O")
                    {
                        sSql = "Select A.ReceiptTypeId,A.PaymentSchId,0 QualifierId,A.FlatId,B.OtherCostName ReceiptType,A.Amount, " +
                                " isnull(Sum(D.PaidAmount),0)PaidAmount,isnull(Sum(A.Amount),0)-isnull(Sum(D.PaidAmount),0) BalanceAmount,0 CurrentAmount" +
                                " From dbo.FlatReceiptType A" +
                                " Inner Join dbo.OtherCostMaster B on A.OtherCostId=B.OtherCostId " +
                                " Left Join ReceiptTrans C On C.FlatId=A.FlatId" +
                                " Inner Join ReceiptShTrans D On C.ReceiptTransId=D.ReceiptTransId" +
                                " Where A.PaymentSchId= " + argPaySchId + " Group By A.ReceiptTypeId,A.PaymentSchId,QualifierId,A.FlatId,B.OtherCostName,A.Amount";
                    }
                    else
                    {
                        sSql = "Select A.ReceiptTypeId,0 QualifierId,B.PaymentSchId,B.FlatId,A.ReceiptTypeName ReceiptType,isnull(Sum(D.Amount),0) Amount," +
                                " isnull(Sum(D.PaidAmount),0)PaidAmount,isnull(Sum(D.Amount),0)-isnull(Sum(D.PaidAmount),0) BalanceAmount,0 CurrentAmount From dbo.ReceiptType A " +
                                " Left Join dbo.FlatReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId And B.SchType<>'Q' and B.PaymentSchId=" + argPaySchId + " " +
                                " Left Join ReceiptTrans C On C.FlatId=B.FlatId" +
                                " Inner Join ReceiptShTrans D On C.ReceiptTransId=D.ReceiptTransId And D.PaymentSchId=" + argPaySchId + " " +
                                " Where A.ReceiptTypeId <>1 And B.Amount<>0 And D.QualifierId=0 Group By A.ReceiptTypeId,QualifierId,B.PaymentSchId,B.FlatId,A.ReceiptTypeName " +
                                " Union All " +
                                " Select B.ReceiptTypeId,0 QualifierId,B.PaymentSchId,B.FlatId,A.OtherCostName ReceiptType,isnull(Sum(B.Amount),0) Amount," +
                                " isnull(Sum(D.PaidAmount),0)PaidAmount,isnull(Sum(B.Amount),0)-isnull(Sum(D.PaidAmount),0) BalanceAmount,0 CurrentAmount from dbo.OtherCostMaster A" +
                                " Left Join dbo.FlatReceiptType B on A.OtherCostId=B.OtherCostId and B.PaymentSchId=" + argPaySchId + " " +
                                " Left Join ReceiptTrans C On C.FlatId=B.FlatId" +
                                " Inner Join ReceiptShTrans D On C.ReceiptTransId=D.ReceiptTransId" +
                                " Where A.OtherCostId " +
                                " in (Select OtherCostId from dbo.OtherCostSetupTrans Where CostCentreId=" + argCCId + " " +
                                " and PayTypeId=" + argPayTypeId + ") And B.Amount<>0 And D.ReceiptTypeId=0 Group By B.ReceiptTypeId,QualifierId,B.PaymentSchId,B.FlatId,A.OtherCostName ";
                    }
                }
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(dt);
                da.Dispose();

                sSql = "Select PaidAmount From ReceiptShTrans Where PaymentSchId=" + argPaySchId + " And QualifierId<>0";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtQual = new DataTable();
                da.Fill(dtQual);
                da.Dispose();
                if (dtQual.Rows.Count > 0) { for (int i = 0; i < dtQual.Rows.Count; i++) { if (Convert.ToDecimal(dtQual.Rows[i]["PaidAmount"]) == 0) bAnsQual = true; } }

                if (bAnsQual == true)
                {
                    if (argTypewise == true)
                    {
                        sSql = "Select B.ReceiptTypeId,C.QualifierId,B.PaymentSchId,B.FlatId,C.QualifierName ReceiptType,Sum(A.Amount)Amount," +
                                " isnull(Sum(E.PaidAmount),0)PaidAmount,isnull(Sum(A.Amount),0)-isnull(Sum(E.PaidAmount),0) BalanceAmount,0 CurrentAmount " +
                                " From dbo.FlatReceiptQualifier A  " +
                                " Inner Join dbo.FlatReceiptType B On A.SchId=B.SchId  " +
                                " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On A.QualifierId=C.QualifierId  " +
                                " Left Join ReceiptTrans D On D.FlatId=B.FlatId" +
                                " Left Join ReceiptShTrans E On D.ReceiptTransId=E.ReceiptTransId" +
                                " Where B.FlatId=" + argFlatId + " AND QualType='" + argQualType + "' And B.PaymentSchId=" + argPaySchId + " And B.Amount<>0 " +
                                " Group By B.ReceiptTypeId,C.QualifierId,B.PaymentSchId,B.FlatId,C.QualifierName ";
                    }
                    else
                    {
                        sSql = "Select 0 ReceiptTypeId,A.QualifierId,B.PaymentSchId,B.FlatId,A.QualifierName ReceiptType,Sum(B.Amount)Amount," +
                                " isnull(Sum(E.PaidAmount),0)PaidAmount,isnull(sum(B.Amount),0)-isnull(Sum(E.PaidAmount),0) BalanceAmount,0 CurrentAmount " +
                                " from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A  " +
                                " Inner Join dbo.PaySchTaxFlat B On A.QualifierId=B.QualifierId " +
                                " Left Join dbo.FlatTax C On C.QualifierId=B.QualifierId and C.FlatId=B.FlatId" +
                                " Left Join dbo.ReceiptTrans D On D.FlatId=B.FlatId" +
                                " Left Join dbo.ReceiptShTrans E On D.ReceiptTransId=E.ReceiptTransId" +
                                " Where QualType='" + argQualType + "' And B.FlatId=" + argFlatId + " And B.PaymentSchId=" + argPaySchId + " And B.Amount<>0 " +
                                " Group By ReceiptTypeId,A.QualifierId,B.PaymentSchId,B.FlatId,A.QualifierName ";
                    }
                }
                else
                {
                    if (argTypewise == true)
                    {
                        sSql = "Select B.ReceiptTypeId,C.QualifierId,B.PaymentSchId,B.FlatId,C.QualifierName ReceiptType,Sum(A.Amount)Amount," +
                                " isnull(Sum(E.PaidAmount),0)PaidAmount,isnull(Sum(A.Amount),0)-isnull(Sum(E.PaidAmount),0) BalanceAmount,0 CurrentAmount " +
                                " From dbo.FlatReceiptQualifier A  " +
                                " Inner Join dbo.FlatReceiptType B On A.SchId=B.SchId  " +
                                " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On A.QualifierId=C.QualifierId  " +
                                " Left Join ReceiptTrans D On D.FlatId=B.FlatId" +
                                " Inner Join ReceiptShTrans E On D.ReceiptTransId=E.ReceiptTransId" +
                                " Where B.FlatId=" + argFlatId + " AND QualType='" + argQualType + "' And B.PaymentSchId=" + argPaySchId + " And B.Amount<>0 " +
                                " Group By B.ReceiptTypeId,C.QualifierId,B.PaymentSchId,B.FlatId,C.QualifierName ";
                    }
                    else
                    {
                        sSql = "Select 0 ReceiptTypeId,A.QualifierId,B.PaymentSchId,B.FlatId,A.QualifierName ReceiptType,Sum(B.Amount)Amount," +
                                " isnull(Sum(E.PaidAmount),0)PaidAmount,isnull(sum(B.Amount),0)-isnull(Sum(E.PaidAmount),0) BalanceAmount,0 CurrentAmount " +
                                " from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A  " +
                                " Inner Join dbo.PaySchTaxFlat B On A.QualifierId=B.QualifierId " +
                                " Left Join dbo.FlatTax C On C.QualifierId=B.QualifierId and C.FlatId=B.FlatId" +
                                " Left Join dbo.ReceiptTrans D On D.FlatId=B.FlatId" +
                                " Inner Join dbo.ReceiptShTrans E On D.ReceiptTransId=E.ReceiptTransId" +
                                " Where QualType='" + argQualType + "' And B.FlatId=" + argFlatId + " And B.PaymentSchId=" + argPaySchId + " And B.Amount<>0 " +
                                " Group By ReceiptTypeId,A.QualifierId,B.PaymentSchId,B.FlatId,A.QualifierName ";
                    }
                }
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(dtQ);
                da.Dispose();

                DataRow drT;
                if (dtQ.Rows.Count > 0)
                {
                    for (int i = 0; i < dtQ.Rows.Count; i++)
                    {
                        drT = dt.NewRow();
                        drT["ReceiptTypeId"] = dtQ.Rows[i]["ReceiptTypeId"];
                        drT["QualifierId"] = dtQ.Rows[i]["QualifierId"];
                        drT["PaymentSchId"] = dtQ.Rows[i]["PaymentSchId"];
                        drT["FlatId"] = dtQ.Rows[i]["FlatId"];
                        drT["ReceiptType"] = dtQ.Rows[i]["ReceiptType"];
                        drT["Amount"] = dtQ.Rows[i]["Amount"];
                        drT["PaidAmount"] = dtQ.Rows[i]["PaidAmount"];
                        drT["BalanceAmount"] = dtQ.Rows[i]["BalanceAmount"];
                        drT["CurrentAmount"] = dtQ.Rows[i]["CurrentAmount"];
                        dt.Rows.Add(drT);
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
            return dt;
        }

        public static DataTable GetReceiptQualMaster(int argBuyerId, DateTime argDate, string argQualType)
        {
            DataTable dt = new DataTable();
            DataTable dtQ;
            SqlDataAdapter da;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select A.PaymentSchId,B.CostCentreId,A.TemplateId,B.PayTypeId,C.Typewise,A.SchType,B.FlatId,A.SchDate,B.FlatNo,'ScheduleBill' ReceiptType,A.Description,A.NetAmount,A.PaidAmount,A.NetAmount-A.PaidAmount BalanceAmount," +
                        " Cast(0 as Decimal(18,3)) Amount From PaymentScheduleFlat A " +
                        " Inner Join FlatDetails B On A.FlatId=B.FlatId " +
                        " Inner Join PaySchType C On C.TypeId=B.PayTypeId " +
                        " Where LeadId=" + argBuyerId + " And TemplateId<>0 And A.NetAmount-A.PaidAmount>0 and A.SchDate <=  '" + string.Format(Convert.ToDateTime(argDate).ToString("dd-MMM-yyyy")) + "' " +
                        " Order By A.SortOrder";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtM = new DataTable();
                da.Fill(dtM);
                da.Dispose();

                if (dtM.Rows.Count > 0)
                {
                    for (int k = 0; k < dtM.Rows.Count; k++)
                    {
                        string argType = dtM.Rows[k]["SchType"].ToString();
                        int iPaySchId = Convert.ToInt32(dtM.Rows[k]["PaymentSchId"]);
                        int iCCId = Convert.ToInt32(dtM.Rows[k]["CostCentreId"]);
                        bool bTypewise = Convert.ToBoolean(dtM.Rows[k]["Typewise"]);
                        int iFlatId = Convert.ToInt32(dtM.Rows[k]["FlatId"]);
                        int iPayTypeId = Convert.ToInt32(dtM.Rows[k]["PayTypeId"]);

                        if (argType == "O")
                        {
                            sSql = "Select A.ReceiptTypeId,A.PaymentSchId,0 QualifierId,A.FlatId,B.OtherCostName ReceiptType,A.Amount, " +
                                    " isnull(Sum(D.PaidAmount),0)PaidAmount,isnull(Sum(A.Amount),0)-isnull(Sum(D.PaidAmount),0) BalanceAmount,0 CurrentAmount" +
                                    " From dbo.FlatReceiptType A" +
                                    " Inner Join dbo.OtherCostMaster B on A.OtherCostId=B.OtherCostId " +
                                    " Left Join ReceiptTrans C On C.FlatId=A.FlatId" +
                                    " Left Join ReceiptShTrans D On C.ReceiptTransId=D.ReceiptTransId" +
                                    " Where A.PaymentSchId= " + iPaySchId + " Group By A.ReceiptTypeId,A.PaymentSchId,QualifierId,A.FlatId,B.OtherCostName,A.Amount";
                        }
                        else
                        {
                            sSql = "Select A.ReceiptTypeId,0 QualifierId,B.PaymentSchId,B.FlatId,A.ReceiptTypeName ReceiptType,isnull(Sum(B.Amount),0) Amount," +
                                    " isnull(Sum(D.PaidAmount),0)PaidAmount,isnull(Sum(B.Amount),0)-isnull(Sum(D.PaidAmount),0) BalanceAmount,0 CurrentAmount From dbo.ReceiptType A " +
                                    " Left Join dbo.FlatReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId And B.SchType<>'Q' and B.PaymentSchId=" + iPaySchId + " " +
                                    " Left Join ReceiptTrans C On C.FlatId=B.FlatId" +
                                    " left Join ReceiptShTrans D On C.ReceiptTransId=D.ReceiptTransId And D.PaymentSchId=" + iPaySchId + " " +
                                    " Where A.ReceiptTypeId <>1 And B.Amount<>0 Group By A.ReceiptTypeId,QualifierId,B.PaymentSchId,B.FlatId,A.ReceiptTypeName " +
                                    " Union All " +
                                    " Select B.ReceiptTypeId,0 QualifierId,B.PaymentSchId,B.FlatId,A.OtherCostName ReceiptType,isnull(Sum(B.Amount),0) Amount," +
                                    " isnull(Sum(D.PaidAmount),0)PaidAmount,isnull(Sum(B.Amount),0)-isnull(Sum(D.PaidAmount),0) BalanceAmount,0 CurrentAmount from dbo.OtherCostMaster A" +
                                    " Left Join dbo.FlatReceiptType B on A.OtherCostId=B.OtherCostId and B.PaymentSchId=" + iPaySchId + " " +
                                    " Left Join ReceiptTrans C On C.FlatId=B.FlatId" +
                                    " left Join ReceiptShTrans D On C.ReceiptTransId=D.ReceiptTransId" +
                                    " Where A.OtherCostId " +
                                    " in (Select OtherCostId from dbo.OtherCostSetupTrans Where CostCentreId=" + iCCId + " " +
                                    " and PayTypeId=" + iPayTypeId + ") And B.Amount<>0 Group By B.ReceiptTypeId,QualifierId,B.PaymentSchId,B.FlatId,A.OtherCostName ";
                        }

                        da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                        da.Fill(dt);
                        da.Dispose();

                        if (bTypewise == true)
                        {
                            sSql = "Select B.ReceiptTypeId,C.QualifierId,B.PaymentSchId,B.FlatId,C.QualifierName ReceiptType,Sum(A.Amount)Amount," +
                                    " isnull(Sum(E.PaidAmount),0)PaidAmount,isnull(Sum(A.Amount),0)-isnull(Sum(E.PaidAmount),0) BalanceAmount,0 CurrentAmount " +
                                    " From dbo.FlatReceiptQualifier A  " +
                                    " Inner Join dbo.FlatReceiptType B On A.SchId=B.SchId  " +
                                    " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp C On A.QualifierId=C.QualifierId  " +
                                    " Left Join ReceiptTrans D On D.FlatId=B.FlatId" +
                                    " Left Join ReceiptShTrans E On D.ReceiptTransId=E.ReceiptTransId" +
                                    " Where B.FlatId=" + iFlatId + " AND QualType='" + argQualType + "' And B.PaymentSchId=" + iPaySchId + " And B.Amount<>0 " +
                                    " Group By B.ReceiptTypeId,C.QualifierId,B.PaymentSchId,B.FlatId,C.QualifierName ";
                        }
                        else
                        {
                            sSql = "Select 0 ReceiptTypeId,A.QualifierId,B.PaymentSchId,B.FlatId,A.QualifierName ReceiptType,Sum(B.Amount)Amount," +
                                    " isnull(Sum(E.PaidAmount),0)PaidAmount,isnull(sum(B.Amount),0)-isnull(Sum(E.PaidAmount),0) BalanceAmount,0 CurrentAmount " +
                                    " from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A  " +
                                    " Inner Join dbo.PaySchTaxFlat B On A.QualifierId=B.QualifierId " +
                                    " Left Join dbo.FlatTax C On C.QualifierId=B.QualifierId and C.FlatId=B.FlatId" +
                                    " Left Join dbo.ReceiptTrans D On D.FlatId=B.FlatId" +
                                    " Left Join dbo.ReceiptShTrans E On D.ReceiptTransId=E.ReceiptTransId" +
                                    " Where QualType='" + argQualType + "' And B.FlatId=" + iFlatId + " And B.PaymentSchId=" + iPaySchId + " And B.Amount<>0 " +
                                    " Group By ReceiptTypeId,A.QualifierId,B.PaymentSchId,B.FlatId,A.QualifierName ";
                        }
                        da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                        dtQ = new DataTable();
                        da.Fill(dtQ);
                        da.Dispose();

                        DataRow drT;
                        if (dtQ.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtQ.Rows.Count; i++)
                            {
                                drT = dt.NewRow();
                                drT["ReceiptTypeId"] = dtQ.Rows[i]["ReceiptTypeId"];
                                drT["QualifierId"] = dtQ.Rows[i]["QualifierId"];
                                drT["PaymentSchId"] = dtQ.Rows[i]["PaymentSchId"];
                                drT["FlatId"] = dtQ.Rows[i]["FlatId"];
                                drT["ReceiptType"] = dtQ.Rows[i]["ReceiptType"];
                                drT["Amount"] = dtQ.Rows[i]["Amount"];
                                drT["PaidAmount"] = dtQ.Rows[i]["PaidAmount"];
                                drT["BalanceAmount"] = dtQ.Rows[i]["BalanceAmount"];
                                drT["CurrentAmount"] = dtQ.Rows[i]["CurrentAmount"];




                                dt.Rows.Add(drT);
                            }
                        }
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
            return dt;
        }

        public static DataSet GetPBReceiptTypeFlat(int argTempId, string argType, int argPayTypeId, int argCCId, int argBillId)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                if (argType == "O")
                {
                    sSql = "Select A.ReceiptTypeId,A.OtherCostId,A.SchType,Convert(bit,1,1) Sel, B.OtherCostName ReceiptType,A.Percentage,A.Amount,A.NetAmount from dbo.PBReceiptType A " +
                           "Inner Join dbo.OtherCostMaster B on A.OtherCostId=B.OtherCostId " +
                           "Where PBillId= " + argBillId;
                }
                else
                {
                    sSql = "Select TemplateId from dbo.PaymentSchedule Where SchType='A' and TypeId=" + argPayTypeId + " and CostCentreId= " + argCCId;
                    bool bAdvance = false;
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    DataTable dtT = new DataTable();
                    da.Fill(dtT);
                    if (dtT.Rows.Count > 0) { bAdvance = true; }
                    da.Dispose();
                    dtT.Dispose();

                    if (bAdvance == false)
                    {
                        sSql = "";
                    }
                    else { sSql = ""; }

                    sSql = sSql + "Select A.ReceiptTypeId,0 OtherCostId,'R' SchType,Case When B.ReceiptTypeId is null then" +
                           " Convert(bit,0,0) Else Convert(bit,1,1) End Sel,A.ReceiptTypeName ReceiptType,ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount,isnull(B.NetAmount,0) NetAmount From dbo.ReceiptType A " +
                           "Left Join dbo.PBReceiptType B on A.ReceiptTypeId=B.ReceiptTypeId and B.PBillId=" + argBillId + " " +
                           "Where A.ReceiptTypeId <>1 " +
                           "Union All " +
                           "Select 0 ReceiptTypeId,A.OtherCostId,'O' SchType,Case When B.ReceiptTypeId is null then" +
                           " Convert(bit,0,0) Else Convert(bit,1,1) End Sel,A.OtherCostName ReceiptType,ISNULL(B.Percentage,0) Percentage,isnull(B.Amount,0) Amount,isnull(B.NetAmount,0) NetAmount from dbo.OtherCostMaster A " +
                           "Left Join dbo.PBReceiptType B on A.OtherCostId=B.OtherCostId and B.PBillId=" + argBillId + " " +
                           "Where A.OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans Where CostCentreId=" + argCCId + " and PayTypeId=" + argPayTypeId + ")";
                }
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "ReceiptType");
                da.Dispose();

                sSql = String.Format("Select *,B.PaymentSchId,B.ReceiptTypeId,B.OtherCostId,B.SchType From dbo.PBReceiptTypeQualifier A Inner Join dbo.PBReceiptType B on A.SchId=B.SchId Where B.PBillId= {0}", argBillId);
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Qualifier");
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

        internal static DataTable GetStagesLevel(int argLevelId, int argCostCentreId)
        {
            DataTable dtStage = null;
            SqlDataAdapter sda;
            string sSql = "";

            try
            {
                sSql = "Select A.StageName, A.StageId, Convert(bit,0,0) as Sel from dbo.Stages A " +
                       "Where StageId not in (Select StageId from dbo.LevelStageTrans Where LevelId = " + argLevelId +
                       " AND A.CostCentreId= " + argCostCentreId + ") AND A.CostCentreId= " + argCostCentreId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dtStage = new DataTable();
                sda.Fill(dtStage);
                dtStage.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtStage;
        }

        public static void UpdateFlatPaySchDate(int argFlatId, int argTempId, int argTypeId, bool argAfter, string argDurType, int argduration, string argDate, int argCCId, int argPayTypeId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                int i = 0;
                if (argAfter == true) { i = 1; }

                string sSql = "";
                if (argDate == null)
                {
                    sSql = "Update dbo.PaymentScheduleFlat Set PreStageTypeId = " + argTypeId + ",DateAfter = " + i + ",DurationType = '" + argDurType +
                            "',Duration= " + argduration + ",SchDate = null " +
                            "Where TemplateId = " + argTempId + " And FlatId=" + argFlatId + " And PaymentSchId=" + argPayTypeId + "";
                }

                else
                {
                    sSql = "Update dbo.PaymentScheduleFlat Set PreStageTypeId = " + argTypeId + ",DateAfter = " + i + ",DurationType = '" + argDurType +
                           "',Duration= " + argduration + ", SchDate = '" + argDate + "' " +
                           "Where TemplateId = " + argTempId + " And FlatId=" + argFlatId + " And PaymentSchId=" + argPayTypeId + "";
                }
                SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                //Update SchDate

                sSql = "Select StartDate,EndDate from dbo.ProjectInfo Where CostCentreId= " + argCCId;
                cmd = new SqlCommand(sSql, conn, tran);
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                sdr.Close();
                dt.Dispose();

                DateTime StartDate = DateTime.Now, EndDate = DateTime.Now;
                if (dt.Rows.Count > 0)
                {
                    StartDate = Convert.ToDateTime(dt.Rows[0]["StartDate"]);
                    EndDate = Convert.ToDateTime(dt.Rows[0]["EndDate"]);
                }

                sSql = "Select SortOrder From dbo.PaymentScheduleFlat Where CostCentreId=" + argCCId + " And TemplateId=" + argTempId + " And FlatId=" + argFlatId + " ";
                cmd = new SqlCommand(sSql, conn, tran);
                dt = new DataTable();
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                int iSortOrder = 0;
                if (dt.Rows.Count > 0) { iSortOrder = Convert.ToInt32(dt.Rows[0]["SortOrder"]); }

                sSql = "Select * From dbo.PaymentScheduleFlat Where CostCentreId=" + argCCId + " And SortOrder>=" + iSortOrder + " " +
                        " And FlatId=" + argFlatId + " Order by SortOrder";
                cmd = new SqlCommand(sSql, conn, tran);
                dt = new DataTable();
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        int iTemplateId = Convert.ToInt32(dt.Rows[j]["TemplateId"]);
                        int ipre = Convert.ToInt32(dt.Rows[j]["PreStageTypeId"]);
                        int iDateAfter = Convert.ToInt32(dt.Rows[j]["DateAfter"]);
                        int iDuration = Convert.ToInt32(dt.Rows[j]["Duration"]);
                        string sDurType = dt.Rows[j]["DurationType"].ToString();

                        if (ipre == -1) { } else if (ipre == -2) { } else if (ipre == -3) { } else if (ipre == 0) { } else { iTemplateId = ipre; }

                        sSql = "Select SchDate From dbo.PaymentScheduleFlat Where CostCentreId=" + argCCId + " And FlatId=" + argFlatId + "" +
                                " And TemplateId=" + iTemplateId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        DataTable dtDate = new DataTable();
                        sdr = cmd.ExecuteReader();
                        dtDate.Load(sdr);
                        sdr.Close();
                        cmd.Dispose();

                        sSql = "Select FinaliseDate From dbo.BuyerDetail Where CostCentreId=" + argCCId + " And FlatId=" + argFlatId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        DataTable dtFinDate = new DataTable();
                        sdr = cmd.ExecuteReader();
                        dtFinDate.Load(sdr);
                        sdr.Close();
                        cmd.Dispose();

                        DateTime dFinDate = DateTime.MinValue;
                        if (dtFinDate.Rows.Count > 0) { dFinDate = Convert.ToDateTime(dtFinDate.Rows[0]["FinaliseDate"]); }

                        DateTime SchDate = DateTime.MinValue;
                        if (ipre == -1)
                        {
                            if (dFinDate == DateTime.MinValue)
                                SchDate = Convert.ToDateTime(null);
                            else
                                SchDate = dFinDate;
                        }
                        else if (ipre == -2)
                            SchDate = StartDate;
                        else if (ipre == -3)
                            SchDate = EndDate;
                        else
                            SchDate = Convert.ToDateTime(CommFun.IsNullCheck(dtDate.Rows[0]["SchDate"], CommFun.datatypes.VarTypeDate));

                        if (sDurType == "D")
                        {
                            if (SchDate != DateTime.MinValue)
                            {
                                if (iDateAfter == 0)
                                    SchDate = SchDate.AddDays(iDuration);
                                else
                                    SchDate = SchDate.AddDays(-iDuration);
                            }
                        }
                        else if (sDurType == "M")
                        {
                            if (SchDate != DateTime.MinValue)
                            {
                                if (iDateAfter == 0)
                                    SchDate = SchDate.AddMonths(iDuration);
                                else
                                    SchDate = SchDate.AddDays(-iDuration);
                            }
                        }

                        //iTemplateId = Convert.ToInt32(dt.Rows[j]["TemplateId"]);
                        //ipre = Convert.ToInt32(dt.Rows[j]["PreStageTypeId"]);

                        sSql = "Update dbo.PaymentScheduleFlat Set SchDate=" + "@SchDate" + " " +
                                " Where TemplateId=" + dt.Rows[j]["TemplateId"] + " And FlatId=" + argFlatId + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        SqlParameter parameter = new SqlParameter() { ParameterName = "@SchDate", DbType = DbType.DateTime };
                        if (SchDate == DateTime.MinValue)
                            parameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            parameter.Value = SchDate;
                        cmd.Parameters.Add(parameter);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    dt.Dispose();
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

        public static void InsertReceiptQualifier(DataTable argPayTrans, int argCCId, int argTId)
        {
            int ipaySchId = 0;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = String.Format("DELETE FROM dbo.PaymentSchedule WHERE CostCentreId={0} AND TypeId={1}", argCCId, argTId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    for (int t = 0; t < argPayTrans.Rows.Count; t++)
                    {
                        string nxtSchDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(argPayTrans.Rows[t]["SchDate"]));
                        sSql = String.Format("INSERT INTO dbo.PaymentSchedule(CostCentreId,TypeId,SchType,Description,SchDescId,StageId,OtherCostId,SchDate,DateAfter,Duration,DurationType,SchPercent,Amount,PreStageTypeId) Values({0},{1},'{2}','{3}',{4},{5},{6},'{7}',{8},{9},'{10}',{11},{12},{13})SELECT SCOPE_IDENTITY();", argPayTrans.Rows[t]["CCId"], argTId, argPayTrans.Rows[t]["EntryType"], argPayTrans.Rows[t]["Description"], argPayTrans.Rows[t]["DescId"], argPayTrans.Rows[t]["StageId"], argPayTrans.Rows[t]["OtherCostId"], nxtSchDate, Convert.ToInt32(argPayTrans.Rows[t]["DateAfter"].ToString()), argPayTrans.Rows[t]["Duration"], Convert.ToChar(argPayTrans.Rows[t]["DurationType"].ToString()), argPayTrans.Rows[t]["AmtPercent"], argPayTrans.Rows[t]["Amount"], argPayTrans.Rows[t]["PreStageTypeId"]);
                        cmd = new SqlCommand(sSql, conn, tran);
                        ipaySchId = int.Parse(cmd.ExecuteScalar().ToString());

                    }
                    tran.Commit();
                }
                catch (SqlException e)
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

        public static DataTable GetAllFlats(int argCCId)
        {
            SqlDataAdapter sda; DataTable dtFlat = null; string sSql = "";
            //string sSql = "SELECT FlatId FROM dbo.FlatDetails Where CostCentreId=" + argCCId + "";
            sSql = "SELECT A.FlatId FROM dbo.FlatDetails A Where A.CostCentreId=" + argCCId + " " +
                    " And FlatId Not In(Select Distinct FlatId From dbo.PaymentScheduleFlat " +
                    " Where (BillPassed=1 Or PaidAmount>0)) And FlatId " +
                    " Not In(Select Distinct FlatId From ReceiptTrans Where Amount>0 And CostCentreId=" + argCCId + ")";
            try
            {
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dtFlat = new DataTable();
                sda.Fill(dtFlat);
                dtFlat.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtFlat;
        }

        public static bool CheckBillPassed(int argFlatId)
        {
            bool bAns = false;
            try
            {
                string sSql = "Select DISTINCT FlatId From dbo.PaymentScheduleFlat " +
                        " Where FlatId=" + argFlatId + " And (BillPassed=1 Or PaidAmount>0)" +
                        " UNION ALL " +
                        " Select DISTINCT FlatId From ReceiptTrans Where FlatId=" + argFlatId + " " +
                        " UNION ALL " +
                        " Select DISTINCT FlatId From ProgressBillRegister Where FlatId=" + argFlatId + "";

                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                DataTable dtFlat = new DataTable();
                sda.Fill(dtFlat);
                sda.Dispose();

                if (dtFlat.Rows.Count > 0) { bAns = true; }
                dtFlat.Dispose();
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

        public static decimal CheckReceiptAmt(int argFlatId, int argPaySchId, int argReceiptId, int argOCId, decimal argAmt)
        {
            bool bAns = false; decimal dNetAmt = 0;
            decimal dLandValue = 0; decimal dConValue = 0; decimal dOCAmt = 0;
            SqlDataAdapter sda;
            try
            {
                BsfGlobal.OpenCRMDB();
                string sSql = "Select * From FlatDetails Where FlatId=" + argFlatId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader sdr = cmd.ExecuteReader();
                DataTable dtF = new DataTable();
                dtF.Load(sdr);
                cmd.Dispose();
                if (dtF.Rows.Count > 0)
                {
                    dLandValue = Convert.ToDecimal(dtF.Rows[0]["LandRate"]);
                    dConValue = Convert.ToDecimal(dtF.Rows[0]["BaseAmt"]) - Convert.ToDecimal(dtF.Rows[0]["LandRate"]);
                }

                sSql = "Select Amount From dbo.FlatOtherCost Where FlatId=" + argFlatId + " And OtherCostId=" + argOCId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                sdr = cmd.ExecuteReader();
                DataTable dtOC = new DataTable();
                dtOC.Load(sdr);
                cmd.Dispose();
                if (dtOC.Rows.Count > 0)
                {
                    dOCAmt = Convert.ToDecimal(dtOC.Rows[0]["Amount"]);
                }

                sSql = "Select Sum(Amount) Amount From dbo.FlatReceiptType Where " +
                    " FlatId=" + argFlatId + " And ReceiptTypeId=" + argReceiptId + " And OtherCostId=" + argOCId + " " +
                    " And PaymentSchId<>" + argPaySchId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    decimal Amt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)) + argAmt;
                    if (argReceiptId == 2)
                    {
                        if (Amt > dLandValue) { dNetAmt = dLandValue - Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); bAns = true; } else { dNetAmt = dLandValue; }
                    }
                    else if (argReceiptId == 3)
                    {
                        if (Amt > dConValue) { dNetAmt = dConValue - Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); bAns = true; } else { dNetAmt = dConValue; }
                    }
                    else if (argOCId > 0)
                    {
                        if (Amt > dOCAmt) { dNetAmt = dOCAmt - Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); bAns = true; } else { dNetAmt = dOCAmt; }
                    }
                }

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

            return dNetAmt;
        }

        public static string GetStatus(int argFlatId)
        {
            SqlDataAdapter da; string sStatus = "";
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select Status From dbo.FlatDetails Where FlatId=" + argFlatId;
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtT = new DataTable();
                da.Fill(dtT);
                if (dtT.Rows.Count > 0) { sStatus = dtT.Rows[0]["Status"].ToString(); }
                da.Dispose();
                dtT.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return sStatus;
        }

        #endregion

        #region Qualifier Settings for Payment Schedule

        public static DataTable GetQual(int argQId, DateTime argDate, string argQualType)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                if (argDate == DateTime.MinValue) { argDate = DateTime.Now; }

                string sSql = "Select PeriodId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriod Where QualType='" + argQualType + "' and " +
                                "((TDate is not null and Fdate <= '" + argDate.ToString("dd MMM yyyy") + "' and TDate >= '" + argDate.ToString("dd MMM yyyy") + "') or " +
                                "(TDate is null  and FDate <= '" + argDate.ToString("dd MMM yyyy") + "'))";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dtT = new DataTable();
                dtT.Load(dr);
                dr.Close();
                cmd.Dispose();

                int iPeriodId = 0;
                if (dtT.Rows.Count > 0) { iPeriodId = Convert.ToInt32(dtT.Rows[0]["PeriodId"]); }
                dtT.Dispose();

                if (iPeriodId != 0)
                {
                    if (argQId == 0)
                    {
                        sSql = "Select A.QualifierId,B.Expression,B.ExpPer,A.Add_Less_Flag,B.SurCharge,B.EDCess,B.HEDCess,B.Net,0 Taxable From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A " +
                               "Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriodTrans B on A.QualifierId=B.QualifierId and B.PeriodId = " + iPeriodId + 
                               " Where B.ExpPer<>0";
                    }
                    else
                    {
                        sSql = "Select B.Expression,B.ExpPer,A.Add_Less_Flag,B.SurCharge,B.EDCess,B.HEDCess,B.Net,0 Taxable From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A " +
                               "Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriodTrans B on A.QualifierId=B.QualifierId and B.PeriodId = " + iPeriodId + "" +
                               "Where A.QualifierId = " + argQId;
                    }
                }
                else
                {
                    if (argQId == 0)
                    {
                        sSql = "Select QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,HEDCess,Net,0 Taxable "+
                                " From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp Where ExpPer<>0";
                    }
                    else
                    {
                        sSql = "Select Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,HEDCess,Net,0 Taxable From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp " +
                               "Where QualifierId = " + argQId;
                    }
                }

                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable GetQual(int argQId, DateTime argDate, string argQualType, SqlConnection argConn, SqlTransaction argTrans)
        {
            if (argDate == DateTime.MinValue) { argDate = DateTime.Now; }

            int iPeriodId = 0;

            string sSql = "Select PeriodId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriod Where QualType='" + argQualType + "' and " +
                            "((TDate is not null and Fdate <= '" + argDate.ToString("dd MMM yyyy") + "' and TDate >= '" + argDate.ToString("dd MMM yyyy") + "' ) or " +
                            "(TDate is null  and FDate <= '" + argDate.ToString("dd MMM yyyy") + "'))";
            SqlCommand cmd = new SqlCommand(sSql, argConn, argTrans);
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

            cmd = new SqlCommand(sSql, argConn, argTrans);
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            cmd.Dispose();

            return dt;
        }

        public static DataTable QualifierSelect(int argQId, string argQType, DateTime argDate)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = new DataTable();
            try
            {
                if (argDate == DateTime.MinValue) { argDate = DateTime.Now; }

                string sSql = "Select * from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp Where QualType='" + argQType + "' and Sel=1";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();

                if (dt.Rows.Count == 0)
                {
                    sSql = "Select PeriodId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriod Where QualType='" + argQType + "' and " +
                            "((TDate is not null and Fdate <= '" + argDate.AddDays(1).ToString("dd MMM yyyy") +
                            "' and TDate >= '" + argDate.AddDays(1).ToString("dd MMM yyyy") + "') or " +
                            "(TDate is null  and FDate <= '" + argDate.AddDays(1).ToString("dd MMM yyyy") + "'))";
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    DataTable dtT = new DataTable();
                    sda.Fill(dtT);
                    sda.Dispose();

                    int iPeriodId = 0;
                    if (dtT.Rows.Count > 0) { iPeriodId = Convert.ToInt32(dtT.Rows[0]["PeriodId"]); }
                    dtT.Dispose();

                    if (argQId == 0)
                    {
                        if (iPeriodId != 0)
                        {
                            sSql = "Select A.QualifierId,A.QualId,C.Expression,C.Add_Less_Flag,C.Net," +
                                    "C.ExpPer,C.SurCharge,C.EDCess,C.HEDCess,A.QualMId,A.QualTypeId,A.Taxable From  " +
                                    "[" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A  " +
                                    "Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualifierType B on A.QualTypeId=B.QualTypeId " +
                                    "Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriodTrans C on A.QualifierId=C.QualifierId " +
                                    " AND C.Sel=1 AND C.PeriodId=" + iPeriodId + " " +
                                    "Where A.QualType = '" + argQType + "' Order by  A.SortOrder";
                        }
                        else
                        {
                            sSql = "Select Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,HEDCess,Net,Taxable From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp";
                        }
                        sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                        dt = new DataTable();
                        sda.Fill(dt);
                        sda.Dispose();
                    }
                    else
                    {
                        if (iPeriodId != 0)
                        {
                            sSql = "Select A.QualifierId,A.QualId,C.Expression,C.Add_Less_Flag,C.Net," +
                                    "C.ExpPer,C.SurCharge,C.EDCess,C.HEDCess,A.QualMId,A.QualTypeId,A.Taxable From  " +
                                    "[" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A  " +
                                    "Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualifierType B on A.QualTypeId=B.QualTypeId " +
                                    "Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriodTrans C on A.QualifierId=C.QualifierId and C.Sel=1 and C.PeriodId=" + iPeriodId + " " +
                                    "Where A.QualType = '" + argQType + "' And A.QualifierId=" + argQId + " Order by  A.SortOrder";                            
                        }
                        else
                        {
                            sSql = "Select Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,HEDCess,Net,Taxable From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp " +
                                     "Where QualifierId = " + argQId;
                        }
                        sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                        dt = new DataTable();
                        sda.Fill(dt);
                        sda.Dispose();
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
                if (argDate == DateTime.MinValue) { argDate = DateTime.Now; }

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
                BsfGlobal.g_CRMDB.Close();
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable QualifierSelect(int argQId, string argQType, DateTime argDate, SqlConnection argConn, SqlTransaction argTrans)
        {
            SqlCommand cmd;
            DataTable dt = new DataTable();
            DataTable dtT = new DataTable();
            string sSql = "";
            int iPeriodId = 0;

            try
            {
                if (argDate == DateTime.MinValue) { argDate = DateTime.Now; }

                sSql = "Select PeriodId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriod Where QualType='" + argQType + "' and " +
                        "((TDate is not null and Fdate <= '" + argDate.ToString("dd MMM yyyy") + "' and TDate >= '" + argDate.ToString("dd MMM yyyy") + "' ) or " +
                        "(TDate is null  and FDate <= '" + argDate.ToString("dd MMM yyyy") + "'))";
                cmd = new SqlCommand(sSql, argConn, argTrans);
                SqlDataReader dr = cmd.ExecuteReader();
                dtT = new DataTable();
                dtT.Load(dr);
                dr.Close();
                cmd.Dispose();

                if (dtT.Rows.Count > 0) { iPeriodId = Convert.ToInt32(dtT.Rows[0]["PeriodId"]); }
                dtT.Dispose();

                if (iPeriodId != 0)
                {
                    sSql = "Select A.QualifierId,A.QualId,C.Expression,C.Add_Less_Flag,C.Net," +
                        "C.ExpPer,C.SurCharge,C.EDCess,C.HEDCess,A.QualMId,A.QualTypeId From  " +
                        "[" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp A  " +
                        "Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualifierType B on A.QualTypeId=B.QualTypeId " +
                        "Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriodTrans C on A.QualifierId=C.QualifierId and C.Sel=1 and C.PeriodId=" + iPeriodId + " " +
                        "Where A.QualType = '" + argQType + "' AND A.QualifierId=" + argQId + " Order by A.SortOrder";
                }
                else
                {
                    sSql = "Select Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,HEDCess,Net From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp " +
                           "Where QualifierId = " + argQId;
                }
                cmd = new SqlCommand(sSql, argConn, argTrans);
                dr = cmd.ExecuteReader();
                dt.Load(dr);
                dr.Close();

            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public static DataTable GetSTSettings(string argWOType, DateTime argDate, SqlConnection argConn, SqlTransaction argTrans)
        {
            DataTable dt = new DataTable();
            string sSql = "";
            int iPeriodId = 0;

            if (argDate == DateTime.MinValue) { argDate = DateTime.Now; }

            try
            {
                sSql = "Select PeriodId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualPeriod Where QualType='S' and " +
                        "((TDate IS NOT NULL AND Fdate<='" + argDate.ToString("dd MMM yyyy") + "' AND TDate>='" + argDate.ToString("dd MMM yyyy") + "' ) or " +
                        "(TDate IS NULL AND FDate<='" + argDate.ToString("dd MMM yyyy") + "'))";
                SqlCommand cmd = new SqlCommand(sSql, argConn, argTrans);
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
                cmd = new SqlCommand(sSql, argConn, argTrans);
                dr = cmd.ExecuteReader();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        #endregion
    }
}
