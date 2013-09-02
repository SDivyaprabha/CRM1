using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace CRM.DataLayer
{
    class AvailabilityDL
    {
        public DataTable GetFlatDetails(int argCCId,string argType,int argLandId,string argBlockId)
        {
            DataTable dt = new DataTable();

            BsfGlobal.OpenCRMDB();
            string sSql = "", sCond = "";
            try
            {
                if (argBlockId == "") { sCond = ""; } else { sCond = "And A.BlockId In(" + argBlockId + ")"; }
                if (argType == "L")
                {
                    sSql = "Select A.PlotDetailsId FlatId,A.PlotNo FlatNo,B.PlotTypeName,A.Area, " +
                           "(Case A.Status When 'S' Then 'Sold'  When 'B' Then 'Block' When 'R' Then 'Reserve' " +
                           "When 'U' Then 'UnSold' End) Status from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A  " +
                           "Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotType B on A.PlotTypeId=B.PlotTypeId " +
                           "Where A.LandRegisterId= " + argLandId;
                }
                else
                {
                    sSql = "Select A.FlatId,A.FlatNo,B.BlockName Block,C.LevelName Level,D.TypeName,A.Area," +
                            " (Case A.Status When 'S' Then 'Sold'  When 'B' Then 'Block' When 'R' Then 'Reserve'" +
                            " When 'U' Then 'UnSold' End) Status,E.Description Facing from dbo.FlatDetails A  " +
                            " Left Join dbo.BlockMaster B on A.BlockId=B.BlockId  " +
                            " Left Join dbo.LevelMaster C on A.LevelId=C.LevelId  " +
                            " Left Join dbo.FlatType D on A.FlatTypeId=D.FlatTypeId " +
                            " Left Join dbo.Facing E On A.FacingId=E.FacingId And E.CostCentreId=" + argCCId + "" +
                            " Where A.CostCentreId= " + argCCId + " " + sCond + " ORDER BY B.SortOrder,C.SortOrder,A.SortOrder,dbo.Val(A.FlatNo)";
                }
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        public DataTable GetCostCentreList()
        {
            DataTable dt = new DataTable();
            BsfGlobal.OpenWorkFlowDB(); string sSql = "";
            try
            {
                sSql = "Select CostCentreId,CostCentreName,B.BusinessType,B.LandId from OperationalCostCentre A " +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister B on A.ProjectDB=B.ProjectName " +
                        " Where B.BusinessType in('B','L') And CostCentreId Not In (Select CostCentreId From dbo.UserCostCentreTrans  Where UserId=" + BsfGlobal.g_lUserId + ")" +
                        " Order By CostCentreName ";
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                BsfGlobal.g_WorkFlowDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public DataTable GetBlockList(int argCCId)
        {
            DataTable dt = new DataTable();
            BsfGlobal.OpenCRMDB(); 
            string sSql = "";
            try
            {
                sSql = "Select BlockId,BlockName From dbo.BlockMaster Where CostCentreId=" + argCCId + " Order By SortOrder";
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        public static DataTable GetFlatVDetails(int argCCId,int argFlatId,string argType,int argLandId)
        {
            DataTable dt = new DataTable();
            try
            {
                BsfGlobal.OpenCRMDB(); decimal dQualAmt = 0;

                string sSql = "";
                if (argType == "L")
                {
                    sSql = "Select Sum(Case When Add_Less_Flag='-' Then A.Amount*-1 Else A.Amount End) Amount" +
                            " from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptQualifier A Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotReceiptType B On A.SchId=B.SchId" +
                            " Where B.PlotDetailsId=" + argFlatId + "";
                    SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dtQ = new DataTable();
                    dtQ.Load(dr);
                    if (dtQ.Rows.Count > 0) { dQualAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }

                    sSql = "Select A.PlotNo,B.PlotTypeName,F.LeadName BuyerName,A.SurveyNo,A.PattaNo,A.PattaName,A.Area,A.Rate,A.BaseAmount," +
                            " A.AdvanceAmount,A.GuideLine,A.OtherCost,(" + dQualAmt + "+A.BaseAmount+A.OtherCost) NetAmount," +
                            " (Case A.Status When 'S' Then 'Sold'  When 'B' Then 'Block' When 'R' Then 'Reserve' " +
                            " When 'U' Then 'UnSold' End) Status,North,South,East,West from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails A  " +
                            " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotType B on A.PlotTypeId=B.PlotTypeId " +
                            " Left Join dbo.BuyerDetail E On A.BuyerId=E.LeadId And A.PlotDetailsId=E.PlotId" +
                            " Left Join dbo.LeadRegister F On E.LeadId=F.LeadId " +
                            " Where A.LandRegisterId= " + argLandId + " And A.PlotDetailsId=" + argFlatId + "";
                }

                else
                {

                    sSql = "Select A.FlatNo " + CommFun.m_sFuncName + "No,B.BlockName Block,C.LevelName Level,D.TypeName," +
                        " (Case A.Status When 'S' Then 'Sold'  When 'B' Then 'Block' When 'R' Then 'Reserve' When 'U' Then 'UnSold' End) Status, " +
                        " G.Description Facing, " +
                        " F.LeadName BuyerName,Case E.CustomerType When 'I' Then 'Investor' When 'B' Then 'Buyer' Else ''" +
                        " End CustomerType,A.Area,A.Rate,A.BaseAmt,A.AdvAmount,A.USLand UDSLandArea,A.USLandAmt LandAmount, " +
                        " A.TotalCarPark CarPark,A.OtherCostAmt,A.NetAmt  from dbo.FlatDetails A " +
                        " Left Join dbo.BlockMaster B on A.BlockId=B.BlockId " +
                        " Left Join dbo.LevelMaster C on A.LevelId=C.LevelId " +
                        " Left Join dbo.FlatType D on A.FlatTypeId=D.FlatTypeId " +
                        " Left Join dbo.BuyerDetail E On A.LeadId=E.LeadId " +
                        " Left Join dbo.LeadRegister F On E.LeadId=F.LeadId " +
                        " Left Join dbo.Facing G On A.FacingId=G.FacingId And G.CostCentreId= " + argCCId + "" +
                        " Where A.CostCentreId= " + argCCId + " And A.FlatId=" + argFlatId + " ";
                }
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        public static DataTable GetFlatTrans(int argFlatId, string argType)
        {
            SqlDataAdapter sda;
            string sSql = "";
            DataTable dt = new DataTable();
            dt.Columns.Add("Step", typeof(Int32));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Status", typeof(Int32));


            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "Select ROW_NUMBER() OVER (ORDER BY B.SortOrder) As Step,A.CheckListId, B.CheckListName Description, " +
                       "Case When ISNULL(C.Status,0) <> 0 then ROW_NUMBER() OVER (ORDER BY B.SortOrder) else  null end Status from dbo.FlatTypeChecklist A " +
                       "Inner Join dbo.CheckListMaster B On A.CheckListId=B.CheckListId And A.Status=1" +
                       "Left Join dbo.FlatChecklist C On A.CheckListId=C.CheckListId And C.FlatId=" + argFlatId + " " +
                       "Where A.FlatTypeId IN (Select FlatTypeId From dbo.FlatDetails Where FlatId=" + argFlatId + ") and B.TypeName='" + argType + "' " +
                       "Order by B.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtT = new DataTable();
                sda.Fill(dtT);
                sda.Dispose();


                //sSql = "Select CheckListId,ExpCompletionDate,CompletionDate from FlatChecklist Where FlatId=" + argFlatId;
                sSql = " Select A.CheckListId,A.ExpCompletionDate,C.CompletionDate From FlatTypeChecklist A" +
                        " Inner Join CheckListMaster B On A.CheckListId=B.CheckListId And A.Status=1" +
                        " Left Join FlatChecklist C On A.CheckListId=C.CheckListId And C.FlatId=" + argFlatId + ""+
                        " Where A.FlatTypeId " +
                        " IN (Select FlatTypeId From FlatDetails Where FlatId=" + argFlatId + ") and B.TypeName='" + argType + "'"+
                        " Order by B.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtC = new DataTable();
                sda.Fill(dtC);
                sda.Dispose();

                DataView dv;
                DataRow dRow;

                int iCheckListId = 0;
                int iStatus = 0;
                string sDes = "";
                int iStep = 0;

                for (int i = 0; i < dtT.Rows.Count; i++)
                {
                    iCheckListId = Convert.ToInt32(dtT.Rows[i]["CheckListId"]);
                    iStep = Convert.ToInt32(dtT.Rows[i]["Step"]);
                    iStatus = Convert.ToInt32(CommFun.IsNullCheck(dtT.Rows[i]["Status"], CommFun.datatypes.vartypenumeric));
                    sDes = dtT.Rows[i]["Description"].ToString();

                    dv = new DataView(dtC);
                    dv.RowFilter = "CheckListId = " + iCheckListId;


                    if (dv.ToTable().Rows.Count > 0)
                    {
                        if (iStatus == 0)
                        {
                            sDes = sDes + " (" + Convert.ToDateTime(CommFun.IsNullCheck(dv.ToTable().Rows[0]["ExpCompletionDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MM-yyyy") + ")";
                        }
                        else
                        {
                            sDes = sDes + " (" + Convert.ToDateTime(CommFun.IsNullCheck(dv.ToTable().Rows[0]["CompletionDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MM-yyyy") + ")";
                        }
                    }
                    dv.Dispose();

                    dRow = dt.NewRow();
                    dRow["Step"] = iStep;
                    dRow["Description"] = sDes;
                    dRow["Status"] = dtT.Rows[i]["Status"];

                    dt.Rows.Add(dRow);

                }

                dtT.Dispose();

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

        public static DataTable GetPlotTrans(int argFlatId, string argType)
        {
            SqlDataAdapter sda;
            string sSql = "";
            DataTable dt = new DataTable();
            dt.Columns.Add("Step", typeof(Int32));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Status", typeof(Int32));

            DataTable dtT = new DataTable();
            dtT.Columns.Add("Step", typeof(Int32));
            dtT.Columns.Add("CheckListId", typeof(Int32));
            dtT.Columns.Add("Description", typeof(string));
            dtT.Columns.Add("Status",typeof(Int32));

            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "Select A.CheckListId From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotDetailsCheckList A" +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.CheckListMaster B On A.CheckListId=B.CheckListId" +
                        " Where A.PlotDetailsId=" + argFlatId + " And B.Type='" + argType + "' Order by B.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtS = new DataTable();
                sda.Fill(dtS);
                sda.Dispose();
                
                sSql = "Select ROW_NUMBER() OVER (ORDER BY B.SortOrder) As Step,A.CheckListId, B.CheckListName Description" +
                        " From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotTypeCheckList A Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.CheckListMaster B On A.CheckListId=B.CheckListId " +
                        " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotDetailsCheckList C On A.CheckListId=C.CheckListId And C.PlotDetailsId=" + argFlatId + " Where A.PlotTypeId " +
                        " IN (Select PlotTypeId From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails Where PlotDetailsId=" + argFlatId + ") and B.Type='" + argType + "' Order by B.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtSt = new DataTable();
                sda.Fill(dtSt);
                sda.Dispose();

                if (dtSt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSt.Rows.Count; i++)
                    {
                        DataRow dr;
                        dr = dtT.NewRow();
                        dr["Step"] = dtSt.Rows[i]["Step"];
                        dr["CheckListId"] = dtSt.Rows[i]["CheckListId"];
                        dr["Description"] = dtSt.Rows[i]["Description"];
                        dr["Status"] = DBNull.Value;
                        dtT.Rows.Add(dr);
                    }
                }
                
                if (dtS.Rows.Count > 0)
                {
                    for (int i = 0; i < dtS.Rows.Count; i++)
                    {
                        if (dtT.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtT.Rows.Count; j++)
                            {
                                if(Convert.ToInt32(dtT.Rows[j]["CheckListId"])==Convert.ToInt32(dtS.Rows[i]["CheckListId"]))
                                dtT.Rows[j]["Status"] = dtT.Rows[j]["Step"];
                                //else dtT.Rows[j]["Status"] = DBNull.Value;
                            }
                        }
                        //else
                        //{
                        //    dtT.Rows[i]["Status"] = DBNull.Value;
                        //}
                    }
                }

                sSql = " Select A.CheckListId,A.ExpCompletionDate,C.CompletionDate From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotTypeCheckList A " +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.CheckListMaster B On A.CheckListId=B.CheckListId  " +
                        " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.PlotDetailsCheckList C On A.CheckListId=C.CheckListId And C.PlotDetailsId=" + argFlatId + " Where A.PlotTypeId  " +
                        " IN (Select PlotTypeId From [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails Where PlotDetailsId=" + argFlatId + ") and B.Type='" + argType + "' Order by B.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtC = new DataTable();
                sda.Fill(dtC);
                sda.Dispose();

                DataView dv;
                DataRow dRow;

                int iCheckListId = 0;
                int iStatus = 0;
                string sDes = "";
                int iStep = 0;

                for (int i = 0; i < dtT.Rows.Count; i++)
                {
                    iCheckListId = Convert.ToInt32(dtT.Rows[i]["CheckListId"]);
                    iStep = Convert.ToInt32(dtT.Rows[i]["Step"]);
                    iStatus = Convert.ToInt32(CommFun.IsNullCheck(dtT.Rows[i]["Status"], CommFun.datatypes.vartypenumeric));
                    sDes = dtT.Rows[i]["Description"].ToString();

                    dv = new DataView(dtC);
                    dv.RowFilter = "CheckListId = " + iCheckListId;


                    if (dv.ToTable().Rows.Count > 0)
                    {
                        if (iStatus == 0)
                        {
                            sDes = sDes + " (" + Convert.ToDateTime(CommFun.IsNullCheck(dv.ToTable().Rows[0]["ExpCompletionDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MM-yyyy") + ")";
                        }
                        else
                        {
                            sDes = sDes + " (" + Convert.ToDateTime(CommFun.IsNullCheck(dv.ToTable().Rows[0]["CompletionDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MM-yyyy") + ")";
                        }
                    }
                    dv.Dispose();

                    dRow = dt.NewRow();
                    dRow["Step"] = iStep;
                    dRow["Description"] = sDes;
                    dRow["Status"] = dtT.Rows[i]["Status"];

                    dt.Rows.Add(dRow);

                }

                dtT.Dispose();

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

        public static DataTable GetProjectTrans(int argCCId, string argType)
        {
            SqlDataAdapter sda;
            string sSql = "";
            DataTable dt = new DataTable();
            dt.Columns.Add("Step", typeof(Int32));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Status", typeof(Int32));


            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "Select ROW_NUMBER() OVER (ORDER BY A.SortOrder) As Step,A.CheckListId, A.CheckListName Description," +
                        " Case When ISNULL(B.Status,0) <> 0 then ROW_NUMBER() OVER (ORDER BY A.SortOrder) else  null end Status from dbo.CheckListMaster A " +
                        " Left Join dbo.CCCheckListTrans B On A.CheckListId=B.CheckListId " +
                        " And B.CostCentreId=" + argCCId + " " +
                        " Where A.TypeName='P' Order by A.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtT = new DataTable();
                sda.Fill(dtT);
                sda.Dispose();

                sSql = "  Select A.CheckListId,B.CompletionDate From CheckListMaster A   " +
                        " Left Join dbo.CCCheckListTrans B On A.CheckListId=B.CheckListId And B.CostCentreId=" + argCCId + "" +
                        " Where A.TypeName='P' Order by A.SortOrder";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtC = new DataTable();
                sda.Fill(dtC);
                sda.Dispose();

                DataView dv;
                DataRow dRow;

                int iCheckListId = 0;
                int iStatus = 0;
                string sDes = "";
                int iStep = 0;

                for (int i = 0; i < dtT.Rows.Count; i++)
                {
                    iCheckListId = Convert.ToInt32(dtT.Rows[i]["CheckListId"]);
                    iStep = Convert.ToInt32(dtT.Rows[i]["Step"]);
                    iStatus = Convert.ToInt32(CommFun.IsNullCheck(dtT.Rows[i]["Status"], CommFun.datatypes.vartypenumeric));
                    sDes = dtT.Rows[i]["Description"].ToString();

                    dv = new DataView(dtC);
                    dv.RowFilter = "CheckListId = " + iCheckListId;


                    if (dv.ToTable().Rows.Count > 0)
                    {
                       sDes = sDes + " (" + Convert.ToDateTime(CommFun.IsNullCheck(dv.ToTable().Rows[0]["CompletionDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MM-yyyy") + ")";
                    }
                    dv.Dispose();

                    dRow = dt.NewRow();
                    dRow["Step"] = iStep;
                    dRow["Description"] = sDes;
                    dRow["Status"] = dtT.Rows[i]["Status"];

                    dt.Rows.Add(dRow);

                }

                dtT.Dispose();

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

    }
}
