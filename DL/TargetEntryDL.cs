using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CRM.BO;

namespace CRM.DL
{
    class TargetEntryDL
    {
        #region Methods

        internal static int InsertTargetMaster(string s_Mode, TargetEntryBO TarEntBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtTarMas = null;
            SqlDataAdapter sda;
            SqlCommand sCmd;
            string s_TarDate = "";
            string s_FromDate = "";
            int i_TargetId = 0;

            try
            {
                s_TarDate = string.Format("{0:dd/MMM/yyyy}", TarEntBO.DE_TargetDate);
                s_FromDate = string.Format("{0:dd/MMM/yyyy}", TarEntBO.DE_FromDate);

                if (s_Mode == "A")
                {
                    ssql = String.Format("Insert into TargetMaster(TargetDate, RefNo, FromDate, CostCentre, PeriodType, TargetType, Incentive, IncentiveType, IncentiveFrom, NoofPeriods) Values('{0}', '{1}', '{2}', {3}, '{4}', '{5}', {6}, '{7}', '{8}', {9}) SELECT SCOPE_IDENTITY();", s_TarDate, TarEntBO.s_RefNo, s_FromDate, TarEntBO.i_CostCentreId, TarEntBO.s_PeriodType, TarEntBO.s_TargetType, TarEntBO.d_Incentive, TarEntBO.s_IncenType, TarEntBO.s_Incentivefrom, TarEntBO.i_NoOfPeriod);
                    sda = new SqlDataAdapter(ssql, BsfGlobal.OpenCRMDB());
                    dtTarMas = new DataTable();
                    sda.Fill(dtTarMas);
                    i_TargetId = Convert.ToInt32(dtTarMas.Rows[0]["Column1"].ToString());
                    TarEntBO.i_TargetId = i_TargetId;
                }
                else
                {
                    ssql = String.Format("Update TargetMaster set TargetDate='{0}', RefNo='{1}', FromDate='{2}', CostCentre={3}, PeriodType='{4}', TargetType='{5}', Incentive={6}, IncentiveType='{7}', IncentiveFrom='{8}', NoofPeriods={9} Where TargetId={10}", s_TarDate, TarEntBO.s_RefNo, s_FromDate, TarEntBO.i_CostCentreId, TarEntBO.s_PeriodType, TarEntBO.s_TargetType, TarEntBO.d_Incentive, TarEntBO.s_IncenType, TarEntBO.s_Incentivefrom, TarEntBO.i_NoOfPeriod, TarEntBO.i_TargetId);
                    sCmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                    sCmd.ExecuteNonQuery();
                }
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return i_TargetId;
        }

        internal static DataTable InsertTargetTrans(string s_Mode, TargetEntryBO TarEntBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtTarTrans = null;
            SqlCommand sCmd;

            try
            {
                if (s_Mode == "A")
                {
                    ssql = String.Format("Insert into TargetTrans(TargetId, ExecutiveId, TMonth, TYear, TValue) Values({0} , {1}, {2}, {3}, {4})", TarEntBO.i_TargetId, TarEntBO.i_ExeId, TarEntBO.i_TMonth, TarEntBO.i_TYear, TarEntBO.d_TValue);
                    sCmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                    sCmd.ExecuteNonQuery();
                    sCmd.Dispose();

                    ssql = "Insert into TargetAmtTrans(TargetId, ExecutiveId, TMonth, TYear, TValue) Values(" +
                        " " + TarEntBO.i_TargetId + "," + TarEntBO.i_ExeId + "," + TarEntBO.i_TMonth + "," + TarEntBO.i_TYear + "," +
                        " " + TarEntBO.d_UnitValue + ")";
                    sCmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                    sCmd.ExecuteNonQuery();
                    sCmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update TargetTrans set ExecutiveId={0}, TMonth={1}, TYear={2},TValue={3} Where TargetTransId={4} AND TargetId={5}", TarEntBO.i_ExeId, TarEntBO.i_TMonth, TarEntBO.i_TYear, TarEntBO.d_TValue, TarEntBO.i_TargetTransId, TarEntBO.i_TargetId);
                    sCmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                    sCmd.ExecuteNonQuery();
                    sCmd.Dispose();

                    ssql = "Update TargetAmtTrans set ExecutiveId=" + TarEntBO.i_ExeId + ", TMonth=" + TarEntBO.i_TMonth + "," +
                        " TYear=" + TarEntBO.i_TYear + ",TValue=" + TarEntBO.d_UnitValue + " Where TargetTransId=" + TarEntBO.i_TargetTransUnitId + " " +
                        " AND TargetId=" + TarEntBO.i_TargetId + "";
                    sCmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                    sCmd.ExecuteNonQuery();
                    sCmd.Dispose();
                }

            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtTarTrans;
        }

        internal static DataTable InsertIncentiveTrans(string s_Mode, DataTable dt, TargetEntryBO TargetBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dtIncenTrans = null;
            SqlCommand sCmd;

            try
            {
                
                
                    //if (s_Mode == "A")
                    //{
                    ssql = "Delete From IncentiveTrans Where TargetId=" + TargetBO.i_TargetId + "";
                    sCmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                    sCmd.ExecuteNonQuery();
                    sCmd.Dispose();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (s_Mode == "A")
                            if(TargetBO.s_IncenType=="L")
                                ssql = String.Format("Insert into IncentiveTrans(TargetId, FromValue, ToValue, IncValue) Values({0} , {1}, {2}, {3})", TargetBO.i_TargetId,CommFun.IsNullCheck(dt.Rows[i]["From"], CommFun.datatypes.vartypenumeric),CommFun.IsNullCheck( dt.Rows[i]["To"], CommFun.datatypes.vartypenumeric),CommFun.IsNullCheck( dt.Rows[i]["Amount"], CommFun.datatypes.vartypenumeric));
                        else ssql = String.Format("Insert into IncentiveTrans(TargetId, FromValue, ToValue, IncValue) Values({0} , {1}, {2}, {3})", TargetBO.i_TargetId,CommFun.IsNullCheck( dt.Rows[i]["From"], CommFun.datatypes.vartypenumeric),CommFun.IsNullCheck( dt.Rows[i]["To"], CommFun.datatypes.vartypenumeric),CommFun.IsNullCheck( dt.Rows[i]["RateofIncentive"], CommFun.datatypes.vartypenumeric));
                        else
                            ssql = String.Format("Insert into IncentiveTrans(TargetId, FromValue, ToValue, IncValue) Values({0} , {1}, {2}, {3})", TargetBO.i_TargetId, CommFun.IsNullCheck(dt.Rows[i]["FromValue"], CommFun.datatypes.vartypenumeric), CommFun.IsNullCheck(dt.Rows[i]["ToValue"], CommFun.datatypes.vartypenumeric), CommFun.IsNullCheck(dt.Rows[i]["IncValue"], CommFun.datatypes.vartypenumeric));

                        sCmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                        sCmd.ExecuteNonQuery();
                        sCmd.Dispose();

                    }
                    //}
                    //else
                    //{
                    //    ssql = String.Format("Update IncentiveTrans set FromValue={0}, ToValue={1}, IncValue={2} Where IncentiveId={3} AND TargetId={4}", dt.Rows[i]["FromValue"], dt.Rows[i]["ToValue"], dt.Rows[i]["IncValue"], TargetBO.i_IncentiveId, TargetBO.i_TargetId);
                    //    sCmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                    //    sCmd.ExecuteNonQuery();
                    //}
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dtIncenTrans;
        }

        internal static DataTable GetCostCentre()
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            DataTable dt = null;
            SqlDataAdapter sda;

            try
            {
                //ssql = String.Format("Select CostCentreId, CostCentreName from ["+ BsfGlobal.g_sWorkFlowDBName +"].dbo.OperationalCostCentre Order By CostCentreName");
                sSql = "Select CostCentreId,CostCentreName From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                        " Where ProjectDB in(Select ProjectName from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister " +
                        " Where BusinessType in('B','L')) and CostCentreId not in (Select CostCentreId " +
                        " From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans " +
                        " Where UserId=" + BsfGlobal.g_lUserId + ") Order By CostCentreName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable GetExecutive(string argId)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dt = null;
            SqlDataAdapter sda;

            try
            {
                if (argId == "")
                    ssql = "Select 0 RowId,UserId ExecutiveId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As ExecutiveName,Convert(bit,0,0) Sel " +
                    " From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                    " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId " +
                    " Where B.PositionType='M' ";
                else
                    ssql = "Select 0 RowId,UserId ExecutiveId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As ExecutiveName,Convert(bit,0,0) Sel " +
                        " From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId " +
                        " Where B.PositionType='M' And UserId NOT IN(" + argId + ")";
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static bool NoofPerFound(int argExecId,int argNoofper,int argCCId)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dt = null;
            SqlDataAdapter sda;
            bool bAns=false;

            try
            {
                ssql = " Select Distinct A.NoofPeriods From TargetMaster A Inner Join TargetTrans B On A.TargetId=B.TargetId" +
                       " Where B.ExecutiveId=" + argExecId + " And A.CostCentre=" + argCCId + " And A.NoofPeriods=" + argNoofper + "";
                sda = new SqlDataAdapter(ssql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0) bAns = true;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return bAns;
        }

        public static DataSet GetProjectReport()
        {
            DataSet ds = new DataSet();
            string sSql = "";
            //sSql = "Select Distinct A.CostCentre,C.CostCentreName,B.ExecutiveId,EmployeeName ExecName From TargetMaster A " +
            //        " Left Join TargetTrans B On A.TargetId=B.TargetId" +
            //        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On A.CostCentre=C.CostCentreId" +
            //        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users D On D.UserId=B.ExecutiveId " +
            //        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position E on E.PositionId=D.PositionId Where E.PositionType='M'";
            sSql = "Select Distinct C.CostCentreId,C.CostCentreName,D.UserId ExecutiveId,Case When D.EmployeeName='' Then D.UserName Else D.EmployeeName End As ExecName From  " +
            " [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users D On D.UserId=D.UserId" +
            " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position E on E.PositionId=D.PositionId Where E.PositionType='M'";

            BsfGlobal.OpenCRMDB();
            SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "Executive");
            da.Dispose();

            //sSql = "Select ExecutiveId,COUNT(LeadID) CLead From LeadRegister Group by ExecutiveId";
            sSql = "Select CostCentreId,ExecutiveId,COUNT(LeadID) CLead From LeadRegister Group by CostCentreId,ExecutiveId";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "LeadCount");
            da.Dispose();

            //sSql = "Select ExecutiveId,Count(A.FlatId) CFlats,SUM(A.BaseAmt) Amt From FlatDetails A " +
            //        " Inner Join LeadFlatInfo B on A.FlatId=B.FlatId" +
            //        " Inner Join LeadRegister C on C.LeadId=B.LeadId" +
            //        " Group by ExecutiveId";
            sSql = "Select A.CostCentreId,ExecutiveId,Count(A.FlatId) CFlats,SUM(A.BaseAmt) Amt From FlatDetails A  " +
                    " Inner Join LeadFlatInfo B on A.FlatId=B.FlatId " +
                    " Inner Join LeadRegister C on C.LeadId=B.LeadId Group by A.CostCentreId,ExecutiveId";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "FlatCount");
            da.Dispose();

            BsfGlobal.g_CRMDB.Close();

            return ds;

        }

        public static DataSet GetBuyerReport(int argExecId)
        {
            DataSet ds = new DataSet();
            string sSql = "";
            sSql = "Select A.CostCentre,C.ExecutiveId,Case When D.EmployeeName='' Then D.UserName Else D.EmployeeName End As ExecName,SUM(IncValue) IncentivePayable,0 Paid,0 Balance From TargetMaster A " +
                    " Inner Join IncentiveTrans B On A.TargetId=B.TargetId" +
                    " Inner Join TargetTrans C On C.TargetId=A.TargetId" +
                    " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users D On D.UserId=C.ExecutiveId " +
                    " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position E on E.PositionId=D.PositionId " +
                    " Where E.PositionType='M'And A.CostCentre=" + argExecId + "" +
                    " Group By CostCentre,ExecutiveId,EmployeeName";

            BsfGlobal.OpenCRMDB();
            SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "Executive");
            da.Dispose();

            sSql = "Select ExecutiveId,SUM(A.BaseAmt) Amt From FlatDetails A " +
                    " Inner Join LeadFlatInfo B on A.FlatId=B.FlatId" +
                    " Inner Join LeadRegister C on C.LeadId=B.LeadId" +
                    " Group by ExecutiveId";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "FlatCount");
            da.Dispose();

            BsfGlobal.g_CRMDB.Close();

            return ds;

        }

        public static DataSet GetExecReport(int argCCId,DateTime argMonth)
        {
            DataSet ds = new DataSet();
            string sSql = "";
            sSql = " Select A.CostCentreId,C.ExecutiveId,Case When D.EmployeeName='' Then D.UserName Else D.EmployeeName End As ExecName," +
                    " SUM(IncValue) IncentivePayable,0 Paid,0 Balance From TargetMaster A " +
                    " Inner Join IncentiveTrans B On A.TargetId=B.TargetId" +
                    " Inner Join TargetTrans C On C.TargetId=A.TargetId" +
                    " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users D On D.UserId=C.ExecutiveId " +
                    " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position E on E.PositionId=D.PositionId " +
                    " Where E.PositionType='M' And A.CostCentreId=" + argCCId + " And C.ExecutiveId=" + BsfGlobal.g_lUserId + " " +
                    " Group By CostCentreId,C.ExecutiveId,D.EmployeeName,D.UserName";
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "Executive");
            da.Dispose();

            sSql = " Select D.ExecutiveId,SUM(A.BaseAmt) Amt From dbo.FlatDetails A  " +
                     " Inner Join dbo.LeadFlatInfo B on A.FlatId=B.FlatId " +
                     " Inner Join dbo.LeadRegister C on C.LeadId=B.LeadId " +
                     " Inner Join dbo.LeadExecutiveInfo D On D.LeadId=C.LeadId  " +
                     " Inner Join dbo.BuyerDetail E On E.FlatId=A.FlatId And A.LeadId=E.LeadId And E.Status='S' " +
                     " Where MONTH(E.FinaliseDate)=" + argMonth.Month +
                     " Group by D.ExecutiveId";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "FlatCount");
            da.Dispose();

            sSql = " Select B.ExecutiveId,Sum(TValue)TValue From TargetMaster A Inner Join TargetTrans B On A.TargetId=B.TargetId " +
                     " Where TMonth=" + argMonth.Month + " And TYear=" + argMonth.Year +
                     " Group By B.ExecutiveId";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "Target");
            da.Dispose();

            sSql = "  Select B.ExecutiveId,TMonth,TYear,IncentiveType,FromValue,ToValue,IncValue From dbo.TargetMaster A " +
                      " Inner Join dbo.TargetTrans B On A.TargetId=B.TargetId " +
                      " Inner Join dbo.IncentiveTrans C On C.TargetId=A.TargetId " +
                      " Where TMonth=" + argMonth.Month + " And TYear=" + argMonth.Year;
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "Incentive");
            da.Dispose();

            BsfGlobal.g_CRMDB.Close();

            return ds;

        }

        public static DataSet GetExecDESReport(DateTime argDate)
        {
            DataSet ds = new DataSet();
            BsfGlobal.OpenCRMDB();

            string sSql = "";
           // sSql = "Select * From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans Where sUserId=" + BsfGlobal.g_lUserId + "";
           // SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
           // SqlDataReader dr;
           // dr = cmd.ExecuteReader();
           // DataTable dt = new DataTable();
           // dt.Load(dr);
           // cmd.Dispose();

           // sSql = "Select Distinct C.CostCentreId,D.UserId ExecutiveId,C.CostCentreName,EmployeeName ExecName From  " +
           //" [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users D On D.UserId=D.UserId" +
           //" Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position E on E.PositionId=D.PositionId Where E.PositionType='M' And " +
           //" C.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") And ";
           // if (dt.Rows.Count > 0)
           // {
           //     sSql = sSql + " D.UserId In(Select UserId from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users Where UserId=" + BsfGlobal.g_lUserId + " Or " +
           //     " UserId In (Select UserId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans Where sUserId=" + BsfGlobal.g_lUserId + ")) ";
           // }
           // else
           // { sSql = sSql + " D.UserId = " + BsfGlobal.g_lUserId + ""; }

            sSql = "Select Distinct C.CostCentreId,D.UserId ExecutiveId,C.CostCentreName,Case When D.EmployeeName='' Then D.UserName Else D.EmployeeName End As ExecName From  " +
                   " [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users D On D.UserId=D.UserId" +
                   " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position E on E.PositionId=D.PositionId Where E.PositionType='M'" +
                   " And ProjectDB In(Select ProjectName From  " +
                   " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType IN('B','L')) And C.CostCentreId Not In " +
                   " (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ")";
            SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "Executive");
            da.Dispose();

            sSql = "Select A.ExecutiveId,A.CostCentreId,Count(A.ExecutiveId)EnquiriesAlloted From LeadProjectInfo A " +
                    " INNER JOIN dbo.CallSheet B ON A.LeadId=B.LeadId " +
                    " Where TrnDate='" + argDate.ToString("dd-MMM-yyyy") + "' " +
                    " Group By A.ExecutiveId,A.CostCentreId";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "EnqAlloted");
            da.Dispose();

            sSql = "Select ExecutiveId,ProjectId CostCentreId,Count(ExecutiveId)FollowUps From CallSheet " +
                " Where TrnDate='" + argDate.ToString("dd-MMM-yyyy") + "' Group By ExecutiveId,ProjectId";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "FollowUps");
            da.Dispose();

            sSql = "Select ExecutiveId,ProjectId CostCentreId,Count(ExecutiveId)Warm From CallSheet A " +
                    " Inner Join StatusMaster B On A.StatusID=B.StatusId " +
                    " Where B.Description='Warm' And TrnDate='" + argDate.ToString("dd-MMM-yyyy") + "' "+
                    " Group By ExecutiveId,ProjectId";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "Warm");
            da.Dispose();

            sSql = "Select ExecutiveId,ProjectId CostCentreId,Count(ExecutiveId)Hot From CallSheet A " +
                    " Inner Join StatusMaster B On A.StatusID=B.StatusId " +
                    " Where B.Description='Hot' And TrnDate='" + argDate.ToString("dd-MMM-yyyy") + "' "+
                    " Group By ExecutiveId,ProjectId";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "Hot");
            da.Dispose();

            sSql = "Select ExecutiveId,ProjectId CostCentreId,Count(ExecutiveId)Finalization From CallSheet A " +
                    " Inner Join CallType B On A.CallTypeID=B.CallTypeId " +
                    " Where B.Description='Finalization' And TrnDate='" + argDate.ToString("dd-MMM-yyyy") + "' " +
                    " Group By ExecutiveId,ProjectId";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "Finalization");
            da.Dispose();

            sSql = "Select ExecutiveId,ProjectId CostCentreId,Count(ExecutiveId)[Drop] From CallSheet A " +
                    " Inner Join CallType B On A.CallTypeID=B.CallTypeId " +
                    " Where B.Description='Drop' And TrnDate='" + argDate.ToString("dd-MMM-yyyy") + "' " +
                    " Group By ExecutiveId,ProjectId";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "Drop");
            da.Dispose();

            //sSql = "Select ExecutiveId,ProjectId CostCentreId,0 PendingCalls" +
            //    //" Count(ExecutiveId)PendingCalls "+
            //    " From CallSheet Group By ExecutiveId,ProjectId";
            if (BsfGlobal.FindPermission("Pre-Followup-Allow-Other-Users-Lead") == false)
            {
                sSql = "SELECT E.ExecutiveId,P.CostCentreId,Count(E.ExecutiveId)PendingCalls FROM dbo.CallSheet C " +
                        " INNER JOIN dbo.LeadRegister L  ON C.LeadId=L.LeadId " +
                        " Inner Join dbo.LeadExecutiveInfo E On E.LeadId=L.LeadId " +
                        " INNER JOIN dbo.LeadProjectInfo P ON P.LeadId=E.LeadId " +
                        " WHERE E.ExecutiveId=" + BsfGlobal.g_lUserId + " AND L.NextCallDate=C.NextCallDate " +
                        " AND C.NextCallDate='" + argDate.ToString("dd-MMM-yyyy") + "'" +
                        " And C.EntryId In (Select Max(EntryId)EntryId From dbo.CallSheet C " +
                        " INNER JOIN dbo.LeadRegister L  ON C.LeadId=L.LeadId " +
                        " Inner Join dbo.LeadExecutiveInfo E On E.LeadId=L.LeadId " +
                        " WHERE E.ExecutiveId=" + BsfGlobal.g_lUserId + " And C.CallTypeID!=3 " +
                        " AND L.NextCallDate=C.NextCallDate AND C.NextCallDate='" + argDate.ToString("dd-MMM-yyyy") + "' Group By E.LeadId)" +
                        " Group By E.ExecutiveId,P.CostCentreId ";
            }
            else
            {
                sSql = "SELECT E.ExecutiveId,P.CostCentreId,Count(E.ExecutiveId)PendingCalls FROM dbo.CallSheet C " +
                        " INNER JOIN dbo.LeadRegister L  ON C.LeadId=L.LeadId " +
                        " Inner Join dbo.LeadExecutiveInfo E On E.LeadId=L.LeadId " +
                        " INNER JOIN dbo.LeadProjectInfo P ON P.LeadId=E.LeadId " +
                        " WHERE L.NextCallDate=C.NextCallDate " +
                        " AND C.NextCallDate='" + argDate.ToString("dd-MMM-yyyy") + "'" +
                        " And C.EntryId In (Select Max(EntryId)EntryId From dbo.CallSheet C " +
                        " INNER JOIN dbo.LeadRegister L  ON C.LeadId=L.LeadId " +
                        " Inner Join dbo.LeadExecutiveInfo E On E.LeadId=L.LeadId " +
                        " WHERE E.ExecutiveId<>0 And C.CallTypeID!=3 And " +
                        " L.NextCallDate=C.NextCallDate AND C.NextCallDate='" + argDate.ToString("dd-MMM-yyyy") + "' Group By E.LeadId)" +
                        " Group By E.ExecutiveId,P.CostCentreId ";
            }
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "PendingCalls");
            da.Dispose();

            BsfGlobal.g_CRMDB.Close();

            return ds;

        }

        #region new methods

        public static DataSet GetPerfAnalysis(DateTime argAsOnDate, DateTime argFromDate, DateTime argToDate, string argType)
        {
            DataSet ds = new DataSet();
            string sSql = "";
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            bool bOtherExec = false;
            SqlCommand cmd; SqlDataReader dr;

            try
            {
                string sUser = CommFun.GetOtherExecTrans(conn, tran);

                sSql = "Select * From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans Where sUserId=" + BsfGlobal.g_lUserId + "";

                cmd = new SqlCommand(sSql, conn, tran);
                dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                if (dt.Rows.Count > 0) { bOtherExec = true; } else { bOtherExec = false; }
                dr.Close();

                sSql = "Select A.UserId ExecutiveId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As ExecName From  " +
                       " [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                       " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on B.PositionId=A.PositionId" +
                       " Where B.PositionType='M'";
                if (bOtherExec == false) { sSql = sSql + " Order By EmployeeName"; }
                else
                {
                    sSql = sSql + " AND A.UserId In " +
                     " (" + sUser.TrimEnd(',') + ") Order By EmployeeName";
                }

                cmd = new SqlCommand(sSql, conn, tran);
                dr = cmd.ExecuteReader();
                DataTable dtE = new DataTable();
                dtE.Load(dr);
                dtE.TableName = "Executive";
                ds.Tables.Add(dtE);
                //da.Fill(ds, "Executive");
                //da.Dispose();

                if (argType == "AsOn")
                {
                    sSql = "Select A.ExecutiveId,COUNT(LeadId) CLead From ( " +
                           " Select B.ExecutiveId,A.LeadId From dbo.CallSheet A  " +
                           " Inner Join dbo.LeadExecutiveInfo B On A.LeadId=B.LeadId And A.ExecutiveId=B.ExecutiveId " +
                           " Where TrnDate <= '" + argAsOnDate.ToString("dd-MMM-yyyy") + "'" +
                           " Group by B.ExecutiveId,A.LeadId)A Group By A.ExecutiveId";
                }
                else
                sSql = "Select A.ExecutiveId,COUNT(LeadId) CLead From ( " +
                        " Select B.ExecutiveId,A.LeadId From dbo.CallSheet A  " +
                        " Inner Join dbo.LeadExecutiveInfo B On A.LeadId=B.LeadId And A.ExecutiveId=B.ExecutiveId " +
                        " Where TrnDate Between '" + argFromDate.ToString("dd-MMM-yyyy") + "' And '" + argToDate.ToString("dd-MMM-yyyy") + "'" +
                        " Group by B.ExecutiveId,A.LeadId)A Group By A.ExecutiveId";
                cmd = new SqlCommand(sSql, conn, tran);
                dr = cmd.ExecuteReader();
                DataTable dtL = new DataTable();
                dtL.Load(dr);
                dtL.TableName = "LeadCount";
                ds.Tables.Add(dtL);
                //da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                //da.Fill(ds, "LeadCount");
                //da.Dispose();

                if (argType == "AsOn")
                    sSql = "Select D.ExecutiveId,Count(A.FlatId) CFlats,SUM(A.BaseAmt) Amt From FlatDetails A   " +
                            " Inner Join LeadFlatInfo B on A.FlatId=B.FlatId  " +
                            " Inner Join LeadRegister C on C.LeadId=B.LeadId " +
                            " Inner Join LeadExecutiveInfo D On D.LeadId=C.LeadId " +
                            " Inner Join dbo.BuyerDetail BD On BD.FlatId=A.FlatId " +
                            " Where FinaliseDate<= '" + argAsOnDate.ToString("dd-MMM-yyyy") + "' " +
                            " Group by D.ExecutiveId";
                else
                    sSql = "Select D.ExecutiveId,Count(A.FlatId) CFlats,SUM(A.BaseAmt) Amt From FlatDetails A   " +
                        " Inner Join LeadFlatInfo B on A.FlatId=B.FlatId  " +
                        " Inner Join LeadRegister C on C.LeadId=B.LeadId " +
                        " Inner Join LeadExecutiveInfo D On D.LeadId=C.LeadId " +
                        " Inner Join dbo.BuyerDetail BD On BD.FlatId=A.FlatId " +
                        " Where FinaliseDate Between '" + argFromDate.ToString("dd-MMM-yyyy") + "' And '" + argToDate.ToString("dd-MMM-yyyy") + "' " +
                        " Group by D.ExecutiveId";
                cmd = new SqlCommand(sSql, conn, tran);
                dr = cmd.ExecuteReader();
                DataTable dtF = new DataTable();
                dtF.Load(dr);
                dtF.TableName = "FlatCount";
                ds.Tables.Add(dtF);
                //da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                //da.Fill(ds, "FlatCount");
                //da.Dispose();

            }
            catch (SqlException ex)
            {
                tran.Rollback();
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }

            return ds;

        }

        public static DataSet GetProjectAnalysis(int argExecId, DateTime argAsOnDate, DateTime argFromDate, DateTime argToDate, string argType)
        {
            DataSet ds = new DataSet();
            string sSql = "";
            sSql = "Select Distinct C.CostCentreId,C.CostCentreName,D.UserId ExecutiveId,Case When D.EmployeeName='' Then D.UserName Else D.EmployeeName End As ExecName From  " +
                    " [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users D On D.UserId=D.UserId " +
                    " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position E on E.PositionId=D.PositionId Where E.PositionType='M'" +
                    " And ProjectDB In(Select ProjectName From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType IN('B','L')) " +
                    " And C.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") " +
                    " And D.UserId=" + argExecId + "";
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "Executive");
            da.Dispose();

            if (argType == "AsOn")
                sSql = "Select A.CostCentreId,A.ExecutiveId,COUNT(LeadId) CLead From ( " +
                        " Select C.CostCentreId,B.ExecutiveId,A.LeadId From dbo.CallSheet A " +
                        " Inner Join dbo.LeadExecutiveInfo B On A.LeadId=B.LeadId And A.ExecutiveId=B.ExecutiveId " +
                        " Inner Join dbo.LeadProjectInfo C On C.LeadId=B.LeadId And A.ExecutiveId=C.ExecutiveId " +
                        " Where B.ExecutiveId=" + argExecId + " And TrnDate <='" + argAsOnDate.ToString("dd-MMM-yyyy") + "' " +
                        " Group by C.CostCentreId,B.ExecutiveId,A.LeadId)A Group By A.CostCentreId,A.ExecutiveId";
            else
                sSql = "Select A.CostCentreId,A.ExecutiveId,COUNT(LeadId) CLead From ( " +
                        " Select C.CostCentreId,B.ExecutiveId,A.LeadId From dbo.CallSheet A " +
                        " Inner Join dbo.LeadExecutiveInfo B On A.LeadId=B.LeadId And A.ExecutiveId=B.ExecutiveId " +
                        " Inner Join dbo.LeadProjectInfo C On C.LeadId=B.LeadId And A.ExecutiveId=C.ExecutiveId " +
                        " Where B.ExecutiveId=" + argExecId + " And TrnDate Between '" + argFromDate.ToString("dd-MMM-yyyy") + "' And '" + argToDate.ToString("dd-MMM-yyyy") + "' " +
                        " Group by C.CostCentreId,B.ExecutiveId,A.LeadId)A Group By A.CostCentreId,A.ExecutiveId";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "LeadCount");
            da.Dispose();

            if (argType == "AsOn")
                sSql = "Select A.CostCentreId,D.ExecutiveId,Count(A.FlatId) CFlats,SUM(A.BaseAmt) Amt From dbo.FlatDetails A   " +
                        " Inner Join dbo.LeadFlatInfo B on A.FlatId=B.FlatId Inner Join dbo.LeadRegister C on C.LeadId=B.LeadId " +
                        " Inner Join dbo.LeadExecutiveInfo D On D.LeadId=C.LeadId" +
                        " Inner Join dbo.BuyerDetail BD On BD.FlatId=A.FlatId " +
                        " Where D.ExecutiveId=" + argExecId + " " +
                        " And FinaliseDate <='" + argAsOnDate.ToString("dd-MMM-yyyy") + "' " +
                        " Group by A.CostCentreId,D.ExecutiveId";
            else
                sSql = "Select A.CostCentreId,D.ExecutiveId,Count(A.FlatId) CFlats,SUM(A.BaseAmt) Amt From dbo.FlatDetails A   " +
                        " Inner Join dbo.LeadFlatInfo B on A.FlatId=B.FlatId Inner Join dbo.LeadRegister C on C.LeadId=B.LeadId " +
                        " Inner Join dbo.LeadExecutiveInfo D On D.LeadId=C.LeadId" +
                        " Inner Join dbo.BuyerDetail BD On BD.FlatId=A.FlatId " +
                        " Where D.ExecutiveId=" + argExecId + " " +
                        " And FinaliseDate Between '" + argFromDate.ToString("dd-MMM-yyyy") + "' And '" + argToDate.ToString("dd-MMM-yyyy") + "' " +
                        " Group by A.CostCentreId,D.ExecutiveId";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds, "FlatCount");
            da.Dispose();

            BsfGlobal.g_CRMDB.Close();

            return ds;

        }

        public static DataTable GetSoldUnits(int argExecId, int argCCId, DateTime argAsOnDate, DateTime argFromDate, DateTime argToDate,string argType)
        {
            DataTable ds = new DataTable();
            string sSql = "";
            ////sSql = "Select A.FlatNo,C.LeadName,EmployeeName ExecName,C.LeadType,A.BaseAmt From FlatDetails A Inner Join BuyerDetail B On A.FlatId=B.FlatId" +
            ////        " Inner Join LeadRegister C On A.LeadId=C.LeadId " +
            ////        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users D On D.UserId=B.ExecutiveId" +
            ////        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position E on E.PositionId=D.PositionId Where E.PositionType='M' And" +
            ////        " C.ExecutiveId=" + argExecId + " And A.CostCentreId=" + argCCId + "";
            if (argType == "AsOn")
                sSql = "Select A.FlatNo,C.LeadName,Case When D.EmployeeName='' Then D.UserName Else D.EmployeeName End As ExecName,C.LeadType,A.BaseAmt From dbo.FlatDetails A " +
                        " Inner Join dbo.BuyerDetail B On A.LeadId=B.LeadId " +//A.FlatId=B.FlatId " +
                        " Inner Join dbo.LeadRegister C On A.LeadId=C.LeadId  " +
                        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users D On D.UserId=B.ExecutiveId " +
                        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position E " +
                        " On E.PositionId=D.PositionId Inner Join dbo.LeadExecutiveInfo F On F.LeadId=C.LeadId" +
                        " Where E.PositionType='M' And F.ExecutiveId=" + argExecId + " And A.CostCentreId=" + argCCId + " " +
                        " And FinaliseDate <= '" + argAsOnDate.ToString("dd-MMM-yyyy") + "' ";
            else
                sSql = "Select A.FlatNo,C.LeadName,Case When D.EmployeeName='' Then D.UserName Else D.EmployeeName End As ExecName,C.LeadType,A.BaseAmt From dbo.FlatDetails A " +
                    " Inner Join dbo.BuyerDetail B On A.LeadId=B.LeadId " +//A.FlatId=B.FlatId " +
                    " Inner Join dbo.LeadRegister C On A.LeadId=C.LeadId  " +
                    " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users D On D.UserId=B.ExecutiveId " +
                    " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position E " +
                    " On E.PositionId=D.PositionId Inner Join dbo.LeadExecutiveInfo F On F.LeadId=C.LeadId" +
                    " Where E.PositionType='M' And F.ExecutiveId=" + argExecId + " And A.CostCentreId=" + argCCId + " " +
                    " And FinaliseDate Between '" + argFromDate.ToString("dd-MMM-yyyy") + "' And '" + argToDate.ToString("dd-MMM-yyyy") + "' ";
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            da.Fill(ds);
            da.Dispose();

            BsfGlobal.g_CRMDB.Close();

            return ds;

        }

        internal static bool PowerUserFound()
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dt = null;
            SqlDataAdapter sda;
            bool bAns = false;

            try
            {
                ssql = "Select * From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A  " +
                        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on B.PositionId=A.PositionId And A.PowerUser=1 " +
                        " Where UserId In(Select UserId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans " +
                        " Where UserId=" + BsfGlobal.g_lUserId + ") ";
                sda = new SqlDataAdapter(ssql, BsfGlobal.OpenCRMDB());
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0) bAns = true;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return bAns;
        }

        #endregion


        #endregion
    }
}
