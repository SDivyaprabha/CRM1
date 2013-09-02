using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DataLayer;

namespace CRM.BusinessLayer
{
    class CompetitorBL
    {
        CRM.DataLayer.CompetitorDL m_oComp;

        public CompetitorBL()
        {
            m_oComp = new DataLayer.CompetitorDL();
        }

        public DataTable GetCompetitorDetails(int argCCID, string argCCName)
        {
            return m_oComp.GetCompetitorDetails(argCCID,argCCName);
        }
        public DataSet GetAmenities(int argCCID)
        {
            return m_oComp.GetAmenities(argCCID);
        }
        public DataSet GetService(int argCCID)
        {
            return m_oComp.GetService(argCCID);
        }
        public static DataTable GetTemplate(int argProjId)
        {
            return CompetitorDL.GetTemplate(argProjId);
        }
        public static bool TemplateFound(string argName, int argProjId)
        {
            return CompetitorDL.TemplateFound(argName, argProjId);
        }
        public static int InsertTempName(string argTempName, int argProjId)
        {
            return CompetitorDL.InsertTempName(argTempName, argProjId);
        }
        public static void UpdateTemplate(int argTempId, string argTempName)
        {
            CompetitorDL.UpdateTemplate(argTempId, argTempName);
        }
        public static byte[] GetDocTemp(int argTempId)
        {
            return CompetitorDL.GetDocTemp(argTempId);
        }
        public static void DeleteTempate(int argId)
        {
            CompetitorDL.DeleteTempate(argId);
        }
        public static void TemplateAttach(int argTemId, byte[] argImageData, System.IO.FileStream fileMode,string argExt)
        {
            CompetitorDL.TemplateAttach(argTemId, argImageData, fileMode,argExt);
        }
        public static void RemoveTempate(int argId)
        {
            CompetitorDL.RemoveTempate(argId);
        }

        public static DataSet GetReport(int argCCId,int argFlatTypeId)
        {
            return CompetitorDL.GetReport(argCCId,argFlatTypeId);
        }

        #region CompetitiveMaster

        public static DataTable GetCompMaster()
        {
            return CompetitorDL.GetCompMaster();
        }

        public static void InsertCompetitorMaster(string argDesc)
        {
            CompetitorDL.InsertCompetitorMaster(argDesc);
        }

        public static void UpdateCompetitorMaster(int argCompId, string argCompName)
        {
            CompetitorDL.UpdateCompetitorMaster(argCompId, argCompName);
        }

        public static void DeleteCompetitorMaster(int argCompId)
        {
            CompetitorDL.DeleteCompetitorMaster(argCompId);
        }

        public static bool CheckCompMasterUsed(int argCompId)
        {
            return CompetitorDL.CheckCompMasterUsed(argCompId);
        }

        #endregion

        #region FlatTypeMaster

        public static DataTable GetFlatTypeReport(int argCCId)
        {
            return CompetitorDL.GetFlatTypeReport(argCCId);
        }

        public static DataTable GetFlatTypeMaster()
        {
            return CompetitorDL.GetFlatTypeMaster();
        }

        public static void InsertFlatTypeMaster(string argDesc)
        {
            CompetitorDL.InsertFlatTypeMaster(argDesc);
        }

        public static void UpdateFlatTypeMaster(int argCompId, string argCompName)
        {
            CompetitorDL.UpdateFlatTypeMaster(argCompId, argCompName);
        }

        public static void DeleteFlatTypeMaster(int argCompId)
        {
            CompetitorDL.DeleteFlatTypeMaster(argCompId);
        }

        public static bool CheckFlatTypeMasterUsed(int argCompId)
        {
            return CompetitorDL.CheckFlatTypeMasterUsed(argCompId);
        }

        public static DataTable GetFlatType(string argFlatTypeId)
        {
            return CompetitorDL.GetFlatType(argFlatTypeId);
        }

        public static DataTable GetFlatTypeComp(int argProjectId)
        {
            return CompetitorDL.GetFlatTypeComp(argProjectId);
        }

        public static void InsertFlatTypeComp(DataTable dtFlatTypetrans, int argProjectId)
        {
            CompetitorDL.InsertFlatTypeComp(dtFlatTypetrans,argProjectId);
        }

        public static DataTable Location()
        {
            return CompetitorDL.Location();
        }

        public static bool CheckFlatTypeUsed(int argProjectId)
        {
            return CompetitorDL.CheckFlatTypeUsed(argProjectId);
        }

        #endregion

    }
}
