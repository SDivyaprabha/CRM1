using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DataLayer;
using CRM.BusinessObjects;

namespace CRM.BusinessLayer
{
    class FeatureBL
    {
        #region Objects
        FeatureDL oFDL;

        #endregion

        #region Constructor

        public FeatureBL()
        {
            oFDL = new FeatureDL();
        }
        #endregion

        #region Methods
        public DataTable GetFMaster()
        {
            return oFDL.GetFMaster();
        }
        public int InsertFDesc(string argDescription)
        {
            return oFDL.InsertFDesc(argDescription);
        }
        public void UpdateFDesc(int argFId, string argDescription)
        {
            oFDL.UpdateFDesc(argFId, argDescription);
        }
        public void DeleteFeature(int argId)
        {
            oFDL.DeleteFeature(argId);
        }

        public bool CheckUsed(int argId)
        {
            return oFDL.CheckUsed(argId);
        }
        public void InsertFeatureTrans(int argId, DataTable argTransId)
        {
            oFDL.InsertFeatureTrans(argId, argTransId);
        }

        public DataTable GetFlatTypeFeatureList(int argFlatTypeID)
        {
            return oFDL.GetFlatTypeFeatureList(argFlatTypeID);
        }


        #endregion

    }
}
