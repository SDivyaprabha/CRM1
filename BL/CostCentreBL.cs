using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CRM.BusinessObjects;
using CRM.DataLayer;

namespace CRM.BusinessLayer
{
    class CostCentreBL
    {
        #region Variables
            DataTable dtExec;
            DataTable dtCompany;
            DataTable dtFacc;
            DataTable dtProj;
            DataTable dtanalHead;
            DataTable dtLevel;

            int ccId;
            string ccName;
            string address;
            int execId;
            int teamId;
            int faCCId;
            string faCCName;
            int analTypeId;
            int companyID;
            string companyName;
            string dataFleName;
            Boolean freeze;
            decimal glValue;
            decimal ocbaseAmt;
            string paySchType;
            string projDB;
            string extraDB;
            DateTime targetDate;
            string remarks;
        

        #endregion

        #region Objects
            CRM.DataLayer.CostCentreDL oCostCentreDL;

        #endregion

        #region Constructor
        public CostCentreBL()
        {
            oCostCentreDL = new CRM.DataLayer.CostCentreDL();
        }
        #endregion

        #region Methods
        public CheckedListBox ChkLevel { get; set; }
        
        public DataTable GetExecutive()
            {
                try
                {
                    dtExec = oCostCentreDL.GetExecutive();

                }
                catch (Exception e)
                {
                    throw e;
                }
                return dtExec;
            }
        
        public DataTable GetCompany()
            {
                try
                {
                    dtCompany = oCostCentreDL.GetCompany();

                }
                catch (Exception e)
                {
                    throw e;
                }
                return dtCompany;
            }
        
        public DataTable GetProj()
            {
                try
                {
                    dtProj = oCostCentreDL.GetProj();

                }
                catch (Exception e)
                {
                    throw e;
                }
                return dtProj;
            }
        
        public DataTable GetLevel(int CCId)
            {
                try
                {
                    dtLevel = oCostCentreDL.GetLevel(CCId);

                }
                catch (Exception e)
                {
                    throw e;
                }
                dtLevel.AcceptChanges();
                return dtLevel;
            }

        public DataTable GetFaCC(string Companyname)
            {
                try
                {
                    dtFacc = oCostCentreDL.GetFaCC(Companyname);

                }
                catch (Exception e)
                {
                   /// throw e;
                    MessageBox.Show(e.Message, "");
                }
                return dtFacc;
            }        
                
        public DataTable DtanalHead
        {
        get { return dtanalHead;}
        set { dtanalHead=value;}
        }

        public int InsertCompetitor(CompetitorBO argObject,DataTable argdtFT)
        {
            return oCostCentreDL.InsertCompetitor(argObject,argdtFT);
        }

        public void UpdateCompetitor(CompetitorBO argObject,int argProjectId,DataTable argdtFT)
        {
            oCostCentreDL.UpdateCompetitor(argObject, argProjectId,argdtFT);
        }

        public DataTable GetCompetitor()
        {
            return oCostCentreDL.GetCompetitor();
        }

        public bool CompFound(int argCompId)
        {
            return oCostCentreDL.CompFound(argCompId);
        }

        public void DeleteCompetitor(int argProjectId)
        {
            oCostCentreDL.DeleteCompetitor(argProjectId);
        }

        public DataTable GetCompDetails(int argProjectId)
        {
            return oCostCentreDL.GetCompDetails(argProjectId);
        }

        public DataTable GetFlatType(int argCCId)
        {
            return oCostCentreDL.GetFlatType(argCCId);
        }

        public bool CheckProjectFound(string argProjName, int argCompId)
        {
            return oCostCentreDL.CheckProjectFound(argProjName, argCompId);
        }

        public static bool StageListFound(UnitDirBL OUintDirBL, int argId)
        {
            return CostCentreDL.StageListFound(OUintDirBL,argId);
        }

        public static void CompititorTemplAttach(int argCompId, byte[] argImageData, System.IO.FileStream fileMode)
        {
            CostCentreDL.CompititorTemplAttach(argCompId, argImageData, fileMode);
        }

        public static byte[] GetDocTemp(int argCompId)
        {
            return CostCentreDL.GetDocTemp(argCompId);
        }
        #endregion
        
        #region Properties
            public int CCId
            {
                get { return ccId; }
                set { ccId = value; }
            }
            public string CCName
            {
                get { return ccName; }
                set { ccName = value; }
            }

            public string Address
            {
                get { return address; }
                set { address = value; }
            }
            public int ExecId
            {
                get { return execId; }
                set { execId = value; }
            }
            public int ProjTypeId { get; set; }

            public int TeamId
            {
                get { return teamId; }
                set { teamId = value; }
            }

            public int FaCCID
            {
                get { return faCCId; }
                set { faCCId = value; }
            }
            public string FaCCName
            {
                get { return faCCName; }
                set { faCCName = value; }
            }
            public int AnalTypeId
            {
                get { return analTypeId; }
                set { analTypeId = value; }
            }
            public string ProjType { get; set; }

            public int CompanyId
            {
                get { return companyID; }
                set { companyID = value; }
            }

            public string ComanyName
            {
                get { return companyName; }
                set { companyName = value; }
            }
            public string DataFileName
            {
                get { return dataFleName; }
                set { dataFleName = value; }
            }

            public Boolean Freeze
            {
                get { return freeze; }
                set { freeze = value; }
            }

            public decimal GLValue
            {
                get { return glValue; }
                set { glValue = value; }
            }

            public string PaymentSchType
            {
                get { return paySchType; }
                set { paySchType = value; }
            }

            public DateTime TargetDate
            {
                get { return targetDate; }
                set { targetDate = value; }
            }
            public string ProjDB
            {
                get { return projDB; }
                set { projDB = value; }
            }

            public string ExtraDB
            {
                get { return extraDB; }
                set { extraDB = value; }
            }
            public decimal OCBaseAmt
            {
                get { return ocbaseAmt; }
                set { ocbaseAmt = value; }
            }

           
            public string Remarks
            {
                get { return remarks; }
                set { remarks = value; }
            }
            
            #endregion


    }
}
