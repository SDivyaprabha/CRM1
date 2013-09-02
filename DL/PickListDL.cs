using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CRM.DataLayer
{
    class PickListDL
    {

        #region Methods

        public DataTable GetCountry(SqlConnection Con)
        {
            DataTable dtCountry = null;
            SqlDataAdapter sda;
            try
            {
                sda = new SqlDataAdapter("SELECT CountryId as Id,CountryName as Description FROM dbo.Countrymaster WHERE CountryName  <> '' Order By CountryName", Con);
                dtCountry = new DataTable();
                sda.Fill(dtCountry);
                dtCountry.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return dtCountry;

        }

        public DataTable GetEmployment(SqlConnection Con)
        {
            DataTable dtEmployment = null;
            SqlDataAdapter sda;
            try
            {
                sda = new SqlDataAdapter("SELECT EmploymentId as Id,Description FROM dbo.Employment Order By Description", Con);
                dtEmployment = new DataTable();
                sda.Fill(dtEmployment);
                dtEmployment.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return dtEmployment;

        }
        
        public DataTable GetReligion(SqlConnection Con)
        {
            DataTable dtReligion = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT ReligionId as Id,ReligionName as Description FROM dbo.ReligionMaster WHERE ReligionName <> '' Order By ReligionName", Con);
                dtReligion = new DataTable();
                sda.Fill(dtReligion);
                dtReligion.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return dtReligion;

        }

        public DataTable GetApartment(SqlConnection Con)
        {
            DataTable dtApartment = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT ApartmentId as Id,Description FROM dbo.Apartment Order By Description", Con);
                dtApartment = new DataTable();
                sda.Fill(dtApartment);
                dtApartment.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return dtApartment;

        }

        public DataTable GetApartmentType(SqlConnection Con)
        {
            DataTable dtApartmentType = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT ApartmentTypeId as Id,Description FROM dbo.ApartmentType Order By Description", Con);
                dtApartmentType = new DataTable();
                sda.Fill(dtApartmentType);
                dtApartmentType.Dispose();
            }

            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return dtApartmentType;

        }

        public DataTable GetStay(SqlConnection Con)
        {
            DataTable dtStay = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT StayId as Id,Description FROM dbo.Stay Order By Description", Con);
                dtStay = new DataTable();
                sda.Fill(dtStay);
                dtStay.Dispose();
            }

            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return dtStay;

        }

        public DataTable GetGuestHouse(SqlConnection Con)
        {
            DataTable dtguestHouse = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT GuestHouseId as Id,Description FROM dbo.GuestHouse Order By Description", Con);
                dtguestHouse = new DataTable();
                sda.Fill(dtguestHouse);
                dtguestHouse.Dispose();
            }

            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return dtguestHouse;

        }

        public DataTable GetPosesses(SqlConnection Con)
        {
            DataTable dtPosesses = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT PossessId as Id,Description FROM dbo.PossessMaster Order By Description", Con);
                dtPosesses = new DataTable();
                sda.Fill(dtPosesses);
                dtPosesses.Dispose();
            }

            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return dtPosesses;

        }

        public DataTable GetFacility(SqlConnection Con)
        {
            DataTable dtFacility = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT FacilityId as Id,Description FROM dbo.FacilityMaster Order By Description", Con);
                dtFacility = new DataTable();
                sda.Fill(dtFacility);
                dtFacility.Dispose();
            }

            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return dtFacility;

        }

        public DataTable GetArea(SqlConnection Con)
        {
            DataTable dtArea = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT AreaId as Id,Description FROM dbo.LeadAreaMaster Order By Description", Con);
                dtArea = new DataTable();
                sda.Fill(dtArea);
                dtArea.Dispose();
            }

            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return dtArea;

        }

        public DataTable GetAcheivement(SqlConnection Con)
        {
            DataTable dtAcheivement = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT AchievementId as [Id],Description as Description FROM dbo.Achievement Order By [Name]", Con);
                dtAcheivement = new DataTable();
                sda.Fill(dtAcheivement);
                dtAcheivement.Dispose();
            }

            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return dtAcheivement;

        }

        public DataTable GetCostCentre()
        {
            DataTable dtCC = null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT CostCentreId as [Id],CostCentreName as Description,Cast(0 as Bit) as Sel FROM ["+ BsfGlobal.g_sWorkFlowDBName +"].dbo.OperationalCostCentre Order By CostCentreName", BsfGlobal.OpenCRMDB());
                dtCC= new DataTable();
                sda.Fill(dtCC);
                dtCC.AcceptChanges();
                dtCC.Dispose();
            }

            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return dtCC;

        }

        #endregion
        
    }
}
