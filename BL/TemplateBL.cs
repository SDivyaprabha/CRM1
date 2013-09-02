using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DataLayer;
using CRM.BusinessObjects;

namespace CRM.BusinessLayer
{
    class TemplateBL
    {
        #region Objects
        TemplateDL oTempDL;

        #endregion

        #region Constructor

        public TemplateBL()
        {
            oTempDL = new TemplateDL();
        }
        #endregion

        #region Methods
        public DataTable GetTemplate(string argType)
        {
            return oTempDL.GetTemplate(argType);
        }
        public int InsertTempname(string argTempName, string argTempType)
        {
            return oTempDL.InsertTempname(argTempName,argTempType);
        }
        public void TemplateAttach(int argTemId, byte[] argImageData, System.IO.FileStream fileMode)
        {
            oTempDL.TemplateAttach(argTemId,  argImageData, fileMode);
        }
        public void DeleteTempate(int argId)
        {
            oTempDL.DeleteTempate(argId);
        }
        public void RemoveTempate(int argId)
        {
            oTempDL.RemoveTempate(argId);
        }

        public byte[] GetDocTemp(int argTempId)
        {
            return oTempDL.GetDocTemp(argTempId);
        }

        //public DataTable GetTemplateList()
        //{
        //    return oTempDL.GetTemplateList();
        //}
        public DataTable GetFlatDetails(int argFlatId)
        {
            return oTempDL.GetFlatDetails(argFlatId);
        }
        public void InsertTempTable(DataTable argdtCaption)
        {
            oTempDL.InsertTempTable(argdtCaption);
        }
        public DataTable GetTempMegreList()
        {
            return oTempDL.GetTempMegreList();
        }

        public void UpdateTemplate(int argTempId, string argTempName)
        {
            oTempDL.UpdateTemplate(argTempId, argTempName);
        }

        public bool CheckTemplateUsed(int argTempId)
        {
           return  oTempDL.CheckTemplateUsed(argTempId);
        }

        #endregion

        #region CostCentre/Flat Wise Methods

        public DataTable GetCCTemplate(string frmWhere,int argCCId,int argFlatId)
        {
            return oTempDL.GetCCTemplate(frmWhere,argCCId,argFlatId);
        }

        public void InsertCCTempname(string frmWhere, int argTempID)
        {
            oTempDL.InsertCCTempname(frmWhere, argTempID);
        }

        public void UpdateCCTempate(string frmWhere, int argTemId,  byte[] argImageData, int argCCId, int argFlatId, System.IO.FileStream fileMode,string argExt)
        {
            oTempDL.UpdateCCTempate(frmWhere, argTemId, argImageData, argCCId, argFlatId, fileMode, argExt);
        }

        public void DeleteCCTempate(string frmWhere, int argTempId, int argCCId, int argFlatId)
        {
            oTempDL.DeleteCCTempate(frmWhere,argTempId,argCCId,argFlatId);
        }
        public DataTable GetCCTemplateView(string frmWhere, int argID, int argFID)
        {
            return oTempDL.GetCCTemplateView(frmWhere, argID, argFID);
        }

        public System.Drawing.Bitmap GetImage(int argCCId, int argFLatId, int argTemplateId, string argfrmWhere)
        {
            return oTempDL.GetImage(argCCId,argFLatId,argTemplateId,argfrmWhere);
        }
        public DataTable GetFlatTemplateList()
        {
            return oTempDL.GetFlatTemplateList();
        }

        public DataTable GetFlatDocDetails(int argFTempId, int argCCId)
        {
            return oTempDL.GetFlatDocDetails(argFTempId,argCCId);
        }

        public DataTable GetFlatDocCreate(int argCCId,int argTempId)
        {
            return oTempDL.GetFlatDocCreate(argCCId,argTempId);
        }
        public void RemoveTemplateDoc(int argFlatId, int argTempId)
        {
            oTempDL.RemoveTemplateDoc(argFlatId, argTempId);
        }

        public string GetDocumentPath()
        {
            return oTempDL.GetDocumentPath();
        }
        public void InsertFlatDocTrans(DataTable argDtTrans, int argTempId)
        {
            oTempDL.InsertFlatDocTrans(argDtTrans, argTempId);
        }
        public System.Drawing.Image GetPDF(int argCCId, int argFLatId, int argTemplateId, string argfrmWhere)
        {
            return oTempDL.GetPDF(argCCId, argFLatId, argTemplateId, argfrmWhere);
        }

        public byte[] CCGetDocTemp(int argTempId, string argFrom)
        {
            return oTempDL.CCGetDocTemp(argTempId, argFrom);
        }
        public string CCGetExtension(int argTempId, string argFrom)
        {
            return oTempDL.CCGetExtension(argTempId, argFrom);
        }
        public string CompGetExtension(int argTempId)
        {
            return oTempDL.CompGetExtension(argTempId);
        }
        #endregion


      

    }
}
