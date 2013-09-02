using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.DataLayer;
using System.Data;
using CRM.BusinessObjects;

namespace CRM.BusinessLayer
{
    class ExtraBillBL
    {
        #region Objects
        ExtraBillDL m_lDL;
        #endregion
        #region Constructor

        public ExtraBillBL()
        {
            m_lDL = new ExtraBillDL();
        }
        #endregion

        #region Methods

        public static DataTable GetExtraBill(int argFlatId)
        {
            return ExtraBillDL.GetExtraBill(argFlatId);
        }

        public DataTable GetProject()
        {
            return m_lDL.GetProject();
        }
        public DataTable GetFlatNo(int argCCId)
        {
            return m_lDL.GetFlatNo(argCCId);
        }
        public DataSet GetRegisterDetails(int argBillRegId, string argProjName)
        {
            return m_lDL.GetRegisterDetails(argBillRegId,argProjName);
        }
        public DataTable GetExtraItemDetails(int argFlatId,string argProjName)
        {
            return m_lDL.GetExtraItemDetails(argFlatId,argProjName);
        }
        public int InsertExtraBill(ExtraBillRegBO argBRegBo,DataTable argBTrans,DataTable argQTrans,int argFlatId)
        {
            return m_lDL.InsertExtraBill(argBRegBo, argBTrans, argQTrans, argFlatId);
        }
        public void UpdateExtraBill(ExtraBillRegBO argBRegBo, DataTable argBTrans, DataTable argQTrans)
        {
            m_lDL.UpdateExtraBill(argBRegBo, argBTrans, argQTrans);
        }
        public DataTable GetExtraBillList(DateTime frmDate, DateTime toDate)
        {
            return m_lDL.GetExtraBillList(frmDate, toDate);
        }
        #endregion
    }
}
