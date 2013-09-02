using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DataLayer;
using CRM.BusinessObjects;

namespace CRM.BusinessLayer
{
    class ComplaintDetBL
    {
          #region Objects
        readonly ComplaintDetDL oComplaintDetDL;
  
        #endregion

        #region Constructor
        public ComplaintDetBL()
        {
            //
            // TODO: Add constructor logic here
            //
            oComplaintDetDL = new ComplaintDetDL();
        }
        #endregion

        #region Methods

        public static DataTable PopulateNature()
        {
            return ComplaintDetDL.PopulateNature();
        }

        public static DataTable PopulateProject(int Id)
        {
            return ComplaintDetDL.PopulateProject(Id);
        }

        public static DataTable PopulateExecutive()
        {
            return ComplaintDetDL.PopulateExecutive();
        }

        public static DataTable PopulateEmployee()
        {
            return ComplaintDetDL.PopulateEmployee();
        }

        public static DataTable PopulateNatureComp()
        {
            return ComplaintDetDL.PopulateNatureComp();
        }

        public static DataTable Fill_ComplaintRegister(int argATRegId)
        {
            return ComplaintDetDL.Fill_ComplaintRegister(argATRegId);
        }

        public static DataTable Populate_ComplaintRegister(DateTime frmDate, DateTime toDate)
        {
            return ComplaintDetDL.Populate_ComplaintRegister(frmDate, toDate);
        }

        public static DataTable Populate_ComplaintRegisterChange(int argEntryId)
        {
            return ComplaintDetDL.Populate_ComplaintRegisterChange(argEntryId);
        }

        public static bool InsertCompDetails( string argAlert)
        {
            return ComplaintDetDL.InsertCompDetails(argAlert);
        }

        public static bool UpdateCompDetails()
        {
            return ComplaintDetDL.UpdateCompDetails();
        }

        public static bool DeleteCompRegister(int RegId, int argCostId, string argVouNo)
        {
            return ComplaintDetDL.DeleteCompRegister(RegId, argCostId, argVouNo);
        }

        public static void DeleteCompMaster(int RegId)
        {
            ComplaintDetDL.DeleteCompMaster(RegId);
        }

        public static DataTable CompliantCheck(int FlatId)
        {
            return ComplaintDetDL.CompliantCheck(FlatId);
        }

        public static void InsertCompliantMaater(string argName)
        {
            ComplaintDetDL.InsertCompliantMaater(argName);
        }
        public static void UpdateCompliantMaater(string argName, int argId)
        {
            ComplaintDetDL.UpdateCompliantMaater(argName, argId);
        }

        #endregion

    }
}
