using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessObjects;

namespace CRM.DataLayer
{
    class BankDL
    {
        #region Methods

        public static DataTable getBank()
        {
            SqlDataAdapter da;
            DataTable dt = null;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select BankId,BankName from dbo.BankMaster";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;

        }

        public static DataTable GetBankDetails()
        {
            SqlDataAdapter da;
            DataTable dt = null;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select * from dbo.BankMaster";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;

        }

        public static DataTable getEditBank(int argBankId)
        {
            SqlDataAdapter da;
            DataTable dt = null;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select * from dbo.BankMaster Where BankId=" + argBankId + "";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;

        }

        public static DataTable getBankname()
        {
            SqlDataAdapter da;
            DataTable dt = null;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select BankName,InterestRate,LoanAmount,ProcessingDays From dbo.BankMaster";
                //sSql = "Select BankName,InterestRate,LoanAmount,ProcessingDays,ProcessingFee,LegalFee, " +
                //        " Case When Insurance=1 Then 'Yes' Else 'No' End Insurance From dbo.BankMaster A Inner Join " +
                //        " dbo.BankDetails B On A.BankId=B.BankId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;

        }

        public static DataTable getLoanInfo()
        {
            SqlDataAdapter da;
            DataTable dt = null;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select BranchId,LoanPer,LoanAccNo,LoanAppDate from dbo.BuyerDetail A " +
                    " Where FlatId=" + BankInfoBO.FlatId;
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;

        }

        public static DataTable getBranchName()
        {
            SqlDataAdapter da;
            DataTable dt = null;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select BranchId,Branch BranchName from dbo.BankDetails ";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;

        }

        public static void UpdateBankInfo()
        {
            try
            {
                string sSql = "";
                sSql = "Update dbo.BuyerDetail Set BranchId=" + BankInfoBO.BranchId + ",LoanPer=" + BankInfoBO.LoanPer + ", " +
                              "LoanAppDate='" + BankInfoBO.LoanAppDate.ToString("dd-MMM-yyyy") + "',LoanAccNo=" + BankInfoBO.LoanAccNo + " " +
                              " Where FlatId=" + BankInfoBO.FlatId + "";
                BsfGlobal.OpenCRMDB();
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
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
        }

        public static int InsertBank()
        {
            int identity = 0;

            try
            {
                SqlCommand cmd;
                string sSql = "Insert into dbo.BankMaster (BankName,InterestRate,LoanAmount,ProcessingDays)" +
                              "Values('" + BankBO.BankName + "','" + BankBO.IntRate + "'," + BankBO.LoanAmt + "," +
                              " '" + BankBO.PDays + "') SELECT SCOPE_IDENTITY();";
                BsfGlobal.OpenCRMDB();
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                identity = int.Parse(cmd.ExecuteScalar().ToString());
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
            return identity;
        }

        public static void UpdateBank()
        {
            try
            {
                SqlCommand cmd;
                string sSql = "Update dbo.BankMaster Set BankName='" + BankBO.BankName + "',InterestRate=" + BankBO.IntRate + "," +
                              " LoanAmount=" + BankBO.LoanAmt + ",ProcessingDays='" + BankBO.PDays + "' " +
                              " Where BankId=" + BankBO.BankId + "";
                BsfGlobal.OpenCRMDB();
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
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
        }

        public static int CheckListSortIdFound(string argName)
        {
            int ians = 0;
            try
            {
                DataTable dt;
                string sSql = "Select Max(SortOrder) SortOrder from dbo.CheckListMaster Where TypeName='" + argName + "'";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    ians = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["SortOrder"], CommFun.datatypes.vartypenumeric));
                }
                da.Dispose();
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

            return ians;
        }

        public static bool CheckListFound(int argId, string argName,string argType)
        {
            bool bans = false;
            try
            {
                DataTable dt;
                string sSql = "Select CheckListId from dbo.CheckListMaster Where CheckListId <> " + argId + " " +
                              "and CheckListName = '" + argName + "' And TypeName='" + argType + "'";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                if (dt.Rows.Count > 0) { bans = true; }
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

            return bans;
        }

        public static bool TemplateFound(string argName,string argType)
        {
            bool bans = false;
            try
            {
                DataTable dt;
                string sSql = "Select TemplateId from dbo.Template Where " +
                              " TemplateName = '" + argName + "' And TemplateType='" + argType + "'";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                if (dt.Rows.Count > 0) { bans = true; }
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

            return bans;
        }

        public static int InsertCheckList(CheckListBO argObject)
        {
            int identity = 0;

            try
            {
                SqlCommand cmd;
                string sSql = "Insert into dbo.CheckListMaster (TypeName,CheckListName,SortOrder)" +                              
                              "Values('"+ argObject.TypeName +"','" + argObject.CheckListName + "',"+ argObject.SortOrder +") SELECT SCOPE_IDENTITY();";
                BsfGlobal.OpenCRMDB();
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                identity = int.Parse(cmd.ExecuteScalar().ToString());
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
            return identity;
        }

        public static void UpdateCheckList(CheckListBO argObject)
        {
            try
            {
                SqlCommand cmd;
                string sSql = "Update dbo.CheckListMaster set CheckListName='" + argObject.CheckListName + "',TypeName='"+ argObject.TypeName +"' Where CheckListId=" + argObject.CheckListId + "";
                BsfGlobal.OpenCRMDB();
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
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

        }

        public static bool DocuFound(int argChekId)
        {
            bool bans = false;
            try
            {
                DataTable dt;
                string sSql = "Select CheckListId from dbo.CCCheckListTrans Where CheckListId= " + argChekId + " " +
                              "Union All " +
                              "Select CheckListId from dbo.FlatTypeChecklist Where CheckListId=" + argChekId + " " +
                              "Union All " +
                              "Select CheckListId from dbo.FlatChecklist Where CheckListId=" + argChekId;
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                if (dt.Rows.Count > 0) { bans = true; }
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

            return bans;
        }

        public static bool BankIDFound(int argBankId)
        {
            bool bans = false;
            try
            {
                DataTable dt;
                string sSql = "Select BankId From dbo.BankDetails Where BankId=" + argBankId;
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                if (dt.Rows.Count > 0) { bans = true; }
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

            return bans;
        }

        public static bool LoanFound(int argFlatId)
        {
            bool bans = false;
            try
            {
                DataTable dt;
                string sSql = "Select PaymentOption From dbo.BuyerDetail Where FlatId=" + argFlatId;
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                if (dt.Rows.Count > 0) {if(dt.Rows[0]["PaymentOption"].ToString()=="L") bans = true; }
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

            return bans;
        }

        internal static bool BankFound(int argBranchId)
        {
            bool bans = false;
            try
            {
                DataTable dt;
                string sSql = "Select BranchId From dbo.BuyerDetail Where BranchId= " + argBranchId;
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                if (dt.Rows.Count > 0) { bans = true; }
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

            return bans;
        }

        public static void DeleteChekList(int argChekId)
        {
          
            try
            {
                DataTable dt;
                string sSql = "delete from dbo.CheckListMaster Where CheckListId = " + argChekId;
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();               
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
           
        }

        public static void DeleteBank(int argBankId)
        {
            try
            {
                DataTable dt;
                string sSql = "Delete From dbo.BankMaster Where BankId = " + argBankId;
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
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

        }

        public static DataTable getCheckList(string argType)
        {
            SqlDataAdapter da;
            DataTable dt = null;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select CheckListId,CheckListName,SortOrder,SysDefault from dbo.CheckListMaster Where TypeName = '" + argType + "' Order by SortOrder";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;

        }

        public static void InsertCheckListTrans(DataTable argdtTrans)
        {
            SqlCommand cmd = null;
            SqlConnection conn;
            string sSql = "";
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            string sDate = "";
            string RDate = "";
            try
            {
                for (int k = 0; k < argdtTrans.Rows.Count; k++)
                {
                    sDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(argdtTrans.Rows[k]["SubmitDate"].ToString()));
                    RDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(argdtTrans.Rows[k]["ReceiveDate"].ToString()));



                    sSql = "Insert into dbo.FinalizationCheckListTrans(FlatId,CheckListId,SubmitReq,SubmitDate,ReceiveDate)" +
                                  "Values(" + argdtTrans.Rows[k]["FlatId"].ToString() + "," + argdtTrans.Rows[k]["CheckListId"].ToString() + "," + argdtTrans.Rows[k]["SubmitReq"].ToString() + ",'" + sDate + "','" + RDate + "')";
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
                conn.Close();
            }           
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
                    
                    int ChkId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["CheckListId"].ToString(), CommFun.datatypes.vartypenumeric));
                    int iOrder = i + 1;
                    sSql = "Update dbo.CheckListMaster Set SortOrder=" + iOrder + " Where CheckListId=" + ChkId + " ";
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

        public static DataTable getCostCentre()
        {
            SqlDataAdapter da;
            DataTable dt = null;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select CostCentreId,CostCentreName,CAST(0 as bit) Approve From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre " +
                        " Where ProjectDB in(Select ProjectName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister " +
                        " Where BusinessType in('B','L')) and CostCentreId not in (Select CostCentreId " +
                        " From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") Order By CostCentreName";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;

        }

        public static DataTable GetSlabStructure(int argBranchId)
        {
            SqlDataAdapter da;
            DataTable dt = null;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select BranchId,LoanDescription,FromValue,ToValue,LoanPeriod,InterestRate,MortageValue From dbo.BankSlabStructure " +
                    " Where BranchId=" + argBranchId + "";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;

        }

        public static DataTable getBankCostCentre(int argBranchId)
        {
            SqlDataAdapter da;
            DataTable dt = null;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                //sSql = "SELECT CostCentreId,CostCentreName,CAST(0 as bit) Approve FROM dbo.OperationalCostCentre C ORDER BY CostCentreName";
                sSql = "SELECT CostCentreId,CostCentreName,CAST(0 as bit) Approve FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C " +
                        " WHERE CostCentreId NOT IN (SELECT CostCentreId FROM BankCC WHERE BranchId=" + argBranchId + ") And" +
                        " ProjectDB in(Select ProjectName from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister " +
                        " Where BusinessType in('B','L')) and CostCentreId not in (Select CostCentreId " +
                        " From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                        " UNION ALL  " +
                        " SELECT C.CostCentreId,CostCentreName,Approval Approve FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C " +
                        " LEFT JOIN BankCC B ON B.CostCentreId=C.CostCentreId WHERE B.BranchId=" + argBranchId + " And" +
                        " ProjectDB in(Select ProjectName from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister " +
                        " Where BusinessType in('B','L')) and C.CostCentreId not in (Select CostCentreId " +
                        " From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ")" +
                        " ORDER BY CostCentreName";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;

        }

        public static DataTable getBankBranch(int argBranchId)
        {
            SqlDataAdapter da;
            DataTable dt = null;
            string sSql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sSql = "Select * FROM BankDetails Where BranchId=" + argBranchId;
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;

        }

        public static void UpdateBankBranch(int argBranchId,DataTable argdt,DataTable argdtS)
        {
            SqlCommand cmd;
            string sSql = "";

            try
            {
                BsfGlobal.OpenCRMDB();
                if (argBranchId == 0)
                {
                    sSql = "INSERT INTO dbo.BankDetails(BankId,Branch,Address1,Address2,City,State,Pincode,Country, ContPerson," +
                        " IFSCCode,Mobile,Phone,Fax,IntRate,[LoanAmount%],ProcDays,ReqDocs,ProcessingFee,LegalFee,Insurance,Remarks) VALUES " +
                        " ( " + BankDetailsBO.BankId + ",'" + BankDetailsBO.BranchName + "','" + BankDetailsBO.Addr1 + "'," +
                        " '" + BankDetailsBO.Addr2 + "', '" + BankDetailsBO.City + "', '" + BankDetailsBO.State + "', " +
                        " '" + BankDetailsBO.PIN + "','" + BankDetailsBO.Country + "','" + BankDetailsBO.Contact + "', " +
                        " '" + BankDetailsBO.IFC + "', '" + BankDetailsBO.Mobile + "', '" + BankDetailsBO.Phone + "'," +
                        " '" + BankDetailsBO.FAX + "'," + BankDetailsBO.IntRate + "," + BankDetailsBO.LoanPer + ", " +
                        " " + BankDetailsBO.PrDays + ", '" + BankDetailsBO.Doc + "'," + BankDetailsBO.ProcFee + ","+
                        " " + BankDetailsBO.LegalFee + "," + BankDetailsBO.Insurance + ",'" + BankDetailsBO.Remarks + "')SELECT SCOPE_IDENTITY()";
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    argBranchId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();
                    //CommFun.InsertLog(DateTime.Now, "Bank Master-Add", "A", "Add Bank Master", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                    BsfGlobal.InsertLog(DateTime.Now, "Bank Master-Add", "A", "Add Bank Master", BankDetailsBO.BankId, 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                }
                else
                {
                    sSql = "UPDATE dbo.BankDetails SET BankId=" + BankDetailsBO.BankId + ",Branch='" + BankDetailsBO.BranchName + "'," +
                        " Address1='" + BankDetailsBO.Addr1 + "',Address2='" + BankDetailsBO.Addr2 + "',City='" + BankDetailsBO.City + "'," +
                        " State= '" + BankDetailsBO.State + "',Pincode='" + BankDetailsBO.PIN + "',Country='" + BankDetailsBO.Country + "'," +
                        " ContPerson='" + BankDetailsBO.Contact + "',IFSCCode='" + BankDetailsBO.IFC + "',Mobile='" + BankDetailsBO.Mobile + "'," +
                        " Phone='" + BankDetailsBO.Phone + "',Fax='" + BankDetailsBO.FAX + "',IntRate=" + BankDetailsBO.IntRate + "," +
                        " [LoanAmount%]=" + BankDetailsBO.LoanPer + ",ProcDays=" + BankDetailsBO.PrDays + ",ReqDocs='" + BankDetailsBO.Doc + "'," +
                        " ProcessingFee=" + BankDetailsBO.ProcFee + ",LegalFee=" + BankDetailsBO.LegalFee + ", " +
                        " Insurance=" + BankDetailsBO.Insurance + ",Remarks='" + BankDetailsBO.Remarks + "' " +
                        " WHERE BranchId=" + argBranchId + "";
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    //CommFun.InsertLog(DateTime.Now, "Bank Master-Edit", "E", "Edit Bank Master", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                    BsfGlobal.InsertLog(DateTime.Now, "Bank Master-Edit", "E", "Edit Bank Master", BankDetailsBO.BankId, 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                }

                sSql = "DELETE FROM dbo.BankCC WHERE BranchId=" + argBranchId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                for (int i = 0; i < argdt.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(argdt.Rows[i]["Approve"]) == true)
                    {
                        sSql = "INSERT INTO dbo.BankCC(BranchId,CostCentreId,Approval) VALUES(" + argBranchId + "," +
                            " " + argdt.Rows[i]["CostCentreId"] + ",1)";
                        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }

                sSql = "DELETE FROM dbo.BankSlabStructure WHERE BranchId=" + argBranchId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                for (int i = 0; i < argdtS.Rows.Count; i++)
                {
                    if (argdtS.Rows[i]["LoanDescription"].ToString() != "")
                    {
                        sSql = "INSERT INTO dbo.BankSlabStructure(BranchId,LoanDescription,FromValue,ToValue,LoanPeriod,InterestRate,MortageValue) VALUES" +
                            " (" + argBranchId + ",'" + argdtS.Rows[i]["LoanDescription"] + "'," + argdtS.Rows[i]["FromValue"] + "," +
                            " " + argdtS.Rows[i]["ToValue"] + ",'" + argdtS.Rows[i]["LoanPeriod"] + "'," + argdtS.Rows[i]["InterestRate"] + "," +
                            " " + argdtS.Rows[i]["MortageValue"] + ")";
                        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
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

        public static void DeleteBankBranch(int argBranchId)
        {
            SqlCommand cmd;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "DELETE FROM BankDetails WHERE BranchId=" + argBranchId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
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
        }

        public static DataTable GetBankBranchReg(int argBranchId)
        {
            SqlDataAdapter da; DataTable dt=null;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT A.BranchId,A.BankId,B.BankName,Branch,IFSCCode,IntRate FROM dbo.BankDetails A" +
                        " Inner Join dbo.BankMaster B On A.BankId=B.BankId " +
                        " Where BranchId=" + argBranchId + "";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
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

        public static DataTable GetBankRegister()
        {
            SqlDataAdapter da; DataTable dt=null;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT A.BranchId,A.BankId,Branch,B.BankName,IFSCCode,IntRate FROM dbo.BankDetails A" +
                    " Inner Join dbo.BankMaster B On A.BankId=B.BankId ORDER BY BankName";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
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

        #endregion
    }
}
