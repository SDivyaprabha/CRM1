using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace CRM.DataLayer
{
    class ClientDL
    {

        public static DataSet GetProsAndBuyer(int argLCCId, int argBCCId, string argCCId)
        {
            DataSet ds = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();

                sSql = "Select DISTINCT A.LeadId,A.LeadName Name,A.LeadType From dbo.LeadRegister A " +
                        " LEFT JOIN dbo.LeadExecutiveInfo B On A.LeadId=B.LeadId " +
                        " INNER JOIN dbo.LeadProjectInfo P On P.LeadId=A.LeadId " +
                        //" Where P.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") And" +
                        " Where A.LeadId Not In (Select LeadId from dbo.BuyerDetail) ";
                if (argLCCId > 0) { sSql = sSql + " and P.CostCentreId = " + argLCCId; }
                if (argCCId != "") { sSql = sSql + " and P.CostCentreId NOT IN(" + argCCId + ")"; }
                sSql = sSql + " Order by LeadName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                ds = new DataSet();
                sda.Fill(ds);
                sda.Dispose();

                //sSql = "Select A.LeadId,LeadName Name,A.LeadType From dbo.LeadRegister A " +
                //    " Inner Join LeadExecutiveInfo B On A.LeadId=B.LeadId " +
                //       "Where A.LeadId in (Select LeadId from dbo.BuyerDetail ";
                sSql = "Select DISTINCT A.LeadId,A.LeadName Name,D.FlatId,D.FlatNo,A.LeadType,P.CostCentreId From dbo.LeadRegister A  " +
                        " LEFT JOIN LeadExecutiveInfo B On A.LeadId=B.LeadId " +
                        " INNER JOIN dbo.LeadProjectInfo P On P.LeadId=A.LeadId " +
                        " INNER JOIN CallSheet C On A.LeadId=C.LeadId " +
                        " INNER JOIN dbo.FlatDetails D ON A.LeadId=D.LeadId" +
                    //" Where P.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") And" +
                        " Where A.LeadId IN(Select LeadId from dbo.BuyerDetail) " +
                        " GROUP BY A.LeadName,A.LeadId,D.FlatId,D.FlatNo,A.LeadType,P.CostCentreId";
                if (argBCCId > 0)
                {
                    sSql = "Select DISTINCT A.LeadId,A.LeadName Name,D.FlatId,D.FlatNo,A.LeadType,P.CostCentreId From dbo.LeadRegister A  " +
                            " LEFT JOIN LeadExecutiveInfo B On A.LeadId=B.LeadId " +
                            " INNER JOIN dbo.LeadProjectInfo P On P.LeadId=A.LeadId " +
                            " INNER JOIN CallSheet C On A.LeadId=C.LeadId " +
                            " INNER JOIN dbo.FlatDetails D ON A.LeadId=D.LeadId" +
                        //" Where P.CostCentreId Not In (Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") And" +
                            " Where A.LeadId In (Select LeadId from dbo.BuyerDetail Where CostCentreId = " + argBCCId + ")"+
                            " And C.ProjectId= " + argBCCId +
                            " GROUP BY A.LeadName,A.LeadId,D.FlatId,D.FlatNo,A.LeadType,P.CostCentreId";
                }
                //if (argBCCId > 0) { sSql = sSql + " Where B.CostCentreId = " + argBCCId; }
                sSql = sSql + " Order by LeadName";

                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtBuyer = new DataTable();
                sda.Fill(dtBuyer);
                sda.Dispose();

                if (argCCId == "") { ds.Tables.Add(dtBuyer); }
                else
                {
                    DataView dv = new DataView(dtBuyer);
                    dv.RowFilter = "CostCentreId Not In (" + argCCId + ")";
                    ds.Tables.Add(dv.ToTable());
                }
            }
            catch(Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return ds;
        }

        public static DataTable GetProsDetails(int argEnqId)
        {
            DataTable dt = new DataTable();
            string sSql = "";
            SqlDataAdapter sda;
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "Select C.CostCentreName ProjectName,B.EmployeeName ExecutiveName,Convert(Varchar(10),A.TrnDate,103) CallDate," +
                     " Convert(Varchar(10),A.NextCallDate,103) NextCallDate,D.Description Status, " +
                     "E.Description Nature,F.Description CallType,A.Remarks from dbo.CallSheet A  " +
                     "Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users B On A.ExecutiveId=B.UserId " +
                     "Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On A.ProjectId=C.CostCentreId " +
                     "Left Join dbo.StatusMaster D On A.StatusID=D.StatusId " +
                     "Left Join dbo.NatureMaster E On A.NatureID=E.NatureId " +
                     "Left Join dbo.CallType F On A.CallTypeID=F.CallTypeId " +
                     "Where LeadId = " + argEnqId + " ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        public static DataSet GetContactDetails(int argEnqId)
        {
            DataSet ds=null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "Select A.Address1,C.CityName,D.StateName,B.CountryName,A.PinCode,A.LandLine Phone,A.Mobile,A.Email,A.Fax,A.PanNo,A.PassportNo From dbo.LeadCommAddressInfo  A " +
                        "Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Countrymaster B On A.Country =B.CountryId " +
                        "Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Citymaster C On A.CityId=C.CityId " +
                        "Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Statemaster D On A.StateId=D.StateId " +
                        "Where A.LeadId = "+ argEnqId +" ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                ds = new DataSet();
                sda.Fill(ds, "Comm.Address");
                sda.Dispose();

                sSql = "Select A.Address1,C.CityName,D.StateName,B.CountryName,A.PinCode,A.LandLine Phone,A.Mobile,A.Email,A.Fax,A.PanNo,'' PassportNo From dbo.LeadPermanentAddressInfo  A " +
                        "Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Countrymaster B On A.Country =B.CountryId " +
                        "Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Citymaster C On A.CityId=C.CityId " +
                        "Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Statemaster D On A.StateId=D.StateId " +
                        "Where A.LeadId = "+ argEnqId +" ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Per.Address");
                sda.Dispose();

                sSql = "Select A.Address1,C.CityName,D.StateName,B.CountryName,A.PinCode,A.LandLine Phone,A.Mobile,A.Email,A.Fax,A.PanNo,'' PassportNo From dbo.LeadOffAddressInfo  A " +
                        "Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Countrymaster B On A.Country=B.CountryId " +
                        "Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Citymaster C On A.CityId=C.CityId " +
                        "Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Statemaster D On A.StateId=D.StateId " +
                        "Where A.LeadId = " + argEnqId + " ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "OfficeAddress");
                sda.Dispose();

                sSql = "Select A.Address1,C.CityName,D.StateName,B.CountryName,A.PinCode,A.LandLine Phone,A.Mobile,A.Email,A.Fax,A.PanNo,A.PassportNo From dbo.LeadNRIAddressInfo  A " +
                        "Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Countrymaster B On A.Country =B.CountryId " +
                        "Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Citymaster C On A.CityId=C.CityId " +
                        "Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Statemaster D On A.StateId=D.StateId " +
                        "Where A.LeadId = " + argEnqId + " ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "NRIAddress");
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
            return ds;
        }

        public static DataTable GetFlatDetails(int argEnqId, int argFlatId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                //sSql = "Select D.CostCentreName ProjectName,A.FlatNo,C.TypeName FlatType,Convert(Varchar(10),A.RegDate,103) Registration,A.FlatId,A.CostCentreId from dbo.FlatDetails A  " +
                //       "Inner Join dbo.LeadFlatInfo B On A.FlatId=B.FlatId " +
                //       "Left Join dbo.FlatType C On A.FlatTypeId=C.FlatTypeId " +
                //       "Inner Join ["+ BsfGlobal.g_sWorkFlowDBName +"].dbo.OperationalCostCentre D On A.CostCentreId=D.CostCentreId Where B.LeadId="+ argEnqId +" ";
                sSql = "Select D.CostCentreName ProjectName," +
                    " Case When F.FlatNo Is Null Then B.PlotNo Else F.FlatNo End FlatNo," +
                    " Case When T.TypeName Is Null Then C.PlotTypeName Else T.TypeName End FlatType," +
                    " Case When Convert(Varchar(10),F.RegDate,103) Is Null Then Convert(Varchar(10),E.RegDate,103) Else Convert(Varchar(10),F.RegDate,103) End Registration," +
                    " Case When F.FlatId Is Null Then B.PlotDetailsId Else F.FlatId End FlatId, " +
                    " E.CostCentreId,B.LandRegisterId From dbo.LeadFlatInfo A  " +
                    " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotDetails B On A.PlotId=B.PlotDetailsId " +
                    " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.LandPlotType C On B.PlotTypeId=C.PlotTypeId " +
                    " Left Join dbo.FlatDetails F On F.FlatId=A.FlatId" +
                    " Left Join dbo.FlatType T On T.FlatTypeId=F.FlatTypeId" +
                    " Left Join dbo.BuyerDetail E On E.PlotId=B.PlotDetailsId Or E.FlatId=F.FlatId" +
                    " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre D On E.CostCentreId=D.CostCentreId " +
                    " Where E.LeadId=" + argEnqId + " AND F.FlatId=" + argFlatId + " OR B.BuyerId=" + argEnqId +"";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        public static DataTable GetFollowupDetails(int argEnqId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "Select Case When U.EmployeeName='' Then U.UserName Else U.EmployeeName End As ExecName,C.CostCentreName,A.LeadName,Convert(Varchar(10),B.TrnDate,103) Date,F.Description CallType,D.Description Status," +
                       " E.Description Nature,Convert(Varchar(10),B.NextCallDate,103) NextCallDate,B.Remarks from dbo.LeadRegister A " +
                       "Inner Join dbo.CallSheet B On A.LeadId=B.LeadId " +
                       "Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On B.ProjectId=C.CostCentreId " +
                       "Left Join dbo.StatusMaster D On B.StatusID=D.StatusId " +
                       "Left Join dbo.NatureMaster E On B.NatureID=E.NatureId " +
                       "Left Join dbo.CallType F On B.CallTypeID=F.CallTypeId " +
                       " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users U On U.UserId=B.ExecutiveId" +
                       " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position P on P.PositionId=U.PositionId " +
                       "Where A.LeadId IN (Select LeadId from dbo.LeadFlatInfo Where Buyer=1) And A.LeadId=" + argEnqId + " ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        public static DataTable GetPostFollowupDetails(int argEnqId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();

                sSql = "Select Case When U.EmployeeName='' Then U.UserName Else U.EmployeeName End As ExecName,C.CostCentreName,A.LeadName,Convert(Varchar(10),B.BuyerDate,103) Date,F.Description CallType," +
                        " E.Description Nature,Convert(Varchar(10),B.NextCallDate,103) NextCallDate,B.Remarks " +
                        " from dbo.LeadRegister A Inner Join dbo.FollowUp B On A.LeadId=B.LeadId " +
                        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On B.CostCentreId=C.CostCentreId " +
                        " Left Join dbo.NatureMaster E On B.NatureID=E.NatureId " +
                        " Left Join dbo.CallType F On B.CallTypeID=F.CallTypeId " +
                        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users U On U.UserId=B.ExecutiveId" +
                        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position P on P.PositionId=U.PositionId " +
                        " Where A.LeadId " +
                        " IN (Select LeadId from dbo.LeadFlatInfo Where Buyer=1) And A.LeadId=" + argEnqId + " ";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
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

        
    }
}
