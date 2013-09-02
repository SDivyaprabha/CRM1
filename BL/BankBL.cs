using System;
using System.Collections.Generic;
using System.Linq;
using CRM.DataLayer;
using System.Data;
using CRM.BusinessObjects;

namespace CRM.BusinessLayer
{
    class BankBL
    {
        #region Methods

        public static DataTable getBank()
        {
            return BankDL.getBank();
        }
        public static bool BankIDFound(int argBankId)
        {
            return BankDL.BankIDFound(argBankId);
        }
        public static bool LoanFound(int argFlatId)
        {
            return BankDL.LoanFound(argFlatId);
        }
        public static DataTable GetBankDetails()
        {
            return BankDL.GetBankDetails();
        }
        public static void DeleteBank(int argBankId)
        {
            BankDL.DeleteBank(argBankId);
        }

        public static DataTable getBankname()
        {
            return BankDL.getBankname();
        }
        public static DataTable getBranchName()
        {
            return BankDL.getBranchName();
        }
        public static void UpdateBankInfo()
        {
            BankDL.UpdateBankInfo();
        }
        public static DataTable getLoanInfo()
        {
            return BankDL.getLoanInfo();
        }
        public static DataTable getEditBank(int argBankId)
        {
            return BankDL.getEditBank(argBankId);
        }
        public static int InsertBank()
        {
            return BankDL.InsertBank();
        }

        public static void UpdateBank()
        {
            BankDL.UpdateBank();
        }

        public static int CheckListSortIdFound(string argName)
        {
            return BankDL.CheckListSortIdFound(argName);
        }
        public static bool CheckListFound(int argId, string argName,string argType)
        {
            return BankDL.CheckListFound(argId, argName, argType);
        }

        public static bool TemplateFound(string argName, string argType)
        {
            return BankDL.TemplateFound(argName, argType);
        }

        public static int InsertCheckList(CheckListBO argObject)
        {
            return BankDL.InsertCheckList(argObject);
        }

        public static void UpdateSortOrder(DataTable dt)
        {
            BankDL.UpdateSortOrder(dt);
        }

        public static void UpdateCheckList(CheckListBO argObject)
        {
            BankDL.UpdateCheckList(argObject);
        }

        public static bool DocuFound(int argChekId)
        {
            return BankDL.DocuFound(argChekId);
        }

        public static bool BankFound(int argBranchId)
        {
            return BankDL.BankFound(argBranchId);
        }

        public static void DeleteChekList(int argChekId)
        {
            BankDL.DeleteChekList(argChekId);
        }

        public static DataTable getCheckList(string argType)
        {
            return BankDL.getCheckList(argType);
        }

        public static void InsertCheckListTrans(DataTable argdtTrans)
        {
            BankDL.InsertCheckListTrans(argdtTrans);
        }
        
        public static DataTable getCostCentre()
        {
            return BankDL.getCostCentre();
        }

        public static DataTable GetSlabStructure(int argBranchId)
        {
            return BankDL.GetSlabStructure(argBranchId);
        }

        public static DataTable getBankCostCentre(int argBranchId)
        {
            return BankDL.getBankCostCentre(argBranchId);
        }

        public static DataTable getBankBranch(int argBranchId)
        {
            return BankDL.getBankBranch(argBranchId);
        }

        public static void UpdateBankBranch(int argBranchId, DataTable argdt,DataTable argdtS)
        {
            BankDL.UpdateBankBranch(argBranchId, argdt,argdtS);
        }

        public static void DeleteBankBranch(int argBranchId)
        {
            BankDL.DeleteBankBranch(argBranchId);
        }

        public static DataTable GetBankBranchReg(int argBranchId)
        {
            return BankDL.GetBankBranchReg(argBranchId);
        }

        public static DataTable GetBankRegister()
        {
            return BankDL.GetBankRegister();
        }

        #endregion
    }
}
