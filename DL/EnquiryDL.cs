using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessLayer;

namespace CRM.DataLayer
{
    class EnquiryDL
    {
        #region Methods

        public DataTable GetReligion(SqlConnection Con)
        {
            DataTable dtReligion = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT ReligionId as Id,ReligionName as Description FROM dbo.ReligionMaster WHERE ReligionName <> '' Order By ReligionName", Con);
                dtReligion = new DataTable();
                sda.Fill(dtReligion);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dtReligion;

        }

        public DataTable GetCountry(SqlConnection Con)
        {
            DataTable dtCountry = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT CountryId as Id,CountryName as Description FROM dbo.Countrymaster WHERE CountryName  <> '' Order By CountryName", Con);
                dtCountry = new DataTable();
                sda.Fill(dtCountry);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dtCountry;

        }

        public DataTable GetApartment(SqlConnection Con)
        {
            DataTable dtApartment = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT ApartmentId as Id,Description as Name FROM dbo.Apartment Order By Description", Con);
                dtApartment = new DataTable();
                sda.Fill(dtApartment);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dtApartment;

        }

        public DataSet GetData()
        {
            DataSet ds;
            SqlDataAdapter sd;
            sd = new SqlDataAdapter("FillEnquiry",  BsfGlobal.OpenCRMDB());
            ds = new DataSet();
            sd.Fill(ds);
            return ds;
        }

        public DataTable GetEmployment(SqlConnection Con)
        {
            DataTable dtEmployment = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT EmploymentId as Id,Description as Name FROM dbo.Employment Order By Description", Con);
                dtEmployment = new DataTable();
                sda.Fill(dtEmployment);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dtEmployment;

        }

        public DataTable GetApartmentsize(SqlConnection Con)
        {
            DataTable dtApartSize = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT ApartmentSizeId as Id, str(cast(ApartmentSizeFrom as int),10) + ' -' + str(cast(ApartmentSizeTo as int),10) as ApartmentSize FROM dbo.ApartmentSize", Con);
                dtApartSize = new DataTable();
                sda.Fill(dtApartSize);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dtApartSize;

        }

        public DataTable GetApartmentType(SqlConnection Con)
        {
            DataTable dtApartType = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT ApartmentTypeId as Id,Description as Name FROM dbo.ApartmentType Order By Description", Con);
                dtApartType = new DataTable();
                sda.Fill(dtApartType);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dtApartType;

        }

        public DataTable GetStay(SqlConnection Con)
        {
            DataTable dtStay = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT StayId as Id,Description as Name FROM dbo.Stay Order By Description", Con);
                dtStay = new DataTable();
                sda.Fill(dtStay);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dtStay;

        }

        public DataTable GetGuestHouse(SqlConnection Con)
        {
            DataTable dtGuestHouse = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT GuestHouseId as Id,Description as Name FROM dbo.GuestHouse Order By Description", Con);
                dtGuestHouse = new DataTable();
                sda.Fill(dtGuestHouse);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dtGuestHouse;

        }

        public DataTable GetCostPreference(SqlConnection Con)
        {
            DataTable dtCostPref = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT CostPreferenceId as Id, str(cast(CostPreferenceFrom  as decimal(18,3))) + '   -' + str(cast(CostPreferenceTo as decimal(18,3))) as CostPreference FROM dbo.CostPreference", Con);
                dtCostPref = new DataTable();
                sda.Fill(dtCostPref);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dtCostPref;

        }

        public DataTable GetPosesses(SqlConnection Con)
        {
            DataTable dtPosess = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT PossessId as Id,Description as Name FROM dbo.Possess Order By Description", Con);
                dtPosess = new DataTable();
                sda.Fill(dtPosess);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dtPosess;

        }

        public DataTable GetFacility(SqlConnection Con)
        {
            DataTable dtFacility = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT FacilityId as Id,Description as Name FROM dbo.Facility Order By Description", Con);
                dtFacility = new DataTable();
                sda.Fill(dtFacility);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dtFacility;

        }

        public DataTable GetArea(SqlConnection Con)
        {
            DataTable dtArea = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT AreaId as Id,Description as Name FROM dbo.Area Order By Description", Con);
                dtArea = new DataTable();
                sda.Fill(dtArea);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dtArea;

        }

        public DataTable GetProject(SqlConnection Con)
        {
            DataTable dtProject = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT CostCentreId Id,CostCentreName Name FROM dbo.CostCentre order by CostCentreName", Con);
                dtProject = new DataTable();
                sda.Fill(dtProject);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dtProject;

        }

        public DataTable GetIncome(SqlConnection Con)
        {
            DataTable GetIncome = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT IncomeId Id, str(cast(Incomefrom as float),10) + '   -' + str(cast(IncomeTo as float),12) as Income  FROM dbo.Income", Con);
                GetIncome = new DataTable();
                sda.Fill(GetIncome);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return GetIncome;

        }

        public DataTable GetSource(SqlConnection Con)
        {
            DataTable dtSource = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT SourceID Id,Sourcename Description FROM dbo.EnquirySource ORDER BY SourceName", Con);
                dtSource = new DataTable();
                sda.Fill(dtSource);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dtSource;

        }

        public DataTable GetSubSource(SqlConnection Con, int argSourceId)
        {
            DataTable dtsubSource = null;
            SqlDataAdapter sda;
            string sSql;

            try
            {
                sSql = " SELECT A.SubSourceId ID,A.SubSourceName Description FROM dbo.EnquirySubSource A" +
                       " INNER JOIN dbo.EnquirySource B ON A.SourceId=B.SourceId " +
                       " WHERE B.SubSource='Y' AND B.SourceId='" + argSourceId + "' ORDER BY SubSourceName";

                sda = new SqlDataAdapter(sSql, Con);
                dtsubSource = new DataTable();
                sda.Fill(dtsubSource);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dtsubSource;

        }

        public DataTable GetExecutive(SqlConnection Con)
        {
            DataTable dt=null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("Select UserId ExecId,Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As ExecName,0 Sel From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId Where B.PositionType='M' ORDER BY EmployeeName", Con);
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

        public void Update(EnquiryBL oEnqBL)
        {
            SqlCommand cmd;
            try
            {

                cmd = new SqlCommand("UpdateEnquiry",  BsfGlobal.OpenCRMDB());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                if (oEnqBL.Enquiryid == 0)
                {
                    cmd.Parameters.AddWithValue("@Flag", 1);
                    cmd.Parameters.AddWithValue("@Enquiryid", oEnqBL.Enquiryid);
                    cmd.Parameters["@Enquiryid"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Buyer", oEnqBL.Buyer);
                    cmd.Parameters.AddWithValue("@AccountId", oEnqBL.Accountid);
                    cmd.Parameters.AddWithValue("@BuyerId", oEnqBL.Buyerid);
                    cmd.Parameters.AddWithValue("@CallTypeID", oEnqBL.CallTypeID);
                    cmd.Parameters.AddWithValue("@Projectid", oEnqBL.Projectid);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Flag", 2);
                    cmd.Parameters.AddWithValue("@Enquiryid",oEnqBL.Enquiryid);
                    cmd.Parameters.AddWithValue("@AccountId", oEnqBL.Accountid);
                    cmd.Parameters.AddWithValue("@Buyer", oEnqBL.Buyer);
                    cmd.Parameters.AddWithValue("@BuyerId", oEnqBL.Buyerid);
                    cmd.Parameters.AddWithValue("@CallTypeID", oEnqBL.CallTypeID);
                    cmd.Parameters.AddWithValue("@Projectid", oEnqBL.Projectid);
                }

                cmd.Parameters.AddWithValue("@ProjTypeID", oEnqBL.Projtypeid);
                cmd.Parameters.AddWithValue("@FlatID", oEnqBL.Flatid);

                cmd.Parameters.AddWithValue("@EnqDate", oEnqBL.Enquirydate);
                cmd.Parameters.AddWithValue("@Enquiryname", oEnqBL.Enquiryname);
                cmd.Parameters.AddWithValue("@Profession", oEnqBL.Profession);
                cmd.Parameters.AddWithValue("@Organization", oEnqBL.Organization);
                cmd.Parameters.AddWithValue("@Nationality", oEnqBL.Nationality);
                cmd.Parameters.AddWithValue("@Address1 ", oEnqBL.Address1);
                cmd.Parameters.AddWithValue("@Address2", oEnqBL.Address2);
                cmd.Parameters.AddWithValue("@City", oEnqBL.City);
                cmd.Parameters.AddWithValue("@State", oEnqBL.State);
                cmd.Parameters.AddWithValue("@Pincode", oEnqBL.Pincode);
                cmd.Parameters.AddWithValue("@Phoneoff", oEnqBL.Phoneoff);
                cmd.Parameters.AddWithValue("@Phoneres", oEnqBL.Phoneres);
                cmd.Parameters.AddWithValue("@Mobile", oEnqBL.Mobile);
                cmd.Parameters.AddWithValue("@Fax", oEnqBL.Fax);
                cmd.Parameters.AddWithValue("@Email", oEnqBL.Email);
                cmd.Parameters.AddWithValue("@Web", oEnqBL.Website);
                cmd.Parameters.AddWithValue("@MaritalStatus", oEnqBL.Maritalstatus);


                cmd.Parameters.AddWithValue("@EnquiryModeId", oEnqBL.Enquirymodeid);
                cmd.Parameters.AddWithValue("@EnquirySubModeId", oEnqBL.Enquirysubmodeid);
                
                cmd.Parameters.AddWithValue("@Executiveid", oEnqBL.Executiveid);
                cmd.Parameters.AddWithValue("@Nextfollowupdate", oEnqBL.Nextfollowupdate);
                cmd.Parameters.AddWithValue("@Remarks", oEnqBL.Remarks);
                cmd.Parameters.AddWithValue("@Active", 1);
                cmd.Parameters.AddWithValue("@Additionalinfo", oEnqBL.Additionalinfo);
                cmd.Parameters.AddWithValue("@Employmentid ", oEnqBL.Employmentid);
                cmd.Parameters.AddWithValue("@Apartmentid", oEnqBL.Apartmentid);
                cmd.Parameters.AddWithValue("@Apartmentsizeid", oEnqBL.Apartmentsizeid);
                cmd.Parameters.AddWithValue("@Apartmenttypeid", oEnqBL.Apartmenttypeid);
                cmd.Parameters.AddWithValue("@Costpreferenceid", oEnqBL.Costpreferenceid);
                cmd.Parameters.AddWithValue("@Guesthouseid", oEnqBL.Guesthouseid);
                cmd.Parameters.AddWithValue("@Incomeid", oEnqBL.Incomeid);
                cmd.Parameters.AddWithValue("@Religionid", oEnqBL.Religionid);
                cmd.Parameters.AddWithValue("@StayId", oEnqBL.Stayid);
                
                cmd.Parameters.AddWithValue("@Reasonid", oEnqBL.Reasonid);
                cmd.Parameters.AddWithValue("@Freasonid", oEnqBL.Freasonid);
                cmd.Parameters.AddWithValue("@Brokerage", Convert.ToBoolean(oEnqBL.Brokerage));
                cmd.Parameters.AddWithValue("@BrokerId", oEnqBL.BrokerId);
                cmd.Parameters.AddWithValue("@Commissionper", oEnqBL.Commissionper);


                cmd.Parameters.AddWithValue("@Amount", oEnqBL.Amount);
                cmd.Parameters.AddWithValue("@Birthdate", oEnqBL.Birthdate);
                cmd.Parameters.AddWithValue("@Weddingdate", oEnqBL.Weddingdate);
                cmd.Parameters.AddWithValue("@Country", oEnqBL.Country);
                cmd.Parameters.AddWithValue("@Citizenship", oEnqBL.Nationality);
                cmd.Parameters.AddWithValue("@NoofChild", oEnqBL.Noofchild);
                cmd.Parameters.AddWithValue("@AchievementId", oEnqBL.Achievementid);
                
                cmd.Parameters.AddWithValue("@Coappname", oEnqBL.Coappname);
                cmd.Parameters.AddWithValue("@Coaddress1", oEnqBL.Coaddress1);
                cmd.Parameters.AddWithValue("@Coaddress2", oEnqBL.Coaddress2);
                cmd.Parameters.AddWithValue("@Coappcity", oEnqBL.Coappcity);
                cmd.Parameters.AddWithValue("@CoFatHusname", oEnqBL.Coappfathusname);
                cmd.Parameters.AddWithValue("@Age", oEnqBL.Age);
                cmd.Parameters.AddWithValue("@Coappage", oEnqBL.Coappage);
                cmd.Parameters.AddWithValue("@FatHusName", oEnqBL.Fathusname);
                cmd.Parameters.AddWithValue("@Panno", oEnqBL.Panno);

                cmd.Parameters.AddWithValue("@PAdd1", oEnqBL.Padrs1);
                cmd.Parameters.AddWithValue("@PAdd2", oEnqBL.Padrs2);
                cmd.Parameters.AddWithValue("@Pcity", oEnqBL.Pcity);
                cmd.Parameters.AddWithValue("@PState", oEnqBL.Pstate);
                cmd.Parameters.AddWithValue("@PCountry", oEnqBL.Pcountry);
                cmd.Parameters.AddWithValue("@PpinCode", oEnqBL.Ppincode);
                cmd.Parameters.AddWithValue("@BankName", oEnqBL.Bankname);
                cmd.Parameters.AddWithValue("@LoanDetails", oEnqBL.Loandetails);
                cmd.Parameters.AddWithValue("@Disqual", oEnqBL.Disqual);
                cmd.Parameters.AddWithValue("@No_of_emp", oEnqBL.No_of_emp);
                cmd.Parameters.AddWithValue("@AnnualIncome", oEnqBL.Annual_revenue);
                cmd.Parameters.AddWithValue("@Industry", oEnqBL.Industry);
                cmd.Parameters.AddWithValue("@Description", oEnqBL.Description);
                cmd.Parameters.AddWithValue("@Created_UserId", oEnqBL.Created_UserId);
                cmd.Parameters.AddWithValue("@Modified_UserId", oEnqBL.Modified_UserId);
                cmd.Parameters.AddWithValue("@Modified_Date", DBNull.Value);
                cmd.Parameters.AddWithValue("@EnqType", oEnqBL.LeadType);
                cmd.Parameters.AddWithValue("@Prev", oEnqBL.PrevCust);

                cmd.ExecuteNonQuery();
                //CommFun.InsertLog(DateTime.Now, "Enquiry Details-Edit", "E", "Edit ENQUIRY Details", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                BsfGlobal.InsertLog(DateTime.Now, "Enquiry Details-Edit", "E", "Edit ENQUIRY Details", oEnqBL.Enquiryid, oEnqBL.Projectid, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);

                oEnqBL.Enquiryid = (int)cmd.Parameters["@Enquiryid"].Value;

                if (oEnqBL.ChkPos.Items.Count > 0)
                {
                    string sSql;
                    sSql = "DELETE FROM dbo.PossessDetail WHERE EnquiryId='" + oEnqBL.Enquiryid + "'";
                    cmd = new SqlCommand(sSql,  BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("UpdatePos",  BsfGlobal.OpenCRMDB());
                    cmd.CommandType = CommandType.StoredProcedure;

                    for (int i = 0; i < oEnqBL.ChkPos.CheckedItems.Count; i++)
                    {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@PossessId", Convert.ToInt32(((System.Data.DataRowView)(oEnqBL.ChkPos.Items[i])).Row.ItemArray[0]));
                            cmd.Parameters.AddWithValue("@EnqId", oEnqBL.Enquiryid);
                            cmd.Parameters.AddWithValue("@SortOrder", i+1);
                            cmd.ExecuteNonQuery();
                        
                    }
                }

                if (oEnqBL.ChkArea.Items.Count > 0)
                {
                    string sSql;
                    sSql = "DELETE FROM dbo.AreaDetail WHERE EnquiryId='" + oEnqBL.Enquiryid + "'";
                    cmd = new SqlCommand(sSql,  BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("UpdateArea",  BsfGlobal.OpenCRMDB());
                    cmd.CommandType = CommandType.StoredProcedure;

                    for (int i = 0; i < oEnqBL.ChkArea.CheckedItems.Count; i++)
                    {

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@AreaId", Convert.ToInt32(((System.Data.DataRowView)(oEnqBL.ChkArea.Items[i])).Row.ItemArray[0]));
                        cmd.Parameters.AddWithValue("@EnqId", oEnqBL.Enquiryid);
                        cmd.Parameters.AddWithValue("@SortOrder", i + 1);
                        cmd.ExecuteNonQuery();


                    }
                }
                if (oEnqBL.ChkFacility.Items.Count > 0)
                {
                    string sSql;
                    sSql = "DELETE FROM dbo.FacilityDetail WHERE EnquiryId='" + oEnqBL.Enquiryid + "'";
                    cmd = new SqlCommand(sSql,  BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("UpdateFacility",  BsfGlobal.OpenCRMDB());
                    cmd.CommandType = CommandType.StoredProcedure;

                    for (int i = 0; i < oEnqBL.ChkFacility.CheckedItems.Count; i++)
                    {

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@FacilityId", Convert.ToInt32(((System.Data.DataRowView)(oEnqBL.ChkFacility.Items[i])).Row.ItemArray[0]));
                        cmd.Parameters.AddWithValue("@EnqId", oEnqBL.Enquiryid);
                        cmd.Parameters.AddWithValue("@SortOrder", i + 1);
                        cmd.ExecuteNonQuery();


                    }
                }


            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }

        }

        #endregion
    }
}

