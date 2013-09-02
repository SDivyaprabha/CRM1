using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.BusinessObjects;
using System.Data.SqlClient;

namespace CRM.DataLayer
{
    public class OPPCampaignDL
    {

        #region Methods

        public bool OppNameFound(int argReqId, string argoppName,int argCCId)
        {
            bool bans = false;
            try
            {
                DataTable dt;
                string sSql = "Select RequestId From dbo.OpportunityRequest Where RequestId <> " + argReqId + " " +
                              "and OpportunityName = '" + argoppName + "' And CCId=" + argCCId + "";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                if (dt.Rows.Count > 0) { bans = true; }
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

            return bans;
        }

        public int InsertOpportunity(OpportunityBO argObject)
        {
            int identity = 0;
            try
            {
                SqlCommand cmd;
                string ReqDate = string.Format("{0:dd/MMM/yyyy}", argObject.ReqDate);
                string sSql = "Insert Into dbo.OpportunityRequest (RequestNo,RequestDate,ExecutiveId," +
                              "CCId,OpportunityName,Amount) " +
                              "Values('" + argObject.ReqNo + "', '" + ReqDate + "'," +
                              "'" + argObject.ExecutiveId + "', " + argObject.CCId + "," +
                              "'" + argObject.OppName + "', " + argObject.Amount + ") SELECT SCOPE_IDENTITY();";
                BsfGlobal.OpenCRMDB();
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                identity = int.Parse(cmd.ExecuteScalar().ToString());
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
            return identity;
        }

        public int InsertCampaign(CampaignBO argObject, DataTable argdtT, DataTable argdtQ)
        {
            int identity = 0;
            BsfGlobal.OpenCRMDB();

            try
            {
                SqlCommand cmd;
                string campaignDate = string.Format("{0:dd/MMM/yyyy}", argObject.CampaignDate);

                string sSql = "Insert Into dbo.CampaignDetails (CCId,RequestId,CampaignDate,CampaignName,DurationType,DurationPeriod,TotAmount,NetAmount) " +
                              "Values(" + argObject.CCId + ",'" + argObject.CReqId + "','" + campaignDate + "', '" + argObject.CampName + "','" +
                              argObject.DurType + "'," + argObject.DurPeriod + ", " + argObject.TotAmount + ", " + argObject.NetAmount + ") SELECT SCOPE_IDENTITY();";
                BsfGlobal.OpenCRMDB();
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                identity = int.Parse(cmd.ExecuteScalar().ToString());
                cmd.Dispose();

                if (argdtT != null)
                {
                    for (int i = 0; i < argdtT.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(argdtT.Rows[i]["Sel"]) == true)
                        {
                            sSql = "Insert Into dbo.CampaignTrans(CampaignId,CostCentreId,Amount,NetAmount)Values(" + identity + "," +
                              " " + argdtT.Rows[i]["CostCentreId"] + ",'" + argdtT.Rows[i]["Amount"] + "'," + argdtT.Rows[i]["NetAmount"] + ")";
                            cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                }

                if (argdtQ != null)
                {
                    for (int i = 0; i < argdtQ.Rows.Count; i++)
                    {
                        sSql = "Insert Into dbo.CampaignQualifier(CampaignId,CostCentreId,Description,Flag,Amount)Values(" + identity + "," + argdtQ.Rows[i]["CostCentreId"] + "," +
                          " '" + argdtQ.Rows[i]["Description"] + "','" + argdtQ.Rows[i]["Flag"] + "'," + argdtQ.Rows[i]["Amount"] + ")";
                        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

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
            return identity;
        }

        public static void InsertCampaignAmount(DataTable argDt,int argCampId)
        {
            try
            {
                SqlCommand cmd;
                string sSql = "";

                if (argDt.Rows.Count>0)
                {
                    sSql = "Delete From dbo.CampaignAmount Where CampaignId=" + argCampId + "";
                    BsfGlobal.OpenCRMDB();
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    for(int i=0;i<argDt.Rows.Count;i++)
                    {
                        sSql = "Insert Into dbo.CampaignAmount(CampaignId,Description,Flag,Amount)Values(" + argCampId + "," +
                        " '" + argDt.Rows[i]["Description"] + "','" + argDt.Rows[i]["Flag"] + "'," + argDt.Rows[i]["Amount"] + ")";
                        BsfGlobal.OpenCRMDB();
                        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
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
        }

        public void UpdateOpportunity(OpportunityBO argObject)
        {        

            try
            {
                SqlCommand cmd;
                string oppDate = string.Format("{0:dd/MMM/yyyy}", argObject.ReqDate);
                string sSql = "Update dbo.OpportunityRequest Set RequestNo='" + argObject.ReqNo + "',RequestDate='" + oppDate + "',ExecutiveId='" + argObject.ExecutiveId + "'," +
                              "CCId=" + argObject.CCId + ",OpportunityName='" + argObject.OppName + "',Amount=" + argObject.Amount + " Where RequestId=" + argObject.ReqId + "";
                BsfGlobal.OpenCRMDB();
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

        public DataTable GetCostCentre()
        {
            DataTable dt = null;
            try
            {
                string sSql = "Select CostCentreId, CostCentreName,ProjectDB From dbo.OperationalCostCentre";
                BsfGlobal.OpenWorkFlowDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                dt.Dispose();
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

        public DataTable GetOpportunity()
        {
            DataTable dt = null;
            try
            {
                BsfGlobal.OpenCRMDB();
                string sSql = "Select RequestNo,RequestId,OpportunityName,C.CostCentreName,C.CostCentreId From dbo.OpportunityRequest O" +
                                " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On O.CCId=C.CostCentreId" +
                                " Order By OpportunityName";
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
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
            return dt;
        }

        public static DataTable GetCampaignAmount(int argCampId)
        {
            DataTable dt = null;
            try
            {
                BsfGlobal.OpenCRMDB();
                string sSql = "Select CampaignId, CostCentreId, Description, Flag, Amount From dbo.CampaignQualifier Where CampaignId=" + argCampId + "";
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
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
            return dt;
        }

        public static decimal GetPrevCampaignAmount(int argOppId, int argCampId)
        {
            BsfGlobal.OpenCRMDB();
            decimal dCampAmount = 0;
            try
            {
                string sSql = "Select ISNULL(SUM(NetAmount),0) Amount From dbo.CampaignDetails Where RequestId=" + argOppId + " AND CampaignId<>" + argCampId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                dCampAmount = Convert.ToDecimal(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
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
            return dCampAmount;
        }

        public DataTable GetCampaign(int argCCId)
        {
            DataTable dt = null;
            try
            {
                string sSql = "Select A.RequestId,A.CampaignId,A.CampaignName From dbo.CampaignDetails A" +
                                " Inner Join dbo.OpportunityRequest B On A.RequestId=B.RequestId Where B.CCId=" + argCCId + "" +
                                " Order By CampaignName ";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
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
            return dt;
        }

        public DataTable GetCampaignDetails()
        {
            DataTable dt = null;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                SqlCommand cmd;
                sSql = "Update CampaignDetails Set NetAmount=TotAmount Where NetAmount=0";
                cmd = new SqlCommand(sSql,BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "Select Distinct A.RequestId,A.CampaignId,A.CampaignName From dbo.CampaignDetails A" +
                                " Order By CampaignName";
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
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
            return dt;
        }

        public DataTable GetExecutive()
        {
            DataTable dt = null;
            try
            {
                string sSql = "Select UserId ExecId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As ExecName,0 Sel From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId Where B.PositionType='M' ORDER BY EmployeeName";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
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
            return dt;
        }

        public DataTable GetOPPReqDetails(int argReqId)
        {
            DataTable dt = null;
            try
            {
                string sSql = "Select A.*,B.CostCentreName From dbo.OpportunityRequest A Inner Join ["+ BsfGlobal.g_sWorkFlowDBName +"].dbo.OperationalCostCentre B"+
                    " On A.CCId=B.CostCentreId where RequestId=" + argReqId + " ";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
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
            return dt;
        }

        public DataTable GetOpCostCentre()
        {
            DataTable dt = null;
            try
            {
                string sSql = "";
                //string sSql = "Select CostCentreId,CostCentreName,ProjectDB from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre Order By CostCentreName";
                sSql = "Select CostCentreId,CostCentreName,ProjectDB From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre " +
                        " Where ProjectDB in(Select ProjectName from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister " +
                        " Where BusinessType in('B','L')) and CostCentreId not in (Select CostCentreId " +
                        " From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") Order By CostCentreName";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
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
            return dt;
        }

        public DataTable GetOppId(int argCCId)
        {
            DataTable dt = null;
            try
            {
                string sSql = "Select RequestId,OpportunityName From dbo.OpportunityRequest " +
                    " Where CCId=" + argCCId + " Order By OpportunityName";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
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
            return dt;
        }

        public DataTable GetOpReq()
        {
            DataTable dt = null;
            try
            {
                string sSql = "Select RequestId,RequestNo From dbo.OpportunityRequest";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
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
            return dt;
        }

        public void UpdateCampaign(CampaignBO argObject,DataTable argdtT,DataTable argdtQ)
        {
            try
            {
                SqlCommand cmd;
                string campaignDate = string.Format("{0:dd/MMM/yyyy}", argObject.CampaignDate);
                string sSql = "Update dbo.CampaignDetails set CCId=" + argObject.CCId + ",RequestId='" + argObject.CReqId + "',CampaignDate='" + campaignDate + "', CampaignName='" +
                    argObject.CampName + "',DurationType='" + argObject.DurType + "',DurationPeriod=" + argObject.DurPeriod + ",TotAmount=" +
                    argObject.TotAmount + ",NetAmount=" + argObject.NetAmount + " Where CampaignId=" + argObject.CampId + "";
                BsfGlobal.OpenCRMDB();
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB); 
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "Delete From dbo.CampaignTrans Where CampaignId=" + argObject.CampId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "Delete From dbo.CampaignQualifier Where CampaignId=" + argObject.CampId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                if (argdtT != null)
                {
                    for (int i = 0; i < argdtT.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(argdtT.Rows[i]["Sel"]) == true)
                        {
                            sSql = "Insert Into dbo.CampaignTrans(CampaignId,CostCentreId,Amount,NetAmount)Values(" + argObject.CampId + "," +
                              " " + argdtT.Rows[i]["CostCentreId"] + ",'" + argdtT.Rows[i]["Amount"] + "'," + argdtT.Rows[i]["NetAmount"] + ")";
                            cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                }

                if (argdtQ != null)
                {
                    for (int i = 0; i < argdtQ.Rows.Count; i++)
                    {
                        sSql = "Insert Into dbo.CampaignQualifier(CampaignId,CostCentreId,Description,Flag,Amount)Values(" + argObject.CampId + "," + argdtQ.Rows[i]["CostCentreId"] + "," +
                          " '" + argdtQ.Rows[i]["Description"] + "','" + argdtQ.Rows[i]["Flag"] + "'," + argdtQ.Rows[i]["Amount"] + ")";
                        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                    }
                }

                //if (argdt != null)
                //{
                //    if (argdt.Rows.Count > 0)
                //    {
                //        sSql = "Delete From dbo.CampaignQualifier Where CampaignId=" + argObject.CampId + "";
                //        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                //        cmd.ExecuteNonQuery();
                //        cmd.Dispose();

                //        for (int i = 0; i < argdt.Rows.Count; i++)
                //        {
                //            sSql = "Insert Into dbo.CampaignQualifier(CampaignId,Description,Flag,Amount)Values(" + argObject.CampId + "," +
                //            " '" + argdt.Rows[i]["Description"] + "','" + argdt.Rows[i]["Flag"] + "'," + argdt.Rows[i]["Amount"] + ")";
                //            cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                //            cmd.ExecuteNonQuery();
                //            cmd.Dispose();
                //        }
                //    }
                //}
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

        public bool OPPFound(int argReqId)
        {
            bool bans = false;
            try
            {
                DataTable dt;
                string sSql = "Select RequestId From dbo.CampaignDetails Where RequestId = " + argReqId;
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                if (dt.Rows.Count > 0) { bans = true; }
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

            return bans;
        }

        public void DeleteOpportunity(int argReqId)
        {
            try
            {
                SqlCommand cmd;
                string sSql = "Delete From dbo.OpportunityRequest Where RequestId = " + argReqId;
                BsfGlobal.OpenCRMDB();
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

        public void DeleteCampaign(int argCampId)
        {
            try
            {
                SqlCommand cmd;
                string sSql = "Delete From dbo.CampaignDetails Where CampaignId = " + argCampId;
                BsfGlobal.OpenCRMDB();
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

        public DataTable GetCampaignDetails(int argcampId)
        {
            DataTable dt = null;
            try
            {
                string sSql = "Select C.CCId,O.OpportunityName,C.CampaignName,C.CampaignDate,DurationPeriod,"+
                               " DurationType,TotAmount,C.NetAmount,C.RequestId "+
                               " From dbo.CampaignDetails C " +
                               " Inner Join dbo.OpportunityRequest O" +
                               " On C.RequestId=O.RequestId Where CampaignId=" + argcampId + "";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
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
            return dt;
        }

        public int GetReqId(string argReqNo)
        {
            int getReqId = 0;         
            SqlCommand Cmd;
            string sSql = "";
            try
            {
                sSql = "Select RequestId From dbo.OpportunityRequest Where RequestNo = '" + argReqNo + "'";
                BsfGlobal.OpenCRMDB();
                Cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                getReqId = Convert.ToInt32(Cmd.ExecuteScalar());
                Cmd.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return getReqId;
        }

        internal static DataTable GetCampaignList(int argReqId, int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Select CampaignDate, CampaignName, TotAmount From dbo.CampaignDetails Where RequestId=" + argReqId + " And CCId=" + argCCId + "";
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
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

        internal static DataTable GetCampaignListTrans(int argCampId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            string sSql = "";
            try
            {
                if (argCampId == 0)
                {
                    sSql = " Select A.CostCentreId,A.CostCentreName, 0.00 Amount,0.00 NetAmount, CONVERT(bit, 0, 1) Sel From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre A " +
                            " Left Join dbo.CampaignTrans B On A.CostCentreId=B.CostCentreId " +
                            " Where ProjectDB In(Select ProjectName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType In('B','L')) " +
                            " and A.CostCentreId not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                            " Where UserId=" + BsfGlobal.g_lUserId + ") Order By A.CostCentreName";
                }
                else
                {
                    sSql = " Select A.CostCentreId,A.CostCentreName,IsNull(Amount,0)Amount,IsNull(NetAmount,0)NetAmount, " +
                            " Case When B.CostCentreId IS NULL Then cast(0 as bit) Else cast(1 as bit) End as Sel From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre A " +
                            " Left Join dbo.CampaignTrans B On A.CostCentreId=B.CostCentreId And B.CampaignId=" + argCampId + " " +
                            " Where ProjectDB In(Select ProjectName From " +
                            " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType In('B','L')) " +
                            " and A.CostCentreId not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                            " Where UserId=" + BsfGlobal.g_lUserId + ") Order By A.CostCentreName";
                }
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                da.Fill(dt);
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

        public DataSet GetCampDetails(int argReqId, string argFromDate, string argToDate)
        {
            DataSet ds = new DataSet();
            string sSql = "";
            try
            {
                //sSql = "Select B.CCId CostCentreId,A.CampaignId,A.CampaignDate,A.CampaignName,C.CostCentreName,A.TotAmount From dbo.CampaignDetails  A " +
                //       "Inner Join dbo.OpportunityRequest B on A.RequestId=B.RequestId " +
                //       "Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C on B.CCId = C.CostCentreId " +
                //       " Where A.RequestId=" + argReqId + " " +
                //       " And A.CampaignDate Between '" + argFromDate + "' And '" + argToDate + "'  " +
                //       " Order by A.CampaignDate,A.CampaignId";
                sSql = "Select CT.CostCentreId,A.CampaignId,A.CampaignDate,A.CampaignName,C.CostCentreName,CT.NetAmount TotAmount From dbo.CampaignDetails  A " +
                        " Inner Join dbo.CampaignTrans CT On CT.CampaignId=A.CampaignId " +
                        " Inner Join dbo.OpportunityRequest B on A.RequestId=B.RequestId " +
                        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On CT.CostCentreId = C.CostCentreId  " +
                        " Where A.RequestId=" + argReqId + " And A.CampaignDate Between '" + argFromDate + "' And '" + argToDate + "' Order by A.CampaignDate,A.CampaignId";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Campaign");
                da.Dispose();

                sSql = "Select A.CostCentreId,A.CampaignId,COUNT(A.LeadID) CLead From dbo.LeadProjectInfo  A " +
                        " Inner Join dbo.CampaignDetails B On A.CampaignId=B.CampaignId " +
                        " Where B.CampaignDate Between '" + argFromDate + "' And '" + argToDate + "'  " +
                        " Group by A.CostCentreId,A.CampaignId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "LeadCount");
                da.Dispose();

                sSql = "Select A.CostCentreId,P.CampaignId,Count(A.FlatId) CFlats,SUM(A.NetAmt) Amt From dbo.FlatDetails A " +
                        " Inner Join dbo.LeadFlatInfo B on A.FlatId=B.FlatId " +
                        " Inner Join dbo.LeadRegister C on C.LeadId=B.LeadId" +
                        " Inner Join dbo.LeadProjectInfo P On P.LeadId=C.LeadId" +
                        " Inner Join dbo.CampaignDetails D On D.CampaignId=P.CampaignId " +
                        " Where D.CampaignDate Between '" + argFromDate + "' And '" + argToDate + "'  " +
                        " Group by A.CostCentreId,P.CampaignId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "FlatCount");
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

        public DataSet GetOpportunityDetails(string argFromDate, string argToDate)
        {
            DataSet ds = new DataSet();
            string sSql = "";
            try
            {
                sSql = "Select A.RequestId,A.RequestDate,A.OpportunityName,C.CostCentreId,C.CostCentreName,A.Amount From dbo.OpportunityRequest A " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C on A.CCId = C.CostCentreId " +
                        " Where C.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                        " And A.RequestDate Between '" + argFromDate + "' And '" + argToDate + "' " +
                        " Order by A.RequestDate,A.RequestId";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Opportunity");
                da.Dispose();

                //sSql = "Select C.RequestId,COUNT(LeadID) CLead From dbo.LeadRegister A Inner Join dbo.CampaignDetails B On" +
                //       " A.CampaignId=B.CampaignId Inner Join dbo.OpportunityRequest C On B.RequestId=C.RequestId "+
                //       " Group by C.RequestId";
                sSql = "Select C.RequestId,COUNT(A.LeadID) CLead From dbo.LeadRegister A " +
                        " Inner Join dbo.LeadProjectInfo P On P.LeadId=A.LeadId" +
                        " Inner Join dbo.CampaignDetails B On P.CampaignId=B.CampaignId" +
                        " Inner Join dbo.OpportunityRequest C On B.RequestId=C.RequestId "+
                        " Where RequestDate Between '" + argFromDate + "' And '" + argToDate + "' " +
                        " Group by C.RequestId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "LeadCount");
                da.Dispose();

                //sSql = "Select D.RequestId,Count(A.FlatId) CFlats,SUM(A.NetAmt) Amt From dbo.FlatDetails A" +
                //        " Inner Join dbo.LeadFlatInfo B on A.FlatId=B.FlatId " +
                //        " Inner Join dbo.LeadRegister C on C.LeadId=B.LeadId Inner Join dbo.CampaignDetails D On" +
                //        " C.CampaignId=D.CampaignId Inner Join dbo.OpportunityRequest E On D.RequestId=E.RequestId"+
                //        " Group by D.RequestId";
                sSql = "Select D.RequestId,Count(A.FlatId) CFlats,SUM(A.NetAmt) Amt From dbo.FlatDetails A " +
                        " Inner Join dbo.LeadFlatInfo B on A.FlatId=B.FlatId  " +
                        " Inner Join dbo.LeadRegister C on C.LeadId=B.LeadId " +
                        " Inner Join dbo.LeadProjectInfo P On P.LeadId=C.LeadId" +
                        " Inner Join dbo.CampaignDetails D On P.CampaignId=D.CampaignId " +
                        " Inner Join dbo.OpportunityRequest E On D.RequestId=E.RequestId " +
                        " Where RequestDate Between '" + argFromDate + "' And '" + argToDate + "' " +
                        " Group by D.RequestId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "FlatCount");
                da.Dispose();

                sSql = "Select A.RequestId,Sum(CT.NetAmount) TotAmount From dbo.CampaignDetails  A " +
                        " Inner Join dbo.CampaignTrans CT On CT.CampaignId=A.CampaignId "+
                        " Inner Join dbo.OpportunityRequest B on A.RequestId=B.RequestId " +
                        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C on B.CCId=C.CostCentreId" +
                        " Where B.RequestDate Between '" + argFromDate + "' And '" + argToDate + "' " +
                        " Group by A.RequestId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Campaign");
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

        public DataSet GetCostOpportunityDetails()
        {
            DataSet ds = new DataSet();
            string sSql = "";
            try
            {
                sSql = "Select A.CCId,A.RequestId,C.CostCentreName,A.OpportunityName,A.Amount From dbo.OpportunityRequest  A " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C on A.CCId = C.CostCentreId" +
                        " Where C.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                        " Order by A.RequestDate,A.RequestId";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Opportunity");
                da.Dispose();
                
                //sSql = "Select A.RequestId,Sum(A.TotAmount) TotAmount From dbo.CampaignDetails  A " +
                //        " Inner Join dbo.OpportunityRequest B on A.RequestId=B.RequestId " +
                //        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C on B.CCId=C.CostCentreId" +
                //        " Group by A.RequestId";
                sSql = "Select A.RequestId,Sum(CT.NetAmount) TotAmount From dbo.CampaignDetails A  " +
                        " Inner Join dbo.CampaignTrans CT On CT.CampaignId=A.CampaignId " +
                        " Inner Join dbo.OpportunityRequest B On A.RequestId=B.RequestId  " +
                        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On B.CCId=C.CostCentreId Group by A.RequestId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Campaign");
                da.Dispose();

                sSql = "Select CostCentreId,Sum(Area) Area From dbo.FlatDetails Group By CostCentreId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Area");
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

        public DataTable GetCostCampDetails(int argReqId)
        {
            DataTable ds = new DataTable();
            string sSql = "";
            try
            {
                //sSql = "Select A.CampaignId,C.CostCentreName,A.CampaignName,A.TotAmount From dbo.CampaignDetails  A " +
                //       "Inner Join dbo.OpportunityRequest B On A.RequestId=B.RequestId " +
                //       "Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C on B.CCId = C.CostCentreId " +
                //       "Where A.RequestId=" + argReqId + " Order by A.CampaignDate,A.CampaignId";
                sSql = "Select A.CampaignId,C.CostCentreName,A.CampaignName,CT.NetAmount TotAmount From dbo.CampaignDetails A " +
                        " Inner Join dbo.CampaignTrans CT On CT.CampaignId=A.CampaignId " +
                        " Inner Join dbo.OpportunityRequest B On A.RequestId=B.RequestId " +
                        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C on CT.CostCentreId = C.CostCentreId " +
                        " Where A.RequestId=" + argReqId + " Order by A.CampaignDate,A.CampaignId";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds);
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

        public DataSet GetCampDailyDetails(DateTime argDate)
        {
            DataSet ds = new DataSet();
            string sSql = "";
            try
            {
                //sSql = "Select A.CampaignId,C.CostCentreId,C.CostCentreName,A.CampaignName From dbo.CampaignDetails  A " +
                //        " Inner Join dbo.OpportunityRequest B On A.RequestId=B.RequestId " +
                //        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C on B.CCId=C.CostCentreId " +
                //        " Where C.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                //        " Order by A.CampaignDate,A.CampaignId";
                sSql = "Select A.CampaignId,C.CostCentreId,C.CostCentreName,A.CampaignName From dbo.CampaignDetails  A  " +
                        " Inner Join dbo.CampaignTrans CT On CT.CampaignId=A.CampaignId " +
                        " Inner Join dbo.OpportunityRequest B On A.RequestId=B.RequestId  " +
                        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C on CT.CostCentreId=C.CostCentreId  " +
                        " Where C.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                        " Where UserId=" + BsfGlobal.g_lUserId + ")  Order by A.CampaignDate,A.CampaignId";
                BsfGlobal.OpenCRMDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Campaign");
                da.Dispose();

                sSql = "Select A.LeadId,A.ProjectId CostCentreId,B.CampaignId,Count(Distinct A.NatureID) Email From CallSheet A  " +
                        " Inner Join LeadProjectInfo B On A.LeadId=B.LeadId " +
                        " Inner Join NatureMaster C On C.NatureId=A.NatureID " +
                        " Where C.Description='Email' And A.TrnDate='" + Convert.ToDateTime(argDate).ToString("dd-MMM-yyyy") + "' " +
                        " Group By A.LeadId,A.ProjectId,B.CampaignId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Email");
                da.Dispose();

                sSql = "Select A.LeadId,A.ProjectId CostCentreId,B.CampaignId,Count(Distinct A.NatureID) Telephone From CallSheet A  " +
                        " Inner Join LeadProjectInfo B On A.LeadId=B.LeadId " +
                        " Inner Join NatureMaster C On C.NatureId=A.NatureID " +
                        " Where C.Description='Telephone' And A.TrnDate='" + Convert.ToDateTime(argDate).ToString("dd-MMM-yyyy") + "' " +
                        " Group By A.LeadId,A.ProjectId,B.CampaignId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Telephone");
                da.Dispose();

                sSql = "Select A.LeadId,A.ProjectId CostCentreId,B.CampaignId,Count(Distinct A.NatureID) SiteVisit From CallSheet A  " +
                        " Inner Join LeadProjectInfo B On A.LeadId=B.LeadId " +
                        " Inner Join NatureMaster C On C.NatureId=A.NatureID " +
                        " Where C.Description='Sitevisit' And A.TrnDate='" + Convert.ToDateTime(argDate).ToString("dd-MMM-yyyy") + "' " +
                        " Group By A.LeadId,A.ProjectId,B.CampaignId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "SiteVisit");
                da.Dispose();

                sSql = "Select A.LeadId,A.ProjectId CostCentreId,B.CampaignId,Count(Distinct A.NatureID) Website From CallSheet A  " +
                        " Inner Join LeadProjectInfo B On A.LeadId=B.LeadId " +
                        " Inner Join NatureMaster C On C.NatureId=A.NatureID " +
                        " Where C.Description='Website' And A.TrnDate='" + Convert.ToDateTime(argDate).ToString("dd-MMM-yyyy") + "' " +
                        " Group By A.LeadId,A.ProjectId,B.CampaignId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "Website");
                da.Dispose();

                sSql = "Select Count(SMSSent) BulkSMS From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.PendingSMS Where SMSSent=1 " +
                    " And CONVERT(varchar(10),TransDate,103)='" + Convert.ToDateTime(argDate).ToString("dd/MM/yyyy") + "'";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "BulkSMS");
                da.Dispose();

                sSql = "Select Count(EmailSent) BulkEmail From dbo.PendingEmail Where EmailSent=1 " +
                    " And CONVERT(varchar(10),TransDate,103)='" + Convert.ToDateTime(argDate).ToString("dd/MM/yyyy") + "'";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                da.Fill(ds, "BulkEmail");
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
        

        #endregion
    }
}