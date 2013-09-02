using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessObjects;
using Microsoft.VisualBasic;
using Qualifier;

namespace CRM.DataLayer
{
    public class CallsheetentryDL
    {
        #region Methods

        public DataSet GetData()
        {
            BsfGlobal.OpenCRMDB();
            DataSet ds = new DataSet();
            try
            {
                string sSql = "Select UserId ExecId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As ExecName From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId Where B.PositionType='M'";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                SqlDataReader dreader = cmd.ExecuteReader();
                ds.Load(dreader, LoadOption.OverwriteChanges, "Executive");
                dreader.Close();
                cmd.Dispose();

                sSql = "SELECT StatusId,Description FROM dbo.StatusMaster ORDER BY SortId";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dreader = cmd.ExecuteReader();
                ds.Load(dreader, LoadOption.OverwriteChanges, "Status");
                dreader.Close();
                cmd.Dispose();

                sSql = "SELECT CallTypeID,Description,SortId FROM dbo.CallType ORDER BY SortID";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dreader = cmd.ExecuteReader();
                ds.Load(dreader, LoadOption.OverwriteChanges, "CallType");
                dreader.Close();
                cmd.Dispose();

                sSql = "SELECT NatureId,Description FROM dbo.NatureMaster ORDER BY SortID";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dreader = cmd.ExecuteReader();
                ds.Load(dreader, LoadOption.OverwriteChanges, "Nature");
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
            
            return ds;
        }

        public static DataTable GetProject()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "Select CostCentreId Id,CostCentreName Name,ProjectDB From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                        " Where ProjectDB In(Select ProjectName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType IN('B'))" +
                        " and CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                        " Where UserId=" + BsfGlobal.g_lUserId + ") Order By Name";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
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

        public static DataTable GetCallSheetProject()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "Select CostCentreId Id,CostCentreName Name,ProjectDB From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                        " Where ProjectDB In(Select ProjectName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType IN('B','L'))" +
                        " and CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                        " Where UserId=" + BsfGlobal.g_lUserId + ") Order By Name";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
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

        public int InsertCallSheet(CallSheetEntryMasterBO argCallSheet, string argmode, DataTable dtEnqTrans, DataTable dtFinal,string argFlatNo,bool argChkSend)
        {
            int iEntryId = 0; 
            SqlConnection conn;
            conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            string sSql = "";

            decimal dAdvAmt = 0; decimal dRate = 0; int iCompanyId = 0;
            BsfGlobal.VoucherType oVType = new BsfGlobal.VoucherType();
            BsfGlobal.VoucherType oVCCType = new BsfGlobal.VoucherType();
            BsfGlobal.VoucherType oVCompanyType = new BsfGlobal.VoucherType();

            try
            {
                if (Convert.ToDateTime(CommFun.IsNullCheck(argCallSheet.TrnDate, CommFun.datatypes.VarTypeDate)) == DateTime.MinValue) { argCallSheet.TrnDate = DateTime.Now; }

                if (argmode == "A")
                {
                    if (argCallSheet.CallType == "Finalization")
                    {
                        if (dtEnqTrans.Rows.Count > 0)
                        {
                            oVType = BsfGlobal.GetVoucherNo(85, Convert.ToDateTime(argCallSheet.TrnDate), 0, 0, conn, tran,"I");
                            if (oVType.GenType == true)
                            {
                                dtEnqTrans.Rows[0]["GAllotNo"] = oVType.VoucherNo;
                            }

                            oVCCType = BsfGlobal.GetVoucherNo(85, Convert.ToDateTime(argCallSheet.TrnDate), 0, argCallSheet.ProjID, conn, tran, "I");
                            if (oVCCType.GenType == true)
                            {
                                dtEnqTrans.Rows[0]["CCAllotNo"] = oVCCType.VoucherNo;
                            }
                            iCompanyId = CallsheetentryDL.GetCompanyId(argCallSheet.ProjID);
                            oVCompanyType = BsfGlobal.GetVoucherNo(85, Convert.ToDateTime(argCallSheet.TrnDate), iCompanyId, 0, conn, tran, "I");
                            if (oVCompanyType.GenType == true)
                            {
                                dtEnqTrans.Rows[0]["COAllotNo"] = oVCompanyType.VoucherNo;
                            }
                        }
                    }

                    if (argCallSheet.CallType == "Finalisation" || argCallSheet.CallType == "Finalization") { argCallSheet.NextCallDate = DateTime.MinValue; }

                    if (argCallSheet.CallType == "Drop" || argCallSheet.CallType == "Finalisation" || argCallSheet.CallType == "Finalization")
                    {
                        sSql = "INSERT INTO dbo.CallSheet(ExecutiveId,LeadId,ProjectId,TrnDate,CallFF,StatusID,NatureID,CallTypeID,NextCallDate,Remarks,ReminderId,Reminder,UserId) " +
                                "VALUES (" + argCallSheet.ExecutiveID + "," + argCallSheet.LeadID + "," + argCallSheet.ProjID + ",@TrnDate,'" + argCallSheet.CallFF + "'," +
                                " " + argCallSheet.StatusId + "," + argCallSheet.NatureID + "," + argCallSheet.CallTypeID + ",@NextCallDate,'" + argCallSheet.Remarks + "'," +
                                " " + argCallSheet.ReminderId + ",'" + argCallSheet.Reminder + "'," + BsfGlobal.g_lUserId + ")  SELECT SCOPE_IDENTITY();";
                        cmd = new SqlCommand(sSql, conn, tran);
                        
                        SqlParameter dateparameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@TrnDate" };                        
                        if (Convert.ToDateTime(CommFun.IsNullCheck(argCallSheet.TrnDate, CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                            dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            dateparameter.Value = argCallSheet.TrnDate;
                        cmd.Parameters.Add(dateparameter);

                        dateparameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@NextCallDate" };
                        if (Convert.ToDateTime(CommFun.IsNullCheck(argCallSheet.NextCallDate, CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                            dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            dateparameter.Value = argCallSheet.NextCallDate;
                        cmd.Parameters.Add(dateparameter);

                        iEntryId = int.Parse(cmd.ExecuteScalar().ToString());
                        cmd.Dispose();

                        sSql = "Update dbo.LeadRegister SET NextCallDate=NULL,CallTypeId=" + argCallSheet.CallTypeID + ", " +
                                "Email='" + argCallSheet.Email + "' WHERE LeadId=" + argCallSheet.LeadID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Update dbo.LeadCommAddressInfo SET Email='" + argCallSheet.Email + "' " +
                                "WHERE LeadId=" + argCallSheet.LeadID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    else
                    {
                        sSql = "INSERT INTO dbo.CallSheet(ExecutiveId,LeadId,ProjectId,TrnDate,CallFF,StatusID,NatureID,CallTypeID,NextCallDate,Remarks,ReminderId,Reminder,UserId) " +
                                "VALUES (" + argCallSheet.ExecutiveID + "," + argCallSheet.LeadID + "," + argCallSheet.ProjID + ",@TrnDate,'" + argCallSheet.CallFF + "'," +
                                " " + argCallSheet.StatusId + "," + argCallSheet.NatureID + "," + argCallSheet.CallTypeID + ",@NextCallDate,'" + argCallSheet.Remarks + "'," +
                                " " + argCallSheet.ReminderId + ",'" + argCallSheet.Reminder + "'," + BsfGlobal.g_lUserId + ")  SELECT SCOPE_IDENTITY();";

                        cmd = new SqlCommand(sSql, conn, tran);

                        SqlParameter dateparameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@TrnDate" };
                        if (Convert.ToDateTime(CommFun.IsNullCheck(argCallSheet.TrnDate, CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                            dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            dateparameter.Value = argCallSheet.TrnDate;
                        cmd.Parameters.Add(dateparameter);

                        dateparameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@NextCallDate" };
                        if (Convert.ToDateTime(CommFun.IsNullCheck(argCallSheet.NextCallDate, CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                            dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            dateparameter.Value = argCallSheet.NextCallDate;
                        cmd.Parameters.Add(dateparameter);

                        iEntryId = int.Parse(cmd.ExecuteScalar().ToString());
                        cmd.Dispose();

                        sSql = "Update dbo.LeadRegister SET NextCallDate=@NextCallDate,CallTypeId=" + argCallSheet.CallTypeID + ", " +
                                "Email='" + argCallSheet.Email + "' WHERE LeadId=" + argCallSheet.LeadID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        dateparameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@NextCallDate" };
                        if (Convert.ToDateTime(CommFun.IsNullCheck(argCallSheet.NextCallDate, CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                            dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            dateparameter.Value = argCallSheet.NextCallDate;
                        cmd.Parameters.Add(dateparameter);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Update dbo.LeadProjectInfo SET NextCallDate=@NextCallDate WHERE CostCentreId=" + argCallSheet.ProjID + " AND LeadId=" + argCallSheet.LeadID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        dateparameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@NextCallDate" };
                        if (Convert.ToDateTime(CommFun.IsNullCheck(argCallSheet.NextCallDate, CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                            dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            dateparameter.Value = argCallSheet.NextCallDate;
                        cmd.Parameters.Add(dateparameter);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Update dbo.LeadCommAddressInfo SET Email='" + argCallSheet.Email + "' " +
                                "WHERE LeadId=" + argCallSheet.LeadID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    //CommFun.InsertLog(DateTime.Now, "CallSheet Entry-Add", "N", "Add CallSheet Entry", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                    BsfGlobal.InsertLog(DateTime.Now, "Pre-Followup-Add", "N", "Pre-Followup", argCallSheet.LeadID, argCallSheet.ProjID, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                }
                else
                {
                    //Allotment No
                    iCompanyId = CallsheetentryDL.GetCompanyId(argCallSheet.ProjID);
                    if (argCallSheet.CallType == "Finalization")
                    {
                        if (dtEnqTrans.Rows.Count > 0)
                        {
                            if (Convert.ToDateTime(argCallSheet.TrnDate) != Convert.ToDateTime(argCallSheet.TrnDate))//if (Convert.ToDateTime(UsageBO.USDate) != Convert.ToDateTime(dEDate.EditValue))
                            {
                                if (oVType.PeriodWise == true)
                                {
                                    oVType = new BsfGlobal.VoucherType();
                                    oVCCType = new BsfGlobal.VoucherType();
                                    oVCompanyType = new BsfGlobal.VoucherType();
                                    if (BsfGlobal.CheckPeriodChange(Convert.ToDateTime(argCallSheet.TrnDate), Convert.ToDateTime(argCallSheet.TrnDate)) == true)//if (BsfGlobal.CheckPeriodChange(Convert.ToDateTime(UsageBO.USDate), Convert.ToDateTime(dEDate.EditValue)) == true)
                                    {
                                        oVType = BsfGlobal.GetVoucherNo(85, Convert.ToDateTime(argCallSheet.TrnDate), 0, 0);
                                        //txtAllot.Text = oVType.VoucherNo;
                                        dtEnqTrans.Rows[0]["GAllotNo"] = oVType.VoucherNo;


                                        oVCCType = BsfGlobal.GetVoucherNo(85, Convert.ToDateTime(argCallSheet.TrnDate), 0, argCallSheet.ProjID);
                                        if (oVCCType.GenType == true)
                                        {
                                            //txtCCINo.Text = oVCCType.VoucherNo;
                                            dtEnqTrans.Rows[0]["CCAllotNo"] = oVCCType.VoucherNo;
                                        }

                                        oVCompanyType = BsfGlobal.GetVoucherNo(85, Convert.ToDateTime(argCallSheet.TrnDate), iCompanyId, 0);
                                        if (oVCompanyType.GenType == true)
                                        {
                                            //txtCINo.Text = oVCompanyType.VoucherNo;
                                            dtEnqTrans.Rows[0]["COAllotNo"] = oVCompanyType.VoucherNo;
                                        }
                                        //BsfGlobal.UpdateMaxNo(85, oVType, 0, 0);
                                        //BsfGlobal.UpdateMaxNo(85, oVType, 0, argCallSheet.ProjID);
                                        //BsfGlobal.UpdateMaxNo(85, oVType, iCompanyId, 0);
                                    }
                                }
                            }
                        }
                    }

                    if (argmode != "A") { iEntryId = argCallSheet.EntryID ; }
                    if (argCallSheet.CallType == "Drop" || argCallSheet.CallType == "Finalisation" || argCallSheet.CallType == "Finalization")
                    {
                        sSql = "Update dbo.Callsheet SET ExecutiveId=" + argCallSheet.ExecutiveID + ",LeadId=" + argCallSheet.LeadID + ",ProjectId=" + argCallSheet.ProjID +
                                ",TrnDate=@TrnDate,CallFF='" + argCallSheet.CallFF + "',StatusID=" + argCallSheet.StatusId + ", " +
                                "NatureID=" + argCallSheet.NatureID + ",CallTypeID=" + argCallSheet.CallTypeID + ",NextCallDate=NULL, " +
                                "Remarks='" + argCallSheet.Remarks + "',ReminderID=0,Reminder='" + argCallSheet.Reminder + "',UserId=" + BsfGlobal.g_lUserId + " " +
                                " WHERE EntryID=" + argCallSheet.EntryID + "";
                        cmd = new SqlCommand(sSql, conn, tran);

                        SqlParameter dateparameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@TrnDate" };
                        if (Convert.ToDateTime(CommFun.IsNullCheck(argCallSheet.TrnDate, CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                            dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            dateparameter.Value = argCallSheet.TrnDate;
                        cmd.Parameters.Add(dateparameter);

                        dateparameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@NextCallDate" };
                        if (Convert.ToDateTime(CommFun.IsNullCheck(argCallSheet.NextCallDate, CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                            dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            dateparameter.Value = argCallSheet.NextCallDate;
                        cmd.Parameters.Add(dateparameter);

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Update dbo.LeadRegister SET NextCallDate=NULL,CallTypeId=" + argCallSheet.CallTypeID + ", " +
                                "Email='" + argCallSheet.Email + "' WHERE LeadId=" + argCallSheet.LeadID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Update dbo.LeadCommAddressInfo SET Email='" + argCallSheet.Email + "' " +
                                "WHERE LeadId=" + argCallSheet.LeadID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    else
                    {
                        sSql = "Update dbo.Callsheet SET ExecutiveId=" + argCallSheet.ExecutiveID + ",LeadId=" + argCallSheet.LeadID + ",ProjectId=" + argCallSheet.ProjID +
                                ",TrnDate=@TrnDate,CallFF='" + argCallSheet.CallFF + "',StatusID=" + argCallSheet.StatusId + ", " +
                                "NatureID=" + argCallSheet.NatureID + ",CallTypeID=" + argCallSheet.CallTypeID + ",NextCallDate=@NextCallDate, " +
                                "Remarks='" + argCallSheet.Remarks + "',ReminderID=" + argCallSheet.ReminderId + ",Reminder='" + argCallSheet.Reminder + "',UserId=" + BsfGlobal.g_lUserId + " " +
                                "WHERE EntryID=" + argCallSheet.EntryID + "";
                        cmd = new SqlCommand(sSql, conn, tran);

                        SqlParameter dateparameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@TrnDate" };
                        if (Convert.ToDateTime(CommFun.IsNullCheck(argCallSheet.TrnDate, CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                            dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            dateparameter.Value = argCallSheet.TrnDate;
                        cmd.Parameters.Add(dateparameter);

                        dateparameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@NextCallDate" };
                        if (Convert.ToDateTime(CommFun.IsNullCheck(argCallSheet.NextCallDate, CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                            dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            dateparameter.Value = argCallSheet.NextCallDate;
                        cmd.Parameters.Add(dateparameter);

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Update dbo.LeadRegister SET NextCallDate=@NextCallDate,CallTypeId=" + argCallSheet.CallTypeID + ", " +
                                "Email='" + argCallSheet.Email + "' WHERE LeadId=" + argCallSheet.LeadID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        dateparameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@NextCallDate" };
                        if (Convert.ToDateTime(CommFun.IsNullCheck(argCallSheet.NextCallDate, CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                            dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        else
                            dateparameter.Value = argCallSheet.NextCallDate;
                        cmd.Parameters.Add(dateparameter);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Update dbo.LeadCommAddressInfo SET Email='" + argCallSheet.Email + "' " +
                                "WHERE LeadId=" + argCallSheet.LeadID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    //CommFun.InsertLog(DateTime.Now, "CallSheet Entry-Edit", "E", "Edit CallSheet Entry", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                    BsfGlobal.InsertLog(DateTime.Now, "Pre-Followup-Modify", "E", "Pre-Followup", argCallSheet.LeadID, argCallSheet.ProjID, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);

                }
                sSql = "";
                DateTime dFinaliseDate = DateTime.MinValue;
                if (argCallSheet.CallType == "Finalization" || argCallSheet.CallType == "Finalisation" || argCallSheet.CallType == "Block")
                {
                    if (dtEnqTrans != null)
                    {
                        if (argmode == "A")
                        {
                            sSql = "Delete From dbo.BuyerDetail Where LeadId=" + argCallSheet.LeadID + " And FlatId=" + argCallSheet.FlatID + " ";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            if (dtEnqTrans.Rows.Count > 0)
                            {
                                sSql = "INSERT INTO dbo.BuyerDetail(AllotmentNo,CCAllotNo,COAllotNo,CostCentreId,ExecutiveId,LeadId,EntryId,FlatId,PaySchId,Status,CustomerType," +
                                        " PaymentOption,BranchId,LoanPer,LoanAccNo,BrokerId,BrokerComm,BrokerAmount,ValidUpto,FinaliseDate,PostSaleExecId,Advance) Values" +
                                        " ('" + dtEnqTrans.Rows[0]["GAllotNo"] + "','" + dtEnqTrans.Rows[0]["CCAllotNo"] + "','" + dtEnqTrans.Rows[0]["COAllotNo"] + "'," + argCallSheet.ProjID + "," + argCallSheet.ExecutiveID + "," + argCallSheet.LeadID + "," +
                                        " " + iEntryId + "," + argCallSheet.FlatID + "," + dtEnqTrans.Rows[0]["PaySchId"] + ",'" + dtEnqTrans.Rows[0]["Status"] + "'," +
                                        " '" + dtEnqTrans.Rows[0]["CustomerType"] + "','" + dtEnqTrans.Rows[0]["PaymentOption"] + "'," +
                                        " " + dtEnqTrans.Rows[0]["BranchId"] + "," + dtEnqTrans.Rows[0]["LoanPer"] + ",'" + dtEnqTrans.Rows[0]["LoanAccNo"] + "'," +
                                        " " + dtEnqTrans.Rows[0]["BrokerId"] + "," + dtEnqTrans.Rows[0]["ComPer"] + "," + dtEnqTrans.Rows[0]["ComAmount"] + "," +
                                        " @ValidUpto,@FinaliseDate," + dtEnqTrans.Rows[0]["PostSaleExecId"] + "," + dtEnqTrans.Rows[0]["AdvAmt"] + ") ";
                                cmd = new SqlCommand(sSql, conn, tran);

                                SqlParameter dateparameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@ValidUpto" };
                                if (Convert.ToDateTime(CommFun.IsNullCheck(dtEnqTrans.Rows[0]["ValidUpto"], CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                                    dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                                else
                                    dateparameter.Value = Convert.ToDateTime(CommFun.IsNullCheck(dtEnqTrans.Rows[0]["ValidUpto"], CommFun.datatypes.VarTypeDate));
                                cmd.Parameters.Add(dateparameter);

                                dateparameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@FinaliseDate" };
                                dFinaliseDate = Convert.ToDateTime(CommFun.IsNullCheck(dtEnqTrans.Rows[0]["FinaliseDate"], CommFun.datatypes.VarTypeDate));
                                if (dFinaliseDate == DateTime.MinValue)
                                    dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                                else
                                    dateparameter.Value = dFinaliseDate;
                                cmd.Parameters.Add(dateparameter);

                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }

                            if (argCallSheet.CallType == "Block")
                                sSql = "Update dbo.BuyerDetail Set Status='B' WHERE FlatId=" + argCallSheet.FlatID + " ";
                            else
                                sSql = "Update dbo.BuyerDetail Set Status='S' WHERE FlatId=" + argCallSheet.FlatID + " ";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            if (argCallSheet.CallType == "Block")
                                sSql = "Update dbo.FlatDetails Set Status='B',LeadId=" + argCallSheet.LeadID + ",PayTypeId=" + dtEnqTrans.Rows[0]["PaySchId"] + " WHERE FlatId=" + argCallSheet.FlatID + " AND CostCentreId=" + argCallSheet.ProjID + "";
                            else
                                sSql = "Update dbo.FlatDetails Set Status='S',LeadId=" + argCallSheet.LeadID + ",PayTypeId=" + dtEnqTrans.Rows[0]["PaySchId"] + " WHERE FlatId=" + argCallSheet.FlatID + " AND CostCentreId=" + argCallSheet.ProjID + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            if (argCallSheet.CallType == "Block")
                            {
                                sSql = "INSERT INTO dbo.BlockUnits(CostCentreId,FlatId,CustomerType,LeadId,BlockUpto,Date,BlockType) Values(" + argCallSheet.ProjID + "," +
                                 " " + argCallSheet.FlatID + ",'" + dtEnqTrans.Rows[0]["CustomerType"] + "'," + argCallSheet.LeadID + ",@ValidUpto," +
                                " '" + String.Format(Convert.ToDateTime(DateTime.Now).ToString("MM-dd-yyyy")) + "','B')";
                                cmd = new SqlCommand(sSql, conn, tran);
                                SqlParameter dateparameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@ValidUpto" };
                                if (Convert.ToDateTime(CommFun.IsNullCheck(dtEnqTrans.Rows[0]["ValidUpto"], CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                                    dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                                else
                                    dateparameter.Value = Convert.ToDateTime(CommFun.IsNullCheck(dtEnqTrans.Rows[0]["ValidUpto"], CommFun.datatypes.VarTypeDate));
                                cmd.Parameters.Add(dateparameter);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }

                            sSql = "INSERT INTO dbo.LeadFlatInfo (LeadId,Buyer,FlatId,CostCentreId)VALUES(" + argCallSheet.LeadID + ",1," + argCallSheet.FlatID + "," + argCallSheet.ProjID + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                        }
                        else
                        {
                            if (dtEnqTrans.Rows.Count > 0)
                            {
                                iEntryId = Convert.ToInt32(dtEnqTrans.Rows[0]["CallSheetEntryId"]);
                                sSql = "Update dbo.BuyerDetail Set AllotmentNo='" + dtEnqTrans.Rows[0]["GAllotNo"] + "',CCAllotNo='" + dtEnqTrans.Rows[0]["CCAllotNo"] + "'," +
                                        " COAllotNo='" + dtEnqTrans.Rows[0]["COAllotNo"] + "',CostCentreId=" + argCallSheet.ProjID + ",ExecutiveId=" + argCallSheet.ExecutiveID + "," +
                                        " LeadId=" + argCallSheet.LeadID + ",EntryId=" + iEntryId + ",FlatId=" + argCallSheet.FlatID + ",PaySchId=" + dtEnqTrans.Rows[0]["PaySchId"] + "," +
                                        " Status='" + dtEnqTrans.Rows[0]["Status"] + "',CustomerType='" + dtEnqTrans.Rows[0]["CustomerType"] + "'," +
                                        " PaymentOption='" + dtEnqTrans.Rows[0]["PaymentOption"] + "',BranchId=" + dtEnqTrans.Rows[0]["BranchId"] + "," +
                                        " LoanPer=" + dtEnqTrans.Rows[0]["LoanPer"] + ",LoanAccNo='" + dtEnqTrans.Rows[0]["LoanAccNo"] + "',BrokerId=" + dtEnqTrans.Rows[0]["BrokerId"] + "," +
                                        " BrokerComm=" + dtEnqTrans.Rows[0]["ComPer"] + ",BrokerAmount=" + dtEnqTrans.Rows[0]["ComAmount"] + "," +
                                        " ValidUpto=@ValidUpto,FinaliseDate=@FinaliseDate," +
                                        " PostSaleExecId=" + dtEnqTrans.Rows[0]["PostSaleExecId"] + ",Advance=" + dtEnqTrans.Rows[0]["AdvAmt"] + "" +
                                        " WHERE EntryId=" + iEntryId + " ";
                                cmd = new SqlCommand(sSql, conn, tran);

                                SqlParameter dateparameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@ValidUpto" };
                                if (Convert.ToDateTime(CommFun.IsNullCheck(dtEnqTrans.Rows[0]["ValidUpto"], CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                                    dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                                else
                                    dateparameter.Value = Convert.ToDateTime(CommFun.IsNullCheck(dtEnqTrans.Rows[0]["ValidUpto"], CommFun.datatypes.VarTypeDate));
                                cmd.Parameters.Add(dateparameter);

                                dateparameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@FinaliseDate" };
                                dFinaliseDate = Convert.ToDateTime(CommFun.IsNullCheck(dtEnqTrans.Rows[0]["FinaliseDate"], CommFun.datatypes.VarTypeDate));
                                if (dFinaliseDate == DateTime.MinValue)
                                    dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                                else
                                    dateparameter.Value = dFinaliseDate;
                                cmd.Parameters.Add(dateparameter);

                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                sSql = "UPDATE dbo.BlockUnits SET CustomerType='" + dtEnqTrans.Rows[0]["CustomerType"] + "',LeadId=" + argCallSheet.LeadID + ", BlockUpto=@ValidUpto" +
                                        " WHERE FlatId=" + argCallSheet.FlatID + " And BlockType='B'";
                                cmd = new SqlCommand(sSql, conn, tran);
                                dateparameter = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@ValidUpto" };
                                if (Convert.ToDateTime(CommFun.IsNullCheck(dtEnqTrans.Rows[0]["ValidUpto"], CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                                    dateparameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                                else
                                    dateparameter.Value = Convert.ToDateTime(CommFun.IsNullCheck(dtEnqTrans.Rows[0]["ValidUpto"], CommFun.datatypes.VarTypeDate));
                                cmd.Parameters.Add(dateparameter);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                sSql = "Update dbo.FollowUp Set ExecutiveId=" + dtEnqTrans.Rows[0]["PostSaleExecId"] + "" +
                                       " Where FlatId=" + argCallSheet.FlatID + " AND CostCentreId=" + argCallSheet.ProjID + "";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                if (argCallSheet.CallType == "Block")
                                    sSql = "Update dbo.BuyerDetail Set Status='B' WHERE FlatId=" + argCallSheet.FlatID + " ";
                                else
                                    sSql = "Update dbo.BuyerDetail Set Status='S' WHERE FlatId=" + argCallSheet.FlatID + " ";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                if (argCallSheet.CallType == "Block")
                                    sSql = "Update dbo.FlatDetails Set Status='B',LeadId=" + argCallSheet.LeadID + ",PayTypeId=" + dtEnqTrans.Rows[0]["PaySchId"] + " WHERE FlatId=" + argCallSheet.FlatID + " AND CostCentreId=" + argCallSheet.ProjID + "";
                                else
                                    sSql = "Update dbo.FlatDetails Set Status='S',LeadId=" + argCallSheet.LeadID + ",PayTypeId=" + dtEnqTrans.Rows[0]["PaySchId"] + " WHERE FlatId=" + argCallSheet.FlatID + " AND CostCentreId=" + argCallSheet.ProjID + "";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }
                    }

                    sSql = "Select Rate,AdvAmount From dbo.FlatDetails Where FlatId=" + argCallSheet.FlatID + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dtA = new DataTable();
                    dtA.Load(dr);
                    if (dtA.Rows.Count > 0 && dtEnqTrans.Rows.Count > 0) 
                    { 
                        dAdvAmt = Convert.ToDecimal(dtA.Rows[0]["AdvAmount"]);
                        dRate = Convert.ToDecimal(dtA.Rows[0]["Rate"]);
                        if (dAdvAmt != Convert.ToDecimal(dtEnqTrans.Rows[0]["AdvAmt"]))
                        {
                            sSql = "Update dbo.FlatDetails Set AdvAmount=" + dtEnqTrans.Rows[0]["AdvAmt"] + " WHERE FlatId=" + argCallSheet.FlatID + " ";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }

                    //PaymentScheduleDL.InsertFlatScheduleI(argCallSheet.FlatID, conn, tran);

                    SqlDataReader sdr, sdr1, sdr2; DataTable dt, dt1; int iStgId = 0, iTemplateId = 0;
                    int iDateAfter = 0, iDuration = 0; string sDurType = ""; DateTime SchDate; int iSortOrder = 0;
                    DateTime StartDate = DateTime.Now; DateTime EndDate = DateTime.Now; int ipre = 0;
                    sSql = "Select TemplateId,PreStageTypeId from dbo.PaymentScheduleFlat Where FlatId=" + argCallSheet.FlatID + " And PreStageTypeId=-1";
                    cmd = new SqlCommand(sSql, conn, tran);
                    sdr = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(sdr); cmd.Dispose();

                    if (dt.Rows.Count > 0)
                    {
                        iStgId = Convert.ToInt32(dt.Rows[0]["PreStageTypeId"]);
                        iTemplateId = Convert.ToInt32(dt.Rows[0]["TemplateId"]);
                    }
                    dt.Dispose();

                    sSql = "Select SortOrder From dbo.PaymentScheduleFlat Where FlatId=" + argCallSheet.FlatID + " And TemplateId=" + iTemplateId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    sdr2 = cmd.ExecuteReader();
                    dt1 = new DataTable();
                    dt1.Load(sdr2); cmd.Dispose();
                    dt1.Dispose();

                    if (dt1.Rows.Count > 0)
                    {
                        iSortOrder = Convert.ToInt32(dt1.Rows[0]["SortOrder"]);
                    }

                    sSql = "select StartDate,EndDate From ProjectInfo Where CostCentreId= " + argCallSheet.ProjID;
                    cmd = new SqlCommand(sSql, conn, tran);
                    dt = new DataTable();
                    sdr = cmd.ExecuteReader();
                    dt.Load(sdr);
                    dt.Dispose();

                    if (dt.Rows.Count > 0)
                    { 
                        StartDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["StartDate"], CommFun.datatypes.VarTypeDate));
                        EndDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["EndDate"], CommFun.datatypes.VarTypeDate));
                    }

                    sSql = "Update dbo.PaymentScheduleFlat Set SchDate=@FinaliseDate" +
                            " Where TemplateId=" + iTemplateId + " And FlatId=" + argCallSheet.FlatID + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    SqlParameter dateparameter1 = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@FinaliseDate" };
                    if (dFinaliseDate == DateTime.MinValue)
                        dateparameter1.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        dateparameter1.Value = dFinaliseDate;
                    cmd.Parameters.Add(dateparameter1);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Update dbo.PaymentScheduleFlat Set SchDate=@FinaliseDate" +
                            " Where TemplateId=0 And FlatId=" + argCallSheet.FlatID + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    dateparameter1 = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@FinaliseDate" };
                    if (dFinaliseDate == DateTime.MinValue)
                        dateparameter1.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    else
                        dateparameter1.Value = dFinaliseDate;
                    cmd.Parameters.Add(dateparameter1);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (iStgId == -1)
                    {
                        if (iStgId == -1)
                        {
                            sSql = "Select A.PreStageTypeId,A.CostCentreId,A.TemplateId,A.DateAfter,A.Duration,A.Durationtype from dbo.PaymentScheduleFlat A" +
                                    " Left Join dbo.ProgressBillRegister B On A.FlatId=B.FlatId " +
                                    " Where A.FlatId=" + argCallSheet.FlatID + " And A.SortOrder>=" + iSortOrder + "" +
                                    " And A.PaymentSchId Not In " +
                                    " (Select PaySchId From dbo.ProgressBillRegister Where FlatId=" + argCallSheet.FlatID + ") Order By A.SortOrder";
                        }
                        cmd = new SqlCommand(sSql, conn, tran);
                        sdr1 = cmd.ExecuteReader();
                        dt = new DataTable();
                        dt.Load(sdr1);
                        cmd.Dispose();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            iTemplateId = Convert.ToInt32(dt.Rows[i]["TemplateId"]);
                            ipre = Convert.ToInt32(dt.Rows[i]["PreStageTypeId"]);
                            iDateAfter = Convert.ToInt32(dt.Rows[i]["DateAfter"]);
                            iDuration = Convert.ToInt32(dt.Rows[i]["Duration"]);
                            sDurType = dt.Rows[i]["DurationType"].ToString();

                            if (ipre == -1) { } else if (ipre == -2) { } else if (ipre == -3) { } else if (ipre == 0) { } else { iTemplateId = ipre; }

                            sSql = "Select SchDate From dbo.PaymentScheduleFlat Where CostCentreId=" + dt.Rows[i]["CostCentreId"] + " And FlatId=" + argCallSheet.FlatID + "" +
                                  " And TemplateId=" + iTemplateId + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            DataTable dtDate = new DataTable();
                            sdr = cmd.ExecuteReader();
                            dtDate.Load(sdr);
                            dtDate.Dispose();

                            if (ipre == -1) { SchDate = Convert.ToDateTime(CommFun.IsNullCheck(dFinaliseDate, CommFun.datatypes.VarTypeDate)); }
                            else if (ipre == -2) { SchDate = StartDate; }
                            else if (ipre == -3) { SchDate = EndDate; }
                            else
                                SchDate = Convert.ToDateTime(CommFun.IsNullCheck(dtDate.Rows[0]["SchDate"], CommFun.datatypes.VarTypeDate));

                            if (sDurType == "D")
                            { if (iDateAfter == 0) SchDate = SchDate.AddDays(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }
                            else if (sDurType == "M")
                            { if (iDateAfter == 0) SchDate = SchDate.AddMonths(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }


                            sSql = "Update dbo.PaymentScheduleFlat Set SchDate=@SchDate" +
                                    " Where TemplateId=" + dt.Rows[i]["TemplateId"] + " And FlatId=" + argCallSheet.FlatID + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            dateparameter1 = new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@SchDate" };
                            if (Convert.ToDateTime(CommFun.IsNullCheck(SchDate, CommFun.datatypes.VarTypeDate)) == DateTime.MinValue)
                                dateparameter1.Value = System.Data.SqlTypes.SqlDateTime.Null;
                            else
                                dateparameter1.Value = Convert.ToDateTime(CommFun.IsNullCheck(SchDate, CommFun.datatypes.VarTypeDate));
                            cmd.Parameters.Add(dateparameter1);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                        }
                    }

                    //InsertChangeRate(argCallSheet.FlatID, dRate, Convert.ToDecimal(dtEnqTrans.Rows[0]["Rate"]), conn, tran);

                    if (argmode == "A")
                    {
                        if (argCallSheet.CallType == "Finalization" || argCallSheet.CallType == "Finalisation")
                        {
                            PaymentScheduleDL.InsertFinalFlatScheduleI(argCallSheet.FlatID,"S", conn, tran);
                        }
                        else if (argCallSheet.CallType == "Block")
                        {
                            PaymentScheduleDL.InsertFinalFlatScheduleI(argCallSheet.FlatID, "B", conn, tran);
                        }
                        else { PaymentScheduleDL.InsertFlatScheduleI(argCallSheet.FlatID, conn, tran); }
                    }

                    if (dtEnqTrans.Rows.Count > 0)
                    {
                        InsertChangeRate(argCallSheet.FlatID, dRate, Convert.ToDecimal(dtEnqTrans.Rows[0]["Rate"]), conn, tran);
                    }

                    //UnitDirDL.InsertFlatCheckList(dtFinal, argCallSheet.FlatID, "F", argChkSend, argCallSheet.ProjID,argFlatNo);
                    if (dtFinal != null)
                    {
                        UnitDirDL.InsertFlatChk(dtFinal, argCallSheet.FlatID, "F", argChkSend, argFlatNo, argCallSheet.ProjID, conn, tran);
                    }
                }

                if (argmode == "A")
                {
                    if (argCallSheet.CallType == "Finalization" || argCallSheet.CallType == "Finalisation")
                    {
                        if (oVType.GenType == true)
                        {
                            BsfGlobal.UpdateMaxNo(85, oVType, 0, 0, conn, tran);
                        }
                        if (oVCCType.GenType == true)
                        {
                            BsfGlobal.UpdateMaxNo(85, oVCCType, 0, argCallSheet.ProjID, conn, tran);
                        }
                        if (oVCompanyType.GenType == true)
                        {
                            BsfGlobal.UpdateMaxNo(85, oVCompanyType, iCompanyId, 0, conn, tran);
                        }
                    }
                }

                tran.Commit();

                if (argCallSheet.CallType == "Finalization" || argCallSheet.CallType == "Finalisation" || argCallSheet.CallType == "Block")
                {
                    if (argmode == "E")
                    {
                        if (dtEnqTrans.Rows.Count > 0)
                        {
                            BsfGlobal.InsertLog(DateTime.Now, "Flat-Finalisation-Modify", "N", "Flat Finalisation", argCallSheet.FlatID, argCallSheet.ProjID, 0,
                                                BsfGlobal.g_sCRMDBName, argFlatNo, BsfGlobal.g_lUserId, Convert.ToDecimal(dtEnqTrans.Rows[0]["AdvAmt"]), 0);
                        }
                        else
                        {
                            BsfGlobal.InsertLog(DateTime.Now, "Flat-Finalisation-Modify", "N", "Flat Finalisation", argCallSheet.FlatID, argCallSheet.ProjID, 0,
                                                BsfGlobal.g_sCRMDBName, argFlatNo, BsfGlobal.g_lUserId, 0, 0);
                        }
                    }
                    else
                    {
                        if (dtEnqTrans.Rows.Count > 0)
                        {
                            BsfGlobal.InsertLog(DateTime.Now, "Flat-Finalisation-Create", "C", "Flat Finalisation", argCallSheet.FlatID, argCallSheet.ProjID, 0,
                                                BsfGlobal.g_sCRMDBName, argFlatNo, BsfGlobal.g_lUserId, Convert.ToDecimal(dtEnqTrans.Rows[0]["AdvAmt"]), 0);
                        }
                        else
                        {
                            BsfGlobal.InsertLog(DateTime.Now, "Flat-Finalisation-Create", "C", "Flat Finalisation", argCallSheet.FlatID, argCallSheet.ProjID, 0, 
                                                BsfGlobal.g_sCRMDBName, argFlatNo, BsfGlobal.g_lUserId, 0, 0);
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
                conn.Close();
            }
            return iEntryId;
        }

        public int InsertPlotCallSheet(CallSheetEntryMasterBO argCallSheet, string argmode, DataTable dtEnqTrans, DataTable dtFinal, string argFlatNo, bool argChkSend,DataTable dtLand,int argLandId)
        {
            int iEntryId = 0;
            SqlConnection conn;
            conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            string sSql = "";
            string nxtCallDate = string.Format("{0:dd/MMM/yyyy}", argCallSheet.NextCallDate);
            //argCallSheet.NextCallDate.ToString("dd/MM/yyyy");
            if (argCallSheet.TrnDate == DateTime.MinValue) { argCallSheet.TrnDate = DateTime.Now; }
            string trnDate = string.Format("{0:dd/MMM/yyyy}", argCallSheet.TrnDate);
            //argCallSheet.TrnDate.ToString("dd/MM/yyyy");
            string validUpto = ""; string FinaliseDate = "";
            if (nxtCallDate == "01/Jan/0001")
                nxtCallDate = null;

            decimal dAdvAmt = 0; decimal dRate = 0;

            try
            {

                if (argmode == "A")
                {
                    if (argCallSheet.CallType == "Drop" || argCallSheet.CallType == "Finalisation" || argCallSheet.CallType == "Finalization")
                    {
                        sSql = "INSERT INTO dbo.CallSheet(ExecutiveId,LeadId,ProjectId,TrnDate,CallFF,StatusID,NatureID,CallTypeID,NextCallDate,Remarks,ReminderId,Reminder) " +
                        "VALUES (" + argCallSheet.ExecutiveID + "," + argCallSheet.LeadID + "," + argCallSheet.ProjID + ",'" + trnDate + "','" + argCallSheet.CallFF + "'," + argCallSheet.StatusId + "," + argCallSheet.NatureID + "," + argCallSheet.CallTypeID + ",NULL,'" + argCallSheet.Remarks + "'," + argCallSheet.ReminderId + ",'" + argCallSheet.Reminder + "')  SELECT SCOPE_IDENTITY();";
                        cmd = new SqlCommand(sSql, conn, tran);
                        iEntryId = int.Parse(cmd.ExecuteScalar().ToString());
                        cmd.Dispose();

                        sSql = "Update dbo.LeadRegister SET NextCallDate=NULL,CallTypeId=" + argCallSheet.CallTypeID + ", " +
                            "Email='" + argCallSheet.Email + "' WHERE LeadId=" + argCallSheet.LeadID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Update dbo.LeadCommAddressInfo SET Email='" + argCallSheet.Email + "' " +
                            "WHERE LeadId=" + argCallSheet.LeadID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    else
                    {
                        sSql = "INSERT INTO dbo.CallSheet(ExecutiveId,LeadId,ProjectId,TrnDate,CallFF,StatusID,NatureID,CallTypeID,NextCallDate,Remarks,ReminderId,Reminder) " +
                            "VALUES (" + argCallSheet.ExecutiveID + "," + argCallSheet.LeadID + "," + argCallSheet.ProjID + ",'" + trnDate + "','" + argCallSheet.CallFF + "'," + argCallSheet.StatusId + "," + argCallSheet.NatureID + "," + argCallSheet.CallTypeID + ",'" + nxtCallDate + "','" + argCallSheet.Remarks + "'," + argCallSheet.ReminderId + ",'" + argCallSheet.Reminder + "')  SELECT SCOPE_IDENTITY();";

                        cmd = new SqlCommand(sSql, conn, tran);
                        iEntryId = int.Parse(cmd.ExecuteScalar().ToString());
                        cmd.Dispose();

                        sSql = "Update dbo.LeadRegister SET NextCallDate='" + nxtCallDate + "',CallTypeId=" + argCallSheet.CallTypeID + ", " +
                            "Email='" + argCallSheet.Email + "' WHERE LeadId=" + argCallSheet.LeadID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Update dbo.LeadCommAddressInfo SET Email='" + argCallSheet.Email + "' " +
                            "WHERE LeadId=" + argCallSheet.LeadID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    //CommFun.InsertLog(DateTime.Now, "CallSheet Entry-Add", "N", "Add CallSheet Entry", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                    BsfGlobal.InsertLog(DateTime.Now, "CallSheet Entry-Add", "N", "Add CallSheet Entry", argCallSheet.LeadID, argCallSheet.ProjID, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                }
                else
                {
                    if (argmode != "A") { iEntryId = argCallSheet.EntryID; }
                    if (argCallSheet.CallType == "Drop" || argCallSheet.CallType == "Finalisation" || argCallSheet.CallType == "Finalization")
                    {
                        sSql = "Update dbo.Callsheet SET ExecutiveId=" + argCallSheet.ExecutiveID + ",LeadId=" + argCallSheet.LeadID + ",ProjectId=" + argCallSheet.ProjID + ",TrnDate='" + trnDate + "',CallFF='" + argCallSheet.CallFF + "',StatusID=" + argCallSheet.StatusId + ", " +
                            "NatureID=" + argCallSheet.NatureID + ",CallTypeID=" + argCallSheet.CallTypeID + ",NextCallDate=NULL, " +
                            "Remarks='" + argCallSheet.Remarks + "',ReminderID=0,Reminder='" + argCallSheet.Reminder + "' " +
                            "WHERE EntryID=" + argCallSheet.EntryID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Update dbo.LeadRegister SET NextCallDate=NULL,CallTypeId=" + argCallSheet.CallTypeID + ", " +
                            "Email='" + argCallSheet.Email + "' WHERE LeadId=" + argCallSheet.LeadID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Update dbo.LeadCommAddressInfo SET Email='" + argCallSheet.Email + "' " +
                            "WHERE LeadId=" + argCallSheet.LeadID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    else
                    {
                        sSql = "Update dbo.Callsheet SET ExecutiveId=" + argCallSheet.ExecutiveID + ",LeadId=" + argCallSheet.LeadID + ",ProjectId=" + argCallSheet.ProjID + ",TrnDate='" + trnDate + "',CallFF='" + argCallSheet.CallFF + "',StatusID=" + argCallSheet.StatusId + ", " +
                        "NatureID=" + argCallSheet.NatureID + ",CallTypeID=" + argCallSheet.CallTypeID + ",NextCallDate='" + nxtCallDate + "', " +
                        "Remarks='" + argCallSheet.Remarks + "',ReminderID=" + argCallSheet.ReminderId + ",Reminder='" + argCallSheet.Reminder + "' " +
                        "WHERE EntryID=" + argCallSheet.EntryID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Update dbo.LeadRegister SET NextCallDate='" + nxtCallDate + "',CallTypeId=" + argCallSheet.CallTypeID + ", " +
                            "Email='" + argCallSheet.Email + "' WHERE LeadId=" + argCallSheet.LeadID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        sSql = "Update dbo.LeadCommAddressInfo SET Email='" + argCallSheet.Email + "' " +
                            "WHERE LeadId=" + argCallSheet.LeadID + "";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    //CommFun.InsertLog(DateTime.Now, "CallSheet Entry-Edit", "E", "Edit CallSheet Entry", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                    BsfGlobal.InsertLog(DateTime.Now, "CallSheet Entry-Edit", "E", "Edit CallSheet Entry", argCallSheet.LeadID, argCallSheet.ProjID, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                }
                sSql = "";
                if (argCallSheet.CallType == "Finalization" || argCallSheet.CallType == "Finalisation" || argCallSheet.CallType == "Block")
                {
                    if (dtEnqTrans != null)
                    {
                        if (dtEnqTrans.Rows[0]["ValidUpto"].ToString() != "")
                        {
                            validUpto = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(dtEnqTrans.Rows[0]["ValidUpto"].ToString()));
                        }
                        if (dtEnqTrans.Rows[0]["FinaliseDate"].ToString() != "")
                        {
                            FinaliseDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(dtEnqTrans.Rows[0]["FinaliseDate"].ToString()));
                        }
                        if (argmode == "A")
                        {
                            if (dtEnqTrans.Rows.Count > 0)
                            {
                                sSql = "INSERT INTO dbo.BuyerDetail(CostCentreId,ExecutiveId,LeadId,EntryId,FlatId,PaySchId,Status,CustomerType," +
                                    " PaymentOption,BranchId,LoanPer,LoanAccNo,BrokerId,BrokerComm,BrokerAmount,ValidUpto,FinaliseDate,PostSaleExecId,Advance,PlotId,RegDate) Values" +
                                    " (" + argCallSheet.ProjID + "," + argCallSheet.ExecutiveID + "," + argCallSheet.LeadID + "," +
                                    " " + iEntryId + ",0," + dtEnqTrans.Rows[0]["PaySchId"] + ",'" + dtEnqTrans.Rows[0]["Status"] + "'," +
                                    " '" + dtEnqTrans.Rows[0]["CustomerType"] + "','" + dtEnqTrans.Rows[0]["PaymentOption"] + "'," +
                                    " " + dtEnqTrans.Rows[0]["BranchId"] + "," + dtEnqTrans.Rows[0]["LoanPer"] + ",'" + dtEnqTrans.Rows[0]["LoanAccNo"] + "'," +
                                    " " + dtEnqTrans.Rows[0]["BrokerId"] + "," + dtEnqTrans.Rows[0]["ComPer"] + "," + dtEnqTrans.Rows[0]["ComAmount"] + "," +
                                    " '" + validUpto + "','" + FinaliseDate + "'," + dtEnqTrans.Rows[0]["PostSaleExecId"] + "," + dtEnqTrans.Rows[0]["AdvAmt"] + "," +
                                    " " + dtEnqTrans.Rows[0]["PlotId"] + ",'" + string.Format(Convert.ToDateTime(dtEnqTrans.Rows[0]["RegDate"]).ToString("dd-MMM-yyyy")) + "') ";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }

                            if (argCallSheet.CallType == "Block")
                                sSql = "Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails Set Status='B',BuyerId=" + argCallSheet.LeadID + ",ExecutiveId=" + dtEnqTrans.Rows[0]["PostSaleExecId"] + " WHERE PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + " ";
                            else
                                sSql = "Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails Set Status='S',BuyerId=" + argCallSheet.LeadID + ",ExecutiveId=" + dtEnqTrans.Rows[0]["PostSaleExecId"] + " WHERE PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + " ";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            if (argCallSheet.CallType == "Block")
                            {
                                sSql = "INSERT INTO dbo.BlockUnits(CostCentreId,PlotId,CustomerType,LeadId,BlockUpto,Date,BlockType) Values(" + argCallSheet.ProjID + "," +
                                 " " + dtEnqTrans.Rows[0]["PlotId"] + ",'" + dtEnqTrans.Rows[0]["CustomerType"] + "'," + argCallSheet.LeadID + ",'" + validUpto + "',"+
                                " '" + String.Format(Convert.ToDateTime(DateTime.Now).ToString("MM-dd-yyyy")) + "','B')";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }

                            sSql = "INSERT INTO dbo.LeadFlatInfo (LeadId,Buyer,PlotId,CostCentreId)VALUES(" + argCallSheet.LeadID + ",1," + dtEnqTrans.Rows[0]["PlotId"] + "," +argCallSheet.ProjID + ")";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            
                        }
                        else
                        {
                            iEntryId = Convert.ToInt32(dtEnqTrans.Rows[0]["CallSheetEntryId"]);
                            sSql = "Update dbo.BuyerDetail Set CostCentreId=" + argCallSheet.ProjID + ",ExecutiveId=" + argCallSheet.ExecutiveID + "," +
                                " LeadId=" + argCallSheet.LeadID + ",EntryId=" + iEntryId + ",FlatId=0,PaySchId=" + dtEnqTrans.Rows[0]["PaySchId"] + "," +
                                " Status='" + dtEnqTrans.Rows[0]["Status"] + "',CustomerType='" + dtEnqTrans.Rows[0]["CustomerType"] + "'," +
                                " PaymentOption='" + dtEnqTrans.Rows[0]["PaymentOption"] + "',BranchId=" + dtEnqTrans.Rows[0]["BranchId"] + "," +
                                " LoanPer=" + dtEnqTrans.Rows[0]["LoanPer"] + ",LoanAccNo='" + dtEnqTrans.Rows[0]["LoanAccNo"] + "',BrokerId=" + dtEnqTrans.Rows[0]["BrokerId"] + "," +
                                " BrokerComm=" + dtEnqTrans.Rows[0]["ComPer"] + ",BrokerAmount=" + dtEnqTrans.Rows[0]["ComAmount"] + "," +
                                " ValidUpto='" + validUpto + "',FinaliseDate='" + FinaliseDate + "'," +
                                " PostSaleExecId=" + dtEnqTrans.Rows[0]["PostSaleExecId"] + ",Advance=" + dtEnqTrans.Rows[0]["AdvAmt"] + "," +
                                " PlotId=" + dtEnqTrans.Rows[0]["PlotId"] + ",RegDate='" + string.Format(Convert.ToDateTime(dtEnqTrans.Rows[0]["RegDate"]).ToString("dd-MMM-yyyy")) + "'" +
                                " WHERE EntryId=" + iEntryId + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            if (argCallSheet.CallType == "Block")
                                sSql = "Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails Set Status='B',BuyerId=" + argCallSheet.LeadID + " WHERE PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + " ";
                            else
                                sSql = "Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails Set Status='S',BuyerId=" + argCallSheet.LeadID + " WHERE PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + " ";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            if (argCallSheet.CallType == "Block")
                            {
                                sSql = "UPDATE dbo.BlockUnits SET CustomerType='" + dtEnqTrans.Rows[0]["CustomerType"] + "',"+
                                    " LeadId=" + argCallSheet.LeadID + ", BlockUpto='" + validUpto + "' "+
                                    " WHERE PlotId=" + dtEnqTrans.Rows[0]["PlotId"] + " And BlockType='B'";
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                            }

                            

                            //sSql = "Update dbo.FollowUp Set ExecutiveId=" + dtEnqTrans.Rows[0]["PostSaleExecId"] + "" +
                            //       " Where FlatId=" + argCallSheet.FlatID + " AND CostCentreId=" + argCallSheet.ProjID + "";
                            //cmd = new SqlCommand(sSql, conn, tran);
                            //cmd.ExecuteNonQuery();
                            //cmd.Dispose();
                        }
                    }

                    sSql = "Select Rate,AdvanceAmount From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails Where PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dtA = new DataTable();
                    dtA.Load(dr);
                    if (dtA.Rows.Count > 0)
                    {
                        dAdvAmt = Convert.ToDecimal(dtA.Rows[0]["AdvanceAmount"]);
                        dRate = Convert.ToDecimal(dtA.Rows[0]["Rate"]);
                        if (dAdvAmt != Convert.ToDecimal(dtEnqTrans.Rows[0]["AdvAmt"]))
                        {
                            sSql = "Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails Set AdvanceAmount=" + dtEnqTrans.Rows[0]["AdvAmt"] + " WHERE PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + " ";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            CallsheetentryDL.InsertPlotScheduleI(Convert.ToInt32(dtEnqTrans.Rows[0]["PlotId"]), conn, tran);
                            //PaymentScheduleDL.InsertFlatScheduleI(argCallSheet.FlatID, conn, tran);
                        }
                    }

                    SqlDataReader sdr, sdr1, sdr2; DataTable dt, dt1; int iStgId = 0, iTemplateId = 0; int ipre = 0;
                    int iDateAfter = 0, iDuration = 0; string sDurType = ""; DateTime SchDate; int iSortOrder = 0; DateTime StartDate = DateTime.Now;
                    sSql = "Select TemplateId,PreStageTypeId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot Where PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + " And PreStageTypeId=-1";
                    cmd = new SqlCommand(sSql, conn, tran);
                    sdr = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(sdr); cmd.Dispose();

                    if (dt.Rows.Count > 0)
                    {
                        iStgId = Convert.ToInt32(dt.Rows[0]["PreStageTypeId"]);
                        iTemplateId = Convert.ToInt32(dt.Rows[0]["TemplateId"]);
                    }
                    dt.Dispose();

                    sSql = "Select SortOrder From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot Where PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + " And TemplateId=" + iTemplateId + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    sdr2 = cmd.ExecuteReader();
                    dt1 = new DataTable();
                    dt1.Load(sdr2); cmd.Dispose();
                    dt1.Dispose();

                    if (dt1.Rows.Count > 0)
                    {
                        iSortOrder = Convert.ToInt32(dt1.Rows[0]["SortOrder"]);
                    }

                    sSql = "Select StartDate from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandBankRegister Where LandRegisterId= " + argLandId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    dt = new DataTable();
                    sdr = cmd.ExecuteReader();
                    dt.Load(sdr);
                    dt.Dispose();

                    if (dt.Rows.Count > 0)
                        StartDate = Convert.ToDateTime(dt.Rows[0]["StartDate"]);

                    sSql = "Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot Set SchDate='" + FinaliseDate + "'" +
                        " Where TemplateId=" + iTemplateId + " And PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot Set SchDate='" + FinaliseDate + "'" +
                        " Where TemplateId=0 And PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (iStgId == -1)
                    {
                        if (iStgId == -1)
                            sSql = "Select A.PreStageTypeId,A.LandRegId,A.TemplateId,A.DateAfter,A.Duration,A.Durationtype from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot A" +
                            " Left Join dbo.PlotProgressBillRegister B On A.PlotDetailsId=B.PlotDetailsId " +
                            " Where A.PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + " And A.SortOrder>=" + iSortOrder + "" +
                            " And A.PaymentSchId Not In " +
                            " (Select PaySchId From dbo.PlotProgressBillRegister Where PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + ") Order By A.SortOrder";

                        cmd = new SqlCommand(sSql, conn, tran);
                        sdr1 = cmd.ExecuteReader();
                        dt = new DataTable();
                        dt.Load(sdr1);
                        cmd.Dispose();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            iTemplateId = Convert.ToInt32(dt.Rows[i]["TemplateId"]);
                            ipre = Convert.ToInt32(dt.Rows[i]["PreStageTypeId"]);
                            iDateAfter = Convert.ToInt32(dt.Rows[i]["DateAfter"]);
                            iDuration = Convert.ToInt32(dt.Rows[i]["Duration"]);
                            sDurType = dt.Rows[i]["DurationType"].ToString();
                            //SchDate = Convert.ToDateTime(FinaliseDate);

                            if (ipre == -1) { } else if (ipre == -2) { } else if (ipre == -3) { } else if (ipre == 0) { } else { iTemplateId = ipre; }

                            sSql = "Select SchDate From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot Where LandRegId=" + dt.Rows[i]["LandRegId"] + " And PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + "" +
                                  " And TemplateId=" + iTemplateId + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            DataTable dtDate = new DataTable();
                            sdr = cmd.ExecuteReader();
                            dtDate.Load(sdr);
                            dtDate.Dispose();

                            if (ipre == -1) { SchDate = Convert.ToDateTime(CommFun.IsNullCheck(FinaliseDate, CommFun.datatypes.VarTypeDate)); }
                            else if (ipre == -2) { SchDate = StartDate; }
                            else
                                SchDate = Convert.ToDateTime(CommFun.IsNullCheck(dtDate.Rows[0]["SchDate"], CommFun.datatypes.VarTypeDate));

                            //if (i == 0) SchDate = Convert.ToDateTime(FinaliseDate);
                            //else SchDate = Convert.ToDateTime(CommFun.IsNullCheck(dtDate.Rows[0]["SchDate"], CommFun.datatypes.VarTypeDate));

                            if (sDurType == "D")
                            { if (iDateAfter == 0) SchDate = SchDate.AddDays(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }
                            else if (sDurType == "M")
                            { if (iDateAfter == 0) SchDate = SchDate.AddMonths(iDuration); else  SchDate = SchDate.AddDays(-iDuration); }


                            sSql = "Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot Set SchDate='" + string.Format(Convert.ToDateTime(SchDate).ToString("dd-MMM-yyyy")) + "'" +
                                " Where TemplateId=" + Convert.ToInt32(dt.Rows[i]["TemplateId"]) + " And PlotDetailsId=" + dtEnqTrans.Rows[0]["PlotId"] + "";
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                        }
                    }

                    InsertPlotChangeRate(Convert.ToInt32(dtEnqTrans.Rows[0]["PlotId"]), dRate, Convert.ToDecimal(dtEnqTrans.Rows[0]["Rate"]), conn, tran);

                    //UnitDirDL.InsertFlatCheckList(dtFinal, argCallSheet.FlatID, "F", argChkSend, argCallSheet.ProjID,argFlatNo);
                    if (dtFinal != null || dtLand!=null)
                        UnitDirDL.InsertPlotChk(dtFinal, Convert.ToInt32(dtEnqTrans.Rows[0]["PlotId"]), "F", argChkSend, argFlatNo, argCallSheet.ProjID, conn, tran,dtLand);
                }
                tran.Commit();

                if (argCallSheet.CallType == "Finalization" || argCallSheet.CallType == "Finalisation" || argCallSheet.CallType == "Block")
                {
                    if (argmode == "E")
                    {
                        BsfGlobal.InsertLog(DateTime.Now, "Flat-Finalisation-Modify", "N", "Flat Finalisation", argCallSheet.FlatID, argCallSheet.ProjID, 0, BsfGlobal.g_sCRMDBName, argFlatNo, BsfGlobal.g_lUserId, Convert.ToDecimal(dtEnqTrans.Rows[0]["AdvAmt"]), 0);
                    }
                    else
                    {
                        BsfGlobal.InsertLog(DateTime.Now, "Flat-Finalisation-Create", "C", "Flat Finalisation", argCallSheet.FlatID, argCallSheet.ProjID, 0, BsfGlobal.g_sCRMDBName, argFlatNo, BsfGlobal.g_lUserId, Convert.ToDecimal(dtEnqTrans.Rows[0]["AdvAmt"]), 0);
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
                conn.Close();
            }
            return iEntryId;
        }

        internal static void InsertPlotScheduleI(int argPlotDetId,SqlConnection conn,SqlTransaction tran)
        {
            string sSql = "";

            SqlDataReader sdr;
            SqlCommand cmd;
            DataTable dt = new DataTable();

            int i_LandRegId = 0;
            int i_PlotTypeId = 0;
            int i_PaySchTypeId = 0;
            decimal d_BaseAmt = 0;
            decimal d_AdvAmt = 0;
            decimal d_AdvBalAmt = 0;
            decimal d_LandAmt = 0;
            decimal d_NetAmt = 0;
            decimal d_OtherAmt = 0;
            decimal d_RAmt = 0;
            int i_ReceiptId = 0;
            int i_ROtherCostId = 0;
            string s_RSchType = "";
            bool b_Advance = false;
            int i_PaymentSchId = 0;
            string s_SchType = "";
            int i_OtherCostId = 0;
            decimal d_RPer = 0;
            decimal d_SchPercent = 0;
            decimal d_QBaseAmt = 0;
            decimal d_QNetAmt = 0;
            int i_TemplateId = 0;
            int i_SchId = 0;
            int i_RSchId = 0;
            decimal d_TNetAmt = 0;
            decimal d_BalAmt = 0;
            DataRow[] drT;
            cRateQualR RAQual;
            Collection QualVBC;

            DataTable dtReceipt = new DataTable();


                sSql = String.Format("Delete from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot Where PlotDetailsId={0}", argPlotDetId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = String.Format("Delete from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptQualifier Where SchId IN(Select SchId from" +
                    " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType Where PlotDetailsId={0})", argPlotDetId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = String.Format("Delete from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType Where PlotDetailsId={0}", argPlotDetId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = String.Format("Select PlotTypeId, LandRegisterId, PaymentScheduleId, BaseAmount, AdvanceAmount," +
                    " OtherCost, Rate from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails Where PlotDetailsId={0}", argPlotDetId);
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(sdr);

                sdr.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    i_LandRegId = Convert.ToInt32(dt.Rows[0]["LandRegisterId"].ToString());
                    i_PlotTypeId = Convert.ToInt32(dt.Rows[0]["PlotTypeId"].ToString());
                    i_PaySchTypeId = Convert.ToInt32(dt.Rows[0]["PaymentScheduleId"].ToString());
                    d_BaseAmt = Convert.ToDecimal(dt.Rows[0]["BaseAmount"].ToString());
                    d_AdvAmt = Convert.ToDecimal(dt.Rows[0]["AdvanceAmount"].ToString());
                    d_LandAmt = Convert.ToDecimal(dt.Rows[0]["BaseAmount"].ToString());
                    //d_OtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["OtherCost"],
                    //    CommFun.datatypes.vartypenumeric));
                }
                dt.Dispose();

                sSql = String.Format("Select TemplateId From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedule Where TypeId={0} and LandRegId={1}" +
                    " and SchType='A'", i_PaySchTypeId, i_LandRegId);
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0) { b_Advance = true; }
                dt.Dispose();

                sSql = String.Format("Select Sum(Case When Sign='-' then Amount*(-1) else Amount End) Amount from" +
                    " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotDetailsOtherCost Where PlotDetailsId={0} AND OtherCostId in (Select OtherCostId from" +
                    " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.OtherCostSetupTrans Where PayTypeId={1} and LandRegId={2})", argPlotDetId, i_PaySchTypeId, i_LandRegId);
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(sdr);
                sdr.Close();
                cmd.Dispose();
                dt.Dispose();

                if (dt.Rows.Count > 0)
                    d_OtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"],
                        CommFun.datatypes.vartypenumeric));


                if (b_Advance == true)
                    d_NetAmt = d_BaseAmt + d_OtherAmt - d_AdvAmt;
                else
                    d_NetAmt = d_BaseAmt + d_OtherAmt;

                sSql = String.Format("Insert into [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot(PlotDetailsId, TemplateId, LandRegId, SchType," +
                    " Description, SchDescId, OtherCostId, SchDate, DateAfter, Duration, DurationType, SchPercent, Amount," +
                    " PreStageTypeId, SortOrder) Select {0}, TemplateId, LandRegId, SchType, Description, SchDescId," +
                    " OtherCostId, SchDate, DateAfter, Duration, DurationType, SchPercent, Amount, PreStageTypeId, SortOrder" +
                    " from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedule Where TypeId={1} and LandRegId={2}", argPlotDetId, i_PaySchTypeId, i_LandRegId);
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();


                sSql = String.Format("Select ReceiptTypeId, OtherCostId, SchType from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ReceiptTypeOrder Where PayTypeId={0}" +
                    " and LandRegId={1} and SchType<>'A' Order by SortOrder", i_PaySchTypeId, i_LandRegId);
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                DataTable dtReceiptOrder = new DataTable();
                dtReceiptOrder.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                sSql = String.Format("Select OtherCostId, Sign, Amount from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotDetailsOtherCost Where PlotDetailsId={0}",
                    argPlotDetId);
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

                DataRow drR = dtReceipt.NewRow();
                drR["Id"] = 1;
                drR["SchType"] = "A";
                drR["Amount"] = d_AdvAmt;
                drR["RAmount"] = 0;
                dtReceipt.Rows.Add(drR);

                drR = dtReceipt.NewRow();
                drR["Id"] = 2;
                drR["SchType"] = "R";
                drR["Amount"] = d_LandAmt;
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

                sSql = String.Format("Select SchId, TemplateId, ReceiptTypeId, Percentage, OtherCostId, SchType from" +
                    " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandReceiptType Where TemplateId in (Select TemplateId from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedule Where TypeId={0}" +
                    " AND LandRegId={1}) Order by SortOrder", i_PaySchTypeId, i_LandRegId);
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                DataTable dtTemp = new DataTable();
                dtTemp.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                sSql = String.Format("Select * from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandReceiptQualifier Where LandRegId={0}", i_LandRegId);
                cmd = new SqlCommand(sSql, conn, tran);
                sdr = cmd.ExecuteReader();
                DataTable dtQual = new DataTable();
                dtQual.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                sSql = String.Format("Select PaymentSchId, TemplateId, SchType, OtherCostId, SchPercent from" +
                    " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot Where PlotDetailsId={0} Order by SortOrder", argPlotDetId);
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
                d_AdvBalAmt = d_AdvAmt;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    i_PaymentSchId = Convert.ToInt32(dt.Rows[i]["PaymentSchId"].ToString());
                    i_TemplateId = Convert.ToInt32(dt.Rows[i]["TemplateId"].ToString());
                    s_SchType = dt.Rows[i]["SchType"].ToString();
                    i_OtherCostId = Convert.ToInt32(dt.Rows[i]["OtherCostId"].ToString());
                    d_SchPercent = Convert.ToDecimal(dt.Rows[i]["SchPercent"].ToString());
                    d_TNetAmt = 0;

                    dAmt = 0;
                    if (s_SchType == "A")
                        dAmt = d_AdvAmt;
                    else if (s_SchType == "O")
                    {
                        dv = new DataView(dtT);
                        dv.RowFilter = "OtherCostId = " + i_OtherCostId;
                        if (dv.ToTable().Rows.Count > 0)
                        {
                            if (dv.ToTable().Rows[0]["Sign"].ToString() == "-")
                                dAmt = Convert.ToDecimal(dv.ToTable().Rows[0]["Amount"].ToString()) * (-1);
                            else
                                dAmt = Convert.ToDecimal(dv.ToTable().Rows[0]["Amount"].ToString());
                        }
                        dv.Dispose();
                    }
                    else
                    {
                        dAmt = d_NetAmt * d_SchPercent / 100;
                    }

                    dtTempT = new DataTable();
                    dv = new DataView(dtTemp);
                    dv.RowFilter = "TemplateId = " + i_TemplateId;
                    dtTempT = dv.ToTable();
                    dv.Dispose();

                    if (dtTempT.Rows.Count == 1 && s_SchType == "O")
                    {
                        sSql = String.Format("Select Amount from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotDetailsOtherCost" +
                            " Where PlotDetailsId={0} AND OtherCostId={1}", argPlotDetId, i_OtherCostId);
                        cmd = new SqlCommand(sSql, conn, tran);
                        decimal d_Amtt = Convert.ToDecimal(CommFun.IsNullCheck(cmd.ExecuteScalar(),
                            CommFun.datatypes.vartypenumeric));
                        cmd.Dispose();

                        dAmt = d_Amtt;

                        sSql = String.Format("Insert into [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType(PaymentSchId, PlotDetailsId, ReceiptTypeId," +
                            " OtherCostId, SchType, Percentage, Amount, NetAmount) Values({0}, {1}, 0, {2}, '{3}', 100, {4}," +
                            " {4}) SELECT SCOPE_IDENTITY();", i_PaymentSchId, argPlotDetId, i_OtherCostId, s_SchType, dAmt);
                        cmd = new SqlCommand(sSql, conn, tran);
                        i_RSchId = int.Parse(cmd.ExecuteScalar().ToString());
                        cmd.Dispose();

                        drT = dtReceipt.Select(String.Format("SchType = 'O' and Id = {0}", i_OtherCostId));

                        if (drT.Length > 0)
                            drT[0]["RAmount"] = dAmt;

                        d_QNetAmt = dAmt;

                        dtQualT = new DataTable();
                        dv = new DataView(dtQual);
                        dv.RowFilter = String.Format("SchType = '{0}' and OtherCostId = {1}", s_SchType, i_OtherCostId);
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

                            using (Qualifier.frmQualifier qul = new Qualifier.frmQualifier())
                            {
                                d_QBaseAmt = dAmt;
                                d_QNetAmt = dAmt; decimal dTaxAmt = 0;
                                decimal dVATAmt = 0;
                                if (qul.Execute("B", ref d_QBaseAmt, ref QualVBC, ref d_QNetAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                                {
                                    foreach (cRateQualR d in QualVBC)
                                    {
                                        sSql = String.Format("Insert into [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptQualifier(SchId, QualifierId, Expression," + " ExpPer, Add_Less_Flag, SurCharge, EDCess, ExpValue, ExpPerValue, SurValue, EDValue," + " Amount) Values({0}, {1}, '{2}', {3}, '{4}', {5}, {6}, {7}, {8}, {9}, {10}, {11})", i_RSchId, d.RateID, d.Expression, d.ExpPer, d.Add_Less_Flag, d.SurPer, d.EDPer, d.ExpValue, d.ExpPerValue, d.SurValue, d.EDValue, d.Amount);
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                }
                                sSql = String.Format("Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType Set NetAmount={0} Where SchId={1}", d_QNetAmt, i_RSchId);
                                cmd = new SqlCommand(sSql, conn, tran);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }

                        sSql = String.Format("Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot Set Amount={0}, NetAmount={1} Where" +
                            " PaymentSchId={2}", dAmt, d_QNetAmt, i_PaymentSchId);
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        d_TNetAmt = d_TNetAmt + d_QNetAmt;
                    }
                    else
                    {
                        d_BalAmt = dAmt;
                        for (int j = 0; j < dtTempT.Rows.Count; j++)
                        {
                            i_SchId = Convert.ToInt32(dtTempT.Rows[j]["SchId"].ToString());
                            d_RPer = Convert.ToDecimal(dtTempT.Rows[j]["Percentage"].ToString());
                            s_RSchType = dtTempT.Rows[j]["SchType"].ToString();
                            i_ReceiptId = Convert.ToInt32(dtTempT.Rows[j]["ReceiptTypeId"].ToString());
                            i_ROtherCostId = Convert.ToInt32(dtTempT.Rows[j]["OtherCostId"].ToString());

                            if (d_RPer != 0)
                                d_RAmt = dAmt * d_RPer / 100;
                            else
                                d_RAmt = d_BalAmt;

                            if (d_RAmt > d_BalAmt)
                                d_RAmt = d_BalAmt;

                            if (s_RSchType == "A" && b_Advance == false)
                            {
                                dAdvRAmt = d_AdvAmt * d_RPer / 100;

                                if (dAdvRAmt > d_AdvBalAmt)
                                    dAdvRAmt = d_AdvBalAmt;

                                d_AdvBalAmt = d_AdvBalAmt - dAdvRAmt;
                                d_TNetAmt = d_TNetAmt - dAdvRAmt;

                                sSql = String.Format("Insert into [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType(PaymentSchId, PlotDetailsId," +
                                    " ReceiptTypeId, OtherCostId, SchType, Percentage, Amount, NetAmount) Values({0}, {1}," +
                                    " {2}, {3}, '{4}', {5}, 0, {6}) SELECT SCOPE_IDENTITY();", i_PaymentSchId, argPlotDetId,
                                    i_ReceiptId, i_ROtherCostId, s_RSchType, d_RPer, dAdvRAmt);

                                cmd = new SqlCommand(sSql, conn, tran);
                                i_RSchId = int.Parse(cmd.ExecuteScalar().ToString());
                                cmd.Dispose();
                            }
                            else
                            {
                                if (s_RSchType == "A")
                                    drT = dtReceipt.Select("SchType = 'A'");
                                else if (s_RSchType == "O")
                                    drT = dtReceipt.Select(String.Format("SchType='O' and Id={0}", i_ROtherCostId));
                                else
                                    drT = dtReceipt.Select(String.Format("SchType='R' and Id={0}", i_ReceiptId));

                                decimal dRTAmt = 0;
                                decimal dRRAmt = 0;

                                if (drT.Length > 0)
                                {
                                    dRTAmt = Convert.ToDecimal(drT[0]["Amount"].ToString());
                                    dRRAmt = Convert.ToDecimal(drT[0]["RAmount"].ToString());
                                }

                                if (d_RAmt > (dRTAmt - dRRAmt))
                                    d_RAmt = dRTAmt - dRRAmt;

                                if (drT.Length > 0)
                                    drT[0]["RAmount"] = dRRAmt + d_RAmt;

                                if (dAmt == 0)
                                    d_RPer = 0;
                                else
                                    d_RPer = (d_RAmt / dAmt) * 100;

                                d_BalAmt = d_BalAmt - d_RAmt;

                                d_QNetAmt = d_RAmt;

                                if (drT.Length > 0)
                                {
                                    if (d_BalAmt > 0 || d_RAmt > 0)
                                    {
                                        if (d_RPer == 0) d_RPer = 100;

                                        sSql = String.Format("Insert into [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType(PaymentSchId, PlotDetailsId," +
                                            " ReceiptTypeId, OtherCostId, SchType, Percentage, Amount, NetAmount) Values({0}, {1}, {2}," +
                                            " {3}, '{4}', {5}, {6}, {6}) SELECT SCOPE_IDENTITY();", i_PaymentSchId, argPlotDetId,
                                            i_ReceiptId, i_ROtherCostId, s_RSchType, d_RPer, d_RAmt);

                                        cmd = new SqlCommand(sSql, conn, tran);
                                        i_RSchId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                                        cmd.Dispose();

                                        d_QNetAmt = d_RAmt;

                                        dtQualT = new DataTable();
                                        dv = new DataView(dtQual);
                                        dv.RowFilter = String.Format("SchType = '{0}' and ReceiptTypeId = {1} and OtherCostId = {2}", s_RSchType, i_ReceiptId, i_ROtherCostId);
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

                                            using (Qualifier.frmQualifier qul = new Qualifier.frmQualifier())
                                            {
                                                d_QBaseAmt = d_RAmt;
                                                d_QNetAmt = d_RAmt; decimal dTaxAmt = 0;
                                                decimal dVATAmt = 0;
                                                if (qul.Execute("B", ref d_QBaseAmt, ref QualVBC, ref d_QNetAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                                                {
                                                    foreach (cRateQualR d in QualVBC)
                                                    {
                                                        sSql = String.Format("Insert into [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptQualifier(SchId, QualifierId," + " Expression, ExpPer, Add_Less_Flag, SurCharge, EDCess, ExpValue, ExpPerValue," + " SurValue, EDValue, Amount) Values({0}, {1}, '{2}', {3}, '{4}', {5}, {6}, {7}," + " {8}, {9}, {10}, {11})", i_RSchId, d.RateID, d.Expression, d.ExpPer, d.Add_Less_Flag, d.SurPer, d.EDPer, d.ExpValue, d.ExpPerValue, d.SurValue, d.EDValue, d.Amount);
                                                        cmd = new SqlCommand(sSql, conn, tran);
                                                        cmd.ExecuteNonQuery();
                                                        cmd.Dispose();
                                                    }
                                                }
                                                sSql = String.Format("Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType Set NetAmount={0} Where SchId={1}", d_QNetAmt, i_RSchId);
                                                cmd = new SqlCommand(sSql, conn, tran);
                                                cmd.ExecuteNonQuery();
                                                cmd.Dispose();
                                            }
                                        }
                                    }
                                }
                                d_TNetAmt = d_TNetAmt + d_QNetAmt;
                            }
                        }

                        if (d_BalAmt > 0)
                        {
                            // d_TNetAmt = 0;
                            for (int j = 0; j < dtReceiptOrder.Rows.Count; j++)
                            {
                                d_RAmt = d_BalAmt;

                                s_RSchType = dtReceiptOrder.Rows[j]["SchType"].ToString();
                                i_ReceiptId = Convert.ToInt32(dtReceiptOrder.Rows[j]["ReceiptTypeId"].ToString());
                                i_ROtherCostId = Convert.ToInt32(dtReceiptOrder.Rows[j]["OtherCostId"].ToString());

                                if (s_RSchType == "O")
                                    drT = dtReceipt.Select(String.Format("SchType = 'O' and Id = {0}", i_ROtherCostId));
                                else
                                    drT = dtReceipt.Select(String.Format("SchType = 'R' and Id = {0}", i_ReceiptId));

                                //decimal dRTAmt = 0;
                                //decimal dRRAmt = 0;

                                //if (drT.Length > 0)
                                //{
                                //    dRTAmt = Convert.ToDecimal(drT[0]["Amount"].ToString());
                                //    dRRAmt = Convert.ToDecimal(drT[0]["RAmount"].ToString());
                                //}

                                //if (d_RAmt > dRTAmt - dRRAmt)
                                //    d_RAmt = dRTAmt - dRRAmt;

                                //if (drT.Length > 0)
                                //    drT[0]["RAmount"] = dRRAmt + d_RAmt;

                                if (d_RAmt > 0)
                                {
                                    decimal dPCAmt = 0;
                                    bool bAns = false;
                                    sSql = String.Format("Select SchId, Amount, NetAmount from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType Where" +
                                        " PaymentSchId={0} and PlotDetailsId={1} and ReceiptTypeId={2} and OtherCostId={3}" +
                                        " and SchType='{4}'", i_PaymentSchId, argPlotDetId, i_ReceiptId, i_ROtherCostId,
                                        s_RSchType);
                                    cmd = new SqlCommand(sSql, conn, tran);
                                    sdr = cmd.ExecuteReader();
                                    DataTable dtP = new DataTable();
                                    dtP.Load(sdr);
                                    sdr.Close();
                                    cmd.Dispose();

                                    if (dtP.Rows.Count > 0)
                                    {
                                        dPCAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtP.Rows[0]["Amount"],
                                            CommFun.datatypes.vartypenumeric));
                                        //d_TNetAmt = d_TNetAmt - dPCAmt;
                                        //d_BalAmt = dPCAmt;
                                        i_RSchId = Convert.ToInt32(dtP.Rows[0]["SchId"].ToString());
                                        bAns = true;
                                    }
                                    dtP.Dispose();

                                    if (bAns == true)
                                    {
                                        d_RAmt = dPCAmt;
                                        d_RPer = (d_RAmt / dAmt) * 100;

                                        sSql = String.Format("Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType Set Amount={0}, Percentage={1}, NetAmount={0}" +
                                            " Where SchId={2}", d_RAmt, d_RPer, i_RSchId);
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();

                                        sSql = String.Format("Delete from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptQualifier Where SchId = {0}", i_RSchId);
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                    else
                                    {
                                        d_RPer = (d_RAmt / dAmt) * 100;

                                        sSql = String.Format("Insert into [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType(PaymentSchId, PlotDetailsId," +
                                            " ReceiptTypeId, OtherCostId, SchType, Percentage, Amount, NetAmount) Values({0}," +
                                            " {1}, {2}, {3}, '{4}', {5}, {6}, {6}) SELECT SCOPE_IDENTITY();", i_PaymentSchId,
                                            argPlotDetId, i_ReceiptId, i_ROtherCostId, s_RSchType, d_RPer, d_RAmt);
                                        cmd = new SqlCommand(sSql, conn, tran);
                                        i_RSchId = int.Parse(cmd.ExecuteScalar().ToString());
                                        cmd.Dispose();
                                    }

                                    d_QNetAmt = d_RAmt;

                                    dtQualT = new DataTable();
                                    dv = new DataView(dtQual);

                                    if (s_RSchType == "O")
                                        dv.RowFilter = String.Format("SchType = 'O' and ReceiptTypeId = 0 and OtherCostId = {0}", i_ROtherCostId);
                                    else
                                        dv.RowFilter = String.Format("SchType = 'R' and ReceiptTypeId = {0} and OtherCostId = 0", i_ReceiptId);

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

                                        using (Qualifier.frmQualifier qul = new Qualifier.frmQualifier())
                                        {
                                            d_QBaseAmt = d_RAmt;
                                            d_QNetAmt = d_RAmt; decimal dTaxAmt = 0;
                                            decimal dVATAmt = 0;
                                            if (qul.Execute("B", ref d_QBaseAmt, ref QualVBC, ref d_QNetAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                                            {
                                                foreach (cRateQualR d in QualVBC)
                                                {
                                                    sSql = String.Format("Insert into [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptQualifier(SchId, QualifierId," + " Expression, ExpPer, Add_Less_Flag, SurCharge, EDCess, ExpValue," + " ExpPerValue, SurValue, EDValue, Amount) Values({0}, {1}, '{2}', {3}," + " '{4}', {5}, {6}, {7}, {8}, {9}, {10}, {11})", i_RSchId, d.RateID, d.Expression, d.ExpPer, d.Add_Less_Flag, d.SurPer, d.EDPer, d.ExpValue, d.ExpPerValue, d.SurValue, d.EDValue, d.Amount);
                                                    cmd = new SqlCommand(sSql, conn, tran);
                                                    cmd.ExecuteNonQuery();
                                                    cmd.Dispose();
                                                }
                                            }
                                            sSql = String.Format("Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType Set NetAmount={0} Where SchId={1}", d_QNetAmt, i_RSchId);
                                            cmd = new SqlCommand(sSql, conn, tran);
                                            cmd.ExecuteNonQuery();
                                            cmd.Dispose();
                                        }
                                    }

                                    d_TNetAmt = d_TNetAmt + d_QNetAmt;
                                    d_BalAmt = d_BalAmt - d_RAmt;
                                    if (d_BalAmt <= 0) { break; }
                                }
                            }
                        }

                        //modified
                        //if (d_TNetAmt > dAmt)
                        sSql = String.Format("Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot Set Amount={0}, NetAmount={1}" +
                            " Where PaymentSchId={2}", dAmt, d_TNetAmt, i_PaymentSchId);
                        //else
                        //    sSql = String.Format("Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot Set Amount={0}, NetAmount={0}" +
                        //        " Where PaymentSchId={1}", dAmt, i_PaymentSchId);

                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
                dt.Dispose();

                if (b_Advance == false)
                {
                    sSql = String.Format("Insert into [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PaymentSchedulePlot(PlotDetailsId, TemplateId, LandRegId, SchType," +
                        " Description, SchDescId, OtherCostId, Amount, NetAmount, PreStageTypeId, SortOrder, SchDate)" +
                        " Values({0}, 0, {1}, 'A', 'Advance', 0, 0, 0, {2}, 0, 0, NULL)", argPlotDetId, i_LandRegId, d_AdvAmt);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            
        }

        public static void InsertChangeRate(int argFlatId,decimal argOldRate,decimal argNewRate,SqlConnection conn,SqlTransaction tran)
        {
            string sSql = "";
            decimal dFBaseAmt = 0; decimal dFNetAmt = 0;

            if (argOldRate != argNewRate)
                {
                    sSql = "Select Area,OtherCostAmt From FlatDetails Where FlatId=" + argFlatId + "";
                    SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    cmd.Dispose();

                    if(dt.Rows.Count>0)
                    {
                        dFBaseAmt = Convert.ToDecimal(dt.Rows[0]["Area"]) * argNewRate;
                        dFNetAmt = dFBaseAmt + Convert.ToDecimal(dt.Rows[0]["OtherCostAmt"]);

                        sSql = "Update FlatDetails Set Rate=" + argNewRate + ", " +
                            " BaseAmt=" + dFBaseAmt + ",NetAmt=" + dFNetAmt + "" +
                            " Where FlatId=" + argFlatId + " ";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        PaymentScheduleDL.InsertFlatScheduleI(argFlatId, conn, tran);
                    }
                }
        }

        public static void InsertPlotChangeRate(int argPlotId, decimal argOldRate, decimal argNewRate, SqlConnection conn, SqlTransaction tran)
        {
            string sSql = "";
            decimal dFBaseAmt = 0; decimal dFNetAmt = 0;

            if (argOldRate != argNewRate)
            {
                sSql = "Select Area,OtherCost From ["+ BsfGlobal.g_sRateAnalDBName +"].dbo.LandPlotDetails Where PlotDetailsId=" + argPlotId + "";
                SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    dFBaseAmt = Convert.ToDecimal(dt.Rows[0]["Area"]) * argNewRate;
                    dFNetAmt = dFBaseAmt + Convert.ToDecimal(dt.Rows[0]["OtherCost"]);

                    sSql = "Update [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails Set Rate=" + argNewRate + ", " +
                        " BaseAmount=" + dFBaseAmt + ",NetAmount=" + dFNetAmt + "" +
                        " Where PlotDetailsId=" + argPlotId + " ";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //PaymentScheduleDL.InsertFlatScheduleI(argPlotId, conn, tran);
                }
            }
        }

        public static DataTable GetCall()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;

            try
            {
                //if (BsfGlobal.FindPermission("Followup-Entry-Allow-Other-Users-Lead") == false)
                //{
                String sSql = "UPDATE LeadProjectInfo SET NextCallDate=L.NextCallDate FROM LeadProjectInfo P " +
                                "JOIN (SELECT LeadId,NextCallDate FROM LeadRegister WHERE NextCallDate IS NOT NULL) L " +
                                "ON P.LeadId=L.LeadId AND P.NextCallDate IS NULL";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "SELECT DISTINCT E.ExecutiveId,P.CampaignId,C.EntryId,C.LeadId,P.CostCentreId ProjectId,C.TrnDate,C.StatusID,C.NatureID,C.CallTypeID, " +
                        " C.Remarks,C.ReminderId,P.NextCallDate,L.LeadName,C.CallFF Call,L.Mobile,L.Email FROM dbo.CallSheet C " +
                        " INNER JOIN dbo.LeadRegister L  ON C.LeadId=L.LeadId " +
                        " INNER Join dbo.LeadExecutiveInfo E On E.LeadId=L.LeadId " +//And E.CostCentreId=C.ProjectId " +
                        " INNER JOIN dbo.LeadProjectInfo P ON P.LeadId=E.LeadId And P.CostCentreId=E.CostCentreId " +
                        " WHERE E.ExecutiveId=" + BsfGlobal.g_lUserId + " AND L.NextCallDate=C.NextCallDate " +
                        " AND P.NextCallDate<='" + Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy") + "'" +
                        " And C.EntryId In(Select Max(EntryId)EntryId From dbo.CallSheet C " +
                        " INNER JOIN dbo.LeadRegister L  ON C.LeadId=L.LeadId " +
                        " INNER Join dbo.LeadExecutiveInfo E On E.LeadId=L.LeadId " +
                        " INNER JOIN dbo.LeadProjectInfo P ON P.LeadId=E.LeadId And P.CostCentreId=E.CostCentreId " +
                        " WHERE E.ExecutiveId=" + BsfGlobal.g_lUserId + " And C.CallTypeID!=3 " +
                        " AND L.NextCallDate=C.NextCallDate AND P.NextCallDate<='" + Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy") +
                        "' Group By E.LeadId)" +
                        " Order By P.NextCallDate Desc";
                //}
                //else
                //{
                //    sSql = "SELECT E.ExecutiveId,P.CampaignId,C.EntryId,C.LeadId,P.CostCentreId ProjectId,C.TrnDate,C.StatusID,C.NatureID,C.CallTypeID, " +
                //            " C.Remarks,C.ReminderId,C.NextCallDate,L.LeadName,C.CallFF Call,L.Mobile,L.Email FROM dbo.CallSheet C " +
                //            " INNER JOIN dbo.LeadRegister L  ON C.LeadId=L.LeadId " +
                //            " Inner Join dbo.LeadExecutiveInfo E On E.LeadId=L.LeadId " +//And E.CostCentreId=C.ProjectId " +
                //            " INNER JOIN dbo.LeadProjectInfo P ON P.LeadId=E.LeadId " +//And P.CostCentreId=E.CostCentreId " +
                //            " WHERE L.NextCallDate=C.NextCallDate " +
                //            " AND C.NextCallDate<='" + Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy") + "'" +
                //            " And C.EntryId In (Select Max(EntryId)EntryId From dbo.CallSheet C " +
                //            " INNER JOIN dbo.LeadRegister L  ON C.LeadId=L.LeadId " +
                //            " Inner Join dbo.LeadExecutiveInfo E On E.LeadId=L.LeadId " +
                //            " WHERE E.ExecutiveId<>0 And C.CallTypeID!=3 And " +
                //            " L.NextCallDate=C.NextCallDate AND C.NextCallDate<='" + Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy") + "' Group By E.LeadId)" +
                //            " Order By C.NextCallDate ";
                //}

                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
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

        public static DataTable GetEditRegisterCall(int argEntryId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "Select A.EntryId,E.ExecutiveId,A.CallFF,A.LeadId,A.TrnDate,A.CallTypeID,A.StatusID,A.NatureID,A.NextCallDate,A.ReminderId," +
                    " A.Remarks,A.ProjectId,A.NextCallDate,B.Mobile,B.Email From CallSheet A " +
                    " Inner Join LeadRegister B On A.LeadId=B.LeadId Inner Join dbo.LeadExecutiveInfo E ON B.LeadId=E.LeadId " +
                    " Inner Join dbo.LeadProjectInfo P On E.LeadId=P.LeadId " +
                    " Where EntryId=" + argEntryId + "";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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

        public static DataTable GetEditRegisterBuyerDet(int argEntryId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "Select A.AllotmentNo GAllotNo,COAllotNo,CCAllotNo,C.CostCentreName ProjName,A.CostCentreId,A.AccountId,A.ExecutiveId,"+
                              "A.LeadId,A.EntryId CallSheetEntryId,A.FlatId,A.PaySchId,A.Status,A.CustomerType,A.PaymentOption,A.BranchId,A.LoanPer,A.LoanAppDate," + 
                              " A.LoanAccNo,A.BrokerId,A.BrokerComm ComPer,A.BrokerAmount ComAmount,A.ValidUpto,A.FinaliseDate,A.PostSaleExecId,A.Advance AdvAmt,"+
                              "A.Approve,A.AdvRefId,A.PlotId,A.RegDate,B.Rate,B.FlatTypeId From dbo.BuyerDetail A " +
                              " Inner Join dbo.FlatDetails B On A.FlatId=B.FlatId "+
                              " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On B.CostCentreId=C.CostCentreId" +
                              " Where A.EntryId=" + argEntryId + "";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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


        public static DataTable GetComments(int argLeadId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                //sSql = "SELECT Top 1 A.EntryId,C.Description Status,NextCallDate PreviousCallDate,Remarks " +
                //        " FROM ( " +
                //        " Select Distinct Top 1 A.EntryId From CallSheet A " +
                //        " Where A.LeadId=" + argLeadId + " " +
                //        " ORDER BY EntryId DESC) A Inner Join CallSheet B On A.EntryId=B.EntryId " +
                //        " Left Join StatusMaster C On C.StatusID=B.StatusId " +
                //        " Left Join NatureMaster D On D.NatureID=B.NatureId " +
                //        " ORDER BY A.EntryId ";
                String sSql = "SELECT A.EntryId,C.Description Status,NextCallDate PreviousCallDate,Remarks  FROM (  " +
                        " Select A.EntryId From dbo.CallSheet A  Where A.LeadId=" + argLeadId + " ) A " +
                        " Inner Join dbo.CallSheet B On A.EntryId=B.EntryId  " +
                        " Left Join dbo.StatusMaster C On C.StatusID=B.StatusId  " +
                        " Left Join dbo.NatureMaster D On D.NatureID=B.NatureId  ORDER BY A.EntryId DESC";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
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


        public static DataTable GetCampaign()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Select A.CampaignId,A.CampaignName From dbo.CampaignDetails A" +
                                " Inner Join dbo.OpportunityRequest B On A.RequestId=B.RequestId " +
                                " Order By CampaignName ";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
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

        public static DataTable GetRegisterCall()
        {
            DataTable dt=null;
            SqlDataAdapter sda;
            String sSql = "";
            sSql = "SELECT C.LeadId,C.EntryId,Convert(Varchar(10),C.TrnDate,103) Date,OP.CostCentreName,U.UserName ExecutiveName,E.LeadName, CT.Description CallType,S.Description Status," +
                    "N.Description Nature,Convert(Varchar(10),C.NextCallDate,103) NextCallDate FROM dbo.CallSheet C " +
                    "INNER JOIN dbo.LeadRegister E ON C.LeadId=E.LeadId " +
                    "LEFT JOIN dbo.CallType CT ON C.CallTypeID=CT.CallTypeID " +
                    "LEFT JOIN dbo.StatusMaster S ON C.StatusID=S.StatusId " +
                    "LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users U on C.ExecutiveId=U.UserId " +
                    "LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OP on C.ProjectId=OP.CostCentreId " +
                    "LEFT JOIN dbo.NatureMaster N ON C.NatureID=N.NatureId Order by C.TrnDate";
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

        public static DataTable GetNextCallReport()
        {
            DataTable dt=null;
            SqlDataAdapter sda;
            String sSql = "";
            sSql = "Select E.CostCentreName,A.NextCallDate,A.TrnDate CalledDate,B.LeadName,C.Description Status,D.Description Nature from CallSheet A" +
                    " Left Join LeadRegister B on A.LeadId=B.LeadId" +
                    " Left Join StatusMaster C on A.StatusID=C.StatusId" +
                    " Left Join NatureMaster D on A.NatureID=D.NatureId" +
                    " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre E On E.CostCentreId=A.ProjectId" +
                    " Where A.NextCallDate in (Select MAX(NextCallDate) from CallSheet Group by LeadId)" +
                    " and A.CallTypeID =1 AND A.ExecutiveId=" + BsfGlobal.g_lUserId + "";
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

        public static DataTable GetNextCallReportPost()
        {
            DataTable dt=null;
            SqlDataAdapter sda;
            String sSql = "";
            sSql = "Select E.CostCentreName,A.NextCallDate,A.BuyerDate CalledDate,B.LeadName,D.Description Nature from FollowUp A" +
                    " Left Join LeadRegister B on A.LeadId=B.LeadId" +
                    " Left Join NatureMaster D on A.NatureID=D.NatureId" +
                    " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre E On E.CostCentreId=A.CostCentreId" +
                    " AND A.ExecutiveId=" + BsfGlobal.g_lUserId + " ";
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

        public static DataTable GetFromRegisterCall(string argFromDate, string argToDate, bool argOtherExec)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "";
                if (BsfGlobal.FindPermission("Pre-Followup-Allow-Other-Users-Followup") == false)
                {
                    sSql = "SELECT C.LeadId,C.EntryId,Convert(Varchar(10),C.TrnDate,103) Date,OP.CostCentreName,U.UserName ExecutiveName," +
                            " E.LeadName,E.Mobile,C.CallFF Call, CT.Description CallType,S.Description Status," +
                            " N.Description Nature,Convert(Varchar(10),C.NextCallDate,103) NextCallDate,C.Remarks, LC.CampaignName FROM dbo.CallSheet C " +
                            " INNER JOIN dbo.LeadRegister E ON C.LeadId=E.LeadId " +
                            " LEFT JOIN dbo.CallType CT ON C.CallTypeID=CT.CallTypeID " +
                            " LEFT JOIN dbo.StatusMaster S ON C.StatusID=S.StatusId " +
                            " LEFT Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users U on C.ExecutiveId=U.UserId " +
                            " LEFT Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OP on C.ProjectId=OP.CostCentreId " +
                            " LEFT JOIN dbo.NatureMaster N ON C.NatureID=N.NatureId " +
                            " LEFT JOIN dbo.LeadProjectInfo LP ON E.LeadId=LP.LeadId " +
                            " LEFT JOIN dbo.CampaignDetails LC ON LP.CampaignId=LC.CampaignId " +
                            " Where C.TrnDate Between '" + argFromDate + "' And '" + argToDate + "'";

                    //" Order by C.TrnDate";
                    if (argOtherExec == false) { sSql = sSql + " and C.ExecutiveId = " + BsfGlobal.g_lUserId + " Order By C.TrnDate"; }
                    else
                    {
                        sSql = sSql + " AND C.ExecutiveId In " +
                         " (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users Where UserId=" + BsfGlobal.g_lUserId + " or " +
                         " UserId in (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans Where sUserId=" + BsfGlobal.g_lUserId + ")) Order By C.TrnDate";
                    }
                }
                else
                {
                    sSql = "SELECT C.LeadId,C.EntryId,Convert(Varchar(10),C.TrnDate,103) Date,OP.CostCentreName,U.UserName ExecutiveName," +
                            " E.LeadName,E.Mobile,C.CallFF Call, CT.Description CallType,S.Description Status," +
                            " N.Description Nature,Convert(Varchar(10),C.NextCallDate,103) NextCallDate,C.Remarks, LC.CampaignName FROM dbo.CallSheet C " +
                            " INNER JOIN dbo.LeadRegister E ON C.LeadId=E.LeadId " +
                            " LEFT JOIN dbo.CallType CT ON C.CallTypeID=CT.CallTypeID " +
                            " LEFT JOIN dbo.StatusMaster S ON C.StatusID=S.StatusId " +
                            " LEFT Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users U on C.ExecutiveId=U.UserId " +
                            " LEFT Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OP on C.ProjectId=OP.CostCentreId " +
                            " LEFT JOIN dbo.NatureMaster N ON C.NatureID=N.NatureId " +
                            " LEFT JOIN dbo.LeadProjectInfo LP ON E.LeadId=LP.LeadId " +
                            " LEFT JOIN dbo.CampaignDetails LC ON LP.CampaignId=LC.CampaignId " +
                            " Where C.TrnDate Between '" + argFromDate + "' And '" + argToDate + "' ";

                    if (argOtherExec == false)
                    {
                        sSql = sSql + " Order By C.TrnDate";
                    }
                    else
                    {
                        sSql = sSql + " AND C.ExecutiveId In " +
                            " (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users Where UserId=" + BsfGlobal.g_lUserId + " or " +
                            " UserId in (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans Where sUserId=" + BsfGlobal.g_lUserId + ")) Order By C.TrnDate";
                    }
                }

                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
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

        public static DataTable GetFromEditRegisterCall(int argRegId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            String sSql = "";
            sSql = "Select C.LeadId,C.EntryId,Convert(Varchar(10),C.TrnDate,103) Date,OP.CostCentreName,U.UserName ExecutiveName, " +
                    " E.LeadName,E.Mobile,C.CallFF Call, CT.Description CallType,S.Description Status, " +
                    " N.Description Nature,Convert(Varchar(10),C.NextCallDate,103) NextCallDate,C.Remarks From CallSheet C " +
                    " INNER JOIN dbo.LeadRegister E ON C.LeadId=E.LeadId " +
                    " Left JOIN dbo.CallType CT ON C.CallTypeID=CT.CallTypeID " +
                    " Left JOIN dbo.StatusMaster S ON C.StatusID=S.StatusId " +
                    " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users U on C.ExecutiveId=U.UserId " +
                    " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OP on C.ProjectId=OP.CostCentreId " +
                    " Left JOIN dbo.NatureMaster N ON C.NatureID=N.NatureId Where C.EntryId=" + argRegId + "";
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

        public static DataTable GetGridCallSheet(int argEntryId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "SELECT C.LeadId,C.EntryId,Convert(Varchar(10),C.TrnDate,103) Date,OP.CostCentreName,U.UserName ExecutiveName," +
                              " E.LeadName,E.Mobile,C.CallFF Call, CT.Description CallType,S.Description Status," +
                              " N.Description Nature,Convert(Varchar(10),C.NextCallDate,103) NextCallDate,C.Remarks FROM dbo.CallSheet C " +
                              " INNER JOIN dbo.LeadRegister E ON C.LeadId=E.LeadId " +
                              " Left JOIN dbo.CallType CT ON C.CallTypeID=CT.CallTypeID " +
                              " Left JOIN dbo.StatusMaster S ON C.StatusID=S.StatusId " +
                              " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users U on C.ExecutiveId=U.UserId " +
                              " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre OP on C.ProjectId=OP.CostCentreId " +
                              " Left JOIN dbo.NatureMaster N ON C.NatureID=N.NatureId " +
                              " Where C.EntryId=" + argEntryId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
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

        public static DataTable FillLead(string m_sType, bool m_bOtherExec)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "";
                if (BsfGlobal.FindPermission("Pre-Followup-Allow-Other-Users-Lead") == false)
                {
                    if (m_sType == "CallReg")
                    {
                        sSql = "SELECT R.LeadId,R.LeadName,R.NextCallDate,R.Mobile,R.Email,P.CampaignId,E.ExecutiveId FROM LeadRegister R " +
                                " LEFT Join LeadExecutiveInfo E On E.LeadId=R.LeadId " +
                                " Inner Join dbo.LeadProjectInfo P On P.LeadId=E.LeadId " +
                                " ORDER BY LeadName";
                    }
                    else
                    {
                        sSql = "SELECT R.LeadId,R.LeadName,R.NextCallDate,R.Mobile,R.Email,P.CampaignId,E.ExecutiveId FROM LeadRegister R " +
                                " LEFT Join LeadExecutiveInfo E On E.LeadId=R.LeadId ";
                        
                        if (m_bOtherExec == false)
                        {
                            sSql = sSql + " and E.ExecutiveId = " + BsfGlobal.g_lUserId + " Inner Join dbo.LeadProjectInfo P On P.LeadId=E.LeadId " +
                                          " Where R.CallTypeId!=3 Order By LeadName";
                        }
                        else
                        {
                            sSql = sSql + " AND E.ExecutiveId In " +
                                         " (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users Where UserId=" + BsfGlobal.g_lUserId + " or " +
                                         " UserId in (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans Where sUserId=" + BsfGlobal.g_lUserId + ")) " +
                                         " Inner Join dbo.LeadProjectInfo P On P.LeadId=E.LeadId " +
                                         " Where R.CallTypeId!=3 Order By R.LeadName";
                        }
                    }
                }
                else
                {
                    if (m_sType == "CallReg")
                    {
                        sSql = "SELECT R.LeadId,R.LeadName,R.NextCallDate,R.Mobile,R.Email,P.CampaignId,E.ExecutiveId FROM LeadRegister R  " +
                                " LEFT Join LeadExecutiveInfo E On E.LeadId=R.LeadId " +
                                " Inner Join dbo.LeadProjectInfo P On P.LeadId=E.LeadId " +//And P.CostCentreId=E.CostCentreId " +
                                " ORDER BY LeadName";
                    }
                    else
                    {
                        sSql = "SELECT R.LeadId,R.LeadName,R.NextCallDate,R.Mobile,R.Email,P.CampaignId,E.ExecutiveId FROM LeadRegister R  " +
                                " LEFT Join LeadExecutiveInfo E On E.LeadId=R.LeadId ";

                        if (m_bOtherExec == false)
                        {
                            sSql = sSql + " Inner Join dbo.LeadProjectInfo P On P.LeadId=E.LeadId And P.CostCentreId=E.CostCentreId " +
                                        " Where R.CallTypeId!=3 AND E.ExecutiveId<>0 Order By LeadName";
                        }
                        else
                        {
                            sSql = sSql + " AND E.ExecutiveId In " +
                                            " (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users Where UserId=" + BsfGlobal.g_lUserId + " or " +
                                            " UserId in (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans Where sUserId=" + BsfGlobal.g_lUserId + ")) " +
                                            " Inner Join dbo.LeadProjectInfo P On P.LeadId=E.LeadId " +
                                            " Where R.CallTypeId!=3 Order By R.LeadName";
                        }
                    }
                }
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
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

        public static DataTable GetOtherReport(string argFromDate, string argToDate,bool argSel)
        {
            DataTable dt=null;
            SqlDataAdapter sda;
            String sSql = "";
            //sSql = " Select EmployeeName ExecName,A.NextCallDate,A.TrnDate PrevCallDate,E.CostCentreName,B.LeadName,C.Description Status,D.Description Nature,B.Mobile ContactNo from CallSheet A" +
            //        " Left Join LeadRegister B on A.LeadId=B.LeadId" +
            //        " Left Join StatusMaster C on A.StatusID=C.StatusId" +
            //        " Left Join NatureMaster D on A.NatureID=D.NatureId" +
            //        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre E On E.CostCentreId=A.ProjectId" +
            //        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users U On U.UserId=A.ExecutiveId " +
            //        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position P On U.PositionId=P.PositionId" +
            //        " Where A.NextCallDate in (Select MAX(NextCallDate) from CallSheet Group by LeadId)" +
            //        " and A.CallFF In('Fresh','FollowUp') And P.PositionType='M' And A.NextCallDate Between '" + argFromDate + "' And '" + argToDate + "' ";
            //if (argSel == false) { sSql = sSql + " and A.ExecutiveId = " + BsfGlobal.g_lUserId + " Order By A.NextCallDate"; }
            //else
            //{
            //    sSql = sSql + " AND A.ExecutiveId In " +
            //     " (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users Where UserId=" + BsfGlobal.g_lUserId + " or " +
            //     " UserId in (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans Where sUserId=" + BsfGlobal.g_lUserId + "))";
            //}
            if (BsfGlobal.FindPermission("Pre-Followup-Allow-Other-Users-Followup") == false)
            {
                sSql = " Select Case When U.EmployeeName='' Then U.UserName Else U.EmployeeName End As ExecName,A.NextCallDate,A.TrnDate PrevCallDate,E.CostCentreName,B.LeadName,B.Mobile ContactNo from CallSheet A" +
                        " Left Join LeadRegister B on A.LeadId=B.LeadId" +
                        " Left Join StatusMaster C on A.StatusID=C.StatusId" +
                        " Left Join NatureMaster D on A.NatureID=D.NatureId" +
                        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre E On E.CostCentreId=A.ProjectId" +
                        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users U On U.UserId=A.ExecutiveId " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position P On U.PositionId=P.PositionId" +
                        " Where A.NextCallDate in (Select MAX(NextCallDate) from CallSheet Group by LeadId)" +
                        " and A.CallFF In('Fresh','FollowUp') And P.PositionType='M' And A.NextCallDate Between '" + argFromDate + "' And '" + argToDate + "' ";
                if (argSel == false)
                {
                    sSql = sSql + " and A.ExecutiveId = " + BsfGlobal.g_lUserId + "" +
                        " Group By A.LeadId,U.EmployeeName,U.UserName,A.NextCallDate,A.TrnDate,E.CostCentreName,B.LeadName,B.Mobile Order By A.NextCallDate";
                }
                else
                {
                    sSql = sSql + " AND A.ExecutiveId In " +
                     " (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users Where UserId=" + BsfGlobal.g_lUserId + " or " +
                     " UserId in (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans Where sUserId=" + BsfGlobal.g_lUserId + "))" +
                     " Group By A.LeadId,U.EmployeeName,U.UserName,A.NextCallDate,A.TrnDate,E.CostCentreName,B.LeadName,B.Mobile Order By A.NextCallDate";
                }
            }
            else
            {
                sSql = " Select Case When U.EmployeeName='' Then U.UserName Else U.EmployeeName End As ExecName,A.NextCallDate,A.TrnDate PrevCallDate,E.CostCentreName,B.LeadName,B.Mobile ContactNo from CallSheet A " +
                    " Left Join LeadRegister B on A.LeadId=B.LeadId " +
                    " Left Join StatusMaster C on A.StatusID=C.StatusId " +
                    " Left Join NatureMaster D on A.NatureID=D.NatureId " +
                    " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre E On E.CostCentreId=A.ProjectId " +
                    " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users U On U.UserId=A.ExecutiveId " +
                    " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position P On U.PositionId=P.PositionId " +
                    " Where A.NextCallDate in (Select MAX(NextCallDate) from CallSheet Group by LeadId) " +
                    " and A.CallFF In('Fresh','FollowUp') And P.PositionType='M' And A.NextCallDate Between '" + argFromDate + "' And '" + argToDate + "' ";
                    //" Group By A.LeadId,EmployeeName,A.NextCallDate,A.TrnDate,E.CostCentreName,B.LeadName,B.Mobile Order By A.NextCallDate";
                if (argSel == false)
                {
                    sSql = sSql + " Group By A.LeadId,U.EmployeeName,U.UserName,A.NextCallDate,A.TrnDate,E.CostCentreName,B.LeadName,B.Mobile Order By A.NextCallDate";
                }
                else
                {
                    sSql = sSql + " AND A.ExecutiveId In " +
                     " (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users Where UserId=" + BsfGlobal.g_lUserId + " or " +
                     " UserId in (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans Where sUserId=" + BsfGlobal.g_lUserId + "))" +
                     " Group By A.LeadId,U.EmployeeName,U.UserName,A.NextCallDate,A.TrnDate,E.CostCentreName,B.LeadName,B.Mobile Order By A.NextCallDate";
                }
            }
            try
            {
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;

        }

        public static DataTable GetOtherReportPost(string argFromDate, string argToDate, bool argSel)
        {
            DataTable dt=null;
            SqlDataAdapter sda;
            String sSql = "";

            if (BsfGlobal.FindPermission("Post-Followup-Allow-Other-Users-Followup") == false)
            {
                sSql = "Select Case When U.EmployeeName='' Then U.UserName Else U.EmployeeName End As ExecName,A.NextCallDate,A.BuyerDate PrevCallDate,E.CostCentreName,B.LeadName,B.Mobile ContactNo from FollowUp A" +
                       " Left Join LeadRegister B on A.LeadId=B.LeadId" +
                       " Left Join NatureMaster D on A.NatureID=D.NatureId" +
                       " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre E On E.CostCentreId=A.CostCentreId" +
                       " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users U On U.UserId=A.ExecutiveId " +
                       " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position P On U.PositionId=P.PositionId" +
                       " Where P.PositionType='M' " +
                       " And A.NextCallDate Between '" + argFromDate + "' And '" + argToDate + "' ";

                if (argSel == false)
                {
                    sSql = sSql + " and A.ExecutiveId = " + BsfGlobal.g_lUserId + "" +
                        " Group By U.EmployeeName,U.UserName,A.NextCallDate,A.BuyerDate,E.CostCentreName,B.LeadName,B.Mobile Order By A.NextCallDate";
                }
                else
                {
                    sSql = sSql + " AND A.ExecutiveId In " +
                                    " (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users Where UserId=" + BsfGlobal.g_lUserId + " or " +
                                    " UserId in (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans Where sUserId=" + BsfGlobal.g_lUserId + "))" +
                                    " Group By U.EmployeeName,U.UserName,A.NextCallDate,A.BuyerDate,E.CostCentreName,B.LeadName,B.Mobile Order By A.NextCallDate";
                }
            }
            else
            {
                sSql = "Select Case When U.EmployeeName='' Then U.UserName Else U.EmployeeName End As ExecName,A.NextCallDate,A.BuyerDate PrevCallDate,E.CostCentreName,B.LeadName,B.Mobile ContactNo from FollowUp A " +
                        " Left Join LeadRegister B on A.LeadId=B.LeadId " +
                        " Left Join NatureMaster D on A.NatureID=D.NatureId " +
                        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre E On E.CostCentreId=A.CostCentreId " +
                        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users U On U.UserId=A.ExecutiveId " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position P On U.PositionId=P.PositionId " +
                        " Where P.PositionType='M' " +
                        " And A.NextCallDate Between '" + argFromDate + "' And '" + argToDate + "' ";

                if (argSel == false)
                {
                    sSql = sSql + " Group By U.EmployeeName,U.UserName,A.NextCallDate,A.BuyerDate,E.CostCentreName,B.LeadName,B.Mobile Order By A.NextCallDate";
                }
                else
                {
                    sSql = sSql + " AND A.ExecutiveId In " +
                     " (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users Where UserId=" + BsfGlobal.g_lUserId + " or " +
                     " UserId in (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans Where sUserId=" + BsfGlobal.g_lUserId + "))" +
                     " Group By U.EmployeeName,U.UserName,A.NextCallDate,A.BuyerDate,E.CostCentreName,B.LeadName,B.Mobile Order By A.NextCallDate";
                }
            }
            try
            {
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;

        }

        public static bool GetOtherExecCall()
        {
            BsfGlobal.OpenCRMDB();
            bool bAns = false;
            try
            {
                String sSql = "Select COUNT(*) From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans Where sUserId=" + BsfGlobal.g_lUserId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                int iCount = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                cmd.Dispose();
                if (iCount > 0) { bAns = true; }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return bAns;
        }

        public static bool GetExecCall(int argLeadId)
        {
            BsfGlobal.OpenCRMDB();
            bool bAns = false;
            try
            {
                String sSql = "Select COUNT(ExecutiveId) From dbo.LeadExecutiveInfo Where LeadId=" + argLeadId + " AND ExecutiveId<>0";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.OpenCRMDB());
                int iCount = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                cmd.Dispose();

                if (iCount > 0) { bAns = true; }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return bAns;
        }

        public static int InsertBuyerDetail(DataTable dtEnqTrans)
        {
            if (dtEnqTrans.Rows.Count == 0) { return 0; }

            int iEntryId = 0;
            int iBuyerId = 0;
            SqlConnection conn;
            conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            string sSql = "";
           
            string validUpto = "";
            try
            {
                sSql = "";
                if (dtEnqTrans.Rows[0]["CallType"].ToString() == "Finalization" || dtEnqTrans.Rows[0]["CallType"].ToString() == "Finalisation" || dtEnqTrans.Rows[0]["CallType"].ToString() == "Block")
                {
                    if (dtEnqTrans.Rows[0]["ValidUpto"].ToString() != "")
                    {
                        validUpto = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(dtEnqTrans.Rows[0]["ValidUpto"].ToString()));
                    }
                    if (dtEnqTrans.Rows.Count > 0)
                    {
                        sSql = "INSERT INTO dbo.BuyerDetail(CostCentreId,ExecutiveId,LeadId,FlatId,Status,CustomerType," +
                            " PaymentOption,BankId,LoanPer,BrokerId,BrokerComm,BrokerAmount,ValidUpto) Values" +
                            " (" + dtEnqTrans.Rows[0]["CostCentreId"] + "," + dtEnqTrans.Rows[0]["ExecutiveId"] + "," +
                            " " + dtEnqTrans.Rows[0]["LeadId"] + "," + dtEnqTrans.Rows[0]["FlatId"] + ",'" + dtEnqTrans.Rows[0]["Status"] + "'," +
                            " '" + dtEnqTrans.Rows[0]["CustomerType"] + "','" + dtEnqTrans.Rows[0]["PaymentOption"] + "'," +
                            " " + dtEnqTrans.Rows[0]["BankId"] + "," + dtEnqTrans.Rows[0]["LoanPer"] + "," +
                            " " + dtEnqTrans.Rows[0]["BrokerId"] + "," + dtEnqTrans.Rows[0]["ComPer"] + "," + dtEnqTrans.Rows[0]["ComAmount"] + "," +
                            " '" + validUpto + "') SELECT SCOPE_IDENTITY();";
                        cmd = new SqlCommand(sSql, conn, tran);
                        iBuyerId = int.Parse(cmd.ExecuteScalar().ToString());
                        cmd.Dispose();
                    }

                    sSql = "Update dbo.FlatDetails Set Status='S',BuyerId=" + iBuyerId + " WHERE FlatId=" + dtEnqTrans.Rows[0]["FlatId"] + 
                            " AND CostCentreId=" + dtEnqTrans.Rows[0]["CostCentreId"] + "";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sSql = "INSERT INTO dbo.LeadFlatInfo (LeadId,Buyer,FlatId,BuyerId,CostCentreId)VALUES(" + dtEnqTrans.Rows[0]["LeadId"] + 
                            ",1," + dtEnqTrans.Rows[0]["FlatId"] + "," + iBuyerId + "," + dtEnqTrans.Rows[0]["CostCentreId"] + ")";
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
                conn.Dispose();
                conn.Close();
            }
            return iEntryId;
        }

        public static bool CallSheetFound(int argLeadId, int argEntryId)
        {
            BsfGlobal.OpenCRMDB();
            bool bans = false;
            try
            {
                DataTable dt = null;
                string sSql = "Select COUNT(*) From dbo.CallSheet Where TrnDate=" +
                                " (Select MAX(TrnDate) NextCallDate From dbo.CallSheet Where LeadId=" + argLeadId + " Group By LeadId)" +
                                " And EntryId=" + argEntryId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                int iCount = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                cmd.Dispose();

                if (iCount > 0) { bans = true; }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }

            return bans;
        }

        public static int GetCompanyId(int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            int iCompanyId = 0;
            try
            {
                string sSql = "Select A.CompanyId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CostCentre A " +
                              "Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B on A.CostCentreId=B.FACostCentreId" +
                              " Where B.CostCentreId=" + argCCId;
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();

                if (dt.Rows.Count > 0)
                    iCompanyId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["CompanyId"], CommFun.datatypes.vartypenumeric));
                else
                    iCompanyId = 0;
                dt.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return iCompanyId;
        }

        #endregion

        #region PostSale CallSheet

        public static DataTable GetCallMaster()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "Select CallTypeId,Description From PostCallType Order By Description";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
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

        public static void InsertCallTypeMaster(string argDesc)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sSql = "INSERT INTO PostCallType (Description) VALUES('" + argDesc + "')";
                SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                tran.Commit();
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

        public static void UpdateCallTypeMaster(string argDesc, int argTypeId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sSql = "UPDATE PostCalltype SET Description='" + argDesc + "'  WHERE CallTypeId=" + argTypeId + "";
                SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                tran.Commit();
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

        public static bool CallTypeMasterFound(int argTypeId)
        {
            BsfGlobal.OpenCRMDB();
            bool bans = false;
            try
            {
                string sSql = "Select COUNT(*) From FollowUp Where CallTypeId=" + argTypeId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                int iCount = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                cmd.Dispose();

                if (iCount > 0) { bans = true; }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return bans;
        }

        public static bool CallTypeSysFound(int argTypeId)
        {
            BsfGlobal.OpenCRMDB();
            bool bans = false;
            try
            {
                string sSql = "Select COUNT(*) From PostCallType Where CallTypeId=" + argTypeId + " AND SysDefault=1";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                int iCount = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                cmd.Dispose();

                if (iCount > 0) { bans = true; }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return bans;
        }

        public static void DeleteCallTypeMaster(int argTypeId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sSql = "Delete From PostCallType Where CallTypeId=" + argTypeId + "";
                SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                tran.Commit();
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

        public static DataTable PostGetCall()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "";
                if (BsfGlobal.FindPermission("Post-Followup-Allow-Other-Users-Lead") == false)
                {
                    sSql = "Select B.LeadId,B.LeadName,C.CostCentreName,A.CostCentreId,A.NatureId,A.CallTypeId, A.FlatId,A.PlotDetailsId," +
                            " A.FollowUpReq,A.BuyerDate,A.NextCallDate,A.Remarks,B.Mobile,B.Email From dbo.FollowUp A  " +
                            " Inner Join dbo.LeadRegister B On A.LeadId=B.LeadId " +
                            " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On A.CostCentreId=C.CostCentreId " +
                            " Where EntryId IN(Select MAX(EntryId)Id From FollowUp Group By LeadId,CostCentreId,FlatId)  " +
                            " And A.ExecutiveId=" + BsfGlobal.g_lUserId + " And A.FollowUpReq=1 " +
                            " And A.NextCallDate<='" + Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy") + "'" +
                            " AND A.ExecutiveId IN(Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users " +
                            " Where PositionId IN(Select PositionId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position Where PositionType='C') " +
                            " AND A.ExecutiveId=" + BsfGlobal.g_lUserId + ") " +
                            " Order By LeadName";
                }
                else
                {
                    sSql = "Select B.LeadId,B.LeadName,C.CostCentreName,A.CostCentreId,A.NatureId,A.CallTypeId, A.FlatId,A.PlotDetailsId," +
                            " A.FollowUpReq,A.BuyerDate,A.NextCallDate,A.Remarks,B.Mobile,B.Email From dbo.FollowUp A  " +
                            " Inner Join dbo.LeadRegister B On A.LeadId=B.LeadId " +
                            " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On A.CostCentreId=C.CostCentreId " +
                            " Where EntryId IN(Select MAX(EntryId)Id From FollowUp Group By LeadId,CostCentreId,FlatId) " +
                            " And A.FollowUpReq=1 And A.NextCallDate<='" + Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy") + "' " +
                            " AND A.ExecutiveId IN(Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users " +
                            " Where PositionId IN(Select PositionId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position Where PositionType='C')) " +
                            " Order By LeadName";
                }

                if (sSql != "")
                {
                    SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    dt = new DataTable();
                    SqlDataReader dreader = cmd.ExecuteReader();
                    dt.Load(dreader);
                    dreader.Close();
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
            return dt;

        }

        public static DataTable PostGetCallEdit(string argFromDate, string argToDate, bool argSel)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "";
                if (BsfGlobal.FindPermission("Post-Followup-Allow-Other-Users-Followup") == false)
                {
                    sSql = "Select A.EntryId,B.CostCentreName,C.LeadName," +
                            " Case When D.FlatNo Is Null Then L.PlotNo Else D.FlatNo End FlatNo," +
                            " Case When D.FlatNo Is Null Then 'Plot' Else 'Land' End Type," +
                            " E.Description Nature,F.Description CallType,A.NextCallDate,A.Remarks From FollowUp A " +
                            " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B On A.CostCentreId=B.CostCentreId" +
                            " Inner Join LeadRegister C On C.LeadId=A.LeadId " +
                            " Left Join FlatDetails D On D.FlatId=A.FlatId" +
                            " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails L on L.PlotDetailsId=A.PlotDetailsId" +
                            " Inner Join NatureMaster E On E.NatureId=A.NatureId" +
                            " Inner Join PostCallType F On F.CallTypeId=A.CallTypeId" +
                            " Where A.BuyerDate Between '" + argFromDate + "' And '" + argToDate + "'";
                    //" Order by A.BuyerDate";
                    if (argSel == false) { sSql = sSql + " and A.ExecutiveId = " + BsfGlobal.g_lUserId + " Order By A.BuyerDate"; }
                    else
                    {
                        sSql = sSql + " AND A.ExecutiveId In " +
                         " (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users Where UserId=" + BsfGlobal.g_lUserId + " or " +
                         " UserId in (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans Where sUserId=" + BsfGlobal.g_lUserId + ")) Order By A.BuyerDate";
                    }
                }
                else
                {
                    sSql = "Select A.EntryId,B.CostCentreName,C.LeadName," +
                            " Case When D.FlatNo Is Null Then L.PlotNo Else D.FlatNo End FlatNo," +
                            " Case When D.FlatNo Is Null Then 'Plot' Else 'Land' End Type," +
                            " E.Description Nature,F.Description CallType,A.NextCallDate,A.Remarks From FollowUp A " +
                            " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B On A.CostCentreId=B.CostCentreId" +
                            " Inner Join LeadRegister C On C.LeadId=A.LeadId " +
                            " Left Join FlatDetails D On D.FlatId=A.FlatId" +
                            " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails L on L.PlotDetailsId=A.PlotDetailsId" +
                            " Inner Join NatureMaster E On E.NatureId=A.NatureId" +
                            " Inner Join PostCallType F On F.CallTypeId=A.CallTypeId" +
                            " Where A.BuyerDate Between '" + argFromDate + "' And '" + argToDate + "' ";
                    if (argSel == false) { sSql = sSql + " Order By A.BuyerDate"; }
                    else
                    {
                        sSql = sSql + " AND A.ExecutiveId In " +
                         " (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users Where UserId=" + BsfGlobal.g_lUserId + " or " +
                         " UserId in (Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans Where sUserId=" + BsfGlobal.g_lUserId + ")) Order By A.BuyerDate";
                    }
                }

                if (sSql != "")
                {
                    SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    dt = new DataTable();
                    SqlDataReader dreader = cmd.ExecuteReader();
                    dt.Load(dreader);
                    dreader.Close();
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
            return dt;
        }

        public static DataTable GetBuyer(int argCCId, string argsType)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "";
                if (BsfGlobal.FindPermission("Post-Followup-Allow-Other-Users-Lead") == false)
                {
                    if (argsType == "B")
                    {
                        sSql = "SELECT Distinct R.LeadId,R.LeadName FROM dbo.LeadRegister R " +
                                " Inner Join dbo.BuyerDetail C On C.LeadId=R.LeadId " +
                                " Inner Join dbo.FlatDetails F On F.LeadId=C.LeadId And F.FlatId=C.FlatId " +
                                " Inner Join dbo.LeadProjectInfo L On L.LeadId=R.LeadId " +
                                " WHERE L.ExecutiveId=" + BsfGlobal.g_lUserId + " And C.CostCentreId=" + argCCId + " " +
                                " And C.LeadId<>0 ORDER BY LeadName";
                    }
                    else
                    {
                        sSql = "SELECT Distinct R.LeadId,R.LeadName FROM dbo.LeadRegister R " +
                                " Inner Join dbo.BuyerDetail C On C.LeadId=R.LeadId" +
                                " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails D On D.BuyerId=C.LeadId And D.PlotDetailsId=C.PlotId " +
                                " WHERE C.ExecutiveId=" + BsfGlobal.g_lUserId + " And C.CostCentreId=" + argCCId + " " +
                                " And C.LeadId<>0 ORDER BY LeadName";
                    }
                }
                else
                {
                    if (argsType == "B")
                    {
                        sSql = "SELECT Distinct R.LeadId,R.LeadName FROM dbo.LeadRegister R " +
                                " Inner Join dbo.BuyerDetail C On C.LeadId=R.LeadId " +
                                " Inner Join dbo.FlatDetails F On F.LeadId=C.LeadId And F.FlatId=C.FlatId " +
                                " WHERE C.CostCentreId=" + argCCId + " " +
                                " And C.LeadId<>0 ORDER BY LeadName";
                    }
                    else
                    {
                        sSql = "SELECT Distinct R.LeadId,R.LeadName FROM dbo.LeadRegister R " +
                               " Inner Join dbo.BuyerDetail C On C.LeadId=R.LeadId" +
                               " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails D On D.BuyerId=C.LeadId And D.PlotDetailsId=C.PlotId " +
                               " WHERE C.CostCentreId=" + argCCId + " " +
                               " And C.LeadId<>0 ORDER BY LeadName";
                    }
                }

                if (sSql != "")
                {
                    SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    dt = new DataTable();
                    SqlDataReader dreader = cmd.ExecuteReader();
                    dt.Load(dreader);
                    dreader.Close();
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
            return dt;
        }

        public static DataTable GetFlat(int argCCId, string argsType)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "";
                if (argsType == "B")
                {
                    sSql = "SELECT F.FlatId,F.FlatNo FROM dbo.LeadRegister R" +
                          " Inner Join dbo.BuyerDetail C On C.LeadId=R.LeadId " +
                          " Inner Join dbo.FlatDetails F On F.LeadId=C.LeadId And F.FlatId=C.FlatId" +
                          " WHERE C.CostCentreId=" + argCCId + "" +
                          " And C.LeadId<>0 ORDER BY LeadName";
                }
                else
                {
                    sSql = "SELECT D.PlotDetailsId FlatId,D.PlotNo FlatNo FROM dbo.LeadRegister R " +
                            " Inner Join dbo.BuyerDetail C On C.LeadId=R.LeadId" +
                            " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails D On D.BuyerId=C.LeadId  And D.PlotDetailsId=C.PlotId" +
                            " WHERE C.CostCentreId=" + argCCId + "" +
                            " And C.LeadId<>0 ORDER BY LeadName";
                }
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
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

        public static int GetBuyerId(int argCCId, string argsType, int argFlatId)
        {
            BsfGlobal.OpenCRMDB();
            int iBuyerId = 0;
            try
            {
                String sSql = "";
                if (argsType == "B")
                {
                    sSql = "SELECT R.LeadId FROM dbo.LeadRegister R" +
                            " Inner Join dbo.BuyerDetail C On C.LeadId=R.LeadId" +
                            " Inner Join dbo.FlatDetails F On F.LeadId=C.LeadId And F.FlatId=C.FlatId" +
                            " Inner Join dbo.LeadProjectInfo L On L.LeadId=R.LeadId " +
                            " WHERE C.CostCentreId=" + argCCId + " And C.LeadId<>0 And C.FlatId=" + argFlatId;

                    if (BsfGlobal.FindPermission("Post-Followup-Allow-Other-Users-Lead") == false)
                    {
                        sSql = sSql + " AND L.ExecutiveId=" + BsfGlobal.g_lUserId;
                    }
                }
                else
                {
                    sSql = "SELECT R.LeadId FROM dbo.LeadRegister R " +
                            " Inner Join dbo.BuyerDetail C On C.LeadId=R.LeadId" +
                            " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails D On D.BuyerId=C.LeadId And D.PlotDetailsId=C.PlotId " +
                            " WHERE C.CostCentreId=" + argCCId + "  And C.LeadId<>0 And C.PlotId=" + argFlatId;

                    if (BsfGlobal.FindPermission("Post-Followup-Allow-Other-Users-Lead") == false)
                    {
                        sSql = sSql + " AND C.ExecutiveId=" + BsfGlobal.g_lUserId;
                    }
                }

                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                DataTable dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0) { iBuyerId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["LeadId"])); }
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
            return iBuyerId;
        }

        public static int GetFlatId(int argCCId, string argsType, int argLeadId)
        {
            BsfGlobal.OpenCRMDB();
            int iFlatId = 0;
            try
            {
                String sSql = "";
                if (argsType == "B")
                {
                    sSql = "SELECT F.FlatId FROM dbo.LeadRegister R" +
                            " Inner Join dbo.BuyerDetail C On C.LeadId=R.LeadId" +
                            " Inner Join dbo.FlatDetails F On F.LeadId=C.LeadId And F.FlatId=C.FlatId" +
                            " Inner Join dbo.LeadProjectInfo L On L.LeadId=R.LeadId " +
                            " WHERE  C.CostCentreId=" + argCCId + " And C.LeadId=" + argLeadId;

                    if (BsfGlobal.FindPermission("Post-Followup-Allow-Other-Users-Lead") == false)
                    {
                        sSql = sSql + " AND L.ExecutiveId=" + BsfGlobal.g_lUserId;
                    }
                }
                else
                {
                    sSql = "SELECT D.PlotDetailsId FlatId FROM dbo.LeadRegister R" +
                            " Inner Join dbo.BuyerDetail C On C.LeadId=R.LeadId" +
                            " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails D On D.BuyerId=C.LeadId And D.PlotDetailsId=C.PlotId" +
                            " WHERE C.CostCentreId=" + argCCId + "  And C.LeadId=" + argLeadId;

                    if (BsfGlobal.FindPermission("Post-Followup-Allow-Other-Users-Lead") == false)
                    {
                        sSql = sSql + " AND C.ExecutiveId=" + BsfGlobal.g_lUserId;
                    }
                }

                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                DataTable dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
                dt.Load(dreader);
                dreader.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0) { iFlatId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["FlatId"])); }
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
            return iFlatId;
        }

        public static DataTable GetNature()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "SELECT NatureId,Description From NatureMaster Order By SortId";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
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

        public static void InsertFollowUp(string argsType)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            int iEntryId = 0;
            try
            {
                string sSql = "";
                if (argsType == "B")
                {
                    if (FollowUpBO.NextCallDate == "null")
                        sSql = "INSERT INTO FollowUp (ExecutiveId,LeadId,CostCentreId,BuyerDate,FlatId,NatureId,CallTypeId,FollowUpReq,NextCallDate,Remarks) VALUES" +
                            " (" + FollowUpBO.ExecutiveId + "," + FollowUpBO.LeadId + "," + FollowUpBO.CostCentreId + ",'" + FollowUpBO.BuyerDate + "'," +
                            " " + FollowUpBO.FlatId + "," + FollowUpBO.NatureId + "," + FollowUpBO.CallTypeId + "," +
                            " '" + FollowUpBO.FollowUpReq + "',Null,'" + FollowUpBO.Remarks + "')SELECT SCOPE_IDENTITY();";
                    else
                        sSql = "INSERT INTO FollowUp (ExecutiveId,LeadId,CostCentreId,BuyerDate,FlatId,NatureId,CallTypeId,FollowUpReq,NextCallDate,Remarks) VALUES" +
                        " (" + FollowUpBO.ExecutiveId + "," + FollowUpBO.LeadId + "," + FollowUpBO.CostCentreId + ",'" + FollowUpBO.BuyerDate + "'," +
                        " " + FollowUpBO.FlatId + "," + FollowUpBO.NatureId + "," + FollowUpBO.CallTypeId + "," +
                        " '" + FollowUpBO.FollowUpReq + "','" + FollowUpBO.NextCallDate + "','" + FollowUpBO.Remarks + "')SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    if (FollowUpBO.NextCallDate == "null" )
                        sSql = "INSERT INTO FollowUp (ExecutiveId,LeadId,CostCentreId,BuyerDate,PlotDetailsId,NatureId,CallTypeId,FollowUpReq,NextCallDate,Remarks) VALUES" +
                            " (" + FollowUpBO.ExecutiveId + "," + FollowUpBO.LeadId + "," + FollowUpBO.CostCentreId + ",'" + FollowUpBO.BuyerDate + "'," +
                            " " + FollowUpBO.FlatId + "," + FollowUpBO.NatureId + "," + FollowUpBO.CallTypeId + "," +
                            " '" + FollowUpBO.FollowUpReq + "',Null,'" + FollowUpBO.Remarks + "')SELECT SCOPE_IDENTITY();";
                    else
                        sSql = "INSERT INTO FollowUp (ExecutiveId,LeadId,CostCentreId,BuyerDate,PlotDetailsId,NatureId,CallTypeId,FollowUpReq,NextCallDate,Remarks) VALUES" +
                        " (" + FollowUpBO.ExecutiveId + "," + FollowUpBO.LeadId + "," + FollowUpBO.CostCentreId + ",'" + FollowUpBO.BuyerDate + "'," +
                        " " + FollowUpBO.FlatId + "," + FollowUpBO.NatureId + "," + FollowUpBO.CallTypeId + "," +
                        " '" + FollowUpBO.FollowUpReq + "','" + FollowUpBO.NextCallDate + "','" + FollowUpBO.Remarks + "')SELECT SCOPE_IDENTITY();";
                }
                SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                iEntryId = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                cmd.Dispose();
                tran.Commit();

                BsfGlobal.InsertLog(DateTime.Now, "Post-Followup-Add", "N", "Post-Followup", iEntryId, FollowUpBO.CostCentreId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
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

        public static void UpdateFollowUp(int argEntryId,string argsType)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sSql = "";
                if (argsType == "B")
                {
                    if (FollowUpBO.NextCallDate == "null")
                    {
                        sSql = "Update FollowUp Set ExecutiveId=" + FollowUpBO.ExecutiveId + ",LeadId=" + FollowUpBO.LeadId + "," +
                                " CostCentreId=" + FollowUpBO.CostCentreId + ",BuyerDate='" + FollowUpBO.BuyerDate + "'," +
                                " FlatId=" + FollowUpBO.FlatId + ",NatureId=" + FollowUpBO.NatureId + ",CallTypeId=" + FollowUpBO.CallTypeId + "," +
                                " FollowUpReq='" + FollowUpBO.FollowUpReq + "',NextCallDate=" + FollowUpBO.NextCallDate + "," +
                                " Remarks='" + FollowUpBO.Remarks + "' Where EntryId=" + argEntryId + "";
                    }
                    else
                    {
                        sSql = "Update FollowUp Set ExecutiveId=" + FollowUpBO.ExecutiveId + ",LeadId=" + FollowUpBO.LeadId + "," +
                               " CostCentreId=" + FollowUpBO.CostCentreId + ",BuyerDate='" + FollowUpBO.BuyerDate + "'," +
                               " FlatId=" + FollowUpBO.FlatId + ",NatureId=" + FollowUpBO.NatureId + ",CallTypeId=" + FollowUpBO.CallTypeId + "," +
                               " FollowUpReq='" + FollowUpBO.FollowUpReq + "',NextCallDate='" + FollowUpBO.NextCallDate + "'," +
                               " Remarks='" + FollowUpBO.Remarks + "' Where EntryId=" + argEntryId + "";
                    }
                }
                else
                {
                    if (FollowUpBO.NextCallDate == "null")
                    {
                        sSql = "Update FollowUp Set ExecutiveId=" + FollowUpBO.ExecutiveId + ",LeadId=" + FollowUpBO.LeadId + "," +
                                " CostCentreId=" + FollowUpBO.CostCentreId + ",BuyerDate='" + FollowUpBO.BuyerDate + "'," +
                                " PlotDetailsId=" + FollowUpBO.FlatId + ",NatureId=" + FollowUpBO.NatureId + ",CallTypeId=" + FollowUpBO.CallTypeId + "," +
                                " FollowUpReq='" + FollowUpBO.FollowUpReq + "',NextCallDate=" + FollowUpBO.NextCallDate + "," +
                                " Remarks='" + FollowUpBO.Remarks + "' Where EntryId=" + argEntryId + "";
                    }
                    else
                    {
                        sSql = "Update FollowUp Set ExecutiveId=" + FollowUpBO.ExecutiveId + ",LeadId=" + FollowUpBO.LeadId + "," +
                               " CostCentreId=" + FollowUpBO.CostCentreId + ",BuyerDate='" + FollowUpBO.BuyerDate + "'," +
                               " PlotDetailsId=" + FollowUpBO.FlatId + ",NatureId=" + FollowUpBO.NatureId + ",CallTypeId=" + FollowUpBO.CallTypeId + "," +
                               " FollowUpReq='" + FollowUpBO.FollowUpReq + "',NextCallDate='" + FollowUpBO.NextCallDate + "'," +
                               " Remarks='" + FollowUpBO.Remarks + "' Where EntryId=" + argEntryId + "";
                    }
                }
                SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                tran.Commit();

                BsfGlobal.InsertLog(DateTime.Now, "Post-Followup-Modify", "N", "Post-Followup", argEntryId, FollowUpBO.CostCentreId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
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

        public static DataTable PostRegEdit(int argEntryId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "Select * From dbo.FollowUp Where EntryId=" + argEntryId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
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

        public static DataTable PostRegChangeGrid(int argEntryId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "Select B.CostCentreName,C.FlatNo,D.LeadName,E.Description Nature,F.Description CallType,A.FollowUpReq,A.NextCallDate,A.Remarks From dbo.FollowUp A " +
                    " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B On A.CostCentreId=B.CostCentreId " +
                    " Inner Join dbo.FlatDetails C On C.FlatId=A.FlatId " +
                    " Inner Join dbo.LeadRegister D On D.LeadId=A.LeadId " +
                    " Inner Join dbo.NatureMaster E On E.NatureId=A.NatureId " +
                    " Inner Join dbo.CallType F On F.CallTypeId=A.CallTypeId Where A.EntryId=" + argEntryId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
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

        public static void DeleteFollowUp(int argEntryId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sSql = "Delete From FollowUp Where EntryId=" + argEntryId + "";
                SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                tran.Commit();
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

        #endregion

        #region Plot

        public static DataTable GetPlotType(int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "Select PlotTypeId,PlotTypeName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotType " +
                              " Where LandRegisterId=" + argCCId + " Order By PlotTypeName";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
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

        public static DataTable GetPlotDetails(int argCCId, string argMode, int argLeadId, string argType)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "";
                if (argType == "B")
                {
                    sSql = "SELECT A.PlotTypeId,A.PlotDetailsId,A.PlotNo FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A" +
                        " Where A.Status='B' And A.BuyerId=" + argLeadId + " And A.LandRegisterId=" + argCCId + " Order By A.PlotNo";
                }
                else
                {
                    if (argMode == "A")
                        sSql = "SELECT A.PlotTypeId,A.PlotDetailsId,A.PlotNo FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A" +
                            " where A.Status='U' AND A.LandRegisterId=" + argCCId + " Order By A.PlotNo";
                    else
                        sSql = "SELECT A.PlotTypeId,A.PlotDetailsId,A.PlotNo FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A" +
                            " where LandRegisterId=" + argCCId + " Order By A.PlotNo";
                }
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
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

        #region Reminder

        public static DataTable GetReminder()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Select ReminderId,ReminderName From dbo.Reminder ";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                SqlDataReader dreader = cmd.ExecuteReader();
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

        public static void UpdateReminderMaster(int argRemId, string argDesc)
        {
            BsfGlobal.OpenCRMDB();
            try
            {
                string sSql = "UPDATE dbo.Reminder SET ReminderName='" + argDesc + "' WHERE ReminderId=" + argRemId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
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
        }

        public static void InsertReminderMaster(string argDesc)
        {
            BsfGlobal.OpenCRMDB();
            try
            {
                string sSql = "INSERT INTO dbo.Reminder (ReminderName) VALUES ('" + argDesc + "')";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
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
        }

        public static void DeleteReminderMaster(int argRemId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    string sSql = "DELETE FROM dbo.Reminder WHERE ReminderId=" + argRemId + "";
                    SqlCommand cmd = new SqlCommand(sSql, conn, tran);
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

        public static bool FoundReminder(int argRemId)
        {
            BsfGlobal.OpenCRMDB();
            bool bAns = false;
            try
            {
                string sSql = " Select COUNT(*) From dbo.CallSheet Where ReminderId= " + argRemId + " ";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                int iCount = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                cmd.Dispose();

                if (iCount > 0) { bAns = true; }
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

        internal static bool GetFinalisedFlat(int argEntryId, int argLeadId)
        {
            BsfGlobal.OpenCRMDB();
            bool bFinalised = false;
            try
            {
                String sSql = "";
                if (argEntryId == 0)
                    sSql = "Select COUNT(*) from dbo.BuyerDetail Where LeadId=" + argLeadId + "";
                else
                    sSql = "Select COUNT(*) from dbo.BuyerDetail Where EntryId=" + argEntryId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                int iCount = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                cmd.Dispose();

                if (iCount > 0)
                    bFinalised = true;
                else
                    bFinalised = false;
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return bFinalised;
        }
    }
}
